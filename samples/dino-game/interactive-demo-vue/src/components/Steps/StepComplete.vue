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

    </style>    
      
          
    <template>
        <div class="release-runner">
            Complete!
        </div>
    </template>