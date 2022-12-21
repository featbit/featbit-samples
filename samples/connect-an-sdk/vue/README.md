a. Create a basic vue project, and includes Pinia in the project.

```
npm init vue@latest
```
<img src='https://user-images.githubusercontent.com/68597908/208920573-0bd5a510-d732-4a7d-bda2-8315c2a6f9ad.png' width='668px' />

b. Install project by running `npm install`.
c. Install FeatBit javascript SDK

```
npm install featbit-js-client-sdk
```
d. Create a file `featbit.js` under folder `src`
e. Copy code below into file `featbit.js`

```javascript
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

        const fbS = useFeatBitStore()
        fbClient.on('ready', (c) => c.length ? fbS.flags = flagsProxy() : null);
        fbClient.on("ff_update", (c) => c.length ? fbS.flags = flagsProxy() : null);
    }
}
```

f. Under folder `src/components/`, find file file `HelloWorld.vue`. Insert code below into file

```javascript
import { useFeatBitStore } from '@/featbit'

const featureStore = useFeatBitStore();

<h2 v-if="featureStore.flags['game-runner'] === true" 
    style="margin-top:30px;color:darkblue;">
    You connected to FeatBit !!!
</h2>
```

> Steps above introduced a very basic usage of SDK which to test connection between SDK and server.
> 
> Check more usage with [documentation](https://featbit.gitbook.io/docs/getting-started/4.-connect-an-sdk/client-side-sdks-for-web-app#vue) and our [Dino Game demo](https://github.com/featbit/featbit-samples/tree/connectansdk/vue/samples/dino-game/interactive-demo-vue)
