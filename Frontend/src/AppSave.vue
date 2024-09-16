<template>
  <div class="min-h-screen flex items-center justify-center bg-gray-100">
    <!-- <MyHeader style=" border: 1px solid #ccc; border-radius: 2px; padding: 10px"/> -->
    <div v-if="isConnected">
      <MyChat :messages="messages" :MyChatRoom="MyChatRoom" :sendMessage="sendMessage" />
    </div>
    <div v-else>
      <WaitingRoom :join-chat="joinChat" />
    </div> 
  </div>
</template>

<script>
import WaitingRoom from "./components/WaitingRoom.vue";
import { ref, watch} from "vue";
import { HubConnectionBuilder } from "@microsoft/signalr";
import MyChat from "./components/MyChat.vue";
import MyHeader from './components/MyHeader.vue';

export default {
  name: "App",
  components: {
    WaitingRoom,
    MyChat
    //MyHeader
  },

  setup() {
    const connection = ref(null);
    const messages = ref([]);
    const userName = ref('');
    const MyChatRoom = ref('');

    const joinChat = async (userName, chatRoom) => {
      const hubConnection = new HubConnectionBuilder()
        .withUrl("https://localhost:7264/chat")
        .withAutomaticReconnect()
        .build();

      hubConnection.on("ReceiveMessage", (userName, message) => {
        messages.value = [...messages.value, { userName, message }];
        console.log(messages.value);
      });

      try {
        await hubConnection.start();
        await hubConnection.invoke("JoinChat", { userName, chatRoom });

        connection.value = hubConnection;
        MyChatRoom.value = chatRoom;
        console.log(connection);
      } catch (err) {
        console.log(err);
      }
    }

    const sendMessage = async (message) => {
      await connection.value.invoke("SendMessage", message);
    };
    // Watching for changes in connection
    const isConnected = ref(false);
    watch(connection, (newConnection) => {
      if (newConnection !== null) {
        isConnected.value = true;
      } else {
        isConnected.value = false;
      }
    });

    return {
      userName,
      // chatRoom,
      joinChat,
      messages,
      connection,
      isConnected,
      MyChatRoom, 
      sendMessage
    };
  },
};
</script>

<style>

</style>
