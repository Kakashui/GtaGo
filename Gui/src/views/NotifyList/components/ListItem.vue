<template>
  <div class="notify">
    <img class="notify_bg" src="/img/hud/notifyList/bg.png" alt="bg">
    <!-- <div class="notify__substrate" :style="{ background: `radial-gradient(54% 147.27% at 50% -47.27%, rgba(${this.item.type.color}, 0.5) 0%, rgba(${this.item.type.color}, 0) 100%)` }"></div> -->
    <div class="notify__circle" :style="{filter: `drop-shadow(0 .5rem 1.5rem rgba(${this.item.type.color}, .2)`}">
      <div class="decorate" :style="{background: `rgb(${this.item.type.color})`}"></div>
      <svg
          xmlns="http://www.w3.org/2000/svg"
          viewBox="0 0 60 60"
          fill="none"
        >
          <circle
            cx="30"
            cy="30"
            stroke-linecap="round"
            stroke-width="4"
            :r="circleRadius"
            :stroke="`rgb(${this.item.type.color})`"
            :stroke-dasharray="calcCircumference"
            :stroke-dashoffset="circleValue"
          />
        </svg>
    </div>
    <div class="notify__main">
      <div class="title">{{ item.type.title }}
      </div>
      <div class="text">{{ message }}</div>
    </div>
  </div>
</template>

<script>
import { mapState, mapMutations, mapGetters } from 'vuex'

export default {
  name: 'Item',

  data: function () {
    return {
      progressWidth: 0,
      duration: this.item.time,
      start: (new Date()).valueOf(),
      message: null,
      interval: null,
      circleRadius: 26
    }
  },

  props: {
    item: Object
  },

  computed: {
    ...mapState('notifyList', ['waitingItems', 'items']),
    ...mapGetters('localization', ['loc']),
    calcCircumference: function () {
      let number = 2 * Math.PI * this.circleRadius
      return  number
    },
    circleValue: function () {
      //const offset = this.calcCircumference
      let maxCircleValuePer = this.calcCircumference - ((this.calcCircumference) / 100) * this.progressWidth
      return maxCircleValuePer
    },
    end: function () {
      return this.start + this.duration
    }
  },

  methods: {
    ...mapMutations('notifyList', [
      'removeItem',
      'notify',
      'removeWaitingItem',
      'addItem'
    ]),
    showProgress: function (start, end) {
      this.interval = setInterval(() => {
        const now = (new Date()).valueOf()

        if (now >= end) {
          this.progressWidth = 100
          clearInterval(this.interval)
        }
        this.progressWidth = ((now - start) / (end - start)) * 100
      }, 1)
    },
    buildMessage(){
      this.message = this.loc(this.item.message);
    }
  },
  beforeDestroy(){
    clearInterval(this.interval);
  },
  mounted: function () {
    const item = this.item;
    this.buildMessage();
    this.showProgress(this.start, this.end);
    setTimeout(() => {
      this.removeItem(item)

      if (this.waitingItems.length > 0) {
        this.addItem(this.waitingItems[0])
        this.removeWaitingItem(this.waitingItems[0])
      }
    }, this.item.time)
  }
}
</script>

<style lang="scss" scoped>
.notify {
  pointer-events: none;
  width: 100%;
  height: 19.2rem;
  position: relative;
  display: flex;
  justify-content: center;
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  margin-bottom: 3.5rem;
  padding-top: 1.5rem;
  &_bg{
    position: fixed;
    top: 0;
    opacity: .5;
  }
  &__circle{
    width: 3rem;
    height: 3rem;
    border-radius: 50%;
    display: flex;
    align-items: center;
    justify-content: center;
    position: relative;
    svg{
      width: 100%;
      height: 100%;
      transform-origin: 50% 50%;
      transform: rotate(-90deg);
    }
    &:after{
      content: '';
      position: absolute;
      width: 1rem;
      height: 1rem;
      background-size: contain;
      background-repeat: no-repeat;
      background-position: center;
      background-image: url('/img/hud/notifyList/whoop.svg');
    }
    .decorate{
      width: 4rem;
      height: 4rem;
      position: absolute;
      filter: blur(2.5rem);
      border-radius: 50%;
    }
  }
  &__main {
    z-index: 20;
    display: flex;
    flex-direction: column;
    align-items: flex-start;
    height: fit-content;
    padding-left: 1rem;
    margin-left: 1rem;
    border-left: .1rem solid rgba(255, 255, 255, 0.3);
    .title {
      font-weight: bold;
      font-size: 1.8rem;
      line-height: 2.05rem;
      letter-spacing: .1rem;
      text-transform: uppercase;
      color: #FFFFFF;
      text-shadow: -.2rem .35rem .5rem rgba(41, 47, 68, 0.25);
      margin-bottom: .4rem;
    }
    .text {
      font-weight: normal;
      font-size: .9rem;
      line-height: 1.2rem;
      letter-spacing: 0.1em;
      text-transform: uppercase;
      color: rgba(255, 255, 255, 1);
      text-shadow: 0 .2rem .2rem rgba(0, 0, 0, 0.25);
      max-width: 19.1rem;
    }
  }
}
</style>
