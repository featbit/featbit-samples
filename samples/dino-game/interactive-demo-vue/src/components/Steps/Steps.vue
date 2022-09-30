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
    <div class="steps-wrapper">
        
    </div>
</template>