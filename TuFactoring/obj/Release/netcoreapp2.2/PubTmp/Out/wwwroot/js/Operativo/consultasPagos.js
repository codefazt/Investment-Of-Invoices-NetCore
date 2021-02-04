new Vue({
    el: "#appConsultasPagosBackoffice",
    i18n,
    vuetify: new Vuetify({
        lang: {
            t: (key, ...params) => i18n.t(key, params)
        }
    }),
    data: {
        modalLogout: { mostrar: false },
        i18n: i18n,
        respondQuery: [],
        widthTelefono: widthTelefono,
        formatoMonedaInput: formatoMonedaInput,
        formatoMoneda: formatoMoneda,
        backEndDateFormat: backEndDateFormat,
        headersReceipts: [
            { text: i18n.t("headers.n"), value: "n", align: "center" },
            { text: i18n.t("headers.receiptDate"), value: "receipt_date", align: "center" },
            { text: i18n.t("headers.bankConfirming"), value: "entity.person.name", align: "center" },
            { text: i18n.t("headers.payReceivedBy"), value: "receiver.name", align: "center" },
            { text: i18n.t("headers.coin"), value: "coin", align: "center" },
            { text: i18n.t("headers.amountToPay"), value: "valorneto", align: "center" },
            { text: i18n.t("headers.amountPaid"), value: "total", align: "center" },
            { text: i18n.t("headers.estatus"), value: "state", align: "center" },
            { text: i18n.t("headers.opciones"), value: "detail", align: "center" },
        ],
        headerFacturasFactor: [
            { text: i18n.t("headers.n"), value: "n", align: "center" },
            { text: i18n.t("headers.proveedor"), value: "invoice.supplier.name", align: "center" },
            { text: i18n.t("headers.cliente"), value: "invoice.debtor.name", align: "center" },
            { text: i18n.t("headers.numeroFactura"), value: "invoice.number", align: "center" },
            { text: i18n.t("headers.fechaVencimiento"), value: "expiration_date", align: "center" },
            { text: i18n.t("headers.valorNeto"), value: "valorneto", align: "center" },
            { text: i18n.t("headers.comisionServicio"), value: "commission", align: "center" },
            { text: i18n.t("headers.discount"), value: "discount", align: "center" }
        ],
        headerPayments: [
            { text: i18n.t("headers.n"), value: "n", align: "center" },
            { text: i18n.t("headers.paymentDate"), value: "payment_date", align: "center" },
            { text: i18n.t("headers.receiptBank"), value: "entity.person.name", align: "center" },
            { text: i18n.t("headers.accountNumber"), value: "account_number", align: "center" },
            { text: i18n.t("headers.referenceNumber"), value: "number", align: "center" },
            { text: i18n.t("headers.amountPayment"), value: "amount", align: "center" },
            { text: i18n.t("headers.estatus"), value: "state", align: "center" },
        ],
        accounts: [],
        receipts: [],
        banks: [],
        firstArray: [],
        settings: [],
        dataFacturas: {},
        dataPagos: {},
        selected: {},
        envio:false,
        errorProveedor:false,
        dialogTransfer: false,
        dialogDetails: false,
        dialogDetailsPayments:false,
        dialogSeguro: false,
        errorReference: false,
        textoErrorReference: "",
        errorAmount: false,
        filterData: [],
        textoErrorAmount: "",
        errorAccountNumber: false,
        textoErrorAccountNumber: "",
        textoRefrescar: "",
        loading: true,
        cargando: true,
    },
    created: function () {

        this.lang = document.getElementsByTagName("html")[0].getAttribute("lang")

        try {
            this.filter = JSON.parse(document.getElementById('filterData').value);
            document.getElementById("eliminarData").removeChild(document.getElementById('filterData'));

        } catch (e)
        {
            this.filter = null
            console.log(e)
        }
        document.getElementById("contenido").removeAttribute("hidden")
        this.loading = false
        this.cargando = false
        tiempoLogin(this.modalLogout)
    },
    mounted: async function () {
        await this.payedInvoices();
        await this.unpayedInvoices();
        tiempoLogin(this.modalLogout)
    },
    methods: {

        async payedInvoices(factura) {

            if (this.loading) return
            this.loading = true
            let take = 100;
            const data = await axios.post('ConsultasPagos?handler=PayedInvoices', { pagination: { take: take, skip: this.totalFacturas }, filter: this.filter }, { headers: { "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val() } })
                .then(function (response) {
                    // handle success
                    resetTime()
                    return response;
                });

            if (data.data.length == 0) {
                this.loading = false
                this.buscarFacturas = false
            }

            if (data.data.length > 0 && data.data[0].errors == notAuthorized) {
                window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                return
            }

            if (data.data.length < (take / 2)) this.buscarFacturas = false
            this.totalFacturas += parseInt(data.data.length)

            data.data.map(data => {
                this.respondQuery.push(data)
            })

            this.loading = false;
            return this.buscarFacturas
        },

        async unpayedInvoices(factura) {

            if (this.loading) return
            this.loading = true
            let take = 100;
            const data = await axios.post('ConsultasPagos?handler=UnpayedInvoices', { pagination: { take: take, skip: this.totalFacturas }, filter: this.filter }, { headers: { "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val() } })
                .then(function (response) {
                    // handle success
                    resetTime()
                    return response;
                });

            if (data.data.length == 0) {
                this.loading = false
                this.buscarFacturas = false
            }

            if (data.data.length > 0 && data.data[0].errors == notAuthorized) {
                window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                return
            }

            if (data.data.length < (take / 2)) this.buscarFacturas = false
            this.totalFacturas += parseInt(data.data.length)

            data.data.map(datos => {
                this.respondQuery.push(datos)
            })

            this.loading = false;
            return this.buscarFacturas
        },

        DetalleFacturas(item) {
            this.dataFacturas = item
            this.dialogDetails = true
        },

        DetallePagos(item) {
            this.dataPagos = item
            this.dialogDetailsPayments = true
        },

        BotonPago(item) {
            this.selected = item
            this.dialogSeguro = true
        },

        DatosPanelTransferir(item) {
            this.selected = item
            this.selected.transfer = {}
            this.selected.transfer.amount = formatoMonedaInput(item.amount - item.paid, this.lang, this.selected.currency.digits)
            this.textoErrorReference = ""
            this.textoErrorAccountNumber = ""
            this.textoErrorAmount = ""


            this.dialogTransfer = true
        },

        formatear() {
            this.errorAmount = false
            this.textoErrorAmount = ""
            var amount = formatoMoneda(this.selected.transfer.amount, this.lang)

            if (amount != null && amount != "") {
                amount = amount + ""
                if (amount.split(".")[0].length > 13) {
                    this.errorAmount = true
                    this.textoErrorAmount = i18n.t("montoMayorSuperior")

                }

                amount = parseFloat(amount)
            }


            if (!this.errorAmount) {
                var amountCompare = new Intl.NumberFormat("en", { maximumFractionDigits: 2, }).format(this.selected.amount - this.selected.paid)

                if (amount <= 0) {
                    this.errorAmount = true
                    this.textoErrorAmount = i18n.t("invalidAmount")
                } else if (amount > amountCompare) {
                    this.errorAmount = true
                    this.textoErrorAmount = i18n.t("montoPagarSuperior")
                }
            }

            this.selected.transfer.amount = formatoMonedaInput(amount, this.lang, this.selected.currency.digits)

            this.refrescar()
        },

        refrescar() {
            this.textoRefrescar = "|"
            setTimeout(500, this.textoRefrescar = "")
        },

        validarReferencia() {
            let emailRegex = /^_\-\.\,\&\%\#\!\*\(\)\$\:\;\[\]\{\}\"\'\s\xF1\xD1]+$/;


            if (this.selected.transfer.number == null ||
                this.selected.transfer.number == '') {

                this.errorReference = true;
                this.textoErrorReference = 'Introduzca N° de Refrencia.';

            } else if (emailRegex.test(this.selected.transfer.number)) {

                this.errorReference = true;
                this.textoErrorReference = 'El Formato que introdujo es Erroneo';

            } else if (this.selected.transfer.number.length < 10) {

                this.errorReference = true;
                this.textoErrorReference = 'minimo 10 numeros';

            } else if (this.selected.transfer.number.length > 255) {

                this.errorReference = true;
                this.textoErrorReference = 'max 255 letras';

            } else {
                this.errorReference = false;
                this.textoErrorReference = '';
            }

            this.refrescar()
        },

        montoPagar() {
            if (this.selected.paying_account != null) {
                return formatoMonedaInput(this.selected.amount, this.lang, this.selected.currency.digits)
            }

            return formatoMonedaInput(formatoMoneda(this.selected.transfer.amount, this.lang), this.lang, this.selected.currency.digits)
        },
        //
        obtenetValorNet(array) {
            var sumatoria = 0

            array.map(data => {
                if (data.invoice.amount != null)
                    sumatoria += data.invoice.amount
            })

            return sumatoria
        }
    },
})