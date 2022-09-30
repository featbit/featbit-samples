import ffcClient from "ffc-js-client-side-sdk";
import { defineStore } from 'pinia'

export const flagsDefaultValues = {
    "game-runner": "false",
    "difficulty-mode": "easy"
}

export const featBit = {
    install(app, options) {
        const _option = Object.assign(
            {
                secret: "ZTczLTFiMTctNCUyMDIyMDkyOTA1MDUwOV9fMTU5X18yMzVfXzQ1MV9fZGVmYXVsdF9lY2RjMA==", // replace with your won secret
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
        ffcClient.on("ff_update", (changes) => changes.length ? store.setFlags(changes) : null);
        ffcClient.waitUntilReady().then((changes) => changes.length ? store.setFlags(changes) : null);
    }
}


export const useFeatBitStore = defineStore('featbit', {
    state: () => ({
        flags: flagsDefaultValues
    }),
    actions: {
        setFlags(changes) {
            changes.map(obj => {
                this.flags[obj.id] = ffcClient.variation(obj.id, flagsDefaultValues[obj.id] || '');
            });
        },
    },
})