

import { defineStore } from 'pinia'
import { ref, computed } from "vue";

export const useStepsStore = defineStore('steps', () => {
    const currentStep = ref(0)
    const changeDifficultyTaskEnabled = ref(false)
    const completeTaskEnabled = ref(false)

    const taskIndex = computed(() => {
        if (currentStep.value < 2)
            return 0;
        else if (currentStep.value < 4)
            return 1;
        else
            return 2;
    })

    return {
        currentStep,
        changeDifficultyTaskEnabled,
        completeTaskEnabled,
        taskIndex
    }
})