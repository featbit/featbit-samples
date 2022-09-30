import { defineStore } from 'pinia'
import { ref, computed } from "vue";
import { useFeatBitStore } from '@/featbit'

export const useStepsStore = defineStore('steps', () => {
    const currentStep = ref(0)
    const dinoGame = ref({
        currentStep: 0,
        gameIsReleased: false
    })
    const task2Enabled = ref(false)

    // const showPreviousButton = computed(() => state.currentStep > 0)
    // const showNextButton = computed(() => state.currentStep < 2)
    function nextStep() {
        currentStep.value++
    }
    function previousStep() {
        currentStep.value--
    }

    const featBitStore = useFeatBitStore();
    // useFeatBitStore.$subscribe((mutation, state) => {
    //     if (featBitStore.flags && featBitStore.flags["game-runner"])
    //         task2Enabled.value = featBitStore.flags["game-runner"];
    // })

    return {
        currentStep,
        dinoGame,
        task2Enabled,
        // showPreviousButton,
        // showNextButton,
        nextStep,
        previousStep
    }
})