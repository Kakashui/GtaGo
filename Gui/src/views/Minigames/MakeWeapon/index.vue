<template>
  <div class="makeWeapon">
    <Main />
    <ModalHelp v-if="helpVisibility"/>
    <FinalAnimation />
  </div>
</template>

<script>
import Main from './components/Main';
import ModalHelp from './components/ModalHelp';
import FinalAnimation from './components/FinalAnimation';
import { mapState, mapMutations } from 'vuex';
export default {
  name: 'app',
  computed: {
    ...mapState('gameMakeWeapon/pageState', [
      'profileVisibility',
      'helpVisibility',
      'finalDetail',
    ])
  },
  methods: {
    ...mapMutations("gameMakeWeapon/pageState", ["setProfileVisibility","setAnimationActive"]),
    ...mapMutations("gameMakeWeapon/gameState", ["stopGame"]),
    keyHandler(e){
      if(e.keyCode === 27){
        this.stopGame();
        this.setProfileVisibility(true);
        this.setAnimationActive(false);
        window.mp.triggerServer("mg:makeweapon:quit");
      }
    }
  },
  components: {
    Main,
    ModalHelp,
    FinalAnimation,
  },
  mounted(){
    addEventListener("keyup", this.keyHandler);
  },
  beforeDestroy(){
    removeEventListener("keyup", this.keyHandler);
  }
};
</script>

<style scoped lang="scss">
.makeWeapon{
  color: #fff;
  position: fixed;
  left: 0;
  top: 0;
  width: 100vw;
  height: 100vh;
}
.modal-bg {
  // display: none;
  width: 100vw;
  height: 100vh;
  position: fixed;
  top: 0;
  left: 0;
  background: rgba(#111, 0.7);  
  z-index: -1;
  opacity: 0;
  transition: opacity 0.2s ease;
  &.active {
    z-index: 4;
    opacity: 1;
  }
  &.transparent {
    background: #0da;
    background: transparent;
    z-index: 10;
    opacity: 1;
  }
}

</style>
