<template>
    <div class="split" v-if="from && to && item">
        <div class="split-dialog">
            <div class="split-tittle">
                <div class="split-tittle-type">{{loc("inv_action_split")}}</div>
                <div class="split-tittle-name">{{loc(item.getDisplayName())}}</div>
            </div>
            <div class="split-body">
                <div class="split-info">
                    <div class="split-info-tittle">{{loc("inv_count_short")}}</div>
                    <input type="number" class="split-info-value" :min="1" :max="item.count - 1" v-model="count">
                </div>
                <Slider v-model="count" :min="1" :max="item.count - 1"/>
                <div class="split-buttons">
                    <div class="split-button" @click="move">{{loc("inv_split_confirm")}}</div>
                    <div class="split-button" @click="close">{{loc("inv_split_cancel")}}</div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import Slider from 'vue-slider-component'
import { mapGetters } from 'vuex'

export default {
    props:['from', 'to', 'item'],
    data() {
        return {
            count: 1,
        }
    },
    computed: {
        ...mapGetters('localization', ['loc'])
    },
    methods: {       
        close(){
            this.$emit("onSplitClose");
        },
        move(){
            const count = parseInt(this.count);
            if(isNaN(count)) return;
            if(count > this.item.count || count < 1) return;
            this.$emit("onSpliteMove", count);
        }
    },
    components:{
        Slider
    }
}
</script>


<style lang="scss">


    .split{
        //------------------------------------------
        //          SLIDER
        //------------------------------------------
        .vue-slider-disabled {
            opacity: 0.5;
            cursor: not-allowed;
        }

        /* rail style */
        .vue-slider-rail {
            width: 16rem;
            margin: auto;
            background-color: rgba(#fff, .1);
            border-radius: 15px;
            margin: 1rem auto;
        }

        /* process style */
        .vue-slider-process {
        background: linear-gradient(96.5deg, #3A244C 0%, #8E52AA 100%);
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
        width: 1.2rem;
        height: 1.2rem;
        border-radius: 50%;
        margin-top: -.2rem;
        background: #7A3E96;
        box-shadow: 0 .5rem 1rem #8E52AA;
        border: .2rem solid #8E52AA;
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

        position: fixed;
        left: 0;
        top: 0;
        width: 100vw;
        height: 100vh;
        z-index: 1100;
        background-color: rgba(0,0,0,.5);
        display: flex;
        justify-content: center;
        align-items: center;
        &-dialog{
            width: 19rem;
            border: 2px solid rgba(#fff, .2);
            box-shadow: 0px 1rem 3rem rgba(0, 13, 18, 0.5);
        }
        &-tittle{
            height: 4rem; 
            background: url(/img/inventory/common/otherHead.png) no-repeat center;
            background-size: cover;
            padding-top: 1rem;
            font-size: 1.1rem;
            line-height: 1.1rem;
            color: #fff;
            &-type{
                margin-left: 1.2rem;
                font-size: .8rem;
                line-height: .85rem;
                color: rgba(#fff, .5);
            } 
            &-name{
                margin-left: 1.2rem;
                margin-top: .2rem;
                font-size: 1.1rem;
                line-height: 1.1rem;
                font-weight: 300;
            }
        }
        &-info{
            display: flex;
            align-items: center;
            justify-content: space-between;
            color: #fff;
            margin: auto;
            width: 16rem;
            font-size: .9rem;
            line-height: 1.1rem;
            &-value{
                width: 3rem;
                text-align: center;
                background: none;
                border: 1px solid rgba(#fff, .1);
                color: #fff;
                padding: .2rem;
                &::-webkit-inner-spin-button{
                    display: none;
                }
            }
        }
        &-body{
            padding: 1.2rem 0 .1rem;
            background-color: #142126;
            text-align: center;           
        }
        &-buttons{
            margin-top: 2.5rem;
            display: flex;
            align-items: center;
            justify-content: space-around;
        }
        &-button{
            height: 3.5rem;
            width: 49%;
            color: rgba(#fff, .5);
            background-color: #000D12;
            display: flex;
            justify-content: center;
            align-items: center;
            &:hover{
                background-color: #8E52AA;
                color: #fff;
            }
        }
    }
</style>