<template>
    <div>
        <div class="craft">
            <div class="craft-close" @click="close">X</div>
            <div class="craft-materials">{{`${loc('craft_5')}: ${materials} ${loc('craft_1')}`}}</div>
            <div class="craft-tittle">{{loc('craft_2')}}</div>
            <div class="craft-container" v-if="items.length > 0">
                <div class="craft-info">
                    <div class="craft-info-price">{{`${items[current].price * count} ${loc('craft_1')}`}}</div>
                    <div class="craft-info-center">
                        <img :src="`/img/craftMenu/bg.png`" alt="bg" class="craft-info-bg">
                        <img :src="`/img/common/weapons/img_${items[current].img}.png`" alt="item" class="craft-info-img" :style="{
                'transform': `rotate(${items[current].rotate}deg)`}">
                        <div class="craft-info-btn" @click="craft">
                            <div class="craft-info-btn-item">Craft <br/> x {{count}}</div>
                        </div>
                    </div>
                    <div class="craft-info-tittle">{{items[current].tittle}}</div>
                </div>
                <div class="craft-items">
                    <div class="craft-item" v-for="(item, index) in items" :key="index" @click="select(index)">
                        <img :src="`/img/common/weapons/img_${item.img}.png`" alt="item" class="craft-item-img">
                        <div class="craft-item-info">
                            <div class="craft-item-info-tittle">{{item.tittle}}</div>
                            <div class="craft-item-info-price">{{`${item.price} ${loc('craft_1')}`}}</div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="craft-range" :style="{transform: `translateY(${range}rem)`}">
            <input type="range" v-model="count">
        </div>
    </div>
</template>

<script>
import {mapState, mapGetters} from 'vuex'

export default {
    data() {
        return {
            current: 0,
            count: 1,
            range: -5,
            lasrCheck: 0
        }
    },
    computed: {
        ...mapGetters('localization',['loc']),
        ...mapState('craftMenu',['items', 'materials']),        
    },
    methods: {
        select(index){
            this.count = 1;
            this.current = index;
            this.range = this.items[this.current].isAmmo ? 1 : -15;
        },
        craft(){
            if(Date.now() < this.lastCheck) {
                this.$store.commit('notifyList/notify', {type: 1, position: 0, message: this.loc('craft_3'), time: 1500})
                return;
            }
            this.lastCheck = Date.now() + 500;
            const item = this.items[this.current];
            if(!item) return;
            const count =  item.isAmmo ? this.count : 1;
            if(item.price * count > this.materials) {
                this.$store.commit('notifyList/notify', {type: 1, position: 0, message: this.loc('craft_4'), time: 2000});
                return;
            }
            window.mp.triggerServer(item.isAmmo ? "wcraftammo" : "wcraft", this.current, count);
        },
        close(){
            window.mp.trigger('guiClose');
        }
    },
    beforeCreate(){
        //this.$store.commit('craftMenu/setData', {frac:10, mats:5})
    }
}
</script>

