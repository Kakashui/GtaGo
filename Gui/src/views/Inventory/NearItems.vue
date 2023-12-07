<template>
    <div class="near">
        <div class="near-tittle">
            {{loc("inv_near_tittle")}}
        </div> 
        <div class="near-subtittle">
            {{loc("inv_near_sublittle")}}
        </div>
        <ListItems 
            :id="0"
            :drag="drag"
            :width="2"             
            :sortByIndex ="false"
            :canDrop="false"
            @onMouseDown="onMouseDown"  
            @onDoubleClick="onDoubleClick"

        />        
    </div>
</template>

<script>
import { mapGetters } from 'vuex';
import ListItems from './ListItems';


export default {
    props: ['drag'],
    methods: {
        onMouseDown(dragObject){
            this.$emit("onMouseDown", dragObject);
        },
        onDoubleClick(adress, item){
            this.$emit("onDoubleClick", adress, item);
        }
    },
    components:{
        ListItems
    },
    computed: {
        items(){
            const items = [...this.inventory.items];
            return items;
        },
        ...mapGetters('localization', ['loc'])
    },
}
</script>

<style lang="scss" scoped>
    .near{
        width: 11rem;
        height: 100%;
        &-tittle{
           font-size: 50px;
            line-height: 50px;
            letter-spacing: 0.03em;
            color: #FFFFFF;
        }
        &-subtittle{
            font-size: .9rem;
            line-height: 1.2rem;
            letter-spacing: 0.03em;
            color: rgba(255, 255, 255, 0.5);
            text-transform: none;
            margin: .3rem 0 1.2rem 0;
        }   
    }
</style>