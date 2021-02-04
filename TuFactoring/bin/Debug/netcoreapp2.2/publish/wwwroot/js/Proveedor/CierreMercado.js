new Vue({
    el: '#appCierreMercado',
    i18n,
    store: vuexLayout,
    vuetify: new Vuetify({
        lang: {
            t: (key, ...params) => i18n.t(key, params)
        }
    }),
    data: {
        modalLogout: { mostrar: false },
        dialogSeguro: false,
        indexActual:-1,
        perPage: 9,
        itemsPerPageOptions:[3,9,18],
        tamanoTlf: tamanoTlf,
        header: [
            { text: i18n.t("headers.n"), value: "n", align: "center" },
            { text: i18n.t("headers.clienteComprador"), value: "client", align: "center" },
            { text: i18n.t("headers.fechaVencimientoNumero"), value: "expiration_date", align: "center" },
            { text: i18n.t("headers.valorNeto"), value: "valorneto", align: "center" },
            { text: i18n.t("headers.ofertaPropuesta"), value: "offert", align: "center" },
            { text: i18n.t("headers.montoDescontarAnualizado"), value: "amount", align: "center" },
            { text: i18n.t("headers.comisionServicio"), value: "comission", align: "center" },
            { text: i18n.t("headers.montoRecibir"), value: "amount_reciv", align: "center" },
            { text: i18n.t("headers.opciones"), value: "opciones", align: "center" },
        ],
        dialog:false,
        menu: false,
        hints: true,
        fav: true,
        message: false,
        cerrarMordisco: true,
        filter: {},
        totalFacturas: [],
        buscarFacturas: [],
        loading: [],
        options: [],
        moment: moment,
        formatoMonedaInput: formatoMonedaInput,
        backEndDateFormat: backEndDateFormat,
        formatoMoneda: formatoMoneda,
        lang: "es",
        envio:false,
        auction: { state: '', closed: '' },
        digits: 2,
        idCurrency:0,
        compra: '',
        facturasFiltradas: [],
        filtrarFactor: '',
        filtrarDebtor: '',
        REComas: /[,]{2,}/,
        dateMin: moment().subtract(100, 'year').format('YYYY-01-01'),
        dateMax: moment().add(100, 'year').format('YYYY-MM-DD'),
        validateDate: '',
        validateAmount: '',
        marketNotClosed:false,
        filtroDateI: '',
        filtroDateF: '',
        filtroAmountI: '',
        filtroAmountF: '',
        filtroBanco: '',
        cargando: true,
        facturas: [],
        bancos: [],
        nuevas: [],
        empresas: [], //prueba
    },
    created: function () {
        this.cargando = true;
        let auction = JSON.parse(document.getElementById('Auction').value);

        auction.length != 0 ? this.auction = auction[0] : ''

        if (this.auction.state != "closed") {
            this.facturas = []
            this.marketNotClosed = true
            this.facturasFiltradas = []
        } else {

            this.marketNotClosed = false
            /*this.facturas = (datosFactura === null ? [] : datosFactura.filter(data => data != null));

            this.facturasFiltradas = this.facturas

            this.facturas.length > 0 ? this.digits = this.facturas[0].invoice.currency.digits : 2*/

            window.onload = this.relog

        }

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

        document.getElementById("appCierreMercado").removeAttribute("hidden")
        this.lang = document.getElementsByTagName("html")[0].getAttribute("lang")
        this.cargando = false;
        tiempoLogin(this.modalLogout)

    },
    mounted: async function () {
        tiempoLogin(this.modalLogout)

        for (var i = 0; i < this.currencies.length; i++) {
            await this.llenarFacturas(i);
        }

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
            await axios.post('?handler=Deals', { pagination: { take: take, skip: this.totalFacturas[indice] }, filter: this.filter[indice] },
                {
                    headers: {
                        "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                    }
                })
                .then((respond) => {
                    resetTime()
                    console.log(respond)
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
        aComprar: function (index) {
            this.compra = { id: "" }
            if(index !== null && index !== '')
                this.compra = this.facturas[index]

            
        },
        //
        vender() {

            if(this.envio) return

            if (this.auction.state != "closed") {
                toastr.warning(i18n.t("cierreNoActivo"))
                return
            }

            $('#modal').modal('toggle');

            this.envio = true
            axios.post("?handler=aceptar", { bid_id: this.compra.bids[0].id, publication_id: this.compra.id },
                {
                    headers: {
                        "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                    }
                }).then(resp => {
                    resetTime()
                    if(resp.data == null) resp.data = resp

                    if (typeof resp.data === 'string' || resp.data instanceof String) {

                        if (resp.data.includes("<!DOCTYPE html>")) {
                            window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired";
                            toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br>" + i18n.t("errorBaseDatos"));
                            return;
                        }
                    }

                    if (resp.data.errors == notAuthorized) {
                        window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                        return
                    }

                    if (resp.data == null || resp.data.error != null) {

                        this.compra = ''
                        this.envio = false

                        toastr.warning(i18n.tc("mensajesModal.problemasRespuesta", 2, {0:"aceptaci�n de venta de factura"}))
                        return
                    }
                    
                    for (let i = 0; i < this.facturas.length; i++) {
                        if (this.facturas[i].id == resp.data.id) {
                            this.facturas.splice(i, 1)
                            break
                        }
                    }
                    this.envio = false

                    toastr.success(i18n.t("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.t("mensajesModal.aceptarOfertaCierreMercado"))
                }).catch(e => { console.log(e); toastr.error(i18n.t("errorRespuesta")); this.compra = ''; this.envio = false;  })
        },
        //Rechazar oferta del inversionista
        rechazar() {
            if (this.envio) return

            if (this.auction.state != "closed") {
                toastr.warning(i18n.t("cierreNoActivo"))
                return
            }

            $('#modal').modal('toggle');

            this.envio = true
            let rechazada = this.compra.id

                axios.post('?handler=rechazar', { publication_id: rechazada },
                {
                    headers: {
                        "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                    }
                })
                .then((resp) => {
                    resetTime()
                    if (resp.data == null) resp.data = resp

                    if (typeof resp.data === 'string' || resp.data instanceof String) {

                        if (resp.data.includes("<!DOCTYPE html>")) {
                            window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired";
                            toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br>" + i18n.t("errorBaseDatos"));
                            return;
                        }
                    }


                    if (resp.data.errors == notAuthorized) {
                        window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                        return
                    }

                    if (resp.data == null || resp.data.error != null) {

                        this.compra = ''
                        this.envio = false

                        toastr.warning(i18n.tc("mensajesModal.problemasRespuesta", 2, { 0: "rechazo de venta de factura" }))
                        return
                    }

                    for (let i = 0; i < this.facturas.length; i++) {
                        if (this.facturas[i].id == resp.data.id) {
                            this.facturas.splice(i, 1)
                            break
                        }
                    }
                    
                    this.envio = false

                    toastr.success(i18n.t("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.t("mensajesModal.rechazarOfertaBanco"))
                }).catch((e) => { console.log(e); toastr.error(i18n.t("errorRespuesta")); this.envio = false })

        },
        //
        funcionIntermedia(idCurrency) {
            this.dialogSeguro = true
            this.idCurrency = idCurrency
        },
        //
        venderAll(idCurrency) {

            if (this.envio || this.facturas.length == 0) return

            if (this.auction.state != "closed") {
                toastr.warning(i18n.t("cierreNoActivo"))
                return
            }
            
            
            this.envio = true
            
            var invoices = []

            let filteredInvoices = []

            filteredInvoices = filtrarPublicationsCurrency(this.facturas, idCurrency)

            filteredInvoices.map(data => invoices.push({ bid_id: data.bids[0].id, publication_id:data.id }))
            
            axios.post("?handler=aceptarAll", invoices,
                {
                    headers: {
                        "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                    }
                }).then(resp => {
                    resetTime()
                    if (resp.data == null) resp.data = resp


                    if (typeof resp.data === 'string' || resp.data instanceof String) {

                        if (resp.data.includes("<!DOCTYPE html>")) {
                            window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired";
                            toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br>" + i18n.t("errorBaseDatos"));
                            return;
                        }
                    }

                    if (resp.data == null || resp.data.length == 0) {

                        this.compra = ''
                        this.envio = false

                        toastr.warning(i18n.tc("mensajesModal.problemasRespuesta", 2, { 0: "aceptaci�n de venta de factura" }))
                        return
                    }

                    var state = 0

                    resp.data.map(data => {
                        if (data.errors == notAuthorized) {
                            window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                            return
                        }

                        if (data.error != null) {
                           
                            state = 1
                            return
                        }

                        for (let i = 0; i < this.facturas.length; i++) {
                            if (this.facturas[i].id == data.id) {
                                this.facturas.splice(i, 1)
                                break
                            }
                        }

                    })
                    this.envio = false

                    if (state == 0) toastr.success(i18n.t("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.t("mensajesModal.aceptarTodasOfertas"))
                    else toastr.warning(i18n.tc("mensajesModal.problemasRespuesta", 2, { 0: "aceptaci�n de venta de factura" }))
                }).catch(e => { console.log(e); toastr.error(i18n.t("errorRespuesta")); this.compra = ''; this.envio = false; })
        },
        //
        methodPublicationsCurrency(idCurrency) {
            return filtrarPublicationsCurrency(this.facturas, idCurrency)
        },
        //
        relog: function () {
            var duration = moment(this.auction.closed).diff(moment(), "second")

            if (duration <= 0) {
                this.auction.state = "payed"
                return
            }

            display = document.querySelector('#time');
            startTimer(duration, display);

        },
        //
       
    },
  
});
