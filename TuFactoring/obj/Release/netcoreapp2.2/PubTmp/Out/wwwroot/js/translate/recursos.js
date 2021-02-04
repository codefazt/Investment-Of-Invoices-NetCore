var notAuthorized = "You are not authorised to perform this action"
var widthTelefono = 960
let timeCurrentLogout2 = 0
let timeLogout =16
let timeLessLogout = 0
let modalSession = {}

function backEndDateFormat(date) {
    if (date == null || date == '') return ''
    try {

        if (date.length > 10) date = date.substr(0, 10);
        return moment(date, 'MM/DD/YYYY').format('DD/MM/YYYY');
    } catch (e) { console.log(e); return date.substr(0, 10); }
}
//
function backEndDateFormat2(date) {
    if (date == null || date == '') return ''
    try {
        
        return moment(date, 'MM/DD/YYYY hh:mm:ss').format('hh:mm:ss A');
    } catch (e) { console.log(e); return date.substr(0, 10); }
}
// Cálculo de Días por Vencer
function diferenciaFechas(date) {

    return moment(date, "DD/MM/YYYY").diff(moment(), "days");
}
//
function diferenciaFechaFiltrar(dateFiltro, dateItem) {

    return moment(dateFiltro, "YYYY-MM-DD").diff(moment(dateItem, "DD/MM/YYYY"), "days")
} 
//
function formatoMoneda(monto,lang) {
    monto += ""
    constante = 0
    
    if(monto == 0 || monto == '') return 0

    data = obtenerLang(lang)
    
    switch (data) {
        case "es": monto = monto.replace(/[^\d\,]/g, "").replace(",", ".")
            break
        case "en": monto = monto.replace(/[^\d\.]/g, "")
            break
    }
    return parseFloat(monto)
}
//
function collapseAll(id) {
        try {
            var i = 0
            while (true) {
                if ($(id + i).hasClass("show")) {
                    $(id + i).removeClass("show")
                } else {
                    $(id + i).addClass("show")
                }
                i++
            }
        }
        catch (e) {

    }
    
}

//
function rellenarMonto(monto, comienzo) {
    
    for (var i = comienzo; i < digitsActual; i++) {

        if (i == -1) {
            monto += "."
        } else {
            monto += "0"
        }
    }

    return monto
}
//
function formatoMonedaInput(valor, lang,digits) {

    if(valor == 0 || valor == '') return ''

    data = obtenerLang(lang)
    
    let monto = new Intl.NumberFormat(data, {
        maximumFractionDigits: (digits == null || digits == undefined) ? 2: digits,
        minimumFractionDigits: digits
    }).format(valor)

    if (isNaN(formatoMoneda(monto,data))) return ''

    if (data.substr(0, 2) == "es" ) {
        return formatoMilEs(monto)
    }
    
    return monto
}
//
function obtenerLang(lang) {
    switch (lang) {
        case "ESS":
        case "ESV":
        case "es": return "es"; break
        case "ENU":
        case "en": return "en"; break;
        default: return "es"
    }
}
//
function formatoMilEs(monto) {
    let verificacion = monto.split(",")
    let luegoComa = ''

    if (verificacion[0].length != 4) return monto

    if (verificacion.length > 1) {
        luegoComa = verificacion[1]
        verificacion = verificacion[0]
    } else {
        verificacion = verificacion[0]
    }

    monto = verificacion[0] + "." + verificacion[1] + "" + verificacion[2] + "" + verificacion[3]

    if (luegoComa != '') monto += "," + luegoComa

    return monto;
}
//
function revisarInputOferta(event,lang) {
    if ((event.keyCode < 48 || event.keyCode > 57) && (event.keyCode != fraccion(lang))) event.returnValue = false;
}
//
function fraccion(lang) {

    data = obtenerLang(lang)

    switch (data) {
        case "es": return 44; break; // ,
        case "en": return 46; break; // .
        default: return 44
    }
}

