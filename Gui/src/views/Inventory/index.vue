<template>
    <div class="inv" id="drop" adress="out">
        <div class="inv-container">
            <div class="inv-drop">
                <div class="inv-drop-tittle">{{loc("inv_drop_item")}}</div>
                <img src="/img/inventory/common/drop.svg" alt="drop" class="inv-drop-img">
            </div>
            <div class="inv-main">
                <div class="inv-line"></div>
                <div class="inv-parts">                   
                    <img src="/img/inventory/common/logo.png" alt="logo" class="inv-parts-logo">
                    <NearItems
                        :drag="dragObject" 
                        @onMouseDown="onMouseDown" 
                        @onDoubleClick="onDoubleClick"
                    />    
                    <Personal
                        :drag="dragObject" 
                        @onMouseDown="onMouseDown"
                        @onDoubleClick="onDoubleClick"
                    />
                    <Dynamic 
                        :darag="dragObject" 
                        @onMouseDown="onMouseDown"
                        @onDoubleClick="onDoubleClick"
                    />
                </div>  
                <FastAccess 
                    :drag="dragObject"
                    @onMouseDown="onMouseDown"
                />
            </div>
                      
        </div>
        <Other 
            v-for="(other, key) in otherInventories"
            :id="key"   
            :name="other"    
            :key="key"
            :drag="dragObject" 
            @onMouseDown="onMouseDown"
            @onDoubleClick="onDoubleClick"
        />
        <Avatar :drag="dragObject" />
        <Tooltip :tooltip="tooltip" v-if="tooltip.show"/>
        <Split 
            :from="split.from" 
            :to="split.to" 
            :item="split.item"
            @onSplitClose="split.close()" 
            @onSpliteMove="splitMove"
        />
        <Confirm 
            :msg="confirm.msg" 
            v-if="confirm.show" 
            @confirmAction="confirmAction" 
        />
    </div>
</template>

<script>
import Personal from './Personal'
import Avatar from './Avatar'
import Dynamic from './Dynamic'
import Other from './OtherInventory'
import Split from './SplitDialog'
import NearItems from './NearItems'
import FastAccess from './FastAccess'
import Tooltip from './Tooltip'
import Confirm from './Confirmation'

import {mapState, mapGetters} from 'vuex'

