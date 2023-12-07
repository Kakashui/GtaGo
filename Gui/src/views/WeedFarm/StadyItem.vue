<template>
    <div class="weedfarm-stady">
        <img src="/img/weedFarm/decor1.png" alt="decor" v-if="index === 1" class="weedfarm-stady_img" style="bottom: -25%">
        <img src="/img/weedFarm/decor2.png" alt="decor" v-if="index === 2" class="weedfarm-stady_img" style="bottom: -15%; right: -3%">
        <div class="weedfarm-stady-head">
            <div class="weedfarm-stady-head_sub">{{loc("weedfarm:stady:stady")}} {{index + 1}}</div>
            <div class="weedfarm-stady-head_tit">{{loc(`weedfarm:stady:name:${index}`)}}</div>
            <div class="weedfarm-stady-head_lable"> {{index + 1}}</div>
        </div>
        <div class="weedfarm-stady-body">
            {{value}} / {{maxOnStady[index]}}
        </div>
    </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
export default {
    props:['index', 'value'],
    computed: {
        ...mapGetters('localization', ['loc']),
        ...mapState('weedFarm', ['maxOnStady'])
    },
}
</script>

<style lang="scss">
    .weedfarm-stady{
        width: 50%;
        height: 50%;
        background: linear-gradient(179.01deg, rgba(255, 255, 255, 0.06) 0.85%, rgba(255, 255, 255, 0) 99.15%);
        position: relative;
        border-right: .1rem solid rgba(#fff, .2);
        letter-spacing: .05rem;        
        &_img{
            position: absolute;
            right: 0;
        }
        &-head{
            height: 4.8rem;
            width: 100%;
            border-bottom: .1rem dashed rgba(#fff, .2);
            padding-left: 3rem;
            position: relative;
            display: flex;
            flex-direction: column;
            justify-content: center;
            &_sub{
                color: rgba(#fff, .4);
                font-size: .8rem;
            }
            &_tit{
                font-size: 1.5rem;
                margin-top: .3rem;
            }
            &_lable{
                position: absolute;
                width:1.4rem;
                height: 1.4rem;
                display: flex;
                align-items: center;
                justify-content: center;
                bottom: -.7rem;
                background-color: #A8CA08;
                box-shadow: 0px 4px 30px 0px #C1E704B2;
                border-radius: 50%;
            }           
        } 
        &-body{
            display: flex;
            align-items: center;
            font-size: 4rem;
            height: 50%;
            padding: 3rem;
        }

    }
</style>