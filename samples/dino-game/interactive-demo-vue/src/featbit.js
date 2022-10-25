import ffcClient from "ffc-js-client-side-sdk";
import { defineStore } from 'pinia'

export const flagsDefaultValues = {
    "game-runner": "false",
    "difficulty-mode": "easy"
}

// This helps VUE project to do feature flag usage statistics, this also helps implement an easy way of bootstrap
export const createFlagsProxy = () => {
    return new Proxy({}, {
        get(target, prop, receiver) {
            return (typeof prop === 'string' && !prop.startsWith('__v_')) ?
                ffcClient.variation(prop, flagsDefaultValues[prop] || '') : '';
        }
    })
}

export const featBit = {
    install(app, options) {
        let envkey = window.location.search.substring(1).replace('key=', ''); // http://localhost:5173?key=ZTczLTFiMTctNCUyMDIyMDkyOTA1MDUwOV9fMTU5X18yMzVfXzQ1MV9fZGVmYXVsdF9lY2RjMA==
        
        ffcClient.init({
            secret: envkey,
            user: {
                id: 'my-user',
                userName: 'my user',
                customizedProperties: [
                    {
                        "name": "kamar",
                        "value": "100"
                    },
                    {
                        "name": "Kamar",
                        "value": "100"
                    },
                    {
                        "name": "frequency",
                        "value": "3.5"
                    },
                    {
                        "name": "subLevel",
                        "value": "Free"
                    },
                    {
                        "name": "orgId",
                        "value": "org-001-AA"
                    },
                ]
            },
        });
        
        const store = useFeatBitStore()
        ffcClient.on("ff_update", (changes) => changes.length ? store.flags = createFlagsProxy() : null);
        ffcClient.waitUntilReady().then((changes) => changes.length ? store.flags = createFlagsProxy() : null);
    }
}

export const useFeatBitStore = defineStore('featbit', {
    state: () => ({
        flags: createFlagsProxy() // with proxy method, states won't be observable in VUE devtool, you can switch to "flags: flagsDefaultValues", but it's not friendly for feature flag usage statistics. You can check localstorage to know the current value of feature flag.
    })
})