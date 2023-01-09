


<script setup>
import { useFeatBitStore } from '@/featbit'
import fbClient from "featbit-js-client-sdk";

defineProps({
})

const features = useFeatBitStore();

const sleep = ms => new Promise(r => setTimeout(r, ms));
const random = (max) => Math.floor(Math.random() * max);

const runInsights = async () => {
  while (true) {
    fbClient.identify({
      keyId: 'uuid-' + Date.now(),
      name: 'uname-' + Date.now(),
      customizedProperties: []
    })
    await sleep(500);
    console.log(fbClient.variation('wechat-payment'));
    const r = random(10);
    await sleep(r * 200);
  }
}
</script>

<template>
  <div class="greetings">
    <button @click="runInsights">run</button>
  </div>
</template>

<style scoped>

</style>
