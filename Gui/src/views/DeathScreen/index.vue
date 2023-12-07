<template>   
    <div class="deathscreen"> 
        <img src="/img/deathScreen/blood.png" alt="blood" class="deathscreen_blood">
        <img src="/img/deathScreen/red.png" alt="red" class="deathscreen_red">
        <div class="deathscreen-hint">
            <div class="deathscreen-hint_timer">
                <svg class="deathscreen-hint_circle" viewBox="0 0 200 200" xmlns="http://www.w3.org/2000/svg" version="1.1">
                    <circle cx="100" cy="100" :r="radius" stroke="#FFFFFF55" stroke-width="8" fill="none"/>
                    <circle cx="100" cy="100" :r="radius" stroke="#FFFFFF" stroke-width="8" fill="none" :style="{'stroke-dasharray': dashArray}"/>
                </svg>
                 
                <div class="deathscreen-hint_bg">!</div>
                <!-- <span class="deathscreen-hint_number">{{ formattedTimeLeft }}</span> -->
            </div>
            <div class="deathscreen-hint_info">
                <div class="deathscreen-hint_title">{{loc("dscreen:hint:tit")}}</div>
                <div class="deathscreen-hint_subtitle">{{loc(currentHint)}}</div>
            </div>
        </div> 
        <div class="deathscreen-body">
            <img src="/img/deathScreen/death.png" alt="death" class="deathscreen-body_title">
            <div class="deathscreen-body_subtitle">{{loc("dscreen:help:wait")}}</div>
            <div class="deathscreen-progress">
                <div class="deathscreen-progress_body">
                    <div class="deathscreen-progress_thumb" :style="{'width': `${timerProgress}%`}"></div>
                </div>
                
            </div>
            <div class="deathscreen-body_time">
                {{minutes}}:{{seconds}}
            </div>
            <div @click="callEms" class="deathscreen-body_btn" v-if="medicsBtn">
                 <img src="/img/deathScreen/ems.png" alt="ems">
                {{ loc('death_menu_3') }}
            </div>
            <div @click="die" class="deathscreen-body_btn deathscreen-body_die" v-if="deathBtn">
                 <img src="/img/deathScreen/die.png" alt="die">
                {{ loc('death_menu_5') }}
            </div>
        </div>
  </div>
</template>

<script>
import {mapState, mapGetters} from 'vuex'

export default {
    data() {
        return {
            radius: 80,
            hintTime: 60,
            hintLength: 60,
            hintInterval: null,
            currentHintIndex: 0,
            shufledHints:[]
        }
    },
    computed:{
        ...mapState('deathScreen', ['time', 'startTime', 'medicsBtn', 'deathBtn', 'hints']),
        ...mapGetters('localization', ['loc']),
        dashArray(){
            const plength = Math.floor(this.percentLength * this.currentPercent);
            return `${plength} ${this.circleLength - plength}`;
        },
        circleLength(){
            return Math.floor(2 * Math.PI * this.radius);
        },
        percentLength(){
            return this.circleLength / 100;
        },
        currentPercent(){
            return Math.floor(this.hintTime / this.hintLength * 100)
        },
        currentHint(){
            return this.shufledHints[this.currentHintIndex]
        },
        timerProgress(){
            return 100 - Math.floor(this.time / this.startTime * 100)
        },
        minutes(){
            const minutes = Math.floor(this.time / 60);
            return minutes < 10 ? `0${minutes}` : `${minutes}`;
        },
        seconds(){
            const seconds = this.time % 60;
            return seconds < 10 ? `0${seconds}` : `${seconds}`;
        }
    },
    methods:{
        die(){
            window.mp.triggerServer("dieEms");
        },
        callEms(){
            window.mp.triggerServer("callEms");
        },
        hintHandler(){
            this.hintTime --;
            if(this.hintTime < 1){
                this.nextHint();
            }
        },
        nextHint(){
            if(this.currentHintIndex + 1 >= this.shufledHints.length)
                this.currentHintIndex = 0
            else
                this.currentHintIndex++

            this.hintTime = this.hintLength;
        },
        shuffle(array) {
            const result = [...array];
            for (let i = result.length - 1; i > 0; i--) {
                let j = Math.floor(Math.random() * (i + 1));
               
                [result[i], result[j]] = [result[j], result[i]];
            }
            return result;
        }
    },    
    mounted() {
        if(process.env.NODE_ENV == 'development')
            this.$store.commit("deathScreen/setTime", 300)
        this.hintLength = this.hintTime;
        this.shufledHints = this.shuffle(this.hints);
        this.hintInterval = setInterval(this.hintHandler, 1000)
    },
    beforeDestroy(){
        clearInterval(this.hintInterval)
    }
}
</script>

