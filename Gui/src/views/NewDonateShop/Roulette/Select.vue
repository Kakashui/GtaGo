<template>
    <div class="newdshop-roull-select">
        <div class="newdshop-roull-select_card" v-for="(item, index) in config" :key="index" >
            <div class="newdshop-roull-select_title">{{item.name}}</div>
            <Item :item="item"  class="newdshop-roull-select_item" @onClick="itemSelect(item)" :hover="true"/>            
            <div class="newdshop-roull-select_info" @click="showInfo(item)" @mouseenter="playHover">{{loc("newdshop:roul:select:info")}}</div>
        </div>        
    </div>
</template>

<script>
import Item from './Item'
import {getRouletteConfig} from '../../../configs/newDonateShop'
import {mapGetters} from 'vuex'

export default {
  
    computed:{
        ...mapGetters('localization', ['loc']),
        config(){
            return getRouletteConfig();
        }
    },
    methods: {
        itemSelect(item){
            this.$emit("onSelect", item);
            this.playClick();
        },
        showInfo(item){
            this.$emit("onShowInfo", item)
            this.info = item;
        },
        playHover(){
            this.$store.commit("sounds/play", {"name": "dshop_hover", "volume": .7})
        },
        playClick(){
            this.$store.commit("sounds/play", {"name": "dshop_click", "volume": .7})
        }
    },
    components:{
        Item
    }
}
</script>

<style lang="scss">
.newdshop-roull-select{
    display: flex;
    align-items: center;
    justify-content: center;
    margin-top: -5rem;
    &_item{
        width: 14rem;
        height: 14rem;
        margin: 0 2rem;
    }
    &_title{
        font-size: 3rem;
        text-shadow: 0px 4px 40px rgba(255, 255, 255, 0.6);
        transform: rotate(-6deg);
        margin: 1.5rem auto;
        text-align: center;
        margin-left: -1.5rem;
    }
    &_info{
        width: 10rem;
        height: 2.9rem;
        display: flex;
        align-items: center;
        justify-content: center;
        margin: 1.5rem auto;
        border: 1px solid #6BACC9;
        border-radius: 5rem;
        color: #68AAC5;
        font-size: .9rem;
        &:hover{
            box-shadow: 0 0 1rem #6BACC9;
        }
    }
}

.rltinfo-enter-active, .rltinfo-leave-active {
    transition: all .3s;
    opacity: 1;
}
.rltinfo-leave-to, .rltinfo-enter /* .fade-leave-active to version 2.1.8 */ {
    transform: translateY(120%);
    opacity: 0;
}

</style>