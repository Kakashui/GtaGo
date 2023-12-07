<template>
    <div class="referals-tab">
        <img src="/img/optionsMenu/referalsTab/bg_img.svg" alt="bgimg" class="referals-tab_bgimg">
        <div class="referals-tab-side">
            <div class="referals-tab-title">
                <div class="referals-tab-title_text">{{loc("mm_main_nav_t_6")}}</div>
                <div class="referals-tab-title_text">{{loc("mm_main_nav_d_6")}}</div>
                <img src="/img/optionsMenu/referalsTab/box.png" alt="man"  class="referals-tab-title_box">
                <div class="referals-tab-title-count">
                    <div class="referals-tab-title-count_title">{{loc("mmain:referal:count:t")}}</div>
                    <div class="referals-tab-title-count_row">
                        <img src="/img/optionsMenu/referalsTab/man_single.svg" alt="man">
                        <span>{{referals.total}}</span>
                    </div>
                </div>
            </div>
            <div class="referals-tab_subtitle">
                {{loc("mmain:referal:subtitle")}}
            </div>
            <div class="referals-tab-progress">
                <div class="referals-tab-progress_decor1"></div>
                <div class="referals-tab-progress_decor2"></div>
                <ProgressItem v-for="(item, key, index) in items" :key="index" :item="item" :count="key" :compleeted="key <= referals.total" />
            </div>
        </div>
        <div class="referals-tab-side">
            <div class="referals-tab-code">
                <div class="referals-tab-code_title">{{loc("mmain:referal:code:t")}}</div>
                <div class="referals-tab-code-row">
                    <div class="referals-tab-code_number" id="refcode">{{referals.code}}</div>
                    <!-- <div class="referals-tab-code_copy" @click="copy"><span>{{loc("mmain:referal:copy")}}</span> <img src="/img/optionsMenu/referalsTab/copy.svg" alt="copy"></div> -->
                </div>
                <div class="referals-tab-code_subtitle">{{loc("mmain:referal:code:sub")}}</div>
            </div>
            <div class="referals-tab_delimiter"></div>
            <div class="referals-tab-desc">
                <div class="referals-tab-desc_item" v-for="(item, index) in description" :key="index">
                    <img src="/img/optionsMenu/referalsTab/marker.svg" alt="marker">
                    {{loc(item)}}
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
import ProgressItem from './ProgressItem.vue'
import referalRewards from '../../../../configs/optionsMenu/referalRewards'

export default {
    data() {
        return {
            // items:{
            //     "10": {title: "100000 $", img: "money"},                
            //     "20": {title: "Уникальная одежда", img: "cloth"},                
            //     "30": {title: "Семья бесплатно", img: "grenade"},                
            //     "40": {title: "VIP PREMIUM на 2 месяца", img: "ticket"},                
            //     "50": {title: "Lada Priora", img: "car"},                
            //     "60": {title: "100000 $", img: "money"},                
            //     "70": {title: "Уникальная одежда", img: "cloth"},                
            //     "80": {title: "Семья бесплатно", img: "grenade"},                
            //     "90": {title: "VIP PREMIUM на 2 месяца", img: "ticket"},                
            //     "100": {title: "Lada Priora", img: "car"},  
            // },
            description:[
                // "mmain:referal:desc:1",
                "mmain:referal:desc:2",
                "mmain:referal:desc:3"
            ]
        }
    },
    methods: {
        copy(){
            navigator.clipboard.writeText(document.getElementById("refcode").innerHTML);
            this.$store.commit("notifyList/notify", { type: 2, position: 0, message: "Скопировано", time: 1000 });
        }
    },
    computed:{
        ...mapGetters("localization", ["loc"]),
        ...mapState("optionsMenu", ["referals"]),
        items() {
            return referalRewards;
        },
        decorHeight(){
            //console.log(document.getElementsByClassName("referals-tab-progress")[0].scrollHeight);
            return document.getElementsByClassName("referals-tab-progress")[0] ? 
                document.getElementsByClassName("referals-tab-progress")[0].scrollHeight : 
                0;
        }
    },
    components:{
        ProgressItem
    }
}
</script>

