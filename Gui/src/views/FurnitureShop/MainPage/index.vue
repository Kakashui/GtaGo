<template>
  <div class="main-page">
    <div class="main-page__banner">
      <div class="title">{{loc('furnitureShop_9')}} desk.store!</div>
      <div class="btn btn-go" @click="setCurrentPage('ProductsPage', { name: 'shop:furn:nav:1', key: 'all'})">{{loc('furnitureShop_10')}}</div>
    </div>
    <div class="main-page__offers">
      <div class="offer">
        <div class="offer__nav">
          <div class="offer__nav-info">
            <div class="title">{{loc('furnitureShop_11')}}</div>
            <div class="btn btn-go" @click="setCurrentPage('ProductsPage', { name: 'shop:furn:nav:2', key: 'sofas' })">{{loc('furnitureShop_12')}}</div>
          </div>
        </div>
        <div class="offer__list">
          <div 
            class="item"
            v-for="item in arraySpecialFirst"
            :key="item.id"
            @click="showItem(item.key, item.name, item.cost, item.desc)"
          >
            <div class="img" :style="{backgroundImage: 'url(/img/furnitureShop/items/'+item.key+'.png)'}"></div>
            <div class="info">
              <div class="info__icon"></div>
              <div class="info__cost">$ {{item.cost * pricePart}}</div>
              <div class="info__name">{{item.name}}</div>
            </div>
          </div>
        </div>
      </div>
      <div class="offer">
        <div class="offer__list">
          <div 
            class="item"
            v-for="item in arraySpecialSecond"
            :key="item.id"
            @click="showItem(item.key, item.name, item.cost, item.desc)"
          >
            <div class="img" :style="{backgroundImage: 'url(/img/furnitureShop/items/'+item.key+'.png)'}"></div>
            <div class="info">
              <div class="info__icon"></div>
              <div class="info__cost">$ {{item.cost * pricePart}}</div>
              <div class="info__name">{{item.name}}</div>
            </div>
          </div>
        </div>
        <div class="offer__nav">
          <div class="offer__nav-info">
            <div class="title">{{loc('furnitureShop_13')}}</div>
            <div class="btn btn-go" @click="setCurrentPage('ProductsPage', {name: 'shop:furn:nav:3', key: 'desks'})">{{loc('furnitureShop_12')}}</div>
          </div>
        </div>
      </div>
    </div>
    <div class="main-page__new">
      <div class="main-page__new-title">{{loc('furnitureShop_14')}}</div>
      <div class="main-page__new-nav">
        <div
          :class="[{ active: item.key === currentList }, 'item']"
          v-for="item in currentNav"
          :key="item.id"
          @click="setCurrentList(item.key)"
        >
          <div 
            v-if="item.key === currentList"
            class="icon"
            :style="{backgroundImage: 'url(/img/furnitureShop/icon-'+ item.key +'-active.svg)'}"
          ></div>
          <div 
            v-else
            class="icon"
            :style="{backgroundImage: 'url(/img/furnitureShop/icon-'+ item.key +'.svg)'}"
          ></div>
          <div class="text">{{item.name}}</div>
        </div>
      </div>
      <div class="current-list">
        <products-list-item
          v-for="item in arrayCurrentList"
          :item="item"
          :key="item.id"
        />
      </div>
    </div>
    <div class="modal-wrap" v-if="showModal">
      <div class="modal">
        <div
          class="btn modal__btn-close"
          @click="closeModal"
        ></div>
          <div class="img" :style="{backgroundImage: 'url(/img/furnitureShop/items/'+this.currentItem.key+'.png)'}"></div>
          <div class="info">
            <div class="info__cost">$ {{this.currentItem.cost * pricePart}}</div>
            <div class="info__name">{{this.currentItem.name}}</div>
            <div class="info__desc">{{this.currentItem.desc}}</div>
          <div 
            class="btn btn-buy" 
            @click="itemBuy"
          >{{loc('furnitureShop_8')}}</div>
          </div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapMutations, mapState } from 'vuex'
import ProductsListItem from '../ProductsPage/ProductsListItem'

export default {
  name: 'MainPage',

  components: {
    ProductsListItem
  },

  props: {
    currentPage: Object,
    setCurrentPage: Function,
  },

  data: function() {
    return {
      showModal: false,
      currentList: 'sofas',
      currentNav: [
        {
          name: 'shop:furn:nav:4',
          key: 'sofas',
        },
        {
          name: 'shop:furn:nav:2',
          key: 'beds',
        },
        {
          name: 'shop:furn:nav:3',
          key: 'desks',
        },
        {
          name: 'shop:furn:nav:5',
          key: 'floorlamps',
        },
      ]
    }
  },

  computed: {
    ...mapState('furnitureShop', ['productsList', 'currentItem', 'pricePart']),
    ...mapGetters('localization', ['loc']),
    arraySpecialFirst: function() {
      let array = this.productsList.filter(i=>i.category === 'sofas')
      let sortArray = array.slice(-4)
      return sortArray
    },
    arraySpecialSecond: function() {
      let array = this.productsList.filter(i=>i.category === 'desks')
      let sortArray = array.slice(-4)
      return sortArray
    },
    arrayCurrentList: function() {
      let array = this.productsList.filter(i=>i.category === this.currentList)
      let sortArray = array.slice(-4)
      return sortArray
    }
  },

  methods: {
    ...mapMutations('furnitureShop', ['setCurrentItem', 'putInBasket']),
    showItem: function(key, name, cost, desc) {
      this.showModal = true
      this.setCurrentItem({key: key, name: name, cost: cost, desc: desc})
    },
    closeModal: function() {
      this.showModal = false
    },
    itemBuy: function() {
      this.closeModal()
      this.putInBasket({name: this.currentItem.name, cost: this.currentItem.cost, key: this.currentItem.key})
    },
    setCurrentList: function(key) {
      this.currentList = key
    }
  }
}
</script>

