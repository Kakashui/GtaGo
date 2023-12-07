<template>
    <div class="witem" @click="onclick" :class="{'witem-selected': isCurrent}">
        <img class="witem-img" :src="weapon.getImage()" alt="weapon" :style="{'transform': weapon.isAmmo() ? '' : 'rotate(-30deg)'}">
        <div class="witem-desc">            
            <div class="witem-name">
            {{loc(weapon.getName())}}
            </div>
            <div class="witem-price">
                {{weapon.price}}$
            </div>
        </div>        
        
    </div>
</template>

<script>
import {mapGetters} from 'vuex'

export default {
    props:['weapon', 'index', 'isCurrent'],
    computed:{
        ...mapGetters('localization', ['loc'])
    },
    methods: {
        onclick(){
            this.$emit('onSelect', this.index)
        }
    },
}
</script>

<style lang="scss" scoped>
.witem{
    height: 4rem;
    width: 12rem;
    margin: .5rem;
    background: rgba(#000, 0.15);
    position: relative;
    display: flex;
    align-items: center;
    justify-content: space-between;
    padding-left: 1.2rem;
    border: .1rem solid rgba(#000, 0.15);
    padding: 0 .5rem;
    overflow: hidden;
    &:hover{
        background: rgba(#fff, 0.15);
        border: .1rem solid rgba(#C1E704, .7)
    }
    &-desc{
        height: 80%;
        width: 100%;
        display: flex;
        flex-direction: column;
        justify-content: space-between;
        align-items: flex-end;
    }
    &-img{
        height: 3rem;
        width: 3rem;
        filter: drop-shadow(0px .02rem 1rem #C1E704);        
    }
    &-selected{
       background-color: rgba(0,0,0,.7);
       border: .1rem solid rgba(#C1E704, .7)
    }
    &-name{
        font-size: 1.1rem;
        word-wrap: none;
        white-space: nowrap;
    }
    &-price{
        color:#C1E704;
        font-size: 1.8rem;
    }
}
</style>