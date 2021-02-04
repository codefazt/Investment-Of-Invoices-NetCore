var app = new Vue({
    el: '#appConsultas',
    store: vuexLayout,
    i18n,
    vuetify: new Vuetify({
        lang: {
            t: (key, ...params) => i18n.t(key, params)
        }
    }),
    data: {
        isStatus: isStatus,
        getDateStatus: getDateStatus,
        i18n:i18n,
        loadingDetail: false,
        modalLogout: { mostrar: false },
        widthTelefono: widthTelefono,
        isMyOffert: isMyOffert,
        lang: "es",
        symbol: "$",
        iso_4217: "",
        digits: 2,
        owner:"",
        montoTotalPagar: 0,
        backEndDateFormat: backEndDateFormat ,
        formatoMonedaInput: formatoMonedaInput,
        cargando: true,
        abrirModal: false,
        x: null,
        buscarFacturas: true,
        headerConsultaD: [
            { text: i18n.t("headers.n"), value: "n", align: "center" },
            { text: i18n.t("headers.program"), value: 'program', sortable: false, align: "center" },
            { text: i18n.t("headers.proveedor"), value: 'supplier.name', sortable: false },
            { text: i18n.t("headers.numeroFactura"), value: 'number', align: 'center', sortable: false },
            { text: i18n.t("headers.fechaVencimiento"), value: 'expiration_date', align: 'center', sortable: false },
            { text: i18n.t("headers.monto"), value: 'amount', align: 'center', sortable: false },
            { text: i18n.t("headers.financiamiento"), value: 'request_financing', align: 'center', sortable: false },
            { text: i18n.t("headers.opciones"), value: 'options', align: 'center', sortable: false },
        ],
        headerConsultaDFintech: [
            { text: i18n.t("headers.n"), value: "n", align: "center" },
            { text: i18n.t("headers.program"), value: 'program', sortable: false, align: "center" },
            { text: i18n.t("headers.proveedor"), value: 'supplier.name', sortable: false },
            { text: i18n.t("headers.numeroFactura"), value: 'number', align: 'center', sortable: false },
            { text: i18n.t("headers.fechaVencimiento"), value: 'expiration_date', align: 'center', sortable: false },
            { text: i18n.t("headers.monto"), value: 'amount', align: 'center', sortable: false },
            { text: i18n.t("headers.opciones"), value: 'options', align: 'center', sortable: false },
        ],
        headerConsultaS: [
            { text: i18n.t("headers.n"), value: "n", align: "center" },
            { text: i18n.t("headers.program"), value: 'program', sortable: false, align: "center" },
            { text: i18n.t("headers.cliente"), value: 'debtor.name', sortable: false },
            { text: i18n.t("headers.numeroFactura"), value: 'number', align: 'center', sortable: false },            
            { text: i18n.t("headers.fechaVencimiento"), value: 'expiration_date', align: 'center', sortable: false },
            { text: i18n.t("headers.monto"), value: 'amount', align: 'center', sortable: false },
            { text: i18n.t("headers.opciones"), value: 'options', align: 'center', sortable: false },  
        ],
        headerConsultaF: [
            { text: i18n.t("headers.n"), value: "n", align: "center" },
            { text: i18n.t("headers.program"), value: 'program', sortable: false, align: "center" },
            { text: i18n.t("headers.proveedor"), value: 'supplier.name', sortable: false },
            { text: i18n.t("headers.bancoConfirmante"), value: 'publications[0].entity.person.name', align:'center', sortable: false },
            { text: i18n.t("headers.numeroFactura"), value: 'number', align: 'center', sortable: false },          
            { text: i18n.t("headers.fechaVencimiento"), value: 'expiration_date', align: 'center', sortable: false },
            { text: i18n.t("headers.monto"), value: 'amount', align: 'center', sortable: false },
            { text: i18n.t("headers.opciones"), value: 'options', align: 'center', sortable: false },  
        ],
        headerConsultaC: [
            { text: i18n.t("headers.n"), value: "n", align: "center" },
            { text: i18n.t("headers.program"), value: 'program', sortable: false, align: "center" },
            { text: i18n.t("headers.proveedor"), value: 'supplier.name', sortable: false },
            { text: i18n.t("headers.cliente"), value: 'debtor.name', sortable: false },
            { text: i18n.t("headers.numeroFactura"), value: 'number', align: 'center', sortable: false },
            { text: i18n.t("headers.fechaVencimiento"), value: 'expiration_date', align: 'center', sortable: false },
            { text: i18n.t("headers.monto"), value: 'amount', align: 'center', sortable: false },
            { text: i18n.t("headers.financiamiento"), value: 'request_financing', align: 'center', sortable: false},
            { text: i18n.t("headers.opciones"), value: 'options', align: 'center', sortable: false },  
        ],
        headerConsultaCFintech: [
            { text: i18n.t("headers.n"), value: "n", align: "center" },
            { text: i18n.t("headers.program"), value: 'program', sortable: false, align: "center" },
            { text: i18n.t("headers.proveedor"), value: 'supplier.name', sortable: false },
            { text: i18n.t("headers.cliente"), value: 'debtor.name', sortable: false },
            { text: i18n.t("headers.numeroFactura"), value: 'number', align: 'center', sortable: false },
            { text: i18n.t("headers.fechaVencimiento"), value: 'expiration_date', align: 'center', sortable: false },
            { text: i18n.t("headers.monto"), value: 'amount', align: 'center', sortable: false },
            { text: i18n.t("headers.opciones"), value: 'options', align: 'center', sortable: false },
        ],
        headerConsultaB: [
            { text: i18n.t("headers.n"), value: "n", align: "center" },
            { text: i18n.t("headers.program"), value: 'program', sortable: false, align: "center" },
            { text: i18n.t("headers.proveedor"), value: 'supplier.name', sortable: false },
            { text: i18n.t("headers.cliente"), value: 'debtor.name', sortable: false },
            { text: i18n.t("headers.numeroFactura"), value: 'number', align: 'center', sortable: false },
            { text: i18n.t("headers.fechaVencimiento"), value: 'expiration_date', align: 'center', sortable: false },
            { text: i18n.t("headers.originalAmount"), value: 'original_amount', align: 'center', sortable: false },
            { text: i18n.t("headers.monto"), value: 'amount', align: 'center', sortable: false },
            { text: i18n.t("headers.financiamiento"), value: 'request_financing', align: 'center', sortable: false },
            { text: i18n.t("headers.opciones"), value: 'options', align: 'center', sortable: false },       
        ],
        headerConsultaBFintech: [
            { text: i18n.t("headers.n"), value: "n", align: "center" },
            { text: i18n.t("headers.program"), value: 'program', sortable: false, align: "center" },
            { text: i18n.t("headers.proveedor"), value: 'supplier.name', sortable: false },
            { text: i18n.t("headers.cliente"), value: 'debtor.name', sortable: false },
            { text: i18n.t("headers.numeroFactura"), value: 'number', align: 'center', sortable: false },
            { text: i18n.t("headers.fechaVencimiento"), value: 'expiration_date', align: 'center', sortable: false },
            { text: i18n.t("headers.originalAmount"), value: 'original_amount', align: 'center', sortable: false },
            { text: i18n.t("headers.monto"), value: 'amount', align: 'center', sortable: false },
            { text: i18n.t("headers.opciones"), value: 'options', align: 'center', sortable: false },
        ],
        hasSuccess: 'is-valid',
        hasError: 'is-invalid',
        errorProveedor: false,
        moment: moment,
        loading: false,
        options: {},
        envio: false,
        totalFacturas: 0,

        facturas: [],
        data: null,
        filterData: [],
        participant: null,

    },
    watch: {
        options: {
            async handler() {

                if (this.options.itemsPerPage == -1) {
                    llamadaRecursiva(this.buscarFacturas, this.llenar_consulta)
                    return
                }

                if (this.buscarFacturas && this.options.page * this.options.itemsPerPage >= this.facturas.length - this.options.itemsPerPage) {
                    await this.llenar_consulta();
                }
            },
            deep: true,
        },
    },
    created: function () {
        
        try {
            this.lang = langActual
            this.digits = digitsActual
            this.filter = JSON.parse(document.getElementById('filterData').value);
            document.getElementById("eliminarData").removeChild(document.getElementById('filterData'));
        } catch (e) {
            this.filter = null
        }

        try {
            this.owner = document.getElementById('owner').value;
            document.getElementById("eliminarData").removeChild(document.getElementById('owner'));
        } catch (e) {}

        document.getElementById('contenido').removeAttribute('hidden');
        this.cargando = false;

    },
    mounted: async function () {
        await this.llenar_consulta();
        tiempoLogin(this.modalLogout)
    },
    methods: {

        estado_factura(estado) {
            estado_actual = '-';

            switch (estado) {
                case "created": estado_actual = i18n.t("CREATED_INVOICE"); break;
                case "draft": estado_actual = i18n.t("CREATED_INVOICE"); break;
                case "revised": estado_actual = i18n.t("REVISED_INVOICE"); break;
                case "posted": estado_actual = i18n.t("POSTULATED_INVOICE"); break;
                case "rejected": estado_actual = i18n.t("REJECTED"); break;
                case "rejectedForClient": estado_actual = i18n.t("REJECTED_FOR_CLIENT"); break;
                case "confirmed": estado_actual = i18n.t("CONFIRMED_INVOICE"); break;
                case "confirmedForClient": estado_actual = i18n.t("CONFIRMED_FOR_CLIENT"); break;
                case "offered": estado_actual = i18n.t("OFFERED_INVOICE"); break;
                case "offeredBank": estado_actual = i18n.t("OFFERED_BANK_INVOICE"); break;
                case "published": estado_actual = i18n.t("PUBLISHED_INVOICE"); break;
                case "released": estado_actual = i18n.t("RELEASED_INVOICE"); break;
                case "sold": estado_actual = i18n.t("SOLD_INVOICE"); break;
                case "soldBank": estado_actual = i18n.t("SOLD_BANK_INVOICE"); break;
                case "soldSupplier": estado_actual = i18n.t("SOLD_SUPPLIER_INVOICE"); break;
                case "postponed": estado_actual = i18n.t("POSTPONED_INVOICE"); break;
                case "overdue": estado_actual = i18n.t("EXPIRED_INVOICE"); break;
                case "paid": estado_actual = i18n.t("PAID"); break;
                case "finalize": estado_actual = i18n.t("FINALIZE_INVOICE"); break;
                case "finalizeBank": estado_actual = i18n.t("FINALIZE_BANK_INVOICE"); break;
                case "processing": estado_actual = i18n.t("PROCESSING_INVOICE"); break;
                case "paidToOverdue": estado_actual = i18n.t("PAID_OVERDUE_INVOICE"); break;
                case "confirmedToOverdue": estado_actual = i18n.t("CONFIRMED_OVERDUE_INVOICE"); break;
                case "soldToOverdue": estado_actual = i18n.t("SOLD_OVERDUE_INVOICE"); break;
                case "soldToOverdueSupplier": estado_actual = i18n.t("SOLD_OVERDUE_SUPPLIER_INVOICE"); break;
            }

            return estado_actual;
        },

        async llenar_consulta(factura) {
            if (this.loading) return
            this.loading = true

            let take = 100;
            const data = await axios.post('Consultas?handler=LlenarConsulta', { pagination: { take: take, skip: this.totalFacturas }, filter: this.filter }, { headers: { "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val() } })
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

            if (data.data.length < (take/2)) this.buscarFacturas = false
            this.totalFacturas += parseInt(data.data.length)

            data.data.map(data => {
                if (data == null) return

                this.symbol = data.currency.symbol
                this.iso_4217 = data.currency.iso_4217


                this.facturas.push(data)
                this.montoTotalPagar += data.amount
            })

            if (this.montoTotalPagar == null || this.montoTotalPagar == "") {
                this.montoTotalPagar = 0
            }
            
            this.loading = false;
            return this.buscarFacturas
        },
        //
        async detalles_facturas(factura) {
            if (this.loadingDetail) return
            this.loadingDetail = true

            const data = await axios.post('Consultas?handler=Detalle', factura, { headers: { "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val() } })
                .then(function (response) {
                    this.loadingDetail = false
                    resetTime()
                    return response;
                }).catch(e => {
                    this.loadingDetail = false
                });

            this.loadingDetail = false
            if (typeof data.data === 'string' || data.data instanceof String) {

                if (data.data.includes("<!DOCTYPE html>")) {
                    window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired";
                    toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br>" + i18n.t("errorBaseDatos"));
                    return;
                }
            }

            if (data.data != null && data.data.errors == notAuthorized) {
                window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                return
            }

            if (data.data == null || data.data.errors != null) {
                toastr.warning(i18n.t("errorRespuestaDetalles"));
                return
            }

            this.data = data.data;
            this.data.state_mostrar = factura.state_mostrar
            if (this.data.publications != null && this.data.publications[0] != null && factura.publications != null && factura.publications[0] != null) {
                this.data.publications[0].state_mostrar = factura.publications[0].state_mostrar
            }
            
            $('#discountModal').modal({
                show: true
            }); 
            

        },
   
    },
    computed: {
        validarNominal() {
            if (this.nuevo.monto == null || this.nuevo.monto == '') {
                return true
            }

            let monto = this.cambiarFormatoMoneda(this.nuevo.monto, 1)

            if (monto <= 0 || monto == '' || this.REPuntos.test(monto)) return true

            return false
        },
        porcentaje: {
            get: function () {
                if (this.nuevo.deduccion.monto == '' || this.nuevo.deduccion.monto == 0) return ''

                let res = this.cambiarFormatoMoneda(this.nuevo.deduccion.monto, 1) > 0 ? this.cambiarFormatoMoneda((this.cambiarFormatoMoneda(this.nuevo.deduccion.monto, 1) * 100) / this.cambiarFormatoMoneda(this.nuevo.monto, 1), 0) : 0;

                return res
            },
            set: function (value) {
                this.nuevo.deduccion.monto = this.cambiarFormatoMoneda((this.cambiarFormatoMoneda(value, 1) * this.cambiarFormatoMoneda(this.nuevo.monto, 1)) / 100, 0)

            }
        },
    },
});