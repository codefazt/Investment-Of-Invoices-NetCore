var app = new Vue({
    el: '#appEjecutivoCuentas',
    i18n,
    store: vuexLayout,
    vuetify: new Vuetify({
        lang: {
            t: (key, ...params) => i18n.t(key, params)
        }
    }),
    data: {
        modalLogout: { mostrar: false },
        listaCities: listaCities,
        listaProvincias: [],
        listaCiudades: [],
        envio: false,
        filter: [],
        widthTelefono: widthTelefono,

        listaTablas: [],
        tablaDeTurno: [],
        index: 0,
        id_filtro: 0,
        //Opciones de las Tablas
        listaLoading: [],
        listaBuscarClientes: [],
        listaTotalClientes: [],
        nTotalClientes: 0,
        currencies: {},
        listaTablasPrueba: [],

        dialogCambiarMonto: false,
        buscarClientes: true,
        totalClientes: 0,
        moment: moment,
        loading: true,
        listaOptions: [],
        options: {},
        DatosDetalles: null,
        modalCargandoDetalle: false,
        DatosResividos: [],
        listaCurrencies: null,
        limiteCliente: {
            person: null,
            currency: 0,
            available: null,
            abbreviation: null
        },
        tamanoTlf: tamanoTlf,
        // Comienzo de Datos para Menu Principal
        cerrarMordisco: true,
        drawer: false,
        detalleUsuario: false,
        // fin de Datos para Menu Principal
        mostrarMonto: null,
        formatoMonedaInput: formatoMonedaInput,
        formatoMoneda: formatoMoneda,
        cargando: true,
        abrirModal: false,
        resetTexto: true,
        x: null,
        errorTipoMoneda: '',
        errorTipoMonedaTexto: '',
        errorMontoRiesgo: '',
        errorMontoRiesgoTexto: '',
        MontoMaxRiesgo: '',
        headerCliente: [
            { text: i18n.t("headers.n"), value: "n", align: "center" },
            { text: i18n.t("headers.cliente"), value: 'company' },
            { text: i18n.t("headers.representanteLegal"), value: 'representante' },
            { text: i18n.t("headers.estado"), value: 'estado', align: 'center', },
            { text: i18n.t("headers.tipo"), value: 'finans', align: 'center', },
            { text: i18n.t("headers.moneda"), value: 'tipomoneda', align: 'center', },
            { text: i18n.t("headers.valorTotalLimite"), value: 'available', align: 'center', },
            { text: i18n.t("headers.montoSobrante"), value: 'usage', align: 'center', },
            { text: i18n.t("headers.opciones"), value: 'action', align: 'center' },
        ],
        hasSuccess: 'is-valid',
        hasError: 'is-invalid'

    },
    watch: {

        listaOptions: {
            async handler() {
                console.log("detect")
                if (this.listaTablas.length > 0) {
                    for (let i = 0; i < this.listaOptions.length; i++) {
                        if (this.listaBuscarClientes[i] && this.listaOptions[i].page * this.listaOptions[i].itemsPerPage >= this.listaTablas[i].length - this.listaOptions[i].itemsPerPage) {

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

        console.log(this.filter)
        console.log(this.currencies)
        //this.currencies.map(() => { this.listaOptions.push({}); this.listaBuscarClientes.push(true); this.listaLoading.push(false); this.listaTotalClientes.push(0) })
        document.getElementById('contenido').removeAttribute('hidden');
        this.cargando = false;
    },
    methods: {

        async llenar_consulta(i) {

            if (this.listaLoading[i]) return
            this.listaLoading[i] = true
            let take = 100;
            if (i == null) i = 0;
            this.listaProvincias = JSON.parse(document.getElementById('listadosInicialesJson').value);

            const data = await axios.post('EjecutivoCuentas?handler=LlenarTabla', { pagination: { take: take, skip: this.listaTotalClientes[i] }, filter: this.filter[i] }, { headers: { "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val() } })
                .then(function (response) {

                    console.log(response)
                    resetTime();
                    return response;
                });
            this.listaLoading[i] = false
            if (typeof data.data === 'string' || data.data instanceof String) {
                console.log(data);
                if (data.data.includes("<!DOCTYPE html>")) {
                    window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired";
                    toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br />" + i18n.t("errorBaseDatos"));
                    return;
                }
            }
            //this.DatosDetalles = JSON.parse(JSON.stringify(data.data));
            if (data != null) {
                if (data.data != null) {

                    if (data.data.prospecto != null) {

                        console.log("Prospecto")
                        console.log(data.data.prospecto)

                        let listaEstados = data.data.estados;
                        this.listaCurrencies = listaEstados.currencies;
                        let id_pais = data.data.idCurrency;

                        if (data.data.prospecto.list != null) {
                            if (data.data.prospecto.list.length == 0) {
                                this.loading = false
                                this.buscarClientes = false
                                this.listaBuscarClientes[i] = false
                            }

                            if (data.data.prospecto.list.length < take) this.listaBuscarClientes[i] = false
                            this.listaTotalClientes[i] += parseInt(data.data.prospecto.list.length)

                            this.tablaDeTurno = []
                            data.data.prospecto.list.map(data => {
                                if (data == null) return

                                //Componentes del Monto
                                let usage = 0;
                                let available = 0;
                                let currency = null;
                                let abbreviation = null;
                                let currency_id = 0;
                                let financiamiento = "CREDIT";

                                if (data.discriminator == 'PERSON') {
                                    data.company = data.firstName;
                                    representante = data.firstName;
                                }
                                if (data.contacts != null && data.discriminator == 'LEGAL') {
                                    for (let i = 0; i < data.contacts.length; i++) {
                                        if (data.contacts[i].label == 'LEGAL') {
                                            representante = data.contacts[i].name;
                                        }
                                    }
                                }
                                if (listaEstados != null) {
                                    for (let i = 0; i < listaEstados.regions.length; i++) {
                                        if (data.addresses != null) {
                                            if (data.addresses[0].region == listaEstados.regions[i].id) estado = listaEstados.regions[i].name;
                                        }
                                    }
                                }
                                if (data.quotas != null) {

                                    for (let i = 0; i < data.quotas.length; i++) {

                                        if ((data.quotas[i].abbreviation == "CREDIT" || data.quotas[i].abbreviation == "FINANCING") && data.quotas[i].currency == id_pais) {

                                            if (listaEstados.currencies != null) {
                                                for (let j = 0; j < listaEstados.currencies.length; j++) {
                                                    if (data.quotas[i].currency == listaEstados.currencies[j].id) {

                                                        if (data.quotas[i].usage < 1) {

                                                            data.quotas[i].available > 0 ? usage = data.quotas[i].available : usage = "0,00";

                                                        } else {
                                                            (data.quotas[i].available - data.quotas[i].usage) > 0 ? usage = (data.quotas[i].available - data.quotas[i].usage) : usage = "0,00";
                                                        }

                                                        //usage = data.quotas[i].usage;
                                                        available = data.quotas[i].available;
                                                        currency = listaEstados.currencies[j].symbol;
                                                        abbreviation = listaEstados.currencies[j].iso_4217;
                                                        currency_id = listaEstados.currencies[j].id;
                                                        financiamiento = data.quotas[i].abbreviation;
                                                    }
                                                }
                                            }
                                        }
                                        else {
                                            listaEstados.currencies.map(moneda => {

                                                if (moneda.id == id_pais) { currency = moneda.symbol; }
                                                if (moneda.id == id_pais) { abbreviation = moneda.iso_4217; }

                                            })
                                        }
                                    }
                                }
                                else {

                                    listaEstados.currencies.map(moneda => {

                                        if (moneda.id == id_pais) { currency = moneda.symbol; }
                                        if (moneda.id == id_pais) { abbreviation = moneda.iso_4217; }

                                    })

                                    usage = 0;
                                    available = 0;
                                    currency_id = 0;
                                    financiamiento = "CREDIT";
                                }
                                //Crear Tabla
                                this.tablaDeTurno.push({

                                    person_id: data.id,
                                    discriminator: data.discriminator,
                                    company: data.company,
                                    representante: representante,
                                    estado: estado,
                                    usage: usage,
                                    available: available,
                                    currency: currency,
                                    abbreviation: abbreviation,
                                    currency_id: currency_id,
                                    financiamiento: financiamiento

                                });
                            });

                            this.listaTablasPrueba.push(this.tablaDeTurno)
                            console.log(this.listaTablasPrueba)
                        }
                    } 

                }
            }
            this.loading = false;

        },
        async detallesProspecto(datos) {

            console.log(this.listaCurrencies)
            console.log(datos);
            this.resetModal();

            this.listaCurrencies.map(monedas => {

                if (monedas.symbol == datos.currency) this.limiteCliente.currency = monedas.id;
            })
            this.limiteCliente.person = datos.person_id;
            this.limiteCliente.abbreviation = datos.financiamiento;
            //this.limiteCliente.abbreviation = "CREDIT";

            this.modalCargandoDetalle = true;
            let person = { id: datos.person_id }
            const data = await axios.post('EjecutivoCuentas?handler=DetalleCliente', person, { headers: { "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val() } })
                .then(function (response) {
                    // handle success
                    resetTime();
                    if (response.data == null || response.data == "Error") {
                        window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                        return null;
                    }
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

            this.DatosDetalles = data;
            this.modalCargandoDetalle = false;
            this.detalleUsuario = true;
        },

        resetModal() {
            this.errorTipoMoneda = null;
            this.errorTipoMonedaTexto = null;
            this.limiteCliente.available = null;
            this.errorMontoRiesgo = null;
            this.errorMontoRiesgoTexto = null;
            this.limiteCliente.currency = 0;
        },
        validarTipoMoneda() {

            if (this.limiteCliente.currency == null ||
                this.limiteCliente.currency == 0 ||
                this.limiteCliente.currency == '') {

                this.errorTipoMoneda = this.hasError;
                this.errorTipoMonedaTexto = 'Seleccione un tipo de moneda';

            } else {
                this.errorTipoMoneda = this.hasSuccess;
                this.errorTipoMonedaTexto = '';
            }

        },
        validarMontoRiesgo() {

            if (this.limiteCliente.available == null || this.limiteCliente.available == "" || this.limiteCliente.available[0] == "-" ||
                this.limiteCliente.available[0] == " " || this.limiteCliente.available[0] == 0 || this.limiteCliente.available < 1) {

                this.errorMontoRiesgo = this.hasError;
                this.errorMontoRiesgoTexto = i18n.t("valid.limiteMin");

            } else {
                this.errorMontoRiesgo = this.hasSuccess;
                this.errorMontoRiesgoTexto = '';

                this.limiteCliente.available = formatoMonedaInput(formatoMoneda(this.limiteCliente.available,'es'), 'es', 2);
                if (this.limiteCliente.available == null) {
                    this.limiteCliente.available = 1;
                }
            }
        },
        habilitarModal() {

            if (this.limiteCliente.available == null || this.limiteCliente.available == "" || this.limiteCliente.available[0] == "-" ||
                this.limiteCliente.available[0] == " " || this.limiteCliente.available[0] == 0 || this.limiteCliente.available < 1) {

                this.errorMontoRiesgo = this.hasError;
                this.errorMontoRiesgoTexto = i18n.t("valid.limiteMin");
            }
            else this.dialogCambiarMonto = true;
        },
        montoFancy(num) {

            var formatNumber = {
                separador: ".", // separador para los miles
                sepDecimal: ',', // separador para los decimales
                formatear: function (num) {
                    num += '';
                    var splitStr = num.split('.');
                    var splitLeft = splitStr[0];
                    var splitRight = splitStr.length > 1 ? this.sepDecimal + splitStr[1] : '';
                    var regx = /(\d+)(\d{3})/;
                    while (regx.test(splitLeft)) {
                        splitLeft = splitLeft.replace(regx, '$1' + this.separador + '$2');
                    }
                    return this.simbol + splitLeft + splitRight;
                },
                new: function (num, simbol) {
                    this.simbol = simbol || '';
                    return this.formatear(num);
                }
            }

            return formatNumber.new(num);
        },
        primeraMayus(string) {
        return string.charAt(0).toUpperCase() + string.slice(1);
        },
        async enviarDatos(montoLimite) {

            this.totalClientes = 0;
            this.limiteCliente.available = formatoMoneda(this.limiteCliente.available, 'es');

            const data = await axios.post('EjecutivoCuentas?handler=AsignarLimite', this.limiteCliente, { headers: { "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val() } })
                .then(function (response) {
                    resetTime();
                    if (response.data == null || response.data == "Error") {
                        window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                        return null;
                    }
                    return response;

                }).catch((err) => {
                    toastr.error("Ocurrio algun Error en la base de Datos vuelva a intentar mas tarde")
                    return err;
                });

            if (typeof data.data === 'string' || data.data instanceof String) {
                console.log(data);
                if (data.data.includes("<!DOCTYPE html>")) {
                    window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired";
                    toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br />" + i18n.t("errorBaseDatos"));
                    return;
                }
            }

            if (data.data.includes("success:") == true) {

                this.listaTablasPrueba = [];
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
                
                toastr.success(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br />" + i18n.t("asignarLimiteRiesgo"));

            } else {
                if (data.data == "quota allocation error") toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br /> error de asignación de cuota.");
                else if (data.data == "Debtor not account with entity") toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br />" + i18n.t("tooltip.limiteCreditoSinCuentaBancaria"));
                else if (data.data == "You are not authorised to perform this action") toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br />" + i18n.t("tooltip.noAuthorizado"));
                else if (data.data == "Debtor not account with currency") toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br />" + i18n.t("tooltip.limiteCreditoSinCuentaBancaria"));
                else toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br /> Ocurrio un error en la Conexión. Por favor recargué la página.");
            }
            this.detalleUsuario = false;
        },

    },
    computed: {
        habilitarBotonAsignar() {

            if (this.errorMontoRiesgo == this.hasSuccess) {
                return false;
            } else {
                return true;
            }
            
        },
        mensajesComputed() {
            return vuexLayout.state.mensajes
        },
    },
    mounted: async function () {
        tiempoLogin(this.modalLogout)

        for (var i = 0; i < this.currencies.length; i++) {
            await this.llenar_consulta(i)
        }

    }
});
