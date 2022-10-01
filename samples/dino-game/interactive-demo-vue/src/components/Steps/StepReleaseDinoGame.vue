<script>
import { useStepsStore } from '@/stores/steps'
import { useFeatBitStore } from '@/featbit'

export default {
    components: {
    },
    setup() {
        const store = useStepsStore();
        const featBitStore = useFeatBitStore();

        return {
            store,
            featBitStore
        };
    },
}
</script>
      
<style lang="less" scoped>
@import "./Steps.less";
</style>    
      
          
<template>
    <div class="release-runner">
        <div class="title">
            <h1>Releasing Dino Game</h1>
        </div>
        <div class="steps">
            <div class="step1" v-if="store.currentStep == 0">
                <p>// Set up</p>
                <p>We're connecting your FeatBit account to Dino Game so you can use
                    feature flags to release and manage the game.
                </p>
                <p>
                    Using your client-side ID to initiate a connection...
                </p>
                <p>
                    <span style="color: yellow;font-weight: 600;margin-right: 15px;">Success !</span>
                    Now that FeatBit is connected, we can release the Dino Game now.
                </p>
            </div>

            <div class="step2" v-if="store.currentStep == 1">
                <p>// Release Dino Game</p>
                <p>Back to the flags list portal which contains "game-runner" feature flag.
                    Click on the name or detail link to go to the feature flag targeting page.</p>
                <img src="@/assets/imgs/the-flags-list.png" />
                <p>Switch feature flag from Off to ON. The feature flag will serve variation "true"
                    to the Dino Game. It means the Dino Game (below) will be released immediately.
                </p>
                <img src="@/assets/imgs/ff-switch-on.png" />
            </div>

            <div class="steps-action">
                <a-button v-if="store.currentStep == 1" @click="store.currentStep--">
                    Previous
                </a-button>
                <a-button v-if="store.currentStep == 0" style="margin-right;: 8px" type="primary"
                    @click="store.currentStep++">Next</a-button>
                <a-button v-if="featBitStore.flags['game-runner'] === true" type="primary" style="float:right;"
                    @click="store.currentStep=2">Next Task</a-button>
            </div>

            <div class="step2" v-if="store.currentStep == 1 && featBitStore.flags['game-runner'] === true" style="margin-top:20px;margin-bottom: 5px;">
                <p style="font-size: 27px;margin-bottom:0px;">
                    <span style="color: yellow;font-weight: 600;margin-right: 10px;">Success !</span>
                    Dino Game was released. Look ↓↓↓↓↓↓ <br /> 
                    Press space key to start the game
                </p>
            </div>
        </div>
    </div>
</template>