<template>
  <div class="app-store-tab">
    <div class="app-store-tab__header">
      <p class="desc">{{loc('phone:store:desc')}}</p>
      <p class="title">{{loc('phone:store:tit')}}</p>
    </div>
    <div>
      <div class="app-store-tab__apps">
        <apps-item
          v-for="item in getApplications"
          :key="item.id"
          :item="item"
        />
      </div>
    </div>
  </div>
</template>

<script>
import { mapState, mapMutations, mapGetters } from 'vuex'
import AppsItem from './AppsItem'

export default {
  name: 'AppStoreTab',

  components: { AppsItem },

  computed: {
    ...mapState('smartphone/appPage', ['applications']),
    getApplications: function(){
      return this.applications.filter(item => item.key != 'AppStoreTab')
    },
    ...mapGetters('localization', ['loc'])
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
.app-store-tab::v-deep .vb > .vb-dragger {
  margin: 0 0.25rem 0 0;
}
.app-store-tab {
  height: 100%;
  width: 100%;
  background: #F3F2F7;
  padding: 2.5rem 0 0.75rem 0;
  position: relative;
  display: flex;
  flex-direction: column;
  &::after {
    content: '';
    position: absolute;
    bottom: 0;
    left: 0;
    width: 100%;
    height: 4rem;
    background: linear-gradient(180deg, rgba(243, 242, 247, 0) 0%, #F3F2F7 100%);
  }
  &__header {
    padding: 0 0 0.75rem 1rem;
    .desc {
      font-size: 0.7rem;
      color: #8A8A8E;
    }
    .title {
      font-size: 1.5rem;
      color: #020202;
      line-height: 1.75rem;
      font-weight: bold;
    }
  }
  &__apps {
    padding: 0 0 0 0.75rem;
    height: 24rem;
    position: relative;
    overflow-y: scroll;
    &::-webkit-scrollbar {
      width: 0;
    }
    &::-webkit-scrollbar-track {
      border: solid 0 transparent;
      margin: .1rem;
    }
    &::-webkit-scrollbar-thumb {
      border: solid 0 transparent;      
      background-color: rgba(#fff,.1);
    }
  }
}
</style>
