<template>
  <div class="news-tab">
    <main-content v-if="newsData"/>
    <preloader-content v-else />
  </div>
</template>

<script>
import { mapState } from 'vuex'
import PreloaderContent from './PreloaderContent'
import MainContent from './MainContent'

export default {
  name: 'NewsTab',

  components: {
    PreloaderContent,
    MainContent
  },

  computed: {
    ...mapState('smartphone/newsPage', ['newsData'])
  }
}
</script>

<style lang="scss" scoped>
.news-tab {
  width: 100%;
  height: 100%;
}
</style>
