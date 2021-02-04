const store = new Vuex.Store({
    state: {
        RcontratoAceptado: null
    },
    mutations: {

        mutacionAceptarContrato(state, respuesta) {
            state.RcontratoAceptado = respuesta;
        },

    },
    actions: {

        async contratoAceptado({ commit }) {

            var data = await axios.post('', null, { headers: { "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val() } })
                .then((respond) => {
                    console.log(respond);
                    return respond;

                }).catch((respond) => {
                    console.log(respond);
                    toastr.error("Error", "Problemas con la Base de Datos.");
                });

            if (data.data != "Error") {

                //commit('mutacionAceptarContrato', true)

                $("#contratoProveedor").addClass("fade");
                $('#contratoProveedor').removeClass('show');
                //$("#contratoTerminoCondiciones").modal("hide");
                

            } else toastr.error("Error", "Usuario Inexistente.");

        }
    }
});