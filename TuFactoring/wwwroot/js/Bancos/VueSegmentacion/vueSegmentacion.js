// JavaScript con Vue Segmentacion

var app = new Vue({
    el: '#appSegmentacion',
    i18n,
    store: vuexLayout,
    vuetify: new Vuetify({
        lang: {
            t: (key, ...params) => i18n.t(key, params)
        }
    }),
    data: {
        dialogAyuda: false,
        bloqueoBoton: false,
        modalLogout: { mostrar: false },
        listaCities: listaCities,
        listaProvincias: [],
        listaCiudades: [],
        envio: false,
        filter: null,
        widthTelefono: widthTelefono,

        buscarClientes: true,
        totalClientes: 0,
        moment: moment,
        loading: false,
        options: {},
        DatosDetalles: null,
        modalCargandoDetalle: false,


        tamanoTlf: tamanoTlf,
        // Comienzo de Datos para Menu Principal
        cerrarMordisco: true,
        fav: true,
        menu: false,
        message: false,
        hints: true,
        Mensajeria: {
            notificaciones: true,
            mensajes: true,
        },
        drawer: false,
        detalleUsuario: false,
        // fin de Datos para Menu Principal
        headerCliente: [
            { text: i18n.t("headers.n"), value: "n", align: "center" },
            { text: i18n.t("headers.cliente"), value: 'company' },
            { text: i18n.t("headers.representanteLegal"), value: 'representante' },
            { text: i18n.t("headers.estado"), value: 'estado', align: 'center', },
            { text: i18n.t("headers.categoria"), value: 'category', align: 'center', },
            { text: i18n.t("headers.opciones"), value: 'action', align: 'center' },
        ],

        cargando: true,
        abrirModal: false,
        resetTexto: true,
        x: null,
        DatosResividos: [],
        EjecutivoSeleccionado: [],
        ejecutivoAsignado: true,
        segmentar: {        // nombre de la Variable y propiedades provicionales
            country: 0,
            confirmant: null,
            user: null
        },
        listaEjecutivos: null,
        errorEjecutivo: '',
        hasSuccess: 'is-valid',
        hasError: 'is-invalid',
        EjecutivoUsed: null,

    },
    watch: {
        options: {
            async handler() {
                if (this.buscarClientes && this.options.page * this.options.itemsPerPage >= this.DatosResividos.length - this.options.itemsPerPage) {
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

        async llenar_consulta() {

            if (this.loading) return
            this.loading = true
            let take = 100;
            this.listaProvincias = JSON.parse(document.getElementById('listadosInicialesJson').value);

            const data = await axios.post('Segmentacion?handler=LlenarTabla', { pagination: { take: take, skip: this.totalClientes }, filter: this.filter }, { headers: { "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val() } })
                .then(function (response) {

                    resetTime();
                    if (response.data.prospecto == null) {
                        window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                        return
                    }
                    return response;
                });

            //this.DatosDetalles = JSON.parse(JSON.stringify(data.data));
            if (data != null) {
                if (data.data != null) {

                    if (data.data.prospecto != null) {

                        if (data.data.prospecto.error == "You are not authorised to perform this action") {
                            toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br />" + i18n.t("tooltip.noAuthorizado"));
                            window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                        }

                        if (data.data.prospecto.list.length == 0) {
                            this.loading = false
                            this.buscarClientes = false
                        }

                        if (data.data.prospecto.list.length < take) this.buscarClientes = false
                        this.totalClientes += parseInt(data.data.prospecto.list.length)

                        let identities = null;
                        let representante = '-';
                        let category = '-';
                        let estado = '-';
                        let EjecutivoCuenta = null;//'Ejecutivo Asignado';
                        let IdEjecutivoCuenta = null;//'Ejecutivo Asignado';
                        let listaEstados = data.data.estados;
                        this.listaEjecutivos = data.data.listaejecutivos;
                        data.data.prospecto.list.map(data => {
                            if (data == null) return

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
                            if (data.executives != null) {
                                for (let i = 0; i < this.listaEjecutivos.length; i++) {
                                    if (data.executives[0].id == this.listaEjecutivos[i].id) {
                                        EjecutivoCuenta = this.listaEjecutivos[i].name;
                                        IdEjecutivoCuenta = this.listaEjecutivos[i].id;
                                    }
                                }
                            } else {
                                EjecutivoCuenta = null;
                                IdEjecutivoCuenta = null;
                            }

                            if (listaEstados != null) {
                                for (let i = 0; i < listaEstados.regions.length; i++) {
                                    if (data.addresses != null) {
                                        if (data.addresses[0].region == listaEstados.regions[i].id) estado = listaEstados.regions[i].name;
                                    }

                                }
                            }
                            if (data.identities != null) {

                                identities = { ...data.identities };
                                switch (data.identities[0].participant) {
                                    case 'DEBTOR': category = 'EMPRESA'; break;
                                    case 'FACTOR': category = 'INVERSIONISTA'; break;
                                    case 'SUPPLIER': category = 'PROVEEDOR'; break;
                                    default: '-';
                                }

                            }
                            //Crear Tabla
                            this.DatosResividos.push({

                                person_id: data.id,
                                discriminator: data.discriminator,
                                company: data.company,
                                representante: representante,
                                estado: estado,
                                category: category,
                                ejecutivo: EjecutivoCuenta,
                                idEjecutivo: IdEjecutivoCuenta,
                                identities: identities
                            });
                        });
                    }
                }
            }
            this.loading = false;
        },
        async detallesProspecto(datos) {

            this.EjecutivoUsed = datos.idEjecutivo;
            this.segmentar.user = null;
            this.errorEjecutivo = '';
            this.ejecutivoAsignado = true;

            this.modalCargandoDetalle = true;
            let person = { id: datos.person_id }
            this.segmentar.confirmant = datos.person_id;

            const data = await axios.post('?handler=DetalleCliente', person, { headers: { "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val() } })
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
                if (data.includes("<!DOCTYPE html>")) {
                    window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired";
                    toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br />" + i18n.t("errorBaseDatos"));
                    return;
                }
            }

            if (data.error == "You are not authorised to perform this action") {
                toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br />" + i18n.t("tooltip.noAuthorizado"));
                window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
            }

            console.log(data)
            this.DatosDetalles = data;
            this.modalCargandoDetalle = false;
            this.detalleUsuario = true;
        },
        validarEjecutivo() {

            if (this.segmentar.user == null || this.segmentar.user == '') {

                this.errorEjecutivo = this.hasError;
                this.ejecutivoAsignado = true;

            } else {
                this.errorEjecutivo = this.hasSuccess;
                this.ejecutivoAsignado = false;
            }
        },
        titleCaps(string) {

            
            let aux = ""
            let ant = ""
            let primera = true

            let casosEspecialesFinal = /(\.)$/
            let conectores = /^(el|la|yo|si|con|etc|and|of|by|de|y|los)+$/


            string.split(" ").map(data => {
            

                if (data == undefined || data == null || data == "") return


                if (primera) {
                    aux = data[0].toUpperCase() + "" + data.slice(1)+" "
                    ant = data
                    primera = false
                   return
                }

                let palabraTestear = data

                if (casosEspecialesFinal.test(palabraTestear))
                    palabraTestear = palabraTestear.substr(0, palabraTestear.length - 1)

                if (conectores.test(palabraTestear) && !casosEspecialesFinal.test(ant)) {
                    aux += data.toLowerCase() + " "
                    ant = data
                } else {
                    aux += data[0].toUpperCase() + data.slice(1) + " "
                    ant = data
                }
            })
            return aux
           },
        async enviarDatos(segmentar) {

            //this.EjecutivoUsed == segmentar.user
            if (this.EjecutivoUsed == segmentar.user) {
                toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br />" + "El cliente ya tiene selecionado dicho ejecutivo de cuenta.");
                return
            }

            if (this.bloqueoBoton) {
                return
            }
            
            this.errorEjecutivo = '';
            this.ejecutivoAsignado = true;
            this.bloqueoBoton = true;

            const data = await axios.post('Segmentacion?handler=SegmentarCliente', segmentar, { headers: { "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val() } })
                .then(function (response) {

                    resetTime();
                    return response;

                }).catch((err) => {

                    toastr.error(i18n.t("errorBaseDatos"));
                    return err;
                });

            if (typeof data.data === 'string' || data.data instanceof String) {
                if (data.data.includes("<!DOCTYPE html>")) {
                    window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired";
                    toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br />" + i18n.t("errorBaseDatos"));
                    return;
                }
            }
            if (data.data != null) {

                if (data.data.includes("success:") == true) {

                    this.detalleUsuario = false;
                    this.bloqueoBoton = false;
                    toastr.success(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br />" + i18n.t("segmentarUsuario"));
                    this.DatosResividos = [];
                    this.llenar_consulta();

                    let temporalListaEjecutivos = this.listaEjecutivos;
                    this.EjecutivoSeleccionado = [];

                    for (let i = 0; i < temporalListaEjecutivos.length; i++) { this.EjecutivoSeleccionado.push(temporalListaEjecutivos[i].name); }

                    let lenguage = 'es-VE';
                    switch (document.getElementsByTagName("html")[0].getAttribute("lang")) {
                        case 'ENU': lenguage = 'en'; break;
                        case 'ESS': lenguage = 'es-AR'; break;
                        case 'ESV': lenguage = 'es-VE'; break;
                    }

                } else {
                    if (data.data == "verification not found") toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br /> La verificación no fue encontrada.");
                    else if (data.data == "You are not authorised to perform this action") {

                        toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br />" + i18n.t("tooltip.noAuthorizado"));
                        window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                    }
                    else toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br /> Ocurrio un error en la Conexión. Por favor recargué la página.");
                }
                this.detalleUsuario = false;
                this.bloqueoBoton = false;

            } else { toastr.error(i18n.t("errorBaseDatos")); }

        },

    },
    computed: {
        habilitarBotonAsignar() {
            return this.ejecutivoAsignado;
        },
        cambioEjecutivo(ejecutivo, index) {
            return ejecutivo[index];
        },
        mounted: async function () {
            await this.llenar_consulta()
        },
    },
    mounted: async function () {
        tiempoLogin(this.modalLogout)
    }
});
