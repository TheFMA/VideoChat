<template>
  <div class="flex min-h-full flex-col justify-center px-6 py-12 lg:px-8">
    <div class="sm:mx-auto sm:w-full sm:max-w-sm">
      <img class="mx-auto h-10 w-auto" src="https://tailwindui.com/img/logos/mark.svg?color=indigo&shade=600" alt="Your Company">
      <h2 class="mt-10 text-center text-2xl font-bold leading-9 tracking-tight text-gray-900">Онлайн чат</h2>
    </div>

    <div class="mt-10 sm:mx-auto sm:w-full sm:max-w-sm">
      <form @submit.prevent="onSubmit" class="space-y-6" action="#" method="POST">
        <div>
          <label for="email" class="block text-sm font-medium leading-6 text-gray-900">Имя пользователя</label>
          <div class="mt-2">
            <input v-model="userName" id="email" name="email" type="text" autocomplete="email" required class="block w-full rounded-md border-0 py-1.5 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm sm:leading-6">
          </div>
        </div>

        <div>
          <div class="flex items-center justify-between">
            <label for="password" class="block text-sm font-medium leading-6 text-gray-900">Название чата</label>
          </div>
          <div class="mt-2">
            <input v-model="chatRoom" id="password" name="password" autocomplete="current-password" required class="block w-full rounded-md border-0 py-1.5 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm sm:leading-6">
          </div>
        </div>

        <div>
            <button type="submit" class="flex w-full justify-center rounded-md bg-indigo-600 px-3 py-1.5 text-sm font-semibold leading-6 text-white shadow-sm hover:bg-indigo-500 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-indigo-600">Присоединиться</button>
        </div>
      </form>

    </div>
  </div>  
</template>

<script>
// import { CButton, CHeading, CText, CInput} from "@chakra-ui/vue";
import { ref } from 'vue';
export default {
    // components: {
    //     // CButton,
    //     // CHeading,
    //     // CText, 
    //     // CInput,
    // },
    props: ['joinChat'],
    setup(props) {
      const userName = ref('');
      const chatRoom = ref('');

      const onSubmit = () => {
        props.joinChat(userName.value, chatRoom.value);
      };

      const openModal = () => {
        this.$emit('open-modal');
    }

      return { userName, chatRoom, onSubmit, openModal };
    }

}
</script>

<style>
    /* form {
        display: flex;
        flex-direction: column;
        align-items: center;
        margin: 0 auto; 
        max-width: 20rem; 
    } */
    .enterButton {
        background-color: blue;
        color: white;
        padding: 5px 10px;
        border: none;
        border-radius: 5px;
        cursor: pointer;
        margin-top: 10px;
    }
</style>