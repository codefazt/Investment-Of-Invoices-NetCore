var app = new Vue({
    el: '#appConsultasFinanciamiento',
    store: vuexLayout,
    i18n,
    vuetify: new Vuetify({
        lang: {
            t: (key, ...params) => i18n.t(key, params)
        }
    }),
    data: {
        filtersIsEmpty: filtersIsEmpty,
        filterIsEmpty: filterIsEmpty,
        arrayCondition: arrayCondition,
        lang: "es",
        symbol: "$",
        iso_4217: "",
        digits: 2,
        montoTotalPagar: 0,
        montoTotalFinanciado: 0,
        facturas: [],
        buscarFacturas: true,
        loading: false,
        loading2: false,
        options2: {},
        totalFacturasD: 0,
        lastID: null,

        modalLogout: { mostrar: false },
        backEndDateFormat: backEndDateFormat,
        formatoMonedaInput: formatoMonedaInput,
        modalCargandoDetalle: false,
        filter: [],
        cargando: true,
        abrirModal: false,
        x: null,
        facturasJson: [],
        buscarClientes: true,
        DatosDetalles: null,
        headerConsulta: [
            { text: i18n.t("headers.n"), value: "n", align: "center" },
            { text: i18n.t("headers.cliente"), value: 'name', sortable: true },
            { text: i18n.t("headers.tipo"), value: 'finans', align: 'center', sortable: true },
            { text: i18n.t("headers.moneda"), value: 'tipomoneda', align: 'center', sortable: true },
            { text: i18n.t("headers.montoAsignado"), value: 'available', align: 'center', sortable: true },
            { text: i18n.t("headers.montoUtilizado"), value: 'usage', align: 'center', sortable: true },
            { text: i18n.t("headers.financiedAmountBank"), value: 'financied', align: 'center', sortable: true },
            { text: i18n.t("headers.details"), value: 'action', align: 'center', sortable: false },
        ],
        headerConsultaC: [
            { text: i18n.t("headers.proveedor"), value: 'supplier.name', sortable: false },
            { text: i18n.t("headers.numeroFactura"), value: 'number', align: 'center', sortable: false },
            { text: i18n.t("headers.fechaVencimiento"), value: 'expiration_date', align: 'center', sortable: false },            
            { text: i18n.t("headers.originalAmount"), value: 'original_amount', align: 'center', sortable: false },
            { text: i18n.t("headers.monto"), value: 'amount', align: 'center', sortable: false },
            { text: i18n.t("headers.financiamiento"), value: 'request_financing', align: 'center', sortable: false },
        ],
        hasSuccess: 'is-valid',
        hasError: 'is-invalid',
        errorProveedor: false,
        moment: moment,
        totalNeto: 0,
        options: {},
        envio: false,
        totalFacturas: 0,
        clientes: [],
        prefixRepresentante: null,
        prefixContact: null,
        filterData: [],
        participant: null,
        nameEstado: "",
        invoices: [],
        nameCiudad: "",
        arrayClientes: [],

        //Opciones de las Tablas
        listaOptions: [],
        listaLoading: [],
        listaBuscarClientes: [],
        listaTotalClientes: [],
        nTotalClientes: 0,
        currencies: {},
    },
    watch: {
        options: {
            async handler() {
                if (this.buscarClientes && this.options.page * this.options.itemsPerPage >= this.clientes.length - this.options.itemsPerPage) {
                    await this.llenar_consulta();
                }
            },
            deep: true,
        },
        listaOptions: {
            async handler() {
                console.log("detect")
                if (this.arrayClientes.length > 0) {
                    for (let i = 0; i < this.listaOptions.length; i++) {
                        if (this.arrayClientes[i] != null) {
                            if (this.listaBuscarClientes[i] && this.listaOptions[i].page * this.listaOptions[i].itemsPerPage >= this.arrayClientes[i].length - this.listaOptions[i].itemsPerPage) {

                                var aux = this.filter[i].currency

                                this.filter[i].currency = this.currencies[i].id
                                await this.llenar_consulta(i);

                                this.filter[i].currency = aux

                            }
                        }
                    }
                }
            },
            deep: true,
        },
        options2: {
            async handler() {
                if (this.buscarFacturas && this.options2.page * this.options2.itemsPerPage >= this.facturas.length - this.options2.itemsPerPage) {
                    await this.detalles_cliente();
                }
            },
            deep: true,
        },
    },
    created: function () {

        try {
            this.currencies = JSON.parse(document.getElementById('currenciesData').value);
            //this.filter = JSON.parse(document.getElementById('filterData').value);
            

            for (var i = 0; i < this.currencies.length; i++) {
                this.listaOptions.push({});
                this.listaBuscarClientes.push(true);
                this.listaLoading.push(false);
                this.listaTotalClientes.push(0)
                this.filter[i] = JSON.parse(document.getElementById('filterData+' + i).value);
                this.filter[i].currency = this.currencies[i].id;
                document.getElementById("eliminarData").removeChild(document.getElementById('filterData+' + i));
            }
            document.getElementById("eliminarData").removeChild(document.getElementById('currenciesData'));

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
        this.lang = document.getElementsByTagName("html")[0].getAttribute("lang")
        document.getElementById('contenido').removeAttribute('hidden');
        this.cargando = false;

    },
    methods: {
        montoTotalInvoices: function (arreglo) {
            let acumulador = 0
            for (let x = 0; x < arreglo.length; x++) {
                acumulador = acumulador + arreglo[x].usage
            }
            return acumulador
        },
        montoTotalInvoicesFinancieds: function (arreglo) {
            let acumulador = 0
            for (let x = 0; x < arreglo.length; x++) {
                acumulador = acumulador + arreglo[x].sumAccountantsInvoices
            }
            return acumulador
        },
        estructura_del_detalle(data, idents) {

            idents = JSON.parse(idents)
            this.datos_adicionales_cliente.representante = {};
            this.datos_adicionales_cliente.contacts = {};
            this.datos_adicionales_cliente.accounts = {};
            this.datos_adicionales_cliente.addresses = {};
            this.datos_adicionales_cliente.phone = {};
            this.datos_adicionales_cliente.confirmings = null;

            for (let i = 0; i < data.contacts.length; i++) {

                if (data.contacts[i].category == "LEGAL") {

                    for (let j = 0; j < idents.identifications.length; j++) {

                        if (idents.identifications[j].id == data.contacts[i].identificationId) {

                            if (idents.identifications[j].prefix == false) {
                                this.prefixRepresentante = idents.identifications[j].abbreviation
                            }
                        }
                    }

                    this.datos_adicionales_cliente.representante = data.contacts[i];

                } else {
                    for (let j = 0; j < idents.identifications.length; j++) {

                        if (idents.identifications[j].id == data.contacts[i].identificationId) {

                            if (idents.identifications[j].prefix == false) {
                                this.prefixContact = idents.identifications[j].abbreviation
                            }
                        }
                    }
                    this.datos_adicionales_cliente.contacts = data.contacts[i];
                }

            }

            for (let i = 0; i < data.addresses.length; i++) this.datos_adicionales_cliente.addresses = data.addresses[i];
            for (let i = 0; i < data.accounts.length; i++) this.datos_adicionales_cliente.accounts = data.accounts[i];
            for (let i = 0; i < data.phone.length; i++) this.datos_adicionales_cliente.phone = data.phones[i];
            for (let i = 0; i < data.confirmings.length; i++) this.datos_adicionales_cliente.confirmings = data.confirmings[i];

            for (let i = 0; i < idents.subdivisions.length; i++) {

                for (let j = 0; j < idents.subdivisions[i].cities.length; j++) {

                    if (idents.subdivisions[i].cities[j].id == this.datos_adicionales_cliente.addresses.cityId) {
                        this.nameCiudad = idents.subdivisions[i].cities[j].name;
                        this.nameEstado = idents.subdivisions[i].name;
                        continue
                    }

                }
            }
            this.modalCargandoDetalle = false;
            this.abrirModal = true;
        },

        async llenar_consulta(i) {
            //JSON.parse(JSON.stringify(registro));

            if (this.listaLoading[i]) return
            this.loading = true;
            this.listaLoading[i] = true
            let take = 100;
            if (i == null) i = 0;
            let Country = JSON.parse(document.getElementById('listadosInicialesJson').value);
            const data = await axios.post('?handler=LlenarConsulta', { pagination: { take: take, skip: this.totalFacturas[i] }, filter: this.filter[i] }, { headers: { "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val() } })
                .then(function (response) {
                    // handle success
                    resetTime();

                    if (response != null) {
                        if (response.data <= 0) response.data = null;
                    }

                    console.log(response.data)
                    return response.data;

                }).catch(function (error) {

                    window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired";
                    toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br />" + i18n.t("errorBaseDatos"));
                });

            if (typeof data.lista === 'string' || data.lista instanceof String) {
                con
                if (data.includes("<!DOCTYPE html>")) {
                    window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired";
                    toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br />" + i18n.t("errorBaseDatos"));
                    return;
                }
            }

            if (data == null || data == "Error") {
                window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                return null;
            }

            let listaEstados = data.estado;
            this.listaLoading[i] = false
            if (data.lista != null) {

                if (data.lista.list != null) {

                    if (data.lista.list.length == 0) {
                        this.loading = false
                        this.buscarClientes = false
                        this.listaBuscarClientes[i] = false
                    }

                    if (data.lista.list.length < take) this.listaBuscarClientes[i] = false
                    this.listaTotalClientes[i] += parseInt(data.lista.list.length)

                    if (data.lista.list[0] != null) {
                        if (data.lista.list[0].error != null && data.lista.list[0].error != "") {
                            this.loading = false
                            this.buscarClientes = false

                            if (data.lista.list[0].error == "You are not authorised to perform this action") {
                                toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br />" + i18n.t("tooltip.noAuthorizado"));
                                window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                            }
                            else if (data.lista.list[0].error == "internal system error") toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br />" + i18n.t("tooltip.internalError"));
                            else toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br />" + data.lista.list[0].error);
                        }
                    }

                    var currencyDeTurno = listaEstados.currencies[i];
                    this.clientes = [];
                    data.lista.list.map(subElemnt => {
                        if (subElemnt == null) return
                        let usage = 0;
                        let available = 0;
                        let currency = null;
                        let abbreviation = null;
                        let currency_id = 0;
                        let financiamiento = "CREDIT";
                        let sumAccountantsInvoices = 0;

                        data.listaFinanciedInvoices.map(accountants => {

                            accountants.list.map(subAccountants => {

                                if (currencyDeTurno.id == accountants.currency && subElemnt.id == subAccountants.peopleID) {
                                    sumAccountantsInvoices = subAccountants.sum;
                                }

                            });

                        });
                        if (subElemnt.discriminator == "PERSON") subElemnt.name = subElemnt.name;
                        else {
                            if (subElemnt.contacts != null) {
                                for (let i = 0; i < subElemnt.contacts.length; i++) {
                                    if (subElemnt.contacts[i].label == "LEGAL") subElemnt.emails = [{ address: subElemnt.contacts[i].email }];
                                }
                            }
                        }
                        //Recorre identifications
                        for (let i = 0; i < Country.identifications.length; i++) {

                            if (Country.identifications[i].id == subElemnt.documents[0].identification) {

                                if (Country.identifications[i].prefix == false && subElemnt.documents != null) subElemnt.documents[0].number = Country.identifications[i].abbreviation + '-' + subElemnt.documents[0].number;
                                else {
                                    if (subElemnt.documents != null) {
                                        for (let j = 0; j < Country.identifications[i].prefixes.length; j++) {

                                            if (Country.identifications[i].prefixes[j].id == subElemnt.documents[0].prefix)
                                                subElemnt.documents[0].number = Country.identifications[i].prefixes[j].abbreviation + '-' + subElemnt.documents[0].number;
                                        }
                                    }
                                }
                            }
                        }

                        //Define quotas
                        if (subElemnt.quotas != null) {

                            for (let i = 0; i < subElemnt.quotas.length; i++) {

                                if (subElemnt.quotas[i].abbreviation == "CREDIT" || subElemnt.quotas[i].abbreviation == "FINANCING") {

                                    if (listaEstados.currencies != null) {
                                        for (let j = 0; j < listaEstados.currencies.length; j++) {
                                            if (subElemnt.quotas[i].currency == listaEstados.currencies[j].id) {

                                                if (subElemnt.quotas[i].usage < 1) {

                                                    subElemnt.quotas[i].available > 0 ? usage = subElemnt.quotas[i].usage : usage = 0;

                                                } else {
                                                    (subElemnt.quotas[i].available - subElemnt.quotas[i].usage) > 0 ? usage = subElemnt.quotas[i].usage : usage = 0;
                                                }

                                                //usage = data.quotas[i].usage;
                                                available = subElemnt.quotas[i].available;
                                                currency = listaEstados.currencies[j].symbol;
                                                abbreviation = listaEstados.currencies[j].iso_4217;
                                                currency_id = listaEstados.currencies[j].id;
                                                financiamiento = subElemnt.quotas[i].abbreviation;
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        else {
                            for (let j = 0; j < listaEstados.currencies.length; j++) {

                                currency = listaEstados.currencies[j].symbol;
                                abbreviation = listaEstados.currencies[j].iso_4217;
                            }
                            this.totalNeto = 0;
                            usage = 0;
                            available = 0;
                            currency_id = 0;
                            financiamiento = "CREDIT";
                        }
                        this.clientes.push({

                            person_id: subElemnt.id,
                            discriminator: subElemnt.discriminator,
                            name: subElemnt.name,
                            number: subElemnt.documents[0].number,
                            usage: usage,
                            available: available,
                            currency: currency,
                            abbreviation: abbreviation,
                            currency_id: currency_id,
                            financiamiento: financiamiento,
                            sumAccountantsInvoices: sumAccountantsInvoices
                        });

                    });
                    this.arrayClientes.push(this.clientes);
                    console.log(this.arrayClientes)
                }

            }
     
            this.loading = false;
        },
        async detalles_cliente(cliente, index) {
            if (this.loading2) return
            this.loading2 = true
            let take = 100;
            let peopleUser = null;
            let expiration_from = null;
            let expiration_to = null;

            if (cliente != null) {
                peopleUser = JSON.parse(JSON.stringify(cliente));
                this.montoTotalPagar = 0;
                this.montoTotalFinanciado = 0;
                this.lastID = peopleUser.person_id;
                this.totalFacturasD = 0;
                this.facturas = [];
            }


            if (this.filter != null) {
                if (this.filter[index] != null) {

                    expiration_to = this.filter[index].expiration_to;
                    expiration_from = this.filter[index].expiration_from;
                }
            }
            let filtrado = {
                debtorId: peopleUser.person_id,
                currency: peopleUser.currency_id,
                expiration_to: expiration_to,
                expiration_from: expiration_from
            }

            this.modalCargandoDetalle = true;
            const data = await axios.post('?handler=Detalle', { pagination: { take: take, skip: this.totalFacturasD }, filter: filtrado }, { headers: { "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val() } })
                .then(function (response) {
                    // handle success
                    resetTime();
                    if (response == null || response.data == null || response.data == "Error") {
                        window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                        return null;
                    }
                    return response.data;
                });

            if (typeof data === 'string' || data instanceof String) {
                if (data.includes("<!DOCTYPE html>")) {
                    window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired";
                    toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br />" + i18n.t("errorBaseDatos"));
                    return;
                }
            }

            if (data.length < (take / 2)) this.buscarFacturas = false
            this.totalFacturasD += parseInt(data.length)
            data.map(data => {
                if (data == null) return
                this.symbol = data.currency.symbol
                this.iso_4217 = data.currency.iso_4217
                if (data.term_days > 0) {
                    this.facturas.push(data)
                    this.montoTotalPagar += data.amount
                    if (data.request_financing) {
                        this.montoTotalFinanciado += data.amount
                    }
                }

            })

            if (this.montoTotalPagar == null || this.montoTotalPagar == "") {
                this.montoTotalPagar = 0
            }

            if (this.montoTotalFinanciado == null || this.montoTotalFinanciado == "") {
                this.montoTotalFinanciado = 0
            }
            this.modalCargandoDetalle = false;
            this.abrirModal = true;
            this.loading2 = false;

        },
    },
    computed: {

    },
    mounted: async function () {
        tiempoLogin(this.modalLogout)
        for (var i = 0; i < this.currencies.length; i++) {
            await this.llenar_consulta(i)
        }
        setTimeout(() => iniciarButtonFilters(), 500)
    },
});