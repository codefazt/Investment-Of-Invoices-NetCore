//Direcciones de Vue con Vuex

const store = new Vuex.Store({
    state: {
        resetearRegistro: null,
        registroGuardar: [],
        mensajes: [],
        CorreoRepresentanteLegal: null,
        respuestaComprobacionDoc: false,
        respuestaComprobacionDocError: '',
        respuestaComprobacionDocErrorTexto: '',
        desicionRegistro: 2,
        boleanoDesicionRegistro: true,
        registroExistente: null,

    },
    mutations: {

        mutacionReceteo(state, data) {
            state.resetearRegistro = data;
        },
        mutacionRegistroDocVerificacion(state, respuesta) {

            state.respuestaComprobacionDoc = respuesta.llamadoDoc;
            state.respuestaComprobacionDocError = respuesta.mensajeError;
            state.respuestaComprobacionDocErrorTexto = respuesta.textoMensajeError;

            if (respuesta.data == null) {
                state.registroExistente = null;
                state.boleanoDesicionRegistro = false;
                state.desicionRegistro = 1;

            } else {

                state.CorreoRepresentanteLegal = respuesta.data.representante.email;
                state.registroExistente = respuesta.data;
                state.boleanoDesicionRegistro = false;
                state.desicionRegistro = 0;

            }

        },
    },
    actions: {

        guardarFormularioProveedor: async function ({ commit }, registroOriginal) {

            registroGuardar = JSON.parse(JSON.stringify(registroOriginal));

            registroGuardar.administrador = null;
            registroGuardar.dob = null;

            if (registroGuardar.contacto != null) {

                if (registroGuardar.contacto.documento.number == "" || registroGuardar.contacto.documento.number == null ||
                    registroGuardar.contacto.name == "" ||
                    registroGuardar.contacto.lastName == "" ||
                    registroGuardar.contacto.phone.number == "" ||
                    registroGuardar.contacto.email == "") {

                    registroGuardar.contacto = null;
                }
            }
            

            if (registroGuardar.proveedores.length > 0) {

                respaldoAsociados = registroGuardar.proveedores;

                for (var i = 0; i < registroGuardar.proveedores.length; i++) {

                    if (registroGuardar.proveedores[i].representante.documento.number == "" ||
                        registroGuardar.proveedores[i].representante.documento.number == null ||
                        registroGuardar.proveedores[i].representante.name == "" ||
                        registroGuardar.proveedores[i].representante.lastName == "" ||
                        registroGuardar.proveedores[i].representante.phone.number == "" ||
                        registroGuardar.proveedores[i].representante.email == "") {

                        registroGuardar.proveedores[i].representante = null;
                    }
                    if (registroGuardar.proveedores[i].contacto.documento.number == "" ||
                        registroGuardar.proveedores[i].contacto.documento.number == null ||
                        registroGuardar.proveedores[i].contacto.name == "" ||
                        registroGuardar.proveedores[i].contacto.lastName == "" ||
                        registroGuardar.proveedores[i].contacto.phone.number == "" ||
                        registroGuardar.proveedores[i].contacto.email == "") {

                        registroGuardar.proveedores[i].contacto = null;
                    }
                }
            }
            await axios.post('', registroGuardar, { headers: { "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val() } })
                .then((respond) => {

                    if (respond.data == 'Error') toastr.error(i18n.t("tooltip.errorUsuarioRegistrado"));

                    else if (respond.data == 'Fallo al Validar') {
                        toastr.error("Tiene uno o varios errores al validar");
                    }
                    else {

                        commit('mutacionReceteo', respond.data);
                        if (registroGuardar.tipoRegistro == 'EMPRESA') {
                            toastr.success("<div class='text-justify'>" + i18n.t("tooltip.queridoUsuario") + "<br /><br />" + i18n.t("tooltip.registroEmpresa") + "</div>");
                            /*
                             * Condicional para ver si es el banco o es por Autogestion
                             * toastr.success(i18n.t("tooltip.queridoUsuario") + "<br />" + i18n.t("tooltip.registroEmpresaBanco"));
                             */
                        } else {
                            toastr.success("<div class='text-justify'>" + i18n.t("tooltip.queridoUsuario") + "<br /><br />" + i18n.t("tooltip.registroProveedor") + "</div>");
                        }
                    }

                }).catch((error) => {

                    console.log(error);
                    toastr.error("Ocurrio un Error con la Base de Datos");
                });

        },
        guardarFormularioBanco: function ({ commit }, registroOriginal) {

            registroGuardar = JSON.parse(JSON.stringify(registroOriginal));
            registroGuardar.cuentas = null;
            registroGuardar.proveedores = null;
            registroGuardar.contacto = null;

            axios.post('', registroGuardar, { headers: { "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val() } })
                .then((respond) => {

                    if (respond.data == 'Error') toastr.error(i18n.t("tooltip.errorUsuarioRegistrado"));

                    else if (respond.data == 'Fallo al Validar') {
                        toastr.error("Tiene uno o varios errores al validar");
                    }
                    else {

                        commit('mutacionReceteo', respond.data);
                        toastr.success("<div class='text-justify'>" + i18n.t("tooltip.queridoUsuario") + "<br /><br />" + i18n.t("tooltip.registroBanco") + "</div>");
                    }
                }).catch((error) => {

                    console.log(error);
                    toastr.error("Ocurrio un Error con la Base de Datos");
                });

        },
        guardarFormularioInversionista: function ({ commit }, registroOriginal) {

            registroGuardar = JSON.parse(JSON.stringify(registroOriginal));
            registroGuardar.proveedores = null;
            registroGuardar.administrador = null;
            registroGuardar.contacto = null;

            if (registroGuardar.tipoRegistro == 'NATURAL') {
                registroGuardar.representante = null;
            }

            axios.post('', registroGuardar, { headers: { "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val() } })
                .then((respond) => {

                    if (respond.data == 'Error') toastr.error(i18n.t("tooltip.errorUsuarioRegistrado"));
                    else if (respond.data == 'Fallo al Validar') {
                        toastr.error("Tiene uno o varios errores al validar");

                    } else {

                        commit('mutacionReceteo', respond.data);
                        toastr.success("<div class='text-justify'>" + i18n.t("tooltip.queridoUsuario") + "<br /><br />" + i18n.t("tooltip.registroEmpresa") + "</div>");
                    }
                }).catch((error) => {

                    console.log(error);
                    toastr.error("Ocurrio un Error con la Base de Datos");

                });

        },

        verificarRegistro: function ({ commit }, comprobarDoc) {

            let comparacionRegistrosBank = 'nada';
            let comparacionRegistros = comprobarDoc.participant;
            if (comprobarDoc.participant == 'BANK') comparacionRegistrosBank = 'CONFIRMANT';

            let hasError = 'is-invalid';
            let HasSuccess = 'is-valid';

            let errorMensaje = ''

            if (comparacionRegistros == 'SUPPLIER') {
                errorMensaje = i18n.t("valid.usuarioExistenteProveedor");
            } else if (comparacionRegistros == 'DEBTOR') {
                errorMensaje = i18n.t("valid.usuarioExistenteEmpresa");
            } else if (comparacionRegistros == 'FACTOR') {
                errorMensaje = i18n.t("valid.usuarioExistenteFactor");
            } else if (comparacionRegistros == 'BANK' || comparacionRegistros == 'CONFIRMANT') {
                errorMensaje = i18n.t("valid.usuarioExistenteBanco");
            }

            axios.post('Register?handler=VerificarDoc', comprobarDoc, { headers: { "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val() } })
                .then((respond) => {
                    let respuestaData = { mensajeError: hasError, textoMensajeError: 'Usuario Existente', llamadoDoc: false, data: {} }

                    if (respond.data != null) {

                        if (respond.data.legalName != null || respond.data.legalName != '') {

                            let validarRegistro = respond.data.tipoRegistro.split(",");

                            if (comparacionRegistros == validarRegistro[0] ||
                                comparacionRegistros == validarRegistro[1] ||
                                comparacionRegistros == validarRegistro[2] ||
                                comparacionRegistros == validarRegistro[3] ||
                                comparacionRegistros == validarRegistro[4] ||
                                comparacionRegistrosBank == validarRegistro[0] ||
                                comparacionRegistrosBank == validarRegistro[1] ||
                                comparacionRegistrosBank == validarRegistro[2] ||
                                comparacionRegistrosBank == validarRegistro[3] ||
                                comparacionRegistrosBank == validarRegistro[4]) {

                                respuestaData = { mensajeError: hasError, textoMensajeError: errorMensaje, llamadoDoc: false, data: null }
                                commit('mutacionRegistroDocVerificacion', respuestaData);

                            } else if (comparacionRegistros == 'FACTOR' && validarRegistro == '') {

                                respuestaData = { mensajeError: hasError, textoMensajeError: errorMensaje, llamadoDoc: false, data: null }
                                commit('mutacionRegistroDocVerificacion', respuestaData);

                            } else {
                                respuestaData = { mensajeError: HasSuccess, textoMensajeError: '', llamadoDoc: false, data: respond.data }
                                commit('mutacionRegistroDocVerificacion', respuestaData);
                            }

                        } else {
                            respuestaData = { mensajeError: hasError, textoMensajeError: errorMensaje, llamadoDoc: false, data: respond.data }
                            commit('mutacionRegistroDocVerificacion', respuestaData);
                        }

                    } else {

                        respuestaData = { mensajeError: HasSuccess, textoMensajeError: '', llamadoDoc: false, data: null }
                        commit('mutacionRegistroDocVerificacion', respuestaData);
                    }

                }).catch(function (error) {

                    let respuestaData = { mensajeError: HasSuccess, textoMensajeError: '', llamadoDoc: false, data: null }
                    commit('mutacionRegistroDocVerificacion', respuestaData);

                });



        }
    }
});
