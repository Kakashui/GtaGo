<template>
    <div class="maskshop-colors">
        <div class="maskshop-colors_title">{{loc("shop:mask:color")}}</div>
        <div class="maskshop-colors_row">       
            <!-- <img src="/img/maskShop/arrow.svg" alt="left" class="maskshop-colors_next">   -->
            <div class="maskshop-colors_list">
                <div class="maskshop-colors_color" 
                    v-for="(color, index) in colors" :key="index" 
                    :class="{'maskshop-colors_selected': current === index}"
                    @click="select(index)"
                >
                    <img :src="`/img/maskShop/${item.Variation}.png`" alt="img" v-if="color !== -1">
                </div>
            </div>           
            <!-- <img src="/img/maskShop/arrow.svg" alt="right" class="maskshop-colors_next"> -->
        </div>
        <div class="maskshop-colors_info">
            <div class="maskshop-colors_title">Mask {{item.Variation}}</div>
            <div class="maskshop-colors_price">$ {{price}}</div>
        </div>
        <div class="maskshop-colors_btn" @click="buy">{{loc("shop:mask:buy")}}</div>
    </div>
</template>

<script>
import { mapGetters } from 'vuex'
export default {
    props:["item", "price"],
    data() {
        return {
            current: 0
        }
    },
    computed:{
        ...mapGetters("localization", ["loc"]),
        colors(){
            const colors = [...this.item.Colors];
            while (colors.length < 8) {
                colors.push(-1)
            }
            return colors;
        }
    },
    methods: {
        select(index){
            if(this.colors[index] < 0 || this.current === index) return;
            this.current = index;        
            this.$emit("onSelectColor", this.current);
        },
        buy(){
            this.$emit("onBuyMask", this.current);
        }
    },
    watch:{
        item(){
            this.current = 0;
            this.$emit("onSelectColor", this.current);
        }
    }
}
</script>

<style lang="scss">
    .maskshop-colors{
        text-transform: uppercase;
        color: #fff;
        width: 19.5rem;
        height: 26.5rem;
        background: linear-gradient(180deg, rgba(41, 255, 242, 0.3) 0%, rgba(41, 255, 242, 0) 100%),linear-gradient(0deg, rgba(0, 0, 0, 0.8), rgba(0, 0, 0, 0.8));
        margin-right: 2rem;
        padding-top: 3rem;
        margin-top: -4rem;
        &_row{
            display: flex;
            align-items: center;
            justify-content: center;
        }
        &_title{
            text-align: center;
            letter-spacing: .08rem;
            font-weight: bold;
        }
        &_list{
            display: flex;
            flex-wrap: wrap;
            width: 15rem;
            height: 7rem;
            overflow-y: scroll;
            margin-top: 1rem;
            &::-webkit-scrollbar {
                width: .2rem;
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
        &_next{
            &:first-child{
                transform: rotate(180deg);
            }
            margin: 0 .5rem;
        }
        &_color{
            width: 3.1rem;
            height: 3.1rem;
            margin: .2rem;
            border: .1rem solid rgba(#fff, .2);
            position: relative;
            background-color: rgba(#000, .2);
            img{
                width: 100%;
                height: 100%;
            }
            &:hover{
                background-color: rgba(#fff, .2);
            }
        }
        &_selected{
            //background-color: #fff;
            //border: .1 solid #fff;
            background-color: rgba(#fff, .2);
            border: .1rem solid #B6D300;
            box-shadow: 0 0 .7rem 0 #B6D300;
        }
        &_info{
            height: 6rem;
            display: flex;
            align-items: center;
            justify-content: center;
            flex-direction: column;
            margin: auto;
            margin-top: 1.5rem;
            border-top: .1rem solid rgba(#fff, .2);
            border-bottom: .1rem solid rgba(#fff, .2);
            width: 15rem;
        }
        &_title{
            color: #B6D300;
        }
        &_price{
            font-size: 2rem;
        }
        &_btn{
            height: 3rem;
            width: 15rem;
            margin: auto;
            margin-top: 1rem;
            background-color: #B6D300;
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 1.2rem;
            letter-spacing: .05rem;
            &:hover{
                transform: scale(1.02);
                box-shadow: 0 0 .5rem 0 #B6D300;
            }
        }
    }
</style>