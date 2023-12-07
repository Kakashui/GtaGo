<template>
    <div class="newdshop-shop-cat">
        <div class="newdshop-shop-cat-container">

            <div class="newdshop-shop-cat-type1 newdshop-shop-cat-hover" @click="select(1)" @mouseenter="playHover">
                <img src="/img/newDonateShop/shop/menu/clothes.png" alt="clothes" class="newdshop-shop-cat-type1_img">
                <div class="newdshop-shop-cat-sub">{{toCatalog}}</div>
                <div class="newdshop-shop-cat-type1_title">{{loc("newdshop:shop:select:cloth")}}</div>
            </div>
            <div class="newdshop-shop-cat-type1 newdshop-shop-cat-hover" @click="select(2)" @mouseenter="playHover">
                 <img src="/img/newDonateShop/shop/menu/vehicles.png" alt="vehicles" class="newdshop-shop-cat-type1_img">
                <div class="newdshop-shop-cat-sub">{{toCatalog}}</div>
                <div class="newdshop-shop-cat-type1_title">{{loc("newdshop:shop:select:veh")}}</div>
            </div>

            <div class="newdshop-shop-cat-type2 newdshop-shop-cat-hover" @click="select(3)" @mouseenter="playHover">
                 <img src="/img/newDonateShop/shop/menu/guns.png" alt="guns" class="newdshop-shop-cat-type2_img">
                <div class="newdshop-shop-cat-type2_title">{{loc("newdshop:shop:select:wep")}}</div>
            </div>
            <div class="newdshop-shop-cat-type2 newdshop-shop-cat-hover" @click="select(4)" @mouseenter="playHover">
                 <img src="/img/newDonateShop/shop/menu/animals.png" alt="animals" class="newdshop-shop-cat-type2_img">
                <div class="newdshop-shop-cat-type2_title">{{loc("newdshop:shop:select:animal")}}</div>
            </div>
            <div class="newdshop-shop-cat-type2 newdshop-shop-cat-hover"><!-- @click="select(5)" @mouseenter="playHover" -->  
                <img src="/img/newDonateShop/shop/menu/vinils.png" alt="vinils" class="newdshop-shop-cat-type2_img">
                <div class="newdshop-shop-cat-type2_title">{{loc("newdshop:shop:select:vinils")}}</div>
                <div class="commingsoon"></div>
            </div>
            <div class="newdshop-shop-cat-type2 newdshop-shop-cat-hover" @click="select(6)" @mouseenter="playHover">
                 <img src="/img/newDonateShop/shop/menu/other.png" alt="other" class="newdshop-shop-cat-type2_img">
                <div class="newdshop-shop-cat-type2_title">{{loc("newdshop:shop:select:other")}}</div>
            </div>

            <div class="newdshop-shop-cat-type3 newdshop-shop-cat-hover"><!-- @click="select(7)" @mouseenter="playHover" -->                
                 <img src="/img/newDonateShop/shop/menu/plate.png" alt="plate" class="newdshop-shop-cat-type3_img">                 
                <div class="newdshop-shop-cat-sub">{{toCatalog}}</div>
                <div class="newdshop-shop-cat-type3_title">{{loc("newdshop:shop:select:plate")}}</div>
                <div class="commingsoon"></div>
            </div>
            
            <div class="newdshop-shop-cat-type4 newdshop-shop-cat-hover"> <!-- @click="select(8)" @mouseenter="playHover" -->
                <img src="/img/newDonateShop/shop/menu/phone.png" alt="phone" class="newdshop-shop-cat-type4_img">                 
                <div class="newdshop-shop-cat-sub">{{toCatalog}}</div>
                <div class="newdshop-shop-cat-type3_title">{{loc("newdshop:shop:select:phone")}}</div>
                <div class="commingsoon"></div>
            </div>

        </div>
    </div>
</template>

<script>
import {mapGetters} from 'vuex';

export default {   
    computed:{
        ...mapGetters('localization', ['loc']),
        toCatalog(){
            return this.loc('newdshop:shop:select:tocatalog');
        }
    },
    methods: {
        select(category){
            this.$emit("onSelect", category);
        },
        playHover(){
            this.$store.commit("sounds/play", {"name": "dshop_hover", "volume": .7})
        },
        keyHandler(e){
           if(e.keyCode == 27){
                this.$emit("onCloseMenu")
           }   
        }
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
    .newdshop-shop-cat{
        background: url(/img/newDonateShop/shop/menu/bg.png) no-repeat center;
        background-size: cover;
        width: 100%;
        height: 100%;
        padding-top: 4rem;
        &-container{  
            box-sizing: border-box;          
            width: 75rem;
            height:40rem;
            margin: auto;
            position: relative;
        }
        &-sub{
            position: absolute;
            top: 2rem;
            left: 2rem;
            font-size: 1rem;
        }
        &-type1{            
            position: absolute;
            top: 0;
            left: 0;
            &_img{
                width: 22rem;
                height: 29rem;
            }
            &_title{
                position: absolute;
                top: 3rem;
                left: 2rem;
                font-size: 3rem;
            }
            &:nth-child(2){
                left: 23rem;
            }
        }
        &-type2{            
            position: absolute;
            &_img{
                width: 14rem;
                height: 14rem;
            }          
            &_title{
                position: absolute;
                top: 1rem;
                width: 100%;
                text-align: center;
                font-size: 2rem;
            }
            &:nth-child(3){
                top: 0;
                left: 46rem;
            }
            &:nth-child(4){
                top: 0;
                left: 61rem;
            }
            &:nth-child(5){
                top: 15rem;
                left: 46rem;
            }
            &:nth-child(6){
                top: 15rem;
                left: 61rem;
            }
        }
        
        &-type3{            
            position: absolute;
            top: 30rem;
            left: 0;
            &_img{
                width: 45rem;
                height: 9rem;
            }          
            &_title{
                position: absolute;
                top: 3rem;
                left: 2rem;
                font-size: 2rem;
            }
        }
        &-type4{            
            position: absolute;
            top: 30rem;
            left: 46rem;
            &_img{
                width: 29rem;
                height: 9rem;
            }
        }
        &-hover{
            &:hover{
                transform: scale(1.02);
                img{
                    box-shadow: 0 0 .5rem rgba(#D51A1B, .5);
                }
            }
        }
    }
.commingsoon{
    width: 100%;
    height: 100%;
    background: url(/img/common/commingsoon.svg) rgba(#000, .9) no-repeat center;
    background-size: contain;
    position: absolute;
    top: 0;
    left: 0;
    border: .1rem solid rgba(#fff, .2);
}
</style>