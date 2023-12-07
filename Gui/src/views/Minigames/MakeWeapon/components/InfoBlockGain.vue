<template>
  <div class="info-block info-block-gain">
    <div class="info-block-gain__title">{{loc('mw:info:gain:tit')}}</div>
    <div class="info-block-gain__text">{{ '$ ' + userGain }}</div>
  </div>
</template>
<script>
import {mapState, mapGetters} from 'vuex'
export default {
  name: 'InfoBlockGain',
  computed: {
    ...mapState('gameMakeWeapon/userData', ['userGain']),
    ...mapGetters('localization', ['loc'])
  }
};
</script>
<style scoped lang="scss">
.info-block-gain {
  width: 17.5rem;
  height: 7.8rem;
  position: absolute;
  top: 6.1rem;
  left: 0;
  padding: 2.2rem 0 0 4.5rem;
  background-size: contain;
  background: url('/img/miniGames/makeWeapon/intersect-left.png') center center no-repeat;
  line-height: 1.15;
  text-align: left;
  &__title {
    margin-bottom: 0.5rem;
    
    font-size: 1rem;
    //font-weight: 700;
    //color: #fff;
  }
  &__text {
    //font-weight: 700;
    font-size: 1.5rem;
    color: #dcff00;
  }
}
</style>
