<template>
  <div class="business-item">
    <div class="title">
      <span class="title-text">{{ loc(item.Name) }}</span>
      <button @click="deleteItem" class="title-button">{{loc('biz:menu:item:del')}}</button>
    </div>
    <div class="price">
      <div class="price__item order">
        <div class="text">
          <span class="text__info">{{loc('biz:menu:item:price')}}</span>
          <span class="text__value">
            {{ item.OrderPrice }}$
          </span>
        </div>
        <business-item-actions @typing="orderPrice = $event" :input="orderPrice" :propFunc="changeOrderPrice"/>
      </div>
      <div class="price__item order">
        <div class="text">
          <span class="text__info">{{loc('biz:menu:item:price:max')}}</span>
          <span class="text__value">
            {{ item.MaxPrice }}
            {{ item.MaxMinType }}
          </span>
        </div>
        <business-item-actions @typing="maxPrice = $event" :input="maxPrice" :propFunc="changeMaxPrice"/>
      </div>
      <div class="price__item order">
        <div class="text">
          <span class="text__info">{{loc('biz:menu:item:price:min')}}</span>
          <span class="text__value">
            {{ item.MinPrice }}
            {{ item.MaxMinType }}
          </span>
        </div>
        <business-item-actions @typing="minPrice = $event" :input="minPrice" :propFunc="changeMinPrice"/>
      </div>
      <div class="price__item order">
        <div class="text">
          <span class="text__info">{{loc('biz:menu:item:stock')}}</span>
          <span class="text__value">
            {{ item.StockCapacity }}
            {{ loc('gui_773') }}
          </span>
        </div>
        <business-item-actions @typing="stockCapacity = $event" :input="stockCapacity" :propFunc="changeStockCapacity"/>
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
import BusinessItemActions from './BusinessItemActions'

export default {
  name: 'BusinessItem',

  components: { BusinessItemActions },

  data: function () {
    return {
      orderPrice: '',
      maxPrice: '',
      minPrice: '',
      stockCapacity: ''
    }
  },

  props: ['item'],
  computed:{
    ...mapGetters('localization', ['loc'])
  },
  methods: {
    changeOrderPrice () {
      window.mp.trigger('bizsetts:changeOrderPrice', this.orderPrice, this.item.Name)
    },

    changeMaxPrice () {
      window.mp.trigger('bizsetts:changeMaxPrice', this.maxPrice, this.item.Name)
    },

    changeMinPrice () {
      window.mp.trigger('bizsetts:changeMinPrice', this.minPrice, this.item.Name)
    },

    changeStockCapacity () {
      window.mp.trigger('bizsetts:changeStockCapacity', this.stockCapacity, this.item.Name)
    },

    deleteItem () {
      window.mp.trigger('bizsetts:delete', this.item.Name)
    }
  }
}
</script>

<style lang="scss" scoped>
.business-item {
  display: block;
  background-color: rgba(33, 33, 37, .9);
  margin: 0 0 1rem 0;
  padding: 1rem;
  border: .1rem solid rgba(78, 149, 37, .3);
  border-radius: .2rem;
  .title {
    display: flex;
    align-items: center;
    &-text {
      font-size: 2rem;
      color: #fff;
    }
    &-button {
      padding: .35rem .8rem;
      outline: none;
      border: none;
      font-size: 1.2rem;
      background-color: #4e9525;
      color: #fff;
      letter-spacing: .4px;
      margin: 0 0 0 1rem;
    }
  }
  .price {
    display: flex;
    flex-direction: column;
    &__item {
      margin: .6rem 0 0 0;
      display: flex;
      justify-content: space-between;
      align-items: center;
      .text {
        font-size: 1rem;
        color: #dddddd;
      }
    }
  }
}
</style>
