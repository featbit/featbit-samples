import fbClient from "featbit-js-client-sdk";
import { defineStore } from 'pinia';
import { datadogRum } from '@datadog/browser-rum';

export const flagsDefaultValues = {
    // "game-runner": "false",
    // "difficulty-mode": "easy"
}

// This helps VUE project to do feature flag usage statistics, this also helps implement an easy way of bootstrap
export const createFlagsProxy = () => {
    return new Proxy({}, {
        get(target, prop, receiver) {
            var returnValue =
                (typeof prop === 'string' && !prop.startsWith('__v_')) ?
                    fbClient.variation(prop, flagsDefaultValues[prop] || '') : '';
            console.log(`featbit: ${typeof prop === 'string' ? prop : ''} = ${returnValue}`)
            return returnValue;
        }
    })
}

export const featBit = {
    install(app, options) {
        // const urlParams = new URLSearchParams(window.location.search);
        // let envkey = urlParams.get("envKey");
        // let evaluationUrl = urlParams.get("evaluationUrl");        
        let envkey = 'gvkuIffZRkWoXWM-VumvsAJGumzSi8qUeo7MWeDXG0jQ';
        let evaluationUrl = 'http://localhost:5100';
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


        datadogRum.init({
            applicationId: 'ef15baa3-3d4c-4c9f-843e-4ad92bcd6b4e',
            clientToken: 'pubb317bf6eb6d87f8cbab78c9299391288',
            site: 'datadoghq.eu',
            service:'featbit-feature-flag-rum',
            env:'demo',
            // Specify a version number to identify the deployed version of your application in Datadog 
            // version: '1.0.0', 
            sessionSampleRate:100,
            sessionReplaySampleRate: 20,
            trackUserInteractions: true,
            trackResources: true,
            trackLongTasks: true,
            defaultPrivacyLevel:'mask-user-input'
        });
            
        datadogRum.startSessionReplayRecording();
    }
}

export const useFeatBitStore = defineStore('featbit', {
    state: () => ({
        flags: createFlagsProxy() // with proxy method, states won't be observable in VUE devtool, you can switch to "flags: flagsDefaultValues", but it's not friendly for feature flag usage statistics. You can check localstorage to know the current value of feature flag.
    })
})