<template>
    <div class="newdshop-inv">
        <div class="newdshop-inv-items">
            <div class="newdshop-inv-items_title">{{loc('newdshop:inv:items:tit')}}</div>
            <div class="newdshop-inv-items_list">
                <Item v-for="(item, index) in itemList" :key="index" 
                    :item="item" 
                    :count="+item.count" 
                    :selected="currentItem !== null && currentItem === item"
                    @onSelect="selectItem"
                />
            </div>
        </div>
        <div class="newdshop-inv-info">
            <template v-if="currentItem">
                <div class="newdshop-inv-info_title">
                    {{currentItem.item.name}}
                </div>
                <img v-if="currentItem" class="newdshop-inv-info_img" :src="`/img/newDonateShop/items/${currentItem.item.image}.png`" alt="item">
                <div class="newdshop-inv-info_btns">
                    <div v-if="isStackable" class="newdshop-inv-info_stack">
                        <input class="newdshop-inv-info_input" type="number" v-model="count" min="1" :max="currentItem.count">
                        <div class="newdshop-inv-info_btn2" @mouseenter="playHover" @click="toInvenntory">{{loc('newdshop:inv:info:btn:take')}}</div>
                    </div>
                    <div v-else class="newdshop-inv-info_btn" @mouseenter="playHover" @click="toInvenntory">{{loc('newdshop:inv:info:btn:take')}}</div>
                    <div class="newdshop-inv-info_btn" @mouseenter="playHover"  @click="sell" v-if="canSell">
                        {{loc('newdshop:inv:info:btn:sell')}} X{{currentItem.item.count}} 
                        <div class="newdshop-inv-info_decor"></div> 
                        <img src="/img/newDonateShop/header_coin.png" alt="coin" class="newdshop-inv-info_coin"> 
                        <div class="newdshop-inv-info_price">{{sellPrice.toLocaleString()}}</div>
                    </div>
                </div>
            </template>
        </div>
    </div>
</template>

<script>
import { mapState, mapGetters } from 'vuex'
import Item from './Item'
import {itemById,getRarityPrice} from '../../../configs/newDonateShop'

export default {   
    data() {
        return {
            currentItem: null,
            count: 0,
            spamProtection: 0
        }
    },
    computed:{
        ...mapState("newDonateShop/inventory", ["items"]),
        ...mapGetters('localization', ['loc']),
        itemList(){
            const list = [];
            this.items.forEach(i => {
                const item = itemById(i.id);
                if(item !== undefined)
                    list.push({item, count: i.count, sell: i.sell});
                else 
                    console.log(`badInventoryItem: ${i.id}`)
            });
          
            if(list.length < 36){
                for (let index = list.length; index < 36; index++) {
                    list.push({item:null, count:-1, sell: false})                    
                }
            }
            return list;
        },
        sellPrice(){
            return getRarityPrice(this.currentItem.item.rarity)
        },
        canSell(){
            return this.currentItem.sell && this.currentItem.count >= this.currentItem.item.count
        },
        isStackable(){
            return this.currentItem.item.data.Stackable
        }
    },
    methods: {
        isSpam(){
            if(this.spamProtection > Date.now()) return true;
            this.spamProtection = Date.now() + 1000;
            return false;
        },
        selectItem(item){
            if(this.currentItem === item){
                this.toInvenntory();
            }else{
                this.currentItem = item;
                this.count = 1;
                this.playClick();
            }
        },
        playHover(){
            if(this.item === null) return;
            this.$store.commit("sounds/play", {"name": "dshop_hover", "volume": .7})
        },
        playClick(){
            this.$store.commit("sounds/play", {"name": "dshop_click", "volume": .7})
        },
        toInvenntory(){
            if(this.isSpam()) return;
            if(this.count < 1 || this.count > this.currentItem.count){
                this.count = 1;
                return;
            }
            window.mp.triggerServer("dshop:inventory:item:use", this.currentItem.item.id, this.currentItem.sell, this.isStackable ? this.count : 1);
            this.currentItem = null;
            this.playClick();
        },
        sell(){
            if(this.isSpam()) return;
            window.mp.triggerServer("dshop:inventory:item:sell", this.currentItem.item.id, this.currentItem.sell)
            this.currentItem = null;
            this.playClick();
        },
        keyHandler(e){
            if(e.keyCode == 27)
                this.$emit("onCloseMenu");
        }
    },
    components:{
        Item
    },
    mounted(){
        addEventListener("keyup", this.keyHandler);
    },
    beforeDestroy(){
        removeEventListener("keyup", this.keyHandler);
    }
}
</script>

<style lang="scss">
.newdshop-inv{
   text-transform: uppercase;
    display: flex;
    justify-content: space-between;
    padding: 0 5rem;
    width: 100%;
    height: 100%;
    background: url(/img/optionsMenu/bg.png) no-repeat center;
    background-size: cover;
    &-items{
        width: 37rem;
        &_title{
            margin-top: 5rem;
            font-size: 3rem;
        }
        &_list{
            max-height: 40rem;
            display: flex;
            flex-wrap: wrap;
            overflow: hidden scroll;
            &::-webkit-scrollbar {
                width: 0;
            }
                &::-webkit-scrollbar-track {
                border: solid 0 transparent;
                margin: .1rem;
            }
                &::-webkit-scrollbar-thumb {
                border: solid 0 transparent;      
                background-color: rgba(#fff,.1);
            }
        }
    }
    &-info{
        width: 45rem ;
        height: 45rem;
        background: url(/img/newDonateShop/inventory/info_bg.svg) no-repeat center;
        background-size: contain;
        display: flex;
        align-items: center;
        justify-content: center;
        flex-direction: column;
        &_img{
            height: 13rem;
        }
        &_title{
            position: absolute;
            top: 6rem;
            
            font-size: 2rem;
            text-shadow: -.5rem -.5rem 2rem rgba(255, 255, 255, 0.5);
        }
        &_btns{
            position: absolute;
            bottom: 6rem;
        }
        &_btn{
            width: 19rem;
            height: 3.1rem;
            display: flex;
            align-items: center;
            justify-content: center;
            background: rgba(255, 255, 255, 0.2);
            border-radius: .2rem;
            margin-top: 1rem;
            &:first-child{
                background: #9FBA1B;
            }
            &:hover{
                transform: scale(1.02)
            }
        }
        &_stack{
            display: flex;
            align-items: center;
            justify-content: space-between;
        }
        &_btn2{
            width: 14rem;
            height: 3.1rem;
            display: flex;
            align-items: center;
            justify-content: center;
            border-radius: .2rem;
            margin-top: 1rem;
            background: #9FBA1B;
            &:hover{
                transform: scale(1.02)
            }
        }
        &_input{
            width: 4.5rem;
            height: 3.1rem;
            border-radius: .2rem;
            margin-top: 1rem;
            background: rgba(0, 0, 0, 0.5);
            border: 1px solid rgba(255, 255, 255, 0.5);
            color: #fff;
            text-align: center;
            font-size: 1.5rem;
            text-transform: uppercase;
            &::-webkit-inner-spin-button {
                -webkit-appearance: none;
                margin: 0;
            }
        }
        &_decor{
            width: 1px;
            height: 1.5rem;
            background: rgba(255, 255, 255, 0.2);
            margin: 0 .5rem;
        }
        &_coin{
            margin-right: .5rem;
            filter: drop-shadow(0 0 .2rem #FFF205);
        }
    }
}
</style>