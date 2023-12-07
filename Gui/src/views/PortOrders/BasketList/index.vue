<template>
  <div class="basket-list">
    <div class="paper-check">
      <div class="header">
        <div class="title">{{loc('PortOrders_7')}}</div>
        <div 
          class="btn-clear"
          @click="clearBasket"
        ></div>
      </div>
      <div class="desc" v-if="this.basket.length === 0">
        <div class="text">{{loc('PortOrders_8')}}</div>
        <div class="icon"></div>
      </div>
      <div 
        v-else 
        class="basket-wrap"
      >
        <basket-list-item 
          v-for="(item, index) in this.basket"
          :key="item.i"
          :item="item"
          :index="index"
        />
      </div>
      <div class="result">
        <div class="result__desc">{{loc('PortOrders_9')}}:</div>
        <div class="result__value">${{basketTotal}}</div>
      </div>
    </div>
    <div
      class="btn-buy"
      @click="buyProducts()"
    >{{loc('PortOrders_10')}}</div>
  </div>
</template>

<script>
import { mapGetters, mapMutations, mapState } from 'vuex'
import BasketListItem from './BasketListItem'

export default {
  name: 'ProductsList',

  components: {
    BasketListItem
  },

  computed: {
    ...mapState('portOrders', ['basket']),
    ...mapGetters('localization', ['loc']),
    basketTotal: function() {
      return this.basket.reduce((total, basketItem) => total + basketItem.cost*basketItem.count, 0)
    }
  },

  methods: {
    ...mapMutations('portOrders', ['clearBasket']),
    buyProducts: function() {
      if( this.basket.length == 0 ) return;
      const items = []
      this.basket.forEach(item => {
        items.push({id: item.id, count: item.count})
        //items[item.id] = item.count
      })
      this.clearBasket()
      window.mp.trigger('portOrders:buy', JSON.stringify(items))
    }
  }
}
</script>

<style lang="scss" scoped>
.basket-list{
  display: flex;
  flex-flow: column;
  width: 16.1rem;
  min-width: 16.1rem;
  .paper-check{
    display: flex;
    flex-flow: column;
    width: 100%;
    height: 27.65rem;
    min-height: 27.65rem;
    padding: 1.1rem 0;
    margin-bottom: .5rem;
    background-image: url('/img/portOrders/paper-check-basket.png');
    background-size: contain;
    background-position: center;
    background-repeat: no-repeat;
    .header{
      display: flex;
      align-items: center;
      justify-content: space-between;
      padding: 0 1em;
      .title{
        font-size: 1rem;
        color: #000000;
        font-weight: bold;
      }
      .btn-clear{
        height: 1.2rem;
        width: 1.2rem;
        background-size: contain;
        background-repeat: no-repeat;
        background-position: center;
        background-image: url('/img/portOrders/basket.svg');
        transition: .2s;
        &:hover{
          transform: scale(1.1);
          transition: .2s;
        }
      }
    }
    .desc{
      display: flex;
      flex-flow: column;
      align-items: center;
      justify-content: flex-start;
      align-self: center;
      height: 100%;
      margin-top: 4rem;
      .text{
        font-weight: bold;
        font-size: 1.1rem;
        line-height: 1.15rem;
        color: rgba(0, 0, 0, 0.4);
        margin-bottom: 1rem;
      }
      .icon{
        width: 7.6rem;
        height: 7.6rem;
        background-image: url('/img/portOrders/empty-basket.svg');
        background-size: contain;
        background-position: center;
        background-repeat: no-repeat;
      }
    }
    .basket-wrap{
      height: 100%;
      overflow-y: auto;
      margin-top: 1.6rem;
      display: flex;
      flex-flow: column;
       &::-webkit-scrollbar {
        width: .3rem;
        background: transparent;
      }
      &::-webkit-scrollbar-thumb{
        background: #D5D5D5;
        border-radius: .2rem; 
      }
    }
    .result{
      display: flex;
      align-items: flex-end;
      justify-content: space-between;
      margin-top: 2.14rem;
      padding: 0 1rem;
      &__desc{
        font-weight: bold;
        font-size: 1rem;
        line-height: 1.15rem;
        text-transform: uppercase;
        color: rgba(0, 0, 0, 0.4);
      }
      &__value{
        text-transform: uppercase;
        font-weight: bold;
        font-size: 1.5rem;
        line-height: 1.5rem;
        text-align: right;
        color: #000000;
      }
    }
  }
  .btn-buy{
    width: 100%;
    height: 3.35rem;
    min-height: 3.35rem;
    background: #48B3EF;
    border-radius: .2rem;
    font-size: 1rem;
    color: #FFFFFF;
    display: flex;
    align-items: center;
    justify-content: center;
  }
}
</style>
