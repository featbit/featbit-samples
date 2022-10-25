<script>
import DinoGameCore from './DinoGameCore.vue'
import { useFeatBitStore } from '@/featbit'

export default {
    components: {
        DinoGameCore
    },
    setup() {
        const featBitStore = useFeatBitStore();

        return {
            featBitStore
        }
    }
}
</script>

<style lang="less">
@import "./DinoGame.less";
</style>    
    
<template>
    <div class="dino-game-wrapper">
        <DinoGameCore v-if="featBitStore.flags['game-runner']== true" :difficulty="featBitStore.flags['difficulty-mode']" />
        <div class="comingsoon-wrapper" v-if="featBitStore.flags['game-runner'] == false">
            <h1>Swith "game-runner" feature flag to ON to release Dino Game</h1>
        </div>
    </div>
</template>