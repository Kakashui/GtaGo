<template>
  <div class="general-section">
    <transition name="general-section" appear>
      <component :is="currentSubsection" :setContactPopupData="setContactPopupData" />
    </transition>
  </div>
</template>

<script>
import { mapState } from 'vuex'

import GeneralSubsection from './GeneralSubsection'
import UserSubsection from '../UserSubsection'

export default {
  name: 'GeneralSection',

  components: { GeneralSubsection, UserSubsection },

  props: {
    setContactPopupData: Function
  },

  computed: {
    ...mapState('smartphone', ['currentTab']),

    currentSubsection: function() {
      return this.currentTab.subsection ? this.currentTab.subsection : 'GeneralSubsection'
    }
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
.general-section {
  height: 100%;
  width: 100%;
}
.general-section-enter-active {
  animation: fade 0.4s;
  position: absolute;
}
</style>
