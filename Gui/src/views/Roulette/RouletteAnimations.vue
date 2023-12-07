<template>
  <div class="roulette-animations">
    <RouletteAnimationsItem
      v-for="(item, index) in animList"
      :key="index"
      :item="item"
    />
  </div>
</template>

<script>
import { mapState } from 'vuex'
import RouletteAnimationsItem from './RouletteAnimationsItem'
export default {
  name: 'RouletteAnimations',
  components: {
    RouletteAnimationsItem
  },
  computed: {
    ...mapState('roulette', ['animList'])
  }
}
</script>

<style lang="scss" scoped>
.roulette-animations {
  display: flex;
  flex-direction: column;
  position: absolute;
  right: 2rem;
  top: 50%;
  transform: translateY(-50%);
  &__item{
    margin-bottom: 2rem;
    width: 2.6rem;
    height: 2.6rem;
    position: relative;
    display: flex;
    align-items: center;
    justify-content: center;
    &:last-child{
      margin-bottom: 0;
    }
    .item-bg{
      width: 100%;
      height: 100%;
      border-radius: 50%;
      position: absolute;
      background: rgba(255, 255, 255, 0.1);
      border-color: 2px solid transparent;
    }
    svg{
      fill: #fff;
      position: absolute;
      width: 100%;
      height: 100%;
    }
    &:hover{
      .item-bg{
        background: rgba(3, 27, 40, 0.7);
        border-color: #C1E704;
        box-shadow: 0px 12px 32px rgba(193, 231, 4, 0.65);
      }
      svg{
        fill: #C1E704;
      }
    }
  }
}
</style>
