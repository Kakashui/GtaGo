<template>
    <div class="newdshop-shop-common-item" :class="{'newdshop-shop-common-item_excl': exclusive}" @mouseenter="playHover" @click="selectItem">
        <div class="newdshop-shop-common-item_img">
            <img :src="`/img/newDonateShop/items/${item.image}.png`" alt="item" :style="{'filter': `drop-shadow(0 0 .2rem ${item.color})`}">
        </div>
        <div class="newdshop-shop-common-item_name">
            {{item.name}}
        </div>
        <div class="newdshop-shop-common-item_price">
            {{price.toLocaleString()}} 
            <img src="/img/newDonateShop/header_coin.png" alt="header_coin">
        </div>
    </div>
</template>

<script>
import { mapState } from 'vuex';
import {itemPriceById} from '../../../configs/newDonateShop'


export default {
    props:{
        item: Object,
        exclusive: Boolean
    },
    methods: {
        playHover(){
            this.$store.commit("sounds/play", {"name": "dshop_hover", "volume": .7})
        },
        selectItem(){
            this.$emit("onItemSelect", this.item);
        }
    },
    computed:{
        ...mapState('newDonateShop', ['updatedPrices']),
        price(){
            return itemPriceById(this.item.id, this.updatedPrices)
        }
    }
}
</script>


<style lang="scss">
.newdshop-shop-common-item{
    width: 13.5rem;
    height: 23rem;
    margin: .5rem;
    position: relative;
    background: rgba(10, 11, 12, 0.5);
    border: 1px solid rgba(255, 255, 255, 0.1);
    box-shadow: 0px 0px .2rem rgba(75, 138, 158, 0.5);
    &:hover{
        border: 4px solid #4B8A9E;
        box-sizing: border-box;
        box-shadow: 0px 0px .5 #4B8A9E;
        transform: scale(1.02);
    }
    &_img{
        height: 16rem;
        display: flex;
        align-items: center;
        justify-content: center;
        width: 100%;
        img{
            width: 70%;
        }
    }
    &_name{
        padding: 1rem 0;
        width: 100%;
        text-align: center;
        border-top: 2px solid rgba(75, 138, 158, 0.5);
        border-bottom: 2px solid rgba(75, 138, 158, 0.5);
        font-size: 1rem;
        
    }
    &_price{
        font-size: 2rem;
        font-weight: 300;
        text-shadow: 0 0 .2rem #fff;
        width: 100%;
        height: 3.5rem;
        display: flex;
        justify-content: center;
        align-items: center;
        img{
            margin-left: .5rem;
        }
    }
    &_excl{
        border: 2px solid rgba(#C9AD5E, 1);
        box-shadow: inset 0 0 .6rem rgba(#C9AD5E, .3);
        &:hover{
            border: 2px solid rgba(#C9AD5E, 4);
            box-shadow: inset 0 0 6rem rgba(#C9AD5E, .3);            
        }
    }
}
</style>