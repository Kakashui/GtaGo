<template>
  <div class="products-list-item">
    <div class="products-list-item__name">{{item.name}}</div>
    <div class="products-list-item__desc">{{item.desc}}</div>
    <div class="products-list-item__info">
      <div class="cost">${{item.cost}}</div>
      <div 
        class="btn-buy"
        @click="setCurrentItem({id: item.id, name: item.name, desc: item.desc, cost: item.cost, count: 1})"
      >{{loc('PortOrders_6')}}</div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapMutations } from 'vuex'
export default {
  name: 'ProductsListItem',

  props: {
    item: Object
  },

  computed: {
    ...mapGetters('localization', ['loc']),
  },

  methods: {
    ...mapMutations('portOrders', ['setCurrentItem'])
  }
}
</script>

<style lang="scss" scoped>
.products-list-item{
  display: flex;
  justify-content: flex-start;
  flex-flow: column;
  padding: 1rem;
  background: #fff;
  font-size: .85rem;
  line-height: 1rem;
  .btn-buy{
    opacity: 0;
  }
  &:hover{
    .btn-buy{
      opacity: 1;
      transition: .2s;
    }
  }
  &__name{
    font-weight: bold;
    color: #000000;
    margin-bottom: .5rem;
  }
  &__desc{
    height: 100%;
    color: rgba(0, 0, 0, 0.3);
  }
  &__info{
    display: flex;
    width: 100%;
    align-items: center;
    justify-content: space-between;
    .cost{
      text-transform: uppercase;
      font-style: normal;
      font-weight: bold;
      font-size: 1rem;
      line-height: 1rem;
      color: #000000;
    }
    .btn-buy{
      padding: .4rem .6rem;
      background: #48B3EF;
      border-radius: .2rem;
      font-size: .8rem;
      color: #FFFFFF;
      transition: .2s;
      &:hover{
        transition: .2s;
        transform: scale(1.05);
      }
    }
  }
}
</style>
