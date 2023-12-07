<template>
  <div class="bank-credit-options">
    <div class="bank-credit-options__range">
      <div class="bank-credit-options-title">{{loc('bank:menu:154')}}</div>
      <div class="bank-credit-options__range-wrap">
        <div class="bank-credit-options__range-value">$ {{ creditSumm.toLocaleString('ru') }}</div>
        <div class="bank-credit-options__range-prompt">
          <div class="bank-credit-options__range-desc">{{loc('bank:menu:155')}} $ {{ creditOptionsData.minAmount.toLocaleString('ru') }}</div>
          <div class="bank-credit-options__range-desc">{{loc('bank:menu:156')}} $ {{ creditOptionsData.maxAmount.toLocaleString('ru') }}</div>
        </div>
        <range-slider
          :min="creditOptionsData.minAmount"
          :max="creditOptionsData.maxAmount"
          step="1000"
          v-model.number="creditSumm"
        />
      </div>
    </div>
    <div class="bank-credit-options__range">
      <div class="bank-credit-options-title">{{loc('bank:menu:157')}}</div>
      <div class="bank-credit-options__range-wrap">
        <div class="bank-credit-options__range-value">{{loc(`bank:menu:158@${creditDays}`)}}</div>
        <div class="bank-credit-options__range-prompt">
          <div class="bank-credit-options__range-desc">{{loc('bank:menu:155')}} {{ creditOptionsData.minDays }}</div>
          <div class="bank-credit-options__range-desc">{{loc('bank:menu:156')}} {{ creditOptionsData.maxDays }}</div>
        </div>
        <range-slider
          :min="creditOptionsData.minDays"
          :max="creditOptionsData.maxDays"
          step="5"
          v-model.number="creditDays"
        />
      </div>
    </div>
    <div class="bank-credit-options__choice">
      <div class="bank-credit-options-title">{{loc('bank:menu:159')}}</div>
      <div 
        v-for="item in paymentTypes"
        :key="item.key"
        :class="[{ active: item.key === currentPayment }, 'bank-credit-options__choice-item']"
        @click="setCurrentPayment(item.key)"
      >
        <div class="radio"></div>
        <div class="text">{{item.text}}</div>
        <div class="bank-prompt-wrap">
          <div class="bank-prompt">{{item.desc}}</div>
        </div>
      </div>
      <div 
        class="btn-main"
        @click="calcCredit"
      >{{loc('bank:menu:160')}}</div>
    </div>
    <div class="bank-credit-options__outcome">
      <div class="bank-credit-options__outcome-col">
        <div class="bank-credit-options__outcome-title">{{loc('bank:menu:161')}}</div>
        <div class="bank-credit-options__outcome-value">{{getRate()}}%</div>
      </div>
      <div class="bank-credit-options__outcome-col">
        <div class="bank-credit-options__outcome-title">{{loc('bank:menu:162')}}</div>
        <div class="bank-credit-options__outcome-value">$ {{calculatedAmount()}}</div>
      </div>
    </div>
    <div 
      class="bank-credit-options-btn btn-main"
      @click="createCredit"
    >{{loc('bank:menu:163')}}</div>
  </div>
</template>

<script>
import RangeSlider from 'vue-range-slider'
import 'vue-range-slider/dist/vue-range-slider.css'
import { mapState, mapGetters } from 'vuex'
export default {
  name: 'BankCreditOptions',
  components: {
    RangeSlider,
  },
  props: {
    isPledged: Boolean
  },
  data() {
    return { 
      creditSumm: 0,
      creditDays: 0,
      currentPayment: null,
      paymentTypes: null
    }
  },
  computed: {
    ...mapState('bank/credit', ['currentCreditOptionsResult', 'creditOptionsData', 'selectedCreditProperty']),
    ...mapGetters('localization',['loc']),
  },
  methods: {
    setCurrentPayment(value) {
      this.currentPayment = value
    },
    createCredit() {
      if (this.isPledged) {
        if (this.selectedCreditProperty != null) {
          window.mp.triggerServer('bank:createCredit', this.creditSumm, this.creditDays, this.currentPayment, this.selectedCreditProperty.type, this.selectedCreditProperty.id)
        }
      }
      else {
          window.mp.triggerServer('bank:createCredit', this.creditSumm, this.creditDays, this.currentPayment, 'Invalid', 0)
      }
    },
    calcCredit() {
      if (this.isPledged && this.selectedCreditProperty != null || !this.isPledged)
        window.mp.triggerServer('bank:calcCredit', this.creditSumm, this.creditDays, this.currentPayment, this.isPledged)
    },
    getRate() {
      return this.currentCreditOptionsResult.rate > 0 ? this.currentCreditOptionsResult.rate : '-'
    },
    calculatedAmount() {
      return this.currentCreditOptionsResult.calculatedAmount > 0 ? this.currentCreditOptionsResult.calculatedAmount.toLocaleString('ru') : '-'
    }
  },
  mounted() {
    this.creditSumm = this.creditOptionsData.minAmount
    this.creditDays = this.creditOptionsData.minDays
    this.paymentTypes = [
        {
          key: 'Differentiated',
          text: this.loc('bank:menu:164'),
          desc: this.loc('bank:menu:166'),
        },
        {
          key: 'Annuity',
          text: this.loc('bank:menu:165'),
          desc: this.loc('bank:menu:167'),
        }
      ]
    this.currentPayment = this.paymentTypes[0].key
  }
}
</script>

