import { createRouter, createWebHistory, RouteRecordRaw } from 'vue-router';
import PersonasView from '@/views/PersonasView.vue';

const routes: RouteRecordRaw[] = [
  { path: '/', name: 'home', component: PersonasView }
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;
