<template>
  <div class="shopping-cart">
    <div class="title">{{loc('burgerShop_2')}}</div>
    <div class="shopping-cart__list">
      <shopping-cart-item
        v-for="(item, index) in this.shoppingCart"
        :key="item.id"
        :item="item"
        :index="index"
      />
    </div>
    <div class="total">
      <div class="desc">{{loc('burgerShop_3')}}</div>
      <div class="value">{{shoppingCartTotal}}$</div>
    </div>
    <div class="btn-buy" @click="buyProducts()">{{loc('burgerShop_4')}}</div>
  </div>
</template>

<script>
import { mapState, mapMutations, mapGetters } from 'vuex'

import ShoppingCartItem from './ShoppingCartItem'

export default {
  name: 'ShopItems',

  components: {
    ShoppingCartItem
  },

  computed: {
    ...mapState('burgerShop', ['items', 'shoppingCart']),
    ...mapGetters('localization', ['loc']),
    shoppingCartTotal: function() {
      return this.shoppingCart.reduce((total, shoppingCarProduct) => total + shoppingCarProduct.cost*shoppingCarProduct.count, 0)
    }
  },

  methods: {
    ...mapMutations('burgerShop', ['clearShoppingCart']),
    buyProducts: function() {
      if(this.shoppingCart.length == 0 || this.shoppingCartTotal > this.money) return;
      const items = {};
      this.shoppingCart.forEach(item => {
        items[item.unicName] = item.count;
      });
      this.clearShoppingCart();
      window.mp.trigger('burgerShot::buy', JSON.stringify(items));
    },
  }
  
}
</script>

<style lang="scss" scoped>
.shopping-cart{
  display: flex;
  flex-flow: column;
  align-items: flex-start;
  justify-content: flex-start;
  width: 13rem;
  position: relative;
  .title{
    position: absolute;
    left: 0;
    top: 0;
    transform: translateY(calc( -100% - 0.75rem));
    font-weight: 500;
    font-size: 1.5rem;
    line-height: 1.75rem;
    color: #FFFFFF;
  }
  &__list{
    display: flex;
    flex-flow: column;
    width: 100%;
    overflow-y: auto;
    height: 14.5rem;
    min-height: 14.5rem;
    margin-bottom: 1.25rem;
    padding-right: .5rem;
    margin-right: -.5rem;
    &::-webkit-scrollbar {
      width: .3rem;
      background: transparent;
    }
    &::-webkit-scrollbar-thumb {
      background: rgba(255, 255, 255, 0.06);
      border-radius: .25rem;
    }
  }
  .total{
    display: flex;
    flex-flow: column;
    align-items: flex-start;
    .desc{
      font-size: .9rem;
      line-height: 1.05rem;
      text-align: center;
      color: rgba(255, 255, 255, 0.7);
    }
    .value{ 
      font-size: 1.8rem;
      line-height: 2.1rem;
      color: #FFFFFF;
    }
  }
  .btn-buy{
    display: flex;
    align-items: center;
    justify-content: center;
    width: 100%;
    height: 3rem;
    min-height: 3rem;
    margin-top: 1rem;
    font-weight: 500;
    font-size: 1rem;
    line-height: 1.2rem;
    color: #FFFFFF;
    background: #B6D300;
    border-radius: .1rem;
    transition: .3s;
    &:hover{
      box-shadow: 0 .25rem 2rem rgba(193, 231, 4, 0.25);
      transition: .3s;
    }
  }
}
</style>
