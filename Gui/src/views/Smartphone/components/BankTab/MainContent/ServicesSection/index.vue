<template>
  <div class="services-section">
    <transition name="transfer-section">
      <component
        :is="currentTab.subsection"
        :sumModel="sumModel"
        @input="(newSumModel) => {sumModel = newSumModel}"
      ></component>
    </transition>
  </div>
</template>

<script>
import { mapState } from 'vuex'
import GeneralSubsection from './GeneralSubsection'
import BusinessSubsection from './BusinessSubsection'
import HomeSubsection from './HomeSubsection'
import PenaltySubsection from './PenaltySubsection'
import NotifySubsection from './NotifySubsection'

export default {
  name: 'ServicesSection',

  components: {
    GeneralSubsection,
    BusinessSubsection,
    HomeSubsection,
    PenaltySubsection,
    NotifySubsection
  },

  data: function() {
    return {
      sumModel: ''
    }
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
.services-section {
  height: 100%;
}
.transfer-section-enter-active {
  animation: fade 0.4s;
  position: absolute;
}
</style>
