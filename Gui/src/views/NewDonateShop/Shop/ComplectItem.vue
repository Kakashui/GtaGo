<template>
    <div class="newdshop-shop-complitem">
        <div class="newdshop-shop-complitem_img">
            <img :src="`/img/newDonateShop/items/${item.image}.png`" alt="item">
        </div>
        <div class="newdshop-shop-complitem_info">
            <div class="newdshop-shop-complitem_name">{{item.name}}</div>
            <div class="newdshop-shop-complitem_discount">-{{discount}}%</div>
            <div class="newdshop-shop-complitem_row">
                <div class="newdshop-shop-complitem_nprice">{{newPrice.toLocaleString()}}</div>                
                <img class="newdshop-shop-complitem_coin" src="/img/newDonateShop/header_coin.png" alt="header_coin">
                <div class="newdshop-shop-complitem_bprice">{{item.price.toLocaleString()}}</div>
            </div>
        </div>
    </div>
</template>

<script>
import { mapState } from 'vuex';
import {itemPriceById} from '../../../configs/newDonateShop'

export default {
    props:{
        item: Object,
        discount: Number
    },
    computed:{
        ...mapState('newDonateShop', ['updatedPrices']),
        newPrice(){
            return itemPriceById(this.item.id, this.updatedPrices);
        }
    }
}
</script>

<style lang="scss">
.newdshop-shop-complitem{
    width: 11rem;
    height: 11rem;
    border: 1px solid rgba(255, 255, 255, 0.2);
    margin: 0 .5rem;
    position: relative;
    &_img{
        display: flex;
        align-items: center;
        justify-content: center;
        height: 8rem;
        img{
            height: 90%;
        }
    }
    &_name{
        
        font-size: .8rem;
    }
    &_info{        
        background: rgba(34, 34, 34, 0.7);
        height: 3rem;
        padding: .3rem 1rem;
    }
    &_discount{
        position: absolute;
        right: -.5rem;
        bottom: 1rem;
        width: 2.5rem;
        text-align: center;
        font-size: .8rem;
        padding: .3rem 0;
        background-color: #CE1717;
    }
    &_row{
        display: flex;
        align-items: center;
        margin-top: .2rem;
    }
    &_coin{
        width: .9rem;
        height: .9rem;
        margin: 0 .3rem;
    }
    &_nprice{

    }
    &_bprice{
        color: #FF0000;
        font-size: .8rem;
        text-decoration: line-through;
    }
}
</style>