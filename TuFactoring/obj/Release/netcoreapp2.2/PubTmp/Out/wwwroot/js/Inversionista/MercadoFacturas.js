new Vue({
    el: "#appMercadoFacturas", 
    i18n,
    store: vuexLayout,
    vuetify: new Vuetify({
        lang: {
            t: (key, ...params) => i18n.t(key, params)
        }
    }),
    data: {
        modalLogout: { mostrar: false },
        options: {},
        buscarFacturas: true,
        totalFacturas: 0,
        token: null,
        offeredByFactor: false,
        loading: true,
        filter: null,
        ofertaMostrar: {
            profitability: null,
            earnings: null,
            payable: null,
        },
        ofertaActual:'',
        dialogComprar: false,
        dialogOfertarFactura: false,
        dialogVer: false,
        ver: '',
        indexActivo: -1,
        perPage: 9,
        itemsPerPageOptions: [3,9,18,-1],
        tamanoTlf: tamanoTlf,
        header: [
            { text: i18n.t("headers.clienteBanco"), value: "client", align: "center" },
            { text: i18n.t("headers.fechaVencimientoNumero"), value: "invoice.expiration_date", align: "center" },
            { text: i18n.t("headers.valorNeto"), value: "neto", align: "center" },
            { text: i18n.t("headers.mejorOferta"), value: "offert", align: "center" },
            { text: i18n.t("headers.ofertar"), value: "ioffert", align: "center" },
            { text: i18n.t("headers.gananciaRentabilidad"), value: "ganancia", align: "center" },
            { text: i18n.t("headers.montoPagar"), value: "pagar", align: "center" }
        ],
        dialog: false,
        menu: false,
        hints: true,
        fav: true,
        message: false,
        cerrarMordisco: true,
        moment: moment,
        formatoMonedaInput: formatoMonedaInput,
        backEndDateFormat: backEndDateFormat,
        formatoMoneda: formatoMoneda,
        lang: "es",
        country:32,
        auction: { state: '' , opened : ''},
        digits: 2,
        connection: '',
        idFactor: '',
        cargando: true,
        envio : false,
        symbolTitle: '',
        REComas: /[,]{2,}/,
        dateMin: moment().format('YYYY-01-01'),
        dateMax: moment().add(2, 'year').format('YYYY-MM-DD'),
        validateDate: '',
        validateAmount: '',
        filtroDateI: moment().format('YYYY-MM-DD'),
        filtroDateF: '',
        filtroAmountI: '',
        filtroAmountF: '',
        page: 1,
        pageCount: 10,
        marketNotOpened:false,
        facturasFiltradas: [],
        mensajeOfert: '',
        information: '',
        errorMontoAlto: 'El monto debe ser menor o igual a 100',
        facturas: [],
        banco:[],
        empresas: [],
    },
    watch: {
        options: {
            async handler() {
                if (this.options.itemsPerPage == -1) {
                    llamadaRecursiva(this.buscarFacturas, this.llenarFacturas)
                    return
                }

                if (this.buscarFacturas && this.options.page * this.options.itemsPerPage >= this.facturas.length - this.options.itemsPerPage) {
                    await this.llenarFacturas()
                }
            },
            deep: true,
        },
    },
    methods: {
        async llenarFacturas() {
            if (this.loading) return
            this.loading = true

            var take = 100

            await axios.post('?handler=invoices', { pagination: { take: take, skip: this.totalFacturas }, filter: this.filter },
                {
                    headers: {
                        "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                    }
                })
                .then((respond) => {
                    resetTime()
                    this.loading = false
                    if (respond.data === null) {
                        this.buscarFacturas = false
                        return
                    }
                    if (typeof respond.data === 'string' || respond.data instanceof String) {
                        if (respond.data.includes("<!DOCTYPE html>")) {
                            window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired";
                            toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br>" + i18n.t("errorBaseDatos"));
                            return;
                        }
                    }

                    if (respond.data.length > 0 && respond.data[0].errors == notAuthorized) {
                        window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                        return
                    }


                    if (respond.data.length < take / 2)
                        this.buscarFacturas = false

                    this.totalFacturas += respond.data.length

                    this.facturas = respond.data.filter(data => data != null)

                    this.digits = this.facturas.length > 0 ? this.facturas[0].invoice.currency.digits : 2
                    
                    this.facturasFiltradas = this.facturas 

                    this.facturasFiltradas.sort((a, b) => a.isOffered > b.isOffered ? -1 : +(a.isOffered > b.isOffered))
                }).catch((respond) => { console.log(respond); this.loading = false; this.marketNotOpened = true  });


            return (this.buscarFacturas && this.options.itemsPerPage == -1)
        },
        //
        async llenarAuction() {
            await axios.post('?handler=auction', {},
                {
                    headers: {
                        "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                    }
                })
                .then((respond) => {
                    resetTime()
                    if (respond.data == null || respond.data.state != "opened") {
                        this.facturas = []
                        this.facturasFiltradas = []
                        this.marketNotOpened = true 
                        return
                    }

                    this.auction = respond.data

                    if (respond.data != null && respond.data.errors == notAuthorized) {
                        window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                        return
                    }

                    if (this.auction.state != "opened") {
                        return
                    }


                    this.country = this.auction.country

                    this.relog()

                    this.connection = new signalR.HubConnectionBuilder().withUrl("/wsSubastas").build()
                    
                    this.connection.start().then(this.loginGroup).catch(function (err) {
                        this.envio = false
                        return console.error(err.toString());
                    });

                    this.connection.on("Publications", this.entrada)
                    this.connection.on("Cierre", function (message) {
                        if (message == "close") {
                            location.reload()
                        }
                    })
                    
                }).catch((respond) => { console.log(respond); });
        },
        //
        limpiarOferta() {
            this.ofertaMostrar= {
                profitability: null,
                earnings: null,
                payable: null,
            }
            this.ofertaActual= ''
        },

        validarInputOfertar(event) {
            if ((event.keyCode < 48 || event.keyCode > 57) && (event.keyCode != fraccion(this.lang))) event.returnValue = false;
        },

        validarInputBuscadores(event) {
            if ((event.keyCode < 48 || event.keyCode > 57) && (event.keyCode != 44 && event.keyCode != 46)) event.returnValue = false;
        },

        validarCantidad(input) {
            let monto = formatoMoneda($("#" + input).val(),this.lang)

            if (monto > 100)
                $("#" + input).val(100)

            if (monto < 0)
                $("#" + input).val(0)
            
        },

        validarCantidadActual() {
            var oferta = formatoMoneda(this.ofertaActual, this.lang)

            if (oferta >= 100) {
                this.ofertaActual = 99
                oferta = 99
            }

            if (oferta < 0) {
                this.ofertaMostrar.profitability = null
                this.ofertaMostrar.earnings = null
                this.ofertaActual = 0
                return
            }

            if (oferta == "") {
                this.ofertaMostrar.profitability =null
                this.ofertaMostrar.earnings = null
                this.ofertaMostrar.payable = null
                return
            }
            

            if (this.facturasFiltradas[this.indexActivo].discount == 0 || oferta < this.facturasFiltradas[this.indexActivo].discount) {
                var porcentaje = oferta
                var amount = this.facturasFiltradas[this.indexActivo].invoice.amount
               
                this.ofertaMostrar.profitability = porcentaje / this.facturasFiltradas[this.indexActivo].invoice.term_days * 360
                this.ofertaMostrar.earnings = (amount * porcentaje) / 100
                this.ofertaMostrar.payable = amount - (amount * porcentaje) / 100
                return
            }

            this.ofertaMostrar.profitability = null
            this.ofertaMostrar.earnings = null
            this.ofertaMostrar.payable = null
        },

        formatoInputActual() {
            this.ofertaActual = formatoMonedaInput(formatoMoneda(this.ofertaActual, this.lang), this.lang)
        },

        formatoInput(input) {
            $("#" + input).val(formatoMonedaInput(formatoMoneda($("#" + input).val(),this.lang),this.lang))
        },

        async ofertar(item, index) {

            if(this.envio) return
            
            if (moment(this.auction.closed).diff(moment(), "second") <= 0 && this.auction.state == "opened") {
                this.auction.state = "closed"
                this.facturas = []
                this.facturasFiltradas = []
                location.reload()
            }

            if (this.auction.state != "opened") {
                toastr.warning(i18n.t("subastaCerrada"))
                return
            }

            let resp = formatoMoneda($("#inputPorcentaje" + index).val(),this.lang)

            if (resp <= 0 || resp > 100) {
                toastr.warning(i18n.t("ofertaInvalida"))
                $("#tdPorcentaje" + index).addClass("is-invalid")
                return
            }
            
            $("#tdPorcentaje" + index).removeClass("is-invalid")
            if (item.discount != null && item.discount != 0 && resp >= item.discount) {
                $("#inputPorcentaje" + index).val("")

                if (resp != item.discount)
                    toastr.info(i18n.t("mensajesModal.estimadoUsuario") + "<br>" + i18n.t("mensajesModal.ofertaMejor"))
                else
                    toastr.info(i18n.t("mensajesModal.estimadoUsuario") + "<br>" + i18n.t("mensajesModal.ofertaMejor"))
                
                return
            }
            
            if ($("#inputPorcentaje" + index).val() == null || $("#inputPorcentaje"+index).val() == '') return

            if (item.discount == '' || item.discount > resp) {
                
                this.envio = true

                this.connection.invoke("Publicar", {
                    country_id: this.country,
                    publication_id: item.id,
                    factor_id: this.idFactor,
                    bid_amount: resp,
                    token: this.token
                }).catch(function (err) {
                    this.envio = false
                    return console.error("Error al enviar: " + err.toString());
                });
                $("#inputPorcentaje" + index).val("")
                
            } 

        },

        async ofertarActual(item) {
            if (this.envio) return

            if (moment(this.auction.closed).diff(moment(), "second") <= 0 && this.auction.state == "opened") {
                this.auction.state = "closed"
                this.facturas = []
                this.facturasFiltradas = []
            }

            if (this.auction.state != "opened") {
                toastr.warning(i18n.t("subastaCerrada"))
                return
            }


            if (this.ofertaActual == null || this.ofertaActual == '') return

            let resp = formatoMoneda(this.ofertaActual, this.lang)

            if (resp <= 0 || resp > 100) {
                toastr.warning(i18n.t("ofertaInvalida"))
                this.errorOferta = true
                return
            }

            this.errorOferta = false
            if (item.discount != null && item.discount != 0 && resp >= item.discount) {
                this.ofertaActual = ''
                if (resp != item.discount)
                    toastr.info(i18n.t("mensajesModal.estimadoUsuario") + "<br>" + i18n.t("mensajesModal.ofertaMejor"))
                else
                    toastr.info(i18n.t("mensajesModal.estimadoUsuario") + "<br>" + i18n.t("mensajesModal.ofertaIgual"))
                return
            }


            if (item.discount == '' || item.discount > resp) {

                this.envio = true

                this.connection.invoke("Publicar", {
                    country_id: this.country,   
                    publication_id: item.id,
                    factor_id: this.idFactor,
                    bid_amount: resp,
                    token: this.token
                }).catch(function (err) {
                    this.envio = false
                    toastr.error(i18n.t("mensajesModal.estimadoUsuario") + ".<br>" + i18n.t("notConexionDetected") + "<br>" + i18n.t("verifiqueNuevoIntento"))
                    //console.error("Error al enviar: " + err.toString());
                });

                this.ofertaActual = ''
            }

        },

        entrada: function(message) {
            this.envio = false

            if (message != null && message.errors == notAuthorized) {
                window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                return 
            }

            if (message == null || message.error !== null) {
                if (message.error != null) {
                    if (message.error.Message == "Invalid User") {
                        window.location.pathname = "../Logout?returnUrl=~/index?error=sessionExpired"
                        return
                    } else if (message.error == "auction not open") {
                        location.reload()
                        return
                    }
                }

                toastr.warning(i18n.t("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.t("mensajesModal.mercadoCerrado"))
            } else {
                for (let i = 0; i < this.facturas.length; i++) {
                    if (this.facturas[i].id == message.id) {
                        this.facturas[i].discount = message.discount
                        this.facturas[i].bids = [{ factor: { id: '' } }]
                        this.facturas[i].bids[0].factor.id = message.bids[0].factor.id
                        this.facturas[i].payable = message.payable
                        this.facturas[i].profitability = message.profitability
                        this.facturas[i].earnings = message.earnings
                        if (this.facturas[i].bids[0].factor.id == this.idFactor) {
                            resetTime()
                            this.facturas[i].isOffered = true
                            console.log(this.facturas[i].isOffered)
                            toastr.success(i18n.t("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.t("mensajesModal.realizarOfertaInversionista"))
                        }

                        break
                    }
                }
                
            }
            
        },
        
        relog: function () {
            
            var duration = moment(this.auction.closed).diff(moment(), "second")

            if (duration <= 0) {
                this.auction.state = "closed"
                return
            }

            display = document.querySelector('#time');
            

            if (display == null) setTimeout(this.relog, 100)
            else
                startTimer(duration, display);
        },
        //
        loginGroup() {
            this.connection.invoke("loginGroup", ""+this.country).catch(error => {
                console.error("ERROR IN CONNECTION")
                this.connection.stop()
            })
        },
        //
    },   
    //
    computed: {
        tlfTamano() {
            if (document.body.clientWidth <= 900) return 0 

            return 1
        }
    },
    //
    created: function () {
        let idFact = JSON.parse(document.getElementById('factor').value)
        this.token = document.getElementById('token').value
       
        if (idFact != "") {
            try {
                this.filter = JSON.parse(document.getElementById('filterData').value);
                document.getElementById("eliminarData").removeChild(document.getElementById('filterData'));
            } catch (e) {
                this.filter = null
            }
        
        }
        
        if (idFact != null) this.idFactor = idFact
        
        document.getElementById("eliminarData").removeChild(document.getElementById('factor'))
        document.getElementById("eliminarData").removeChild(document.getElementById('token'))

        document.getElementById("appMercadoFacturas").removeAttribute("hidden")
        this.lang = document.getElementsByTagName("html")[0].getAttribute("lang")
        this.cargando = false;

        this.loading = false
    },
    //
    mounted: async function () {
        tiempoLogin(this.modalLogout)
        await this.llenarFacturas()
        await this.llenarAuction()
    }
});
