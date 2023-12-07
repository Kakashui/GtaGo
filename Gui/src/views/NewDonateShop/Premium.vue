<template>
    <div class="newdshop-prem">
        <img :src="girl" alt="girl" class="newdshop-prem-girl">
        <div class="newdshop-prem-block">
            <div class="newdshop-prem-title">
                <img src="/img/newDonateShop/premium/hand.png" alt="hand">
                <div class="newdshop-prem-title-text">Premium</div>
                <div class="newdshop-prem-title-on" v-if="vip">On</div>
                <div v-else class="newdshop-prem-title-off">OFF</div>
            </div>
            <div class="newdshop-prem-decorator"></div>            
            <div class="newdshop-prem-subtitle">{{loc('newdshop:prem:sub')}}</div>
            <img src="/img/newDonateShop/premium/premium.png" alt="premium">
            <div class="newdshop-prem-status" v-if="vip">
                <img src="/img/newDonateShop/premium/lightning.png" alt="lightning">
                {{`${days} ${loc('newdshop:prem:days')}`}}
            </div>
            <div class="newdshop-prem-decorator"></div>
            <div class="newdshop-prem-footer" @click="buyVip">
                <div>{{price.toLocaleString('ru-RU')}}</div>
                <img src="/img/newDonateShop/header_coin.png" alt="header_coin">
                <div class="newdshop-prem-button">{{loc(vip?'newdshop:prem:renew':'newdshop:prem:buy')}}</div>
            </div>
        </div>
        <ConfirmDialog v-if="confirm" :data="confirm" @onClose="confirm = null"/>
    </div>
</template>

<script>
import {mapGetters, mapState} from 'vuex'
import ConfirmDialog from './ConfirmDialog'
import primeAccount from '../../configs/newDonateShop/primeAccount'

export default {
    computed:{
        girl(){
            return `/img/newDonateShop/premium/girl_${this.vip ? 'on': 'off'}.png`;
        },
        ...mapGetters('localization', ['loc']),
        price(){
            return primeAccount.price;
        },
        ...mapState('newDonateShop/prime',['days']),
        vip(){
            return this.days > 0
        }
    },
    methods: {
        buyVip(){
            this.confirm = {
                title: `ndshop:prime:title`,
                price: this.price,
                img: "byvip.png",
                event: "dshop:prime:buy",
                color: "#EDCC6D"
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
            if(e.keyCode == 27)
            {
                if(this.confirm) 
                    this.confirm = null;
                else 
                    this.$emit("onCloseMenu")
            }
                
        }
    },
    data() {
        return {
            confirm: null
        }
    },
    components:{
        ConfirmDialog
    },
    mounted(){
        document.addEventListener("keyup", this.keyHandler);
    },
    beforeDestroy(){
        document.removeEventListener("keyup", this.keyHandler);
    }
    
    
}
</script>

<style lang="scss">
    .newdshop-prem{
        width: 100%;
        height: 100%;
        background: url(/img/newDonateShop/premium/bg.jpg) no-repeat center;
        background-size: cover;
       text-transform: uppercase;
        &-girl{
            position: absolute;
            right: 2rem;
            bottom: 0;
            height: 84vh;
            z-index: 2;
        }
        &-block{
            width: 36rem;
            position: absolute;
            top: 4rem;
            left: 15rem;
        }
        &-title{
            display: flex;
            align-items: flex-end;
            justify-content: center;
            font-size: 4rem;
            &-text{
                margin: 0 1rem;
                margin-bottom: -1.3rem;
            }
            &-on{
                background: url("/img/newDonateShop/premium/gold.png") no-repeat center;
                background-size: cover;
                color: transparent;
                background-clip: text;
                margin-bottom: -1.3rem;
            }
            &-off{
                color: #595B60;
                margin-bottom: -1.3rem;
            }
        }
        &-subtitle{
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 1rem;
            width: 100%;
            font-weight: 300;
            text-align: center;
            letter-spacing: .03rem;
        }
        &-decorator{
            width: 30rem;
            height: 2px;
            margin: 1rem 3rem;
            background: radial-gradient(50% 50% at 50% 50%, rgba(255, 255, 255, 0.6) 0%, rgba(255, 255, 255, 0) 100%);
        }
        &-cards{
            width: 36rem;
            height: 14.5rem;
            display: flex;
            justify-content: space-between;
            align-items: space-between;
            flex-wrap: wrap;
            margin: 2.5rem 0;
        }
        &-card{
            width: 11.2rem;
            height: 6.7rem;
            margin-bottom: 1rem;
            border: 2px solid #EDCC6D;
            box-shadow: 0 0 .5rem #EDCC6D;
            display: flex;
            align-items: center;
            justify-content: center;
            &-text{                
                transform: rotate(-10deg);
                font-size: 1.5rem;
                line-height: 1.7rem;
                text-shadow: none;
                font-weight: bold;
            }
        }
        &-status{
            width: 63vh;
            display: flex; 
            background: url("/img/newDonateShop/premium/gold.png") no-repeat center;
            background-size: contain;
            align-items: center;
            justify-content: center;
            font-size: 3rem;
            font-weight: bold;
            color: transparent;
            background-clip: text;
        }
        &-footer{
            display: flex;
            width: 36rem;
            align-items: center;
            justify-content: center;
            margin-top: 2rem;
            font-size: 2rem;
            font-weight: 300;
            text-shadow: 0 0 .5rem #B28647;
            img{
                margin: 0 1.5rem 0 .5rem;
            }
        }
        &-button{
            background: linear-gradient(267.94deg, #FF3E3E -3.95%, #CE1717 99.03%), #CE1717;
            box-shadow: 0px 10px 60px rgba(206, 23, 23, 0.6);
            border-radius: 3px;
            width: 10rem;
            height: 3rem;
            font-size: 1rem;
            display: flex;
            align-items: center;
            justify-content: center;
            font-weight: bold;
            letter-spacing: .04rem;
            &:hover{
                transform: scale(1.01);
            }
        }
    }
</style>