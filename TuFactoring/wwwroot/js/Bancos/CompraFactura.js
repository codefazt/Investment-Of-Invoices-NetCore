new Vue({
    el: "#appCompraFactura",
    i18n,
    store: vuexLayout,
    vuetify: new Vuetify({
        lang: {
            t: (key, ...params) => i18n.t(key, params),
        },
    }),
    data: {
        filtersIsEmpty: filtersIsEmpty,
        filterIsEmpty: filterIsEmpty,
        arrayCondition: arrayCondition,
        currencies: {},
        montoALiberar: 0,
        filter: [],
        refused: {},
        programConfirming: [],
        dialogAyuda: false,
        dialogRechazarFactura:false,
        totalFacturas: [],
        buscarFacturas: [],
        loading: [],
        options: [],
        modalLogout: { mostrar: false },
        moment: moment,
        tamanoTlf: tamanoTlf,
        headers: [
            { text: "Checks", value: "check", align: "center", sortable: false },
            { text: i18n.t("headers.n"), value: "n", align: "center" },
            { text: i18n.t("headers.clienteProveedor"), value: "client", align: "center" },
            { text: i18n.t("headers.fechaVencimientoNumero"), value: "invoice.expiration_date", align: "center" },
            { text: i18n.t("headers.valorNeto"), value: "invoice.amount", align: "center" },
            { text: i18n.t("headers.ofertar"), value: "offert", align: "center" },
            { text: i18n.t("headers.rentabilidad"), value: "rent", align: "center" },
            { text: i18n.t("headers.montoPagar"), value: "pagar", align: "center" },
            { text: i18n.t("headers.enviar"), value: "opciones", align: "center" }
        ],
        menu: false,
        Mensajeria: {
            notificaciones: true,
            mensajes: true
        },
        cerrarMordisco: true,
        hints: true,
        fav: true,
        objetoCurrency: {
            iso_4217: "",
            symbol: "",
            id: ""
        },
        message: false,
        errorOfertaInput: false,
        liberar: false,
        indexActivo: 0,
        perPage: 9,
        itemsPerPageOptions: [3, 9, 18, -1],
        formatoMonedaInput: formatoMonedaInput,
        backEndDateFormat: backEndDateFormat,
        revisarInputOferta: revisarInputOferta,
        ofertaActual: 0,
        lang: "es",
        dialogOfertarFactura: false,
        indice: -1,
        acumuladorValor: 0,
        digits: 2,
        profitability: 0,
        receivable: 0,
        filtroAplicado:false,
        dialogCompraFactura: false,
        cargando: true,
        envio: false,
        page: 1,
        pageCount: 10,
        selected: [],
        REComas: /[,]{2,}/,
        ofertas: [],
        errorD: false,
        facturas: [],
        empresas: [],
        temp: {
            number: '',
            amount: 0,
            expiration_date: '',
            supplier: ''
        },
        bid_amount: 0,

        montosTotalesConfDir: [],
        montoToalesRespaldo: [],
        maxCurrency: 0,
        titular: "default",
        titularArray: [],

    },
    created: function () {
        this.cargando = true;

        try {
            this.currencies = JSON.parse(document.getElementById('currenciesData').value);
            document.getElementById("eliminarData").removeChild(document.getElementById('currenciesData'));

            for (var i = 0; i < this.currencies.length; i++) {
                this.options.push({});
                this.buscarFacturas.push(true);
                this.loading.push(false);
                this.totalFacturas.push(0)
                this.titularArray.push({ titulo: "default"})

                this.filter[i] = JSON.parse(document.getElementById('filterData+' + i).value);
                this.filter[i].currency = this.currencies[i].id
                document.getElementById("eliminarData").removeChild(document.getElementById('filterData+' + i));
            }
        } catch (e) {
            for (var i = 0; i < this.currencies.length || i < 1; i++) {
                this.options.push({});
                this.buscarFacturas.push(true);
                this.loading.push(false);
                this.totalFacturas.push(0)

                this.filter[0] = null
            }
        }

        document.getElementById("appCompraFactura").removeAttribute("hidden")
        this.lang = document.getElementsByTagName("html")[0].getAttribute("lang")
        this.cargando = false;

    },
    mounted: async function () {
        tiempoLogin(this.modalLogout)

        this.maxCurrency = this.currencies.length
        for (let coin of this.currencies) {
      
            let typeProgram = {
                confirming: {
                    currency: coin.id,
                    amount: 0,
                    program: "CONFIRMING",
                    iso_4217: coin.iso_4217,
                    symbol: coin.symbol,
                },
                direct: {
                    currency: coin.id,
                    amount: 0,
                    program: "DIRECT",
                    iso_4217: coin.iso_4217,
                    symbol: coin.symbol,
                }
            }

            this.montoToalesRespaldo.push(typeProgram)

        }
        console.log("Nuevo Objeto de Prueba")
        console.log(this.montoToalesRespaldo)
        console.log(this.titularArray)

        for (var i = 0; i < this.currencies.length; i++) {

            //console.log(this.currencies)
            this.buscarFacturas[i] = false
            await this.llenarFacturas(i);
        }

        setTimeout(() => iniciarButtonFilters(), 500)

        this.facturas.sort((a, b) => a.invoice.debtor.name.toLowerCase() < b.invoice.debtor.name.toLowerCase() ? -1 : +(a.invoice.debtor.name.toLowerCase() > b.invoice.debtor.name.toLowerCase()))
    },
    watch: {
        options: {
            async handler() {

                for (var i = 0; i < this.currencies.length; i++) {
                    if (this.options[i].itemsPerPage == -1) {
                        llamadaRecursiva(this.buscarFacturas[i], this.llenarFacturas, i)
                        return
                    }

                    var tamanoFacturas = filtrarPublicationsCurrency(this.facturas, this.currencies[i].id).length

                    if (this.buscarFacturas[i] && this.options[i].page * this.options[i].itemsPerPage >= tamanoFacturas - this.options[i].itemsPerPage) {
                        await this.llenarFacturas(i)
                    }
                }
            },
            deep: true,
        },
    },
    methods: {
        //
        async llenarFacturas(indice) {
            if (this.loading[indice]) return
            this.loading[indice] = true

            var take = 100
            await axios.post('?handler=Confirmeds', { pagination: { take: take, skip: this.totalFacturas[indice] }, filter: this.filter[indice] },
                {
                    headers: {
                        "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                    }
                })
                .then((respond) => {
                    resetTime()
                    this.loading[indice] = false
                    if (respond.data.length == 0) {
                        this.buscarFacturas[indice] = false
                        return
                    }

                    if (respond.data.length > 0 && respond.data[0].errors == notAuthorized) {
                        window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                        return
                    }

                    if (respond.data.length < take / 2)
                        this.buscarFacturas[indice] = false

                    this.totalFacturas[indice] += respond.data.length

                    respond.data.map(data => {
                        if (data == null) return

                        for (var i = 0; i < this.facturas.length; i++) {
                            if (this.facturas[i].id == data.id) {
                                this.totalFacturas--
                                return
                            }
                        }

                        this.facturas.push(data)

                    })

                }).catch((respond) => { console.log(respond); this.loading[indice] = false });

            //this.montosTotalesConfDir = { ...this.montoToalesRespaldo }

            if (this.montoToalesRespaldo != null && this.maxCurrency == (indice + 1)) {
                for (let currencyProgram of this.montoToalesRespaldo) {

                    for (let factura of this.facturas) {

                        if (factura.currency != null && factura.currency.id == currencyProgram.confirming.currency) {

                            if (factura.program != null) {
                                if (factura.program.abbreviation == currencyProgram.confirming.program) { currencyProgram.confirming.amount += factura.invoice.amount }
                            }
                        }

                        if (factura.currency != null && factura.currency.id == currencyProgram.direct.currency) {

                            if (factura.program != null) {
                                if (factura.program.abbreviation == currencyProgram.direct.program) { currencyProgram.direct.amount += factura.invoice.amount }
                            }
                        }
                    }
                }
                console.log("montoToalesRespaldo")
                console.log(this.montoToalesRespaldo)
            }

            return (this.buscarFacturas[indice] && this.options[indice].itemsPerPage == -1)
        },
        validarInputOfertar(event) {            
            if ((event.keyCode < 48 || event.keyCode > 57) && (event.keyCode != fraccion(this.lang))) event.returnValue = false;
        },
        //Item de Iterators
        methodPublicationsCurrency(idCurrency) {
            return filtrarPublicationsCurrency(this.facturas, idCurrency)
        },
        //

        offertingInvoice: async function (digits, index) {
            this.indice = index;

            let bid_amount = $("#porcentaje" + index).val()

            bid_amount = formatoMoneda(bid_amount, this.lang)

            let lista = [{
                publication_id: this.facturas[index].id,
                bid_amount: bid_amount == '' ? 0 : bid_amount
            }]

            if (bid_amount > 100 || bid_amount < 0) {
                toastr.warning(i18n.t("montoInvalido"))

                return
            }

            this.enviarOferta(lista)

            liberar = false

        },
        //
        validateLiberation: function (idCurrency) {
            let acumuladorProgram = 0
            let facturasFiltradas = []

            facturasFiltradas = filtrarPublicationsCurrency(this.facturas, idCurrency)

            for (let x = 0; x < facturasFiltradas.length; x++) {
                if (facturasFiltradas[x].program.abbreviation == "DIRECT") {
                    acumuladorProgram = acumuladorProgram + 1
                }
            }
            return acumuladorProgram
        },
        //
        offertAll: async function (idCurrency) {
            let lista = []
            let facturasFiltradas = []

            facturasFiltradas = filtrarPublicationsCurrency(this.facturas, idCurrency)

            facturasFiltradas.map(data => {
                if (data.program.abbreviation == 'CONFIRMING') {                    
                    lista.push({
                        bid_amount: 0,
                        publication_id: data.id
                    })
                }
            })

            liberar = true
            this.enviarOferta(lista)
        },
        // Envio de la(s) oferta(s) *****************************************************************
        enviarOferta: async function (lista) {

            if (this.envio || lista.length == 0) return

            this.envio = true
            let aux = []
            let status = 0
            await axios.post('compraFactura?handler=oferta', lista,
                {
                    headers: {
                        "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                    }
                })
                .then((respond) => {
                    resetTime()
                    this.envio = false

                    if (respond.data != null)
                        respond = respond.data

                    respond.map(data => {
                        if (data.errors != null || data.errors != undefined) {
                            if (data.errors == notAuthorized || data.errors == "Invalid User") {
                                window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                                return
                            }

                        } else {
                            status++
                            for (let i = 0; i < this.facturas.length; i++) {
                                if (this.facturas[i].id == data.id) {
                                    aux.push(data.id)
                                    for (let t = 0; t < this.selected.length; t++) {
                                        if (this.selected[t] == i) {
                                            this.selected.splice(t, 1)
                                            break
                                        }
                                    }

                                    break
                                }

                            }//*************

                        }
                    })

                    if (status > 0) {
                        this.page = 1

                        aux.map(data => {

                            for (let i = 0; i < this.facturas.length; i++) {
                                if (this.facturas[i].id == data) {
                                    this.facturas.splice(i, 1);
                                    this.ofertas.splice(i, 1)
                                    this.indexActivo = 0
                                    break
                                }
                            }

                        })

                    }

                    if (status == respond.length && liberar == false) {
                        toastr.success(i18n.t("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.t("mensajesModal.ofertaRealizada"))
                    } else if (status == respond.length && liberar == true) {
                        toastr.success(i18n.t("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.t("mensajesModal.liberarTodasCompraBanco"))
                    } else if (status > 0) {
                        toastr.success(i18n.t("problemasRespuesta"))
                    } else {
                        toastr.warning(i18n.t("errorRespuesta"))
                    }

                }).catch((respond) => {  toastr.error(i18n.t("errorRespuesta")); this.envio = false; })


        },
        //Rechazar Oferta
        rechazarOferta: async function (item) {

            await axios.post('compraFactura?handler=rechazar', {publication_id: item.id},
                {
                    headers: {
                        "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                    }
                })
                .then((respond) => {
                    resetTime()
                    this.refused = {}

                    if (respond.data != null)
                        this.refused = respond.data

                    if (this.refused.errors != null || this.refused.errors != undefined) {
                        if (this.refused.errors == notAuthorized || this.refused.errors == "Invalid User") {
                            window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                            return
                        }
                    }

                    for (let i = 0; i < this.facturas.length; i++) {
                        if (this.facturas[i].id == this.refused.id) {
                            this.facturas.splice(i, 1);
                            this.ofertas.splice(i, 1)
                            this.indexActivo = 0
                            break
                        }
                    }


                    if (this.refused.id != null) {
                        toastr.success(i18n.t("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.t("mensajesModal.refuseRealizedBank"))
                    }

                }).catch((respond) => { toastr.error(i18n.t("errorRespuesta")); this.envio = false; })


        },
        limpiarOfertas: function () {

            var facturas = this.facturas
            var indexActivo = this.indexActivo

            setTimeout(function () {
                if (facturas[indexActivo] == null) return

                facturas[indexActivo].invoice.profitability = null
                facturas[indexActivo].invoice.receivable = null

                $("#porcentaje" + indexActivo).val("")

            }, 500)
        },
        //
        acumuladorValores: function (idCurrency) {
            let acumulador = 0
            let facturasFiltradas = []

            facturasFiltradas = filtrarPublicationsCurrency(this.facturas, idCurrency)
            
            facturasFiltradas.map(data => {
                acumulador = acumulador + data.invoice.amount
            })

            return acumulador
        },
        freeAllInvoices: function (item) {
            this.montoALiberar = 0
            facturasFiltradas = filtrarPublicationsCurrency(this.facturas, item.id)

            facturasFiltradas.map(datos => {
                if (datos.program.abbreviation == 'CONFIRMING') {
                    this.montoALiberar += datos.invoice.amount
                }
            })
            this.objetoCurrency.iso_4217 = item.iso_4217
            this.objetoCurrency.symbol = item.symbol
            this.objetoCurrency.id = item.id
            this.dialogCompraFactura = true

        },
        //Calcular Oferta propuesta por el Banco
        calcularOferta: function (digits, index) {
            let campo = $("#porcentaje" + index)
            let percentField = document.getElementById("porcentaje" + index).value
            let porcentaje = formatoMoneda(percentField, this.lang)
            let fieldSplit = percentField.split(",")
            this.errorOfertaInput = false

            //Hago split para validar primera posicion del array
            if (fieldSplit[0] < 10) {
                campo.attr('maxlength', 4)
            } else if (fieldSplit[0] >= 10) {
                campo.attr('maxlength', 5)
            }  
            //Valido el porcentaje maximo
            if (porcentaje > 99) {
                $("#porcentaje" + index).val(99)
                porcentaje = 99
            }

            $("#porcentajeCaja" + index).removeClass("is-invalid")
            //Validaciones del input
            if (porcentaje == '' || porcentaje == 0 || porcentaje == 0, 00 || porcentaje == 0.00 || porcentaje == "," || porcentaje == ".") {
                this.facturas[index].invoice.profitability = null
                this.facturas[index].invoice.receivable = null
                this.errorOfertaInput = true //Se utiliza para ocultar el valor si cumple una de las condiciones de arriba
                return
            }
            
            this.facturas[index].invoice.profitability = porcentaje / this.facturas[index].invoice.term_days * 360
            this.facturas[index].invoice.receivable = this.facturas[index].invoice.amount - (this.facturas[index].invoice.amount * porcentaje) / 100
            //Se utiliza para lograr que Vue refresque el valor en la vista
            var term = this.facturas[index].invoice.term_days
            this.facturas[index].invoice.term_days = 0
            this.facturas[index].invoice.term_days = term
            
        },
        //
        formatearInput: function (index) {
            $("#porcentaje" + index).val(formatoMonedaInput(formatoMoneda($("#porcentaje" + index).val(), this.lang), this.lang))
        },
    },
    //
    computed: {
        checkAll: {
            get: function () {
                return (this.selected.length == this.facturas.length && this.facturas.length > 0)
            },
            set: function (value) {
                var selected = [];
                if (value) {
                    for (var i = 0; i < this.facturas.length; i++) {

                        selected.push(i);
                    }
                }
                this.selected = selected.sort((a, b) => b - a);
            }
        },
        total_amount: {
            get: function () {
                return this.bid_amount * this.temp.amount / 100;
            },
            set: function (value) {
                this.bid_amount = (this.temp.amount > 0 ? (value * 100) / this.temp.amount : 0);
            }
        },
        mensajesComputed() {
            return vuexLayout.state.mensajes
        }

    },
});
