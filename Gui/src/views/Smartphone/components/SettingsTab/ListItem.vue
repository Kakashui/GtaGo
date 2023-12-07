<template>
  <div class="item" @click="setCurrentTab({ key: currentTab.key, section: item.section, subsection: item.key, data: { title: item.title } })">
    <img
      :src="`img/hud/smartphone/settingsTab/${item.icon}.svg`"
      alt="airplane"
      class="item__icon"
    >
    <span class="item__title">{{ loc(item.title) }}</span>
    <img
      src="img/hud/smartphone/settingsTab/chevronRight.svg"
      alt="arrow"
      class="item__arrow"
    >
  </div>
</template>

<script>
import { mapState, mapMutations, mapGetters } from 'vuex'

export default {
  name: 'ListItem',

  props: { item: Object },

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
@keyframes shake {
  25% {
    transform: translateX(-0.5rem);
  }
  50% {
    transform: translateX(0);
  }
  75% {
    transform: translateX(-0.25rem);
  }
}
.item {
  display: flex;
  align-items: center;
  position: relative;
  &::after {
    content: '';
    position: absolute;
    height: 1px;
    width: 12.35rem;
    top: 0;
    right: 0;
    background: rgba(195, 195, 195, 0.35);
  }
  &:hover {
    .item__icon {
      transform: translateY(-.1rem);
    }
    .item__title {
      transform: translateY(-.1rem);
    }
    .item__arrow {
      animation: shake 0.4s;
    }
  }
  &__icon {
    width: 1.5rem;
    height: 1.5rem;
    transition: transform .15s;
  }
  &__title {
    margin: 0 0 0 .6rem;
    transition: transform .15s;
    font-size: .8rem;
    font-weight: 300;
    letter-spacing: .025rem;
  }
  &__arrow {
    margin: 0 0 0 auto;
    width: .35rem;
    height: .6rem;
  }
}
</style>
