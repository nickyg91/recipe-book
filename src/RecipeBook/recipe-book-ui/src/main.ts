import { createApp } from 'vue';
import { createPinia } from 'pinia';
import ui from '@nuxt/ui/vue-plugin';

import App from './App.vue';
import router from './router';
import { useUserStore } from './stores/userStore';
import './assets/main.css';

const app = createApp(App);

app.use(createPinia());
app.use(router);
app.use(ui);

const userStore = useUserStore();

userStore.checkAuth();
app.mount('#app');
