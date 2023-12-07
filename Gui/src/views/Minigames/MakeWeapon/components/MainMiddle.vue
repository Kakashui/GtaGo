<template>
  <div class="main__middle">
    <div class="currentGunPreview">
      <img v-bind:src="imgPath" v-bind:class="'gun gun' + currentGun" />
    </div>
    <InfoBlockGain />
    <InfoBlockDetail />
  </div>
</template>

<script>
import InfoBlockGain from './InfoBlockGain'
import InfoBlockDetail from './InfoBlockDetail'
import { mapState } from 'vuex';
export default {
  name: 'MainMiddle',
  computed: {
    ...mapState('gameMakeWeapon/gameState', ['currentGun', 'gameProgress']),
    imgPath() {
      let gunNum = 'gun' + this.currentGun;
      let gunPath =
        '/img/miniGames/makeWeapon/guns/' +
        gunNum +
        '/' +
        gunNum +
        '-' +
        this.gameProgress.join('') +
        '.png';
      return gunPath;
    },
  },
  methods: {},
  components: {
    InfoBlockGain,
    InfoBlockDetail,
  }
};
</script>

<style scoped lang="scss">
.main__middle {
  position: relative;
  width: 100%;
  height: 100%;
  display: flex;
  justify-content: center;
  align-items: center;
  // z-index: 5;
  .gun {
    display: block;
    margin: 7rem auto 0;
    z-index: 3;
    &_1 {
      width: 41rem;
    }
    &_2 {
      width: 33.9rem;
    }
    &_3 {
      width: 39.6rem;
    }
    &_4 {
      width: 34.2rem;
    }
    &_5 {
      width: 38.3rem;
    }
    &_6 {
      width: 32.8rem;
    }
    &_7 {
      width: 40rem;
    }
    &_8 {
      width: 41.5rem;
    }
  }
}
</style>
