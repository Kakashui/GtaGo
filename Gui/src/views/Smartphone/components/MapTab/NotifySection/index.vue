<template>
  <div class="notify-section">
    <div class="notify-section__header">
      <p class="title">Let's go</p>
      <p class="desc">{{loc('phone:gps:notify:1')}}</p>
    </div>
    <img
      src="img/hud/smartphone/mapTab/pinDark.png"
      alt="pin"
      class="notify-section__img"
    >
    <p class="notify-section__desc">{{loc('phone:gps:notify:2')}} <br> {{loc('phone:gps:notify:3')}}</p>
    <div class="notify-section__progress">
      <div class="inner" :style="{ width: progress + '%' }"></div>
    </div>
    <button class="notify-section__btn" @click="setCurrentTab({ key: 'DesktopTab' })">{{loc('phone:gps:notify:4')}}</button>
  </div>
</template>

<script>
import { mapGetters, mapMutations } from 'vuex'

export default {
  name: 'NotifySection',

  data: function() {
    return {
      interval: null,
      progress: 0,
      duration: 3,
    }
  },
  computed: {
    ...mapGetters('localization', ['loc'])
  },
  methods: {
    ...mapMutations('smartphone', ['setCurrentTab']),

    showProgress: function () {
      this.interval = setInterval(() => {
        this.progress++
        if (this.progress === 100) {
          clearInterval(this.interval)
          this.setCurrentTab({ key: 'DesktopTab' })
        }
      }, this.duration * 10)
    }
  },

  mounted: function() {
    this.showProgress()
  },

  beforeDestroy: function() {
    if (this.interval) clearInterval(this.interval)
  }
}
</script>

<style lang="scss" scoped>
.notify-section {
  padding: 3.5rem 1.5rem;
  display: flex;
  flex-direction: column;
  align-items: center;
  height: 100%;
  color: #fff;
  background: center / contain no-repeat url("/img/hud/smartphone/mapTab/notifyBg.png");
  &__header {
    text-transform: uppercase;
    .title {
      font-size: 2rem;
      font-weight: 900;
    }
    .desc {
      font-size: 0.8rem;
      font-weight: 500;
    }
  }
  &__img {
    width: 5.45rem;
    height: 8rem;
    margin: 1.75rem 0 0 0;
  }
  &__desc {
    margin: 2.05rem 0 0 0;
    text-align: center;
    font-size: 0.8rem;
    line-height: 1rem;
  }
  &__progress {
    margin: 2.05rem 0 0 0;
    height: 0.3rem;
    width: 10rem;
    border-radius: 5rem;
    background: rgba(255, 255, 255, 0.4);
    position: relative;
    overflow: hidden;
    .inner {
      position: absolute;
      top: 0;
      left: 0;
      height: 100%;
      background: #fff;
      transition: all 0.03s linear;
    }
  }
  &__btn {
    width: 10rem;
    height: 2.5rem;
    background: #262626;
    border-radius: 5rem;
    color: #fff;
    margin: 2.05rem 0 0 0;
    font-size: 0.7rem;
    font-weight: 500;
  }
}
</style>
