<template>
    <div class="newdshop" :class="{'newdshop_background': !tryMode}">
        <div class="newdshop-header">
            <img src="/img/newDonateShop/header_bg.svg" alt="bg" class="newdshop-header-bg">
            <div class="newdshop-header-title">GO COIN SHOP</div>            
            <div class="newdshop-header-nav">
                <div class="newdshop-header-btn" v-for="(btn, index) in buttons1" :key="'a_' + index" @click="selectTab(btn.tab)" @mouseenter="playHover">
                    <div class="newdshop-header-selected" v-if="btn.tab === currentTab" :style="{'background': btn.bg}"></div>
                    {{loc(btn.title)}}
                </div>
                <div class="newdshop-header-decor"></div>
                <div class="newdshop-header-btn" v-for="(btn, index) in buttons2" :key="'b_' + index" @click="selectTab(btn.tab)" @mouseenter="playHover">
                    <div class="newdshop-header-selected" v-if="btn.tab === currentTab" :style="{'background': btn.bg}"></div>
                    {{loc(btn.title)}}
                </div>
                <div class="newdshop-header-inv" @click="selectTab('Inventory')">
                    {{ itemsCount }}
                    <AddItem v-if="newItem" :key="index" :item="newItem"/>
                    </div>
            </div>
            <div class="newdshop-header-money">
                <div class="newdshop-header-money-item">
                    <img src="/img/newDonateShop/header_coin.png" alt="header_coin">
                    <div class="newdshop-header-money-val">{{coins.toLocaleString()}}</div>
                </div>
                <div class="newdshop-header-money-item">
                    <img src="/img/newDonateShop/header_money.svg" alt="header_money">
                    <div class="newdshop-header-money-val">{{money.toLocaleString()}}</div>
                </div>
                <div class="newdshop-header-exchange" @mouseenter="playHover" @click="exchange"><img src="/img/newDonateShop/exchange.svg" alt="exchange"></div>
                <div class="newdshop-header-donate" @mouseenter="playHover" @click="buy">+</div>
            </div>
        </div>
        <div class="newdshop-body" :class="{'newdshop-body-try': tryMode}">
            <transition name="rltmain" mode="out-in">
                <component :is="currentTab" @onCloseMenu="closeDonateMenu"/>            
            </transition>
        </div>
        <ExchangeDialog v-if="exchangeDialog" :data="exchangeDialog" @onClose="exchangeDialog = null"/>
        <div class="newdshop-escexit" v-if="tryMode">Esc for exit</div>
    </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
import Premium from './Premium'
import Roulette from './Roulette'
import Shop from './Shop'
import Inventory from './Inventory'
import Kits from './Kits'
import ExchangeDialog from './ExchangeDialog'
import AddItem from './AddItemEffect'
//import {itemById} from '../../configs/newDonateShop';

export default {
    data() {
        return {
            buttons1:[
                {title: 'ndsh:index:btn:1', tab: "Premium", bg: 'linear-gradient(180deg, rgba(17, 17, 17, 0.126) 0%, rgba(233, 214, 121, 0.6) 100%)'},
                {title: 'ndsh:index:btn:2', tab: "Roulette", bg: 'linear-gradient(180deg, rgba(0, 0, 0, 0.08) 0%, rgba(75, 138, 158, 0.4) 100%)'},
                //{title: 'ndsh:index:btn:3', tab: "Bingo", bg: 'linear-gradient(180deg, rgba(0, 0, 0, 0.14) 0%, rgba(171, 88, 159, 0.7) 99.65%)'},
                //{title: 'ndsh:index:btn:4', tab: "Loterry", bg: 'linear-gradient(180deg, rgba(0, 0, 0, 0.14) 0%, rgba(147, 43, 251, 0.7) 99.65%)'},
                {title: 'ndsh:index:btn:5', tab: "Shop", bg: 'linear-gradient(180deg, rgba(0, 0, 0, 0) 0%, rgba(255, 0, 0, 0.4) 100%)'}
            ],
            buttons2:[
                {title: 'Go coin packs', tab: "Kits", bg: 'linear-gradient(180deg, rgba(0, 0, 0, 0.12) 0%, rgba(217, 169, 33, 0.6) 99.65%)'},
                {title: 'ndsh:index:btn:6', tab: "Inventory", bg: 'linear-gradient(180deg, rgba(0, 0, 0, 0.1) 0%, rgba(212, 255, 0, 0.5) 99.65%)'},
            ],
            exchangeDialog: null,
            minCoinsAmount: 50
        }
    },
    methods: {
        selectTab(tab){
            if(this.tryMode) return;
            this.$store.commit('newDonateShop/setCurrenTab', tab);
            this.playClick();
        },
        playHover(){
            this.$store.commit("sounds/play", {"name": "dshop_hover", "volume": .7});
        },
        playClick(){
            this.$store.commit("sounds/play", {"name": "dshop_click", "volume": .7});
        },
        exchange(){
            this.playClick();
            const course = this.exchangeCources.coinToVirtual;
            this.exchangeDialog = {
                title: "newdshop:coin:to:rub:tit",
                subtitle: "newdshop:coin:to:rub:sub",
                course,
                currency1: "header_coin.png",
                currency2: "header_money.svg",
                img: "exchange1.png",
                event(amount){
                    if(amount < 0) return;
                    if(this.coins < amount / course){                        
                        this.$store.commit('notifyList/notify', {type: 1, position: 0, message: this.loc('dshop:item:buy:wrong:coins'), time: 3000});
                        return;
                    }
                    window.mp.triggerServer("dshop:coins:exch:money", amount)
                }
            }
        },
        buy(){            
            this.playClick();
            const course = this.exchangeCources.rubToCoin;
            this.exchangeDialog = {
                title: "newdshop:rub:to:coin:tit",
                subtitle: "newdshop:rub:to:coin:sub",
                course,
                currency1: "rub.svg",
                currency2: "header_coin.png",
                img: "exchange1.png",
                event(amount){ 
                    if(amount < 0) return;                  
                    if(amount < this.minCoinsAmount){                        
                        this.$store.commit('notifyList/notify', {type: 1, position: 0, message: this.loc(`dshop:coins:buy:min@${this.minCoinsAmount}`), time: 3000});
                        return;
                    }
                    window.mp.triggerServer("dshop:coins:buy:single", amount)
                }
            }
        },
        closeDonateMenu(){
            if(this.exchangeDialog)
                this.exchangeDialog = null;
            else 
                window.mp.trigger("dshop:close");
        }   
    },
    computed:{        
        ...mapGetters('localization', ['loc']),
        ...mapState('newDonateShop', ['coins', 'currentTab', 'tryMode', 'exchangeCources']),
        ...mapState('hud', ['money']),
        ...mapState('newDonateShop/inventory', {inventory: 'items', newItem:'newItem'}),
        itemsCount(){
            return Object.values(this.inventory).reduce((sum, curr)=>sum + +curr.count, 0);
        }
    },
    mounted() {
       
    },
    components:{
        Premium,
        Roulette,
        Shop,
        Inventory,
        Kits,
        ExchangeDialog,
        AddItem
    }
}
</script>

