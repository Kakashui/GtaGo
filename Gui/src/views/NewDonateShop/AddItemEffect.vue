<template>
    <div class="newdshop-add" :style="{'top': `${current}px`, 'opacity': opacity, 'color': item.color}">
        +<div class="newdshop-add_count">{{item.count}}</div>
        <img class="newdshop-add_img" 
            :src="`/img/newDonateShop/items/${item.image}.png`" 
            alt="item" 
            :style="{'filter': `drop-shadow(0 0 .5rem ${item.color})`}"
        >
        <div class="newdshop-add_name" :style="{'color': item.color}">{{item.name}}</div> 
    </div>
</template>

<script>
import { mapMutations } from 'vuex';
export default {
    props:{
        item: Object
    },
    data() {
        return {
            interval: -1,
            target: 450,
            current: 50,
            opacity: 1
        }
    },
    methods: {
        handler(){
            this.current++;
            this.opacity -= .01
            if(this.current > this.target || this.opacity < .2)
                this.deleteNewItem();
        },
        ...mapMutations('newDonateShop/inventory', ['deleteNewItem'])
    },
    mounted() {
        this.interval = setInterval(this.handler, 40);
    },
    beforeDestroy(){
        clearInterval(this.interval);
    }
}
</script>

<style lang="scss">
.newdshop-add{
    position: absolute;
    left: 0;
    bottom: 0;
    font-size: 2rem;
    z-index: 1000;
    display: flex;
    align-items: center;
    width: 30rem;
    
    &_img{
        width: 3rem;
        height: 3rem;
        margin: 0 1rem;
    }
}
</style>