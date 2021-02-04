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
        culture:"",
        options: {},
        buscarFacturas: true,
        totalFacturas: 0,
        token: null,
        loading: false,
        filter: null,
        page:1,
        perPage: 9,
        itemsPerPageOptions: [3,9,18,-1],
        indexActual: -1,
        dialogVer: false,
        tamanoTlf: tamanoTlf,
        header: [
            { text: i18n.t("headers.n"), value: "n", align: "center" },
            { text: i18n.t("headers.clienteBanco"), value: "client", align: "center" },
            { text: i18n.t("headers.fechaVencimientoNumero"), value: "expiration_date", align: "center" },
            { text: i18n.t("headers.valorNeto"), value: "neto", align: "center" },
            { text: i18n.t("headers.ofertaPropuesta"), value: "offert", align: "center" },
            { text: i18n.t("headers.montoDescontarAnualizado"), value: "amount", align: "center" },
            { text: i18n.t("headers.montoRecibir"), value: "reciv", align: "center" }
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
        auction: { state: '', opened: '' },
        digits: 2,
        cargando: true,
        REComas: /[,]{2,}/,
        dateMin: moment().format('YYYY-MM-DD'),
        dateMax: moment().add(2, 'year').format('YYYY-MM-DD'),
        validateDate: '',
        validateAmount: '',
        filtroDateI: moment().format('YYYY-MM-DD'),
        filtroDateF: '',
        filtroAmountI: '',
        filtroAmountF: '',
        filtroBanco: '',
        page: 1,
        pageCount:10,
        facturas: [],
        facturasFiltradas: []
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
                    
                }).catch((respond) => { console.log(respond); this.loading = false });

            return (this.buscarFacturas && this.options.itemsPerPage == -1)
        },

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

                    this.relog()

                    this.country = this.auction.country

                    this.connection = new signalR.HubConnectionBuilder().withUrl("/wsSubastas").build()

                    this.connection.start().then(this.loginGroup).catch(function (err) {
                        return console.error(err.toString());
                    });

                    this.connection.on("Publications", this.entrada)

                    var culture = this.culture

                    this.connection.on("Cierre", function (mensaje) {
                        if (mensaje == "close") {
                            window.location.pathname = culture+"/supplier/CierreMercado"
                        }
                    })

                }).catch((respond) => { console.log(respond); });
        },

        validarInputBuscadores(event) {
            if ((event.keyCode < 48 || event.keyCode > 57) && (event.keyCode != 44 && event.keyCode != 46)) event.returnValue = false;
        },

        entrada: function (message) {
        
                this.envio = false
            
                for (let i = 0; i < this.facturas.length; i++) {
                    if (this.facturas[i].id == message.id) {
                        this.facturas[i].discount = message.discount
                        this.facturas[i].bids = [{ factor: { id: '' } }]
                        this.facturas[i].bids[0].factor.id = message.bids[0].factor.id
                        this.facturas[i].payable = message.payable
                        this.facturas[i].commission = message.commission
                        this.facturas[i].profitability = message.profitability
                        this.facturas[i].earnings = message.earnings
                        break
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
        
        loginGroup() {
            this.connection.invoke("loginGroup", this.country).catch(error => {
                console.error("ERROR IN CONNECTION")
                this.connection.stop()
            })
        },
        //
    },
    created: function () {
        try {
            this.filter = JSON.parse(document.getElementById('filterData').value);
            this.culture = document.getElementById('culture').value;
            document.getElementById("eliminarData").removeChild(document.getElementById('filterData'));
            document.getElementById("eliminarData").removeChild(document.getElementById('culture'));
        } catch (e) {
            this.filter = null
        }

        document.getElementById("appMercadoFacturas").removeAttribute("hidden")
        this.lang = document.getElementsByTagName("html")[0].getAttribute("lang")
        this.cargando = false;
        
    },
    mounted: async function () {
        tiempoLogin(this.modalLogout)

        await this.llenarFacturas()
        await this.llenarAuction()
    }
});
