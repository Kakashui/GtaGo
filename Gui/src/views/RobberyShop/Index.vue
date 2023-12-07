<template>
    <div class="robshop" v-show="show">
        <div class="robshop_body">
            <div class="robshop_title">{{loc("robshop:title")}}: 
                <svg width="20" height="19" viewBox="0 0 20 19" fill="none" xmlns="http://www.w3.org/2000/svg" v-for="(item, index) in 5" :key="index" class="robshop_star">
                    <path d="M10 0L12.9389 5.95492L19.5106 6.90983L14.7553 11.5451L15.8779 18.0902L10 15L4.12215 18.0902L5.24472 11.5451L0.489435 6.90983L7.06107 5.95492L10 0Z" :fill="level > index ? '#B6D300' : 'white'" fill-opacity="0.3"/>
                </svg>
            </div>
            <Progress :progress="progress"/>
            <Info :money="money" />
        </div>
    </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'

import Progress from './Progress.vue'
import Info from './Info.vue'

export default {
    computed:{
        ...mapState("robberyShop", ["show","progress", "level", "money"]),
        ...mapGetters("localization", ["loc"])
    },
    components:{
        Progress,
        Info
    }
}
</script>


<style lang="scss">
.robshop{
    position: absolute;
    width: 28.5rem;
    height: 11.2rem;
    background: radial-gradient(50% 50% at 50% 50%, rgba(0, 0, 0, 0.6) 0%, rgba(0, 0, 0, 0) 100%);
    right: 0;
    bottom: 5rem;
    display: flex;
    align-items: center;
    justify-content: center;
    color: #fff;
    text-transform: uppercase;
    &_body{
        width: 20rem;
        position: relative;        
    }
    &_title{
        font-size: 1rem;
        line-height: 1.2rem;
        display: flex;
        align-items: center;
    }
    &_star{
        margin-right: .2rem;
        margin-top: -.1rem;
        width: 1rem;
        height: 1rem;
        &:first-child{
            margin-left: .5rem;
        }
    }
}
</style>