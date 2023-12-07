<template>
     <div class="current-gun-bonus" :style="{'margin-top': `${margin}px`, 'opacity': opacity}">
      <div class="current-gun-bonus_item">+10%</div>
      <div class="current-gun-bonus_item">{{loc('mw:bonus:speed')}}</div>
    </div>
</template>

<script>
import { mapGetters, mapMutations } from 'vuex';
export default {
    data() {
        return {
            interval: 0,
            margin: -10,
            opacity: 1
        }
    },
    computed:{
        ...mapGetters('localization', ['loc'])
    },
    methods:{
        ...mapMutations('gameMakeWeapon/gameState', ['setBonusForTime'])
    },
    mounted(){
        this.interval = setInterval(()=>{
            this.margin -=1.5;
            this.opacity -= .025;
            if(this.margin < -75){
                clearInterval(this.interval);
                this.setBonusForTime(false);
            }
        }, 25)
    }
}
</script>

<style lang="scss">
.current-gun{
    &-bonus{ 
        display: flex;
        align-items: center;
        justify-content: space-between;
        position: absolute; 
        right: 0;
        
        letter-spacing: .04rem;
        &_item{    
        &:nth-child(1){
            font-size: 2rem;
            color: #C1E704;
        }
        &:nth-child(2){
            margin-top: .7rem;
            margin-left: .5rem;
            font-size: 1rem;
        }
        }
    }
}
</style>
