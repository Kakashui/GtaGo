<template>
  <div class="basket-list">
    <template v-if="this.basket.length !== 0">
      <div class="basket-list__info">
        <div class="title">{{loc('illegalShop_5')}}</div>
        <div class="products-quantity">{{loc('illegalShop_6')}}: <span>{{sumBasket}}</span></div>
        <div class="money-total">
          <div class="desc">{{loc('illegalShop_2')}}:</div>
          <div class="value">${{basketTotal}}</div>
        </div>
        <div class="btn-buy" @click="buyProducts">{{loc('illegalShop_7')}}</div>
      </div>
      <div class="basket-list__products">
        <basket-list-item 
          v-for="(item, index) in this.basket"
          :key="item.key"
          :item="item"
          :index="index"
        />
      </div>
    </template>
    <template v-else>
      <div class="text-empty">{{loc('illegalShop_8')}}</div>
    </template>
  </div>
</template>

<script>
import { mapGetters } from 'vuex';
import BasketListItem from './BasketListItem'

export default {
  name: 'BasketList',

  components: {
    BasketListItem
  },

  props: {
    basket: Array,
    basketTotal: Number
  },

  computed: {
    ...mapGetters('localization', ['loc']),
    sumBasket: function() {
      let summ = 0
      for (let index = 0; index < this.basket.length; index++) {
        const element = this.basket[index];
        summ = summ + element.count
      }
      return summ
    }
  },

  methods: {
    buyProducts: function() {
      if(this.basket.length == 0) return;
      const items = {};
      this.basket.forEach(item => {
        items[item.product] = item.count;
      });
      window.mp.trigger('illegalShop:buy', JSON.stringify(items));
    }
  }
}
</script>

<style lang="scss" scoped>
.basket-list{
  display: flex;
  align-items: center;
  justify-content: space-between;
  min-height: 27.7rem;
  height: 27.7rem;
  .text-empty{
    display: flex;
    align-items: center;
    justify-content: center;
    font-weight: bold;
    font-size: 1.2rem;
    text-transform: uppercase;
    width: 100%;
    height: 100%;
    margin-bottom: 4rem;
  }
  &__info{
    display: flex;
    flex-flow: column;
    align-items: flex-start;
    justify-content: flex-end;
    font-weight: bold;
    height: 100%;
    width: 12.5rem;
    .title{
      font-size: 1.8rem;
      text-transform: uppercase;
      width: 9.4rem;
      margin-bottom: .6rem;
    }
    .products-quantity{
      font-size: .8rem;
      span{
        color: #94C000;
      }
    }
    .money-total{
      display: flex;
      align-items: flex-start;
      flex-flow: column;
      margin: 1.5rem 0 2rem 0;
      .desc{
        font-size: .7rem;
        color: #94C000;
        margin-bottom: .3rem;
      }
      .value{
        font-size: 1.8rem;
      }
    }
    .btn-buy{
      background: linear-gradient(180deg, rgba(255, 255, 255, 0.5) 0%, rgba(255, 255, 255, 0) 100%), #94C000;
      border-radius: .5rem;
      font-size: 1rem;
      text-transform: uppercase;
      color: #081922;
      width: 100%;
      height: 3.5rem;
      min-height: 3.5rem;
      display: flex;
      align-items: center;
      justify-content: center;
      transition: .2s;
      &:hover{
        transition: .2s;
        transform: scale(1.1);
      }
      &:active{
        transform: scale(1.05);
        transition: .1s;
      }
    }
  }
  &__products{
    height: 100%;
    display: grid;
    grid-template-columns: repeat(1, 34.75rem);
    grid-auto-rows: 4rem;
    grid-row-gap: .85rem;
    height: 100%;
    overflow-y: auto;
    padding-right: 3rem;
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
}
</style>
