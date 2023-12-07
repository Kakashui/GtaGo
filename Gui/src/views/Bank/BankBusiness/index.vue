<template>
  <div class="bank-business">
    <div class="bank-business__interaction">
      <div class="bank-business__interaction--wrap">
        <div class="bank-business__interaction-desc">{{loc('bank:menu:206')}}</div>
        <div class="bank-business__interaction-title">{{bizName}}</div>
        <div class="bank-business__interaction-row">
          <div class="bank-business__interaction-row__col">
            <div class="bank-business__prompt">{{loc('bank:menu:207')}}</div>
            <div class="bank-business__interaction-balance">$ {{bizBalance.toLocaleString('ru')}}</div>
          </div>
          <div 
            :class="[{ active: isCredit }, 'bank-business__interaction-btn']"
            @click="bizCredit"
          >{{loc('bank:menu:208')}}</div>
        </div>
        <div class="bank-business__interaction-profit">
          <div class="bank-business__prompt">{{loc('bank:menu:209')}}</div>
          <div class="bank-business__interaction-profit-value">$ {{getProfit}}</div>
        </div>
      </div>
      <div class="bank-business__interaction-btns">
        <div 
          :class="[{ locked : !accessList.topUp }, 'bank-business__interaction-btns-item']"
          @click="$emit(
            'setCurrentModal', 
            {
              component: 'TopUp', 
              dataModal: {
                type: 'business',
                balance: bizBalance
              }
            } 
          )"
        >
          <div class="icon replenishment"></div>
          <span>{{loc('bank:menu:210')}}</span>
        </div>
        <div 
          :class="[{ locked : !accessList.withdraw }, 'bank-business__interaction-btns-item']"
          @click="$emit(
            'setCurrentModal', 
            {
              component: 'Withdraw', 
              dataModal: {
                type: 'business',
                balance: bizBalance
              }
            } 
          )"
        >
          <div class="icon withdrawal"></div>
          <span>{{loc('bank:menu:211')}}</span>
        </div>
        <div 
          :class="[{ locked : !accessList.bonus }, 'bank-business__interaction-btns-item']"
          @click="$emit(
            'setCurrentModal', 
            {
              component: 'Bonus', 
              dataModal: {
                type: 'business',
                staffList: staffList
              }
            }
          )"
        >
          <div class="icon bonus"></div>
          <span>{{loc('bank:menu:212')}}</span>
        </div>
        <div 
          :class="[{ locked : !accessList.payForBiz }, 'bank-business__interaction-btns-item']"
          @click="$emit(
            'setCurrentModal', 
            {
              component: 'PayForBiz', 
              dataModal: {
                type: 'business',
              }
            } 
          )"
        >
          <div class="icon tax"></div>
          <span>{{loc('bank:menu:213')}}</span>
        </div>
      </div>
    </div>
    <div class="bank-business__transfers">
      <div class="bank-business__transfers-title">{{loc('bank:menu:214')}}</div>
      <div class="bank-business__prompt">{{loc('bank:menu:215')}}</div>
      <div class="bank-business__transfers-list">
        <div
          class="bank-business__transfers-list-item"
          v-for="(item, index) in getTransfers"
          :key="index"
        >
          <div class="bank-business__transfers-list-item__icon">
            <span :style="{backgroundImage: `url(/img/bank/${getIcon(item)}.png)`}"></span>
          </div>
          <div class="bank-business__transfers-list-item__info">
            <div class="bank-business__transfers-list-item__info-name">{{loc(item.comment)}}</div>
          </div>
          <div :class="[{ profit: !item.directFrom }, 'bank-business__transfers-list-item-value']">{{getAmount(item)}} $</div>
        </div>
      </div>
    </div>
    <transition name="bank-modal">
      <component
        :is="currentModal"
        v-if="currentModal"
        @closeModal="closeModal"
      />
    </transition>
  </div>
</template>

