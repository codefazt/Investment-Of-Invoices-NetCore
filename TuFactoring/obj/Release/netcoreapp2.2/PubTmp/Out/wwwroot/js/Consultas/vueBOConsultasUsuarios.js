var app = new Vue({
    el: '#appConsultaUsuariosBO',
    store: vuexLayout,
    i18n,
    vuetify: new Vuetify({
        lang: {
            t: (key, ...params) => i18n.t(key, params)
        }
    }),
    data: {
        modalLogout: { mostrar: false },
        listaCities: listaCities,
        backEndDateFormat: backEndDateFormat,
        formatoMonedaInput: formatoMonedaInput,
        modalCargandoDetalle: false,
        filter: null,
        cargando: true,
        abrirModal: false,
        x: null,
        buscarClientes: true,
        headerConsulta: [
            { text: i18n.t("headers.n"), value: "n", align: "center" },
            { text: i18n.t("headers.cliente"), value: 'name' },
            { text: i18n.t("headers.doc"), value: 'documents[0].number', align: 'center' },
            { text: i18n.t("headers.correoElectronico"), value: 'emails[0].address', align: 'center' },
            { text: i18n.t("headers.estatus"), value: 'status', align: 'center' },
            { text: i18n.t("headers.opciones"), value: 'action', align: 'center' },
        ],
        hasSuccess: 'is-valid',
        hasError: 'is-invalid',
        errorProveedor: false,
        moment: moment,
        loading: false,
        options: {},
        envio: false,
        totalFacturas: 0,
        DatosDetalles: null,
        clientes: [],
        listaProvincias: [],
        listaCiudades: [],
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

        async llenar_consulta(cliente) {

            if (this.loading) return
            this.loading = true
            let take = 100;

            let Country = JSON.parse(document.getElementById('listadosInicialesJson').value);
            this.listaProvincias = JSON.parse(document.getElementById('listadosInicialesJson').value);
            //console.log(Country);

            const data = await axios.post('ConsultaUsuarios?handler=LlenarConsulta', { pagination: { take: take, skip: this.totalFacturas }, filter: this.filter }, { headers: { "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val() } })
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

            if (data.data == null) {
                toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br />" + i18n.t("errorBaseDatos"));
                return
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

                    if (data.documents != null) {
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
                }
                console.log(data)
                this.clientes.push(data)
            })
            this.loading = false;
        },
        async detalles_cliente(cliente) {
            
            peopleUser = JSON.parse(JSON.stringify(cliente));
            this.modalCargandoDetalle = true;
            const data = await axios.post('ConsultaUsuarios?handler=Detalle', cliente, { headers: { "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val() } })
                .then(function (response) {
                    // handle success
                    resetTime();
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
            this.abrirModal = true;

        },
        async CiudadSelected(State_id) {

            let Region = {
                State: State_id
            }

            var data = await axios.post('ConsultaUsuarios?handler=SelectCity', Region, { headers: { "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val() } })
                .then((respond) => {
                    resetTime();
                    return respond.data;

                }).catch(function (error) {

                    console.log("entro en el catch: ");
                    console.log(error);
                });

            if (data != null) {
                this.listCities = data;
                this.listCities.sort((a, b) => a.name < b.name ? -1 : + (a.name > b.name));
            }

        },

    },
    computed: {

    },
    mounted: async function () {
        await this.llenar_consulta();
        tiempoLogin(this.modalLogout);
    },
});