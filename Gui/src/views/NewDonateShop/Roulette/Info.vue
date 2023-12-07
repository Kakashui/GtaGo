<template>
    <div class="newdshop-roull-info">
        <div class="newdshop-roull-info_subtitle">roulette</div>
        <img class="newdshop-roull-info_close" src="/img/newDonateShop/roulette/close.svg" alt="close" @click="close" >
        <div class="newdshop-roull-info_title">{{info.title}}</div>
        <div class="newdshop-roull-info_list">
            <Item class="newdshop-roull-info_item" v-for="(item, index) in items" :key="index" :item="item"/>
        </div>
    </div>
</template>

<script>
import Item from './Item'
import {rouletteItemsById} from '../../../configs/newDonateShop'
import { mapState } from 'vuex'

export default {
    props:{
        info: Object
    },
    computed: {
        ...mapState('newDonateShop', ['gender']),
        items(){
            return rouletteItemsById(this.info.id, this.gender)
        }
    },
    methods: {
        close(){
            this.$emit("onClose")
        }
    },
    components:{
        Item
    }
}
</script>

<style lang="scss">
    .newdshop-roull-info{
        position: absolute;
        background-color: #111;
        width: 60rem;
        height: 45rem;
        margin-top: 4rem;
        box-shadow: 0 0 3rem 0 rgb(75, 138, 158);
        border-radius: .3rem;
        z-index: 5;
        &_list{
            padding: 2rem 0 6rem;
            mask-image: linear-gradient(rgba(#fff, 0), rgba(#fff, 1), rgba(#fff, 0) 91%, rgba(#fff, 0) 100%);
            width: 100%;
            margin: auto;
            height: 38rem;
            display: flex;
            flex-wrap: wrap;
            justify-content: center;
            overflow: hidden scroll;
            &::-webkit-scrollbar {
                width: .2rem;
            }
            &::-webkit-scrollbar-track {
                border: solid 0 transparent;
            }
            &::-webkit-scrollbar-thumb {
                border: solid 0 transparent;      
                background-color: rgba(#fff,.4);
                border-radius: .1rem;
            }
        }
        &_subtitle{
            font-size:.8rem;
            color: rgba(#fff,.5);
            text-align: center;
            margin-top: 2rem;
        }
        &_title{
            font-size: 3rem;
            text-align: center;
        }
        &_item{
            height: 8.5rem;
            width: 8.5rem;
            margin: 1rem;
        }
        &_close{
            position: absolute;
            top: 0;
            right: -2.5rem;
            &:hover{
                filter: drop-shadow(0 0 .1rem #fff);
            }
        }
    }
</style>