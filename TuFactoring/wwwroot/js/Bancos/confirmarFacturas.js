new Vue({
    el: '#appConfirmarFacturas',
    i18n,
    store: vuexLayout,
    vuetify: new Vuetify({
        lang: {
            t: (key, ...params) => i18n.t(key,params),
        }
    }),
    data: {
        filtersIsEmpty: filtersIsEmpty,
        filterIsEmpty: filterIsEmpty,
        arrayCondition: arrayCondition,
        currencies: {},
        dialogAyuda:false,
        modalLogout: { mostrar: false },
        symbol: "",
        filter: [],
        totalFacturas: [],
        buscarFacturas: [],
        loading: [],
        options:[],
        moment:moment,
        tamanoTlf: tamanoTlf,
        dialogConfirmar: false,
        itempage: '',
        headers: [
            { text: i18n.t("headers.check"), value: "check", align: "center", sortable:false },
            { text: i18n.t("headers.cliente"), value: "invoice.debtor.name", align: "center" },
            { text: i18n.t("headers.proveedor"), value: "invoice.supplier.name", align: "center" },
            { text: i18n.t("headers.numeroFactura"), value: "invoice.number", align: "center" },
            { text: i18n.t("headers.fechaVencimiento"), value: "invoice.expiration_date", align: "center" },
            { text: i18n.t("headers.valorNeto"), value: "invoice.amount", align:"center"},
            { text: i18n.t("headers.confirmar"), value: "opciones", align: "center" },
        ],
        formatoMonedaInput: formatoMonedaInput,
        backEndDateFormat: backEndDateFormat,
        lang: "es",
        envio: false,
        page: 1,
        pageCount:10,
        chekeado :false,
        selected:[],
        tabla: [],
        cargando: true,
        facturas: [],
        perPage: 9,
        acumuladorValor: 0,
        itemsPerPageOptions: [3, 9, 18,-1],
        iso_4217:"",
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
        idCurrencies:0,
    },
    created: function () {
        this.cargando = true;

        try {
            this.currencies = JSON.parse(document.getElementById('currenciesData').value);
            document.getElementById("eliminarData").removeChild(document.getElementById('currenciesData'));
            
            for (var i = 0; i < this.currencies.length;i++) {
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

        setTimeout(() => iniciarButtonFilters(), 500)
        this.facturas.sort((a, b) => a.invoice.debtor.name.toLowerCase() < b.invoice.debtor.name.toLowerCase() ? -1 : +(a.invoice.debtor.name.toLowerCase() > b.invoice.debtor.name.toLowerCase()))
    },
    //
    watch: {
        options: {
            async handler() {

                for (var i = 0; i < this.currencies.length;i++) {
                    if (this.options[i].itemsPerPage == -1) {
                        llamadaRecursiva(this.buscarFacturas[i], this.llenarFacturas,i)
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
                        if ($('#collapseCardExample' + indice + '-' + w ).hasClass("show")) {
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
            }catch (e) {
                console.log(e)
            }
        },
        //
        confirmingInvoice: async function (indice) {
            this.cargando = true;
            await this.confirmarFactura(this.facturas[indice]);

            this.cargando = false;
        },
        //
        confirmarFactura: async function (factura, index) {
            if(this.envio) return

            let listado = []
            listado.push({ invoice_id : factura.id })
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
                                    this.selected[t] --
                                }
                            }
                        }

                    })

                    if (status == respond.length) toastr.success(i18n.t("mensajesModal.estimadoUsuario")+ "<br><br>" +i18n.t("confirmacionFacturaIndividual"))
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
                    listado.push({ invoice_id: this.facturas[data].id })
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

                    if(respond.data == null) respond.data = respond

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

            for (let i = 0; i < this.selected.length; i++) {
                facturasFiltradas.push(this.facturas[this.selected[i]])
            }

            facturasFiltradas = filtrarPublicationsCurrency(facturasFiltradas, idCurrency)

            facturasFiltradas.map(data => {
                acumulador = acumulador + data.invoice.amount
            })

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
        }
    },/*
    computed: {
        checkAll: {
            get: function () {
                return (this.selected.length == this.facturas.length && this.facturas.length > 0)
            },
        },
    },*/
    
});

