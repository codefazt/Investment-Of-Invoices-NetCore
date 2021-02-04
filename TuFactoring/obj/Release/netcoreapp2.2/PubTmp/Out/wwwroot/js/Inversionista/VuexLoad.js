const storeInvestor = new Vuex.Store({
  state: {
    deducciones: [],
    errores: [],
    nuevaFactura: [],
    tipoDeduccion: [],
    tipoFactura:[],
    tipoMoneda: []
  },
  mutations: {
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
    //Guarda los datos de una factura
    hacerOferta: async function ({ commit }, factura) {
      //console.log('factura: ');
      //console.log(factura);
      await axios.post('Subasta?handler=Ofertar', factura,
        {
          headers: {
            "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
          }
        })
        .then((respond) => {
          console.log(respond.data);
          if (respond.data.errors !== null)
            commit('muestraErrores', respond.data.errores);
            else
            commit('recargaFactura', respond.data);
        }).catch((respond) => { console.log(respond); });
    },
    
    //
    postularFacturas: async function ({ commit }, facturas) {
      axios.post('PostularFacturas?handler=postular', facturas,
        {
          headers: {
            "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
          }
        })
        .then((respond) => {
          for (var i = 0; i < respond.data.length; i++) {
            if (respond.data[i].errors !== null)
              commit('muestraErrores', respond.data[i].errores);
            else
              commit('recargaFacturas', respond.data[i]);
          }

        });
    }
    //
  }
});



  
