<template>
  <div class="preloader-content">
    <div class="preloader-content__title">Forbes</div>
    <div class="preloader-content__subtitle">Money is the main thing</div>
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
@import url('https://fonts.googleapis.com/css2?family=Playfair+Display:wght@400;700&display=swap');

.preloader-content {
  height: 100%;
  background: center / contain no-repeat url("/img/hud/smartphone/forbesTab/preloaderBg.png");
  color: #fff;
  padding: 4.5rem 0 0 0;
  text-align: center;
  &__title {
    font-family: 'Playfair Display', serif;
    font-size: 3rem;
    font-weight: 700;
    text-transform: uppercase;
    letter-spacing: 0.05rem;
  }
  &__subtitle {
    font-size: 0.8rem;
    font-weight: 300;
    text-transform: uppercase;
  }
}
</style>
