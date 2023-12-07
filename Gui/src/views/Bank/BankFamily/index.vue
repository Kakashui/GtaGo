<template>
  <div class="bank-family">
    <div class="bank-family__info">
      <div class="bank-family__info-desc">{{loc('bank:menu:95')}}</div>
      <div class="bank-family__info-title">{{familyName}}</div>
      <div class="bank-family__prompt">{{loc('bank:menu:96')}}</div>
      <div class="bank-family__info-balance">$ {{familyBalance.toLocaleString('ru')}}</div>
      <div class="bank-family__income">
        <div class="bank-family__income-display">
          <div class="bank-family__income-display__info">
            <div class="bank-family__prompt">{{loc('bank:menu:97')}}</div>
            <div class="bank-family__income-value">+ {{(calcSum(workProfitArray) + calcSum(otherProfitArray)).toLocaleString('ru')}} $</div>
          </div>
          <svg xmlns="http://www.w3.org/2000/svg" width="270" height="270" viewBox="0 0 270 270" fill="none">
            <circle class="back" />
            <circle 
              :stroke-dasharray="calcCircumference" 
              :stroke-dashoffset="circleValue" 
            />
          </svg>
        </div>
        <div class="bank-family__income-prompt">
          <div class="bank-family__income-prompt-item">
            <div class="bank-family__income-prompt-item-desc job">{{loc('bank:menu:98')}}</div>
            <div class="bank-family__income-prompt-item-value">+ {{calcSum(workProfitArray).toLocaleString('ru')}} $</div>
          </div>
          <div class="bank-family__income-prompt-item">
            <div class="bank-family__income-prompt-item-desc">{{loc('bank:menu:99')}}</div>
            <div class="bank-family__income-prompt-item-value">+ {{calcSum(otherProfitArray).toLocaleString('ru')}} $</div>
          </div>
        </div>
      </div>
    </div>
    <div class="bank-family__transfers">
      <div class="bank-family__transfers-title">{{loc('bank:menu:100')}}</div>
      <div class="bank-family__prompt">{{loc('bank:menu:101')}}</div>
      <div class="bank-family__transfers-list">
        <div 
          class="bank-family__transfers-list-item"
          v-for="(item, index) in getTransfers"
          :key="index"
        >
          <div class="bank-family__transfers-list-item__icon">
            <span :style="{backgroundImage: `url(/img/bank/${getIcon(item)}.png)`}"></span>
          </div>
          <div class="bank-family__transfers-list-item__info">
            <div class="bank-family__transfers-list-item__info-name">{{loc(item.comment)}}</div>
          </div>
          <div :class="[{ profit: !item.directFrom}, 'bank-family__transfers-list-item-value']">{{getAmount(item)}} $</div>
        </div>
      </div>
      <div class="bank-family__transfers__btns">
        <div 
          class="bank-family__transfers__btn replenishment"
          @click="$emit(
            'setCurrentModal', 
            {
              component: 'TopUp', 
              dataModal: {
                type: 'family',
                balance: familyBalance
              }
            } 
          )"
        >{{loc('bank:menu:102')}}</div>
        <div 
          class="bank-family__transfers__btn withdrawal"
          @click="$emit(
            'setCurrentModal', 
            {
              component: 'Withdraw', 
              dataModal: {
                type: 'family',
                balance: familyBalance
              }
            } 
          )"
        >{{loc('bank:menu:103')}}</div>
        <div 
          class="bank-family__transfers__btn bonus"
          @click="$emit(
            'setCurrentModal', 
            {
              component: 'Bonus', 
              dataModal: {
                type: 'family',
                staffList: staffList
              }
            }
          )"
        >{{loc('bank:menu:104')}}</div>
        <div 
          class="bank-family__transfers__btn bonus"
          @click="setCurrentModal('PayForHouse')"
        >{{loc('bank:menu:105')}}</div>
      </div>
    </div>
    <transition name="bank-modal">
      <component 
        v-if="currentModal"
        :is="currentModal"
        @closeModal="closeModal"
      />
    </transition>
  </div>
</template>

<script>
import { mapState, mapGetters } from 'vuex'
import PayForHouse from './modals/BankFamilyPayForHouse'
export default {
  name: 'BankFamily',
  components: {
    PayForHouse,
  },
  data() {
    return {
      currentCharity: null,
      currentModal: null,
    }
  },
  computed: {
    ...mapState('bank/family', ['familyName', 'transfersList', 'familyBalance', 'staffList']),
    ...mapGetters('localization',['loc']),
    calcCircumference: function () {
      let number = 2 * Math.PI * 119
      return  number
    },
    getTransfers() {
      return this.transfersList.slice().sort((a,b) => b.date - a.date);
    },
    workProfitArray() {
      return this.transfersList.filter(element => !element.directFrom && element.comment == 'Money_Work')
    },
    otherProfitArray() {
      return this.transfersList.filter(element => !element.directFrom && element.comment != 'Money_Work')
    },
    circleValue: function () {
      let workProfit = this.calcSum(this.workProfitArray)
      let totalProfit = this.calcSum(this.otherProfitArray) + workProfit
      const maxCircleValue = this.calcCircumference
      const maxCircleValuePer = (maxCircleValue / totalProfit) * workProfit
      return maxCircleValue - maxCircleValuePer
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
    setCurrentModal(value) {
      this.currentModal = value
    },
    closeModal() {
      this.currentModal = null
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
.bank-family{
  width: 64rem;
  height: 43rem;
  display: flex;
  justify-content: space-between;
  &:before{
    content: '';
    width: 32rem;
    height: 48.9rem;
    background-size: contain;
    background-repeat: no-repeat;
    background-position: bottom right;
    position: absolute;
    right: 0;
    bottom: 0;
    background-image: url('/img/bank/decorate-family.png');
    pointer-events: none;
    // animation: showFamily .7s;
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
    background: linear-gradient(180deg, rgba(3, 12, 32, 0) 0%, #030C20 100%);
    border-radius: 0 0 .5rem .5rem;
    border-top: 1px solid rgba(255, 255, 255, 0.2);
    padding: 2.25rem 2rem 3rem 2rem;
    height: 25rem;
    &-display{
      width: 13.5rem;
      height: 13.5rem;
      position: relative;
      display: flex;
      align-items: center;
      justify-content: center;
      margin-bottom: 1rem;
      &__info{
        display: flex;
        flex-direction: column;
        align-items: center;
        justify-content: center;
        position: absolute;
        &:before{
          content: '';
          width: 9.5rem;
          height: 9.5rem;
          border-radius: 50%;
          background: rgba(220, 186, 88, 0.6);
          filter: blur(4.5rem);
          position: absolute;
        }
      }
      svg{
        position: absolute;
        width: 100%;
        height: 100%;
        transform-origin: 50% 50%;
        transform: rotate(-90deg);
        circle{
          fill: none;
          stroke: #DCBA58;
          cx: 135;
          cy: 135;
          r: 119;
          stroke-width: 16;
          &.back{
            stroke: #5E5CE6;
          }
        }
      }
    }
    &-value{
      font-weight: normal;
      font-size: 2rem;
      line-height: 2.4rem;
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
            &.work{
              background-image: url('/img/bank/work-icon.png');
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
      margin-top: 2rem;
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
      height: 7.5rem;
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
  @keyframes showFamily{
    from {
      transform: translateX(100%);
    }
    to {
      transform: translateX(0);
    }
  }
}
</style>
