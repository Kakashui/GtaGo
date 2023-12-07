<template>
    <div class="mplant-inv">
        <div class="mplant-inv-block">
            <div class="mplant-inv-block_title">{{loc("mplant:inv:ore")}}</div>
            <div class="mplant-inv_body">
                <inventory-list :items="inventory.ore" @onitemselect="selectOreItem" />
            </div>
        </div>

        <div class="mplant-inv-furnace">
            <!-- <img src="/img/miniGames/metalPlant/common/title.png" alt="title" class="mplant-inv-furnace_bg"> -->
            <div class="mplant-inv-furnace_title">{{loc("mplant:inv:furnace")}}</div>
            <div class="mplant-inv-furnace_sub">{{loc("mplant:inv:sub:1")}}<br>{{loc("mplant:inv:sub:2")}} </div>
            <div class="mplant-inv-furnace_body">
                <div>
                    <div class="mplant-inv-furnace_name">{{loc(oreName)}}</div>
                    <div class="mplant-inv-furnace_item">
                        <img :src="`/img/miniGames/metalPlant/resources/${image(furnace.ore)}.png`" alt="item" v-if="furnace.ore">
                    </div>
                </div>
                <div>
                    <div class="mplant-inv-furnace_name">{{loc(fuelName)}}</div>
                    <div class="mplant-inv-furnace_item">
                        <img :src="`/img/miniGames/metalPlant/resources/${image(furnace.fuel)}.png`" alt="item" v-if="furnace.fuel">
                    </div>
                </div>
            </div>
            <div class="mplant-inv-furnace_horn">
                <Horn />
            </div>
        </div>
        
        <div class="mplant-inv-block">
            <div class="mplant-inv-block_title">{{loc("mplant:inv:fuel")}}</div>
            <div class="mplant-inv_body">
                <inventory-list :items="inventory.fuel" @onitemselect="selectFuelItem" />                
            </div>
        </div>
    </div >
</template>

<script>
import { mapGetters, mapMutations, mapState } from 'vuex'
import InventoryList from './InventoryList.vue'
import Horn from './Horn.vue'

export default {
    computed:{        
        ...mapState("gameMetalPlant", ["inventory","furnace", "itemConfigs"]),
        ...mapGetters("localization", ["loc"]),
        oreName(){
            return this.itemConfigs[this.furnace.ore] ? this.itemConfigs[this.furnace.ore].DisplayName : "mplant:res:none";
        },
        fuelName(){
            return this.itemConfigs[this.furnace.fuel] ? this.itemConfigs[this.furnace.fuel].DisplayName : "mplant:res:none";
        }
    },
    components:{
        InventoryList,
        Horn
    },
    methods: {
        selectOreItem(ore){
            this.loadOreToFurnace(ore);
        },
        selectFuelItem(fuel){
            this.loadFuelToFurnace(fuel);
        },
        image(id){
            return this.itemConfigs[id].Image
        },
        ...mapMutations('gameMetalPlant', ['loadFuelToFurnace', 'loadOreToFurnace'])
    }
}
</script>

<style lang="scss">
    .mplant-inv{
        width: 100%;
        height: 100%;
        background-color: rgba(0,0,0,.4);
        display: flex;
        align-items: center;
        justify-content: center;
        &-furnace{
            text-align: center;
            height: 38rem;
            width: 22.3rem;
            position: relative;
            margin: 0 6rem;
            &_bg{
                position: absolute;
                top: -.2rem;
                left: 0;
                opacity: .5;
                width: 100%;
            }
            &_title{
                text-transform: uppercase;
                font-size: 3rem;
                text-align: center;
                color: #ffffff;
                background: url(/img/miniGames/metalPlant/common/title.png) no-repeat center;
                background-size: cover;   
            }
            &_sub{
                letter-spacing: .07rem;
                font-size: 1.1rem;
            }
            &_body{
                width: 22.3rem;
                margin-top: 4rem;
                display: flex;
                align-items: center;
                justify-content: space-between;
            }           
            &_item{
                width: 9.65rem;
                height: 9.65rem;
                background-color: rgba(#fff, .1);
                border: .05rem solid rgba(#fff, .2);
                position: relative;
                display: flex;
                align-items: center;
                justify-content: center;
                margin-top: .5rem;
                img{
                    width: 90%;
                    height: 90%;
                    -webkit-user-drag: none;
                }
            }
            &_horn{
                width: 100%;
                display: flex;
                align-items: center;
                justify-content: center;
            }
        }
        &-block{
            height: 38rem;
            width: 15rem;
            position: relative;
            //border: 1px solid #fff;
            &_title{
                text-transform: uppercase;
                font-size: 3rem;
            }
        }
        &_body{
            width: 100%;
            margin-top: 1rem;
            //border: 1px solid #fff;
        }
    }
</style>