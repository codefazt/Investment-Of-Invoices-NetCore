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
        modalLogout: { mostrar: false },
        loading: false,
        totalFacturas: 0,
        buscarFacturas: true,
        options: {},
        loading2: false,
        dialogConfirmPosponed: false,
        dialogConfirmPublish: false,
        totalFacturas2: 0,
        buscarFacturas2: true,
        options2: {},
        filter: {},
        filter2: {},
        moment: moment,
        perPage: 9,
        perPage2: 9,
        itemsPerPageOptions: [3, 9, 18,-1],
        itemsPerPageOptions2: [3, 9, 18,-1],
        indexActual: -1,
        dialogVer: false,
        mostrarOfertas: true,
        mostrarPublicar: true,
        tamanoTlf: tamanoTlf,
        backEndDateFormat: backEndDateFormat,
        formatoMonedaInput: formatoMonedaInput,
        lang:"es",
        cargando:true,
        envio: false,
        tabla: [],
        cargando: false,
        ofertadas: [],
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
    watch: {
        options: {
            async handler() {
                if (this.options.itemsPerPage == -1) {
                    llamadaRecursiva(this.buscarFacturas, this.cargarOfertadas)
                    return
                }

                if (this.buscarFacturas && this.options.page * this.options.itemsPerPage >= this.ofertadas.length - this.options.itemsPerPage) {
                    await this.cargarOfertadas()
                }
            },
            deep: true,
        },
        options2: {
            async handler() {
                if (this.options2.itemsPerPage == -1) {
                    llamadaRecursiva(this.buscarFacturas2, this.cargarConfirmadas)
                    return
                }

                if (this.buscarFacturas2 && this.options2.page * this.options2.itemsPerPage >= this.confirmadas.length - this.options2.itemsPerPage) {
                    await this.cargarConfirmadas()
                }
            },
            deep: true,
        },
    },
    methods: {
        cargarOfertadas: async function () {
            if (this.loading) return
            this.loading = true
            var take = 100

            await axios.post('?handler=releasable', { pagination: { take: take, skip: this.totalFacturas }, filter: this.filter, type: "offered" },
                {
                    headers: {
                        "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                    }
                })
                .then((r) => {
                    resetTime()
                    this.loading = false
                    if (r.data == null || r.data.length == 0) {
                        this.buscarFacturas = false
                        return
                    }

                    if (r.data.length > 0 && r.data[0].errors == notAuthorized) {
                        window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                        return
                    }

                    if (r.data.length < take / 2)
                        this.buscarFacturas = false
                    
                    this.totalFacturas += r.data.length

                    r.data.map(data => {
                        if (data != null)
                            this.ofertadas.push(data)

                    })
                    
                }).catch(e => { console.log(e); this.loading = false });

            return (this.buscarFacturas && this.options.itemsPerPage == -1)
        },
        cargarConfirmadas: async function () {
            if (this.loading2) return
            this.loading2 = true
            var take = 100
            
            await axios.post('?handler=releasable', { pagination: { take: take, skip: this.totalFacturas2 }, filter: this.filter2, type: "released" },
                {
                    headers: {
                        "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                    }
                })
                .then((r) => {
                    resetTime()
                    this.loading2 = false
                    if (r.data == null || r.data.length == 0) {
                        this.buscarFacturas2 = false
                        return
                    }

                    if (r.data.length > 0 && r.data[0].errors == notAuthorized) {
                        window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                        return
                    }

                    if (r.data.length < take / 2)
                        this.buscarFacturas2 = false

                    this.totalFacturas2 += r.data.length

                    r.data.map(data => {
                        if (data != null)
                            this.confirmadas.push(data)
                    })
                }).catch(e => { console.log(e); this.loading2 = false });

            return (this.buscarFacturas2 && this.options2.itemsPerPage == -1)
        },
      publicar: async function (indice) {
          if (this.envio) return

        this.envio = true
          
        await axios.post('publicarFacturas?handler=publicar', { invoice_id: this.confirmadas[indice].id},
            {
                headers: {
                    "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                }
            })
            .then((resp) => {
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
                        window.location.pathname ="../Index"
                        return
                    }

                    toastr.warning(i18n.tc("mensajesModal.problemasRespuesta", 2, { 0: "publicaci�n de factura" }))
                    return
                }
                this.confirmadas.splice(indice,1)
                toastr.success(i18n.t("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.t("mensajesModal.publicarUnaFactura"))
            }).catch((e) => { console.log(e); toastr.error(i18n.t("errorRespuesta")); this.envio = false })
        
    },
      //Indica al sistema que se acepta la oferta de la factura seleccionada
      vender: async function (indice) {
          if (this.envio) return

          this.envio = true

        await axios.post('publicarFacturas?handler=vender', { invoice_id: this.ofertadas[indice].id },
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

                this.ofertadas.splice(indice,1)

                toastr.success(i18n.t("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.t("mensajesModal.aceptarOfertaBanco"))
            }).catch((e) => { console.log(e); toastr.error(i18n.t("errorRespuesta")); this.envio = false;})
      
      },
      //Rechazar la oferta del banco
      rechazarOfertada: async function (indice) {
          if(this.envio) return

              this.envio = true
          let rechazada = this.ofertadas[indice].id

          await axios.post('publicarFacturas?handler=rechazar', { invoice_id : rechazada} ,
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

                    this.ofertadas.splice(indice,1)
                    toastr.success(i18n.t("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.t("mensajesModal.rechazarOfertaBanco"))
                }).catch((e) => { console.log(e); toastr.error(i18n.t("errorRespuesta")); this.envio = false })
        
      },
      //Indica al sistema que la factura seleccionada es dejada hasta su vencimiento
      posponer: async function (arreglo, indice) {
        if(this.envio) return
     
        let id =0

        if (arreglo === 'confirmadas') {
            id = this.confirmadas[indice].id
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

                if (arreglo == "confirmadas") {
                    this.confirmadas.splice(indice,1)
                } else {
                    this.ofertadas.splice(indice,1)
                }

                toastr.success(i18n.t("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.t("mensajesModal.posponerFactura"))
            }).catch((e) => { console.log(e); toastr.error(i18n.t("errorRespuesta")); this.envio = false; })
          
        },
        //
        accionBoton: function () {
            switch (this.mensajeActual) {
                case 0: this.venderAll(); break;
                case 1: this.rechazarAll(); break;
                case 2: this.publicarAll(); break;
                case 3: this.posponerAll(); break;
                case 4: this.posponer(); break;
                case 5: this.publicar(); break;
            }
        },
        //
        venderAll: async function () {
            if (this.envio) return

            this.envio = true

            var ids = []

            this.ofertadas.map(data => {
                ids.push({ invoice_id: data.id })
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

                        for (var w = 0; w < this.ofertadas.length; w++) {
                            if (this.ofertadas[w].id == resp[i].id) {
                                this.ofertadas.splice(w, 1)
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
        rechazarAll: async function () {
            if (this.envio) return

            this.envio = true
            var rechazada = []

            this.ofertadas.map(data => {
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

                        if (data == null || data.error != null ) {
                            estado = 1
                            return
                        }

                        for (var i = 0; i < this.ofertadas.length; i++) {

                            if (this.ofertadas[i].id == data.id) {
                                this.ofertadas.splice(i,1)
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
        //
        publicarAll: async function () {
            if (this.envio) return

            this.envio = true

            var invoices = []

            this.confirmadas.map(data => {
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
                       
                        for (var i = 0; i < this.confirmadas.length; i++) {
                           if (this.confirmadas[i].id == data.id) {
                                this.confirmadas.splice(i, 1)
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
        posponerAll: async function () {
            if (this.envio) return
            
            var invoices = []

            this.confirmadas.map(data => {
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

                        for (var i = 0; i < this.confirmadas.length; i++) {
                            if (this.confirmadas[i].id == data.id) {
                                this.confirmadas.splice(i, 1)
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
    created: function () {
        this.filter = JSON.parse(document.getElementById('contenidoFilter').value);
        this.filter2 = JSON.parse(document.getElementById('contenidoFilter2').value);

        document.getElementById("appPublicarFacturas").removeChild(document.getElementById('contenidoFilter'))
        document.getElementById("appPublicarFacturas").removeChild(document.getElementById('contenidoFilter2'))

        document.getElementById("appPublicarFacturas").removeAttribute("hidden")
        this.lang = document.getElementsByTagName("html")[0].getAttribute("lang")
        this.cargando = false;
    },
    mounted: async function () {
        tiempoLogin(this.modalLogout)
        await this.cargarOfertadas()
        await this.cargarConfirmadas()
    }
});
