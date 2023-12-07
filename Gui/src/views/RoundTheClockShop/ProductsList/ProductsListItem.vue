<template>
  <div class="item">
    <div class="item__name">{{loc(item.name)}}</div>
    <div class="item__price">{{item.cost}}</div>
    <div class="item__image" v-bind:style="{ backgroundImage: 'url(/img/roundTheClockShop/products/' + item.image + '.png)'}"></div>
    <div 
      class="btn-add"
      @click="putInBasket({item: item.name, cost: item.cost, image: item.image, product: item.product})"
    >{{loc("shop24_3")}}</div>
  </div>
</template>

<script>
import {mapState, mapMutations, mapGetters} from 'vuex'

export default {
  name: 'ProductsListItem',

  props: {
    item: Object,
    index: Number
  },

  computed: {
    ...mapState('roundTheClockShop', ['productsSell', 'basket']),
    ...mapGetters('localization', ['loc']),
  },

  methods: {
    ...mapMutations('roundTheClockShop', ['putInBasket'])
  }
}
</script>

<style lang="scss" scoped>
  .item{
    background-image: url('/img/roundTheClockShop/item-bg.svg');
    background-size: 100% 100%;
    padding: 2.25rem .5rem 0 1.75rem;
    position: relative;
    display: flex;
    flex-flow: column;
    align-items: flex-start;
    &__name{
      font-weight: 300;
      font-size: 1.3rem;
      line-height: 1.55rem;
      margin-bottom: .25rem;
      color: #FFFFFF;
    }
    &__price{
      font-weight: 300;
      font-size: 1rem;
      line-height: 1.2rem;
      color: #C9EA27;
      &:after{
        content: '$';
      }
    }
    &__image, .btn-add{
      position: absolute;
      bottom: 0;
    }
    &__image{
      height: 12rem;
      width: 7.5rem;
      right: .56rem;
      z-index: 1;
      background-size: contain;
      background-repeat: no-repeat;
      background-position: right bottom;
    }
    .btn-add{
      display: flex;
      align-items: center;
      justify-content: center;
      width: 71%;
      height: 3rem;
      min-height: 3rem;
      left: 0;
      z-index: 2;
      background: rgba(255, 255, 255, 0.2);
      font-weight: 500;
      font-size: .85rem;
      line-height: 1.6rem;
      color: #FFFFFF;
      &:hover{
        background: #A2BC1F;
        box-shadow: 0 1rem 2rem rgba(193, 231, 4, 0.2);
        transition: .3s;
      }
    }
  }
</style>
