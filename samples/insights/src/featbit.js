import fbClient from "featbit-js-client-sdk";
import { defineStore } from 'pinia'

export const defaultFlags = {
    "game-runner": false,
}

const flagsProxy = () => {
    return new Proxy({}, {
        get(target, prop, receiver) {
            return (typeof prop === 'string' && !prop.startsWith('__v_')) ?
                fbClient.variation(prop, defaultFlags[prop] || '') : '';
        }
    })
}

export const useFeatBitStore = defineStore('featbit', {
    state: () => ({ flags: () => flagsProxy() })
})

export const featBit = {
    async install(app, options) {
        fbClient.init({
            secret: 'uZsYcDHE3EyB5UygUlugfwZ-sgT9f4-0G9dGYmhgUpng',
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

        const fbS = useFeatBitStore()
        fbClient.on('ready', (c) => c.length ? fbS.flags = flagsProxy() : null);
        fbClient.on("ff_update", (c) => c.length ? fbS.flags = flagsProxy() : null);
    }
}