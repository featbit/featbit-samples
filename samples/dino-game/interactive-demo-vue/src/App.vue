<script setup>
// This starter template is using Vue 3 <script setup> SFCs
// Check out https://vuejs.org/api/sfc-script-setup.html#script-setup
import Steps from './components/Steps/Steps.vue'
import DinoGame from './components/DinoGame/DinoGame.vue'
import { useFeatBitStore } from '@/stores/featbit'
import ffcClient from "ffc-js-client-side-sdk";

// const store = useFeatBitStore()

// ffcClient.on("ff_update", (changes) => {
//   if (changes.length) {
//     store.setFlags(changes)
//   }
// });

// ffcClient.waitUntilReady().then((data) => {
//   if (data.length) {
//     store.setFlags(data)
//   }
// });


</script>

<template>
  <div>
    <Steps />
    <DinoGame />
  </div>
</template>

<style scoped>

</style>
