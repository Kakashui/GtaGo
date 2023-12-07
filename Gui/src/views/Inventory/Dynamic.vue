<template>
    <div class="dynamic">        
        <component 
            :is="dynamicType"
            :drag="darag"
            @onMouseDown="onMouseDown"
            @onDoubleClick="onDoubleClick"
        />
    </div>
</template>

<script>
import Equip from './Equip'
import Stock from './Stock'
import { mapState } from 'vuex'

export default {
    props: ['darag'],
    methods: {
        onMouseDown(dragObject){
            this.$emit("onMouseDown", dragObject);
        },
        onDoubleClick(adress, item){
            this.$emit("onDoubleClick", adress, item);
        }
    },
    computed:{
        ...mapState("inventory", ["dynamicType", "stock"])
    },
    components:{
        Equip,
        Stock
    }
}
</script>

<style lang="scss" scoped>
.dynamic{
    width: 24rem;
    height: 100%;
    &-tittle{
        font-weight: bold;
        color: #fff;
        font-size: 1.3rem;
        width: 100%;
        margin-bottom: .5rem;
    }
}
</style>