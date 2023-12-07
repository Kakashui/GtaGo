<template>
  <div class="products-list">
    <products-list-item
      v-for="item in this.productsSell"
      :key="item.key"
      :item="item"
    />
  </div>
</template>

<script>
import { mapState } from 'vuex'
import ProductsListItem from './ProductsListItem'

export default {
  name: 'ProductsList',

  components: {
    ProductsListItem
  },

  computed: {
    ...mapState('illegalShop', ['productsSell'])
  }
}
</script>

<style lang="scss" scoped>
.products-list{
  display: grid;
  grid-template-columns: repeat(5, 10rem);
  grid-auto-rows: 12.35rem;
  grid-row-gap: .95rem;
  grid-column-gap: .85rem;
  overflow-y: auto;
  min-height: 27.7rem;
  max-height: 27.7rem;
  padding-right: .85rem;
  &::-webkit-scrollbar{
    width: .35rem;
    background: rgba(148, 192, 0, 0.1);
    border-radius: .3rem;
  }
  &::-webkit-scrollbar-thumb{
    background: #94C000;
    border-radius: .3rem;
  }
}
</style>
