<template>
    <div class="biginfo" v-show="biginfo !== null">
        <div class="biginfo_title">{{loc("Подсказка")}}</div>
        <hr>
        <div class="biginfo_text">{{loc(biginfo)}}</div>
        <div class="biginfo_btn" @click="close">{{loc("Закрыть")}}</div>
    </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
export default {
    computed:{
        ...mapState("hud", ["biginfo"]),
        ...mapGetters("localization", ["loc"])
    },
    methods: {
        close(){
            window.mp.trigger("biginfo:hide");
        }
    },
}
</script>

<style lang="scss">
    .biginfo{
        background: linear-gradient(180deg, rgba(41, 255, 242, 0.3) 0%, rgba(41, 255, 242, 0) 100%),linear-gradient(0deg, rgba(0, 0, 0, 0.9), rgba(0, 0, 0, 0.9));
        width: 40rem;
        height: 24rem;
        position: absolute;
        //border: 1px solid #000;
        top: 15rem;
        left: 30rem;
        text-transform: uppercase;
        padding: 3rem;
        color: #fff;
        hr{
            border-color: rgba(#fff, .5);
            margin: 1.5rem 0;
        }
        &_text{
            letter-spacing: .07rem;
        }
        &_title{
            font-size: 3rem;
            font-weight: bold;
            background: linear-gradient(180deg, #FFFFFF 0%, rgba(255, 255, 255, 0) 100%);
            text-align: center;
            //margin-top: 3rem;
            background-clip: text;
            -webkit-text-fill-color: transparent;
            letter-spacing: .1rem;
        }
        &_btn{
            display: flex;
            align-items: center;
            justify-content: center;
            background-color: #B6D300;
            font-weight: bold;
            letter-spacing: .1rem;
            font-size: 1.5rem;
            width: 8rem;
            height: 3.5rem;
            position: absolute;
            bottom: -1.75rem;
            left: 16rem;
            box-shadow: 0 0 1.5rem 0 #B6D300;
            &:hover{
                transform: scale(1.02);
            }
        }
    }
</style>