<template>
  <div class="preloader-content">
    <span class="preloader-content__title">Taxi Job</span>
    <p class="preloader-content__desc">{{loc('phone:taxi:job:3')}}</p>
    <div class="preloader-content__main">
      <div class="block-left"></div>
      <div class="block-center"></div>
      <div class="block-right"></div>
    </div>
  </div>
</template>

<script>
import { mapState, mapMutations, mapGetters } from 'vuex'

export default {
  name: 'PreloaderContent',

  computed: {
    ...mapState('smartphone', ['colorTheme']),
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    ...mapMutations('smartphone', ['setColorTheme'])
  },

  mounted: function() {
    this.setColorTheme({ header: this.colorTheme.header, button: 'none' })
  }
}
</script>

<style lang="scss" scoped>
@keyframes slide {
  from {
    transform: translateY(0);
  }
  to {
    transform: translateY(8rem);
  }
}
.preloader-content {
  height: 100%;
  background: center / cover no-repeat url("/img/hud/smartphone/taxiJobTab/bg-main.png");
  text-align: center;
  padding: 5.5rem 0 0 0;
  color: #fff;
  &__title {
    text-transform: uppercase;
    font-size: 4rem;
    line-height: 4rem;
    letter-spacing: 0.04em;
    font-weight: bold;
    text-transform: uppercase;
  }
  &__desc {
    font-size: 0.8rem;
  }
  &__main {
    margin: 1.95rem 0 0 0;
    display: flex;
    .block-left {
      width: 5rem;
      height: 5rem;
      background: rgba(255, 255, 255, 0.2);
      margin: 3rem 0 0 0;
    }
    .block-center {
      width: 3rem;
      height: 3rem;
      background: #fff;
      animation: slide 0.8s linear infinite alternate;
    }
    .block-right {
      width: 3.75rem;
      height: 3.75rem;
      background: rgba(255, 255, 255, 0.5);
      margin: 3rem 0 0 0;
    }
  }
}
</style>
