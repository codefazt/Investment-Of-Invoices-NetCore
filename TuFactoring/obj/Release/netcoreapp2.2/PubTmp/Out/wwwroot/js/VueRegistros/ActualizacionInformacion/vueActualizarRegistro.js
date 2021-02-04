var app = new Vue({
    el: '#appRegistroEmpresas',
    i18n,
    store: store,
    vuetify: new Vuetify({
        lang: {
            t: (key, ...params) => i18n.t(key, params)
        }
    }),
    data: {
        modalLogout: { mostrar: false },
        maxlegthTelefono: null,
        maxlegthCuentaBancaria: null,
        BancoSelected: "",
        placeholderCuentaBancaria: null,
        placeholderTelefono: null,
        backEndDateFormat: backEndDateFormat,
        dialogInversionista: false,
        contratoTerminoCondiciones: false,
        contratoProveedor: false,
        blockAsociadoRegistrado: false,
        boleanoDesicionRegistro: true,
        listCities: [],
        ExprAccount: null,
        ExprPhone: null,
        resetearRegistro: null,
        agreementsContratoMarco: null,

        dialogBorrarAsociado: false,
        dialogBorrarCuenta: false,
        borrarCuentaSelect: null,
        pickedCheck: false,
        page: 1,
        blockAsociado: false,
        cargando: true,
        tamanoTlf: tamanoTlf,
        fav: true,
        menu: false,
        message: false,
        hints: true,
        Mensajeria: {
            notificaciones: true,
            mensajes: true,
        },
        Operativo: true,
        TitularAsociado: '',
        cerrarMordisco: true,
        botonSiguiente: 0,
        drawer: false,
        dialog: false,
        dialogCuentas: false,
        dialogAsociado: false,
        headersBanco: [
            {
                text: i18n.t("headers.entidadBancaria"), align: 'center', value: 'entity',
            },
            { text: i18n.t("headers.tipoCuenta"), value: 'account', align: 'center' },
            { text: i18n.t("headers.moneda"), value: 'currency', align: 'center' },
            { text: i18n.t("headers.numeroCuenta"), value: 'accountNumber', align: 'center' },
            { text: i18n.t("headers.cuentaPrincipal"), value: 'default', align: 'center' },
            { text: i18n.t("headers.acciones"), value: 'action', align: 'center' },
        ],
        headersAsociado: [

            { text: i18n.t("headers.cliente"), value: 'company' },
            { text: i18n.t("headers.numeroDoc"), value: 'number', align: 'center' },
            { text: i18n.t("headers.contacto"), value: 'name' },
            { text: i18n.t("headers.correoElectronico"), value: 'email' },

            { text: i18n.t("headers.estatus"), value: 'estado', align: 'center' },
            //{ text: i18n.t("headers.asociados"), value: 'invitado', align: 'center' },
            { text: i18n.t("headers.acciones"), value: 'action', align: 'center' },

        ],
        headersContrato: [

            { text: i18n.t("headers.doct"), value: 'abbreviation' },
            { text: i18n.t("headers.bank"), value: 'entity' },
            { text: i18n.t("headers.aceptado"), value: 'accepted', align: 'center' },
            { text: i18n.t("headers.fechaAceptacion"), value: 'fechaaceptada', align: 'center' },
            { text: i18n.t("headers.acciones"), value: 'action', align: 'center' },

        ],
        cargado: true,
        eleccion: 'EMPRESA',
        tipoDoc: 'LEGAL',
        principalProveedorRepresentante: false,
        principalProveedorContacto: false,
        updatePrincipalProveedor: false,
        updatePrincipalProveedorNmr: '',
        updatePrincipalProveedorPrefix: '',
        boleanoDesicionDireccion2: true,
        personaContactoVacio: true,
        validateReset: false,
        tamanioDiv: "col-md-8",
        REOneLetter: /[A-Za-zÁ-ý]{1,}/,
        REOnlyZero: /^[0]+$/,
        RETwoPoint: /[.]{2,}/,
        dateMin: moment().subtract(100, 'year').format('YYYY-01-01'),
        dateMax: moment().subtract(18, 'year').format('YYYY-MM-DD'),
        errorMessageDireccionShort: 'Direccion debe tener al menos 15 caracteres',
        errorMessageInvalidField: "Campo invalido",
        errorMessageEmailCaracters: "Correo electronico debe contener entre 10 - 60 caracteres",
        errorMessageMoreTwoLetters: "No se permiten más de dos letras iguales seguidas",

        comprobarDoc: {
            document: { identification: -1, prefix: null, number: '' }
        },
        comprobarDocA: {
            document: { identification: -1, prefix: null, number: '' }
        },
        nombreBanco: '',
        x: null,
        picked: 'Gestion de Cuenta',
        registro: [], //registro para mandar al Grapql
        dataPaises: [], // datos iniciales de cada pais
        inicializarCuentas: false, // indicar si la cuenta existe
        selectCuentas: 999, //Cuenta Seleccionada
        selectAsociado: 999, //Cliente o Proveedor Seleccionada
        idCiudad: 0,
        hasError: 'is-invalid',
        hasSuccess: 'is-valid',

        //---------------------- Datos Comerciales -------------------------------------
        //Validaciones DocumentoPrincipal
        errorDocumento: '',
        errorTextoDocumentoPrincipal: '',

        //Validaciones NombreLegal
        errorNombreLegal: '',
        errorTextoNombreLegal: '',

        //Validaciones Nombre Comercial
        errorNombreComercial: '',
        errorTextoNombreComercial: '',

        //Validaciones Purpuse - Actividad Comercial
        errorPurpose: '',
        errorTextoPurpose: '',

        //Validaciones Ocupacion - Ocupacion
        errorOccupation: '',
        errorOccupationTexto: '',

        //Validaciones Fecha de Nacimiento
        errorFechaNacimiento: '',
        errorFechaNacimientoTexto: '',

        //Validaciones Segundo Nombre 
        errorOtroNombre: '',
        errorOtroNombreTexto: '',

        //Validaciones email Persona Natural
        errorEmailNatural: '',
        errorEmailNaturalTexto: '',

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

        //---------------------- Datos Administrador -------------------------------------

        errorDocumentoAdministrador: '',
        errorDocumentoAdministradorTexto: '',

        //Validaciones Nombre Administrador
        errorNombreAdministrador: '',
        errorNombreAdministradorTexto: '',

        //Validaciones Apellido Administrador
        errorApellidoAdministrador: '',
        errorApellidoAdministradorTexto: '',

        //Validaciones Telefono Administrador
        errorTelefonoAdministrador: '',
        errorTelefonoAdministradorTexto: '',

        //Validaciones Email del Administrador
        errorEmailAdministrador: '',
        errorEmailAdministradorTexto: '',

        //---------------------- Datos Persona de Contacto -------------------------------------

        //Validaciones Documento Cliente Asociado
        errorDocumentoAsociadoPrincipal: '',
        errorDocumentoAsociadoTextoPrincipal: '',

        //Validaciones NombreLegal Cliente Asociado
        errorNombreLegalAsociadoPrincipal: '',
        errorNombreLegalAsociadoTextoPrincipal: '',

        //Validaciones Documento Cliente Asociado Persona de Contacto
        errorDocumentoAsociadoContactoPrincipal: '',
        errorDocumentoAsociadoContactoTextoPrincipal: '',

        //Validaciones Nombre Cliente Asociado Persona de Contacto
        errorNombreContactoPrincipal: '',
        errorNombreContactoTextoPrincipal: '',

        //Validaciones Apellido Cliente Asociado Persona de Contacto
        errorApellidoContactoPrincipal: '',
        errorApellidoContactoTextoPrincipal: '',

        //Validaciones Telefono Cliente Asociado Persona de Contacto
        errorTelefonoContactoPrincipal: '',
        errorTelefonoContactoTextoPrincipal: '',

        //Validaciones Email del Cliente Asociado Persona de Contacto
        errorEmailContactoPrincipal: '',
        errorEmailContactoTextoPrincipal: '',

        //---------------------- Datos Direcciones -------------------------------------

        //Validaciones Direcciones
        errorDireccion: '',
        errorTextoDireccion: '',

        //Validaciones Direccion 2
        errorDireccion2: '',
        errorDireccion2Texto: '',

        //Validaciones Estado
        errorEstado: '',
        errorEstadoTexto: '',

        //Validaciones Direcciones Ciudad
        errorCiudad: '',
        errorCiudadTexto: '',

        //Validaciones TelefonoDirecciones
        errorTelefonoDirecciones: '',
        errorTextoTelefonoDirecciones: '',

        //---------------------- Datos Banco Imagen y numero de Ruteo -------------------------------------

        //validar Routin_number
        errorRoutinNumber: '',
        errorRoutinNumberTexto: '',


        //---------------------- Datos CuentasBancarias -------------------------------------

        //Validaciones idBanco

        idBanco: '',
        idBancoTexto: '',

        //Validaciones tipo de Moneda
        tipoMonedaBanco: '',
        tipoMonedaBancoTexto: '',

        //Validaciones tipoCuentaBanco
        tipoCuentaBanco: '',
        tipoCuentaBancoTexto: '',

        //Validar TitularBanco
        TituluarBanco: '',
        TituluarBancoTexto: '',

        //Validar numeroCuenta
        numeroCuentaBanco: '',
        numeroCuentaBancoTexto: '',

        errorCuentasBancariasGestionCuenta: '',

        //---------------------- Datos Clientes Asociados -------------------------------------

        //Validaciones Documento Cliente Asociado
        errorDocumentoAsociado: '',
        errorDocumentoAsociadoTexto: '',

        //Validaciones NombreLegal Cliente Asociado
        errorNombreLegalAsociado: '',
        errorNombreLegalAsociadoTexto: '',

        //Validaciones Documento Cliente Asociado Persona de Contacto
        errorDocumentoAsociadoContacto: '',
        errorDocumentoAsociadoContactoTexto: '',

        //Validaciones Nombre Cliente Asociado Persona de Contacto
        errorNombreContacto: '',
        errorNombreContactoTexto: '',

        //Validaciones Apellido Cliente Asociado Persona de Contacto
        errorApellidoContacto: '',
        errorApellidoContactoTexto: '',

        //Validaciones Telefono Cliente Asociado Persona de Contacto
        errorTelefonoContacto: '',
        errorTelefonoContactoTexto: '',

        //Validaciones Email del Cliente Asociado Persona de Contacto
        errorEmailContacto: '',
        errorEmailContactoTexto: '',

        //Validaciones Documento Cliente Asociado Representante Legal
        errorDocumentoAsociadoRepresentante: '',
        errorDocumentoAsociadoRepresentanteTexto: '',

        //Validaciones Nombre Cliente Asociado Representante Legal
        errorNombreRepresentante: '',
        errorNombreRepresentanteTexto: '',

        //Validaciones Apellido Cliente Asociado Representante Legal
        errorApellidoRepresentante: '',
        errorApellidoRepresentanteTexto: '',

        //Validaciones Telefono Cliente Asociado Representante Legal
        errorTelefonoAsociadoRepresentante: '',
        errorTelefonoAsociadoRepresentanteTexto: '',

        //Validaciones Email del Representante Legal
        errorEmailAsociadoRepresentante: '',
        errorEmailAsociadoRepresentanteTexto: '',

    },
    created: function () {

        this.registro = JSON.parse(document.getElementById('contenidoRaw').value);
        //this.registro.registrarse = this.registro.registrarseA;

        if (this.registro != null) {

            if (this.registro.registrarse != null) {

                if (this.registro.registrarse.document == null) toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br/><br/> No cuenta con ningun datos Comerciales imposibilitando su Actualización por favor comuniquese con el Operativo de TuFactoring para mayor Información");
                if (this.registro.contacto != null) this.registro.contacto.prefix = '';
            }
        }
        
        console.log(this.registro)
        this.Inicio();

        if (this.registro.representante != null && this.registro.user.discriminator == "LEGAL") {

            if (this.registro.representante.documentNumber == null || this.registro.representante.documentNumber == "" ||
                this.registro.representante.name == null || this.registro.representante.name == "" ||
                this.registro.representante.email == null || this.registro.representante.email == "" ||
                this.registro.representante.phoneNumber == null || this.registro.representante.phoneNumber == "" ) {

                toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br/><br/> Usted no cuenta con los datos requeridos del Representante Legal. <br/>Por favor comuníquese con el Operativo de TuF@ctoring, para actualizar su información.");
            }
        }
        document.getElementById('contenido').removeAttribute('hidden');
        this.cargando = false;
    },
    methods: {
        Inicio() {

            if (this.registro.perfil.contacts != null) {
                for (let i = 0; i < this.registro.perfil.contacts.length; i++) {

                    if (this.registro.perfil.contacts[i].label == "LEGAL") this.registro.representante = this.registro.perfil.contacts[i];
                    if (this.registro.perfil.contacts[i].label == "CONTACT") this.registro.contacto = this.registro.perfil.contacts[i];
                    if (this.registro.perfil.contacts[i].label == "ADMINISTRATOR") this.registro.administrador = this.registro.perfil.contacts[i];
                }
            }

            if (this.registro.user.participant == "SUPPLIER") this.TitularAsociado = 'Clientes';
            else if (this.registro.user.participant == "DEBTOR") this.TitularAsociado = 'Proveedores';

            if (this.registro.dataPaises != null) {

                if (this.registro.cities != null) {
                    if (this.registro.cities.regions != null) {
                        if (this.registro.cities.regions[0] != null) this.listCities = this.registro.cities.regions[0].cities;
                    }
                }

                if (this.listCities != null) this.listCities.sort((a, b) => a.name < b.name ? -1 : + (a.name > b.name));

                this.dataPaises = this.registro.dataPaises;
                if (this.dataPaises.categories != null) this.dataPaises.categories.sort((a, b) => a.name < b.name ? -1 : +(a.name > b.name))
                if (this.dataPaises.regions != null) this.dataPaises.regions.sort((a, b) => a.name < b.name ? -1 : +(a.name > b.name))
                if (this.dataPaises.entities != null) {
                    for (let i = 0; i < this.dataPaises.entities.length; i++) {
                        this.dataPaises.entities[i].person.name = this.dataPaises.entities[i].person.name.toUpperCase();
                    }
                    this.dataPaises.entities.sort((a, b) => a.person.name < b.person.name ? -1 : +(a.person.name > b.person.name));
                }
                if (this.dataPaises.currencies != null) this.dataPaises.currencies.sort((a, b) => a.name < b.name ? -1 : +(a.name > b.name))

            }

            if (this.dataPaises.settings != null) {

                for (let i = 0; i < this.dataPaises.settings.length; i++) {
                    if (this.dataPaises.settings[i].abbreviation == 'REGEXP_ACCOUNT_NUMBER') {
                        this.ExprAccount = this.dataPaises.settings[i].content;
                        this.placeholderCuentaBancaria = this.dataPaises.settings[i].mask_edit;
                    }
                    if (this.dataPaises.settings[i].abbreviation == 'REGEXP_PHONE') {
                        this.ExprPhone = this.dataPaises.settings[i].content;
                        this.placeholderTelefono = this.dataPaises.settings[i].mask_edit;
                    }
                    if (this.dataPaises.settings[i].abbreviation == 'MAXLEN_PHONE') {
                        this.maxlegthTelefono = this.dataPaises.settings[i].content;
                    }
                    if (this.dataPaises.settings[i].abbreviation == 'MAXLEN_ACCOUNT_NUMBER') {
                        this.maxlegthCuentaBancaria = this.dataPaises.settings[i].content;
                    }
                }
            }

            if (this.registro.registrarse != null) {

                this.registro.registrarse.participant = this.registro.user.participant;
                this.rellenoDatosInicial();
            }

            if (this.registro.registrarse.accounts != null) {

                let cuentaProvicional = JSON.parse(JSON.stringify(this.registro.registrarse.accounts));
                this.registro.registrarse.accounts = [];
                this.registro.nombresBancos = [];

                for (let i = 0; i < cuentaProvicional.length; i++) {
                    if (cuentaProvicional[i].status == true) {
                        this.registro.registrarse.accounts.push(cuentaProvicional[i]);

                        for (let j = 0; j < this.dataPaises.entities.length; j++) {

                            if (this.dataPaises.entities[j].id == cuentaProvicional[i].entity) {
                                this.registro.nombresBancos.push(this.dataPaises.entities[j].person.company);
                            }
                        }
                    }
                    
                }

                if (this.registro.perfil.agreements != null) {

                    let Contrato = JSON.parse(JSON.stringify(this.registro.perfil.agreements));
                    let conteoContrato = 0;
                    this.registro.perfil.agreements = [];
                    for (let i = 0; i < Contrato.length; i++) {

                        if (Contrato[i].abbreviation == "TERMS" && conteoContrato == 0) {
                            this.registro.perfil.agreements.push(Contrato[i]);
                            conteoContrato++;
                        }

                        if (Contrato[i].abbreviation == "MEMBERSHIP" && Contrato[i].participant == "SUPPLIER" && this.registro.user.participant == "SUPPLIER" ) this.registro.perfil.agreements.push(Contrato[i]);
                    }
                }
            }

            if (this.registro.representante != null) {

                if (this.registro.representante.documentNumber == null || this.registro.representante.documentNumber == '') this.errorDocumentoRepresentanteLegal = '';
                if (this.registro.representante.phoneNumber == null || this.registro.representante.phoneNumber == '') this.errorTelefonoRepresentante = '';
            }

            if (this.registro.registrarse.address != null) {

                if (this.registro.registrarse.address.line1 == null || this.registro.registrarse.address.line1 == "") this.errorDireccion = '';

                if (this.registro.registrarse.address.region == null ||
                    this.registro.registrarse.address.region == "" ||
                    this.registro.registrarse.address.region == 0) this.errorEstado = '';

                if (this.registro.registrarse.address.city == null ||
                    this.registro.registrarse.address.city == "" ||
                    this.registro.registrarse.address.city == 0) this.errorCiudad = '';
            }

            if (this.registro.registrarse.phone != null) {
                if (this.registro.registrarse.phone.number == null || this.registro.registrarse.phone.number == '') this.errorTelefonoDirecciones = '';

            } else this.errorTelefonoDirecciones = '';

            if (this.registro.administrador != null) {
                if (this.registro.administrador.documentNumber == null || this.registro.administrador.documentNumber == '') this.errorDocumentoAdministrador = '';
                if (this.registro.administrador.name == null || this.registro.administrador.name == '') this.errorNombreAdministrador = '';
                if (this.registro.administrador.phoneNumber == null || this.registro.administrador.phoneNumber == '') this.errorTelefonoAdministrador = '';
                if (this.registro.administrador.email == null || this.registro.administrador.email == '') this.errorEmailAdministrador = '';
            }

            this.registro.asociadoActual.prefix = '';
            
        },
        limpiarMensajes: function () {
            store.commit("limpiarMensajes")
        },
        rellenoDatosInicial() {

            this.errorDocumentoRepresentanteLegal = this.hasSuccess;
            this.errorTextoDocumentoRepresentanteLegal = '';

            this.errorNombresRepresentanteLegal = this.hasSuccess;
            this.errorTextoNombresRepresentanteLegal = '';

            this.errorApellidosRepresentanteLegal = this.hasSuccess;
            this.errorTextoApellidosRepresentanteLegal = '';

            this.errorTelefonoRepresentante = this.hasSuccess;
            this.errorTextoTelefonoRepresentante = '';

            this.errorEmailRepresentante = this.hasSuccess;
            this.errorTextoEmailRepresentante = '';

            this.errorDocumentoAdministrador = this.hasSuccess;
            this.errorDocumentoAdministradorTexto = '';

            //Validaciones Nombre Administrador
            this.errorNombreAdministrador = this.hasSuccess;
            this.errorNombreAdministradorTexto = '';

            //Validaciones Apellido Administrador
            this.errorApellidoAdministrador = this.hasSuccess;
            this.errorApellidoAdministradorTexto = '';

            //Validaciones Telefono Administrador
            this.errorTelefonoAdministrador = this.hasSuccess;
            this.errorTelefonoAdministradorTexto = '';

            //Validaciones Email del Administrador
            this.errorEmailAdministrador = this.hasSuccess;
            this.errorEmailAdministradorTexto = '';

            if (this.registro.contacto != null) {

                if ((this.registro.contacto.documentNumber != null &&
                    this.registro.contacto.documentNumber != '') &&
                    (this.registro.contacto.email != null &&
                    this.registro.contacto.email != '') &&
                    (this.registro.contacto.phoneNumber != null &&
                    this.registro.contacto.phoneNumber != '') &&
                    (this.registro.contacto.name != null &&
                    this.registro.contacto.name != '')) {

                    //----- Persona de Contacto
                    this.errorDocumentoAsociadoContactoPrincipal = this.hasSuccess;
                    this.errorDocumentoAsociadoContactoTextoPrincipal = '';

                    this.errorNombreContactoPrincipal = this.hasSuccess;
                    this.errorNombreContactoTextoPrincipal = '';

                    this.errorApellidoContactoPrincipal = this.hasSuccess;
                    this.errorTextoApellidoContactoPrincipal = '';

                    this.errorTelefonoContactoPrincipal = this.hasSuccess;
                    this.errorTelefonoContactoTextoPrincipal = '';

                    this.errorEmailContactoPrincipal = this.hasSuccess;
                    this.errorEmailContactoTextoPrincipal = '';
                    //------------------------------
                }

            }

            this.idCiudad = this.registro.registrarse.address.region;
            if (this.registro.registrarse.address.line2 != null && this.registro.registrarse.address.line2 != '') {
                this.errorDireccion2 = this.hasSuccess;

            } else this.errorDireccion2 = '';

            this.errorDireccion = this.hasSuccess;
            this.errorTextoDireccion = '';

            this.errorDireccion2Texto = '';

            this.errorEstado = this.hasSuccess
            this.errorEstadoTexto = ''


            this.errorCiudad = this.hasSuccess;
            this.errorCiudadTexto = '';

            this.errorTelefonoDirecciones = this.hasSuccess;
            this.errorTextoTelefonoDirecciones = '';

            if (this.registro.registrarse.accounts != null) {
                for (let i = 0; i < this.registro.registrarse.accounts.length; i++) {

                    for (let j = 0; j < this.dataPaises.entities.length; j++) {

                        if (this.dataPaises.entities[j].id == this.registro.registrarse.accounts[i].entity && 
                            this.registro.registrarse.accounts[i].status == true) {
                            this.registro.nombresBancos.push(this.dataPaises.entities[j].person.company);
                        }
                    }
                }
            }

            if ((this.registro.registrarse.company != null && this.registro.registrarse.company != '') ||
                (this.registro.registrarse.firstName != null && this.registro.registrarse.firstName != '')) this.TituluarBanco = this.hasSuccess;
        }, // Seguir Trabajando en el rellenar de los datos ---------------------------------

        eleccionRegistro(Rol) {

            this.registro.rol = Rol;
            this.eleccion = 'EMPRESA';
            this.tipoDoc = 'LEGAL';

            if (Rol == 1 || Rol == 5) {

                this.registro.registrarse.tipoRegistro = 'EMPRESA';
                this.registro.registrarse.participant = 'DEBTOR';

                this.comprobarDoc.tipoRegistro = 'EMPRESA';
                this.comprobarDoc.participant = 'DEBTOR';

                this.TitularAsociado = 'Clientes';
            }
            else if (Rol == 2) {

                this.registro.registrarse.tipoRegistro = 'PROVEEDOR';
                this.registro.registrarse.participant = 'SUPPLIER';

                this.comprobarDoc.tipoRegistro = 'PROVEEDOR';
                this.comprobarDoc.participant = 'SUPPLIER';

                this.TitularAsociado = 'Proveedores';
            }
            else if (Rol == 3) {

                this.registro.registrarse.tipoRegistro = 'INVERSIONISTA';
                this.registro.registrarse.participant = 'FACTOR';

                this.comprobarDoc.tipoRegistro = 'EMPRESA';
                this.comprobarDoc.participant = 'FACTOR';
            }
            else if (Rol == 4) {

                this.registro.registrarse.tipoRegistro = 'BANCO';
                this.registro.registrarse.participant = 'BANK';

                this.comprobarDoc.tipoRegistro = 'BANCO';
                this.comprobarDoc.participant = 'BANK';
            }



            if (this.validateReset) {

                this.vaciarCampos();
            } else {
                this.vaciarCamposCambiarRegistro();
            }

        },
        documentoIdentificacion(identificacion) {

            let tituloIdentificacion = '';

            for (var i = 0; i < identificacion.length; i++) {

                if (identificacion[i].discriminator == 'LEGAL') {

                    tituloIdentificacion = identificacion[i].abbreviation;
                }
            }

            return tituloIdentificacion;
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
        verificarFecha() {
            var w = $("#datepicker").val().split("-")

            var dateMax = this.dateMax.split("-")
            var dateMin = this.dateMin.split("-")
            if (w[0] == undefined || w[1] == undefined || w[2] == undefined) {

                this.errorFechaNacimiento = this.hasError
                this.errorFechaNacimientoTexto = 'Formato de fecha de nacimiento inválido';
                return
            } else if (!moment($("#datepicker").val()).isValid()) {
                this.errorFechaNacimiento = ''
                this.errorFechaNacimientoTexto = 'Formato de fecha de nacimiento inválido';
                return
            } else if (w[0] == undefined || w[1] == undefined || w[2] == undefined
                || w[0] < dateMin[0] || w[0] > dateMax[0]
                || (w[0] == dateMax[0] && w[1] > dateMax[1])
                || (w[0] == dateMax[0] && w[1] == dateMax[1] && w[2] > dateMax[2])
                || (w[0] == dateMin[0] && w[1] < dateMin[1])
                || (w[0] == dateMin[0] && w[1] == dateMin[1] && w[2] < dateMin[2])) {
                this.errorFechaNacimiento = this.hasError
                this.errorFechaNacimientoTexto = 'Formato de fecha de nacimiento inválido';
                return
            }


            this.errorFechaNacimiento = this.hasSuccess;
            this.errorFechaNacimientoTexto = ''
        },

        validarDocumentoPrincipal(ExpreRegular) {

            var regExp = new RegExp(ExpreRegular);

            if (this.registro.registrarse.document.prefix === '' ||
                this.registro.registrarse.document.prefix == 0) {

                this.errorDocumento = this.hasError;
                this.errorTextoDocumentoPrincipal = i18n.t("valid.tipoDoc");

            } else if (this.registro.registrarse.document.number == '' ||
                this.registro.registrarse.document.number == null) {

                this.errorDocumento = this.hasError;
                this.errorTextoDocumentoPrincipal = i18n.t("valid.docRequerido");

            } else if (!regExp.test(this.registro.registrarse.document.number)) {
                this.errorDocumento = this.hasError;
                this.errorTextoDocumentoPrincipal = i18n.t("valid.formatDoc");

            } else if (this.REOnlyZero.test(this.registro.registrarse.document.number)) {
                this.errorDocumento = this.hasError;
                this.errorTextoDocumentoPrincipal = i18n.t("valid.docCero");

            } else {
                this.errorDocumento = this.hasSuccess;
                this.errorTextoDocumentoPrincipal = '';
            }

        },
        validarNombreLegal() {

            let emailRegex = null;
            if (this.tipoDoc == 'LEGAL') emailRegex = /[A-Za-zÁ-ý]{1,}/;
            else emailRegex = /^[A-Za-zÁ-Ýá-ý\s\'\.]+$/i;

            if (this.registro.registrarse.company === '' ||
                this.registro.registrarse.company == null ||
                this.registro.registrarse.company[0] == ' ') {

                this.errorNombreLegal = this.hasError;
                this.TituluarBanco = this.hasError;

                if (this.tipoDoc == 'LEGAL') {
                    this.errorTextoNombreLegal = i18n.t("valid.LegalRepRequerido");
                    this.TituluarBancoTexto = i18n.t("valid.LegalRepRequerido");
                }
                else {
                    this.errorTextoNombreLegal = i18n.t("valid.NombreInversionistaNatural");
                    this.TituluarBancoTexto = i18n.t("valid.NombreInversionistaNatural");
                }


            } else if (!emailRegex.test(this.registro.registrarse.company) && this.tipoDoc == 'LEGAL') {

                this.errorNombreLegal = this.hasError;
                this.errorTextoNombreLegal = i18n.t("valid.replicaLetra");

            } else if (!emailRegex.test(this.registro.registrarse.company) && this.tipoDoc == 'PERSON') {

                this.errorNombreLegal = this.hasError;
                this.errorTextoNombreLegal = i18n.t("valid.charNoPermitidosNombres");

            } else if (this.registro.registrarse.company.length < 4 && this.tipoDoc == 'LEGAL') {

                this.errorTextoNombreLegal = i18n.t("valid.minimoChar");
                this.errorNombreLegal = this.hasError;

            } else if (this.registro.registrarse.company.length < 2 && this.tipoDoc == 'PERSON') {

                this.errorTextoNombreLegal = i18n.t("valid.minimo2Char");
                this.errorNombreLegal = this.hasError;

            } else if (this.registro.registrarse.company.length > 255) {

                this.errorNombreLegal = this.hasError;
                this.errorTextoNombreLegal = i18n.t("valid.maxChar");

            } else {
                this.errorNombreLegal = this.hasSuccess;
                this.errorTextoNombreLegal = '';

                this.TituluarBanco = this.hasSuccess;
                this.TituluarBancoTexto = '';
            }
        },
        validarNombresPerson() {

            let RE = /^([[A-Za-zÁ-Ýá-ýñÑ\´]{2,}[\s]{1,1}[[A-Za-zÁ-Ýá-ýñÑ\´]{2,}[[A-Za-zÁ-Ýá-ýñÑ\s\.\´]{0,})+$/i
            let emailRegex = /^[[A-Za-zÁ-Ýá-ýñÑ\s\´\.]+$/i;
   
            if (this.registro.registrarse.firstName == '' ||
                this.registro.registrarse.firstName == null ||
                this.registro.registrarse.firstName[0] == ' ') {

                this.errorNombreLegal = this.hasError;
                this.TituluarBanco = this.hasError;
                this.errorTextoNombreLegal = i18n.t("valid.NombreInversionistaNatural");
                this.TituluarBancoTexto = i18n.t("valid.NombreInversionistaNatural");

            } else if (this.registro.registrarse.firstName.length < 2) {

                this.errorTextoNombreLegal = i18n.t("valid.minimo2Char");
                this.errorNombreLegal = this.hasError;

            } else if (this.registro.registrarse.firstName.length > 255) {

                this.errorNombreLegal = this.hasError;
                this.errorTextoNombreLegal = i18n.t("valid.maxChar");

            } else if (!emailRegex.test(this.registro.registrarse.firstName)) {

                this.errorNombreLegal = this.hasError;
                this.errorTextoNombreLegal = i18n.t("valid.charNoPermitidosNombres");

            } else if (!RE.test(this.registro.registrarse.firstName)) {

                this.errorNombreLegal = this.hasError;
                this.errorTextoNombreLegal = i18n.t("valid.NombreInversionistaNatural");

            } else if (this.VerificarLetrasSeguidas(this.registro.registrarse.firstName)) {

                this.errorNombreLegal = this.hasError;
                this.errorTextoNombreLegal = i18n.t("valid.max2CharRepetidos");

            } else {
                this.errorNombreLegal = this.hasSuccess;
                this.errorTextoNombreLegal = '';
                this.TituluarBanco = this.hasSuccess;
                this.TituluarBancoTexto = '';

            }
        },
        validarPurpuse() {

            if (this.registro.registrarse.category == null || this.registro.registrarse.category == 'SelecioneActividadComercial' || this.registro.registrarse.category == '') {
                this.errorPurpose = this.hasError;
                this.errorTextoPurpose = i18n.t("valid.actComercial");

            } else {
                this.errorPurpose = this.hasSuccess;
                this.errorTextoPurpose = '';
            }
        },
        // opcionales
        CargaConstitutiva(event) {

            let archivo = event.target.files[0];
            if (archivo.type == 'application/pdf') {

                let reader = new FileReader();

                reader.readAsDataURL(archivo);
                reader.onload = evt => {
                    let docPdf = evt.target.result;

                    if (docPdf.match('data:application/pdf;base64,JVBER')) {

                        this.registro.registrarse.file = docPdf;

                    } else {

                        document.getElementById("file").value = "";
                        alert(i18n.t("valid.cargaConst"));
                    }
                }
            } else {
                document.getElementById("file").value = "";
                alert(i18n.t("valid.cargaConst"));
            }
        },
        validarEmailNatural() {

            let RE = /[.]{2,}/
            emailRegex = /^[\w.]{1,64}@(?:[A-Z0-9]{2,63}\.){1,125}[A-Z]{2,63}$/i;

            if (this.registro.registrarse.email.address == '' ||
                this.registro.registrarse.email.address == null) {

                this.errorEmailNatural = this.hasError;
                this.errorEmailNaturalTexto = i18n.t("valid.emailRequerido");

            } else if (!emailRegex.test(this.registro.registrarse.email.address) || RE.test(this.registro.registrarse.email.address)) {

                this.errorEmailNatural = this.hasError;
                this.errorEmailNaturalTexto = i18n.t("valid.emailFormatoInvalido");

            } else if (this.registro.registrarse.email.address.length < 10 || this.registro.registrarse.email.address.length > 60) {

                this.errorEmailNatural = this.hasError;
                this.errorEmailNaturalTexto = i18n.t("valid.emailMaxChar");

            } else {
                this.errorEmailNatural = this.hasSuccess;
                this.errorEmailNaturalTexto = '';

            }
        },
        validarOcupacion() {

            if (this.registro.registrarse.category == 0 || this.registro.registrarse.category == '' || this.registro.registrarse.category == null) {
                this.errorOccupation = this.hasError;
                this.errorOccupationTexto = i18n.t("valid.ocupation");

            } else {
                this.errorOccupation = this.hasSuccess;
                this.errorOccupationTexto = '';
            }
        },

        validarDocumentoRepresentanteLegal(ExpreRegular, mask_edit) {

            var regExp = new RegExp(ExpreRegular);

            if (this.registro.representante.prefix == '' ||
                this.registro.representante.prefix == 0) {

                this.errorDocumentoRepresentanteLegal = this.hasError;
                this.errorTextoDocumentoRepresentanteLegal = i18n.t("valid.tipoDoc");

            } else if (this.registro.representante.documentNumber == '' ||
                this.registro.representante.documentNumber == null) {

                this.errorDocumentoRepresentanteLegal = this.hasError;
                this.errorTextoDocumentoRepresentanteLegal = i18n.t("valid.docRequerido");


            } else if (!regExp.test(this.registro.representante.documentNumber)) {
                this.errorDocumentoRepresentanteLegal = this.hasError;
                this.errorTextoDocumentoRepresentanteLegal = i18n.t("valid.formatDoc") + " " + mask_edit;

            } else if (this.REOnlyZero.test(this.registro.representante.documentNumber)) {
                this.errorDocumentoRepresentanteLegal = this.hasError;
                this.errorTextoDocumentoRepresentanteLegal = i18n.t("valid.docCero");
            } else {
                this.errorDocumentoRepresentanteLegal = this.hasSuccess;
                this.errorTextoDocumentoRepresentanteLegal = '';

            }
        },
        validarNombresRepresentanteLegal() {

            let RE = /^([[A-Za-zÁ-Ýá-ýñÑ\´]{2,}[\s]{1,1}[[A-Za-zÁ-Ýá-ýñÑ\´]{2,}[[A-Za-zÁ-Ýá-ýñÑ\s\.\´]{0,})+$/i
            let emailRegex = /^[[A-Za-zÁ-Ýá-ýñÑ\s\´\.]+$/i;

            if (this.registro.representante.name == '' ||
                this.registro.representante.name == null ||
                this.registro.representante.name[0] == ' ') {

                this.errorNombresRepresentanteLegal = this.hasError;
                this.errorTextoNombresRepresentanteLegal = i18n.t("valid.nombreRequerido");

            } else if (this.registro.representante.name.length < 2) {

                this.errorNombresRepresentanteLegal = this.hasError;
                this.errorTextoNombresRepresentanteLegal = i18n.t("valid.minimo2Char");

            } else if (this.registro.representante.name.length > 255) {

                this.errorNombresRepresentanteLegal = this.hasError;
                this.errorTextoNombresRepresentanteLegal = i18n.t("valid.maxChar");

            } else if (!emailRegex.test(this.registro.representante.name)) {

                this.errorNombresRepresentanteLegal = this.hasError;
                this.errorTextoNombresRepresentanteLegal = i18n.t("valid.charNoPermitidosNombres");

            } else if (!RE.test(this.registro.representante.name)) {

                this.errorNombresRepresentanteLegal = this.hasError;
                this.errorTextoNombresRepresentanteLegal = i18n.t("valid.NombreInversionistaNatural");

            } else if (this.VerificarLetrasSeguidas(this.registro.representante.name)) {

                this.errorNombresRepresentanteLegal = this.hasError;
                this.errorTextoNombresRepresentanteLegal = i18n.t("valid.max2CharRepetidos");

            } else {
                this.errorNombresRepresentanteLegal = this.hasSuccess;
                this.errorTextoNombresRepresentanteLegal = '';

            }
        },
        validarTelefonoRepresentanteLegal() {

            let emailRegex = new RegExp(this.ExprPhone);

            if (this.registro.representante.phoneNumber == '' || this.registro.representante.phoneNumber == null) {
                this.errorTelefonoRepresentante = this.hasError;
                this.errorTextoTelefonoRepresentante = i18n.t("valid.telefonoRequerido");


            } else if (this.registro.representante.phoneNumber.length < 4 ||
                this.registro.representante.phoneNumber.length == 1) {

                this.errorTelefonoRepresentante = this.hasError;
                this.errorTextoTelefonoRepresentante = i18n.t("valid.telefonoFormatInvalid") + " " + this.placeholderTelefono;


            } else if (this.registro.representante.phoneNumber.length > 50) {

                this.errorTelefonoRepresentante = this.hasError;
                this.errorTextoTelefonoRepresentante = i18n.t("valid.telefonoFormatInvalid") + " " + this.placeholderTelefono;


            } else if (this.REOnlyZero.test(this.registro.representante.phoneNumber)) {

                this.errorTelefonoRepresentante = this.hasError;
                this.errorTextoTelefonoRepresentante = i18n.t("valid.docCero");

            } else if (!emailRegex.test(this.registro.representante.phoneNumber)) {

                this.errorTelefonoRepresentante = this.hasError;
                this.errorTextoTelefonoRepresentante = i18n.t("valid.telefonoFormatInvalid") + " " + this.placeholderTelefono;

            } else {
                this.errorTelefonoRepresentante = this.hasSuccess;
                this.errorTextoTelefonoRepresentante = '';

            }
        },
        validarEmailRepresentanteLegal() {

            let RE = /[.]{2,}/
            emailRegex = /^[\w.]{1,64}@(?:[A-Z0-9]{2,63}\.){1,125}[A-Z]{2,63}$/i;

            if (this.registro.representante.email == '' ||
                this.registro.representante.email == null) {

                this.errorEmailRepresentante = this.hasError;
                this.errorTextoEmailRepresentante = i18n.t("valid.emailRequerido");

            } else if (!emailRegex.test(this.registro.representante.email) || RE.test(this.registro.representante.email)) {

                this.errorEmailRepresentante = this.hasError;
                this.errorTextoEmailRepresentante = i18n.t("valid.emailFormatoInvalido");

            } else if (this.registro.representante.email.length < 10) {
                this.errorEmailRepresentante = this.hasError;
                this.errorTextoEmailRepresentante = i18n.t("valid.emailMaxChar");

            } else if (this.registro.representante.email.length > 60) {

                this.errorEmailRepresentante = this.hasError;
                this.errorTextoEmailRepresentante = i18n.t("valid.emailMaxChar");

            } else {
                this.errorEmailRepresentante = this.hasSuccess;
                this.errorTextoEmailRepresentante = '';

            }
        },

        validarDocumentoAsociadoContactoPrincipal(ExpreRegular, mask_edit) {

            var regExp = new RegExp(ExpreRegular);
            if (this.registro.contacto.prefix == '' ||
                this.registro.contacto.prefix == 0) {

                this.errorDocumentoAsociadoContactoPrincipal = this.hasError;
                this.errorDocumentoAsociadoContactoTextoPrincipal = i18n.t("valid.tipoDoc");


            } else if (this.registro.contacto.documentNumber == '' ||
                this.registro.contacto.documentNumber == null) {

                this.errorDocumentoAsociadoContactoPrincipal = this.hasError;
                this.errorDocumentoAsociadoContactoTextoPrincipal = i18n.t("valid.docRequerido");


            } else if (!regExp.test(this.registro.contacto.documentNumber)) {
                this.errorDocumentoAsociadoContactoPrincipal = this.hasError;
                this.errorDocumentoAsociadoContactoTextoPrincipal = i18n.t("valid.formatDoc") + " (" + mask_edit + ")";

            } else if (this.REOnlyZero.test(this.registro.contacto.documentNumber)) {
                this.errorDocumentoAsociadoContactoPrincipal = this.hasError;
                this.errorDocumentoAsociadoContactoTextoPrincipal = i18n.t("valid.docCero");

            } else {
                this.errorDocumentoAsociadoContactoPrincipal = this.hasSuccess;
                this.errorDocumentoAsociadoContactoTextoPrincipal = '';
                return
            }

            if (this.registro.contacto.documentNumber == '' ||
                this.registro.contacto.documentNumber == null &&
                this.registro.contacto.prefix == '' ||
                this.registro.contacto.prefix == null) {

                this.errorDocumentoAsociadoContactoPrincipal = '';
                this.errorDocumentoAsociadoContactoTextoPrincipal = '';

            }

            this.errorNombreContactoPrincipal = '';
            this.errorNombreContactoTextoPrincipal = '';

            this.errorApellidoContactoPrincipal = '';
            this.errorApellidoContactoTextoPrincipal = '';

            this.errorTelefonoContactoPrincipal = '';
            this.errorTelefonoContactoTextoPrincipal = '';

            this.errorEmailContactoPrincipal = '';
            this.errorEmailContactoTextoPrincipal = '';

            this.registro.contacto.name = ''
            this.registro.contacto.phoneNumber = ''
            this.registro.contacto.email = ''
        },
        validarNombresAsociadoContactoPrincipal() {

            let RE = /^([[A-Za-zÁ-Ýá-ýñÑ\´]{2,}[\s]{1,1}[[A-Za-zÁ-Ýá-ýñÑ\´]{2,}[[A-Za-zÁ-Ýá-ýñÑ\s\.\´]{0,})+$/i
            let emailRegex = /^[[A-Za-zÁ-Ýá-ýñÑ\s\´\.]+$/i;

            if (this.registro.contacto.name == '' ||
                this.registro.contacto.name == null ||
                this.registro.contacto.name[0] == ' ') {

                this.errorNombreContactoPrincipal = this.hasError;
                this.errorNombreContactoTextoPrincipal = i18n.t("valid.nombreRequerido");

            } else if (this.registro.contacto.name.length < 2) {

                this.errorNombreContactoPrincipal = this.hasError;
                this.errorNombreContactoTextoPrincipal = i18n.t("valid.minimo2Char");

            } else if (this.registro.contacto.name.length > 255) {

                this.errorNombreContactoPrincipal = this.hasError;
                this.errorNombreContactoTextoPrincipal = i18n.t("valid.maxChar");


            } else if (!emailRegex.test(this.registro.contacto.name)) {

                this.errorNombreContactoPrincipal = this.hasError;
                this.errorNombreContactoTextoPrincipal = i18n.t("valid.charNoPermitidosNombres");

            } else if (!RE.test(this.registro.contacto.name)) {

                this.errorNombreContactoPrincipal = this.hasError;
                this.errorNombreContactoTextoPrincipal = i18n.t("valid.NombreInversionistaNatural");

            } else if (this.VerificarLetrasSeguidas(this.registro.contacto.name)) {

                this.errorNombreContactoPrincipal = this.hasError;
                this.errorNombreContactoTextoPrincipal = i18n.t("valid.max2CharRepetidos");

            } else {
                this.errorNombreContactoPrincipal = this.hasSuccess;
                this.errorNombreContactoTextoPrincipal = '';

            }
        },
        validarTelefonoContactoPrincipal() {
            var emailRegex = new RegExp(this.ExprPhone);

            if (this.registro.contacto.phoneNumber == '' || this.registro.contacto.phoneNumber == null) {

                this.errorTelefonoContactoPrincipal = this.hasError;
                this.errorTelefonoContactoTextoPrincipal = i18n.t("valid.telefonoRequerido");

            } else if (this.REOnlyZero.test(this.registro.contacto.phoneNumber)) {

                this.errorTelefonoContactoPrincipal = this.hasError;
                this.errorTelefonoContactoTextoPrincipal = i18n.t("valid.mayorDeCero");

            } else if (this.registro.contacto.phoneNumber.length < 4) {

                this.errorTelefonoContactoPrincipal = this.hasError;
                this.errorTelefonoContactoTextoPrincipal = i18n.t("valid.telefonoFormatInvalid") + " " + this.placeholderTelefono;

            } else if (this.registro.contacto.phoneNumber.length > 50) {

                this.errorTelefonoContactoPrincipal = this.hasError;
                this.errorTelefonoContactoTextoPrincipal = i18n.t("valid.telefonoFormatInvalid") + " " + this.placeholderTelefono;

            } else if (!emailRegex.test(this.registro.contacto.phoneNumber)) {

                this.errorTelefonoContactoPrincipal = this.hasError;
                this.errorTelefonoContactoTextoPrincipal = i18n.t("valid.telefonoFormatInvalid") + " " + this.placeholderTelefono;

            } else {
                this.errorTelefonoContactoPrincipal = this.hasSuccess;
                this.errorTelefonoContactoTextoPrincipal = '';

            }
        },
        validarEmailContactoPrincipal() {
            let RE = /[\.]{2,}/
            emailRegex = /^[\w.]{1,64}@(?:[A-Z0-9]{2,63}\.){1,125}[A-Z]{2,63}$/i;

            if (this.registro.contacto.email == '' || this.registro.contacto.email == null) {

                this.errorEmailContactoPrincipal = this.hasError;
                this.errorEmailContactoTextoPrincipal = i18n.t("valid.emailRequerido");


            } else if (!emailRegex.test(this.registro.contacto.email) || RE.test(this.registro.contacto.email)) {

                this.errorEmailContactoPrincipal = this.hasError;
                this.errorEmailContactoTextoPrincipal = i18n.t("valid.emailFormatoInvalido");

            } else if (this.registro.contacto.email.length < 10 || this.registro.contacto.email.length > 60) {

                this.errorEmailContactoPrincipal = this.hasError;
                this.errorEmailContactoTextoPrincipal = i18n.t("valid.emailMaxChar");

            } else {
                this.errorEmailContactoPrincipal = this.hasSuccess;
                this.errorEmailContactoTextoPrincipal = '';
            }
        },

        validarDireccion() {

            let emailRegex = /^[0-9\_\-\.\,\&\%\#\!\*\(\)\$\:\;\[\]\{\}\"\'\s\xF1\xD1]+$/;

            if (this.registro.registrarse.address.line1 == '' ||
                this.registro.registrarse.address.line1 == null ||
                this.registro.registrarse.address.line1[0] == ' ') {

                this.errorDireccion = this.hasError;
                this.errorTextoDireccion = i18n.t("valid.direccionPrincipalRequerida");

            } else if (emailRegex.test(this.registro.registrarse.address.line1)) {

                this.errorDireccion = this.hasError;
                this.errorTextoDireccion = i18n.t("valid.direccionAlphNumer");

            } else if (this.registro.registrarse.address.line1.length < 15) {

                this.errorDireccion = this.hasError;
                this.errorTextoDireccion = i18n.t("valid.direccionMaxChar");

            } else if (this.registro.registrarse.address.line1.length > 255) {

                this.errorDireccion = this.hasError;
                this.errorTextoDireccion = i18n.t("valid.maxChar");


            } else if (this.VerificarLetrasSeguidas(this.registro.registrarse.address.line1)) {

                this.errorDireccion = this.hasError;
                this.errorTextoDireccion = i18n.t("valid.max2CharRepetidos");

            } else {
                this.errorDireccion = this.hasSuccess;
                this.errorTextoDireccion = '';
            }
        },
        restablecerCiudad() {
            if (this.idCiudad == 0) {

                this.errorEstado = this.hasError
                this.errorEstadoTexto = i18n.t("valid.seleccionEstado");
                return
            }

            this.registro.registrarse.address.region = this.idCiudad;
            this.errorEstado = this.hasSuccess
            this.errorEstadoTexto = ''
            this.errorCiudad = ''
            this.errorCiudadTexto = ''
            this.registro.registrarse.address.city = 0;

            this.CiudadSelected(this.idCiudad);
        },
        validarDireccion2() {
            let emailRegex = /^[0-9\_\-\.\,\&\%\#\!\*\(\)\$\:\;\[\]\{\}\"\'\s\xF1\xD1]+$/;

            if (this.registro.registrarse.address.line2 == '' ||
                this.registro.registrarse.address.line2 == null ||
                this.registro.registrarse.address.line2[0] == ' ') {

                this.errorDireccion2 = '';
                this.errorDireccion2Texto = '';

            } else if (emailRegex.test(this.registro.registrarse.address.line2)) {

                this.errorDireccion2 = this.hasError;
                this.errorDireccion2Texto = i18n.t("valid.direccionAlphNumer");

            } else if (this.registro.registrarse.address.line2.length < 15) {

                this.errorDireccion2 = this.hasError;
                this.errorDireccion2Texto = i18n.t("valid.direccionMaxChar");

            } else if (this.registro.registrarse.address.line2.length > 255) {

                this.errorDireccion2 = this.hasError;
                this.errorDireccion2Texto = i18n.t("valid.maxChar");


            } else if (this.VerificarLetrasSeguidas(this.registro.registrarse.address.line2)) {

                this.errorDireccion2 = this.hasError;
                this.errorDireccion2Texto = i18n.t("valid.max2CharRepetidos");

            } else {
                this.errorDireccion2 = this.hasSuccess;
                this.errorDireccion2Texto = ''

            }
        },
        validarCiudad() {

            if (this.registro.registrarse.address.city == 'Seleccione Ciudad' ||
                this.registro.registrarse.address.city == null ||
                this.registro.registrarse.address.city == '' ||
                this.registro.registrarse.address.city == 0) {

                this.errorCiudad = this.hasError;
                this.errorCiudadTexto = i18n.t("valid.seleccionCiudad");

            } else {
                this.errorCiudad = this.hasSuccess;
                this.errorCiudadTexto = '';

            }
        },
        validarTelefonoDirecciones() {

            let emailRegex = new RegExp(this.ExprPhone);

            if (this.registro.registrarse.phone.number == '' || this.registro.registrarse.phone.number == null) {
                this.errorTelefonoDirecciones = this.hasError;
                this.errorTextoTelefonoDirecciones = i18n.t("valid.telefonoRequerido");

            } else if (this.registro.registrarse.phone.number.length < 6 || this.registro.registrarse.phone.length == 1) {
                this.errorTelefonoDirecciones = this.hasError;
                this.errorTextoTelefonoDirecciones = i18n.t("valid.telefonoFormatInvalid") + " " + this.placeholderTelefono;

            } else if (this.registro.registrarse.phone.number.length > 50) {

                this.errorTelefonoDirecciones = this.hasError;
                this.errorTextoTelefonoDirecciones = i18n.t("valid.telefonoFormatInvalid") + " " + this.placeholderTelefono;

            } else if (this.REOnlyZero.test(this.registro.registrarse.phone.number)) {

                this.errorTelefonoDirecciones = this.hasError;
                this.errorTextoTelefonoDirecciones = i18n.t("valid.docCero");

            } else if (!emailRegex.test(this.registro.registrarse.phone.number)) {

                this.errorTelefonoDirecciones = this.hasError;
                this.errorTextoTelefonoDirecciones = i18n.t("valid.telefonoFormatInvalid") + " " + this.placeholderTelefono;

            } else {
                this.errorTelefonoDirecciones = this.hasSuccess;
                this.errorTextoTelefonoDirecciones = '';

            }
        },

        //--------- Seccion Validacion del Administrador--------------

        validarDocumentoAdministrador(ExpreRegular, mask_edit) {

            var regExp = new RegExp(ExpreRegular);

            if (this.registro.administrador.documentNumber == '' ||
                this.registro.administrador.documentNumber == null ||
                this.registro.administrador.prefix == '' ||
                this.registro.administrador.prefix == 0) {
                this.errorDocumentoAdministrador = this.hasError;
                this.errorDocumentoAdministradorTexto = i18n.t("valid.docRequerido");


            } else if (!regExp.test(this.registro.administrador.documentNumber)) {
                this.errorDocumentoAdministrador = this.hasError;
                this.errorDocumentoAdministradorTexto = i18n.t("valid.formatDoc") + " (" + mask_edit + ")";

            } else if (this.REOnlyZero.test(this.registro.administrador.documentNumber)) {

                this.errorDocumentoAdministrador = this.hasError;
                this.errorDocumentoAdministradorTexto = i18n.t("valid.docCero");


            } else {
                this.errorDocumentoAdministrador = this.hasSuccess;
                this.errorDocumentoAdministradorTexto = '';
            }
        },
        validarNombresAdministrador() {
            let RE = /^([[A-Za-zÁ-Ýá-ýñÑ\´]{2,}[\s]{1,1}[[A-Za-zÁ-Ýá-ýñÑ\´]{2,}[[A-Za-zÁ-Ýá-ýñÑ\s\.\´]{0,})+$/i
            let emailRegex = /^[[A-Za-zÁ-Ýá-ýñÑ\s\´\.]+$/i;

            if (this.registro.administrador.name == '' ||
                this.registro.administrador.name == null ||
                this.registro.administrador.name[0] == ' ') {

                this.errorNombreAdministrador = this.hasError;
                this.errorNombreAdministradorTexto = i18n.t("valid.nombreRequerido");

            } else if (this.registro.administrador.name.length < 2) {

                this.errorNombreAdministrador = this.hasError;
                this.errorNombreAdministradorTexto = i18n.t("valid.minimo2Char");

            } else if (this.registro.administrador.name.length > 255) {

                this.errorNombreAdministrador = this.hasError;
                this.errorNombreAdministradorTexto = i18n.t("valid.maxChar");

            } else if (!emailRegex.test(this.registro.administrador.name)) {

                this.errorNombreAdministrador = this.hasError;
                this.errorNombreAdministradorTexto = i18n.t("valid.charNoPermitidosNombres");

            } else if (!RE.test(this.registro.administrador.name)) {

                this.errorNombreAdministrador = this.hasError;
                this.errorNombreAdministradorTexto = i18n.t("valid.NombreInversionistaNatural");

            } else if (this.VerificarLetrasSeguidas(this.registro.administrador.name)) {

                this.errorNombreAdministrador = this.hasError;
                this.errorNombreAdministradorTexto = i18n.t("valid.charNoPermitidos");

            } else {
                this.errorNombreAdministrador = this.hasSuccess;
                this.errorNombreAdministradorTexto = '';

            }
        },
        validarTelefonoAdministrador() {
            var emailRegex = new RegExp(this.ExprPhone);

            if (this.registro.administrador.phoneNumber == '' || this.registro.administrador.phoneNumber == null) {

                this.errorTelefonoAdministrador = this.hasError;
                this.errorTelefonoAdministradorTexto = i18n.t("valid.telefonoRequerido");


            } else if (this.registro.administrador.phoneNumber.length < 4) {

                this.errorTelefonoAdministrador = this.hasError;
                this.errorTelefonoAdministradorTexto = i18n.t("valid.telefonoFormatInvalid") + " " + this.placeholderTelefono;

            } else if (this.registro.administrador.phoneNumber.length > 50) {

                this.errorTelefonoAdministrador = this.hasError;
                this.errorTelefonoAdministradorTexto = i18n.t("valid.telefonoFormatInvalid") + " " + this.placeholderTelefono;

            } else if (this.REOnlyZero.test(this.registro.administrador.phoneNumber)) {

                this.errorTelefonoAdministrador = this.hasError;
                this.errorTelefonoAdministradorTexto = i18n.t("valid.mayorDeCero");

            } else if (!emailRegex.test(this.registro.administrador.phoneNumber)) {

                this.errorTelefonoAdministrador = this.hasError;
                this.errorTelefonoAdministradorTexto = i18n.t("valid.telefonoFormatInvalid") + " " + this.placeholderTelefono;

            } else {
                this.errorTelefonoAdministrador = this.hasSuccess;
                this.errorTelefonoAdministradorTexto = '';

            }
        },
        validarEmailAdministrador() {

            emailRegex = /^[\w.]{1,64}@(?:[A-Z0-9]{2,63}\.){1,125}[A-Z]{2,63}$/i;

            if (this.registro.administrador.email == '' || this.registro.administrador.email == null) {

                this.errorEmailAdministrador = this.hasError;
                this.errorEmailAdministradorTexto = i18n.t("valid.emailRequerido");


            } else if (!emailRegex.test(this.registro.administrador.email) || this.RETwoPoint.test(this.registro.administrador.email)) {

                this.errorEmailAdministrador = this.hasError;
                this.errorEmailAdministradorTexto = i18n.t("valid.emailFormatoInvalido");

            } else if (this.registro.administrador.email.length < 10 || this.registro.administrador.email.length > 60) {

                this.errorEmailAdministrador = this.hasError;
                this.errorEmailAdministradorTexto = i18n.t("valid.emailMaxChar");

            } else {
                this.errorEmailAdministrador = this.hasSuccess;
                this.errorEmailAdministradorTexto = '';
            }
        },
        validarNumeroRoteo() {

            if (this.registro.registrarse.routing_number == null ||
                this.registro.registrarse.routing_number == '' ||
                this.registro.registrarse.routing_number[0] == ' ') {

                this.errorRoutinNumber = this.hasError;
                this.errorRoutinNumberTexto = i18n.t("valid.codigoBank");

            } else if (this.registro.registrarse.routing_number.length < 4) {

                this.errorRoutinNumber = this.hasError;
                this.errorRoutinNumberTexto = i18n.t("valid.minimoCharBanco");

            } else if (this.registro.registrarse.routing_number.length > 16) {

                this.errorRoutinNumber = this.hasError;
                this.errorRoutinNumberTexto = i18n.t("valid.maxCodigoBank");

            } else if (this.REOnlyZero.test(this.registro.registrarse.routing_number)) {
                this.errorRoutinNumber = this.hasError
                this.errorRoutinNumberTexto = i18n.t("valid.mayorDeCero");
            } else {
                this.errorRoutinNumber = this.hasSuccess;
                this.errorRoutinNumberTexto = "";
            }
        },

        //--------- LogoBanco -------------------------------------
        logoBanco(event) {

            let archivo = event.target.files[0];

            if (archivo.type == 'image/jpeg' || archivo.type == 'image/jpg' || archivo.type == 'image/png' || archivo.type == 'image/ico') {

                let reader = new FileReader();

                reader.readAsDataURL(archivo);
                reader.onload = evt => {
                    let img = new Image();

                    img.src = evt.target.result;

                    if (img.src.match('data:image/jpeg;base64,/9j/') || img.src.match('data:image/png;base64,iVBOR')) {
                        img.onload = () => {

                            if (img.width != 0 && img.height != 0) {

                                this.registro.registrarse.logo = evt.target.result;

                            }
                        }

                    } else {

                        document.getElementById("file").value = "";
                        alert(i18n.t("valid.logoBank"));
                    }

                }

            } else {
                document.getElementById("file").value = "";
                alert(i18n.t("valid.logoBank"));
            }

        },

        //--------------------------------- Validacion de Cuentas Bancarias --------------------------
        validarSeleccionBancaria(event) {

            if (this.registro.cuentaActual.entity == null || this.registro.cuentaActual.entity[0] == ' ' || this.registro.cuentaActual.entity == '') {
                this.idBanco = this.hasError;
                this.idBancoTexto = i18n.t("valid.cuentaBanco");

            } else {

                if (this.registro.registrarse.accounts.length != 0) {

                    for (let i = 0; i < this.registro.registrarse.accounts.length; i++) {

                        /*
                         if (this.registro.registrarse.accounts[i].entity == this.registro.cuentaActual.entity) {
                            this.idBanco = this.hasError;
                            this.idBancoTexto = i18n.t("valid.cuentaBancoUnica");

                            return
                        }
                         */

                        if (this.registro.registrarse.accounts[i].entity == this.registro.cuentaActual.entity &&
                            this.registro.registrarse.accounts[i].currency == this.registro.cuentaActual.currency) {
                            //this.tipoMonedaBanco = this.hasError;
                            //this.tipoMonedaBancoTexto = i18n.t("valid.cuentaBancoUnica");

                            this.idBanco = this.hasError;
                            this.idBancoTexto = i18n.t("valid.cuentaBancoUnica");
                            return
                        }

                        if (this.registro.cuentaActual.currency != null && this.registro.cuentaActual.currency != "") {
                            this.tipoMonedaBanco = this.hasSuccess;
                            this.tipoMonedaBancoTexto = '';
                        }
                    }
                }
                this.idBanco = this.hasSuccess;
                this.idBancoTexto = '';

                this.nombreBanco = event.target.options[event.target.options.selectedIndex].text;
                this.cambioNombreBanco = true;
                console.log(this.picked)
                console.log(this.registro.cuentaActual.accountNumber)
                if (this.registro.cuentaActual.accountNumber != null && this.registro.cuentaActual.accountNumber != "" && this.picked == "Gestion de Cuenta") {
                    this.validarnumeroCuentaBanco();
                }
            }
        },
        validarSeleccionTipoMoneda() {
            if (this.registro.cuentaActual.currency == null || this.registro.cuentaActual.currency == '') {
                this.tipoMonedaBanco = this.hasError;
                this.tipoMonedaBancoTexto = i18n.t("valid.cuentaMoneda");

            } else {

                if (this.registro.registrarse.accounts.length != 0) {

                    for (let i = 0; i < this.registro.registrarse.accounts.length; i++) {

                        if (this.registro.registrarse.accounts[i].entity == this.registro.cuentaActual.entity &&
                            this.registro.registrarse.accounts[i].currency == this.registro.cuentaActual.currency) {
                            this.tipoMonedaBanco = this.hasError;
                            this.tipoMonedaBancoTexto = i18n.t("valid.cuentaBancoUnica");

                            //this.idBanco = this.hasError;
                            //this.idBancoTexto = i18n.t("valid.cuentaBancoUnica");
                            return
                        }

                        if (this.registro.cuentaActual.currency != null && this.registro.cuentaActual.currency != "") {
                            this.idBanco = this.hasSuccess;
                            this.idBancoTexto = '';
                        }
                    }
                }
                this.tipoMonedaBanco = this.hasSuccess;
                this.tipoMonedaBancoTexto = '';
            }
        },
        validarSeleccionTipoCuenta() {
            if (this.registro.cuentaActual.accountType == null || this.registro.cuentaActual.accountType == '') {
                this.tipoCuentaBanco = this.hasError;
                this.tipoCuentaBancoTexto = i18n.t("valid.cuentaTipo");

            } else {
                this.tipoCuentaBanco = this.hasSuccess;
                this.tipoCuentaBancoTexto = '';
            }
        },
        validarNombresTitularBanco() {
            let RE = /^([A-Z\']{2,}[\s]{1,1}[A-Z\.\']{2,}[A-Z\s\.\']{0,})+$/i
            let emailRegex = /^[A-Z\s\'\.]+$/i;

            if (this.registro.cuentaActual.name_on_account == null || this.registro.cuentaActual.name_on_account[0] == ' ' || this.registro.cuentaActual.name_on_account == '') {
                this.TituluarBanco = this.hasError;
                this.TituluarBancoTexto = i18n.t("valid.titularRequerido");

            } else if (this.registro.cuentaActual.name_on_account.length < 2) {

                this.TituluarBanco = this.hasError;
                this.TituluarBancoTexto = i18n.t("valid.titularRequerido");

            } else if (this.registro.cuentaActual.name_on_account.length > 255) {

                this.TituluarBanco = this.hasError;
                this.TituluarBancoTexto = i18n.t("valid.maxChar");

            } else if (!emailRegex.test(this.registro.cuentaActual.name_on_account)) {

                this.TituluarBanco = this.hasError;
                this.TituluarBancoTexto = i18n.t("valid.charNoPermitidosNombres");


            } else if (!RE.test(this.registro.cuentaActual.name_on_account)) {

                this.TituluarBanco = this.hasError;
                this.TituluarBancoTexto = i18n.t("valid.nombreAlphNumer");


            } else if (this.VerificarLetrasSeguidas(this.registro.cuentaActual.name_on_account)) {

                this.TituluarBanco = this.hasError;
                this.TituluarBancoTexto = i18n.t("valid.max2CharRepetidos");


            } else {
                this.TituluarBanco = this.hasSuccess;
                this.TituluarBancoTexto = '';

            }
        },
        validarnumeroCuentaBanco() {

            var emailRegex = new RegExp(this.ExprAccount);

            if (this.registro.cuentaActual.accountNumber == null || this.registro.cuentaActual.accountNumber[0] == ' ' || this.registro.cuentaActual.accountNumber == '') {
                this.numeroCuentaBanco = this.hasError;
                this.numeroCuentaBancoTexto = i18n.t("valid.numeroCuentaRequerido");

            } else if (!emailRegex.test(this.registro.cuentaActual.accountNumber)) {
                this.numeroCuentaBanco = this.hasError;
                this.numeroCuentaBancoTexto = i18n.t("valid.formatCuenta") + " " + this.placeholderCuentaBancaria;

            } else if (this.REOnlyZero.test(this.registro.cuentaActual.accountNumber)) {
                this.numeroCuentaBanco = this.hasError;
                this.numeroCuentaBancoTexto = i18n.t("valid.mayorDeCero");
            } else {

                var contadorCuenta = 0;
                for (var i = 0; i < this.registro.registrarse.accounts.length; i++) {

                    if (this.registro.cuentaActual.accountNumber == this.registro.registrarse.accounts[i].accountNumber) {
                        contadorCuenta++;
                    }
                }

                if (contadorCuenta == 0) {

                    if (this.registro.cuentaActual.entity == null || this.registro.cuentaActual.entity == "") {

                        this.numeroCuentaBanco = this.hasError;
                        this.numeroCuentaBancoTexto = i18n.t("valid.cuentaBanco");

                    } else {

                        this.numeroCuentaBanco = this.hasSuccess;
                        this.numeroCuentaBancoTexto = '';
                        //--------------- segun esto de abajo no va -------------
                        if (this.dataPaises.entities != null && false) {

                            this.dataPaises.entities.map(data => {

                                if (data.id == this.registro.cuentaActual.entity) {

                                    console.log(data)
                                    let distancia = data.routing_number.length;
                                    let codigo_revision = this.registro.cuentaActual.accountNumber.slice(0, distancia);

                                    if (data.routing_number == codigo_revision) {

                                        this.numeroCuentaBanco = this.hasSuccess;
                                        this.numeroCuentaBancoTexto = '';
                                    }
                                    else {
                                        this.numeroCuentaBanco = this.hasError;
                                        this.numeroCuentaBancoTexto = i18n.t("valid.codigoBancarioIncorrecto");
                                    }

                                    return
                                }

                            });
                        }
                        //--------------- segun esto de abajo no va -------------
                    }

                } else if (this.selectCuentas != 999) {

                    if (this.registro.cuentaActual.entity == null || this.registro.cuentaActual.entity == "") {

                        this.numeroCuentaBanco = this.hasError;
                        this.numeroCuentaBancoTexto = i18n.t("valid.cuentaBanco");

                    } else {
                        if (this.dataPaises.entities != null) {

                            this.dataPaises.entities.map(data => {

                                if (data.id == this.registro.cuentaActual.entity) {

                                    console.log(data)
                                    let distancia = data.routing_number.length;
                                    let codigo_revision = this.registro.cuentaActual.accountNumber.slice(0, distancia);

                                    if (data.routing_number == codigo_revision) {

                                        this.numeroCuentaBanco = this.hasSuccess;
                                        this.numeroCuentaBancoTexto = '';
                                    }
                                    else {
                                        this.numeroCuentaBanco = this.hasError;
                                        this.numeroCuentaBancoTexto = i18n.t("valid.codigoBancarioIncorrecto");
                                    }

                                    return
                                }

                            });
                        }
                    }

                } else {

                    this.numeroCuentaBanco = this.hasError;
                    this.numeroCuentaBancoTexto = i18n.t("valid.numCuentaExistente");
                }
            }
        },

        //--------------------------------- Operaciones CRUD de Cuentas Bancarias --------------------------
        seleccionCuenta(cuenta, index) {

            this.dialogCuentas = true;
            this.picked = 'Gestion de Cuenta';

            this.registro.cuentaActual.entity = cuenta.entity;
            this.registro.cuentaActual.accountNumber = cuenta.accountNumber;
            this.registro.cuentaActual.accountType = cuenta.accountType;
            //this.registro.cuentaActual.name_on_account = cuenta.name_on_account;
            this.registro.cuentaActual.currency = cuenta.currency;

            if (cuenta.default == true) this.pickedCheck = true;
            else this.pickedCheck = false;

            this.idBanco = this.hasSuccess;
            this.tipoMonedaBanco = this.hasSuccess;
            this.tipoCuentaBanco = this.hasSuccess;
            this.TituluarBanco = this.hasSuccess;
            this.numeroCuentaBanco = this.hasSuccess;
            this.selectCuentas = index;

            this.idBancoTexto = '';
            this.tipoMonedaBancoTexto = '';
            this.tipoCuentaBancoTexto = '';
            this.TituluarBancoTexto = '';
            this.numeroCuentaBancoTexto = '';
        },
        borrarCuenta(index) {

            if (this.registro.registrarse.accounts != null) {
                this.registro.registrarse.accounts[index].status = false;
            }

            this.guardarActualizacion(this.registro);
        },
        guardarCuenta() {

            if (this.selectCuentas == 999 && this.picked == 'Gestion de Cuenta') {

                let predeterminada = false;
                let contadorCurrency = 0;
                let distanciaCuentas = this.registro.registrarse.accounts.length;
                if (this.pickedCheck) {
                    for (let i = 0; i < this.registro.registrarse.accounts.length; i++) {

                        if (this.registro.registrarse.accounts[i].currency == this.registro.cuentaActual.currency) {
                            this.registro.registrarse.accounts[i].default = false;
                            console.log(this.registro.cuentaActual)
                        }
                        
                    }
                    predeterminada = true;
                }

                if (this.pickedCheck == false) {
                    for (let i = 0; i < this.registro.registrarse.accounts.length; i++) {

                        if (this.registro.registrarse.accounts[i].currency != this.registro.cuentaActual.currency &&
                            this.registro.registrarse.accounts[i].status == true) {
                            contadorCurrency++;
                        }

                    }
                    if (this.registro.registrarse.accounts.length == contadorCurrency) predeterminada = true;                   
                }

                this.registro.registrarse.accounts.push({
                    entity: this.registro.cuentaActual.entity,
                    accountType: this.registro.cuentaActual.accountType,
                    accountNumber: this.registro.cuentaActual.accountNumber,
                    currency: this.registro.cuentaActual.currency,
                    default: predeterminada,
                    status: true
                });
                this.errorCuentasBancariasGestionCuenta = '';

                //-----Resetiar input de Cuentas -----------Poner en Metodo
                this.registro.cuentaActual.entity = '';
                this.registro.cuentaActual.accountType = '';
                this.registro.cuentaActual.accountNumber = '';
                this.registro.cuentaActual.currency = '';
                this.idBanco = '';
                this.tipoMonedaBanco = '';
                this.tipoCuentaBanco = '';
                this.numeroCuentaBanco = '';
                this.selectCuentas = 999;
                this.cambioNombreBanco = false;
                this.pickedCheck = false;
                //this.nombreBanco = '';

            } else {
                this.registro.registrarse.accounts[this.selectCuentas].entity = this.registro.cuentaActual.entity;
                this.registro.registrarse.accounts[this.selectCuentas].accountNumber = this.registro.cuentaActual.accountNumber;
                this.registro.registrarse.accounts[this.selectCuentas].accountType = this.registro.cuentaActual.accountType;
                this.registro.registrarse.accounts[this.selectCuentas].currency = this.registro.cuentaActual.currency;
                this.registro.registrarse.accounts[this.selectCuentas].status = true;

                let predeterminada = false;
                if (this.pickedCheck) {

                    for (let i = 0; i < this.registro.registrarse.accounts.length; i++) {

                        if (this.registro.registrarse.accounts[i].currency == this.registro.cuentaActual.currency) {
                            this.registro.registrarse.accounts[i].default = false;
                            console.log(this.registro.cuentaActual)
                        }
                    }
                    //for (let i = 0; i < this.registro.registrarse.accounts.length; i++) this.registro.registrarse.accounts[i].default = false;
                    predeterminada = true;
                }
                this.registro.registrarse.accounts[this.selectCuentas].default = predeterminada;
                this.errorCuentasBancariasGestionCuenta = '';

                if (this.cambioNombreBanco) {
                    this.registro.nombresBancos[this.selectCuentas] = this.nombreBanco;
                    this.cambioNombreBanco = false;
                }

                //-----Resetiar input de Cuentas -----------Poner en Metodo
                this.registro.cuentaActual.entity = '';
                this.registro.cuentaActual.accountType = '';
                this.registro.cuentaActual.accountNumber = '';
                this.registro.cuentaActual.currency = '';
                this.idBanco = '';
                this.tipoMonedaBanco = '';
                this.tipoCuentaBanco = '';
                this.numeroCuentaBanco = '';
                this.selectCuentas = 999;
                this.cambioNombreBanco = false;
                this.nombreBanco = '';
                this.pickedCheck = false;
            }
            if (this.errorCuentasBancariasGestionCuenta == '') this.guardarActualizacion(this.registro);
        },
        guardarCuentaBackOffice() {

            if (this.selectCuentas == 999 && this.picked == 'Gestion de Cuenta') {

                let predeterminada = false;
                let contadorCurrency = 0;
                let distanciaCuentas = this.registro.registrarse.accounts.length;
                if (this.pickedCheck) {

                    predeterminada = true;
                }

                if (this.pickedCheck == false) {
                    for (let i = 0; i < this.registro.registrarse.accounts.length; i++) {

                        if (this.registro.registrarse.accounts[i].currency != this.registro.cuentaActual.currency &&
                            this.registro.registrarse.accounts[i].status == true) {
                            contadorCurrency++;
                        }

                    }
                    if (this.registro.registrarse.accounts.length == contadorCurrency) predeterminada = true;
                }

                this.registro.registrarse.accounts.push({
                    entity: this.registro.cuentaActual.entity,
                    accountType: this.registro.cuentaActual.accountType,
                    accountNumber: this.registro.cuentaActual.accountNumber,
                    currency: this.registro.cuentaActual.currency,
                    default: predeterminada,
                    status: true
                });
                this.errorCuentasBancariasGestionCuenta = '';

                //-----Resetiar input de Cuentas -----------Poner en Metodo
                this.registro.cuentaActual.entity = '';
                this.registro.cuentaActual.accountType = '';
                this.registro.cuentaActual.accountNumber = '';
                this.registro.cuentaActual.currency = '';
                this.idBanco = '';
                this.tipoMonedaBanco = '';
                this.tipoCuentaBanco = '';
                this.numeroCuentaBanco = '';
                this.selectCuentas = 999;
                this.cambioNombreBanco = false;
                this.pickedCheck = false;
                //this.nombreBanco = '';

            } else {
                this.registro.registrarse.accounts[this.selectCuentas].entity = this.registro.cuentaActual.entity;
                this.registro.registrarse.accounts[this.selectCuentas].accountNumber = this.registro.cuentaActual.accountNumber;
                this.registro.registrarse.accounts[this.selectCuentas].accountType = this.registro.cuentaActual.accountType;
                this.registro.registrarse.accounts[this.selectCuentas].currency = this.registro.cuentaActual.currency;
                this.registro.registrarse.accounts[this.selectCuentas].status = true;

                let predeterminada = false;
                if (this.pickedCheck) {
                    predeterminada = true;
                }
                this.registro.registrarse.accounts[this.selectCuentas].default = predeterminada;
                this.errorCuentasBancariasGestionCuenta = '';

                if (this.cambioNombreBanco) {
                    this.registro.nombresBancos[this.selectCuentas] = this.nombreBanco;
                    this.cambioNombreBanco = false;
                }

                //-----Resetiar input de Cuentas -----------Poner en Metodo
                this.registro.cuentaActual.entity = '';
                this.registro.cuentaActual.accountType = '';
                this.registro.cuentaActual.accountNumber = '';
                this.registro.cuentaActual.currency = '';
                this.idBanco = '';
                this.tipoMonedaBanco = '';
                this.tipoCuentaBanco = '';
                this.numeroCuentaBanco = '';
                this.selectCuentas = 999;
                this.cambioNombreBanco = false;
                this.nombreBanco = '';
                this.pickedCheck = false;
            }
            if (this.errorCuentasBancariasGestionCuenta == '') this.guardarActualizacion(this.registro);
        },
        resetearInputCuentas() {
            this.registro.cuentaActual.entity = '';
            this.registro.cuentaActual.accountType = '';
            this.registro.cuentaActual.accountNumber = '';
            this.registro.cuentaActual.currency = '';
            this.idBanco = '';
            this.tipoMonedaBanco = '';
            this.tipoCuentaBanco = '';
            this.numeroCuentaBanco = '';
            this.selectCuentas = 999;
            this.cambioNombreBanco = false;
            this.nombreBanco = '';

            this.idBancoTexto = '';
            this.tipoMonedaBancoTexto = '';
            this.tipoCuentaBancoTexto = '';
            this.numeroCuentaBancoTexto = '';

            this.dialogCuentas = false;
            this.errorCuentasBancariasGestionCuenta = '';
        },

        //--------------------------------- Validacion de Cliente Asociado --------------------------
        validarDocumentoAsociado(ExpreRegular, mask_edit) {

            console.log(this.registro.registrarse.document.number)
            var regExp = new RegExp(ExpreRegular);

            if (this.registro.asociadoActual.number == null ||
                this.registro.asociadoActual.number == '') {
                this.errorDocumentoAsociado = this.hasError;
                this.errorDocumentoAsociadoTexto = i18n.t("valid.docRequerido");

            } else if (this.registro.asociadoActual.prefix == '' ||
                this.registro.asociadoActual.prefix == 0) {

                this.errorDocumentoAsociado = this.hasError;
                this.errorDocumentoAsociadoTexto = i18n.t("valid.tipoDoc");


            } else if (this.registro.registrarse.document.number == this.registro.asociadoActual.number) {

                this.errorDocumentoAsociado = this.hasError;
                this.errorDocumentoAsociadoTexto = i18n.t("valid.docAsociadoRep");

            } else if (!regExp.test(this.registro.asociadoActual.number)) {
                this.errorDocumentoAsociado = this.hasError;
                this.errorDocumentoAsociadoTexto = i18n.t("valid.formatDoc") + " (" + mask_edit + ")";

            } else if (this.REOnlyZero.test(this.registro.asociadoActual.number)) {
                this.errorDocumentoAsociado = this.hasError;
                this.errorDocumentoAsociadoTexto = i18n.t("valid.docCero");

            } else {
                this.errorDocumentoAsociado = this.hasSuccess;
                this.errorDocumentoAsociadoTexto = '';
            }

            if (this.registro.registrarse.customers != null) {

                for (let i = 0; i < this.registro.registrarse.customers.length; i++) {

                    if (this.registro.asociadoActual.number == this.registro.registrarse.customers[i].number &&
                        this.registro.asociadoActual.prefix == this.registro.registrarse.customers[i].prefix) {
                        this.errorDocumentoAsociado = this.hasError
                        this.errorDocumentoAsociadoTexto = i18n.t("valid.docNumberDuplicado");
                        return
                    }
                }
            }

            if (this.registro.registrarse.suppliers != null) {

                for (let i = 0; i < this.registro.registrarse.suppliers.length; i++) {

                    if (this.registro.asociadoActual.number == this.registro.registrarse.suppliers[i].number &&
                        this.registro.asociadoActual.prefix == this.registro.registrarse.suppliers[i].prefix) {
                        this.errorDocumentoAsociado = this.hasError
                        this.errorDocumentoAsociadoTexto = i18n.t("valid.docNumberDuplicado");
                        return
                    }
                }
            }

            if (this.errorDocumentoAsociado == this.hasSuccess) {
                this.AsociadoExistente();
            }

        },
        validarNombreLegalAsociado() {
            let emailRegex = /[A-Za-zÁ-ý]{1,}/;

            if (this.registro.asociadoActual.legalName == '' ||
                this.registro.asociadoActual.legalName == null ||
                this.registro.asociadoActual.legalName[0] == ' ') {

                this.errorNombreLegalAsociado = this.hasError;
                this.errorNombreLegalAsociadoTexto = i18n.t("valid.LegalRepRequerido");

            } else if (!emailRegex.test(this.registro.asociadoActual.legalName)) {

                this.errorNombreLegalAsociado = this.hasError;
                this.errorNombreLegalAsociadoTexto = i18n.t("valid.charNoPermitidosNombres");

            } else if (this.registro.asociadoActual.legalName.length < 4) {

                this.errorNombreLegalAsociado = this.hasError;
                this.errorNombreLegalAsociadoTexto = i18n.t("valid.minimoChar");

            } else if (this.registro.asociadoActual.legalName.length > 255) {

                this.errorNombreLegalAsociado = this.hasError;
                this.errorNombreLegalAsociadoTexto = i18n.t("valid.maxChar");

            } else if (!this.REOneLetter.test(this.registro.asociadoActual.legalName)) {

                this.errorNombreLegalAsociado = this.hasError;
                this.errorNombreLegalAsociadoTexto = i18n.t("valid.replicaLetra");

            } else {
                this.errorNombreLegalAsociado = this.hasSuccess;
                this.errorNombreLegalAsociadoTexto = '';

            }
        },

        //--------- Seccion Validacion Persona de Contacto--------------
        validarDocumentoAsociadoContacto(ExpreRegular, mask_edit) {

            var regExp = new RegExp(ExpreRegular);

            if (this.registro.asociadoActual.contacto.documento.number == null ||
                this.registro.asociadoActual.contacto.documento.number == '') {

                if (this.principalProveedorRepresentante) {

                    this.errorDocumentoAsociadoContacto = '';
                    this.errorDocumentoAsociadoContactoTexto = '';
                    this.vaciarAsociadoContacto()
                    return
                }

                this.errorDocumentoAsociadoContacto = this.hasError;
                this.errorDocumentoAsociadoContactoTexto = i18n.t("valid.docRequerido");

                return
            } else if (this.registro.asociadoActual.contacto.documento.prefix == '' ||
                this.registro.asociadoActual.contacto.documento.prefix == null) {



                if (this.principalProveedorRepresentante && (this.registro.asociadoActual.contacto.documento.number == ''
                    || this.registro.asociadoActual.contacto.documento.number == null)) {

                    this.errorDocumentoAsociadoContacto = '';
                    this.errorDocumentoAsociadoContactoTexto = '';
                    this.vaciarAsociadoContacto()
                    return
                }

                return

            } else if (!regExp.test(this.registro.asociadoActual.contacto.documento.number)) {
                this.errorDocumentoAsociadoContacto = this.hasError;
                this.errorDocumentoAsociadoContactoTexto = i18n.t("valid.formatDoc") + " (" + mask_edit + ")";

            } else if (this.REOnlyZero.test(this.registro.asociadoActual.contacto.documento.number)) {
                this.errorDocumentoAsociadoContacto = this.hasError;
                this.errorDocumentoAsociadoContactoTexto = i18n.t("valid.docCero");


            } else {
                this.errorDocumentoAsociadoContacto = this.hasSuccess;
                this.errorDocumentoAsociadoContactoTexto = '';
            }
        },
        validarNombreLegalAsociado() {
            let emailRegex = /[A-Za-zÁ-ý]{1,}/;

            if (this.registro.asociadoActual.company == '' ||
                this.registro.asociadoActual.company == null ||
                this.registro.asociadoActual.company[0] == ' ') {

                this.errorNombreLegalAsociado = this.hasError;
                this.errorNombreLegalAsociadoTexto = i18n.t("valid.LegalRepRequerido");

            } else if (!emailRegex.test(this.registro.asociadoActual.company)) {

                this.errorNombreLegalAsociado = this.hasError;
                this.errorNombreLegalAsociadoTexto = i18n.t("valid.charNoPermitidosNombres");

            } else if (this.registro.asociadoActual.company.length < 4) {

                this.errorNombreLegalAsociado = this.hasError;
                this.errorNombreLegalAsociadoTexto = i18n.t("valid.minimoChar");

            } else if (this.registro.asociadoActual.company.length > 255) {

                this.errorNombreLegalAsociado = this.hasError;
                this.errorNombreLegalAsociadoTexto = i18n.t("valid.maxChar");

            } else if (!this.REOneLetter.test(this.registro.asociadoActual.company)) {

                this.errorNombreLegalAsociado = this.hasError;
                this.errorNombreLegalAsociadoTexto = i18n.t("valid.replicaLetra");

            } else {
                this.errorNombreLegalAsociado = this.hasSuccess;
                this.errorNombreLegalAsociadoTexto = '';

            }
        },
        validarNombresAsociadoContacto() {

            let RE = /^([[A-Za-zÁ-Ýá-ýñÑ\´]{2,}[\s]{1,1}[[A-Za-zÁ-Ýá-ýñÑ\´]{2,}[[A-Za-zÁ-Ýá-ýñÑ\s\.\´]{0,})+$/i
            let emailRegex = /^[[A-Za-zÁ-Ýá-ýñÑ\s\´\.]+$/i;

            if (this.registro.asociadoActual.name == '' ||
                this.registro.asociadoActual.name == null ||
                this.registro.asociadoActual.name[0] == ' ') {

                if (this.principalProveedorRepresentante) {
                    this.errorNombreContacto = '';
                    this.errorNombreContactoTexto = '';
                    return
                }

                this.errorNombreContacto = this.hasError;
                this.errorNombreContactoTexto = i18n.t("valid.nombreRequerido");

            } else if (this.registro.asociadoActual.name.length < 2) {

                this.errorNombreContacto = this.hasError;
                this.errorNombreContactoTexto = i18n.t("valid.minimo2Char");

            } else if (!emailRegex.test(this.registro.asociadoActual.name)) {

                this.errorNombreContacto = this.hasError;
                this.errorNombreContactoTexto = i18n.t("valid.charNoPermitidosNombres");

            } else if (!RE.test(this.registro.asociadoActual.name)) {

                this.errorNombreLegal = this.hasError;
                this.errorTextoNombreLegal = i18n.t("valid.NombreInversionistaNatural");

            } else if (this.VerificarLetrasSeguidas(this.registro.asociadoActual.name)) {

                this.errorNombreContacto = this.hasError;
                this.errorNombreContactoTexto = i18n.t("valid.max2CharRepetidos");

            } else if (!this.REOneLetter.test(this.registro.asociadoActual.name)) {
                this.errorNombreContacto = this.hasError;
                this.errorNombreContactoTexto = i18n.t("valid.nombreAlphNumer");

            } else if (this.registro.asociadoActual.name.length > 255) {
                this.errorNombreContacto = this.hasError;
                this.errorNombreContactoTexto = i18n.t("valid.maxChar");

            } else {
                this.errorNombreContacto = this.hasSuccess;
                this.errorNombreContactoTexto = '';

            }

        },
        validarTelefonoContacto() {
            var emailRegex = new RegExp(this.ExprPhone);

            if (this.registro.asociadoActual.phoneNumber == '' || this.registro.asociadoActual.phoneNumber == null) {

                if (this.principalProveedorRepresentante) {

                    this.errorTelefonoContacto = '';
                    this.errorTelefonoContactoTexto = '';
                    return
                }

                this.errorTelefonoContacto = this.hasError;
                this.errorTelefonoContactoTexto = i18n.t("valid.telefonoRequerido");


            } else if (this.registro.asociadoActual.phoneNumber.length < 4) {

                this.errorTelefonoContacto = this.hasError;
                this.errorTelefonoContactoTexto = i18n.t("valid.telefonoFormatInvalid") + " " + this.placeholderTelefono;

            } else if (this.REOnlyZero.test(this.registro.asociadoActual.phoneNumber)) {

                this.errorTelefonoContacto = this.hasError;
                this.errorTelefonoContactoTexto = i18n.t("valid.docCero");

            } else if (!emailRegex.test(this.registro.asociadoActual.phoneNumber)) {

                this.errorTelefonoContacto = this.hasError;
                this.errorTelefonoContactoTexto = i18n.t("valid.telefonoFormatInvalid") + " " + this.placeholderTelefono;

            } else {
                this.errorTelefonoContacto = this.hasSuccess;
                this.errorTelefonoContactoTexto = '';
            }
        },
        validarEmailContacto() {
            let RE = /[.]{2,}/
            emailRegex = /^[\w.]{1,64}@(?:[A-Z0-9]{2,63}\.){1,125}[A-Z]{2,63}$/i;

            if (this.registro.asociadoActual.email == '' || this.registro.asociadoActual.email == null) {

                if (this.principalProveedorRepresentante) {

                    this.errorEmailContacto = '';
                    this.errorEmailContactoTexto = '';
                    return
                }

                this.errorEmailContacto = this.hasError;
                this.errorEmailContactoTexto = i18n.t("valid.emailRequerido");


            } else if (!emailRegex.test(this.registro.asociadoActual.email) || RE.test(this.registro.asociadoActual.email)) {

                this.errorEmailContacto = this.hasError;
                this.errorEmailContactoTexto = i18n.t("valid.emailFormatoInvalido");

            } else if (this.registro.asociadoActual.email.length > 60) {

                this.errorEmailContacto = this.hasError;
                this.errorEmailContactoTexto = i18n.t("valid.emailMaxChar");

            } else {
                this.errorEmailContacto = this.hasSuccess;
                this.errorEmailContactoTexto = '';
            }

        },
        //--------------------------------- Operaciones CRUD del Asociado --------------------------
        seleccionAsociado(asociado, index) {

            this.registro.asociadoActual = JSON.parse(JSON.stringify(asociado));
            this.dialogAsociado = true;

            this.errorDocumentoAsociado = this.hasSuccess;
            this.errorNombreLegalAsociado = this.hasSuccess;
            this.errorNombreContacto = this.hasSuccess;
            this.errorEmailContacto = this.hasSuccess;
            this.errorTelefonoContacto = this.hasSuccess;

            this.selectAsociado = index;
        },
        borrarAsociado(index) {
            this.registro.registrarse.proveedores.splice(index, 1);
            this.guardarActualizacion(this.registro);
        },
        guardarAsociado() {

            if (this.selectAsociado == 999) {

                if (this.registro.user.participant == 'DEBTOR') {
                    this.registro.registrarse.suppliers.push({
                        identification: this.registro.asociadoActual.identification,
                        prefix: this.registro.asociadoActual.prefix,
                        number: this.registro.asociadoActual.number,
                        company: this.registro.asociadoActual.company,
                        name: this.registro.asociadoActual.name,
                        email: this.registro.asociadoActual.email,
                        phoneNumber: this.registro.asociadoActual.phoneNumber,
                        person: this.registro.asociadoActual.person
                    });
                }
                else if (this.registro.user.participant == 'SUPPLIER') {
                    this.registro.registrarse.customers.push({
                        identification: this.registro.asociadoActual.identification,
                        prefix: this.registro.asociadoActual.prefix,
                        number: this.registro.asociadoActual.number,
                        company: this.registro.asociadoActual.company,
                        name: this.registro.asociadoActual.name,
                        email: this.registro.asociadoActual.email,
                        phoneNumber: this.registro.asociadoActual.phoneNumber,
                        person: this.registro.asociadoActual.person,
                    });
                }
                //-----Resetiar input de Cuentas -----------Poner en Metodo
                this.reseteo();

            } else {

                if (this.registro.registrarse.suppliers.length > 0) {
                    this.registro.registrarse.suppliers[this.selectAsociado].number = this.registro.asociadoActual.number;
                    this.registro.registrarse.suppliers[this.selectAsociado].company = this.registro.asociadoActual.company;
                    this.registro.registrarse.suppliers[this.selectAsociado].name = this.registro.asociadoActual.name;
                    this.registro.registrarse.suppliers[this.selectAsociado].email = this.registro.asociadoActual.email;
                    this.registro.registrarse.suppliers[this.selectAsociado].phoneNumber = this.registro.asociadoActual.phoneNumber;
                    this.registro.registrarse.suppliers[this.selectAsociado].person = this.registro.asociadoActual.person;
                }
                else if (this.registro.registrarse.customers.length > 0) {
                    this.registro.registrarse.customers[this.selectAsociado].number = this.registro.asociadoActual.number;
                    this.registro.registrarse.customers[this.selectAsociado].company = this.registro.asociadoActual.company;
                    this.registro.registrarse.customers[this.selectAsociado].name = this.registro.asociadoActual.name;
                    this.registro.registrarse.customers[this.selectAsociado].email = this.registro.asociadoActual.email;
                    this.registro.registrarse.customers[this.selectAsociado].phoneNumber = this.registro.asociadoActual.phoneNumber;
                    this.registro.registrarse.customers[this.selectAsociado].person = this.registro.asociadoActual.person;
                    //this.registro.registrarse.customers[this.selectAsociado] = JSON.parse(JSON.stringify(this.registro.asociadoActual));
                }

                this.reseteo();
            }

            console.log(this.registro.registrarse.suppliers)
            console.log(this.registro.registrarse.customers)
            this.guardarActualizacion(this.registro);
            this.reseteo();
        },
        reseteo() {

            this.blockAsociado = false;
            this.dialogAsociado = false;
            this.registro.asociadoActual.number = '';
            this.registro.asociadoActual.company = '';
            this.registro.asociadoActual.person = null;
            this.registro.asociadoActual.name = '';
            this.registro.asociadoActual.email = '';
            this.registro.asociadoActual.phoneNumber = '';

            this.selectAsociado = 999;

            this.errorDocumentoAsociado = '';
            this.errorNombreLegalAsociado = '';
            this.errorNombreContacto = '';
            this.errorTelefonoContacto = '';
            this.errorEmailContacto = '';

            //texto
            this.errorDocumentoAsociadoTexto = ''
            this.errorNombreLegalAsociadoTexto = ''
            this.errorNombreContactoTexto = '';
            this.errorTelefonoContactoTexto = '';
            this.errorEmailContactoTexto = '';

        },

        rellenarContrato(contrato) {
            console.log(contrato)
            this.agreementsContratoMarco = contrato;

            if (this.registro.dataPaises != null) {

                if (this.registro.dataPaises.entities != null) {
                    this.registro.dataPaises.entities.map(data => {

                        if (data.id == contrato.entity) {
                            this.BancoSelected = data.person.name;
                            return
                        }
                    });
                }
            }
            
        },
        async AsociadoExistente() {

            this.comprobarDocA.document.identification = this.registro.asociadoActual.identification;
            this.comprobarDocA.document.prefix = this.registro.asociadoActual.prefix;
            this.comprobarDocA.document.number = this.registro.asociadoActual.number;

            let data = await axios.post('../Identity/Account/Register?handler=VerificarDocActualizar', this.comprobarDocA, { headers: { "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val() } })
                .then((respond) => {
                    resetTime();
                    return respond.data;
                }).catch(function (error) {

                });

            if (data != null) {

                console.log(data)
                this.registro.asociadoActual = data;
                if (this.registro.asociadoActual.company != null && this.registro.asociadoActual.company != '') {

                    this.errorNombreLegalAsociado = this.hasSuccess;
                    this.errorNombreLegalAsociadoTexto = null;
                }
                if (this.registro.asociadoActual.name != null && this.registro.asociadoActual.name != '') {

                    this.errorNombreContacto = this.hasSuccess;
                    this.errorNombreContactoTexto = null;
                }
                if (this.registro.asociadoActual.email != null && this.registro.asociadoActual.email != '') {

                    this.errorEmailContacto = this.hasSuccess;
                    this.errorEmailContactoTexto = null;
                }
                if (this.registro.asociadoActual.phoneNumber != null && this.registro.asociadoActual.phoneNumber != '') {

                    this.errorTelefonoContacto = this.hasSuccess;
                    this.errorTelefonoContactoTexto = null;
                }
                this.blockAsociado = true;
                this.blockAsociadoRegistrado = true;

            }
            else {
                this.blockAsociado = false;
                this.blockAsociadoRegistrado = false;
                this.errorNombreLegalAsociado = '';
                this.errorNombreLegalAsociadoTexto = '';
                this.registro.asociadoActual.company = null;

                this.errorNombreContacto = '';
                this.errorNombreContactoTexto = '';
                this.registro.asociadoActual.name = null;

                this.errorTelefonoContacto = '';
                this.errorTelefonoContactoTexto = '';
                this.registro.asociadoActual.phoneNumber = null;

                this.errorEmailContacto = '';
                this.errorEmailContactoTexto = '';
                this.registro.asociadoActual.email = null;
            }

            if (this.registro.registrarse.customers != null) {
                if (this.registro.registrarse.customers.length > 0) {
                    for (let i = 0; i < this.registro.registrarse.customers.length; i++) {
                        if (this.registro.registrarse.customers[i].number == this.registro.asociadoActual.number) {
                            this.errorDocumentoAsociado = this.hasError;
                            this.errorDocumentoAsociadoTexto = i18n.t("valid.numeroDocDuplicado");
                        }
                    }
                }
            }
            else if (this.registro.registrarse.suppliers != null) {
                if (this.registro.registrarse.suppliers.length > 0) {
                    for (let i = 0; i < this.registro.registrarse.suppliers.length; i++) {
                        if (this.registro.registrarse.suppliers[i].number == this.registro.asociadoActual.number) {
                            this.errorDocumentoAsociado = this.hasError;
                            this.errorDocumentoAsociadoTexto = i18n.t("valid.numeroDocDuplicado");
                        }
                    }
                }

            } else { this.errorDocumentoAsociado = this.hasSuccess; this.errorDocumentoAsociadoTexto = null; }

        },
        async CiudadSelected(State_id) {

            let Region = {
                State: State_id
            }

            var data = await axios.post('ActualizarEmpresa?handler=SelectCity', Region, { headers: { "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val() } })
                .then((respond) => {
                    resetTime();
                    return respond.data;

                }).catch(function (error) {

                    console.log("entro en el catch: ");
                    console.log(error);
                });

            this.listCities = data;
            if (this.listCities != null) this.listCities.sort((a, b) => a.name < b.name ? -1 : + (a.name > b.name));
        },
        guardarActualizacion(registro) {
            this.ContenidoParaMandar(registro, "");
        },
        async ContenidoParaMandar(registro, ruta) {
            rgj = JSON.parse(JSON.stringify(registro));
            if (rgj.registrarse.discriminator == 'LEGAL') {
                rgj.registrarse.email = null;
                rgj.registrarse.firstName = null;
                rgj.registrarse.lastName = null;
                rgj.registrarse.phone.label = 'MAIN';
            }
            else {
                rgj.registrarse.email.label = "MAIN";
                rgj.registrarse.company = null;
                rgj.registrarse.lastName = '';
                rgj.registrarse.phone.label = 'MAIN';
            }

            if (rgj.user.participant == 'DEBTOR' || rgj.user.participant == 'SUPPLIER') {
                // Regsitro Empresa, Proveedor, Banco registrando Empresa
                rgj.representante.label = 'LEGAL';
                if ((rgj.contacto.name != null && rgj.contacto.name != '') &&
                    (rgj.contacto.documentNumber != null && rgj.contacto.documentNumber != '') &&
                    (rgj.contacto.phoneNumber != null && rgj.contacto.phoneNumber != '') &&
                    (rgj.contacto.email != null && rgj.contacto.email != '')) {

                    rgj.contacto.label = 'CONTACT';
                    rgj.registrarse.contacts.push(rgj.contacto);
                }
                if (rgj.user.participant == 'SUPPLIER') rgj.registrarse.suppliers = null;
                if (rgj.user.participant == 'DEBTOR') rgj.registrarse.customers = null;
                if (rgj.registrarse.accounts.length == 0) rgj.registrarse.accounts = null;
                rgj.registrarse.contacts.push(rgj.representante);

            } else if (rgj.user.participant == 'FACTOR' && rgj.registrarse.discriminator == 'LEGAL') {
                // Regsitro Factor LEGAL
                rgj.registrarse.contacts.push(rgj.representante);
                rgj.registrarse.suppliers = null;
                rgj.registrarse.customers = null;
                if (rgj.registrarse.accounts.length == 0) rgj.registrarse.accounts = null;

            } else if (rgj.user.participant == 'FACTOR' && rgj.registrarse.discriminator == 'PRSON') {
                // Regsitro Factor PEOPLE
                rgj.registrarse.suppliers = null;
                rgj.registrarse.customers = null;
                if (rgj.registrarse.accounts.length == 0) rgj.registrarse.accounts = null;

            } else if (rgj.user.participant == 'CONFIRMANT') {
                // Operativo TuFactoring
                rgj.representante.label = 'LEGAL';
                rgj.registrarse.contacts.push(rgj.representante);
                rgj.administrador.label = 'ADMINISTRATOR'
                rgj.registrarse.contacts.push(rgj.administrador);
                rgj.registrarse.accounts = null;
                rgj.registrarse.participant = 'CONFIRMANT';
                rgj.registrarse.suppliers = null;
                rgj.registrarse.customers = null;
            }

            if (rgj.registrarse.customers != null) {
                let copyCustomers = JSON.parse(JSON.stringify(rgj.registrarse.customers));
                let id_person = null;
                rgj.registrarse.customers = [];
                for (let i = 0; i < copyCustomers.length; i++) {

                    if (copyCustomers[i].person != null) {
                        if (copyCustomers[i].person.id != null) {
                            id_person = copyCustomers[i].person.id;

                        } else {
                            id_person = copyCustomers[i].person;
                        }
                    }
                    rgj.registrarse.customers.push({
                        id: copyCustomers[i].id,
                        name: copyCustomers[i].name,
                        company: copyCustomers[i].company,
                        number: copyCustomers[i].number,
                        identification: copyCustomers[i].identification,
                        prefix: copyCustomers[i].prefix,
                        phoneNumber: copyCustomers[i].phoneNumber,
                        email: copyCustomers[i].email,
                        state: copyCustomers[i].state,
                        invited: copyCustomers[i].invited,
                        person: id_person,
                    });
                }            
            }
            if (rgj.registrarse.suppliers != null) {
                let copySuppliers = JSON.parse(JSON.stringify(rgj.registrarse.suppliers));
                let id_person = null;
                rgj.registrarse.suppliers = [];
                for (let i = 0; i < copySuppliers.length; i++) {

                    if (copySuppliers[i].person != null) {
                        if (copySuppliers[i].person.id != null) {
                            id_person = copySuppliers[i].person.id;

                        } else {
                            id_person = copySuppliers[i].person;
                        }
                    }
                    rgj.registrarse.suppliers.push({
                        id: copySuppliers[i].id,
                        name: copySuppliers[i].name,
                        company: copySuppliers[i].company,
                        number: copySuppliers[i].number,
                        identification: copySuppliers[i].identification,
                        prefix: copySuppliers[i].prefix,
                        phoneNumber: copySuppliers[i].phoneNumber,
                        email: copySuppliers[i].email,
                        state: copySuppliers[i].state,
                        invited: copySuppliers[i].invited,
                        person: id_person,
                    });
                }
            }
            let registroGuardar = rgj.registrarse;

            var data = await axios.post(ruta, registroGuardar, { headers: { "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val() } })
                .then((respond) => {
                    resetTime();
                    console.log(respond.data)

                    if (respond.data == null) toastr.error("Se han presentado inconvenientes al actualizar la información, por favor verifique e intente nuevamente");

                    if (respond.data.error == null) {
                        toastr.success("<div class='text-justify'>" + i18n.t("tooltip.queridoUsuario") + "<br /><br />" + i18n.t("tooltip.actualizarPerfil") + "</div>");
                        return respond.data.person;
                    }
                    else if (respond.data.error == "You are not authorised to perform this action") toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br />" + i18n.t("tooltip.noAuthorizado"));
                    else if (respond.data.error == "internal system error") toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br />" + i18n.t("tooltip.internalError"));
                    else if (respond.data.error.includes("DisplayNumber") == true) toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br />" + i18n.t("tooltip.formatoDocumento"));
                    else if (respond.data.error.includes("checkAccountNumber") == true) toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br />" + i18n.t("tooltip.formatoCuentaBancaria"));
                    else toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br />" + respond.data.error);
                    return respond.data.person;

                }).catch((error) => {
                    console.log(error)
                    if (typeof error === 'string' || error instanceof String) {

                        if (error.includes("<!DOCTYPE html>")) {
                            window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired";
                            toastr.error("Se han presentado inconvenientes al actualizar la información, por favor verifique e intente nuevamente");
                            return;
                        }
                    }
                    toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br />" + i18n.t("errorConexion"));
                });

            if (typeof data === 'string' || data instanceof String) {
                console.log(data);
                if (data.includes("<!DOCTYPE html>")) {
                    window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired";
                    toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br />" + i18n.t("errorBaseDatos"));
                    return;
                }
            }

            if (data == null) {
                this.resetearRegistro = null;
            }
            else {
                this.registro = data;
                this.Inicio();
            }
        },

        //En Construccion -------------------------------------
        validacionBotonGuardar() {

            //Direciones
            if (this.registro.registrarse.address.line1 == '' || this.registro.registrarse.address.line1 == null || this.registro.registrarse.address.line1[0] == ' ') {

                this.errorDireccion = this.hasError;
                this.errorTextoDireccion = i18n.t("valid.direccionPrincipalRequerida");
            }
            if (this.registro.registrarse.phone.number == '' || this.registro.registrarse.phone.number == null) {

                this.errorTelefonoDirecciones = this.hasError;
                this.errorTextoTelefonoDirecciones = i18n.t("valid.telefonoRequerido");
            }
            //Fin Direciones

            //Representante Legal
            if (this.registro.representante.documentNumber == '' || this.registro.representante.documentNumber == null) {

                this.errorDocumentoRepresentanteLegal = this.hasError;
                this.errorTextoDocumentoRepresentanteLegal = i18n.t("valid.docRequerido");
            }
            if (this.registro.representante.name == '' ||this.registro.representante.name == null || this.registro.representante.name[0] == ' ') {

                this.errorNombresRepresentanteLegal = this.hasError;
                this.errorTextoNombresRepresentanteLegal = i18n.t("valid.nombreRequerido");
            }
            if (this.registro.representante.phoneNumber == '' || this.registro.representante.phoneNumber == null) {
                this.errorTelefonoRepresentante = this.hasError;
                this.errorTextoTelefonoRepresentante = i18n.t("valid.telefonoRequerido");
            }
            if (this.registro.representante.email == '' || this.registro.representante.email == null) {

                this.errorEmailRepresentante = this.hasError;
                this.errorTextoEmailRepresentante = i18n.t("valid.emailRequerido");
            }
            //Fin del Representante

            //Persona de Contacto
            if (this.registro.contacto.documentNumber == '' || this.registro.contacto.documentNumber == null) {

                this.errorDocumentoAsociadoContactoPrincipal = this.hasError;
                this.errorDocumentoAsociadoContactoTextoPrincipal = i18n.t("valid.docRequerido");
            }
            if (this.registro.contacto.name == '' || this.registro.contacto.name == null || this.registro.contacto.name[0] == ' ') {

                this.errorNombreContactoPrincipal = this.hasError;
                this.errorNombreContactoTextoPrincipal = i18n.t("valid.nombreRequerido");
            }
            if (this.registro.contacto.phoneNumber == '' || this.registro.contacto.phoneNumber == null) {

                this.errorTelefonoContactoPrincipal = this.hasError;
                this.errorTelefonoContactoTextoPrincipal = i18n.t("valid.telefonoRequerido");
            }
            if (this.registro.contacto.email == '' || this.registro.contacto.email == null) {

                this.errorEmailContactoPrincipal = this.hasError;
                this.errorEmailContactoTextoPrincipal = i18n.t("valid.emailRequerido");
            }
            //Fin del Contacto


        },

        async ToggleInvitation(asociado) {

            let invitado = { invitation: asociado.id }
            let data = await axios.post('ActualizarEmpresa?handler=ToggleInvitation', invitado, { headers: { "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val() } })
                .then((respond) => {
                    resetTime();
                    if (respond.data == null) toastr.error("Se han presentado inconvenientes al actualizar la información, por favor verifique e intente nuevamente");
                    
                    else toastr.success("<div class='text-justify'>" + i18n.t("tooltip.queridoUsuario") + "<br /><br />" + i18n.t("tooltip.actualizarPerfil") + "</div>");

                    return respond.data;
                }).catch(function (error) {

                });

            if (data == null) {
                this.resetearRegistro = null;

            }
            else {
                this.registro = data;
                this.Inicio();
            }
        },
        async CancelInvitation(asociado) {

            let invitado = { invitation: asociado.id }
            let data = await axios.post('ActualizarEmpresa?handler=CancelInvitation', invitado, { headers: { "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val() } })
                .then((respond) => {
                    resetTime();
                    if (respond.data == null) toastr.error("Se han presentado inconvenientes al actualizar la información, por favor verifique e intente nuevamente");
                    else toastr.success("<div class='text-justify'>" + i18n.t("tooltip.queridoUsuario") + "<br /><br />" + i18n.t("tooltip.actualizarPerfil") + "</div>");

                    return respond.data;
                }).catch(function (error) {

                });

            if (data == null) {
                this.resetearRegistro = null;

            }
            else {
                this.registro = data;
                this.Inicio();
            }
        },
        async AcceptInvitation(asociado) {

            let invitado = { invitation: asociado.id }
            let data = await axios.post('ActualizarEmpresa?handler=AcceptInvitation', invitado, { headers: { "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val() } })
                .then((respond) => {
                    resetTime();
                    if (respond.data == null) toastr.error("Se han presentado inconvenientes al actualizar la información, por favor verifique e intente nuevamente");
                    else toastr.success("<div class='text-justify'>" + i18n.t("tooltip.queridoUsuario") + "<br /><br />" + i18n.t("tooltip.actualizarPerfil") + "</div>");

                    return respond.data;
                }).catch(function (error) {

                });

            if (data == null) {
                this.resetearRegistro = null;

            }
            else {
                this.registro = data;
                this.Inicio();
            }
        },
        async RejectInvitation(asociado) {

            let invitado = { invitation: asociado.id }
            let data = await axios.post('ActualizarEmpresa?handler=RejectInvitation', invitado, { headers: { "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val() } })
                .then((respond) => {
                    resetTime();
                    if (respond.data == null) toastr.error("Se han presentado inconvenientes al actualizar la información, por favor verifique e intente nuevamente");
                    else toastr.success("<div class='text-justify'>" + i18n.t("tooltip.queridoUsuario") + "<br /><br />" + i18n.t("tooltip.actualizarPerfil") + "</div>");

                    return respond.data;
                }).catch(function (error) {

                });

            if (data == null) {
                this.resetearRegistro = null;

            }
            else {
                this.registro = data;
                this.Inicio();
            }
        },

        async AceptacionContrato() {

            this.agreementsContratoMarco.accepted = true;
            let data = await axios.post('ActualizarEmpresa?handler=Contrato', this.agreementsContratoMarco, { headers: { "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val() } })
                .then((respond) => {
                    resetTime();
                    if (respond.data == null) toastr.error("Se han presentado inconvenientes al actualizar la información, por favor verifique e intente nuevamente");
                    else toastr.success("<div class='text-justify'>" + i18n.t("tooltip.queridoUsuario") + "<br /><br />" + i18n.t("tooltip.actualizarPerfil") + "</div>");

                    return respond.data;
                }).catch(function (error) {

                });

            if (data == null) {
                this.resetearRegistro = null;
            }
            else {
                this.registro = data;
                this.Inicio();
            }

            this.contratoProveedor = false;
        },
    },
    computed: {
        //...Vuex.mapState(['respuestaPerfil', 'desicionRegistro', 'registroExistente', 'respuestaFormulario', 'respuestaComprobacionDoc', 'respuestaComprobacionDocError', 'respuestaComprobacionDocErrorTexto']),
        cambiarCiudad() {
            if (this.idCiudad == 0) this.tamanioDiv = "col-md-6"
            else this.tamanioDiv = "col-md-4"

            return this.idCiudad;
        },
        habilitarGuardarCuentas() {

            var noPasar = true;

            if (this.picked != 'Gestion de Cuenta' && this.idBanco == this.hasSuccess) {

                this.registro.cuentaActual.accountType = '';
                this.registro.cuentaActual.name_on_account = '';
                this.registro.cuentaActual.accountNumber = '';
                this.registro.cuentaActual.currency = '';

                this.tipoCuentaBanco = '';
                this.tipoMonedaBanco = '';
                this.idBancoTexto = '';
                this.tipoCuentaBancoTexto = '';
                this.tipoMonedaBancoTexto = '';
                this.TituluarBancoTexto = '';
                this.numeroCuentaBanco = '';
                this.numeroCuentaBancoTexto = '';

                noPasar = false;

            } else {

                if (this.idBanco == this.hasSuccess &&
                    this.tipoMonedaBanco == this.hasSuccess &&
                    this.tipoCuentaBanco == this.hasSuccess &&
                    this.TituluarBanco == this.hasSuccess &&
                    this.numeroCuentaBanco == this.hasSuccess) {

                    noPasar = false;

                } else {
                    noPasar = true;
                }
            }

            return noPasar;
        },
        habilitarGuardarAsociado() {

            if (this.errorDocumentoAsociado == this.hasSuccess &&
                this.errorNombreLegalAsociado == this.hasSuccess) {

                if (this.errorNombreContacto == this.hasSuccess &&
                    this.errorTelefonoContacto == this.hasSuccess &&
                    this.errorEmailContacto == this.hasSuccess) {

                    return false;

                } else {
                    return true;
                }

            } else {
                return true;
            }
        },
        habilitarbotonFormulario() {

            this.registro.registrarse.participant = this.registro.user.participant;
            if (this.registro.user.participant == 'FACTOR') {

                if (this.registro.user.discriminator == 'LEGAL') {

                    if (this.registro.registrarse.document.number != null && this.registro.registrarse.document.number != "" &&
                        this.registro.registrarse.company != null && this.registro.registrarse.company != "" &&
                        this.registro.registrarse.category != null && this.registro.registrarse.category != "" &&

                        this.registro.representante.documentNumber != null && this.registro.representante.documentNumber != "" &&
                        this.registro.representante.name != null && this.registro.representante.name != "" &&
                        this.registro.representante.email != null && this.registro.representante.email != "" &&
                        this.registro.representante.phoneNumber != null && this.registro.representante.phoneNumber != "" &&

                        this.errorDocumentoRepresentanteLegal == this.hasSuccess &&
                        this.errorNombresRepresentanteLegal == this.hasSuccess &&
                        this.errorTelefonoRepresentante == this.hasSuccess &&
                        this.errorEmailRepresentante == this.hasSuccess &&
                        this.errorDireccion == this.hasSuccess &&
                        (this.errorDireccion2 == '' || this.errorDireccion2 == this.hasSuccess) &&
                        this.errorCiudad == this.hasSuccess &&
                        this.errorTelefonoDirecciones == this.hasSuccess && 
                        this.registro.registrarse.accounts.length > 0) {

                        return false;

                    } else {

                        return true;
                    }

                } else {

                    if (this.registro.registrarse.document.number != null && this.registro.registrarse.document.number != "" &&
                        this.registro.registrarse.firstName != null && this.registro.registrarse.firstName != "" &&
                        this.registro.registrarse.category != null && this.registro.registrarse.category != "" &&

                        this.errorDireccion == this.hasSuccess &&
                        (this.errorDireccion2 == '' || this.errorDireccion2 == this.hasSuccess) &&
                        this.errorCiudad == this.hasSuccess &&
                        this.errorTelefonoDirecciones == this.hasSuccess &&
                        this.registro.registrarse.accounts.length > 0) {

                        return false;

                    } else {

                        return true;
                    }
                }
            }
            else if (this.registro.user.participant == 'DEBTOR' || this.registro.user.participant == 'SUPPLIER') {

                if (this.registro.registrarse.document.number != null && this.registro.registrarse.document.number != "" &&
                    this.registro.registrarse.company != null && this.registro.registrarse.company != "" &&
                    this.registro.registrarse.category != null && this.registro.registrarse.category != "" &&

                    this.registro.representante.documentNumber != null && this.registro.representante.documentNumber != "" &&
                    this.registro.representante.name != null && this.registro.representante.name != "" &&
                    this.registro.representante.email != null && this.registro.representante.email != "" &&
                    this.registro.representante.phoneNumber != null && this.registro.representante.phoneNumber != "" &&

                    this.errorDocumentoRepresentanteLegal == this.hasSuccess &&
                    this.errorNombresRepresentanteLegal == this.hasSuccess &&
                    this.errorTelefonoRepresentante == this.hasSuccess &&
                    this.errorEmailRepresentante == this.hasSuccess &&
                    this.errorDireccion == this.hasSuccess &&
                    (this.errorDireccion2 == '' || this.errorDireccion2 == this.hasSuccess) &&
                    this.errorCiudad == this.hasSuccess &&
                    this.errorTelefonoDirecciones == this.hasSuccess &&
                    this.registro.registrarse.accounts.length > 0) {

                    if (this.errorDocumentoAsociadoContactoPrincipal == this.hasSuccess) {

                        if (this.errorNombreContactoPrincipal == this.hasError || this.errorNombreContactoPrincipal == "" || this.errorNombreContactoPrincipal == null ||
                            this.errorTelefonoContactoPrincipal == this.hasError || this.errorTelefonoContactoPrincipal == "" || this.errorTelefonoContactoPrincipal == null ||
                            this.errorEmailContactoPrincipal == this.hasError || this.errorEmailContactoPrincipal == "" || this.errorEmailContactoPrincipal == null) {

                            return true;
                        }
                    }
                    return false;

                } else {

                    return true;
                }
            }
            else if (this.registro.user.participant == 'CONFIRMANT') {

                if (this.registro.registrarse.document.number != null && this.registro.registrarse.document.number != "" &&
                    this.registro.registrarse.company != null && this.registro.registrarse.company != "" &&
                    this.registro.registrarse.category != null && this.registro.registrarse.category != "" &&

                    this.registro.representante.documentNumber != null && this.registro.representante.documentNumber != "" &&
                    this.registro.representante.name != null && this.registro.representante.name != "" &&
                    this.registro.representante.email != null && this.registro.representante.email != "" &&
                    this.registro.representante.phoneNumber != null && this.registro.representante.phoneNumber != "" &&

                    this.errorDocumentoRepresentanteLegal == this.hasSuccess &&
                    this.errorNombresRepresentanteLegal == this.hasSuccess &&
                    this.errorTelefonoRepresentante == this.hasSuccess &&
                    this.errorEmailRepresentante == this.hasSuccess &&
                    this.errorDocumentoAdministrador == this.hasSuccess &&
                    this.errorNombreAdministrador == this.hasSuccess &&
                    this.errorTelefonoAdministrador == this.hasSuccess &&
                    this.errorEmailAdministrador == this.hasSuccess &&
                    (this.errorDireccion2 == '' || this.errorDireccion2 == this.hasSuccess) &&
                    this.errorDireccion == this.hasSuccess &&
                    this.errorCiudad == this.hasSuccess &&
                    this.errorTelefonoDirecciones == this.hasSuccess) {

                    return false;
                } else {

                    return true;
                }
            }

        },
        resultadoLogo() {
            return this.logo;
        },
        mensajesComputed() {
            return store.state.mensajes
        }
    },
    mounted: async function () {
        tiempoLogin(this.modalLogout)
    }
});
