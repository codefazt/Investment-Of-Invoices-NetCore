new Vue({
    el: '#appCargaMasiva',
    store: vuexLayout,
    i18n,
    vuetify: new Vuetify({
        lang: {
            t:(key,...params) => i18n.t(key,params)
        }
    }),
    data: {
        modalLogout: { mostrar: false },
        fraccion: fraccion,
        widthTelefono: widthTelefono,
        estadoCarga: 0,
        idDeduccion: -1,
        dialogAyuda: false,
        formatoMoneda: formatoMoneda,
        isValid: 'is-valid',
        isInvalid:'is-invalid',
        indice : 0,
        headers3: [
            { text: i18n.t("headers.tipo"), value: "tipo", align: "center" },
            { text: i18n.t("headers.code"), value: "number", align: "center" },
            { text: i18n.t("headers.amountDeduction"), value: "monto", align: "center" },
            { text: i18n.t("headers.opciones"), value: "opciones", align: "center" }
        ],
        tipoMoneda: [],
        proveedores: [],
        nuevo: {
            id: null,
            idPais: 0,
            idCliente: 0,
            numero: '',
            monto: '',
            tipoMoneda: 0,
            fechaEmision: '',
            fechaVencimiento: '',
            proveedor: {
                id: '0',
                nombre: '',
                number: '',
            },
            totalDeducciones: '',
            deduccion: {
                tipo: 0,
                numero: "",
                monto: '',
            }
        },
        errorProveedor: false,
        errorNum: false,
        errorFactura: false,
        errorMoneda: false,
        errorOriginalAmount: false,
        errorIssued: {
            igual: false,
            superiorToDate: false,
            superior: false,
            vacio: false
        },
        errorInvoiceNumber: {
            vacio: false,
            superior: false,
            mismoProveedor: false,
            mismoCliente: false,
            invoiceExist: false,
            formatoInvalido:false  
        },
        errorExpired: {
            igual: false,
            inferior: false,
            inferiorToDate: false,
            vacio: false,
            termDays: false
        },
        errorDeductions: {
            vacio: false,
            superior: false,
        },
        errorDeductionsAmount: {
            vacio: false,
            superior: false,
            superiorNominal: false,
            igualNominal: false,
            superiorTrece: false,
            superiorTreceDeduccion: false
        },
        errorPorcentaje: {
            vacio: false,
            superior: false,
            igual: false,
            invalido:false,
        },
        errorCargaMasiva: {
            archivoInvalido: false,
            archivoManipulado: false,
            archivoPocosRegistros: false,
            archivoSinRegistros: false
        },
        errorDeduccionDuplicado: false,
        errorAmount: false,
        errorClasses: {
            //Pantalla principal
            supplier: '',
            currencyType: '',
            amountSuperior:'',
            issuedDate: '',
            expirationDate: '',
            amount: '',
            invoiceNumber: '',
            //Deducciones
            deductionType: '',
            deductionNumber: '',
            deductionAmount: '',
            deductionPercentage:'',
        },
        errorDeduccion: false,
        errorNumeroDeduccion: false,
        errorMaxLengthInvoice: false,
        errorTipoDeduccion: false,
        errorMontoDeduccion: false,
        dialogDeduccion: false,
        dialogEliminar: false,
        dialogDeleteDeduccion:false,
        dialog: false,
        tamanoTlf: tamanoTlf,
        lang: "es",
        page2: 1,
        maxLengthInvoices:0,
        errorNetAmount:false,
        digits: 2,
        enviando: false,
        page: 1,
        pageCount: 10,
        REPuntos: /[.]{2,}/,
        REComas: /[,]{2,}/,
        msg: '',
        placeholderFactura: '',
        facturasGuardar: [],
        catalogo: [],
        cargando: true,
        errorD: false,
        moment: moment,
        formatoMonedaInput: formatoMonedaInput,
        facturas: [],
        facturasErroneas: [],
        selectedFile: { type:'',file:''},
        headers1: [
            {
                text: i18n.t("headers.n"), value: 'n', align: "center"
            },
            {
                text: i18n.t("headers.proveedor"), value: 'supplier.name', align: "center"
            },
            {
                text: i18n.t("headers.numeroFactura"), value: 'number', align: "center"
            },
            {
                text: i18n.t("headers.fechaVencimiento"), value: 'expiration_date', align: "center"
            },
            {
                text: i18n.t("headers.originalAmount"), value: 'original_amount', align: "center"
            },
            {
                text: i18n.t("headers.monto"), value: 'amount', align: "center"
            },
            {
                text: i18n.t("headers.eliminar"), value: 'opciones', align: "center"
            }
        ],
        headers2: [
            {
             text: i18n.t("headers.n"), value: "n", align: "center" 
            },
            {
                text: i18n.t("headers.numeroProveedor"), value: "supplier.number", align: "center"
            },
            {
                text: i18n.t("headers.numeroFactura"), value: "number", align: "center"
            },
            {
                text: i18n.t("headers.moneda"), value: "currency.iso_4217", align: "center"
            },
            {
                text: i18n.t("headers.fechaEmision"), value: "issued_date", align: "center"
            },
            {
                text: i18n.t("headers.fechaVencimiento"), value: "expiration_date", align:"center"
            },
            {
                text: i18n.tc("headers.mensaje", 2), value: "message", align:"center"
            }
        ],
        footer: {
            itemsPerPageAllText:"",
            itemsPerPageText:"",
            pageText:""
        },
        tipoDeduccion: [],
        deducciones: [],
        facturasProvar: [{supplier_name:"asdasd"}]
    },
    methods: {
        async llenarCatalogo() {
            await axios.post('CargaManual?handler=catalogo', {},
                {
                    headers: {
                        "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                    }
                })
                .then((respond) => {
                    resetTime()
                    if (respond.data == null) {
                        this.tipoDeduccion = [];
                        this.tipoMoneda = [];
                        return
                    }

                    if (respond.data.length > 0 && respond.data[0].errors == notAuthorized) {
                        window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                        return
                    }


                    this.catalogo = JSON.parse(respond.data)

                    this.catalogo.settings.map((datos) => {
                        if (datos.abbreviation == "REGEXP_INVOICE") {
                            this.placeholderFactura = datos.mask_edit
                        }

                        if (datos.abbreviation == "MAXLEN_INVOICE") {
                            this.maxLengthInvoices = datos.content
                        }
                    })

                    if (this.catalogo.length == 0) return
                    
                    this.catalogo.charges.map(charge => {
                        if (charge.abbreviation != "CMS") {
                            this.tipoDeduccion.push(charge)
                        }
                    })
                    this.tipoMoneda = this.catalogo.currencies;
                    this.nuevo.tipoMoneda = (this.tipoMoneda.length === 1 ? this.tipoMoneda[0].id : 0);
                    this.nuevo.idPais = this.catalogo.id;

                    this.tipoDeduccion.sort((a, b) => a.name.toLowerCase() < b.name.toLowerCase() ? -1 : +(a.name.toLowerCase() > b.name.toLowerCase()))
                    
                }).catch((respond) => { console.log(respond); });
        },
        //
        async llenarProveedores() {

            await axios.post('CargaManual?handler=proveedores', {},
                {
                    headers: {
                        "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                    }
                })
                .then((respond) => {
                    resetTime()
                    if (respond.data == null) return

                    if (respond.data.length > 0 && respond.data[0].errors == notAuthorized) {
                        window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                        return
                    }

                    this.proveedores = respond.data

                    this.proveedores.sort((a, b) => a.name.toLowerCase() < b.name.toLowerCase() ? -1 : +(a.name.toLowerCase() > b.name.toLowerCase()))
                }).catch((respond) => { console.log(respond); });
        },
        //
        limpiarDeducciones: function () {
            this.idDeduccion = -1
            this.nmrDeduccion = ''
            this.nuevo.totalDeducciones = this.totalizarDeducciones(this.deducciones);
            this.nuevo.deduccion.monto = '';
            this.nuevo.deduccion.numero = '';
            this.nuevo.deduccion.tipo = 0;
            this.errorD = false;
            this.deduccionIndexActual = -1;
        },
        //
        changeToUpper: function () {
            let invoiceNumber = document.getElementById("#txtFactura").value

            invoiceNumber.toUpperCase();

            return
        },
        //
        actualizarDeduccion: function () {
            let suma = formatoMoneda(this.nuevo.deduccion.monto, this.lang) - this.deducciones[this.idDeduccion].amount

            if (parseFloat(suma) > parseFloat(formatoMoneda(this.nuevo.monto, this.lang))) {
                this.errorPorcentaje.superior = true
                this.errorDeductionsAmount.superiorNominal = true
                this.errorD = true
                return
            }

            for (let i = 0; i < this.deducciones.length; i++) {
                suma = parseFloat(suma) + parseFloat(this.deducciones[i].amount)

                if (parseFloat(suma) > parseFloat(formatoMoneda(this.nuevo.monto, this.lang))) {
                    this.errorPorcentaje.superior = true
                    this.errorDeductionsAmount.superiorNominal = true
                    this.errorD = true;
                    return
                }
            }
            this.deducciones[this.idDeduccion].amount = parseFloat(formatoMoneda(this.nuevo.deduccion.monto, this.lang))
            this.deducciones[this.idDeduccion].charge_type_id = this.nuevo.deduccion.tipo
            this.deducciones[this.idDeduccion].number = this.nuevo.deduccion.numero

            this.limpiarDeducciones()

        },
        //
        reiniciar: function () {
            this.proveedorIdEditar = ''
            this.nmrFacturaEdita = ''
            this.error = false; //Validación de Errores
            this.errorD = false;//Validación de Errores en Deducciones
            this.cargando = false;
            this.nuevo.id = null;
            this.nuevo.idPais = 0;
            this.nuevo.idCliente = "";
            this.indice = -1;
            this.nuevo.numero = '';
            this.nuevo.monto = '';
            this.nuevo.tipoMoneda = (this.tipoMoneda.length === 1 ? this.tipoMoneda[0].id : 0);//928;
            this.nuevo.fechaEmision = '';
            this.nuevo.fechaVencimiento = '';
            this.nuevo.proveedor.id = '0';
            this.deducciones = [];
            this.nuevo.totalDeducciones = 0.0;
            this.nuevo.deduccion.tipo = 0;
            this.nuevo.deduccion.numero = "";
            this.nuevo.deduccion.monto = '';
            this.nmrDeduccion = '';
            this.idDeduccion = -1
        },
        validarProveedor() {
            if (this.nuevo.proveedor.id == 0) {
                this.errorProveedor = true
                this.errorClasses.supplier = this.isInvalid
            } else {
                this.errorProveedor = false
                this.errorClasses.supplier = this.isValid
            }
        },
        //
        validarCurrency() {
            if (this.nuevo.tipoMoneda == 0) {
                this.errorMoneda = true
                this.errorClasses.currencyType = this.isInvalid
            } else {
                this.errorMoneda = false
                this.errorClasses.currencyType = this.isValid
            }
        },
        //
        validarDeductionType() {
            if (this.nuevo.deduccion.tipo == 0) {
                this.errorTipoDeduccion = true
                this.errorClasses.deductionType = this.isInvalid
            } else {
                this.errorTipoDeduccion = false
                this.errorClasses.deductionType = this.isValid
            }
        },
        //
        enviarFactura: async function (factura) {
            this.cargando = true;
            
            await this.guardarFactura(factura);
            

            this.cargando = false;
        },
        //
        guardarFactura: async function (factura) {
            if (this.envio) return

            this.dialog = false
            this.envio = true
            await axios.post('CargaManual?handler=Guardar', factura,
                {
                    headers: {
                        "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                    }
                })
                .then((respuesta) => {
                    resetTime()
                    this.envio = false
                    var errores = ""
                    if (respuesta.data == null)
                        resp = respuesta
                    else
                        resp = respuesta.data

                    if (resp.errors != null) {
                        
                        if (resp.errors == "Invalid User") {
                            window.location.pathname = "../Index"
                            return
                        } else if (resp.errors == "invoice found") {
                            toastr.warning(i18n.tc("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.tc("facturaExistente", 2, { 0: resp.number }))
                            errores = i18n.tc("facturaExistente", 2, { 0: resp.number })
                            this.dialog = true
                            this.limpiarFieldErrors();
                        } else if (resp.errors == "term days not valid") {
                            toastr.warning(i18n.tc("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.tc("mensajesModal.fechaVencimientoTermDays"))
                            errores = i18n.tc("Error en la Fecha de Vencimiento")
                            this.dialog = true
                            this.limpiarFieldErrors();
                        } else if (resp.errors == "supplier has not accepted invitation") {
                            toastr.warning(i18n.tc("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.tc("mensajesModal.supplierHasNotAcceptedDebtorInvitation"))
                            errores = i18n.tc("mensajesModal.supplierHasNotAcceptedDebtorInvitationMassive")
                            this.dialog = true
                            this.limpiarFieldErrors();
                        } else if (resp.errors == "Supplier don't have accounts") {
                            toastr.warning(i18n.tc("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.tc("mensajesModal.supplierDontHaveAccountCreateInvoiceDebtor"))
                            errores = i18n.tc("mensajesModal.supplierDontHaveAccountCreateInvoice")
                            this.dialog = true
                            this.limpiarFieldErrors(); 
                        } else if (resp.errors == "Debtor don't have accounts") {
                            toastr.warning(i18n.tc("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.tc("mensajesModal.supplierDontHaveAccountCreateInvoice"))
                            errores = i18n.tc("mensajesModal.debtorDontHaveAccountCreateInvoice")
                            this.dialog = true
                            this.limpiarFieldErrors();
                        } else if (resp.length > 0 && resp[0].errors == notAuthorized) {
                            window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                            return
                        } else if (resp.errors == "Key: 'Invoice.Number' Error:Field validation for 'Number' failed on the 'invoiceNumber' tag") {
                            toastr.warning(i18n.tc("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.tc("formatoNumeroInvalido"))
                            errores = i18n.tc("formatoNumeroInvalido")
                            this.dialog = true
                            this.limpiarFieldErrors();
                        } else {
                            errores = resp.errors
                        }
                       
                    } else {
                        toastr.success(i18n.t("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.t("mensajesModal.cargaFactura"))
                    }

                    this.reiniciar();
                }).catch((respond) => { toastr.error(i18n.t("errorRespuesta")); this.envio = false; });
        },
        //
        limpiarFieldErrors() {
                //Pantalla principal
                this.errorClasses.supplier = ''
                this.errorClasses.currencyType = ''
                this.errorClasses.amountSuperior = ''
                this.errorClasses.issuedDate = ''
                this.errorClasses.expirationDate = ''
                this.errorClasses.amount = ''
                this.errorClasses.invoiceNumber = ''
                this.errorClasses.deductionType = ''
                this.errorClasses.deductionNumber = ''
                this.errorClasses.deductionAmount = ''
                this.errorClasses.deductionPercentage = ''
        },        
        //
        editarDeduccion: function (index) {

            if (index < 0 || index >= this.deducciones.length) {
                toastr.warning(i18n.t("errorAccion"))
                return
            }
            this.idDeduccion = index
            this.nmrDeduccion = this.deducciones[index].number
            this.nuevo.deduccion.monto = formatoMonedaInput(this.deducciones[index].amount, this.lang, this.digits)
            this.nuevo.deduccion.numero = this.deducciones[index].number
            this.nuevo.deduccion.tipo = this.deducciones[index].charge_type_id
        },
        //
        eliminarDeduccion: function (index) {

            if (this.idDeduccion != -1) {
                this.idDeduccion = -1
                this.nmrDeduccion = ""
                this.nuevo.deduccion.monto = ""
                this.nuevo.deduccion.numero = ""
                this.nuevo.deduccion.tipo = 0
            }

            this.deducciones.splice(index, 1);
            this.nuevo.totalDeducciones = this.totalizarDeducciones(this.deducciones);
            toastr.success(i18n.t("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.t("mensajesModal.deleteDeduccion"))
        },
        //
        obtenerIso4217() {
            for (let i = 0; i < this.tipoMoneda.length; i++) {
                if (this.tipoMoneda[i].id == this.nuevo.tipoMoneda) {
                    return this.tipoMoneda[i].iso_4217
                }
            }

        },
        //
        obtenerSymbolo() {
            for (let i = 0; i < this.tipoMoneda.length; i++) {
                if (this.tipoMoneda[i].id == this.nuevo.tipoMoneda) {
                    return this.tipoMoneda[i].symbol
                }
            }

        },
        //
        nombreCargo: function (id) {
            for (var i = 0; i < this.tipoDeduccion.length; i++) {
                if (this.tipoDeduccion[i].id == id)
                    return this.tipoDeduccion[i].name;
            }
            return 'Sin Nombre';
        },
        //
        totalizarDeducciones: function (deducciones) {
            if (deducciones === null) return 0;
            let totalD = 0;
            for (var i = 0; i < deducciones.length; i++) {
                totalD += parseFloat(deducciones[i].amount);
            }
            return totalD;
        },
        //
        encuentraErrorDeduccion: function () {
            this.errorD = false;
            if (this.nuevo.deduccion.monto == 0 || this.nuevo.deduccion.monto == '') return true;
            if (this.nuevo.deduccion.numero.length == 0) return true;
            if (this.nuevo.deduccion.tipo == 0) return true;

            for (let i = 0; i < this.deducciones.length; i++) {
                if (this.deducciones[i].number == this.nuevo.deduccion.numero && this.deducciones[i].number != this.nmrDeduccion) {
                    //toastr.warning(i18n.t("numeroDeduccionDuplicado"))
                    this.errorDeduccionDuplicado = true
                    return true
                } else {
                    this.errorDeduccionDuplicado = false
                }
            }

            return false;
        },
        //
        agregarDeducciones: function () {
            if (!( this.encuentraErrorDeduccion())) {

                if (this.idDeduccion != -1) {
                    this.actualizarDeduccion()
                    return
                }

                let suma = formatoMoneda(this.nuevo.deduccion.monto, this.lang)

                if (parseFloat(suma) > parseFloat(formatoMoneda(this.nuevo.monto, this.lang))) {
                    //toastr.warning(i18n.t("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.t("deduccionesSuperanMonto"))
                    this.errorD = true
                    this.errorDeductionsAmount.superior = true
                    this.errorPorcentaje.superior = true
                    this.errorDeductionsAmount.superiorNominal = true
                    return
                }

                for (let i = 0; i < this.deducciones.length; i++) {
                    suma = parseFloat(suma) + parseFloat(this.deducciones[i].amount)

                    if (parseFloat(suma) > parseFloat(formatoMoneda(this.nuevo.monto, this.lang))) {
                       //toastr.warning(i18n.t("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.t("deduccionesSuperanMonto"))
                        this.errorPorcentaje.superior = true
                        this.errorDeductionsAmount.superiorNominal = true
                        this.errorD = true;
                        return
                    }
                }

                this.deducciones.push({
                    amount: parseFloat(formatoMoneda(this.nuevo.deduccion.monto,this.lang)),
                    number: this.nuevo.deduccion.numero,
                    charge_type_id: this.nuevo.deduccion.tipo,
                    currency_id: this.nuevo.tipoMoneda

                });
                this.errorDeductionsAmount.vacio = false
                this.errorDeductionsAmount.superior = false
                this.errorDeductionsAmount.igualNominal = false
                this.errorDeductionsAmount.superiorNominal = false
                this.errorPorcentaje.vacio = false
                this.errorPorcentaje.igual = false
                this.errorPorcentaje.superior = false
                this.errorClasses.deductionType = ''
                this.errorClasses.deductionNumber = ''
                this.errorClasses.deductionAmount = ''
                this.errorClasses.deductionPercentage = ''
                this.nuevo.totalDeducciones = this.totalizarDeducciones(this.deducciones);
                this.nuevo.deduccion.monto = '';
                this.nuevo.deduccion.numero = '';
                this.nuevo.deduccion.tipo = 0;
                this.errorD = false;
            } else
                this.errorD = true
            
        },
        //
        validarPorcentaje() {
            this.errorClasses.deductionAmount = ''
            if (this.porcentaje == 0 || this.porcentaje == '') {
                this.errorPorcentaje.vacio = true
                this.errorClasses.deductionPercentage = this.isInvalid
                this.errorClasses.deductionAmount = ''
                this.errorDeductionsAmount.vacio = false
                this.errorDeductionsAmount.superior = false
                this.errorDeductionsAmount.igualNominal = false
                this.errorDeductionsAmount.superiorNominal = false
                return
            }

            this.errorClasses.deductionPercentage = this.isValid
            this.errorClasses.deductionAmount = ''
            this.errorPorcentaje.vacio = false

            if (formatoMoneda(this.porcentaje, this.lang, 2) > 100 && formatoMoneda(this.porcentaje, this.lang, 2) <= formatoMoneda(9999, this.lang, 2)) {
                this.errorClasses.deductionAmount = ''
                this.errorPorcentaje.superior = true
                this.errorClasses.deductionPercentage = this.isInvalid
                this.errorDeductionsAmount.vacio = false
                this.errorDeductionsAmount.superior = false
                this.errorDeductionsAmount.igualNominal = false
                this.errorDeductionsAmount.superiorNominal = false
                return                
            }

            this.errorClasses.deductionPercentage = this.isValid
            this.errorClasses.deductionAmount = ''
            this.errorPorcentaje.superior = false

            if (this.porcentaje == 100) {
                this.errorPorcentaje.igual = true
                this.errorClasses.deductionPercentage = this.isInvalid
                this.errorClasses.deductionAmount = ''                
                this.errorDeductionsAmount.vacio = false
                this.errorDeductionsAmount.superior = false
                this.errorDeductionsAmount.igualNominal = false
                this.errorDeductionsAmount.superiorNominal = false
                return
            }

            this.errorClasses.deductionPercentage = this.isValid
            this.errorClasses.deductionAmount = ''
            this.errorPorcentaje.igual = false
            this.errorDeductionsAmount.vacio = false
            this.errorDeductionsAmount.superior = false
            this.errorDeductionsAmount.igualNominal = false
            this.errorDeductionsAmount.superiorNominal = false

            if (this.porcentaje.length > 5) {
                this.errorClasses.deductionPercentage = this.isInvalid
                this.errorPorcentaje.invalido = true
            } else {
                this.errorClasses.deductionPercentage = this.isValid
                this.errorPorcentaje.invalido = false
            }

        },
        //Validate Deduction Amount
        validarDeductionAmount() {

            if (this.nuevo.deduccion.monto == '' || this.nuevo.deduccion.monto == 0) {
                this.errorDeductionsAmount.vacio = true
                this.errorClasses.deductionPercentage = ''
                this.errorClasses.deductionAmount = this.isInvalid
                this.errorPorcentaje.invalido = false
                this.errorPorcentaje.igual = false
                this.errorPorcentaje.superior = false
                this.errorPorcentaje.vacio = false
                return
            }
            this.errorDeductionsAmount.vacio = false

            if (formatoMoneda(this.nuevo.deduccion.monto,this.lang,2) == formatoMoneda(this.nuevo.monto,this.lang,2)) {
                this.errorDeductionsAmount.igualNominal = true
                this.errorClasses.deductionPercentage = ''
                this.errorClasses.deductionAmount = this.isInvalid
                this.errorPorcentaje.invalido = false
                this.errorPorcentaje.igual = false
                this.errorPorcentaje.superior = false
                this.errorPorcentaje.vacio = false
                return
            }
            this.errorClasses.deductionAmount = this.isValid
            this.errorDeductionsAmount.igualNominal = false

            if (formatoMoneda(this.nuevo.deduccion.monto) > formatoMoneda(this.nuevo.monto)) {
                this.errorDeductionsAmount.superiorNominal = true
                this.errorClasses.deductionPercentage = ''
                this.errorClasses.deductionAmount = this.isInvalid
                this.errorPorcentaje.invalido = false
                this.errorPorcentaje.igual = false
                this.errorPorcentaje.superior = false
                this.errorPorcentaje.vacio = false
                return
            }
            this.errorClasses.deductionAmount = this.isValid
            this.errorDeductionsAmount.superiorNominal = false

            if (this.lang == "ESV" || this.lang == "ESS" || this.lang == "es") {
                var montoLongitud = this.nuevo.deduccion.monto.split(",")

                if (this.nuevo.deduccion.monto == 0 || this.nuevo.deduccion.monto == "" || montoLongitud[0].length > 17) {
                    this.errorClasses.deductionAmount = this.isInvalid
                    this.errorClasses.deductionPercentage = ''
                } else {
                    this.errorClasses.deductionAmount = this.isValid
                }

                if (montoLongitud[0].length > 17) {
                    
                    this.errorDeductionsAmount.superiorTreceDeduccion = true
                } else {
                    this.errorDeductionsAmount.superiorTreceDeduccion = false
                }
            }
            else if (this.lang == "ENU" || this.lang == "en") {
                var lengthAmount = this.nuevo.deduccion.monto.split(".")


                if (this.nuevo.deduccion.monto == 0 || this.nuevo.deduccion.monto == "" || lengthAmount[0].length > 17) {
                    this.errorClasses.deductionAmount = this.isInvalid
                } else {
                    this.errorClasses.deductionAmount = this.isValid
                }

                if (lengthAmount[0].length > 17) {
                    this.errorDeductionsAmount.superiorTreceDeduccion = true
                } else {
                    this.errorDeductionsAmount.superiorTreceDeduccion = false
                }
            } 

            this.errorClasses.deductionPercentage = ''
            this.errorPorcentaje.invalido = false
            this.errorPorcentaje.igual = false
            this.errorPorcentaje.superior = false
            this.errorPorcentaje.vacio = false

        },
        //Validate Deduction Number
        validarDeductionNumber() {
            //If is empty
            if (this.nuevo.deduccion.numero == '' || this.nuevo.deduccion.numero == 0) {
                this.errorDeductions.vacio = true
                this.errorClasses.deductionNumber = this.isInvalid
                return
            }
            this.errorClasses.deductionNumber = this.isValid
            this.errorDeductions.vacio = false
            //Length of the value
            if (this.nuevo.deduccion.numero.length >= 255) {
                this.errorClasses.deductionNumber = this.isInvalid
                this.errorDeductions.superior = true
                return
            }
            this.errorClasses.deductionNumber = this.isValid
            this.errorDeductions.superior = false
        },
        //
        limpiarCampos() {
            //Llevar a Pantalla Principal
            this.estadoCarga = 0
            //Limpiar Inputs
            this.nuevo.monto = ""
            this.nuevo.proveedor.id = 0
            this.nuevo.numero = ""
            this.nuevo.tipoMoneda = 0
            this.nuevo.fechaEmision = ""
            this.nuevo.fechaVencimiento = ""
            this.nuevo.deduccion.tipo = 0
            this.nuevo.deduccion.numero = ""
            this.nuevo.deduccion.monto = ""
            this.porcentaje = ""
            this.nuevo.totalDeducciones = ""
            this.deducciones.length = 0
            //Numero de Factura
            this.errorInvoiceNumber.vacio = false
            this.errorInvoiceNumber.superior = false
            this.errorInvoiceNumber.formatoInvalido = false
            //Numero de Deduccion
            this.errorDeductions.superior = false
            this.errorDeductions.vacio = false
            //Monto de Deduccion
            this.errorDeductionsAmount.superiorNominal = false
            this.errorDeductionsAmount.vacio = false
            this.errorDeductionsAmount.superior = false
            this.errorDeductionsAmount.igualNominal = false
            //Porcentaje
            this.errorPorcentaje.vacio = false
            this.errorPorcentaje.superior = false
            this.errorPorcentaje.igual = false
            //Fecha de Vencimiento
            this.errorExpired.igual = false
            this.errorExpired.inferior = false
            this.errorExpired.inferiorToDate = false
            this.errorExpired.vacio = false
            //Fecha de Emision
            this.errorIssued.vacio = false
            this.errorIssued.superior = false
            this.errorIssued.igual = false
            //Proveedor
            this.errorProveedor = false
            //Tipo de Factura
            this.errorFactura = false
            //Valor Nominal
            this.errorOriginalAmount = false
            //Tipo de Moneda
            this.errorMoneda = false
            //Tipo de Deduccion
            this.errorTipoDeduccion = false
            //Valor neto
            this.errorNetAmount = true
            //Limpiar Tabla de Deducciones Agregadas
            
        },
        ///Valida Monto Nominal
        validarNominalAmount() {

            if (this.nuevo.monto == 0 || this.nuevo.monto == "") {
                this.errorOriginalAmount = true
            } else {
                this.errorOriginalAmount = false
            }

            if (this.lang == "ESV" || this.lang == "ESS" || this.lang == "es") {
                var longitudMonto = this.nuevo.monto.split(",")

                if (this.nuevo.monto == 0 || this.nuevo.monto == "" || longitudMonto[0].length > 17) {
                    this.errorClasses.amount = this.isInvalid
                } else {
                    this.errorClasses.amount = this.isValid
                }

                if (longitudMonto[0].length > 17 && longitudMonto[0].length > 0) {
                    this.errorDeductionsAmount.superiorTrece = true
                } else {
                    this.errorDeductionsAmount.superiorTrece = false
                }
            }
            else if (this.lang == "ENU" || this.lang == "en") {
                var amountLength = this.nuevo.monto.split(".")

                if (this.nuevo.monto == 0 || this.nuevo.monto == "" || amountLength[0].length > 17) {
                    this.errorClasses.amount = this.isInvalid
                } else {
                    this.errorClasses.amount = this.isValid
                }

                if (amountLength[0].length > 17 && longitudMonto[0].length > 0) {
                    this.errorDeductionsAmount.superiorTrece = true
                } else {
                    this.errorDeductionsAmount.superiorTrece = false
                }
            } 

        },
        //Validacion del Numero de Factura para la Carga Manual
        validarInvoiceNumber() {

            let RE = ""

            let inputNumber = document.getElementById("txtFactura").value

            this.nuevo.numero = inputNumber.toUpperCase()

            this.catalogo.settings.map((data) => {
                if (data.abbreviation == "REGEXP_INVOICE") {
                    RE = new RegExp(data.content)
                }
            })

            //Validar Numero de Factura Vacio
            if (this.nuevo.numero == '' || this.nuevo.numero.length == 0) {
                this.errorInvoiceNumber.vacio = true
                this.errorClasses.invoiceNumber = this.isInvalid
                return
            }

                this.errorInvoiceNumber.vacio = false
                this.errorClasses.invoiceNumber = this.isValid
            //Validar Longitud del Número de Factura
            if (this.nuevo.numero.length >= 175) {
                this.errorInvoiceNumber.superior = true
                this.errorClasses.invoiceNumber = this.isInvalid
                return
            }

                this.errorInvoiceNumber.superior = false
                this.errorClasses.invoiceNumber = this.isValid
            //Validar que cumpla con el formato según el pais correspondiente
            if (!RE.test(this.nuevo.numero)) {
                this.errorInvoiceNumber.formatoInvalido = true
                this.errorClasses.invoiceNumber = this.isInvalid
                return
            }

                this.errorInvoiceNumber.formatoInvalido = false
                this.errorClasses.invoiceNumber = this.isValid
        },
         //Validacion de Fecha de Emision para la Carga Manual
        validarIssued() {
            //Si es vacia
            if (this.nuevo.fechaEmision == '') {
                this.errorIssued.vacio = true
                this.errorClasses.issuedDate = this.isInvalid
                return
            }

                this.errorIssued.vacio = false
                this.errorClasses.issuedDate = this.isValid
            //Si es mayor al dia actual
            if (moment(this.nuevo.fechaEmision).diff(moment(), "days") > 0) {
                this.errorIssued.superior = true
                this.errorClasses.issuedDate = this.isInvalid
                return
            }

                this.errorIssued.superior = false
                this.errorClasses.issuedDate = this.isValid
            //Si es igual a la Fecha de Vencimiento
            if (moment(this.nuevo.fechaEmision).diff(this.nuevo.fechaVencimiento) == 0) {
                this.errorIssued.igual = true
                this.errorClasses.issuedDate = this.isInvalid
                return
            }

                this.errorIssued.igual = false
                this.errorClasses.issuedDate = this.isValid

            if (this.nuevo.fechaVencimiento != '') this.validarExpired
        },
        //Validacion de Fecha de Vencimiento
        validarExpired() {
            //Si es vacia
            if (this.nuevo.fechaVencimiento == '') {
                this.errorExpired.vacio = true
                this.errorClasses.expirationDate = this.isInvalid
                return
            }

                this.errorExpired.vacio = false
                this.errorClasses.expirationDate = this.isValid
            
            //Si es menor al día actual
            if (moment(this.nuevo.fechaVencimiento).diff(moment(), "days") < 0) {
                this.errorExpired.inferior = true
                this.errorClasses.expirationDate = this.isInvalid
                return
            }

                this.errorExpired.inferior = false
                this.errorClasses.expirationDate = this.isValid
                
            //Si es igual a la Fecha de Emision
            if (moment(this.nuevo.fechaVencimiento).diff(this.nuevo.fechaEmision) == 0) {
                this.errorExpired.igual = true
                this.errorClasses.expirationDate = this.isInvalid
                return
            }

                this.errorExpired.igual = false
                this.errorClasses.expirationDate = this.isValid

            //En caso de colocar la Fecha de Vencimiento más no la de Emisión
            if (this.nuevo.fechaEmision != '') this.validarIssued
        },
        //Validacion del Valor Neto de la Factura
        valorNeto: function () {
            let monto
            //Si es vacio
            if (this.nuevo.monto == 0 || this.nuevo.monto == '') return ''

            monto = formatoMoneda(this.nuevo.monto, this.lang) - this.nuevo.totalDeducciones

            return formatoMonedaInput(monto, this.lang, this.digits)

        },
        //
        limpiarProcesadas() {
            this.facturas = []
        },
        //
        limpiar() {
            this.facturasErroneas = []
        },
        //************************************************************
        nombreProveedor(id) {
            for (let i = 0; i < this.proveedores.length; i++) {
                if (this.proveedores[i].id == id) {
                    return this.proveedores[i].name
                }
            }
            return "Nombre no encontrado"
        },
        //************************************************************
        onFileSelected: function (event) {
            
                if (event.target.files.length == 0) return

                this.selectedFile.type = event.target.files[0].type
                this.selectedFile.file = event.target.files[0]
                this.onUpload()
            },
        //************************************************************
        onUpload: function () {
              if (this.selectedFile !== null) {
                  if(this.selectedFile.type == 'text/plain') {
                    
                     if ((this.selectedFile.file.size / 1024) <= 400) {
                         this.cargarArchivo(this.selectedFile.file);
                         this.errorCargaMasiva.archivoInvalido = false
                          this.selectedFile = { type: '', file: '' }
                          $("#facturaInputFile").val("")
                    } else {
                         this.selectedFile = { type: '', file: '' }
                        $("#facturaInputFile").val("")
                        toastr.warning(i18n.t("tamanoTxtInvalido"))
                    }
                  } else {
                      this.selectedFile = { type: '', file: '' }
                      $("#facturaInputFile").val("")
                      this.errorCargaMasiva.archivoInvalido = true
                  }
            }
          },
        //************************************************************
        guardarFacturas: async function () {
                this.enviando = true
                let facturasCargadas = []
                
                axios.post('?handler=guardar', this.facturas ,
                    {
                        headers: {
                            "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                        }
                    })
                    .then((respond) => {
                        resetTime()
                        this.enviando = false
                        var status = 0
                        var errores =[]
                        var data = ''
                    
                        if (respond.data == null || respond.data == undefined) {
                            data = respond
                        } else {
                            data = respond.data
                        }

                        if(data.length > 0 && data[0].errors == notAuthorized) {
                            window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                            return
                        } 


                        if (data == "Invalid User") {
                            window.location.pathname ="../Index"
                            return
                        }
                        for (var i = 0; i < data.length; i++) {
                            if (data[i].errors != null || data[i].errors != undefined) {
                                if (data[i].errors == "invoice found") {
                                    errores.push(i18n.tc("facturaExistente", 2, { 0: data[i].number }))
                                    continue
                                } else if (data[i].errors == "supplier has not accepted invitation") {
                                    errores.push(i18n.tc("mensajesModal.supplierHasNotAcceptedDebtorInvitationMassive"))
                                    continue
                                } else if (data[i].errors == "Supplier don't have accounts") {
                                    errores.push(i18n.tc("mensajesModal.supplierDontHaveAccountCreateInvoicesDebtor"))
                                    continue
                                } else if (data[i].errors == "Debtor don't have accounts") {
                                    errores.push(i18n.tc("mensajesModal.supplierDontHaveAccountCreateInvoices"))
                                    continue
                                }
                                errores.push(i18n.t(data[i].errors))
                            } else {
                                facturasCargadas.push(i)
                                status++
                            }
                        }
                       
                        this.quitarFacturas(facturasCargadas,errores)

                        if (status == data.length) {
                            toastr.success(i18n.t("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.t("mensajesModal.cargaFacturaMasiva"))
                        }
                    
                        if (status != data.length) {
                            toastr.warning(i18n.tc("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.tc("mensajesModal.incovenientesFacturas"))
                        } 
                    
                    }).catch(e => { this.enviando = false; toastr.error(i18n.t("errorRespuesta")); });
            
            },
        //************************************************************
        quitarFacturas: function (facturas,errores) {
                facturas.sort((a, b) => a < b ? -1 : +(a > b))
                
                for (var t = 0; t < facturas.length;t++) {
                    this.facturas.splice(facturas[t]-t,1)
                }

                for (var i = 0; i < this.facturas.length; i++) {
                    var data = this.facturas[i]

                    this.facturasErroneas.push(
                        {
                            supplier: { number: data.supplier.number },
                            number: data.number,
                            currency: data.currency,
                            issued_date: data.issued_date,
                            expiration_date: data.expiration_date,
                            amount: data.amount,
                            message: errores[i]
                        }
                    )
                }

                this.facturas = []

                this.facturasErroneas.sort((a, b) => a.supplier.number.toLowerCase() < b.supplier.number.toLowerCase() ? -1 : (a.supplier.number.toLowerCase() > b.supplier.number.toLowerCase()))
            },
        //************************************************************
        cargarArchivo: function (archivo) {
              var lector = new FileReader()

              lector.onload = this.cargarData

              try {
                  lector.readAsText(archivo)
              } catch (e) {
                  $("#facturaInputFile").val("")
                  toastr.warning(i18n.t("errorLeerArchivo"))
              }
          
            },
        //************************************************************
        cargarData: function (e) {
                var contenido = e.target.result
                var rE = /^[A-Za-z0-9\|\,\-\%\!\¿\?\#\.\_\?\s\n\r]+$/                
                
            if (contenido == '') {
                $("#facturaInputFile").val("")
                this.errorCargaMasiva.archivoSinRegistros = true
                return
            }
            else {
                this.errorCargaMasiva.archivoSinRegistros = false
            }

                if (!rE.test(contenido)) {
                    $("#facturaInputFile").val("")
                    this.errorCargaMasiva.archivoManipulado = true
                    return
                } else {
                    this.errorCargaMasiva.archivoManipulado = false
                }

                let guardar = true
                contenido = contenido.split(/\n/)

                if (contenido.length <= 1) {
                    $("#facturaInputFile").val("")
                    toastr.warning(i18n.t("mensajesModal.pocosRegistros"))
                    return
                }

                
                
                contador = 0
                this.facturasGuardar = []
                this.facturasErroneas = []
                this.facturas = []
              contenido.map(data => {
                  contador++

                  if(contador >= 2000) return

                  var factura = data.split("|")

                  if(factura == null || factura == 0 || factura.length == 0) return

                  factura = this.verificarInformacionInicial(factura)

                  if (factura.length > 5) factura[5] = formatoMoneda(factura[5], this.lang)

                  if (factura.length > 6) factura[6] = formatoMoneda(factura[6], this.lang)
            
                  var facturaActual = {
                                          supplier: { number: factura[0] },
                                          number: factura.length > 1 ? factura[1] : "",
                                          currency: { iso_4217: factura.length > 2 ? factura[2] : "" },
                                          issued_date: factura.length > 3 ? factura[3] : "",
                                          expiration_date: factura.length > 4 ? factura[4] : "",
                                          original_amount: factura.length > 5 ? factura[5] : "",
                                          amount: factura.length > 6 ? factura[6] : "",
                                          message: ""
                                      }
                  
                  if (factura.length < 7) {
                      facturaActual.message = i18n.t("numeroCamposInvalido7",{0: factura.length})
                      this.facturasErroneas.push(facturaActual)
                      guardar = true
                      return                      
                  }

                  if (factura.length > 7) {
                      facturaActual.message = i18n.t("numeroCamposInvalidoMas7", { 0: factura.length })
                      this.facturasErroneas.push(facturaActual)
                      guardar = true
                      return
                  }
                  var encontrado = { currency: false,supplier_id: false, number: false }

                  var contador = 0

                  for (let i = 0; i < this.catalogo.identifications.length; i++) {
                      let regular = new RegExp(this.catalogo.identifications[i].regexp)
                      if (!regular.test(factura[0])) {
                          contador++
                      } else {
                          break
                      }
                  }

                  if (contador > 0 && contador == this.catalogo.identifications.length) {
                      facturaActual.message = i18n.t("proveedorInvalido")
                      this.facturasErroneas.push(facturaActual)
                      return 
                  }

                  for (let i = 0; i < this.proveedores.length; i++) {
                      if (this.proveedores[i].documents[0].number == factura[0]) {
                          factura[0] = { supplier_id: this.proveedores[i].id, name: this.proveedores[i].name, number : factura[0] }
                          encontrado.supplier_id = true
                          break
                      }
                  }
              
                  for (let i = 0; i < this.catalogo.currencies.length; i++) {
                      if (this.catalogo.currencies[i].iso_4217.toLowerCase() == factura[2].toLowerCase()) {
                          factura[2] = { id: this.catalogo.currencies[i].id, symbol: this.catalogo.currencies[i].symbol, iso_4217: this.catalogo.currencies[i].iso_4217}
                          encontrado.currency = true

                          break
                      }
                  }

                  if (!encontrado.currency || !encontrado.supplier_id) {
                      let msg = ""
                      if (!encontrado.currency)
                          msg = "tipoMoneda"

                      if (!encontrado.supplier_id)
                          msg = "proveedorInvalido2"

                      facturaActual.message = i18n.t(msg)
                      this.facturasErroneas.push(facturaActual)

                      return
                  }

                  if (isNaN(factura[5])) {
                      facturaActual.message = i18n.t("montoInvalido")
                      this.facturasErroneas.push(facturaActual)
                      return
                  }

                  if (isNaN(factura[6])) {
                      facturaActual.message = i18n.t("montoInvalido")
                      this.facturasErroneas.push(facturaActual)
                      return
                  }

                  let monto = factura[5] + ""
                  let montoSplited = monto.split(".")

                  if (montoSplited[0].length > 13) {
                      facturaActual.message = i18n.t("montoMayorSuperior")
                      this.facturasErroneas.push(facturaActual)
                      return
                  }

                  if ((factura[5] + "").length > 16) {
                      facturaActual.message = i18n.t("montoMayorSuperior")
                      this.facturasErroneas.push(facturaActual)
                      return
                  }

                  let amount = factura[6] + ""
                  let splitedAmount = amount.split(".")

                  if (splitedAmount[0].length > 13) {
                      facturaActual.message = i18n.t("montoMayorSuperior")
                      this.facturasErroneas.push(facturaActual)
                      return
                  }

                  if ((factura[6] + "").length > 16) {
                      facturaActual.message = i18n.t("montoMayorSuperior")
                      this.facturasErroneas.push(facturaActual)
                      return
                  }


                  try {
                      var facturaVerificar = {
                          supplier_id: factura[0].supplier_id,
                          supplier: { name: factura[0].name, number: factura[0].number },
                          number: factura[1],
                          original_amount: factura[5],
                          currency_id: factura[2].id,
                          currency: { symbol: factura[2].symbol, iso_4217: factura[2].iso_4217},
                          issued_date: factura[3],
                          expiration_date: factura[4],
                          amount: factura[6]
                      }
                  } catch (e) {
                      facturaActual.message = i18n.t("montoInvalido")
                      this.facturasErroneas.push(facturaActual)
                      return
                  }
              
                  if (this.validarFactura(facturaVerificar)) {
                        facturaActual.message =  i18n.t(this.msg)
                      this.facturasErroneas.push(facturaActual)
                      return
                  }

                  this.facturasGuardar.push(facturaVerificar)

              })
            

              if (!guardar) {
                  this.selectedFile = { type: '', file: '' }
                  $("#facturaInputFile").val("")
                  return
              }

                this.facturas = this.facturasGuardar
            },
        //************************************************************
        verificarInformacionInicial: function (factura) {
                for (let i = 0; i < factura.length; i++) {
                    if (factura[i].length > 25) {
                        factura[i] = factura[i].substring(0, 25) + "...";
                    }
                }
                return factura
            },
        //************************************************************
        eliminar: function (index) {
                this.facturas.splice(index,1)
            },
        //************************************************************
        validarFactura: function (data) {
            let RE = ""
            
            this.catalogo.settings.map((dataH) => {
                if (dataH.abbreviation == "REGEXP_INVOICE") {
                    RE = new RegExp(dataH.content)
                }
            })
            this.msg  = ''
            var ahora = moment()
            var tomorrow = moment().add(1, "days")
            var vencimiento = data.expiration_date

                if ( data.amount <= 0) {
                    this.msg = "montoInvalido"
                    return true
                }

                if (data.original_amount <= 0) {
                    this.msg = "montoInvalido"
                    return true
                }

                if (data.amount > data.original_amount) {
                    this.msg = "amountSuperiorToOriginal"
                    return true
                }

                if (data.number == "") {
                    this.msg = "formatoNumeroInvalido"
                    return true
                }

                if (!RE.test(data.number)) {
                    this.msg = "formatoNumeroInvalido"
                    return true;
                }

                if (data.issued_date == "" || !moment(data.issued_date).isValid()) {
                    this.msg = "fechaEmisionInvalida"
                    return true
                }


                if (data.expiration_date == "" || !moment(data.expiration_date).isValid() ) {
                    this.msg = "fechaExpiracionInvalida";
                    return true
                }

                if (moment(data.issued_date, "YYYY-MM-DD").diff(ahora, "year") < -2) {
                    this.msg="fechaEmisionInvalida2"
                    return true
                }

                if (moment(data.issued_date, "YYYY-MM-DD").diff(ahora.format("YYYY-MM-DD"), "days") > 0) {
                    this.msg = "fechaEmisionInvalida3"
                    return true;
                }

                if (!data.issued_date.match(/\d{4}-\d{2}-\d{2}/)) {
                    this.msg = "fechaEmisionInvalida"
                    return true;
                }

                if (moment(data.expiration_date, "YYYY-MM-DD").diff(ahora, "year") > 2) {
                    this.msg = "fechaExpiracionInvalida2"
                    return true;
                }

                if (!data.expiration_date.match(/\d{4}-\d{2}-\d{2}/)) {
                    this.msg = "fechaExpiracionInvalida"
                    return true;
                }

                if (moment(data.expiration_date, "YYYY-MM-DD") < moment(data.issued_date, "YYYY-MM-DD")) {
                    this.msg = "fechaVencimientoMenorEmision"
                    return true;
                }

            if (moment(data.expiration_date, "YYYY-MM-DD").diff(ahora.format("YYYY-MM-DD"), "days") <= 4) {
                this.msg = "fechaVencimientoMenor4"
                return true;
                }

                    
            if (moment(data.expiration_date, "YYYY-MM-DD").diff(ahora.format("YYYY-MM-DD"), "days") <= 0) {
                    this.msg = "fechaVencimientoInvalida3"
                    return true;
                }
                if (moment(data.issued_date, "YYYY-MM-DD").diff(moment(data.expiration_date, "YYYY-MM-DD"), "days") >= 0) {
                    this.msg = "fechaIguales"
                    return true;
                }
            
                return false;
            },
        //
        agregarFactura: function () {

            if (!this.encuentraErrorFactura()) {

                this.enviarFactura({
                    country_id: this.nuevo.idPais,
                    supplier_id: this.nuevo.proveedor.id,
                    debtor_id: this.nuevo.idCliente,
                    number: this.nuevo.numero,
                    original_amount: parseFloat(formatoMoneda(this.nuevo.monto, this.lang)),
                    currency_id: this.nuevo.tipoMoneda,
                    amount:0,
                    //
                    issued_date: this.nuevo.fechaEmision,
                    expiration_date: this.nuevo.fechaVencimiento,
                    //

                    charges: this.deducciones
                });

                return
            }

            toastr.warning(i18n.t(this.message))

        },
        //
        encuentraErrorFactura: function () {
            this.error = false;
            var ahora = moment()

            let RE = ""


            this.catalogo.settings.map((data) => {
                if (data.abbreviation == "REGEXP_INVOICE") {
                    RE = new RegExp(data.content)
                }
            })

            if (formatoMoneda(this.nuevo.monto, this.lang) <= 0) {
                this.message = "montoInvalido"
                return true
            }

            if (!RE.test(this.nuevo.numero)) {
                this.message = "formatoNumeroInvalido"
                return true;
            }
            if (this.nuevo.numero.length >= 175) {
                this.message= ""
            }
            if (formatoMoneda(this.nuevo.monto, this.lang) - this.nuevo.totalDeducciones <= 0) {                
                return true;
            }
            if (this.nuevo.fechaEmision == "" || moment(this.nuevo.fechaEmision, "YYYY-MM-DD").diff(ahora, "year") < -2) {
                this.message = "fechaEmisionInvalida"
                return true
            }
            if (this.nuevo.fechaVencimiento == "" || moment(this.nuevo.fechaVencimiento, "YYYY-MM-DD").diff(ahora, "year") > 2) {
                this.message = "fechaExpiracionInvalida"
                return true;
            }
            
            if (!moment(this.nuevo.fechaEmision).isValid() || moment(this.nuevo.fechaEmision, "YYYY-MM-DD").diff(ahora, "days") > 0) {
                this.message = "fechaEmisionInvalida3"
                return true;
            }
            if (!moment(this.nuevo.fechaVencimiento).isValid() || moment(this.nuevo.fechaVencimiento, "YYYY-MM-DD").diff(ahora, "days") <= -1) {
                this.message = "fechaVencimientoInvalida3"
                return true;
            }

            if (moment(this.nuevo.fechaEmision, "YYYY-MM-DD").diff(moment(this.nuevo.fechaVencimiento, "YYYY-MM-DD"), "days") >= 0) {
                this.message = "fechaIguales"
                return true;
            }

            if (this.nuevo.proveedor.id == '0' || this.nuevo.proveedor.id == '') {
                this.message = "proveedorInvalido"
                return true;
            }
            

            if (this.nuevo.tipoMoneda <= 0) {
                this.message = "tipoMoneda"
                return true;
            }
            this.message = ""
            return false;
        },
        //
    },
    computed: {
        validarNominal() {
            if (this.nuevo.monto == null || this.nuevo.monto == '') {
                return true
            }

            let monto = formatoMoneda(this.nuevo.monto, this.lang)

            if (monto <= 0 || monto == '' || this.REPuntos.test(monto)) return true

            return false
        },
        porcentaje: {
            get: function () {
                if (this.nuevo.deduccion.monto == '' || this.nuevo.deduccion.monto == 0 || isNaN(formatoMoneda(this.nuevo.deduccion.monto, this.lang))) return ''
                let res = (formatoMoneda(this.nuevo.deduccion.monto, this.lang) * 100) / formatoMoneda(this.nuevo.monto, this.lang)
                return formatoMonedaInput(res, this.lang)
            },
            set: function (value) {
                this.nuevo.deduccion.monto = formatoMonedaInput((formatoMoneda(value, this.lang) * formatoMoneda(this.nuevo.monto, this.lang)) / 100, this.lang, this.digits)
            }
        },
        mensajesComputed() {
            return vuexLayout.state.mensajes
        }
    },
    created: function () {
        document.getElementById("appCargaMasiva").removeAttribute("hidden")
        this.lang = document.getElementsByTagName("html")[0].getAttribute("lang")

        this.cargando = false;
    },
    mounted: async function () {

        tiempoLogin(this.modalLogout)

        await this.llenarCatalogo()
        await this.llenarProveedores()
    }
});                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  