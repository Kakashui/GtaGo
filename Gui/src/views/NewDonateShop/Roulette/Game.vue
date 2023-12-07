<template>
    <div class="newdshop-roull-game" ref="drparent">
        <div class="newdshop-roull-game_subtitle">{{loc('newdshop:roul:game:sub')}}</div>
        <div class="newdshop-roull-game_title">{{type.name}}</div>
        <div class="newdshop-roull-game_top"></div>
        <div class="newdshop-roull-game_top"><img src="/img/newDonateShop/roulette/marker.svg" alt="marker"></div>
        <canvas :width="width" :height="height" ref="drcanvas"/>
        <div class="newdshop-roull-game_bottom"><img src="/img/newDonateShop/roulette/marker.svg" alt="marker"></div>
        <div class="newdshop-roull-game-button" @click="start">
            <div class="newdshop-roull-game-button_container" v-if="this.isRun">
                <div>{{loc('newdshop:roul:game:btn:stop')}}</div>
            </div>
            <div class="newdshop-roull-game-button_container" v-else>
                <div>{{loc('newdshop:roul:game:btn:start')}}</div>
                <div class="newdshop-roull-game-button_decor"></div>
                <img :src="currencyImg" alt="coin">
                <div>{{type.price.toLocaleString()}}</div>
            </div>
        </div>
    </div>
</template>

<script>
import {mapState, mapGetters, mapActions} from 'vuex'
import Slot from './Slot'
import {rouletteItemsById} from '../../../configs/newDonateShop'

