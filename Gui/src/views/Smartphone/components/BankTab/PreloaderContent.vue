<template>
  <div class="preloader-content">
    <span class="preloader-content__title">Mobile finance</span>
    <span class="preloader-content__logo">Bank go</span>
  </div>
</template>

<script>
import { mapMutations } from 'vuex'

export default {
  name: 'PreloaderContent',

  methods: {
    ...mapMutations('smartphone', ['setColorTheme'])
  },

  mounted: function() {
    this.setColorTheme({ header: 'light', button: 'none' })
  }
}
</script>

<style lang="scss" scoped>
.preloader-content {
  height: 100%;
  background: center / contain no-repeat url("/img/hud/smartphone/bankTab/preloaderBg.png");
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  color: #fff;
  font-weight: 300;
  &__title {
    font-size: 0.8rem;
    letter-spacing: 0.2rem;
  }

  &__logo {
    font-size: 2rem;
    letter-spacing: 0.4rem;
    text-transform: uppercase;
  }
}
</style>
