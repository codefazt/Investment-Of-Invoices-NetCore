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
        widthTelefono: widthTelefono,
        loading : false,
        tamanoTlf: tamanoTlf,
        header: [
            { text: i18n.t("headers.n"), value: "n", align: "center" },
            { text: i18n.t("headers.nombre"), value: "name", align: "center" },
            { text: i18n.t("headers.correoElectronico"), value: "email", align: "center" },
            { text: i18n.t("headers.creadoEn"), value: "created_at", align: "center" },
            { text: i18n.t("headers.opciones"), value: "opciones", align: "center" },
        ],
        bloquear: false,
        invitar:false,
        menu: false,
        Mensajeria: {
            notificaciones: true,
            mensajes: true
        },
        cerrarMordisco: true,
        hints: true,
        fav: true,
        message: false,
        cargando: true,
        enviando: false,
        personal: [],
        roles: [],
        nuevo: {
            id: '',
            roles: [],
            roles_id: [],
            name: '',
            foto: '',
            email: '',
            created_at:'',
            state: 'active'
        },
        indice: -1,
        backEndDateFormat:backEndDateFormat 
    },
    created: function () {
        document.getElementById("app").removeAttribute("hidden")
    },
    mounted: async function () {
        tiempoLogin(this.modalLogout)
        await this.obtenerUsuarios()
        await this.obtenerRoles()
    },
    methods: {
        async obtenerUsuarios() {
            this.loading = true
           await axios.post('?handler=users', {},
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

                    this.personal = respuesta
                    this.ordenarPersonal()
                    this.obtenerRolesID(this.personal)
                }).catch((respond) => { console.error(respond); this.loading = false })
            
        },
        //
        async obtenerRoles() {
            await axios.post('?handler=roles', {},
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

                    if (respuesta.length > 0 && respuesta[0].error == notAuthorized) {
                        window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                        return
                    }

                    this.roles = respuesta

                    this.ordenarRoles()
                    
                }).catch((respond) => { console.error(respond); })

        },
        //
        ordenarPersonal() {
            try {
                this.personal.sort((a, b) => a.name.toLowerCase() < b.name.toLowerCase() ? -1 : +(a.name.toLowerCase() > b.name.toLowerCase()))

            } catch (e) {}
        },
        //
        ordenarRoles() {
            this.roles.sort((a, b) => a.name.toLowerCase() < b.name.toLowerCase() ? -1 : +(a.name.toLowerCase() > b.name.toLowerCase()))
        },
        //
        limpiar() {
            this.nuevo.id = ''
            this.nuevo.name = ""
            this.nuevo.foto = ""
            this.nuevo.roles = []
            this.nuevo.roles_id = []
            this.nuevo.state = 'active'
            this.nuevo.email = ""
            this.nuevo.created_at = ""
            this.indice = -1
        },
        //
        validarUser(user) {
            if (user.name == null || user.email == null || user.name == "" || user.email == "" || user.roles == null || user.roles.length == 0 ||
                user.name.length > 255  || user.email.length > 255      
                ) {
                return 1
            }
            
            if (this.personal != null) {
                for (var i = 0; i < this.personal.length; i++) {
                    if (this.personal[i].email == user.email && this.personal[i].id != user.id) {
                        return 2
                    }
                }
            }
            
        },
        //
        procesoUsuario() {
            this.nuevo.roles = []
            this.nuevo.roles_id.map(data => {
                var role = this.roles.find(element => element.id == data)

                if(role != null) 
                    this.nuevo.roles.push(role)
                
            })

            nuevo = {
                id: '',
                created_at: this.nuevo.created_at,
                roles: this.nuevo.roles,
                name: this.nuevo.name,
                foto: this.nuevo.foto,
                email: this.nuevo.email,
                roles_id: this.nuevo.roles_id,
                state: this.nuevo.state
            }

            if (this.indice != -1) {
                nuevo.id = this.personal[this.indice].id
            }

            var valid = this.validarUser(nuevo)

            if (valid == 1) {
                toastr.warning(i18n.t("invalidUser"))
                return
            } else if (valid == 2) {
                toastr.warning(i18n.t("duplicatedEmail"))
                return
            }

            this.invitar = false

            if (this.indice == -1) {
                this.crearUsuario(nuevo)
            } else {
                this.updateUsuario(nuevo)
            }

            this.ordenarPersonal()
        },
        //
        async crearUsuario(nuevo) {

            if (this.enviando) return

            this.enviando = true

            await axios.post("?handler=crear", {
                name: nuevo.name,
                email: nuevo.email,
                roles: nuevo.roles,
            }, {
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

                if (r.error != null) {
                    if (r.error == notAuthorized) {
                        window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                        return
                    } else if (r.error == "invalidUser" || r.error == "duplicatedEmail") {
                        toastr.warning(i18n.t(r.error))
                    } else {
                        toastr.warning(i18n.t("problemasRespuesta"))
                    }
                    this.enviando = false
                    return
                }


                this.enviando = false
                nuevo.id = r.id
                nuevo.createdAt = r.createdAt
                nuevo.state= "invite"
                this.personal.push(nuevo)
                this.limpiar()
                toastr.success(i18n.t("exitoRespuesta"))
            }).catch(err => { console.log(err); toastr.error(i18n.t("errorRespuesta")); this.enviando =false })
        },
        //
        async updateUsuario(nuevo) {

            if (this.enviando) return

            this.enviando = true
            nuevo.id = this.personal[this.indice].id
            await axios.post("?handler=actualizar", {
                    name: nuevo.name,
                    email: nuevo.email,
                    roles: nuevo.roles,
                    roles_id : nuevo.roles_id,
                    id: nuevo.id
                }, {
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

                if (r.error != null && r.error != "") {
                    if (r.error == notAuthorized) {
                        window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                        return
                    } else if (r.error == "invalidUser" || r.error == "duplicatedEmail") {
                        toastr.warning(i18n.t(r.error))
                    } else {
                        toastr.warning(i18n.t("problemasRespuesta"))
                    }
                    this.enviando = false
                    return
                }

                for (let i = 0; i < this.personal.length; i++) {
                    if (this.personal[i].id == r.id) {
                        this.personal[i].name = r.name
                        this.personal[i].foto = r.foto
                        this.personal[i].email = r.email
                        this.personal[i].roles_id = r.roles_id

                        for (var w = 0; w < this.personal[i].roles_id.length; w++) {
                            this.personal[i].roles_id[w] = parseInt(this.personal[i].roles_id[w])
                        }
                    }
                }
                this.enviando = false
                toastr.success(i18n.t("exitoRespuesta"))
                }).catch(error => { console.log(error); this.enviando = false; toastr.error(i18n.t("errorRespuesta")) })
        },
        //
        async blockPersonal(indice) { 

            if (this.enviando) return

            this.enviando = true
            await axios.post("?handler=bloquear", { id: this.personal[indice].id, state : this.personal[indice].state }, {
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

                if (r.error != null) {
                    if (r.error == notAuthorized) {
                        window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                        return
                    }
                    toastr.warning(i18n.t("problemasRespuesta"))
                    return
                }

                this.personal[indice].state = this.personal[indice].state == 'active' ? 'banned':'active'
                this.personal.push({})

                this.personal.pop();
                this.enviando = false
                toastr.success(i18n.t("exitoRespuesta"))
                }).catch(error => { console.log(error); toastr.error(i18n.t("errorRespuesta")); this.enviando = false })
        },
        //
        editar(indice) {
            this.nuevo.name = this.personal[indice].name
            this.nuevo.foto = this.personal[indice].foto
            this.nuevo.roles = this.personal[indice].roles
            this.nuevo.email = this.personal[indice].email
            this.created_at = this.personal[indice].created_at
            this.nuevo.roles_id = this.personal[indice].roles_id
            this.indice = indice
        },
        //
        obtenerRolesID(usuarios) {
   
            for (let i = 0; i < usuarios.length; i++) {
                usuarios[i].roles_id = []
                
                if (usuarios[i].roles == null) continue


                for (let t = 0; t < usuarios[i].roles.length; t++) {
                    usuarios[i].roles_id.push(usuarios[i].roles[t].id)
                }
            }
            
        }
    }

})