<template>
    <div class="mplant-gwin">
         <div class="mplant-gwin_body">
            <div class="mplant-gwin_title">{{title}}</div>
            <div class="mplant-gwin_diamond mplant-gwin_img" v-if="gameResult.state === 3">
                <img src="/img/miniGames/metalPlant/resources/diamond.png" alt="diamond" class="mplant-gwin_el">
            </div>
            <div class="mplant-gwin_img mplant-gwin_metal"  v-else>
                <img v-for="(perfect, index) in gameResult.metals" :key="index" :src="`/img/miniGames/metalPlant/resources/${metalIcon(perfect)}.png`" alt="el" class="mplant-gwin_el">
            </div>
            <div class="mplant-gwin-info">
                Получено: 
                <div v-if="perfectCount > 0" class="mplant-gwin-info_item"><img :src="`/img/miniGames/metalPlant/resources/${metalIcon(true)}.png`" alt="perfect">{{perfectCount}}</div>
                <div v-if="commonCount > 0" class="mplant-gwin-info_item"><img :src="`/img/miniGames/metalPlant/resources/${metalIcon(false)}.png`" alt="common"> {{commonCount}}</div>
                <div v-if="gameResult.state === 3" class="mplant-gwin-info_item"><img src="/img/miniGames/metalPlant/resources/diamond.png" alt="diamond">1</div>
            </div>
            <div class="mplant-prepare_delimiter" style="transform: scale(.5, .3)"></div>
            <div class="mplant-gwin_result">
                <div class="mplant-gwin_el">
                    <Element :element="validElementIcon"/>
                    <div class="mplant-gwin_count">{{game.valid}}/{{game.condition}}</div>
                    <div class="mplant-gwin_perc">{{validCoef}} %</div>
                </div>                
                <div class="mplant-gwin_el">
                    <Element :element="invalidElementIcon"/>
                    <div class="mplant-gwin_count">{{game.invalid}}/{{game.condition}}</div>
                    <div class="mplant-gwin_perc">{{invalidCoef}} %</div>
                </div>
            </div>
            <div class="mplant-gwin_btn" @click="confirm">Ok</div>
        </div>
    </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
import Element from './Element.vue'

export default {
     computed:{
        ...mapState("gameMetalPlant", ["gameResult", "game", "invalidElementIcon", "validElementIcon", "furnace"]),
        ...mapGetters("localization", ["loc"]),
        title(){
            return this.gameResult.state === 2 ?
                "Completed": "Diamond";
        },
        validCoef(){
            if(this.game.condition == 0) return 0;
            return Math.floor(this.game.valid / this.game.condition * 100);
        },
        invalidCoef(){
            return 100 - this.validCoef;
        },
        perfectCount(){
            return this.gameResult.metals.filter(m=>m).length
        },
        commonCount(){
            return this.gameResult.metals.length - this.perfectCount
        },
        diamondCount(){
            return this.gameResult.state - 2
        }
    },
    methods:{
        confirm(){
            window.mp.trigger("mg:metalplant:quit")
        },
        metalIcon(perfect){
            switch (this.furnace.ore) {
                case 256: return perfect ? 'copper_perfect' : 'copper';
                case 257: return perfect ? 'iron_perfect' : 'iron'; 
                default:  return perfect ? 'aluminum_perfect' : 'aluminum';
            } 
        }
    },
    components:{
        Element
    },
    mounted(){
        this.$store.commit("sounds/play", { name: `metalPlant_gameWin`, volume: 0.2})
    }
}
</script>

<style lang="scss">
    .mplant-gwin{
        position: absolute;
        left: 0;
        top: 0;
        z-index: 501;
        width: 100%;
        height: 100%;
        display: flex;
        align-items: center;
        justify-content: center;
        &_body{
            width: 32rem;
            //height: 32rem;
            background-color:  rgba(#000, .5);
            border: .1rem solid rgba(#fff, .1);
            border-radius: 1rem;
            position: relative;
        }
        &_title{
            font-size: 3rem;
            text-align: center;
            text-transform: uppercase;
            text-shadow: 0 0 1rem #FF0707;
            margin-top: 1rem;
        }
        &_sub{
            text-align: center;
            letter-spacing: .05rem;
        }
        &_img{
            position: relative;
            width: 22rem;
            height: 13rem;
            margin: 0 auto;
            margin-top: 1rem;
            display: flex;
            align-items: center;
            justify-content: center;
            background-size: cover;
        }
        &_diamond{
            background: url(/img/miniGames/metalPlant/common/diamond_light.png) no-repeat center/cover;
            img{
                height: 10rem;
                width: 10rem;
            }
        }
        &_metal{
            background: url(/img/miniGames/metalPlant/common/metal_light.png) no-repeat center/cover;
            img{
                position: absolute;
                height: 8rem;
                width: 8rem;
                &:nth-child(1){
                    margin-left: 0rem;
                    margin-top: -3rem;
                }
                &:nth-child(2){
                    margin-left: -7rem;
                    margin-top: 1rem;
                }
                &:nth-child(3){
                    margin-left: 7rem;
                    margin-top: 1rem;
                }
                &:nth-child(4){
                    margin-left: 0;
                    margin-top: 6rem;
                }
            }
        }
        &_btn{
            background-color: rgba(#fff, .05);
            border: .1rem solid rgba(#fff, .1);
            width: 13rem;
            height: 3.5rem;
            border-radius: .15rem;
            font-size: 1.2rem;
            display: flex;
            align-items: center;
            justify-content: center;
            margin: 2rem auto;
            &:hover{
                transform: scale(1.03);
            }
        }
        &_el{
            display: flex;
            align-items: center;
            justify-content: space-between;
            margin: 0 auto;
            width: 12rem;
        }
        &_perc{
            margin-left: auto;
        }
        &_count{
            margin-left: 1rem;
        }
        &-info{
            display: flex;
            align-items: center;
            justify-content: center;
            img{
                width: 2rem;
                height: 2rem;
            }
            &_item{
                display: flex;
                align-items: center;
                justify-content: center;
                margin-left: 1rem;
            }
        }
    }
</style>