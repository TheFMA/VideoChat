
// import App from '@/App.vue';
// import MyRoom from '@/components/MyRoom.vue';
// import { createRouter, createWebHistory } from 'vue-router';
// // Другие импорты и роуты

// const routes = [
//   // Ваши другие маршруты
//   {
//     path: '/chat',
//     name: 'MyRoom',
//     component: MyRoom
//   },
//   {
//     path: '/',
//     component: App
//   }
// ];

// const router = createRouter({
//   routes,
//   history: createWebHistory(process.env.BASE_URL)
// });

// export default router;

import App from '@/App.vue';
import MyRoom from '@/components/MyRoom.vue';
import { createRouter, createWebHistory } from 'vue-router';

const routes = [
  {
    path: '/',
    component: App
  },
  {
    path: '/chat',
    component: MyRoom
  }
];

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes
});

export default router;

