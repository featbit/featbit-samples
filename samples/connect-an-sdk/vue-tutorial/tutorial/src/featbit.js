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
            secret: 'YLTN6P8iz0Ch9W2QYNfgng9K3OiGCqQEKhWG24GWnOrw',
            api: 'http://localhost:5100',
            user: {
                keyId: 'user-uuid-or-keyid-' + Date.now(),
                name: 'Love FeatBit ' + Date.now(),
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