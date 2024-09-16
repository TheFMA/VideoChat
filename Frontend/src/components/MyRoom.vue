<template>
  <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css">
  <div>
    <div style="width: 75%; display: inline-block;">
      <!-- <video ref="localVideo" autoplay></video> -->
      <video ref="localVideo" autoplay style="width: 40%; height: 45%; margin-left: 45px;"></video>
      <div style="position: absolute; bottom: 30px; left: 50%; transform: translateX(-50%); display: flex; justify-content: space-between; width: 100px;">
        <button @click="toggleAudio" style="background: none; border: none;"><i class="fas fa-microphone"></i></button>
        <button @click="toggleVideo" style="background: none; border: none;"><i class="fas fa-video"></i></button>
      </div>
    </div>
    <div style="width: 25%; height: 100vh; display: inline-block;">
      <MyChat :messages="messages" :MyChatRoom="MyChatRoom" :sendMessage="sendMessage" />
    </div>
  </div>
</template>

<script>
  import MyChat from './MyChat.vue';
  import { ref, onMounted, onBeforeUnmount } from 'vue';

export default {
  components: {
    MyChat
  },
  props: {
      messages: Array, 
      MyChatRoom: String,
      sendMessage: Function
  },
  setup() {
  const localVideo = ref(null);
  const remoteVideo = ref(null);
  let localStream;

  const startWebRTC = async () => {
    const stream = await navigator.mediaDevices.getUserMedia({ video: true, audio: true });
    localVideo.value.srcObject = stream;
    localStream = stream;

    const configuration = { iceServers: [{ urls: 'stun:stun.l.google.com:19302' }] };
    const peerConnection = new RTCPeerConnection(configuration);

    stream.getTracks().forEach((track) => peerConnection.addTrack(track, stream));

    peerConnection.ontrack = (event) => {
      remoteVideo.value.srcObject = event.streams[0];
    };

    const offer = await peerConnection.createOffer();
    await peerConnection.setLocalDescription(offer);
    // Теперь offer необходимо передать другому узлу, и там применить через setRemoteDescription
  };

   const toggleAudio =() => {
      const videoElement = localVideo.value;
      videoElement.muted = !videoElement.muted;
    };
    const toggleVideo = () => {
      const videoElement = localVideo.value;
      videoElement.srcObject.getVideoTracks().forEach((track) => (track.enabled = !track.enabled));
    }

  onMounted(() => {
    startWebRTC();
  });

  onBeforeUnmount(() => {
    if (localStream) {
      localStream.getTracks().forEach(track => track.stop());
    }
  });

  return {
    localVideo,
    remoteVideo,
    toggleAudio,
    toggleVideo
  };
}

};
</script>

<style>
.container {
display: flex;
width: 100%;
height: 100%;
}

.chat-left {
width: 75%;
}

.chat-right {
width: 25%;
}
.round-button {
    background-color: blue;
    border: none;
    border-radius: 50%; /* Радиус равный половине ширины/высоты делает кнопку круглой */
    width: 40px; /* Вы можете настроить ширину по своему усмотрению */
    height: 40px; /* Вы можете настроить высоту по своему усмотрению */
    display: flex;
    align-items: center;
    justify-content: center;
    margin: 5px; /* Добавим немного отступа вокруг кнопок */
}
</style>