<style lang="scss" scoped>
    .craft{
        width: 60rem;
        height: 35rem;
        background: url(/img/common/textures/texture_wite_1024.jpg) no-repeat center;
        background-size: cover;
        position: relative;
        border-radius: .2rem;
        perspective: 18rem;
        z-index: 2;
        box-shadow: -.2rem .2rem .3rem 0 rgba(#000,.5);
        &-materials{
            position: absolute;
            left: 3rem;
            top: 2.5rem;
            font-weight: bold;
            color: #666;
        }
        &-tittle{
            height: 4rem;
            display: flex;
            align-items: center;
            justify-content: center;
            font-weight: bold;
            font-size: 1.6rem;
            width: 100%;
            text-shadow: -.1rem .1rem .2rem rgba(#000,.5);
            text-transform: uppercase;
        }
        &-container{
            display: flex;
            justify-content: space-around;
            align-items: flex-start;
            position: relative;
            height: 32rem;
            width:100%;
        }
        &-info{
            width: 40%;
            height: 85%;
            background-color: rgba(#fff,.7);
            border-radius: .2rem;
            box-shadow: -.2rem .2rem .3rem 0 rgba(#000,.5);
            transform: rotateY(1deg);
            &-price{
                text-align: center;
                font-weight: bold;
                font-size: 1.2rem;
                padding: 1rem 0;
            }
            &-center{
               width: 20rem;
               height: 20rem;
               margin: auto;
               position: relative;
               display: flex;
               align-items: center;
               justify-content: center;
            }
            &-img{
                width: 130%;
                margin-left: -1rem;
            }       
            &-bg{
               position: absolute;
               width: 100%;
               left: -1.5rem;
            }          
            &-btn{
               position: absolute;
               width: 7rem;
               height: 7rem;
               right: 0;
               perspective: 2rem;
               bottom: 0rem;
               text-align: center;
               &-item{
                   width: 100%;
                   height: 100%;
                   background: url(/img/common/textures/texture_button_go_now.jpg) no-repeat center ;
                   border-radius: 50%;
                   box-shadow: -.1rem .1rem .2rem 0 #000;
                   display: flex;
                   align-items: center;
                   justify-content: center;
                   color: #fff;
                   font-weight: bold;
                   transform: rotateZ(15deg);
                   text-transform: uppercase;
                   font-size: 1.3rem;
                   transition: all .4s;
                   text-shadow: -.1rem .1rem .2rem #000;
                   &:hover{
                        background: url(/img/common/textures/texture_black_for_circle.png) no-repeat  center;
                        transform: rotateZ(5deg) scale(1.1);
                        box-shadow: -.2rem .2rem .4rem 0 #000;
                   }
               }
            }    
            &-tittle{
                text-align: center;
                font-weight: bold;
                padding: 1rem 0;
                font-size: 1.5rem;
            }
        }
        &-items{
            width: 45%;
            height: 85%;
            border-radius: 1rem;
            overflow: auto auto;            
            transform: rotateY(-1deg);
            perspective: 10rem;
            &::-webkit-scrollbar {
                width: .5vh;
                background: rgba(229, 226, 226, 0.9);
            }
            &::-webkit-scrollbar-thumb {
                background: rgb(108, 109, 108);
                border-radius: 2vh;
            }
        }
        &-item{
            position: relative;
            width: 90%;
            height: 15%;
            margin: .3rem auto;
            border-radius: .2rem;
            background-color: rgba(#fff,.7);
            box-shadow: .2rem .2rem .2rem 0 rgba(#000, .5);
            display: flex;
            justify-content: space-between;
            align-items: center;
            transition: all .3s;
            &:hover{
                transform: scale(1.02) rotateY(1deg);
            box-shadow: -.2rem .2rem .4rem 0 rgba(#000, .5);
            }
            &-img{
                height: 100%;
                margin-left: 1.5rem;
            }
            &-info{
                height: 100%;
                width: 50%;
                margin-right: 1.5rem;
                display: flex;
                flex-direction: column;
                justify-content: center;
                &-tittle{
                    font-size: .8rem;
                    color: #666;
                    margin-bottom: .2rem;
                }
                &-price{
                    font-weight: bold;
                }
            }
        }
        &-close{
            position: absolute;
            right: 2rem;
            top: 1rem;
            background: url(/img/common/textures/texture_black_for_circle.png) no-repeat center;
            background-size: contain;
            width: 2rem;
            height: 2rem;
            border-radius: 50%; 
            color: #fff;
            display: flex;
            justify-content: center;
            align-items: center;
            transition: all .2s;
            box-shadow: -.2rem .2rem .3rem 0 #000;
            &:hover{
                background-image: url(/img/common/textures/texture_button_go_now.jpg);
                transform: scale(0.95);
                box-shadow: -.1rem .1rem .1rem 0 #000;
                border: 1px solid #fff;
            }
        }
        &-range{            
            background: url(/img/common/textures/texture_wite_1024.jpg) no-repeat center;
            background-size: cover;
            width: 30rem;
            height: 4rem;
            border-radius: 1rem;
            display: flex;
            align-items: center;
            justify-content: center;
            margin-left: 15rem;
            transform: translateY();
            transition: all .5s ease;
            z-index: 1;
            box-shadow: -.1rem .1rem .1rem 0 rgba(#000, .5);
            input{
                width: 80%;
                background: url(/img/common/textures/green_texture_long-112x300.jpg);
                background-size: cover;
                -webkit-appearance: none;
                box-shadow: .1vh .1vh .2vh 0 #000 inset;
                height: .4rem;
                border-radius: .5vh;
                &::-webkit-slider-thumb {
                    &:before{
                        display: block;
                        color: #fff;
                        content: "Dqwdqwd";
                        position: absolute;
                        left: 0;
                        top: 0;
                    }
                    -webkit-appearance: none;
                    background: url(/img/common/textures/texture_button_go_now.jpg);
                    border-radius: 3vh;
                    width: 6vh;
                    height: 3vh;
                    transition: all .5s;
                    box-shadow: -.2vh .2vh .5vh 0 #000;
                    position: relative;
                    &:hover{
                        background: url(/img/common/textures/texture_black_for_circle.png);
                    }
                }
            }
        }
    }
</style>