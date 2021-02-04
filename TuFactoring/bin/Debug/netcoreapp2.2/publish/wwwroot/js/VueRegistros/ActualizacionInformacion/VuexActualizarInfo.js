//Direcciones de Vue con Vuex

const store = new Vuex.Store({
    state: {
        mensajes: [],
        registroGuardar: null,
        respuestaPerfil: null,
        respuestaComprobacionDoc: false,
        respuestaComprobacionDocError: '',
        respuestaComprobacionDocErrorTexto: '',
        boleanoDesicionRegistro: true,
        registroExistente: null,
    },
    mutations: {

        mutacionRespuestaFormulario(state, ConsultaActualizada) {
            state.respuestaPerfil = ConsultaActualizada;
        },
        mensajes: function (state) {
            try {
                var id = JSON.parse(document.getElementById('People').value);

                connection = new signalR.HubConnectionBuilder().withUrl("/wsUser").build()

                connection.start().then(function () {
                    connection.invoke("loginGroup", id).catch(error => {
                        console.error("ERROR IN CONNECTION")
                        connection.stop()
                    })


                }).catch(function (err) {
                    return console.error(err.toString());
                });

                connection.on("Message", function (mensaje) {
                    state.mensajes.push(mensaje)
                })

                //document.getElementById("eliminarApp").removeChild(document.getElementById('People'))
            } catch (error) {
            }

        },
        //***********************************************************************************************************************
        limpiarMensajes: function (state) {
            state.mensajes = []
        }
    },
    actions: {

        guardarActualizacion: function ({ commit }, registroOriginal) {

            registroGuardar = JSON.parse(JSON.stringify(registroOriginal));
            registroGuardar.documentoPrincipal = null;

            if ('DEBTOR' == registroGuardar.participant || 'SUPPLIER' == registroGuardar.participant ) {

                registroGuardar.administrador = null;

                if (registroGuardar.contacto != null) {
                    if ((registroGuardar.contacto.documento.number == null) ||
                        (registroGuardar.contacto.name == null) ||
                        (registroGuardar.contacto.lastName == null) ||
                        (registroGuardar.contacto.email == null)) {

                        registroGuardar.contacto = null;
                    }
                }
            }
            else if ('FACTOR' == registroGuardar.participant) {

                registroGuardar.administrador = null;
                registroGuardar.contacto = null;
                registroGuardar.proveedores = null;

                if (registroGuardar.representante != null) {

                    if ((registroGuardar.representante.documento.number == null) ||
                        (registroGuardar.representante.name != null) ||
                        (registroGuardar.representante.lastName != null) ||
                        (registroGuardar.representante.email != null)) {

                        registroGuardar.representante = null;
                    }
                }
            }
            else if ('CONFIRMANT' == registroGuardar.participant) {

                registroGuardar.contacto = null;
                registroGuardar.proveedores = null;
                registroGuardar.cuentas = null;
            }

            registroGuardar.participant = null;
            axios.post('', registroGuardar, { headers: { "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val() } })
                .then((respond) => {
  
                    if (respond.data != "Error") {

                        commit('mutacionRespuestaFormulario', respond.data);
                        toastr.success("<div class='text-justify'>" + i18n.t("tooltip.queridoUsuario") + "<br /><br />" + i18n.t("tooltip.actualizarPerfil") + "</div>");

                    } else {
                        toastr.error(i18n.t("errorBaseDatos"));
                    }

                }).catch((error) => {

                    console.log(error);
                    toastr.error(i18n.t("errorBaseDatos"));

                });

        },

    }
});