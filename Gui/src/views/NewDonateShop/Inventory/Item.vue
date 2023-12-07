<template>
    <div class="newdshop-inv-item" @click="select" @mouseenter="playHover">
        <img v-if="item.item" class="newdshop-inv-item_img" :src="`/img/newDonateShop/items/${item.item.image}.png`" alt="item" :style="{'filter': `drop-shadow(0 0 .1rem ${item.item.color})`}">
        <div v-if="item.item" class="newdshop-inv-item_count">x{{item.count}}</div>
        <div v-if="selected" class="newdshop-inv-item_selected"></div>
        <img v-if="item.item && item.sell" src="/img/newDonateShop/header_coin.png" class="newdshop-inv-item_coin" alt="coin">
    </div>
</template>

<script>
export default {
    props:{
        item: Object,
        selected: Boolean
    },
    methods: {
        select(){
            if(this.item.item=== null) return;
            this.$emit("onSelect", this.item)
        },
        playHover(){
            if(this.item.item === null) return;
            this.$store.commit("sounds/play", {"name": "dshop_hover", "volume": .7})
        },
    },
}
</script>

<style lang="scss">
.newdshop-inv-item{
    position: relative;
    width: 4.5rem;
    height: 4.5rem;
    display: flex;
    align-items: center;
    justify-content: center;
    //background: radial-gradient(80.56% 80.56% at 50% 76.11%, rgba(255, 255, 255, 0.15) 0%, rgba(255, 255, 255, 0) 100%);
    background-color: rgba(255, 255, 255, 0.03);
    border: 2px solid rgba(255, 255, 255, 0.1);
    margin: .5rem;
    &:hover{
        border: 2px solid rgba(255, 255, 255, 0.3);
    }
    &_img{
        height: 90%;
        width: 90%;
    }
    &_count{
        position: absolute;
        top: .2rem;
        left: .2rem;
        font-size: .9rem;
        font-weight: 300;
        letter-spacing: .05rem;
    }
    &_selected{
        background: #9FBA1B;
        box-shadow: 0 -.2rem 1.2rem #9FBA1B;
        height: 2px;
        width: 100%;
        position: absolute;
        bottom: 0;
        left: 0;
    }
    &_coin{
        position: absolute;
        right: .2rem;
        top: .2rem;
        width: 1rem;
        height: 1rem;
        filter: drop-shadow(0 0 .2rem #FFF205);
    }
}
</style>