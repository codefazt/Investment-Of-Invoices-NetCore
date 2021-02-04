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
            { text: i18n.t("headers.nombre"), value: "name", align: "center" },
            { text: i18n.t("headers.abreviacion"), value: "abbreviation", align: "center" },
            { text: i18n.t("headers.discriminador"), value: "participant", align: "center" },
            { text: i18n.t("headers.opciones"), value: "opciones", align: "center" },
        ],
        message: false,
        cargando: true,
        enviando: false,
        discriminators:[],
        roles: [],
        nuevo: {
            id: 0,
            name: '',
            abbreviation: '',
            participant: '',
            status: 'active'
        },
        indice: -1,
        dialogBloq: false,
        backEndDateFormat: backEndDateFormat 
    },
    created: function () {
        this.cargando = true
        
        document.getElementById("app").removeAttribute("hidden")

        this.cargando = false
    },
    mounted: async function () {
        tiempoLogin(this.modalLogout)
        await this.obtenerDiscriminators()
        await this.obtenerRoles()
    },
    methods: {
        //
        async obtenerRoles() {
            this.loading = true
            await axios.post('?handler=roles', {},
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

                    if (respuesta.length > 0 && respuesta[0].error == notAuthorized) {
                        window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                        return
                    }

                    this.roles = respuesta
                    this.ordenarRoles();
                }).catch((respond) => { console.error(respond); this.loading = false})

        },
        //
        async obtenerDiscriminators() {
            await axios.post('?handler=discriminator', {},
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
                    
                    this.discriminators = respuesta
                    this.ordenarDiscriminators();
                }).catch((respond) => { console.error(respond); })

        },
        //
        ordenarRoles() {
            this.roles.sort((a, b) => a.name.toLowerCase() < b.name.toLowerCase() ? -1 : +(a.name.toLowerCase() > b.name.toLowerCase()))
        },
        //
        ordenarDiscriminators() {
            this.discriminators.sort((a, b) => a.toLowerCase() < b.toLowerCase() ? -1 : +(a.toLowerCase() > b.toLowerCase()))
        },
        //
        limpiar() {
            this.nuevo = {
                id: 0,
                name : '',
                abbreviation: '',
                participant : this.discriminators.length > 0 ? this.discriminators[0] : '',
                status: 'active'
            }
            this.indice = -1
        },
        //
        validarRol(rol) {
            if (rol.name == '' || rol.abbreviation == '' || rol.participant == '' ||
                rol.name.length > 255 || rol.abbreviation.length > 255 || rol.participant.length > 255     
                ) {
                return 1
            }
            
        },
        //
        procesoRol() {

            nuevo = {
                id: this.nuevo.id,
                name: this.nuevo.name,
                abbreviation: this.nuevo.abbreviation,
                participant: this.discriminatorBD(this.nuevo.participant),
                status: this.nuevo.status
            }

            if (this.validarRol(nuevo)) {
                toastr.warning(i18n.t("invalidRole"))
                return
            }

            if (this.indice == -1) {
                this.crearRol(nuevo)
            } else {
                this.updateRol(nuevo)
            }

            this.ordenarRoles()
        },
        //
        discriminatorBD(palabra) {
            switch (palabra) {
                case "EMPRESA": return "DEBTOR";
                case "PROVEEDOR": return "SUPPLIER"; 
                case "BANCO": return "CONFIRMANT"; 
                case "INVERSIONISTA": return "FACTOR";
                case "BACKOFFICE": return "BACKOFFICE";
                default: return palabra
            }
        },
        //
        async crearRol(nuevo) {
            if(this.enviando) return

            this.enviando = true

            await axios.post("?handler=crear", nuevo , {
                headers: {
                    "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                }
            }).then(resp => {
                resetTime()
                this.enviando = false
                var r = ""
                if (resp.data == null && resp == null ) {
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

                if (r != null && r.error == notAuthorized) {
                    window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                    return
                }

                if (r.error != null) {
                    toastr.warning(i18n.t("problemasRespuesta"))
                    return
                }


                nuevo.id = r.id
                nuevo.status = 'active'
                this.roles.push(nuevo)
                this.edit = false
                this.limpiar()
                toastr.success(i18n.t("exitoRespuesta"))
                }).catch(err => { console.log(err); toastr.error(i18n.t("errorRespuesta")); this.enviando =false })
        },
        //
        async updateRol(nuevo) {

            if (this.enviando) return

            this.enviando = true
            nuevo.id = this.roles[this.indice].id
            await axios.post("?handler=actualizar", nuevo, {
                headers: {
                    "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                }
            }).then(resp => {
                resetTime()
                this.enviando = false
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

                if (r != null && r.error == notAuthorized) {
                    window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                    return
                }

                if (r.error != null) {
                    toastr.warning(i18n.t("problemasRespuesta"))
                    return
                }
                
                this.roles[this.indice].name = nuevo.name
                this.roles[this.indice].participant = nuevo.participant
                    
                this.edit = false
                this.limpiar()
                toastr.success(i18n.t("exitoRespuesta"))
                }).catch(error => { console.log(error); this.enviando = false; toastr.error(i18n.t("errorRespuesta")) })
        },
        //
        async blockRol(index) {
            if (this.enviando) return

            this.enviando = true
            
            await axios.post("?handler=bloquear", this.roles[index], {
                headers: {
                    "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                }
            }).then(resp => {
                resetTime()
                this.enviando = false
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

                if (r != null && r.error == notAuthorized) {
                    window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                    return
                }

                if (r.error != null) {
                    toastr.warning(i18n.t("problemasRespuesta"))
                    return
                }

                if (this.roles[index].status == 'active')
                    this.roles[index].status = 'banned'
                else
                    this.roles[index].status = 'active'

                this.dialogBloq = false
                this.limpiar()
                toastr.success(i18n.t("exitoRespuesta"))
            }).catch(error => { console.log(error); this.enviando = false; toastr.error(i18n.t("errorRespuesta")) })
        },
        //
        editar(indice) {
            this.nuevo.name = this.roles[indice].name
            this.nuevo.abbreviation = this.roles[indice].abbreviation
            this.nuevo.participant = this.roles[indice].participant
            this.indice = indice
        },
    }

})