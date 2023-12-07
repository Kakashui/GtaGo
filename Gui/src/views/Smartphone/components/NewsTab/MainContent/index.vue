<template>
  <div class="main-content">
    <component :is="currentTab.section" />
  </div>
</template>

<script>
import { mapState } from 'vuex'
import GeneralSection from './GeneralSection'
import CurrentSection from './CurrentSection'

export default {
  name: 'MainContent',
  
  components: {
    GeneralSection,
    CurrentSection
  },

  computed: {
    ...mapState('smartphone', ['currentTab'])
  }
}
</script>

<style lang="scss" scoped>
.main-content {
  width: 100%;
  height: 100%;
  background: #fff;
  position: relative;
}
.main-content::v-deep .vb > .vb-dragger {
  margin: 0 0.1rem 0 0;
}
</style>
