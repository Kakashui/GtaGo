<template>
    <div class="mplant-gover">
        <div class="mplant-gover_body">
            <div class="mplant-gover_title">Game Over</div>
            <div class="mplant-prepare_delimiter" style="transform: scale(.5, .3)"></div>
            <div class="mplant-gover_sub" v-html="subtitle"></div>
            <div class="mplant-gover_img">
                <img :src="`/img/miniGames/metalPlant/common/${gameResult.state === 0 ? 'temp_lose' : 'elements_lose'}.png`" alt="el" class="mplant-gover_el">
            </div>
            <div class="mplant-gover_btn" @click="confirm">Ok</div>
        </div>
    </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
export default {
    computed:{
        ...mapState("gameMetalPlant", ["gameResult"]),
        ...mapGetters("localization", ["loc"]),
        subtitle(){
            return this.loc(this.gameResult.state === 0 ?
                "mplant:gover:tit:1": "mplant:gover:tit:2");
        }
    },
    methods:{
        confirm(){
            window.mp.trigger("mg:metalplant:quit")
        }
    },
    mounted(){
        this.$store.commit("sounds/play", { name: `metalPlant_gameOver`, volume: 0.2})
    }
}
</script>

<style lang="scss">
    .mplant-gover{
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
            height: 32rem;
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
            background: url(/img/miniGames/metalPlant/common/red_light.png) no-repeat;
            background-size: cover;
            background-position: 0 40%;
        }
        img{
            height: 70%;
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
            margin: 0 auto;
            margin-top: 1rem;
            &:hover{
                transform: scale(1.03);
            }
        }
    }
</style>