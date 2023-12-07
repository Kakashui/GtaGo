<template>
    <div class="newdshop-kit">
        <div class="newdshop-kit-item" v-for="(item, index) in coinKits" :key="index" 
            :style="{'background': `url(/img/newDonateShop/kits/${item.Id}.png)`}" 
            @click="selectKit(item)"
            @mouseenter="playHover"
        >
            <div class="newdshop-kit-item_coins">                
                <img src="/img/newDonateShop/header_coin.png" alt="coin" class="newdshop-kit-item_coin">
                {{item.Coins.toLocaleString()}}
            </div>
            <div class="newdshop-kit-item_bonus">    
                <span>Bonus</span>          
                <img src="/img/newDonateShop/header_coin.png" alt="coin" class="newdshop-kit-item_bon">
                {{(item.Coins - item.Price).toLocaleString()}}
            </div>
            <div class="newdshop-kit-item_btn">
                <span>{{loc('newdshop:kits:item:buy')}}</span> <span>{{item.Price.toLocaleString('ru-RU', { style: 'currency', currency })}}</span>
            </div>
        </div>
        <ConfirmDialog v-if="confirm" :data="confirm" @onClose="confirm = null"/>
    </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
//import {getCoinKitsConfig} from '../../../configs/newDonateShop'
import ConfirmDialog from '../ConfirmDialog'

export default {
    computed:{
        ...mapGetters('localization', ['loc']),
        ...mapState('newDonateShop', ['currency', 'exchangeCources', 'coinKits'])        
    },
    data() {
        return {
            confirm: null
        }
    },
    methods: {
        selectKit(config){
            this.confirm = {
                title: `ndshop:kit:title@${config.Coins}`,
                price: config.Price,
                img: `coinskit.png`,
                event: "dshop:coins:kit:buy",
                id: config.Id,
                color: "#EDCC6D",
                noCheck: true
            }
            this.playClick();
        },
        playHover(){
            this.$store.commit("sounds/play", {"name": "dshop_hover", "volume": .7});
        },
        playClick(){
            this.$store.commit("sounds/play", {"name": "dshop_click", "volume": .7});
        },
        keyHandler(e){
           if(e.keyCode == 27){
                if(this.confirm)
                    this.confirm = null;
                else
                    this.$emit("onCloseMenu")
           }   
        }
    },
    components:{
        ConfirmDialog
    },
    created(){
        addEventListener("keyup", this.keyHandler);
    },
    beforeDestroy(){
        removeEventListener("keyup", this.keyHandler);
    },
}
</script>

<style lang="scss">
.newdshop-kit{
    width: 100%;
    height: 100%;
    display: flex;
    align-items: center;
    justify-content: center;
    background: url(/img/newDonateShop/kits/bg.png) no-repeat center;
    background-size: cover;
   text-transform: uppercase;
    &-item{
        width: 16.5rem;
        height: 28.8rem;
        margin: .5rem;
        box-sizing: border-box;
        position: relative;
        &:nth-child(1){
            border: 2px solid rgba(75, 138, 158, 0.5);
            &:hover{
                box-shadow:(0px 0px .8rem rgba(75, 138, 158, 0.7));
            }
        }
        &:nth-child(2){
            border: 2px solid rgba(75, 138, 158, 0.5);
            &:hover{
                box-shadow:(0px 0px .8rem rgba(75, 138, 158, 0.7));
            }
        }
        &:nth-child(3){
            border: 2px solid rgba(#EDCC6D, 0.5);
            &:hover{
                box-shadow:(0px 0px .8rem rgba(#EDCC6D, 0.7));
            }
        }
        &:nth-child(4){
            border: 2px solid rgba(#EDCC6D, 0.5);
            &:hover{
               box-shadow:(0px 0px .8rem rgba(#EDCC6D, 0.7));
            }
        }
        &:nth-child(5){
            border: 2px solid rgba(#EDCC6D, 0.5);
            &:hover{
                box-shadow:(0px 0px .8rem rgba(#EDCC6D, 0.7));
            }
        }
        &_btn{
            width: 15rem;
            height: 3rem;
            position: absolute;
            bottom: .75rem;
            left: .75rem;
            display: flex;
            align-items: center;
            justify-content: space-between;
            background: #9C8953;
            border-radius: .2rem;
            padding: 0 2rem;
            
        }
        &_coins{
            margin-top: 12rem;
            height: 4.6rem;
            width: 100%;
            display: flex;
            justify-content: center;
            align-items: center;
            background: rgba(34, 34, 34, 0.6);
            color: #EDCC6D;
            font-size: 2.5rem;
            text-shadow: 0px 0px 1rem #EDCC6D;
        }
        &_coin{
            margin: 0 .5rem;
            width: 2rem;
            height: 2rem;
        }
        &_bonus{
            height: 3.5rem;
            display: flex;
            align-items: center;
            justify-content: center;
            background: rgba(255, 255, 255, 0.1);
            color: #EDCC6D;
            text-shadow: 0px 0px 1rem #EDCC6D;
            span{
                color: #FFF;
                margin: 0 .5rem;
                
            }
        }
        &_bon{
            margin: 0 .5rem;
        }
    }
}
</style>