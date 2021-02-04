new Vue({
    el: '#appConfirmarFacturas',
    i18n,
    store: vuexLayout,
    vuetify: new Vuetify({
        lang: {
            t: (key, ...params) => i18n.t(key, params),
        }
    }),
    data: {
        filtrarPublicationsCurrency: filtrarPublicationsCurrency,
        filtersIsEmpty: filtersIsEmpty,
        filterIsEmpty: filterIsEmpty,
        arrayCondition: arrayCondition,
        dialogConfirmarRechazo: false,
        currencies: {},
        dialogAyuda: false,
        modalLogout: { mostrar: false },
        symbol: "",
        filter: [],
        dialogConfirmarModalUnisono: false,
        totalFacturas: [],
        buscarFacturas: [],
        invoiceAmount: 0,
        loading: [],
        indexActivo: 0,
        options: [],
        moment: moment,
        tamanoTlf: tamanoTlf,
        dialogConfirmar: false,
        itempage: '',
        headers: [
            { text: i18n.t("headers.check"), value: "check", align: "center", sortable: false },
            { text: i18n.t("headers.cliente"), value: "invoice.debtor.name", align: "center" },
            { text: i18n.t("headers.proveedor"), value: "invoice.supplier.name", align: "center" },
            { text: i18n.t("headers.numeroFactura"), value: "invoice.number", align: "center" },
            { text: i18n.t("headers.fechaVencimiento"), value: "invoice.expiration_date", align: "center" },
            { text: i18n.t("headers.valorNeto"), value: "invoice.amount", align: "center" },
            { text: i18n.t("headers.confirmar"), value: "opciones", align: "center" },
        ],
        headers2: [
            { text: i18n.t("headers.proveedor"), value: "invoice.supplier.name", align: "center" },
            { text: i18n.t("headers.numeroFactura"), value: "invoice.number", align: "center" },
            { text: i18n.t("headers.fechaVencimiento"), value: "invoice.expiration_date", align: "center" },
            { text: i18n.t("headers.originalAmount"), value: 'invoice.original_amount', align: 'center' },
            { text: i18n.t("headers.valorNeto"), value: "invoice.amount", align: "center" },
            { text: i18n.t("headers.opciones"), value: "opciones", align: "center" }
        ],
        headers3: [
            { text: i18n.t("headers.tipo"), value: "tipo", align: "center" },
            { text: i18n.t("headers.code"), value: "number", align: "center" },
            { text: i18n.t("headers.amountDeduction"), value: "monto", align: "center" },
            { text: i18n.t("headers.opciones"), value: "opciones", align: "center" }
        ],
        formatoMoneda: formatoMoneda,
        formatoMonedaInput: formatoMonedaInput,
        backEndDateFormat: backEndDateFormat,
        lang: "es",
        envio: false,
        nuevo: [],
        widthTelefono: widthTelefono,
        page: 1,
        invoiceTransition: {},
        dialogConfirmarModal: false,
        pageCount: 10,
        valorAcumulado: 0,
        chekeado: false,
        selected: [],
        unisonas: [],
        tabla: [],
        cargando: true,
        objetoCurrency: {
            iso_4217: "",
            symbol: "",
            id: "",
            index: 0,
        },
        objetoCurrencyUnisono: {
            iso_4217: "",
            symbol: "",
            id: "",
            index: 0,
            digits: 0,
        },
        facturas: [],
        dialogConfirmarAceptarUnisona: false,
        perPage: 9,
        acumuladorValor: 0,
        itemsPerPageOptions: [3, 9, 18, -1],
        iso_4217: "",
        valorAcumuladoUnisono: 0,
        temp: {
            number: '',
            amount: 0,
            expiration_date: '',
            supplier: '',
            supplier_id: '',
            debtor: '',
            status: 0
        },
        empresas: [],
        nuevas: [],
        bid_amount: 0,
        indice: 0,
        isMayor30: false,
        errorD: false,
        idCurrencies: 0,
        nuevoDeducion: {
            id: null,
            idPais: 0,
            idCliente: 0,
            numero: '',
            monto: 0.0,
            tipoMoneda: 0,
            nombreMoneda: '',
            state: '',
            fechaEmision: '',
            fechaVencimiento: '',
            proveedor: {
                id: '0',
                nombre: ''
            },
            totalDeducciones: 0.0,
            deduccion: {
                tipo: 0,
                numero: "",
                monto: 0.0
            }
        },
        //Errores Deruciones
        errorAmount: false,
        errorDeduccion: false,
        errorNumeroDeduccion: false,
        errorDeduccionDuplicado: false,
        errorTipoDeduccion: false,
        errorMontoDeduccion: false,
        errorNetAmount: false,
        errorProveedor: false,
        errorNum: false,
        errorFactura: false,
        errorMoneda: false,
        enviando: false,
        errorOriginalAmount: false,
        dialogDeduccion: false,
        dialogEliminar: false,
        moment: moment,
        errorInvoiceNumber: {
            vacio: false,
            superior: false,
            mismoProveedor: false,
            mismoCliente: false,
            invoiceExist: false,
            formatoInvalido: false
        },
        errorIssued: {
            igual: false,
            superiorToDate: false,
            superior: false,
            vacio: false
        },
        errorExpired: {
            igual: false,
            inferior: false,
            inferiorToDate: false,
            vacio: false
        },
        errorPorcentaje: {
            vacio: false,
            superior: false,
            igual: false
        },
        errorDeductions: {
            vacio: false,
            superior: false,
        },
        errorDeductionsAmount: {
            vacio: false,
            superior: false,
            superiorNominal: false,
            igualNominal: false,
            superiorTrece: false,
            superiorTreceDeduccion: false
        },
        proveedorIdEditar: '',
        nmrFacturaEdita: '',
        enviando: false,
        deduccionIndexActual: -1,
        idDeduccion: -1,
        dialogActualizarDeduccion: false,
        dialogEliminarDeduccion: false,
        dialogDeduciones: false,
        tipoDeduccion: [],
        deducciones: [],
        tipoMoneda: [],
        settings: {},
        digits: 2,
        REPuntos: /[.]{2,}/,
        REComas: /[,]{2,}/,
        deduccionInvoiceActual: {},
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

        document.getElementById("appConfirmarFacturas").removeAttribute("hidden")
        this.lang = document.getElementsByTagName("html")[0].getAttribute("lang")
        this.cargando = false;

    },
    //
    mounted: async function () {
        tiempoLogin(this.modalLogout)

        for (var i = 0; i < this.currencies.length; i++) {
            await this.llenarFacturas(i);
        }
        await this.llenarCatalogo();
        setTimeout(() => iniciarButtonFilters(), 500)
        this.facturas.sort((a, b) => a.invoice.supplier.name.toLowerCase() < b.invoice.supplier.name.toLowerCase() ? -1 : +(a.invoice.supplier.name.toLowerCase() > b.invoice.supplier.name.toLowerCase()))
    },
    //
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
    //
    methods: {
        //
        async llenarFacturas(indice) {
            if (this.loading[indice]) return
            this.loading[indice] = true

            var take = 100
            await axios.post('?handler=candidates', { pagination: { take: take, skip: this.totalFacturas[indice] }, filter: this.filter[indice] },
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
                    /*
                    var idmax=0
                    respond.data.map(data => {
                        if (data == null) return

                        var data2 = JSON.parse(JSON.stringify(data))

                        data2.id='12345678'+idmax
                        data2.currency.id = 840
                        data2.currency.iso_4217 = "USD"

                        idmax++
                        this.facturas.push(data2)
                    })*/

                    //console.log(this.facturas)

                }).catch((respond) => { console.log(respond); this.loading[indice] = false });

            return (this.buscarFacturas[indice] && this.options[indice].itemsPerPage == -1)
        },
        //
        rechazarOfertada: async function (indice) {
            if (this.envio) return

            this.envio = true
            let rechazada = this.facturas[indice].id

            await axios.post('?handler=rechazar', { invoice_id: rechazada },
                {
                    headers: {
                        "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                    }
                })
                .then((resp) => {
                    resetTime()
                    this.envio = false

                    if (resp.data != null)
                        resp = resp.data

                    if (typeof resp === 'string' || resp instanceof String) {

                        if (resp.includes("<!DOCTYPE html>")) {
                            window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired";
                            toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br>" + i18n.t("errorBaseDatos"));
                            return;
                        }
                    }

                    if (resp != null && resp.errors == notAuthorized) {
                        window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                        return
                    }

                    if (resp.error != null) {
                        if (resp.error.Message == "Invalid User") {
                            window.location.pathname = "../Index"
                            return
                        }
                        toastr.warning(i18n.tc("mensajesModal.problemasRespuesta", 2, { 0: "rechazar la oferta" }))
                        return
                    }

                    this.facturas.splice(indice, 1)
                    toastr.success(i18n.t("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.t("mensajesModal.rechazarFacturasConfirmDebtorIndividual"))
                }).catch((e) => { console.log(e); toastr.error(i18n.t("errorRespuesta")); this.envio = false })

        },
        //
        abrirDialogUnisonas(invoice, index) {
            //Inicialización
            this.unisonas = []
            this.dialogConfirmarModalUnisono = true
            this.valorAcumuladoUnisono = 0
            //Asignación
            this.objetoCurrencyUnisono.iso_4217 = invoice.currency.iso_4217
            this.objetoCurrencyUnisono.symbol = invoice.currency.symbol
            this.objetoCurrencyUnisono.digits = invoice.currency.digits
            this.objetoCurrencyUnisono.index = index
            this.valorAcumuladoUnisono = invoice.invoice.amount
            this.unisonas.push(invoice)
        },
        //
        checkAll2: function (indice) {
            try {
                var include = false
                var data = filtrarPublicationsCurrency(this.facturas, this.currencies[indice].id)

                for (var i = 0; i < data.length; i++) {

                    if (!this.selected.includes(this.facturas.indexOf(data[i]))) {
                        this.selected.push(this.facturas.indexOf(data[i]))
                        include = true
                    }
                }

                if (include) {
                    for (var w = 0; w < data.length; w++) {
                        if ($('#collapseCardExample' + indice + '-' + w).hasClass("show")) {
                            $("#collapseCardExample" + indice + "-" + w).removeClass("show")
                            $("#collapseCardExample" + indice + "-" + w).addClass("show")
                        } else {
                            $("#collapseCardExample" + indice + "-" + w).addClass("show")
                        }
                    }
                } else {
                    for (var w = 0; w < data.length; w++) {
                        if ($("#collapseCardExample" + indice + "-" + w).hasClass("show")) {
                            $("#collapseCardExample" + indice + "-" + w).removeClass("show")
                        } else {
                            $("#collapseCardExample" + indice + "-" + w).addClass("collapse")
                        }
                        this.selected = removeItemFromArr(this.selected, this.facturas.indexOf(data[w]))
                    }
                }
            } catch (e) {
                console.log(e)
            }
        },
        //
        limpiarConfirmadas() {
            this.selected = []
            this.unisonas = []
        },
        //
        acumularValoresConfirmacion: function () {
            let acumulador = 0

            this.nuevo.map(valor => {
                acumulador = acumulador + valor.invoice.amount
            })

            return acumulador
        },
        //
        cancelarAsignacionUnisona: function (indice, idCurrency) {
            this.valorAcumulado = this.valorAcumulado - this.unisonas[indice].invoice.amount
            this.unisonas.splice(indice, 1);
        },
        //
        cancelarAsignacion: function (indice, idCurrency) {
            this.valorAcumulado = this.valorAcumulado - this.nuevo[indice].invoice.amount
            this.nuevo.splice(indice, 1);
        },
        //
        posiblesFacturas: function (currency, index) {
            this.objetoCurrency.iso_4217 = currency.iso_4217
            this.objetoCurrency.symbol = currency.symbol
            this.objetoCurrency.index = index
            this.valorAcumulado = 0
            this.objetoCurrency.id = currency.id
            this.nuevo = []

            for (var i = 0; i < this.selected.length; i++) {
                if (this.facturas[this.selected[i]].currency.id == this.objetoCurrency.id) {
                    this.nuevo.push(this.facturas[this.selected[i]]);
                }
            }

            this.nuevo.sort((a, b) => a.invoice.supplier.name.toLowerCase() < b.invoice.supplier.name.toLowerCase() ?
                -1 : +a.invoice.supplier.name.toLowerCase() > b.invoice.supplier.name.toLowerCase())


            this.nuevo.map(data => {
                this.valorAcumulado = this.valorAcumulado + data.invoice.amount
            })

            $("#myModal").modal("show")
        },
        //
        confirmingInvoice: async function (indice) {
            this.cargando = true;
            await this.confirmarFactura(this.facturas[indice]);

            this.cargando = false;
        },
        //
        confirmarFactura: async function (factura, index) {
            if (this.envio) return

            let listado = []
            listado.push({ invoice_id: factura.id, confirmant: factura.entity.id })
            let status = 0
            this.envio = true
            await axios.post('confirmarFacturas?handler=confirma', listado,
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
                        if (data.errors != null) {
                            if (data.errors == notAuthorized || data.errors == "Invalid User") {
                                window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                                return
                            }

                        } else {
                            status++;
                            let indice = 0
                            for (let i = 0; i < this.facturas.length; i++) {
                                if (this.facturas[i].id == data.id) {
                                    this.facturas.splice(i, 1)
                                    indice = i
                                    for (let t = 0; t < this.selected.length; t++) {
                                        if (this.selected[t] == i) {
                                            this.selected.splice(t, 1)
                                        }
                                    }
                                    break
                                }

                            }

                            for (let t = 0; t < this.selected.length; t++) {
                                if (this.selected[t] > indice) {
                                    this.selected[t]--
                                }
                            }
                        }

                    })

                    if (status == respond.length) toastr.success(i18n.t("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.t("confirmacionFacturaIndividual"))
                    else if (status > 0) toastr.warning(i18n.t("problemasRespuesta"))
                    else toastr.warning(i18n.t("errorRespuesta"))

                }).catch((respond) => { toastr.error(i18n.t("errorRespuesta")); this.envio = false; });


        },
        //
        confirmarFacturas: async function () {
            if (this.envio || this.selected.length == 0) return
            this.envio = true
            let listado = []
            let status = 0

            this.selected.map(data => {
                if (this.facturas[data].currency.id == this.idCurrencies)
                    listado.push({ invoice_id: this.facturas[data].id, confirmant: this.facturas[data].entity.id })
            })

            await axios.post('confirmarFacturas?handler=confirma', listado,
                {
                    headers: {
                        "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                    }
                })
                .then((respond) => {
                    resetTime()
                    this.envio = false

                    if (respond.data == null) respond.data = respond

                    respond.data.map(data => {
                        if (data.errors != null || data.errors != undefined) {
                            if (data.errors == notAuthorized || data.errors == "Invalid User") {
                                window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                                return
                            }


                        } else {
                            status++
                            for (let i = 0; i < this.facturas.length; i++) {
                                if (this.facturas[i].id == data.id) {
                                    this.facturas.splice(i, 1)
                                    for (let t = 0; t < this.selected.length; t++) {
                                        if (this.selected[t] == i) {
                                            this.selected.splice(t, 1)

                                            for (let w = 0; w < this.selected.length; w++) {
                                                if (this.selected[w] > i)
                                                    this.selected[w]--
                                            }
                                            break
                                        }
                                    }
                                    break
                                }
                            }//******************
                        }

                    })

                    if (status == respond.data.length) toastr.success(i18n.t("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.t("confirmacionFacturaIndividual"))
                    else if (status > 0) toastr.warning(i18n.t("problemasRespuesta"))
                    else toastr.warning(i18n.t("errorRespuesta"))

                }).catch((respond) => { toastr.error(i18n.t("errorRespuesta")); this.envio = false; });

        },
        //
        rechazarAll: async function (item) {
            if (this.envio) return
            let status = 0

            this.envio = true

            facturasFiltradas = filtrarPublicationsCurrency(this.facturas, item)

            var rechazada = []

            facturasFiltradas.map(data => {
                rechazada.push({ invoice_id: data.id })
            })

            await axios.post('?handler=rechazarAll', rechazada,
                {
                    headers: {
                        "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                    }
                })
                .then((respond) => {
                    resetTime()
                    this.envio = false

                    if (respond.data == null) respond.data = respond

                    respond.data.map(data => {
                        if (data.errors != null || data.errors != undefined) {
                            if (data.errors == notAuthorized || data.errors == "Invalid User") {
                                window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                                return
                            }


                        } else {
                            status++
                            for (let i = 0; i < this.facturas.length; i++) {
                                if (this.facturas[i].id == data.id) {
                                    this.facturas.splice(i, 1)
                                    for (let t = 0; t < this.selected.length; t++) {
                                        if (this.selected[t] == i) {
                                            this.selected.splice(t, 1)

                                            for (let w = 0; w < this.selected.length; w++) {
                                                if (this.selected[w] > i)
                                                    this.selected[w]--
                                            }
                                            break
                                        }
                                    }
                                    break
                                }
                            }//******************
                        }

                    })

                    if (status == respond.data.length) {
                        toastr.success(i18n.t("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.t("mensajesModal.rechazarFacturasConfirmDebtor"))
                    } else {
                        toastr.warning(i18n.tc("mensajesModal.problemasRespuesta", 2, { 0: "rechazo de facturas" }))
                    }

                }).catch((e) => { console.log(e); toastr.error(i18n.t("errorRespuesta")); this.envio = false })
        },
        //
        forOffert: function (indice) {
            this.indice = (this.facturas.length > 0 ? indice : -1);
            if (this.facturas.length > 0) {
                this.temp.number = this.facturas[indice].invoice.number;
                this.temp.amount = this.facturas[indice].invoice.amount;
                this.temp.expiration_date = this.facturas[indice].invoice.term_days;
                this.temp.supplier = this.facturas[indice].invoice.supplier;
            }
        },
        //
        acumuladorValores: function (idCurrency) {
            let acumulador = 0
            let facturasFiltradas = []

            if (idCurrency != null || idCurrency != 0) {
                for (let i = 0; i < this.selected.length; i++) {
                    facturasFiltradas.push(this.facturas[this.selected[i]])
                }

                facturasFiltradas = filtrarPublicationsCurrency(facturasFiltradas, idCurrency)

                facturasFiltradas.map(data => {
                    acumulador = acumulador + data.invoice.original_amount
                })

                return acumulador
            }

            return acumulador

        },
        //
        offertingInvoice: async function (indice) {
            this.cargando = true;
            await this.comprarFacturas({ publication_id: this.facturas[indice].id, bid_amount: this.bid_amount });
            this.nuevas.push(storeBank.state.nuevaFactura);
            this.facturas.splice(indice, 1);
            this.cargando = false;

        },
        //
        methodPublicationsCurrency(idCurrency) {
            return filtrarPublicationsCurrency(this.facturas, idCurrency)
        },
        //
        checkAll(index) {
            var data = filtrarPublicationsCurrency(this.facturas, this.currencies[index].id)

            if (data.length == 0) return false

            var includeTotal = true
            var selected = this.selected
            var facturas = this.facturas

            data.map(a => {
                if (!selected.includes(facturas.indexOf(a))) {
                    includeTotal = false
                }
            })

            return includeTotal
        },
        //
        abrirDialogConfirmar(index) {
            this.dialogConfirmar = true
            this.symbol = this.currencies[index].symbol
            this.iso_4217 = this.currencies[index].iso_4217
            this.idCurrencies = this.currencies[index].id
        },

        abrirDialogConfirmarUnisono(data, index) {
            this.invoiceTransition = {}
            this.invoiceTransition = data[0]
            this.invoiceAmount = 0
            this.invoiceAmount = data[0].invoice.amount
            this.symbol = this.currencies[index].symbol
            this.iso_4217 = this.currencies[index].iso_4217
            this.idCurrencies = this.currencies[index].id
            this.dialogConfirmarAceptarUnisona = true
        },

        abrirDialogConfirmarRechazo(index) {
            this.dialogConfirmarRechazo = true
            this.symbol = this.currencies[index].symbol
            this.iso_4217 = this.currencies[index].iso_4217
            this.idCurrencies = this.currencies[index].id
        },




        //
        editFactura: async function (facturaActual, index) {
            console.log("editFactura")
            console.log(facturaActual)
            console.log(index)

            this.deduccionInvoiceActual = facturaActual.invoice
            let invoice = facturaActual.invoice
            this.indice = index;
            this.nuevoDeducion.id = invoice.id
            this.nuevoDeducion.numero = invoice.number;
            this.nuevoDeducion.monto = formatoMonedaInput(invoice.original_amount, this.lang, this.digits);
            //this.nuevoDeducion.proveedor.id = this.factura.supplier.id;
            this.nuevoDeducion.tipoMoneda = facturaActual.currency.id;

            //this.proveedorIdEditar = this.nuevoDeducion.proveedor.id
            this.nmrFacturaEdita = this.nuevoDeducion.numero

            this.nuevoDeducion.fechaEmision = moment(invoice.issued_date, "DD/MM/YYYY").format("YYYY-MM-DD")
            this.nuevoDeducion.fechaVencimiento = moment(invoice.expiration_date, "DD/MM/YYYY").format("YYYY-MM-DD")

            this.nuevoDeducion.state = invoice.state

            this.deducciones = [];
            this.nuevoDeducion.totalDeducciones = 0.0;
            if (invoice.charges != null) {
                for (var i = 0; i < invoice.charges.length; i++) {
                    this.nuevoDeducion.totalDeducciones += invoice.charges[i].amount;
                    this.deducciones.push({
                        id: invoice.charges[i].id,
                        amount: invoice.charges[i].amount,
                        number: invoice.charges[i].number,
                        charge_type_id: invoice.charges[i].charge_type_id
                    });
                }
            }



            /*
             
             let invoice = this.facturas[index].invoice
            this.indice = index;
            this.nuevoDeducion.id = invoice.id
            this.nuevoDeducion.numero = invoice.number;
            this.nuevoDeducion.monto = formatoMonedaInput(invoice.original_amount, this.lang, this.digits);
            //this.nuevoDeducion.proveedor.id = this.factura.supplier.id;
            this.nuevoDeducion.tipoMoneda = this.facturas[index].currency.id;

            //this.proveedorIdEditar = this.nuevoDeducion.proveedor.id
            this.nmrFacturaEdita = this.nuevoDeducion.numero

            this.nuevoDeducion.fechaEmision = moment(invoice.issued_date, "DD/MM/YYYY").format("YYYY-MM-DD")
            this.nuevoDeducion.fechaVencimiento = moment(invoice.expiration_date, "DD/MM/YYYY").format("YYYY-MM-DD")

            this.nuevoDeducion.state = invoice.state

            this.deducciones = [];
            this.nuevoDeducion.totalDeducciones = 0.0;
            if (invoice.charges != null) {
                for (var i = 0; i < invoice.charges.length; i++) {
                    this.nuevoDeducion.totalDeducciones += invoice.charges[i].amount;
                    this.deducciones.push({
                        id: invoice.charges[i].id,
                        amount: invoice.charges[i].amount,
                        number: invoice.charges[i].number,
                        charge_type_id: invoice.charges[i].charge_type_id
                    });
                }
            }
             
             
             */
            //
        },
        //
        limpiarErrores() {

            //Numero de Deduccion
            this.errorDeductions.superior = false
            this.errorDeductions.vacio = false
            //Monto de Deduccion
            this.errorDeductionsAmount.superiorNominal = false
            this.errorDeductionsAmount.vacio = false
            this.errorDeductionsAmount.superior = false
            this.errorDeductionsAmount.igualNominal = false
            //Porcentaje
            this.errorPorcentaje.vacio = false
            this.errorPorcentaje.superior = false
            this.errorPorcentaje.igual = false
            //Fecha de Vencimiento
            this.errorExpired.igual = false
            this.errorExpired.inferior = false
            this.errorExpired.inferiorToDate = false
            this.errorExpired.vacio = false
            //Fecha de Emision
            this.errorIssued.vacio = false
            this.errorIssued.superior = false
            this.errorIssued.igual = false
            //Proveedor
            this.errorProveedor = false
            //Tipo de Factura
            this.errorFactura = false
            //Valor Nominal
            this.errorOriginalAmount = false
            //Tipo de Moneda
            this.errorMoneda = false
            //Tipo de Deduccion
            this.errorTipoDeduccion = false
            //Valor neto
            this.errorNetAmount = true
        },
        //
        limpiarCampos() {
            //Llevar a Pantalla Principal
            this.estadoCarga = 0
            //Numero de Factura
            this.errorInvoiceNumber.vacio = false
            this.errorInvoiceNumber.superior = false
            this.errorInvoiceNumber.formatoInvalido = false
            //Numero de Deduccion
            this.errorDeductions.superior = false
            this.errorDeductions.vacio = false
            //Monto de Deduccion
            this.errorDeductionsAmount.superiorNominal = false
            this.errorDeductionsAmount.vacio = false
            this.errorDeductionsAmount.superior = false
            this.errorDeductionsAmount.igualNominal = false
            //Porcentaje
            this.errorPorcentaje.vacio = false
            this.errorPorcentaje.superior = false
            this.errorPorcentaje.igual = false
            //Fecha de Vencimiento
            this.errorExpired.igual = false
            this.errorExpired.inferior = false
            this.errorExpired.inferiorToDate = false
            this.errorExpired.vacio = false
            //Fecha de Emision
            this.errorIssued.vacio = false
            this.errorIssued.superior = false
            this.errorIssued.igual = false
            //Proveedor
            this.errorProveedor = false
            //Tipo de Factura
            this.errorFactura = false
            //Valor Nominal
            this.errorOriginalAmount = false
            //Tipo de Moneda
            this.errorMoneda = false
            //Tipo de Deduccion
            this.errorTipoDeduccion = false
            //Valor neto
            this.errorNetAmount = true
            //Limpiar Inputs de Deducciones
            this.idDeduccion = -1
            this.nmrDeduccion = ''
            this.porcentaje
            this.nuevoDeducion.totalDeducciones = this.totalizarDeducciones(this.deducciones);
            this.nuevoDeducion.deduccion.monto = '';
            this.nuevoDeducion.deduccion.numero = '';
            this.nuevoDeducion.deduccion.tipo = 0;
            this.errorD = false;
            this.deduccionIndexActual = -1;


        },
        //valida los errores en la carga de datos de los input's de las Deducciones
        encuentraErrorDeduccion: function () {
            this.errorD = false;
            if (this.nuevoDeducion.deduccion.monto == 0 || this.nuevoDeducion.deduccion.monto == '') return true;
            if (this.nuevoDeducion.deduccion.numero.length == 0) return true;
            if (this.nuevoDeducion.deduccion.tipo == 0) return true;

            for (let i = 0; i < this.deducciones.length; i++) {
                if (this.deducciones[i].number == this.nuevoDeducion.deduccion.numero && this.deducciones[i].number != this.nmrDeduccion) {
                    this.errorDeduccionDuplicado = true
                    return true
                } else {
                    this.errorDeduccionDuplicado = false
                }
            }

            return false;
        },
        //Totaliza las deducciones para mostrarlas en la pantalla
        totalizarDeducciones: function (deducciones) {
            if (deducciones === null) return 0;
            let totalD = 0;
            for (var i = 0; i < deducciones.length; i++) {
                totalD += (deducciones[i].amount);
            }
            return totalD;
        },
        //Guarda los datos en el Arreglo de Deducciones para guardar en el arreglo de Facturas
        agregarDeducciones: async function () {
            if (!this.encuentraErrorDeduccion()) {

                if (this.idDeduccion != -1) {
                    this.actualizarDeduccion()
                    return
                }

                let suma = formatoMoneda(this.nuevoDeducion.deduccion.monto, this.lang)

                if (parseFloat(suma) > parseFloat(formatoMoneda(this.nuevoDeducion.monto, this.lang))) {
                    this.errorD = true
                    this.errorDeductionsAmount.superior = true
                    this.errorPorcentaje.superior = true
                    this.errorDeductionsAmount.superiorNominal = true
                    return
                }

                for (let i = 0; i < this.deducciones.length; i++) {
                    suma = parseFloat(suma) + parseFloat(this.deducciones[i].amount)

                    if (parseFloat(suma) > parseFloat(formatoMoneda(this.nuevoDeducion.monto, this.lang))) {
                        this.errorPorcentaje.superior = true
                        this.errorDeductionsAmount.superiorNominal = true
                        this.errorD = true;
                        return
                    }
                }

                await this.crearDeduccion()

                this.errorD = false;
            } else
                this.errorD = true

        },
        //
        limpiarDeducciones: function () {
            this.idDeduccion = -1
            this.nmrDeduccion = ''
            this.nuevoDeducion.totalDeducciones = this.totalizarDeducciones(this.deducciones);
            this.nuevoDeducion.deduccion.monto = '';
            this.nuevoDeducion.deduccion.numero = '';
            this.nuevoDeducion.deduccion.tipo = 0;
            this.errorD = false;
            this.deduccionIndexActual = -1;
        },
        //
        //
        dialogoActualizar: function () {
            this.dialogActualizarDeduccion = true
            this.deduccionIndexActual = this.idDeduccion
        },
        crearDeduccion: async function () {
            if (this.envio) return

            this.envio = true


            await axios.post('?handler=CreateDeduction', {
                invoice_id: this.nuevoDeducion.id,
                currency_id: this.nuevoDeducion.tipoMoneda,
                charge_type_id: this.nuevoDeducion.deduccion.tipo,
                number: this.nuevoDeducion.deduccion.numero,
                amount: formatoMoneda(this.nuevoDeducion.deduccion.monto, this.lang),
            },
                {
                    headers: {
                        "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                    }
                })
                .then((respuesta) => {
                    resetTime()
                    this.envio = false
                    var resp = ''

                    if (respuesta.data == null)
                        resp = respuesta
                    else
                        resp = respuesta.data

                    if (resp.error != null && resp.error == "Deducciones superan monto") {
                        toastr.warning(i18n.t("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.t("mensajesModal.crearDeduccionMayorMonto"))
                        return
                    } else if (resp.length > 0 && resp[0].errors == notAuthorized) {
                        window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                        return
                    }

                    this.deducciones.push({
                        id: resp.id,
                        currency_id: this.nuevoDeducion.tipoMoneda,
                        charge_type_id: this.nuevoDeducion.deduccion.tipo,
                        number: this.nuevoDeducion.deduccion.numero,
                        amount: formatoMoneda(this.nuevoDeducion.deduccion.monto, this.lang),
                    })
                    this.nuevoDeducion.totalDeducciones = this.totalizarDeducciones(this.deducciones);

                    this.facturas.map((data, index) => {

                        if (data.invoice.id == this.deduccionInvoiceActual.id) {

                            console.log("Este fue el que entro")
                            console.log(data)
                            data.invoice.charges = this.deducciones;
                            data.invoice.amount = formatoMoneda(this.nuevoDeducion.monto, this.lang) - this.nuevoDeducion.totalDeducciones;
                        }
                    })
                    //this.facturas[this.indice].invoice.charges = this.deducciones;
                    //this.facturas[this.indice].invoice.amount = formatoMoneda(this.nuevoDeducion.monto, this.lang) - this.nuevoDeducion.totalDeducciones;
                    toastr.success(i18n.t("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.t("mensajesModal.creacionDeduccion"))
                    this.errorDeductionsAmount.vacio = false
                    this.errorDeductionsAmount.superior = false
                    this.errorDeductionsAmount.igualNominal = false
                    this.errorDeductionsAmount.superiorNominal = false
                    this.errorPorcentaje.vacio = false
                    this.errorPorcentaje.igual = false
                    this.errorPorcentaje.superior = false
                    this.limpiarDeducciones()
                    recallVueArray(this.facturas);
                }).catch((respond) => { toastr.error(i18n.t("errorRespuesta")); this.envio = false; this.nuevoDeducion.deduccion.id = ''; });

            recallVueArray(this.facturas);
        },
        //
        actualizarDeduccion: async function () {
            if (this.envio) return

            this.envio = true

            let suma = formatoMoneda(this.nuevoDeducion.deduccion.monto, this.lang) - this.deducciones[this.idDeduccion].amount

            if (parseFloat(suma) > parseFloat(formatoMoneda(this.nuevoDeducion.monto, this.lang))) {
                this.errorPorcentaje.superior = true
                this.errorDeductionsAmount.superiorNominal = true
                this.errorD = true
                this.envio = false
                return
            }

            for (let i = 0; i < this.deducciones.length; i++) {
                suma = parseFloat(suma) + parseFloat(this.deducciones[i].amount)

                if (parseFloat(suma) > parseFloat(formatoMoneda(this.nuevoDeducion.monto, this.lang))) {
                    this.errorPorcentaje.superior = true
                    this.errorDeductionsAmount.superiorNominal = true
                    this.errorD = true;
                    this.envio = false;
                    return
                }
            }

            this.errorDeductionsAmount.vacio = false
            this.errorDeductionsAmount.superior = false
            this.errorDeductionsAmount.igualNominal = false
            this.errorDeductionsAmount.superiorNominal = false
            this.errorPorcentaje.vacio = false
            this.errorPorcentaje.igual = false
            this.errorPorcentaje.superior = false


            await axios.post('?handler=UpdateDeduction', {
                id: this.nuevoDeducion.deduccion.id,
                charge_type_id: this.nuevoDeducion.deduccion.tipo,
                invoice_id: this.nuevoDeducion.id,
                currency_id: this.nuevoDeducion.tipoMoneda,
                number: this.nuevoDeducion.deduccion.numero,
                amount: formatoMoneda(this.nuevoDeducion.deduccion.monto, this.lang),
            },
                {
                    headers: {
                        "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                    }
                })
                .then((respuesta) => {
                    resetTime()
                    this.envio = false
                    var resp = ''

                    if (respuesta.data == null)
                        resp = respuesta
                    else
                        resp = respuesta.data

                    if (resp.error != null) {
                        toastr.warning(i18n.tc("mensajesModal.problemasRespuesta", 2, { 0: "Actualizar Deducción" }))
                        return
                    } else if (resp.length > 0 && resp[0].errors == notAuthorized) {
                        window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                        return
                    }

                    this.deducciones[this.idDeduccion].amount = parseFloat(formatoMoneda(this.nuevoDeducion.deduccion.monto, this.lang))
                    this.deducciones[this.idDeduccion].charge_type_id = this.nuevoDeducion.deduccion.tipo
                    this.deducciones[this.idDeduccion].number = this.nuevoDeducion.deduccion.numero
                    this.nuevoDeducion.totalDeducciones = this.totalizarDeducciones(this.deducciones);

                    this.facturas.map((data, index) => {

                        if (data.invoice.id == this.deduccionInvoiceActual.id) {

                            console.log("Este fue el que entro")
                            console.log(data)
                            data.invoice.charges = this.deducciones;
                            data.invoice.amount = formatoMoneda(this.nuevoDeducion.monto, this.lang) - this.nuevoDeducion.totalDeducciones;
                        }
                    })
                    //this.facturas[this.indice].invoice.charges = this.deducciones;
                    //this.facturas[this.indice].invoice.amount = formatoMoneda(this.nuevoDeducion.monto, this.lang) - this.nuevoDeducion.totalDeducciones;
                    toastr.success(i18n.t("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.t("mensajesModal.actualizarDeduccion"))
                    this.limpiarDeducciones()
                }).catch((respond) => { toastr.error(i18n.t("errorRespuesta")); this.envio = false; });

            recallVueArray(this.facturas);
        },
        //
        editarDeduccion: function (index) {

            this.nuevoDeducion.deduccion.id = this.deducciones[index].id
            this.nuevoDeducion.deduccion.tipo = this.deducciones[index].charge_type_id
            this.nuevoDeducion.deduccion.numero = this.deducciones[index].number
            this.nuevoDeducion.deduccion.monto = this.formatoMonedaInput(this.deducciones[index].amount, this.lang, this.digits)
            this.idDeduccion = index

            this.limpiarErrores()
            //this.limpiarCampos()
        },
        //Eliminar deducción del Arreglo
        eliminarDeduccion: async function (index) {
            if (this.envio) return
            if (index < 0 || index >= this.deducciones.length) {
                toastr.warning(i18n.t("errorAccion"))
                return
            }

            this.envio = true

            await axios.post('?handler=DeleteDeduction', { id: this.deducciones[index].id },
                {
                    headers: {
                        "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                    }
                })
                .then((respuesta) => {
                    resetTime()
                    this.envio = false
                    var resp = ''

                    if (respuesta.data == null)
                        resp = respuesta
                    else
                        resp = respuesta.data

                    if (resp.error != null) {
                        toastr.warning(i18n.tc("mensajesModal.problemasRespuesta", 2, { 0: "Eliminar Deducción" }))
                        return
                    } else if (resp.length > 0 && resp[0].errors == notAuthorized) {
                        window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                        return
                    }

                    if (this.idDeduccion != -1) {
                        this.idDeduccion = -1
                        this.nmrDeduccion = ""
                        this.nuevoDeducion.deduccion.monto = ""
                        this.nuevoDeducion.deduccion.numero = ""
                        this.nuevoDeducion.deduccion.tipo = 0
                    }

                    this.deducciones.splice(index, 1);
                    this.nuevoDeducion.totalDeducciones = this.totalizarDeducciones(this.deducciones);
                    this.facturas.map((data, index) => {

                        if (data.invoice.id == this.deduccionInvoiceActual.id) {

                            console.log("Este fue el que entro")
                            console.log(data)
                            data.invoice.charges = this.deducciones;
                            data.invoice.amount = formatoMoneda(this.nuevoDeducion.monto, this.lang) - this.nuevoDeducion.totalDeducciones;
                        }
                    })
                    //this.facturas[this.indice].invoice.charges = this.deducciones;
                    //this.facturas[this.indice].invoice.amount = formatoMoneda(this.nuevoDeducion.monto, this.lang) - this.nuevoDeducion.totalDeducciones;
                    toastr.success(i18n.t("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.t("mensajesModal.deleteDeduccion"))
                }).catch((respond) => { toastr.error(i18n.t("errorRespuesta")); this.envio = false; });


        },
        //
        nombreCargo: function (id) {
            for (var i = 0; i < this.tipoDeduccion.length; i++) {
                if (this.tipoDeduccion[i].id == id)
                    return this.tipoDeduccion[i].name;
            }
            return 'Sin Nombre';
        },
        //
        async llenarCatalogo() {
            await axios.post('?handler=catalogo', {},
                {
                    headers: {
                        "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                    }
                })
                .then((respond) => {
                    resetTime()
                    if (respond.data == null) {
                        this.tipoFactura = [];
                        this.tipoDeduccion = [];
                        this.tipoMoneda = [];
                        return
                    }

                    if (respond.data.length > 0 && respond.data[0].errors == notAuthorized) {
                        window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                        return
                    }


                    var catalogo = JSON.parse(respond.data)

                    catalogo.settings.map((datos) => {
                        if (datos.abbreviation == "REGEXP_INVOICE") {
                            this.placeholderFactura = datos.mask_edit
                        }

                        if (datos.abbreviation == "MAXLEN_INVOICE") {
                            this.maxLengthInvoice = datos.content
                        }
                    })

                    this.settings = catalogo.settings;
                    let tempCharge = catalogo.charges;
                    tempCharge.map(temp => {
                        if (temp.abbreviation != "CMS") {
                            this.tipoDeduccion.push(temp)
                        }
                    })
                    this.tipoMoneda = catalogo.currencies;
                    this.nuevoDeducion.tipoMoneda = (this.tipoMoneda.length === 1 ? this.tipoMoneda[0].id : 0);
                    this.nuevoDeducion.idPais = catalogo.id;
                    this.digits = this.tipoMoneda.digits == null ? 2 : this.tipoMoneda.digits

                }).catch((respond) => { console.log(respond); });
        },
        //
        obtenerSymbolo() {
            for (let i = 0; i < this.tipoMoneda.length; i++) {
                if (this.tipoMoneda[i].id == this.nuevoDeducion.tipoMoneda) {
                    return this.tipoMoneda[i].symbol
                }
            }

        },
        //
        obtenerIso4217() {

            for (let i = 0; i < this.tipoMoneda.length; i++) {
                if (this.tipoMoneda[i].id == this.nuevoDeducion.tipoMoneda) {
                    return this.tipoMoneda[i].iso_4217
                }
            }

        },
        ///
        validarNominalAmount() {

            if (this.lang == "ESV" || this.lang == "ESS" || this.lang == "es") {
                var longitudMonto = this.nuevoDeducion.monto.split(",")
                if (longitudMonto[0].length > 17) {
                    this.errorDeductionsAmount.superiorTrece = true
                } else {
                    this.errorDeductionsAmount.superiorTrece = false
                }
            }
            else if (this.lang == "ENU" || this.lang == "en") {
                var amountLength = this.nuevoDeducion.monto.split(".")
                if (amountLength[0].length > 17) {
                    this.errorDeductionsAmount.superiorTrece = true
                } else {
                    this.errorDeductionsAmount.superiorTrece = false
                }
            }

        },
        //
        validarDeductionAmount() {

            if (this.nuevoDeducion.deduccion.monto == '' || this.nuevoDeducion.deduccion.monto == 0) {
                this.errorDeductionsAmount.vacio = true
                this.errorPorcentaje.vacio = false
                this.errorPorcentaje.igual = false
                this.errorPorcentaje.superior = false

                return
            }

            this.errorDeductionsAmount.vacio = false

            if (this.nuevoDeducion.deduccion.monto.length >= 17) {
                this.errorDeductionsAmount.superior = true
                this.errorPorcentaje.vacio = false
                this.errorPorcentaje.igual = false
                this.errorPorcentaje.superior = false

                return
            }

            this.errorDeductionsAmount.superior = false

            if (formatoMoneda(this.nuevoDeducion.deduccion.monto, this.lang, 2) == formatoMoneda(this.nuevoDeducion.monto, this.lang, 2)) {
                this.errorDeductionsAmount.igualNominal = true
                this.errorPorcentaje.vacio = false
                this.errorPorcentaje.igual = false
                this.errorPorcentaje.superior = false

                return
            }
            this.errorDeductionsAmount.igualNominal = false

            if (formatoMoneda(this.nuevoDeducion.deduccion.monto) > formatoMoneda(this.nuevoDeducion.monto)) {
                this.errorDeductionsAmount.superiorNominal = true
                this.errorPorcentaje.vacio = false
                this.errorPorcentaje.igual = false
                this.errorPorcentaje.superior = false

                return
            }

            this.errorDeductionsAmount.superiorNominal = false

            if (this.lang == "ESV" || this.lang == "ESS" || this.lang == "es") {
                var montoLongitud = this.nuevoDeducion.deduccion.monto.split(",")
                if (montoLongitud[0].length > 13) {
                    this.errorDeductionsAmount.superiorTreceDeduccion = true
                } else {
                    this.errorDeductionsAmount.superiorTreceDeduccion = false
                }
            }
            else if (this.lang == "ENU" || this.lang == "en") {
                var lengthAmount = this.nuevoDeducion.deduccion.monto.split(".")
                if (lengthAmount[0].length > 13) {
                    this.errorDeductionsAmount.superiorTreceDeduccion = true
                } else {
                    this.errorDeductionsAmount.superiorTreceDeduccion = false
                }
            }

            this.errorPorcentaje.vacio = false
            this.errorPorcentaje.igual = false
            this.errorPorcentaje.superior = false

        },
        //
        validarDeductionNumber() {

            if (this.nuevoDeducion.deduccion.numero == '' || this.nuevoDeducion.deduccion.numero == 0) {
                this.errorDeductions.vacio = true
                return
            }

            this.errorDeductions.vacio = false

            if (this.nuevoDeducion.deduccion.numero.length > 255) {
                this.errorDeductions.superior = true
                return
            }

            this.errorDeductions.superior = false
        },
        //
        validateStatus(item) {
            let boolState = false
            if (item.publications != null) {
                if (item.publications[0].program.abbreviation == "DIRECT" && item.publications[0].state == "offered" ||
                    item.publications[0].program.abbreviation == "DIRECT" && item.publications[0].state == "processing" ||
                    item.publications[0].program.abbreviation == "DIRECT" && item.publications[0].state == "paid" ||
                    item.publications[0].program.abbreviation == "DIRECT" && item.publications[0].state == "sold") {
                    boolState = true
                    return boolState
                }
            }

            return boolState
        },
        //
        validarInvoiceNumber() {

            let RE = ""

            this.settings.map((dataH) => {
                if (dataH.abbreviation == "REGEXP_INVOICE") {
                    RE = new RegExp(dataH.content)
                }
            })
            if (this.nuevoDeducion.numero == '' || this.nuevoDeducion.numero.length == 0) {
                this.errorInvoiceNumber.vacio = true
                return
            }

            this.errorInvoiceNumber.vacio = false

            if (this.nuevoDeducion.numero.length >= 175) {
                this.errorInvoiceNumber.superior = true
                return
            }

            this.errorInvoiceNumber.superior = false

            if (!RE.test(this.nuevoDeducion.numero)) {
                this.errorInvoiceNumber.formatoInvalido = true
                return
            }
            this.errorInvoiceNumber.formatoInvalido = false
        },
        //
        validarIssued() {

            if (this.nuevoDeducion.fechaEmision == '') {
                this.errorIssued.vacio = true
                return
            }

            this.errorIssued.vacio = false

            if (moment(this.nuevoDeducion.fechaEmision).diff(moment(), "days") > 0) {
                this.errorIssued.superior = true
                return
            }

            this.errorIssued.superior = false

            if (moment(this.nuevoDeducion.fechaEmision).diff(this.nuevoDeducion.fechaVencimiento) == 0) {
                this.errorIssued.igual = true
                return
            }

            this.errorIssued.igual = false

            if (this.nuevoDeducion.fechaVencimiento != '') this.validarExpired
        },
        //
        validarPorcentaje() {

            if (this.porcentaje == 0 || this.porcentaje == '') {
                this.errorPorcentaje.vacio = true
                this.errorDeductionsAmount.vacio = false
                this.errorDeductionsAmount.superior = false
                this.errorDeductionsAmount.igualNominal = false
                this.errorDeductionsAmount.superiorNominal = false
                return
            }

            this.errorPorcentaje.vacio = false

            if (formatoMoneda(this.porcentaje, this.lang, 2) > 100 && formatoMoneda(this.porcentaje, this.lang, 2) <= formatoMoneda(9999, this.lang, 2)) {
                this.errorPorcentaje.superior = true
                this.errorDeductionsAmount.vacio = false
                this.errorDeductionsAmount.superior = false
                this.errorDeductionsAmount.igualNominal = false
                this.errorDeductionsAmount.superiorNominal = false
                return
            }

            this.errorPorcentaje.superior = false


            if (this.porcentaje == 100) {
                this.errorPorcentaje.igual = true
                this.errorDeductionsAmount.vacio = false
                this.errorDeductionsAmount.superior = false
                this.errorDeductionsAmount.igualNominal = false
                this.errorDeductionsAmount.superiorNominal = false
                return
            }

            this.errorPorcentaje.igual = false
            this.errorDeductionsAmount.vacio = false
            this.errorDeductionsAmount.superior = false
            this.errorDeductionsAmount.igualNominal = false
            this.errorDeductionsAmount.superiorNominal = false

        },
        //
        //Muestra el la tabla el nombre de la Deducción a partir del id guardado
        nombreCargo: function (id) {
            for (var i = 0; i < this.tipoDeduccion.length; i++) {
                if (this.tipoDeduccion[i].id == id)
                    return this.tipoDeduccion[i].name;
            }
            return 'Sin Nombre';
        },
        //
        botonVerde: function () {
            return (this.indice == -1 ? "Guardar" : "Modificar");
        },
        //
        botonRojo: function () {
            return (this.indice == -1 ? "Limpiar" : "Cancelar");
        },
        //
        valorNeto: function () {
            let monto
            if (this.nuevoDeducion.monto == 0 || this.nuevoDeducion.monto == '') return ''

            monto = formatoMoneda(this.nuevoDeducion.monto, this.lang) - this.nuevoDeducion.totalDeducciones

            return formatoMonedaInput(monto, this.lang, this.digits)
        },
    },
    computed: {
        validarNominal() {
            if (this.nuevoDeducion.monto == null || this.nuevoDeducion.monto == '') {
                return true
            }

            let monto = formatoMoneda(this.nuevoDeducion.monto, this.lang)

            if (monto <= 0 || monto == '' || this.REPuntos.test(monto)) return true

            return false
        },
        porcentaje: {
            get: function () {
                if (this.nuevoDeducion.deduccion.monto == '' || this.nuevoDeducion.deduccion.monto == 0 || isNaN(formatoMoneda(this.nuevoDeducion.deduccion.monto, this.lang))) return ''

                let res = (formatoMoneda(this.nuevoDeducion.deduccion.monto, this.lang) * 100) / formatoMoneda(this.nuevoDeducion.monto, this.lang)
                return formatoMonedaInput(res, this.lang)
            },
            set: function (value) {
                this.nuevoDeducion.deduccion.monto = formatoMonedaInput((formatoMoneda(value, this.lang) * formatoMoneda(this.nuevoDeducion.monto, this.lang)) / 100, this.lang, this.digits)
            }
        },
    },

});

