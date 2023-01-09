<script setup>
import { useFeatBitStore } from '@/featbit'
import fbClient from "featbit-js-client-sdk";

defineProps({
})

const features = useFeatBitStore();

const sleep = ms => new Promise(r => setTimeout(r, ms));
const random = (max) => Math.floor(Math.random() * max);

const runABTest = async () => {
  let v0Count = 0;
  let v1Count = 0;
  let v2Count = 0;
  while (true) {
    fbClient.identify({
      keyId: 'uuid-' + Date.now(),
      name: 'uname-' + Date.now(),
      customizedProperties: []
    })
    await sleep(500);
    const vr = fbClient.variation('ai-face-verification-before-service');
    await sleep(200);

    if (vr != 'no') {
      const ir = random(3);
      if (ir % 4 == 0 && vr == 'v1') {
        fbClient.sendCustomEvent([{
          eventName: 'cert-pass-rate',
          numericValue: 1
        }])
      }
      if (ir % 3 == 0 && vr == 'v2') {
        fbClient.sendCustomEvent([{
          eventName: 'cert-pass-rate',
          numericValue: 1
        }])
      }

      const ir2 = random(3);
      if (ir2 % 3 == 0) {
        fbClient.sendCustomEvent([{
          eventName: 'click-review',
          numericValue: 1
        }])
      }
    }

    // const r = random(12);
    // const avg = 3.8 + (r / 12)
    // fbClient.sendCustomEvent([{
    //   eventName: 'avg-rating-score',
    //   numericValue: avg
    // }])

    // console.log('vr', vr)
    if (vr == 'no')
      v0Count++
    if (vr == 'v1')
      v1Count++
    if (vr == 'v2')
      v2Count++

    console.log(v0Count, v1Count, v2Count)

    // console.log('avg', avg)


    await sleep(200);

  }
}
</script>

<template>
  <div class="greetings">
    <button @click="runABTest">run abtest</button>
  </div>
</template>

<style scoped>

</style>