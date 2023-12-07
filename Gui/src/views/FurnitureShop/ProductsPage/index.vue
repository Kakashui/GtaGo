<template>
  <div class="products-page">
    <div class="title">{{loc(this.currentPage.data.name)}}<span>({{this.sortItems.length}})</span></div>
    <div class="block-sort">
      <div class="block-sort__price">
        <div class="desc">{{loc('furnitureShop_3')}}:</div>
        <div :class="[{ active : this.sortMinCost !== null && this.sortMinCost !== ''}, 'input-wrap']">{{loc('furnitureShop_4')}} <input
          type="number"
          min="0"
          placeholder="20"
          v-model.number="sortMinCost"
        ></div>
        <div :class="[{ active : this.sortMaxCost !== null && this.sortMaxCost !== '' }, 'input-wrap']">{{loc('furnitureShop_5')}} <input
          type="number"
          min="0"
          placeholder="2000000"
          v-model.number="sortMaxCost"
        ></div>
        <div 
          class="btn btn-discharge"
          v-if="this.sortMaxCost !== null && this.sortMaxCost !== '' || this.sortMinCost !== null && this.sortMinCost !== ''"
          @click="dischargeChanges"
        ></div>
      </div>
      <div class="block-sort__priority">
        <div class="selected-value">
          <span v-if="this.sortCost === 0">{{loc('furnitureShop_6')}}</span>
          <span v-else>{{loc('furnitureShop_7')}}</span>
        </div>
        <div class="switch">
          <div class="value" @click="setSortCost(0)">{{loc('furnitureShop_6')}}</div>
          <div class="value" @click="setSortCost(1)">{{loc('furnitureShop_7')}}</div>
        </div>
      </div>
    </div>
    <div class="products-list">
      <products-list-item
        v-for="item in sortItems"
        :key="item.id"
        :item="item"
      />
    </div>
  </div>
</template>

<script>
import { mapState, mapMutations, mapGetters } from 'vuex'
import ProductsListItem from './ProductsListItem'

export default {
  name: 'ProductsPage',

  components: {
    ProductsListItem
  },

  props: {
    currentPage: Object
  },

  data: function() {
    return {
      sortMinCost: null,
      sortMaxCost: null,
      sortCost: 0,
    }
  },

  computed: {
    ...mapState('furnitureShop', ['categories', 'productsList', 'basket']),
    ...mapGetters('localization', ['loc']),
    sortItems: function() {
      let array = this.productsList.filter(i=>i.category === this.currentPage.data.key)
      
      if (this.currentPage.data.key === 'all') {
        array = this.productsList
      }

      if (this.sortMinCost !== null && this.sortMinCost !== '') {
        array = array.filter(i=>i.cost >= this.sortMinCost)
      }

      if (this.sortMaxCost !== null && this.sortMaxCost !== '') {
        array = array.filter(i=>i.cost <= this.sortMaxCost)
      }

      if (this.sortCost === 1) {
        array = array.sort(function(a, b) {
          return a.cost - b.cost;
        })
      } else {
        array = array.sort(function(a, b) {
          return b.cost - a.cost;
        })
      }

      return array
    }    
  },

  methods: {
    ...mapMutations('furnitureShop', ['putInBasket']),
    setSortCost: function(value) {
      this.sortCost = value
    },
    dischargeChanges: function() {
      this.sortMinCost = null
      this.sortMaxCost = null
    }
  }
}
</script>

<style lang="scss" scoped>
.products-page{
  display: flex;
  flex-flow: column;
  padding: 0 10rem 1rem 10rem;
  width: 100%;
  min-height: 100%;
  background: #F2F3F5;
  .title{
    margin-top: 2rem;
    font-weight: bold;
    font-size: 2.2rem;
    line-height: 2.55rem;
    span{
      font-size: 1rem;
      line-height: 1.15rem;
      font-weight: normal;
      color: #7E858B;
      margin-left: .2rem;
    }
  }
  .block-sort{
    display: flex;
    align-items: center;
    justify-content: space-between;
    background: #FFFFFF;
    padding: 1.2rem 1.6rem;
    width: 100%;
    margin: 2rem 0;
    color: #7B8288;
    &__price{
      display: flex;
      align-items: center;
      .desc{
        margin-right: .4rem;
        font-weight: bold;
      }
      .input-wrap{
        position: relative;
        display: flex;
        align-items: center;
        &.active{
          input{
            border: 1px solid #CBF2ED;
            transition: .2s;
          }
        }
        &:before{
          content: '$';
          position: absolute;
          font-size: 1rem;
          line-height: 1.15rem;
          right: 6.1rem;
        }
        input{
          border:1px solid rgba(123, 130, 136, 0.6);
          width: 6.7rem;
          box-sizing: border-box;
          border-radius: .1rem;
          padding: .2rem 0 .2rem 1.2rem;
          margin: 0 .4rem;
          font-size: 1rem;
          line-height: 1.15rem;
          color: #7B8288;
          text-align: left;
          background: transparent;
          z-index: 2;
          &::placeholder{
            color: #b1bac0;
          }
          &::-webkit-outer-spin-button, &::-webkit-inner-spin-button {
            -webkit-appearance: none;
          }
        }
      }
      .btn-discharge{
        width: 1.5rem;
        height: 1.5rem;
        background-image: url('/img/furnitureShop/discharge.svg');
        background-size: contain;
        background-position: center;
        background-repeat: no-repeat;
      }
    }
    &__priority{
      display: flex;
      align-items: center;
      position: relative;
      min-width: 10.8rem;
      &:hover{
        .switch{
          height: 5.6rem;
          opacity: 1;
          transition: .2s;
        }
      }
      .selected-value{
        width: 100%;
        display: flex;
        align-items: center;
        padding-right: 2.2rem;
        font-weight: bold;
        font-size: 1rem;
        line-height: 1.7rem;
        position: relative;
        &:after{
          content: '';
          position: absolute;
          right: 0;
          width: 1.2rem;
          height: 1.2rem;
          background-image: url('/img/furnitureShop/arrow-down.svg');
          background-size: contain;
          background-position: center;
          background-repeat: no-repeat;
        }
      }
      .switch{
        display: flex;
        flex-flow: column;
        align-items: center;
        background: #F2F3F5;
        border: .1rem solid #53D3C3;
        border-radius: .1rem;
        padding: .5rem;
        position: absolute;
        left: 0;
        bottom: 0;
        transform: translateY(100%);
        height: 0;
        opacity: 0;
        overflow: hidden;
        z-index: 3;
        .value{
          width: 100%;
          padding: .3rem .5rem;
          font-size: 1rem;
          line-height: 1.5rem;
          color: #7B8288;
          border-radius: .1rem;
          background: #fff;
          white-space: pre;
          margin-bottom: .2rem;
          &:last-child{
            margin-bottom: 0;
          }
          &:hover{
            color: #fff;
            background: #333333;
          }
        }
      }
    }
  }
  .products-list{
    display: grid;
    grid-template-columns: repeat(4, 1fr);
    grid-auto-rows: 16.25rem;
    grid-row-gap: 2rem;
    grid-column-gap: 1rem;
  }
}
</style>
