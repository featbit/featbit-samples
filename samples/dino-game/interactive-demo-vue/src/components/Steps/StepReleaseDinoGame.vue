<script>
import { useStepsStore } from '@/stores/steps'

export default {
    components: {
    },
    setup() {
        const store = useStepsStore();
        return {
            store
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
            <div class="step1" v-if="store.dinoGame.currentStep == 0">
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
            <div class="step2" v-if="store.dinoGame.currentStep == 1">
                <p>// Release Dino Game</p>
                <p>Back to the flags list portal which contains "game-runner" feature flag.
                    Click on the name or detail link to go to the feature flag targeting page.</p>
                <img src="@/assets/imgs/the-flags-list.png" />
                <p>Switch feature flag from Off to ON. The feature flag will serve variation "true"
                    to the Dino Game. It means the Dino Game (below) will be released immediately.
                </p>
                <img src="@/assets/imgs/ff-switch-on.png" />
                <p>
                    <span style="color: yellow;font-weight: 600;margin-right: 15px;">Success !</span>
                    The Dino Game was just released by you !.
                </p>
            </div>
            <div class="steps-action">
                <a-button v-if="store.dinoGame.currentStep == 0" style="margin-right;: 8px" type="primary"
                    @click="store.dinoGame.currentStep++">Next</a-button>
                <a-button v-if="store.dinoGame.currentStep == 1" @click="store.dinoGame.currentStep--">Previous
                </a-button>
                <a-button v-if="store.task2Enabled === true" type="primary" style="float:right;"
                    @click="store.dinoGame.currentStep--">Next Task</a-button>
            </div>
        </div>
    </div>
</template>