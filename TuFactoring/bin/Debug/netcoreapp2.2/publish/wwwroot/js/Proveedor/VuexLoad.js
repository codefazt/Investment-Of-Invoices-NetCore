const storeSupplier = new Vuex.Store({
  state: {
    status: false,
    deducciones: [],
    errores: [],
    nuevaFactura: [],
    tipoDeduccion: [],
    tipoFactura:[],
    tipoMoneda: [],
    currencyConfig: {
      symbol: '',
      thousandsSeparator: '.',
      fractionCount: 3,
      fractionSeparator: ',',
      symbolPosition: 'front',
      symbolSpacing: false
    }
  },
  mutations: {
    currencyFormat(state, data) {
      state.currencyConfig.thousandsSeparator = data.thousands;
      state.currencyConfig.fractionSeparator = data.decimal;
      state.currencyConfig.fractionCount = data.digits;
    },

    limpiar(state) {
      state.errores = [];
      state.nuevaFactura = [];
    },

    recargaFacturas(state, datos) {
      state.nuevaFactura.push(datos);
    },

    recargaFactura(state, dato) {
      state.nuevaFactura = null;
      state.nuevaFactura = dato;
    },

    muestraErrores(state, datos) {
      console.log(datos);
    },
    
  },
  actions: {
    
    //Publica la Factura Seleccionada
    publicarFactura: async function ({ commit }, dato) {
      console.log(dato);
      await axios.post('publicarFacturas?handler=publicar', { invoice_id: dato },
        {
          headers: {
            "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
          }
        })
          .then((resp) => {
              console.log(resp.data)
              if (resp.data.error != null) {
                  this.state.status = false
                  toastr.warning("Error al publicar la factura")
                  return
              }

              this.state.status = true
              toastr.success("Factura publicada con exito")
          }).catch((e) => { console.log(e); toastr.error("A ocurrido un error al publicar la factura") })
    },
    //Acepta la oferta del Banco para una factura
    venderFactura: async function ({ commit }, dato) {
      console.log(dato);
      console.log('-----------');
      await axios.post('publicarFacturas?handler=vender', { invoice_id: dato },
        {
          headers: {
            "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
          }
        })
        .then((resp) => {
            console.log("En la respuesta de la venta: ")
            console.log(resp.data)
            if (resp.data.error != null) {
                  this.state.status = false
                  toastr.warning("Error al comprar la factura")
                  return
              }

              this.state.status = true
              toastr.success("Factura comprar con exito")
          }).catch((e) => { console.log(e); toastr.error("A ocurrido un error al comprar la factura") })
      //commit('recargaFactura', offert);
    },
    //Deja la Factura en estado de Espera a su Vencimiento
    posponerFactura: async function ({ commit }, dato) {
      axios.post('publicarFacturas?handler=posponer', { invoice_id: dato },
        {
          headers: {
            "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
          }
        })
        .then((resp) => {
              console.log(resp.data)
              if (resp.data.error != null) {
                  this.state.status = false
                  toastr.warning("Error al posponer la factura")
                  return
              }

              this.state.status = true

            toastr.info("Factura Pospuesta con exito");
          }).catch((e) => { console.log(e); toastr.error("A ocurrido un error al posponer la factura") })
    },
    
  }
});



  
