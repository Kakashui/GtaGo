<template>
    <div class="hmodshop-item">
        <hr>
        <div class="hmodshop-item_title">
            <div class="hmodshop-item_min" @click="select">
                <img src="/img/handlingModShop/min.svg" alt="min" :class="{'hmodshop-item_show': show}">
            </div>
            <div class="hmodshop-item_titext">
                {{loc(`shmodsop:${item.key}:title`)}}
            </div> 
        </div>
        <div class="hmodshop-item-range" v-if="show && !isBool">
            <Slider 
                :min="item.value.min"
                :max="item.value.max"
                :interval="item.value.step"
                v-model="newValue"
                @change="changeValue"
            />
            <div class="hmodshop-item-range_info">
                <div class="hmodshop-item-range_info_i hmodshop-tc-grey">
                    <div>{{loc("hmodshop:range:def")}}</div>  
                    <div class="hmodshop-tc-white hmodshop-item-range_val">{{item.defaultValue}}</div>
                </div>
                <div class="hmodshop-tc-white hmodshop-item-range_point">·</div>
                <div class="hmodshop-item-range_info_i  hmodshop-tc-white">
                    <div>{{loc("hmodshop:range:curr")}}</div>  
                    <div class="hmodshop-tc-green hmodshop-item-range_val">{{newValue}}</div>
                </div>
            </div>
            <div class="hmodshop-item_desc hmodshop-tc-grey">
                {{loc(`shmodsop:${item.key}:desc`)}}
            </div>           
        </div>
        <div class="hmodshop-item-switch" v-else-if="show">            
            <div class="hmodshop-item_desc hmodshop-tc-grey">
                {{loc(`shmodsop:${item.key}:desc`)}}
            </div> 
            <div class="hmodshop-item_test" :class="{'hmodshop-item_teston': newValue}" @click="changeValue2">
                {{loc(newValue ? "shmodsop:btn:test:on" : "shmodsop:btn:test")}}
            </div>
        </div>
        <div class="hmodshop-item_btns" v-if="show">
                <div class="hmodshop-item_btn" @click="buy">
                    <div>{{loc("shmodsop:btn:buy")}}</div> 
                    <div>{{getBuyPrice()}}$</div>
                </div>
                <div class="hmodshop-item_btn" @click="sell">
                    <div>{{loc("shmodsop:btn:sell")}}</div> 
                    <div>{{getSellPrice()}}$</div>
                </div>
        </div>
    </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
import Slider from 'vue-slider-component'
import handlingParts from '../../configs/shops/handlingParts'

export default {
    props:["item", "show"],
    data() {
        return {
            newValue: 0,
            spam: 0
        }
    },
    computed:{
        ...mapGetters("localization", ["loc"]),
        ...mapState("handlingModShop", ["pricePart", "priceCar", "percentCarPrice"]),
        isBool(){
            return typeof this.item.value.current === 'boolean';
        }
    },
    methods: {
        select(){
            this.$emit("onselect")
        },
        changeValue(val){
            this.$emit("onchange", this.item.key, val)
        },
        changeValue2(){
            this.newValue = !this.newValue;
            this.$emit("onchange", this.item.key,  this.newValue)
        },
        buy(){
            if(this.spam > Date.now()) return;
            this.spam = Date.now() + 500;
            if(this.newValue === this.item.value.current) return;
            this.$emit("onbuy", this.item.key, this.newValue)
        },
        sell(){
            if(this.spam > Date.now()) return;
            this.spam = Date.now() + 500;
            if(this.item.defaultValue === this.item.value.current) return;
            this.$emit("onsell", this.item.key)
        },
        getBuyPrice() {
            return this.pricePart * Math.round(handlingParts[this.item.key].Modifire * (this.priceCar / this.percentCarPrice * handlingParts[this.item.key].KoefCar + 1));
        },
        getSellPrice() {
            return this.pricePart * handlingParts[this.item.key].Stock;
        }
    },
    components:{
        Slider
    },
    mounted() {
        this.newValue = this.item.value.current
    },
}
</script>

