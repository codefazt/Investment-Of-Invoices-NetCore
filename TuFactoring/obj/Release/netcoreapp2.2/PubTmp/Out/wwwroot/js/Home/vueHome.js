new Vue({
    //store: storeLogin,
    el: "#appHome",
    i18n,
    vuetify: new Vuetify({
        lang: {
            t: (key, ...params) => i18n.t(key, params)
        }
    }),
    data: {
        modalLogout: { mostrar: false },
        ContratoUser: null,
        contratoProveedorModal: null,
        contratoProveedor: false,
        contratoTerminoCondiciones: false,
        cargado: true,

        Const: true,
        agreementsTerminos: null,
        agreementsContratoMarco: null,
        nombreBancoDeTurno: "",
        alMenosUnContratoAceptado: false

    },
    methods: {
        revisionContratos(data) {

            this.ContratoUser = data;
            let ContratosAceptados = 0;
            let Contratos = 0;

            if (this.ContratoUser != null && this.ContratoUser.participant != "BACKOFFICE") {
                for (let i = 0; i < this.ContratoUser.person.agreements.length; i++) {

                    console.log(this.ContratoUser.person.agreements)

                    if ((this.ContratoUser.person.agreements[i].abbreviation == "TERMS") &&
                        (this.ContratoUser.person.agreements[i].acceptedAt == null) &&
                        (this.ContratoUser.participant == "DEBTOR" || this.ContratoUser.participant == "CONFIRMANT")) {

                        this.contratoTerminoCondiciones = true;
                        this.agreementsTerminos = this.ContratoUser.person.agreements[i];
                    }

                    if ((this.ContratoUser.person.agreements[i].abbreviation == "MEMBERSHIP") &&
                        (this.ContratoUser.person.agreements[i].acceptedAt == null) &&
                        (this.ContratoUser.participant == "SUPPLIER")) {

                        this.contratoProveedor = true;
                        this.nombreBancoDeTurno = this.ContratoUser.person.name;
                        this.agreementsContratoMarco = this.ContratoUser.person.agreements[i];

                    }

                    if (this.ContratoUser.person.agreements[i].abbreviation == "MEMBERSHIP" &&
                        this.ContratoUser.person.agreements[i].participant == "SUPPLIER") Contratos++;

                    if (this.ContratoUser.person.agreements[i].accepted == true &&
                        this.ContratoUser.person.agreements[i].abbreviation == "MEMBERSHIP" &&
                        this.ContratoUser.person.agreements[i].participant == "SUPPLIER") {

                        ContratosAceptados++;
                    }
                }
            }

            console.log(Contratos)
            console.log(ContratosAceptados)
            if (this.ContratoUser.state == 'no_const' && ContratosAceptados < Contratos) {

                console.log(this.ContratoUser)
                this.Const = false;
                this.contratoProveedor = false;
                toastr.warning(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br />" + i18n.t("tooltip.contratoMarcoPendiente"));
            }
        },
        async contratoAceptado() {

            if ((this.ContratoUser.participant == "DEBTOR" || this.ContratoUser.participant == "CONFIRMANT")) {

                this.agreementsTerminos.accepted = true;
            }

            var data = await axios.post('', this.agreementsTerminos, { headers: { "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val() } })
                .then((respond) => {
                    resetTime()
                    console.log(respond);
                    toastr.success(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br />" + i18n.t("tooltip.contratoTerminos"));
                    
                    return respond;

                }).catch((respond) => {
                    console.log(respond);
                    toastr.error("Error", i18n.t("mensajesModal.estimadoUsuario") + "<br /><br />" + "Problemas de Conexion se recomienda recargar la pag.");
                });

            if (data.data != null) {

                this.contratoProveedor = false;
                this.contratoTerminoCondiciones = false;

            } else toastr.error("Error", "Usuario Inexistente.");

        },
        async contratoMarcoAceptado() {

            if ((this.ContratoUser.participant == "SUPPLIER")) {

                this.agreementsContratoMarco.accepted = true;
            }

            var data = await axios.post('', this.agreementsContratoMarco, { headers: { "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val() } })
                .then((respond) => {
                    resetTime()
                    console.log(respond);
                    toastr.success(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br />" + i18n.t("tooltip.contratoMarco"));
                    setTimeout(function () { window.location.href = "/Supplier/index"; }, 2500);
                    return respond;

                }).catch((respond) => {
                    console.log(respond);
                    toastr.error("Error", i18n.t("mensajesModal.estimadoUsuario") + "<br /><br />" + "Problemas de Conexion se recomienda recargar la pag.");
                });

            if (data.data != null) {

                this.contratoProveedor = false;
                this.contratoTerminoCondiciones = false;
                this.revisionContratos(data.data);

            } else toastr.error("Error", "Usuario Inexistente.");

        }
        
  },
    computed: {

  },
    created: function () {

        this.ContratoUser = JSON.parse(document.getElementById('contenidoRaw').value);
        //console.log(this.ContratoUser);

        this.revisionContratos(this.ContratoUser);
        document.getElementById('contenido').removeAttribute('hidden');
        this.cargado = false;
        tiempoLogin(this.modalLogout)
    }
});