export default { 
    computed: {
        ...mapState('inventory', ['otherInventories', 'equip', 'personalId', 'stock']),
        ...mapGetters('inventory', ['getById']),
        ...mapGetters('localization', ['loc'])
    },
    data() {
        return {
            dragObject: null,
            spamPotection: 0,
            spamPotectionTime: 700,
            tooltip: {
                x: 500,
                y: 500,
                el: null,
                timer: 0,
                item: null,
                show: true
            },
            split: {
                from: undefined,
                to: undefined,
                close(){
                    this.from = undefined;
                    this.to = undefined;
                },
            },
            confirm:{
                msg: "inv_confirm_drop",
                callback: null,
                show: false,
            }
        }
    },
    methods: {
        //events
        onMouseDown(dragObject){
            dragObject.store = this.$store;
            this.dragObject = dragObject;
            this.tooltip.show = false;
        },
        onDoubleClick(adress, item){
            if(item == null || this.spamPotection > Date.now()) return;
            this.spamPotection = Date.now() + this.spamPotectionTime;
            if(item.isBackpack() || item.isKeyRing()){
                this.$store.commit("inventory/openOther", item);
            } else if (item.isItemBox()){
                const split = adress.split('_');
                if(split[0] == "inv")
                    window.mp.triggerServer("inv:use:itembox", split[1], split[2]);
            } else if(item.isUsable()){
                const split = adress.split('_');
                if(split[0] == "inv" && this.personalId == split[1]){
                    window.mp.trigger("inv:use:item", split[2]);                    
                    window.mp.trigger("gui:inv:close");
                }                    
            }
            else if(item.isWeapon()){
                let freeSlot = -1;
                item.config.Slots.forEach(slot=>{
                    if(!this.equip.weapons[slot]){
                        freeSlot = slot;
                        return;
                    }
                })
                if(freeSlot !== -1){
                    const split = adress.split('_');
                    if(split[0] == "inv" && this.personalId == split[1])
                        window.mp.triggerServer("equip:equip", split[1], split[2], freeSlot)
                }
            } else if(item.isMask()){
                if(!this.equip.clothes[1]){
                    const split = adress.split('_');
                    if(split[0] == "inv" && this.personalId == split[1])
                        window.mp.triggerServer("equip:equip", split[1], split[2], 1)
                }                
            }
                
        },
        onMousemove(e){
            if(this.dragObject == null) {
                const el = document.elementFromPoint(e.clientX, e.clientY);
                if(this.tooltip.el == el) return;
                this.tooltip.el = el;
                if(this.tooltip.el == null){
                    this.tooltip.show = false;
                }else{
                    if(el.id == "drop"){
                         const adress = el.getAttribute("adress").split('_');
                        if(adress[0]=="out")
                            this.tooltip.show = false;
                        else{
                            if(adress[0] == "eq"){
                                this.tooltip.item = adress[1] == 1 ? this.equip.weapons[adress[2]] : this.equip.clothes[adress[2]]
                            }else if(adress[0] == "inv"){
                                this.tooltip.item = this.getById(adress[1]).items.find(i=>i.index == adress[2]);
                            }else this.tooltip.item = null;
                            if(this.tooltip.item){
                                this.tooltip.timer = setTimeout((el)=>{
                                    if(this.dragObject == null && this.tooltip.el === el){
                                        const pos = this.tooltip.el.getBoundingClientRect();
                                        this.tooltip.x = pos.x;
                                        this.tooltip.y = pos.y + pos.height;
                                        this.tooltip.show = true;
                                    }
                                    
                                },1000, this.tooltip.el)
                            }else this.tooltip.show = false;
                        }
                        
                    }else this.tooltip.show = false;
                }
            }else this.dragObject.mouseMoveHandle(e);
        },
        onMouseRelease(e){
            if(this.dragObject == null) return;
            if(!this.dragObject.dragStart) {
                this.deleteDragObject();
            }else{
                this.$store.commit("inventory/closeOther", this.dragObject.item.inventoryId);
                this.mouseReleaseHandler(e);
                this.deleteDragObject();
            }
        },
        onKeyDown(e){
            if(this.dragObject !== null && this.dragObject.item.isStackable() && this.dragObject.item.count > 1 && !this.dragObject.alt && e.keyCode === 17)
                this.dragObject.alt = true;
        },
        onKeyUp(e){
            if(this.dragObject !== null && this.dragObject.alt && e.keyCode === 17)
                this.dragObject.alt = false;
        },

        //handlers        
        mouseReleaseHandler(){
            if(this.dragObject.overAdress  == null) return;           
            if(this.dragObject.overAdress == this.dragObject.adress) return;
            if(this.spamPotection > Date.now()) return;
            this.spamPotection = Date.now() + this.spamPotectionTime;     
            const to = this.dragObject.overAdress.split('_');
            const from = this.dragObject.adress.split('_');
            const toType = to[0];
            if(from[0] == "access" && from[0] !== toType){
                this.$store.commit("inventory/resetFastAccessButton", from[1]);
            } else if(toType == "access"){
                this.$store.commit("inventory/setFastAccessButton", {key: to[1], from});
            } else if(toType == "out"){
                if(from[1] == 0) return;
                if(this.dragObject.alt){
                    this.split.from = from;
                    this.split.to = to;
                    this.split.item = this.dragObject.item;
                }else
                    this.dropItem(from, this.dragObject.item.count);
            }else if(toType == "eq"){
                if(!this.dragObject.item.isEquipable() || !this.dragObject.item.isSlotValid(to[2])) return;
                this.toEquip(from, to)
            }else if(toType == "inv"){
                if(this.dragObject.alt){
                    this.split.from = from;
                    this.split.to = to;
                    this.split.item = this.dragObject.item;
                }else{
                    const count = this.dragObject.item.count;
                    if(this.isCanMove(from, to, count))
                        this.toInventory(from, to, count)
                }
            }
        },
        isCanMove(from, to, count){
            //from
            const fromType = from[0];
            const fromId = from[1];
            const fromIndex = from[2];
            //to
            const toType = to[0];
            const toId = to[1];
            const toIndex = to[2];

            let fromItem;
            if(fromType == "inv"){
                const fromInventory = this.getById(fromId);
                if(fromInventory == undefined) return false;
                fromItem = fromId == 0 ? fromInventory.items[fromIndex] : fromInventory.items.find(i=>i.index == fromIndex);
            }else if(fromType == "eq"){
                if(fromId == 1) fromItem = this.equip.weapons[fromIndex];
                else fromItem = this.equip.clothes[fromIndex];
            }
            if(fromItem == undefined) return false;
            if(fromItem.count < count){
                window.setData('notifyList/notify', {type: 1, position:2, message: "inv_wrong_count", time: 3000});
                return false;
            }
            if(toType == "eq") return false;
            const toInventory = this.getById(toId);
            const exists = toInventory.items.find(i=>i.index == toIndex);
            if(exists !== undefined && (!exists.isStackable() || exists.id != fromItem.id)) return false;
            if(toId == 0 || toId == fromId) return true;
            if(toInventory == undefined) return false;
            let weight = 0;
            toInventory.items.forEach(item => {
                weight += item.getWeight();
            });
            weight += fromItem.getWeight();
            if(weight >  toInventory.maxWeight){
                 window.setData('notifyList/notify', {type: 1, position:2, message: "inv_wrong_weight", time: 3000});
                return false;
            }
            return true;
        },    
        confirmAction(result){
            if(result && this.confirm.callback !== null) this.confirm.callback();
            this.confirm.callback = null;
            this.confirm.show = false;
        },
        //actions
        deleteDragObject(){
            this.dragObject = null;
        },
        splitMove(count){
            if(this.split.to[0] == "out")
                this.dropItem(this.split.from, count);
            else if(this.isCanMove(this.split.from, this.split.to, count))
                this.toInventory( this.split.from, this.split.to, count);
            this.split.close();
        },  
        toInventory(from, to, count){
            if(to[1] == 0) return;            
            if(from[0] == "eq"){
                const item = from[1] == 1 ? this.equip.weapons[from[2]] : this.equip.clothes[from[2]];
                if(item == null || (item.promo && to[1] != this.personalId)){
                    this.confirm.msg = "inv_confirm_nomove"
                    this.confirm.callback = null;
                    this.confirm.show = true;
                    return;
                }
                window.mp.triggerServer("equip:remove", from[1], from[2], to[1], to[2]);
            }                
            else if(from[0] == "inv"){
                if(from[1] == 0){
                    const item = this.getById(0).items[from[2]];
                    if(item === undefined) return;
                    if(this.stock.id == to[1] && this.stock.type == 3 && !item.isEquipClothes())
                         window.setData('notifyList/notify', {type: 1, position:2, message: "inv_msg_cloth_only", time: 3000});
                    else 
                        window.mp.triggerServer("inv:pickup", item.index, to[1], to[2], count);
                }else{
                    const item = this.getById(from[1]).items.find(i=>i.index == from[2]);
                    if(item === undefined) return;
                    if(item.promo && to[1] != this.personalId){
                        this.confirm.msg = "inv_confirm_nomove"
                        this.confirm.callback = null;
                        this.confirm.show = true;
                        return;
                    }
                    if(this.stock.id == to[1] && this.stock.type == 3 && !item.isEquipClothes())
                         window.setData('notifyList/notify', {type: 1, position:2, message: "inv_msg_cloth_only", time: 3000});
                    else 
                    window.mp.triggerServer("inv:moveItem", from[1], from[2], to[1], to[2], count);
                }
                    
            }
        },        
        dropItem(from, count){
            if(from[0] === "eq"){
                const item = from[1] == 1 ? this.equip.weapons[from[2]] : this.equip.clothes[from[2]];
                if(item.promo){
                    this.confirm.callback = ()=>{
                        window.mp.triggerServer("equip:drop", from[1], from[2]);
                    };
                    this.confirm.msg = "inv_confirm_drop";
                    this.confirm.show = true;
                }else
                    window.mp.triggerServer("equip:drop", from[1], from[2]);
            }
            else if(from[0] === "inv"){
                const item = this.getById(from[1]).items.find(i=>i.index == from[2]);
                if(item.promo){
                    this.confirm.callback = ()=>{
                        window.mp.triggerServer("inv:drop", from[1], from[2], count);
                    };
                    this.confirm.msg = "inv_confirm_drop";
                    this.confirm.show = true;
                }else
                    window.mp.triggerServer("inv:drop", from[1], from[2], count);
            }
        },
        toEquip(from, to){
             if(from[0] == "eq"){
                window.mp.triggerServer("equip:move", from[1],from[2], to[2])
            }else if(from[0] == "inv"){
                if(from[1] == 0){
                    const item = this.getById(0).items[from[2]];
                    if(item === undefined) return;
                    window.mp.triggerServer("equip:pickup", item.index, to[2]);
                } else 
                    window.mp.triggerServer("equip:equip", from[1], from[2], to[2]);
            }
        },

        //dev
        setDevMod(){
            this.$store.commit('inventory/updateEquip', {type: 2, slot: 8, item:[10,1,-1, 0]});
            this.$store.commit('inventory/updateEquip', {type: 2, slot: 14, item:[5,1,23, 1, 11968,10]});
            this.$store.commit('inventory/updateEquip',  {type: 1, slot: 1, item:[76,1,-1, 1, 11423123]});
            this.$store.commit('inventory/setPersonalId', 1);
            this.$store.commit('inventory/update', {
                id: 1,
                maxWeight: 40000,
                size: 36,
                items: [[23,1,1,0],[13, 1, 2, 1],[76, 1, 13, 0],[18, 15, 5, 1],[5, 1, 7, 0, 2, 5000], [1, 1, 4, 0, 2, 5000]]
            });
            this.$store.commit('inventory/update', {
                id: 2,
                maxWeight: 50000,
                size: 50,
                items:[[7, 1, 1, 1],[1, 5, 4, 0],[10, 2, 14, 1],[4, 1, 12, 0]]
            });
            this.$store.commit('inventory/setStock', {"id": 1,"type": 3});
            this.$store.commit('inventory/setDynamic', 'Equip');
            this.$store.commit('inventory/update', {id:0, items:[[7,1,12, 1],[7,1,12, 1]], maxWeight: -1, size: 2});
            //this.$store.commit("inventory/setFastAccessData", {"1": {type: "eq", id: 1}, "2": {type: "eq", id: 2}, "3": {type: "eq", id: 3}, "4": {type: "eq", id: 4}, "5": null, "6": null, "7": null, "8": null, "9": null, "10": null});
        }
    },
    mounted() {
        document.addEventListener( "keydown", this.onKeyDown);
        document.addEventListener( "keyup", this.onKeyUp);
        document.addEventListener("mouseup", this.onMouseRelease);
        document.addEventListener("mousemove", this.onMousemove);
        if(process.env.NODE_ENV == 'development') this.setDevMod();
    },
    beforeDestroy(){
        document.removeEventListener( "keydown", this.onKeyDown);
        document.removeEventListener( "keyup", this.onKeyUp);
        document.removeEventListener("mouseup", this.onMouseRelease);
        document.removeEventListener("mousemove", this.onMousemove);
    },
    components:{
        Personal,
        Avatar,
        Dynamic,
        Other,
        Split,
        NearItems,
        FastAccess,
        Tooltip,
        Confirm
    }
}
</script>

