<template>
  <div class="information-tab">
    <div class="information-tab__side">
      <div class="title">{{loc("mm_info_tittle")}}</div>
      <div class="subtitle">{{loc("mm_info_subtittle")}}</div>
      <div class="navigation">
        <navigation-item
          v-for="(item, index) in navItems"
          :key="index"
          :item="item"
        />
      </div>
    </div>
    <div class="information-tab__main">
      <component :is="currentSection" />
    </div>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
import NavigationItem from './common/NavigationItem'
import WorksSection from './sections/WorksSection'

export default {
  name: 'InformationTab',

  components: {
    NavigationItem,
    WorksSection
  },
  computed:{
    ...mapGetters('localization', ['loc'])
  },
  data: function() {
    return {
      currentSection: 'WorksSection',
      navItems: [
        { title: 'mm_info_title', subtitle: 'mm_info_sub'},
        {},
        {}
      ]
    }
  }
}
</script>

<style lang="scss" scoped>
.information-tab {
  display: flex;
  padding: 3.55rem 0 0 0;
  &__side {
    display: flex;
    flex-direction: column;
    margin: 0 6.5rem 0 0;
    .title {
      font-size: 5rem;
      line-height: 5rem;
      color: #fff;
      margin: 0 0 0.5rem 0;
      width: 25rem;
    }
    .subtitle {
      font-size: 0.9rem;
      letter-spacing: 0.03em;
      line-height: 1.2rem;
      color: rgba(255, 255, 255, 0.5);
      width: 25rem;
      margin: 0 0 2rem 0;
    }
    .navigation {
      display: flex;
      flex-direction: column;
    }
  }
}
</style>
