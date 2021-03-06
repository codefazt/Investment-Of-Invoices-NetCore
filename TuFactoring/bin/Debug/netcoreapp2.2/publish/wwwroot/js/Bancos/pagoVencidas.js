﻿new Vue({
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
        formatoMoneda: formatoMoneda,
        backEndDateFormat: backEndDateFormat,
        headersExpireds: [
            { text: i18n.t("headers.n"), value: "n", align: "center" },
            { text: i18n.t("headers.payer"), value: "payer.name", align: "center" },
            { text: i18n.t("headers.receiver"), value: "receiver.name", align: "center" },
            { text: i18n.t("headers.amountToPay"), value: "amount", align: "center" },
            { text: i18n.t("headers.detail"), value: "detail", align: "center" },
            { text: i18n.t("headers.opciones"), value: "pay", align: "center" }
        ],
        headersRencociled: [
            { text: i18n.t("headers.n"), value: "n", align: "center" },
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
            { text: i18n.t("headers.valorNeto"), value: "valorneto", align: "center" },
            { text: i18n.t("headers.discount"), value: "discount", align: "center" },
            { text: i18n.t("headers.amountToPay"), value: "payable", align: "center" },
            { text: i18n.t("headers.commission"), value: "commission", align: "center" },
            { text: i18n.t("headers.financing"), value: "request_financing", align: "center" },
        ],
        receipts: [],
        banks: [],
        reconcileds: [],
        expireds: [],
        settings: [],
        dialogConfirmTransfer: false,
        datos: {},
        processing: false,
        selected: {},
        dialogTransfer: false,
        pruebaExpireds: [],
        dialogDetails: false,
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
            if (dataOverdue != "" && dataOverdue[x].state == "draft" || dataOverdue != "" && dataOverdue[x].state == "processing") {
                this.expireds.push(dataOverdue[x])
            }
        }


        /*this.expireds = dataOverdue
        this.reconcileds = dataReconciled*/

        /*for (let x = 0; x < data.length; x++) {
            if (dataOverdue != "" && dataOverdue[x].abbreviation == "OVERDUE") {
                this.expireds.push(data[x])
            } else if (data != "" && data[x].abbreviation == "RECONCILED") {
                this.reconcileds.push(data[x])
            }
        }*/

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
            //this.dialogSeguro = true
            this.dialogConfirmTransfer = true
        },
        DatosPanelTransferir(item) {
            this.selected = item
            this.selected.transfer = {}
            //this.dialogTransfer = true
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

                    /*if (respond.data.state == "processing") {
                        this.processing = true
                    } else if (respond.data.state == "paid" || respond.data.state == "unpaid") {
                        this.expireds.splice(this.expireds.indexOf(item), 1)
                    }*/

                    this.expireds.splice(this.expireds.indexOf(item), 1)
                    toastr.success(i18n.t("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.t("mensajesModal.paymentRealized"))
                }).catch((respond) => { console.log(respond); this.loading = false });
        },
    },
})