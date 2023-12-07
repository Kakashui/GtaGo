<template>
  <div :class="[{ active: item.key === currentFilter }, 'navigation-item']" @click="setFilter">
    <span class="navigation-item__text">{{ loc(item.text) }}</span>
    <transition name="bar">
      <div class="navigation-item__bar" v-show="item.key === currentFilter"></div>
    </transition>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
export default {
  name: 'NavigationItem',

  props: { item: Object, currentFilter: String },
  computed: {
    ...mapGetters('localization', ['loc'])
  },
  methods: {
    setFilter: function() {
      this.$emit('setCurrentFilter', this.item.key)
    }
  }
}
</script>

<style lang="scss" scoped>
.navigation-item {
  position: relative;
  padding: 0 0 0.5rem 0;
  margin: 0 0 0 0.75rem;
  &:first-child {
    margin: 0;
  }
  &.active {
    .navigation-item__text {
      color: #007AFF;
    }
  }
  &__text {
    font-size: 0.7rem;
    line-height: 0.8rem;
    color: #9E9EA2;
    transition: color .15s;
  }
  &__bar {
    display: block;
    height: 0.15rem;
    width: 100%;
    background: #007AFF;
    box-shadow: 0px 0.15rem 0.5rem rgba(0, 122, 255, 0.25);
    position: absolute;
    bottom: 0;
    left: 0;
  }
}
.bar-enter-active, .bar-leave-active {
  transition: transform .15s;
}
.bar-enter, .bar-leave-to {
  transform: scale(0);
}
</style>
