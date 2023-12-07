<template>
    <div class="weedfarm-comp">
        <div class="weedfarm-comp-info">
            <div class="weedfarm-comp-info_tit">
                {{loc('weedfarm:comp:info:tit')}}
            </div>
            <div class="weedfarm-comp-info-total">
                <div class="weedfarm-comp-info-total_tit">
                {{loc('weedfarm:comp:total:tit')}}</div>
                <div class="weedfarm-comp-info-total_amount">{{complited}} / {{total}}</div>
            </div>
        </div>
        <div class="weedfarm-comp_list">
            <div class="weedfarm-comp-item">
                <div class="weedfarm-comp-item_head1">{{loc("weedfarm:head:1")}}</div>
                <div class="weedfarm-comp-item_head2">{{loc("weedfarm:head:2")}}</div>
                <div class="weedfarm-comp-item_head2">{{loc("weedfarm:head:3")}}</div>
                <div class="weedfarm-comp-item_head2">{{loc("weedfarm:head:4")}}</div>
            </div>
            <div class="weedfarm-comp_list_container">
                <div class="weedfarm-comp-item" v-for="(pos, key) in traders" :key="key">
                    <div class="weedfarm-comp-item_img" :style="{
                        'background-image': `url(/img/weedFarm/components/${key}.png)`,  
                        'border': isExists(key) ? '.1rem solid #C1E704' : '.05rem solid rgba(255,255,255,.2)'}"></div>
                    <div class="weedfarm-comp-item_text">{{getName(key)}}</div>
                    <div class="weedfarm-comp-item_text">{{getAdress(key)}}</div>
                    <div class="weedfarm-comp-item_exists" v-if="isExists(key)"><img src="/img/weedFarm/check.svg" alt="check">{{loc('weedfarm:comp:exists')}}</div>
                    <div class="weedfarm-comp-item_btn" v-else  @click="setPath(pos)"><img src="/img/weedFarm/gps.svg" alt="check">{{loc('weedfarm:comp:gps')}}</div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
export default {
    data() {
        return {
            currPath: null
        }
    },
    computed:{
        ...mapState('weedFarm', ['traders', 'components']),
        ...mapGetters('localization', ['loc']),
        complited(){
            let count = 0;
            this.components.forEach(comp => {
                if(comp.Exists) count++;
            });
            return count;
        },
        total(){
            return Object.keys(this.traders).length;
        }
    },
    methods: {
        setPath(pos){
            if(this.currPath === pos) return;
            window.mp.trigger("weed:path:set", pos.x, pos.y);
            this.currPath = pos;
        },
        isExists(id){
            return this.components.findIndex(c=>c == id) !== -1;
        },
        getName(id){
            return this.loc(`weedfarm:comp:name:${id}`);
        },
        getAdress(id){
            return this.loc(`weedfarm:comp:adress:${id}`);
        } 
    },
}
</script>

<style lang="scss">
    .weedfarm-comp{
        width: 100%;
        height: 100%;
        display: flex;
        align-items: center;
        justify-content: space-between;
        background-color: #111;
        text-transform: uppercase;
        &-info{
            min-width: 26rem;
            height: 100%; 
            background: url(/img/weedFarm/bg2.png) no-repeat center;
            background-size: cover;
            border-right: 1px solid rgba(#fff, .2);
            &_tit{
                width: 20rem;
                font-size: 4rem;
                margin: 3rem auto;
            }
            &-total{
                width: 20rem;
                margin: auto;
                display: flex;
                align-items: center;
                &_amount{
                    margin-left: 1rem;
                    font-size: 4rem;
                }
                &_tit{
                    font-size: 1.1rem;
                    letter-spacing: .07rem;
                    color: rgba(#fff, .7);
                    transform: rotate(-90deg);
                }
            }
        }
        &_list{
            width: 100%;
            height: 100%;
            padding-top: 1rem;
            &_container{
                height: 92%;
                position: relative;
                overflow-y: auto;  
                &::-webkit-scrollbar {
                    width: .15rem;
                }
                &::-webkit-scrollbar-track {
                    border: solid 0 transparent;
                    margin: .2rem;
                }
                &::-webkit-scrollbar-thumb {
                    border: solid 0 transparent;      
                    background-color: rgba(#fff,.2);
                }
                &>div{                
                    border-bottom: .05rem solid rgba(#fff, .2);
                }
            }            
        } 
        &-item{            
            margin: 0 auto;
            display: flex;
            align-items: center;
            justify-content: space-between;
            width: 90%;
            position: relative;
            &_img{
                height: 4.1rem;
                width: 4.1rem;
                margin: 1rem 0;
                background-repeat: no-repeat;
                background-position: center;
                background-size: 200%;
            }
            &_text{
                height: 100%;
                width: 9rem;
            }
            &_exists{                
                width: 9rem;
                display: flex;
                align-items: center;
                img{
                    margin-right: .5rem;
                }
            }
            &_btn{                
                width: 9rem;
                display: flex;
                align-items: center;
                justify-content: center;
                border: .1rem solid #C1E704;
                letter-spacing: .04rem;
                height: 2.8rem;
                font-size: .8rem;
                &:hover{
                    background-color: rgba(#fff, .1);
                }
                img{
                    margin-right: .5rem;
                }
            }
            &_head1{
                color: rgba(#fff, .5);
                width: 4.1rem;
                font-size: .8rem;
                padding: .5rem 0;
                letter-spacing: .06rem;
            }
            &_head2{                
                color: rgba(#fff, .5);
                letter-spacing: .06rem;
                width: 9rem;
                font-size: .8rem;
            }
        }
    }
</style>