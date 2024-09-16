<template>
    <div class="modal-overlay">
      <div class="modal">
        <slot></slot>
        <button @click="closeModal">Close</button>
      </div>
    </div>
  </template>
  
  <script>
  export default {
    methods: {
      closeModal() {
        this.$emit('close-modal');
      }
    }
  }
  </script>
  
  <style scoped>
  .modal-overlay {
    position: fixed;
    top: 0;
    left: 0;
    right: 0;
    bottom: 0;
    background-color: rgba(0, 0, 0, 0.5); /* semi-transparent black overlay */
    display: flex;
    justify-content: center;
    align-items: center;
    z-index: 9999; /* Ensure the modal is in front of other elements */
  }
  
  .modal {
    background-color: white;
    padding: 20px;
    border-radius: 5px;
    z-index: 10000;  /* Make sure the modal itself is above the overlay */
  }
  
  button {
    margin-top: 10px;
  }
  </style>
  