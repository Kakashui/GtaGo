<template>
    <div class="weedfarm-greeting">
        <div class="weedfarm-greeting_tit">Marijuana Farm</div>
        <div class="weedfarm-greeting_sub">
            {{loc("weedfarm:greeten:sub")}}
        </div>
        <div class="weedfarm-greeting_btn">{{loc("weedfarm:greeten:btn")}}</div>
    </div>
</template>


<script>
import { mapGetters } from 'vuex'
export default {
   computed:{
       ...mapGetters('localization', ['loc'])
   }
}
</script>

<style lang="scss">
    .weedfarm-greeting{
        width: 100%;
        height: 100%;
        //border: 1px solid #000;
        background: url(/img/weedFarm/bg1.jpg) no-repeat center;
        background-size: cover;
        display: flex;
        align-items: center;
        justify-content: center;
        flex-direction: column;
        div{
            max-width: 26rem;
            text-align: center;
        }
        &_tit{
            text-transform: uppercase;
            font-size: 5rem;
            margin-top: -2rem;
        }
        &_sub{
            font-size: 1.1rem;
        }
        &_btn{
            width: 10rem;
            height: 3.15rem;
            background-color: #ED128F;
            text-transform: uppercase;
            display: flex;
            align-items: center;
            margin-top: 3rem;
            justify-content: center;
            &:hover{
                transform: scale(1.02);
            }
        }
    }
</style>