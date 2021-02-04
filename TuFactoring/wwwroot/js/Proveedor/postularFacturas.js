new Vue({
    el: '#appPostularFactura',
    i18n,
    vuetify: new Vuetify({
        lang: {
            t: (key, ...params) => i18n.t(key, params)
        }
    }),
    data: {
        filtersIsEmpty: filtersIsEmpty,
        filterIsEmpty: filterIsEmpty,
        arrayCondition: arrayCondition,
        currencies: {},
        modalLogout: { mostrar: false },
        dialogAyuda: false,
        options: [],
        idBancoActual: '0',
        objetoCurrency: {
            iso_4217: "",
            symbol: "",
            id: ""
        },
        buscarFacturas: [],
        entityArray: {
            bank: {
                person: {
                    name
                }
            }

        },
        dataAccounts: [],
        valorAcumulado: 0,
        bankSelected: false,
        totalFacturas: [],
        widthTelefono: widthTelefono,
        filter: [],
        page: 1,
        errorAcumulated: false,
        itemsPerPage: 9,
        perPage: 9,
        itemsPerPageOptions: [3, 9, 18, -1],
        tamanoTlf: tamanoTlf,
        headers: [
            { text: i18n.t("headers.check"), value: "check", align: "center", sortable: false },
            { text: i18n.t("headers.n"), value: "n", align: "center" },
            { text: i18n.t("headers.proveedor"), value: "supplier.name", align: "center" },
            { text: i18n.t("headers.numeroFactura"), value: "number", align: "center" },
            { text: i18n.t("headers.fechaVencimiento"), value: "expiration_date", align: "center" },
            { text: i18n.t("headers.valorNeto"), value: "amount", align: "center" },
            { text: i18n.t("headers.opciones"), value: "opciones", align: "center" }
        ],
        headers2: [
            { text: i18n.t("headers.cliente"), value: "debtor.name", align: "center" },
            { text: i18n.t("headers.numeroFactura"), value: "number", align: "center" },
            { text: i18n.t("headers.fechaVencimiento"), value: "expiration_date", align: "center" },
            { text: i18n.t("headers.originalAmount"), value: "original_amount", align: "center" },
            { text: i18n.t("headers.valorNeto"), value: "amount", align: "center" },
            { text: i18n.t("headers.opciones"), value: "opciones", align: "center" }
        ],
        dialogPostular: false,
        moment: moment,
        formatoMonedaInput: formatoMonedaInput,
        backEndDateFormat: backEndDateFormat,
        lang: "es",
        digits: 2,
        cargando: true,
        envio: false,
        quotaAvailabe: 0,
        quotaUsage: 0,
        pageCount: 10,
        pageCount2: 5,
        page: 1,
        page2: 1,
        chekeado: false,
        tipoMoneda: 0,
        facturasFiltradas: [],
        filtroProveedor: '',
        errorPostular: false,
        errorEnviar: false,
        facturas: [],
        dataAccounts: [],
        nuevo: [],
        indice: -1,
        catalogo: [],
        quotaSymbol: '',
        quotaIso: '',
        Bancos: [],
        idBanco: [],
        monedaQuota: {
            symbol: '0',
            iso_4217: '0'
        },
        totalDeducciones: 0,
        proveedores: [],
        postular: [],
        selected: [],
        ckeckAll: false,
        loading: []
    },
    created: function () {
        this.cargando = true;

        try {
            this.currencies = JSON.parse(document.getElementById('currenciesData').value);
            document.getElementById("eliminarData").removeChild(document.getElementById('currenciesData'));

            for (var i = 0; i < this.currencies.length; i++) {
                this.options.push({})
                this.buscarFacturas.push(true)
                this.loading.push(false)
                this.totalFacturas.push(0)
                this.idBanco.push('0')


                this.filter[i] = JSON.parse(document.getElementById('filterData+' + i).value)
                this.filter[i].currency = this.currencies[i].id
                document.getElementById("eliminarData").removeChild(document.getElementById('filterData+' + i))
            }
        } catch (e) {
            for (var i = 0; i < this.currencies.length || i < 1; i++) {
                this.options.push({})
                this.buscarFacturas.push(true)
                this.loading.push(false)
                this.totalFacturas.push(0)
                this.idBanco.push('0')

                this.filter[0] = null
            }
        }

        document.getElementById("appPostularFactura").removeAttribute("hidden")
        this.lang = document.getElementsByTagName("html")[0].getAttribute("lang")
        this.cargando = false;
    },
    mounted: async function () {
        tiempoLogin(this.modalLogout)
        for (var i = 0; i < this.currencies.length; i++) {
            await this.llenarPostulates(i);
        }

        await this.llenarCatalogo()
        await this.llenarConfirmants()
        await this.getAccounts()
        setTimeout(() => iniciarButtonFilters(), 500)
    },
    watch: {
        options: {
            async handler() {
                for (var i = 0; i < this.currencies.length; i++) {
                    if (this.options[i].itemsPerPage == -1) {
                        llamadaRecursiva(this.buscarFacturas[i], this.llenarPostulates, i)
                        return
                    }

                    var tamanoFacturas = filtrarPublicationsCurrency(this.facturasFiltradas, this.currencies[i].id).length

                    if (this.buscarFacturas[i] && this.options[i].page * this.options[i].itemsPerPage >= tamanoFacturas - this.options[i].itemsPerPage) {
                        await this.llenarPostulates(i)
                    }
                }
            },
            deep: true,
        },
    },
    methods: {
        async llenarPostulates(indice) {
            if (this.loading[indice]) return
            this.loading[indice] = true

            var take = 100
            await axios.post('?handler=postulates', { pagination: { take: take, skip: this.totalFacturas[indice] }, filter: this.filter[indice] },
                {
                    headers: {
                        "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                    }
                })
                .then((respond) => {
                    resetTime()
                    this.loading[indice] = false
                    if (respond.data === null) {
                        this.buscarFacturas[indice] = false
                        return
                    }

                    if (typeof respond === 'string' || respond instanceof String) {

                        if (respond.includes("<!DOCTYPE html>")) {
                            window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired";
                            toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br>" + i18n.t("errorBaseDatos"));
                            return;
                        }
                    }

                    if (respond.data.length > 0 && respond.data[0].errors == notAuthorized) {
                        window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                        return
                    }
                    if (respond.data.length < take / 2)
                        this.buscarFacturas[indice] = false

                    this.totalFacturas[indice] += respond.data.length

                    respond.data.map(data => {
                        if (data == null) return

                        for (var i = 0; i < this.facturas.length; i++) {
                            if (this.facturas[i].id == data.id) {
                                this.totalFacturas--
                                return
                            }
                        }

                        if (data != null) {
                            data.expiration_date = moment(data.expiration_date.substr(0, 10), 'MM/DD/YYYY').format("DD/MM/YYYY")

                            this.facturas.push(data)
                        }
                    })

                    this.facturas.sort((a, b) => a.supplier.name.toLowerCase() < b.supplier.name.toLowerCase() ?
                        -1 : +a.supplier.name.toLowerCase() > b.supplier.name.toLowerCase())

                    this.facturasFiltradas = this.facturas

                    this.facturasFiltradas.sort((a, b) => a.supplier.name.toLowerCase() < b.supplier.name.toLowerCase() ?
                        -1 : +a.supplier.name.toLowerCase() > b.supplier.name.toLowerCase())

                }).catch((respond) => { console.log(respond); this.loading[indice] = false });

            return (this.buscarFacturas[indice] && this.options[indice].itemsPerPage == -1)
        },
        //
        async llenarConfirmants() {
            await axios.post('?handler=confirmants', {},
                {
                    headers: {
                        "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                    }
                })
                .then((respond) => {
                    resetTime()
                    if (respond.data === null) {
                        this.Bancos = [];
                        return
                    }

                    if (typeof respond === 'string' || respond instanceof String) {

                        if (respond.includes("<!DOCTYPE html>")) {
                            window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired";
                            toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br>" + i18n.t("errorBaseDatos"));
                            return;
                        }
                    }

                    if (respond.data.length > 0 && respond.data[0].errors == notAuthorized) {
                        window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                        return
                    }

                    this.Bancos = respond.data;
                    this.Bancos.sort((a, b) => a.person.name.toLowerCase() < b.person.name.toLowerCase() ? -1 : (a.person.name.toLowerCase() > b.person.name.toLowerCase()))

                }).catch((respond) => { console.log(respond); });
        },
        //
        async getAccounts() {
            await axios.post('?handler=Accounts', {},
                {
                    headers: {
                        "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                    }
                })
                .then((respond) => {
                    resetTime()
                    if (respond.data === null) {
                        this.dataAccounts = [];
                        return
                    }

                    if (typeof respond === 'string' || respond instanceof String) {

                        if (respond.includes("<!DOCTYPE html>")) {
                            window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired";
                            toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br>" + i18n.t("errorBaseDatos"));
                            return;
                        }
                    }

                    if (respond.data.length > 0 && respond.data[0].errors == notAuthorized) {
                        window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                        return
                    }

                    this.dataAccounts = respond.data;

                }).catch((respond) => { console.log(respond); });
        },
        //
        async llenarCatalogo() {
            await axios.post('?handler=catalogo', {},
                {
                    headers: {
                        "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                    }
                })
                .then((respond) => {
                    resetTime()
                    if (respond.data == null) {
                        this.tipoMoneda = [];
                        return
                    }

                    if (respond.data.length > 0 && respond.data[0].errors == notAuthorized) {
                        window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                        return
                    }


                    this.catalogo = JSON.parse(respond.data)

                    this.tipoMoneda = this.catalogo.currencies;

                    if (this.dataAccounts != null) {
                        for (let w = 0; w < this.dataAccounts.length; w++) {
                            for (let y = 0; y < this.catalogo.currencies.length; y++) {
                                if (this.dataAccounts[w].currency == this.catalogo.currencies[y].id) {
                                    this.quotaIso = this.tipoMoneda[y].iso_4217
                                    this.quotaSymbol = this.tipoMoneda[y].symbol
                                    this.quotaDigits = this.tipoMoneda[y].digits
                                }
                            }
                        }
                    }


                    if (this.catalogo.length == 0) return

                }).catch((respond) => { console.log(respond); });
        },
        //
        methodPublicationsCurrency(idCurrency) {
            //Ordenamiento previo a ser Filtrado por Currency            
            return filtrarPublicationsCurrency(this.facturas, idCurrency)
        },
        //
        obtenerBancos(idCurrency) {
            let arreglo = []
            for (let x = 0; x < this.Bancos.length; x++) {
                for (let y = 0; y < this.dataAccounts.length; y++) {
                    if (this.Bancos[x].id == this.dataAccounts[y].entity.id && this.dataAccounts[y].currency == idCurrency) {
                        arreglo.push(this.Bancos[x])
                    }
                }

            }

            let bankArray = new Set(arreglo);

            if (this.dataAccounts != null) {
                for (let w = 0; w < this.dataAccounts.length; w++) {
                    if (bankArray.length == 0 && this.dataAccounts[w].currency == idCurrency) {
                        this.entityArray.bank.person.name = i18n.t("seleccione")
                        return this.entityArray
                    }
                }
            }

            if (bankArray.length == 0) {
                this.entityArray.bank.person.name = i18n.t("seleccione")
                return this.entityArray
            }

            return bankArray
        },
        calcularSeleccionadas: function (currency, index) {
            let quotaUsage = 0
            let quotaAvailable = 0
            let quotaDisponible = 0

            if (this.dataAccounts != null) {
                for (let w = 0; w < this.dataAccounts.length; w++) {
                    if (this.dataAccounts[w].entity.id == this.idBanco[index] && currency == this.dataAccounts[w].currency) {
                        quotaUsage = this.dataAccounts[w].usage
                        quotaAvailable = this.dataAccounts[w].available
                    }
                }
            }

            quotaDisponible = quotaAvailable - quotaUsage

            return quotaDisponible - this.acumuladorValores(currency)

        },
        //
        checkAll2: function (indice) {
            try {
                var include = false
                var data = filtrarPublicationsCurrency(this.facturasFiltradas, this.currencies[indice].id)

                for (var i = 0; i < data.length; i++) {

                    if (!this.selected.includes(this.facturasFiltradas.indexOf(data[i]))) {
                        this.selected.push(this.facturasFiltradas.indexOf(data[i]))
                        include = true
                    }
                }

                if (include) {
                    for (var w = 0; w < data.length; w++) {
                        if ($('#collapseCardExample' + indice + '-' + w).hasClass("show")) {
                            $("#collapseCardExample" + indice + "-" + w).removeClass("show")
                            $("#collapseCardExample" + indice + "-" + w).addClass("show")
                        } else {
                            $("#collapseCardExample" + indice + "-" + w).addClass("show")
                        }
                    }
                } else {
                    for (var w = 0; w < data.length; w++) {
                        if ($("#collapseCardExample" + indice + "-" + w).hasClass("show")) {
                            $("#collapseCardExample" + indice + "-" + w).removeClass("show")
                        } else {
                            $("#collapseCardExample" + indice + "-" + w).addClass("collapse")
                        }
                        this.selected = removeItemFromArr(this.selected, this.facturasFiltradas.indexOf(data[w]))
                    }
                }
            } catch (e) {
                console.log(e)
            }
        },
        //
        /*financiedAll2: function (indice) {

            var data = filtrarPublicationsCurrency(this.facturasFiltradas, this.currencies[indice].id)


            if (this.financiedAll(indice) == false) {
                for (var i = 0; i < data.length; i++) {
                    data[i].request_financing = true
                }
            } else {
                for (var i = 0; i < data.length; i++) {
                    data[i].request_financing = false
                }
            }
        },*/
        //
        acumuladorValores: function (idCurrency) {
            let acumulatedInvoices = []
            let acumulador = 0

            for (let i = 0; i < this.selected.length; i++) {
                acumulatedInvoices.push(this.facturasFiltradas[this.selected[i]])
            }

            acumulatedInvoices = filtrarPublicationsCurrency(acumulatedInvoices, idCurrency)

            acumulatedInvoices.map(data => {
                acumulador = acumulador + data.amount
            })

            return acumulador
        },
        //
        cancelarAsignacion: function (indice, idCurrency) {
            this.valorAcumulado = this.valorAcumulado - this.nuevo[indice].amount
            this.nuevo.splice(indice, 1);
        },
        // Pasa las facturas al segundo arreglo para asignar un Banco y Postular
        posiblesFacturas: function (currency, index) {
            this.errorPostular = false;
            this.objetoCurrency.iso_4217 = currency.iso_4217
            this.objetoCurrency.symbol = currency.symbol
            this.valorAcumulado = 0
            this.objetoCurrency.id = currency.id
            this.nuevo = []
            if (this.idBanco[index] === '0' || this.selected.length === 0) {
                this.errorPostular = true;
                return
            }
            else {
                for (var i = 0; i < this.selected.length; i++) {
                    if (this.facturasFiltradas[this.selected[i]].currency.id == this.objetoCurrency.id) {
                        this.nuevo.push(this.facturasFiltradas[this.selected[i]]);
                    }
                }
                this.nuevo.sort((a, b) => a.supplier.name.toLowerCase() < b.supplier.name.toLowerCase() ?
                    -1 : +a.supplier.name.toLowerCase() > b.supplier.name.toLowerCase())
            }

            this.nuevo.map(data => {
                this.valorAcumulado = this.valorAcumulado + data.amount
            })
            this.idBancoActual = this.idBanco[index]
            $("#myModal").modal("show")
        },
        //
        confirmarPostuladas: async function () {
            //OnPost
            for (var i = 0; i < this.nuevo.length; i++) {
                this.nuevo[i].publication = { entity: { id: this.idBancoActual } };
            }
            let data = {
                nuevo: this.nuevo
            }

            await this.postularFacturas(data);

            $("#myModal").modal("hide")
            this.nuevo = [];
        },
        //
        limpiarPostuladas: function () {
            this.selected = []
            this.valorAcumulado = 0
            for (let i = 0; i < this.idBanco.length; i++) {
                this.idBanco[i] = '0'
            }

            for (let i = 0; i < this.facturasFiltradas.length; i++)
                this.facturasFiltradas[i].request_financing = false
        },
        //
        financiar: function (tab, indice) {

            if (this.facturasFiltradas[indice].term_days == 0) {

                toastr.info(i18n.t("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.t("mensajesModal.notFinancingTerm"))

                return
            }

            this.facturasFiltradas[indice].request_financing = !this.facturasFiltradas[indice].request_financing
        },
        //
        nombreBanco: function (id) {
            for (var i = 0; i < this.Bancos.length; i++) {
                if (this.Bancos[i].id == id) {
                    return this.Bancos[i].person.name;
                }
            }
            return 'Sin Nombre';
        },
        //
        filtrar() {
            if (this.facturas == []) return

            if (this.filtroProveedor == '') {
                this.facturasFiltradas = this.facturas
                this.ordenarLista()
                this.checks()

                return
                return
            }

            this.facturasFiltradas = this.facturas.filter(data => data.supplier.name.toLowerCase().includes(this.filtroProveedor.toLowerCase()))

            this.ordenarLista()
            this.checks()
            this.page = 1
        },
        //
        ordenarLista() {
            this.facturas.sort((a, b) => a.supplier.name.toLowerCase() < b.supplier.name.toLowerCase() ?
                -1 : +a.supplier.name.toLowerCase() > b.supplier.name.toLowerCase())
        },
        checks: function () {
            var selected = [];

            for (let i = 0; i < this.facturasFiltradas.length; i++) {
                if (this.chekeado) {
                    selected.push(i)
                    continue
                }

                for (let t = 0; t < this.selected.length; t++) {
                    if (this.facturasFiltradas[i].id == this.facturas[this.selected[t]].id) {
                        selected.push(i)
                        break
                    }
                }
            }
            this.selected = selected.sort((a, b) => b - a);
        },

        /*callbackQuota: async function () {
            await this.detailQuota()
        },*/

        checkAll(index) {
            var data = filtrarPublicationsCurrency(this.facturasFiltradas, this.currencies[index].id)

            if (data.length == 0) return false

            var includeTotal = true
            var selected = this.selected
            var facturas = this.facturasFiltradas

            data.map(a => {
                if (!selected.includes(facturas.indexOf(a))) {
                    includeTotal = false
                }
            })

            return includeTotal
        },

        financiedAll(index) {

            var data = filtrarPublicationsCurrency(this.facturasFiltradas, this.currencies[index].id)

            if (data.length == 0) return false

            var result = true

            for (let w = 0; w < data.length; w++) {
                if (data[w].request_financing == false) {
                    result = false
                }
            }
            return result
        },

        //*****************************************************************************
        postularUnisono: async function (item, index) {
            if (this.idBanco[index] == null || this.idBanco[index] == "" || this.idBanco[index] == 0) {
                return
            }

            item.publication = { entity: { id: this.idBanco[index] } }

            let arrayEnviar = []

            arrayEnviar.push(item)

            let data = {
                nuevo: arrayEnviar//,
                // facturas: this.facturas
            }

            await this.postularFacturas(data)
        },
        //*****************************************************************************
        postularFacturas: async function (facturas) {
            if (this.envio) return

            this.envio = true

            axios.post('PostularFacturas?handler=postular', facturas.nuevo,
                {
                    headers: {
                        "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                    }
                })
                .then((respond) => {
                    resetTime()
                    this.envio = false
                    if (respond.data != null)
                        respond = respond.data


                    if (typeof respond === 'string' || respond instanceof String) {

                        if (respond.includes("<!DOCTYPE html>")) {
                            window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired";
                            toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br>" + i18n.t("errorBaseDatos"));
                            return;
                        }
                    }


                    let estado = {
                        fallos: 0,
                        status: false,
                        error: true,
                        errorFinanciar: true
                    }



                    for (var t = 0; t < respond.length; t++) {
                        if (respond[t].errors != null) {
                            if (respond[t].errors == "Error al financiar") {
                                if (estado.errorFinanciar) {
                                    toastr.warning(i18n.tc("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.t("errorFinanciar"))
                                    estado.errorFinanciar = false
                                }

                                this.quitarFactura(respond[t])
                                continue
                            } else if (respond[t].errors == "supplier has not been verifier" || respond[t].errors == "supplier has not accepted terms") {
                                if (estado.error) {
                                    toastr.warning(i18n.tc("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.tc("mensajesModal.errorSupplierNotVerified"))
                                    estado.error = false
                                }

                                continue
                            } else if (respond[t].errors == "supplier has not accepted membership with cnfirmant") {
                                if (estado.error) {
                                    toastr.warning(i18n.tc("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.tc("mensajesModal.errorSupplierNotContract"))
                                    estado.error = false
                                }

                                continue
                            } else if (respond[t].errors == "debtor has not accepted terms" || respond[t].errors == "debtor don't have accounts with entity" ||
                                respond[t].errors == "debtor has not been verifier") {
                                if (estado.error) {
                                    toastr.warning(i18n.tc("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.tc("mensajesModal.errorDebtorAccountWithEntity"))
                                    estado.error = false
                                }

                                continue
                            } else if (respond[t].errors == "quotas not available") {
                                if (estado.error) {
                                    toastr.warning(i18n.tc("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.tc("mensajesModal.errorRiskLimitNotAvailableClient"))
                                    estado.error = false
                                }

                                continue
                            } else if (respond[t].errors == "quota not found") {
                                if (estado.error) {
                                    toastr.warning(i18n.tc("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.tc("mensajesModal.errorRiskLimitNotFoundClient"))
                                    estado.error = false
                                }
                                continue
                            } else if (respond[t].errors == notAuthorized) {
                                window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                                return
                            } else if (respond[t].errors == "supplier has not accepted invitation") {
                                if (estado.error) {
                                    toastr.warning(i18n.tc("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.tc("mensajesModal.errorSupplierNotAcceptedInvitationClient"))
                                    estado.error = false
                                }
                                continue
                            } else if (respond[t].errors == "field [terms_days] not valid") {
                                if (estado.error) {
                                    toastr.warning(i18n.tc("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.tc("mensajesModal.postulateInvoicesLower30"))
                                    estado.error = false
                                }
                                continue
                            } else if (respond[t].errors == "entity don't have accounts") {
                                if (estado.error) {
                                    toastr.warning(i18n.tc("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.tc("mensajesModal.errorEntityDontHaveAccount"))
                                    estado.error = false
                                }
                                continue
                            } else if (respond[t].errors == "backOffice don't have accounts with entity") {
                                if (estado.error) {
                                    toastr.warning(i18n.tc("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.tc("mensajesModal.errorBackOfficeDontHaveAccountPostulation"))
                                    estado.error = false
                                }
                                continue
                            }
                            else if (respond[t].errors == "supplier don't have accounts") {
                                if (estado.error) {
                                    toastr.warning(i18n.tc("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.tc("mensajesModal.errorSupplierDontHaveAccount", 2, { 0: respond[t].name }))
                                    estado.error = false
                                }

                                continue
                            }


                            estado.fallos++
                        } else {
                            this.quitarFactura(respond[t])

                            estado.status = true
                        }

                    }

                    if (estado.status) {
                        if (estado.fallos == 0) {
                            toastr.success(i18n.t("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.t("mensajesModal.postularFactura"))
                            //this.callbackQuota()
                        }
                    } else {
                        if (estado.fallos > 0) {
                            toastr.warning(i18n.tc("mensajesModal.problemasRespuesta", 2, { 0: "postular facturas" }))
                        }
                    }
                    this.limpiarPostuladas();
                }).catch((respond) => { toastr.error(i18n.t("errorRespuesta")); console.log(respond); this.envio = false; this.limpiarPostuladas(); });
        },
        //*****************************************************************************
        quitarFactura: async function (factura) {
            for (let i = 0; i < this.facturasFiltradas.length; i++) {

                if (this.facturasFiltradas[i].id == factura.id) {
                    this.facturasFiltradas.splice(i, 1)
                    for (let t = 0; t < this.selected.length; t++) {

                        if (this.selected[t] == i) {
                            this.selected.splice(t, 1)

                            for (var w = 0; w < this.selected.length; w++) {
                                if (this.selected[w] > i)
                                    this.selected[w]--
                            }
                            break
                        }
                    }
                    break
                }
            }

            for (let i = 0; i < this.facturas.length; i++) {
                if (this.facturas[i].id == factura.id) {
                    this.facturas.splice(i, 1)
                    break
                }
            }
        },
        activarPostulacion(index) {
            return (this.idBanco[index] === '0' || this.idBanco[index] === '' || this.selected.length === 0)
        },
    },

    computed: {
        numberOfPages() {
            return Math.ceil(this.facturasFiltradas.length / this.itemsPerPage)
        },
        filteredKeys() {
            return this.keys.filter(key => key !== `Name`)
        },
        //
        //marcar todos los registros de la tabla
        /*checkAll: {
            get: function () {
                return (this.selected.length == this.facturasFiltradas.length && this.facturasFiltradas.length > 0)
            },
            set: function (value) {
                var selected = [];
                this.chekeado = !this.chekeado
                if (value) {
                    for (var i = 0; i < this.facturasFiltradas.length; i++) {
                        if (this.filtroProveedor == '' || this.facturasFiltradas[i].supplier_id == this.filtroProveedor)
                            selected.push(i);

                    }
                }
                this.selected = selected.sort((a, b) => b - a);
            }
        },*/
        /*financiedAll: {
            get: function () {
                var result = true

                this.facturasFiltradas.map(data => { if (!data.request_financing) result = false})

                return result
            },
        },*/


    },
});