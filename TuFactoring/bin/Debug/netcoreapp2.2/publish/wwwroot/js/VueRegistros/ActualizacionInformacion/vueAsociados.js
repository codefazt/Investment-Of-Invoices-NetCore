var app = new Vue({
    el: '#appAsociados',
    i18n,
    store: store,
    vuetify: new Vuetify({
        lang: {
            t: (key, ...params) => i18n.t(key, params)
        }
    }),
    data: {
        modalLogout: { mostrar: false },
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

        headersAsociado: [

            { text: i18n.t("headers.cliente"), value: 'company' },
            //{ text: i18n.t("headers.prefix"), value: 'prefix' },
            { text: i18n.t("headers.numeroDoc"), value: 'number', align: 'center' },
            { text: i18n.t("headers.contacto"), value: 'name' },
            { text: i18n.t("headers.correoElectronico"), value: 'email' },

            { text: i18n.t("headers.estatus"), value: 'estado', align: 'center' },
            //{ text: i18n.t("headers.asociados"), value: 'invitado', align: 'center' },
            { text: i18n.t("headers.acciones"), value: 'action', align: 'center' },

        ],
        headersSupplier: [

            { text: i18n.t("headers.proveedor"), value: 'company' },
            //{ text: i18n.t("headers.prefix"), value: 'prefix' },
            { text: i18n.t("headers.numeroDoc"), value: 'number', align: 'center' },
            { text: i18n.t("headers.contacto"), value: 'name' },
            { text: i18n.t("headers.correoElectronico"), value: 'email' },

            { text: i18n.t("headers.estatus"), value: 'estado', align: 'center' },
            //{ text: i18n.t("headers.asociados"), value: 'invitado', align: 'center' },
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
        console.log(this.registro)
        this.Inicio();
        document.getElementById('contenido').removeAttribute('hidden');
        this.cargando = false;
    },
    methods: {
        Inicio() {

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
                }
            }

            if (this.registro.registrarse != null) {

                this.registro.registrarse.participant = this.registro.user.participant;
                this.rellenoDatosInicial();
            }

            this.registro.asociadoActual.prefix = '';
        },
        limpiarMensajes: function () {
            store.commit("limpiarMensajes")
        },
        rellenoDatosInicial() {

        },
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
        validarNombresAsociadoContacto() {

            let RE = /^([[A-Za-zÁ-Ýá-ýñÑ\´]{2,}[\s]{1,1}[[A-Za-zÁ-Ýá-ýñÑ\´]{2,}[[A-Za-zÁ-Ýá-ýñÑ\s\.\´]{0,})+$/i
            let emailRegex = /^[[A-Za-zÁ-Ýá-ýñÑ\s\´\.]+$/i;

            if (this.registro.asociadoActual.name == '' ||
                this.registro.asociadoActual.name == null ||
                this.registro.asociadoActual.name[0] == ' ') {

                this.errorNombreContacto = this.hasError;
                this.errorNombreContactoTexto = i18n.t("valid.nombreRequerido");

            } else if (!emailRegex.test(this.registro.asociadoActual.name)) {

                this.errorNombreContacto = this.hasError;
                this.errorNombreContactoTexto = i18n.t("valid.charNoPermitidosNombres");

            } else if (!RE.test(this.registro.asociadoActual.name)) {

                this.errorNombreContacto = this.hasError;
                this.errorNombreContactoTexto = i18n.t("valid.NombreInversionistaNatural");

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
                this.registro.asociadoActual.person = null;
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
        guardarActualizacion(registro) {
            this.ContenidoParaMandar(registro, "");
        },

        async ContenidoParaMandar(registro, ruta) {
            rgj = JSON.parse(JSON.stringify(registro));
            if (rgj.user.participant == 'SUPPLIER') rgj.registrarse.suppliers = null;
            if (rgj.user.participant == 'DEBTOR') rgj.registrarse.customers = null;

            if (rgj.registrarse.customers != null) {
                let copyCustomers = JSON.parse(JSON.stringify(rgj.registrarse.customers));
                rgj.registrarse.customers = [];
                for (let i = 0; i < copyCustomers.length; i++) {
                    let id_person = null;
                    if (copyCustomers[i].person != null) {
                        if (copyCustomers[i].person.id != null) id_person = copyCustomers[i].person.id;
                        else id_person = copyCustomers[i].person;
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
                
                rgj.registrarse.suppliers = [];
                for (let i = 0; i < copySuppliers.length; i++) {

                    let id_person = null;
                    if (copySuppliers[i].person != null) {
                        if (copySuppliers[i].person.id != null) id_person = copySuppliers[i].person.id;
                        else id_person = copySuppliers[i].person;
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
                    if (respond.data == null) toastr.error("Se han presentado inconvenientes al actualizar la información, por favor verifique e intente nuevamente");
                    else toastr.success("<div class='text-justify'>" + i18n.t("tooltip.queridoUsuario") + "<br /><br />" + i18n.t("tooltip.actualizarPerfil") + "</div>");

                    return respond.data;

                }).catch((error) => {
                    console.log(error)
                    if (typeof error === 'string' || error instanceof String) {

                        if (error.includes("<!DOCTYPE html>")) {
                            window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired";
                            toastr.error("Se han presentado inconvenientes al actualizar la información, por favor verifique e intente nuevamente");
                            return;
                        }
                    }
                    toastr.error("Se han presentado inconvenientes al actualizar la información, por favor verifique e intente nuevamente");
                });

            if (typeof data === 'string' || data instanceof String) {
                console.log(data);
                if (data.includes("<!DOCTYPE html>")) {
                    window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired";
                    toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br />" + i18n.t("errorBaseDatos"));
                    return;
                }
            }

            if (data == null) this.resetearRegistro = null;
            else {
                this.registro = data;
                this.Inicio();
            }
        },

        async ToggleInvitation(asociado) {

            let invitado = { invitation: asociado.id }
            let data = await axios.post('Asociados?handler=ToggleInvitation', invitado, { headers: { "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val() } })
                .then((respond) => {

                    resetTime();
                    console.log(respond.data)
                    if (respond.data.error != null) {

                        if (respond.data.error == "Invitation not found") toastr.error("<div class='text-justify'>" + i18n.t("tooltip.queridoUsuario") + "<br /><br /> Se han presentado inconvenientes al actualizar la información, por favor verifique e intente nuevamente <br /> Invitación no encontrada </div>");
                        else if (respond.data.error == "You are not authorised to perform this action") toastr.error("<div class='text-justify'>" + i18n.t("tooltip.queridoUsuario") + "<br /><br /> Se han presentado inconvenientes al actualizar la información, por favor verifique e intente nuevamente <br /> No está autorizado para llevar a cabo esta acción. </div>");
                        else toastr.error("<div class='text-justify'>" + i18n.t("tooltip.queridoUsuario") + "<br /><br /> Se han presentado inconvenientes al actualizar la información, por favor verifique e intente nuevamente </div>");
                    }
                    else toastr.success("<div class='text-justify'>" + i18n.t("tooltip.queridoUsuario") + "<br /><br />" + i18n.t("tooltip.actualizarPerfil") + "</div>");

                    return respond.data;
                }).catch(function (error) {

                    console.log(error);
                    toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br />" + i18n.t("errorConexion"));
                });
            console.log(data)
            if (typeof data === 'string' || data instanceof String) {

                if (data.includes("<!DOCTYPE html>")) {
                    window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired";
                    toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br />" + i18n.t("errorConexion"));
                    return;
                }
            }
            if (data.error != null) this.resetearRegistro = null;
            else {
                this.registro = data;
                this.Inicio();
            }
        },
        async CancelInvitation(asociado) {

            let invitado = { invitation: asociado.id }
            let data = await axios.post('Asociados?handler=CancelInvitation', invitado, { headers: { "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val() } })
                .then((respond) => {

                    resetTime();
                    console.log(respond.data)
                    if (respond.data.error != null) {

                        if (respond.data.error == "Invitation not found") toastr.error("<div class='text-justify'>" + i18n.t("tooltip.queridoUsuario") + "<br /><br /> Se han presentado inconvenientes al actualizar la información, por favor verifique e intente nuevamente <br /> Invitación no encontrada </div>");
                        else if (respond.data.error == "You are not authorised to perform this action") toastr.error("<div class='text-justify'>" + i18n.t("tooltip.queridoUsuario") + "<br /><br /> Se han presentado inconvenientes al actualizar la información, por favor verifique e intente nuevamente <br /> No está autorizado para llevar a cabo esta acción. </div>");
                        else toastr.error("<div class='text-justify'>" + i18n.t("tooltip.queridoUsuario") + "<br /><br /> Se han presentado inconvenientes al actualizar la información, por favor verifique e intente nuevamente </div>");
                    }
                    else toastr.success("<div class='text-justify'>" + i18n.t("tooltip.queridoUsuario") + "<br /><br />" + i18n.t("tooltip.actualizarPerfil") + "</div>");

                    return respond.data;
                }).catch(function (error) {

                    console.log(error);
                    toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br />" + i18n.t("errorConexion"));
                });
            console.log(data)
            if (typeof data === 'string' || data instanceof String) {

                if (data.includes("<!DOCTYPE html>")) {
                    window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired";
                    toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br />" + i18n.t("errorConexion"));
                    return;
                }
            }
            if (data.error != null) this.resetearRegistro = null;
            else {
                this.registro = data;
                this.Inicio();
            }
        },
        async AcceptInvitation(asociado) {

            let invitado = { invitation: asociado.id }
            let data = await axios.post('Asociados?handler=AcceptInvitation', invitado, { headers: { "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val() } })
                .then((respond) => {

                    resetTime();
                    console.log(respond.data)
                    if (respond.data.error != null) {

                        if (respond.data.error == "Invitation not found") toastr.error("<div class='text-justify'>" + i18n.t("tooltip.queridoUsuario") + "<br /><br /> Se han presentado inconvenientes al actualizar la información, por favor verifique e intente nuevamente <br /> Invitación no encontrada </div>");
                        else if (respond.data.error == "You are not authorised to perform this action") toastr.error("<div class='text-justify'>" + i18n.t("tooltip.queridoUsuario") + "<br /><br /> Se han presentado inconvenientes al actualizar la información, por favor verifique e intente nuevamente <br /> No está autorizado para llevar a cabo esta acción. </div>");
                        else toastr.error("<div class='text-justify'>" + i18n.t("tooltip.queridoUsuario") + "<br /><br /> Se han presentado inconvenientes al actualizar la información, por favor verifique e intente nuevamente </div>");
                    }
                    else toastr.success("<div class='text-justify'>" + i18n.t("tooltip.queridoUsuario") + "<br /><br />" + i18n.t("tooltip.actualizarPerfil") + "</div>");

                    return respond.data;
                }).catch(function (error) {

                    console.log(error);
                    toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br />" + i18n.t("errorConexion"));
                });
            console.log(data)
            if (typeof data === 'string' || data instanceof String) {

                if (data.includes("<!DOCTYPE html>")) {
                    window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired";
                    toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br />" + i18n.t("errorConexion"));
                    return;
                }
            }
            if (data.error != null) this.resetearRegistro = null;
            else {
                this.registro = data;
                this.Inicio();
            }
        },
        async RejectInvitation(asociado) {

            let invitado = { invitation: asociado.id }
            let data = await axios.post('Asociados?handler=RejectInvitation', invitado, { headers: { "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val() } })
                .then((respond) => {

                    resetTime();
                    console.log(respond.data)
                    if (respond.data.error != null) {

                        if (respond.data.error == "Invitation not found") toastr.error("<div class='text-justify'>" + i18n.t("tooltip.queridoUsuario") + "<br /><br /> Se han presentado inconvenientes al actualizar la información, por favor verifique e intente nuevamente <br /> Invitación no encontrada </div>");
                        else if (respond.data.error == "You are not authorised to perform this action") toastr.error("<div class='text-justify'>" + i18n.t("tooltip.queridoUsuario") + "<br /><br /> Se han presentado inconvenientes al actualizar la información, por favor verifique e intente nuevamente <br /> No está autorizado para llevar a cabo esta acción. </div>");
                        else toastr.error("<div class='text-justify'>" + i18n.t("tooltip.queridoUsuario") + "<br /><br /> Se han presentado inconvenientes al actualizar la información, por favor verifique e intente nuevamente </div>");
                    }
                    else toastr.success("<div class='text-justify'>" + i18n.t("tooltip.queridoUsuario") + "<br /><br />" + i18n.t("tooltip.actualizarPerfil") + "</div>");

                    return respond.data;
                }).catch(function (error) {

                    console.log(error);
                    toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br />" + i18n.t("errorConexion"));
                });
            console.log(data)
            if (typeof data === 'string' || data instanceof String) {

                if (data.includes("<!DOCTYPE html>")) {
                    window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired";
                    toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br />" + i18n.t("errorConexion"));
                    return;
                }
            }
            if (data.error != null) this.resetearRegistro = null;
            else {
                this.registro = data;
                this.Inicio();
            }
        },
        async SendInvitation(asociado) {

            let invitado = { invitation: asociado.id }
            let data = await axios.post('Asociados?handler=SendInvitation', invitado, { headers: { "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val() } })
                .then((respond) => {

                    resetTime();
                    console.log(respond.data)
                    if (respond.data.error != null) {

                        if (respond.data.error == "Invitation not found") toastr.error("<div class='text-justify'>" + i18n.t("tooltip.queridoUsuario") + "<br /><br /> Se han presentado inconvenientes al actualizar la información, por favor verifique e intente nuevamente <br /> Invitación no encontrada </div>");
                        else if (respond.data.error == "You are not authorised to perform this action") toastr.error("<div class='text-justify'>" + i18n.t("tooltip.queridoUsuario") + "<br /><br /> Se han presentado inconvenientes al actualizar la información, por favor verifique e intente nuevamente <br /> No está autorizado para llevar a cabo esta acción. </div>");
                        else toastr.error("<div class='text-justify'>" + i18n.t("tooltip.queridoUsuario") + "<br /><br /> Se han presentado inconvenientes al actualizar la información, por favor verifique e intente nuevamente </div>");
                    }
                    else toastr.success("<div class='text-justify'>" + i18n.t("tooltip.queridoUsuario") + "<br /><br />" + i18n.t("tooltip.actualizarPerfil") + "</div>");

                    return respond.data;
                }).catch(function (error) {

                    console.log(error);
                    toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br />" + i18n.t("errorConexion"));
                });
            console.log(data)
            if (typeof data === 'string' || data instanceof String) {

                if (data.includes("<!DOCTYPE html>")) {
                    window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired";
                    toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br />" + i18n.t("errorConexion"));
                    return;
                }
            }
            if (data.error != null) this.resetearRegistro = null;
            else {
                this.registro = data;
                this.Inicio();
            }
        },

    },
    computed: {
        
        cambiarCiudad() {
            if (this.idCiudad == 0) this.tamanioDiv = "col-md-6"
            else this.tamanioDiv = "col-md-4"

            return this.idCiudad;
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