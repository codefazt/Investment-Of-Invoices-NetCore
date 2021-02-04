// JavaScript con Vue Segmentacion

var app = new Vue({
    el: '#appPagoFacturasBanco',
    i18n,
    store: vuexLayout,
    vuetify: new Vuetify({
        lang: {
            t: (key, ...params) => i18n.t(key, params)
        }
    }),
    data: {
        tamanoTlf: tamanoTlf,
        mensajeSeguro: false,
        // Comienzo de Datos para Menu Principal
        formatoMonedaInput: formatoMonedaInput,
        cerrarMordisco: true,
        fav: true,
        menu: false,
        message: false,
        hints: true,
        Mensajeria: {
            notificaciones: true,
            mensajes: true,
        },
        drawer: false,
        detalleFactura: false,
        // fin de Datos para Menu Principal
        headerPago: [
            { text: 'Pagar A', value: 'facturas' },
            { text: 'Cantidad de Facturas', value: 'invoicescount', align: 'center' },
            { text: 'Monto a Pagar', value: 'monto', align: 'right' },
            { text: 'Detalles', value: 'detalles', align: 'center' },
            { text: 'Pagar', value: 'action', align: 'center' },
        ],
        headerDatosFacturas: [
            { text: 'Proveedores', value: 'clientname' },
            { text: 'Numero de factura', value: 'number', align: 'center' },
            { text: 'Monto de la factura', value: 'monto', align: 'center' },
        ],
        detalleTransferencia: false,
        detalleBotonPago: false,
        page: 1,
        pageCount: 10,
        DatosPagos: [],
        sybolSelected: '',
        codigo_isoSelected: '',
        digitsSelected: '',
        numeroCuentaSeleccionada: '',
        cargando: true,
        // Validar Seleccion de Banco Ventana Transferir
        x: null,
        Errorbank_id: '',
        TextoErrorBank_id: '',

        DatosFacturas: [],
        DatosTraferencia: {
            suppliername: '',
            doucumentoIdentidad: '',
            numeroCuenta: '',
            nombreBancoConfirmante: '',
            sumamount: ''
        },

        AlliedAccounts: [],

        // Validar numero de recibo Bancario
        ErrorReference: '',
        TextoErrorReference: '',

        // Validar numero de recibo Bancario
        ErrorImgRecursos: '',
        TextoErrorImgRecursos: '',

         // Validar Banco de recibo Bancario
        ErrorBankReference: '',
        ErrorBankReferenceTexto: '',
        hasSuccess: 'is-valid',
        hasError: 'is-invalid'

    },
    created: function () {
        this.DatosPagos = JSON.parse(document.getElementById('contenidoRaw').value);
        document.getElementById('contenido').removeAttribute('hidden');
        this.cargando = false;
    },
    methods: {
        limpiarMensajes: function () {
            vuexLayout.commit("limpiarMensajes")
        },
        validarBancoTransferencia() {

            if (this.DatosPagos.payments.bank_id == null || this.DatosPagos.payments.bank_id == '') {

                this.Errorbank_id = this.hasError;
                this.TextoErrorBank_id = 'Por favor elija un banco para transferir';

            } else {
                this.Errorbank_id = this.hasSuccess;
                this.TextoErrorBank_id = '';

                for (let i = 0; i < this.DatosPagos.alliedAccounts.cuentas.length; i++) {

                    if (this.DatosPagos.payments.bank_id == this.DatosPagos.alliedAccounts.cuentas[i].idbanco) {
                        this.numeroCuentaSeleccionada = this.DatosPagos.alliedAccounts.cuentas[i].cuenta;
                    }

                }
            }

        },
        validarBancoReferenciaTransferencia() {

            if (this.DatosPagos.payments.idbankdst == null || this.DatosPagos.payments.idbankdst == '') {

                this.ErrorBankReference = this.hasError;
                this.ErrorBankReferenceTexto = 'Por favor elija banco donde se realizo la Transferencia';

            } else {
                this.ErrorBankReference = this.hasSuccess;
                this.ErrorBankReferenceTexto = '';
            }
        },
        validarCapture(event) {
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

                                this.ErrorImgRecursos = this.hasSuccess;
                                this.TextoErrorImgRecursos = '';
                                this.DatosPagos.payments.comment = evt.target.result;

                            }
                        }

                    } else {

                        document.getElementById("file").value = "";
                        this.ErrorImgRecursos = this.hasError;
                        this.TextoErrorImgRecursos = 'por favor solo formato jpg o png';
                        //alert("por favor solo imagenes .jpeg /.png");
                    }

                }


            } else {
                document.getElementById("file").value = "";
                this.ErrorImgRecursos = this.hasError;
                this.TextoErrorImgRecursos = 'por favor solo formato jpg o png';
                //alert("por favor solo imagenes .jpeg /.png");
            }
        },
        validarReferencia() {

            let emailRegex = /^_\-\.\,\&\%\#\!\*\(\)\$\:\;\[\]\{\}\"\'\s\xF1\xD1]+$/;

            if (this.DatosPagos.payments.reference === '' ||
                this.DatosPagos.payments.reference == null ||
                this.DatosPagos.payments.reference[0] == ' ') {

                this.ErrorReference = this.hasError;
                this.TextoErrorReference = 'Introduzca N° de Refrencia.';

            } else if (emailRegex.test(this.DatosPagos.payments.reference)) {

                this.ErrorReference = this.hasError;
                this.TextoErrorReference = 'El Formato que introdujo es Erroneo';

            } else if (this.DatosPagos.payments.reference.length < 10) {

                this.ErrorReference = this.hasError;
                this.TextoErrorReference = 'minimo 10 numeros';

            } else if (this.DatosPagos.payments.reference.length > 255) {

                this.ErrorReference = this.hasError;
                this.TextoErrorReference = 'max 255 letras';

            } else {
                this.ErrorReference = this.hasSuccess;
                this.TextoErrorReference = '';

            }
        },
        CerrarModal() {

            this.detalleBotonPago = false;
            this.detalleTransferencia = false;
            //Limpiar datos del Banco
            this.DatosPagos.payments.bank_id = null;
            this.TextoErrorBank_id = '';
            this.Errorbank_id = '';
            this.ErrorBankReference = '';
            this.numeroCuentaSeleccionada = '';
            //Limpiar datos de la Referencia
            this.DatosPagos.payments.idbankdst = null;
            this.DatosPagos.payments.reference = '';
            this.TextoErrorReference = '';
            this.ErrorReference = '';
            this.ErrorBankReferenceTexto = '';
        },
        DatosPanelTransferir(TuFactory) {

            this.detalleTransferencia = true;

            this.sybolSelected = TuFactory.currencysymbol;
            this.codigo_isoSelected = TuFactory.currencyiso;
            this.digitsSelected = TuFactory.currencydigits;

            // Meter datos en payments para mutacion
            this.DatosPagos.payments.amount_pay = TuFactory.sumamount;
            this.DatosPagos.payments.type = TuFactory.statusbotonpago;
         
            for (var i = 0; i < TuFactory.invoices.length; i++) {

                this.DatosPagos.payments.invoices.push({
                    invoice_id: TuFactory.invoices[i].id
                });

                //this.DatosPagos.payments.invoices[i].invoice_id = facturas[i].id;
            }

        },
        BotonPago(TuFactory) {

            this.detalleBotonPago = true;

            this.DatosTraferencia.numeroCuenta = TuFactory.numberacountuserbank;

            this.DatosTraferencia.nombreBancoConfirmante = TuFactory.nameBankConfirmant;
            this.DatosTraferencia.sumamount = TuFactory.sumamount;
            this.sybolSelected = TuFactory.currencysymbol;
            this.codigo_isoSelected = TuFactory.currencyiso;
            this.digitsSelected = TuFactory.currencydigits;

            //this.DatosPagos.payments es para la mutacion de Pago de Facturas
            this.DatosPagos.payments.bank_id = TuFactory.idbancoconfirmante; // Id del banco donde se realizara la transferencia
            this.DatosPagos.payments.idbankdst = TuFactory.idbancoconfirmante; // Id del banco donde se Debitara para despues Transferir

            this.DatosPagos.payments.cuentaorg = TuFactory.numberacountuserbank; // numero de cuenta del usuario es decir el origen

            this.DatosPagos.payments.amount_pay = TuFactory.sumamount;
            this.DatosPagos.payments.type = TuFactory.statusbotonpago;
            for (var i = 0; i < TuFactory.invoices.length; i++) {

                this.DatosPagos.payments.invoices.push({
                    invoice_id: TuFactory.invoices[i].id
                });

                
            }
        },
        DetalleFacturas(Pago) {

            this.detalleFactura = true;
            this.DatosFacturas = [];

            for (var i = 0; i < Pago.invoices.length; i++) {
                this.DatosFacturas.push({
                    id: Pago.invoices[i].id,
                    clientname: Pago.invoices[i].suppliername,
                    number: Pago.invoices[i].number,
                    amount: Pago.invoices[i].amount,
                    currencysymbol: Pago.currencysymbol,
                    currencyiso: Pago.currencyiso,
                    currencydigits: Pago.currencydigits,
                });
            }

        },
        DetalleFacturasCliente(Pago) {

            this.detalleFactura = true;
            this.DatosFacturas = [];

            for (var i = 0; i < Pago.invoices.length; i++) {
                this.DatosFacturas.push({
                    id: Pago.invoices[i].id,
                    clientname: Pago.invoices[i].clientname,
                    number: Pago.invoices[i].number,
                    amount: Pago.invoices[i].amount,
                    currencysymbol: Pago.currencysymbol,
                    currencyiso: Pago.currencyiso,
                    currencydigits: Pago.currencydigits,
                });
            }

        },

        async PagarFactura(payments) {

            const data = await axios.post('', payments, { headers: { "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val() } })
                .then(function (response) {
                    stringRespuesta = JSON.parse(response);
                    return stringRespuesta;

                }).catch(err => {
                    toastr.error("No se pudo conectar a base de datos");
                });


            if (data.data != "Error") {
                // handle success
                toastr.success("El pago se ha realizado satisfactoriamente.");
                this.detalleTransferencia = false;
                this.CerrarModal();
                if (data.data != "'[]'" && data.data != "[]" && data.data != [] && data.data != null) this.DatosPagos.facturas = data.data;
                else this.DatosPagos.facturas = [];
                

            } else {
                toastr.error("Numero de Referencia Existente", "Error de Pago");
            }
        }

        //---------------------- Metodos de Vuex -------------------------------------------

        //---------------------- Metodos de Vuex -------------------------------------------
    },
    computed: {
        habilitarBotonTransferir() {

            if (this.ErrorBankReference == this.hasSuccess &&
                this.Errorbank_id == this.hasSuccess &&
                this.ErrorReference == this.hasSuccess &&
                this.ErrorImgRecursos == this.hasSuccess) {

                return false;
            } else {
                return true;
            }
        },
        mensajesComputed() {
            return vuexLayout.state.mensajes
        }
    }
});