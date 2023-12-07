<template>
  <div class="create-section">
    <component :is="currentTab.subsection"></component>
  </div>
</template>

<script>
import { mapState } from 'vuex'

import GeneralSubsection from './GeneralSubsection'
import GroupFirstSubsection from './GroupFirstSubsection'
import GroupSecondSubsection from './GroupSecondSubsection'

export default {

  components: {
    GeneralSubsection,
    GroupFirstSubsection,
    GroupSecondSubsection
  },

  computed: {
    ...mapState('smartphone', ['currentTab'])
  },
}
</script>

<style lang="scss" scoped>
.create-section {
  width: 100%;
  height: 100%;
}
</style>
