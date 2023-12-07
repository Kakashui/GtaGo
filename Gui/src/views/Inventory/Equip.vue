<template>
    <div class="equip nopoint" >
        <img src="/img/inventory/equip/man.png" alt="man" class="equip-man">
        <div class="equip-light"></div>
        <div class="equip-status">
            <div class="equip-tittle">
                {{loc('inv_equip_tittle')}}
            </div>
            <div class="equip-subtittle">
                {{loc('inv_equip_sublittle')}}
            </div>
            <div class="equip-stats">
                <div class="equip-state" v-for="(state, key) in statusDisplays" :key="key">
                    <img :src="`/img/inventory/equip/${key}.svg`" alt="" class="equip-state-img">
                    <div class="equip-state-body">
                        <div class="equip-state-tittle"><span>{{loc(`eq_${key}`)}}</span> <span style="color: #fff">{{state}}%</span></div>
                        <div class="equip-state-progress">
                            <div class="equip-state-progress-thumb" :style="{'width': `${state}%`}"></div>
                        </div>
                    </div>
                   
                </div>
            </div>
        </div>
        
        <div class="equip-list">
            <div class="equip-item equip-clothes" id="drop"
                v-for="(clth, slot) in equip.clothes" 
                :key="`cl_${slot}`" 
                :adress="adress(2, slot)"
                @mousedown="onMouseDown(2, slot, clth, $event)"
                :class="{'equip-active': isActive(2, slot), 'equip-available': isAvailable(2, slot), 'equip-promo': clth && clth.promo}"
            >
                <img 
                    :src="getClothImage(clth, slot)" 
                    :style="{
                        'width': clth == null ? '50%' : '70%',
                        'height': clth == null ? '50%' : '70%'
                    }"
                    alt="clothes"
                >
            </div>
            <div class="equip-item equip-weapon" id="drop"
                v-for="(weap, slot) in equip.weapons" 
                :key="`wep_${slot}`" 
                :adress="adress(1, slot)"
                @mousedown="onMouseDown(1, slot, weap, $event)"
                :class="{'equip-active': isActive(1, slot), 'equip-available': isAvailable(1, slot), 'equip-with-item': weap != null, 'equip-promo': weap && weap.promo}"                
            >
                <img 
                    :src="getWeapImage(weap, slot)" 
                    alt="weapon"                     
                    :style="{
                        'width': weap == null ? '50%' : '70%', 
                        'height': weap == null ? '50%' : '70%'
                    }"
                >
                <div class="equip-item-tittle" 
                    :style="{
                        'color': weap == null ? 'rgba(255, 255, 255, 0.2)' : '#fff'
                    }"
                >
                    Weapon {{slot}}
                </div>
            </div>
            <div class="equip-delimiter"></div>
        </div>
        
    </div>
</template>

<script>
import dragObject from './classes/dargObject'
import {mapState, mapGetters} from 'vuex'

export default {   
    props:['drag', 'checkInventory'],
    data() {
        return {
            lastClick: 0,
            clickDelay: 500
        }
    },
    methods: {
        getClothImage(item, slot){
            return item == null ? `/img/inventory/equip/cloth${slot}.svg` : item.getImage();
        },
        getWeapImage(item, slot){
            return item == null ? `/img/inventory/equip/weap${slot}.svg` : item.getImage();
        },
        onMouseDown(type, slot, item, e){
            if(e.button != 0 || item == null) return;
            if(this.lastClick > Date.now()){
                if(item.isUsable())
                    this.$emit("onDoubleClick", this.adress(type, slot), item)
            }else{
                const object = new dragObject(this.adress(type, slot), item, e.clientX, e.clientY);
                this.$emit("onMouseDown", object)
            }
            this.lastClick = Date.now() + this.clickDelay;
        },
        isActive(type, slot){
            if(!this.drag || !this.drag.dragStart || !this.drag.item) return false;
            return this.drag.overAdress == this.adress(type, slot) && this.isAvailable(type, slot);
        },
        isAvailable(type, slot){
            if(!this.drag || !this.drag.dragStart || !this.drag.item) return false;
            if(this.drag.item.isWeapon()){      
                if(!this.drag.item.isType(type)) return false;
                if(!this.drag.item.isSlotValid(slot)) return false;
            } else {
                if(type == 1 && this.drag.item.isEquipClothes()) return false;
                if(!this.drag.item.isSlotValid(slot)) return false;
            }
            return true;
        },
        adress(type, slot){
            return `eq_${type}_${slot}`
        }
    },
    computed: {
        ...mapState('hud', ['statusDisplays']),
        ...mapGetters('localization', ['loc']),
        equip(){
            if(this.checkInventory) return this.checkInventory.equip
            else return this.$store.state.inventory.equip
        }
    },
}
</script>

