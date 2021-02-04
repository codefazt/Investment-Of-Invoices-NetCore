new Vue({
    el: "#appUsers",
    i18n,
    vuetify: new Vuetify({
        lang: {
            t: (key, ...params) => i18n.t(key, params)
        }
    }),
    data: {
        modalLogout: { mostrar: false },
        widthTelefono: widthTelefono,
        loading: false,
        tamanoTlf: tamanoTlf,
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
        user: [],
        roles: [],
        indice: -1,
        backEndDateFormat: backEndDateFormat,
        REOneLetter: /[A-Za-zÁ-ý]{1,}/,
        REOnlyZero: /^[0]+$/,
        RETwoPoint: /[.]{2,}/,
        hasError: 'is-invalid',
        hasSuccess: 'is-valid',
        errorName: '',
        errorNameText: '',
        errorEmail: '',
        errorEmailText: ''
    },
    created: function () {
        this.user = JSON.parse(document.getElementById('contenidoRaw').value);
        console.log("Usuario")
        console.log(this.user)
        document.getElementById("contenido").removeAttribute("hidden");
        this.cargando = false;
        this.fillUser();
    },
    mounted: async function () {
        tiempoLogin(this.modalLogout)
    },
    methods: {
        VerificarLetrasSeguidas(campo) {
            let RE = /[a-zA-ZÁ-ý\u00C0-\u017F\|\°\¬\\\!\"\#\$\%\&\/\(\)\=\?\'\¡\¿\´\¨\+\*\~\[\{\^\]\}\`\,\.\-\_\:\;]/
            let repeticiones = 0
            let letra = ''

            for (var i = 0; i < campo.length; i++) {

                if (letra == campo[i] && RE.test(letra)) {
                    repeticiones++;

                    if (repeticiones == 2) {
                        return true
                    }
                } else {
                    repeticiones = 0
                    letra = campo[i]
                }

            }
            return false
        },
        fillUser() {

            if (this.user != null) {
                if (this.user.name != null && this.user.name != '') this.errorName = this.hasSuccess;
                if (this.user.email != null && this.user.email != '') this.errorEmail = this.hasSuccess;
            }
        },
        validarNombres() {

            let RE = /^([[A-Za-zÁ-Ýá-ýñÑ\´]{2,}[\s]{1,1}[[A-Za-zÁ-Ýá-ýñÑ\´]{2,}[[A-Za-zÁ-Ýá-ýñÑ\s\.\´]{0,})+$/i
            let emailRegex = /^[[A-Za-zÁ-Ýá-ýñÑ\s\´\.]+$/i;

            if (this.user.name == '' ||
                this.user.name == null ||
                this.user.name[0] == ' ') {

                this.errorName = this.hasError;
                this.errorNameText = i18n.t("valid.nombreRequerido");

            } else if (this.user.name.length < 2) {

                this.errorName = this.hasError;
                this.errorNameText = i18n.t("valid.minimo2Char");

            } else if (this.user.name.length > 255) {

                this.errorName = this.hasError;
                this.errorNameText = i18n.t("valid.maxChar");

            } else if (!emailRegex.test(this.user.name)) {

                this.errorName = this.hasError;
                this.errorNameText = i18n.t("valid.charNoPermitidosNombres");

            } else if (!RE.test(this.user.name)) {

                this.errorName = this.hasError;
                this.errorNameText = i18n.t("valid.NombreInversionistaNatural");

            } else if (this.VerificarLetrasSeguidas(this.user.name)) {

                this.errorName = this.hasError;
                this.errorNameText = i18n.t("valid.max2CharRepetidos");

            } else {
                this.errorName = this.hasSuccess;
                this.errorNameText = '';

            }
        },
        validarEmail() {

            let RE = /[.]{2,}/
            emailRegex = /^[\w.]{1,64}@(?:[A-Z0-9]{2,63}\.){1,125}[A-Z]{2,63}$/i;

            if (this.user.email == '' ||
                this.user.email == null) {

                this.errorEmail = this.hasError;
                this.errorEmailText = i18n.t("valid.emailRequerido");

            } else if (!emailRegex.test(this.user.email) || RE.test(this.user.email)) {

                this.errorEmail = this.hasError;
                this.errorEmailText = i18n.t("valid.emailFormatoInvalido");

            } else if (this.user.email.length < 10) {
                this.errorEmail = this.hasError;
                this.errorEmailText = i18n.t("valid.emailMaxChar");

            } else if (this.user.email.length > 60) {

                this.errorEmail = this.hasError;
                this.errorEmailText = i18n.t("valid.emailMaxChar");

            } else {
                this.errorEmail = this.hasSuccess;
                this.errorEmailText = '';

            }
        },
        async actualizarUser() {

            await axios.post('', this.user, { headers: { "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val() } })
                .then((respond) => {

                    console.log(respond)
                    resetTime()
                    let respuesta = 0;
                    if (respond.data == null && respond == null) {
                        return
                    }

                    if (typeof respond.data === 'string' || respond.data instanceof String) {

                        if (data.data.includes("<!DOCTYPE html>")) {
                            window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired";
                            toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br />" + i18n.t("errorBaseDatos"));
                            return;
                        }
                    }

                    if (respond.data.error == null) {
                        this.user = respond.data
                        toastr.success("<div class='text-justify'>" + i18n.t("tooltip.queridoUsuario") + "<br /><br />" + i18n.t("tooltip.actualizarPerfil") + "</div>");
                    } else {
                        if (respond.data.error == "Cannot perform runtime binding on a null reference") {
                            toastr.error("Se han presentado inconvenientes al actualizar la información, No se puede realizar el enlace de tiempo de ejecución en una referencia nula");
                        }
                        else if (respond.data.error == "duplicatedEmail") {
                            toastr.warning(i18n.t(respond.data.error));
                        }
                        else {
                            toastr.error("Se han presentado inconvenientes al actualizar la información, " + respond.data.error);
                        }                 
                        respuesta = respond
                    }

                    if (respuesta.length > 0) {
                        if (respuesta[0].error == notAuthorized) window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                        return
                    }

                }).catch((respond) => {
                    console.log(respond);
                    toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br />" + i18n.t("errorBaseDatos"));
                })
        },
        
    },
    computed: {

        habilitarBoton() {

            if (this.errorName == this.hasSuccess &&
                this.errorEmail == this.hasSuccess &&
                this.user.roles != null) {

                this.user.roles.map(data => {

                })
                return false;

            } else {

                return true;
            }

        },
    }

})