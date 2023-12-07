<template>
  <button class="button-start" @click="startGame" @mouseenter="playHover">{{loc('mw:btn:start')}}</button>
</template>

<script>
import { mapState, mapActions, mapGetters, mapMutations } from 'vuex';
export default {
  name: 'ButtonStart',
  computed: {
    ...mapState('gameMakeWeapon/pageState', ['profileVisibility']),
    ...mapState('gameMakeWeapon/gameState', ['isGameBegine']),
    ...mapGetters('localization', ['loc'])
  },
  methods: {
    ...mapMutations("gameMakeWeapon/pageState", ["setProfileVisibility"]),
    ...mapActions('gameMakeWeapon/gameState', ['newGame']),
    startGame() {
      //if(this.isGameBegine) return;
      this.setProfileVisibility(false);
      this.newGame();
      this.playClick();
    },
    playHover(){
        this.$store.commit("sounds/play", {"name": "dshop_hover", "volume": .1});
    },
    playClick(){
        this.$store.commit("sounds/play", {"name": "dshop_click", "volume": .1});
    },
  },
};
</script>

<style scoped lang="scss">
.button-start {
  width: 19.2rem;
  height: 3rem;
  background-color: #c1e704;
  border-radius: 0.1rem;
  
  font-size: 1rem;
  font-weight: bold;
  color: #000;
  text-align: center;
  text-transform: uppercase;
  cursor: pointer;
  transition: 0.2s ease;
  margin-top: 1rem;

  &:hover {
    //transform: scale(1.01);
    box-shadow: 0 0 .5rem 0 #c1e704;
    color: #333;
  }
}
</style>
