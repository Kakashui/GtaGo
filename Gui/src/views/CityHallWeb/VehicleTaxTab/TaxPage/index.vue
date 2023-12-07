<template>
  <div class="page">
    <div class="page__title">{{loc('cityHallWeb_132')}}</div>
    <div class="current-tax">
      <div class="current-tax__desc">{{loc('cityHallWeb_134')}}:</div>
      <div 
        class="current-tax__value"
        v-if="currentTax"
      >$ {{currentTax}}</div>
      <div 
        class="current-tax__value"
        v-else
      >$ 0</div>
    </div>
    <template v-if="currentTax">
      <div class="page__desc">{{loc('cityHallWeb_11')}}</div>
      <Payment
        :currentPayment="currentPayment"
        :setCurrentPayment="setCurrentPayment"
      />
      <div 
        class="page__btn btn"
        @click="sendForm"
      >{{loc('cityHallWeb_135')}}</div>
    </template>
  </div>
</template>

<script>
import Payment from '../../components/Payment'
import { mapGetters, mapState } from 'vuex'
export default {
  name: 'TaxPage',

  components: {
    Payment
  },

  data: function() {
    return{ 
      currentTax: null,
      currentPayment: 0, // 0 - money , 1 - card
    }
  },

  methods: {
    sendForm: function() {
      window.mp.trigger('cityHallWeb:sendFormTaxPage', this.currentPayment)
    },

    setCurrentTax: function(value) {
      this.currentTax = value
    },

    setCurrentPayment: function(value) {
      this.currentPayment = value
    }
  },

  computed: {
    ...mapState('cityHallWeb', ['tax']),
    ...mapGetters('localization', ['loc'])
  },

  mounted() {
    this.setCurrentTax(this.tax)
  }
}
</script>

<style lang="scss" scoped>
.city-hall-web{
  .body-tab{
    .page{
      &__title{
        margin-bottom: 1.2rem;
      }
      .current-tax{
        background: #F3F5F7;
        border-radius: .3rem;
        min-height: 5.2rem;
        height: 5.2rem;
        padding: 0 2.1rem;
        width: 100%;
        display: flex;
        align-items: center;
        justify-content: flex-start;
        font-family: Roboto;
        margin-bottom: 2.8rem;
        &__desc{
          font-style: normal;
          font-weight: 500;
          font-size: .8rem;
          text-transform: uppercase;
          color: #46555F;
          margin-right: 1.2rem;
        }
        &__value{
          font-style: normal;
          font-weight: bold;
          font-size: 1rem;
          text-transform: uppercase;
          color: #E00B29;
        }
      }
      .payment-methods{
        display: flex;
        margin-bottom: 1.5rem;
        &__item{
          display: flex;
          align-items: center;
          padding: .3rem .2rem;
          border-bottom: .1rem solid transparent;
          margin-right: 3.2rem;
          &.active{
            border-color: #E00B29;
            svg{
              fill: #E00B29;
            }
            .text{
              color: #E00B29;
            }
          }
          &:last-child{
            margin-right: 0;
          }
          svg{
            fill: rgba(70, 85, 95, 0.6);
            width: 1.2rem;
            height: 1.2rem;
            margin-right: .6rem;
          }
          .text{
            color: rgba(70, 85, 95, 0.6);
            font-family: Roboto;
            font-style: normal;
            font-weight: bold;
            font-size: .8rem;
            text-transform: uppercase;
          }
        }
      }
    }
  }
}
</style>
