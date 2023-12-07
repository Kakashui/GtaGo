<template>
    <div class="customiztion">
        <img class="customiztion_shadow" src="/img/customization/common/shadow.png" alt="shadow">
        <MouseButtonsInfo />
        <transition name="parent-frame" mode="out-in">        
            <ParentFrame v-if="currentCategory === 1" :items="items" /> 
        </transition>         
        <div class="customiztion-header">
             <div class="customiztion-header_logo">
                <img src="/img/customization/common/logo.svg" alt="logo">
            </div>
            <div class="customiztion-header_text">
                <div class="customiztion-header_sub">{{loc("customiz:tit:sub")}}</div>
                <div class="customiztion-header_tit">{{loc("customiz:tit:tit")}}</div>
            </div>
        </div>
        <div class="customiztion_body">       
            <NavBar 
                :categories="categories"
                :current="currentCategory" 
                @onSelectRandom="selectRandom" 
                @onSelectCategory="selectCategory"
            />     
            <div class="customiztion-container">
                <Subcategories 
                    class="customiztion-container_subcategories" 
                    :items="items" 
                    :currentItem="currentItem"
                    @onSelectSubcategory="selectSubcategory"
                />
                <component :is="currentTab" :itemData="itemData" class="customiztion-container_body"/>
                <Save/>
            </div>
        </div>
    </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
import MouseButtonsInfo from './ButtonsInfo'
import ParentFrame from './ParentFrame'
import NavBar from './NavigationBar'
import Info from './tabs/Info'
import Parents from './tabs/Parents'
import Dna from './tabs/Dna'
import Hairs from './tabs/Hairs'
import Skin from './tabs/Skin'
import Clothes from './tabs/Clothes'
import Makeup from './tabs/Makeup'
import Subcategories from './navElements/Subcategories'
import Save from './navElements/SaveCustomization'

export default {
    data() {
        return {
            currentCategory: 0,
            currentItem: 0,
            spamProtection: 0
        }
    },
    computed:{
        ...mapGetters("customization", ["categories"]),
        ...mapState("customization", ["gender"]),
        ...mapGetters("localization", ["loc"]),
        currentTab(){
            return this.categories[this.currentCategory].tab
        },
        items(){
            return this.categories[this.currentCategory].items;
        },
        itemData(){
            if(this.items == undefined) return null;
            return this.items[this.currentItem].itemData
        }
    },
    methods: {
        selectRandom(){
            this.randomParents();
            this.randomDna();
            this.randomHairs();
        },
        selectCategory(index){
            this.currentItem = 0;
            this.currentCategory = index;
            window.mp.trigger("customization:camera:switch", index === 6)
        },
        selectSubcategory(index){
            this.currentItem = index;
        },
        randomParents(){
            const parents = this.categories[1];
            if(parents !== undefined){
                parents.items.forEach(parent => {
                    parent.itemData.value = Math.floor(Math.random() * parent.itemData.items.length);
                    window.mp.trigger("customization:update", parent.itemData.tag,  parent.itemData.items[parent.itemData.value]);
                });
            }
        },
        randomDna(){
            const dna = this.categories[2];
            if(dna !== undefined){
                dna.items.forEach(d => {
                    d.itemData.forEach(i=>{
                        const range = [];
                        for (let index = i.min; index <= i.max; index += i.step) {
                            range.push(index);                            
                        }
                        i.value = range[Math.floor(Math.random() * range.length)]
                        window.mp.trigger("customization:update", i.tag,  i.value)
                    })
                });
            }
        },
        randomHairs(){
            const hairs = this.categories[3];
            if(hairs !== undefined){
                hairs.items.forEach(h => {
                    if(!this.gender && !h.itemData.randomFemale)
                        h.itemData.value = -1;
                    else
                        h.itemData.value = Math.floor(Math.random() * h.itemData.items.length)
                    window.mp.trigger("customization:update", h.itemData.tag,  h.itemData.items[h.itemData.value])
                });
            }
        }
    },
    components:{
        MouseButtonsInfo,
        ParentFrame,
        NavBar,
        Info,
        Parents,
        Dna,
        Hairs,
        Skin,
        Clothes,
        Makeup,
        Subcategories,
        Save
    }
}
</script>

<style lang="scss">
.customiztion{
    position: fixed;
    left: 0;
    top: 0;
    height: 100vh;
    width: 100vw;
    text-transform: uppercase;
    color: #fff;
    &_shadow{
        position: absolute;
        left: 0;
        top: 0;
        height: 54rem;
        z-index: -1;
    }
    &-header{
        height: 8rem;
        display: flex;
        align-items: center;
        &_text{
            margin-left: .5rem;
        }
        &_tit{
            font-size: 2.2rem;
            letter-spacing: .06rem;
        }
        &_sub{
            color: #B6D300;
            font-size: 1.2rem;
            letter-spacing: .06rem;
        } 
        &_logo{
            width: 5.5rem;
            display: flex;
            align-items: center;
            justify-content: center;
            img{
                width: 85%;
            }
        }
    }
    &_body{
        height: 46rem;
        display: flex;
        position: relative;
    }
   
    &-container{
        position: relative;
        height:44rem;
        display: flex;
        &_subcategories{
            width: 3.5rem;
        }
        &_body{
            //border: 1px solid rgba(#fff, .3);
            width: 15rem;
            margin-left: 1rem;
            height: 46rem;
        }
    }
    &-tab_delemiter{
        width: 100%;
        margin: 2rem 0;
    }
}
.parent-frame-enter, .parent-frame-leave-to {
    transform: translateY(120%);
    opacity: 0;
}
.parent-frame-enter-to .parent-frame-leave-to{
    transform: translateY(0);
    opacity: 1;
}
.parent-frame-enter-active, .parent-frame-leave-active {
    transition: all .7s;
    opacity: 1;
}

</style>