<template>
  <div class="hud-tip"
       :style="{ '--rightOffset': this.rightOffset + '%', '--bottomOffset': this.bottomOffset + '%', '--width': this.width + '%' }">
    <div class="tip-background">
      <img class="tip-img" src="/img/hud/tip.png"/>
    </div>
    <div class="tip">
      <span class="tip__header">{{ loc('tip_0') }}</span>
      <span class="tip__text">{{ loc(tip.text) }}</span>  
    </div>
  </div>
</template>
<script>
import { mapState, mapGetters } from 'vuex'

export default {
  computed: {
    ...mapState('hud', ['tip']),
    ...mapGetters('localization', ['loc']),
  },
  
  props: [
      'bottomOffset',
      'width',
      'rightOffset'
  ]
}
</script>

<style lang='scss' scoped>
@keyframes fromLeft{
  from{
    transform: translateY(0%) translateX(-50%) scaleX(0);
  }
  to{
    transform: translateY(0%) translateX(0%) scaleX(1);
  }
}
.hud-tip {
  position: absolute;
  justify-content: stretch;
  align-items: stretch;
  display: flex;
  animation: fromLeft .1s;
  bottom: var(--bottomOffset);
  left: var(--rightOffset);

  width: 18rem;
  padding-right: 2rem;
  background: linear-gradient(90deg, rgba(2, 3, 16, 0.7) 0%, rgba(2, 3, 16, 0) 100%);
  text-transform: uppercase;
  font-weight: bold;
  border-left: 2px solid #B6D300;
}
.tip-img {
  margin-left: 1.2rem;
  margin-right: 1.2rem;
  top: auto;
}
.tip-background {
  justify-content: stretch;
  align-items: center;
  display: flex;
  left: 0;
  background-image: url('/img/hud/tipbg.png');
  background-size: cover;
  background-position: center;
  background-repeat: no-repeat;
}
.tip {
  padding-top: 1.5rem;
  padding-bottom: 1.5rem;
  display: flex;
  flex-flow: column;
  &__header {
    float: left;
    color: #B6D300;
    font-size: .8rem;
    line-height: .8rem;
  }
  &__text {
    line-height: 1.1rem;
    font-size: .9rem;
    color: #fff;
  }
}
</style>
