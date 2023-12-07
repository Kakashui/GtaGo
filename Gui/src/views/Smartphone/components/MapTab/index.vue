<template>
  <div class="map-tab">
    <component :is="currentTab.section"></component>
  </div>
</template>

<script>
import { mapState } from 'vuex'
import GeneralSection from './GeneralSection'
import SecondarySection from './SecondarySection'
import ThirdSection from './ThirdSection'
import NotifySection from './NotifySection'

export default {
  name: 'MapTab',

  components: {
    GeneralSection,
    SecondarySection,
    ThirdSection,
    NotifySection
  },

  computed: {
    ...mapState('smartphone', ['currentTab'])
  }
}
</script>

<style lang="scss" scoped>
@keyframes slide-in {
  from {
    transform: translateY(-100%);
  }
  to {
    transform: translateY(0);
  }
}
.map-tab {
  height: 100%;
  background: center / contain no-repeat url("/img/hud/smartphone/mapTab/bg.png");
  position: relative;
  &::before {
    content: '';
    position: absolute;
    top: 0;
    left: 0;
    width: 100%;
    height: 3.25rem;
    background: linear-gradient(180deg, rgba(0, 225, 0, 0.3) 0%, rgba(0, 225, 0, 0) 100%);
    animation: slide-in 0.8s;
  }
}
</style>
