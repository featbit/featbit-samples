import fbClient from "featbit-js-client-sdk";
import { defineStore } from 'pinia'

export const flagsDefaultValues = {}

const createFlagsProxy = () => {
    return new Proxy({}, {
        get(target, prop, receiver) {
            return (typeof prop === 'string' && !prop.startsWith('__v_')) ?
                fbClient.variation(prop, flagsDefaultValues[prop] || '') : '';
        }
    })
}

export const featBit = {
    async install(app, options) {
        fbClient.init({
            secret: 'KUN1pJU8i0q0T3SaAs1-fAp4vX5E3XM0iC1dHJgoPR_A',
            api: 'http://localhost:5100',
            user: {
                keyId: 'user-uuid-or-keyid',
                name: 'Love FeatBit',
                customizedProperties: [
                    {
                        "name": "email",
                        "value": "test@featbit.co"
                    }
                ]
            },
        });
        
        const store = useFeatBitStore()
        fbClient.waitUntilReady().then((changes) => changes.length ? store.flags = createFlagsProxy() : null);
        fbClient.on("ff_update", (changes) => changes.length ? store.flags = createFlagsProxy() : null);
    }
}

export const useFeatBitStore = defineStore('featbit', {
    state: () => ({
        flags: () => createFlagsProxy()
    })
})