<style lang="scss" scoped>
.equip{
    width: 100%;
    height: 100%;
    position: relative;
    &-man{
        position: absolute;
        top: 2rem;
        right: -5rem;
        pointer-events: none;
    }
    &-delimiter{
        background-color: rgba(#fff, .1);
        position: absolute;
        bottom: .5rem;
        left: 7.4rem;
        width: .05rem;
        height: 21rem;
    }
    &-light{
        width: 22rem;
        height: 22rem;
        position: absolute;
        top: -7rem;
        right: -6rem;
        background: rgba(255, 51, 128, 0.1);
        filter: blur(200px);
        pointer-events: none;
    }
    &-status{
        position: relative;
        width: 12rem;
        pointer-events: none;
    }
    &-state{
        margin: .7rem 0;
        display: flex;
        justify-content: flex-start;
        align-items: center;
        pointer-events: none;
        &-body{
            width: 6rem;
            margin-left: .5rem;
        }
        &-tittle{
            color: rgb(#fff, .5);
            font-weight: normal;
            display: flex;
            align-items: center;
            justify-content: space-between;
            pointer-events: none;
            span{
                &:first-child{                    
                    font-size: .8rem;
                    line-height: .8rem;
                }
            }
        }
        &-progress{
            height: 4px;
            width: 100%;
            background-color: rgba(#fff, .5);
            position: relative;
            overflow: hidden;
            border-radius: 6px;
            &-thumb{
                background-color: #FF3380;
                height: 100%;
            }
        }
    }
    &-tittle{
        color: #fff;
        width: 100%;
        font-size: 2.5rem;
        line-height: 2.5rem;
    }
    &-subtittle{
        width: 100%;
        height: 100%;
        color: rgba(#fff, .5);
        margin-top: .3rem;
    }
    &-list{
        display: flex;
        justify-content: space-around;
        flex-wrap: wrap;
    } 
    &-item{        
        position: absolute;
        width: 4.5rem;
        height: 4.5rem;      
        border: 2px solid rgba(255, 255, 255, 0.1);
        filter: drop-shadow(0px 10px 30px rgba(0, 0, 0, 0.4));
        display: flex;
        align-items: center;
        justify-content: center;
        margin-bottom: .5rem;
        img{
            -webkit-user-drag: none;
            -o-user-drag: none;
            width: 70%;
            height: 70%;
            pointer-events: none;
        }
        &-tittle{
            position: absolute;
            transform: rotate(-90deg);
            font-weight: normal;
            letter-spacing: .05rem;
            left: -2.3rem;
            bottom: 1.2rem;
            text-align: left;
        }
    }
    
    &-clothes{
        background: radial-gradient(125% 125% at 50% 50%, #000000 0%, rgba(0, 0, 0, 0) 100%);     
       
        &:nth-child(13){
            bottom: 0;
            right: 0;
        }
        &:nth-child(12){
            bottom: 0;
            right: 5.5rem;
        }
        &:nth-child(14){
            bottom: 0;
            right: 11rem;
        }
        &:nth-child(9){
            bottom: 5.5rem;
            right: 0;
        }
        &:nth-child(11){
            bottom: 5.5rem;
            right: 5.5rem;
        }
        &:nth-child(10){
            bottom: 5.5rem;
            right: 11rem;
        }
        &:nth-child(8){
            bottom: 11rem;
            right: 0;
        }
        &:nth-child(7){
            bottom: 11rem;
            right: 5.5rem;
        }
        &:nth-child(6){
            bottom: 11rem;
            right: 11rem;
        }
        &:nth-child(4){
            bottom: 16.5rem;
            right: 0;
        }
        &:nth-child(5){
            bottom: 16.5rem;
            right: 5.5rem;
        }
        &:nth-child(3){
            bottom: 16.5rem;
            right: 11rem;
        }
        &:nth-child(2){
            bottom: 22rem;
            right: 5.5rem;
        }
        &:nth-child(1){
            bottom: 27.5rem;
            right: 5.5rem;
        } 
        &:nth-child(15){
            bottom: 33rem;
            right: 5.5rem;
        } 
        
    }
    &-promo{
         background: radial-gradient(80.56% 80.56% at 50% 76.11%, rgba(182, 211, 0, 0.2) 0%, rgba(182, 211, 0, 0) 100%);
         border: 1px solid rgba(#B6D300, .2);
    }
    &-with-item{
        background-color: rgba(#fff, .03);
    }
    &-weapon{
        left: 1.8rem;       
        &:nth-child(16){
            bottom: 0;
        }
        &:nth-child(17){
            bottom: 5.5rem;
        }
        &:nth-child(18){
            bottom: 11rem;
        }
        &:nth-child(19){
            bottom: 16.5rem;
        }
    }
    &-active{
        background-color: rgba(#fff, .08);
    }
    &-available{
        border: 2px solid rgba(255, 255, 255, 0.6);
    }
}
</style>