import fbClient from "featbit-js-client-sdk";
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
                fbClient.variation(prop, flagsDefaultValues[prop] || '') : '';
        }
    })
}

export const featBit = {
    install(app, options) {
        const urlParams = new URLSearchParams(window.location.search);
        let envkey = urlParams.get("envKey");
        let evaluationUrl = urlParams.get("evaluationUrl");
        console.log(envkey)
        console.log(evaluationUrl)
        fbClient.init({
            secret: envkey,
            api: evaluationUrl,
            user: {
                keyId: 'my-user',
                name: 'my user',
                customizedProperties: [
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
        
        fbClient.on("ff_update", (changes) => changes.length ? store.flags = createFlagsProxy() : null);

        fbClient.waitUntilReady().then((changes) => changes.length ? store.flags = createFlagsProxy() : null);
    }
}

export const useFeatBitStore = defineStore('featbit', {
    state: () => ({
        flags: createFlagsProxy() // with proxy method, states won't be observable in VUE devtool, you can switch to "flags: flagsDefaultValues", but it's not friendly for feature flag usage statistics. You can check localstorage to know the current value of feature flag.
    })
})