export default {
    props:{
        type: Object
    },
    data() {
        return {
            width: 0,
            height: 0,
            canvas: null,
            ctx: null,
            countElements: 5,
            speed: 0,
            frameRate: 36,
            maxSpeed: 150,
            duration: 1500,
            timeToStop: 0,
            sizeItem: 0,
            state:[],
            itemList: [],
            bgImg: null,
            lastFrame: 0,
            headResult: null,
            winResult: null,
            cancel: false,
            antiFlood: 0,
            stopFrom: 0,
            sound: true,
            winBuilded: false,
            floodProtection: 0
        }
    },
    computed:{
        ...mapState('newDonateShop/roulette', ['isRun', 'isStop', 'isWin', 'result']),
        ...mapState('newDonateShop', ['coins', 'gender']),
        ...mapState('hud', ['money']),
        ...mapGetters('localization', ['loc']),
        currencyImg(){
            return this.type.forCoins ? '/img/newDonateShop/header_coin.png' : '/img/newDonateShop/header_money.svg'
        }
    },
    methods: {
        ...mapActions('newDonateShop/roulette', ['requestStart','requestStop', 'requestFastStop', 'requestWin']),
        start(){
            //if(this.floodProtection > Date.now()) return;
            if(this.isRun){
                if(this.result !== -1 && !this.winBuilded){
                    this.speed = 0;
                    this.state = [];
                    this.timeToStop = Date.now();
                    this.builWinCombination(this.result);
                    this.requestFastStop(this.type.forCoins);
                }
            }
            else
            {
                if(this.type.forCoins && this.coins < this.type.price){
                    window.setData('notifyList/notify', {type: 1, position:2, message: 'dshop:item:buy:wrong:coins', time: 3000});
                    return;
                }
                if(!this.type.forCoins && this.money < this.type.price){
                    window.setData('notifyList/notify', {type: 1, position:2, message: 'dshop:item:buy:wrong:money', time: 3000});
                    return;
                }
                //this.floodProtection = Date.now() + 500;
                this.winBuilded = false;
                this.requestStart(this.type.id);
                this.timeToStop = Date.now() + this.duration;               
            }              
        },
        addRandomElement(){
            const last = this.state[this.state.length - 1];
            let currentOffset = last ? (last.posX + last.size) : 0;
            
            const item = this.itemList[Math.floor(Math.random()*this.itemList.length)];
            const slot = new Slot(this.ctx, item, this.height, this.bgImg, currentOffset);
            this.state.push(slot);
            return slot;
        },
        addWinElement(id){
            const last = this.state[this.state.length - 1];
            let currentOffset = last ? (last.posX + last.size) : 0;
            const item = this.itemList.find(i=>i.id == id);
            const slot = new Slot(this.ctx, item, this.height, this.bgImg, currentOffset);
            this.state.push(slot);
            return slot;
        },
        removeElement(){
            this.state.shift();
            //this.$store.commit("sounds/play", { name: `tick`, volume: 0.03 })
        },
        initGame(){
            this.state = [];
            for (let index = 0; index < this.countElements + 2; index++) {
                this.addRandomElement();                
            }
            requestAnimationFrame(this.updateState, this.canvas);
            document.addEventListener("keyup", this.keyHandler)
        },
        updateState(){
            if(this.cancel) return;
            if(this.lastFrame < Date.now() ){
                if(this.isRun){
                    this.lastFrame = Date.now() + this.frameRate;

                    if(this.timeToStop < Date.now())
                        this.requestStop();

                    if(this.isStop)
                        this.handleStop();
                    else
                        this.handleLoop();                    
                }
                this.drawFrame();
            }
            requestAnimationFrame(this.updateState, this.canvas);
        },
        handleLoop(){
            if(!this) return
            if(this.maxSpeed > this.speed)
                this.speed += 5;

            if(this.state[0].canDelete()){
                this.removeElement();
                this.addRandomElement();
            }

            this.state.forEach(item => {
                item.moveX(this.speed);
            });
        },
        handleStop(){
            if(this.speed > this.maxSpeed * .2){
                if(this.state[0].canDelete()){
                    this.removeElement();
                    this.addRandomElement();
                }

                this.state.forEach(item => {
                    item.moveX(this.speed);
                });
                this.speed -= 1;
            }else{                
                if(!this.winBuilded){                   
                    this.builWinCombination(this.result);
                    this.stopFrom = this.headResult.posX;
                }
                const kof = Math.abs(this.headResult.posX / this.stopFrom);                
                this.speed = Math.max(Math.floor(this.maxSpeed * kof * .2), 5);    
                if(this.headResult.posX < 1) {
                    const fix = this.headResult.posX;
                    this.state.forEach(item => {
                        item.fixPos(fix);
                    });
                    this.speed = 0;
                    this.requestWin(this.type.forCoins);
                }else{
                    this.state.forEach(item => {
                        item.moveX(this.speed);
                    });
                }
                if(this.state[0].canDelete()){
                    this.removeElement();
                }
            }
        },
        builWinCombination(val){
            this.winBuilded = true;
            for (let index = 0; index < this.countElements; index++) {
                if(index == 0){
                    this.headResult = this.addRandomElement();                  
                }else if(index == 2){
                    this.winResult = this.addWinElement(val);
                }else
                    this.addRandomElement();                            
            }
        },
        drawFrame(){
            this.ctx.globalCompositeOperation ="normal";
            this.ctx.clearRect(0,0, this.width, this.height);
            this.state.forEach(item => {
                item.draw();
            });
            
            if(this.isWin && this.winResult)
                this.winResult.drawEffect();

            let gradient = this.ctx.createLinearGradient(0,0, this.width, this.height);
            gradient.addColorStop(0, 'rgba(0,0,0,0)');
            gradient.addColorStop(.05, 'rgba(0,0,0,.1)');
            gradient.addColorStop(.2, 'rgba(0,0,0,1)');
            gradient.addColorStop(.8, 'rgba(0,0,0,1)');
            gradient.addColorStop(.95, 'rgba(0,0,0,.1)');
            gradient.addColorStop(1, 'rgba(0,0,0,0)');
            this.ctx.fillStyle = gradient;
            this.ctx.globalCompositeOperation ="destination-in";
            this.ctx.beginPath();
            this.ctx.fillRect(0,0,this.width, this.height);
        },
        initImages(){
            this.bgImg = new Image(this.height, this.height);
            this.bgImg.src = "/img/newDonateShop/roulette/itemBg.png";
            this.itemList.forEach(item => {
                item.img = new Image(this.height, this.height);
                item.img.src = `/img/newDonateShop/items/${item.image}.png`;
            });
        },
        keyHandler(e){
            if(e.keyCode === 13){
                this.start();
            }
        }
    },
    mounted(){
        this.itemList = [];
        this.$store.commit('newDonateShop/roulette/reset');
        const parrent = this.$refs["drparent"];
        this.width = parrent.offsetWidth;
        this.height = parrent.offsetWidth / this.countElements;
        this.itemList = rouletteItemsById(this.type.id, this.gender);
        this.initImages();
        this.canvas = this.$refs["drcanvas"];
        this.ctx = this.canvas.getContext("2d");
        this.frameRate = Math.floor(1000/this.frameRate);
        setTimeout(this.initGame, 500);
    },
    beforeDestroy(){
        document.removeEventListener("keyup", this.keyHandler);
        this.cancel = true;
    }
}
</script>

<style lang="scss">
    .newdshop-roull-game{
        width: 80rem;
        align-self: flex-start;
        margin-top: 1.5rem;
        &_subtitle{
            margin-top: 3rem;
            font-size: .8rem;
            color: rgba(#fff, .5);
            text-align: center;
        }
        &_title{
            font-size: 3.5rem;
            font-weight: 700;
            letter-spacing: .07rem;
            text-align: center;
            margin-bottom: 4rem;
            text-shadow: 0 0 .8rem rgba(#fff, .7);
        }
        &_top{
            text-align: center;
        }
        &_bottom{
            text-align: center;
            transform: rotate(180deg);
        }
        &-button{
            background-color: #4B8A9E;
            width: 10.7rem;
            margin: 3rem auto;
            border-radius: .2rem;
            transform: skew(-10deg);
            &_container{
                height: 3rem;
                width: 10.7rem;
                transform: skew(10deg);
                display: flex;
                justify-content: center;
                align-items: center;
            }
            &_decor{
                background-color: rgba(255, 255, 255, 0.2);
                width: 1px;
                height: 1.5rem;
                margin: 0 .5rem;
            }
            img{
                margin: 0 .2rem;
                
            }
            &:hover{
                transform: scale(1.02) skew(-10deg);
                box-shadow: 0 0 .5rem #4B8A9E;
            }     
        }
        canvas{
            margin-left: .6rem;
        }
    }
</style>