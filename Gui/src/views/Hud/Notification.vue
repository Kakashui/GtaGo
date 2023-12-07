<template>
  <div class="notification">
    <div class="notification__desc">{{ loc("Alerts_2") }}</div>
    <div class="notification__status">{{ loc("Alerts_1") }}</div>
    <div class="notification__progress">
      <div class="progress-back"></div>
      <div class="progress-line"></div>
    </div>
    <div class="notification__text">{{ loc(notificationText) }}</div>
  </div>
  
</template>

<script>
import { mapState, mapGetters } from 'vuex'
export default {
  name: 'Notifiation',

  computed: {
    ...mapGetters('localization', ['loc']),
    ...mapState('hud', ['notificationText'])
  }
}
</script>

<style lang="scss" scoped>
@keyframes show {
  from {
    opacity: 0;
  }

  to {
    opacity: 1;
  }
}
@keyframes progress {
  from {
    width: 0%;
  }

  to {
    width: 100%;
  }
}
.notification{
  animation-duration: .3s;
  animation-name: show;
  text-transform: uppercase;
  color: #fff;
  text-transform: uppercase;
  font-weight: bold;
  display: flex;
  flex-flow: column;
  position: absolute;
  left: 50%;
  top: 50%;
  transform: translateY(-50%) translateX(-50%);
  width: 18rem;
  &:before{
    content: '';
    width: 15rem;
    height: 30.5rem;
    background: linear-gradient(0deg, rgba(0, 0, 0, 0.4), rgba(0, 0, 0, 0.4));
    filter: blur(10rem);
    position: absolute;
    top: 50%;
    left: 50%;
    transform: translateX(-50%) translateY(-50%);
    z-index: 1;
  }
  &>div{
    z-index: 1;
  }
  &__desc, &__status{
    padding-right: 1.25rem;
  }
  &__desc{
    font-size: 1.5rem;
    line-height: 1.5rem;
  }
  &__status{
    color: #B6D300;
    font-size: 5rem;
    line-height: 5rem;
    -webkit-text-stroke: 1px #DBFF00;
    text-shadow: 0 1rem 5rem rgba(168, 195, 2, 0.7);
    position: relative;
    &:before{
      content: '';
      background-image: url('/img/hud/notification/exclamation.svg');
      background-size: contain;
      background-position: center bottom;
      background-repeat: no-repeat;
      width: 11.75rem;
      height: 10.25rem;
      position: absolute;
      right: 0;
      bottom: 0;
    }
  }
  &__progress{
    width: calc(100% - 1.25rem);
    height: .3rem;
    position: relative;
    overflow: hidden;
    margin-bottom: 1.25rem;
    .progress-back, .progress-line{
      height: 100%;
      position: absolute;
      border-radius: .3rem;
      left: 0;
      top: 0;
    }
    .progress-back{
      background: rgba(255, 255, 255, 0.2);
      width: 100%;
    }
    .progress-line{
      width: 100%;
      background: #FFFFFF;
      animation: 2s progress linear;
    }
  }
  &__text{
    text-align: right;
    letter-spacing: 0.1em;
    font-size: 1rem;
    line-height: 1.2rem;
  }
}
</style>
