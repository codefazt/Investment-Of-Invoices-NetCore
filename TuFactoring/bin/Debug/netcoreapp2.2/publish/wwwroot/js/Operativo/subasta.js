new Vue({
    el: "#app",
    i18n,
    vuetify: new Vuetify({
        lang: {
            t: (key, ...params) => i18n.t(key, params)
        }
    }),
    data: {
        modalLogout: { mostrar: false },
        country:0,
        connection: {},
        loading: false,
        opcion: -1,
        dialog2: false,
        header: [
            { text: i18n.t("headers.fechaCreacion"), value: "date", align: "center" },
            { text: i18n.t("headers.apertura"), value: "opening", align: "center"},
            { text: i18n.t("headers.cierre"), value: "closing", align: "center"},
            { text: i18n.t("headers.pago"), value: "payments", align: "center"},
            { text: i18n.t("headers.finalizacion"), value: "ending", align: "center"},
        ],
        tamanoTlf: tamanoTlf,
        backEndDateFormat: backEndDateFormat,
        backEndDateFormat2: backEndDateFormat2,
        menu: false,
        Mensajeria: {
            notificaciones: true,
            mensajes: false
        },
        dialog: false,
        cargando: true,
        hints: true,
        fav: true,
        message: false,
        cerrarMordisco: true,
        lang: "es",
        currentFalso: [{opening: null, ending:null,closing:null,conciliation:null,payments:null,date:null}],
        current: '',
        envio: false,
    },
    created: function () {
        document.getElementById("app").removeAttribute("hidden")

        this.cargando = false;
    },
    mounted: async function () {
        tiempoLogin(this.modalLogout)
        await this.obtenerCurrent()
    },
    methods: {
        async obtenerCurrent() {
            this.loading = true

            await axios.post('?handler=Current', null,
                {
                    headers: {
                        "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                    }
                })
                .then((respond) => {
                    resetTime()
                    this.loading = false

                    if (respond.data == null) respond.data = respond

                    if (respond.data.error != null && respond.data.error == notAuthorized) {
                        window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                        return
                    }
                    
                    this.current = respond.data

                    this.currentFalso[0] = this.current
                    this.country = this.current.country
                }).catch((respond) => { console.log(respond); this.loading = false});
            
        },

        async createAuction() {
            if (this.envio) return

            this.envio = true

            await axios.post('?handler=create', {},
                {
                    headers: {
                        "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                    }
                })
                .then((respond) => {
                    resetTime()
                    if (respond.data == null) respond.data = respond

                    if (typeof respond.data === 'string' || respond.data instanceof String) {

                        if (respond.data.includes("<!DOCTYPE html>")) {
                            window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired";
                            toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br>" + i18n.t("errorBaseDatos"));
                            return;
                        }
                    }

                    if (respond.data.error != null && respond.data.error == notAuthorized) {
                        window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                        return
                    }

                    if (respond.data.error != null) {
                        toastr.warning(i18n.t("problemasRespuesta"))
                        
                        this.envio = false
                        return
                    }

                    this.current = respond.data
                    
                    toastr.success(i18n.t("exitoRespuesta"))
                    this.envio = false
                }).catch((respond) => { console.log(respond); toastr.warning(i18n.t("errorRespuesta")); this.envio = false });
        },

        async openAuction() {

            if (this.envio) return

            this.envio = true

            await axios.post('?handler=open', { id : this.current.id},
                {
                    headers: {
                        "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                    }
                })
                .then((respond) => {
                    resetTime()
                    if (respond.data == null) respond.data = respond

                    if (typeof respond.data === 'string' || respond.data instanceof String) {

                        if (respond.data.includes("<!DOCTYPE html>")) {
                            window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired";
                            toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br>" + i18n.t("errorBaseDatos"));
                            return;
                        }
                    }

                    if (respond.data.error != null && respond.data.error == notAuthorized) {
                        window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                        return
                    }

                    if (respond.data.error != null) {
                        toastr.warning(i18n.t("problemasRespuesta"))
                        this.envio = false
                        return
                    }

                    this.current.state = respond.data.state
                    this.current.opened = respond.data.opened
                    
                    toastr.success(i18n.t("exitoRespuesta"))
                    this.envio = false
                }).catch((respond) => { console.log(respond); toastr.warning(i18n.t("errorRespuesta")); this.envio = false });
        },

        async closeAuction() {
            if (this.envio) return

            this.envio = true

            await axios.post('?handler=close', { id : this.current.id},
                {
                    headers: {
                        "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                    }
                })
                .then((respond) => {
                    resetTime()
                    if (respond.data == null) respond.data = respond

                    if (typeof respond.data === 'string' || respond.data instanceof String) {

                        if (respond.data.includes("<!DOCTYPE html>")) {
                            window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired";
                            toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br>" + i18n.t("errorBaseDatos"));
                            return;
                        }
                    }

                    if (respond.data.error != null && respond.data.error == notAuthorized) {
                        window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                        return
                    }

                    if (respond.data.error != null) {
                        toastr.warning(i18n.t("problemasRespuesta"))
                        this.envio = false
                        return
                    }

                    this.current.state = respond.data.state
                    this.current.closed = respond.data.closed

                    toastr.success(i18n.t("exitoRespuesta"))
                    this.envio = false

                    this.connection = new signalR.HubConnectionBuilder().withUrl("/wsSubastas").build()

                    this.connection.start().then(this.closeGroup).catch(function (err) {
                        return console.error(err.toString());
                    });
                    
                }).catch((respond) => { console.log(respond); toastr.error(i18n.t("errorRespuesta")); this.envio = false });
        },

        async endingAuction() {
            if (this.envio) return

            this.envio = true

            await axios.post('?handler=ending', {id: this.current.id},
                {
                    headers: {
                        "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                    }
                })
                .then((respond) => {
                    resetTime()
                    if (respond.data == null) respond.data = respond

                    if (typeof respond.data === 'string' || respond.data instanceof String) {

                        if (respond.data.includes("<!DOCTYPE html>")) {
                            window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired";
                            toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br>" + i18n.t("errorBaseDatos"));
                            return;
                        }
                    }

                    if (respond.data.error != null && respond.data.error == notAuthorized) {
                        window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                        return
                    }

                    if (respond.data.error != null) {
                        console.log(respond.data.error)
                        toastr.warning(i18n.t("problemasRespuesta"))
                         
                        this.envio = false
                        return
                    }

                    this.current.state = respond.data.state
                    this.current.finalized = respond.data.finalized

                    toastr.success(i18n.t("exitoRespuesta"))
                     
                    this.envio = false
                }).catch((respond) => { console.log(respond); toastr.warning(i18n.t("errorRespuesta"));   this.envio = false });
        },
        /*
        async conciliation() {
            if (this.envio) return

            this.envio = true

            await axios.post('?handler=conciliation', {},
                {
                    headers: {
                        "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                    }
                })
                .then((respond) => {
                    if (respond.data == null) respond.data = respond
                    respond.data = JSON.parse(respond.data)
                    if (respond.data.error != null) {
                        toastr.warning(i18n.t("problemasRespuesta"))
                         
                        this.envio = false
                        return
                    }

                    this.current.status = respond.data.status
                    this.current.conciliation = respond.data.conciliation

                    toastr.success(i18n.t("exitoRespuesta"))
                     
                    this.envio = false
                }).catch((respond) => { console.log(respond); toastr.warning(i18n.t("errorRespuesta"));   this.envio = false });
        },
        */
        async payments() {
            if (this.envio) return

            this.envio = true

            await axios.post('?handler=payments', {id : this.current.id},
                {
                    headers: {
                        "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                    }
                })
                .then((respond) => {
                    resetTime()
                    if (respond.data == null) respond.data = respond

                    respond.data = JSON.parse(respond.data)

                    if (typeof respond.data === 'string' || respond.data instanceof String) {

                        if (respond.data.includes("<!DOCTYPE html>")) {
                            window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired";
                            toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br>" + i18n.t("errorBaseDatos"));
                            return;
                        }
                    }

                    if (respond.data.error != null && respond.data.error == notAuthorized) {
                        window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                        return
                    }

                    if (respond.data.error != null) {
                        toastr.warning(i18n.t("problemasRespuesta"))
                        
                        this.envio = false
                        return
                    }

                    this.current.state = respond.data.state
                    this.current.payed = respond.data.payed

                    toastr.success(i18n.t("exitoRespuesta"))
                     
                    this.envio = false
                }).catch((respond) => { console.log(respond); toastr.warning(i18n.t("errorRespuesta"));  this.envio = false });
        },

        botonSubasta() {
            var state = document.getElementById("state" + this.current.state)

            if (state == null) return document.getElementById("statefinalized").innerHTML

            return state.innerHTML;
        },

        accionBoton() {
            switch (this.current.state) {
                case "created": this.openAuction(); break;
                case "opened": this.closeAuction(); break;
                case "closed": this.payments(); break;
                case "payed": this.endingAuction(); break;
                default: this.createAuction();
            }
        },

        accionBoton2() {
            switch (this.opcion) {
                case 0: this.openAuction(); break;
                case 1: this.closeAuction(); break;
                case 2: this.payments(); break;
                case 4: this.endingAuction(); break;
                default: this.createAuction();
            }
        },

        closeGroup() {
            this.connection.invoke("Cierre", "" + this.country).catch(error => {
                this.connection.stop()
            })
        }

    },
})