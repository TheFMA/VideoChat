<template>
  <div>
    <title>Название вашего сайта</title>
    <!-- <header>
      <a href="/">
        <h1>JoyMeet</h1>
      </a>
        <div>
            <div v-if="email">
              <span >{{ email }}</span>
              <a v-if="email" @click="logout" class="btn-gray" style="padding-left: 20px;">Log out</a>
            </div>
            <div v-else>
              <button  @click="openLogInModal" class="btn-blue">Log in</button>
              <button  @click="openSignUpModal" class="btn-gray" style="padding-left: 20px;" >Sign up</button>
            </div>
        </div>
    </header> -->
    <header>
      <!-- <a @click="gotoHomePage"> -->
      <a @click="this.$router.push('/')">
        <h1>JoyMeet</h1>
      </a>
      <div v-if="loggedIn">
        <span>{{ email }}</span>
        <button @click="logout" class="btn-gray" style="padding-left: 20px;">Log out</button>
      </div>
      <div v-else>
        <button @click="openLogInModal" class="btn-blue">Log in</button>
        <button @click="openSignUpModal" class="btn-gray" style="padding-left: 20px;">Sign up</button>
      </div>
    </header>
    <div v-if="isConnected">
      <!-- <MyChat :messages="messages" :MyChatRoom="MyChatRoom" :sendMessage="sendMessage" /> -->
      <!-- <MyRoom @click = "$router.push('/chat')" :messages="messages" :MyChatRoom="MyChatRoom" :sendMessage="sendMessage" /> -->
       <MyRoom :messages="messages" :MyChatRoom="MyChatRoom" :sendMessage="sendMessage" />
        <!-- <router-view></router-view> -->
    </div>
    <div v-else>
    <main class="main-background">

    <div>
      <MyModal  v-if="showLogInModal" @close-modal="showLogInModal = false" >
        <MyLogin :login-user="loginUser" @login-user="showLogInModal = false" @open-log-in-modal="openLogInModal"></MyLogin>
      </MyModal>
      <a @click="openModal" class="btn-blue">Join Chat</a>
      <MyModal v-if="showModal" @close-modal="showModal = false">
        <WaitingRoom :join-chat="joinChat" @join-chat="showModal = false" @open-modal="showModal = true"></WaitingRoom>
      </MyModal>
      <MyModal v-if="showSignUpModal" @close-modal="showSignUpModal = false">
        <MySignUp :sign-up="signUp" @sign-up-user="showSignUpModal = false" @open-sign-up-modal="openSignUpModal"></MySignUp>
      </MyModal>
      <div v-if="showMessage" class="success-message">
        <p>Регистрация прошла успешно!</p>
      </div>
    </div> 
    </main>
    </div>
  </div>
</template>

