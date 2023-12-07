<template>
  <div class="settings-tab">
    <transition name="settings-tab">
      <component
        :is="currentTab.section"
        :configuration="configuration"
      />
    </transition>
  </div>
</template>

<script>
import { mapState, mapMutations } from 'vuex'

import GeneralSection from './GeneralSection'
import SecondarySection from './SecondarySection'

export default {
  name: 'SettingsTab',

  components: { GeneralSection, SecondarySection },

  computed: {
    ...mapState('smartphone', ['configuration', 'currentTab']),
  },

  methods: {
    ...mapMutations('smartphone', ['setColorTheme'])
  },

  mounted: function() {
    this.setColorTheme({ header: 'dark', button: 'dark' })
  }
}
</script>

<style lang="scss" scoped>
@keyframes slide {
  from {
    transform: translateX(100%);
  }
  to {
    transform: translateX(0);
  }
}
@keyframes fade {
  from {
    opacity: 1;
  }
  to {
    opacity: 0;
  }
}
.settings-tab {
  position: relative;
  width: 100%;
  height: 100%;
  background: #F3F2F7;
  border-radius: 1.6rem;
  padding: 2.6rem 0 0 0;
  color: #020202;
}
.settings-tab-enter-active {
  animation: slide 0.4s;
  position: absolute;
}
.settings-tab-leave-active  {
  animation: fade 0.2s;
  position: absolute;
}
</style>
