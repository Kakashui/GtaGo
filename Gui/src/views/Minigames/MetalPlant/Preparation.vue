<template>
    <div class="mplant-prepare">
        <div class="mplant-prepare_body">
            <div class="mplant-prepare_title">
                {{loc("mplant:prepare:tit")}}
            </div> 
            <div class="mplant-prepare_btn" @click="start">
                Start
            </div>
            <div class="mplant-prepare_delimiter"></div>
            <div class="mplant-prepare_sub">{{loc("mplant:prepare:sub")}}</div>
            <div class="mplant-prepare-info">
                <div class="mplant-prepare-info_row">
                    <div class="mplant-prepare-info_block">
                        <div class="mplant-prepare-info_title">{{loc("mplant:prepare:invalid")}}</div>
                        <div class="mplant-prepare-info_elements">
                            <Element v-for="(element, index) in allInvalidElements" :key="index" :element="element"/>
                        </div>
                    </div>
                    <div class="mplant-prepare-info_block">
                        <div class="mplant-prepare-info_title">{{loc("mplant:prepare:valid")}}</div>
                        <div class="mplant-prepare-info_elements">
                            <Element v-for="(element, index) in allValidElements" :key="index" :element="element"/>                            
                        </div>
                    </div>
                </div>
                <div class="mplant-prepare_delimiter" style="transform: scale(.7, .3);"></div>
                <div class="mplant-prepare-info_row">
                    <div class="mplant-prepare-info_block">
                        <div class="mplant-prepare-info_elements">
                            <Element :element="tempUpElement"/>
                            <div class="mplant-prepare-info_title">{{loc("mplant:prepare:tup")}}</div>
                        </div>
                    </div>
                    <div class="mplant-prepare-info_block">
                        <div class="mplant-prepare-info_elements"> 
                            <Element :element="tempDownElement"/>
                            <div class="mplant-prepare-info_title">{{loc("mplant:prepare:down")}}</div>                      
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import { mapActions, mapGetters } from 'vuex';
import Element from './Element.vue';

export default {
    data() {
        return {
            timer: 5,
            interval: -1
        }
    },
    computed:{
        ...mapGetters("gameMetalPlant", ["allValidElements", "allInvalidElements", "tempUpElement", "tempDownElement"]),
        ...mapGetters("localization", ["loc"])
    },
    methods: {
        tick(){
            this.timer--;
            if(this.timer < 1){
                //clearInterval(this.interval);
                this.startGame();
                this.$store.commit("sounds/play", { name: `metalPlant_start`, volume: 0.5})
            }else this.$store.commit("sounds/play", { name: `metalPlant_timer`, volume: 0.5})
        },
        ...mapActions("gameMetalPlant", ["startGame"]),
        start(){
            this.startGame();
            this.$store.commit("sounds/play", { name: `metalPlant_start`, volume: 0.5})
        }
    },
    components:{
        Element
    },
    mounted(){
        //this.interval = setInterval(this.tick, 1000);        
        //this.$store.commit("sounds/play", { name: `metalPlant_timer`, volume: 0.5});
    },
    beforeDestroy(){
        //clearInterval(this.interval)
    }
}
</script>

<style lang="scss">
    .mplant-prepare{
        z-index: 500;
        position: absolute;
        top: 0;
        left: 0;
        width: 100vw;
        height: 100vh;
        background-color: rgba(#000, .8);
        display: flex;
        align-items: center;
        justify-content: center;
        &_body{
            width: 40rem;
            height: 32.5rem;
        }
        &_title{
            font-size: 3rem;
            text-transform: uppercase;
            text-align: center;
        }
        &_sub{
            color: rgba(#fff, .6);
            font-size: 1.2rem;
            text-align: center;
            margin-top: 1.5rem;
        }
        &_btn{
            font-size: 3rem;
            text-shadow: 0 0 .5rem #FF0012;
            text-align: center;
            margin: 1.5rem auto;
            border: .1rem solid #FF0012;
            border-radius: .4rem;
            text-transform: uppercase;
            width: 15rem;
            &:hover{
                transform: scale(1.02);
            }
        }
        &_delimiter{
            height: .1rem;
            margin: 1.5rem auto;
            background: linear-gradient(135deg, rgba(#fff, .1) 0% , rgba(#fff, .3) 20%, rgba(#fff, .5) 50%, rgba(#fff, .3) 80%, rgba(#fff, .1) 100%);
        }
        &-info{
            width: 100%;
            height: 16.8rem;
            border-radius: 1rem;
            border: 1px solid rgba(#fff, .3);
            margin-top: 1rem;
            &_row{
                display: flex;
                justify-content: space-around;
                margin-top: 2rem;
            }
            // &_block{         
            //     // border: 1px solid #fff;
            // }
            &_title{
                font-size: 1.2rem;
            }
            &_elements{
                display: flex;
                align-items: center;
                margin-top: .6rem;
                width: 13rem;
                //border: 1px solid #fff;
                div{
                    margin-right: .6rem;
                }
            }
        }
    }
</style>