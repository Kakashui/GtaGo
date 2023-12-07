<template>
  <div class="illegal-shop">
    <div class="illegal-shop__body">
      <div class="btn btn-close" @click="closeIllegalShop"></div>
      <div class="illegal-shop__body-header">
        <div class="logo">
          <div class="logo__img"></div>
          <div class="logo__title">Illegal Shop</div>
        </div>
        <div class="money">
          <div class="money__desc">{{loc('illegalShop_1')}}:</div>
          <div class="money__value">${{ money.toLocaleString() }}</div>
        </div>
        <div
          class="money"
          v-if="this.mainPage"
        >
          <div class="money__desc">{{loc('illegalShop_2')}}:</div>
          <div class="money__value">${{basketTotal}}</div>
        </div>
        <div 
          class="basket-nav"
          @click="setPage"
        >
          <template v-if="this.mainPage">
            <div class="basket-nav__text">{{loc('illegalShop_3')}}</div>
            <div class="basket-nav__icon">
              <div 
                class="product-count"
                v-if="this.basket.length > 0"
              >{{this.basket.length}}</div>
            </div>
          </template>
          <template v-else>
            <div class="basket-nav__text">{{loc('illegalShop_4')}}</div>
          </template>
        </div>
      </div>
      <div class="illegal-shop__body-container">
        <products-list v-if="this.mainPage" />
        <basket-list 
          v-else
          :basket="this.basket"
          :basketTotal="this.basketTotal"
         />
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'

import ProductsList from './ProductsList'
import BasketList from './BasketList'

export default {
  name: 'IllegalShop',

  components: {
    ProductsList,
    BasketList
  },

  data: function() {
    return {
      mainPage: true
    }
  },

  methods: {
    setPage: function() {
      this.mainPage = !this.mainPage
    },
    closeIllegalShop: function() {
      window.mp.trigger('illegalShop:close')
    }
  },

  computed: {
    ...mapState('illegalShop', ['basket']),
    ...mapState('hud', ['money']),
    ...mapGetters('localization', ['loc']),
    basketTotal: function() {
      return this.basket.reduce((total, basketItem) => total + basketItem.cost*basketItem.count, 0)
    }
  }
}
</script>

<style lang="scss" scoped>
.illegal-shop{
  display: flex;
  align-items: center;
  justify-content: center;
  width: 100%;
  height: 100%;
  position: absolute;
  top: 0;
  left: 0;
  box-shadow: inset 0 .2rem .85rem 38.7rem rgba(5, 26, 38, 0.71);
  color: #FFFFFF;
  .btn{
    display: flex;
    align-items: center;
    justify-content: center;
    background-size: contain;
    background-repeat: no-repeat;
    background-position: center;
    position: relative;
    transition: .2s;
    &:hover{
      transition: .2s;
    }
  }
  .btn-close{
    position: absolute;
    right: 2.7rem;
    top: 2.85rem;
    background-image: url('/img/illegalShop/close.svg');
    width: 1rem;
    height: 1rem;
    z-index: 1;
    &:hover{
      transform: scale(1.1);
    }
  }
  &__body{
    display: flex;
    flex-flow: column;
    background: linear-gradient(248.66deg, rgba(148, 192, 0, 0.26) 0%, rgba(0, 133, 255, 0.04) 63.54%), rgba(17, 17, 17, 0.85);
    border-radius: 1.5rem;
    padding: 2.2rem 2.7rem 1.7rem 2.7rem;
    position: relative;
    width: 60rem;
    &-header{
      display: flex;
      justify-content: flex-start;
      align-items: center;
      width: 100%;
      margin-bottom: 2.7rem;
      position: relative;
      .logo{
        display: flex;
        align-items: center;
        justify-content: center;
        margin-right: 4.5rem;
        width: 11.9rem;
        min-width: 11.9rem;
        &__img{
          width: 3.25rem;
          min-width: 3.25rem;
          height: 3.3rem;
          background-image: url('/img/illegalShop/logo.png');
          background-size: contain;
          background-repeat: no-repeat;
          background-position: center;
          margin-right: .9rem;
        }
        &__title{
          text-transform: uppercase;
          font-style: italic;
          font-weight: 800;
          font-size: 1.1rem;
          line-height: 95.02%;
        }
      }
      .money{
        width: 16%;
        display: flex;
        align-items: flex-start;
        flex-flow: column;
        &__desc, &__value{
          font-weight: bold;
        }
        &__desc{
          font-size: .6rem;
          color: #94C000;
        }
        &__value{
          font-size: .9rem;
        }
      }
      .basket-nav{
        position: absolute;
        right: 2.5rem;
        display: flex;
        align-items: center;
        justify-content: center;
        min-height: 3rem;
        height: 3rem;
        width: 10rem;
        border: .1rem solid #94C000;
        border-radius: 1rem;
        transition: .2s;
        &:hover{
          transform: scale(1.1);
          transition: .2s;
        }
        &:active{
          transform: scale(1.05);
          transition: .1s;
        }
        &__text{
          font-weight: bold;
          font-size: .9rem;
          text-transform: uppercase;
        }
        &__icon{
          margin-left: .6rem;
          width: 1.5rem;
          height: 1.25rem;
          position: relative;
          background-image: url('/img/illegalShop/basket-icon.svg');
          background-size: contain;
          background-repeat: no-repeat;
          background-position: center;
          .product-count{
            width: 1.1rem;
            height: 1.1rem;
            border-radius: 50%;
            background: #94C000;
            display: flex;
            align-items: center;
            justify-content: center;
            color: #000;
            font-weight: bold;
            font-size: .6rem;
            position: absolute;
            right: -.65rem;
            top: -.25rem;
          }
        }
      }
    }
    &-container{
      min-height: 27.7rem;
    }
  }
}
</style>
