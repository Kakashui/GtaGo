<template>
    <div class="newdshop-shop-clexcl">
        <div class="newdshop-shop-clexcl-head">
            <div class="newdshop-shop-clexcl-head_title">{{loc("newdshop:shop:excl:title")}}</div>
            <div class="newdshop-shop-clexcl-head_sub">{{loc("newdshop:shop:excl:sub1")}}<br>{{loc("newdshop:shop:excl:sub2")}}</div>
        </div>
        <div class="newdshop-shop-list">
            <Item v-for="(item, index) in items" :key="index" :item="item" @onItemSelect="selectItem" :exclusive="true" />
        </div>
        <div class="newdshop-shop-clexcl-decor"></div>
    </div>
</template>

<script>
import { mapGetters } from 'vuex'
import Item from './Item'

export default {
    props:{
        items: Array
    },
    computed:{
        ...mapGetters('localization',['loc'])
    },
    methods: {
        selectItem(item){
            this.$emit("onItemSelect", item);
        }
    },
    components:{
        Item
    }
}
</script>

<style lang="scss">
    .newdshop-shop-clexcl{
        margin: 3rem auto 0;
        width: 72.5rem;
        &-head{
            display: flex;
            &_title{
                font-size: 3rem;
            }
            &_sub{
                font-size: 1rem;
                color: #C9AE5F;
                
                margin-left: 1rem;
                margin-top: .7rem;
            }
        }
        &-decor{
            background: rgba(255, 255, 255, 0.2);
            width: 100%;
            height: 1px;
            margin-top: 1.5rem;
        }
    }
</style>