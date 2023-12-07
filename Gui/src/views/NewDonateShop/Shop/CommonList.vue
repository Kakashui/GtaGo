<template>
    <div class="newdshop-shop-clcommon">
        <div class="newdshop-shop-list newdshop-shop-clexcl">
            <Item v-for="(item, index) in items" :key="index" :item="item" @onItemSelect="selectItem"/>            
        </div>        
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
    .newdshop-shop-clcommon{        
        margin: 3rem auto 0;
        width: 72.5rem;
    }
</style>