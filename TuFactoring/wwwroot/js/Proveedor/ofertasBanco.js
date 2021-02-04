new Vue({
    el: "#appOfertasBanco",
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
        currencies: {},
        filter: [],
        totalFacturas: [],
        buscarFacturas: [],
        loading: [],
        options: [],
        modalLogout: { mostrar: false },
        dialogConfirmPosponed: false,
        dialogAyuda: false,
        dialogConfirmPublish: false,
        moment: moment,
        perPage: 9,
        perPage2: 9,
        itemsPerPageOptions: [3, 9, 18, -1],
        indexActual: -1,
        dialogVer: false,
        mostrarOfertas: true,
        mostrarPublicar: true,
        tamanoTlf: tamanoTlf,
        backEndDateFormat: backEndDateFormat,
        formatoMonedaInput: formatoMonedaInput,
        lang: "es",
        cargando: true,
        envio: false,
        currencyValidator: {},
        tabla: [],
        cargando: false,
        ofertadas: [],
        facturas: [],
        dialogSeguro: false,
        dialogSeguroRechazo: false,
        dialogSeguroPosponer: false,
        dialogSeguroPublicar: false,
        mensajeActual: -1,
        mensajesDialog: [
            i18n.t("ofertasBancoSeguro"),
            i18n.t("rechazarBancoSeguro"),
            i18n.t("publicaFacturaSeguro"),
            i18n.t("posponerFacturaSeguro"),
            i18n.t("posponerUnaFacturaSeguro"),
            i18n.t("publicarUnaFacturaSeguro")
        ],
        confirmadas: [],
        nuevas: [],
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

        document.getElementById("appOfertasBanco").removeAttribute("hidden")
        this.lang = document.getElementsByTagName("html")[0].getAttribute("lang")
        this.cargando = false;
    },
    mounted: async function () {
        tiempoLogin(this.modalLogout)

        for (var i = 0; i < this.currencies.length; i++) {
            await this.cargarOfertadas(i);
        }

        setTimeout(() => iniciarButtonFilters(), 500)
    },
    watch: {
        options: {
            async handler() {

                for (var i = 0; i < this.currencies.length; i++) {
                    if (this.options[i].itemsPerPage == -1) {
                        llamadaRecursiva(this.buscarFacturas[i], this.cargarOfertadas, i)
                        return
                    }

                    var tamanoFacturas = filtrarPublicationsCurrency(this.facturas, this.currencies[i].id).length

                    if (this.buscarFacturas[i] && this.options[i].page * this.options[i].itemsPerPage >= tamanoFacturas - this.options[i].itemsPerPage) {
                        await this.cargarOfertadas(i)
                    }
                }
            },
            deep: true,
        },
    },
    methods: {
        cargarOfertadas: async function (indice) {
            if (this.loading[indice]) return
            this.loading[indice] = true
            var take = 100

            await axios.post('?handler=releasable', { pagination: { take: take, skip: this.totalFacturas[indice] }, filter: this.filter[indice], type: "offered" },
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

                }).catch(e => { console.log(e); this.loading[indice] = false });

            recallVueArray(this.buscarFacturas) 
            return (this.buscarFacturas[indice] && this.options[indice].itemsPerPage == -1)
        },
        //
        methodPublicationsCurrency(idCurrency) {
            return filtrarPublicationsCurrency(this.facturas, idCurrency)
        },
        //Indica al sistema que se acepta la oferta de la factura seleccionada
        vender: async function (indice) {
            if (this.envio) return

            this.envio = true

            await axios.post('?handler=vender', { invoice_id: this.facturas[indice].id },
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
                        toastr.warning(i18n.t("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.t("mensajesModal.problemaAceptarOfertaBanco"))
                        return
                    }

                    this.facturas.splice(indice, 1)

                    toastr.success(i18n.t("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.t("mensajesModal.aceptarOfertaBanco"))
                }).catch((e) => { console.log(e); toastr.error(i18n.t("errorRespuesta")); this.envio = false; })

        },
        //Rechazar la oferta del banco
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
                    toastr.success(i18n.t("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.t("mensajesModal.rechazarOfertaBanco"))
                }).catch((e) => { console.log(e); toastr.error(i18n.t("errorRespuesta")); this.envio = false })

        },
        //
        accionBoton: function (currency) {
            switch (this.mensajeActual) {
                case 0: this.venderAll(currency); break;
                case 1: this.rechazarAll(currency); break;
            }
        },
        //
        venderAll: async function (item) {
            if (this.envio) return
            this.envio = true

            facturasFiltradas = filtrarPublicationsCurrency(this.facturas, item.id)

            var ids = []

            facturasFiltradas.map(data => {
                ids.push({ publication_id: data.id })
            })

            await axios.post('?handler=venderAll', ids,
                {
                    headers: {
                        "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                    }
                })
                .then((resp) => {
                    resetTime()
                    this.envio = false
                    var estado = 0
                    if (resp.data != null)
                        resp = resp.data

                    if (typeof resp === 'string' || resp instanceof String) {

                        if (resp.includes("<!DOCTYPE html>")) {
                            window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired";
                            toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br>" + i18n.t("errorBaseDatos"));
                            return;
                        }
                    }

                    for (var i = 0; i < resp.length; i++) {

                        if (resp[i] != null && resp[i].errors == notAuthorized) {
                            window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                            return
                        }

                        if (resp[i].error != null) {
                            estado = 1;
                            continue
                        }

                        for (var w = 0; w < this.facturas.length; w++) {
                            if (this.facturas[w].id == resp[i].id) {
                                this.facturas.splice(w, 1)
                                break
                            }
                        }
                    }

                    if (estado == 0) {
                        toastr.success(i18n.t("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.t("mensajesModal.ofertaAceptada"))
                    } else {
                        toastr.warning(i18n.tc("mensajesModal.problemasRespuesta", 2, { 0: "aceptaci�n de venta de facturas" }))
                    }

                }).catch((e) => { console.log(e); toastr.error(i18n.t("errorRespuesta")); this.envio = false; })

        },
        //
        rechazarAll: async function (item) {
            if (this.envio) return

            this.envio = true

            facturasFiltradas = filtrarPublicationsCurrency(this.facturas, item.id)

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
                .then((resp) => {
                    resetTime()
                    this.envio = false
                    var estado = 0

                    if (resp.data != null)
                        resp = resp.data

                    if (typeof resp === 'string' || resp instanceof String) {

                        if (resp.includes("<!DOCTYPE html>")) {
                            window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired";
                            toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br>" + i18n.t("errorBaseDatos"));
                            return;
                        }
                    }

                    resp.map(data => {

                        if (data != null && data.errors == notAuthorized) {
                            window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                            return
                        }

                        if (data == null || data.error != null) {
                            estado = 1
                            return
                        }

                        for (var i = 0; i < this.facturas.length; i++) {

                            if (this.facturas[i].id == data.id) {
                                this.facturas.splice(i, 1)
                            }
                        }
                    })

                    if (estado == 0) {
                        toastr.success(i18n.t("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.t("mensajesModal.ofertaRechazada"))
                    } else {
                        toastr.warning(i18n.tc("mensajesModal.problemasRespuesta", 2, { 0: "rechazo de facturas" }))
                    }

                }).catch((e) => { console.log(e); toastr.error(i18n.t("errorRespuesta")); this.envio = false })
        },      
    },

});