<style lang="scss" scoped>
.deathscreen{
    position: fixed;
    top: 0;
    left: 0;
    width: 100vw;
    height: 100vh;
    display: flex;
    justify-content: center;
    align-items: center;
    flex-direction: column;
    transition: all .5s;
    text-transform: uppercase;
    background: radial-gradient(49.17% 87.41% at 50% 50%, rgba(2, 10, 30, 0) 87.25%, rgba(2, 10, 30, 0.3) 100%),linear-gradient(0deg, rgba(2, 10, 30, 0.7), rgba(2, 10, 30, 0.7));
    color: #fff;
    &_blood{
        position: absolute;
        top: 0;
        width: 30rem;
    }
    &_red{
        position: absolute;
    }
    &-hint{
        position: absolute;
        left: 1rem;
        top: 25rem;
        display: flex;
        align-items: center;
        &_timer{
            display: flex;
            width: 4.5rem;
            height: 4.5rem;
            align-items: center;
            justify-content: center;
            position: relative;
        }
        &_circle{
            position: absolute;
            width: 100%;
            height: 100%;
            //border: 1px solid #000;
        }
        &_info{
            margin-left: 1.5rem;
            width: 20rem;
        }
        &_title{
            font-size: 1.7rem;
            letter-spacing: .08rem;
        }
        &_subtitle{
            letter-spacing: .06rem;
            margin-top: .2rem;
            min-height: 2.5rem;
        }
        &_bg{
            width: 68%;
            height: 68%;
            position: absolute;
            border-radius: 50%;
            display: flex;
            align-items: center;
            justify-content: center;
            background: radial-gradient(50% 50% at 50% 50%, rgba(255, 255, 255, 0) 0%, rgba(255, 255, 255, 0.2) 100%);
            font-size: 2rem;
        }       
        circle{        
            transition: all 1s linear;
            stroke-dashoffset: 135;
        }
    }
    &-body{
        position: relative;
        height: 30em;
        margin-top: 8rem;
        //border: 1px solid #000;
        //background: #FF0E0E99;
        //background: radial-gradient(#FF0E0E55 50%, #FF0E0E00 80%, #FF0E0E00 0%);
        &_title{
            height: 11rem;
        }
        &_subtitle{
            margin-top: .5rem;
            font-size: 1.2rem;
            letter-spacing: .15rem;
            text-align: center;
        }
        &_time{
            margin-left: 10rem;
            font-size: 5.5rem;
            font-weight: bold;
            margin-top: 1rem;
        }
        &_btn{
            background-color: rgba(#fff, .2);
            height: 4.5rem;
            width: 16rem;
            display: flex;
            align-items: center;
            justify-content: center;
            border-radius: 2.25rem;
            margin: 1rem auto 0;
            img{
                margin-right: 1rem;
            }
            &:hover{
                transform: scale(1.02);
            }
        }
        &_die{
            background: linear-gradient(90deg, #F05053 0%, #FF9191 100%);
        }
    }
    &-progress{
        margin-top: .5rem;
        width: 100%;
        background: radial-gradient(50% 50% at 50% 50%, rgba(255, 255, 255, 0.4) 0%, rgba(255, 255, 255, 0) 100%);
        padding: .75rem 0;
        &_body{
            position: relative;
            width: 80%;
            margin: auto;
        }
        &_thumb{
            height: 100%;
            margin: 0 auto;
            background-color: #fff;
            height: .3rem;
            border-radius: .15rem;
            transition: all 1s;
        }
    }
}
</style>