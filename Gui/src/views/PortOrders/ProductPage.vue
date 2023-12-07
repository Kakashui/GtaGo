<template>
  <div class="product-page">
    <products-list/>
    <basket-list/>
  </div>
</template>

<script>
import ProductsList from './ProductsList'
import BasketList from './BasketList'
export default {
  name: 'ProductPage',

  components: {
    ProductsList,
    BasketList
  },
}
</script>

<style lang="scss" scoped>
  .product-page{
    display: flex;
  }
</style>
