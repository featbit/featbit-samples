<script setup>
// This starter template is using Vue 3 <script setup> SFCs
// Check out https://vuejs.org/api/sfc-script-setup.html#script-setup
import Steps from './components/Steps/Steps.vue'
import DinoGame from './components/DinoGame/DinoGame.vue'
import { ffcStore } from "./featbit";

</script>

<template>
  <div>
    <Steps />
    <DinoGame v-if="ffcStore.flags['game-runner'] === true" />
  </div>
</template>

<style scoped>

</style>
