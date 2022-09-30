import ffcClient from "ffc-js-client-side-sdk";
import { defineStore } from 'pinia'
import { useStepsStore } from './steps'

export const flagsDefaultValues = {
    // "game-runner": "false",
    // "difficulty-mode": "easy"
}

export const useFeatBitStore = defineStore('featbit', {
    state: () => ({
        flags: flagsDefaultValues
    }),
    getters: {
    },
    actions: {
        setFlags(changes) {
            changes.map(obj => {
                this.flags[obj.id] = ffcClient.variation(obj.id, flagsDefaultValues[obj.id] || '');
            });
        },
    },
})

export const featBit = {
    install(app, options) {
        const _option = Object.assign(
            {
                secret: "ZTczLTFiMTctNCUyMDIyMDkyOTA1MDUwOV9fMTU5X18yMzVfXzQ1MV9fZGVmYXVsdF9lY2RjMA==", // replace with your won secret
                // anonymous: false,
                user: {
                    id: 'my-user',
                    userName: 'my user',
                    email: '',
                    customizedProperties: [
                        {
                            "name": "sex",
                            "value": "male"
                        }]
                },
                bootstrap: Object.keys(flagsDefaultValues).map(obj => ({
                    id: obj,
                    variation: flagsDefaultValues[obj],
                    variationOptions: []
                }))
            }
        );
        ffcClient.init(_option);

        const store = useFeatBitStore()

        ffcClient.on("ff_update", (changes) => {
            if (changes.length) {
                store.setFlags(changes)
            }
        });

        ffcClient.waitUntilReady().then((data) => {
            if (data.length) {
                store.setFlags(data)
            }
        });
    },
    setup() {

    }
}


