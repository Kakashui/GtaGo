<template>
  <div class="button-profile">
    <a class="button-profile__icon" @click.stop="showProfile" @mouseenter="playHover"></a>
    <div class="button-profile__wrong">
      {{loc('mw:btn:profile:warn')}}     
    </div>
  </div>
</template>

<script>
import { mapState, mapMutations, mapGetters } from "vuex";
export default {
  name: "ButtonProfile",
  computed: {
    ...mapState("gameMakeWeapon/pageState", ["profileVisibility"]),
    ...mapGetters('localization', ['loc'])
  },
  methods: {
    ...mapMutations("gameMakeWeapon/pageState", ["setProfileVisibility"]),   
    ...mapMutations("gameMakeWeapon/gameState", ["stopGame"]),
    showProfile() {      
      this.stopGame();
      this.setProfileVisibility(true);
      this.playClick();
    },
    playHover(){
        this.$store.commit("sounds/play", {"name": "dshop_hover", "volume": .1});
    },
    playClick(){
        this.$store.commit("sounds/play", {"name": "dshop_click", "volume": .1});
    }
  },
};
</script>

<style scoped lang="scss">
.button-profile {
  z-index: 2;
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
    background-image: url("/img/miniGames/makeWeapon/profile-icon.png");
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

  &__wrong {
    opacity: 0;
    display: block;
    position: absolute;
    top: 5.2rem;
    width: 21rem;
    padding: 1rem 1.2rem;
    background-color: rgba(#f00, 0.4);
    border: 0.2rem solid #f00;
    transition: opacity 0.2s ease;
    z-index: 2;
    font-size: 0.9rem;
    color: #fff;
    text-align: left;
  }

  &__icon:hover ~ &__wrong {
    opacity: 1;
  }
}
</style>
