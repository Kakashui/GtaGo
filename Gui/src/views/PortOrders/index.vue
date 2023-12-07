<template>
  <div class="port-orders">
    <div class="header">
      <div class="header__url">
        <div class="site-tab">
          <div class="logo-head"></div>
          <div class="text">Port.com</div>
          <div class="btn btn-close"></div>
          <div class="btn btn-add"></div>
        </div>
        <div class="btn btn-back"></div>
        <div class="btn btn-forward"></div>
        <div class="btn btn-refresh"></div>
        <div class="url">www.port.com</div>
      </div>
    </div>
    <div class="body">
      <div class="header-site">
        <div class="logo">port.com</div>
        <div class="header-site-nav">
          <nav-item
            v-for="item in navItems"
            :item="item"
            :key="item.key"
            :currentPage="currentPage"
            :setCurrentPage="setCurrentPage"
          />
        </div>
      </div>
      <component 
        :is="currentPage"
        :setCurrentPage="setCurrentPage"
      />
      <div class="modal-wrap" v-if="this.showModal">
      <div class="modal">
        <div
          class="btn modal__btn-close"
          @click="closeModal"
        ></div>
        <div class="modal__header">
          <div class="info">
            <div class="info__name">{{this.currentItem.name}}</div>
            <div class="info__desc">{{this.currentItem.desc}}</div>
          </div>
          <div class="cost">${{this.currentItem.cost*this.countProduct}}</div>
        </div>
        <div class="modal__body">
          <div class="title">{{loc('PortOrders_4')}}</div>
          <div class="input-wrap">
            <input class="input" type="number" v-model="countProduct">
            <span>{{loc('PortOrders_16')}}</span>
          </div>
          <div class="btn-save" @click="addProduct()">{{loc('PortOrders_5')}}</div>
        </div>
      </div>
    </div>
    </div>
  </div>
</template>

<script>
import MainPage from './MainPage'
import ProductPage from './ProductPage'
import AboutUsPage from './AboutUsPage'
import NavItem from './NavItem'
import { mapGetters, mapMutations, mapState } from 'vuex'

export default {
  name: 'PortOrders',

  components: {
    MainPage,
    ProductPage,
    AboutUsPage,
    NavItem
  },

  data: function() {
    return {
      countProduct: 1,
      currentPage: 'MainPage',
      navItems: [
        {
          name: 'PortOrders_1',
          pageUrl: 'MainPage'
        },
        {
          name: 'PortOrders_2',
          pageUrl: 'ProductPage'
        },
        {
          name: 'PortOrders_3',
          pageUrl: 'AboutUsPage'
        }
      ]
    }
  },
  
  computed: {
    ...mapState('portOrders', ['basketTotal', 'showModal', 'currentItem']),
    ...mapGetters('localization', ['loc']),
  },

  methods: {
    ...mapMutations('portOrders', ['putInBasket', 'closeModal']),
    setCurrentPage: function(page) {
      this.currentPage = page
    },
    addProduct: function() {
      this.putInBasket({id: this.currentItem.id, count: this.countProduct})
      this.countProduct = 1
    }
  }
}
</script>

