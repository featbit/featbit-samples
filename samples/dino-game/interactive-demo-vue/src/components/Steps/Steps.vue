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
        <a-steps :current="store.taskIndex">
            <a-step title="Release Dino Game">
            </a-step>
            <a-step title="Change Game Difficulty" />
            <a-step title="Demo complete" />
        </a-steps>
        <div class="steps-content">
            <StepReleaseDinoGame v-if="store.taskIndex == 0" />
            <StepGameDifficulty v-if="store.taskIndex == 1" />
            <StepComplete v-if="store.taskIndex == 2" />
        </div>
    </div>
</template>