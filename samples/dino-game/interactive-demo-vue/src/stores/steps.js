

import { defineStore } from 'pinia'
import { ref, computed } from "vue";
import { useFeatBitStore } from '@/featbit'

export const useStepsStore = defineStore('steps', () => {
    const currentTaskIndex = ref(0)
    const releaseDinoGame = ref({
        currentStep: 0
    })
    const releaseDinoGameDifficulty = ref({
        enabled: false,
        currentStep: 0
    })
    function setDinoGameDifficultyAlive(isActive) {
        this.releaseDinoGameDifficulty.enabled = isActive;
    }
    function setCurrentTaskIndex(index) {
        this.currentTaskIndex = index;
    }

    // subscribe if game-runner is true, enable task2's next-task button
    useFeatBitStore().$subscribe((mutation, state) => {
        useStepsStore().setDinoGameDifficultyAlive(state.flags["game-runner"] || false);
    })

    return {
        currentTaskIndex,
        releaseDinoGame,
        releaseDinoGameDifficulty,
        setDinoGameDifficultyAlive,
        setCurrentTaskIndex
    }
})