<style lang="scss">
.hmodshop-item{
    width: 14.5rem;
    position: relative;
    direction: ltr;
    transition: all .5s;
    &_btns{
        margin-top: .5rem;
        display: flex;
        align-items: center;
        justify-content: space-between;
    }
    &_btn{
        width: 49%;
        height: 3rem;
        display: flex;
        flex-direction: column;
        justify-content: center;
        align-items: center;
        background-color: rgba(#fff, .2);
        color: #fff;
        border-radius: .2rem;
        &:hover{
            background: #B6D300;
            box-shadow: 0px .2rem .5rem rgba(168, 195, 2, 0.6);
        }
    }
    hr{
        margin: 1rem 0;
        border-color: rgba(#fff, .2);
    }    
    //border: 1px solid #000;
    &_title{
        color: #B6D300;
        letter-spacing: .06rem;
        display: flex;
        justify-content: space-between;
        align-items: center;
        min-height: 2rem;
    }
    &_titext{
        width: 12.5rem;
        //border: 1px solid #000;
    }
    &_min{
        width: 1.3rem;
        height: 1.3rem;
        background-color: #fff;
        border-radius: 50%;
        display: flex;
        align-items: center;
        justify-content: center;
        img{
            width: 70%;
        }
        &:hover{
            box-shadow: 0 0 .5rem 0 #fff;
        }
    }
    &-range{
        &_info{
            display: flex;
            align-items: center;
            font-weight: .9rem;
            margin-top: .2rem;
            &_i{
                 display: flex;
                align-items: center;
            }
        }
        &_point{
            margin: 0 .5rem;
        }
        &_val{
            margin-left: .3rem;
        }
    }
    .vue-slider{
      &-disabled {
        opacity: 0.5;
        cursor: not-allowed;
      }
      &-rail {
          width: 98%;
          background-color: rgba(#fff, 1);
          border-radius: 15px;
          margin: 0 auto;
      }
      &-process {
        background: linear-gradient(270deg, #B6D300 0%, rgba(#B6D300, .3) 100%);
        border-radius: 15px;
      }
      &-mark{
        z-index: 4;
        &:first-child .vue-slider-mark-step, .vue-slider-mark:last-child .vue-slider-mark-step {
          display: none;
        }
        &-step {
          border-radius: 50%;
          background-color: rgba(0, 0, 0, 1);
        }
        &-label {
          font-size: 14px;
          white-space: nowrap;
        }
      }
      &-dot{
        &-handle {
          cursor: pointer;
          width: .7rem;
          height: .7rem;
          border-radius: 50%;
          margin-top: 0;
          background: #B6D300;
          border: .1rem solid #B6D300;
          box-sizing: border-box;
          box-shadow: none;
          color: #fff;
        }
        &-tooltip{
          &-inner {
            opacity: 0;
            // background-color: #9BAE22;
            // border-radius: .4rem;
            // width: 1.7rem;
            // height: 1.7rem;
            // border: 1px solid #FFF;
            // padding: .5rem;
            // display: flex;
            // align-items: center;
            // justify-content: center;
            // margin-left: .5rem;
            // margin-bottom: .5rem;
            // color: #fff;
            // &-bottom::after {
            //   background-color: #9BAE22;
            //   border: 1px solid #FFF;
            // }
            // &-left::after {
            //   background-color: #9BAE22;
            //   border: 1px solid #FFF;
            // }
            // &-right::after {
            //   background-color: #9BAE22;
            //   border: 1px solid #FFF;
            // }
          }
          &-wrapper {
            opacity: 0;
            transition: all 0.3s;
          }
          &-wrapper-show {
            opacity: 1;
          }
        }
      }
    }
    &_show{
        transform: rotate(180deg);
    }
    &_desc{
        margin-top: 1rem;
        font-size: .9rem;
        letter-spacing: .03rem;
        width: 100%;
    }
    &_test{
        width: 100%;
        background-color: rgba(#fff, .2);
        height: 3rem;
        display: flex;
        align-items: center;
        justify-content: center;
        border-radius: .2rem;
        margin-top: .5rem;
        font-size: .9rem;
        color: #fff;
        letter-spacing: .07rem;
        &:hover{
            background: #B6D300;
            box-shadow: 0px .2rem .5rem rgba(168, 195, 2, 0.6);
        }
        &on{
            background-color: #fff;
            color: #000;
            font-weight: bold;
            letter-spacing: .07rem;
        }
    }
}
</style>