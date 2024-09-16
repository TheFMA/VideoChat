<template>
    <div class="w-fit ">
      <span>{{ messageInfo.userName }}</span>
      <div class="p-2 bg-gray-100 rounded-lg shadow-md">
        {{ messageInfo.message }}
      </div>
    </div>
</template>
  
<script>
  export default {
    props: ['messageInfo', 'messages'],
  };
</script>

<style>
    
</style>