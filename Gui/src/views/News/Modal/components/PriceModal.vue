<template>
  <div class="price-modal">
    <div class="price-modal__wrap">
      <div class="price-modal__wrap-title">{{loc('news:price:1')}}</div>
      <div class="price-modal__wrap-info">
        <div class="price-modal__wrap-info-desc">{{loc('news:price:2')}}</div>
        <input 
          type="number"
          class="price-modal__wrap-info-input"
          v-model="currentPricePerSymbol"
        >
      </div>
      <div class="price-modal__wrap__btns">
        <div 
          class="btn price-modal__wrap__btns-btn price-modal__wrap__btns-btn--apply"
          @click="savePricePerSymbol"
        >{{loc('news:price:3')}}</div>
        <div 
          class="btn price-modal__wrap__btns-btn"
          @click="setModal({show: false, currentModal: null, data: null})"
        >{{loc('news:price:4')}}</div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapMutations, mapState } from 'vuex'
export default {
  name: 'PriceModal',

  data: function() {
    return {
      currentPricePerSymbol: null
    }
  },

  computed: {
    ...mapState('news', ['pricePerSymbol']),
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    ...mapMutations('news', ['setModal']),
    savePricePerSymbol: function() {
      if (this.currentPricePerSymbol) {
        window.mp.trigger('news:savePricePerSymbol', this.currentPricePerSymbol)
        this.setModal({show: false, currentModal: null, data: null})
      }
    }
  },

  mounted() {
    this.currentPricePerSymbol = this.pricePerSymbol
  }
}
</script>

<style lang="scss" scoped>
.price-modal{
  display: flex;
  flex-direction: column;
  align-items: center;
  background: #FFFFFF;
  border-radius: 1rem;
  padding: 2rem 0;
  width: 29.55rem;
  &__wrap{
    display: flex;
    flex-direction: column;
    &-title, &-info{
      margin-bottom: 1rem;
    }
    &-title{
      font-weight: bold;
      font-size: 1.5rem;
      line-height: 1.7rem;
      text-align: center;
      color: #000000;
    }
    &-info{
      width: 100%;
      display: flex;
      flex-direction: column;
      background: rgba(175, 185, 197, 0.12);
      border-radius: .3rem;
      &-desc, &-input{
        padding: .6rem 0;
      }
      &-desc{
        width: 100%;
        font-weight: normal;
        font-size: .8rem;
        line-height: .9rem;
        text-align: center;
        color: #AFB9C5;
        border-bottom: 1px solid rgba(175, 185, 197, 0.2);
      }
      &-input{
        background-color: transparent;
        width: 100%;
        text-align: center;
        font-weight: bold;
        font-size: 1rem;
        color: #000000;
        &::-webkit-outer-spin-button,
        &::-webkit-inner-spin-button {
          -webkit-appearance: none;
        }
      }
    }
    &__btns{
      display: flex;
      align-items: center;
      &-btn{
        width: 6rem;
        height: 2.5rem;
        border-radius: 6px;
        border: 1px solid #E30000;
        color: #E30000;
        margin-right: 1rem;
        transition: .3s;
        &:hover{
          filter: drop-shadow(0 0 .15rem #D92C2C);
        }
        &:last-child{
          margin-right: 0;
        }
        &--apply{
          background: linear-gradient(0deg, #E30000, #E30000);
          color: #fff;
          border: none;
          &:hover{
            box-shadow: 0 0 .6rem #D92C2C;
          }
        }
      }
    }
  }
}
</style>
