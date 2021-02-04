new Vue({
    el: "#appGestionBancos",
    i18n,
    vuetify: new Vuetify({
        lang: {
            t: (key, ...params) => i18n.t(key, params)
        }
    }),
    data: {
        modalLogout: { mostrar: false },
        country: 0,
        connection: {},
        loading: false,
        actualizarPerfil: false,
        opcion: -1,
        modalCargandoDetalle: false,
        modalNewLegalRepresentant:false,
        dialog2: false,
        hasError: 'is-invalid',
        hasSuccess: 'is-valid',
        newLegal: {
            id:'',
            label: 'LEGAL',
            identification: 0,
            prefix: 0,
            name: '',
            phoneNumber: '',
            email: '',
        },
        nuevo: {
            label: 'LEGAL',
            identification: 0,
            prefix: 0,
            name: '',
            phoneNumber: '',
            email: '',
        },
        header: [
            { text: i18n.t("headers.n"), value: "n", align: "center" },
            { text: i18n.t("headers.bankName"), value: "person.name", align: "center" },
            { text: i18n.t("headers.routingNumber"), value: "routing_number", align: "center" },
            { text: i18n.t("headers.representanteLegal"), value: 'legal', align: 'center' },
            { text: i18n.t("headers.alliedBank"), value: 'related', align: 'center' },
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
        representanteLegal: {
            idPerson: '',
            prefix: '',
            documentNumber: '',
            abbreviation:'',
            email: '',
            identification: '',
            name: '',
            phone: '',
        },
        dialogUpdateLegal: false,
        cargando: true,
        entities: [],
        catalogo: [],
        REOneLetter: /[A-Za-zÁ-ý]{1,}/,
        REOnlyZero: /^[0]+$/,
        RETwoPoint: /[.]{2,}/,
        validDocument: false,
        validName: false,
        validPhone: false,
        validEmail:false,
        //---------------------- Datos Representante Legal -------------------------------------

        //Validaciones DocumentoRepresentanteLegal
        errorDocumentoRepresentanteLegal: '',
        errorTextoDocumentoRepresentanteLegal: '',

        //Validaciones Nombres del Representante Legal
        errorNombresRepresentanteLegal: '',
        errorTextoNombresRepresentanteLegal: '',

        //Validaciones Apellidos del Representante Legal
        errorApellidosRepresentanteLegal: '',
        errorTextoApellidosRepresentanteLegal: '',

        //Validaciones Telefono del Representante Legal
        errorTelefonoRepresentante: '',
        errorTextoTelefonoRepresentante: '',

        //Validaciones Email del Representante Legal
        errorEmailRepresentante: '',
        errorTextoEmailRepresentante: '',

        maxLengthPhone: 0,
        placeholderPhone:"",
        numberRegexp:"",
        hints: true,
        fav: true,
        message: false,
        cerrarMordisco: true,
        lang: "es",
        currentFalso: [{ opening: null, ending: null, closing: null, conciliation: null, payments: null, date: null }],
        current: '',
        envio: false,
    },
    created: function () {
        document.getElementById("appGestionBancos").removeAttribute("hidden")

        this.cargando = false;
    },
    mounted: async function () {
        tiempoLogin(this.modalLogout)
        await this.getCountryEntities()
        await this.llenarCatalogo()
    },
    methods: {
        async getCountryEntities() {
            this.loading = true

            await axios.post('?handler=Entities', null,
                {
                    headers: {
                        "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                    }
                })
                .then((respond) => {
                    resetTime()
                    this.loading = false

                    console.log(respond)

                    if (respond.data == null) respond.data = respond

                    if (respond.data.error != null && respond.data.error == notAuthorized) {
                        window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                        return
                    }

                    this.entities = respond.data.entities

                    this.entities.sort((a, b) => a.routing_number.toLowerCase() < b.routing_number.toLowerCase() ?
                        -1 : +a.routing_number.toLowerCase() > b.routing_number.toLowerCase())

                }).catch((respond) => { console.log(respond); this.loading = false });
        },

        async llenarCatalogo() {
            await axios.post('?handler=catalogo', {},
                {
                    headers: {
                        "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                    }
                })
                .then((respond) => {
                    resetTime()
                    if (respond.data == null) {
                        this.catalogo = []
                        return
                    }

                    if (respond.data.length > 0 && respond.data[0].errors == notAuthorized) {
                        window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                        return
                    }


                    this.catalogo = JSON.parse(respond.data)

                    this.catalogo.settings.map((datos) => {
                        if (datos.abbreviation == "MAXLEN_PHONE") {
                            this.maxLengthPhone = datos.content
                        }

                        if (datos.abbreviation == "REGEXP_PHONE") {
                            this.numberRegexp = datos.content
                        }

                        if (datos.abbreviation == 'REGEXP_PHONE') {
                            this.placeholderPhone = datos.mask_edit
                        }

                    })

                }).catch((respond) => { console.log(respond); });
        },

        vaciarCampos() {
            this.actualizarPerfil = false
            this.representanteLegal.identification = ""
            this.representanteLegal.email = ""
            this.representanteLegal.prefix = ""
            this.representanteLegal.idPerson= ""
            this.representanteLegal.phone = ""
            this.representanteLegal.name = ""
            this.representanteLegal.documentNumber = ""
            this.representanteLegal.abbreviation = ""

            //Nuevos Campos

            this.nuevo.name = ""
            this.nuevo.email = ""
            this.nuevo.idPerson = ""
            this.nuevo.phone = ""
            this.nuevo.prefix = ""
            this.nuevo.documentNumber = ""
            this.nuevo.abbreviation = ""
            this.nuevo.identification = ""                    
        },

        async getDataFromEntity(item) {
            this.vaciarCampos()
            this.representanteLegal.idPerson = item.person.id
            this.nuevo.idPerson = item.person.id
            this.modalCargandoDetalle = true
            await axios.post('?handler=GetDataFromEntiy', item,
                {
                    headers: {
                        "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                    }
                })
                .then((respond) => {
                    resetTime()
                    if (respond.data.contacts == null) {
                        this.actualizarPerfil = false
                        this.representanteLegal.identification = ""
                        this.representanteLegal.email = ""
                        this.representanteLegal.prefix = ""
                        this.representanteLegal.phone = ""
                        this.representanteLegal.name = ""
                        this.representanteLegal.documentNumber = ""
                        this.representanteLegal.abbreviation = ""
                        this.modalCargandoDetalle = false
                        toastr.warning(i18n.tc("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.tc("mensajesModal.errorLegalRepresentantNotExist"))
                        return
                    }
                    var contacts = respond.data.contacts
                    if (this.catalogo != null) {
                        var identificaciones = this.catalogo.identifications
                    }
                    let arrayIdentifications = []

                    if (contacts != null) {
                        for (let y = 0; y < contacts.length; y++) {
                            if (contacts[y].label == "LEGAL") {
                                this.representanteLegal.identification = contacts[y].identification
                                this.representanteLegal.email = contacts[y].email
                                this.representanteLegal.prefix = contacts[y].prefix
                                this.representanteLegal.phone = contacts[y].phoneNumber
                                this.representanteLegal.name = contacts[y].name
                                this.representanteLegal.documentNumber = contacts[y].documentNumber
                            }
                        }
                    }
                    


                    identificaciones.map(data => {
                        if (data.prefix == true) {
                            arrayIdentifications.push(data)
                        }
                    })
                    
                    for (let x = 0; x < identificaciones.length; x++) { 

                        if (identificaciones[x].prefix == false && identificaciones[x].discriminator == "LEGAL") {
                            this.modalCargandoDetalle = false
                            this.dialogUpdateLegal = true 
                            this.representanteLegal.abbreviation = identificaciones[x].abbreviation
                            return
                        }

                        for (let y = 0; y < arrayIdentifications[y].prefixes.length; y++) {
                            if (identificaciones[x].id == this.representanteLegal.identification && this.representanteLegal.prefix == arrayIdentifications[x].prefixes[y].id ) {
                                this.representanteLegal.abbreviation = arrayIdentifications[x].prefixes[y].abbreviation
                                this.modalCargandoDetalle = false
                                this.dialogUpdateLegal = true 
                                return
                            }
                        }
                    }

                        
                    if (respond.data.length > 0 && respond.data[0].errors == notAuthorized) {
                        window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                        return
                    }


                }).catch((respond) => { console.log(respond); });
        },

        validarDocumentoRepresentanteLegal(ExpreRegular, mask_edit) {

            var regExp = new RegExp(ExpreRegular);
            if (this.nuevo.prefix == '' ||
                this.nuevo.prefix == 0) {

                this.errorDocumentoRepresentanteLegal = this.hasError;
                this.errorTextoDocumentoRepresentanteLegal = i18n.t("valid.tipoDoc");

            } else if (this.nuevo.documentNumber == '' ||
                this.nuevo.documentNumber == null) {

                this.errorDocumentoRepresentanteLegal = this.hasError;
                this.errorTextoDocumentoRepresentanteLegal = i18n.t("valid.docRequerido");


            } else if (!regExp.test(this.nuevo.documentNumber)) {
                this.errorDocumentoRepresentanteLegal = this.hasError;
                this.errorTextoDocumentoRepresentanteLegal = i18n.t("valid.formatDoc") + " (" + mask_edit + ")";

            } else if (this.REOnlyZero.test(this.nuevo.documentNumber)) {
                this.errorDocumentoRepresentanteLegal = this.hasError;
                this.errorTextoDocumentoRepresentanteLegal = i18n.t("valid.docCero");
            } else {
                this.validDocument = true
                this.errorDocumentoRepresentanteLegal = this.hasSuccess;
                this.errorTextoDocumentoRepresentanteLegal = '';

            }
        },

        validarNombresRepresentanteLegal() {

            let RE = /^([[A-Za-zÁ-Ýá-ýñÑ\´]{2,}[\s]{1,1}[[A-Za-zÁ-Ýá-ýñÑ\´]{1,}[[A-Za-zÁ-Ýá-ýñÑ\s\.\´]{0,})+$/i
            let emailRegex = /^[[A-Za-zÁ-Ýá-ýñÑ\s\´\.]+$/i;

            if (this.nuevo.name == '' ||
                this.nuevo.name == null ||
                this.nuevo.name[0] == ' ') {

                this.errorNombresRepresentanteLegal = this.hasError;
                this.errorTextoNombresRepresentanteLegal = i18n.t("valid.nombreRequerido");

            } else if (this.nuevo.name.length < 2) {

                this.errorNombresRepresentanteLegal = this.hasError;
                this.errorTextoNombresRepresentanteLegal = i18n.t("valid.minimo2Char");

            } else if (this.nuevo.name.length > 255) {

                this.errorNombresRepresentanteLegal = this.hasError;
                this.errorTextoNombresRepresentanteLegal = i18n.t("valid.maxChar");

            } else if (!emailRegex.test(this.nuevo.name)) {

                this.errorNombresRepresentanteLegal = this.hasError;
                this.errorTextoNombresRepresentanteLegal = i18n.t("valid.charNoPermitidosNombres");

            } else if (!RE.test(this.nuevo.name)) {

                this.errorNombresRepresentanteLegal = this.hasError;
                this.errorTextoNombresRepresentanteLegal = i18n.t("valid.NombreInversionistaNatural");

            } else if (this.VerificarLetrasSeguidas(this.nuevo.name)) {

                this.errorNombresRepresentanteLegal = this.hasError;
                this.errorTextoNombresRepresentanteLegal = i18n.t("valid.max2CharRepetidos");

            } else {
                this.validName = true
                this.errorNombresRepresentanteLegal = this.hasSuccess;
                this.errorTextoNombresRepresentanteLegal = '';

            }
        },
        validarTelefonoRepresentanteLegal() {

            let emailRegex = new RegExp(this.numberRegexp);

            if (this.nuevo.phone == '' || this.nuevo.phone  == null) {
                this.errorTelefonoRepresentante = this.hasError;
                this.errorTextoTelefonoRepresentante = i18n.t("valid.telefonoRequerido");


            } else if (this.nuevo.phone.length < 4 ||
                this.nuevo.phone.length == 1) {

                this.errorTelefonoRepresentante = this.hasError;
                this.errorTextoTelefonoRepresentante = i18n.t("valid.telefonoFormatInvalid") + " " + this.placeholderPhone;


            } else if (this.nuevo.phone.length > 50) {

                this.errorTelefonoRepresentante = this.hasError;
                this.errorTextoTelefonoRepresentante = i18n.t("valid.telefonoFormatInvalid") + " " + this.placeholderPhone;


            } else if (this.REOnlyZero.test(this.nuevo.phone)) {

                this.errorTelefonoRepresentante = this.hasError;
                this.errorTextoTelefonoRepresentante = i18n.t("valid.docCero");

            } else if (!emailRegex.test(this.nuevo.phone)) {

                this.errorTelefonoRepresentante = this.hasError;
                this.errorTextoTelefonoRepresentante = i18n.t("valid.telefonoFormatInvalid") + " " + this.placeholderPhone;

            } else {
                this.validPhone = true
                this.errorTelefonoRepresentante = this.hasSuccess;
                this.errorTextoTelefonoRepresentante = '';

            }
        },
        validarEmailRepresentanteLegal() {

            let RE = /[.]{2,}/
            emailRegex = /^[\w.]{1,64}@(?:[A-Z0-9]{2,63}\.){1,125}[A-Z]{2,63}$/i;

            if (this.nuevo.email == '' ||
                this.nuevo.email == null) {

                this.errorEmailRepresentante = this.hasError;
                this.errorTextoEmailRepresentante = i18n.t("valid.emailRequerido");

            } else if (!emailRegex.test(this.nuevo.email) || RE.test(this.nuevo.email)) {

                this.errorEmailRepresentante = this.hasError;
                this.errorTextoEmailRepresentante = i18n.t("valid.emailFormatoInvalido");

            } else if (this.nuevo.email.length < 10) {
                this.errorEmailRepresentante = this.hasError;
                this.errorTextoEmailRepresentante = i18n.t("valid.emailMaxChar");

            } else if (this.nuevo.email.length > 60) {

                this.errorEmailRepresentante = this.hasError;
                this.errorTextoEmailRepresentante = i18n.t("valid.emailMaxChar");

            } else {
                this.validEmail = true
                this.errorEmailRepresentante = this.hasSuccess;
                this.errorTextoEmailRepresentante = '';

            }
        },

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

        ordenarArreglo: async function () {

            this.newLegal.id = this.nuevo.idPerson
            this.newLegal.label = "LEGAL"
            this.newLegal.identification = this.nuevo.identification
            this.newLegal.prefix = this.nuevo.prefix
            this.newLegal.documentNumber = this.nuevo.documentNumber
            this.newLegal.name = this.nuevo.name
            this.newLegal.phoneNumber = this.nuevo.phone
            this.newLegal.email = this.nuevo.email

            await this.updateLegal(this.newLegal);
        },

        async updateLegal(newLegal) {

            await axios.post('?handler=UpdateLegalRepresentant', newLegal,
                {
                    headers: {
                        "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                    }
                })
                .then((respond) => {
                    resetTime()

                    if (respond.data != null && respond.data.error == notAuthorized) {
                        window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                        return
                    }

                    if (respond.data != null) {
                        toastr.success(i18n.tc("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.tc("mensajesModal.legalRepresentantUpdated"))
                        this.modalNewLegalRepresentant = false
                        return
                    }
                    
                    toastr.warning(i18n.tc("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.tc("mensajesModal.errorAlliedBank"))

                }).catch((respond) => { console.log(respond); this.loading = false });
        },

        async makeEntityAllied(item) {

            await axios.post('?handler=MakeAllied', { id: item.id, related: item.related },
                {
                    headers: {
                        "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                    }
                })
                .then((respond) => {
                    resetTime()

                    if (respond.data != null && respond.data.errors == notAuthorized) {
                        window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                        return
                    } else if (respond.data == null && respond.data.errors != null) {
                        toastr.warning(i18n.tc("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.tc("mensajesModal.errorAlliedBank"))
                        return
                    } else if (respond.data != null && respond.data.related == false) {
                        for (let x = 0; x < this.entities.length; x++) {
                            if (this.entities[x].id == item.id) {
                                item.related = false
                            }
                        }
                        toastr.success(i18n.tc("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.tc("mensajesModal.removeAlliedBank"))
                    }
                        else {
                        for (let x = 0; x < this.entities.length; x++) {
                            if (this.entities[x].id == item.id) {
                                item.related = true
                            }
                        }
                        toastr.success(i18n.tc("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.tc("mensajesModal.newAlliedBank"))
                    }

                }).catch((respond) => { console.log(respond); this.loading = false });
        },

    },
})