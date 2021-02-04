new Vue({
    el: "#appCargaManual",
    i18n,
    store: vuexLayout,
    vuetify: new Vuetify({
        lang: {
            t:(key, ...params) => i18n.t(key,params)
        }
    }),
    data: {
        modalLogout: { mostrar: false },
        arrayCondition: arrayCondition,
        settings: {},
        widthTelefono: widthTelefono,
        filter: {},
        currencies: {},
        totalFacturas: [],
        buscarFacturas: [],
        loading: [],
        options: [],
        estadoCarga: 0,
        dialog:false,
        dialogFactura: false,
        dialogFilter : false,
        dialogActualizarDeduccion: false,
        dialogEliminarDeduccion: false,
        deduccionIndexActual: -1,
        tamanoTlf: tamanoTlf,
        errorProveedor: false,
        errorNum: false,
        errorFactura: false,
        errorMoneda: false,
        enviando: false,
        errorOriginalAmount: false,
        errorIssued: {
            igual: false,
            superiorToDate: false,
            superior: false,
            vacio: false
        },
        errorExpired: {
            igual: false,
            inferior: false,
            inferiorToDate: false,
            vacio: false
        },
        errorInvoiceNumber: {
            vacio: false,
            superior: false,
            mismoProveedor: false,
            mismoCliente: false,
            invoiceExist: false,
            formatoInvalido: false
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
            superiorTreceDeduccion:false
        },
        errorPorcentaje: {
            vacio: false,
            superior: false,
            igual: false
        },
        errorAmount: false,
        errorDeduccion: false,
        errorNumeroDeduccion: false,
        errorDeduccionDuplicado:false,
        errorTipoDeduccion: false,
        errorMontoDeduccion: false,
        errorNetAmount:false,
        dialogDeduccion: false,
        dialogEliminar: false,
        moment: moment,
        formatoMonedaInput: formatoMonedaInput,
        backEndDateFormat: backEndDateFormat,
        formatoMoneda: formatoMoneda,
        lang: "es",
        digits: 2,
        maxLengthInvoice:0,
        envio: false,
        placeholderFactura:'',
        nmrDeduccion: '',
        idDeduccion: -1,
        REPuntos: /[.]{2,}/,
        REComas: /[,]{2,}/,
        montoValido: true,
        proveedorIdEditar: '',
        nmrFacturaEdita: '',
        message: '',
        error: false, //Validación de Errores
        errorD: false,//Validación de Errores en Deducciones
        cargando: true,
        indice: -1,
        factura: [],
        perPage: 10,
        itemsPerPageOptions: [10, -1],
        tipoMoneda: [],
        nombreMoneda: [],
        tipoFactura: [],
        proveedores: [],
        supplies: [],
        tipoDeduccion: [],
        deducciones: [],
        nuevo: {
            id: null,
            idPais: 0,
            idCliente: 0,
            numero: '',
            monto: 0.0,
            tipoMoneda: 0,
            nombreMoneda: '',
            state: '',
            fechaEmision: '',
            fechaVencimiento: '',
            proveedor: {
                id: '0',
                nombre: ''
            },
            totalDeducciones: 0.0,
            deduccion: {
                tipo: 0,
                numero: "",
                monto: 0.0
            }
        },
        headers1: [
            { text: i18n.t("headers.n"), value: "n", align: "center" },
            { text: i18n.t("headers.proveedor"), value: "supplier.name", align: "center" },
            { text: i18n.t("headers.numeroFactura"), value: "number", align: "center" },
            { text: i18n.t("headers.fechaVencimiento"), value: "expiration_date", align: "center" },
            { text: i18n.t("headers.valorNeto"), value: "amount", align: "center" },
            { text: i18n.t("headers.opciones"),  value: "opciones", align:"center"}
        ],
        headers2: [
            { text: i18n.t("headers.tipo"), value: "tipo", align: "center" },
            { text: i18n.t("headers.numero"), value: "number", align: "center" },
            { text: i18n.t("headers.monto"), value: "monto", align: "center" },
            { text: i18n.t("headers.opciones"), value: "opciones", align: "center"}
        ],
        headers3: [
            { text: i18n.t("headers.tipo"), value: "tipo", align: "center" },
            { text: i18n.t("headers.numero"), value: "number", align: "center" },
            { text: i18n.t("headers.monto"), value: "monto", align: "center" },
            { text: i18n.t("headers.opciones"), value: "opciones", align: "center" }
        ],
    },
    //
    created: function () {
        this.cargando = true;

        try {
            this.currencies = JSON.parse(document.getElementById('currenciesData').value);
            document.getElementById("eliminarData").removeChild(document.getElementById('currenciesData'));

            for (var i = 0; i < this.currencies.length; i++) {
                this.options.push({});
                this.buscarFacturas.push(true);
                this.loading.push(false);
                this.totalFacturas.push(0)

                this.filter[i] = JSON.parse(document.getElementById('filterData+' + i).value);
                this.filter[i].currency = this.currencies[i].id
                document.getElementById("eliminarData").removeChild(document.getElementById('filterData+' + i));
            }
        } catch (e) {
            console.log(e)
            for (var i = 0; i < this.currencies.length || i < 1; i++) {
                this.options.push({});
                this.buscarFacturas.push(true);
                this.loading.push(false);
                this.totalFacturas.push(0)

                this.filter[0] = null
            }
        }

        document.getElementById("appCargaManual").removeAttribute("hidden")
        this.lang = document.getElementsByTagName("html")[0].getAttribute("lang")
        this.cargando = false;

    },
    //
    mounted: async function () {
        tiempoLogin(this.modalLogout)

        for (var i = 0; i < this.currencies.length; i++) {
            this.buscarFacturas[i] = false
            await this.llenarFacturas(i);
        }

        await this.llenarCatalogo();
        await this.llenarProveedores();
    },
    watch: {
        options: {
            async handler() {

                for (var i = 0; i < this.currencies.length; i++) {
                    if (this.options[i].itemsPerPage == -1) {
                        llamadaRecursiva(this.buscarFacturas[i], this.llenarFacturas, i)
                        return
                    }

                    var tamanoFacturas = filtrarPublicationsCurrency(this.factura, this.currencies[i].id).length

                    if (this.buscarFacturas[i] && this.options[i].page * this.options[i].itemsPerPage >= tamanoFacturas - this.options[i].itemsPerPage) {
                        await this.llenarFacturas(i)
                    }
                }
            },
            deep: true,
        },
    },
    methods: {
        async llenarFacturas(indice) {
            if (this.loading[indice]) return
            this.loading[indice] = true

            var take = 100
            await axios.post('?handler=invoices', { pagination: { take: take, skip: this.totalFacturas[indice] }, filter: this.filter[indice] },
                {
                    headers: {
                        "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                    }
                })
                .then((respond) => {
                    console.log(respond)
                    resetTime()
                    this.loading[indice] = false
                    if (respond.data.length == 0) {
                        this.buscarFacturas[indice] = false
                        return
                    }

                    if (respond.data.length > 0 && respond.data[0].errors == notAuthorized) {
                        window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                        return
                    }

                    if (respond.data.length < take / 2)
                        this.buscarFacturas[indice] = false

                    this.totalFacturas[indice] += respond.data.length

                    respond.data.map(data => {
                        if (data == null) return 

                        data.issued_date = backEndDateFormat(data.issued_date);
                        data.expiration_date = backEndDateFormat(data.expiration_date);
                        if (data.charges === null)
                            data.charges = [];
                        
                        for (var i = 0; i < this.factura.length; i++) {
                            if (this.factura[i].id == data.id) {
                                this.totalFacturas --
                                return
                            }
                        }

                        this.factura.push(data) 
                    })
                    

                    this.ordenarLista()
                    this.loading = false
                }).catch((respond) => { console.log(respond); this.loading[indice] = false });

            return (this.buscarFacturas[indice] && this.options[indice].itemsPerPage == -1)
        },
        //
        async llenarCatalogo() {
            await axios.post('?handler=catalogo', {},
                {
                    headers: {
                        "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                    }
                })
                .then((respond) => {
                    resetTime()
                    if (respond.data == null) {
                        this.tipoFactura = [];
                        this.tipoDeduccion = [];
                        this.tipoMoneda = [];
                        return
                    }

                    if (respond.data.length > 0 && respond.data[0].errors == notAuthorized) {
                        window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                        return
                    }
                    
                    
                    var catalogo = JSON.parse(respond.data)

                    catalogo.settings.map((datos) => {
                        if (datos.abbreviation == "REGEXP_INVOICE") {
                            this.placeholderFactura = datos.mask_edit
                        }

                        if (datos.abbreviation == "MAXLEN_INVOICE") {
                            this.maxLengthInvoice = datos.content
                        }
                    })

                    this.settings = catalogo.settings;
                    this.tipoDeduccion = catalogo.charges;
                    this.tipoMoneda = catalogo.currencies;
                    this.nuevo.tipoMoneda = (this.tipoMoneda.length === 1 ? this.tipoMoneda[0].id : 0);
                    this.nuevo.idPais = catalogo.id;
                    this.digits = this.tipoMoneda.digits == null ? 2 : this.tipoMoneda.digits
                    
                }).catch((respond) => { console.log(respond); });
        },
        //
        async llenarProveedores() {
            
            await axios.post('?handler=proveedores', {},
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
        obtenerSymbolo() {
            for (let i = 0; i < this.tipoMoneda.length; i++) {
                if (this.tipoMoneda[i].id == this.nuevo.tipoMoneda) {
                    return this.tipoMoneda[i].symbol
                }
            }

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
        methodPublicationsCurrency(idCurrency) {
            return filtrarPublicationsCurrency(this.factura, idCurrency)
        },
        //
        validarInvoiceNumber() {

            let RE = ""

            this.settings.map((dataH) => {
                if (dataH.abbreviation == "REGEXP_INVOICE") {
                    RE = new RegExp(dataH.content)
                }
            })
            if (this.nuevo.numero == '' || this.nuevo.numero.length == 0) {
                this.errorInvoiceNumber.vacio = true
                return
            }

            this.errorInvoiceNumber.vacio = false

            if (this.nuevo.numero.length >= 175) {
                this.errorInvoiceNumber.superior = true
                return
            }

            this.errorInvoiceNumber.superior = false

            if (!RE.test(this.nuevo.numero)) {
                this.errorInvoiceNumber.formatoInvalido = true
                return
            }
            this.errorInvoiceNumber.formatoInvalido = false
        },
         //
        validarIssued() {

            if (this.nuevo.fechaEmision == '') {
                this.errorIssued.vacio = true
                return
            }

            this.errorIssued.vacio = false

            if (moment(this.nuevo.fechaEmision).diff(moment(),"days") > 0) {
                this.errorIssued.superior = true
                return
            }

            this.errorIssued.superior = false

            if (moment(this.nuevo.fechaEmision).diff(this.nuevo.fechaVencimiento) == 0) {
                this.errorIssued.igual = true
                return
            }

            this.errorIssued.igual = false

            if (this.nuevo.fechaVencimiento != '') this.validarExpired
        },
        //
        //
        validarPorcentaje() {

            if (this.porcentaje == 0 || this.porcentaje == '') {
                this.errorPorcentaje.vacio = true
                this.errorDeductionsAmount.vacio = false
                this.errorDeductionsAmount.superior = false
                this.errorDeductionsAmount.igualNominal = false
                this.errorDeductionsAmount.superiorNominal = false
                return
            }

            this.errorPorcentaje.vacio = false

            if (formatoMoneda(this.porcentaje, this.lang, 2) > 100 && formatoMoneda(this.porcentaje, this.lang, 2) <= formatoMoneda(9999, this.lang, 2)) {
                this.errorPorcentaje.superior = true
                this.errorDeductionsAmount.vacio = false
                this.errorDeductionsAmount.superior = false
                this.errorDeductionsAmount.igualNominal = false
                this.errorDeductionsAmount.superiorNominal = false
                return
            }

            this.errorPorcentaje.superior = false


            if (this.porcentaje == 100) {
                this.errorPorcentaje.igual = true
                this.errorDeductionsAmount.vacio = false
                this.errorDeductionsAmount.superior = false
                this.errorDeductionsAmount.igualNominal = false
                this.errorDeductionsAmount.superiorNominal = false
                return
            }

            this.errorPorcentaje.igual = false
            this.errorDeductionsAmount.vacio = false
            this.errorDeductionsAmount.superior = false
            this.errorDeductionsAmount.igualNominal = false
            this.errorDeductionsAmount.superiorNominal = false

        },
        //
//
        limpiarCampos() {
            //Llevar a Pantalla Principal
            this.estadoCarga = 0
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
            //Limpiar Inputs de Deducciones
            this.idDeduccion = -1
            this.nmrDeduccion = ''
            this.porcentaje
            this.nuevo.totalDeducciones = this.totalizarDeducciones(this.deducciones);
            this.nuevo.deduccion.monto = '';
            this.nuevo.deduccion.numero = '';
            this.nuevo.deduccion.tipo = 0;
            this.errorD = false;
            this.deduccionIndexActual = -1;
            
            
        },
        ///
        validarNominalAmount() {

            if (this.lang == "ESV" || this.lang == "ESS" || this.lang == "es") {
                var longitudMonto = this.nuevo.monto.split(",")
                if (longitudMonto[0].length > 17) {
                    this.errorDeductionsAmount.superiorTrece = true
                } else {
                    this.errorDeductionsAmount.superiorTrece = false
                }
            }
            else if (this.lang == "ENU" || this.lang == "en") {
                var amountLength = this.nuevo.monto.split(".")
                if (amountLength[0].length > 17) {
                    this.errorDeductionsAmount.superiorTrece = true
                } else {
                    this.errorDeductionsAmount.superiorTrece = false
                }
            }

        },
        //
        validarDeductionAmount() {

            if (this.nuevo.deduccion.monto == '' || this.nuevo.deduccion.monto == 0) {
                this.errorDeductionsAmount.vacio = true
                this.errorPorcentaje.vacio = false
                this.errorPorcentaje.igual = false
                this.errorPorcentaje.superior = false

                return
            }

            this.errorDeductionsAmount.vacio = false

            if (this.nuevo.deduccion.monto.length >= 17) {
                this.errorDeductionsAmount.superior = true
                this.errorPorcentaje.vacio = false
                this.errorPorcentaje.igual = false
                this.errorPorcentaje.superior = false

                return
            }

            this.errorDeductionsAmount.superior = false

            if (formatoMoneda(this.nuevo.deduccion.monto, this.lang, 2) == formatoMoneda(this.nuevo.monto, this.lang, 2)) {
                this.errorDeductionsAmount.igualNominal = true
                this.errorPorcentaje.vacio = false
                this.errorPorcentaje.igual = false
                this.errorPorcentaje.superior = false

                return
            }
            this.errorDeductionsAmount.igualNominal = false

            if (formatoMoneda(this.nuevo.deduccion.monto) > formatoMoneda(this.nuevo.monto)) {
                this.errorDeductionsAmount.superiorNominal = true
                this.errorPorcentaje.vacio = false
                this.errorPorcentaje.igual = false
                this.errorPorcentaje.superior = false

                return
            }

            this.errorDeductionsAmount.superiorNominal = false

            if (this.lang == "ESV" || this.lang == "ESS" || this.lang == "es") {
                var montoLongitud = this.nuevo.deduccion.monto.split(",")
                if (montoLongitud[0].length > 13) {
                    this.errorDeductionsAmount.superiorTreceDeduccion = true
                } else {
                    this.errorDeductionsAmount.superiorTreceDeduccion = false
                }
            }
            else if (this.lang == "ENU" || this.lang == "en") {
                var lengthAmount = this.nuevo.deduccion.monto.split(".")
                if (lengthAmount[0].length > 13) {
                    this.errorDeductionsAmount.superiorTreceDeduccion = true
                } else {
                    this.errorDeductionsAmount.superiorTreceDeduccion = false
                }
            } 

            this.errorPorcentaje.vacio = false
            this.errorPorcentaje.igual = false
            this.errorPorcentaje.superior = false

        },
        //
        validarDeductionNumber() {

            if (this.nuevo.deduccion.numero == '' || this.nuevo.deduccion.numero == 0) {
                this.errorDeductions.vacio = true
                return
            }

            this.errorDeductions.vacio = false

            if (this.nuevo.deduccion.numero.length > 255) {
                this.errorDeductions.superior = true
                return
            }

            this.errorDeductions.superior = false
        },
        //
        validarExpired() {

            if (this.nuevo.fechaVencimiento == '') {
                this.errorExpired.vacio = true
                return
            }

            this.errorExpired.vacio = false

            if (moment(this.nuevo.fechaVencimiento).diff(moment(), "days") < 0) {
                this.errorExpired.inferior = true
                return
            }

            this.errorExpired.inferior = false

            if(moment(this.nuevo.fechaVencimiento).diff(this.nuevo.fechaEmision) == 0)
            {
                this.errorExpired.igual = true
                return
            }

            this.errorExpired.igual = false

            if (this.nuevo.fechaEmision != '') this.validarIssued
        },
        //inicializa los valores de los input's
        reiniciar: function () {
            this.proveedorIdEditar = ''
            this.nmrFacturaEdita = ''
            this.error = false; //Validación de Errores
            this.errorD = false;//Validación de Errores en Deducciones
            this.cargando = false;
            this.nuevo.id = null;
            this.nuevo.idPais = 0;
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
        //Muestra el la tabla el nombre de la Deducción a partir del id guardado
        nombreCargo: function (id) {
            for (var i = 0; i < this.tipoDeduccion.length; i++) {
                if (this.tipoDeduccion[i].id == id)
                    return this.tipoDeduccion[i].name;
            }
            return 'Sin Nombre';
        },
        //
        botonVerde: function () {
            return (this.indice == -1 ? "Guardar" : "Modificar");
        },
        //
        botonRojo: function () {
            return (this.indice == -1 ? "Limpiar" : "Cancelar");
        },
        //
        encuentraErrorFactura: function () {
            this.error = false;
            let RE = ""

            this.settings.map((dataH) => {
                if (dataH.abbreviation == "REGEXP_INVOICE") {
                    RE = new RegExp(dataH.content)
                }
            })
            var ahora = moment()

            if (formatoMoneda(this.nuevo.monto,this.lang) <= 0) {
                this.message = "montoInvalido"
                return true
            }

            if (!RE.test(this.nuevo.numero)) {
                this.message = "formatoNumeroInvalido"
                return true;
            }
            if (formatoMoneda(this.nuevo.monto, this.lang) - this.nuevo.totalDeducciones <= 0) {
                this.message = "valorNetoNegativo"
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
            if (!moment(this.nuevo.fechaVencimiento).isValid() || moment(this.nuevo.fechaVencimiento, "YYYY-MM-DD").diff(ahora, "days") < 0) {
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
        //valida los errores en la carga de datos de los input's de las Deducciones
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
        //Guarda los datos en el Arreglo de Facturas para enviar al servidor
        agregarFactura: function () {

            if (!this.encuentraErrorFactura()) {
                this.enviarFactura({
                    id: this.nuevo.id,
                    supplier_id: this.nuevo.proveedor.id,
                    //debtor_id: this.nuevo.idCliente,
                    number: this.nuevo.numero,
                    original_amount: formatoMoneda(this.nuevo.monto, this.lang),
                    currency_id: this.nuevo.tipoMoneda,
                    issued_date: this.nuevo.fechaEmision,
                    expiration_date: this.nuevo.fechaVencimiento,
                    charges: this.deducciones
                });
                
                return
            }

           // toastr.warning(i18n.t(this.message))
        },
        //Guarda los datos en el Arreglo de Deducciones para guardar en el arreglo de Facturas
        agregarDeducciones: async function () {
            if (!this.encuentraErrorDeduccion()) {

                if (this.idDeduccion != -1) {
                    this.actualizarDeduccion()
                    return
                }
                
                let suma = formatoMoneda(this.nuevo.deduccion.monto, this.lang)

                if (parseFloat(suma) > parseFloat(formatoMoneda(this.nuevo.monto,this.lang))) {
                    //toastr.warning(i18n.t("deduccionesSuperanMonto"))
                    this.errorD = true
                    this.errorDeductionsAmount.superior = true
                    this.errorPorcentaje.superior = true
                    this.errorDeductionsAmount.superiorNominal = true
                    return
                }

                for (let i = 0; i < this.deducciones.length; i++) {
                    suma = parseFloat(suma) + parseFloat(this.deducciones[i].amount)

                    if (parseFloat(suma) > parseFloat(formatoMoneda(this.nuevo.monto, this.lang))) {
                        //toastr.warning(i18n.t("deduccionesSuperanMonto"))
                        this.errorPorcentaje.superior = true
                        this.errorDeductionsAmount.superiorNominal = true
                        this.errorD = true;
                        return
                    }
                }

                await this.crearDeduccion()

                this.errorD = false;
            } else
                this.errorD = true
            
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
        crearDeduccion: async function () {
            if (this.envio) return

            this.envio = true 
            

            await axios.post('?handler=CreateDeduction', {
                invoice_id: this.nuevo.id,
                currency_id: this.nuevo.tipoMoneda,
                charge_type_id: this.nuevo.deduccion.tipo,
                number: this.nuevo.deduccion.numero,
                amount: formatoMoneda(this.nuevo.deduccion.monto,this.lang),
            },
                {
                    headers: {
                        "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                    }
                })
                .then((respuesta) => {
                    resetTime()
                    this.envio = false
                    var resp = ''

                    if (respuesta.data == null)
                        resp = respuesta
                    else
                        resp = respuesta.data
                    
                    if (resp.error != null && resp.error == "Deducciones superan monto") {
                        toastr.warning(i18n.t("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.t("mensajesModal.crearDeduccionMayorMonto"))
                        return
                    } else if (resp.length > 0 && resp[0].errors == notAuthorized){
                        window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                        return
                    }
                    
                    this.deducciones.push({
                        id: resp.id,
                        currency_id: this.nuevo.tipoMoneda,
                        charge_type_id: this.nuevo.deduccion.tipo,
                        number: this.nuevo.deduccion.numero,
                        amount: formatoMoneda(this.nuevo.deduccion.monto, this.lang),
                    })
                    this.nuevo.totalDeducciones = this.totalizarDeducciones(this.deducciones);
                    this.factura[this.indice].charges = this.deducciones;
                    this.factura[this.indice].amount = formatoMoneda(this.nuevo.monto, this.lang) - this.nuevo.totalDeducciones;
                    toastr.success(i18n.t("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.t("mensajesModal.creacionDeduccion"))
                    this.errorDeductionsAmount.vacio = false
                    this.errorDeductionsAmount.superior = false
                    this.errorDeductionsAmount.igualNominal = false
                    this.errorDeductionsAmount.superiorNominal = false
                    this.errorPorcentaje.vacio = false
                    this.errorPorcentaje.igual = false
                    this.errorPorcentaje.superior = false
                    this.limpiarDeducciones()
                }).catch((respond) => { toastr.error(i18n.t("errorRespuesta")); this.envio = false; this.nuevo.deduccion.id = ''; });

        },
        //
        actualizarDeduccion: async function () {
            if (this.envio) return

            this.envio = true

            let suma = formatoMoneda(this.nuevo.deduccion.monto, this.lang) - this.deducciones[this.idDeduccion].amount

            if (parseFloat(suma) > parseFloat(formatoMoneda(this.nuevo.monto, this.lang))) {
                //toastr.warning(i18n.t("deduccionesSuperanMonto"))
                this.errorPorcentaje.superior = true
                this.errorDeductionsAmount.superiorNominal = true
                this.errorD = true
                this.envio = false
                return
            }

            for (let i = 0; i < this.deducciones.length; i++) {
                suma = parseFloat(suma) + parseFloat(this.deducciones[i].amount)

                if (parseFloat(suma) > parseFloat(formatoMoneda(this.nuevo.monto, this.lang))) {
                    //toastr.warning(i18n.t("deduccionesSuperanMonto"))
                    this.errorPorcentaje.superior = true
                    this.errorDeductionsAmount.superiorNominal = true
                    this.errorD = true;
                    this.envio = false;
                    return
                }
            }

            this.errorDeductionsAmount.vacio = false
            this.errorDeductionsAmount.superior = false
            this.errorDeductionsAmount.igualNominal = false
            this.errorDeductionsAmount.superiorNominal = false
            this.errorPorcentaje.vacio = false
            this.errorPorcentaje.igual = false
            this.errorPorcentaje.superior = false


            await axios.post('?handler=UpdateDeduction', {
                id: this.nuevo.deduccion.id,
                charge_type_id: this.nuevo.deduccion.tipo,
                invoice_id: this.nuevo.id,
                currency_id:this.nuevo.tipoMoneda,
                number: this.nuevo.deduccion.numero,
                amount: formatoMoneda(this.nuevo.deduccion.monto, this.lang),
            },
                {
                    headers: {
                        "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                    }
                })
                .then((respuesta) => {
                    resetTime()
                    this.envio = false
                    var resp = ''

                    if (respuesta.data == null)
                        resp = respuesta
                    else
                        resp = respuesta.data

                    if (resp.error != null) {
                        toastr.warning(i18n.tc("mensajesModal.problemasRespuesta", 2, { 0: "Actualizar Deducción" }))
                        return
                    } else if (resp.length > 0 && resp[0].errors == notAuthorized) {
                        window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                        return
                    }

                    this.deducciones[this.idDeduccion].amount = parseFloat(formatoMoneda(this.nuevo.deduccion.monto, this.lang))
                    this.deducciones[this.idDeduccion].charge_type_id = this.nuevo.deduccion.tipo
                    this.deducciones[this.idDeduccion].number = this.nuevo.deduccion.numero
                    this.nuevo.totalDeducciones = this.totalizarDeducciones(this.deducciones);
                    this.factura[this.indice].charges = this.deducciones;
                    this.factura[this.indice].amount = formatoMoneda(this.nuevo.monto, this.lang) - this.nuevo.totalDeducciones;
                    toastr.success(i18n.t("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.t("mensajesModal.actualizarDeduccion"))
                    this.limpiarDeducciones()
                }).catch((respond) => { toastr.error(i18n.t("errorRespuesta")); this.envio = false; });
        },
        //
        editarDeduccion:  function (index) {
            this.nuevo.deduccion.id = this.deducciones[index].id
            this.nuevo.deduccion.tipo = this.deducciones[index].charge_type_id
            this.nuevo.deduccion.numero = this.deducciones[index].number
            this.nuevo.deduccion.monto = this.formatoMonedaInput(this.deducciones[index].amount,this.lang,this.digits)
            this.idDeduccion = index
        },
        //Eliminar deducción del Arreglo
        eliminarDeduccion: async function (index) {
            if (this.envio) return
            if (index < 0 || index >= this.deducciones.length) {
                toastr.warning(i18n.t("errorAccion"))
                return
            }
            
            this.envio = true

            await axios.post('?handler=DeleteDeduction', { id: this.deducciones[index].id},
                {
                    headers: {
                        "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                    }
                })
                .then((respuesta) => {
                    resetTime()
                    this.envio = false
                    var resp = ''

                    if (respuesta.data == null)
                        resp = respuesta
                    else
                        resp = respuesta.data

                    if (resp.error != null) {
                        toastr.warning(i18n.tc("mensajesModal.problemasRespuesta", 2, { 0: "Eliminar Deducción" }))
                        return
                    } else if (resp.length > 0 && resp[0].errors == notAuthorized) {
                        window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                        return
                    }

                    if (this.idDeduccion != -1) {
                        this.idDeduccion = -1
                        this.nmrDeduccion = ""
                        this.nuevo.deduccion.monto = ""
                        this.nuevo.deduccion.numero = ""
                        this.nuevo.deduccion.tipo = 0
                    }

                    this.deducciones.splice(index, 1);
                    this.nuevo.totalDeducciones = this.totalizarDeducciones(this.deducciones);
                    this.factura[this.indice].charges = this.deducciones;
                    this.factura[this.indice].amount = formatoMoneda(this.nuevo.monto, this.lang) - this.nuevo.totalDeducciones;
                    toastr.success(i18n.t("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.t("mensajesModal.deleteDeduccion"))
                }).catch((respond) => { toastr.error(i18n.t("errorRespuesta")); this.envio = false; });

           
        },
        //
        dialogoActualizar: function () {
            this.dialogActualizarDeduccion = true
            this.deduccionIndexActual = this.idDeduccion
        },
        //Eliminar Factura del Arreglo
        deleteFactura: async function (index) {
            this.cargando = true;
            // Elimina la Factura de la Base de datos
            if (this.factura[index].id != null){
                await this.eliminarFactura(this.factura[index], index);
            } else {
                toastr.warning(i18n.t("facturaInvalida"))
            }
            this.cargando = false;
        },
        //Elimina una factura
        eliminarFactura: async function (factura, index) {
            if (this.envio) return

            this.envio = true
            await axios.post('?handler=eliminar', {
                invoice_id: factura.id
            },
                {
                    headers: {
                        "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                    }
                })
                .then((respond) => {
                    resetTime()
                    if (respond.data.errors != null || respond.data.errors != undefined) {
                        toastr.warning(i18n.t("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.t("mensajesModal.eliminacionFacturaProblema"))
                        this.envio = false
                        return
                    } else if (respond.data.length > 0 && respond.data[0].errors == notAuthorized) {
                        window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                        return
                    }
                    this.envio = false
                    toastr.success(i18n.t("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.t("mensajesModal.eliminacionFactura"))

                    this.factura.splice(index, 1)
                }).catch((respond) => { toastr.error(i18n.t("errorRespuesta")); this.envio = false; });

            this.index = -1

        },
        //
        editFactura: async function (index) {
            this.indice = index;
            this.nuevo.id = this.factura[index].id
            this.nuevo.numero = this.factura[index].number;
            this.nuevo.monto = formatoMonedaInput(this.factura[index].original_amount, this.lang, this.digits);
            this.nuevo.proveedor.id = this.factura[index].supplier.id;
            this.nuevo.tipoMoneda = this.factura[index].currency.id;

            this.proveedorIdEditar = this.nuevo.proveedor.id
            this.nmrFacturaEdita = this.nuevo.numero

            this.nuevo.fechaEmision = moment(this.factura[index].issued_date, "DD/MM/YYYY").format("YYYY-MM-DD")
            this.nuevo.fechaVencimiento = moment(this.factura[index].expiration_date, "DD/MM/YYYY").format("YYYY-MM-DD")

            this.nuevo.state = this.factura[index].state

            this.deducciones = [];
            this.nuevo.totalDeducciones = 0.0;
            if (this.factura[index].charges != null)
                for (var i = 0; i < this.factura[index].charges.length; i++) {
                    this.nuevo.totalDeducciones += this.factura[index].charges[i].amount;
                    this.deducciones.push({
                        id: this.factura[index].charges[i].id,
                        amount: this.factura[index].charges[i].amount,
                        number: this.factura[index].charges[i].number,
                        charge_type_id: this.factura[index].charges[i].charge_type_id
                    });
                }
            //

        },
        //Totaliza las deducciones para mostrarlas en la pantalla
        totalizarDeducciones: function (deducciones) {
            if (deducciones === null) return 0;
            let totalD = 0;
            for (var i = 0; i < deducciones.length; i++) {
                totalD += (deducciones[i].amount);
            }
            return totalD;
        },
        //
        enviarFactura: async function (factura) {
            this.cargando = true;


            
            await this.actualizarFactura(factura);
            this.cargando = false;
        },
        //
        valorNeto: function () {
            let monto
            if (this.nuevo.monto == 0 || this.nuevo.monto == '') return ''

            monto = formatoMoneda(this.nuevo.monto, this.lang) - this.nuevo.totalDeducciones
            
            return formatoMonedaInput(monto, this.lang, this.digits)
        },
        //Guarda los datos de una factura
        guardarFactura: async function (factura) {
            if (this.envio) return

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

                    if (respuesta.data == null)
                        resp = respuesta
                    else
                        resp = respuesta.data
                    
                    if (resp.errors != null) {

                        if (resp.errors == "Invalid User") {
                            window.location.pathname = "../Index"
                            return
                        } else if (resp.errors == "invoice found") {
                            toastr.warning(i18n.t("facturaExistente"))
                        } else if (resp.length > 0 && resp[0].errors == notAuthorized) {
                            window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                            return
                        }
                        else {
                            toastr.warning(i18n.tc("mensajesModal.problemasRespuesta", 2, {0:"registro de factura"}))
                        }

                       
                    } else {
                        toastr.success(i18n.t("mensajesModal.estimadoUsuario")+"<br><br>"+i18n.t("mensajesModal.cargaFactura"))

                        resp.status = 0
                        this.factura.push(resp)
                        this.factura[this.factura.length - 1].expiration_date = backEndDateFormat(resp.expiration_date)
                        this.factura[this.factura.length - 1].issued_date = backEndDateFormat(resp.issued_date)
                        

                        this.ordenarLista();

                    }

                    this.reiniciar();
                }).catch((respond) => { toastr.error(i18n.t("errorRespuesta")); this.envio = false; });
        },
        //Modifica los datos de una Factura
        actualizarFactura: async function (factura) {
            if (this.envio) return
            let ahora = moment()
            if (moment(factura.expiration_date, "YYYY-MM-DD").diff(ahora, "days") <= 3) {
                toastr.warning(i18n.tc("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.tc("mensajesModal.fechaVencimientoTermDays"))
                return false
            }

            this.envio = true
            await axios.post('?handler=Actualizar', {
                        supplier_id: factura.supplier_id,
                        number: factura.number,
                        original_amount: factura.original_amount,
                        currency_id: factura.currency_id,
                        issued_date: factura.issued_date,
                        expiration_date: factura.expiration_date,
                        charges: factura.charges,
                        id: factura.id
                },
                {
                    headers: {
                        "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                    }
                })
                .then((respond) => {
                    resetTime()
                    this.envio = false
                    console.log(respond)
                    if (respond.data.errors != null || respond.data.errors != undefined) {

                        if (respond.data.errors == "Invalid User") {
                            window.location.pathname = "../Index"
                            return
                        } else if (respond.data.errors == "invoice already exist") {
                            toastr.warning(i18n.t("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.t("facturaExistenteEdicion", { 0: factura.number }))
                            return
                        } else if (respond.data.length > 0 && respond.data[0].errors == notAuthorized) {
                            window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                            return
                        } else if (respond.data.length > 0 && respond.data[0].errors == "Invalid invoice to update") {
                            toastr.warning(i18n.t("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.t("facturaNotUpdate", { 0: factura.number }))
                        }

                        //toastr.warning(i18n.t("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.t("facturaExistenteEdicion", { 0: factura.number }))
                    } else {
                        toastr.success(i18n.t("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.t("mensajesModal.actualizarFactura"))
                        this.renovarFactura()
                        this.reiniciar()
                    }

                    
                }).catch((respond) => { toastr.error(i18n.t("errorRespuesta")); this.envio = false; });
        },
        //
        ordenarLista: async function () {
            try {
                this.factura.sort((a, b) => a.supplier.name.toUpperCase() < b.supplier.name.toUpperCase() ? -1 : +a.supplier.name.toUpperCase() > b.supplier.name.toUpperCase())
            } catch (e) {
                console.log(e)
            }
        },
        //
        renovarFactura: async function () {
            this.factura[this.indice].supplier_id = this.nuevo.proveedor.id;
            //this.factura[this.indice].debtor_id = this.nuevo.idCliente;
            this.factura[this.indice].number = this.nuevo.numero;
            this.factura[this.indice].original_amount = parseFloat(formatoMoneda(this.nuevo.monto, this.lang));
            this.factura[this.indice].amount = formatoMoneda(this.nuevo.monto, this.lang) - this.nuevo.totalDeducciones;    
            this.factura[this.indice].currency_id = this.nuevo.tipoMoneda;
            this.factura[this.indice].issued_date = moment(this.nuevo.fechaEmision, "YYYY-MM-DD").format("DD/MM/YYYY");
            this.factura[this.indice].expiration_date = moment(this.nuevo.fechaVencimiento, "YYYY-MM-DD").format("DD/MM/YYYY");
            this.factura[this.indice].charges = this.deducciones;
            this.factura[this.indice].term_days = diferenciaFechas(this.factura[this.indice].expiration_date)
        },
    },
    //
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
                if (this.nuevo.deduccion.monto == '' || this.nuevo.deduccion.monto == 0 || isNaN(formatoMoneda(this.nuevo.deduccion.monto,this.lang))) return ''
                
                let res = (formatoMoneda(this.nuevo.deduccion.monto, this.lang) * 100) / formatoMoneda(this.nuevo.monto,this.lang)
                return formatoMonedaInput(res, this.lang)
            },
            set: function (value) {
                this.nuevo.deduccion.monto = formatoMonedaInput((formatoMoneda(value, this.lang) * formatoMoneda(this.nuevo.monto, this.lang)) / 100, this.lang, this.digits)
            }
        },
    },

});;
