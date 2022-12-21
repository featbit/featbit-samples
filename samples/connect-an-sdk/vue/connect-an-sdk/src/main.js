import { createApp } from 'vue'
import App from './App.vue'

import './assets/main.css'
import { createPinia } from 'pinia'
import { featBit } from './featbit'


// createApp(App).mount('#app')
createApp(App).use(createPinia()).use(featBit).mount('#app');
