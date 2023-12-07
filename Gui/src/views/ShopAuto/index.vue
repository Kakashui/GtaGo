<template>
    <div class="auto">
        <div class="auto-items auto-menu">
            <div class="container">
                <div 
                    v-for="(item, index) in models" :key="`i_${index}`" 
                    class="auto-item" 
                    :class="{'auto-active': current == index}" @click="selectCar(index)"
                >
                    {{ getModelName(index) }} 
                    <span class="auto-price">
                        {{ getPrice( index ) }}$
                    </span>
                </div>
            </div>
        </div>    
        <div class="select-car" v-show="!selected">{{loc(selectCarText)}}</div>
        <div class="auto-colors auto-menu">
            <div class="container">
                <div v-for="(color, index) in colors" :key="`c_${index}`" :style="{'background-color': color.val}" class="auto-color" @click="selectColor(index)" :class="{'auto-color-active': currentColor == index}">
                </div>
            </div>
        </div>
        <div class="auto-btns">
              
            <div class="container">
                <div @click="buy" class="auto-btn auto-buy"> {{ `${loc('cef_287')} ${ prices[current]} '$'`}} </div>
                <div @click="exit" class="auto-btn auto-exit">{{loc('cef_288')}}</div>
                <div @click="testdrive" class="auto-btn auto-exit">{{loc('autoshop_1')}}</div>
            </div>
        </div>
    </div>
</template>

<script>
import { mapState, mapGetters } from 'vuex'

export default {
    data() {
        return {
            current: 0,
            selected: false,
            currentColor: 0,
            warning: 'cef_294',
            selectCarText: 'cef_295',
            indexM: 0,
            indexC: 0,
            modelName: {
                "16challenger": "Dodge challenger",
                "bmwe34": "Bmw e34",
                "bmwm2": "Bmw M2",
                "bentyaga18": "Bentley Bentayga",
                "rrphantom": "Phantom",
                "snake": "Mustang Shelby GT",
                "g65amg":"Mercedes G65 AMG",
                "e63s":"Mercedes E63s",
                "18rs7":"Audi-RS7",
                "2019M5":"BmwM5f90",
                "amggt":"Mercedes AMG-GT",
                "cayen19":"NEWCayenne",
                "g63":"Mercedes G63 AMG",
                "gt63":"Mercedes GT63s",
                "gtr":"Nissan GTR",
                "impreza18":"Subaru Impreza",
                "lc200":"LandCruiser200",
                "m8":"Bmw M8",
                "maseratigt":"Maserati GT",
                "maybach":"Mercedes Maybach",
                "mustang65":"Ford Mustang GT",
                "panamera":"Porshe Panamera Turbo",
                "rrover17":"RangeRover Autobiographie",
                "rs6":"Audi RS6",
                "senna":"Maclaren-Senna",
                "srt8":"Jeep SRT-8",
                "m5e60":"BmwM5e60",
                "s63":"Mercedes s63coupe",
                "shelbygt500":"ShelbyGT500",
                "w140":"Mercedes-w140",
                "tahoe":"Chevrolet-Tahoe",
            }
        }
    },
    watch: {
        active(oldVal, newVal){
            if(newVal) this.selected = false;
        }
    },
    methods: {
        getModelName(index){
            return this.displayNames[index];
        },
        selectCar(index){
            this.current = index;
            if(!this.selected) this.selected = true;
           window.mp.trigger('auto','model', this.current)
        },
        selectColor(index){
            this.currentColor = index;
           window.mp.trigger('auto','color', this.currentColor)
        }, 
        buy(){
            if(!this.selected) return;
           window.mp.trigger('buyAuto')
        },
        exit(){
            this.reset()
           window.mp.trigger('closeAuto')
        },
        testdrive(){
           window.mp.trigger('testDriveAuto')
        },
        reset(){
            this.models=[]
            this.colors=[]
            this.prices=[]
            this.displayNames=[]
        },
        getPrice(index){            
            //return this.prices[index] +  (this.vip ? ' GOCOINS' : ' $');
            return this.prices[index] +   ' $';
        }
    },
    computed:{
        ...mapGetters('localization',['loc']),
        ...mapState('shopAuto', ['vip', 'models', 'colors', 'prices', 'displayNames'])
    }
}
</script>

<style lang="scss" scoped>

.auto{
    position: fixed;
    left: 0;
    right: 0;
    top: 0;
    bottom: 0;
    width: 100vw;
    height: 100vh;  
    display: flex;
    justify-content: space-between;
    align-items: center; 
    background: url(/client_packages/cef/img/bg_auth.jpg);
 }
 
 .container{
     width: 90%;
     height: 100%;
     margin: 0 auto;
     padding: 4vh 0;
 }
 
 .auto-menu{
     perspective: 350px;
     width: 20vw;
     text-align: center;
     margin: -10vh 2vh 0;
 }
 
 .auto-item{
     margin: 1vh 0;
     padding: 1vh;
     color: #fff;
     font-size: 1.4rem;
 }
 .auto-item:hover{
     background-color: rgba(255, 255, 255, .3);
 }
 .auto-active{
     background-color: rgba(255, 255, 255, .3);  
 }
 .auto-color{
     width: 4vh;
     height: 4vh;
     margin: 1vh;
     border-radius: 50%;
 }
 .auto-color:hover{
     box-shadow: 0 0 1vh 0 #fff;
     transform: scale(1.1);
 }
 .auto-color-active{
     box-shadow: 0 0 1vh 0 #fff;
     transform: scale(1.1);
 }
 .auto-colors .container{
     transform: rotateY(-10deg);
     display: flex;
     justify-content: center;
     align-items: center;
     flex-wrap: wrap;
     background-color: rgba(0, 0, 0, .4);
 }
 
 .auto-items .container{
     transform: rotateY(10deg);
     overflow: hidden scroll;
     max-height: 50vh;
     background-color: rgba(0, 0, 0, .4);
 }
 .container::-webkit-scrollbar {
   display: none;
 }

 .auto-btns{
     position: absolute;
     bottom: 0;
     width: 100vw;
     background-color: rgba(255, 255, 255, .1);
 }
 .auto-btns .container{
     display: flex;
     width: 50vh;
 }
 .auto-btn{
     width: 10vw;
     border-radius: 7px;
     margin: 2vh 1vw;
     padding: 1vh;
     text-align: center;
 }
 .auto-btn:hover{
     transform: scale(1.05);
     transition: all .3s;
     box-shadow: 2px 2px 5px 0 #000;
 }
 .auto-buy{
     background-color: #8FB700;
 }
 .auto-exit{
     background-color: rgba(0,0,0);
     color: #ffffff;
 }
 .auto .warning{
     color: red;
     text-align: center;
     font-weight: bold;
 }
 .select-car{
    font-size: 3rem;
    font-weight: bold;
    color: #FFF;
}
</style>