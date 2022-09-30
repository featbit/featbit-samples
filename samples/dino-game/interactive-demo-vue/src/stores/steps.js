import { defineStore } from 'pinia'

export const useStepsStore = defineStore('steps', {
    state: () => ({ 
        currentStep: 0, 
        dinoGame: {
            currentStep: 0,
            gameIsReleased: false
        }
    }),
    getters: {
        showPreviousButton: (state) => state.currentStep > 0,
        showNextButton: (state) => state.currentStep < 2,
    },
    actions: {
        nextStep() {    
            this.currentStep++
        },
        previousStep(){
            this.currentStep--
        }
    },
})