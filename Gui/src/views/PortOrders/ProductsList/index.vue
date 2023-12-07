<template>
  <div class="products-list">
    <div class="scroll-wrap">
      <products-list-item
        v-for="item in this.productsList"
        :key="item.index"
        :item="item"
      />
    </div>
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
    ...mapState('portOrders', ['productsList'])
  }
}
</script>

<style lang="scss" scoped>
.products-list{
  display: flex;
  max-height: 37.75rem;
  transform-origin: 50% 50%;
  transform: rotateY(180deg);
  overflow-y: auto;
  margin-right: 1.3rem;
  &::-webkit-scrollbar {
    width: .28rem;
    background: rgba(255, 255, 255, 0.3);
    border-radius: .14rem; 
  }
  &::-webkit-scrollbar-thumb{
    background: rgba(255, 255, 255, 1);
    border-radius: .14rem; 
  }
  .scroll-wrap{
    display: grid;
    grid-template-columns: repeat(3, 11.9rem);
    grid-auto-rows: 8.75rem;
    grid-gap: 1rem;
    margin-right: 1rem;
    transform-origin: 50% 50%;
    transform: rotateY(-180deg);
  }
}
</style>
