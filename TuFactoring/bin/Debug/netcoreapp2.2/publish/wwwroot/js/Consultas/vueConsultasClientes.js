var app = new Vue({
    el: '#appConsultasClientes',
    store: vuexLayout,
    i18n,
    vuetify: new Vuetify({
        lang: {
            t: (key, ...params) => i18n.t(key, params)
        }
    }),
    data: {
        modalLogout: { mostrar: false },
        backEndDateFormat: backEndDateFormat,
        formatoMonedaInput: formatoMonedaInput,
        modalCargandoDetalle: false,
        filter: null,
        cargando: true,
        abrirModal: false,
        x: null,
        buscarClientes: true,
        DatosDetalles: null,
        headerConsulta: [
            { text: i18n.t("headers.n"), value: "n", align: "center" },
            { text: i18n.t("headers.cliente"), value: 'name', sortable: false },
            { text: i18n.t("headers.doc"), value: 'documents[0].number', align: 'center', sortable: false },
            { text: i18n.t("headers.correoElectronico"), value: 'emails[0].address', align: 'center', sortable: false },
            //{ text: i18n.t("headers.limiteCredito"), value: 'confirmings', align: 'center', sortable: false },
            { text: i18n.t("headers.opciones"), value: 'action', align: 'center', sortable: false },
        ],
        hasSuccess: 'is-valid',
        hasError: 'is-invalid',
        errorProveedor: false,
        moment: moment,
        loading: false,
        options: {},
        envio: false,
        totalFacturas: 0,

        clientes: [],
        prefixRepresentante: null,
        prefixContact: null,
        filterData: [],
        participant: null,
        nameEstado: "",
        nameCiudad: ""
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
    },
    created: function () {

        try {
            this.filter = JSON.parse(document.getElementById('filterData').value);
            document.getElementById("eliminarData").removeChild(document.getElementById('filterData'));
        } catch (e) {
            this.filter = null
        }
        document.getElementById('contenido').removeAttribute('hidden');
        this.cargando = false;

    },
    methods: {

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

        async llenar_consulta(cliente) {

            if (this.loading) return
            this.loading = true
            let take = 100;
            let Country = JSON.parse(document.getElementById('listadosInicialesJson').value);
            const data = await axios.post('ConsultaClientes?handler=LlenarConsulta', { pagination: { take: take, skip: this.totalFacturas }, filter: this.filter }, { headers: { "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val() } })
                .then(function (response) {
                    // handle success
                    resetTime();
                    return response;
                });

            if (typeof data.data === 'string' || data.data instanceof String) {
                console.log(data);
                if (data.data.includes("<!DOCTYPE html>")) {
                    window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired";
                    toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br />" + i18n.t("errorBaseDatos"));
                    return;
                }
            }
            
            if (data.data == null || data.data == "Error") {
                window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                return null;
            }

            if (data.data.length == 0) {
                this.loading = false
                this.buscarClientes = false
            }

            if (data.data.length < take) this.buscarClientes = false
            this.totalFacturas += parseInt(data.data.length)

            data.data.map(data => {
                if (data == null) return

                if (data.discriminator == "PERSON") data.name = data.name;
                else {
                    if (data.contacts != null) {
                        for (let i = 0; i < data.contacts.length; i++) {
                            if (data.contacts[i].label == "LEGAL") data.emails = [{ address: data.contacts[i].email }];
                        }
                    }

                }

                for (let i = 0; i < Country.identifications.length; i++) {

                    if (Country.identifications[i].id == data.documents[0].identification) {

                        if (Country.identifications[i].prefix == false && data.documents != null) data.documents[0].number = Country.identifications[i].abbreviation + '-' + data.documents[0].number;
                        else {
                            if (data.documents != null) {
                                for (let j = 0; j < Country.identifications[i].prefixes.length; j++) {

                                    if (Country.identifications[i].prefixes[j].id == data.documents[0].prefix)
                                        data.documents[0].number = Country.identifications[i].prefixes[j].abbreviation + '-' + data.documents[0].number;
                                }
                            }
                        }
                    }
                }
                this.clientes.push(data)
            })

            this.loading = false;
        },
        async detalles_cliente(cliente) {

            peopleUser = JSON.parse(JSON.stringify(cliente));
            this.modalCargandoDetalle = true;
            const data = await axios.post('ConsultaClientes?handler=Detalle', cliente, { headers: { "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val() } })
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
                console.log(data);
                if (data.includes("<!DOCTYPE html>")) {
                    window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired";
                    toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br />" + i18n.t("errorBaseDatos"));
                    return;
                }
            }

            console.log(data)
            this.DatosDetalles = data;
            this.modalCargandoDetalle = false;
            this.abrirModal = true;
            //this.estructura_del_detalle(data.data.detallesClientes, data.data.indens);

        },

    },
    computed: {

    },
    mounted: async function () {
        tiempoLogin(this.modalLogout)
        await this.llenar_consulta()
    },
});