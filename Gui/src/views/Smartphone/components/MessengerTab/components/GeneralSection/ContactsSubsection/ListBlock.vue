<template>
  <div class="list-block">
    <div class="list-block__header">{{ block.category }}</div>
    <div class="block__contacts">
      <list-item
        v-for="(item, index) in block.contacts"
        :key="index"
        :item="item"
      />
    </div>
  </div>
</template>

<script>
import ListItem from './ListItem'

export default {
  name: 'ListBlock',

  components: { ListItem },

  props: { block: Object }
}
</script>

<style lang="scss" scoped>
.list-block {
  display: flex;
  flex-direction: column;
  &__header {
    background: rgba(118, 118, 128, 0.12);
    padding: 0 0 0 1rem;
    font-size: .8rem;
  }
  &__contacts {
    display: flex;
    flex-direction: column;
  }
}
</style>
