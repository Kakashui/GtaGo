<template>
    <div class="newdshop-shop-common-dialog">
        <div class="newdshop-shop-common-dialog_body">
            <div class="newdshop-shop-common-dialog_img">
                <img :src="`/img/newDonateShop/items/${item.image}.png`" alt="item" :style="{'filter': `drop-shadow(0 0 4rem ${item.color})`}">
            </div>
            <div class="newdshop-shop-common-dialog_info">
                <div class="newdshop-shop-common-dialog_title">{{item.name}}</div>
                <div class="newdshop-shop-common-dialog_price">{{price.toLocaleString()}}<img src="/img/newDonateShop/header_coin.png" alt="header_coin"></div>
                <div class="newdshop-shop-common-dialog_btns">
                    <div class="newdshop-shop-common-dialog_btn" @mouseenter="playHover" @click="action(1)">{{loc('newdshop:shop:dialog:btn:buy')}}</div>
                    <!-- <div class="newdshop-shop-common-dialog_btn" @mouseenter="playHover" @click="action(2)" v-if="item.category === 1">{{loc('newdshop:shop:dialog:btn:try')}}</div> -->
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import {itemPriceById} from '../../../configs/newDonateShop'
import { mapState, mapGetters } from 'vuex';

export default {
    props:{
        item: Object
    },
    computed:{
        ...mapState('newDonateShop', ['updatedPrices']),
        ...mapGetters('localization', ['loc']),
        price(){
            return itemPriceById(this.item.id, this.updatedPrices);
        }
    },
    methods: {
        close(){
            this.$emit("onDialogCallback", this.item, 0);
        },
        action(action){
            this.$emit("onDialogCallback", this.item, action);
        },
        playHover(){
            this.$store.commit("sounds/play", {"name": "dshop_hover", "volume": .7})
        }
    },
}
</script>

<style lang="scss">
    .newdshop-shop-common-dialog{
        position: fixed;
        top: 0;
        left: 0;
        width: 100vw;
        height: 100vh;
        display: flex;
        align-items: center;
        justify-content: center;
        background-color: rgba(#111, .95);
        &_body{
            width: 45rem;
            height: 24.5rem;
            border: 2px solid rgba(255, 255, 255, 0.1);
            position: relative;
            display: flex;
        }
        &_info{
            width: 60%;
        }
        &_img{
            width: 40%;
            height: 100%;
            display: flex;
            align-items: center;
            justify-content: flex-end;
            img{
                width: 70%;
            }
        }
        &_title{
            
            font-size: 2rem;
            width: 100%;
            text-align: center;
            margin-top: 3rem;
        }
        &_price{
            height: 4.5rem;
            display: flex;
            align-items: center;
            justify-content: center;
            color: #EDCC6D;
            text-shadow: 0 0 .3rem #EDCC6D;
            font-weight: 300;
            font-size: 2rem;
            letter-spacing: .04rem;
            img{
                margin-left: .8rem;
            }
        }
        &_btns{
            width: 100%;
            display: flex;
            align-items: center;
            justify-content: center;
        }
        &_btn{
            font-size: 1.2rem;
            width: 8.3rem;
            height: 3.35rem;
            display: flex;
            align-items: center;
            justify-content: center;
            letter-spacing: .08rem;
            border: 2px solid rgba(255, 255, 255, 0.3);
            margin: .5rem;
            &:first-child{
                background-color: #9FBA1B;
                border: 2px solid #9FBA1B;
            }
            &:hover{
                transform: scale(1.02)
            }
        }
    }
</style>