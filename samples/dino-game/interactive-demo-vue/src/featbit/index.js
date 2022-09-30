import {flagsDefaultValues, option} from "./config";
import ffcClient from "ffc-js-client-side-sdk";
import {createFlagsProxy} from "./utils";
import {reactive} from "vue";

const _option = Object.assign(
    {
        anonymous: true,
        enableDataSync: true,
        bootstrap: Object.keys(flagsDefaultValues).map(k => ({
            id: k,
            variation: flagsDefaultValues[k],
            variationOptions: []
        }))
    },
    option || {}
);

ffcClient.init(_option);

export const ffcStore = reactive({
    flags: createFlagsProxy(),
    setFlags(flagsProxy) {
        this.flags = flagsProxy
    }
});

ffcClient.on("ff_update", (changes) => {
    if (changes.length) {
        ffcStore.setFlags(createFlagsProxy());
        console.log(changes);
    }
});

ffcClient.waitUntilReady().then((data) => {
    if (data.length) {
        ffcStore.setFlags(createFlagsProxy());
    }
});






