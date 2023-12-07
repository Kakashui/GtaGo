<template>
  <div class="round-the-clock-shop">
    <div class="block-products">
      <div class="header">
        <div class="desc">Welcome to</div>
        <div class="title">Shop 24/7</div>
      </div>
      <ProductsList/>
    </div>
    <div class="block-basket">
      <div class="header">
        <div class="desc">Cart of</div>
        <div class="title">Goods</div>
      </div>
      <BasketList/>
      <div class="cashbox">
        <div class="cashbox__title">{{loc("shop24_0")}}</div>
        <div class="cashbox__total-amount" :style="{'color': color}">{{basketTotal}}</div>
        <div class="btn btn-buy" @click="buyProducts()">{{loc("shop24_2")}}</div>
        <div class="btn btn-cancel" @click="close">{{loc("shop24_1")}}</div>
      </div>
    </div>
  </div>
</template>

<script>
import {mapMutations, mapState, mapGetters} from 'vuex'

import ProductsList from './ProductsList'
import BasketList from './BasketList'

export default {
  name: 'RoundTheClockShop',

  components: {
    ProductsList,
    BasketList
  },

  computed: {
    ...mapState('roundTheClockShop', ['basket', 'money']),
    ...mapGetters('localization', ['loc']),
    color(){
      return this.basketTotal > this.money ? "#FF0000" : "$FFFFFF";
    },
    basketTotal: function() {
      return this.basket.reduce((total, basketItem) => total + basketItem.cost*basketItem.count, 0)
    }
  },

  methods: {
    ...mapMutations('roundTheClockShop', ['clearBasket']),
    buyProducts: function() {
      if(this.basket.length == 0 || this.basketTotal > this.money) return;
      const items = {};
      this.basket.forEach(item => {
        items[item.product] = item.count;
      });
      this.clearBasket();
      window.mp.trigger('shop24:buy', JSON.stringify(items));
    },
    close(){
      this.clearBasket();
      window.mp.trigger("shop24:close")
    }
  }
}
</script>

<style lang="scss" scoped>
  .round-the-clock-shop{
    position: absolute;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    z-index: 1;
    background: linear-gradient(0deg, rgba(20, 20, 20, 0.25), rgba(20, 20, 20, 0.25)), linear-gradient(252.52deg, #1D1D1D -0.34%, #1A1E29 99.55%);
    display: flex;
    .block-products, .block-basket{
      display: flex;
      flex-flow: column;
      .header{
        display: flex;
        flex-flow: column;
        min-height: 4.95rem;
        .desc{
          text-transform: uppercase;
          font-weight: 300;
          font-size: 1.5rem;
          line-height: 1.5rem;
          margin-bottom: .25rem;
          color: #FFFFFF;
        }
        .title{
          text-transform: uppercase;
          font-style: normal;
          font-weight: bold;
          font-size: 3.2rem;
          line-height: 3.2rem;
          color: #FFFFFF;
        }
      }
    }
    .block-products{
      width: 100%;
      padding: 2.5rem 2.2rem 0 3.4rem;
      .header{
        padding-left: 10px;
        margin-bottom: 36px;
      }
    }
    .block-basket{
      width: 18.7rem;
      min-width: 18.7rem;
      height: 100%;
      padding: 2.5rem 2.5rem 1.2rem 2.5rem;
      background: rgba(2, 3, 7, 0.8);
      .header{
        margin-bottom: 1.5rem;
      }
      .cashbox{
        width: 100%;
        display: flex;
        flex-flow: column;
        align-items: center;
        margin-bottom: 2rem;
        &__title{
          font-size: .9rem;
          line-height: 1.6rem;
          text-align: center;
          color: rgba(255, 255, 255, 0.7);
          white-space: pre;
        }
        &__total-amount{
          text-transform: uppercase;
          font-weight: 300;
          font-size: 1.8rem;
          line-height: 1.8rem;
          text-align: center;
          color: #FFFFFF;
          margin: .35rem 0 .65rem 0;
          &:after{
            content: '$';
          }
        }
        .btn{
          display: flex;
          align-items: center;
          justify-content: center;
          width: 100%;
          background: rgba(255, 255, 255, 0.06);
          border-radius: .15rem;
          font-size: .75rem;
          text-align: center;
          color: rgba(255, 255, 255, 0.8);
          margin-bottom: .85rem;
          height: 2.5rem;
          min-height: 2.5rem;
          &:last-child{
            margin-bottom: 0;
          }
          &:hover{
            background: rgba(162, 188, 31, 0.74);
            box-shadow: 0 .25rem 2rem rgba(193, 231, 4, 0.25);
            border-radius: 2px;
            transition: .5s;
          }
          &.btn-buy{
            font-weight: bold;
          }
        }
      }
    }
  }
</style>
