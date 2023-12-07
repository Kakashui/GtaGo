<template>
    <div class="wcomponents">
        <div class="wcomponents-tittle">{{loc(`wshop_comp_cat_${cSlot.toLowerCase()}`)}}</div>
        <div class="wcomponents-items" v-if="ComponentSlots[cSlot] !== undefined">
            <div class="wcomponents-item" 
                v-if="cSlot != 'Clip'"
                @click="select(-1)"
            >
               <img :src="`/img/weaponShop/components/${cSlot.toLowerCase()}.png`" alt="component" class="wcomponents-item-img">
                <img v-if="current == -1" src="/img/weaponShop/check.svg" alt="check" class="wcomponents-item-check">
            </div>
            <div class="wcomponents-item" 
                v-for="(component, index) in components" 
                :key="index" 
                @click="select(index)"
            >
                <img :src="`/img/weaponShop/components/${cSlot.toLowerCase()}.png`" alt="component" class="wcomponents-item-img">
                <img v-if="current == index" src="/img/weaponShop/check.svg" alt="check" class="wcomponents-item-check">
            </div>
        </div>
    </div>
</template>

<script>
import { mapState, mapGetters } from 'vuex'
import ComponentSlots from './ComponentSlots'

export default {
    props:['components', 'weaponPrice', 'cSlot'],
    computed:{
        ...mapGetters('localization', ['loc']),
        ...mapState('weaponShop', ['loadingWeapon']),
        price(){
            switch (this.cSlot) {
                case 'Muzzle': return .3
                case 'FlashLight': return .1
                case 'Clip': return .2
                case 'Scope': return .5
                case 'Grip': return .1
                case 'Skin': return 5            
                default: return .5
            }
        }
    },
    data() {
        return {
            current: this.cSlot == 'Clip' ? 0 : -1,
            ComponentSlots
        }
    },
    watch: {
        components(){
            this.current = this.cSlot == 'Clip' ? 0 : -1
        }
    },
    methods: {
        select(index){
            if(this.loadingWeapon || this.current == index) return;
            this.current = index;
            this.$emit("onSelect", this.ComponentSlots[this.cSlot], this.current, {slot: this.cSlot, price: this.price});
        }
    }
}
</script>

<style lang="scss" scoped>
    .wcomponents{
        height: 100%;
        margin-top: .5rem;
        width: 100%;
        color: #fff;
        &-tittle{
            margin-bottom: .2rem;
            font-size: .7rem;
            line-height: .7rem;
        }
        &-items{
            height: 90%;
            overflow-y: auto;
            display: flex;
            justify-content: flex-start;
        }
        &-item{
            height: 3rem;
            width: 3rem;
            display: flex;
            align-items: center;
            justify-content: center;
            margin-right: .5rem;
            background-color: rgba(#000, .45);
            position: relative;
            border: 2px solid rgba(#000, .0);
            &-img{
                width: 80%;
                height: 80%;
            }
            &-check{
                position: absolute;
                right: -.5rem;
                top: -.2rem;
            }
            &:hover{                
                border: 2px solid rgba(#C1E704, .45);
            }
        }
    }
</style>