<template>
    <div class="artimer">
        <img src="/img/hud/artimer.svg" alt="artimer" class="artimer-img">
        <div class="artimer-tittle">{{loc('arrets:timer:tit')}}</div>
        <div class="artimer-timer">{{minutes}}:{{seconds}}</div>
        <div class="artimer-progress">
            <div class="artimer-progress-thumb" :style="{'width': `${progress}%`}"></div>
        </div>
        <img src="/img/hud/arwire.png" alt="artimer" class="artimer-wire">
    </div>
</template>

<script>
import { mapGetters } from 'vuex';
export default {
    props:{
        timer: Object,
    },
    data() {
        return {            
            interval: 0,
            minutes: '00',
            seconds: '00',
            total: 100,
            progress: 100,
        }
    },
    computed: {
        ...mapGetters('localization', ['loc'])
    },
    methods: {
        tick(){
            const interval = (this.timer.expiried - Date.now()) / 1000;
            if(interval > 0){
                let min = `${Math.floor(interval / 60)}`
                if(min == '0'){
                    min = '00';
                }
                else{
                    while (min.length < 2) {
                        min = "0" + min;
                    }
                }
                let seconds = `${Math.floor(interval % 60)}`
                if(seconds == '0'){
                    seconds = '00';
                }
                else{
                    while (seconds.length < 2) {
                        seconds = "0" + seconds;
                    }
                }
                this.minutes = min;
                this.seconds = seconds;
                this.progress = Math.floor( interval / this.total * 100);
                //window.console.log(this.progress)
            }else{
                this.$store.commit('hud/resetArrestTimer')
            }            
        }
    },
    mounted() {
        this.total = (this.timer.expiried - Date.now()) / 1000;
        this.interval = setInterval(this.tick, 1000)
    },
    beforeDestroy(){
        clearInterval(this.interval)
    }
}
</script>

<style lang="scss" scoped>
    .artimer{
        position: fixed;
        text-transform: uppercase;
        top: 0;
        left: 50%;
        color: #fff;
        &-img{
            width: 8rem;
            height: 8rem;
            position: absolute;
        }
        &-tittle{
            margin-top: 1.8rem;
            width: 8rem;
            text-align: center;
            font-size: 1.2rem;
        }
        &-timer{
            width: 8rem;
            text-align: center;
            font-size: 3rem;
            line-height: 2.6rem;
            font-weight: 300;
        }
        &-progress{
            width: 13rem;
            margin-left: -2.5rem;
            margin-top: .5rem;
            height: .2rem;
            background-color:rgba(#fff, .2);
            position: relative;
            border-radius: .2rem;
            overflow: hidden;
            &-thumb{
                background-color:#fff;
                border-radius: .2rem;
                height: 100%;
                transition: all 1s linear;
            }
        }
        &-wire{
            position: fixed;
            bottom: 0;
            right: 0;
        }
    }
</style>