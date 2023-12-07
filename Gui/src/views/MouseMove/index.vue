<template>
    <div class="mouse-move"
        v-if="enabled"
        :style="{width: move.size.x + 'rem', height: move.size.y + 'rem'}"
        @mousemove.self="moveChar" 
        @mousedown="startMove" 
        @mouseup="cancelMove" 
        @mouseleave="cancelMove"
        @mousewheel="mousewheel"
    >
        <div
            v-if="move.position"
            class="mouse-move-icons"
            :style="{ left: move.position.left, top: move.position.top }"
        >
            <img v-for="(ico, index) in icons" :key="index" :src="`/img/mouseMove/${ico}.svg`" alt="ico" v-show="move.showIcons[index]">
        </div>
        <div v-else class="mouse-move-icons">
            <img v-for="(ico, index) in icons" :key="index" :src="`/img/mouseMove/${ico}.svg`" alt="ico" v-show="move.showIcons[index]">
        </div>
    </div>
</template>

<script>
import {mapState} from 'vuex'
export default {
    data() {
        return {
            lastPos:{
                x: 0,
                y: 0
            },
            startLeftBtn: false,
            startRigthBtn: false,
        }
    },
    computed: {
        ...mapState('mouseMove', ['move', 'enabled', 'icons'])
    },
    methods: {
        moveChar(e){
            if(this.startLeftBtn){
                let direction = this.getDirection(e);
                if(direction.x !== 0 && this.move.values[0].enabled){
                    if(this.move.values[0].invert) direction.x *= -1;
                    this.move.values[0].value = Math.min(Math.max((this.move.values[0].value + this.move.values[0].step * direction.x), this.move.values[0].min), this.move.values[0].max);             
                    window.mp.trigger(this.move.values[0].callback, this.move.values[0].value.toFixed(3));
                }
                if(direction.y !== 0 && this.move.values[1].enabled){
                        if(this.move.values[1].invert) direction.y *= -1;
                        this.move.values[1].value = Math.min(Math.max((this.move.values[1].value + this.move.values[1].step * direction.y), this.move.values[1].min), this.move.values[1].max);                    
                        window.mp.trigger(this.move.values[1].callback, this.move.values[1].value.toFixed(3));
                }
            }
            else if(this.startRigthBtn){
                let direction = this.getDirection(e);
                if(direction.x !== 0 && this.move.values[2].enabled){
                    if(this.move.values[2].invert) direction.x *= -1;
                    this.move.values[2].value = Math.min(Math.max((this.move.values[2].value + this.move.values[2].step * direction.x), this.move.values[2].min), this.move.values[2].max);
                    window.mp.trigger(this.move.values[2].callback, this.move.values[2].value.toFixed(3), this.move.values[0].value.toFixed(3));
                }
                if(direction.y !== 0 && this.move.values[3].enabled){
                    if(this.move.values[3].invert) direction.y *= -1;
                    this.move.values[3].value = Math.min(Math.max((this.move.values[3].value + this.move.values[3].step * direction.y), this.move.values[3].min), this.move.values[3].max);                    
                    window.mp.trigger(this.move.values[3].callback, this.move.values[3].value.toFixed(3));
                }
            }
        },
        getDirection(e){
            const ditX = e.screenX - this.lastPos.x;
            const ditY = e.screenY - this.lastPos.y;
            const result = {x: 0, y: 0};

            if(ditX > 0) result.x = 1;
            else if(ditX < 0) result.x = -1;
            
            if(ditY > 0) result.y = 1;
            else if(ditY < 0) result.y = -1;

            this.lastPos.x = e.screenX
            this.lastPos.y = e.screenY

            return result;
        },
        cancelMove(){
            this.startLeftBtn = false;
            this.startRigthBtn = false;
        },
        startMove(e){
            this.lastPos.x = e.screenX;
            this.lastPos.x = e.screenY;
            if(e.button == 0) this.startLeftBtn = true;
            else if(e.button == 2) this.startRigthBtn = true;
        },
        mousewheel(e){
            if( !this.move.values[4].enabled) return;
            let direction = 0;
            if(e.deltaY > 0) direction = 1;
            else if(e.deltaY < 0) direction = -1;
            if(this.move.values[4].invert) direction *= -1;
            this.move.values[4].value = Math.min(Math.max((this.move.values[4].value + this.move.values[4].step * direction), this.move.values[4].min), this.move.values[4].max);
            window.mp.trigger(this.move.values[4].callback, this.move.values[4].value.toFixed(3), this.move.values[0].value.toFixed(3));
        },
        click(){
            this.$store.commit('sounds/play', {name:"click", volume: .5})
        }
    },
}
</script>

<style lang="scss" scoped>
    .mouse-move{
        width: 45rem;
        height: 40rem;
        position: absolute;
        border-radius: 50%;
        &-icons{
            position: fixed;
            top: 94%;
            height: 5rem;
            display: flex;
            align-items: center;
            justify-content: center;
            left: 50%;
            transform: translate(-50%, -50%);
            img{
                opacity: .7;
                height: 90%;
                margin: 0 1rem;
                &:nth-child(1){                    
                    height: 110%;
                    margin-top: -1rem;
                }
            }
        }
    }
</style>