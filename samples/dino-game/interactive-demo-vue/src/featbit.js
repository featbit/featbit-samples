import fbClient from "featbit-js-client-sdk";
import { defineStore } from 'pinia';
import { datadogRum } from '@datadog/browser-rum';

export const flagsDefaultValues = {
    "game-runner": "false",
    "difficulty-mode": "easy"
}

// This helps VUE project to do feature flag usage statistics, this also helps implement an easy way of bootstrap
export const createFlagsProxy = () => {
    return new Proxy({}, {
        get(target, prop, receiver) {
            //
            // Code sample for DataDog rum integration
            //
            // var returnValue =
            //     (typeof prop === 'string' && !prop.startsWith('__v_')) ?
            //         fbClient.variation(prop, flagsDefaultValues[prop] || '') : '';
            // if(typeof prop === 'string'){
            //     console.log(`featbit: ${typeof prop === 'string' ? prop : ''} = ${returnValue}`)
            //     datadogRum.addFeatureFlagEvaluation(prop, returnValue);
            // }
            // return returnValue;

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

        let userSimluateIndex = Date.now();
        let userId = "simluation-user-id-" + userSimluateIndex;
        let userName = "user name " + userSimluateIndex;
        
        fbClient.init({
            secret: envkey,
            api: evaluationUrl,
            user: {
                keyId: userId,
                name: userName,
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

        //
        // datadog rum sample code
        //
        // datadogRum.setUser({
        //     id: userId,
        //     name: userName
        // })
        // datadogRum.init({
        //     applicationId: 'ef15baa3-3d4c-4c9f-843e-4ad92bcd6b4e',
        //     clientToken: 'pubb317bf6eb6d87f8cbab78c9299391288',
        //     site: 'datadoghq.eu',
        //     service:'featbit-feature-flag-rum',
        //     env:'demo',
        //     sessionSampleRate:100,
        //     sessionReplaySampleRate: 20,
        //     trackUserInteractions: true,
        //     trackResources: true,
        //     trackLongTasks: true,
        //     defaultPrivacyLevel:'mask-user-input',
        //     enableExperimentalFeatures: ["feature_flags"],
        // });
        // datadogRum.startSessionReplayRecording();
    }
}

export const useFeatBitStore = defineStore('featbit', {
    state: () => ({
        flags: createFlagsProxy() // with proxy method, states won't be observable in VUE devtool, you can switch to "flags: flagsDefaultValues", but it's not friendly for feature flag usage statistics. You can check localstorage to know the current value of feature flag.
    })
})