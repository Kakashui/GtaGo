<template>
    <div class="temo">
        <svg class="temo_body" viewBox="0 0 200 200" version="1.1" xmlns="http://www.w3.org/2000/svg">
            <circle :r="radius" cx="100" cy="100"  fill="none" stroke="#FFFFFF55" stroke-width="8" />
            <circle :r="radius" cx="100" cy="100"  fill="none" stroke="#9EBF3D" stroke-width="8" :style="{'stroke-dasharray': dashArray}" class="temo-progress"/>
        </svg>
        <div class="temo-title">{{minutes}}:{{seconds}}</div>
    </div>
</template>

<script>
import { mapMutations } from 'vuex';
export default {
    props:["time", "length"],
    data() {
        return {
            radius: 85
        }
    },
    computed:{
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
            return Math.floor(this.time / this.length * 100)
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
    methods: {
        ...mapMutations("timerTemo", ["start"])
    }
}
</script>

<style lang="scss">
.temo{
    position: absolute;
    top: 2rem;
    width: 7rem;
    height: 7rem;
    display: flex;
    align-items: center;
    justify-content: center;
    &_body{
        position: absolute;
        width: 100%;
        height: 100%;
    }
    circle{        
        transition: all 1s linear;
        stroke-dashoffset: 135;
    }
    &-progress{
        filter: drop-shadow(0 0 .3rem #9EBF3D);
    }
    &-title{
        position: absolute;
        color: #9EBF3D;
        text-align: center;
        width: 100%;
        font-size: 2rem;
        font-weight: bold;
        text-shadow: 0 0 .7rem #9EBF3DAA ;
    }
}
</style>