<template>
  <div class="main-page">
    <div class="main-page__info">
      <div class="greeting">{{loc('PortOrders_11')}}, <br/><span>{{userName}}</span>!</div>
      <div class="desc">{{loc('PortOrders_12')}}</div>
    </div>
    <div class="main-page__offers">
      <div class="title">{{buissName}}</div>
      <div class="desc">{{loc('PortOrders_13')}}</div>
      <div class="new-items">
        <products-list-item
          v-for="item in this.productsListBuiss"
          :key="item.index"
          :item="item"
        />
      </div>
      <div class="prompt">{{loc('PortOrders_14')}}</div>
      <div class="btn-go" @click="setCurrentPage('ProductPage')">{{loc('PortOrders_15')}}</div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
import ProductsListItem from './ProductsList/ProductsListItem'

export default {
  name: 'MainPage',

  components: {
    ProductsListItem
  },

  props: {
    setCurrentPage: Function
  },

  computed: {
    ...mapState('portOrders', ['productsList', 'userName', 'buissName']),
    ...mapGetters('localization', ['loc']),
    productsListBuiss: function() {
      let newArray = this.productsList.slice(-2)
      return newArray
    }
  }
}
</script>

<style lang="scss" scoped>
.main-page{
  display: flex;
  align-items: flex-start;
  justify-content: space-between;
  padding: 2.65rem 20.4rem 1rem 20.4rem;
  color: #FFFFFF;
  width: 100%;
  &__info{
    display: flex;
    flex-flow: column;
    align-items: flex-start;
    .greeting{
      font-size: 2.5rem;
      line-height: 2.95rem;
      font-weight: bold;
      width: 23.4rem;
    }
    .desc{
      padding-top: 2rem;
      position: relative;
      margin-top: 2rem;
      font-size: 1.1rem;
      line-height: 1.3rem;
      &:before{
        content: '';
        background: rgba(255, 255, 255, 0.4);
        height: .1rem;
        width: 5rem;
        position: absolute;
        top: 0;
        left: 0;
      }
    }
  }
  &__offers{
    background: linear-gradient(180deg, rgba(72, 179, 239, 0.8) 0%, rgba(72, 179, 239, 0) 100%);
    background-color: #000;
    border-radius: 1rem;
    padding: 2rem;
    display: flex;
    flex-flow: column;
    align-items: flex-start;
    width: 28.8rem;
    height: 30.25rem;
    .title{
      font-weight: bold;
      font-size: 2.2rem;
      line-height: 2.6rem;
      text-transform: uppercase;
      color: #FFFFFF;
    }
    .desc{
      font-weight: bold;
      font-size: 1.2rem;
      line-height: 1.4rem;
      margin: 2rem 0 1rem 0;
      text-transform: uppercase;
      color: rgba(255, 255, 255, 0.8);
    }
    .new-items{
      display: grid;
      width: 100%;
      grid-template-columns: repeat(2, 11.9rem);
      grid-template-rows: 8.75rem;
      grid-gap: 1rem;
    }
    .prompt{
      font-size:1rem;
      line-height: 1.15rem;
      color: rgba(255, 255, 255, 0.8);
      margin: 2rem 0 1rem 0;
    }
    .btn-go{
      background: #48B3EF;
      border-radius:1rem;
      padding: 1.2rem 1.6rem;
      font-size: 1rem;
      transition: .2s;
      &:hover{
        transition: .2s;
        transform: scale(1.1);
      }
    }
  }
}
</style>
