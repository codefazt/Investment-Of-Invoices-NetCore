// JavaScript con Vue Segmentacion

var app = new Vue({
    el: '#appConciliacion',
    i18n,
    vuetify: new Vuetify({
        lang: {
            t: (key, ...params) => i18n.t(key, params)
        }
    }),
    data: {
        i18n: i18n,
        datosConfirmacionPagosPorConciliar: null,
        datosConfirmacionMovements: null,
        datosConfirmacion: {},
        boton_movement: false,
        id_movement: null,
        id_payment: '',
        dialogAyuda: false,
        modalLogout: { mostrar: false },
        tablas: [],
        payments: [],
        backEndDateFormat: backEndDateFormat,
        formatoMonedaInput: formatoMonedaInput,
        hasSuccess: 'is-valid',
        hasError: 'is-invalid',
        //Tabla
        //Cabecera de la tabla
        headerPagos: [
            { text: i18n.t("headers.n"), value: "n", align: "center" },
            { text: i18n.t("headers.seleccione"), value: "radio", align: "center" }, 
            { text: i18n.t("headers.dataPaid"), value: 'payment_date', align: 'center' },
            { text: i18n.t("headers.bankConfirming"), value: 'img', align: 'center' },
            { text: i18n.t("headers.cuentaReceptora"), value: 'receiving_account_name', align: 'center' },
            { text: i18n.t("headers.payer"), value: 'payer' },
            { text: i18n.t("headers.amountPaid"), value: 'amount' },
            { text: i18n.t("headers.referencia"), value: 'referencia', align: 'center' },
            { text: i18n.t("headers.opciones"), value: 'action', align: 'center' },
        ],
        headerMovements: [
            { text: i18n.t("headers.n"), value: "n", align: "center" },
            { text: i18n.t("headers.fechaTransaccion"), value: "movement_dated", align: "center" },
            { text: i18n.t("headers.bankConfirming"), value: 'entity', align: 'center' },
            //{ text: i18n.t("headers.coin"), value: "currency", align: "center" },
            { text: i18n.t("headers.cuentaReceptora"), value: 'account_number', align: 'center' },
            { text: i18n.t("headers.amountPaid"), value: 'amount' },
            { text: i18n.t("headers.referencia"), value: 'number', align: 'center' },
            { text: i18n.t("headers.opciones"), value: 'action', align: 'center' },
        ],
        headerPagosConfirmacion: [
            { text: i18n.t("headers.n"), value: "n", align: "center" },
            { text: i18n.t("headers.seleccione"), value: "radio", align: "center" },
            { text: i18n.t("headers.dataPaid"), value: 'payment_date', align: 'center' },
            { text: i18n.t("headers.bankConfirming"), value: 'img', align: 'center' },
            { text: i18n.t("headers.cuentaReceptora"), value: 'receiving_account_name', align: 'center' },
            { text: i18n.t("headers.payer"), value: 'payer' },
            { text: i18n.t("headers.amountPaid"), value: 'amount' },
            { text: i18n.t("headers.referencia"), value: 'referencia', align: 'center' },
            { text: i18n.t("headers.opciones"), value: 'action', align: 'center' },
        ],
        headerMovementsConfirmacion: [
            { text: i18n.t("headers.fechaTransaccion"), value: "movement_dated", align: "center" },
            { text: i18n.t("headers.bankConfirming"), value: 'entity', align: 'center' },
            //{ text: i18n.t("headers.coin"), value: "currency", align: "center" },
            { text: i18n.t("headers.cuentaReceptora"), value: 'account_number', align: 'center' },
            { text: i18n.t("headers.amountPaid"), value: 'amount' },
            { text: i18n.t("headers.referencia"), value: 'number', align: 'center' },
        ],
        dialogConciliar: false,
        dialogBloquear: false,
        dialogmovement: false,
        buscarClientes: true,
        totalClientes: null,
        DatosResividos: [],
        id_conciliado: null,
        moment: moment,
        envio: false,
        loading: false,
        tablaMovemnts: [],
        loadingMovements: false,
        filterMovements: '',
        buscarMovements: true,
        totalMovements: 0,

        options: {},
        listaLoading: [],
        listaBuscarClientes: [],
        listaTotalClientes: [],
        listaOptions: [],
        filter: [],
        modalCargandoDetalle: false,
        dialogArchivo: false,
        selectedFile: { type: '', file: '' },
        file: '',
        habilitarBtnFile: true,
        ErroresArchivo: {
            vacio: true,
            formatoInvalido: true,
            maxSize: true
        },
        ErroresContenidoFile: {
            fecha: true,
            moneda: true,
            monto: true,
            numeroCuenta: true,
            referencia: true,
            formato: true,
        },
        formatoValido: false,
        errorBanco: '',
        errorBancoTexto: '',
        errorMoneda: '',
        errorMonedaTexto: '',
        hasError: 'is-invalid',
        hasSuccess: 'is-valid',
    },
    watch: {
        listaOptions: {
            async handler() {
                
                if (this.tablas.length > 0) {

                    for (let i = 0; i < this.listaOptions.length; i++) {
                        if (this.tablas[i] != null) {
                            if (this.listaBuscarClientes[i] && this.listaOptions[i].page * this.listaOptions[i].itemsPerPage >= this.tablas[i].length - this.listaOptions[i].itemsPerPage) {

                                var aux = this.filter[i].currency

                                this.filter[i].currency = this.currencies[i].id
                                await this.llenar_consulta(i);

                                this.filter[i].currency = aux

                            }
                        }
                    }
                }
            },
            deep: true,
        },
        options: {
            async handler() {
                if (this.buscarMovements && this.options.page * this.options.itemsPerPage >= this.DatosResividos.length - this.options.itemsPerPage) {
                    await this.ConsultaMovements();
                }
            },
            deep: true,
        },
    },
    created: function () {

        try {
            this.lang = document.getElementsByTagName("html")[0].getAttribute("lang")
            this.currencies = JSON.parse(document.getElementById('currenciesData').value);
            document.getElementById("eliminarData").removeChild(document.getElementById('currenciesData'));

            for (var i = 0; i < this.currencies.length; i++) {
                this.listaOptions.push({});
                this.listaBuscarClientes.push(true);
                this.listaLoading.push(false);
                this.listaTotalClientes.push(0)
                this.filter[i] = JSON.parse(document.getElementById('filterData+' + i).value);
                this.filter[i].currency = this.currencies[i].id;
                document.getElementById("eliminarData").removeChild(document.getElementById('filterData+' + i));
            }

            //this.filter = JSON.parse(document.getElementById('filterData').value);
            //document.getElementById("eliminarData").removeChild(document.getElementById('filterData'));
        } catch (e) {
            for (var i = 0; i < this.currencies.length || i < 1; i++) {
                this.listaOptions.push({});
                this.listaBuscarClientes.push(true);
                this.listaLoading.push(false);
                this.listaTotalClientes.push(0)
                this.filter[0] = null
            }
        }
        document.getElementById('contenido').removeAttribute('hidden');
        this.cargado = false;
    },
    methods: {
        async llenar_consulta(i) {

            if (this.listaLoading[i]) return
            this.listaLoading[i] = true
            let take = 100;
            if (i == null) i = 0;
            let pag = this.filter[i]; //Provicional

            const data = await axios.post('Conciliacion?handler=LlenarTabla', pag, { headers: { "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val() } })
                .then(function (response) {
                    // handle success
                    resetTime();
                    return response.data;
                });

            this.listaLoading[i] = false
            if (typeof data === 'string' || data instanceof String) {
                if (data.includes("<!DOCTYPE html>")) {
                    window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired";
                    toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br />" + i18n.t("errorBaseDatos"));
                    return;
                }
            }
            //this.DatosDetalles = JSON.parse(JSON.stringify(data.data));
            if (data != null) {

                if (data.concilar != null) {
                    this.payments = [];

                    if (data.error == "You are not authorised to perform this action") {
                        toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br />" + i18n.t("tooltip.noAuthorizado"));
                        window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                    }

                    if (data.concilar.length == 0) {
                        this.loading = false
                        this.buscarClientes = false
                        this.listaBuscarClientes[i] = false
                        return
                    }

                    if (data.concilar.length < take) this.listaBuscarClientes[i] = false
                    this.listaTotalClientes[i] += data.concilar.length

                    data.concilar.map(payment => {
                        if (payment == null) return

                        let img_number = null;
                        if (payment.receipt != null) {
                            if (payment.receipt.receiving_account != null) {
                                if (payment.receipt.receiving_account.entity != null) {
                                    if (payment.receipt.receiving_account.entity.routing_number != null) img_number = payment.receipt.receiving_account.entity.routing_number;
                                }
                            }
                        }
                        //Crear Tabla
                        this.payments.push({
                            id: payment.id,
                            payer: payment.receipt.payer.name,
                            receiving_account_name: payment.receipt.receiving_account.entity.person.name,
                            receiving_account_accountNumber: payment.receipt.receiving_account.accountNumber,
                            entity: payment.entity.person.name,
                            amount: payment.amount,
                            referencia: payment.number,
                            state: payment.state,
                            currency: payment.currency,
                            account_number: payment.account_number,
                            payment_date: payment.payment_date,
                            img: img_number
                        });
                    });
                    this.tablas.push(this.payments)
                }

            }

            if (data == "You are not authorised to perform this action") {
                toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br />" + i18n.t("errorBaseDatos"));
            }
            this.loading = false;
        },
        async ConsultaMovements() {
            if (this.loadingMovements) return
            this.loadingMovements = true
            let take = 100;
            const data = await axios.post('?handler=Movements', { pagination: { take: take, skip: this.totalMovements }, filter: this.filterMovements }, { headers: { "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val() } })
                .then(function (response) {
                    // handle success
                    resetTime()
                    return response;
                });

            if (data != null) {

                this.tablaMovemnts = [];
                if (data.data.list.length == 0) {
                    this.loadingMovements = false
                    this.buscarMovements = false
                }

                if (data.data.error == "You are not authorised to perform this action") {
                    window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                    return
                }

                if (data.data.list.length < (take / 2)) this.buscarMovements = false
                this.totalMovements += parseInt(data.data.list.length)

            // Contenido de la respuesta 
                data.data.list.map(movement => {

                    this.tablaMovemnts.push({

                        //id: movement.payment_id,
                        id: movement.id,
                        payment_id: movement.payment_id,
                        movement_dated : movement.movement_dated,
                        account_number : movement.account_number,
                        number: movement.number,
                        amount: movement.amount,
                        entity: movement.entity.person.name,
                        currency: movement.currency.iso_4217,
                        digits: movement.currency.digits,
                        symbol: movement.currency.symbol,
                        img: movement.entity.routing_number,
                    });
                });
            // ---------------------------------------
            }

            this.loadingMovements = false;
            return this.buscarMovements
        },
        conciliar() {
            ruta = 'Conciliacion?handler=Conciliar';
            this.accion(ruta);
            
        },
        bloquear() {
            ruta = 'Conciliacion?handler=Bloquear';
            this.accion(ruta);
        },
        movementRejected() {
            ruta = 'Conciliacion?handler=MovementReject';
            this.accion(ruta);
        },
        async accion(ruta) {

            if (this.boton_movement == false) {
                this.id_payment = null;
                this.id_movement = null;
            }
            var conciliar = {
                id: this.id_conciliado,
                name: this.id_movement
            }
            const data = await axios.post(ruta, conciliar, { headers: { "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val() } })
                .then(function (response) {
                    // handle success
                    resetTime();
                    return response.data;
                });

            this.boton_movement = true;
            if (typeof data === 'string' || data instanceof String) {
                if (data.includes("<!DOCTYPE html>")) {
                    window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired";
                    toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br />" + i18n.t("errorBaseDatos"));
                    return;
                }
            }

            if (data != null) {

                if (data.error == "You are not authorised to perform this action") {
                    toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br />" + i18n.t("tooltip.noAuthorizado"));
                    window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                }

                if (data.accion == "conciliar" && data.error == null) toastr.success(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br />" + i18n.t("tooltip.conciliarPago"));
                else if (data.accion == "bloq" && data.error == null) toastr.success(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br />" + i18n.t("tooltip.rechazarPago"));
                else if (data.accion == "movement" && data.error == null) toastr.success(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br />" + i18n.t("tooltip.movementPago"));
                else if (data.error == "Payment not found") toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br />" + i18n.t("tooltip.pagoNotFound"));
                else if (data.error == "Movement currency not equal") toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br />" + i18n.t("tooltip.currencyNotEqual"));
                else toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br />" + data.error);

                this.boton_movement = false;
                this.id_movement = null;
                this.id_payment = null;
                this.tablas = [];
                this.listaOptions = [];
                this.listaBuscarClientes = [];
                this.listaLoading = [];
                this.listaTotalClientes = [];

                for (var i = 0; i < this.currencies.length; i++) {

                    this.listaOptions.push({});
                    this.listaBuscarClientes.push(true);
                    this.listaLoading.push(false);
                    this.listaTotalClientes.push(0)

                    var aux = this.filter[i].currency
                    this.filter[i].currency = this.currencies[i].id
                    await this.llenar_consulta(i);
                    this.filter[i].currency = aux

                }
                await this.ConsultaMovements();

            } else {
                toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br />" + i18n.t("errorBaseDatos"));
            }
        },

        cargarArchivo: function (archivo) {
            var lector = new FileReader()

            try {
                lector.readAsText(archivo)

                this.ErroresArchivo.vacio = false;
                this.ErroresArchivo.maxSize = false;
                this.ErroresArchivo.formatoInvalido = false;
            } catch (e) {
                $("#facturaInputFile").val("")
                toastr.warning(i18n.t("errorLeerArchivo"))
            }

        },
        validarClickArchivo() {

            if (this.errorBanco == this.hasSuccess && this.errorMoneda == this.hasSuccess) {
                this.habilitarBtnFile = false;
            }
            else {
                this.habilitarBtnFile = true;
                toastr.error(i18n.t("tooltip.allCamposConciliarFile"));
            }
           
        },
        objetoTablaPrincipal(item) {
            this.datosConfirmacionPagosPorConciliar = item;
        },
        objetoTablaMovements(item) {
            this.datosConfirmacionMovements = item;
        },

        validacionBanco() {

            if (document.getElementById("IdBank") == null) {

                this.errorBanco = this.hasError;
                this.errorBancoTexto = i18n.t("valid.cuentaBanco");

            } else if (document.getElementById("IdBank").value == null || document.getElementById("IdBank").value == "") {

                this.errorBanco = this.hasError;
                this.errorBancoTexto = i18n.t("valid.cuentaBanco");

            } else {
                this.errorBanco = this.hasSuccess;
                this.errorBancoTexto = "";
            }

            if (this.formatoValido == true &&
                this.ErroresArchivo.formatoInvalido == false &&
                this.ErroresArchivo.vacio == false &&
                this.errorBanco == 'is-valid' &&
                this.errorMoneda == 'is-valid') this.dialogArchivo = true;
        },
        validacionMoneda() {
            if (document.getElementById("IdMoneda") == null) {

                this.errorMoneda = this.hasError;
                this.errorMonedaTexto = i18n.t("valid.cuentaMoneda");

            } else if (document.getElementById("IdMoneda").value == null || document.getElementById("IdMoneda").value == "") {

                this.errorMoneda = this.hasError;
                this.errorMonedaTexto = i18n.t("valid.cuentaMoneda");

            } else {
                this.errorMoneda = this.hasSuccess;
                this.errorMonedaTexto = "";
            }

            if (this.formatoValido == true &&
                this.ErroresArchivo.formatoInvalido == false &&
                this.ErroresArchivo.vacio == false &&
                this.errorBanco == 'is-valid' &&
                this.errorMoneda == 'is-valid') this.dialogArchivo = true;
        },
        validacionArchivo() {
            if (this.file !== null) {
                if (this.file.type == 'text/plain') {

                    if ((this.file.size / 1024) >= 400) {

                        toastr.warning(i18n.t("tamanoTxtInvalido"))

                    } else {
                        this.cargarArchivo(this.file);
                    }

                } else {
                    this.ErroresArchivo.formatoInvalido = false;
                    
                }
            }
        },
        validarFecha(dateString) {
            
            var regEx = /^\d{4}-\d{2}-\d{2}$/;
            if (!dateString.match(regEx)) return false;  // Invalid format
            var d = new Date(dateString);
            var dNum = d.getTime();
            if (!dNum && dNum !== 0) return false; // NaN value, Invalid date
            return d.toISOString().slice(0, 10) === dateString;
            
        },
        resetValid() {

            this.ErroresContenidoFile.fecha = true;
            this.ErroresContenidoFile.moneda = true;
            this.ErroresContenidoFile.monto = true;
            this.ErroresContenidoFile.numeroCuenta = true;
            this.ErroresContenidoFile.referencia = true;
            this.ErroresContenidoFile.formato = true;
            this.formatoValido = false;
            document.getElementById("file").value = null;
            this.file = null;
        },
        openFile(file) {
           
            try {

                var lector = new FileReader()
                lector.onload = (res) => {

                    let elementos = [];
                    let subElementos = [];
                    const archivo = res.target.result;
                    let errorFormato = false;

                    elementos = archivo.split(/\r?\n/)
                    elementos.map(dato => {
                        subElementos = dato.split(";")

                        if (subElementos.length != 5 && errorFormato == false) {
                            toastr.error("No Cumple con el Formato Requerido");
                            this.ErroresContenidoFile.formato = false;
                            errorFormato = true;
                        }
                        var ExpreNumeroCuenta = new RegExp(/^[A-Za-z0-9\s]+$/g);
                        var pattern = /^\d+(\.\d{1,2})?$/;
                        let contadorMoneda = 0;
                        let idMoneda = null;

                        let fecha = subElementos[0];
                        let iso_4217 = subElementos[1];
                        let numCuenta = subElementos[2];
                        let monto = subElementos[3];
                        let referencia = subElementos[4];

                        this.currencies.map(moneda => {
                            if (moneda.iso_4217 == iso_4217) {
                                contadorMoneda++;
                                idMoneda = moneda.id;
                            }
                        });

                        if (document.getElementById("IdMoneda").value != idMoneda) this.ErroresContenidoFile.moneda = false;
                        //if (contadorMoneda == 0) this.ErroresContenidoFile.moneda = false;
                        if (this.validarFecha(fecha) == false) this.ErroresContenidoFile.fecha = false;
                        if (pattern.test(monto) == false) this.ErroresContenidoFile.monto = false;
                        if (ExpreNumeroCuenta.test(numCuenta) == false) this.ErroresContenidoFile.numeroCuenta = false;

                    });

                    if (this.ErroresContenidoFile.fecha == false) toastr.error(i18n.t("tooltip.ArchivoformatoFechaPago") + " (AAAA-MM-DD)");
                    else if (this.ErroresContenidoFile.moneda == false) toastr.error(i18n.t("tooltip.ArchivoformatoTipoMoneda"));
                    else if (this.ErroresContenidoFile.monto == false) toastr.error(i18n.t("tooltip.ArchivoformatoMontoPagado") + " (99999.99)");
                    else if (this.ErroresContenidoFile.numeroCuenta == false) toastr.error(i18n.t("tooltip.formatoCuentaBancaria"));

                    if (this.ErroresContenidoFile.formato == true && this.ErroresContenidoFile.numeroCuenta == true && this.ErroresContenidoFile.monto == true && this.ErroresContenidoFile.moneda == true && this.ErroresContenidoFile.fecha == true) {
                        this.formatoValido = true;
                    }
                    else {
                        this.resetValid();
                    }

                    if (this.formatoValido == true &&
                        this.ErroresArchivo.formatoInvalido == false &&
                        this.ErroresArchivo.vacio == false &&
                        this.errorBanco == 'is-valid' &&
                        this.errorMoneda == 'is-valid') this.dialogArchivo = true;

                }

                lector.readAsText(file);
                
            } catch (e) {
                $("#file").val("")
                toastr.warning(i18n.t("errorLeerArchivo"))
            }
        },
        handleFileUpload() {

            this.file = this.$refs.file.files[0];
            this.validacionArchivo();
            this.openFile(this.file)
        },
        async recargarTablas() {

            this.boton_movement = false;
            this.id_movement = null;
            this.id_payment = null;
            this.tablas = [];
            this.listaOptions = [];
            this.listaBuscarClientes = [];
            this.listaLoading = [];
            this.listaTotalClientes = [];

            for (var i = 0; i < this.currencies.length; i++) {

                this.listaOptions.push({});
                this.listaBuscarClientes.push(true);
                this.listaLoading.push(false);
                this.listaTotalClientes.push(0)

                var aux = this.filter[i].currency
                this.filter[i].currency = this.currencies[i].id
                await this.llenar_consulta(i);
                this.filter[i].currency = aux

            }
            await this.ConsultaMovements();

        },
        async submitFile() {
            var formData = new FormData();
            var idBanco = document.getElementById("IdBank").value;
            var idMoneda = document.getElementById("IdMoneda").value;
            formData.append('File', this.file);
            formData.append('IdBank', idBanco);
            formData.append('IdMoneda', idMoneda);
            
            const data = await axios.post("Conciliacion?handler=Archivo", formData, { headers: { "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val() } })
            .then(function (response) {

                resetTime();
                return response.data;
            });

            if (data == "successfull") {
                toastr.success(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br />" + i18n.t("tooltip.archivoConciliar"))
                document.getElementById("IdBank").value = '';
                document.getElementById("IdMoneda").value = '';
                this.file = null;
                this.ErroresArchivo.formatoInvalido = true;
                this.ErroresArchivo.vacio = true;
                this.errorBanco = '';
                this.errorBancoTexto = '';
                this.errorMoneda = '';
                this.errorMonedaTexto = '';
                this.resetValid();

                //var t = setTimeout(this.recargarTablas, 70000);
                //clearTimeout(t);

            } else if (data == "You are not authorised to perform this action") {
                toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br />" + i18n.t("tooltip.noAuthorizado"));
                window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"

            } else {
                toastr.error(data)
            }
             
        },

    },
    computed: {
        
    },
    mounted: async function () {
        tiempoLogin(this.modalLogout)
        for (var i = 0; i < this.currencies.length; i++) {
            await this.llenar_consulta(i)
        }
        //await this.ConsultaMovements();
    },
});