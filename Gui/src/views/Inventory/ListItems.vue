<template>
    <div class="list"
        :class="{'nopoint': canDrop}"
        :style="{'width': width == undefined ? '96%' : `${width * 5.6}rem`}"
    >
        <div 
            class="list-item" :id="canDrop ? 'drop' : 'nodrop'" 
            v-for="(item, index) in items" 
            :key="index" 
            :adress="adress(index)" 
            :class="{'list-active': isActive(index), 'list-with-item': item && !item.promo, 'list-with-promo': item && item.promo}"
            @mousedown="onMouseDown(index, item, $event)"
        >
            <img  v-if="item" :src="item.getImage()" alt="item">
            <div class="list-item-count" v-if="item">x{{item.count}}</div>
            <div class="list-item-promo" v-if="item && item.promo"></div>
        </div>
    </div>
</template>

<script>
import dragObject from './classes/dargObject'
import {mapGetters} from 'vuex'

export default {
    props:["id", "drag", "width", "sortByIndex", "canDrop", "checkInventory"],
    data() {
        return {
            lastClick: 0,
            clickDelay: 500
        }
    },
    computed: {
        ...mapGetters('inventory', ['getById']),
        inventory(){
            if(this.checkInventory) return this.checkInventory;
            else return this.getById(this.id);
        },
        items(){
            const items = [];
            const size = Math.max(this.inventory.size, this.inventory.items.length);
            for (let i = 0; i < size; i++) {
                if(this.sortByIndex){
                    const index = this.inventory.items.findIndex(item=>item.index == i);
                    items.push(index === -1 ? null : this.inventory.items[index]);
                }else{
                    items.push(this.inventory.items[i] == undefined ? null : this.inventory.items[i]);
                }
            }
            return items;
        }
    },
    methods: {
        adress(index){
            return `inv_${this.id}_${index}`
        },
        isActive(index){
            return this.drag && this.adress(index) == this.drag.overAdress
        },
        onMouseDown(index, item, e){
            if(item == null) return;
            var adress = this.adress(index);
            if(e.button === 2){
                if(item.isUsable() || item.isWeapon() || item.isMask()){
                    this.$emit("onDoubleClick", adress, item)
                }
            }else if(this.lastClick > Date.now()){
                if(item.isUsable())                    
                    this.$emit("onDoubleClick", adress, item)
                
            }else{
                const object = new dragObject(adress, item, e.clientX, e.clientY);
                this.$emit("onMouseDown", object)
            }
            this.lastClick = Date.now() + this.clickDelay;            
        },
    }
}
</script>

<style lang="scss" scoped>
.list{
    display: flex;
    flex-wrap: wrap;
    justify-content: flex-start;
    overflow: hidden auto;
    margin: auto;
    max-height: 33rem;
    &::-webkit-scrollbar {
      width: 2px;
    }
    &::-webkit-scrollbar-track {
      border: solid 0 transparent;
      margin: .1rem;
    }
    &::-webkit-scrollbar-thumb {
      border: solid 0 transparent;      
      background-color: rgba(#fff,.1);
    }
    &-item{
        width: 4.5rem;
        height: 4.5rem;
        margin: 0 1rem 1rem 0;
        background: rgba(255, 255, 255, 0.03);
        border: 2px solid rgba(255, 255, 255, 0.1);
        box-sizing: border-box;
        display: flex;
        align-items: center;
        justify-content: center;
        position: relative;
        img{
            width: 70%;
            height: 70%;
            -webkit-user-drag: none;
            -o-user-drag: none;
            pointer-events: none;            
        }
        &-count{
            color: #fff;
            position: absolute;
            top: .1rem;
            left: .3rem;
            font-size: .9rem;
            line-height: 1.2rem;
            letter-spacing: .03rem;
        }
        &-promo{
            position: absolute;
            height: .01rem;
            width: 100%;
            left: 0;
            bottom: 0;
            background: #B6D300;
        }
    }
    &-active{
        border: 2px solid rgba(255, 255, 255, 0.6);
        
    }
    &-with-item{
        background: radial-gradient(80.56% 80.56% at 50% 76.11%, rgba(255, 255, 255, 0.15) 0%, rgba(255, 255, 255, 0) 100%);
    }
    &-with-promo{        
        background: radial-gradient(80.56% 80.56% at 50% 76.11%, rgba(182, 211, 0, 0.2) 0%, rgba(182, 211, 0, 0) 100%);
    }
}
</style>