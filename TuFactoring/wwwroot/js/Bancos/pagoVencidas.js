new Vue({
    el: "#appPagoVencidas",
    i18n,
    vuetify: new Vuetify({
        lang: {
            t: (key, ...params) => i18n.t(key, params)
        }
    }),
    data: {
        modalLogout: { mostrar: false },
        formatoMonedaInput: formatoMonedaInput,
        stateProcessing: false,
        stateUnpaid: false,
        statePaid: false,
        formatoMoneda: formatoMoneda,
        backEndDateFormat: backEndDateFormat,
        headersExpireds: [
            { text: i18n.t("headers.n"), value: "n", align: "center" },
            { text: i18n.t("headers.program"), value: "program.abbreviation", align: "center" },
            { text: i18n.t("headers.receiptDate"), value: "receipt_date", align: "center" },
            { text: i18n.t("headers.payer"), value: "payer.name", align: "center" },
            { text: i18n.t("headers.receiver"), value: "receiver.name", align: "center" },
            { text: i18n.t("headers.amountToPay"), value: "amount", align: "center" },
            { text: i18n.t("headers.detail"), value: "detail", align: "center" },
            { text: i18n.t("headers.opciones"), value: "pay", align: "center" }
        ],
        headersRencociled: [
            { text: i18n.t("headers.n"), value: "n", align: "center" },
            { text: i18n.t("headers.receiptDate"), value: "receipt_date", align: "center" },
            { text: i18n.t("headers.payer"), value: "payer.name", align: "center" },
            { text: i18n.t("headers.receiver"), value: "receiver.name", align: "center" },
            { text: i18n.t("headers.amountToPay"), value: "amount", align: "center" },
            { text: i18n.t("headers.detail"), value: "detail", align: "center" },
            { text: i18n.t("headers.opciones"), value: "pay", align: "center" }
        ],
        headerDetailPublications: [
            { text: i18n.t("headers.n"), value: "n", align: "center" },
            { text: i18n.t("headers.proveedorCliente"), value: "invoice.supplier.name", align: "center" },
            { text: i18n.t("headers.numeroFactura"), value: "invoice.number", align: "center" },
            { text: i18n.t("headers.originalAmount"), value: "valornominal", align: "center" },
            { text: i18n.t("headers.valorNeto"), value: "valorneto", align: "center" },
            { text: i18n.t("headers.amountToPay"), value: "amountpay", align: "center" },
            { text: i18n.t("headers.financing"), value: "request_financing", align: "center" },
        ],
        receipts: [],
        banks: [],
        reconcileds: [],
        expireds: [],
        settings: [],
        dialogConfirmTransfer: false,
        dialogConfirmPayment:false,
        datos: {},
        processing: false,
        selected: {},
        dialogTransfer: false,
        pruebaExpireds: [],
        dialogDetails: false,
        pagoConfirmado: {
            iso_4217: '',
            symbol: '',
            digits: 0,
            amountTransfer: 0,
            amountButton: 0,
        },
        dialogSeguro: false,
        errorReference: false,
        textoErrorReference: "",
        errorAmount: false,
        textoErrorAmount: "",
        errorAccountNumber: false,
        textoErrorAccountNumber: "",
        textoRefrescar: "",
        loading: true,
        cargando: true,
    },
    created: function () {
        var dataOverdue = JSON.parse(document.getElementById("jsonOverdue").value)

        if (dataOverdue.length > 0 && dataOverdue[0].errors == notAuthorized) {
            window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
            return
        }

        for (let x = 0; x < dataOverdue.length; x++) {
            if (dataOverdue != "" && dataOverdue[x].state == "draft" || dataOverdue != "" && dataOverdue[x].state == "processing" || dataOverdue != "" && dataOverdue[x].state == "unpaid") {
                this.expireds.push(dataOverdue[x])
            }
        }

        this.lang = document.getElementsByTagName("html")[0].getAttribute("lang")

        tiempoLogin(this.modalLogout)

        document.getElementById("contenido").removeAttribute("hidden")
        try {

            document.getElementById("appPagoVencidas").removeChild(document.getElementById('jsonOverdue'))

        } catch (e) { console.log(e) }
        this.loading = false
        this.cargando = false
    },
    methods: {
        DetalleFacturas(item) {
            this.datos = item
            this.dialogDetails = true
        },
        BotonPago(item) {
            this.selected = item
            this.dialogConfirmTransfer = true
        },
        secondConfirmation(item) {
            this.pagoConfirmado = item
            this.pagoConfirmado.symbol = item.currency.symbol
            this.pagoConfirmado.iso_4217 = item.currency.iso_4217
            this.pagoConfirmado.digits = item.currency.digits
            this.pagoConfirmado.amountButton = item.amount
            this.dialogConfirmPayment = true
        },
        DatosPanelTransferir(item) {
            this.selected = item
            this.selected.transfer = {}
            this.dialogConfirmTransfer = true
        },
        formatear() {
            this.errorAmount = false
            this.textoErrorAmount = ""
            var amount = formatoMoneda(this.selected.transfer.amount, this.lang)

            if (amount > this.selected.amount || amount <= 0) {
                this.errorAmount = true
                this.textoErrorAmount = i18n.t("invalidAmount")
            }

            this.selected.transfer.amount = formatoMonedaInput(amount, this.lang, this.selected.currency.digits)

            this.refrescar()
        },
        refrescar() {
            this.textoRefrescar = "|"
            setTimeout(500, this.textoRefrescar = "")
        },
        montoPagar() {
            if (this.selected.paying_account != null) {
                return formatoMonedaInput(this.selected.amount, this.lang, this.selected.currency.digits)
            }
            console.log(formatoMonedaInput(formatoMoneda(this.selected.transfer.amount, this.lang), this.lang, this.selected.currency.digits))

            return formatoMonedaInput(formatoMoneda(this.selected.transfer.amount, this.lang), this.lang, this.selected.currency.digits)
        },
        realizarPago(item) {
            if (item.abbreviation == "OVERDUE") {
                this.pagarFacturaOverdue(item)
            }
        },

        obtenerValorNominal(array) {
            var sumatoria = 0

            if (array == null || array == undefined) return sumatoria

            array.map(data => {
                if (data.invoice.original_amount != null)
                    sumatoria += data.invoice.original_amount
            })

            return sumatoria
        },

        obtenerValorNet(array) {
            var sumatoria = 0

            if (array == null || array == undefined) return sumatoria

            array.map(data => {
                if (data.invoice.amount != null)
                    sumatoria += data.invoice.amount
            })

            return sumatoria
        },
        obtenerAmountDiscount(array) {
            var sumatoria = 0

            if (array == null || array == undefined) return sumatoria

            array.map(data => {
                if (data.earnings) {
                    sumatoria += data.earnings
                }
            })

            return sumatoria
        },
        obtenerAmountCommission(array) {
            var sumatoria = 0

            if (array == null || array == undefined) return sumatoria


            array.map(data => {
                if (data.commission != null)
                    sumatoria += data.commission
            })


            return sumatoria
        },
        //Pago overdued's
        async pagarFacturaOverdue(item) {
            if (this.loading) {
                toastr.warning(i18n.t("proccessInProgress"))
                return
            }
            this.loading = true

            await axios.post('?handler=pay', { receipt: item.id },
                {
                    headers: {
                        "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                    }
                })
                .then((respond) => {
                    resetTime()
                    this.loading = false

                    if (respond.data != null && respond.data.errors == notAuthorized) {
                        window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                        return
                    }

                    if (respond.data == null || respond.data.errors == "Error") {
                        toastr.warning(i18n.tc("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.tc("mensajesModal.aplicacionDePagoError"))
                        return
                    }

                    if (respond.data.state == "processing") {
                        for (let x = 0; x < this.expireds.length; x++) {
                            if (this.expireds[x].id == item.id) {
                                this.expireds[x].state = "processing"
                            }
                        }
                        this.stateProcessing = true
                        toastr.success(i18n.t("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.t("mensajesModal.paymentRealized"))
                        return
                    } else if (respond.data.state == "paid") {
                        this.expireds.splice(this.expireds.indexOf(item), 1)
                        toastr.success(i18n.t("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.t("mensajesModal.paymentRealized"))
                        return
                    } else if (respond.data.state == "unpaid") {
                        for (let i = 0; i < this.expireds.length; i++) {
                            if (this.expireds[i].id == item.id) {
                                this.expireds[i].state = "unpaid"
                            }
                        }
                        this.stateUnpaid = true
                        return
                    }

                }).catch((respond) => { console.log(respond); this.loading = false });
        },
    },
})