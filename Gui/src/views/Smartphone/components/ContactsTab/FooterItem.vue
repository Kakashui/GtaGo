<template>
  <div :class="[{ active: item.section === currentTab.section }, 'item']" @click="setCurrentTab({ key: currentTab.key, section: item.section, subsection: item.subsection })">
    <div class="item__icon" :style="{ '-webkit-mask': 'url(' + `img/hud/smartphone/contactsTab/${item.key}.svg` + ') no-repeat center / contain' }"></div>
    <span class="item__title">{{ loc(item.title) }}</span>
  </div>
</template>

<script>
import { mapState, mapMutations, mapGetters } from 'vuex'

export default {
  name: 'FooterItem',

  props: {
    item: Object,
    setCurrentSection: Function,
    currentSection: String
  },

  computed: {
    ...mapState('smartphone', ['currentTab']),
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    ...mapMutations('smartphone', ['setCurrentTab'])
  }
}
</script>

<style lang="scss" scoped>
.item {
  display: flex;
  flex-direction: column;
  align-items: center;
  &.active {
    .item__icon {
      background-color: #007AFF;
    }
    .item__title {
      color: #007AFF;
    }
  }
  &__icon {
    width: 1.3rem;
    height: 1.3rem;
    background-color: #AAAAAA;
  }
  &__title {
    color: #8A8A8E;
    font-size: 0.55rem;
    margin: .2rem 0 0 0;
  }
}
</style>
