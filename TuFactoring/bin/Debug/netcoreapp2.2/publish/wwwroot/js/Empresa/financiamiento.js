new Vue({
    el: "#app",
    i18n,
    store: vuexLayout,
    vuetify: new Vuetify({
        lang: {
            t:(key,...params) => i18n.t(key,params)
        }
    }),
    data: {
        filter: {},
        totalFacturas: [],
        buscarFacturas: [],
        loading: [],
        options: [],
        indice:0,
        arrayCondition: arrayCondition,
        currencies: {},
        modalLogout: { mostrar: false },
        dialogAyuda:false,
        estado: true,
        indexActual: 0,
        perPage: 9,
        itemsPerPageOptions: [3,9,18,-1],
        tamanoTlf: tamanoTlf,
        dialogSolicitar: false,
        headers: [
            { text: i18n.t("headers.n"), value: "n", align: "center" },
            { text: i18n.t("headers.proveedor"), value: "supplier.name", align: "center" },
            { text: i18n.t("headers.numeroFactura"), value: "number", align: "center" },
            { text: i18n.t("headers.fechaVencimiento"), value: "expiration_date", align: "center" },
            { text: i18n.t("headers.valorNeto"), value: "amount", align: "center" },
            { text: i18n.t("headers.banco"), value: "publication.entity.person.name", align: "center" },
            { text: i18n.t("headers.financiamiento"), value: "financiamiento", align: "center" }
        ],
        menu: false,
        Mensajeria: {
            notificaciones: true,
            mensajes: true
        },
        cerrarMordisco: true,
        dialogSolicitarCancelacion:false,
        hints: true,
        fav: true,
        message: false,
        moment: moment,
        formatoMonedaInput: formatoMonedaInput,
        backEndDateFormat: backEndDateFormat,
        lang:"es",
        cargando : true,
        envio: false,
        REComas: /[,]{2,}/,
        invoicesChange: [],
        invoicesShow: [],
        invoices: [],
        selected: [],
        banks: [],
        proveedores: [],
        filtroSupplier: '',
        filtroBank: '',
        page: 1,
        pageCount: 10,
    },
    created: function () {
        this.cargando = true;

        try {
            this.currencies = JSON.parse(document.getElementById('currenciesData').value);
            document.getElementById("eliminarData").removeChild(document.getElementById('currenciesData'));

            for (var i = 0; i < this.currencies.length; i++) {
                this.options.push({});
                this.buscarFacturas.push(true);
                this.loading.push(false);
                this.totalFacturas.push(0)

                this.filter[i] = JSON.parse(document.getElementById('filterData+' + i).value);
                this.filter[i].currency = this.currencies[i].id
                document.getElementById("eliminarData").removeChild(document.getElementById('filterData+' + i));
            }
        } catch (e) {
            for (var i = 0; i < this.currencies.length || i < 1; i++) {
                this.options.push({});
                this.buscarFacturas.push(true);
                this.loading.push(false);
                this.totalFacturas.push(0)

                this.filter[0] = null
            }
        }

        document.getElementById("app").removeAttribute("hidden")
        this.lang = document.getElementsByTagName("html")[0].getAttribute("lang")
        this.cargando = false

    },
    mounted: async function () {
        tiempoLogin(this.modalLogout)

        for (var i = 0; i < this.currencies.length; i++) {
            await this.llenarFinancing(i);
        }

    },
    watch: {
        options: {
            async handler() {

                for (var i = 0; i < this.currencies.length; i++) {
                    if (this.options[i].itemsPerPage == -1) {
                        llamadaRecursiva(this.buscarFacturas[i], this.llenarFinancing, i)
                        return
                    }

                    var tamanoFacturas = filtrarPublicationsCurrency(this.invoicesShow, this.currencies[i].id).length

                    if (this.buscarFacturas[i] && this.options[i].page * this.options[i].itemsPerPage >= tamanoFacturas - this.options[i].itemsPerPage) {
                        await this.llenarFinancing(i)
                    }
                }
            },
            deep: true,
        },
    },
    methods: {
        async llenarFinancing(indice) {
            if (this.loading[indice]) return
            this.loading[indice] = true

            var take = 100
            await axios.post('?handler=financiable', { pagination: { take: take, skip: this.totalFacturas[indice] }, filter: this.filter[indice] },
                {
                    headers: {
                        "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                    }
                })
                .then((respond) => {
                    resetTime()
                    this.loading[indice] = false
                    if (respond.data.length == 0) {
                        this.buscarFacturas[indice] = false
                        return
                    }

                    if (respond.data.length > 0 && respond.data[0].errors == notAuthorized) {
                        window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                        return
                    }

                    if (respond.data.length < take / 2)
                        this.buscarFacturas[indice] = false

                    this.totalFacturas[indice] += respond.data.length

                    respond.data.map(data => {
                        if (data != null) 
                            this.invoices.push(data)
                        
                    })
                    
                    this.invoices.sort((a, b) => a.supplier.name.toUpperCase() < b.supplier.name.toUpperCase()
                        ? -1 : +a.supplier.name.toUpperCase() > b.supplier.name.toUpperCase())

                    this.filtrar()
                }).catch((respond) => { console.log(respond); this.loading[indice] = false });

            return (this.buscarFacturas[indice] && this.options[indice].itemsPerPage == -1)
        },
        //
        filtrar() {
            this.selected = []
            this.invoicesShow = this.invoices.filter( data => data.term_days > 3)
            
            let aux = []
            for (let t = 0; t < this.invoicesShow.length; t++) {
                let exist = false
                for (let i = 0; i < this.invoicesChange.length; i++) {
                    if (this.invoicesShow[t].id ==
                        this.invoicesChange[i].id) {
                        aux.push(this.invoicesChange[i])
                        if (!this.invoicesShow[t].request_financing) this.selected.push(t)
                        exist = true
                        break
                    }
                }

                if (!exist && this.invoicesShow[t].request_financing) {
                    this.selected.push(t)
                }

            }
            this.invoicesChange = aux
        },
        //
        validarComas(monto) {
            let comas = 0
            if (monto == null || monto == 0 || monto == '' || monto.length == 1 && monto[0] == ',') return ''

            if (this.REComas.test(monto)) return 0

            for (let i = 0; i < monto.length; i++) {
                if (monto[i] == ',') comas++

                if (comas >= 2) return 0
            }

            return 1
        },
        //
        financiar2(indice) {
            let invoicesFinancingChange = []
            console.log(indice)
            console.log(this.currencies[indice].id)
            var invoicesFiltradas = filtrarPublicationsCurrency(this.invoicesShow, this.currencies[indice].id)
            
            invoicesFiltradas.map(data => {
                if (data.request_financing != this.estado) {
                    invoicesFinancingChange.push({ id: data.id })
                }
            })

            this.financiar(invoicesFinancingChange)
        },
        //
        financiar3(item) {
            let data = []
            data.push({id: item.id})

            this.financiar(data)
        },
        methodPublicationsCurrency(idCurrency) {
            return filtrarPublicationsCurrency(this.invoicesShow, idCurrency)
        },
        //
        financiar(data) {

            if (this.envio) {
                return
            }

            this.envio = true
            
            axios.post('?handler=financiar', data,
                {
                    headers: {
                        "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                    }
                })
                .then((respond) => {
                    resetTime()
                    this.envio = false
                    let contador = 0
                    let contadorError = 0
                    let ids = []
                    if (respond.data != null)
                        respond = respond.data


                    if (typeof respond === 'string' || respond instanceof String) {

                        if (respond.includes("<!DOCTYPE html>")) {
                            window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired";
                            toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br>" + i18n.t("errorBaseDatos"));
                            return;
                        }
                    }

                    for (var i = 0; i < respond.length; i++) {
                        if (respond[i].errors != null) {
                            contadorError++
                            let error = respond[i].errors
                            if (error == notAuthorized) {
                                window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                                return
                            }else if (error == "Data null")
                                break
                            else if (error == "Invalid User") {
                                window.location.pathname = "../Index"
                                return
                            }
                            else if (respond[i].id != null) {
                                for (let w = 0; w < this.invoicesShow.length; w++) {
                                    if (this.invoicesShow[w].id == respond[i].id) {
                                        let bandera = false
                                        for (let t = 0; t < this.selected.length; t++) {

                                            if (this.selected[t] == w) {
                                                this.selected.splice(t, 1)
                                                bandera = true
                                                break
                                            }
                                        }

                                        if (!bandera) this.selected.push(w)

                                        toastr.warning(i18n.t("errorFinanciarSolicitar") + " '" + this.invoicesShow[w].number + "'")
                                    }
                                }
                            }

                        } else {
                            ids.push(respond[i].id)
                            contador++
                        }
                    }

                    ids.map(data => {

                        for (var i = 0; i < this.invoicesShow.length; i++) {
                            if (this.invoicesShow[i].id == data) {
                                this.invoicesShow[i].request_financing = !this.invoicesShow[i].request_financing
                                return
                            }
                        }


                    })

                    if (contador == respond.length) {
                        if (contador == 1) {
                            if (respond[0].request_financing) {
                                toastr.success(i18n.t("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.t("mensajesModal.financiamientoFactura"))
                            } else {
                                toastr.success(i18n.t("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.t("mensajesModal.financiamientoFacturaCancelacion"))
                            }
                        } else {
                            if (this.estado == true) {
                                toastr.success(i18n.t("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.t("mensajesModal.financiamientoFactura"))
                            } else {
                                toastr.success(i18n.t("mensajesModal.estimadoUsuario") + "<br><br>" + i18n.t("mensajesModal.financiamientoFacturaCancelacion"))
                            }
                        }
                    } else if (contadorError == respond.length) toastr.warning(i18n.tc("errorRespuesta", 2, { 0: "financiamiento de facturas"}))
                    else toastr.warning(i18n.tc("mensajesModal.problemasRespuesta", 2, {0:"financiamiento de facturas"}))
                    
                    this.invoicesChange = []
                }).catch(e => { console.log(e); toastr.error(i18n.t("errorRespuesta")); this.envio = false;  });
        },
        //
        seleccionar(index) {
            this.cambiarFinancing(index);
            if (this.invoicesChange.length == 0) {
                this.invoicesChange.push({
                    index: index,
                    id: this.invoicesShow[index].id
                })
                return
            }
            

            for (let i = 0; i < this.invoicesChange.length; i++) {
                if (this.invoicesChange[i].id == this.invoicesShow[index].id) {
                    this.invoicesChange.splice(i,1)
                    return
                }
            }

            this.invoicesChange.push({
                index: index,
                id: this.invoicesShow[index].id
            })

        },
        //
        cambiarFinancing(index) {
            for (let i = 0; i < this.invoices.length; i++) {
                if (this.invoices[i].id == this.invoicesShow[index].id) {
                    this.invoices[i].request_financing = !this.invoices[i].request_financing
                    this.invoicesShow[index].request_financing = this.invoices[i].request_financing

                    break
                }
            }
        },
        //
       
    }

})