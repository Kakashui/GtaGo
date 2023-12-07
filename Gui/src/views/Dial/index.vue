<template>
    <div :class="{'dial': true,'shake3': shake1,'shake2': shake2,'shake1': shake3}">
        <div class="img base"></div>
        <div class="img num" :style="{'transform': `rotate(${-rot}deg)`}"></div>
        <div class="img center"></div>
    </div> 
</template>

<script>
export default {
    data() {
        return {
            rot: 0,
            off : 2,
            audio: null,
            val:4
        }
    },
    methods: {
        keyPress(e){
            if(e.which == 37 || e.which == 38)
                this.update(-1)
            else if(e.which == 39 || e.which == 40)
                this.update(+1)
            else if(e.which == 32 || e.which == 13)
                this.send();
        },
        send(){
            if(this.rot == this.val){
                window.mp.trigger("dial", 'call', true);
            } else {
                window.mp.trigger("dial", 'call', false);
            }
        },
        update(to){
            this.rot += +to;
            if(this.rot > 359)this.rot = 0;
            if(this.rot < 0)this.rot = 359;
            if(this.rot % 2) return;
            this.audio.play();
        }
    },
    computed:{
        shake1(){
            return (this.rot == this.val)
        },
        shake2(){
            return (this.val - this.off <= this.rot && this.rot <= this.val + this.off)
        },
        shake3(){
            return (this.val - this.off - Math.random(2, 4) <= this.rot && this.rot <= this.val + this.off + Math.random(2, 4))
        }
    },
    mounted(){
        document.addEventListener("keydown", this.keyPress)
        this.audio = new Audio();
        this.audio.src = "/sounds/dial/tick.mp3";
        this.audio.volume = .4;
    },
    beforeDestroy(){
        document.removeEventListener("keydown",this.keyPress)
    }
}
</script>

<style lang="scss" scoped>
.dial{
    position: absolute;
    width: 300px;
    height: 300px;
    left: 50%;
    top: 50%;
    margin: -150px 0 0 -150px;
}
.dial .img{
    background-repeat: no-repeat;
    background-size: contain;
    width: 300px;
    height: 300px;
    position: fixed;
}
.dial .base{background-image: url('/img/dial/s1.png')}
.dial .num{background-image: url('/img/dial/s2.png')}
.dial .center{background-image: url('/img/dial/s3.png')}

@keyframes shake {
    0% { transform: translate(1px, 1px); }
    10% { transform: translate(-1px, -1px); }
    20% { transform: translate(-1px, 0px); }
    30% { transform: translate(0px, 1px); }
    40% { transform: translate(1px, -1px); }
    50% { transform: translate(-1px, 1px); }
    60% { transform: translate(-1px, 1px); }
    70% { transform: translate(1px, 1px); }
    80% { transform: translate(-1px, -1px); }
    90% { transform: translate(1px, 1px); }
    100% { transform: translate(1px, -1px); }
}
.shake1 {
    animation-name: shake;
    animation-duration: 6s;
    transform-origin:50% 50%;
    animation-iteration-count: infinite;
    animation-timing-function: linear;
}
.shake2 {
    animation-name: shake;
    animation-duration: 5s;
    transform-origin:75% 75%;
    animation-iteration-count: infinite;
    animation-timing-function: linear;
}
.shake3 {
    animation-name: shake;
    animation-duration: 4s;
    transform-origin:100% 100%;
    animation-iteration-count: infinite;
    animation-timing-function: linear;
}
.shake4 {
    animation-name: shake;
    animation-duration: 3s;
    transform-origin:50% 50%;
    animation-iteration-count: infinite;
    animation-timing-function: linear;
}
.shake1,.shake2,.shake3,.shake4 {
    display:inline-block;
}
</style>