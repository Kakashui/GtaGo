<template>
    <div class="newdshop-shop-back" @click="back" @mouseenter="playHover">
        <div class="newdshop-shop-back_img">
            <img src="/img/newDonateShop/shop/back.svg" alt="back">
        </div>
        {{loc("newdshop:shop:btn:back")}}
    </div>
</template>
<script>
import {mapGetters} from 'vuex'

export default {
    computed:{        
        ...mapGetters('localization', ['loc']),
    },
    methods: {
        back(){
            this.$emit("onClick", 0)
        },
        playHover(){
            this.$store.commit("sounds/play", {"name": "dshop_hover", "volume": .7})
        }
    }
}
</script>

<style lang="scss">
.newdshop-shop-back{
    display: flex;
    align-items: center;
    justify-content: center;
    font-size: 1rem;
    opacity: .6;
    position: absolute;
    left: 3rem;
    top: 1rem;
    z-index: 2;
    &_img{
        border-radius: 50%;
        width: 2.5rem;
        height: 2.5rem;
        border: 2px solid #B6D300;
        display: flex;
        align-items: center;
        justify-content: center;
        margin-right: 1rem;

    }
    &:hover{
        opacity: 1;
    }
}
</style>