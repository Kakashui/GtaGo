<template>
    <div class="weedfarm-control">
        <div class="weedfarm-control-left">
            <div class="weedfarm-control_head">
                {{ownerName}}
            </div>
            <div class="weedfarm-control_title">
                {{loc("weedfarm:cboard:tit:1")}}
            </div>
            <div class="weedfarm-control-left_list">                
                <SystemItem  v-for="(value, _, index) in systems" :key="index" :index="index" :value="value"/>
            </div>
        </div>
        <div class="weedfarm-control-right">
            <div class="weedfarm-control_head">
                {{loc('weedfarm:control:profit')}} <div class="weedfarm-control_profit">$ {{profit.toLocaleString()}}</div>
            </div>
            <div class="weedfarm-control_title">
                {{loc("weedfarm:cboard:tit:2")}}
            </div>
             <div class="weedfarm-control-right_list">                
                <StadyItem  v-for="(value, _, index) in stady" :key="index" :index="index" :value="value"/>
                <div class="weedfarm-control-right_btn" @click="toWork">{{loc(onDeliveryJob ? 'weedfarm:cboard:btn:2':'weedfarm:cboard:btn')}}</div>
            </div>
        </div>        
    </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
import SystemItem from './SystemItem'
import StadyItem from './StadyItem'

export default {
    computed:{
        ...mapState('weedFarm', ['profit', 'ownerName', 'systems', 'stady','onDeliveryJob']),
        ...mapGetters('localization', ['loc'])
    },
    data() {
        return {
            spam: 0,
        }
    },
    components:{
        SystemItem,
        StadyItem
    },
    methods: {
        toWork(){
            if(Date.now() < this.spam) return;
            this.spam = Date.now() + 1000;
            if(this.onDeliveryJob)
                window.mp.triggerServer("weedfarm:delivery:end")
            else
                window.mp.triggerServer("weedfarm:delivery:begine")
        }
    },
}
</script>

<style lang="scss">
    .weedfarm-control{
        width: 100%;
        height: 100%;
        display: flex;
        align-items: center;
        justify-content: space-between;
        background-color: #111;
        text-transform: uppercase;
        &_head{
            height: 5rem;
            display: flex;
            align-items: center;
            border-bottom: .1rem solid rgba(#fff, .2);
            padding-left: 2rem;
            color: rgba(#fff, .5);
            letter-spacing: .05rem;
        }
        &_profit{
            margin-left: 1rem;
            color: #fff;
        }
        &-left{
            height: 100%;
            min-width: 19.5rem;
            border-right: .1rem solid rgba(#fff, .2);
            background: linear-gradient(270deg, rgba(255, 255, 255, 0.08) 0%, rgba(255, 255, 255, 0) 102.76%);
            position: relative;
        }
        &-right{
            height: 100%;
            width: 100%;
            &_list{
                width: 100%;
                height: 30.5rem;
                display: flex;
                align-items: center;
                justify-content: center;
                flex-wrap: wrap;
            }
            &_btn{
                position: absolute;
                right: 2rem;
                bottom: 1rem;
                border: .1rem solid #A8CA08;
                color: #A8CA08;
                font-size: 1.2rem;
                letter-spacing: .05rem;
                //font-weight: bold;
                width: 15rem;
                height: 2.5rem;
                display: flex;
                align-items: center;
                justify-content: center;
                &:hover{
                    transform: scale(1.02);
                }
            }
        }
        &_title{
            width: 100%;
            font-size: 2.5rem;
            height:5rem;
            text-shadow: 0 0 1.5rem rgba(#fff, .5);
            display: flex;
            align-items: center;
            padding-left: 2rem;
        }
    }
</style>