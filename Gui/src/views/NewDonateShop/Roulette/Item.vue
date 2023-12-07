<template>
    <div class="newdshop-roull-item" 
        :style="{'border-color': item.color}" 
        @mouseenter="mouseenter(item.color)"
        @mouseleave="mouseenter()"
        @click="onClick"
    >
        <img  class="newdshop-roull-item_img" :src="image" alt="item" :style="{'filter':`drop-shadow(0 0 .2rem ${shadow})`}">
        <img class="newdshop-roull-item_effect" v-if="item.effect" :src="`/img/newDonateShop/roulette/effects/${item.effect}.gif`" alt="effect">
    </div>
</template>

<script>
export default {
    props: {
        item: Object,
        hover: Boolean
    },
    data() {
        return {
            shadow: "#000"
        }
    },
    computed:{
        image(){
            return this.hover ? `/img/newDonateShop/roulette/types/${this.item.id}.png` : `/img/newDonateShop/items/${this.item.image}.png`;
        }
    },
    methods: {
        mouseenter(color){
            if(!this.hover) return
            this.shadow = color || '#000';
            this.playHover();
        },
        onClick(){
            this.$emit("onClick");
        },
        playHover(){
            this.$store.commit("sounds/play", {"name": "dshop_hover", "volume": .7})
        },
    },
    mounted() {
        if(!this.hover)
            this.shadow = this.item.color
    }
}
</script>

<style lang="scss">
    .newdshop-roull-item{
        border-radius: 50%;
        background: url(/img/newDonateShop/roulette/itemBg.png) no-repeat center;
        background-size: cover;
        display: flex;
        align-items: center;
        justify-content: center;
        border-width: 1%;
        border-style: solid;
        overflow: hidden;
        position: relative;
        &_img{
            position: absolute;
            z-index: 0;
            width: 100%;
            height: 100%;
        }
        &_effect{
            z-index: 1;
            height: 90%;
            width: 90%;
            mix-blend-mode: screen;
        }
    }
</style>