<style lang="scss" scoped>
.port-orders{
  display: flex;
  flex-flow: column;
  width: 100%;
  height: 100%;
  position: absolute;
  top: 0;
  left: 0;
  .btn{
    background-size: contain;
    background-repeat: no-repeat;
    background-position: center;
  }
  .header{
    display: flex;
    flex-flow: column;
    width: 100%;
    padding-top: 3.5rem;
    background-color: #000;
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
      padding: 0 5rem 0 1rem;
      font-size: 1rem;
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
        background-image: url('/img/siteHeader/Subtract.png');
        background-size: contain;
        background-repeat: no-repeat;
        background-position: bottom;
        padding: 0 2rem;
        .text{
          font-weight: bold;
          font-size: .9em;
          line-height: 1.05rem;
          color: #000000;
          margin: 0 4.65rem 0 .5rem;
        }
        .logo-head, .btn-close{
          width: 1.1rem;
          height: 1.1rem;
        }
        .logo-head{
          background-size: contain;
          background-repeat: no-repeat;
          background-position: center;
          background-image: url('/img/portOrders/logo-head.svg');
        }
        .btn-close{
          background-image: url('/img/siteHeader/close.svg');
          margin-right: 0;
        }
        .btn-add{
          width: 1.4rem;
          height: 1.4rem;
          margin-right: 0;
          position: absolute;
          right: 0;
          transform: translateX(1.15rem);
          background-image: url('/img/siteHeader/add.svg');
        }
      }
      &>.btn{
        width: 1.4rem;
        height: 1.4rem;
        min-width: 1.4rem;
        margin-right: 1.5rem;
        &.btn-back, &.btn-forward{
          background-image: url('/img/siteHeader/arrow.svg');
        }
        &.btn-forward{
          opacity: .5;
          transform-origin: 50% 50%;
          transform: rotate(180deg);
        }
      }
      .btn-refresh{
        background-image: url('/img/siteHeader/refresh.svg');
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
    width: 100%;
    height: 100%;
    background-image: url('/img/portOrders/bg.png');
    background-size: cover;
    display: flex;
    align-items: flex-start;
    justify-content: center;
    position: relative;
    padding-top: 5.5rem;
    overflow-y: auto;
    .header-site{
      background: #48B3EF;
      min-height: 3.5rem;
      padding-left: 20.4rem;
      position: absolute;
      top: 0;
      left: 0;
      width: 100%;
      display: flex;
      align-items: center;
      .logo{
        display: flex;
        align-items: center;
        font-weight: bold;
        font-size: 1rem;
        line-height: 1.15rem;
        text-transform: uppercase;
        color: #FFFFFF;
        padding-left: 2.2rem;
        height: 1.7rem;
        min-height: 1.7rem;
        background-size: contain;
        background-repeat: no-repeat;
        background-position: left;
        background-image: url('/img/portOrders/logo.svg');
      }
      &-nav{
        display: flex;
        align-items: center;
        margin-left: 3rem;
      }
    }
    .modal-wrap{
      width: 100%;
      height: 100%;
      display: flex;
      align-items: center;
      justify-content: center;
      position: fixed;
      top: 0;
      left: 0;
      background: rgba(0, 0, 0, 0.7);
      .modal{
        width: 19.9rem;
        height: 17.65rem;
        padding: 1rem 1.5rem;
        display: flex;
        flex-flow: column;
        justify-content: space-between;
        align-items: center;
        position: relative;
        background-image: url('/img/portOrders/paper-check.png');
        background-size: contain;
        background-repeat: no-repeat;
        background-position: center;
        &__btn-close{
          position: absolute;
          top: 0;
          right: -.5rem;
          transform: translateX(100%);
          width: 1.7rem;
          height: 1.7rem;
          background-image: url('/img/portOrders/btn-close-modal.png');
        }
        &__header{
          display: flex;
          align-items: flex-start;
          justify-content: space-between;
          width: 100%;
          .info{
            display: flex;
            flex-flow: column;
            font-size: .85rem;
            line-height: 1rem;
            &__name{
              font-weight: bold;
              color: #000000;
            }
            &__desc{
              color: rgba(0, 0, 0, 0.3);
            }
          }
          .cost{
            text-transform: uppercase;
            font-weight: bold;
            font-size: 1rem;
            color: #000000;
          }
        }
        &__body{
          display: flex;
          flex-flow: column;
          align-items: center;
          justify-content: center;
          width: 100%;
          .title{
            font-weight: bold;
            font-size: 1rem;
            line-height: 1.15rem;
            color: #000000;
          }
          .input-wrap{
            display: flex;
            align-items: flex-end;
            justify-content: center;
            margin: 1rem 0 3rem 0;
            .input, span{
              font-size: 1rem;
              line-height: 1.15rem;
              color: #000000;
            }
            .input{ 
              background: transparent;
              outline: none;
              border: 1px solid #48B3EF;
              border-radius: .2rem;
              width: 11.4rem;
              margin-right: .5rem;
              padding: .5rem;
              &::-webkit-outer-spin-button,
              &::-webkit-inner-spin-button {
                -webkit-appearance: none;
              }
            }
            &:after{
              content: '';
              height: 1px;
              width: calc(100% - 3rem);
              background: #EDF0F2;
              position: absolute;
              bottom: 4.15rem;
            }
          }
          .btn-save{
            align-self: flex-end;
            background: #48B3EF;
            border-radius: .2rem;
            font-size: 1rem;
            color: #fff;
            padding: .5rem .6rem;
            transition: .2s;
            &:hover{
              transition: .2s;
              transform: scale(1.1);
            }
          }
        }
      }
    }
  }
}
</style>
