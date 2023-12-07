<template>
    <div class="customiztion-pframe">
        <img class="customiztion-pframe_item" :src="`/img/customization/parents/${father}.png`" alt="father">
        <img class="customiztion-pframe_item" :src="`/img/customization/parents/${mother}.png`" alt="mother">
    </div>
</template>

<script>
import { mapState } from 'vuex'
export default {  
    computed:{
        ...mapState("customization", ["categories"]),
        items(){
            return this.categories[1].items;
        },
        father(){
            return this.items[0].itemData.items[this.items[0].itemData.value]
        },
        mother(){
            return this.items[1].itemData.items[this.items[1].itemData.value]
        }
    }
}
</script>

<style lang="scss">
.customiztion-pframe{
    position: absolute;
    bottom: 2rem;
    right: 2rem;
    width: 17.7rem;
    height: 10rem;
    background: url(/img/customization/common/parentFrame.png) no-repeat center;
    background-size: cover;
    &_item{
        height: 7.5rem;
        width: 7.5rem;
        position: absolute;
        bottom: .1rem;
        &:nth-child(1){
            right: 2rem;
        }
        &:nth-child(2){
            left: 2rem;            
        }
    }
}
</style>