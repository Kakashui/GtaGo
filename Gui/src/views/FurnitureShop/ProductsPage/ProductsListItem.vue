<template>
  <div :class="[{active: inBasket}, 'products-list-item']">
    <div class="img" :style="{backgroundImage: 'url(/img/furnitureShop/items/'+ item.key +'.png)'}"></div>
    <div class="info">
      <div class="info__name">{{item.name}}</div>
      <div class="info__cost">$ {{item.cost * pricePart}}</div>
    </div>
    <div
      class="count-wrap"
      v-if="inBasket"
    >
      <div class="btn btn-down" @click="changeInQuantityDown({key: item.key})"></div>
      <div class="value">{{countInBasket}}</div>
      <div class="btn btn-up" @click="changeInQuantityUp({key: item.key})"></div>
    </div>
    <div 
      v-else
      class="btn btn-add" 
      @click="putInBasket({name: item.name, cost: item.cost, key: item.key})"
    >{{loc('furnitureShop_8')}}</div>
  </div>
</template>

<script>
import { mapGetters, mapMutations, mapState } from 'vuex'
export default {
  name: 'ProductsListItem',

  props: {
    item: Object
  },

  computed: {
    ...mapState('furnitureShop', ['basket', 'pricePart']),
    ...mapGetters('localization', ['loc']),
    inBasket: function() {
      return this.basket.some(element => element.key === this.item.key)
    },
    countInBasket: function() {
      let value = null
      this.basket.forEach((element) => {
        if (element.key === this.item.key) {
          value = element.count
        }
      })
      return value
    }
  },

  methods: {
    ...mapMutations('furnitureShop', ['putInBasket', 'changeInQuantityDown', 'changeInQuantityUp'])
  }
}
</script>

<style lang="scss" scoped>
.products-list-item{
  display: flex;
  flex-flow: column;
  &.active{
    .img{
      &:after{
        transition: .3s;
        transform: translateY(0%);
      }
    }
  }
  .img{
    width: 100%;
    height: 12rem;
    background-size: cover;
    background-position: center;
    background-repeat: no-repeat;
    background-color: #fff;
    position: relative;
    overflow: hidden;
    &:after{
      content: '';
      width: 100%;
      height: .2rem;
      position: absolute;
      left: 0;
      bottom: 0;
      background: #53D3C3;
      transform: translateY(100%);
    }
  }
  .info{
    width: 100%;
    display: flex;
    align-items: center;
    justify-content: space-between;
    font-weight: bold;
    font-size: 1rem;
    line-height: 1.15rem;
    margin: .5rem 0;
    &__name{
      color: rgba(0, 0, 0, 0.5);
    }
    &__cost{
      color: #24486A;
    }
  }
  .count-wrap{
    width: 100%;
    display: flex;
    align-items: center;
    justify-content: flex-start;
    .value{
      min-width: 2.5rem;
      text-align: center;
      font-size: 1rem;
      line-height: 1.15rem;
      color: #000000;
    }
    .btn{
      width: 1.9rem;
      height: 1.9rem;
      min-width: 1.9rem;
      background: #53D3C3;
      border-radius: 50%;
      background-size: contain;
      background-position: center;
      background-repeat: no-repeat;
      &.btn-down{
        background-image: url('/img/furnitureShop/down.svg');
      }
      &.btn-up{
        background-image: url('/img/furnitureShop/up.svg');
      }
    }
  }
  .btn-add{
    width: fit-content;
    background: #53D3C3;
    border-radius: .2rem;
    padding: .5rem 1rem;
    font-weight: bold;
    font-size: .9rem;
    line-height: 1.05rem;
    color: #000000;
    transition: 0;
  }
}
</style>
