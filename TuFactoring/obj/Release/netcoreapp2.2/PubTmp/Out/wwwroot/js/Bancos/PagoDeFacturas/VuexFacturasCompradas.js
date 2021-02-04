const store = new Vuex.Store({
    state: {
        dataRecargada: null
    },
    mutations: {

        recargadoTabla(state, data) {

            if (data.data != "[]" && data.data != [] && data.data != null) state.dataRecargada = data.data;
            else state.dataRecargada = [];
        }
    },
    actions: {

        async PagarFactura({ commit }, payments) {

            const data = await axios.post('', payments, { headers: { "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val() } })
                .then(function (response) {

                    if (response.data != "Error") {
                        // handle success
                        toastr.info("Pago Enviado Espere por su Conciliacion", "Confirmacion");
                        commit("recargadoTabla", response);
                        $("#tranferirBank").modal('hide');
                        //setTimeout(function () { window.location.pathname = "/Banco"; }, 2000);
                    } else {
                        toastr.error("Numero de Referencia Existente", "Error de Pago");
                    }

                    return response;
                }).catch(err => {

                    toastr.error("No se pudo conectar a base de datos", "Error de Pago");
                });
        }

    }
});