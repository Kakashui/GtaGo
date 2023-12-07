<template>
    <div class="customiztion-nav-imglist">
        
        <div class="customiztion-nav-imglist_item" 
            v-for="(img, index) in itemData.items" 
            :key="index"
            :class="{'customiztion-nav-imglist_selected': index === itemData.value}"
            @click="onSelect(index)"
        >
            <img class="customiztion-nav-imglist_img" :src="`/img/customization/${folder}/${img}.png`" alt="category">
        </div>
    </div>
</template>

<script>
import {mapState} from 'vuex'

export default {
    props:{
        itemData: Object
    },
    computed:{
        ...mapState("customization", ["gender"]),
        folder(){
            return this.gender ? this.itemData["folderm"] : this.itemData["folderf"];
        }
    },
    methods: {
        onSelect(index){
            this.itemData.value = index;
            this.$emit("onSelect", this.itemData.tag, this.itemData.items[this.itemData.value])
        }
    },
}
</script>

<style lang="scss">
.customiztion-nav-imglist{
    width: 15.5rem;
    height: 15rem;
    overflow-y: scroll;
    padding-right: .5rem;
    display: flex;
    flex-wrap: wrap;
    align-items: center;
    // justify-content: space-between;
    border-bottom: 1px solid rgba(#fff, .3) ;
    margin: .5rem 0;
    &::-webkit-scrollbar {
      width: .2rem;
    }
    &::-webkit-scrollbar-track {
      border: solid 0 transparent;
      margin: 0;
    }
    &::-webkit-scrollbar-thumb {
      border: solid 0 transparent;      
      background-color: #B6D300;
    }
    &_item{
        width: 4.7rem;
        height: 4.7rem;
        position: relative;
        background-color: rgba(#000, .9);
        margin: .1rem;
        border: .1rem solid rgba(#000, .9);
        &:hover{
            background-color: rgba(#fff, .5);
        }
    }
    &_img{
        width: 100%;
        height: 100%;
    }
    &_selected{
        border: .1rem solid #B6D300;
    }
}
</style>