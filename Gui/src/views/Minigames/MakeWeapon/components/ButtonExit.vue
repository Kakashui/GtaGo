<template>
  <div class="button-exit">
    <a class="button-exit__icon" @click.stop="exit" @mouseenter="playHover"></a>
  </div>
</template>

<script>
import { mapMutations } from 'vuex';

export default {
  name: "ButtonExit",
  methods: {
    ...mapMutations("gameMakeWeapon/pageState", ["setProfileVisibility"]),
    ...mapMutations("gameMakeWeapon/gameState", ["stopGame"]),
    exit(){
      this.setProfileVisibility(true);
      this.stopGame();
      window.mp.triggerServer('mg:makeweapon:quit');
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
.button-exit {
  z-index: 10;
  width: 4.8rem;
  height: 4.8rem;
  background: url("/img/miniGames/makeWeapon/btn-ellipse.png");
  background-size: contain;
  padding: 0.7rem;
  float: left;

  &__icon {
    display: block;
    width: 3.4rem;
    height: 3.4rem;
    border-radius: 3.4rem;
    background-color: #08090b;
    background-image: url("/img/miniGames/makeWeapon/exit-icon.png");
    background-position: center center;
    background-repeat: no-repeat;
    background-size: 1.3rem 1.3rem;
    border: 1px solid rgba(255, 255, 255, 0.1);
    cursor: pointer;
    transition: opacity 0.2s ease;

    &:hover {
      opacity: 0.7;
    }
  }
}
</style>
