new Vue({
    el: "#app",
    i18n,
    store: vuexLayout,
    vuetify: new Vuetify({
        lang: {
            t: (key, ...params) => i18n.t(key, params)
        }
    }),
    data: {
        filtersIsEmpty: filtersIsEmpty,
        filterIsEmpty: filterIsEmpty,
        arrayCondition: arrayCondition,
        filter: [],
        totalFacturas: [],
        dialogAyuda: false,
        buscarFacturas: [],
        loading: [],
        options: [],
        modalLogout: { mostrar: false },
        perPage: 9,
        itemsPerPageOptions:[3,9,18, -1],
        tamanoTlf: tamanoTlf,
        headers: [
            { text: i18n.t("headers.n"), value: "n", align: "center" },
            { text: i18n.t("headers.cliente"), value: "client", align: "center" },
            { text: i18n.t("headers.bancoConfirmante"), value: "confirmant", align: "center" },
            { text: i18n.t("headers.numero"), value: "number", align: "center" },
            { text: i18n.t("headers.fechaVencimiento"), value: "expiration_date", align: "center" },
            { text: i18n.t("headers.valorNeto"), value: "amount", align: "center" },
            { text: i18n.t("headers.publicar"), value: "public", align: "center" }
        ],
        menu:false,
        hints: true,
        fav: true,
        message: false,
        cerrarMordisco: true,
        moment: moment,
        facturas:[],
        formatoMonedaInput: formatoMonedaInput,
        backEndDateFormat: backEndDateFormat,
        formatoMoneda: formatoMoneda,
        lang:"es",
        REPuntos: /[.]{2,}/,
        REComas: /[,]{2,}/,
        cargando: true,
        envio: false,
        page: 1,
        pageCount: 10,
        facturas: [],
        facturasFiltradas: [],
        symbolTitle: '',
        filtrarBanco: '',
        filtrarCliente: ''
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

        document.getElementById("app").removeAttribute("hidden")
        this.lang = document.getElementsByTagName("html")[0].getAttribute("lang")
        this.cargando = false;

    },
    mounted: async function () {
        tiempoLogin(this.modalLogout)

        for (var i = 0; i < this.currencies.length; i++) {
            await this.cargarFacturas(i);
        }

        setTimeout(() => iniciarButtonFilters(), 500)
    },
    watch: {
        options: {
            async handler() {

                for (var i = 0; i < this.currencies.length; i++) {
                    if (this.options[i].itemsPerPage == -1) {
                        llamadaRecursiva(this.buscarFacturas[i], this.cargarFacturas, i)
                        return
                    }

                    var tamanoFacturas = filtrarPublicationsCurrency(this.facturasFiltradas, this.currencies[i].id).length

                    if (this.buscarFacturas[i] && this.options[i].page * this.options[i].itemsPerPage >= tamanoFacturas - this.options[i].itemsPerPage) {
                        await this.cargarFacturas(i)
                    }
                }
            },
            deep: true,
        },
    },
    methods: {
        async cargarFacturas(indice) {
            if (this.loading[indice]) return
            this.loading[indice] = true
            recallVueArray(this.loading);
            var take = 100

            await axios.post('?handler=postponed', { pagination: { take: take, skip: this.totalFacturas[indice] }, filter: this.filter[indice] },
                {
                    headers: {
                        "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                    }
                })
                .then((r) => {
                    resetTime()
                    this.loading[indice] = false
                    recallVueArray(this.loading);
                    if (r.data == null || r.data.length == 0) {
                        this.buscarFacturas[indice] = false
                        return
                    }

                    if (r.data.length >0 && r.data[0].errors == notAuthorized) {
                        window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                        return
                    }

                    if (r.data.length < take / 2)
                        this.buscarFacturas[indice] = false

                    this.totalFacturas[indice] += r.data.length

                    r.data.map(data => {
                        if (data == null) return

                        for (var i = 0; i < this.facturas.length; i++) {
                            if (this.facturas[i].id == data.id) {
                                this.totalFacturas--
                                return
                            }
                        }

                        if (data != null) {
                            this.facturas.push(data)
                        }
                    })

                    this.facturasFiltradas = this.facturas

                    /*r.data.map(data => {
                        if (data == null) return

                            for (var i = 0; i < this.facturasFiltradas.length; i++) {
                                if (this.facturasFiltradas[i].id == data.id) {
                                    this.totalFacturas--
                                    return
                                }
                            }

                            this.facturasFiltradas.push(data)

                    })*/
                }).catch((respond) => {
                    console.log(respond);
                    this.loading[indice] = false
                    recallVueArray(this.loading);
                });
            recallVueArray(this.buscarFacturas)

            console.log(this.facturas)
            return (this.buscarFacturas[indice] && this.options[indice].itemsPerPage == -1)
        },
        //
        ordenarLista() {
            this.facturasFiltradas.sort((a, b) => a.debtor.name.toLowerCase() < b.debtor.name.toLowerCase() ? -1 : +(a.debtor.name.toLowerCase() > b.debtor.name.toLowerCase()))
        },

        methodPublicationsCurrency(idCurrency) {
            return filtrarPublicationsCurrency(this.facturas, idCurrency)
        },
        
        publicar(item) {
            if (this.envio) return

            if (item == null || item.id == null) {
                toastr.warning(i18n.t("facturaInvalida"))
                return
            }
            this.envio = true
            axios.post('?handler=publicar', { invoice_id: item.publications[0].id },
                {
                    headers: {
                        "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                    }
                }).then(resp => {
                    resetTime()
                    this.envio = false

                    if(resp.data == null) resp.data = resp

                    if (typeof resp.data === 'string' || resp.data instanceof String) {

                        if (resp.data.includes("<!DOCTYPE html>")) {
                            window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired";
                            //toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br>" + i18n.t("errorBaseDatos"));
                            return;
                        }
                    }

                    if (resp.data.errors == notAuthorized) {
                        window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                        return
                    }

                    if (resp.data.error != null) {
                        if (resp.data.error.Message == "Invalid User") {
                            window.location.pathname = "../Index"
                            return
                        }

                        toastr.warning(i18n.tc("mensajesModal.problemasRespuesta", 2, { 0: "publicación de factura" }))
                        return
                    }
                 
                    item.publications[0].state = 'published'
                    toastr.success(i18n.t("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.tc("mensajesModal.publicarUnaFactura"))
                }).catch(error => {
                    console.log(error); toastr.error(i18n.t("errorRespuesta")); this.envio = false
        })

        },
        
    },
    
})