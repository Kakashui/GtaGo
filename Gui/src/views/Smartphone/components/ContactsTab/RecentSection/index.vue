<template>
  <div class="recent-section">
    <transition name="recent-section" appear>
      <component :is="currentTab.subsection" :setContactPopupData="setContactPopupData" />
    </transition>
  </div>
</template>

<script>
import { mapState } from 'vuex'

import GeneralSubsection from './GeneralSubsection'
import UserSubsection from '../UserSubsection'

export default {
  name: 'RecentSection',

  props: {
    setContactPopupData: Function
  },

  components: { GeneralSubsection, UserSubsection },

  computed: {
    ...mapState('smartphone', ['currentTab'])
  }
}
</script>

<style lang="scss" scoped>
@keyframes fade {
  from {
    opacity: 0;
  }
  to {
    opacity: 1;
  }
}
.recent-section {
  height: 100%;
  width: 100%;
  position: relative;
}
.recent-section-enter-active {
  animation: fade 0.4s;
  position: absolute;
}
</style>
