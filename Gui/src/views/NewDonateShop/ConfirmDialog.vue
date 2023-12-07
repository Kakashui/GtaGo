<template>
    <div class="newdshop-dialog-confirm">
        <div class="newdshop-dialog-confirm_body" :style="{'border': `2px solid ${data.color}`}">
            <img src="/img/newDonateShop/close.svg" alt="close" class="newdshop-dialog-confirm_close" @click="close" @mouseenter="playHover">
            <div class="newdshop-dialog-confirm_img">
                <img :src="`/img/newDonateShop/items/${data.img}`" alt="confirm" :style="{'filter': `drop-shadow(0 0 .5rem ${data.color})`}">
            </div>
            
            <div class="newdshop-dialog-confirm_info">
                <div class="newdshop-dialog-confirm_title">{{loc(data.title)}}</div>
                <div class="newdshop-dialog-confirm_price">
                    {{data.price}}
                    <img src="/img/newDonateShop/header_coin.png" alt="header_coin">
                </div>
                <div class="newdshop-dialog-confirm_btn" @click="confirm" @mouseenter="playHover">{{loc('newdshop:kits:item:buy')}}</div>
            </div>
        </div>
    </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'


export default {
    props:{
        data: Object
    },
    computed:{
        ...mapGetters('localization', ['loc']),
        ...mapState('newDonateShop', ['coins'])
    },
    data(){
        return{
            spamProtection: 0
        }
    },
    methods:{
        close(){
            this.playClick();
            this.$emit("onClose");
        },
        confirm(){
            if(this.spamProtection > Date.now())return;
            this.spamProtection = Date.now() + 1000;
            if(!this.data.noCheck && this.coins < this.data.price){
                this.$store.commit('notifyList/notify', {type: 1, position: 0, message: this.loc('dshop:item:buy:wrong:coins'), time: 3000});
                return;
            }
            if(this.data.id === undefined)
                window.mp.triggerServer(this.data.event);
            else 
                window.mp.triggerServer(this.data.event, this.data.id);
            this.close();
        },
        playHover(){
            this.$store.commit("sounds/play", {"name": "dshop_hover", "volume": .7});
        },
        playClick(){
            this.$store.commit("sounds/play", {"name": "dshop_click", "volume": .7});
        }
    }
}
</script>

<style lang="scss">
    .newdshop-dialog-confirm{
        position: fixed;
        left: 0;
        top: 0;
        width: 100vw;
        height: 100vh;
        display: flex;
        align-items: center;
        justify-content: center;
        background-color: rgba(#000, .8);
        z-index: 500;        
        &_body{
            width: 45rem;
            height: 20rem;
            display: flex;
            align-items: center;
            justify-content: space-between;
            position: relative;
            padding: 2rem;
            background-color: rgba(#000, .8);
        }
        &_img{
            width: 40%;
            display: flex;
            justify-content: center;
            align-items: center;
            height: 100%;
            img{
                height: 70%;
            }
        }
        &_info{
            width: 60%;
        }
        &_title{
            
            font-size: 2rem;
        }
        &_price{
            font-size: 2rem;
            color: #EDCC6D;
            margin: 1rem 0;
            img{
                margin-left: .5rem;
            }
        }
        &_close{
            position: absolute;
            right: -2.5rem;
            top: -.2rem;
        }
        &_btn{
            width: 9rem;
            height: 3.3rem;
            background-color: #9FBA1B;
            font-size: 1.2rem;
            letter-spacing: .07rem;
            border-radius: .1rem;
            display: flex;
            align-items: center;
            justify-content: center;
        }
    }
</style>