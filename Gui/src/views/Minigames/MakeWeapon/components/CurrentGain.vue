<template>
  <div class="current-gain-wrap">
    <div class="current-gain" v-if="!finalDetail">
      <div class="current-gain__units">%</div>
      <div class="current-gain__progress-bar">
        <span v-bind:style="'width: ' + gameProgressNum + '%'">
          <span></span>
        </span>
      </div>
    </div> 
    <div class="gain-complete" v-if="finalDetail">
      {{loc('mw:currgain:end')}}      
    </div>
    <div class="gain-complete-result" v-if="finalDetail">
      <div class="gain-complete-result_text">{{loc('mw:currgain:money')}}</div>
      <div class="gain-complete-result_sum">{{totalWin.toLocaleString()}} $</div>
    </div>
  </div>
</template>

<script>
import { mapState, mapActions, mapGetters } from 'vuex';
export default {
  name: 'CurrentGain',
  computed: {
    ...mapState('gameMakeWeapon/gameState', ['gameProgressNum', 'compleeted', 'gameBonus', 'currentGun']),
    ...mapGetters('gameMakeWeapon/gunsData', ['getGunByIndex']),
    ...mapGetters('localization', ['loc']),
    ...mapState('gameMakeWeapon/pageState', ['finalDetail']),
    totalWin(){
      if(this.compleeted === 0) return 0
      const weapon = this.getGunByIndex(this.currentGun);
      let total = weapon.price * this.compleeted * .25 ;
      if(this.gameBonus > 0) 
        total += this.gameBonus * total * .1;
      return Math.floor(total);
    }
  },
  methods: {
    ...mapActions('gameMakeWeapon/gameState', ['updateBonus']),
  },
};
</script>
<style scope lang="scss">
.current-gain {
  width: 19.5rem;
  margin: 0 auto;
  z-index: -1;
  opacity: 0;
  transition: opacity .2s ease;

  &__units {
    display: block;
    position: absolute;
    left: -1.5rem;
    top: -0.4rem;
    font-size: 1.5rem;
    //font-weight: bold;
    color: #c1e704;
  }

  &__progress-bar {
    width: 19.5rem;
    height: 1rem;
    position: relative;
    margin: 0.4rem auto 0;
    border: 1px solid rgba(255, 255, 255, 0.2);
    & > span {
      display: block;
      position: absolute;
      left: 0;
      top: 0;
      height: 100%;
      background-color: #dcff00;
      overflow: hidden;
      transition: width 0.1s ease;
      &:after {
        content: '';
        position: absolute;
        top: 0;
        right: 0;
        height: 100%;
        width: 3rem;
        background: url('/img/miniGames/makeWeapon/gun-progress-bar-lighting.png') right
          center no-repeat;
        background-size: cover;
        z-index: 5;
      }
      & > span {
        display: block;
        position: absolute;
        top: 0;
        left: 0;
        bottom: 0;
        right: 0;
        background-image: linear-gradient(
          -60deg,
          rgba(0, 0, 0, 0.1) 25%,
          transparent 25%,
          transparent 50%,
          rgba(0, 0, 0, 0.1) 50%,
          rgba(0, 0, 0, 0.1) 75%,
          transparent 75%,
          transparent
        );
        z-index: 1;
        background-size: 18px 25px;
        // animation: move 2s linear infinite;
        border-top-right-radius: 8px;
        border-bottom-right-radius: 8px;
        border-top-left-radius: 20px;
        border-bottom-left-radius: 20px;
        overflow: hidden;
      }
    }
  }

}
.gain-complete {
  width: 100%;
  transition: opacity .2s ease;
  
  font-size: 2.5rem;
  color: #dcff00;
  text-align: center;
  text-transform: uppercase;
  &-result{
    margin-top: .5rem;
    display: flex;
    align-items: center;
    justify-content: center;
    &_text{

      font-size: 1.5rem;
    }
    &_sum{
      margin-left: .6rem;
      color: #dcff00;
      font-size: 1.5rem;
    }
  }
}
</style>
