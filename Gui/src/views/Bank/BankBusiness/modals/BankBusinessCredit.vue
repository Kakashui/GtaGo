<template>
  <div class="bank-business-credit bank-modal">
    <div class="bank-business-credit__wrap bank-modal__wrap">
      <div 
        class="btn-close"
        @click="$emit('closeModal')"
      ></div>
      <div class="bank-modal-title">Кредит под залог</div>
      <div class="bank-modal-desc bank-business-credit-desc">Кредит выдает на сумму не привышающей от гос цены бизнеса на 50%</div>
      <div class="bank-modal-desc bank-business-credit-desc">Если не погасите кредит вовремя, то банк заберает бизнес себе</div>
      <div class="bank-business-credit__info">
        <div class="bank-modal-desc">Одобренная сумма</div>
        <div class="bank-business-credit__info-value">$ {{currentCredit.approvedAmount.toLocaleString('ru')}}</div>
      </div>
      <div class="bank-business-credit__prompt">
        <div class="bank-business-credit__prompt-desc">Кредит выдаётся под {{approvedRate}}%</div>
        <div class="bank-business-credit__prompt-value">Сумма долга составит: <span>$ {{finishSumm.toLocaleString('ru')}}</span></div>
      </div>
      <div 
        class="bank-modal-btn btn-main"
        @click="takeBizCredit"
      >Взять</div>
    </div>
  </div>
</template>

<script>
import { mapState } from 'vuex'
export default {
  name: 'BankBusinessCredit',
  computed: {
    ...mapState('bank/business', ['currentCredit']),
    approvedRate() {
      return Math.floor(Number(this.currentCredit.rate) * 100)
    },
    finishSumm() {
      return Math.floor(this.currentCredit.approvedAmount * (1 + (this.currentCredit.rate)))
    }
  },
  methods: {
    takeBizCredit() {
      window.mp.triggerServer('Bank:takeBizCredit')
      this.$emit('closeModal')
    }
  }
}
</script>

<style lang="scss">
.bank-business-credit{
  .bank-modal-title{
    margin-bottom: .5rem;
  }
  &-desc{
    position: relative;
    padding-left: 1rem;
    display: flex;
    align-items: center;
    margin-bottom: .7rem;
    &:before{
      content: '';
      height: 1.9rem;
      width: .2rem;
      background: #EE443A;
      border-radius: .5rem;
      position: absolute;
      left: 0;
    }
  }
  &__info{
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    padding: 1.2rem 0;
    border-top: 1px solid rgba(0, 0, 0, 0.1);
    border-bottom: 1px solid rgba(0, 0, 0, 0.1);
    margin-bottom: 1rem;
    position: relative;
    &:before{
      content: '';
      width: 8.85rem;
      height: 5.45rem;
      position: absolute;
      left: -6.5rem;
      background-size: contain;
      background-repeat: no-repeat;
      background-position: center;
      background-image: url('/img/bank/credit-decorate.png');
    }
    .bank-modal-desc{
      margin-bottom: .3rem;
    }
    &-value{
      font-weight: normal;
      font-size: 2rem;
      line-height: 2.4rem;
      letter-spacing: 0.03em;
      color: #5E37B0;
    }
  }
  &__prompt{
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    margin-bottom: 1.5rem;
    font-weight: normal;
    font-size: 1rem;
    line-height: 1.2rem;
    letter-spacing: 0.03em;
    &-desc{
      color: #EE443A;
      margin-bottom: .3rem;
    }
    &-value{
      color: rgba(0, 0, 0, 0.4);
      span{
        color: #000000;
      }
    }
  }
}
</style>
