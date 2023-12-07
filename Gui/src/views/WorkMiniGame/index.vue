<template>
  <div class="mini-game">
    <div class="mini-game__progress">
      <div class="progress__current" :style="{ width: currentProgress + '%' }"></div>
    </div>
    <div class="mini-game__current-btn" ref="currentBtn">{{ currentButton.title }}</div>
  </div>
</template>

<script>
export default {
  name: 'WorkMiniGame',

  data: function () {
    return {
      buttons: [
        { key: 88, title: 'X' },
        { key: 90, title: 'Z' },
        { key: 80, title: 'P' },
        { key: 71, title: 'G' },
        { key: 75, title: 'K' }
      ],
      currentButton: {},
      currentProgress: 10,
      currentStep: 5,
      isStarted: false,
      gameTimer: null,
      gameInterval: 1000
    }
  },

  methods: {
    push: function(event) {
      console.log(`event ${event.keyCode}`);
      if (event.keyCode === this.currentButton.key && this.isStarted) {
        this.currentProgress += this.currentStep
      } else {
        this.currentProgress -= this.currentStep * 2
      }
    },

    removeClickClass: function() {
      if (event.keyCode === this.currentButton.key) this.$refs.currentBtn.classList.remove('minicircle')
    },

    tick: function() {
      this.currentProgress -= 5
      if (this.currentProgress <= 0 && this.isStarted) this.returnResult(false)
      if (this.currentProgress >= 100 && this.isStarted) this.returnResult(true)
    },

    setGameTimer: function() {
      this.gameTimer = setInterval(() => {
        this.tick()
      }, this.gameInterval)
    },

    returnResult: function (value) {
      this.currentProgress = 0
      this.isStarted = false
      clearInterval(this.gameTimer)
      //alert('Game result: ', value) // SOME EVENT
      window.mp.trigger('WORK::TECHNICIAN::DIAGNOSTIC::RESULT::VUE', value)
      window.mp.trigger('WORK::CARTHIEF::DIAGNOSTIC::RESULT::VUE', value)
    },

    getRandomButton: function() {
      this.currentButton = this.buttons[Math.floor(Math.random() * this.buttons.length)]
    }
  },

  mounted: function() {
    this.setGameTimer()
    this.getRandomButton()
    this.isStarted = true
    window.addEventListener("keydown", this.push)
  },

  beforeDestroy: function() {
    clearInterval(this.gameTimer)
    window.removeEventListener("keydown", this.push)
    this.isStarted = false
  }
}
</script>

<style lang="scss" scoped>
.mini-game {
  display: flex;
  flex-direction: column;
  align-items: center;
  &__progress {
    width: 20rem;
    height: 2rem;
    border-radius: 1rem;
    background-color: rgb(19, 19, 19);
    overflow: hidden;
    .progress__current {
      height: 100%;
      background-color: rgb(50, 177, 0);
      transition: all .1s;
    }
  }
  &__current-btn {
    margin: 4rem 0 0 0;
    background-color: rgb(19, 19, 19);
    width: 3rem;
    height: 3rem;
    border-radius: .2rem;
    font-size: 1.7rem;
    text-transform: uppercase;
    display: flex;
    align-items: center;
    justify-content: center;
    color: #fff;
    position: relative;
  }
}
</style>
