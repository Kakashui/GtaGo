<template>
  <div class="info-block-detail" v-bind:style="animationActive?'opacity: 0':''">
    <div class="info-block-detail__title">{{loc('mw:infoblock:tit')}}</div>
    <div class="info-block-detail__text">{{ loc(details[currentDetail - 1].name) }}</div>
  </div>
</template>
<script>
import { mapGetters, mapState } from 'vuex';
export default {
  name: 'InfoBlockDetail',
  computed: {
    ...mapState('gameMakeWeapon/gunsData', ['details']),
    ...mapState('gameMakeWeapon/gameState', ['currentDetail']),    
    ...mapState('gameMakeWeapon/pageState', ['animationActive']),
    ...mapGetters('localization', ['loc'])
  }
};
</script>
<style scoped lang="scss">
.info-block-detail {
  width: 17.5rem;
  height: 7.8rem;
  position: absolute;
  right: 0;
  bottom: 2rem;
  padding: 2.5rem 0 0 2rem;
  background-size: contain;
  background: url('/img/miniGames/makeWeapon/intersect-right.png') center center no-repeat;
  text-align: left;
  &__title {
    margin-bottom: 0.3rem;
    
    font-size: 1rem;
    //font-weight: 700;
    color: #fff;
  }
  &__text {
    
    //font-weight: 700;
    font-size: 1rem;
    color: #dcff00;
  }
}
</style>
