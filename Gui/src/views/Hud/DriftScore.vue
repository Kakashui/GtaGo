<template>
  <div class="drift-score">
    <div class="drift-score__info">
      <div class="factor"><span>x</span>{{driftScore.factor}}</div>
      <div class="score-wrap">
        <div class="desc">DRIFT SCORE</div>
        <div class="value">{{driftScore.value}}</div>
      </div>
    </div>
    <div class="drift-score__dynamic">
      <div class="arrow" :style="{transform: 'rotate(' + driftScore.rotateDeg + 'deg)'}"></div>
    </div>
  </div>
</template>

<script>
import { mapState } from 'vuex'
export default {
  name: 'DriftScore',
  computed: {
    ...mapState('hud', ['driftScore'])
  }
}
</script>

<style lang="scss" scoped>
.drift-score{
  display: flex;
  flex-flow: column;
  align-items: center;
  justify-content: space-between;
  padding: 5rem 0 3.75rem 0;
  height: 100vh;
  position: absolute;
  top: 0;
  left: 50%;
  transform: translateX(-50%);
  &__info{
    display: flex;
    align-items: flex-start;
    color: #FFFFFF;
    text-transform: uppercase;
    font-style: normal;
      font-weight: bold;
    .factor{
      font-size: 3.2rem;
      line-height: 3.2rem;
      text-shadow: 0 0 1.6rem #C1E704;
      text-transform: initial;
      position: relative;
      margin-right: 1.35rem;
      padding-right: 1.35rem;
      &:after{
        content: '';
        position: absolute;
        right: 0;
        top: 0;
        width: 1px;
        height: 100%;
        background: radial-gradient(50% 50% at 50% 50%, #FFFFFF 0%, rgba(255, 255, 255, 0) 100%);
      }
      span{
        font-size: 2.4rem;
        line-height: 2.4rem;
      }
    }
    .score-wrap{
      display: flex;
      flex-flow: column;
      align-items: flex-start;
      .desc{
        font-size: 1.05rem;
        line-height: 1.05rem;
        margin-bottom: .25rem;
      }
      .value{
        font-size: 1.8rem;
        line-height: 1.8rem;
        color: #C1E704;
      }
    }
  }
  &__dynamic{
    width: 28.05rem;
    height: 28.05rem;
    position: relative;
    display: flex;
    align-items: center;
    justify-content: center;
    background-size: contain;
    background-position: center bottom;
    background-image: url('/img/driftScore/back.svg');
    .arrow{
      width: 100%;
      height: 100%;
      position: absolute;
      background-image: url('/img/driftScore/arrow.svg');
      background-size: contain;
      background-repeat: no-repeat;
      background-position: center bottom;
      transform-origin: 50% 50%;
    }
  }
}
</style>
