<template>
  <div class="bank-organization">
    <div class="bank-organization__info">
      <div class="bank-organization__info-desc">{{loc('bank:menu:53')}}</div>
      <div class="bank-organization__info-title">{{organizationName}}</div>
      <div class="bank-organization__prompt">{{loc('bank:menu:54')}}</div>
      <div class="bank-organization__info-balance">$ {{organizationBalance.toLocaleString('ru')}}</div>
      <div 
        class="bank-organization__btn"
        @click="$emit(
          'setCurrentModal', 
          {
            component: 'Transfer', 
            dataModal: {
              type: 'fraction',
              balance: organizationBalance
            }
          } 
        )"
      >{{loc('bank:menu:55')}}</div>
      <div class="bank-organization__income">
        <div class="bank-organization__income-display">
          <div class="bank-organization__prompt">{{loc('bank:menu:56')}}</div>
          <div class="bank-organization__income-value">+ {{(calcSum(profitArray)).toLocaleString('ru')}} $</div>
        </div>
      </div>
    </div>
    <div class="bank-organization__transfers">
      <div class="bank-organization__transfers-title">{{loc('bank:menu:57')}}</div>
      <div class="bank-organization__prompt">{{loc('bank:menu:58')}}</div>
      <div class="bank-organization__transfers-list">
        <div 
          class="bank-organization__transfers-list-item"
          v-for="(item, index) in getTransfers"
          :key="index"
        >
          <div class="bank-organization__transfers-list-item__icon">
            <span :style="{backgroundImage: `url(/img/bank/${getIcon(item)}.png)`}"></span>
          </div>
          <div class="bank-organization__transfers-list-item__info">
            <div class="bank-organization__transfers-list-item__info-name">{{loc(item.comment)}}</div>
          </div>
          <div :class="[{ profit: !item.directFrom}, 'bank-organization__transfers-list-item-value']">{{getAmount(item)}} $</div>
        </div>
      </div>
      <div class="bank-organization__transfers__btns">
        <div 
          class="bank-organization__transfers__btn replenishment"
          @click="$emit(
          'setCurrentModal', 
          {
            component: 'TopUp', 
            dataModal: {
              type: 'fraction',
              balance: organizationBalance
            }
          } 
        )"
        >{{loc('bank:menu:59')}}</div>
        <div 
          class="bank-organization__transfers__btn withdrawal"
          @click="$emit(
          'setCurrentModal', 
          {
            component: 'Withdraw', 
            dataModal: {
              type: 'fraction',
              balance: organizationBalance
            }
          } 
        )"
        >{{loc('bank:menu:60')}}</div>
        <div 
          class="bank-organization__transfers__btn bonus"
          @click="$emit(
            'setCurrentModal', 
            {
              component: 'Bonus', 
              dataModal: {
                type: 'fraction',
                staffList: staffList
              }
            }
          )"
        >{{loc('bank:menu:61')}}</div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapState, mapGetters } from 'vuex'
export default {
  name: 'BankOrganization',
  computed: {
    ...mapState('bank/organization', [
      'organizationName',
      'transfersList',
      'organizationBalance',
      'staffList',
    ]),
    ...mapGetters('localization',['loc']),
    getTransfers() {
      return this.transfersList.slice().sort((a,b) => b.date - a.date);
    },
    profitArray() {
      return this.transfersList.filter(element => !element.directFrom)
    },
  },
  methods: {
    calcSum(arr) {
      let sum = 0
      arr.forEach(function(item) {
        sum += item.value - item.tax;
      })
      return sum
    },
    getIcon(transact) {
      if (!transact.directFrom)
        return 'replenishment-icon';
      else
        return 'withdrawal-icon';
      // bonus{
      //   background-image: url('/img/bank/bonus-icon.png');
      // }
      // withdrawal{
      //   background-image: url('/img/bank/withdrawal-icon.png');
      // }
      // tax{
      //   background-image: url('/img/bank/suitcase.png');
      // }
    },
    getAmount(transact) {
      if (transact.directFrom)
        return transact.value.toLocaleString('ru');
      else
        return (transact.value - transact.tax).toLocaleString('ru');
    },
  }
}
</script>

