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
            <div >
                <p>// Task 2: Change Difficulty Mode</p>
                <p>The game launches in easy mode. You can use anothor flag to enable
                    'normal' mode or 'hard' mode.
                </p>
                <p>
                    Back to the flag list, go to targeting page of feature flag "difficulty mode". Turn on the feature flag, and
                    find the 'Default rule' dropdown. Choose a flag variation (<strong>"hard"</strong> or
                    <strong>"normal"</strong> but not "easy") from the dropdown and save.
                </p>
                <img src="@/assets/steps/ff-switch-difficulty.png" />
            </div>
            <div class="steps-action">
                <a-button @click="store.currentStep--">
                    Previous
                </a-button>
                <a-button v-if="featBitStore.flags['difficulty-mode'] !== 'easy'" type="primary" style="float:right;"
                    @click="store.currentStep=3">Next Task</a-button>
            </div>

            <div v-if="featBitStore.flags['difficulty-mode'] !== 'easy'" 
                 style="margin-top:20px;margin-bottom: 5px;">
                <p style="font-size: 27px;margin-bottom:0px;">
                    <span style="color: yellow;font-weight: 600;margin-right: 10px;">Success !</span>
                    The game difficulty mode has been changed. Try the game once again !
                </p>
            </div>
        </div>
    </div>
</template>