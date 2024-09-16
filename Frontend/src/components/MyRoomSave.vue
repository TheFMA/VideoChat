<template>
    <div>
      <div style="width: 75%; display: inline-block;">
        <video ref="localVideo" autoplay></video>
      </div>
      <div style="width: 25%; height: 100vh; display: inline-block;">
        <MyChat :messages="messages" :MyChatRoom="MyChatRoom" :sendMessage="sendMessage" />
      </div>
    </div>
    <!-- <template>
  <div class="container">
    <div class="chat-left"></div>
    <div class="chat-right">
      <MyChat :messages="messages" :MyChatRoom="MyChatRoom" :sendMessage="sendMessage" />
    </div>
  </div>
</template> -->

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
      remoteVideo
    };
  }

  };
  </script>

  <style>
.container {
  display: flex;
  width: 100%;
  height: 100vh; /* Используйте желаемую высоту */
}

.chat-left {
  width: 75%;
}

.chat-right {
  width: 25%;
}
  </style>
  