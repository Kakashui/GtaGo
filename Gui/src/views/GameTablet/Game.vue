<template>
    <div class="minigames-item" @click="startGame">
        <div class="minigames-item-front minigames-item-rot">
            <img :src="`/img/gameTablet/games/${game.img}`"  class="minigames-item-img" alt="img">
        </div>
        <div class="minigames-item-back minigames-item-rot">
            <div class="minigames-item-circle">
                <img src="/img/gameTablet/play.svg" alt="play" class="minigames-item-play">
            </div>
        </div>
        <div class="minigames-item-name">{{game.name}}</div>
    </div>
</template>

<script>
export default {
    props:['game'],
    methods: {
        startGame(){
            this.$store.commit("setPage", this.game.component)
        }
    },
}
</script>

<style lang="scss" scoped>
    .minigames-item{        
        width: 14rem;
        height: 10rem;
        background-color: rgba(#000, .5);
        position: relative;
        //border: 1px solid rgba(#fff, .4);
        border: 1px solid;
        border-image-slice: 1;
        border-width: 1px;
        border-image-source: linear-gradient(to left, rgba(#fff, .4), rgba(#000, .5), rgba(#000, .5), rgba(#fff, .4));
        margin: 2rem 1.1rem;
        perspective: 20rem;
        &-rot{
            width: 10rem;
            height: 10rem;
            position: absolute;
            background-color:#336CFF ;
            backface-visibility: hidden;
            border-radius: 50%;
            left: 2rem;
            top: -2rem;
            transform-origin: center;
            display: flex;
            align-items: center;
            justify-content: center;
            transition: all .5s;
        }
        &-img{
            width: 7rem;
            height: 7rem;
            border-radius: 50%;
            z-index: 2;
            box-shadow: 0 0 2rem 0 #000;
        }
        &-front{            
            transform: rotateY(1);
        }
        &-back{
            transform: rotateY(180deg);            
        }
        &:hover{
            box-shadow: 0 0 1rem 0 #fff;
        }
        &:hover .minigames-item-front{
            transform: rotateY(-180deg);
        }
        &:hover .minigames-item-back{
            transform: rotateY(1deg);
        }
        &-circle{
            width: 7rem;
            height: 7rem;
            border-radius: 50%;
            border: .3rem solid #DCFF00;
            display: flex;
            align-items: center;
            justify-content: center;
        }
        &-play{
            width: 2rem;
            height: 2rem;
        }
        &-name{
            color: #fff;
            position: absolute;
            bottom: .2rem;
            text-align: center;
            width: 100%;
            letter-spacing: .04rem;
        }
    }
</style>