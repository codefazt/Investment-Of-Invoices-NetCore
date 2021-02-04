new Vue({
    el: "#app",
    i18n,
    vuetify: new Vuetify({
        lang: {
            t: (key, ...params) => i18n.t(key, params)
        }
    }),
    data: {
        widthTelefono: widthTelefono,
        modalLogout: { mostrar: false },
        i18n: i18n,
        categories: ["DEBIT","CREDIT"],
        programs: [],
        groups: [],
        details: [],
        currencies: [],
        loading: false,
        edit: false,
        header: [
            { text: i18n.t("headers.n"), value: "n", align: "center" },
            { text: i18n.t("headers.nombre"), value: "name", align: "center" },
            { text: i18n.t("headers.abreviacion"), value: "abbreviation", align: "center" },
            { text: i18n.t("headers.estatus"), value: "status", align: "center" },
            { text: i18n.t("headers.detail"), value: "detail", align: "center" },
            { text: i18n.t("headers.opciones"), value: "opciones", align: "center" },
        ],
        headerDetails: [
            { text: i18n.t("headers.n"), value: "n", align: "center" },
            { text: i18n.t("headers.event"), value: "event", align: "center" },
            { text: i18n.t("headers.concept"), value: "concept", align: "center" },
            { text: i18n.t("headers.account"), value: "account", align: "center" },
            { text: i18n.t("headers.category"), value: "category", align: "center" },
        ],
        headerDetails2: [
            { text: i18n.t("headers.n"), value: "n", align: "center" },
            { text: i18n.t("headers.event"), value: "event", align: "center" },
            { text: i18n.t("headers.concept"), value: "concept", align: "center" },
            { text: i18n.t("headers.account"), value: "account", align: "center" },
            { text: i18n.t("headers.category"), value: "category", align: "center" },
            { text: i18n.t("headers.opciones"), value: "opciones", align: "center" },
        ],
        estadoCarga: 0,
        dialogDetails : false,
        cargando: true,
        enviando: false,
        filter: {},
        nuevo: {
            id: "",
            name: '',
            abbreviation: '',
            description: '',
            program: {
                id:""
            },
            currency: {
                id:""
            },
            details: []
        },
        detail: {
            id:"",
            event: "",
            concept: "",
            category: "",
            account: ""
        },
        indiceDetail:-1,
        indice: -1,
        backEndDateFormat: backEndDateFormat
    },
    created: function () {
        this.cargando = true

        try {
            this.filter = JSON.parse(document.getElementById('filterData').value);
            document.getElementById("eliminarData").removeChild(document.getElementById('filterData'));
        } catch (e) {
            this.filter = null
        }

        document.getElementById("app").removeAttribute("hidden")

        this.cargando = false
    },
    mounted: async function () {
        tiempoLogin(this.modalLogout)
        await this.obtenerGroups()
        await this.obtenerCountry()
    },
    methods: {
        //
        async obtenerGroups() {
            this.loading = true
            await axios.post('?handler=groups', this.filter,
                {
                    headers: {
                        "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                    }
                })
                .then((respond) => {
                    resetTime()
                    this.loading = false
                    var respuesta = []
                    if (respond.data == null && respond == null) {
                        return
                    }
                    
                    if (respond.data != null) {
                        respuesta = respond.data
                    } else {
                        respuesta = respond
                    }

                    if (respuesta.length > 0 && respuesta[0].errors == notAuthorized) {
                        window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                        return
                    } else if (respuesta.length > 0 && respuesta[0].errors != null) {
                        return
                    }

                    this.groups = respuesta
                }).catch((respond) => { console.error(respond); this.loading = false })

        },
        //
        async obtenerCountry() {
            await axios.post('?handler=country', {},
                {
                    headers: {
                        "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                    }
                })
                .then((respond) => {
                    resetTime()
                    var respuesta = []
                    if (respond.data == null && respond == null) {
                        return
                    }

                    if (respuesta.length > 0 && respuesta[0].errors == notAuthorized) {
                        window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                        return
                    }

                    if (respond.data != null) {
                        respuesta = respond.data
                    } else {
                        respuesta = respond
                    }

                    this.programs = respuesta.programs
                    this.currencies = respuesta.currencies
                }).catch((respond) => { console.error(respond); })

        },
        //
        /*
        ordenarDiscriminators() {
            this.discriminators.sort((a, b) => a.toLowerCase() < b.toLowerCase() ? -1 : +(a.toLowerCase() > b.toLowerCase()))
        },
        */
        async addDetail() {
            if (!this.validarDetail(this.detail)) {
                return
            }

            if (this.indiceDetail == -1) {
                this.nuevo.details.push(this.detail)
            } else {
                var nuevoAux = JSON.parse(JSON.stringify(this.groups[this.indice]))

                nuevoAux.details[this.indiceDetail] = JSON.parse(JSON.stringify(this.detail))

                var result = await this.updateGroup(nuevoAux)
                if (result) {
                    this.nuevo.details[this.indiceDetail] = JSON.parse(JSON.stringify(this.detail))
                    this.nuevo.details.push({})
                    this.nuevo.details.pop()
                }
            }

            this.limpiarDetail()
        },
        //
        eliminarDetail(item) {
            this.nuevo.details = removeItemFromArr(this.nuevo.details, item)
            this.limpiarDetail()
        },
        //
        validarDetail(item) {
            if (item.event == "") {
                toastr.warning(i18n.t("needEvent"))
                return false
            }
            if (item.concept == "") {
                toastr.warning(i18n.t("needConcept"))
                return false
            }
            if (item.category == "") {
                toastr.warning(i18n.t("needCategory"))
                return false
            }
            if (item.account == "") {
                toastr.warning(i18n.t("needAccount"))
                return false
            }

            return true
        },
        //
        validarGroup(item) {
            if (item.name == "") {
                toastr.warning(i18n.t("needName"))
                return false
            }
            if (item.abbreviation == "") {
                toastr.warning(i18n.t("needAbbreviation"))
                return false
            }

            if ( item.program.id == "") {
                toastr.warning(i18n.t("needProgram"))
                return false
            }

            if (item.currency.id == "" || item.currency.id == 0) {
                toastr.warning(i18n.t("needCurrency"))
                return false
            }

            if (item.details.length == 0) {
                toastr.warning(i18n.t("needDetails"))
                return false
            }

            return true
        },
        //
        detallesGroup(item) {
            if(item != null && item.details != null) 
                this.details = item.details

            this.dialogDetails = true
        },
        //
        limpiar() {
            this.nuevo = {
                id: "",
                name: '',
                abbreviation: '',
                description: '',
                program: {
                    id:""
                },
                currency: {
                    id:""
                },
                details: []
            }
            
            this.estadoCarga = 0
            this.indice = -1
        },
        //
        limpiarDetail() {
            this.indiceDetail = -1

            this.detail = {
                id: "",
                event: "",
                concept: "",
                category: "",
                account: ""
            }
        },
        //
        async procesoGroup() {
            if (!this.validarGroup(this.nuevo)) {
                return
            }

            this.edit = false

            if (this.indice == -1) {
               await this.crearGroup()
            } else {
               await this.updateGroup(this.nuevo)
            }

            this.limpiar()
        },
        //
        async crearGroup() {
            if (this.enviando) return

            this.enviando = true
            await axios.post("?handler=crear", this.nuevo, {
                headers: {
                    "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                }
            }).then(resp => {
                resetTime()
                this.enviando = false
                var r = ""
                if (resp.data == null && resp == null) {
                    toastr.warning(i18n.t("problemasRespuesta"))
                    this.enviando = false
                    return
                }

                if (resp.data != null) r = resp.data
                else r = resp


                if (typeof r === 'string' || r instanceof String) {

                    if (r.includes("<!DOCTYPE html>")) {
                        window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired";
                        toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br>" + i18n.t("errorBaseDatos"));
                        return;
                    }
                }

                if (r != null && r.errors == notAuthorized) {
                    window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                    return
                }

                if (r.errors != null) {
                    toastr.warning(i18n.t("problemasRespuesta"))
                    return
                }

                if (r.status == null)
                    r.status = true

                this.nuevo = r
                this.nuevo.status = true
                this.groups.push(this.nuevo)
                this.limpiar()
                toastr.success(i18n.t("exitoRespuesta"))
            }).catch(err => { console.log(err); toastr.error(i18n.t("errorRespuesta")); this.enviando = false })
        },
        //
        async updateGroup(item) {
            if (this.enviando) return

            this.enviando = true
            item.id = this.groups[this.indice].id

            var result = await axios.post("?handler=actualizar", item, {
                headers: {
                    "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                }
            }).then(resp => {
                resetTime()
                this.enviando = false
                var r = ""
                if (resp.data == null && resp == null) {
                    toastr.warning(i18n.t("problemasRespuesta"))
                    return false
                }

                if (resp.data != null) r = resp.data
                else r = resp

                if (typeof r === 'string' || r instanceof String) {

                    if (r.includes("<!DOCTYPE html>")) {
                        window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired";
                        toastr.error(i18n.t("mensajesModal.estimadoUsuario") + "<br /><br>" + i18n.t("errorBaseDatos"));
                        return false;
                    }
                }

                if (r != null && r.errors == notAuthorized) {
                    window.location.href = "../Logout?returnUrl=~/index?error=sessionExpired"
                    return false
                }

                if (r.errors != null) {
                    toastr.warning(i18n.t("problemasRespuesta"))
                    return false
                }
                

                if (r.status == null)
                    r.status = true

                this.groups[this.indice] = r

                this.groups.push({});
                this.groups.pop();

                toastr.success(i18n.t("exitoRespuesta"))
                return true
                }).catch(error => { console.log(error); this.enviando = false; toastr.error(i18n.t("errorRespuesta")) })

            return result
        },
        //
        editar(indice) {
            this.estadoCarga = 0
            this.nuevo = JSON.parse(JSON.stringify(this.groups[indice]))
            this.indice = indice
        },
        //
        editarDetail(indice) {
            this.detail = JSON.parse(JSON.stringify(this.nuevo.details[indice]))
            this.indiceDetail = indice
        },
    }

})