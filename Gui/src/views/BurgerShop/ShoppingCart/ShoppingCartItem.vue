<template>
  <div class="list-item-buy">
    <div class="item-img" :style="{ backgroundImage: 'url(/img/burgerShop/items/' + item.unicName + '.png' }"></div>
    <div class="btn btn-remove" @click="changeInQuantityDown({key: index})"></div>
    <div class="value">{{item.count}}</div>
    <div class="btn btn-add" @click="changeInQuantityUp({key: index})"></div>
    <div class="btn btn-remove-all" @click="deleteItem({key: index})"></div>
  </div>
</template>

<script>
import { mapState, mapMutations } from 'vuex'

export default {
  name: 'ShoppingCartItem',

  props: {
    item: Object,
    index: Number
  },

  computed: {
    ...mapState('burgerShop', ['shoppingCart']),
  },

  methods: {
    ...mapMutations('burgerShop', ['deleteItem', 'changeInQuantityUp', 'changeInQuantityDown']),
  }
}
</script>

<style lang="scss" scoped>
.list-item-buy{
  display: flex;
  align-items: center;
  justify-content: space-between;
  background: rgba(255, 255, 255, 0.06);
  height: 4.5rem;
  min-height: 4.5rem;
  margin-bottom: .5rem;
  padding: .5rem;
  &:last-child{
    margin-bottom: 0;
  }
  .item-img, .btn{
    background-size: contain;
    background-repeat: no-repeat;
    background-position: center;
  }
  .item-img{
    width: 3.5rem;
    min-width: 3.5rem;
    height: 3.5rem;
    margin-right: .75rem;
  }
  .btn{
    width: 1.6rem;
    min-width: 1.6rem;
    height: 1.6rem;
    display: flex;
    align-items: center;
    justify-content: center;
    transition: .2s;
    &:hover{
      transition: .2s;
    }
    &.btn-remove, &.btn-add{
      background-color: rgba(255, 255, 255, 0.1);
      &:hover{
        background-color: rgba(255, 255, 255, 0.2);
      }
    }
    &.btn-remove{
      background-image: url('/img/burgerShop/btn-remove.svg');
    }
    &.btn-add{
      background-image: url('/img/burgerShop/btn-add.svg');
    }
    &.btn-remove-all{
      background-image: url('/img/burgerShop/btn-remove-all.svg');
      &:hover{
        transform: scale(1.05);
      }
    }
  }
  .value{
    font-size: .9rem;
    text-align: center;
    color: #FFFFFF;
  }
}
</style>
