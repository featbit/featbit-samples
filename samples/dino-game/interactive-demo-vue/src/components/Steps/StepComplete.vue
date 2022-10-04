<script>
import { useStepsStore } from '@/stores/steps'

export default {
    components: {
    },
    setup() {
        const store = useStepsStore();
        return {
            store
        };
    },
}
</script>
      
<style lang="less" scoped>
@import "./Steps.less";
</style>      
      
          
<template>
    <div class="complete-step">
        <div class="title">
            <h1>Demo Complete</h1>
        </div>
        <div class="steps">
            <div >
                <p>Congratulations! You have completed the Interactive Demo section of the Quick start section</p>
                <p>Next, we recommend that you check out the <a>Connect an SDK</a> section for a quick overview of how to use FeatBit to connect the Feature flags we've connected in this demo!
                </p>
                <ul>
                    <li><a>Click here to go to the documentation page for Connect an SDK</a></li>
                    <li><a href="https://github.com/featbit/featbit-samples/tree/main/samples/dino-game/interactive-demo-vue"
                        target="_blank">Click here to download the source code for the Interactive Demo</a></li>
                </ul>
                <p>
                    Or return to the FeatBit portal as shown below and check the Quick Start Panel for the rest of lessons.
                    <br/>
                    Note: The content completed in <a>Connect an SDK</a> will be used in most of the articles in How-to guides
                </p>
                <img src="@/assets/imgs/get-started-in-portal.png" />
            </div>
            <div class="steps-action">
                <a-button @click="store.currentStep--">
                    Previous
                </a-button>
            </div>
        </div>
    </div>
</template>