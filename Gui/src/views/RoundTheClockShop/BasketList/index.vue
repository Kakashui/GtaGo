<template>
  <div class="basket-list">
    <BasketListItem
      v-for="(item, index) in basket"
      :key="item.id"
      :item="item"
      :index="index"
    />
  </div>
</template>

<script>
import {mapState} from 'vuex'

import BasketListItem from './BasketListItem'

export default {
  name: 'BasketList',

  components: {
    BasketListItem
  },

  computed: {
    ...mapState('roundTheClockShop', ['basket']),
  }
}
</script>

<style lang="scss" scoped>
  .basket-list{
    width: 100%;
    display: grid;
    grid-auto-rows: 6.8rem;
    grid-template-columns: repeat(2, 1fr);
    grid-gap: .85rem;
    overflow-y: scroll;
    height: 100%;
    &::-webkit-scrollbar {
      width: 3px; 
    }
    &::-webkit-scrollbar-thumb {
      background: rgba(255, 255, 255, 0.15);
      border-radius: 5px;
    }
  }
</style>
