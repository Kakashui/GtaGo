<template>
  <div class="shop-items">
    <products-list-item
      v-for="item in itemsList"
      :key="item.id"
      :item="item"
    />
  </div>
</template>

<script>
import { mapState } from 'vuex'

import ProductsListItem from './ProductsListItem'

export default {
  name: 'ShopItems',

  components: {
    ProductsListItem
  },

  computed: {
    ...mapState('burgerShop', ['items', 'shoppingCart']),
    itemsList(){
      return this.items.filter(i=>i.cost > 0)
    }
  }
  
}
</script>

<style lang="scss" scoped>
.shop-items{
  display: grid;
  grid-template-columns: repeat(4, 14rem);
  grid-auto-rows: 14rem;
  grid-column-gap: 3rem;
  grid-row-gap: 1.5rem;
  padding-right: 2.5rem;
  overflow-y: auto;
  height: 100%;
  &::-webkit-scrollbar {
    width: .3rem;
    background: transparent;
  }
  &::-webkit-scrollbar-thumb {
    background: rgba(255, 255, 255, 0.06);
    border-radius: .25rem;
  }
}
</style>