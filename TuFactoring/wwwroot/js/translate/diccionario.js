var langActual = document.getElementsByTagName("html")[0].getAttribute("lang")
var digitsActual = 2

switch (langActual) {
    case "ENU":  break;
    case "ESS": 
    case "ESD":
    case "ESV":  break;
    default: langActual = "ESS"
}


const messages = {
    ENU: idiomaEN,
    ESS: idiomaES,
    ESV: idiomaESV,
    ESD: idiomaESD,
}

const i18n = new VueI18n({
    locale: langActual,
    messages, 
})

/*
var miImg = document.getElementsByTagName("html")[0],
    miMO = new MutationObserver(callbackMO);

miMO.observe(miImg, {
    attributes: true,
    attributeOldValue: true,
    attributeFilter: ['lang']
});

function callbackMO(mutations) {
    mutations.forEach((mutation) => {
            i18n.locale = document.getElementsByTagName("html")[0].getAttribute("lang")
    });
}

*/