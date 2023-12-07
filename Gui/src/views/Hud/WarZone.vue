<template>
  <div class="war-zone">
    <div class="war-zone__prompt"><span>caps</span></div>
    <div class="war-zone__desc">{{loc('war_zone_0')}}</div>
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
export default {
  name: 'WarZone',
  computed: {
    ...mapState('hud', ['warZone']),
    ...mapGetters('localization', ['loc'])
  }
}
</script>

<style lang="scss" scoped>
.war-zone{
  position: absolute;
  left: .5rem;
  top: 50%;
  display: flex;
  align-items: center;
  color: #FFFFFF;
  text-transform: uppercase;
  &__prompt{
    display: flex;
    align-items: center;
    justify-content: center;
    margin-right: .6rem;
    font-weight: bold;
    font-size: 1.3rem;
    text-transform: uppercase;
    position: relative;
    span{
      padding: .25rem .3rem;
      background: #EE443A;
      box-shadow: .5rem 0 2rem #EE443A;
      border-radius: .2rem;
      z-index: 1;
    }
    &:before{
      position: absolute;
      margin-top: -.5rem;
      content: '';
      background-image: url('/img/war/cartridge.png');
      background-size: contain;
      background-repeat: no-repeat;
      background-position: center;
      height: 5.45rem;
      width: 3.75rem;
      z-index: 0;
    }
  }
  &__desc{
    font-weight: bold;
    font-size: 1rem;
    letter-spacing: 0.1em;
    text-transform: uppercase;
    max-width: 5rem;
  }
}
</style>
