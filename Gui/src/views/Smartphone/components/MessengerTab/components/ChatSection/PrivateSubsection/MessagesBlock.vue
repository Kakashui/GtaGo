<template>
  <div class="messages-block">
    <span class="messages-block__date">{{ block.date }}</span>
    <div class="messages-block__list">
      <messages-block-item
        v-for="(item, index) in block.items"
        :key="index"
        :item="item"
      />
    </div>
  </div>
</template>

<script>
import MessagesBlockItem from './MessagesBlockItem'

export default {
  name: 'MessagesBlock',

  components: { MessagesBlockItem },

  props: {
    block: Object
  },
}
</script>

<style lang="scss" scoped>
.messages-block {
  margin: 0.75rem 0 0 0;
  &:last-child {
    margin: 0.75rem 0 0.75rem 0;
  }
  &__date {
    text-align: center;
    display: block;
    font-size: 0.65rem;
    line-height: 0.8rem;
    letter-spacing: 0.1rem;
    color: #A7B7EC;
    margin: 0 0 0.25rem 0;
  }
}
</style>
