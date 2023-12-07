<template>
    <div class="newdshop-exc-dialog">
        <div class="newdshop-exc-dialog_body">
            <img :src="`/img/newDonateShop/${data.img}`" alt="img" class="newdshop-exc-dialog_img" >
            <div class="newdshop-exc-dialog_info">
                <img src="/img/newDonateShop/close.svg" alt="close" class="newdshop-exc-dialog_close" @click="close" @mouseenter="playHover">
                <div class="newdshop-exc-dialog_title">{{loc(data.title)}}</div>
                <div class="newdshop-exc-dialog_subtitle">{{loc(data.subtitle)}}</div>
                
                <div class="newdshop-exc-dialog_row">
                    <div class="newdshop-exc-dialog_input">                    
                        <div class="newdshop-exc-dialog_curr"><img :src="`/img/newDonateShop/${data.currency1}`" alt="currency"></div>
                        <input type="number" v-model="value1" @input="calcV2">
                    </div>
                    <img :src="`/img/newDonateShop/arrows.svg`" alt="arrows" class="newdshop-exc-dialog_arrows">
                    <div class="newdshop-exc-dialog_input">                    
                        <div class="newdshop-exc-dialog_curr"><img :src="`/img/newDonateShop/${data.currency2}`" alt="currency"></div>
                        <input type="number" v-model="value2" @input="calcV1">
                    </div>
                    <div class="newdshop-exc-dialog_btn" @click="confirm" @mouseenter="playHover">
                        {{loc('newdshop:kits:item:buy')}}
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import { mapGetters } from 'vuex'

export default {
    props:{
        data: Object
    },
    data(){
        return{
            value1: 1,
            value2: 1
        }
    },
    computed:{
        ...mapGetters('localization', ['loc'])
    },
    methods: {
        calcV2(){
            this.value2 = Math.floor(this.value1 * this.data.course);
        },
        calcV1(){
            this.value1 = Math.ceil(this.value2 / this.data.course);
        },
        close(){
            this.playClick();
            this.$emit("onClose")
        },
        confirm(){
            if(this.value2 < 1 || this.value1 < 1) return;
            this.data.event(this.value2);
            this.close();
        },
        playHover(){
            this.$store.commit("sounds/play", {"name": "dshop_hover", "volume": .7});
        },
        playClick(){
            this.$store.commit("sounds/play", {"name": "dshop_click", "volume": .7});
        }
    },
    mounted(){
        this.value2 = this.data.course;
    }
}
</script>

<style lang="scss">
.newdshop-exc-dialog{
   text-transform: uppercase;
    position: fixed;
    background-color: rgba(#000, .8);
    left: 0;
    top: 0;
    width: 100vw;
    height: 100vh;
    display: flex;
    align-items: center;
    justify-content: center;
    letter-spacing: .07rem;
    z-index: 500;
    &_img{
        height: 10rem;
    }
    &_body{
        position: relative;
        background-color: #1B1E27;
        height: 17rem;
        width: 45rem;
        display: flex;
        align-items: center;
        justify-content: space-between;
    }
    &_close{
        position: absolute;
        right: -2rem;
        top: 0;
        &:hover{
            filter: drop-shadow(0 0 .5rem);
        }
    }
    &_title{
        font-size: 3rem;
        font-weight: 300;
        line-height: 2rem;
        margin-top: .7rem;
    }
    &_subtitle{
        font-size: .8rem;
        
        margin: 1rem 0;
    }
    &_row{
        display: flex;
        align-items: center;
        justify-content: center;
    }
    &_input{
        display: flex;
        align-items: center;
        justify-content: space-between;
        width: 10rem;
        height: 3rem;
        background: rgba(255, 255, 255, 0.06);
        border-radius: 2px;
        input{
            width: 7rem;
            height: 3rem;
            background: rgba(255, 255, 255, 0);
            color: #fff;
            &::-webkit-inner-spin-button {
                /* display: none; <- Crashes Chrome on hover */
                -webkit-appearance: none;
                margin: 0; /* <-- Apparently some margin are still there even though it's hidden */
            }
        }
    }
    &_curr{
        width: 3rem;
        height: 3rem;
        display: flex;
        justify-content: center;
        align-items: center;
    }
    &_arrows{
        margin: 0 1.5rem;
    }
    &_btn{
        width: 7rem;
        height: 3rem;
        display: flex;
        font-size: 1.2rem;
        letter-spacing: .07rem;
        align-items: center;
        justify-content: center;
        margin: 0 1.5rem;
        background-color: #9FBA1B;
        &:hover{
            box-shadow: 0 0 .5rem #9FBA1B;
        }
    }
}
</style>