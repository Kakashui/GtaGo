<template>
  <div class="header-site">
    <div class="header-site__nav">
      <div class="header-site__nav-title">{{loc('furnitureShop_1')}}</div>
      <div class="header-site__nav-items">
        <div 
          class="item"
          v-for="item in this.categories"
          :key="item.id"
          @click="setCurrentPage('ProductsPage', item)"
        >{{loc(item.name)}}</div>
      </div>
    </div>
    <div class="header-site__title" @click="setCurrentPage('MainPage', null)">Desk.store</div>
    <div class="header-site__basket" @click="setCurrentPage('BasketPage')">
      <div class="header-site__basket-title">{{loc('furnitureShop_2')}}</div>
      <div class="header-site__basket-icon">
        <span 
          class="count"
          v-if="this.basket.length !== 0"
        >{{this.basket.length}}</span>
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
export default {
  name: 'HeaderSite',

  props: {
    setCurrentPage: Function,
  },

  computed: {
    ...mapState('furnitureShop', ['categories', 'basket']),
    ...mapGetters('localization', ['loc'])
  },
}
</script>

<style lang="scss" scoped>
.header-site{
  position: fixed;
  top: 6.8rem;
  background: #E1E3E8;
  height: 4rem;
  min-height: 4rem;
  width: 100%;
  display: flex;
  align-items: center; 
  justify-content: space-between;
  padding: 0 10rem;
  color: #000000;
  z-index: 3;
  &__nav{
    display: flex;
    align-items: center;
    height: 100%;
    &:hover{ 
      .header-site__nav-items{
        height: 8.2rem;
        opacity: 1;
        transition: .2s;
      }
    }
    &-title{
      font-size: 1rem;
      padding-left: 1.2rem;
      background-image: url('/img/furnitureShop/burger-icon.svg');
      background-size: .7rem;
      background-position: left;
      background-repeat: no-repeat;
    }
    &-items{
      opacity: 0;
      height: 0;
      position: absolute;
      display: grid;
      flex-direction: column;
      grid-auto-columns: 1fr;
      grid-template-rows: repeat(4, auto);
      grid-auto-flow: column;
      grid-row-gap: .85rem;
      grid-column-gap: 1.5rem;
      // background-image: url('/img/furnitureShop/Subtract-nav.svg');
      background: #E1E3E8;
      border-radius:  0 0 1rem 1rem;
      bottom: 0;
      transform: translateY(100%);
      padding: 1rem;
      &:before, &:after{
        content: '';
        width: 1rem;
        height: 1rem;
        position: absolute;
        top: 0;
        background-size: contain;
        background-repeat: no-repeat;
      }
      &:before{
        left: 0;
        transform: translateX(-98%);
        background-position: top right;
        background-image: url('/img/furnitureShop/Intersect-left.svg');
      }
      &:after{
        right: 0;
        transform: translateX(98%);
        background-position: top left;
        background-image: url('/img/furnitureShop/Intersect-right.svg');
      }
      .item{
        font-size: .8rem;
        line-height: .9rem;
        color: rgba(0, 0, 0, 0.6);
        &:hover{
          color: rgba(0, 0, 0, 1);
          transition: .2s;
        }
      }
    }
  }
  &__title{
    padding-left: 1.9rem;
    background-image: url('/img/furnitureShop/logo.svg');
    background-size: 1.7rem;
    background-position: left;
    background-repeat: no-repeat;
    font-weight: bold;
    font-size: 1rem;
    line-height: 1.7rem;  
    text-transform: uppercase;
  }
  &__basket{
    display: flex;
    align-items: center;
    &-title{
      font-size: 1rem;
    }
    &-icon{
      width: 1.7rem;
      height: 1.7rem;
      background-image: url('/img/furnitureShop/basket-icon.svg');
      background-size: contain;
      background-position: center;
      background-repeat: no-repeat;
      margin: .25rem 0 0 .5rem;
      position: relative;
      .count{
        width: 1.5rem;
        height: 1.35rem;
        display: flex;
        align-items: center;
        justify-content: center;
        font-size: .8rem;
        background: #53D3C3;
        border-radius: 1rem;
        position: absolute;
        right: -1rem;
        top: -.2rem;
      }
    }
  }
}
</style>
