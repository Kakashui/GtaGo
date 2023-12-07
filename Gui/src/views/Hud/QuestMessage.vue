<template>
    <div class="questmsg" v-show="show && !promptData.show">
        <!-- <img src="/img/hud/questMessage/bg.png" alt="marker" class="questmsg_bg"> -->
        <div class="questmsg_title">
            {{loc(title)}}
        </div>
        <div class="questmsg_subtitle">{{loc(subtitle)}}</div>
    </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
export default {
    computed:{
        ...mapState("hudQuestMessage", ["show", "title", "subtitle", "background"]),
        ...mapState("hud", ["promptData"]),
        ...mapGetters("localization", ["loc"])
        
    }
}
</script>

<style lang="scss">
    .questmsg{
        position: absolute;
        right: -1rem;
        top: 10rem;
        width: 25rem;
        height: 15rem;
        text-transform: uppercase;
        background: url(/img/hud/questMessage/bg.png) no-repeat center;
        background-size: contain;
        //border: 1px solid #000;
        color: #fff;
        &_title{
            font-size: 1.1rem;
            height: 3.5rem;
            width: 16rem;
            margin-top: 4.4rem;
            margin-left: 6.5rem;
            padding: 0 2rem;
            display: flex;
            align-items: center;
            letter-spacing: .08rem;
            font-weight: bold;
            //border: 1px solid #000;
        }
        &_subtitle{
            width: 16rem;
            font-size: .9rem;
            margin-left: 6.7rem;
            letter-spacing: .06rem;
            margin-top: .5rem;
            //border: 1px solid #000;
        }       
        &_bg{
            position: absolute;
            right: 0;
        }
    }
</style>