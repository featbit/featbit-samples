import { createApp } from 'vue'
import { createPinia } from 'pinia'
import App from './App.vue'

import './assets/main.css'

import { featBit } from './featbit'

const app = createApp(App)

app.use(createPinia())

app.use(featBit)

app.mount('#app')
