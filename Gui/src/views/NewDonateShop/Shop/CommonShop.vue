<template>
    <div class="newdshop-shop-common">
        <Back @onClick="back"/>
        <div v-if="category === 1">
            <div class="newdshop-shop-common-menu1">
                <div class="newdshop-shop-common-menu1_btn" :class="{'newdshop-shop-common-menu1_selected': gender}" @click="selectGender(true)" @mouseenter="playHover">
                    {{loc("newdshop:shop:btn:male")}}
                </div>
                <div class="newdshop-shop-common-menu1_btn" :class="{'newdshop-shop-common-menu1_selected': !gender}" @click="selectGender(false)" @mouseenter="playHover">
                    {{loc("newdshop:shop:btn:female")}}
                </div>
            </div>
            <div class="newdshop-shop-common-menu2">
                <div class="newdshop-shop-common-menu2_btn" 
                    v-for="(type, index) in types" :key="index"  
                    :class="{'newdshop-shop-common-menu2_selected': currentType === type}"
                    @click="selectType(type)"
                    @mouseenter="playHover"
                >
                    {{loc(`newdshop:shop:cat:${type}`)}}
                </div> 
            </div>
        </div> 
        <div class="newdshop-shop-common-container">
            <Complect v-if="complect" :complect="complect" @onItemSelect="selectItem"/>
            <div v-else>
                 <div class="newdshop-shop-common_header" v-if="category !== 1 && category !== 6">
                    <img :src="`/img/newDonateShop/shop/headers/${category}.png`" alt="header">
                </div>
                <ExclusiveList :items="exclusiveItems" v-if="exclusiveItems.length > 0" @onItemSelect="selectItem"/>               
                <CommonList :items="commonItems" @onItemSelect="selectItem"/>
            </div>
        </div>
        <ClothDialog v-if="dialog" :item="dialog" @onDialogCallback="dialogCallback"/>
    </div>
</template>

<script>
import {mapGetters, mapState} from 'vuex'
import Back from './Back'
import ExclusiveList from './ExclusiveList'
import CommonList from './CommonList'
import ClothDialog from './ClothDialog'
import Complect from './Complect'
import {itemsByCategory, itemById, itemPriceById} from '../../../configs/newDonateShop'