<style lang="scss" scoped>
.main-page{
  display: flex;
  flex-flow: column;
  padding: 0 7.35rem;
  width: 100%;
  &__banner{
    width: 100%;
    min-height: 21.25rem;
    padding: 3.6rem 45rem 3.6rem 2.6rem;
    background-image: url('/img/furnitureShop/banner.png');
    background-size: cover;
    background-repeat: no-repeat;
    background-position: center;
    position: relative;
    display: flex;
    flex-flow: column;
    align-items: flex-start;
    justify-content: flex-start;
    &:after{
      content: '';
      background: rgba(0, 0, 0, 0.4);
      width: 100%;
      height: 100%;
      position: absolute;
      left: 0;
      top: 0;
      z-index: 1;
    }
    .title, .btn-go{
      z-index: 2;
    }
    .title{
      font-weight: bold;
      font-size: 3rem;
      line-height: 3.5rem;
      text-transform: uppercase;
      color: #fff;
    }
    .btn-go{
      margin-top: 1.4rem;
      background: #53D3C3;
      border-radius: .2rem;
      padding: 1rem 1.6rem;
      font-size: 1.1rem;
      color: #F2F3F5;
    }
  }
  &__offers{
    width: 100%;
    display: flex;
    flex-flow: column;
    margin: 6rem 0;
    .offer{
      display: grid;
      justify-content: center;
      grid-template-columns: repeat(2, 37.5rem);
      grid-template-rows: 35rem;
      grid-gap: 1rem;
      margin-bottom: 2rem;
      &:last-child{
        margin-bottom: 0;
      }
      &>div{
        position: relative;
        &:before, &:after{
          position: absolute;
          -webkit-writing-mode: vertical-rl; 
          writing-mode:tb-rl;
          text-transform: uppercase;
          transform-origin: 50% 50%;
          transform: rotate(180deg);
        }
      }
      &:nth-child(2n+1){
        &>div{
          &:first-child{
            &:before{
              content: '';
              width: .6rem;
              height: .6rem;
              background: rgba(36, 72, 106, 0.4);
              left: -2.4rem;
              bottom: 0;
            }
            &:after{
              content: 'Desk.store';
              font-size: 1rem;
              line-height: 1.15rem;
              color: rgba(36, 72, 106, 0.4);
              left: -2.4rem;
              top: 0;
            }
          }
          &:last-child{
            &:after{
              content: '';
              width: .6rem;
              height: .6rem;
              background: rgba(36, 72, 106, 0.4);
              right: -2.4rem;
              top: 0;
            }
            &:before{
              content: 'Desk.store';
              font-size: 1rem;
              line-height: 1.15rem;
              color: rgba(36, 72, 106, 0.4);
              right: -2.4rem;
              bottom: 0;
            }
          }
        }
      }
      &:nth-child(2n){
        &>div{
          &:last-child{
            &:before{
              content: '';
              width: .6rem;
              height: .6rem;
              background: rgba(36, 72, 106, 0.4);
              right: -2.4rem;
              top: 0;
            }
            &:after{
              content: 'Desk.store';
              font-size: 1rem;
              line-height: 1.15rem;
              color: rgba(36, 72, 106, 0.4);
              right: -2.4rem;
              bottom: 0;
            }
          }
          &:first-child{
            &:after{
              content: '';
              width: .6rem;
              height: .6rem;
              background: rgba(36, 72, 106, 0.4);
              left: -2.4rem;
              bottom: 0;
            }
            &:before{
              content: 'Desk.store';
              font-size: 1rem;
              line-height: 1.15rem;
              color: rgba(36, 72, 106, 0.4);
              left: -2.4rem;
              top: 0;
            }
          }
        }
      }
      &__nav{
        background-image: url('/img/furnitureShop/offer-bg.png');
        background-size: contain;
        background-repeat: no-repeat;
        background-position: center;
        &-info{
          padding: 2rem;
          overflow: hidden;
          height: 100%;
          width: 100%;
          display: flex;
          flex-flow: column;
          align-content: flex-start;
          justify-content: flex-start;
          position: relative;
          &:after{
            content: '';
            width: 32.5rem;
            height: 21.2rem;
            border-radius: 50%;
            background: rgba(0, 0, 0, 0.2);
            backdrop-filter: blur(.5rem);
            transform-origin: 50% 50%;
            transform: rotate(-13.92deg);
            position: absolute;
            left: -5.6rem;
            top: -6.9rem;
            z-index: 1;
          }
          .title, .btn-go{
            position: relative;
            z-index: 2;
          }
          .title{
            font-weight: bold;
            font-size: 2rem;
            line-height: 2.3rem;
            color: #FFFFFF;
          }
        }
        .btn-go{
          background: #53D3C3;
          border-radius: .2rem;
          font-weight: bold;
          font-size: 1rem;
          color: #FFFFFF;
          width: fit-content;
          padding: 1rem 1.2rem;
          margin-top: 1.05rem;
        }
      }
      &__list{
        display: grid;
        grid-template-columns: repeat(2, 18.25rem);
        grid-auto-rows: 17rem;
        grid-gap: 1rem;
        .item{
          padding: 1rem;
          display: flex;
          align-items: center;
          justify-content: center;
          position: relative;
          background: #F2F3F5;
          .img{
            width: 100%;
            height: 100%;
            background-size: contain;
            background-position: center;
            background-repeat: no-repeat;
          }
          .info{
            position: absolute;
            width: calc(100% - 3rem);
            height: calc(100% - 3rem);
            background: rgba(83, 211, 195, 0.3);
            backdrop-filter: blur(1rem);
            display: flex;
            align-items: center;
            justify-content: center;
            flex-flow: column;
            padding: 0 3rem;
            opacity: 0;
            transition: .2s;
            &__icon{
              width: .9rem;
              height: .9rem;
              background-image: url('/img/furnitureShop/loupe.svg');
              background-size: contain;
              background-repeat: no-repeat;
              background-position: center;
            }
            &__cost{
              font-weight: bold;
              font-size: 1rem;
              line-height: 1.15rem;
              color: #FFFFFF;
              margin: .5rem 0 .2rem 0;
            }
            &__name{
              font-size: 1rem;
              line-height: 1.15rem;
              text-align: center;
              text-transform: uppercase;
              color: #000000;
            }
          }
          &:hover{
            .info{
              opacity: 1;
              transition: .2s;
            }
          }
        }
      }
    }
  }
  &__new{
    display: flex;
    align-items: center;
    flex-flow: column;
    font-weight: bold;
    color: #000000;
    &-title{
      font-size: 2.2rem;
      line-height: 2.55rem;
    }
    &-nav{
      display: flex;
      align-items: center;
      margin: 2rem 0 2.5rem 0;
      justify-content: center;
      .item{
        display: flex;
        flex-flow: column;
        align-items: center;
        margin-right: 3rem;
        &.active{
          .text{
            color: #ABB0B5;
          }
        }
        &:last-child{
          margin-right: 0;
        }
        .icon{
          background-size: contain;
          background-position: center;
          background-repeat: no-repeat;
          width: 2.2rem;
          height: 2.2rem;
          margin-bottom: .5rem;
        }
        .text{
          font-size: 1rem;
          line-height: 1.15rem;
        }
      }
    }
    .current-list{
      width: 100%;
      display: grid;
      margin-top: 2.5rem;
      grid-template-columns: repeat(4, 1fr);
      grid-column-gap: 1rem;
      grid-auto-rows: 16.25rem;
      margin-bottom: 4rem;
    }
  }
  .modal-wrap{
    display: flex;
    align-items: center;
    justify-content: center;
    width: 100%;
    height: 100%;
    position: fixed;
    top: 0;
    left: 0;
    background: rgba(0, 0, 0, 0.6);
    z-index: 3;
    .modal{
      display: flex;
      background: #FFFFFF;
      position: relative;
      padding: 1rem;
      align-items: center;
      min-height: 13.5rem;
      height: 13.5rem;
      &__btn-close{
        width: 1.5rem;
        height: 1.5rem;
        background-size: contain;
        background-position: center;
        background-repeat: no-repeat;
        background-image: url('/img/furnitureShop/close-modal.svg');
        position: absolute;
        right: -.45rem;
        top: 0;
        transform: translateX(100%);
      }
      .img{
        width: 13.1rem;
        height: 11.5rem;
        min-width: 13.1rem;
        background-color: #F2F3F5;
        background-size: contain;
        background-position: center;
        background-repeat: no-repeat;
        margin-right: 1rem;
      }
      .info{
        display: flex;
        flex-flow: column;
        align-items: flex-start;
        font-size: 1rem;
        line-height: 1.15rem;
        font-weight: bold;
        height: 100%;
        &__cost{
          color: #24486A;
        }
        &__name{
          color: #000000;
          margin: .5rem 0;
        }
        &__desc{
          font-weight: normal;
          color: #999999;
          height: 100%;
        }
        .btn-buy{
          background: #53D3C3;
          border-radius: .2rem;
          padding: .5rem 1rem;
          font-size: .9rem;
          line-height: 1.05rem;
        }
      }
    }
  }
}
</style>
