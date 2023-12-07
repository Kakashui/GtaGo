<template>
  <div :class="[{ active: item.location === rightTab }, 'item']" @click="setCurrentTab(item.location)">
    <div class="item__desc">{{ loc(item.description) }}</div>
    <div class="item__title">{{ loc(item.title) }}</div>
  </div>
</template>

<script>
import { mapMutations, mapGetters } from 'vuex'

export default {
  name: 'HeaderNavigationMenu',

  props: {
    item: Object,
    rightTab: String
  },
  computed:{
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    ...mapMutations('optionsMenu', ['setCurrentTab'])
  }
}
</script>

<style lang="scss" scoped>
.item {
  display: flex;
  flex-direction: column;
  letter-spacing: 0.03em;
  padding: 0 0 1rem 0;
  position: relative;
  &::after {
    transition: all 0.2s;
    content: "";
    position: absolute;
    transform: translate(-50%, -50%) scale(0);
    top: 100%;
    left: 50%;
    height: 0.1rem;
    width: 100%;
    box-shadow: 0px -1rem 2rem 0.2rem #A2BC1F;
    background: #A2BC1F;
    opacity: 0;
  }
  &:not(:last-child) {
    margin: 0 2.5rem 0 0;
  }
  &.active::after {
    transform: translate(-50%, -50%) scale(1);
    opacity: 1;
  }
  &__desc {
    font-size: 0.9rem;
    line-height: 1.1rem;
    color: rgba(255, 255, 255, 0.3);
    margin: 0 0 0.25rem 0;
  }
  &__title {
    font-size: 1.2rem;
    line-height: 1.45rem;
    color: #FFFFFF;
  }
}
</style>
