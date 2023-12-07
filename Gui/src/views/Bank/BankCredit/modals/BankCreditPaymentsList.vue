<template>
  <div class="bank-credit-payments-list bank-main-modal">
    <div class="bank-credit-payments-list__wrap bank-main-modal__wrap">
      <div 
        class="btn-close"
        @click="$emit('closeModal')"
      ></div>
      <div class="bank-main-modal-title">{{loc('bank:menu:193')}}</div>
      <div class="bank-credit-payments-list-prompt">{{loc('bank:menu:194')}} <span>{{paymentsCount}}</span></div>
      <div class="bank-credit-payments-list-head">
        <div>№</div>
        <div>{{loc('bank:menu:195')}}</div>
        <div>{{loc('bank:menu:196')}}</div>
        <div>{{loc('bank:menu:197')}}</div>
      </div>
      <div class="bank-credit-payments-list-body">
        <div 
          class="bank-credit-payments-list-body__item"
          v-for="(item, index) in dataModal.paymentsList"
          :key="index"
        >
          <div>{{index + 1}}</div>
          <div>{{item.date}}</div>
          <div>${{item.amount.toLocaleString('ru')}}</div>
          <div>${{item.payment.toLocaleString('ru')}}</div>
        </div>
      </div>
      <div class="bank-credit-payments-list__interaction">
        <div 
          v-if="dataModal.payment > dataModal.payedAmount"
          class="bank-credit-payments-list__interaction-btn"
          @click="$emit(
            'setCurrentModal', 
            {
              component: 'Pay',
              dataModal: dataModal
            }
          )"
        >{{loc('bank:menu:198')}}</div>
        <div class="bank-credit-payments-list__interaction-info">
          <div class="bank-credit-payments-list__interaction-info-desc">{{loc('bank:menu:199')}}</div>
          <div class="bank-credit-payments-list__interaction-info-value">${{getPayment(dataModal)}}</div>
        </div>
        <div 
          class="btn-main"
          @click="$emit(
            'setCurrentModal', 
            {
              component: 'PayOff',
              dataModal: dataModal
            }
          )"
        >{{loc('bank:menu:200')}}</div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters } from 'vuex';
export default {
  name: 'BankMainPaymentsList',
  props: {
    dataModal: Object
  },
  computed: {
    ...mapGetters('localization',['loc']),
    paymentsCount() {
      if (this.dataModal.leftPayment > 0)
        return this.dataModal.leftPayment;
      if (this.dataModal.amount > 0)
        return 1;
      return 0;
    }
  },
  methods: {    
    getPayment: function(item) {
      if (item.payment - item.payedAmount > 0)
        return (item.payment - item.payedAmount).toLocaleString('ru');
      return 0;
    }
  }
}
</script>

<style lang="scss">
.bank{
  .bank-credit-payments-list{
    .bank-main-modal-title{
      margin-bottom: 0;
      font-weight: bold;
      font-size: 2.5rem;
      line-height: 2.5rem;
    }
    &__wrap{
      width: 34rem;
      padding: 3rem 2.5rem;
    }
    &-prompt{
      font-size: 1rem;
      line-height: 1.2rem;
      font-style: normal;
      letter-spacing: 0.03em;
      color: rgba(94, 55, 176, 0.4);
      margin-bottom: 1.25rem;
      span{
        color: rgba(94, 55, 176, 1);
      }
    }
    &-head{
      display: flex;
      font-style: normal;
      padding-bottom: .6rem;
      color: rgba(94, 55, 176, 0.4);
      font-size: .9rem;
      line-height: 1.1rem;
      border-bottom: 1px solid rgba(155, 154, 158, 0.2);
      &>div{
        text-align: left;
        &:nth-child(1) {
          width: 2.5rem;
          min-width: 2.5rem;
        }
        &:nth-child(2) {
          width: 8.5rem;
          min-width: 8.5rem;
        }
        &:nth-child(3) {
          width: 8rem;
          min-width: 8rem;
          text-align: right;
        }
        &:last-child {
          width: 100%;
          text-align: right;
        }
      }
    }
    &-body{
      padding-right: 1.1rem;
      min-width: 100%;
      margin-right: -1.1rem;
      max-height: 24.55rem;
      margin-bottom: 1.25rem;
      overflow-y: auto;
      &::-webkit-scrollbar{
        width: .2rem;
        background-color: transparent;
        &-thumb{          
          background: #5E37B0;
          border-radius: .3rem;
        }
      }
      &__item{
        display: flex;
        align-items: center;
        padding: .8rem 0 .6rem 0;
        border-bottom: 1px solid rgba(94, 55, 176, 0.2);
        font-weight: normal;
        font-size: 1rem;
        line-height: 1.2rem;
        color: #200C49;
        text-align: left;
        &>div{
          &:nth-child(1) {
            width: 2.5rem;
            min-width: 2.5rem;
          }
          &:nth-child(2) {
            width: 6rem;
            min-width: 6rem;
            margin-right: 2.5rem;
          }
          &:nth-child(3) {
            width: 8rem;
            min-width: 8rem;
            text-align: right;
            margin-right: 2.5rem;
          }
          &:last-child {
            width: 100%;
            text-align: right;
          }
        }
      }
    }
    &__interaction{
      display: flex;
      align-items: center;
      justify-content: space-between;
      height: 3rem;
      font-weight: normal;
      &>div{
        height: 100%;
      }
      &-btn, .btn-main{
        border-radius: .5rem;
        font-size: 1rem;
        transition: .2s;
      }
      &-btn{
        display: flex;
        align-items: center;
        justify-content: center;
        width: 9rem;
        border: .1rem solid rgba(94, 55, 176, 0.3);
        color: rgba(94, 55, 176, 1);
        background-color: transparent;
        &:hover{
          border-color: rgba(94, 55, 176, 1);
          background-color: rgba(94, 55, 176, 1);
          color: #fff;
        }
      }
      .btn-main{
        width: 11.5rem;
      }
      &-info{
        display: flex;
        flex-direction: column;
        align-items: flex-start;
        justify-content: center;
        min-width: 6.7rem;
        border-right: 1px solid rgba(94, 55, 176, 0.2); 
        &-desc{
          font-size: .9rem;
          line-height: 1.1rem;
          color: rgba(94, 55, 176, 0.4);
          margin-bottom: .2rem;
        }
        &-value{
          font-size: 1rem;
          line-height: 1.2rem;
          color: #200C49;
        }
      }
    }
  }
}
</style>
