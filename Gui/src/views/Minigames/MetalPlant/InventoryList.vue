<template>
    <div class="mplant-inv-list">
        <div class="mplant-inv-list-item" v-for="(item, index) in itemList" :key="index">
            <img :src="`/img/inventory/items/${image(item)}.png`" alt="item" v-if="item" @click="select(item)" @mouseenter="playHover">
        </div>
    </div>
</template>

<script>
import { mapState } from 'vuex';
export default {
    props:['items'],
    computed:{
        ...mapState("gameMetalPlant", ["itemConfigs"]),
        itemList(){
            const list = [];
            for (const key in this.items) {
                 const count = this.items[key];
                 for (let index = 0; index < count; index++) {
                     list.push(key);                     
                 }
            }
            while (list.length < 15) {
                list.push(null)
            }
            return list;
        }
    },
    methods: {
        playHover(){
            this.$store.commit("sounds/play", { name: `dshop_hover`, volume: .1});
        },
        playClick(){
            this.$store.commit("sounds/play", { name: `dshop_click`, volume: .1});
        },
        select(item){
            this.playClick();
            this.$emit("onitemselect", item);
        },
        image(id){
            return this.itemConfigs[id].Image
        }
    },
}
</script>

<style lang="scss">
    .mplant-inv-list{
        width: 100%;
        display: flex;
        justify-content: flex-start;
        flex-wrap: wrap;
        &-item{
            display: flex;
            align-items: center;
            justify-content: center;
            position: relative;
            width: 4.5rem;
            height: 4.5rem;
            background-color: rgba(#fff, .1);
            border: .05rem solid rgba(#fff, .2);
            margin-right: .5rem;
            margin-top: .5rem;
            img{
                width: 60%;
                height: 60%;
                -webkit-user-drag: none;
                &:hover{
                    filter: drop-shadow(0 0 1rem rgba(#fff, .5));
                    transform: scale(1.02);
                }
            }
        }
    }
</style>