<template>
    <div class="wshop-acount">
        <div class="wshop-acount-tittle">{{loc('wshop_enter_count')}}</div>
        <Slider v-model="count" :min="1" :max="1000" />
        <div class="wshop-acount-input">
            <input type="number" v-model="count">
        </div>
    </div>
</template>

<script>
import Slider from 'vue-slider-component'
import { mapGetters } from 'vuex'

export default {
    data() {
        return {
            count: 1
        }
    },
    watch:{
        count(){
            this.$emit('onChage', this.count)
        }
    },
    computed:{
        ...mapGetters('localization', ['loc'])
    },
    components:{
        Slider
    }
}
</script>


<style lang="scss">
    .wshop-acount{
        border: 1px solid rgba(#000, .1);
        width: 100%;
        height: 9rem;
        font-weight: 300;
        background: url(/img/weaponShop/countbg.png) no-repeat center;
        background-size: cover;
        margin-top: 2rem;
        &-tittle{
            font-size: 1rem;
            line-height: 1rem;
            margin: 1rem 0 0 1rem;
        }
        &-input{
            margin-top: 2rem;
            margin-left: 1rem;
            width: 4.1rem;
            height: 2.4rem;
            input{
                width: 100%;
                height: 100%;
                background-color: rgba(#000, 1);
                border: 1px solid rgba(255, 255, 255, 0.15);
                color: #FFC700;
                text-align: center;
                &::-webkit-inner-spin-button{
                    display: none;
                }
            }
        }
    }
//------------------------------------------
        //          SLIDER
        //------------------------------------------
        .vue-slider-disabled {
            opacity: 0.5;
            cursor: not-allowed;
        }

        /* rail style */
        .vue-slider-rail {
            width: 10rem;
            margin: auto;
            background-color: rgba(#fff, 1);
            border-radius: 15px;
            margin: 1rem auto;
        }

        /* process style */
        .vue-slider-process {
        background: linear-gradient(270deg, #FFC700 0%, rgba(255, 199, 0, .3) 100%);
        border-radius: 15px;
        }

        /* mark style */
        .vue-slider-mark {
        z-index: 4;
        }
        .vue-slider-mark:first-child .vue-slider-mark-step, .vue-slider-mark:last-child .vue-slider-mark-step {
        display: none;
        }
        .vue-slider-mark-step {
        width: 100%;
        height: 100%;
        border-radius: 50%;
        background-color: rgba(0, 0, 0, 1);
        }
        .vue-slider-mark-label {
        font-size: 14px;
        white-space: nowrap;
        }
        /* dot style */
        .vue-slider-dot-handle {
        cursor: pointer;
        width: 1rem;
        height: 1rem;
        border-radius: 50%;
        margin-top: -.2rem;
        background: #C09600;
        border: 2px solid #FFC700;
        box-shadow: 0px 4px 12px #FFC700;
        box-sizing: border-box;
        }

        .vue-slider-dot-tooltip-inner {
            opacity: 0;
        }
        .vue-slider-dot-tooltip-inner-bottom::after {
        bottom: 100%;
        left: 50%;
        transform: translate(-50%, 0);
        height: 0;
        width: 0;
        border-color: transparent;
        border-style: solid;
        border-width: 5px;
        border-bottom-color: inherit;
        }
        .vue-slider-dot-tooltip-inner-left::after {
        left: 100%;
        top: 50%;
        transform: translate(0, -50%);
        height: 0;
        width: 0;
        border-color: transparent;
        border-style: solid;
        border-width: 5px;
        border-left-color: inherit;
        }
        .vue-slider-dot-tooltip-inner-right::after {
        right: 100%;
        top: 50%;
        transform: translate(0, -50%);
        height: 0;
        width: 0;
        border-color: transparent;
        border-style: solid;
        border-width: 5px;
        border-right-color: inherit;
        }

        .vue-slider-dot-tooltip-wrapper {
        opacity: 0;
        transition: all 0.3s;
        }
        .vue-slider-dot-tooltip-wrapper-show {
        opacity: 1;
        }

        //-----------------------------------------------------
        //      slider
        //-----------------------------------------------------
</style>