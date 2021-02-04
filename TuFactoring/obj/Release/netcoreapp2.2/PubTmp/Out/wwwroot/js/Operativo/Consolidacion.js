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
            { text: i18n.t("headers.dataPaid"), value: 'payment_date', align: 'center' },
            { text: i18n.t("headers.bankConfirming"), value: 'img', align: 'center' },
            { text: i18n.t("headers.cuentaReceptora"), value: 'receiving_account_name', align: 'center' },
            { text: i18n.t("headers.payer"), value: 'payer' },
            { text: i18n.t("headers.amountPaid"), value: 'amount' },
            { text: i18n.t("headers.referencia"), value: 'referencia', align: 'center' },
            { text: i18n.t("headers.opciones"), value: 'action', align: 'center' },
        ],
        dialogConciliar: false,
        dialogBloquear: false,
        buscarClientes: true,
        totalClientes: null,
        DatosResividos: [],
        id_conciliado: null,
        moment: moment,
        envio: false,
        loading: false,
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
        ErroresArchivo: {
            vacio: true,
            formatoInvalido: true,
            maxSize: true
        },
        errorBanco: '',
        errorBancoTexto: '',
        hasError: 'is-invalid',
        hasSuccess: 'is-valid',
    },
    watch: {
        listaOptions: {
            async handler() {
                console.log("detect")

                if (this.tablas.length > 0) {

                    for (let i = 0; i < this.listaOptions.length; i++) {
                        if (this.listaBuscarClientes[i] && this.listaOptions[i].page * this.listaOptions[i].itemsPerPage >= this.tablas[i].length - this.listaOptions[i].itemsPerPage) {

                            var aux = this.filter[i].currency

                            this.filter[i].currency = this.currencies[i].id
                            await this.llenar_consulta(i);

                            this.filter[i].currency = aux

                        }
                    }
                }
            },
            deep: true,
        },
    },
    created: function () {

        try {

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
                    console.log(response.data)
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
        conciliar() {
            ruta = 'Conciliacion?handler=Conciliar';
            this.accion(ruta);
            
        },
        bloquear() {
            ruta = 'Conciliacion?handler=Bloquear';
            this.accion(ruta);
        },
        async accion(ruta) {

            var conciliar = { id: this.id_conciliado }
            const data = await axios.post(ruta, conciliar, { headers: { "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val() } })
                .then(function (response) {
                    // handle success
                    return response.data;
                });

            if (typeof data === 'string' || data instanceof String) {
                console.log(data);
                if (data.includes("<!DOCTYPE html>")) {
                    window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired";
                    toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br />" + i18n.t("errorBaseDatos"));
                    return;
                }
            }

            if (data != null) {

                if (data.accion == "conciliar") toastr.success(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br />" + i18n.t("tooltip.conciliarPago"));
                else if (data.accion == "bloq") toastr.success(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br />" + i18n.t("tooltip.rechazarPago"));

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

                    console.log("Vueltas al Poner Limite")
                    console.log(this.filter[i].currency)

                }

            } else {
                toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br />" + i18n.t("errorBaseDatos"));
            }
        },


        cargarArchivo: function (archivo) {
            var lector = new FileReader()

            try {
                console.log(archivo)
                lector.readAsText(archivo)

                this.ErroresArchivo.vacio = false;
                this.ErroresArchivo.maxSize = false;
                this.ErroresArchivo.formatoInvalido = false;
            } catch (e) {
                $("#facturaInputFile").val("")
                toastr.warning(i18n.t("errorLeerArchivo"))
            }

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

            if (this.ErroresArchivo.formatoInvalido == false && this.ErroresArchivo.vacio == false && this.errorBanco == 'is-valid') this.dialogArchivo = true;
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
        handleFileUpload() {
            this.file = this.$refs.file.files[0];
            this.validacionArchivo();

            if (this.ErroresArchivo.formatoInvalido == false && this.ErroresArchivo.vacio == false && this.errorBanco == 'is-valid') this.dialogArchivo = true;
        },
        async submitFile() {
            var formData = new FormData();
            var idBanco = document.getElementById("IdBank").value;
            formData.append('File', this.file);
            formData.append('IdBank', idBanco);
            
            const data = await axios.post("Conciliacion?handler=Archivo", formData, { headers: { "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val() } })
            .then(function (response) {
                
                return response.data;
            });

            if (data == "successfull") {
                toastr.success(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br />" + i18n.t("tooltip.archivoConciliar"))
                document.getElementById("IdBank").value = '';
                //$("#IdBank").val("")
                this.file = null;
                this.ErroresArchivo.formatoInvalido = true;
                this.ErroresArchivo.vacio = true;
                this.errorBanco = '';
                this.errorBancoTexto = '';

            } else {
                toastr.success(data)
            }
             
        },

    },
    computed: {
        
    },
    mounted: async function () {
        for (var i = 0; i < this.currencies.length; i++) {
            await this.llenar_consulta(i)
        }
    },
});