import { createApp } from 'vue'
import './style.css'
import App from './App.vue'
import { createPinia } from 'pinia'
import { featBit } from '@/featbit'

const pinia = createPinia()

const app = createApp(App)
app.use(pinia)
console.log(`featbit init`);
app.use(featBit)
app.mount('#app');