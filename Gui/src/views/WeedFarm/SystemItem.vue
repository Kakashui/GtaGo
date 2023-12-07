<template>
    <div class="weedfarm-sys">
        <div class="weedfarm-sys_tit">{{loc(`weedfarm:sys:${index}`)}}</div>
        <div class="weedfarm-sys_sub">
            <div>{{loc("weedfarm:sys:sub")}}</div>
            <div>{{value}}/100</div>
        </div>
        <div class="weedfarm-sys_progress">
            <div class="weedfarm-sys_thumb" :style="{'width': `${value}%`, 'background-color': value > 50 ?'#A8CA08' : '#FF4105'}"></div>
        </div>
        <div class="weedfarm-sys_btn" :style="buttonStyle" v-if="value < 100" @click="fixComponent" >
            {{loc('weedfarm:sys:btn')}} ${{((100-value) * 10).toLocaleString()}}
        </div>
        <div class="weedfarm-sys_btn weedfarm-sys_ok" v-else>
            {{loc('weedfarm:sys:ok')}}
        </div>
    </div>
</template>

<script>
import { mapGetters } from 'vuex'
export default {
    props: ['index', 'value'],
    data() {
        return {
            spam: 0
        }
    },
    computed:{
        ...mapGetters('localization', ['loc']),
        buttonStyle(){
            const color = this.value < 50 ? "#FF4105" : '#A8CA08';            
            return {color, 'border': `.1rem solid ${color}`}
        }
    },
    methods: {
        fixComponent(){
            if(this.spam > Date.now()) return;
            this.spam = Date.now() + 1000;
            window.mp.triggerServer("weedfarm:systems:fix", this.index)
        }
    },
}
</script>

<style lang="scss">
    .weedfarm-sys{
        width: 15rem;
        height: 6.25rem;
        margin: .5rem auto;
        position: relative;
        letter-spacing: .05rem;
        &_sub{
            font-size: .8rem;
            display: flex;
            align-items: center;
            justify-content: space-between;
            text-transform: none;
            margin-top: .6rem;
        }
        &_progress{
            height: .3rem;
            width: 100%;
            position: relative;
            margin-top: .2rem;
            border: .1rem solid rgba(#fff, .2);
        }
        &_thumb{
            position: absolute;
            left: 0;
            top: -.1rem;
            height: .3rem;
            transition: all .4s;
        }
        &_btn{
            width: 7.1rem;
            height: 2.1rem;
            display: flex;
            align-items: center;
            justify-content: center;
            margin-top: .6rem;
            font-size: .8rem;
            &:hover{
                transform: scale(1.02);
            }
        }
        &_ok{
           color: #A8CA08;
        }

    }
</style>