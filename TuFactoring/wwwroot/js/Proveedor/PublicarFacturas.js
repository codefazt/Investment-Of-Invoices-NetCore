new Vue({
    el: "#appPublicarFacturas",
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
        currencyUnisono: {},
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

        document.getElementById("appPublicarFacturas").removeAttribute("hidden")
        this.lang = document.getElementsByTagName("html")[0].getAttribute("lang")
        this.cargando = false;
    },
    mounted: async function () {
        tiempoLogin(this.modalLogout)

        for (var i = 0; i < this.currencies.length; i++) {
            this.buscarFacturas[i] = false
            await this.cargarConfirmadas(i);
        }

        setTimeout(() => iniciarButtonFilters(), 500)
    },
    watch: {
        options: {
            async handler() {

                for (var i = 0; i < this.currencies.length; i++) {
                    if (this.options[i].itemsPerPage == -1) {
                        llamadaRecursiva(this.buscarFacturas[i], this.cargarConfirmadas, i)
                        return
                    }

                    var tamanoFacturas = filtrarPublicationsCurrency(this.facturas, this.currencies[i].id).length

                    if (this.buscarFacturas[i] && this.options[i].page * this.options[i].itemsPerPage >= tamanoFacturas - this.options[i].itemsPerPage) {
                        await this.cargarConfirmadas(i)
                    }
                }
            },
            deep: true,
        },
    },
    methods: {
        cargarConfirmadas: async function (indice) {
            if (this.loading[indice]) return
            this.loading[indice] = true
            var take = 100

            await axios.post('?handler=releasable', { pagination: { take: take, skip: this.totalFacturas[indice] }, filter: this.filter[indice], type: "released" },
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
        //
        publicar: async function (indice) {
          if (this.envio) return

        this.envio = true
          
        await axios.post('publicarFacturas?handler=publicar', { invoice_id: this.facturas[indice].id},
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
                } else if (resp.errors == "entity don't have accounts") {
                    toastr.warning(i18n.tc("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.tc("mensajesModal.errorEntityDontHaveAccountPublication"))
                    return
                } else if (resp.errors == "backOffice don't have accounts with entity") {
                    toastr.warning(i18n.tc("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.tc("mensajesModal.errorBackOfficeDontHaveAccountPostulation"))
                    return
                }

                if (resp.error != null) {
                    if (resp.error.Message == "Invalid User") {
                        window.location.pathname ="../Index"
                        return
                    }

                    toastr.warning(i18n.tc("mensajesModal.problemasRespuesta", 2, { 0: "publicaci�n de factura" }))
                    return
                }
                this.facturas.splice(indice,1)
                toastr.success(i18n.t("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.t("mensajesModal.publicarUnaFactura"))
            }).catch((e) => { console.log(e); toastr.error(i18n.t("errorRespuesta")); this.envio = false })
        
    },
      //Indica al sistema que la factura seleccionada es dejada hasta su vencimiento
      posponer: async function (arreglo, indice) {
        if(this.envio) return
     
        let id =0

        if (arreglo === 'facturas') {
            id = this.facturas[indice].id
        } else {
            id = this.ofertadas[indice].id
        }

          this.envio = true
        axios.post('publicarFacturas?handler=posponer', { invoice_id: id },
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
                    toastr.warning(i18n.tc("mensajesModal.problemasRespuesta", 2, { 0: "posponer la factura" }))
                    return
                }

                if (arreglo == "facturas") {
                    this.facturas.splice(indice,1)
                } else {
                    this.ofertadas.splice(indice,1)
                }

                toastr.success(i18n.t("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.t("mensajesModal.posponerFactura"))
            }).catch((e) => { console.log(e); toastr.error(i18n.t("errorRespuesta")); this.envio = false; })
          
        },
        //
        accionBoton: function (currency) {
            switch (this.mensajeActual) {
                case 2: this.publicarAll(currency); break;
                case 3: this.posponerAll(currency); break;
                case 4: this.posponer(); break;
                case 5: this.publicar(); break;
            }
        },
        publicarAll: async function (item) {
            if (this.envio) return

            this.envio = true

            facturasFiltradas = filtrarPublicationsCurrency(this.facturas, item.id)

            var invoices = []

            facturasFiltradas.map(data => {
                invoices.push({ invoice_id: data.id })
            })

            await axios.post('?handler=publicarAll', invoices,
                {
                    headers: {
                        "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                    }
                })
                .then((resp) => {
                    resetTime()
                    this.envio = false
                    var status = 0
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

                        if (data.error != null) {
                            status = 1
                            return
                        }
                       
                        for (var i = 0; i < this.facturas.length; i++) {
                           if (this.facturas[i].id == data.id) {
                                this.facturas.splice(i, 1)
                                break
                            }
                        }
                    })

                    if (status == 0) {
                        toastr.success(i18n.t("mensajesModal.estimadoUsuario") + "<br><br>" +i18n.t("mensajesModal.publicacionFactura"))
                    } else {
                        toastr.warning(i18n.tc("mensajesModal.problemasRespuesta", 2, { 0: "publicaci�n de facturas" }))
                    }

                    
                }).catch((e) => { console.log(e); toastr.error(i18n.t("errorRespuesta")); this.envio = false })
        },
        //
        posponerAll: async function (item) {
            if (this.envio) return

            facturasFiltradas = filtrarPublicationsCurrency(this.facturas, item.id)
            
            var invoices = []

            facturasFiltradas.map(data => {
                invoices.push({ invoice_id: data.id })
            })

            this.envio = true
            await axios.post('?handler=posponerAll', invoices,
                {
                    headers: {
                        "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                    }
                })
                .then((resp) => {
                    resetTime()
                    this.envio = false
                    var status = 0

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

                        if (resp.error != null) {
                            status = 1
                            return
                        }

                        for (var i = 0; i < this.facturas.length; i++) {
                            if (this.facturas[i].id == data.id) {
                                this.facturas.splice(i, 1)
                            }
                        }
                    })

                    if (status == 0) {
                        toastr.success(i18n.t("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.t("mensajesModal.posponerTodas"))
                    } else {
                        toastr.warning(i18n.tc("mensajesModal.problemasRespuesta", 2, { 0: "posponer las facturas" }))
                    }
                
                }).catch((e) => { console.log(e); toastr.error(i18n.t("errorRespuesta")); this.envio = false; })

        },
    },
   
});
