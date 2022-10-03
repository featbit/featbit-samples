<script>
import DinoGameCore from './DinoGameCore.vue'
import { useFeatBitStore } from '@/featbit'

export default {
    components: {
        DinoGameCore
    },
    setup() {
        const featBitStore = useFeatBitStore();

        // this.featBitStore.$subscribe((mutation, state) => {
        //     if (this.featBitStore.flags["game-runner"] === true){
        //         let div = document.getElementById('dino-game-core');
        //         console.log(div);
        //         new Runner(".interstitial-wrapper", difficulty == 'hard' ? 26 : (difficulty == 'normal' ? 16 : 6));
        //         console.log('this.featBitStore.flags["game-runner"] === true');
        //     }
        // })


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