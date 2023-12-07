<template>
  <div class="user">
    <div class="user__name">sandrexa_Domina</div>
    <div class="user-level">
      <div class="user-level__info">
        <span class="user-level__current-level">{{ userLevel }}</span>
        <span class="user-level__text">LEVEL</span>
        <span class="user-level__progress">{{ userProgress + '%' }}</span>
        <span class="user-level__next-level" v-if="userLevel < 3">{{ userLevel + 1 }}</span>
      </div>
      <div class="user-level__progress-bar">
        <span v-bind:style="'width: ' + userProgress + '%'">
          <span></span>
        </span>
      </div>
    </div>
  </div>
</template>

<script>
import { mapState, mapGetters, mapMutations } from 'vuex';
export default {
  name: 'UserInfo',
  computed: {
    ...mapState('gameMakeWeapon/pageState', ['profileVisibility']),
    ...mapState('gameMakeWeapon/userData', [
      'userName',
      'userExperience',
      'levels'
    ]),
    ...mapGetters('gameMakeWeapon/userData', ['userLevel']),
    userProgress(){
      if(this.levels[this.levels.length - 1].exp < this.userExperience) return 100;
      const lvl = this.userLevel;
      const index = this.levels.findIndex(l=>l.level === lvl);
      if(index === -1 )return 100;
      const from = this.levels[index].exp;
      const to = this.levels[index + 1].exp;
      return Math.floor((this.userExperience - from) / (to -  from) * 100);
    }
  },
  methods: {
    ...mapMutations("gameMakeWeapon/pageState", ["setProfileVisibility"]),
    showProfile() {
      this.setProfileVisibility(true);
    },
  },
};
</script>

<style scoped lang="scss">
.user {
  &__name {
    margin-bottom: 1rem;
    
    font-size: 2rem;
    //font-weight: 700;
    color: #fff;
    text-transform: uppercase;
  }
}
.user-level {
  &__info {
    width: 100%;
    word-wrap: nowrap;
    position: relative;
  }
  &__current-level {
    color: #fff;
    font-size: 1.5rem;
    
    //font-weight: 700;
  }
  &__text {
    font-size: 1.5rem;
    color: rgba(#fff, 0.4);
    
    margin: 0 0.5rem 0 0.3rem;
  }
  &__progress {
    
    font-size: 1.2rem;
    //font-weight: 500;
    line-height: 1.2;
    color: #dcff00;
  }
  &__next-level {
    position: absolute;
    right: 0;
    bottom: 0;
    color: #dcff00;
    font-size: 1.5rem;
    
    //font-weight: 700;
  }
  &__progress-bar {
    width: 100%;
    height: 0.7rem;
    position: relative;
    margin-top: 0.4rem;
    border: 1px solid rgba(255, 255, 255, 0.2);
    & > span {
      display: block;
      position: absolute;
      left: 0;
      top: 0;
      height: 100%;
      background-color: #dcff00;
      overflow: hidden;
      &:after {
        content: '';
        position: absolute;
        top: 0;
        right: 0;
        height: 100%;
        width: 3rem;
        background: url('/img/miniGames/makeWeapon/gun-progress-bar-lighting.png') right center no-repeat;
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
        animation: move 2s linear infinite;
        border-top-right-radius: 8px;
        border-bottom-right-radius: 8px;
        border-top-left-radius: 20px;
        border-bottom-left-radius: 20px;
        overflow: hidden;
      }
    }
  }
}
</style>
