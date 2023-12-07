<template>
    <div class="newdshop-takeitem">
        <div class="newdshop-takeitem_body">
            <div class="newdshop-takeitem_list">
                <Item v-for="(item, index) in itemList" :key="index" 
                    :item="item" 
                    :count="+item.count" 
                    :selected="currentItem !== null && currentItem === item"
                    @onSelect="selectItem"
                />
            </div>
            <div class="newdshop-takeitem_btn" @click="takeItem">{{loc('take:donate:item')}} {{name}}</div>
        </div>
    </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
import Item from './Inventory/Item'
import {itemById} from '../../configs/newDonateShop'

export default {
    data() {
        return {
            currentItem: null
        }
    },
    computed:{
        ...mapState("newDonateShop", ["takeItems"]),
        ...mapGetters('localization', ['loc']),
        itemList(){
            const list = [];
            this.takeItems.forEach(i => {
                const item = itemById(i.id);
                list.push({item, count: i.count, sell: i.sell});
            });
          
            if(list.length < 36){
                for (let index = list.length; index < 36; index++) {
                    list.push({item:null, count:-1, sell: false})                    
                }
            }
            return list;
        },
        name(){
            return this.currentItem ? this.currentItem.item.name : "";
        }
    },
    methods: {
        selectItem(item){
            this.currentItem = item;
        },
        takeItem(){
            if(this.currentItem === null) return;
            window.mp.triggerServer("dshop:item:take", this.currentItem.item.id, this.currentItem.sell);
            window.mp.trigger("dshop:close");
        },
        keyHandler(e){
            if(e.keyCode === 27){
                window.mp.trigger("dshop:close");
            }
        }
    },
    components:{
        Item
    },
    mounted(){
        addEventListener("keyup", this.keyHandler)
    },
    beforeDestroy(){
        removeEventListener("keyup", this.keyHandler)
    }
}
</script>

<style lang="scss">
.newdshop-takeitem{
    position: fixed;
    width: 100vw;
    height: 100vh;
    top: 0;
    left: 0;
    display: flex;
    align-items: center;
    justify-content: center;
    &_body{
        width: 40rem;
        height: 40rem;
        background-color: rgba(0,0,0,.8);
        position: relative;
    }
    &_list{
        width: 100%;
        height: 85%; 
        display: flex;
        flex-wrap: wrap;
        overflow: hidden scroll;
    }
    &_btn{
        color: #000;
        font-size: 1.5rem;
        width: 90%;
        height: 3rem;
        background-color: #9FBA1B;
        display: flex;
        align-items: center;
        justify-content: center;
        margin: 1rem auto;
    }
}
</style>