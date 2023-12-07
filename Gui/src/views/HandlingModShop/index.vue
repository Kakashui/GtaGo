<template>
    <div class="hmodshop">
        <div class="hmodshop_body" v-show="visible">            
            <div class="hmodshop-close" @click="exit">
                <img src="/img/handlingModShop/close.svg" alt="close" class="hmodshop-close_img">
                <div class="hmodshop-close_text">{{loc("hmodshop:exit")}}</div>
            </div>
            <div class="hmodshop_title">{{loc("hmodshop:title")}}</div>
            <div class="hmodshop-items">
                <div class="hmodshop-items_list">
                    <Item 
                        v-for="(item, index) in itemList" :key="index" 
                        :item="item" 
                        :show="index === currentItem" 
                        :index="index"
                        @onselect="selectItem(index)"
                        @onchange="changeValue"
                        @onbuy="buy"
                        @onsell="sell"
                    /> 
                </div>
            </div>
            <div class="hmodshop_body_f">
                <h1 class="hmodshop-tc-green">f2</h1>
                <div class="hmodshop-tc-white">{{loc("hmodshop:body:hint:f2")}}</div>
            </div>
        </div>
        <div class="hmodshop_f" v-show="!visible">
            <div class="hmodshop-tc-green" style="font-size: 3.5rem;">F2</div>
            <div class="hmodshop-tc-white">{{loc("hmodshop:hiht:f2")}}</div>
        </div>
    </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
import Item from './Item.vue'
export default {
    data() {
        return {
            currentItem: 0
        }
    },
    computed:{
        ...mapGetters("localization", ["loc"]),
        ...mapState("handlingModShop", ["items", "visible", "isAdmin"]),
        itemList() {
            if (this.isAdmin)
                return this.items;
            return this.items.filter(item => !item.onlyAdmin);
        }
    },
    methods: {
        selectItem(index){
            this.currentItem = index;
        },
        changeValue(key, value){
            window.mp.trigger("hmodshop:value:change", key, value)
        },
        exit(){
             window.mp.trigger("hmodshop:exit")
        },
        buy(key, value){
            window.mp.triggerServer("handlingShop:buy", key, value);
        },
        sell(key){
            window.mp.triggerServer("handlingShop:sell", key);
        }
    },
    components:{
        Item
    }
}
</script>

<style lang="scss">
    .hmodshop{
        position: absolute;
        left: 0;
        top: 0;
        height: 100vh;
        overflow: hidden;
        width: 50vw;
        background: linear-gradient(90deg, rgba(5, 21, 20, 0.9) 0%, rgba(0, 0, 0, 0) 100%, rgba(5, 21, 20, 0) 100%);
        text-transform: uppercase;
        &_title{
            color: #fff;
            font-size: 3.5rem;
        }
        &_body{
            position: absolute;
            top: 0;
            height: 37rem;
            margin: 2.5%;
            &_f{
                width: 19.5rem;
                height: 5rem;
                display: flex;
                align-items: center;
                background: rgba(0, 0, 0, 0.8);
                margin-top: 1rem;
                color: #fff;
                padding: 1rem 2rem;
                font-size: .9rem;
                h1{
                    font-size: 3.25rem;
                    margin-right: 1rem;
                }
            }
        }
        &-close{
            display: flex;
            align-items: center;
            &_img{
                //background-color: rgba(#fff, .4);
                width: 1.7rem;
                &:hover{
                    transform: scale(1.05);
                }
            }
            &_text{
                color: #B6D300;
                font-size: 1.2rem;
                letter-spacing: .08rem;
                margin-left: 1rem;
            }
        }
        &-items{
            background: linear-gradient(180deg, rgba(41, 255, 242, 0.3) 0%, rgba(41, 255, 242, 0) 100%), rgba(0, 0, 0, 0.9);
            height: 37rem;
            min-width: 19.5rem;
            margin-top: 1rem;
            padding-top: 2rem;
            position: relative;
            &_list{
                direction: rtl;
                overflow-y: scroll;
                width: 15.7rem;
                height: 32rem;
                margin: 0 auto;
                //border: 1px solid #000;
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
        }
        &_f{
            width: 2.5rem;
            position: absolute;
            left: 2rem;
            top: 40%;
        }
        .hmodshop-tc-white{
            color: #fff;
        }
        .hmodshop-tc-grey{
            color: rgba(#fff, .5);
        }   
        .hmodshop-tc-green{
            color: #B6D300;
        }
    }
</style>