<script>
import { mapState, mapGetters } from 'vuex'
import Credit from './modals/BankBusinessCredit'
import CurrentCredit from './modals/BankBusinessCurrentCredit'
export default {
  name: 'Bankbusiness',
  components: {
    Credit,
    CurrentCredit,
  },
  data() {
    return {
      currentModal: null
    }
  },
  computed: {
    ...mapState('bank/business', [
      'bizName',
      'transfersList',
      'bizBalance',
      'bizProfit',
      'isCredit',
      'accessList',
      'staffList',
    ]),
    ...mapGetters('localization',['loc']),
    getProfit() {
      return this.transfersList.filter(item => !item.directFrom).reduce((sum, item) => sum + (item.value - item.tax), 0).toLocaleString('ru')
    },
    getTransfers() {
      return this.transfersList.slice().sort((a,b) => b.date - a.date);
    }
  },
  methods: {
    bizCredit() {
      this.$emit('setCurrentPage', 'Credit')
    },
    setModal(value) {
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
.bank-business{
  width: 64rem;
  height: 43rem;
  display: flex;
  justify-content: space-between;
  &:before{
    content: '';
    width: 13.8rem;
    height: 54rem;
    background-size: contain;
    background-repeat: no-repeat;
    background-position: bottom right;
    position: absolute;
    right: 0;
    bottom: 0;
    background-image: url('/img/bank/decorate-business.png');
    pointer-events: none;
    // animation: showbusiness 1s;
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
  &__interaction{
    display: flex;
    flex-direction: column;
    width: 29rem;
    height: 100%;
    justify-content: space-between;
    &--wrap{
      display: flex;
      flex-direction: column;
      padding: 2.5rem 2.5rem 1rem 2.5rem;
      background: rgba(3, 12, 32, 0.7);
      border-radius: 1rem;
      height: 22.5rem;
      margin-bottom: 1.5rem;
    }
    &-desc{
      font-weight: normal;
      font-size: 1.2rem;
      line-height: 1.45rem;
      letter-spacing: 0.03em;
      color: #B6D300;
    }
    &-title{
      font-weight: bold;
      font-size: 2.4rem;
      line-height: 2.4rem;
      letter-spacing: 0.03em;
      min-height: 4rem;
    }
    &-balance{
      font-weight: normal;
      font-size: 1.8rem;
      line-height: 2.15rem;
      letter-spacing: 0.03em;
    }
    &-row{
      display: flex;
      align-items: center;
      justify-content: space-between;
      margin-bottom: 1.5rem;
      &__col{
        display: flex;
        flex-direction: column;
      }
    }
    &-btn{
      width: 10rem;
      height: 3rem;
      display: flex;
      align-items: center;
      justify-content: center;
      border: .1rem solid rgba(255, 255, 255, 0.4);
      box-sizing: border-box;
      border-radius: .2rem;
      font-weight: normal;
      font-size: 1rem;
      letter-spacing: 0.03em;
      color: rgba(255, 255, 255, 0.4);
      transition: .2s;
      &:not(.active):hover{
        border-color: rgba(255, 255, 255, 1);
        color: rgba(255, 255, 255, 1);
      }
      &.active{
        border-color: #EE443A;
        color: #EE443A;
        &:hover{
          color: #FFFFFF;
          background-color: #EE443A;
        }
      }
    }
    &-profit{
      height: 100%;
      width: calc(100% + (3rem));
      margin-left: -1.5rem;
      background-size: contain;
      background-repeat: no-repeat;
      background-position: center;
      background-image: url('/img/bank/biz-info-bg.png');
      display: flex;
      align-items: center;
      justify-content: center;
      flex-direction: column;
      position: relative;
      &-value{
        font-weight: bold;
        font-size: 2rem;
      }
      &:before{
        content: '';
        width: 6.55rem;
        height: 7.35rem;
        position: absolute;
        left: -2.55rem;
        bottom: -1.9rem;
        background-size: contain;
        background-repeat: no-repeat;
        background-position: center;
        background-image: url('/img/bank/biz-info-decorate.png');
      }
    }
    &-btns{
      display: grid;
      grid-template-columns: repeat(2, 1fr);
      grid-gap: 1rem;
      grid-auto-rows: 9rem;
      &-item{
        display: flex;
        flex-direction: column;
        align-items: center;
        justify-content: center;
        background: rgba(3, 12, 32, 0.6);
        border: .1rem solid rgba(255, 255, 255, 0.2);
        box-sizing: border-box;
        border-radius: .8rem;
        position: relative;
        .icon{
          width: 2rem;
          height: 2rem;
          background-size: contain;
          background-repeat: no-repeat;
          background-position: center;
          margin-bottom: .8rem;
          &.withdrawal{
            background-image: url('/img/bank/withdrawal-icon.png');
          }
          &.replenishment{
            background-image: url('/img/bank/replenishment-icon.png');
          }
          &.bonus{
            background-image: url('/img/bank/bonus-icon.png');
          }
          &.tax{
            background-image: url('/img/bank/suitcase.png');
          }
        }
        span{
          font-weight: normal;
          font-size: 1rem;
          line-height: 1.2rem;
          letter-spacing: 0.03em;
        }
        &.locked{
          pointer-events: none;
          .icon, span{
            opacity: .3;
          }
          &:before{
            content: '';
            width: 2.2rem;
            height: 2.2rem;
            border-radius: 50%;
            background-size: contain;
            background-repeat: no-repeat;
            background-position: center;
            background-color: #fff;
            background-image: url('/img/bank/locked.png');
            position: absolute;
            left: 1rem;
            top: 1rem;
          }
        }
      }
      &-item:not(.locked){
        &::before{
          content: '';
          width: 100%;
          height: 100%;
          background: radial-gradient(84.55% 84.55% at -21.36% -21.14%, #5E37B0 0%, rgba(94, 55, 176, 0) 100%);
          position: absolute;
          left: -.1rem;
          top: -.1rem;
          border-radius: .8rem;
          opacity: .7;
          transition: .3s;
        }
        &:hover{
          &::before{
            opacity: 1;
          }
        }
      }
    }
  }
  &__transfers{
    position: relative;
    padding: 3rem;
    background: rgba(3, 12, 32, 0.7);
    border-radius: 1rem;
    width: 33.5rem;
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
            &.tax{
              background-image: url('/img/bank/suitcase.png');
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
  }
  @keyframes showbusiness{
    from {
      transform: translateX(100%);
    }
    to {
      transform: translateX(0);
    }
  }
}
</style>
