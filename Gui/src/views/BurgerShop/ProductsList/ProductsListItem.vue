<template>
  <div class="list-item">
    <div class="price">{{item.cost}}$</div>
    <div class="img" :style="{backgroundImage: 'url(/img/burgerShop/items/' + item.unicName + '.png'}"></div>
    <div class="title">{{item.name}}</div>
    <div 
      class="btn-buy" 
      @click="putInShoppingCart({ item: item.name, cost: item.cost, unicName: item.unicName })"
    >{{loc('burgerShop_5')}}</div>
  </div>
</template>

<script>
import {mapState, mapMutations, mapGetters} from 'vuex'

export default {
  name: 'ProductsListItem',

  props: {
    item: Object
  },

  computed: {
    ...mapState('burgerShop', ['items', 'shoppingCart']),
    ...mapGetters('localization', ['loc']),
  },

  methods: {
    ...mapMutations('burgerShop', ['putInShoppingCart'])
  }
}
</script>

<style lang="scss" scoped>
.list-item{
  display: flex;
  flex-flow: column;
  align-items: flex-start;
  position: relative;
  padding: 2.25rem 2.25rem 5.25rem 2.25rem;
  background: linear-gradient(129.17deg, rgba(255, 255, 255, 0.1) 0%, rgba(255, 255, 255, 0) 98.82%);
  transition: .2s;
  &:hover{
    background: linear-gradient(157.12deg, #151515 17.04%, rgba(41, 41, 41, 0.15) 106.68%);
    transition: .2s; 
    .btn-buy{
      background-image: url('/img/burgerShop/button-hover.png');
    }
  }
  .price{
    font-size: 1rem;
    line-height: 1.2rem;
    color: #C9EA27;
    position: relative;
    &:before, &:after{
      content: '';
      background: rgba(255, 255, 255, 0.1);
      position: absolute;
      top: -1rem;
      left: -1rem;
    }
    &:before{
      height: 5rem;
      width: 1px;
    }
    &:after{
      width: 5rem;
      height: 1px;
    }
  }
  .img{
    position: absolute;
    right: -2rem;
    top: -1px;
    width: 10rem;
    height: 10rem;
    background-size: contain;
    background-repeat: no-repeat;
    background-position: center;
  }
  .title{
    margin: 3.3rem 0 .5rem 0;
    font-size: 1.35rem;
    line-height: 1.6rem;
    color: #FFFFFF;
  }
  .desc{
    font-weight: 300;
    font-size: .75rem;
    line-height: 1.05rem;
    color: rgba(255, 255, 255, 0.7);
    position: relative;
  }
  .btn-buy{
    height: 3.75rem;
    min-height: 3.75rem;
    width: 100%;
    display: flex;
    align-items: center;
    justify-content: center;
    position: absolute;
    bottom: 0;
    left: 0;
    text-transform: uppercase;
    font-weight: normal;
    font-size: 1.2rem;
    line-height: 1.45rem;
    letter-spacing: 0.02em;
    text-transform: uppercase;
    color: #FFFFFF;
    padding-top: .5rem;
    background-image: url('/img/burgerShop/button.png');
    background-size: cover;
    background-position: center bottom;
    background-repeat: no-repeat;
    transition: .2s;
  }
}
</style>
