<template>
    <div class="mplant-horn">
        <div class="mplant-horn_center"></div>
        <svg viewBox="0 0 200 200" version="1.1" xmlns="http://www.w3.org/2000/svg">
            <circle :r="radius" cx="100" cy="100"  fill="transparent" stroke="#8C0000" :stroke-dasharray="dashArray" stroke-dashoffset="135" ></circle>
        </svg>
        <div class="mplant-horn_button" @click="hornAction">
            <img src="/img/miniGames/metalPlant/common/flame.svg" alt="flame" class="mplant-horn_img">
        </div>
    </div>
</template>

<script>
import { mapActions, mapGetters } from 'vuex'
export default {
    data() {
        return {
            radius: 85
        }
    },
    computed:{
        ...mapGetters('gameMetalPlant', ['furnaceTempearature']),
        dashArray(){
            const plength = this.percentLength * this.furnaceTempearature;
            return `${plength} ${this.circleLength - plength}`;
        },
        // dashOffset(){

        // },
        circleLength(){
            return 2 * Math.PI * this.radius;
        },
        percentLength(){
            return this.circleLength / 100;
        }
    },
    methods: {
        ...mapActions('gameMetalPlant', ['hornAction'])
    },
}
</script>

<style lang="scss">
    .mplant-horn{
        width: 12rem;
        height: 12rem;
        display: flex;
        align-items: center;
        justify-content: center;
        &_center{
            width: 7.8rem;
            height: 7.8rem;
            border-radius: 50%;
            box-shadow: 0 0 .7rem .7rem rgba(#999, .2);
        }
        svg{
            position: absolute;
            width: 10.3rem;
            height: 10.3rem;
            circle{
                stroke-width: .4rem;
                transition: all .5s linear;
            }
        }
        &_button{
            position: absolute;
            width: 4rem;
            height: 4rem;
            background:radial-gradient(ellipse at bottom, #111, #111, #333);
            box-shadow: 0 0 1rem 0 #000;
            border-radius: 50%;
            display: flex; 
            align-items: center;
            justify-content: center;
            img{
                pointer-events: none;
            }
            &:hover{
                transform: scale(1.04);
                background:radial-gradient(ellipse at bottom, #211, #211, #433);
                img{
                    filter: drop-shadow(0 0 .3rem #c00)
                }
            }
        }
    }
</style>