<template>
  <div class="burger-shop">
    <div class="burger-shop-body">
      <div class="burger-shop__header">
        <div class="logo">
          <div class="logo-title">Burger</div>
          <div class="logo-img"></div>
        </div>
        <div class="title">
          <div class="title__name">Menu</div>
          <div class="title__desc">{{loc('burgerShop_0')}}</div>
        </div>
        <div class="btn-close" @click="close">{{loc('burgerShop_1')}}</div>
      </div>
      <div class="burger-shop__body">
        <products-list/>
        <shopping-cart/>
      </div>
    </div>    
  </div>
</template>

<script>
import { mapMutations, mapGetters } from 'vuex';
import ProductsList from './ProductsList'
import ShoppingCart from './ShoppingCart'

export default {
  name: 'BurgerShop',

  components: {
    ProductsList,
    ShoppingCart
  },

  computed: {
    ...mapGetters('localization', ['loc']),
  },

  methods: {
    ...mapMutations('burgerShop', ['clearShoppingCart']),
    close(){
      this.clearShoppingCart();
      window.mp.trigger("burgerShot::close")
    }
  }
}
</script>

<style lang="scss" scoped>
  .burger-shop{
    width: 100%;
    height: 100%;
    position: absolute;
    top: 0;
    left: 0;
    background: url('/img/burgerShop/bg.svg'), linear-gradient(81.22deg, #1E2229 3.62%, rgba(0, 0, 0, 0) 96.65%);
    background-position: bottom;
    background-size: contain;
    background-repeat: no-repeat;
    &-body{
      display: flex;
      flex-flow: column;
      max-width: 1920px;
      height: 100%;
      margin: auto;
      position: relative;
      backdrop-filter: blur(.2rem);
    }
    &__header{
      display: flex;
      align-items: center;
      justify-content: space-between;
      padding: 5rem 8.3rem 0rem 5rem;
      .logo{
        display: flex;
        align-items: center;
        &-img{
          width: 5.5rem;
          height: 5.5rem;
          margin-right: .75rem;
          background-image: url('/img/burgerShop/logo.svg');
          background-size: contain;
          background-repeat: no-repeat;
          background-position: center;
        }
        &-title{
          display: flex;
          flex-flow: column;
          align-items: flex-start;
          font-size: 1.3rem;
          letter-spacing: 0.18em;
          color: #FFFFFF;
          text-align: center;
          display: flex;
          align-items: center;
          justify-content: center;
          width: 1.3rem;
          height: 1.3rem;
          transform: rotate(-90deg);
          margin-right: 1rem;
        }
      }
      .title{
        position: relative;
        display: flex;
        flex-flow: column;
        align-items: center;
        justify-content: center;
        &__name{
          font-weight: 500;
          font-size: 4rem;
          line-height: 4.75rem;
          color: #fff;
        }
        &__desc{
          margin-top: 1rem;
          font-weight: 300;
          font-size: .9rem;
          line-height: 1rem;
          text-align: center;
          color: rgba(255, 255, 255, 0.5);
        }
        &:before, &:after{
          content: '';
          position: absolute;
        }
        &:before{
          width: 25rem;
          height: 5.3rem;          
          background: rgba(193, 231, 4, 0.5);
          opacity: 0.4;
          filter: blur(2.5rem);
          border-radius: 50%;
          top: 1.75rem;
        }
        &:after{
          width: 4.25rem;
          height: 1px;
          bottom: 0;
          transform: translateY(1.75rem);
          background: #C9EA27;
        }
      }
      .btn-close{
        font-weight: 500;
        font-size: 1.25rem;
        line-height: 1.5rem;
        color: #FFFFFF;
        position: relative;
        display: flex;
        align-items: center;
        &:before{
          content: '';
          position: absolute;
          width: 1.1rem;
          height: 1.1rem;
          left: -1rem;
          transform: translateX(-.9rem);
          background-image: url('/img/burgerShop/btn-close.svg');
          background-position: center;
          background-size: contain;
          background-repeat: no-repeat;
        }
      }
    }
    &__body{
      display: flex;
      align-items: flex-start;
      justify-content: space-between;
      height: 100%;
      margin-top: 4.75rem;
      padding: 0 6.5rem 0 5rem;
    }
  }
</style>
