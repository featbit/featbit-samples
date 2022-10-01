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
    <div class="game-difficulty">
        <div class="title">
            <h1>Change Game Difficulty</h1>
        </div>
        <div class="steps">
            <div class="step1" v-if="store.currentStep == 2">
                <p>// Task 2: Change Difficulty Mode</p>
                <p>The game launches in easy mode. You can use anothor flag to enable 
                    'normal' mode or 'hard' mode.
                </p>
                <p>
                    Back to the flag list, go to targeting page of feature flag "difficulty mode".
                    Find the 'Default rule' dropdown. Choose a flag variation from the dropdown and save.
                </p>
                <img src="@/assets/imgs/ff-switch-on.png" />
                <p>
                    <span style="color: yellow;font-weight: 600;margin-right: 15px;">Success !</span>
                    Now that FeatBit is connected, we can release the Dino Game now.
                </p>
            </div>
            <div class="steps-action">
                <a-button @click="store.currentStep--">
                    Previous
                </a-button>
                <!-- <a-button v-if="store.currentStep < 3" style="margin-left: 8px" type="primary"
                    @click="store.currentStep++">Next</a-button> -->
                <a-button v-if="featBitStore.flags['difficulty-mode'] === 'hard'" type="primary" style="float:right;"
                    @click="store.currentStep=2">Next Task</a-button>
            </div>
        </div>
    </div>
</template>