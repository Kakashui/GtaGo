<template>
  <div class="forbes">
    <main-content v-if="forbesItems.length" :forbesItems="forbesItems" />
    <preloader-content v-else />
  </div>
</template>

<script>
import { mapState } from 'vuex'
import MainContent from './MainContent'
import PreloaderContent from './PreloaderContent'

export default {
  name: 'ForbesTab',

  components: { MainContent, PreloaderContent },

  computed: {
    ...mapState('smartphone/forbesPage', ['forbesItems'])
  }
}
</script>

<style lang="scss" scoped>
.forbes {
  height: 100%;
  background: center / cover no-repeat url("/img/hud/smartphone/forbesTab/layer.png"), #000;
}
</style>
