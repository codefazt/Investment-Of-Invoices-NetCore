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
        tiempoAusente: tiempoAusente,
        langActual: langActual,
        maxlegthTelefono: null,
        maxlegthCuentaBancaria: null,
        placeholderCuentaBancaria: null,
        placeholderTelefono: null,
        dialogInversionista: false,
        blockAsociadoRegistrado: false,
        boleanoDesicionRegistro: true,
        listCities: [],
        ExprAccount: null,
        ExprPhone: null,
        resetearRegistro: null,

        todoEnCero: false,
        registroEncrip: null,
        toggledSlideBar: 'toggled',
        nombreBancoProv: '',
        cargando: true,
        //tamanoTlf: tamanoTlf,
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
            { text: i18n.t("headers.acciones"), value: 'action', align: 'center' },
        ],
        headersAsociado: [

            { text: i18n.t("headers.cliente"), value: 'company' },
            { text: i18n.t("headers.numeroDoc"), value: 'number', align: 'center' },
            { text: i18n.t("headers.contacto"), value: 'name' },
            { text: i18n.t("headers.correoElectronico"), value: 'email' },
            { text: i18n.t("headers.acciones"), value: 'action', align: 'center' },

        ],
        blockAsociado: false,
        cargado: true,
        eleccion: 'EMPRESA',
        tipoDoc: 'LEGAL',
        principalProveedorRepresentante: false,
        principalProveedorContacto: false,
        boleanoDesicionDireccion2: true,
        personaContactoVacio: true,
        validateReset: false,
        //tamanioDiv: "col-md-8", esto es para la ciudad
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
        terminosYCondiciones: false,

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

        //Roles LEGAL, CONTACT, ADMINISTRATOR

        this.registro = JSON.parse(document.getElementById('contenidoRaw').value);
        console.log(this.registro);

        if (this.registro != null) {

            this.eleccionRegistro(this.registro.rol);

            if (this.registro.dataPaises == null) {
                toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br />" + "A Ocurrido un problema de conexion vuelva a intentar mas tarde.");
                setTimeout(function () { window.location.href = "../../index"; }, 3000);              
            }

            if (this.registro.dataPaises != null) {

                this.dataPaises = this.registro.dataPaises;
                if (this.dataPaises.categories != null) this.dataPaises.categories.sort((a, b) => a.name < b.name ? -1 : +(a.name > b.name))
                if (this.dataPaises.regions != null) this.dataPaises.regions.sort((a, b) => a.name < b.name ? -1 : +(a.name > b.name))
                if (this.dataPaises.entities != null) {
                    for (let i = 0; i < this.dataPaises.entities.length; i++) {
                        this.dataPaises.entities[i].person.name = this.dataPaises.entities[i].person.name.toUpperCase();
                       // console.log(this.dataPaises.entities[i].person.name)
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
        }

        if (document.getElementById('contenido') != null) document.getElementById('contenido').removeAttribute('hidden');
        document.getElementById('contenido2').removeAttribute('hidden');
        this.cargando = false;
    },
    methods: {
        resetRegistro() {

            if (this.resetearRegistro != null) {

                if (this.validateReset == false) {
                    this.vaciarCamposCambiarRegistro();

                } else {
                    this.registro.registrarse.document.prefix = '';
                    this.registro.registrarse.document.number = '';

                    if (this.registro.administrador != null) {

                        this.registro.administrador.prefix = '';
                        this.registro.administrador.documentNumber = '';
                        this.registro.administrador.name = '';
                        this.registro.administrador.phoneNumber = '';
                        this.registro.administrador.email = '';
                        this.registro.registrarse.logo = '';
                        if (document.getElementById("file") != null) document.getElementById("file").value = null;
                        this.registro.registrarse.routing_number = null;
                        this.registro.registrarse.related = null;

                        this.errorDocumentoAdministrador = '';
                        this.errorDocumentoAdministradorTexto = '';
                        this.errorNombreAdministrador = '';
                        this.errorNombreAdministradorTexto = '';
                        this.errorApellidoAdministrador = '';
                        this.errorApellidoAdministradorTexto = '';
                        this.errorTelefonoAdministrador = '';
                        this.errorTelefonoAdministradorTexto = '';
                        this.errorEmailAdministrador = '';
                        this.errorEmailAdministradorTexto = '';
                        this.errorRoutinNumber = '';
                        this.errorRoutinNumberTexto = '';
                    }
                    this.vaciarCampos();
                }
                this.errorDocumento = '';
                this.errorTextoDocumentoPrincipal = '';
                this.registro.registrarse.accounts = [];
                //this.registro.registrarse.proveedores = [];
                this.resetearRegistro = null;
                this.botonSiguiente = 0;
                return true;

            } else {
                return false;
            }
        },
        limpiarMensajes: function () {
            store.commit("limpiarMensajes")
        },
        funcionTerminosCondiciones() {

            this.terminosYCondiciones = !this.terminosYCondiciones;

            if (this.terminosYCondiciones == true) {
                this.registro.registrarse.accepted_agreement = true;
            }
            else {
                this.registro.registrarse.accepted_agreement = false;
            }
            this.dialogInversionista = false;
            this.dialog = false;
            return this.terminosYCondiciones;
            
        },
        cambioRegistroEmpresa() {

            this.registro.registrarse.documentoPrincipal.prefix = '';
            this.registro.registrarse.documentoPrincipal.number = '';
            this.errorDocumento = '';
            this.errorTextoDocumentoPrincipal = '';

            this.errorNombreLegal = '';
            this.errorTextoNombreLegal = '';
            this.registro.registrarse.legalName = '';

            this.errorNombreComercial = '';
            this.errorTextoNombreComercial = '';
            this.registro.registrarse.commercialName = '';

            this.errorOccupation = '';
            this.errorOccupationTexto = '';
            this.registro.registrarse.occupation = 0;

            this.errorPurpose = '';
            this.errorTextoPurpose = '';
            this.registro.registrarse.purpose = 0;

            this.errorEmailNatural = '';
            this.errorEmailNaturalTexto = '';
            this.registro.registrarse.email = '';

            this.registro.representante.documento.prefix = '';
            this.registro.representante.documento.number = '';
            this.errorDocumentoRepresentanteLegal = '';
            this.errorTextoDocumentoRepresentanteLegal = '';

            this.registro.representante.name = '';
            this.errorNombresRepresentanteLegal = '';
            this.errorTextoNombresRepresentanteLegal = '';

            this.registro.representante.lastName == '';
            this.errorApellidosRepresentanteLegal = '';
            this.errorTextoApellidosRepresentanteLegal = '';

            this.registro.representante.phone.number = '';
            this.errorTelefonoRepresentante = '';
            this.errorTextoTelefonoRepresentante = '';

            this.registro.representante.email = '';
            this.errorEmailRepresentante = '';
            this.errorTextoEmailRepresentante = '';

            this.errorDocumentoAsociadoPrincipal = ''
            this.errorDocumentoAsociadoTextoPrincipal = ''

            this.errorNombreLegalAsociadoPrincipal = ''
            this.errorNombreLegalAsociadoTextoPrincipal = ''

            this.TituluarBanco = '';
            this.eleccion = 'EMPRESA';
            this.comprobarDoc.tipoRegistro = 'EMPRESA';
            this.registro.registrarse.tipoRegistro = 'EMPRESA';

            if (this.errorTextoNombreLegal == i18n.t('nombreRequerido')) this.errorTextoNombreLegal = i18n.t('LegalRepRequerido');
            else if (this.errorTextoNombreLegal == i18n.t('minimo2Char')) this.errorTextoNombreLegal = i18n.t('minimoChar')

            if (this.errorTextoNombreComercial == i18n.t('apellidoRequerido')) this.errorTextoNombreComercial = 'El nombre comercial es requerido';
            else if (this.errorTextoNombreComercial == i18n.t('minimo2Char')) this.errorTextoNombreComercial = i18n.t('minimoChar');
        },
        cambioRegistroNatural() {

            this.registro.registrarse.documentoPrincipal.prefix = '';
            this.registro.registrarse.documentoPrincipal.number = '';
            this.errorDocumento = '';
            this.errorTextoDocumentoPrincipal = '';

            this.errorNombreLegal = '';
            this.errorTextoNombreLegal = '';
            this.registro.registrarse.legalName = '';

            this.errorNombreComercial = '';
            this.errorTextoNombreComercial = '';
            this.registro.registrarse.commercialName = '';

            this.errorOccupation = '';
            this.errorOccupationTexto = '';
            this.registro.registrarse.occupation = 0;

            this.errorPurpose = '';
            this.errorTextoPurpose = '';
            this.registro.registrarse.purpose = 0;

            this.errorEmailNatural = '';
            this.errorEmailNaturalTexto = '';
            this.registro.registrarse.email = '';

            this.registro.representante.documento.prefix = '';
            this.registro.representante.documento.number = '';
            this.errorDocumentoRepresentanteLegal = this.hasSuccess;
            this.errorTextoDocumentoRepresentanteLegal = '';

            this.registro.representante.name = '';
            this.errorNombresRepresentanteLegal = this.hasSuccess;
            this.errorTextoNombresRepresentanteLegal = '';

            this.registro.representante.lastName == '';
            this.errorApellidosRepresentanteLegal = this.hasSuccess;
            this.errorTextoApellidosRepresentanteLegal = '';

            this.registro.representante.phone.number = '';
            this.errorTelefonoRepresentante = this.hasSuccess;
            this.errorTextoTelefonoRepresentante = '';

            this.TituluarBanco = '';
            this.registro.representante.email = '';
            this.errorEmailRepresentante = this.hasSuccess;
            this.errorTextoEmailRepresentante = '';
            this.eleccion = 'NATURAL';
            this.comprobarDoc.tipoRegistro = 'NATURAL';
            this.comprobarDoc.participant = "FACTOR";

            this.registro.registrarse.tipoRegistro = 'NATURAL';

            if (this.errorTextoNombreLegal == i18n.t('LegalRepRequerido')) this.errorTextoNombreLegal = i18n.t('nombreRequerido');
            else if (this.errorTextoNombreLegal == i18n.t('minimoChar')) this.errorTextoNombreLegal = i18n.t('minimo2Char')

            if (this.errorTextoNombreComercial == 'El nombre comercial es requerido') this.errorTextoNombreComercial = i18n.t('apellidoRequerido');
            else if (this.errorTextoNombreComercial == i18n.t('minimoChar')) this.errorTextoNombreComercial = i18n.t('minimo2Char')

        },

        eleccionRegistro(Rol) {

            this.registro.Rol = Rol;
            this.eleccion = 'EMPRESA';
            this.tipoDoc = 'LEGAL';

            if (Rol == 1 || this.registro.user.participant == 'CONFIRMANT') {

                this.registro.registrarse.tipoRegistro = 'EMPRESA';
                this.registro.registrarse.participant = 'DEBTOR';

                this.comprobarDoc.tipoRegistro = 'EMPRESA';
                this.comprobarDoc.participant = 'DEBTOR';

                this.TitularAsociado = 'Proveedores';
            }
            else if (Rol == 2) {

                this.registro.registrarse.tipoRegistro = 'PROVEEDOR';
                this.registro.registrarse.participant = 'SUPPLIER';

                this.comprobarDoc.tipoRegistro = 'PROVEEDOR';
                this.comprobarDoc.participant = 'SUPPLIER';

                this.TitularAsociado = 'Clientes';
            }
            else if (Rol == 3) {

                this.registro.registrarse.tipoRegistro = 'INVERSIONISTA';
                this.registro.registrarse.participant = 'FACTOR';
                this.tipoDoc = 'LEGAL';
                this.comprobarDoc.tipoRegistro = 'EMPRESA';
                this.comprobarDoc.participant = 'FACTOR';
            }
            else if (Rol == 4) {

                this.registro.registrarse.tipoRegistro = 'NATURAL';
                this.registro.registrarse.participant = 'FACTOR';
                this.eleccion = 'NATURAL';
                this.tipoDoc = 'PERSON';
                this.comprobarDoc.tipoRegistro = 'NATURAL';
                this.comprobarDoc.participant = 'FACTOR';
            }
            else if (this.registro.user.participant == 'BACKOFFICE') {

                this.registro.registrarse.tipoRegistro = 'BANCO';
                this.registro.registrarse.participant = 'CONFIRMANT';

                this.comprobarDoc.tipoRegistro = 'BANCO';
                this.comprobarDoc.participant = 'CONFIRMANT';
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
        mascaraOpcion(campo, seccion) {
            if (campo != null && campo != '') {

                //boleanoDesicionRegistro 
                console.log(campo);
                console.log(seccion);
                console.log(this.boleanoDesicionRegistro);
                if (seccion == 'Contacto') this.personaContactoVacio = false;
                if (this.boleanoDesicionRegistro) this.personaContactoVacio = true;

                return this.enmascarar(campo, 3, 4);

            } else {
                if (seccion == 'Contacto') this.personaContactoVacio = true;
                return campo;
            }
        },
        enmascarar(campo, tamanioMin = 3, tamanioMax = 2) {
            return campo.substring(0, tamanioMin) + '*******' + campo.substring(campo.length - tamanioMax, campo.length);
        },
        rellenarCampos(registroExistente) {

            this.validateReset = true
            this.boleanoDesicionRegistro = false;
            this.registro.registrarse.company = this.enmascarar(registroExistente.registrarse.company);
            this.registro.registrarse.category = registroExistente.registrarse.category;
            this.registro.representante.prefix = registroExistente.representante.prefix;
            this.registro.representante.documentNumber = this.enmascarar(registroExistente.representante.documentNumber);
            this.registro.representante.name = this.enmascarar(registroExistente.representante.name);
            this.registro.representante.phoneNumber = this.enmascarar(registroExistente.representante.phoneNumber, 5);
            this.registro.representante.email = this.enmascarar(registroExistente.representante.email, 3, 9);

            this.registro.contacto.prefix = registroExistente.contacto.prefix;
            this.registro.contacto.documentNumber = this.enmascarar(registroExistente.contacto.documentNumber);
            this.registro.contacto.name = this.enmascarar(registroExistente.contacto.name);
            this.registro.contacto.phoneNumber = this.enmascarar(registroExistente.contacto.phoneNumber, 5);
            this.registro.contacto.email = this.enmascarar(registroExistente.contacto.email, 3, 9);
            this.registro.registrarse.address.line1 = this.enmascarar(registroExistente.registrarse.address.line1, 3, 5);

            if (this.registro.registrarse.company != null || this.registro.registrarse.company != '') {

                this.nombreBancoProv = registroExistente.registrarse.company
                this.TituluarBanco = this.hasSuccess;
            }

            if (registroExistente.registrarse.address.line2 != null &&
                registroExistente.registrarse.address.line2 != '') {

                this.registro.registrarse.address.line2 = this.mascaraOpcion(registroExistente.registrarse.address.line2, 'Direcciones');
                this.boleanoDesicionDireccion2 = false

                this.errorDireccion2 = this.hasSuccess;
            } else {
                this.errorDireccion2 = '';
                this.boleanoDesicionDireccion2 = true
                this.registro.registrarse.address.line2 = registroExistente.registrarse.address.line2;
            }
            this.registro.registrarse.address.region = registroExistente.registrarse.address.region;
            this.idCiudad = this.registro.registrarse.address.region;
            this.CiudadSelected(this.registro.registrarse.address.region);
            this.registro.registrarse.phone.number = this.enmascarar(registroExistente.registrarse.phone.number, 5);

            //Seccion de Errores
            this.errorNombreLegal = this.hasSuccess;
            this.errorTextoNombreLegal = '';

            this.errorNombreComercial = this.hasSuccess;
            this.errorTextoNombreComercial = '';

            this.errorPurpose = this.hasSuccess;

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

            if (this.registro.registrarse.address.line2 != '') {
                this.boleanoDesicionDireccion2 = false
            }

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

            this.errorDireccion = this.hasSuccess;
            this.errorTextoDireccion = '';

            this.errorDireccion2Texto = '';

            this.errorCiudad = this.hasSuccess;
            this.errorCiudadTexto = '';

            this.errorTelefonoDirecciones = this.hasSuccess;
            this.errorTextoTelefonoDirecciones = '';

            this.errorEstadoTexto = '';
            this.errorTextoPurpose = '';
            this.errorOccupationTexto = '';
            
        },
        vaciarCampos() {

            this.boleanoDesicionDireccion2 = true
            this.boleanoDesicionRegistro = true;

            this.nombreBancoProv = '';
            this.registro.registrarse.id = null;
            this.registro.registrarse.company = '';
            this.registro.registrarse.category = 0;

            this.registro.representante.prefix = '';
            this.registro.representante.documentNumber = '';
            this.registro.representante.name = '';
            this.registro.representante.phoneNumber = '';
            this.registro.representante.email = '';

            this.registro.contacto.prefix = '';
            this.registro.contacto.documentNumber = '';
            this.registro.contacto.name = '';
            this.registro.contacto.phoneNumber = '';
            this.registro.contacto.email = '';

            this.registro.registrarse.address.line1 = '';
            this.registro.registrarse.address.line2 = '';
            this.registro.registrarse.address.city = 0;
            this.registro.registrarse.phone.number = '';

            this.registro.administrador.prefix = '';
            this.registro.asociadoActual.prefix = '';
            this.registro.registrarse.accounts = [];
            this.listCities = [];
            this.idCiudad = 0;
            this.validateReset = false;

            //Seccion de Errores
            this.errorNombreLegal = '';
            this.errorTextoNombreLegal = '';

            this.errorNombreComercial = '';
            this.errorTextoNombreComercial = '';

            this.errorPurpose = '';
            this.errorTextoPurpose = '';

            this.errorOccupation = '';
            this.errorOccupationTexto = '';

            this.errorDocumentoRepresentanteLegal = '';
            this.errorTextoDocumentoRepresentanteLegal = '';

            this.errorNombresRepresentanteLegal = '';
            this.errorTextoNombresRepresentanteLegal = '';

            this.errorApellidosRepresentanteLegal = '';
            this.errorTextoApellidosRepresentanteLegal = '';

            this.errorTelefonoRepresentante = '';
            this.errorTextoTelefonoRepresentante = '';

            this.errorEmailRepresentante = '';
            this.errorTextoEmailRepresentante = '';

            //----- Persona de Contacto
            this.errorDocumentoAsociadoContactoPrincipal = '';
            this.errorDocumentoAsociadoContactoTextoPrincipal = '';

            this.errorNombreContactoPrincipal = '';
            this.errorNombreContactoTextoPrincipal = '';

            this.errorApellidoContactoPrincipal = '';
            this.errorApellidoContactoTextoPrincipal = '';

            this.errorTelefonoContactoPrincipal = '';
            this.errorTelefonoContactoTextoPrincipal = '';

            this.errorEmailContactoPrincipal = '';
            this.errorEmailContactoTextoPrincipal = '';
            //------------------------------

            this.errorDireccion = '';
            this.errorTextoDireccion = '';

            this.errorDireccion2 = '';
            this.errorDireccion2Texto = '';

            this.errorEstado = '';
            this.errorEstadoTexto = '';

            this.errorCiudad = '';
            this.errorCiudadTexto = '';

            this.errorTelefonoDirecciones = '';
            this.errorTextoTelefonoDirecciones = '';

        },
        vaciarCamposCambiarRegistro() {

            this.boleanoDesicionDireccion2 = true
            this.boleanoDesicionRegistro = true;
            this.nombreBancoProv = '';
            this.registro.registrarse.id = null;
            this.registro.registrarse.document.prefix = '';
            this.registro.registrarse.document.number = '';
            this.registro.registrarse.company = '';
            this.registro.registrarse.firstName = '';
            this.registro.registrarse.lastName = null;
            this.registro.registrarse.category = 0;
            this.registro.registrarse.email.label = 'MAIN'; 
            this.registro.registrarse.email.address = null;
            this.registro.registrarse.phone.number = '';

            this.registro.representante.label = 'LEGAL';
            this.registro.representante.prefix = '';
            this.registro.representante.documentNumber = '';
            this.registro.representante.name = '';
            this.registro.representante.phoneNumber = '';
            this.registro.representante.email = '';

            this.registro.contacto.prefix = '';
            this.registro.contacto.documentNumber = '';
            this.registro.contacto.name = '';
            this.registro.contacto.phoneNumber = '';
            this.registro.contacto.email = '';

            this.registro.administrador.prefix = '';
            this.registro.administrador.documentNumber = '';
            this.registro.administrador.name = '';
            this.registro.administrador.phoneNumber = '';
            this.registro.administrador.email = '';

            if (document.getElementById("file") != null) document.getElementById("file").value = null;
            this.registro.registrarse.routing_number = null;
            this.registro.registrarse.related = null;

            this.registro.registrarse.address.label = 'LEGAL';
            this.registro.registrarse.address.line1 = '';
            this.registro.registrarse.address.line2 = '';
            this.idCiudad = 0;
            this.registro.registrarse.address.city = 0;

            this.registro.registrarse.accounts = [];
            this.registro.registrarse.suppliers = [];
            this.registro.registrarse.customers = [];
            this.registro.nombresBancos = [];

            //Seccion de Errores
            this.errorDocumento = '';
            this.errorTextoDocumentoPrincipal = '';

            this.errorNombreLegal = '';
            this.errorTextoNombreLegal = '';

            this.errorNombreComercial = '';
            this.errorTextoNombreComercial = '';

            this.errorPurpose = '';
            this.errorTextoPurpose = '';

            this.errorOccupation = '';
            this.errorOccupationTexto = '';

            this.errorEmailNatural = '';
            this.errorEmailNaturalTexto = '';
            this.errorFechaNacimiento = '';
            this.errorFechaNacimientoTexto = '';

            this.errorDocumentoRepresentanteLegal = '';
            this.errorTextoDocumentoRepresentanteLegal = '';

            this.errorNombresRepresentanteLegal = '';
            this.errorTextoNombresRepresentanteLegal = '';

            this.errorApellidosRepresentanteLegal = '';
            this.errorTextoApellidosRepresentanteLegal = '';

            this.errorTelefonoRepresentante = '';
            this.errorTextoTelefonoRepresentante = '';

            this.errorEmailRepresentante = '';
            this.errorTextoEmailRepresentante = '';

            //----- Persona de Contacto
            this.errorDocumentoAsociadoContactoPrincipal = '';
            this.errorDocumentoAsociadoContactoTextoPrincipal = '';

            this.errorNombreContactoPrincipal = '';
            this.errorNombreContactoTextoPrincipal = '';

            this.errorApellidoContactoPrincipal = '';
            this.errorApellidoContactoTextoPrincipal = '';

            this.errorTelefonoContactoPrincipal = '';
            this.errorTelefonoContactoTextoPrincipal = '';

            this.errorEmailContactoPrincipal = '';
            this.errorEmailContactoTextoPrincipal = '';
            //------------------------------

            this.errorDocumentoAdministrador = '';
            this.errorDocumentoAdministradorTexto = '';

            //Validaciones Nombre Administrador
            this.errorNombreAdministrador = '';
            this.errorNombreAdministradorTexto = '';

            //Validaciones Apellido Administrador
            this.errorApellidoAdministrador = '';
            this.errorApellidoAdministradorTexto = '';

            //Validaciones Telefono Administrador
            this.errorTelefonoAdministrador = '';
            this.errorTelefonoAdministradorTexto = '';

            //Validaciones Email del Administrador
            this.errorEmailAdministrador = '';
            this.errorEmailAdministradorTexto = '';

            // -----------------------------
            this.errorDireccion = '';
            this.errorTextoDireccion = '';

            this.errorDireccion2 = '';
            this.errorDireccion2Texto = '';

            this.errorEstado = '';
            this.errorEstadoTexto = '';

            this.errorCiudad = '';
            this.errorCiudadTexto = '';

            this.errorTelefonoDirecciones = '';
            this.errorTextoTelefonoDirecciones = '';

            //Banco Titular
            this.TituluarBanco = '';
            this.TituluarBancoTexto = '';

            //validar Routin_number
            this.errorRoutinNumber = '';
            this.errorRoutinNumberTexto = '';

            return true;
        },
        verificarFecha() {
            var w = $("#datepicker").val().split("-")

            var dateMax = this.dateMax.split("-")
            var dateMin = this.dateMin.split("-")
            if (w[0] == undefined || w[1] == undefined || w[2] == undefined) {

                this.errorFechaNacimiento = this.hasError
                this.errorFechaNacimientoTexto = i18n.t("valid.formatoFechaNacimiento");
                return
            } else if (!moment($("#datepicker").val()).isValid()) {
                this.errorFechaNacimiento = ''
                this.errorFechaNacimientoTexto = i18n.t("valid.formatoFechaNacimiento");
                return
            } else if (w[0] == undefined || w[1] == undefined || w[2] == undefined
                || w[0] < dateMin[0] || w[0] > dateMax[0]
                || (w[0] == dateMax[0] && w[1] > dateMax[1])
                || (w[0] == dateMax[0] && w[1] == dateMax[1] && w[2] > dateMax[2])
                || (w[0] == dateMin[0] && w[1] < dateMin[1])
                || (w[0] == dateMin[0] && w[1] == dateMin[1] && w[2] < dateMin[2])) {
                this.errorFechaNacimiento = this.hasError
                this.errorFechaNacimientoTexto = i18n.t("valid.formatoFechaNacimiento");
                return
            }


            this.errorFechaNacimiento = this.hasSuccess;
            this.errorFechaNacimientoTexto = ''
        },

        validarDocumentoPrincipal(ExpreRegular, mask_edit) {

            
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
                this.errorTextoDocumentoPrincipal = i18n.t("valid.formatDoc") + " (" + mask_edit + ")";

            } else if (this.REOnlyZero.test(this.registro.registrarse.document.number)) {
                this.errorDocumento = this.hasError;
                this.errorTextoDocumentoPrincipal = i18n.t("valid.docCero");

            } else {
                this.errorDocumento = this.hasSuccess;
                this.errorTextoDocumentoPrincipal = '';

                this.comprobarDoc.document.identification = this.registro.registrarse.document.identification;
                this.comprobarDoc.document.prefix = this.registro.registrarse.document.prefix;
                this.comprobarDoc.document.number = this.registro.registrarse.document.number;

                this.VerificarRegistro(this.comprobarDoc);

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

            console.log(this.registro.registrarse.firstName)
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
                this.verificarEmail(this.registro.registrarse);
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
            console.log(this.registro.representante.prefix)
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
                this.errorTextoDocumentoRepresentanteLegal = i18n.t("valid.formatDoc") + " (" + mask_edit + ")";

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

            console.log(this.registro.contacto.prefix)
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

            if ((this.registro.contacto.documentNumber == '' ||
                this.registro.contacto.documentNumber == null ) &&
                (this.registro.contacto.prefix == '' ||
                    this.registro.contacto.prefix == null)) {

                console.log("Entro en esta otra")
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
            console.log(emailRegex);
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
                this.registro.registrarse.address.line2  == null ||
                this.registro.registrarse.address.line2 [0] == ' ') {

                this.errorDireccion2 = '';
                this.errorDireccion2Texto = '';

            } else if (emailRegex.test(this.registro.registrarse.address.line2 )) {

                this.errorDireccion2 = this.hasError;
                this.errorDireccion2Texto = i18n.t("valid.direccionAlphNumer");

            } else if (this.registro.registrarse.address.line2 .length < 15) {

                this.errorDireccion2 = this.hasError;
                this.errorDireccion2Texto = i18n.t("valid.direccionMaxChar");

            } else if (this.registro.registrarse.address.line2 .length > 255) {

                this.errorDireccion2 = this.hasError;
                this.errorDireccion2Texto = i18n.t("valid.maxChar");


            } else if (this.VerificarLetrasSeguidas(this.registro.registrarse.address.line2 )) {

                this.errorDireccion2 = this.hasError;
                this.errorDireccion2Texto = i18n.t("valid.max2CharRepetidos");

            } else {
                this.errorDireccion2 = this.hasSuccess;
                this.errorDireccion2Texto = ''

            }
        },
        validarCiudad() {

            if (this.registro.registrarse.address.city == 'Seleccione Ciudad' ||
                this.registro.registrarse.address.city== null ||
                this.registro.registrarse.address.city == '' ||
                this.registro.registrarse.address.city == 0) {

                this.errorCiudad = this.hasError;
                this.errorCiudadTexto = i18n.t("valid.seleccionCiudad");

            } else {
                this.errorCiudad = this.hasSuccess;
                this.errorCiudadTexto = '';

            }
        },
        validarCodigoPostal() {

            let emailRegex = /^[0-9\_\-\.\,\&\%\#\!\*\(\)\$\:\;\[\]\{\}\"\'\s\xF1\xD1]+$/;

            if (this.registro.registrarse.address.zipCode == '' ||
                this.registro.registrarse.address.zipCode == null ||
                this.registro.registrarse.address.zipCode[0] == ' ') {

                this.errorDireccion = this.hasError;
                this.errorTextoDireccion = i18n.t("valid.direccionPrincipalRequerida");

            } else if (emailRegex.test(this.registro.registrarse.address.zipCode)) {

                this.errorDireccion = this.hasError;
                this.errorTextoDireccion = i18n.t("valid.direccionAlphNumer");

            } else if (this.registro.registrarse.address.zipCode.length < 2) {

                this.errorDireccion = this.hasError;
                this.errorTextoDireccion = i18n.t("valid.direccionMaxChar");

            } else if (this.registro.registrarse.address.zipCode.length > 15) {

                this.errorDireccion = this.hasError;
                this.errorTextoDireccion = i18n.t("valid.maxChar");


            } else if (this.VerificarLetrasSeguidas(this.registro.registrarse.address.zipCode)) {

                this.errorDireccion = this.hasError;
                this.errorTextoDireccion = i18n.t("valid.max2CharRepetidos");

            } else {
                this.errorDireccion = this.hasSuccess;
                this.errorTextoDireccion = '';
            }
        },
        validarTelefonoDirecciones() {

            console.log(this.ExprPhone);
            let emailRegex = new RegExp(this.ExprPhone);

            if (this.registro.registrarse.phone.number == '' || this.registro.registrarse.phone.number == null) {
                this.errorTelefonoDirecciones = this.hasError;
                this.errorTextoTelefonoDirecciones = i18n.t("valid.telefonoRequerido");

            } else if (this.registro.registrarse.phone.number.length < 9 || this.registro.registrarse.phone.length == 1) {
                this.errorTelefonoDirecciones = this.hasError;
                this.errorTextoTelefonoDirecciones = i18n.t("valid.telefonoFormatInvalid") + " " + this.placeholderTelefono;

            } else if (this.registro.registrarse.phone.number.length > 20) {

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
            console.log(this.registro.administrador.prefix)

            if (this.registro.administrador.prefix == '' ||
                this.registro.administrador.prefix == 0) {

                this.errorDocumentoAdministrador = this.hasError;
                this.errorDocumentoAdministradorTexto = i18n.t("valid.docRequerido");

            } else if (this.registro.administrador.documentNumber == '' ||
                this.registro.administrador.documentNumber == null) {

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

            console.log(this.dataPaises.entities);

            if (this.registro.registrarse.routing_number == null ||
                this.registro.registrarse.routing_number == '' ||
                this.registro.registrarse.routing_number[0] == ' ') {

                this.errorRoutinNumber = this.hasError;
                this.errorRoutinNumberTexto = i18n.t("valid.codigoBank");

            } else if (this.registro.registrarse.routing_number.length < 3) {

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

            if (this.dataPaises.entities != null) {
                for (let i = 0; i < this.dataPaises.entities.length; i++) {

                    if (this.dataPaises.entities[i].routing_number == this.registro.registrarse.routing_number) {
                        this.errorRoutinNumber = this.hasError;
                        this.errorRoutinNumberTexto = i18n.t("valid.codigoBankExist");
                    }
                }
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

                        if (this.registro.registrarse.accounts[i].entity == this.registro.cuentaActual.entity &&
                            this.registro.registrarse.accounts[i].currency == this.registro.cuentaActual.currency ) {
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
                console.log(this.nombreBanco);
                this.cambioNombreBanco = true;

                console.log(this.picked)
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
        funcionGuardarCuentas(cuentaDefault = 0, CuentaEncriptada = null) {
            
            if (CuentaEncriptada != null && CuentaEncriptada != '') {

                this.registro.registrarse.accounts.push({
                    entity: this.registro.cuentaActual.entity,
                    //name_on_account: CuentaEncriptada, //this.nombreBancoProv,
                    accountType: this.registro.cuentaActual.accountType,
                    accountNumber: this.registro.cuentaActual.accountNumber,
                    currency: this.registro.cuentaActual.currency,
                    //default: cuentaDefault,
                });

            } else {

                this.registro.registrarse.accounts.push({
                    entity: this.registro.cuentaActual.entity,
                    //name_on_account: this.registro.cuentaActual.name_on_account,
                    accountType: this.registro.cuentaActual.accountType,
                    accountNumber: this.registro.cuentaActual.accountNumber,
                    currency: this.registro.cuentaActual.currency,
                    //default: cuentaDefault,
                });
            }

            console.log(this.registro.registrarse.accounts);
        },
        borrarCuenta(index) {

            if (this.registro.registrarse.accounts.length >= 2) {
                if (index == 0) {
                    //this.registro.registrarse.accounts[1].default = 1;
                }
            }
            this.registro.registrarse.accounts.splice(index, 1);
            this.registro.nombresBancos.splice(index, 1);
        },
        guardarCuenta() {

            if (this.selectCuentas == 999 && this.picked == 'Gestion de Cuenta') {

                let cuentaRepetida = true;
                for (let i = 0; i < this.registro.registrarse.accounts.length; i++) {

                    if (this.registro.registrarse.accounts[i].entity == this.registro.cuentaActual.entity &&
                        this.registro.registrarse.accounts[i].accountType == 'REQUEST') {

                        cuentaRepetida = false;
                    }
                }

                if (cuentaRepetida) {

                    if (this.registro.registrarse.accounts.length == 0) {

                        this.funcionGuardarCuentas(1, this.nombreBancoProv);

                    } else {

                        let contador = 0;

                        for (var i = 0; i < this.registro.registrarse.accounts.length; i++) {

                            if (this.registro.registrarse.accounts[i].accountType == 'REQUEST') {
                                contador++;
                            }
                        }

                        if (this.registro.registrarse.accounts.length == contador) this.funcionGuardarCuentas(1);
                        else this.funcionGuardarCuentas(0, this.nombreBancoProv);

                    }

                    this.registro.nombresBancos.push(this.nombreBanco);
                    console.log(this.nombreBanco);
                    console.log(this.registro.nombresBancos);
                    this.errorCuentasBancariasGestionCuenta = '';

                } else {
                    this.errorCuentasBancariasGestionCuenta = 'Ya has registrado la solicitud en ese banco.';
                }

                //-----Resetiar input de Cuentas -----------Poner en Metodo
                this.registro.cuentaActual.entity = '';
                this.registro.cuentaActual.name_on_account = '';
                this.registro.cuentaActual.accountType = '';
                this.registro.cuentaActual.accountNumber = '';
                this.registro.cuentaActual.currency = '';
                this.idBanco = '';
                this.tipoMonedaBanco = '';
                this.tipoCuentaBanco = '';
                this.numeroCuentaBanco = '';
                this.idBancoTexto = '';
                this.selectCuentas = 999;
                this.cambioNombreBanco = false;
                //this.nombreBanco = '';

            } else if (this.picked == 'Crear cuenta nueva') {

                let cuentaRepetida = true;
                for (let i = 0; i < this.registro.registrarse.accounts.length; i++) {

                    if (this.registro.registrarse.accounts[i].entity == this.registro.cuentaActual.entity) {

                        cuentaRepetida = false;
                    }
                }

                if (cuentaRepetida) {

                    this.registro.registrarse.accounts.push({
                        entity: this.registro.cuentaActual.entity,
                        //name_on_account: '',
                        accountType: 'REQUEST',
                        accountNumber: null,
                        //default: 0,
                    });
                    this.registro.nombresBancos.push(this.nombreBanco);
                    this.errorCuentasBancariasGestionCuenta = '';

                } else {
                    this.errorCuentasBancariasGestionCuenta = 'Ya has registrado la solicitud en ese banco.';
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

            } else {
                this.registro.registrarse.accounts[this.selectCuentas].entity = this.registro.cuentaActual.entity;
                this.registro.registrarse.accounts[this.selectCuentas].accountNumber = this.registro.cuentaActual.accountNumber;
                this.registro.registrarse.accounts[this.selectCuentas].accountType = this.registro.cuentaActual.accountType;
                //this.registro.registrarse.accounts[this.selectCuentas].name_on_account = this.registro.cuentaActual.name_on_account;
                this.registro.registrarse.accounts[this.selectCuentas].currency = this.registro.cuentaActual.currency;

                /*
                 if (this.registro.registrarse.accounts[0] != null) this.registro.registrarse.accounts[0].default = 1;
                 */
                if (this.cambioNombreBanco) {
                    this.registro.nombresBancos[this.selectCuentas] = this.nombreBanco;
                    this.cambioNombreBanco = false;
                }
                 

                //-----Resetiar input de Cuentas -----------Poner en Metodo
                this.registro.cuentaActual.entity = '';
                //this.registro.cuentaActual.name_on_account = '';
                this.registro.cuentaActual.accountType = '';
                this.registro.cuentaActual.accountNumber = '';
                this.registro.cuentaActual.currency = '';
                this.idBanco = '';
                this.tipoMonedaBanco = '';
                this.tipoCuentaBanco = '';
                this.numeroCuentaBanco = '';
                this.selectCuentas = 999;

                this.nombreBanco = '';
            }
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
            this.TituluarBancoTexto = '';
            this.numeroCuentaBancoTexto = '';

            this.dialogCuentas = false;
        },

        //--------------------------------- Validacion de Cliente Asociado --------------------------
        validarDocumentoAsociado(ExpreRegular, mask_edit) {

            var regExp = new RegExp(ExpreRegular);

            if (this.registro.asociadoActual.number == null ||
                this.registro.asociadoActual.number == '') {
                this.errorDocumentoAsociado = this.hasError;
                this.errorDocumentoAsociadoTexto = i18n.t("valid.docRequerido");

            } else if (this.registro.asociadoActual.prefix == '' ||
                this.registro.asociadoActual.prefix == 0) {

                this.errorDocumentoAsociado = this.hasError;
                this.errorDocumentoAsociadoTexto = i18n.t("valid.tipoDoc");


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

                //this.errorNombreLegal = this.hasError;
                //this.errorTextoNombreLegal = i18n.t("valid.NombreInversionistaNatural");

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
            
            if (this.registro.registrarse.suppliers.length > 0) {
                this.registro.registrarse.suppliers.splice(index, 1);
            }
            else if (this.registro.registrarse.customers.length > 0) {
                this.registro.registrarse.customers.splice(index, 1);
            }
        },
        guardarAsociado() {

            if (this.selectAsociado == 999) {

                if (this.registro.Rol == 1) {
                    this.registro.registrarse.suppliers.push({
                        identification: this.registro.asociadoActual.identification,
                        prefix: this.registro.asociadoActual.prefix,
                        number: this.registro.asociadoActual.number,
                        company: this.registro.asociadoActual.company,
                        name: this.registro.asociadoActual.name,
                        email: this.registro.asociadoActual.email,
                        phoneNumber: this.registro.asociadoActual.phoneNumber,
                        person: this.registro.asociadoActual.person,
                        block: this.blockAsociadoRegistrado
                    });
                }
                else if (this.registro.Rol == 2) {
                    this.registro.registrarse.customers.push({
                        identification: this.registro.asociadoActual.identification,
                        prefix: this.registro.asociadoActual.prefix,
                        number: this.registro.asociadoActual.number,
                        company: this.registro.asociadoActual.company,
                        name: this.registro.asociadoActual.name,
                        email: this.registro.asociadoActual.email,
                        phoneNumber: this.registro.asociadoActual.phoneNumber,
                        person: this.registro.asociadoActual.person,
                        block: this.blockAsociadoRegistrado
                    });
                }
                
                //-----Resetiar input de Cuentas -----------Poner en Metodo
                this.reseteo();

            } else {

                console.log("Actualizar");
                console.log(this.selectAsociado);
                console.log(JSON.parse(JSON.stringify(this.registro.asociadoActual)));
                if (this.registro.registrarse.suppliers.length > 0) {
                    this.registro.registrarse.suppliers[this.selectAsociado].number = this.registro.asociadoActual.number;
                    this.registro.registrarse.suppliers[this.selectAsociado].company = this.registro.asociadoActual.company;
                    this.registro.registrarse.suppliers[this.selectAsociado].name = this.registro.asociadoActual.name;
                    this.registro.registrarse.suppliers[this.selectAsociado].email = this.registro.asociadoActual.email;
                    this.registro.registrarse.customers[this.selectAsociado].person = this.registro.asociadoActual.person;
                }
                else if (this.registro.registrarse.customers.length > 0) {
                    this.registro.registrarse.customers[this.selectAsociado].number = this.registro.asociadoActual.number;
                    this.registro.registrarse.customers[this.selectAsociado].company = this.registro.asociadoActual.company;
                    this.registro.registrarse.customers[this.selectAsociado].name = this.registro.asociadoActual.name;
                    this.registro.registrarse.customers[this.selectAsociado].email = this.registro.asociadoActual.email;
                    this.registro.registrarse.customers[this.selectAsociado].person = this.registro.asociadoActual.person;
                    //this.registro.registrarse.customers[this.selectAsociado] = JSON.parse(JSON.stringify(this.registro.asociadoActual));
                }

                console.log(this.registro.registrarse.suppliers[this.selectAsociado]);
                console.log(this.registro.registrarse.customers[this.selectAsociado]);
                //-----Resetiar input de Cuentas -----------Poner en Metodo
                this.reseteo();
            }

        },
        reseteo() {

            this.blockAsociado = false;
            this.dialogAsociado = false;
            this.registro.asociadoActual.number = '';
            this.registro.asociadoActual.company = '';

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
       
        async AsociadoExistente() {

            this.comprobarDocA.document.identification = this.registro.asociadoActual.identification;
            this.comprobarDocA.document.prefix = this.registro.asociadoActual.prefix;
            this.comprobarDocA.document.number = this.registro.asociadoActual.number;

            let data = await axios.post('Register?handler=VerificarDocActualizar', this.comprobarDocA, { headers: { "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val() } })
                .then((respond) => {

                    console.log(respond);
                    return respond.data;
                }).catch(function (error) {

                    console.log("entro en el catch: ");
                    console.log(error);
                });

            console.log(data);
            if (data != null) {

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

            console.log(State_id);
            let Region = {
                State: State_id
            }

            var data = await axios.post('Register?handler=SelectCity', Region, { headers: { "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val() } })
                .then((respond) => {

                    console.log(respond);
                    return respond.data;

                }).catch(function (error) {

                    console.log("entro en el catch: ");
                    console.log(error);
                });

            if (data != null) {
                this.listCities = data;
                this.listCities.sort((a, b) => a.name < b.name ? -1 : + (a.name > b.name));
            }
            
        },
        async verificarEmail(usuario) {

            console.log(usuario);
            var data = await axios.post('?handler=VerificarEmail', usuario, { headers: { "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val() } })
                .then((respond) => {

                    console.log(respond);
                    return respond.data;

                }).catch(function (error) {

                    console.log("entro en el catch: ");
                    console.log(error);
                });

            if (data.emailExist == true) {
                this.errorEmailNatural = this.hasError;
                this.errorEmailNaturalTexto = 'El correo electrónico ya se encuentra en uso.';
            }
            else if (data.errors == "record not found") {
                this.errorEmailNatural = this.hasSuccess;
                this.errorEmailNaturalTexto = '';
            }
            else {
                this.errorEmailNatural = this.hasError;
                this.errorEmailNaturalTexto = data.errors;
            }
        },
        async VerificarRegistro(comprobarDoc) {

            let participant = JSON.parse(JSON.stringify(this.registro.registrarse.participant));

            let hasError = 'is-invalid';
            let HasSuccess = 'is-valid';

            let errorMensaje = ''

            if (participant == 'SUPPLIER') {
                errorMensaje = i18n.t("valid.usuarioExistenteProveedor");
            } else if (participant == 'DEBTOR') {
                errorMensaje = i18n.t("valid.usuarioExistenteEmpresa");
            } else if (participant == 'FACTOR') {
                errorMensaje = i18n.t("valid.usuarioExistenteFactor");
            } else if (participant == 'BANK' || participant == 'CONFIRMANT') {
                errorMensaje = i18n.t("valid.usuarioExistenteBanco");
            } 

            var data = await axios.post('Register?handler=VerificarDoc', comprobarDoc, { headers: { "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val() } })
                .then((respond) => {

                    console.log(respond);
                    return respond;

                }).catch(function (error) {

                    console.log(error);
 
                });

            console.log(data);

            if (data != null) {

                if (data.data.contratos != null) {
                    for (let i = 0; i < data.data.contratos.length; i++) {

                        if (participant == data.data.contratos[i].participant || "BACKOFFICE" == data.data.contratos[i].participant) {

                            this.errorDocumento = this.hasError;
                            this.errorTextoDocumentoPrincipal = errorMensaje;
                        }
                    }
                }

                if (this.errorDocumento != this.hasError) {

                    this.registro.registrarse = data.data.registro;
                    this.registro.registrarse.participant = participant;
                    this.registro.registrarse.accounts = [];
                    if (data.data.contacts != null) {
                        for (let i = 0; i < data.data.contacts.length; i++) {
                            if (data.data.contacts[i].label == 'LEGAL') this.registro.representante = data.data.contacts[i];
                            if (data.data.contacts[i].label == 'CONTACT') this.registro.contacto = data.data.contacts[i];
                            if (data.data.contacts[i].label == 'ADMINISTRATOR') this.registro.administrador = data.data.contacts[i];
                        }

                        this.rellenarCampos(JSON.parse(JSON.stringify(this.registro)));
                    }
                }
                
            } else this.vaciarCampos();
        },
        async EnviarDatos(registro) {

            rgj = JSON.parse(JSON.stringify(registro));

            if (rgj.Rol != 4) {
                rgj.registrarse.discriminator = 'LEGAL';
                rgj.registrarse.email = null;
                rgj.registrarse.firstName = null;
                rgj.registrarse.lastName = null;
                rgj.registrarse.phone.label = 'MAIN';
            }
            else {
                rgj.registrarse.discriminator = 'PERSON';
                rgj.registrarse.email.label = "MAIN";
                rgj.registrarse.company = null;
                rgj.registrarse.lastName = '';
                rgj.registrarse.phone.label = 'MAIN';
                //rgj.registrarse.phone.country = 32;
                
            }

            if (rgj.Rol == 1 || rgj.Rol == 2) {
                // Regsitro Empresa, Proveedor, Banco registrando Empresa
                rgj.representante.label = 'LEGAL';
                if ((rgj.contacto.name != null && rgj.contacto.name != '') &&
                    (rgj.contacto.documentNumber != null && rgj.contacto.documentNumber != '') &&
                    (rgj.contacto.phoneNumber != null && rgj.contacto.phoneNumber != '') &&
                    (rgj.contacto.email != null && rgj.contacto.email != '')) {

                    rgj.contacto.label = 'CONTACT';
                    rgj.registrarse.contacts.push(rgj.contacto);
                }

                rgj.registrarse.contacts.push(rgj.representante);
                if (rgj.registrarse.accounts.length == 0) rgj.registrarse.accounts = null;

            } else if (rgj.Rol == 3) {
                // Regsitro Factor LEGAL
                rgj.registrarse.contacts.push(rgj.representante);
                if (rgj.registrarse.accounts.length == 0) rgj.registrarse.accounts = null;

            } else if (rgj.Rol == 4) {
               // Regsitro Factor PEOPLE
                if (rgj.registrarse.accounts.length == 0) rgj.registrarse.accounts = null;

            } else if (rgj.user.participant == 'CONFIRMANT') {
                // Regsitro Factor BANCO
                rgj.registrarse.participant = 'CONFIRMANT';
                rgj.representante.label = 'LEGAL';
                if ((rgj.contacto.name != null && rgj.contacto.name != '') &&
                    (rgj.contacto.documentNumber != null && rgj.contacto.documentNumber != '') &&
                    (rgj.contacto.phoneNumber != null && rgj.contacto.phoneNumber != '') &&
                    (rgj.contacto.email != null && rgj.contacto.email != '')) {

                    rgj.contacto.label = 'CONTACT';
                    rgj.registrarse.contacts.push(rgj.contacto);
                    rgj.registrarse.accepted_agreement = false;
                }
                rgj.registrarse.contacts.push(rgj.representante);
                if (rgj.registrarse.accounts.length == 0) rgj.registrarse.accounts = null;

            } else if (rgj.user.participant == 'BACKOFFICE') {
                // Operativo TuFactoring
                rgj.representante.label = 'LEGAL';
                rgj.registrarse.contacts.push(rgj.representante);
                rgj.administrador.label = 'ADMINISTRATOR'
                rgj.registrarse.contacts.push(rgj.administrador);
                rgj.registrarse.accounts = null;
                rgj.registrarse.participant = 'BACKOFFICE';
                rgj.registrarse.related = false;
                rgj.registrarse.accepted_agreement = false;

                for (let i = 0; i < rgj.dataPaises.allies; i++) {
                    if (rgj.registrarse.routing_number == rgj.dataPaises.allies[i].routing_number) rgj.registrarse.related = true;
                }
            }

            let registroGuardar = rgj.registrarse;

            var tokenReCaptcha = ""

            try {
                tokenReCaptcha = document.getElementById("idrecaptcha").value
            } catch (e) { }

            var data = await axios.post('', { person : registroGuardar, tokenReCaptcha : tokenReCaptcha}, { headers: { "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val() } })
                .then((respond) => {
                    console.log(respond)

                    if (respond.data.errorValidacion != null) {

                        toastr.error(i18n.t("tooltip.errorUsuarioRegistrado") + "<br/>" + respond.data.errorValidacion[0].errorMessage);
                        return respond.data = "Error";
                    }
                    else if (respond.data == "errorCaptcha") {
                        toastr.error(i18n.t(respond.data))
                        pedirTokenRecaptcha("register")
                    }
                    else if (respond.data == 'Error') {

                        toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br/>" + "A ocurrido un fallo en los servicios de TuFactoring por favor se le recomienda intentar mas tarde.");
                    }
                    else if (respond.data == 'cannot be null') {

                        toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br/>" + "Los campos requeridos no pueden estar vacios.");
                    }
                    else if (respond.data == 'person already exists') {

                        toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br/>" + "El documento de Identidad ya se encuentra en uso.");
                    }
                    else if (respond.data.includes("success:")) {

                        if (rgj.registrarse.participant == 'DEBTOR' || rgj.registrarse.participant == 'FACTOR') toastr.success("<div class='text-justify'>" + i18n.t("tooltip.queridoUsuario") + "<br /><br />" + i18n.t("tooltip.registroEmpresa") + "</div>");
                        else if (rgj.registrarse.participant == 'CONFIRMANT' || rgj.registrarse.participant == 'BACKOFFICE') toastr.success("<div class='text-justify'>" + i18n.t("tooltip.queridoUsuario") + "<br /><br />" + i18n.t("tooltip.registroBanco") + "</div>");
                        else toastr.success("<div class='text-justify'>" + i18n.t("tooltip.queridoUsuario") + "<br /><br />" + i18n.t("tooltip.registroProveedor") + "</div>");
                    }
                    else {
                        toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br />" + i18n.t("errorBaseDatos"));
                        setTimeout(function () { window.location.href = "../../index"; }, 3000); 
                    }

                    return respond.data;

                }).catch((error) => {

                    console.log(error);
                    toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br/><br/>" + i18n.t("errorBaseDatos"));
                    setTimeout(function () { window.location.href = "../../index"; }, 3000); 
                });

            console.log(data)
            if (data.includes("success:") == false) this.resetearRegistro = null;
            else this.resetearRegistro = data;
        },
    },
    computed: {
        //...Vuex.mapState(['respuestaComprobacionDoc']),
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
                this.registro.cuentaActual.currency = '';
                this.registro.cuentaActual.accountNumber = '';

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

            if (this.registro.Rol == 3 || this.registro.Rol == 4) {

                if (this.eleccion == 'EMPRESA') {

                    if (this.terminosYCondiciones == true &&
                        this.errorDocumento == this.hasSuccess &&
                        this.errorNombreLegal == this.hasSuccess &&
                        //(this.errorNombreComercial == this.hasSuccess || this.errorNombreComercial == '') &&
                        this.errorPurpose == this.hasSuccess &&
                        this.errorDocumentoRepresentanteLegal == this.hasSuccess &&
                        this.errorNombresRepresentanteLegal == this.hasSuccess &&
                        this.errorTelefonoRepresentante == this.hasSuccess &&
                        this.errorEmailRepresentante == this.hasSuccess &&
                        this.errorDireccion == this.hasSuccess &&
                        (this.errorDireccion2 == '' || this.errorDireccion2 == this.hasSuccess) &&
                        this.errorCiudad == this.hasSuccess &&
                        this.errorTelefonoDirecciones == this.hasSuccess) {

                        if (this.validateReset == false) {
                            if (this.registro.registrarse.accounts.length <= 0) return true;
                        }
                        return false;

                    } else {

                        return true;
                    }

                } else {

                    console.log(this.registro.registrarse.accounts.length)

                    if (this.terminosYCondiciones == true &&
                        this.errorDocumento == this.hasSuccess &&
                        this.errorNombreLegal == this.hasSuccess &&
                        //(this.errorNombreComercial == this.hasSuccess || this.errorNombreComercial == '') &&
                        this.errorOccupation == this.hasSuccess &&
                        this.errorEmailNatural == this.hasSuccess &&
                        (this.errorFechaNacimiento == '' || this.errorFechaNacimiento == this.hasSuccess) &&
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
            else if (this.registro.Rol == 1 || this.registro.Rol == 2 || this.registro.user.participant == 'CONFIRMANT') {

                if (this.registro.user.participant == 'CONFIRMANT') this.terminosYCondiciones = true

                if (this.terminosYCondiciones == true &&
                    this.errorDocumento == this.hasSuccess &&
                    this.errorNombreLegal == this.hasSuccess &&
                    this.errorPurpose == this.hasSuccess &&
                    this.errorDocumentoRepresentanteLegal == this.hasSuccess &&
                    this.errorNombresRepresentanteLegal == this.hasSuccess &&
                    this.errorTelefonoRepresentante == this.hasSuccess &&
                    this.errorEmailRepresentante == this.hasSuccess &&
                    this.errorDireccion == this.hasSuccess &&
                    (this.errorDireccion2 == '' || this.errorDireccion2 == this.hasSuccess) &&
                    this.errorCiudad == this.hasSuccess &&
                    this.errorTelefonoDirecciones == this.hasSuccess) {

                    if (this.validateReset == false) {
                        if (this.registro.registrarse.accounts.length <= 0) return true;
                    }

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
            else if (this.registro.user.participant == 'BACKOFFICE') {

                console.log(this.registro.user.participant);
                if (this.errorDocumento == this.hasSuccess &&
                    this.errorNombreLegal == this.hasSuccess &&
                    //(this.errorNombreComercial == this.hasSuccess || this.errorNombreComercial == '') &&
                    this.errorPurpose == this.hasSuccess &&
                    this.errorDocumentoRepresentanteLegal == this.hasSuccess &&
                    this.errorNombresRepresentanteLegal == this.hasSuccess &&
                    this.errorTelefonoRepresentante == this.hasSuccess &&
                    this.errorEmailRepresentante == this.hasSuccess &&
                    this.errorDocumentoAdministrador == this.hasSuccess &&
                    this.errorNombreAdministrador == this.hasSuccess &&
                    //this.errorApellidoAdministrador == this.hasSuccess &&
                    this.errorTelefonoAdministrador == this.hasSuccess &&
                    this.errorEmailAdministrador == this.hasSuccess &&
                    (this.errorDireccion2 == '' || this.errorDireccion2 == this.hasSuccess) &&
                    this.errorDireccion == this.hasSuccess &&
                    this.errorCiudad == this.hasSuccess &&
                    this.errorTelefonoDirecciones == this.hasSuccess &&
                    this.errorRoutinNumber == this.hasSuccess) {
                    return false;
                } else {

                    return true;
                }
            }

        },
        habilitarBotonSiguente() {

            if (this.errorDocumento == this.hasSuccess &&
                this.errorNombreLegal == this.hasSuccess &&
                //(this.errorNombreComercial == this.hasSuccess || this.errorNombreComercial == '') &&
                this.errorPurpose == this.hasSuccess &&
                this.errorDireccion == this.hasSuccess &&
                (this.errorDireccion2 == '' || this.errorDireccion2 == this.hasSuccess) &&
                this.errorCiudad == this.hasSuccess &&
                this.errorTelefonoDirecciones == this.hasSuccess) {

                return false;

            } else {

                return true;
            }
            
        },
        resultadoLogo() {
            return this.logo;
        },
        
    }
});