export default {
    props:{
        category: Number
    },
    data() {
        return {
            gender: true,
            currentType: 13,
            dialog: null,
            complect: null,
            items: [],
            filtred:[],
            types:[
                0,                
                11,                
                9,                
                4,                
                13,                
                14,                
                1,                
                7,                
                8
            ],
            exclusive: 6
        }
    },
    computed:{        
        ...mapGetters('localization', ['loc']),
        ...mapState('newDonateShop', ['tryMode', 'coins', 'updatedPrices']),
        exclusiveItems(){
            return this.filtred.filter(i=>i.rarity >= this.exclusive)
        },
        commonItems(){
            return this.filtred.filter(i=>i.rarity < this.exclusive)
        }
    },
    methods: {
        selectGender(gender){
            this.gender = gender;
            this.playClick2();
            this.filterItems();
        },
        selectType(type){
            this.currentType = type;
            this.closeComplect();
            this.playClick2();
            this.filterItems();
        },
        selectItem(item){
            if(item.data.Name === 0 && Array.isArray(item.data.Items) &&  !this.complect){
                const itemList = [];
                item.data.Items.forEach(id => {
                    itemList.push(this.items.find(i=>i.id === id))
                });
                this.complect = {
                    item,
                    items: itemList
                }
            }                
            else
                this.dialog = item;
            this.playClick3();
        },
        back(cat){
            this.$emit("onSelect", cat || 0)
        },
        keyHandler(e){
            if(e.keyCode == 27) {
                if(this.tryMode) this.tryItem();
                else if(this.dialog) this.closeDialog();
                else if(this.complect) this.closeComplect();
                else this.back();
            }
        },
        playHover(){
            this.$store.commit("sounds/play", {"name": "dshop_hover", "volume": .7});
        },
        playClick(){
            this.$store.commit("sounds/play", {"name": "dshop_click", "volume": .7});
        },
        playClick2(){
            this.$store.commit("sounds/play", {"name": "dshop_click2", "volume": .7});
        },
        playClick3(){
            this.$store.commit("sounds/play", {"name": "dshop_click3", "volume": .2});
        },
        filterItems(){
            if(this.category === 1)
                this.filtred = this.items.filter(i=>!i.Exclusive && i.data.Name === this.currentType && i.data.Gender === this.gender);
            else
                this.filtred = this.items.filter(i=>!i.Exclusive);

        },
        dialogCallback(item, action){
            switch (action) {
                case 1:
                    this.buyItem(item);
                    this.closeDialog()
                    break;
                case 2:
                    if(item.category === 1)
                        this.tryItem(item);
                    break;            
                default:
                    break;
            }
        },
        closeDialog(){
            this.dialog = null;
            this.playClick();
        },
        closeComplect(){
            this.complect = null;
            this.playClick();
        },
        buyItem(item){
            if(itemPriceById(item.id, this.updatedPrices) > this.coins) return;
            window.mp.triggerServer("dshop:shop:buy", item.id)
        },
        tryItem(item){
            if(!item){
                this.$store.commit("newDonateShop/tryClothes")
            }else{
                const items = [];
                if(item.data.Name === 0){
                    item.data.Items.forEach(i => {
                        items.push(itemById(i).data);
                    });
                }else items.push(item.data);            
                this.$store.commit("newDonateShop/tryClothes", items);
            }
        }
    },
    components:{
        Back,
        ExclusiveList,
        CommonList,
        ClothDialog,
        Complect
    },
    mounted(){
        this.items = itemsByCategory(this.category);
        document.addEventListener("keyup", this.keyHandler);
        this.filterItems();
    },
    beforeDestroy(){
        document.removeEventListener("keyup", this.keyHandler)
    }
}
</script>


<style lang="scss">
.newdshop-shop-common{
    background: url(/img/newDonateShop/shop/bg.png) repeat;
    width: 100%;
    height: 100%;
    &-menu1{
        display: flex;
        padding-left: 11rem;
        align-items: center;
        letter-spacing: .04rem;
        &_btn{
            height: 4.5rem;
            width: 7.8rem;
            display: flex;
            align-items: center;
            justify-content: center;
            color: rgba(#fff, .6);
            &:hover{
                color: #fff;
            }
        }
        &_selected{
            background: linear-gradient(180deg, rgba(34, 34, 34, 0) 36.06%, rgba(34, 34, 34, 0.6) 100%);
            color: #fff;
        }
    }
    &-menu2{
        display: flex;
        align-items: center;
        padding-left: 12rem;
        border: 1px solid rgba(#fff, .2);
        &_btn{
            margin:0 1rem;
            height: 3.75rem;
            color: rgba(#fff, .5);
            position: relative;
            display: flex;
            align-items: center;
            justify-content: center;
            &:hover{                
                color: #fff;
            }
        }
        &_selected{
            color: #fff;
            &:after{
                position: absolute;
                content: '';
                background-color: #FF0000;
                height: 2px;
                width: 100%;
                left: 0;
                bottom: 0;
            }
        }
    }
    &-container{
        height: 100%;        
        width: 100%;
        margin: auto;
        position: relative;
        padding-bottom: 15rem;
        overflow-y: scroll;
        &::-webkit-scrollbar {
            width: 0;
        }
            &::-webkit-scrollbar-track {
            border: solid 0 transparent;
            margin: .1rem;
        }
            &::-webkit-scrollbar-thumb {
            border: solid 0 transparent;      
            background-color: rgba(#fff,.1);
        }
    }
    &_header{
        z-index: 0;
        width: 100%;
        img{
            border-bottom: 1px solid #111;
            width: 100vw;
        };
    }
}
</style>