<style lang="scss">
.bank-organization{
  width: 64rem;
  height: 43rem;
  display: flex;
  justify-content: space-between;
  &:before{
    content: '';
    width: 14.65rem;
    height: 48.15rem;
    background-size: contain;
    background-repeat: no-repeat;
    background-position: bottom right;
    position: absolute;
    right: 0;
    bottom: 0;
    background-image: url('/img/bank/decorate-organization.png');
    pointer-events: none;
    // animation: showOrganization 1s;
  }
  &__btn{
    width: 100%;
    height: 4rem;
    display: flex;
    align-items: center;
    justify-content: center;
    border: .1rem solid rgba(255, 255, 255, 0.4);
    background-color: transparent;
    border-radius: .2rem;
    margin-bottom: 2rem;
    transition: .2s;
    &:hover{
      background-color: rgba(255, 255, 255, 0.1);
    }
  }
  &__prompt{
    font-weight: normal;
    font-size: 1rem;
    line-height: 1.2rem;
    letter-spacing: 0.03em;
    color: rgba(255, 255, 255, 0.5);
    margin-bottom: .3rem;
  }
  &__info{
    display: flex;
    flex-direction: column;
    width: 20rem;
    height: 100%;
    justify-content: space-between;
    &-desc{
      font-weight: normal;
      font-size: 1.2rem;
      line-height: 1.45rem;
      letter-spacing: 0.03em;
      color: #B6D300;
      margin-bottom: .5rem;
    }
    &-title{
      font-weight: bold;
      font-size: 3.5rem;
      line-height: 3.5rem;
      letter-spacing: 0.03em;
      min-height: 7rem;
      margin-bottom: 1.5rem;
    }
    &-balance{
      font-weight: bold;
      font-size: 2.5rem;
      line-height: 2.5rem;
      letter-spacing: 0.03em;
      margin-bottom: 1.5rem;
    }
  }
  &__income{
    display: flex;
    flex-direction: column;
    justify-content: space-between;
    align-items: center;
    background-image: url('/img/bank/profit-bg.png');
    background-size: contain;
    background-repeat: no-repeat;
    background-position: center bottom;
    border-radius: 0 0 .5rem .5rem;
    border-top: 1px solid rgba(255, 255, 255, 0.2);
    padding: 2.25rem 2rem 3rem 2rem;
    height: 21.5rem;
    position: relative;
    &-display{
      position: absolute;
      top: 3rem;
      display: flex;
      flex-direction: column;
      align-items: center;
      justify-content: center;
      .bank-organization__prompt{
        color: #fff;
      }
    }
    &-desc{
      font-weight: normal;
      font-size: 1.1rem;
      line-height: 1.2rem;
      letter-spacing: 0.03em;
    }
    &-value{
      font-weight: normal;
      font-size: 2rem;
      line-height: 2.4rem;
      color: #B6D300;
    }
    &-prompt{
      width: 100%;
      display: flex;
      align-items: center;
      justify-content: space-between;
      padding-top: 1rem;
      border-top: 1px solid rgba(255, 255, 255, 0.2);
      &-item{
        display: flex;
        flex-direction: column;
        align-items: center;
        justify-content: center;
        line-height: 1.2rem;
        letter-spacing: 0.03em;
        &-desc{
          width: 7.5rem;
          height: 2.5rem;
          display: flex;
          align-items: center;
          justify-content: center;
          font-size: 1rem;
          margin-bottom: .5rem;
          border-radius: 5rem;
          background: rgba(94, 92, 230, 0.2);
          color: #5E5CE6;
          &.job{
            background: rgba(220, 186, 88, 0.2);
            color: #DCBA58;
          }
        }
        &-value{
          font-size: 1.1rem;
          color: rgba(255, 255, 255, 0.4);
        }
      }
    }
  }
  &__transfers{
    position: relative;
    padding: 2.5rem 3.5rem 3.5rem 3.5rem;
    background: rgba(3, 12, 32, 0.7);
    border-radius: 1rem;
    width: 37.4rem;
    height: 100%;
    &-title{
      font-weight: bold;
      font-size: 2.5rem;
      line-height: 2.5rem;
      letter-spacing: 0.03em;
    }
    &-list{
      margin-top: 1.3rem;
      overflow-y: auto;
      height: 30.6rem;
      margin-left: 2.5rem;
      padding-right: 1.5rem;
      margin-right: -1.5rem;
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
        align-items: center;
        padding-bottom: .6rem;
        border-bottom: 1px solid rgba(255, 255, 255, 0.2);
        margin-bottom: 1rem;
        &:last-child{
          margin-bottom: 0;
          border-bottom: 0;
        }
        &__icon{
          width: 2.3rem;
          min-width: 2.3rem;
          height: 2.3rem;
          background-color: rgba(255, 255, 255, 0.14);
          border-radius: .3rem;
          display: flex;
          align-items: center;
          justify-content: center;
          margin-right: .8rem;
          span{
            background-size: contain;
            background-repeat: no-repeat;
            background-position: center;
            width: 1.2rem;
            height: 1.2rem;
            &.replenishment{
              background-image: url('/img/bank/replenishment-icon.png');
            }
            &.bonus{
              background-image: url('/img/bank/bonus-icon.png');
            }
            &.withdrawal{
              background-image: url('/img/bank/withdrawal-icon.png');
            }
            &.transfer{
              background-image: url('/img/bank/transfer-icon.png');
            }
          }
        }
        &__info{
          display: flex;
          flex-direction: column;
          white-space: pre;
          &-name{
            font-weight: bold;
            font-size: 1rem;
            line-height: 1rem;
            letter-spacing: 0.03em;
            text-shadow: 0 .5rem 5rem rgba(94, 55, 176, 0.6);
          }
          &-desc{
            font-weight: normal;
            font-size: 1rem;
            line-height: 1.2rem;
            letter-spacing: 0.05em;
            color: rgba(255, 255, 255, 0.4);
          }
        }
        &-value{
          width: 100%;
          text-align: right;
          font-weight: bold;
          font-size: 1.2rem;
          line-height: 1.2rem;
          letter-spacing: 0.03em;
          &:before{
            content: '- ';
          }
          &.profit{
            color: #B6D300;
            text-shadow: 0 .5rem 5rem rgba(94, 55, 176, 0.6);
            &:before{
              content: '+ ';
            }
          }
        }
      }
    }
    &__btns{
      position: absolute;
      left: 0;
      top: 50%;
      transform: translateX(-50%) translateY(-50%);
    }
    &__btn{
      display: flex;
      flex-direction: column;
      align-items: center;
      justify-content: center;
      width: 8rem;
      height: 8rem;
      margin-bottom: .5rem;
      background: #5E37B0;
      border-radius: .2rem;
      position: relative;
      padding: 0 2rem;
      font-weight: normal;
      text-align: center;
      font-size: 1rem;
      line-height: 1.2rem;
      letter-spacing: 0.03em;
      transition: .2s;
      &:hover{
        background-color: #452883;
      }
      &:before{
        content: '';
        width: 2rem;
        height: 2rem;
        background-size: contain;
        background-repeat: no-repeat;
        background-position: center;
        margin-bottom: .7rem;
      }
      &.replenishment{
        &:before {
          background-image: url('/img/bank/replenishment-icon.png');
        }
      }
      &.bonus{
        &:before {
          background-image: url('/img/bank/bonus-icon.png');
        }
      }
      &.withdrawal{
        &:before {
          background-image: url('/img/bank/withdrawal-icon.png');
        }
      }
    }
  }
  @keyframes showOrganization{
    from {
      transform: translateX(100%);
    }
    to {
      transform: translateX(0);
    }
  }
}
</style>
