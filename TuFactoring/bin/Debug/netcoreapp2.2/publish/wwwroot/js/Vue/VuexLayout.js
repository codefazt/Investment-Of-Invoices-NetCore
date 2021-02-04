const vuexLayout = new Vuex.Store({
    state: {
        mensajes: []      
    },
    mutations: {
        mensajes: function (state) {
            try {
                var id = JSON.parse(document.getElementById('People').value);

                connection = new signalR.HubConnectionBuilder().withUrl("/wsUser").build()

                connection.start().then(function () {
                   
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
    }
})