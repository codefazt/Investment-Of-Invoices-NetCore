//Vista Ejecutivo de Cuentas de Vue con Vuex

const store = new Vuex.Store({
    state: {
        ListaActualizada: null
    },
    mutations: {

        mutacionActualizarLista(state, data) {
            state.ListaActualizada = data;
        }
    },
    actions: {

        enviarDatos({ commit }, montoLimite) {

            const data = axios.post('', montoLimite, { headers: { "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val() } })
                .then(function (response) {
                    // handle success
                    toastr.success("Monto Asignado", "Confirmacion");
                    commit('mutacionActualizarLista', response);

                    document.getElementById('form').submit();
                    return response;
                }).catch((err) => {
                    toastr.error("Ocuarrio algun Error en la base de Datos vuelva a intentar mas tarde", "Error.")
                    return err;
                });

        },
    }
});