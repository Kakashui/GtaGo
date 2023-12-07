<template>
  <div class="furniture-shop">
    <div class="header">
      <div class="header__url">
        <div class="site-tab">
          <div class="logo-head"></div>
          <div class="text">Desk.store</div>
          <div class="btn btn-close"></div>
          <div class="btn btn-add"></div>
        </div>
        <div class="btn btn-back" @click="setCurrentPage('MainPage', null)"></div>
        <div class="btn btn-forward"></div>
        <div class="btn btn-refresh"></div>
        <div class="url">www.desk.store.com</div>
      </div>
    </div>
    <div class="body">
      <header-site
        :setCurrentPage="this.setCurrentPage"
      />
      <component 
        :is="currentPage.page"
        :currentPage="this.currentPage"
        :setCurrentPage="this.setCurrentPage"
      />
    </div>
  </div>
</template>

<script>
import MainPage from './MainPage'
import HeaderSite from './HeaderSite'
import ProductsPage from './ProductsPage'
import BasketPage from './BasketPage'

export default {
  name: 'FurnitureShop',

  components: {
    MainPage,
    ProductsPage,
    HeaderSite,
    BasketPage
  },

  data: function() {
    return {
      currentPage: {
        page: 'MainPage',
        data: null
      }
    }
  },

  methods: {
    setCurrentPage: function(page, data) {
      this.currentPage.page = page
      this.currentPage.data = data
    },
  }
}
</script>

<style lang="scss">
.furniture-shop{
  .btn{
    display: flex;
    align-items: center;
    justify-content: center;
    position: relative;
    transition: .2s;
    &:hover{
      transition: .2s;
      transform: scale(1.05);
    }
    &:active{
      transition: .2s;
      transform: scale(1.025);
    }
  }
}
</style>

<style lang="scss" scoped>
.furniture-shop{
  display: flex;
  width: 100%;
  height: 100%;
  position: absolute;
  top: 0;
  left: 0;
  flex-flow: column;
  background: #E1E3E8;
  .btn{
    background-size: contain;
    background-position: center;
    background-repeat: no-repeat;
  }
  .header{
    display: flex;
    flex-flow: column;
    width: 100%;
    padding-top: 3.5rem;
    background-color: #000;
    z-index: 2;
    &__url, &__site{
      width: 100%;
      display: flex;
      align-items: center;
      justify-content: flex-start;
    }
    &__url{
      background-color: #fff;
      position: relative;
      min-height: 3.35rem;
      padding: 0 5.65rem 0 1rem;
      .site-tab{
        position: absolute;
        top: 0;
        left: .6rem;
        transform: translateY(-98%);
        width: fit-content;
        display: flex;
        align-items: center;
        justify-content: flex-start;
        height: 2.25rem;
        min-height: 2.25rem;
        color: #000;
        background-image: url('/img/furnitureShop/Subtract.png');
        background-size: contain;
        background-repeat: no-repeat;
        background-position: bottom;
        padding: 0 2.2rem;
        width: 15.45rem;
        .text{
          font-weight: bold;
          font-size: .9rem;
          line-height: 1rem;
          color: #000000;
          width: 100%;
          margin-left: .45rem;
        }
        .logo-head, .btn-close{
          width: 1.1rem;
          min-width: 1.1rem;
          height: 1.1rem;
        }
        .logo-head{
          background-size: contain;
          background-repeat: no-repeat;
          background-position: center;
          background-image: url('/img/furnitureShop/logo-head.svg');
        }
        .btn-close{
          background-image: url('/img/furnitureShop/close.svg');
        }
        .btn-add{
          width: 1.4rem;
          height: 1.4rem;
          position: absolute;
          right: 0;
          transform: translateX(1.6rem);
          background-image: url('/img/furnitureShop/add.svg');
        }
      }
      &>.btn{
        width: 1.4rem;
        height: 1.4rem;
        min-width: 1.4rem;
        margin-right: 1.5rem;
        &.btn-back, &.btn-forward{
          background-image: url('/img/furnitureShop/arrow.svg');
        }
        &.btn-forward{
          opacity: .5;
          transform-origin: 50% 50%;
          transform: rotate(180deg);
        }
      }
      .btn-refresh{
        background-image: url('/img/furnitureShop/refresh.svg');
        width: 1.8rem;
        height: 1.8rem;
      }
      .url{
        margin-left: .5rem;
        height: 2.2rem;
        min-height: 2.2rem;
        background: #E7EBEE;
        border-radius:  2.2rem;
        display: flex;
        align-items: center;
        justify-content: center;
        width: 100%;
      }
    }
  }
  .body{
    margin-top: 4rem;
    height: 100%;
    overflow-y: auto;
    z-index: 1;
  }
}
</style>
