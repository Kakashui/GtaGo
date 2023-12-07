<template>
  <div class="taxi-job-tab">
    <div class="taxi-job-tab__main">
      <ProfileButton
        :setProfile="setProfile"
      />
      <component :is="currentTab.section" />
    </div>
  </div>
</template>

<script>
import { mapState, mapMutations } from 'vuex'

import GeneralSection from './GeneralSection'
import OrdersSection from './OrdersSection'
import ProfileSection from './ProfileSection'
import NotifySection from './NotifySection'
import ProfileButton from './ProfileButton'

export default {
  name: 'TaxiJobTab',

  components: { 
    GeneralSection, 
    OrdersSection,
    ProfileSection,
    NotifySection,
    ProfileButton
  },

  computed: {
    ...mapState('smartphone', ['currentTab']),
    ...mapState('smartphone/taxiPage', ['taxiJobData'])
  },

  methods: {
    ...mapMutations('smartphone', ['setColorTheme', 'setCurrentTab']),
    
    setProfile: function() {
      switch (this.taxiJobData.stateWork) {
        case ('working'): 
          switch (this.currentTab.section) {
            case ('OrdersSection'): 
              this.setCurrentTab({ key: this.currentTab.key, section: 'ProfileSection', data: null });

              if (!this.taxiJobData.isProfileStatsCached) {
                window.mp.triggerServer("phone::taxijob::requestStats");
              }
              break;
            case ('ProfileSection'): 
              this.setCurrentTab({ key: this.currentTab.key, section: 'OrdersSection', data: null });
              break;
            default:
              console.log('error 1');
              break;
          }
          break;
        default:
          switch (this.currentTab.section) {
            case ('GeneralSection'): 
              this.setCurrentTab({ key: this.currentTab.key, section: 'ProfileSection', data: null });

              if (!this.taxiJobData.isProfileStatsCached) {
                window.mp.triggerServer("phone::taxijob::requestStats");
              }
              break;
            case ('ProfileSection'): 
              this.setCurrentTab({ key: this.currentTab.key, section: 'GeneralSection', data: null });
              break;
            default:
              console.log('error 2');
              break;
          }
          break;
      }
    }
  },

  mounted: function() {
    this.setColorTheme({ header: 'light', button: 'light' })
  }
}
</script>

<style lang="scss" scoped>
.taxi-job-tab {
  height: 100%;
  &__main {
    height: 100%;
    background: center / cover no-repeat url("/img/hud/smartphone/taxiJobTab/bg.png");
  }
}
</style>
