
try {
    var msg = document.getElementById("mensajeError").value
    
    if (msg != null && msg != "") {
        toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.t(msg))
    }    

} catch (e) { }


try {
    msg = document.getElementById("mensajeExito").value
    
    if (msg != null && msg != "") {
        toastr.success(i18n.t("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.t(msg))
    }

} catch (e) {  }


try {
    for (var i = 0; i < 10; i++) {
        msg = document.getElementById("mensajeError-"+i).value

        if (msg != null && msg != "") {
            console.log(msg)
            toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.t(msg))
        }

    }
} catch (e) { }
