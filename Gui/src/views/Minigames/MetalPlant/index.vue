<template>
    <div class="mplant" :style="{'background': `url(/img/miniGames/metalPlant/bg/${background}.jpg) no-repeat center`}">
        <component :is="currantState" />
        <Preparation v-if="preparation" />
    </div>
</template>

<script>
import { mapState } from 'vuex'
import Inventory from './Inventory.vue'
import Preparation from './Preparation.vue'
import Game from './Game.vue'
import GameOver from './GameOver.vue'
import GameWin from './GameWin.vue'


export default { 
    computed:{
        ...mapState('gameMetalPlant', ['currantState', 'preparation']),
        background(){
            switch (this.currantState) {
                case "Inventory": return 3;
                case "Game": return 4;
                case "GameOver": return 2;            
                default: return 1;
            }
        }
    },
    components:{
        Inventory,
        Preparation,
        Game,
        GameOver,
        GameWin
    }
}
</script>

<style lang="scss">
    .mplant{
        color: #fff;
        position: absolute;
        top: 0;
        left: 0;
        width: 100vw;
        height: 100vh;
        background-size: cover;
    }
</style>