function tamanoTlf() {
    if (document.body.clientWidth <= widthTelefono) return 0 //si es un telefono humilde
    
    return 1
}

async function mensajeria() {

    try {
        var connection = new signalR.HubConnectionBuilder().withUrl("/wsUser").build()

        await connection.start().then(async function () {
            await axios.post('../Profile/MantenimientoUsuarios?handler=usuarioData', {},
                {
                    headers: {
                        "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                    }
                })
                .then((respond) => {
                    if (respond == null || respond.data == null) return

                    connection.invoke("loginGroup", respond.data).catch((err) => {
                        console.log(err)
                    })

                    connection.on("Message", (message) => {
                         agregarAlDiv(document.getElementById("messageContent"), message)
                    })
                }).catch((respond) => { console.log(respond); });


        }).catch(function (err) {
        });
    } catch (e) {
    }

    try {
        document.getElementById("limpiarmsg").addEventListener("click", limpiarMssg, false)
        document.getElementById("limpiarmsg").addEventListener("click", (e) => {
            e.stopPropagation();
        })
    } catch (e) { }

    try {

        console.log("online: ", navigator.onLine);

        window.addEventListener("online", () => {

            console.log("online: ", true);
        });

        window.addEventListener("offline", () => {

            console.log("online: ", false);
        });
    } catch (e) { }

    setTimeout(blockMenu, 3000);
}

function agregarAlDiv(div, message) {
    div.innerHTML += `<a class="dropdown-item d-flex align-items-center" href="#">
                            <div class="dropdown-list-image mr-3">
                                <img class="rounded-circle" src="https://source.unsplash.com/fn_BT9fwg_E/60x60" alt="">
                                    <div class="status-indicator bg-success"></div>
                                </div>
                                <div class="font-weight-bold">
                                    <div class="text-truncate">`+ message.title + `</div>
                                    <div class="small text-gray-500">`+ message.body + `</div>
                                </div>
                            </a>`
}

function limpiarMssg(){
  document.getElementById("messageContent").innerHTML = ""
}

function formatoAmount(id) {
    var input = document.getElementById(id)

    input.value = formatoMonedaInput(formatoMoneda(input.value, langActual), langActual, digitsActual)
}

function listaCities(id, listadosInicialesJson) {

    if (id != null && listadosInicialesJson != null) {
        var idEstadoSelect = document.getElementById(id).value;
        var objCities = document.getElementById('filter_City');

        if (objCities != null) {
            while (objCities.length > 0) {
                objCities.remove(0);
            }
        }

        objCities = document.getElementById('filter_City');

        var dataInicial = JSON.parse(document.getElementById(listadosInicialesJson).value);

        for (let i = 0; i < dataInicial.regions.length; i++) {

            if (dataInicial.regions[i].id == idEstadoSelect) {
                console.log(dataInicial.regions[i].cities.length)
                for (let j = 0; j < dataInicial.regions[i].cities.length; j++) {

                    var option = document.createElement("option");
                    option.text = dataInicial.regions[i].cities[j].name;
                    option.value = dataInicial.regions[i].cities[j].id;
                    objCities.add(option);
                }
                continue
            }
        }
    }

}

function validarRegExpr(expreReg, mascara) {
    if (mascara == null) mascara = ""

    var regExp = new RegExp(expreReg);
    let doc = document.getElementById('Input_Number');//Input_Number
    let valid = document.getElementById('validNum');

    if (valid.innerHTML == "" || valid.innerHTML == i18n.t("numberDocumentRequired") || valid.innerHTML == i18n.t("formatoInvalido", [mascara])) {
        if (doc.value == null || doc.value == "") {
            valid.innerHTML = i18n.t("numberDocumentRequired")
            return true
        } else if (!regExp.test(doc.value)) {
            valid.innerHTML = i18n.t("formatoInvalido", [mascara])
            return true
        } else {
            valid.innerHTML = "";
            return false
        }

    }

    return false
}

function validarEmailLogin() {

    let emailInput = document.getElementById('exampleInputEmail');//Caja del Email
    let validEmailLogin = document.getElementById('validEmail');
    let RE = /[.]{2,}/
    let emailRegex = /^[\w.]{1,64}@(?:[A-Z0-9]{2,63}\.){1,125}[A-Z]{2,63}$/i;
    let language = document.getElementsByTagName('html')[0].getAttribute('lang');

    if (language == "ESS" || language == "ESV" || language == "es") {

        if (emailInput.value == null || emailInput.value == "") {
            validEmailLogin.innerHTML = "El Correo Electrónico es requerido";
            return
        } else if (!emailRegex.test(emailInput.value) || RE.test(emailInput.value)) {
            validEmailLogin.innerHTML = "El formato de Correo Electrónico es inválido: goc_08@hotmail.com";
            return
        } else {
            validEmailLogin.innerHTML = "";
            return
        }

    } else if (language == "ENU" || language == "en") {

        if (emailInput.value == null || emailInput.value == "") {
            validEmailLogin.innerHTML = "Email is required";
            return
        } else if (!emailRegex.test(emailInput.value) || RE.test(emailInput.value)) {
            validEmailLogin.innerHTML = "The Email format is invalid: goc_08@hotmail.com";
            return
        } else {
            validEmailLogin.innerHTML = "";
            return
        }

    }

}

function validarPasswordLogin() {

    let passwordInput = document.getElementById('exampleInputPassword');//Caja del Password
    let validPasswordInput = document.getElementById('validPasswordLogin');
    
    if (passwordInput.value == null || passwordInput.value == "") {
        validPasswordInput.innerHTML = i18n.t("passwordRequired")
        return
    } else {
        validPasswordInput.innerHTML = "";
        return
    }

   

}

function validarPrefix() {
    try {
        let selectPrefixList = document.getElementById('selectPrefix');//Caja del Select
        let validPrefix = document.getElementById('validNum');

        if (validPrefix.innerHTML == "" || validPrefix.innerHTML == i18n.t("typeIdentificationRequired")) {
            if (selectPrefixList.value == null || selectPrefixList.value == "" || selectPrefixList.value == "selectId") {
                validPrefix.innerHTML = i18n.t("typeIdentificationRequired");
                return
            } else if (validPrefix.innerHTML == i18n.t("typeIdentificationRequired")) {
                validPrefix.innerHTML = "";
                //buttonSubmit.attr('disabled', false)
                return
            }
        }
    } catch (e) { }
   

}
//Final Funciones Login

//Funciones Forgot Password

function forgotPasswordRegExpr(expreReg, mascara) {

    let regExp = new RegExp(expreReg);
    let doc = document.getElementById('documentForgotPassword');//Input_Number
    let valid = document.getElementById('validDocumentForgotPassword');
    let language = document.getElementsByTagName('html')[0].getAttribute('lang');

    //Valido con un Div si el Discriminator es PERSON para no correr la funcion de abajo
    if ($('#personaNaturalNumber').is(':visible')) {
        return
    }

    if (language == "ESS" || language == "ESV" || language == "es") {

        if (doc.value == null || doc.value == "") {
            valid.innerHTML = "El Número de Documento es requerido";
            return true
        } else if (!regExp.test(doc.value)) {
            valid.innerHTML = "El Formato es inválido (" + mascara + ")";
            return true
        } else {
            valid.innerHTML = "";
            return false
        }

    } else if (language == "ENU" || language == "en") {

        if (doc.value == null || doc.value == "") {
            valid.innerHTML = "Document Number is required";
            return true
        } else if (!regExp.test(doc.value)) {
            valid.innerHTML = "Format is invalid (" + mascara + ")";
            return true
        } else {
            valid.innerHTML = "";
            return false
        }
    }

    return false
}

function emailForgotPassword() {

    let inputEmail = document.getElementById('exampleInputEmail')
    let validEmail = document.getElementById('validEmailForgotPassword')
    let language = document.getElementsByTagName('html')[0].getAttribute('lang');
    let RE = /[.]{2,}/
    let emailRegex = /^[\w.]{1,64}@(?:[A-Z0-9]{2,63}\.){1,125}[A-Z]{2,63}$/i;
    let botonRestablecer = $('#botonRestablecerPassword')

    if (language == "ESS" || language == "ESV" || language == "es") {

        if (inputEmail.value == null || inputEmail.value == "") {
            validEmail.innerHTML = "El Correo Electrónico es requerido";
            botonRestablecer.attr('disabled', true)
            return
        } else if (!emailRegex.test(inputEmail.value) || RE.test(inputEmail.value)) {
            validEmail.innerHTML = "El formato de Correo Electrónico es inválido: goc_08@hotmail.com";
            botonRestablecer.attr('disabled', true)
            return
        } else {
            validEmail.innerHTML = "";
            botonRestablecer.attr('disabled', false)
            return
        }

    } else if (language == "ENU" || language == "en") {

        if (inputEmail.value == null || inputEmail.value == "") {
            validEmail.innerHTML = "Email is required";
            botonRestablecer.attr('disabled', true)
            return
        } else if (!emailRegex.test(inputEmail.value) || RE.test(inputEmail.value)) {
            validEmail.innerHTML = "The Email format is invalid: goc_08@hotmail.com";
            botonRestablecer.attr('disabled', true)
            return
        } else {
            validEmail.innerHTML = "";
            botonRestablecer.attr('disabled', false)
            return
        }

    }
}

//Final Funciones Forgot Password

//Funciones Reset Password
function validarNewPassword() {
    let newPassword = document.getElementById('newPasswordReset')
    let validNewPassword = document.getElementById('validNewPassword');
    let language = document.getElementsByTagName('html')[0].getAttribute('lang');

    if (language == "ESS" || language == "ESV" || language == "es") {

        if (newPassword.value == null || newPassword.value == "") {
            validNewPassword.innerHTML = "La Nueva Contraseña es requerida";
            return true
        } else {
            validNewPassword.innerHTML = "";
            return false
        }

    } else if (language == "ENU" || language == "en") {

        if (newPassword.value == null || newPassword.value == "") {
            validNewPassword.innerHTML = "The New Password required";
            return true
        } else {
            validNewPassword.innerHTML = "";
            return false
        }
    }

}

function validarNewConfirmPassword() {
    let newPassword = document.getElementById('newConfirmPasswordReset')
    let validNewPassword = document.getElementById('validNewConfirmPassword');
    let language = document.getElementsByTagName('html')[0].getAttribute('lang');

    if (language == "ESS" || language == "ESV" || language == "es") {

        if (newPassword.value == null || newPassword.value == "") {
            validNewPassword.innerHTML = "La Nueva Contraseña y la Confirmación no coinciden";
            return true
        } else {
            validNewPassword.innerHTML = "";
            return false
        }

    } else if (language == "ENU" || language == "en") {

        if (newPassword.value == null || newPassword.value == "") {
            validNewPassword.innerHTML = "New Password and Confirmation do not match";
            return true
        } else {
            validNewPassword.innerHTML = "";
            return false
        }
    }
}

//Final Funciones Reset Password
function marginCarrusel() {

    if (document.documentElement.clientWidth < 980) document.getElementById('carouselExampleIndicators').style.marginTop = "3em"
    else document.getElementById('carouselExampleIndicators').style.marginTop = "0"
}

function resize() {

    let sizeScreem = document.getElementById('carouselExampleIndicators');
    if (sizeScreem != null) {

        marginCarrusel();
    }
}


async function  pedirTokenRecaptcha(action) {
    var result = true
    
    await grecaptcha.execute('6LexhPMUAAAAAMc2tL-GP1Sc7-UL-TvnfmZlNtIk', { action: action }).then(function (token) {
        document.getElementById("idrecaptcha").value = token
        result = false
    })
    

    return result
}


async function llamadaRecursiva(continuar, llenar,indice) {
    if (continuar) {
        setTimeout(async function () {
            llamadaRecursiva(await llenar(indice), llenar,indice)
        }, 1000 * 2)
    }
}

function validarPrimeraVezInput(inputID, spanID) {
    //para cuando se usan las validaciones de c#, activar el required la primera vez
    var input = document.getElementById(inputID)
    var span = document.getElementById(spanID)

    if (input.value == "" && span.innerHTML == "") {
        span.innerHTML = input.getAttribute("data-val-required")
    }
}

function blockMenu() {
    if (document.getElementById("menuDesplegable") != null) document.getElementById("menuDesplegable").removeAttribute('hidden');
    if (document.getElementById("menuPublicar") != null) document.getElementById("menuPublicar").classList.remove("disabledMenu");
    if (document.getElementById("menuVencimiento") != null) document.getElementById("menuVencimiento").classList.remove("disabledMenu");
    if (document.getElementById("menuSubastas") != null) document.getElementById("menuSubastas").classList.remove("disabledMenu");
    if (document.getElementById("menuCierre") != null) document.getElementById("menuCierre").classList.remove("disabledMenu");
    if (document.getElementById("menuFacturas") != null) document.getElementById("menuFacturas").classList.remove("disabledMenu");
}


async function tokenSubmit(e) {
    try {
        await pedirTokenRecaptcha("login")

        var eventoSubmit = new MouseEvent('click')

        document.getElementById("buttonSubmit2").dispatchEvent(eventoSubmit)
    } catch (e) {
        toastr.error(i18n.t("mensajesModal.estimadoUsuario") + ".<br>" + i18n.t("notConexionDetected") +"<br>"+i18n.t("verifiqueNuevoIntento"))
    }
}

async function tokenSubmitEnter(e) {
    
    if (e.keyCode == 13) {
        e.preventDefault();
        await tokenSubmit(e)
    } 
}


function tiempoLogin(modal) {
    modalSession = modal
    setInterval(function () {
        let maxTiempo = 1000 * 60 * timeLogout
        let tiempoLessLogoutTotal = 15
        if (timeCurrentLogout2 == maxTiempo) {
            modal.mostrar = true
            timeLessLogout++
            if (timeLessLogout == tiempoLessLogoutTotal) {
                window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired";
            }
        } else {
            timeCurrentLogout2 += 1000
        }
    },1000)
}

function resetTime() {
    timeCurrentLogout2 = 0
}

function tiempoAusente(tiempo=20) {
    let tiempoTranscurrido = 0;
    let tiempoMaximo = 60 * tiempo;

    setInterval(function () {
        tiempoTranscurrido++
        if (tiempoTranscurrido == tiempoMaximo) {
            window.location.href = "./Logout?returnUrl=~/index";
        }

    }, 1000)
}

async function refrescarSession() {

    await axios.post('../Profile/MantenimientoUsuarios?handler=ping', {},
        {
            headers: {
                "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
            }
        })
        .then((respond) => {
            
            if (typeof respond === 'string' || respond instanceof String) {
                if (respond == notAuthorized) {
                    window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                    return
                } else if (respond.includes("<!DOCTYPE html>")) {
                    console.log(respond)
                    window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired";
                    toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br>" + i18n.t("errorBaseDatos"));
                    return;
                }
            }
            
            if (respond.data) {
                timeLessLogout = 0
                timeCurrentLogout2 = 0
                modalSession.mostrar = false

            }
        }).catch((respond) => { console.log(respond); });
}


function filtrarPublicationsCurrency(publications, currency) {
    return publications.filter(publication => publication.currency.id == currency)
}

function removeItemFromArr(arr, item){
    var i = arr.indexOf(item);
    if (i !== -1) {
        arr.splice(i, 1);
    }

    return arr
};
//facturas.length == 0 &&
function arrayCondition(array, condicion) {
    return array.filter(data => data == condicion).length == array.length
}


window.onresize = resize;
window.onload = mensajeria();
