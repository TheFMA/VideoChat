<template >
  <div class="w-1/2 bg-white p-8 rounded shadow-lg">
      <div class="flex flex-row justify-between mb-5">
          <h1>{{ chatRoom }}</h1>
          <button @click="closeChat">Close</button>
      </div>

      <div class="flex flex-col overflow-auto scroll-smooth h-96 gap-3 pb-3">
          <MyMessage v-for="(messageInfo, index) in messages" 
                  :key="index" 
                  :messageInfo="messageInfo"
                   />
          <span ref="messagesEndRef"></span>
      </div>
      <div class="flex gap-3">
          <input type="text" v-model="message" placeholder="Enter message" />
          <button @click="onSendMessage" colorScheme="blue">Send</button>
      </div>
</div>
</template>
<script>
import MyMessage from './MyMessage.vue'
import { ref} from 'vue'
export default {
  components: {
      MyMessage
  },
  props: {
  messages: Array, 
  //chatRoom: String
},
setup(props) {
  const message = ref('');
  // const chatRoom = ref('');
  console.log(props)
  return {
    message,
  };
}
}
</script>
<style>
  
</style>