<style lang="scss">
.bank{
  .bank-credit-options{
    &-btn.btn-main{
      position: absolute;
      left: 0;
      bottom: 0;
      width: 100%;
      height: 4rem;
      &:hover{
        box-shadow: 0 -2.5rem 3rem rgba(94, 55, 176, 0.3);
        background-color: #5E37B0;
      }
    }
  }
}
.bank-credit-options {
  width: 22.5rem;
  background: #030c20;
  border-radius: 0.8rem;
  padding: 2.5rem 2rem 6rem 2rem;
  overflow: hidden;
  position: relative;
  &-title {
    font-weight: normal;
    font-size: 1rem;
    line-height: 1.2rem;
    margin-bottom: 0.2rem;
    position: relative;
    &:before{
      content: '';
      border: .5rem solid #7E55D4;
      border-left-color: transparent;
      border-bottom-color: transparent;
      position: absolute;
      left: -1.6rem;
      transform: translateX(-100%) rotate(45deg);
      box-shadow: 0 0 1rem #7E55D4;
    }
  }
  &__range {
    display: flex;
    flex-direction: column;
    position: relative;
    font-size: 1.5rem;
    letter-spacing: 0.03em;
    margin-bottom: 3rem;
    &:last-child {
      margin-bottom: 0;
    }
    &-wrap {
      background: rgba(255, 255, 255, 0.1);
      border-radius: 0.4rem;
      padding: 0 1.5rem;
      display: flex;
      align-items: center;
      justify-content: center;
      width: 100%;
      height: 3rem;
    }
    &-prompt{
      position: absolute;
      left: 0;
      bottom: -.5rem;
      transform: translateY(100%);
      width: 100%;
      display: flex;
      justify-content: space-between;
    }
    &-desc {
      color: rgba(255, 255, 255, 0.4);
      font-weight: normal;
      font-size: 1rem;
      line-height: 1.2rem;
    }
    &-value {
      font-weight: normal;
      font-size: 1.2rem;
      line-height: 1.45rem;
    }
  }
  &__choice{
    display: flex;
    flex-direction: column;
    align-items: flex-start;
    padding: 1.5rem 0;
    border-top: 1px solid rgba(255, 255, 255, 0.2);
    border-bottom: 1px solid rgba(255, 255, 255, 0.2);
    margin-bottom: 2rem;
    .bank-credit-options-title{
      margin-bottom: .75rem;
    }
    &-item{
      display: flex;
      align-items: center;
      justify-content: center;
      margin-bottom: .75rem;
      &.active{
        .radio{
          background: #FFFFFF;
          border: .4rem solid #5E37B0;
        }
        .text{
          color: #7E55D5;
          text-shadow: 0 0 1.5rem #5E37B0;
        }
      }
      .radio{
        width: 1.3rem;
        height: 1.3rem;
        border-radius: 50%;
        background-color: transparent;
        border: .1rem solid #FFFFFF;
        margin-right: .6rem;
      }
      .text{
        font-weight: normal;
        font-size: .9rem;
        line-height: 1.1rem;
        letter-spacing: 0.03em;
        margin-right: .5rem;
      }
      .bank-prompt-wrap{
        background-size: contain;
        background-repeat: no-repeat;
        background-position: center;
        background-image: url('/img/bank/prompt.png');
        width: 1.3rem;
        height: 1.3rem;
        position: relative;
        .bank-prompt{
          width: 9rem;
        }
      }
    }
    .btn-main{
      height: 3.5rem;
      width: 100%;
      margin-top: .75rem;
    }
  }
  &__outcome{
    display: flex;
    align-items: center;
    justify-content: center;
    &-col{
      display: flex;
      flex-direction: column;
      justify-content: space-between;
      align-items: flex-start;
      width: 100%;
      min-height: 4.25rem;
      &:nth-child(1) {
        width: 8.5rem;
        min-width: 8.5rem;
        margin-right: .5rem;
        &>div{
          font-weight: normal;
        }
        .bank-credit-options__outcome-title{
          max-width: 5.25rem;
        }
      }
    }
    &-title{
      position: relative;
      font-weight: normal;
      font-size: .9rem;
      line-height: 1.1rem;
      span{
        position: absolute;
        white-space: pre;
        bottom: .1rem;
        left: 0;
        transform: translateY(100%);
        color: rgba(255, 255, 255, 0.5);
      }
    }
    &-value{
      font-weight: bold;
      font-size: 1.7rem;
      line-height: 2.05rem;
      letter-spacing: 0.03em;
    }
  }
}
</style>

