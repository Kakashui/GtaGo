<template>
  <div class="products-list">
    <ProductsListItem
      v-for="(item, index) in itemsList"
      :key="item.id"
      :item="item"
      :index="index"
    />
  </div>
</template>

<script>
import {mapState} from 'vuex'

import ProductsListItem from './ProductsListItem'

export default {
  name: 'ProductsList',

  components: {
    ProductsListItem
  },
  
  computed: {
    ...mapState('roundTheClockShop', ['productsSell']),
    itemsList(){
      return this.productsSell.filter(i=>i.cost > 0)
    }
  }
}
</script>

<style lang="scss" scoped>
  .products-list{
    display: grid;
    grid-auto-rows: 17.1rem;
    grid-template-columns: repeat(4, 15.1rem);
    grid-gap: 3.05rem;
    overflow-y: scroll;
    height: 100%;
    padding-right: 2rem;
    &::-webkit-scrollbar {
      width: 3px; 
    }
    &::-webkit-scrollbar-thumb {
      background: rgba(255, 255, 255, 0.15);
      border-radius: 5px;
    }
  }
</style>
