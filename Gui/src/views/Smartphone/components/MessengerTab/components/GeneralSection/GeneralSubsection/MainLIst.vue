<template>
  <div v-bar>
    <div class="main-list">
      <main-list-item
        v-for="(item, index) in items"
        :key="index"
        :item="item"
      />
    </div>
  </div>
</template>

<script>
import MainListItem from './MainListItem'

export default {
  name: 'MainList',

  props: { items: Array },

  components: { MainListItem }
}
</script>

<style lang="scss" scoped>
.main-list {
  height: 20.55rem !important;
}
</style>

<style lang="scss">
.main-list {
  .simplebar-track {
    width: 0.4rem;
  }
  .simplebar-scrollbar::before {
    background: rgba(0, 0, 0, 0.3);
  }
}
</style>
