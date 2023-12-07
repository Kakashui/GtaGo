<template>
  <div class="bar">
    <div class="bar__sale">
      <div class="bar__sale-title">{{loc('bar_0')}}</div>
      <div class="bar__gallery">
        <div 
          class="bar__gallery_products"
          :style="galleryStyles"
        >
          <SaleProduct
            v-for="saleProduct in saleProducts"
            :key="saleProduct.id"
            :saleProduct="saleProduct"
          />
        </div>
        <div class="bar__gallery_nav">
          <div 
            :class="[{active: index === currentGalleryIndex}, 'bar__gallery_nav-item']"
            v-for="(item, index) in saleProducts.length"
            :key="index"
            @click="setCurrentGalleryIndex(index)"
          ></div>
        </div>
      </div>
    </div>
    <div class="bar__products">
      <div class="bar__products-scroll">
        <BarProduct
          v-for="barProduct in productsList"
          :key="barProduct.id"
          :barProduct="barProduct"
        />
      </div>
      <div class="bar__decorate bar__decorate--first"></div>
      <div class="bar__decorate bar__decorate--second"></div>
      <div class="bar__decorate bar__decorate--third"></div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
import BarProduct from './BarProduct'
import SaleProduct from './SaleProduct'
export default {
  name: 'Bar',

  components: {
    BarProduct,
    SaleProduct
  },

  data: function() {
    return {
      currentGalleryIndex: 0,
    }
  },

  computed: {
    ...mapState('bar', ['productsList']),
    ...mapGetters('localization', ['loc']),
    saleProducts: function() {
      const list = this.productsList
      return list.filter(element => element.sale)
    },
    galleryStyles: function() {
      return {
        transform: `translateX(calc(-26.75rem * ${this.currentGalleryIndex}))`, 
        width: `calc(26.75rem * ${this.saleProducts.length})`
      }
    }
  },

  methods: {
    setCurrentGalleryIndex: function(index) {
      this.currentGalleryIndex = index
    }
  },

  mounted() {
    setInterval(() => {
      if (this.currentGalleryIndex < this.saleProducts.length - 1) {
        this.currentGalleryIndex ++
      } else {
        this.currentGalleryIndex = 0
      }
    }, 5000);
  }
}
</script>

<style lang="scss" scoped>
.bar{
  position: absolute;
  width: 55.45rem;
  height: 34.4rem;
  left: 50%;
  top: 50%;
  transform: translateX(-50%) translateY(-50%);
  display: flex;
  text-transform: uppercase;
  &__sale{
    background-color: #000000;
    width: 26.75rem;
    min-width: 26.75rem;
    position: relative;
    &:before{
      content: '';
      width: 2px;
      height: 100%;
      position: absolute;
      right: 0;
      top: 0;
      background-color: black;
      transform: translateX(50%);
    }
    &-title{
      position: absolute;
      left: 2rem;
      top: 2rem;
      font-weight: bold;
      font-size: 2.5rem;
      line-height: 2.5rem;
      text-transform: uppercase;
      color: rgba(255, 255, 255, 0.3);
      z-index: 2;
    }
  }
  &__gallery{
    height: 100%;
    width: 100%;
    overflow: hidden;
    z-index: 1;
    &_products{
      height: 100%;
      display: flex;
      transition: .4s;
    }
    &_nav{
      display: flex;
      align-items: center;
      justify-content: center;
      position: absolute;
      bottom: 2rem;
      left: 50%;
      transform: translateX(-50%);
      &-item{
        width: .3rem;
        height: .3rem;
        background: rgba(255, 255, 255, 0.3);
        border-radius: .5rem;
        margin-right: .6rem;
        transition: .3s;
        &.active{
          width: 1rem;
          background-color: #2a91c2;
        }
        &:last-child{
          margin-right: 0;
        }
      }
    }
  }
  &__products{
    width: 100%;
    background-size: contain;
    background-repeat: no-repeat;
    background-position: center;
    background-image: url('/img/bar/bg.png');
    overflow: hidden;
    position: relative;
    &-scroll{
      height: 100%;
      display: grid;
      grid-template-columns: repeat(3, 1fr);
      grid-auto-rows: 12.15rem;
      grid-row-gap: 3.1rem;
      padding: 4rem 0;
      overflow-y: auto;
      &::-webkit-scrollbar{
        background-color: transparent;
        width: .2rem;
        &-thumb{
          background-color: #414040;
        }
      }
    }
  }
  &__decorate{
    position: absolute;
    background: radial-gradient(50% 50% at 50% 50%, rgba(42, 145, 194, 0) 0%, rgba(42, 145, 194, 0.68) 100%);
    border-radius: 50%;
    transform-origin: 47% 43%;
    z-index: 0;
    &--first{
      width: 8.45rem;
      height: 8.45rem;
      right: -3.6rem;
      top: -2.05rem;
      animation: brownianMotion 12s linear infinite;
    }
    &--second{
      width: 3.65rem;
      height: 3.65rem;
      right: -.3rem;
      top: 3.4rem;
      box-shadow: 0 .2rem 3rem rgba(0, 0, 0, 0.5);
      animation: brownianMotion 17s linear infinite;
    }
    &--third{
      width: 8.45rem;
      height: 8.45rem;
      bottom: -3.6rem;
      left: -2.5rem;
      animation: brownianMotion 20s linear infinite;
    }
  }
}
@keyframes brownianMotion {
  0% {
    transform: translateX(0%) translateY(0%) scale(1) rotate(0deg);
  }
  20% {
    transform: translateX(5%) translateY(6%) scale(1.05) rotate(72deg);
  }
  40% {
    transform: translateX(-4%) translateY(-2%) scale(.9) rotate(144deg);
  }
  60% {
    transform: translateX(4%) translateY(3%) scale(1.07) rotate(216deg);
  }
  80% {
    transform: translateX(-3%) translateY(-1%) scale(.86) rotate(288deg);
  }
  100% {
    transform: translateX(0%) translateY(0%) scale(1) rotate(360deg);
  }
}
@keyframes rotation {
  from {
    transform: rotate(0deg);
  }
  to {
    transform: rotate(360deg);
  }
}
</style>
