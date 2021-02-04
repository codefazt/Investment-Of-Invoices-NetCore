var app = new Vue({
    el: '#appVerificarDatos',
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
        filter: null,
        widthTelefono: widthTelefono,

        buscarClientes: true,
        totalClientes: 0,
        moment: moment,
        loading: false,
        options: {},
        DatosDetalles: null,
        modalCargandoDetalle: false,
        approveVerification: {

            id: null,
            person: null,
            entity: null,
            accepted: false,
            status: null,
            comment: null
        },

        tamanoTlf: tamanoTlf,
        // Comienzo de Datos para Menu Principal
        cerrarMordisco: true,
        drawer: false,
        detalleUsuario: false,
        // fin de Datos para Menu Principal
        cargando: true,
        abrirModal: false,
        rechazo: false,
        x: null,
        DatosResividos: [],
        verificacion: {        // nombre de la Variable y propiedades provicionales
            clienteId: '',
            comentario: '',
        },
        headerCliente: [
            { text: i18n.t("headers.n"), value: "n", align: "center" },
            { text: i18n.t("headers.cliente"), value: 'company' },
            { text: i18n.t("headers.representanteLegal"), value: 'representante' },
            { text: i18n.t("headers.estado"), value: 'estado', align: 'center', },
            { text: i18n.t("headers.categoria"), value: 'category', align: 'center', },
            { text: i18n.t("headers.opciones"), value: 'action', align: 'center' },
        ],
        errorComentario: '',
        errorComentarioTexto: '',
        hasSuccess: 'is-valid',
        hasError: 'is-invalid'

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

        console.log(this.filter)
        //this.DatosResividos = JSON.parse(document.getElementById('contenidoRaw').value);
        document.getElementById('contenido').removeAttribute('hidden');
        this.cargando = false;

    },
    methods: {
        async llenar_consulta() {

            if (this.loading) return
            this.loading = true
            let take = 100;
            this.listaProvincias = JSON.parse(document.getElementById('listadosInicialesJson').value);
            console.log(this.listaProvincias)
            const data = await axios.post('VerificarDatos?handler=LlenarTabla', { pagination: { take: take, skip: this.totalClientes }, filter: this.filter }, { headers: { "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val() } })
                .then(function (respond) {
                    // handle success
                    resetTime();
                    return respond;

                }).catch(function (error) {

                    console.log("entro en el catch: ");
                    console.log(error);
                });

            if (data != null) {
                if (typeof data.data === 'string' || data.data instanceof String) {
                    console.log(data);
                    if (data.data.includes("<!DOCTYPE html>")) {
                        window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired";
                        toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br />" + i18n.t("errorBaseDatos"));
                        return;
                    }
                }
            }

            if (data != null) {
                if (data.data != null) {
                    if (data.data.prospecto.list.length == 0) {
                        this.loading = false
                        this.buscarClientes = false
                    }

                    if (data.data.prospecto.list.length < take) this.buscarClientes = false
                    this.totalClientes += parseInt(data.data.prospecto.list.length)

                    let representante = '-';
                    let category = '-';
                    let estado = '-';
                    let listaEstados = data.data.estados;

                    data.data.prospecto.list.map(data => {
                        if (data == null) return

                        if (data.person.discriminator == 'PERSON') {
                            data.person.company = data.person.firstName;
                            representante = data.person.firstName;
                        }
                        if (data.person.contacts != null && data.person.discriminator == 'LEGAL') {
                            for (let i = 0; i < data.person.contacts.length; i++) {
                                if (data.person.contacts[i].label == 'LEGAL') {
                                    representante = data.person.contacts[i].name;
                                }
                            }
                        }

                        if (listaEstados != null) {
                            for (let i = 0; i < listaEstados.regions.length; i++) {

                                if (data.person.addresses != null) {
                                    if (data.person.addresses[0].region == listaEstados.regions[i].id) estado = listaEstados.regions[i].name;
                                }
                            }
                        }

                        if (data.person.identities != null) {

                            switch (data.person.identities[0].participant) {
                                case 'DEBTOR': category = 'EMPRESA'; break;
                                case 'FACTOR': category = 'INVERSIONISTA'; break;
                                case 'SUPPLIER': category = 'PROVEEDOR'; break;
                                default: '-';
                            }

                        }

                        //Crear Tabla
                        this.DatosResividos.push({
                            verification_id: data.id,
                            person_id: data.person.id,
                            discriminator: data.person.discriminator,
                            company: data.person.company,
                            representante: representante,
                            estado: estado,
                            category: category,
                        });
                    });
                }
            }
            else {
                window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
            }
            //this.facturas.push(data.data);
            this.loading = false;
        },
        async detallesProspecto(datos) {

            this.rechazo = false;
            this.modalCargandoDetalle = true;
            let person = { id: datos.person_id }

            this.approveVerification.id = datos.verification_id;
            //this.approveVerification.person = datos.person_id;

            const data = await axios.post('VerificarDatos?handler=DetalleCliente', person, { headers: { "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val() } })
                .then(function (response) {
                    // handle success
                    resetTime();
                    console.log(response)
                    if (response == null || response.data == null || response.data == "Error") {
                        window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                        return null;
                    }
                    return response.data;

                }).catch(function (error) {

                    console.log("entro en el catch: ");
                    console.log(error);
                });

            if (typeof data === 'string' || data instanceof String) {
                console.log(data);
                if (data.includes("<!DOCTYPE html>")) {
                    window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                }
            }
            
            this.DatosDetalles = data;
            this.modalCargandoDetalle = false;
            this.detalleUsuario = true;
        },
        VerificarEspacios(campo) {
            let RE = /[\.\ ]/
            let repeticiones = 0
            let letra = ''
            for (var i = 0; i < campo.length; i++) {

                if (letra == campo[i] && RE.test(letra)) {
                    repeticiones++;

                    if (repeticiones == 1) {
                        return true
                    }
                } else {
                    repeticiones = 0
                    letra = campo[i]
                }

            }
            return false
        },
        VerificarLetrasSeguidas(campo) {
            let RE = /[a-zA-ZÁ-ý\u00C0-\u017F\|\°\¬\\\!\"\#\$\%\&\/\(\)\=\?\'\¡\¿\´\¨\+\*\~\[\{\^\]\}\`\,\.\-\_\:\;]/
            let repeticiones = 0
            let letra = ''

            for (var i = 0; i < campo.length; i++) {

                if (letra == campo[i] && RE.test(letra)) {
                    repeticiones++;

                    if (repeticiones == 2) {
                        return true
                    }
                } else {
                    repeticiones = 0
                    letra = campo[i]
                }

            }
            return false
        },
        validacionComentario() {
            let emailRegex = /[A-Za-zÁ-ý]{1,}/;
            let espacios = /^[\.]{}+$/i;

            if (this.approveVerification.comment == '' ||
                this.approveVerification.comment == null ||
                this.approveVerification.comment[0] == ' ') {

                this.errorComentario = this.hasError;
                this.errorComentarioTexto = i18n.t("valid.comentarioRechazoRequerido");

            } else if (!emailRegex.test(this.approveVerification.comment)) {

                this.errorComentario = this.hasError;
                this.errorComentarioTexto = i18n.t("valid.comentarioRechazoLetras");

            } else if (this.VerificarLetrasSeguidas(this.approveVerification.comment) || this.VerificarEspacios(this.approveVerification.comment)) {

                this.errorComentario = this.hasError;
                this.errorComentarioTexto = i18n.t("valid.max2CharRepetidos");

            } else if (this.approveVerification.comment.length < 15) {

                this.errorComentario = this.hasError;
                this.errorComentarioTexto = i18n.t("valid.comentarioRechazoMin");

            } else if (this.approveVerification.comment.length > 100) {

                this.errorComentario = this.hasError;
                this.errorComentarioTexto = i18n.t("valid.comentarioRechazoMax");

            } else {
                this.errorComentario = this.hasSuccess;
                this.errorComentarioTexto = '';
            }
        },
        reseteoComentario() {

            this.approveVerification.comment = null;
            this.errorComentario = '';
            this.errorComentarioTexto = '';
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
                    aux = data[0].toUpperCase() + "" + data.slice(1) + " "
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
        async enviarDatos() {

            this.approveVerification.accepted = true;
            const data = await axios.post('VerificarDatos?handler=VerificarProspecto', this.approveVerification, { headers: { "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val() } })
                .then(function (response) {
                    // handle success
                    resetTime();
                    if (response.data == notAuthorized) {
                        window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                        return
                    }
                    return response;
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

                    toastr.success(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br />" + i18n.t("verificarDatos"));
                    this.DatosResividos = [];
                    this.llenar_consulta();
                    this.detalleUsuario = false;

                } else {
                    if (data.data == "verification not found") toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br /> La verificación no fue encontrada.");
                    else if (data.data == "You are not authorised to perform this action") toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br />" + i18n.t("tooltip.noAuthorizado"));
                    else toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br /> Ocurrio un error en la Conexión. Por favor recargué la página.");
                }
                
            } else {
                //window.location.href = "/";
                toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br /> Ocurrio un error en la Conexión. Por favor recargué la página.");
                return;
            }

        },
        async rechazarDatos() {

            this.approveVerification.accepted = false;
            const data = await axios.post('VerificarDatos?handler=VerificarProspecto', this.approveVerification, { headers: { "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val() } })
                .then(function (response) {
                    // handle success
                    resetTime()
                    return response;
                });

            if (typeof data.data === 'string' || data.data instanceof String) {
                console.log(data);
                if (data.data.includes("<!DOCTYPE html>")) {
                    window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired";
                    toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br />" + i18n.t("errorBaseDatos"));
                    return
                }
            }

            if (data.data != null) {

                if (data.data.includes("success:") == true) {

                    toastr.success(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br />" + i18n.t("verificarDatos"));
                    this.DatosResividos = [];
                    this.llenar_consulta();
                    this.detalleUsuario = false;
                    this.reseteoComentario();

                } else {
                    if (data.data == "verification not found") toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br /> La verificación no fue encontrada.");
                    else toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br /> Ocurrio un error en la Conexión. Por favor recargué la página.");
                }

            } else {
                //window.location.href = "/";
                toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br /> Ocurrio un error en la Conexión. Por favor recargué la página.");
                return;
            }

        },
    },
    computed: {
        habilitarbotonFormulario() {

            if (this.errorComentario == this.hasSuccess) {
                return false;
            } else {
                return true;
            }
        },
        mounted: async function () {
            await this.llenar_consulta();

        },
    },
    mounted: async function () {
        tiempoLogin(this.modalLogout)
    }
});