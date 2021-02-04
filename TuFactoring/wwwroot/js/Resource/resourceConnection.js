let connectionSignalRActual = {}
let functionPostConnectionActual = {}
let rutaActual = ""

async function startSignalrConnection(ruta, functionPostConnection) {
    var connection = new signalR.HubConnectionBuilder().withUrl(ruta).build()

    await connection.start().then(() => {
        rutaActual = ruta
        connectionSignalRActual = connection

        if (functionPostConnection != postConnectAux) {
            functionPostConnectionActual = functionPostConnection
        }

        functionPostConnection(connection)
        connection.onclose(closeSignalR)
    }).catch(function (err) {
        setTimeout(() => startSignalrConnection(ruta,functionPostConnection),2000)  
    });

    
}

async function closeSignalR(msg) {
    console.log("disconnected")
    connectionSignalRActual.stop()
    startSignalrConnection(rutaActual, postConnectAux)
}

function postConnectAux(connection) {
    console.log("reconnected")
    functionPostConnectionActual(connection)
}