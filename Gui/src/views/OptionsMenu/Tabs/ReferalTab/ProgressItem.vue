<template>
    <div class="referals-tab-item">
        <div class="referals-tab-item-info">
            <div class="referals-tab-item-info_status" v-if="compleeted">{{loc("mmain:referal:code:item:succ")}}</div>
            <div class="referals-tab-item-info_text">{{loc(item.Desc)}}</div>
            <div v-if="item.Value" class="referals-tab-item-info_text">{{item.Value}}</div>
        </div>
        <div class="referals-tab-item_light" :style="{background}"></div>
        <div class="referals-tab-item_img" :class="{'referals-tab-item_filter': compleeted}">
            <div class="referals-tab-item_img_decor"></div>
            <img :src="`/img/optionsMenu/programTab/${item.Image}.png`" alt="item">
        </div>
        <div class="referals-tab-item_count">
            <img src="/img/optionsMenu/referalsTab/man_many.svg" alt="men">
            {{count}}
        </div>
    </div>
</template>

<script>
import { mapGetters } from 'vuex'
export default {
    props:["count", "item", "compleeted"],
    computed:{
        background(){
            return this.compleeted ? "#29FFF2":"#fff1";
        },
        ...mapGetters("localization", ["loc"])
    }
}
</script>

<style lang="scss">
    .referals-tab-item{
        width: 24rem;
        height: 4.5rem;
        //border: 1px solid #fff;
        margin: 1.5rem 0;
        display: flex;
        align-items: center;
        position: relative;
        justify-content: flex-end;       
        &_count{
            width: 1.3rem;
            text-align: center;
            font-size: 1.6rem;
            font-weight: bold;
            letter-spacing: .07rem;
            img{
                width: 1.3rem;
            }
        }
        &_img{
            width: 4.5rem;
            height: 4.5rem;
            border: .15rem solid #fff;
            border-radius: 50%;
            margin-left: 1.5rem;
            display: flex;
            align-items: center;
            justify-content: center;
            background-color: rgba(#fff, .2); 
            position: relative;
            img{
                position: absolute;
                width: 80%;
                height: 80%;
            }
            &_decor{
                width: 90%;
                height: 90%;
                border: .25rem solid rgba(#000, .5);
                border-radius: 50%;
            }
        }
        &_light{
            width: .2rem;
            height: 60%; 
            margin-left: 1.5rem;           
        }
        &-info{
            margin-left: 1.5rem;
            width: 13rem;
            &_status{
                color: rgba(#fff, .5);
                font-size: .9rem;
                text-align: left;
            }
            &_text{
                text-align: left;
                font-size: 1.1rem;
            }
        }
        &_filter{
            filter: drop-shadow(0 1rem 1.6em #29FFF2);
        }
    }
</style>