<style lang="scss">
    .referals-tab{
        width: 100%;
        display: flex;
        justify-content: space-between;
        position: relative;
        background: url(/img/optionsMenu/referalsTab/bg.png) no-repeat top -1rem center;       
        background-size: auto 57rem ;
        //border: 1px solid #fff;
        color: #fff;
        padding-top: 2rem;
        &-side{
            width: 26rem;
            //height: 48rem;
            //border: 1px solid #fff;
        }
        &-title{
            position: relative;
            &_box{
                position: absolute;
                top: -3rem;
                left: -3rem;
                width: 10rem;
            }
            &_text{
                font-size: 5rem;
                line-height: 5rem;
                letter-spacing: .06rem;
                &:first-child{
                    text-align: right;;
                }
            }
            &-count{
                position: absolute;
                right: 0;
                bottom: 0;
                width: 5rem;
                &_title{
                    font-size: .9rem;
                    text-align: right;
                }
                &_row{
                    display: flex;
                    align-items: center;
                    justify-content: space-between;
                    font-size: 3rem;
                    color: #E0A76A;
                    img{
                        margin-top: .5rem;
                    }
                }
            }
        }
        &_subtitle{
            margin-top: 1.5rem;
            color: rgba(#fff, .5);
            font-size: .9rem;
            letter-spacing: .05rem;
        }
        &-progress{
            width: 100%;
            //border: 1px solid #fff;
            height: 32rem;
            margin-top: 1rem;
            overflow-y: scroll;
            direction: rtl;
            position: relative;
            &_decor1{
                height: 200%;
                width: 4.5rem;
                position: absolute;
                top: 1.5rem;
                left: 4.6rem;
                background-color: rgba(#fff, .1);
                border-radius: 2.25rem;
            }
            &_decor2{
                height: 200%;
                width: .1rem;
                position: absolute;
                top: 2.3rem;
                left: 2.4rem;
                background-color: rgba(#fff, .1);
            }
            &::-webkit-scrollbar {
                width: .2rem;
            }
                &::-webkit-scrollbar-track {
                border: solid 0 transparent;
                margin: .1rem;
            }
                &::-webkit-scrollbar-thumb {
                border: solid 0 transparent;      
                background-color: rgba(#fff,.1);
            }
        }
        &-code{
            &_title{
                color: #E0A76A;
                font-size: 1.3rem;
                letter-spacing: .15rem;
            }
            &-row{
                display: flex;
                align-items: flex-end;
                justify-content: space-between;
                margin-top: .2rem;
            }
            &_number{
                font-size: 8rem;
                line-height: 6rem;
            }
            &_copy{
                font-size: 1.2rem;
                color: rgba(#fff, .5);
                span{
                    border-bottom: .15rem solid #E0A76A;
                }
                img{
                    margin: 0 1rem;
                }
                &:hover{
                    color: #fff;
                    text-shadow: 0 0 .2rem #fff;
                    cursor: pointer;
                }
            }
            &_subtitle{
                margin-top: 2rem;
                font-size: .9rem;
                color: rgba(#fff, .5);
                letter-spacing: .04rem;
            }
        }
        &_delimiter{
            width: 100%;
            height: .1rem;
            background-color: rgba(#fff, .5);
            margin: 3rem 0;
        }
        &-desc{
            &_title{
                color: #E0A76A;
                font-size: 2.5rem;
                letter-spacing: .08rem;
            }
            &_item{
                display: flex;
                align-items: center;
                font-size: 1.3rem;
                margin: 1rem 0;
                letter-spacing: .04rem;
                img{
                    margin-right: 1rem;
                }
            }
        }
        &_bgimg{
            position: fixed;
            right: 0;
            bottom: 0;
        }
    }
</style>