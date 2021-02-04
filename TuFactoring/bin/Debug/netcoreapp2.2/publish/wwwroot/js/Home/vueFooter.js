new Vue({
    //store: storeLogin,
    el: "#appFooter",
    i18n,
    vuetify: new Vuetify({
        lang: {
            t: (key, ...params) => i18n.t(key, params)
        }
    }),
    data: {
        dialog: false,

    },
    methods: {
        
    },
    computed: {

    },
    created: function () {

       
    }
});