<script>
import axios from 'axios';
import WaitingRoom from './components/WaitingRoom.vue';
import MyModal from './components/MyModal.vue';
import { ref, watch} from "vue";
import { HubConnectionBuilder } from "@microsoft/signalr";
// import MyChat from "./components/MyChat.vue";
import MyLogin from "./components/MyLogin.vue";
import MySignUp from './components/MySignUp.vue';
import MyRoom from './components/MyRoom.vue';
import router from './router/router';
export default {
  components: {
    WaitingRoom,
    MyModal,
    // MyChat,
    MyLogin,
    MySignUp,
    MyRoom
  },

  setup() {
    const connection = ref(null);
    const messages = ref([]);
    const userName = ref('');
    const MyChatRoom = ref('');
    const showModal = ref(false);
    const showLogInModal = ref(false);
    const showSignUpModal = ref(false);
    const showMessage = ref(false);
    const email = ref('');
    const password = ref('');
    const loggedIn = ref(false);

    const joinChat = async (userName, chatRoom) => {
      const hubConnection = new HubConnectionBuilder()
        .withUrl("https://localhost:7049/chat")
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
        router.push('/chat');
        console.log(connection);
      } catch (err) {
        console.log(err);
      }
    }

    const openLogInModal = () => {
      showLogInModal.value = true;
    };
    const openSignUpModal = () => {
      showSignUpModal.value = true;
    };
    const openModal = () => {
      showModal.value = true;
    }
    const sendMessage = async (message) => {
      await connection.value.invoke("SendMessage", message);
    };
    const gotoHomePage = () => {
      // Реализация перехода на главную страницу, например:
      this.$router.push('/');
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

    const  loginUser = async(email, password) => {
      try {
        const response = await axios.post('https://localhost:7049/api/Account/login', {
          email: email ,
          password: password
      });

      // Проверяем, получен ли JWT токен в ответе
      if (response.data && response.data.token) {
        const token = response.data.token;
        // Сохраняем полученный токен, например, в localStorage
        localStorage.setItem('token', token);
        console.log('Logged in successfully. JWT token:', token);
        handleLogin(email);
        return token;
      } else {
        // Если токен не был получен, обрабатываем ошибку
        throw new Error('Invalid token received');
      }
    } catch (error) {
      // Обрабатываем ошибку запроса
      console.error('Error logging in:', error);
      throw error;
    }
  };

  const signUp = async(name, email, password, confirmPassword) => {
    try {
        const response = await axios.post('https://localhost:7049/api/Account/register', {
            name: name,
            email: email,
            password: password,
            confirmPassword: confirmPassword
        });

        if (response.status === 200) {
            console.log('User successfully registered:', response.data);
            signUpSuccessMessage();
        }
    } catch (error) {

        console.error('Error registering user:', error);
      }
  }

  const signUpSuccessMessage = () => {
    // Показываем сообщение об успешной регистрации на короткое время
    showMessage.value = true;
    showSignUpModal.value = false;
    setTimeout(() => {
        showMessage.value = false;
    }, 10000); // Скрываем сообщение через две секунды
};

  const handleLogin = (userEmail) => {
      email.value = userEmail;
      loggedIn.value = true;
      showLogInModal.value = false;
    };


    const logout = () => {
      email.value = '';
      loggedIn.value = false;
      // Дополнительные действия по выходу пользователя
    };

    return {
      userName,
      // chatRoom,
      joinChat,
      messages,
      connection,
      isConnected,
      MyChatRoom, 
      sendMessage,
      showModal,
      openModal,
      showLogInModal,
      openLogInModal,
      loginUser,
      email,
      password,
      loggedIn,
      handleLogin,
      logout,
      openSignUpModal,
      showSignUpModal,
      signUp,
      signUpSuccessMessage,
      showMessage,
      gotoHomePage,
    };
  },
}
</script>

<style>
.btn-blue {
  background-color: blue;
  color: white;
  padding: 5px 10px;
  border: none;
  border-radius: 5px;
  cursor: pointer;
}
  body {
    margin: 0;
    padding: 0;
    font-family: Arial, sans-serif;
  }
  header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 10px 20px;
    background-color: #f2f2f2;
  }
  main {
      
    background-size: cover;
    background-position: center;
    height: 100vh;
    display: flex;
    justify-content: center;
    align-items: center;
  }
  .btn-blue {
    background-color: #007bff;
    color: #fff;
    padding: 10px 20px;
    border: none;
    border-radius: 5px;
    text-transform: uppercase;
    text-decoration: none;
  }
  .main-background {
    background-image: url('assets/background.jpg'); 
    background-size: cover;
    background-position: center;
    height: 100vh;
    display: flex;
    justify-content: center;
    align-items: center;
  }
  .success-message {
    position: fixed;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -50%);
    background: rgba(230, 247, 233, 0.9); /* полупрозрачный белый цвет */
    padding: 10px 20px;
    border: 1px solid #4caf50;
    border-radius: 5px;
    display: inline-block;
    color: #4caf50; /* Нежно-зеленый цвет текста */
}
</style>