<template>
  <div class="preloader-content"></div>
</template>

<script>
import { mapMutations } from 'vuex'

export default {
  name: 'PreloaderContent',

  methods: {
    ...mapMutations('smartphone', ['setColorTheme'])
  },

  mounted: function() {
    this.setColorTheme({ header: 'light', button: 'none'})
  }
}
</script>

<style lang="scss" scoped>
.preloader-content {
  width: 100%;
  height: 100%;
  background: center / cover no-repeat url("/img/hud/smartphone/newsTab/preloader/bg.png");
  display: flex;
  align-items: center;
  justify-content: center;
  &::before {
    content: '';
    width: 12rem;
    height: 6.25rem;
    background: center / cover no-repeat url("/img/hud/smartphone/newsTab/preloader/logo.png");
  }
}
</style>
