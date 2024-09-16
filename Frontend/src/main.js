import { createApp } from 'vue'
//import Vue from 'vue'
import App from './App.vue'
import './index.css'
import router from './router/router.js'
// import Chakra, { CThemeProvider, CReset } from '@chakra-ui/vue'

// Vue.use(Chakra)

// new Vue({
//     el: '#app',
//     render: (h) => h(CThemeProvider, [h(CReset), h(App)])
//   }).$mount()
createApp(App).use(router).mount('#app')
