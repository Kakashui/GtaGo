<template>
  <div class="desktop-tab" :style="{ background: 'center / contain no-repeat url(' + `img/hud/smartphone/settingsTab/wallpaper/${configuration.Wallpaper}.png` + '), #000' }">
    <div class="desktop-tab__apps">
      <apps-item
        v-for="item in getApplications"
        :key="item.id"
        :item="item"
      />
    </div>
    <div class="desktop-tab__main">
      <main-item
        v-for="item in getMainApplications"
        :key="item.id"
        :item="item"
      />
    </div>
  </div>
</template>

<script>
import { mapState, mapMutations } from 'vuex'

import AppsItem from './AppsItem'
import MainItem from './MainItem'

export default {
  name: 'DesktopTab',

  components: { AppsItem, MainItem },

  computed: {
    ...mapState('smartphone', ['configuration', 'phoneBalance']),
    ...mapState('smartphone/appPage', ['applications', 'mainApplications']),
    getApplications: function(){
      return this.applications.filter(item => (item.isInstalled || item.key == 'AppStoreTab') && (!item.forMoney || this.phoneBalance >= 50))
    },
    getMainApplications: function(){
      return this.mainApplications.filter(item => !item.forMoney || this.phoneBalance >= 50)
    }
  },

  methods: {
    ...mapMutations('smartphone', ['setColorTheme'])
  },

  mounted: function() {
    this.setColorTheme({ header: 'light', button: 'light' })
  }
}
</script>

<style lang="scss" scoped>
.desktop-tab {
  width: 100%;
  height: 100%;
  padding: 1.25rem .4rem .5rem .4rem;
  display: flex;
  flex-direction: column;
  justify-content: space-between;
  &__apps {
    padding: 1.5rem .7rem;
    display: grid;
    grid-template-columns: repeat(4, 2.8rem);
    grid-gap: .55rem;
  }
  &__main {
    padding: .75rem;
    display: flex;
    justify-content: space-between;
    background: rgba(255, 255, 255, 0.26);
    backdrop-filter: blur(2.7rem);
    border-radius: 1.4rem;
  }
}
</style>
