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
        modalLogout: { mostrar: false },
        widthTelefono: widthTelefono,
        lang: "es",
        symbol: "$",
        iso_4217: "",
        digits: 2,
        backEndDateFormat: backEndDateFormat,
        formatoMonedaInput: formatoMonedaInput,
        cargando: true,
        buscarFacturas: true,
        headerInvoices: [
            { text: i18n.t("headers.numeroFactura"), value: 'invoice.number', align: 'center', sortable: false },
            { text: i18n.t("headers.cliente"), value: 'debtor', align: 'center', sortable: false },
            { text: i18n.t("headers.bank"), value: 'entity', align: 'center', sortable: false },
            { text: i18n.t("headers.fechaVencimiento"), value: 'expiration_date', align: 'center', sortable: false },
            { text: i18n.t("headers.monto"), value: 'amount', align: 'center', sortable: false },
            { text: i18n.t("headers.opciones"), value: 'options', align: 'center', sortable: false },
        ],
        moment: moment,
        loading: false,
        options: {},
        envio: false,
        totalFacturas: 0,
        detailMostrar: false,
        facturas:[],
        data: null,
        detail: {
            invoice: { id: "" }
        },
        cargando:true,
        filter: {},
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
        
        this.lang = langActual
        this.digits = digitsActual

        document.getElementById("contenido").removeAttribute("hidden")
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
                case "posted": estado_actual = i18n.t("POSTULATED_INVOICE"); break;
                case "confirmed": estado_actual = i18n.t("CONFIRMED_INVOICE");; break;
                case "offered": estado_actual = i18n.t("OFFERED_INVOICE"); break;
                case "published": estado_actual = i18n.t("PUBLISHED_INVOICE"); break;
                case "released": estado_actual = i18n.t("RELEASED_INVOICE"); break;
                case "sold": estado_actual = i18n.t("SOLD_INVOICE"); break;
                case "postponed": estado_actual = i18n.t("POSTPONED_INVOICE"); break;
                case "overdue": estado_actual = i18n.t("EXPIRED_INVOICE"); break;
                case "paid": estado_actual = i18n.t("PAID"); break;
                case "finalize": estado_actual = i18n.t("FINALIZE_INVOICE"); break;
                case "processing": estado_actual = i18n.t("PROCESSING_INVOICE"); break;
            }

            return estado_actual;
        },

        async llenar_consulta(factura) {

            if (this.loading) return
            this.loading = true
            let take = 100;
            const data = await axios.post('?handler=LlenarConsulta', { pagination: { take: take, skip: this.totalFacturas }, filter: this.filter }, { headers: { "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val() } })
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
                if (data == null) return

                this.facturas.push(data)
            })
            

            this.loading = false;
            return this.buscarFacturas
        },

        async detalles_facturas(factura) {
            if (this.loading) return
            this.loading = true

            const data = await axios.post('?handler=Detalle', factura, { headers: { "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val() } })
                .then(function (response) {
                    resetTime()
                    return response;
                });

            this.loading = false

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

            this.detail = factura
            this.data = data.data
            this.detailMostrar = true  
        },

    },
});