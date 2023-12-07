<template>
  <div class="level-up">
    <div class="level-up__title">Payday exp</div>
    <div class="level-up__info">
      <div class="desc">{{loc('hud:lvlup:1')}}</div>
      <div class="value">{{levelUp.currentLevel}}</div>
      <div class="level-progress">
        <div class="level-progress__line" :style="{width: lineExp + '%'}">
        </div>
      </div>
      <div class="value">{{levelUp.currentLevel + 1}}</div>
      <div class="desc">{{loc('hud:lvlup:2')}}</div>
    </div>
    <div class="level-up__stats">EXP: {{levelUp.currentExp}}/{{levelUp.maxExp}}</div>
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
export default {
  name: 'LevelUp',

  data: function() {
    return{
      currentLine: 0,
      timer: null
    }
  },

  methods: {
    startTimer() {
      this.timer = setInterval(() => {
        this.currentLine++
      }, 50)
    },
    stopTimer() {
      clearTimeout(this.timer)
      window.mp.trigger('exp:timerStoped')
    },
  },

  computed: {
    ...mapState('hud', ['levelUp']),
    ...mapGetters('localization', ['loc']),
    lineExp: function() {
      return (this.currentLine/this.levelUp.maxExp)*100
    }
  },

  watch: {
    currentLine(time) {
      if (time === this.levelUp.currentExp) {
        this.stopTimer()
      }
    }
  },

  mounted(){
    this.currentLine = this.levelUp.currentExp - this.levelUp.exp
    this.startTimer()
  },

  destroyed() {
    this.stopTimer()
  },
}
</script>

<style lang="scss" scoped>
@keyframes fromUp{
  from{
    transform: translateY(-100%) translateX(-50%);
  }
  to{
    transform: translateY(0%) translateX(-50%);
  }
}
@keyframes levelUp{
  from{
  }
  to{
  }
}

.level-up{
  display: flex;
  align-items: center;
  flex-flow: column;
  position: absolute;
  top: 0;
  left: 50%;
  transform: translateX(-50%);
  padding-top: 2rem;
  animation: fromUp .5s;
  text-transform: uppercase;
  color: #fff;
  z-index: 0;
  &:before{
    position: absolute;
    top: 0;
    content: '';
    width: 50.5rem;
    height: 25.25rem;
    background-size: contain;
    background-repeat: no-repeat;
    background-position: top;
    background-image: url('/img/levelUp/bg.png');
    z-index: -1;
  }
  &__title{
    font-weight: bold;
    font-size: 1.9rem;
    line-height: 1.9rem;
    margin-bottom: .25rem;
  }
  &__info{
    display: flex;
    align-items: center;
    margin-bottom: .55rem;
    .desc{
      font-size: .8rem;
      line-height: .95rem;
      letter-spacing: 0.1em;
      color: rgba(255, 255, 255, 0.7);
      &:first-child{
        margin-right: .5rem;
      }
      &:last-child{
        margin-left: .5rem;
      }
    }
    .value{
      font-size: 1.9rem;
      line-height: 1.9rem;
      &.current{
        color: rgba(255, 255, 255, 0.7);
      }
    }
    .level-progress{
      display: flex;
      align-items: center;
      justify-content: center;
      margin: 0 1rem;
      width: 16rem;
      height: 1rem;
      position: relative;
      background: rgba(255, 255, 255, 0.2);
      &:before, &:after{
        content: '';
        width: 1.5rem;
        height: 1.6rem;
        box-sizing: border-box;
        border-top: .1rem solid rgba(255, 255, 255, 0.2);
        border-bottom: .1rem solid rgba(255, 255, 255, 0.2);
        position: absolute;
      }
      &:before{
        left: -.25rem;
      }
      &:after{
        right: -.25rem;
      }
      &__line{
        position: absolute;
        left: 0;
        top: 0;
        height: 100%;
        background-size: cover;
        background-repeat: no-repeat;
        background-position: left;
        background-image: url('/img/levelUp/line.png');
        box-shadow: 0 .5rem 2.5rem rgba(168, 195, 2, 0.6);
        transition: .2s;
        &:after{
          content: '';
          width: .8rem;
          height: .45rem;
          position: absolute;
          right: 0;
          bottom: -.15rem;
          transform: translateX(50%) translateY(100%);
          background-size: contain;
          background-repeat: no-repeat;
          background-position: center;
          background-image: url('/img/levelUp/arr.svg');
        }
      }
    }
  }
  &__stats{
    font-size: .8rem;
    line-height: .95rem;
    text-align: center;
    letter-spacing: 0.1em;
    text-transform: uppercase;
    font-weight: 300;
  }
}
</style>
