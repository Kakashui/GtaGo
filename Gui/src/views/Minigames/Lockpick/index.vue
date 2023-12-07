<template>
    <div class="mglockpick">
        <img src="/img/miniGames/lockpick/ring.png" alt="ring" class="mglockpick_ring" 
            v-for="(ring, key, index) in rings" :key="index" 
            :ringid="key"
            :style="{'transform': `rotateZ(${ring.angle + ring.offset}deg)`}"
        >
        <img src="/img/miniGames/lockpick/ring_centr.png" alt="ring_centr" class="mglockpick_ring" @click="check">
        <img src="/img/miniGames/lockpick/ring_border.png" alt="ring_centr" class="mglockpick_ring">
        <div v-if="selected !== -1" class="mglockpick_number">{{rings[selected].number}}</div>
        <div class="mglockpick_exit" @click="exit">exit</div>
    </div>
</template>

<script>
import { mapState } from 'vuex';
export default {
    data() {
        return {
            selected: -1,
            step: 1,
            mouse: {
                pressed: false,
                x: 0,
                y: 0,
                el: null
            },
            rings:{
                1: {angle: 0, offset: 0, number: 0, target: 0},                    
                2: {angle: 0, offset: 0, number: 0, target: 25},                    
                3: {angle: 0, offset: 0, number: 0, target: 35},
            }
        }
    },
    methods: {
        check(){
            if(this.spam > Date.now()) return;
            this.spam = Date.now() + 1000;
            if(this.rings[1].number === this.rings[1].target && this.rings[2].number === this.rings[2].target && this.rings[3].number === this.rings[3].target){
                window.mp.triggerServer(this.callback);
                this.$store.commit("notifyList/notify", { type: 2, position: 0, message: "mglockpick:result:succ", time: 1000 });
                this.exit();
            } 
            else
                this.$store.commit("notifyList/notify", { type: 1, position: 0, message: "mglockpick:result:bad", time: 1000 });
        },
        exit(){
            this.$store.commit("gameLockpick/setCallback", "noCallback");
            window.mp.trigger("mg:lockpick:quit");
        },
        onMouseDown(e){
            this.mouse.pressed = true;
            this.mouse.x = e.clientX;
            this.mouse.y = e.clientY;
            this.selected = document.elementFromPoint(this.mouse.x, this.mouse.y).getAttribute("ringid") || -1;
            //console.log(`x: ${e.clientX} y: ${e.clientY} s: ${this.selected}`)
        },
        onMouseUp(){
            if(!this.rings[this.selected]) return;
            this.mouse.pressed = false;
            this.rings[this.selected].angle += this.rings[this.selected].offset;
            this.rings[this.selected].angle %= 360;
            this.rings[this.selected].offset = 0;
            this.selected = -1;
        },
        onMouseMove(e){
            if(this.mouse.pressed && this.selected !== -1){
                const newVal = Math.round((e.clientX - this.mouse.x) * 3.6);
                if(newVal !== this.rings[this.selected].offset){
                    const summ = newVal + this.rings[this.selected].angle;
                    if(summ > 359)
                        this.rings[this.selected].offset = newVal - 360;
                    else if(summ < 0)
                        this.rings[this.selected].offset = newVal + 360
                    else
                        this.rings[this.selected].offset = newVal;
                    
                    this.checkNumber(this.rings[this.selected]);
                }
            }
        },
        checkNumber(ring){
            var newNumber = 100 - Math.round((ring.offset + ring.angle) % 360 / 3.6);
            if(newNumber > 99) newNumber -= 100;
            if(newNumber !== ring.number){
                if(newNumber === ring.target)                    
                    this.$store.commit("sounds/play", { name: `roulette_click`, volume: 0.35}); 
                else                    
                    this.$store.commit("sounds/play", { name: `tick`, volume: 0.25}); 
                ring.number = newNumber;
            }
        },
        getRandomResults(){
            for (const key in this.rings) {
                const ring = this.rings[key];
                ring.number = 0;
                ring.offset = 0;
                ring.angle = 0;
                ring.target = Math.floor(Math.random() * 99);
            }
        }
    },
    computed:{
        ...mapState("gameLockpick", ["callback"])
    },
    mounted() {
        this.getRandomResults();
        addEventListener("mousedown", this.onMouseDown);
        addEventListener("mouseup", this.onMouseUp);
        addEventListener("mousemove", this.onMouseMove);
    },
    beforeDestroy(){
        removeEventListener("mousedown", this.onMouseDown);
        removeEventListener("mouseup", this.onMouseUp);
        removeEventListener("mousemove", this.onMouseMove);
    }
}
</script>

<style lang="scss">
    .mglockpick{
        width: 25rem;
        height: 25rem;
        position: relative;
        display: flex;
        align-items: center;
        justify-content: center;
        &_ring{
            position: absolute;
            border-radius: 50%;
            cursor: pointer;
            -webkit-user-drag: none;
            &:hover{
                filter: brightness(1.2);
            }
            &:nth-child(1){
                width: 100%;
                height: 100%;
            }
            &:nth-child(2){
                width: 70%;
                height: 70%;
            }
            &:nth-child(3){
                width: 48%;
                height: 48%;
            }
            &:nth-child(4){
                -webkit-user-drag: none;
                position: absolute;
                width: 32%;
                height: 32%;
                //pointer-events: none;
            }
            &:nth-child(5){
                width: 118%;
                height: 118%;
                pointer-events: none;
            }
        }
        &_number{
            position: absolute;
            font-size: 1rem;
            //background: rgba(#fff, .5);
            //border: .1rem solid rgba(#fff, .5);
            border-radius: .2rem;
            top: -1.2em
        }
        &_exit{
            position: absolute;
            top: -1.5rem;
            right: -1.5rem;
            color: #fff;
            font-size: 1.5rem;
            text-transform: uppercase;
            letter-spacing: .05rem;
            &:hover{
                color: #ccc;
            }
        }
    }
</style>