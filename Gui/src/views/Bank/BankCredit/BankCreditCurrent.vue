<template>
  <div class="bank-credit-current">
    <div class="bank-credit-current-title">{{loc('bank:menu:168')}}</div>
    <div 
      v-if="creditsList.length > 0"
      class="bank-credit-current__credits"
    >
      <div 
        class="bank-credit-current__credits-item"
        v-for="item in creditsList"
        :key="item.id"
      >
        <div class="bank-credit-current__credits-item-amount">- ${{item.amount.toLocaleString('ru')}}</div>
        <div class="bank-credit-current__credits-item-desc"><span>{{loc('bank:menu:169')}} </span> {{getPledgeName(item)}}</div>
        <div class="bank-credit-current__credits-item-desc"><span>{{loc('bank:menu:170')}} </span> ${{item.payment.toLocaleString('ru')}}</div>
        <div class="bank-credit-current__credits-item-desc"><span>{{loc('bank:menu:171')}} </span> {{item.payedAmount.toLocaleString('ru')}}</div>
        <div class="bank-credit-current__credits-item-desc"><span>{{loc('bank:menu:172')}} </span> {{item.datePayment}}</div>
        <div class="bank-credit-current__credits-item__btns">
          <div 
            class="bank-credit-current__credits-item-btn"
            @click="$emit(
              'setCurrentModal',
              {
                component: 'PaymentsList',
                dataModal: item,
              }
            )"
          >{{loc('bank:menu:173')}}</div>
          <div 
            v-if="item.payment > item.payedAmount"
            class="btn-main"
            @click="$emit(
              'setCurrentModal', 
              {
                component: 'Pay',
                dataModal: item
              }
            )"
          >{{loc('bank:menu:174')}}</div>
        </div>
      </div>
    </div>
    <div 
      v-else
      class="bank-credit-current__no-credits"
    >{{loc('bank:menu:175')}}</div>
    <div class="bank-credit-current__fast-money">
      <div class="bank-credit-current__fast-money-title">{{loc('bank:menu:176')}}</div>
      <div class="bank-credit-current__fast-money-desc">{{loc('bank:menu:177')}}</div>
      <div 
        v-if="fastMoney"
        class="btn-main"
        @click="openFastCreditModal"
      >{{loc('bank:menu:178')}}</div>
      <div 
        v-else
        class="bank-credit-current__fast-money-lock"
      ></div>
    </div>
  </div>
</template>

<script>
import { mapState, mapGetters } from 'vuex'
export default {
  name: 'BankCreditCurrent',

  computed: {
    ...mapState('bank/credit', [
      'fastMoney',
      'creditsList',
      'propertyList',
    ]),
    ...mapGetters('localization',['loc']),
  },
  methods: {
    openFastCreditModal: function() {
      this.$emit(
          'setCurrentModal', 
          {
            component: 'Fast'
          }
        )
    },
    getPledgeName: function(item) {
      const index = this.propertyList.findIndex(prop => prop.id === item.pledgeId && prop.type == item.pledgeType)
      if (index >= 0)
        return this.propertyList[index].name;
      return this.loc("bank:menu:232")
    }
  }
}
</script>

<style lang="scss">
.bank-credit-current{
  width: 17rem;
  display: flex;
  flex-direction: column;
  justify-content: space-between;
  &-title{
    font-weight: bold;
    font-size: 2.5rem;
    line-height: 2.5rem;
    letter-spacing: 0.03em;
  }
  &__no-credits{
    height: 24.5rem;
    padding-right: 5.7rem;
    font-weight: normal;
    font-size: 1.1rem;
    line-height: 1.4rem;
    background-size: contain;
    background-position: center bottom;
    background-repeat: no-repeat;
    background-image: url('/img/bank/no-credits.png');
  }
  &__fast-money{
    display: flex;
    flex-direction: column;
    height: 13.25rem;
    background: rgba(3, 12, 32, 0.7);
    border-radius: .6rem;
    padding: 2rem 1.5rem 1.5rem 1.5rem;
    background-size: contain;
    background-repeat: no-repeat;
    background-position: right bottom;
    background-image: url('/img/bank/fast-money.png');
    &-title{
      font-weight: bold;
      font-size: 1.5rem;
      line-height: 1.5rem;
      margin-bottom: .5rem;
    }
    &-desc{
      font-weight: normal;
      font-size: 1rem;
      line-height: 1.2rem;
      letter-spacing: 0.03em;
      color: rgba(255, 255, 255, 0.6);
      margin-bottom: 1.95rem;
    }
    &-lock{
      width: 2.2rem;
      height: 2.2rem;
      border-radius: 50%;
      box-shadow: 0 0 3rem #5E37B0;
      background-size: contain;
      background-repeat: no-repeat;
      background-position: center;
      background-image: url('/img/bank/lock.png');
    }
    .btn-main{
      width: 8.55rem;
      height: 2.85rem;
      font-size: 1rem;
    }
  }
  &__credits{
    overflow-y: auto;
    height: 24.5rem;
    min-width: 100%;
    padding-right: .45rem;
    margin-right: -.45rem;
    padding-bottom: 1px;
    &::-webkit-scrollbar{
      width: .2rem;
      background-color: transparent;
      &-thumb{
        background: rgba(255, 255, 255, 0.2);
        border-radius: .3rem;
      }
    }
    &-item{
      display: flex;
      flex-direction: column;
      padding: 1.25rem 0;
      border-top: 1px solid rgba(255, 255, 255, 0.2);
      &:last-child{
        border-bottom: 1px solid rgba(255, 255, 255, 0.2);
      }
      &-amount{
        font-weight: normal;
        font-size: 1.7rem;
        line-height: 2.05rem;
        letter-spacing: 0.03em;
        display: flex;
        align-items: center;
        margin-bottom: .65rem;
        &:before{
          content: '';
          width: 1.8rem;
          min-width: 1.8rem;
          height: 1.8rem;
          margin-right: .5rem;
          border-radius: 50%;
          background-color: #5E37B0;
          background-size: .9rem .9rem;
          background-repeat: no-repeat;
          background-position: center;
          background-image: url('/img/bank/withdrawal-icon.png');
        }
      }
      &-desc{
        font-weight: normal;
        font-size: 1rem;
        line-height: 1.2rem;
        white-space: pre;
        margin-bottom: .3rem;
        span{
          color: rgba(255, 255, 255, 0.5);
        }
      }
      &__btns{
        margin-top: .5rem;
        display: flex;
        align-items: center;
        justify-content: space-between;
        &>div{
          width: 47%;
        }
      }
      &-btn, .btn-main{
        font-weight: normal;
        height: 2rem;
        font-size: 1rem;
        line-height: 1.2rem;
        letter-spacing: 0.03em;
        border-radius: .2rem;
        transition: .2s;
      }
      &-btn{
        display: flex;
        align-items: center;
        justify-content: center;
        border: 1px solid rgba(255, 255, 255, 0.4);
        color: rgba(255, 255, 255, 0.4);
        &:hover{
          border-color: #fff;
          color: #fff;
          background-color: rgba(255, 255, 255, 0.1);
        }
      }
    }
  }
}
</style>
