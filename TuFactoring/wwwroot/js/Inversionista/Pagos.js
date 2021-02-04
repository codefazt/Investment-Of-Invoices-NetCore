new Vue({
    el: "#appPagoFacturasBanco",
    i18n,
    vuetify: new Vuetify({
        lang: {
            t: (key, ...params) => i18n.t(key, params)
        }
    }),
    data: {
        selectedPayments: {},
        modalPayments: false,
        modalLogout: { mostrar: false },
        i18n: i18n,
        dialogConfirmPayment: false,
        widthTelefono: widthTelefono,
        stateProcessing: false,
        stateUnpaid: false,
        statePaid: false,
        formatoMonedaInput: formatoMonedaInput,
        formatoMoneda: formatoMoneda,
        backEndDateFormat: backEndDateFormat,
        headersReceipts: [
            { text: i18n.t("headers.n"), value: "n", align: "center" },
            { text: i18n.t("headers.bankConfirming"), value: "entity.person.name", align: "center" },
            { text: i18n.t("headers.coin"), value: "coin", align: "center" },
            { text: i18n.t("headers.originalAmount"), value: 'original_amount', align: 'center' },
            { text: i18n.t("headers.valorNeto"), value: "valorneto", align: "center" },
            { text: i18n.t("headers.amountToPay"), value: "amount", align: "center" },
            { text: i18n.t("headers.paid"), value: "total", align: "center" },
            { text: i18n.t("headers.detail"), value: "detail", align: "center" },
            { text: i18n.t("headers.opciones"), value: "pay", align: "center" }
        ],
        headersReceiptsBanco: [
            { text: i18n.t("headers.n"), value: "n", align: "center" },
            { text: i18n.t("headers.program"), value: "program.abbreviation", align: "center" },
            { text: i18n.t("headers.receiver"), value: "receiver.name", align: "center" },
            { text: i18n.t("headers.coin"), value: "coin", align: "center" },
            { text: i18n.t("headers.originalAmount"), value: 'original_amount', align: 'center' },
            { text: i18n.t("headers.valorNeto"), value: "valorneto", align: "center" },
            { text: i18n.t("headers.amountToPay"), value: "amount", align: "center" },
            { text: i18n.t("headers.detail"), value: "detail", align: "center" },
            { text: i18n.t("headers.opciones"), value: "pay", align: "center" }
        ],
        headerFacturas: [
            { text: i18n.t("headers.n"), value: "n", align: "center" },
            { text: i18n.t("headers.cliente"), value: "invoice.debtor.name", align: "center" },
            { text: i18n.t("headers.numeroFechaVencimiento"), value: "invoice.number", align: "center" },
            { text: i18n.t("headers.coin"), value: "coin", align: "center" },
            { text: i18n.t("headers.originalAmount"), value: 'original_amount', align: 'center' },
            { text: i18n.t("headers.valorNeto"), value: "valorneto", align: "center" },
            { text: i18n.t("headers.discount"), value: "discount", align: "center" },
            { text: i18n.t("headers.amountToPay"), value: "payable", align: "center" },
            { text: i18n.t("headers.commission"), value: "commission", align: "center" },
        ],
        headerFacturasFactor: [
            { text: i18n.t("headers.n"), value: "n", align: "center" },
            { text: i18n.t("headers.proveedor"), value: "invoice.supplier.name", align: "center" },
            { text: i18n.t("headers.numeroFechaVencimiento"), value: "invoice.number", align: "center" },
            { text: i18n.t("headers.coin"), value: "coin", align: "center" },
            { text: i18n.t("headers.originalAmount"), value: 'original_amount', align: 'center' },
            { text: i18n.t("headers.valorNeto"), value: "valorneto", align: "center" },
            { text: i18n.t("headers.discount"), value: "discount", align: "center" },
            { text: i18n.t("headers.amountToPay"), value: "receivable", align: "center" },
        ],
        headerPayments: [
            { text: i18n.t("headers.n"), value: "n", align: "center" },
            { text: i18n.t("headers.accountNumber"), value: "account_number", align: "center" },
            { text: i18n.t("headers.numberRef"), value: "number", align: "center" },
            { text: i18n.t("headers.coin"), value: "coin", align: "center" },
            { text: i18n.t("headers.amountPaid"), value: "amount", align: "center" },
            { text: i18n.t("headers.estatus"), value: "state", align: "center" },
        ],
        accounts: [],
        receipts: [],
        banks: [],
        settings: [],
        auction: [],
        dataFacturas: {},
        pagoConfirmado: {
            iso_4217: '',
            symbol: '',
            digits: 0,
            amountTransfer: 0,
            amountButton: 0,
        },
        selected: {},
        confirmationPay: {},
        dialogTransfer: false,
        auctionPayed: false,
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
        var data = JSON.parse(document.getElementById("contenidoRaw").value)
        this.lang = document.getElementsByTagName("html")[0].getAttribute("lang")
        if (data != "") {
            this.accounts = data.accounts
            this.auction = data.auction
            this.receipts = data.receipts
            this.banks = data.entities
            this.settings = data.settings
            
            //Valida si existe subasta en el día, en caso contrario todos los pagos rezagados se eliminan.
            if (this.auction.length == 0) {
                for (let w = 0; w < this.receipts.length; w++) {
                    if (this.receipts[w].state == "unpaid") {
                        this.receipts.splice(this.receipts.indexOf(this.receipts[w]), 1)
                    }
                }
            }


            //Se recorre el array de subastas y receipts, esto para verificar si el state de la subasta es diferente de "payed" y el state de algún recibo es "unpaid" eliminar esos pagos rezagdos
            for (let x = 0; x < this.auction.length; x++) {
                for (let y = 0; y < this.receipts.length; y++) {
                    if (this.receipts.length > 0) {
                        if (this.auction[x].state != "payed" && this.receipts[y].state == "unpaid") {
                            this.receipts.splice(this.receipts.indexOf(this.receipts[y]), 1)
                        }
                    }
                }
            }
        }


        document.getElementById("contenido").removeAttribute("hidden")
        try {

            document.getElementById("appPagoFacturasBanco").removeChild(document.getElementById('contenidoRaw'))

        } catch (e) { console.log(e) }
        this.loading = false
        this.cargando = false
        tiempoLogin(this.modalLogout)
    },
    methods: {
        addAccountNumber(id, currency) {
            for (var i = 0; i < this.accounts.length; i++) {
                if (id == this.accounts[i].entity.id && currency == this.accounts[i].currency) {
                    this.selected.transfer.account_number = this.accounts[i].accountNumber
                    this.refrescar()
                    return
                }
            }
        },

        CurrencyForBank(id, currency) {
            for (var i = 0; i < this.accounts.length; i++) {
                if (id == this.accounts[i].entity.id && currency == this.accounts[i].currency) {
                    return true
                }
            }

            return false
        },

        eliminarSobrante(item) {
            for (let w = 0; w < this.receipts.length; w++) {
                if (this.receipts[w].id == item.id) {
                    this.receipts.splice(this.receipts.indexOf(this.receipts[w]), 1)
                }
            }
        },

        DetalleFacturas(item) {
            this.dataFacturas = item
            this.dialogDetails = true
        },

        BotonPago(item) {
            this.selected = item
            this.dialogSeguro = true
        },
        secondConfirmation(item) {
            this.pagoConfirmado = item
            this.pagoConfirmado.symbol = item.currency.symbol
            this.pagoConfirmado.iso_4217 = item.currency.iso_4217
            this.pagoConfirmado.digits = item.currency.digits
            if (this.pagoConfirmado.method == "TRANSFER") {
                this.pagoConfirmado.amountTransfer = item.transfer.amount
            }
            this.pagoConfirmado.amountButton = item.amount
            this.dialogConfirmPayment = true
        },

        DatosPanelPayments(item) {
            this.dataFacturas = item
            this.selectedPayments = item.payments

            this.modalPayments = true
        },

        DatosPanelTransferir(item) {
            this.selected = item
            this.selected.transfer = {}
            this.selected.transfer.entity = ""
            this.selected.transfer.amount = formatoMonedaInput(item.amount - item.paid, this.lang, this.selected.currency.digits)
            this.textoErrorReference = ""
            this.textoErrorAccountNumber = ""
            this.textoErrorAmount = ""


            if (this.selected.receiving_account == null) {
                this.selected.receiving_account = { account_number: "" }
            }

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
                this.textoErrorReference = i18n.t("addNumberReference");

            } else if (emailRegex.test(this.selected.transfer.number)) {

                this.errorReference = true;
                this.textoErrorReference = i18n.t("formatInvalid");

            } else if (this.selected.transfer.number.length < 10) {

                this.errorReference = true;
                this.textoErrorReference = i18n.t("minNumber", [10]);

            } else if (this.selected.transfer.number.length > 255) {

                this.errorReference = true;
                this.textoErrorReference = i18n.t("maxNumber", [255]);

            } else {
                this.errorReference = false;
                this.textoErrorReference = '';
            }

            this.refrescar()
        },

        validarAccountNumber() {
            var valido = false

            this.settings.map(data => {
                if (valido || data.abbreviation != "REGEXP_ACCOUNT_NUMBER") return

                var r = new RegExp(data.content);

                if (r.test(this.selected.transfer.account_number))
                    valido = true
            })

            if (!valido) {
                this.textoErrorAccountNumber = i18n.t("errorAccountNumber")
                this.errorAccountNumber = true
            } else {

                for (var i = 0; i < this.banks.length; i++) {
                    if (this.banks[i].id == this.selected.transfer.entity) {

                        var distancia = this.banks[i].routing_number.length
                        var validar = this.selected.transfer.account_number.slice(0, distancia)

                        if (this.banks[i].routing_number != validar) {
                            this.textoErrorAccountNumber = i18n.t("valid.codigoBancarioIncorrecto")
                            this.errorAccountNumber = false
                            return
                        } else {
                            break
                        }
                    }
                }

                this.textoErrorAccountNumber = ""
                this.errorAccountNumber = false
            }
            this.refrescar()
        },

        montoPagar() {
            if (this.selected.paying_account != null) {
                return formatoMonedaInput(this.selected.amount, this.lang, this.selected.currency.digits)
            }

            return formatoMonedaInput(formatoMoneda(this.selected.transfer.amount, this.lang), this.lang, this.selected.currency.digits)
        },

        realizarPago(item) {
            if (item.method != 'TRANSFER')
                this.pagarFactura(item)
            else
                this.transferFactura(item)
        },

        async pagarFactura(item) {
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

                    if (typeof respond.data === 'string' || respond.data instanceof String) {

                        if (respond.data.includes("<!DOCTYPE html>")) {
                            window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired";
                            toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br>" + i18n.t("errorBaseDatos"));
                            return;
                        }
                    }

                    if (respond.data != null && respond.data == notAuthorized) {
                        window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                        return
                    }

                    if (respond.data == null || respond.data == "Error") {
                        toastr.warning(i18n.t("problemasRespuesta"))
                        return
                    }

                    if (respond.data.state == "processing") {
                        for (let x = 0; x < this.receipts.length; x++) {
                            if (this.receipts[x].id == item.id) {
                                this.receipts[x].state = "processing"
                            }
                        }
                        this.stateProcessing = true
                        toastr.success(i18n.t("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.t("mensajesModal.paymentRealized"))
                        return
                    } else if (respond.data.state == "paid") {
                        //this.statePaid = true
                        this.receipts.splice(this.receipts.indexOf(item), 1)
                        toastr.success(i18n.t("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.t("mensajesModal.paymentRealized"))
                        return
                    } else if (respond.data.state == "unpaid") {
                        for (let i = 0; i < this.receipts.length; i++) {
                            if (this.receipts[i].id == item.id) {
                                this.receipts[i].state = "unpaid"
                            }
                        }
                        this.stateUnpaid = true
                        return
                    }

                    this.dialogTransfer = false
                }).catch((respond) => { console.log(respond); this.loading = false });
        },
        //
        async transferFactura(item) {
            if (this.loading) {
                toastr.warning(i18n.t("proccessInProgress"))
                return
            }
            this.loading = true

            item.transfer.receipt = item.id
            item.transfer.currency = item.currency.id
            item.transfer.payment_date = new Date()
            item.transfer.amount = this.formatoMoneda(item.transfer.amount, this.lang)

            await axios.post('?handler=pay', item.transfer,
                {
                    headers: {
                        "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                    }
                })
                .then((respond) => {
                    resetTime()
                    this.loading = false

                    console.log(respond.data)
                    if (respond.data != null && respond.data == notAuthorized) {
                        window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                        return
                    }

                    if (respond.data == "account not valid") {
                        toastr.warning(i18n.t("accountOrcurrencyNotValid"))

                        this.dialogTransfer = false
                        return
                    }

                    if (respond.data == null || respond.data == "Error") {
                        toastr.warning(i18n.t("problemasRespuesta"))

                        this.dialogTransfer = false
                        return
                    }

                    for (var i = 0; i < this.receipts.length; i++) {
                        if (this.receipts[i].id == item.id) {
                            this.receipts[i].paid += item.transfer.amount
                            if (this.receipts[i].amount - this.receipts[i].paid <= 0) {
                                this.receipts[i].state = 'processing'
                            }

                            if (this.receipts[i].payments == null) this.receipts[i].payments = []

                            this.receipts[i].payments.push({
                                account_number: item.transfer.account_number,
                                number: item.transfer.number,
                                amount: item.transfer.amount,
                                state: 'processing'
                            })
                        }
                    }

                    this.dialogTransfer = false
                    toastr.success(i18n.t("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.t("mensajesModal.paymentRealized"))
                }).catch((respond) => { console.log(respond); this.loading = false });

        },
        //
        obtenerValorNet(array) {
            var sumatoria = 0

            if (array == null || array == undefined) return sumatoria

            array.map(data => {
                if (data.invoice.amount != null)
                    sumatoria += data.invoice.amount
            })

            return sumatoria
        },
        //
        obtenerValorNominal(array) {
            var sumatoria = 0

            if (array == null || array == undefined) return sumatoria

            array.map(data => {
                if (data.invoice.original_amount != null)
                    sumatoria += data.invoice.original_amount
            })

            return sumatoria
        },
        //
        obtenerAmountCommission(array) {
            var sumatoria = 0

            if (array == null || array == undefined) return sumatoria

            array.map(data => {
                if (data.commission != null)
                    sumatoria += data.commission
            })

            return sumatoria
        },
        //
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

    },
})