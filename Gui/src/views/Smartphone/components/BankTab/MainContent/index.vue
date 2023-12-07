<template>
  <div class="main-content">
    <transition name="main-content">
      <component :is="currentTab.section" />
    </transition>
  </div>
</template>

<script>
import { mapState } from 'vuex'
import GeneralSection from './GeneralSection'
import TransferSection from './TransferSection'
import ServicesSection from './ServicesSection'

export default {
  name: 'MainContent',

  components: {
    GeneralSection,
    TransferSection,
    ServicesSection
  },

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
.main-content {
  height: 100%;
  position: relative;
  background: center / contain no-repeat url("/img/hud/smartphone/bankTab/mainBg.png"), linear-gradient(0deg, #EBEFFA, #EBEFFA), linear-gradient(0deg, #FFFFFF, #FFFFFF), #F1F3F8;
}
.main-content-enter-active {
  animation: fade 0.4s;
  position: absolute;
}
</style>
