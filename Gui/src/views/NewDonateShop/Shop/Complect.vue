<template>
    <div class="newdshop-shop-comlect">
        <div class="newdshop-shop-comlect_title">{{loc('ndshop:compl:tit')}}</div>
        <div class="newdshop-shop-comlect_row">
            <Item :item="complect.item" @onItemSelect="selectItem"/>
            <div class="newdshop-shop-comlect_sub">{{loc('ndshop:compl:sub')}}</div>
            <div class="newdshop-shop-comlect_list">
                <ComplectItem v-for="(item, index) in complect.items" :key="index" :item="item" :discount="complect.item.data.Discount"/>
            </div>
        </div>
    </div>
</template>

<script>
import Item from './Item'
import ComplectItem from './ComplectItem'
export default {
    props:{
        complect: Object
    },
    components:{
        Item,
        ComplectItem
    },
    methods: {
        selectItem(item){
            this.$emit("onItemSelect", item)
        }
    },
}
</script>

<style lang="scss">
    .newdshop-shop-comlect{
        margin: 3rem auto 0;
        width: 72.5rem;
        &_title{
            font-size: 2rem;
            margin-top: 2.5rem;
            font-weight: 300;
            letter-spacing: .15rem;
        }
        &_row{
            margin-top: 1rem;
            display: flex;
            position: relative;
            justify-content: flex-start;
        }
        &_sub{
            position: absolute;
            bottom: -1.5rem;
            left: 15rem;
            font-size: 1.5rem;
            color: rgba(#fff, .4);
            transform-origin: 0 0;
            transform: rotate(-90deg);
            height: 2rem;
            letter-spacing: .8rem;
        }
        &_list{
            height: 23rem;
            //width: 60rem;
            margin-left: 4rem;
            margin-top: .5rem;
            display: flex;
            flex-wrap: wrap;
            justify-content: space-between;
            align-items: flex-start;
            flex-direction: column;
        }
    }
</style>