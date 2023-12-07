<template>
    <div class="hud-help-btn" v-if="!helpShow">
        <span class="hud-help-btn__icon">M</span>
        <span class="hud-help-btn__text">menu</span>
    </div>
    <div class="hud-help-window" v-else>
        <div class="hud-help-item" v-for="(item, index) in items" :key="index">
            <div class="hud-help-key">{{item.key}}</div>
            <div class="hud-help-desc">{{loc(item.desc)}}</div>
        </div>
       
    </div>
</template>

<script>
import {mapGetters, mapState} from 'vuex'

export default {
    data() {
        return {
            symbol: '<',
            items:[
                {key: 'B', desc: 'cef_186'},                
                {key: 'G', desc: 'cef_187'},                
                {key: 'J', desc: 'cef_188'},                
                {key: 'TAB', desc: 'cef_189'},                
                {key: '↑', desc: 'cef_190'},                
                {key: 'U', desc: 'cef_191'},                
                {key: '2', desc: 'cef_192'},                
                {key: 'L', desc: 'cef_193'},                              
                {key: 'N', desc: 'cef_195'},                
                {key: 'F7', desc: 'cef_196'},                
                {key: 'F3', desc: 'cef_198'},                
                {key: 'F9', desc: 'cef_199'},                                              
                {key: '~', desc: 'cef_202'},
                {key: 'M', desc: 'Главное меню'},
                {key: 'F5', desc: 'Скрыть подсказки'}
            ]
        }
    },
    computed: {
        ...mapGetters('localization', ['loc']),
        ...mapState('hud', ['helpShow'])
    },
}
</script>


<style lang="scss" scoped>
    .hud-help-btn{
        position: absolute;
        left: 0.5rem;
        bottom: 1.5rem;
        color: #fff;
       text-transform: uppercase;
        letter-spacing: 0.1rem;
        &__icon {
            width: 1.5rem;
            height: 1.5rem;
            border-radius: 50%;
            background: rgba(255, 255, 255, 0.3);
            text-align: center;
            display: inline-block;
            line-height: 1.5rem;
        }
        &__text {
            position: absolute;
            bottom: 3.2rem;
            right: -1.3rem;
            width: 4rem;
            text-align: center;
            transform: rotate(270deg);
            display: block;
            color:  rgba(255, 255, 255, 0.5);
        }
    }
    .hud-help{
        &-window{
            position: absolute;
            width: 18rem;
            left: 1rem;
            bottom: 15rem;
            color: #fff;
            font-size: .9rem;
        }
        &-item{
            display: flex;
            justify-content: space-between;
            margin: .2rem 0;
        }
        &-desc{
            text-align: left;
            width: 15rem;
        }
        
    }
</style>