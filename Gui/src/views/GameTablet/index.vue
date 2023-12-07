<template>
    <div class="minigames">
        <div class="minigames-tab">
            <div class="minigames-screen">
                <div class="minigames-tittle">Mini games</div>
                <div class="minigames-list">
                    <Game 
                        v-for="(game, index) in games" :key="index"
                        :game="game"
                    />
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import Game from './Game'
export default {
    data() {
        return {
            games:[
                {name: "Super Mario", img: "mario.png", component: "SuperMario", color: ""},
                {name: "Snake", img: "snake.png", component: "Snake", color: ""},                
                //{name: "Galaxy", img: "mario.png", component: "", color: ""},
            ]
        }
    },
    components:{
        Game
    }
}
</script>

<style lang="scss" scoped>
.minigames{
    position: fixed;
    width: 100vw;
    height: 100vh;
    top: 0;
    left: 0;
    display: flex;
    align-items: center;
    justify-content: center;
    text-transform: uppercase;
    &-tab{
        width: 73rem;
        height: 50.5rem;
        background: url(/img/gameTablet/tab.png) no-repeat center;
        background-size: contain;
        filter: drop-shadow(.3rem .3rem 3rem #000);
        display: flex;
        align-items: center;
        justify-content: center;
    }
    &-screen{
        position: relative;
        width: 65rem;
        height: 40rem;
        margin-top: 2rem;
    }
    &-tittle{
        color: #fff;
        font-size: 3rem;
        letter-spacing: .05rem;
        text-align: center;
    }
    &-list{
        width: 100%;
        height: 37rem;
        margin-top: 2rem;
        display: flex;
        flex-wrap: wrap;
        overflow: hidden scroll; 
        &::-webkit-scrollbar {
            width: 2px;
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
}
</style>