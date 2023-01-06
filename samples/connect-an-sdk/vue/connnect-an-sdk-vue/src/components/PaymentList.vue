<script setup>
import { useFeatBitStore } from '@/featbit'

defineProps({
  msg: {
    type: String,
    required: true
  }
})

const features = useFeatBitStore();
</script>

<template>
  <div class="greetings">
    <ul class="payment-list">
        <li v-if="features.flags['apple-pay'] == true">
            <AppleyPayButton />
        </li>
        <li v-if="features.flags['wechat-pay'] == true">
            <WechatPayButton />
        </li>
        <li v-if="features.flags['stripe'] == true">
            <StripeButton />
        </li>
    </ul>  
  </div>
</template>

<style scoped>
h1 {
  font-weight: 500;
  font-size: 2.6rem;
  top: -10px;
}

h3 {
  font-size: 1.2rem;
}

.greetings h1,
.greetings h3 {
  text-align: center;
}

@media (min-width: 1024px) {

  .greetings h1,
  .greetings h3 {
    text-align: left;
  }
}
</style>
