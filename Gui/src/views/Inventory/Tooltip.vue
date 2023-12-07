<template>
    <div class="tooltip"
        :style="{'top': `${tooltip.y}px`,'left': `${tooltip.x}px`}"
    >
        <div class="tooltip-tittle">
            <div class="tooltip-tittle-type">{{loc(type)}}</div>
            <div class="tooltip-tittle-name">{{loc(name)}}</div>
        </div>
        <div class="tooltip-body">
            <div class="tooltip-item" v-for="(info, index) in infos" :key="index">
                <img :src="`/img/inventory/common/${info.ico}.svg`" alt="icon">
                <div class="tooltip-item-tittle">{{loc(info.tittle)}}:</div>
                <div class="tooltip-item-text">{{info.text}}</div>
            </div>            
            <div class="tooltip-item-promo" v-if="promo">
                <div class="tooltip-item-promo-row">{{loc("inv_promo")}}</div> 
                <div class="tooltip-item-promo-row">{{loc("inv_promo_info")}}</div>
            </div>
        </div>
        <div class="tooltip-footer">
            <img src="/img/inventory/common/mouseLeft.svg" alt="use" class="tooltip-footer-img"><div style="color: #fff">X2</div> <div class="tooltip-footer-desc">- {{loc("inv_action_interact")}}</div>
            
            <div class="tooltip-footer-img" v-if="tooltip.item && tooltip.item.count > 1">Ctrl</div> <div class="tooltip-footer-desc" v-if="tooltip.item && tooltip.item.count > 1">- {{loc("inv_action_split")}}</div>
        </div>
    </div>
</template>

<script>
import {mapGetters} from 'vuex'

export default {
    props:['tooltip'],
    data() {
        return {
            type: "inv_type_other",
            name: "item_invalid",
            promo: false,
            infos: []
        }
    },
    computed: {
        ...mapGetters('localization', ['loc'])
    },
    methods: {
        getClothingName(gender, id, drawable){
            let clothType = -1;
            switch (id) {
                case 1:
                    return `Mask ${drawable}`
                case 2:
                    clothType = 5;
                    break;
                case 4:
                    clothType = 3;
                    break;
                case 8:
                    clothType = 4;
                    break;
                case 9:
                case 10:
                    clothType = 8;
                    break;
                case 11:
                    clothType = 2;
                    break;
                case 12:
                    return 'Armor';
                case 13:
                    clothType = 1;
                    break;
                case 14:
                    clothType = 0;
                    break;
                case 15:
                    clothType = 7;
                    break;
                case 16:
                case 17:
                    clothType = 6;
                    break;
                case 3:
                    clothType = -2;
                    break;
            
                default:
                    break;
            }
            if (clothType > -1)
                return this.$store.getters['inventory/getClothingName']([gender, clothType, drawable]);
            else
                return 'New Hot Sale';
        },
    },
    mounted(){
        if(this.tooltip.item == null ) this.tooltip.show = false;
        else{
            this.infos.push({ico: "pesa_1", tittle: "inv_weight", text: `${(this.tooltip.item.getWeight()/1000).toFixed()} kg - X${this.tooltip.item.count}`});
            if(this.tooltip.item.serial)this.infos.push({ico: "serial", tittle: "inv_serial_number", text: `${this.tooltip.item.serial}`})
            if(this.tooltip.item.vehNumber)this.infos.push({ico: "vehnumber", tittle: "inv_vehicle_number", text: `${this.tooltip.item.vehNumber}`})
            if(this.tooltip.item.storedName && this.tooltip.item.countStored)this.infos.push({ico: "itembox", tittle: "inv_stored_item", text: `${this.loc(this.tooltip.item.storedName)} x${this.tooltip.item.countStored * this.tooltip.item.getCountItemInOneReceive()}`})
            if(this.tooltip.item.gender && this.tooltip.item.drawable)this.infos.push({ico: "serial", tittle: "inv_clothes_name", text: `${this.getClothingName(this.tooltip.item.gender, this.tooltip.item.id, this.tooltip.item.drawable)}`})
            if(this.tooltip.item.costumeName)this.infos.push({ico: "serial", tittle: "inv_clothes_name", text: `${this.loc(this.tooltip.item.costumeName)}`})
            if(this.tooltip.item.countFish)this.infos.push({ico: "serial", tittle: "inv_cage_count_fish", text: `${this.tooltip.item.countFish}`})
            if(this.tooltip.item.water || this.tooltip.item.water === 0)this.infos.push({ico: "serial", tittle: "inv_water", text: `${this.tooltip.item.water}`})


            this.promo = this.tooltip.item.promo;
            this.type = this.tooltip.item.getTypeName();
            this.name = this.tooltip.item.getDisplayName();
        }
        
        //if(this.tooltip.item)
    }
}
</script>

<style lang="scss" scoped>
.tooltip{
    width: 19rem;
    background: #142126;
    position: fixed;
    border: 2px solid rgba(#fff, .1);
    color: #fff;
    pointer-events: none;
    box-shadow: 0 1rem 3rem rgba(0, 13, 18, 0.5);
    &-item{
        display: flex;
        align-items: center;
        margin: .5rem 0;
        img{
            width: 1.1rem;
            height: 1.1rem;
        }
        &-tittle{
            margin-left: .5rem;
            color: rgba(#fff, .5);
        }
        &-text{
            margin-left: .5rem;
        }
        &-promo{
            font-size: .7rem;
            left: .7rem;
            letter-spacing: .06rem;
            margin-top: 1rem;
            &-row{
                text-align-last: left;
                &:nth-child(1){
                    color:rgba( #FF3380, .5);
                    text-shadow: 0 0 .5rem#FF3380;
                }
                &:nth-child(2){
                    color: #fff;
                }
            };
        }
    }
    &-tittle{
        height: 4rem; 
        background: url(/img/inventory/common/otherHead.png) no-repeat center;
        background-size: cover;
        padding-top: 1rem;
        font-size: 1.1rem;
        line-height: 1.1rem;
        &-type{
            margin-left: 1.2rem;
            font-size: .8rem;
            line-height: .85rem;
            color: rgba(#fff, .5);
        } 
        &-name{
            margin-left: 1.2rem;
            margin-top: .2rem;
            font-size: 1.1rem;
            line-height: 1.1rem;
            font-weight: 300;
        }
    }
    &-body{
        margin: 1rem 1.2rem;
    }
    &-footer{
        background-color: #000D12;
        width: 100%;
        height: 2rem;
        display: flex;
        align-items: center;
        justify-content: flex-start;
        font-size: .8rem;
        line-height: .85rem;
        color: rgba(#fff, .5);
        &-img{
            margin-left: .5rem;
            color: #fff;
        }
        &-desc{
            margin-left: .5rem;
            margin-right: .2rem;
        }
    }
}
</style>