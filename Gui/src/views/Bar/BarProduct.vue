<template>
  <div class="bar-product">
    <div 
      class="bar-product-image"
      :style="{backgroundImage: `url(/img/bar/products/${barProduct.img})`}"
    ></div>
    <div class="bar-product__info">
      <div class="bar-product-name">{{barProduct.name}}</div>
      <div class="bar-product-price">{{barProduct.price}}</div>
    </div>
    <div 
      class="bar-product-btn"
      @click="buyProduct"
    >{{loc('bar_1')}}</div>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
export default {
  name: 'BarProduct',

  props: {
    barProduct: Object
  },

  computed: {
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    buyProduct: function() {
      window.mp.triggerServer('alco:bar:buy', this.barProduct.id)
    }
  }
}
</script>

<style lang="scss" scoped>
.bar-product{
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  z-index: 1;
  position: relative;
  color: #FFFFFF;
  $block: &;
  &-image{
    width: 100%;
    height: 8.1rem;
    background-size: contain;
    background-repeat: no-repeat;
    background-position: center;
    margin-bottom: 1rem;
  }
  &__info{
    display: flex;
    flex-direction: column;
    align-items: flex-start;
  }
  &-name{
    font-weight: normal;
    font-size: .9rem;
    line-height: 1.05rem;
    color: rgba(255, 255, 255, 0.5);
    margin-bottom: .5rem;
  }
  &-price{
    font-weight: bold;
    font-size: 1.3rem;
    line-height: 1.5rem;
  }
  &-btn{
    display: flex;
    align-items: center;
    justify-content: center;
    position: absolute;
    width: 5.6rem;
    height: 5.6rem;
    border-radius: 50%;
    font-weight: bold;
    font-size: .8rem;
    line-height: .9rem;
    text-align: center;
    letter-spacing: 0.1em;
    text-transform: uppercase;
    background: radial-gradient(50% 50% at 50% 50%, rgba(17, 15, 15, 0.65) 0%, #2A91C2 100%);
    transition: .3s;
    opacity: 0;
  }
  &:hover{
    & #{$block}-btn{
      opacity: 1;
    }
  }
}
</style>
