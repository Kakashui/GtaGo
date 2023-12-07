<template>
  <div
    class="sale-product"
    :style="{backgroundImage: `url(/img/bar/products/sale/${saleProduct.img})`}"
  >
    <div class="sale-product_price">
      <div class="sale-product_price-sale">-{{saleProduct.sale}}%</div>
      <div class="sale-product_price-final-count">{{finalCount}}</div>
      <div class="sale-product_price-count">{{saleProduct.price}}$</div>
    </div>
    <div 
      class="sale-product-btn"
      @click="buySaleProduct"
    >{{loc('bar_1')}}</div>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
export default {
  name: "SaleProduct",

  props: {
    saleProduct: Object
  },

  computed: {
    ...mapGetters('localization', ['loc']),
    finalCount: function () {
      const price = this.saleProduct.price
      const sale = this.saleProduct.sale
      return price * (1 - (sale / 100))
    }
  },

  methods: {
    buySaleProduct: function() {
      window.mp.triggerServer('alco:bar:buy', this.saleProduct.id)
    }
  }
}
</script>

<style lang="scss" scoped>
.sale-product{
  height: 100%;
  padding: 4.75rem 2rem;
  width: 100%;
  display: flex;
  flex-direction: column;
  align-items: flex-start;
  justify-content: flex-end;
  background-position: center;
  background-size: cover;
  background-repeat: no-repeat;
  &_price{
    display: flex;
    position: relative;
    font-weight: bold;
    font-size: 2rem;
    line-height: 2.3rem;
    text-align: right;
    letter-spacing: 0.04em;
    text-transform: uppercase;
    margin-bottom: 2rem;
    &-sale{
      color: rgba(255,255,255,.7);
      margin-right: 1rem;
    }
    &-final-count{
      color: #2A91C2;
      margin-right: .5rem;
    }
    &-count{
      font-size: 1rem;
      line-height: 1.15rem;
      letter-spacing: 0.04em;
      text-decoration-line: line-through;
      text-transform: uppercase;
      color: #2A91C2;
    }
  }
  &-btn{
    background: rgba(255, 255, 255, 0.24);
    border-radius: .1rem;
    height: 3.3rem;
    width: 11.15rem;
    display: flex;
    align-items: center;
    justify-content: center;
    color: #fff;
    transition: .3s;
    &:hover{
      background: rgba(255, 255, 255, 0.44);
    }
  }
}
</style>