<style lang="scss">
.newdshop{
    position: fixed;
    left: 0;
    top: 0;
    width: 100vw;
    height: 100vh;
    color: #fff;
    &_background{
        background-color: #212121;
    }
    &-header{
        height: 4.1rem;
        width: 100vw;
        justify-content: space-between;
        align-items: center;
        display: flex;
        background-color: #000;
        padding: 0 3rem;
        &-bg{
            position: absolute;
            left: 1rem;
            top: 0;
        }
        &-title{
            color: #fff;
            
            font-size: 2rem;
        }
        &-nav{
            display: flex;
            align-items: center;
            justify-content: space-between;
        }
        &-btn{
            padding: 0 .9rem;
            position: relative;
            font-size: 1rem;
            height: 4.1rem;
            display: flex;
            align-items: center;
            justify-content: space-between;
            &:hover{
                background-color: rgba(#fff,.1);
            }
        }
        &-money{
            display: flex;
            justify-content: center;
            align-items: center;
            &-item{
                display: flex;
                justify-content: center;
                align-items: center;
                margin-right: .5rem;
                img{
                    margin-right: .5rem;
                }
            }
        }
        &-donate{
            border-radius: 50%;
            border: 2px solid #B6D300;
            width: 2rem;
            height: 2rem;
            display: flex;
            align-items: center;
            justify-content: center;
            &:hover{
                background-color: rgba(#fff, .1);
            }
        }
        &-exchange{
            border-radius: 50%;
            border: 2px solid #5B5D6E;
            width: 2rem;
            height: 2rem;
            display: flex;
            align-items: center;
            justify-content: center;
            margin: 0 .57rem;
            &:hover{
                background-color: rgba(#fff, .1);
            }
        }
        &-decor{
            height: 2.2rem;
            width: 1px;
            margin: 0 1rem;
            background-color: rgba(255, 255, 255, 0.5);
        }
        &-inv{
            border-radius: 50%;
            //box-sizing: border-box;
            margin-left: 1rem;
            border: 2px solid #B6D300;
            width: 2rem;
            height: 2rem;
            display: flex;
            align-items: center;
            justify-content: center;
            position: relative;
        }
        &-selected{
            position: absolute;
            bottom: 0;
            left: 0;
            width: 100%;
            height: 70%;
        }
    }
    &-body{
        width: 100vw;
        height: calc(100vh - 4.1rem);
        position: relative;
        transition: all 1s;
        overflow: hidden;
        &-try{
            transform: translateY(-110vh);
        }
    }
    
    &-escexit{
        position: fixed;
        bottom: 3rem;
        left: 0;
        width: 100vw;
        text-align: center;
        font-size: 2.5rem;
        color: rgba(#fff, .6);
    }
    img{        
        -webkit-user-drag: none;
        -o-user-drag: none;
    }
}
.rltmain-leave-active {
    transition: all .3s;
    opacity: 1;
}
.rltmain-enter-active {
    transition: all 1s;
    opacity: 1;
}
.rltmain-leave-to, .rltmain-enter /* .fade-leave-active to version 2.1.8 */ {
    opacity: 0;
}
</style>