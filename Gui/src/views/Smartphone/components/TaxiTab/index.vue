<template>
  <div class="taxi-tab">
    <preloader-content v-if="!taxiData"/>
    <div class="taxi-tab__main" v-else>
      <component :is="currentTab.section" />
    </div>
  </div>
</template>

<script>
import { mapState, mapMutations } from 'vuex'

import GeneralSection from './GeneralSection'
import SecondarySection from './SecondarySection'
import SearchSection from './SearchSection'
import NotifySection from './NotifySection'
import PreloaderContent from './PreloaderContent'

export default {
  name: 'TaxiTab',

  components: { 
    GeneralSection, 
    SecondarySection,
    SearchSection,
    NotifySection,
    PreloaderContent
  },

  computed: {
    ...mapState('smartphone', ['currentTab']),
    ...mapState('smartphone/taxiPage', ['taxiData'])
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
.taxi-tab {
  height: 100%;
  &__main {
    height: 100%;
    background: center / cover no-repeat url("/img/hud/smartphone/taxiTab/bg.png");
  }
}
</style>
