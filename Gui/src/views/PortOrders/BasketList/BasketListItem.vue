<template>
  <div class="basket-list-item">
    <div class="title">{{item.name}}</div>
    <div class="cost">${{item.cost*item.count}}</div>
    <div 
      class="btn-delete"
      @click="deleteItem({key: index})"
    ></div>
  </div>
</template>

<script>
import { mapMutations } from 'vuex'
export default {
  name: 'BasketListItem',

  props: {
    item: Object,
    index: Number
  },

  methods: {
    ...mapMutations('portOrders', ['deleteItem'])
  }
}
</script>

<style lang="scss" scoped>
.basket-list-item{
  display: flex;
  align-items: flex-start;
  justify-content: space-between;
  position: relative;
  padding: .5rem 2.4rem .5rem 1rem;
  &:hover{
    background: #EDF0F2;
    transition: .2s;
    .btn-delete{
      opacity: 1;
    }
  }
  .title{
    font-size: .85rem;
    line-height: 1rem;
    color: #000000;
  }
  .cost{
    text-transform: uppercase;
    font-size: 1rem;
    line-height: 1.2rem;
    text-align: right;
    color: #000000;
  }
  .btn-delete{
    background-image: url('/img/portOrders/delete-item.png');
    background-size: contain;
    background-repeat: no-repeat;
    background-position: center;
    width: 1.2rem;
    height: 1.2rem;
    position: absolute;
    top: .4rem;
    right: .7rem;
    opacity: 0;
    &:hover{
      transform: scale(1.1);
      transition: .2s;
    }
  }
}
</style>
