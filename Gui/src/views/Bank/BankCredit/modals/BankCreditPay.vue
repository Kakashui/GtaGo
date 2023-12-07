<template>
  <div class="bank-credit-pay bank-modal">
    <div class="bank-credit-pay__wrap bank-modal__wrap">
      <div 
        class="btn-close"
        @click="$emit('closeModal')"
      ></div>
      <div class="bank-modal-title">{{loc('bank:menu:201')}}</div>
      <div class="bank-modal-desc bank-credit-pay-desc">
        <div>{{loc('bank:menu:202')}}</div>
        <div>
          <span>{{dataModal.datePayment}}</span>
        </div>
      </div>
      <div class="bank-credit-pay__prompt">
        <div class="bank-credit-pay__prompt-value">$ {{getPayment(dataModal).toLocaleString('ru')}}</div>
        <div class="bank-credit-pay__prompt-desc">{{loc('bank:menu:203')}}</div>
      </div>
      <div 
        class="bank-modal-btn btn-main"
        @click="payCredit"
      >{{loc('bank:menu:204')}}</div>
    </div>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
export default {
  name: 'BankCreditPay',
  props: {
    dataModal: Object
  },
  computed: {
    ...mapGetters('localization',['loc']),
  },
  methods: {
    payCredit() {
      let payment = this.getPayment(this.dataModal)
      if (payment > 0) {
        window.mp.triggerServer('bank:payCredit', this.dataModal.id, payment)
        this.$emit('closeModal')
      }
    },
    getPayment: function(item) {
      if (item.payment - item.payedAmount > 0)
        return (item.payment - item.payedAmount);
      return 0;
    }
  }
}
</script>

<style lang="scss">
.bank-credit-pay{
  &__wrap{
    width: 24rem;
  }
  .bank-modal-title{
    margin-bottom: 0;
  }
  &-desc{
    display: flex;
    flex-direction: column;
    align-items: flex-start;
    justify-content: center;
    margin-bottom: 1.3rem;
    &>div{
      margin-bottom: .2rem;
      span{
        color: #5E37B0;
      }
      &:last-child{
        margin-bottom: 0;
      }
    }
  }
  &__prompt{
    padding-left: 2.4rem;
    position: relative;
    letter-spacing: 0.03em;
    margin-bottom: 2rem;
    &::before{
      content: '';
      width: 1.6rem;
      height: 1.6rem;
      position: absolute;
      left: 0;
      top: 0;
      background-size: contain;
      background-repeat: no-repeat;
      background-position: center;
      background-image: url('/img/bank/documents.png');
    }
    &-value{
      font-weight: bold;
      font-size: 2rem;
      line-height: 2rem;
      color: #5E37B0;
      margin-bottom: .3rem;
    }
    &-desc{
      font-weight: normal;
      font-size: 1rem;
      line-height: 1.2rem;
      color: rgba(0, 0, 0, 0.4);
    }
  }
  .btn-main{
    font-weight: normal;
    font-size: 1rem;
    width: 100%;
    height: 3rem;
  }
}
</style>
