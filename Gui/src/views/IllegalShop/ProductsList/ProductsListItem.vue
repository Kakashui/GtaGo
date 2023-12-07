<template>
  <div
    class="products-list-item"
    :style="{backgroundImage: 'url(/img/illegalShop/items/' + item.product + '.png)'}"
  >
    <div class="products-list-item__name">{{loc(item.name)}}</div>
    <div class="products-list-item__cost">${{item.cost}}</div>
    <div class="products-list-item__count-interaction">
      <div 
        class="btn btn-prev"
        @click="changeInQuantityDown"
      ></div>
      <input 
        class="input-number"
        type="number"
        min="1"
        step="1"
        v-model.number="item.count">
      <div 
        class="btn btn-next"
        @click="changeInQuantityUp"
      ></div>
      <div 
        class="btn btn-done" 
        @click="setCurrentCount(item.name, item.cost, item.count, item.product)"
      ></div>
    </div>
  </div>
</template>

<script>
import { mapMutations, mapGetters } from 'vuex'
export default {
  name: 'ProductsListItem',

  props: {
    item: Object
  },

  // data: function() {
  //   return {
  //     currentCount: 1
  //   }
  // },

  computed: {
    ...mapGetters('localization', ['loc']),
  },

  methods: {
    ...mapMutations('illegalShop', ['putInBasket']),
    changeInQuantityDown: function() {
      if (this.item.count > 1) {
        this.item.count -= 1
      }
    },
    changeInQuantityUp: function() {
      this.item.count += 1
    },
    setCurrentCount: function(name, cost, count, product) {
      if (this.item.count > 0 && Number.isInteger(this.item.count)) {
        this.putInBasket({name: name, cost: cost, count: count, product: product})
        this.item.count = 1
      }
    }
  }
}
</script>

<style lang="scss" scoped>
.products-list-item{
  background-color: rgba(30, 45, 53, 0.4);
  border-radius: 1rem;
  display: flex;
  flex-flow: column;
  align-items: center;
  justify-content: flex-end;
  padding-bottom: 1.25rem;
  background-size: contain;
  background-position: right bottom;
  background-repeat: no-repeat;
  position: relative;
  transition: .2s;
  &:before, &:after{
    content: '';
    width: 100%;
    height: 100%;
    position: absolute;
    bottom: 0;
    left: 0;
    z-index: 1;
    transition: .2s;
    border-radius: 1rem;
    overflow: hidden;
  }
  &:before{
    background: linear-gradient(180deg, rgba(30, 45, 53, 0) 39.06%, rgba(30, 45, 53, 0.72) 72.92%, #1E2D35 100%);
    opacity: 1;
  }
  &:after{
    background: linear-gradient(180deg, rgba(20, 26, 0, 0) 25.91%, #94C000 100%);
    opacity: 0;
  }
  &:hover{
    background-color: rgba(148, 192, 0, 0.1);
    transition: .2s;
    &:before{
      opacity: 0;
      transition: .2s;
    }
    &:after{
      opacity: 1;
      transition: .2s;
    }
    .products-list-item__count-interaction{
      opacity: 1;
      transition: .2s;
    }
  }
  &__name, &__cost{
    z-index: 2;
  }
  &__name{
    font-size: .7rem;
  }
  &__cost{
    font-size: .8rem;
    margin-top: .45rem;
  }
  &__count-interaction{
    display: flex;
    align-items: center;
    justify-content: center;
    background: #FFFFFF;
    border-radius: 1rem;
    width: 6.8rem;
    height: 3.25rem;
    position: absolute;
    bottom: 1rem;
    z-index: 3;
    opacity: 0;
    transition: .2s;
    .btn-prev, .btn-next{
      background-image: url('/img/illegalShop/arrow.svg');
      background-size: contain;
      background-repeat: no-repeat;
      background-position: center;
      width: 1.5rem;
      height: 1.5rem;
    }
    .btn-next{
      transform-origin: 50% 50%;
      transform: rotate(180deg);
    }
    .input-number{
      background: #F3F3F3;
      box-shadow: inset 0 0 .5rem 1px rgba(0, 0, 0, 0.04);
      border-radius: .5rem;
      width: 2.7rem;
      height: 2rem;
      outline: none;
      text-align: center;
      font-weight: bold;
      font-size: .9rem;
      color: rgba(0, 0, 0, 0.85);
      &::-webkit-outer-spin-button, &::-webkit-inner-spin-button{
        -webkit-appearance: none;
      }
    }
    .btn-done{
      background-image: url('/img/illegalShop/done.svg');
      background-size: contain;
      background-repeat: no-repeat;
      background-position: center;
      background-color: #94C000;
      border: .2rem solid #FFFFFF;
      box-sizing: border-box;
      width: 2.5rem;
      height: 2.5rem;
      border-radius: 50%;
      position: absolute;
      bottom: -1.9rem;
      transition: .2s;
      &:hover{
        box-shadow: 0 0 2rem .25rem rgba(188, 192, 0, 0.63);
        transition: .2s;
      }
      &:active{
        transform: scale(.9);
        transition: .2s;
      }
    }
  }
}
</style>
