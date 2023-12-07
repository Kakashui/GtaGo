<template>
    <div class="newdshop-shop">  
        <transition name="dshopsh" mode="out-in">
            <component :is="comp" :category="current" @onSelect="selectCategory" @onCloseMenu="closeMenu"/>     
        </transition>
    </div>
</template>

<script>
import Categories from './Categories'
import CommonShop from './CommonShop'

export default {
    data(){
        return{            
            current: 1,
            comp: "Categories"
        }
    },
    components:{
        Categories,
        CommonShop
    },
    methods: {
        selectCategory(category){
            if(category === 0)
                this.comp = "Categories";
            else                 
                this.comp = "CommonShop";
            this.current = category;
            this.playClick();
        },
        playClick(){
            this.$store.commit("sounds/play", {"name": "dshop_click", "volume": .7})
        },
        closeMenu(){
            this.$emit("onCloseMenu");
        }
    }
}
</script>

<style lang="scss">
    .newdshop-shop{
        width: 100%;
        height: 100%;
       text-transform: uppercase;
        &_back{
            display: flex;
            align-items: center;
            justify-content: center;
            &_img{
                border-radius: 50%;
                width: 2.5rem;
                height: 2.5rem;
                border: 2px solid #B6D300;

            }
        }
        &-list{
            margin-top: 1.5rem;
            width: 100%;
            display: flex;
            flex-wrap: wrap;
        }
    }
    .dshopsh-enter-active, .dshopsh-leave-active {
        transition: all .4s;
        opacity: 1;
    }
    .dshopsh-leave-to, .dshopsh-enter{
        opacity: 0;
    }
</style>