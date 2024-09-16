<template >
  <div class=" bg-white p-8 rounded shadow-lg">
      <div class="flex flex-row justify-between mb-5">
          <h2 class="text-center flex-1 font-bold">{{ MyChatRoom }}</h2>
          <!-- <button @click="closeChat">Close</button> -->
          <button @click="closeChat" class="p-2 rounded-md bg-white hover:bg-gray-100 focus:outline-none">
            <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor" class="h-4 w-4 text-gray-600">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12"></path>
            </svg>
</button>
      </div>

      <div class="flex flex-col overflow-auto scroll-smooth h-96 gap-3 pb-3">
          <MyMessage v-for="(messageInfo, index) in messages" 
                  :key="index" 
                  :messageInfo="messageInfo"
                  
                   />
          <span ref="messagesEndRef"></span>
      </div>
      <div class="flex gap-3">
          <input type="text" v-model="message" placeholder="Enter message" class="border border-gray-300 rounded-md px-2 py-1.5 focus:outline-none focus:ring-2 focus:ring-indigo-600" />
          <button @click="onSendMessage" class="flex w-full justify-center rounded-md bg-indigo-600 px-3 py-1.5 text-sm font-semibold leading-6 text-white shadow-sm hover:bg-indigo-500 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-indigo-600" colorScheme="blue">Send</button>
      </div>
</div>
</template>
<script>
// import { sendMessage } from '@microsoft/signalr/dist/esm/Utils';
import MyMessage from './MyMessage.vue'
import { ref} from 'vue'
export default {
  components: {
      MyMessage
  },
  props: {
  messages: Array, 
  MyChatRoom: String,
  sendMessage: Function
},
setup(props) {
  const message = ref('');
  // const chatRoom = ref('');
  console.log(props)

  const onSendMessage = () => {
      props.sendMessage(message.value);
      message.value = '';
  };

  return {
    message,
    onSendMessage
  };
}
}
</script>
<style>
  
</style>