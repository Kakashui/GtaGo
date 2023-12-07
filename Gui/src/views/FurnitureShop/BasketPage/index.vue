<template>
  <div class="basket-page">
    <div class="basket-page__list">
      <div class="title-wrap">
        <div class="title">{{loc('furnitureShop_2')}}</div>
        <div class="btn btn-clear" @click="clearBasket">{{loc('furnitureShop_15')}}</div>
      </div>
      <basket-list/>
    </div>
    <div class="basket-page__total">
      <div class="title">{{loc('furnitureShop_16')}}</div>
      <div class="info">
        <div class="desc">{{loc('furnitureShop_17')}}:</div>
        <div class="value">$ {{basketTotal}}</div>
      </div>
      <div class="btn btn-pay" @click="buyProducts">{{loc('furnitureShop_18')}}</div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapMutations, mapState } from 'vuex'
import BasketList from './BasketList'
export default {
  name: 'BasketPage',

  components: {
    BasketList
  },

  computed: {
    ...mapState('furnitureShop', ['basket', 'pricePart']),
    ...mapGetters('localization', ['loc']),
    basketTotal: function() {
      return this.basket.reduce((total, basketItem) => total + basketItem.cost * this.pricePart *basketItem.count, 0)
    }
  },

  methods: {
    ...mapMutations('furnitureShop', ['clearBasket']),
    buyProducts: function() {
      if( this.basket.length == 0 ) return;
      const items = []
      this.basket.forEach(item => {
        items.push({key: item.key, count: item.count})
      })
      this.clearBasket()
      window.mp.trigger('furnitureShop:buy', JSON.stringify(items))
    }
  }
}
</script>

<style lang="scss" scoped>
.basket-page{
  display: flex;
  align-items: flex-start;
  justify-content: space-between;
  padding: 6.6rem 10rem 1rem 10rem;
  background: #F2F3F5;
  min-height: 100%;
  width: 100%;
  &__list{
    display: flex;
    position: relative;
    width: 100%;
    .title-wrap{
      position: absolute;
      top: -2rem;
      left: 0;
      width: 100%;
      display: flex;
      align-items: flex-end;
      justify-content: space-between;
      transform: translateY(-100%);
      .title{
        font-weight: bold;
        font-size: 2.2rem;
        line-height: 2.55rem;
        color: #000000;
      }
      .btn-clear{
        font-size: 1rem;
        line-height: 1.15rem;
        color: #7B8288;
      }
    }
  }
  &__total{
    background: #E7EBEE;
    display: flex;
    flex-flow: column;
    padding: 1rem;
    width: 17.9rem;
    min-width: 17.9rem;
    margin-left: 2.5rem;
    .title{
      font-weight: bold;
      font-size: 1rem;
      line-height: 1.4rem;
      color: #000000;
      padding-bottom: 1rem;
      width: 100%;
      text-align: left;
      border-bottom: 1px solid rgba(0, 0, 0, 0.1);
      text-transform: uppercase;
    }
    .info{
      margin: .9rem 0 2rem 0;
      width: 100%;
      display: flex;
      align-items: center;
      justify-content: space-between;
      .desc, .value{
        font-size: 1rem;
        line-height: 1.15rem;
        color: #000000;
      }
    }
    .btn-pay{
      width: 100%;
      padding: .6rem;
      font-size: 1rem;
      line-height: 1.15rem;
      color: #000000;
      background: #53D3C3;
      border-radius: .1rem;
    }
  }
}
</style>
