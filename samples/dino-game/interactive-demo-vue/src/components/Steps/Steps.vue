<script>
import { useStepsStore } from '@/stores/steps'
import StepReleaseDinoGame from './StepReleaseDinoGame.vue'
import StepGameDifficulty from './StepGameDifficulty.vue'
import StepComplete from './StepComplete.vue'

export default {
    components: {
        StepReleaseDinoGame,
        StepGameDifficulty,
        StepComplete
    },
    setup() {
        const store = useStepsStore();
        // const {
        //     currentStep,
        //     nextStep,
        //     previousStep,
        //     showNextButton,
        //     showPreviousButton
        // } = store;
        // return {
        //     currentStep,
        //     nextStep,
        //     previousStep,
        //     showNextButton,
        //     showPreviousButton
        // };
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
    <div class="steps-wrapper">
        <a-steps :current="store.currentStep">
            <a-step title="Release Dino Game">
            </a-step>
            <a-step title="Change Game Difficulty" />
            <a-step title="Demo complete" />
        </a-steps>
        <div class="steps-content">
            <StepReleaseDinoGame v-if="store.currentStep == 0" />
            <StepGameDifficulty v-if="store.currentStep == 1" />
            <StepComplete v-if="store.currentStep == 2" />
        </div>
        <div class="steps-action">
            <a-button v-if="store.showNextButton == true" 
                      style="margin-right;: 8px"
                      type="primary" @click="store.nextStep">Next Step</a-button>
            <a-button v-if="store.showPreviousButton == true" @click="store.previousStep()">Previous
            </a-button>
        </div>
    </div>
</template>