<style lang="scss" scoped>
.inv{
    position: absolute;
    top: 0;
    left: 0;
    width: 100vw;
    height: 100vh;
    background: url(/img/inventory/common/bg.png) no-repeat center;
    background-size: cover;
    text-transform: uppercase;
    font-style: normal;
    font-weight: 300;
    font-size: .9rem;
    line-height: 1.2rem;
    letter-spacing: .03rem;
    &-drop{
        background-color: rgba(#000, .3);
        height: 100%;
        width: 4.5rem;
        display: flex;
        align-items: center;
        justify-content: flex-start;
        flex-direction: column;
        &-tittle{
            font-weight: 400;
            font-size: 1.2rem;
            line-height: 1.45rem;
            letter-spacing: 0.06rem;
            color: rgba(#fff, .2);
            text-transform: uppercase;
            transform: rotate(-90deg);
            width: 12rem;
            margin: 6.5rem 0;
        }
    }
    &-container{
        width: 100%;
        margin: 0 auto;
        height: 100%;
        position: relative;
        display: flex;
        justify-content: space-between;
        align-items: center;
        overflow: hidden;
        &-logo{
            position: absolute;
            right: -11rem;
            top: -5rem;
            z-index: -1;
        }
    }
    &-main{
        height: 100%;
        width: 96rem;
    }
    &-line{ 
        margin: 1.05rem auto 0;
        border-top: 1px solid rgba(#FFF, .1);
        border-bottom: 1px solid rgba(#FF3380, .5);
        height: 7px;
        width: 81rem;
    }
    &-parts{
        margin: 3.5rem auto 0;
        width: 83rem;
        display: flex;
        justify-content: space-between;
        align-items: center;
        height: 39rem; 
        &-logo{
            position: absolute;
            right: -11rem;
            top: -5rem;
            z-index: -1;
        }
    }
}
</style>