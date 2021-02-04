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
        i18n:i18n,
        indice: -1,
        loading: false,
        tamanoTlf: tamanoTlf,
        edit: false,
        header: [
            { text: i18n.t("headers.n"), value: "n", align: "center" },
            { text: i18n.t("headers.abbreviation"), value: "abbreviation", align: "center" },
            { text: i18n.t("headers.description"), value: "description", align: "center" },
            { text: i18n.t("headers.content"), value: "content", align: "center" },
            { text: i18n.t("headers.typeContent"), value: "type_content", align: "center" },
            { text: i18n.t("headers.opciones"), value: "options", align: "center" },
        ],
        menu: false,
        cargando: true,
        enviando: false,
        types_content: [],
        settings: [],
        nuevo: {
            id: 0,
            type_content: '',
            abbreviation: '',
            content: '',
            description: ''
        },
        indice: -1,
        dialogBloq: false
    },
    created: function () {
        this.cargando = true

        document.getElementById("app").removeAttribute("hidden")

        this.cargando = false
    },
    mounted: async function () {
        tiempoLogin(this.modalLogout)
        await this.obtenerTypesContent()
        await this.obtenerSettings()
    },
    methods: {
        //
        async obtenerSettings() {
            this.loading = true
            await axios.post('?handler=settings', {},
                {
                    headers: {
                        "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                    }
                })
                .then((respond) => {
                    resetTime()
                    this.loading = false
                    var respuesta = []
                    if (respond.data == null && respond == null) {
                        return
                    }

                    if (respond.data != null) {
                        respuesta = respond.data
                    } else {
                        respuesta = respond
                    }

                   

                    if (respuesta.length > 0 && respuesta[0].errors == notAuthorized) {
                        window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                        return
                    }

                    this.settings = respuesta
                    this.ordenarSettings();
                }).catch((respond) => { console.error(respond); this.loading = false })

        },
        //
        async obtenerTypesContent() {
            await axios.post('?handler=typesContent', {},
                {
                    headers: {
                        "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                    }
                })
                .then((respond) => {
                    resetTime()
                    var respuesta = []
                    if (respond.data == null && respond == null) {
                        return
                    }

                    if (respond.data != null) {
                        respuesta = respond.data
                    } else {
                        respuesta = respond
                    }

                    if (respuesta.length > 0 && respuesta[0].errors == notAuthorized) {
                        window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                        return
                    }

                    this.types_content = respuesta
                    this.ordenarTypesContent();
                }).catch((respond) => { console.error(respond); })

        },
        //
        ordenarSettings() {
            this.settings.sort((a, b) => a.abbreviation.toLowerCase() < b.abbreviation.toLowerCase() ? -1 : +(a.abbreviation.toLowerCase() > b.abbreviation.toLowerCase()))
        },
        //
        ordenarTypesContent() {
            this.types_content.sort((a, b) => a.toLowerCase() < b.toLowerCase() ? -1 : +(a.toLowerCase() > b.toLowerCase()))
        },
        //
        limpiar() {
            this.nuevo = {
                id: 0,
                type_content : this.types_content.length != 0 ? this.types_content[0] : "",
                abbreviation : '',
                content : '',
                description : ''
            }
            this.indice = -1
        },
        //
        validarSetting(setting) {
            if (setting.content == '' || setting.abbreviation == '' || 
                setting.content.length > 255 || setting.abbreviation.length > 255 || (setting.description != null && setting.description.length > 255)
            ) {
                return 1
            }

        },
        //
        procesoSetting() {
            
            if (this.validarSetting(this.nuevo)) {
                toastr.warning(i18n.t("invalidSetting"))
                return
            }

            if (this.indice == -1) {
                this.crearSetting(this.nuevo)
            } else {
                this.updateSetting(this.nuevo)
            }

            this.ordenarSettings()
        },
        //
        async crearSetting(nuevo) {
            if (this.enviando) return

            this.enviando = true

            await axios.post("?handler=crear", nuevo, {
                headers: {
                    "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                }
            }).then(resp => {
                resetTime()
                var r = ""
                if (resp.data == null && resp == null) {
                    toastr.warning(i18n.t("problemasRespuesta"))
                    this.enviando = false
                    return
                }

                if (resp.data != null) r = resp.data
                else r = resp

                if (typeof r === 'string' || r instanceof String) {

                    if (r.includes("<!DOCTYPE html>")) {
                        window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired";
                        toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br>" + i18n.t("errorBaseDatos"));
                        return;
                    }
                }

                if (r != null && r.errors == notAuthorized) {
                    window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                    return
                }

                if (r.error != null) {
                    toastr.warning(i18n.t("problemasRespuesta"))
                    this.enviando = false
                    return
                }


                this.enviando = false
                nuevo.id = r.id
                this.settings.push(nuevo)
                this.limpiar()
                toastr.success(i18n.t("exitoRespuesta"))
            }).catch(err => { console.log(err); toastr.error(i18n.t("errorRespuesta")); this.enviando = false })
        },
        //
        async updateSetting(nuevo) {
            if (this.enviando) return

            this.enviando = true
            nuevo.id = this.settings[this.indice].id
            await axios.post("?handler=actualizar", nuevo, {
                headers: {
                    "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                }
            }).then(resp => {
                resetTime()
                var r = ""
                if (resp.data == null && resp == null) {
                    toastr.warning(i18n.t("problemasRespuesta"))
                    return
                }

                if (resp.data != null) r = resp.data
                else r = resp

                if (typeof r === 'string' || r instanceof String) {

                    if (r.includes("<!DOCTYPE html>")) {
                        window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired";
                        toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br>" + i18n.t("errorBaseDatos"));
                        return;
                    }
                }

                if (r != null && r.errors == notAuthorized) {
                    window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                    return
                }

                if (r.error != null) {
                    toastr.warning(i18n.t("problemasRespuesta"))
                    return
                }

                this.settings[this.indice].content = nuevo.content
                this.settings[this.indice].abbreviation = nuevo.abbreviation 
                this.settings[this.indice].type_content = nuevo.type_content
                this.settings[this.indice].description = nuevo.description
                this.settings.push({})
                this.settings.pop()
                this.enviando = false
                this.edit = false
                this.limpiar()
                toastr.success(i18n.t("exitoRespuesta"))
            }).catch(error => { console.log(error); this.enviando = false; toastr.error(i18n.t("errorRespuesta")) })
        },
        editar(indice) {
            this.nuevo.content = this.settings[indice].content
            this.nuevo.abbreviation = this.settings[indice].abbreviation
            this.nuevo.type_content = this.settings[indice].type_content
            this.nuevo.description = this.settings[indice].description
            this.indice = indice
        },
    }

})