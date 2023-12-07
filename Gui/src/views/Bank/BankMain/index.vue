<template>
  <div class="bank-main">
    <div>
      <div class="bank-main__card">
        <div class="bank-main__card-logo"></div>
        <div class="bank-main__card-pay"></div>
        <div class="bank-main__card-account">{{loc(`bank:menu:62@${account}`)}}</div>
        <div class="bank-main__card-info">
          <div class="bank-main__card-info-desc">{{loc('bank:menu:63')}}</div>
          <div class="bank-main__card-info-value">{{cardNumber}}</div>
        </div>
      </div>
      <div class="bank-main__balance">
        <div class="bank-main__balance-desc">{{loc('bank:menu:64')}}</div>
        <div class="bank-main__balance-value">$ {{balance.toLocaleString('ru')}}</div>
      </div>
      <div class="bank-main__transactions">
        <div class="bank-main__transactions-title">{{loc('bank:menu:65')}}</div>
        <div class="bank-main__transactions-list">
          <div 
            class="bank-main__transactions-list-item" 
            v-for="(item, index) in getTransactions"
            :key="index"
          >
            <div :class="[{ positive: !item.directFrom }, 'bank-main__transactions-list-item__icon']"></div>
            <div class="bank-main__transactions-list-item__info">
              <div class="bank-main__transactions-list-item__info-value">{{getAmount(item)}} $</div>
              <div class="bank-main__transactions-list-item__info-desc">{{loc(item.comment)}}</div>
            </div>
          </div>
        </div>
      </div>
    </div>
    <div>
      <div class="bank-main__services">
        <div class="bank-main__services-item balance">
          <div class="bank-main__services-item-text">{{loc('bank:menu:66')}}</div>
          <div 
            class="bank-main__btn"
            @click="$emit(
              'setCurrentModal', 
              {
                component: 'TopUp', 
                dataModal: {
                  type: 'personal',
                  balance: balance
                }
              }
            )"
          ></div>
        </div>
        <div class="bank-main__services-item money">
          <div class="bank-main__services-item-text">{{loc('bank:menu:67')}}</div>
          <div 
            class="bank-main__btn"
            @click="$emit(
              'setCurrentModal', 
              {
                component: 'Withdraw', 
                dataModal: {
                  type: 'personal',
                  balance: balance
                }
              }
            )"
          ></div>
        </div>
        <div class="bank-main__services-item transfer">
          <div class="bank-main__services-item-text">{{loc('bank:menu:68')}}</div>
          <div 
            class="bank-main__btn"
            @click="$emit(
              'setCurrentModal', 
              {
                component: 'Transfer', 
                dataModal: {
                  type: 'personal',
                  balance: balance
                }
              } 
            )"
          ></div>
        </div>
      </div>
      <div class="bank-main__prompt">{{loc('bank:menu:69')}}</div>
      <div class="bank-main__info">
        <div 
          class="bank-main__info-item hovered"
          @click="setCurrentModal('PayForHouse')"
        >
          <div class="bank-main__info-item-icon house"></div>
          <div class="bank-main__info-item-title">{{loc('bank:menu:70')}}</div>
          <div class="bank-main__info-item-desc">{{loc('bank:menu:71')}}</div>
        </div>
        <div 
          class="bank-main__info-item hovered"
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
          <div class="bank-main__info-item-icon biz"></div>
          <div class="bank-main__info-item-title">{{loc('bank:menu:72')}}</div>
          <div class="bank-main__info-item-desc">{{loc('bank:menu:73')}}</div>
        </div>
        <div 
          class="bank-main__info-item hovered"
          @click="setCurrentModal('Fines')"
        >
          <div class="bank-main__info-item-icon fine"></div>
          <div class="bank-main__info-item-title">{{loc('bank:menu:74')}}</div>
          <div class="bank-main__info-item-desc">{{loc('bank:menu:75')}}</div>
        </div>
        <div 
          class="bank-main__info-item hovered"
          @click="setCurrentModal('Mobile')"
        >
          <div class="bank-main__info-item-icon mobile"></div>
          <div class="bank-main__info-item-title">{{loc('bank:menu:76')}}</div>
          <div class="bank-main__info-item-desc">{{loc('bank:menu:77')}}</div>
        </div>
        <div class="bank-main__info-item">
          <div class="bank-main__info-item-title">{{loc('bank:menu:78')}}</div>
          <div class="bank-main__info-item-desc">{{loc('bank:menu:79')}}</div>
          <div class="bank-main__info-item__input">
            <input 
              type="number"
              placeholder=""
              v-model="currentCharity"
            >
            <div 
              class="bank-main__btn"
              @click="donateCharity"
            ></div>
          </div>
        </div>
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
import PayForHouse from './modals/BankMainPayForHouse'
import Mobile from './modals/BankMainMobile'
import Fines from './modals/BankMainFines'
export default {
  name: 'BankMain',
  components: {
    PayForHouse,
    Mobile,
    Fines,
  },
  data() {
    return {
      currentCharity: null,
      currentModal: null,
    }
  },
  computed: {
    ...mapState('smartphone/bankPage', ['balance']),
    ...mapState('bank', [
      'account',
      'cardNumber',
      'transactionsList',
    ]),
    ...mapGetters('localization',['loc']),
    getTransactions() {
      return this.transactionsList.slice().sort((a,b) => b.date - a.date);
    }
  },
  methods: {
    donateCharity() {
      if (this.currentCharity > 0) {
        window.mp.triggerServer('government:donateToGov', this.currentCharity, 0, false)
        this.currentCharity = null
      }
    },
    setCurrentModal(value) {
      this.currentModal = value
    },
    closeModal() {
      this.currentModal = null
    },
    getAmount(transact) {
      if (transact.directFrom)
        return `- ${transact.value.toLocaleString('ru')}`
      else
        return `+ ${(transact.value - transact.tax).toLocaleString('ru')}`

    }
  }
}
</script>

<style lang="scss">
.bank-main{
  display: flex;
  &>div{
    display: flex;
    flex-direction: column;
    height: 100%;
    position: relative;
    &:nth-child(1) {
      width: 20rem;
      min-width: 20rem;
      margin-right: 7rem;
      &:after{
        content: '';
        width: 1.5rem;
        height: .75rem;
        background-size: contain;
        background-repeat: no-repeat;
        background-position: center;
        background-image: url('/img/bank/arrow-light.png');
        position: absolute;
        right: -2.25rem;
        top: 5rem;
        transform: translateX(100%);
      }
    }
    &:nth-child(2) {
      width: 100%;
    }
  }
  &-modal{
    position: fixed!important;
    width: 100vw!important;
    height: 100vh!important;
    left: 0;
    top: 0;
    display: flex;
    align-items: center;
    justify-content: center;
    // background: radial-gradient(43.85% 77.96% at 50% 50%, #5E37B0 0%, rgba(94, 55, 176, 0) 100%), rgba(0, 0, 0, 0.9);
    background: rgba(0, 0, 0, 0.6);
    &__wrap{
      width: 24rem;
      padding: 3rem;
      background: #FFFFFF;
      border-radius: .8rem;
      display: flex;
      flex-direction: column;
      position: relative;
    }
    &-title{
      font-weight: bold;
      font-size: 2.5rem;
      line-height: 2.5rem;
      letter-spacing: 0.03em;
      color: #000000;
    }
    &-desc, &-prompt{
      font-weight: normal;
      font-size: 1rem;
      line-height: 1.2rem;
      letter-spacing: 0.03em;
      color: rgba(0, 0, 0, 0.4);
    }
    &-desc{
      margin-bottom: 1.5rem;
    }
    &-prompt{
      margin-bottom: .3rem;
      display: flex;
      align-items: center;
      &.card{
        &:before{
          content: '';
          width: 1.6rem;
          height: 1.6rem;
          margin-right: .5rem;
          background-size: contain;
          background-repeat: no-repeat;
          background-position: center;
          background-image: url('/img/bank/card-icon-gray.png');
        }
      }
    }
    &__block-shadow{
      background: #FFFFFF;
      box-shadow: 0 1.5rem 2.5rem rgba(49, 41, 184, 0.1);
      border-radius: .5rem;
      padding: .85rem 1.5rem;
      position: relative;
      &:before{
        width: 1.6rem;
        height: 1.6rem;
        position: absolute;
        left: 1.5rem;
        top: .9rem;
        margin-right: .5rem;
        background-size: contain;
        background-repeat: no-repeat;
        background-position: center;
      }
      &.house, &.cash, &.biz{
        &:before{
          content: '';
        }
        padding-left: 3.85rem;
      }
      &.house{
        &:before{
          background-image: url('/img/bank/house-icon.png');
        }
      }
      &.cash{
        &:before{
          background-image: url('/img/bank/card-icon.png');
        }
      }
      &.biz{
        &:before{
          background-image: url('/img/bank/biz-icon.png');
        }
      }
    }
    &-btn{
      height: 3rem;
      border-radius: .5rem;
    }
    &__input{
      position: relative;
      width: 100%;
      padding-bottom: .5rem;
      border-bottom: 1px solid  rgba(0, 0, 0, 0.1);
      display: flex;
      align-items: center;
      justify-content: flex-start;
      font-weight: bold;
      font-size: 2rem;
      line-height: 2rem;
      letter-spacing: 0.03em;
      color: #000000;
      margin-bottom: 1.2rem;
      input{
        width: 100%;
        font-weight: bold;
        font-size: 2rem;
        line-height: 2rem;
        letter-spacing: 0.03em;
        color: #000000;
        background-color: transparent;
        border: none;
        outline: none;
        &::-webkit-outer-spin-button,
        &::-webkit-inner-spin-button {
          /* display: none;*/
          -webkit-appearance: none;
        }
        &::placeholder{
          font-weight: normal;
          font-size: 1rem;
          line-height: 1.2rem;
          letter-spacing: 0.03em;
          color: rgba(0, 0, 0, 0.4);
        }
      }
    }
  }
  &__card{
    display: flex;
    flex-direction: column;
    justify-content: space-between;
    align-items: flex-start;
    position: relative; 
    background-size: contain;
    background-repeat: no-repeat;
    background-position: center;
    background-image: url('/img/bank/bg-card.png');
    padding: 1rem;
    width: 100%;
    height: 11rem;
    text-shadow: 0 .1rem .15rem rgba(0, 0, 0, 0.25);
    margin-bottom: 1rem;
    &-account{
      color: #316E76;
      font-size: .9rem;
      line-height: 1.1rem;
      letter-spacing: 0.2em;
    }
    &-logo{
      width: 2.75rem;
      height: .75rem;
      position: absolute;
      right: 1rem;
      top: 1rem;
      background-size: contain;
      background-repeat: no-repeat;
      background-position: center;
      background-image: url('/img/bank/logo.png');
    }
    &-pay{
      width: 1.05rem;
      height: 1.3rem;
      position: absolute;
      right: 1.2rem;
      bottom: 1.2rem;
      background-size: contain;
      background-repeat: no-repeat;
      background-position: center;
      background-image: url('/img/bank/pay.png');
    }
    &-info{
      display: flex;
      flex-direction: column;
      font-weight: normal;
      letter-spacing: 0.3em;
      &-desc{
        font-size: .9rem;
        line-height: 1.1rem;
      }
      &-value{
        font-size: 2rem;
        line-height: 2.4rem;
      }
    }
  }
  &__balance{
    display: flex;
    align-items: center;
    padding: 0 0 .5rem .7rem;
    letter-spacing: 0.03em;
    border-bottom: .1rem solid rgba(255, 255, 255, 0.14);
    margin-bottom: 1.4rem;
    width: 100%;
    &-desc{
      font-weight: normal;
      font-size: 1.2rem;
      line-height: 1.45rem;
      margin-right: 1rem;
    }
    &-value{
      font-weight: bold;
      font-size: 3rem;
    }
  }
  &__transactions{
    display: flex;
    flex-direction: column;
    height: 26rem;
    padding: 2rem 1.75rem 0 0;
    background: #FFFFFF;
    box-shadow: 0 1rem 4.5rem #0A041E;
    border-radius: 1.1rem;
    &-title{
      font-size: 2rem;
      letter-spacing: 0.03em;
      padding-left: 1.75rem;
      color: #262640;
    }
    &-list{
      overflow-y: auto;
      height: 100%;
      padding: 1.1rem .8rem 2rem 1.75rem;
      &::-webkit-scrollbar{
        width: .3rem;
        background: transparent;
        &-thumb{
          background: #5E37B0;
          box-shadow: 0 .5rem 5rem rgba(94, 55, 176, 0.6);
          border-radius: .2rem;
        }
      }
      &-item{
        display: flex;
        align-items: center;
        margin-bottom: 1.05rem;
        &:last-child{
          margin-bottom: 0;
        }
        &__icon{
          width: 2.1rem;
          height: 2.1rem;
          background: #FFFFFF;
          box-shadow: 0 .5rem 2rem rgba(24, 33, 65, 0.31);
          border-radius: .4rem;
          display: flex;
          align-items: center;
          justify-content: center;
          margin-right: .8rem;
          background-size: contain;
          background-repeat: no-repeat;
          background-position: center;
          background-image: url('/img/bank/negative.png');
          &.positive{
            background-image: url('/img/bank/positive.png');
          }
        }
        &__info{
          display: flex;
          flex-direction: column;
          font-size: 1rem;
          &-value{
            font-weight: bold;
            letter-spacing: 0.03em;
            color: #030C20;
            text-shadow: 0 .5rem 5rem rgba(94, 55, 176, 0.6);
            margin-bottom: .2rem;
          }
          &-desc{
            letter-spacing: 0.05em;
            font-weight: normal;
            color: rgba(3, 12, 32, 0.4);
          }
        }
      }
    }
  }
  &__services{
    display: flex;
    align-items: center;
    justify-content: space-between;
    margin-bottom: 3.95rem;
    &-item{
      width: 11rem;
      height: 11rem;
      background: radial-gradient(84.55% 84.55% at -21.36% -21.14%, #5E37B0 0%, rgba(94, 55, 176, 0) 100%);
      background-color: #fff;
      border-radius: 1rem;
      display: flex;
      justify-content: center;
      padding: 6rem 1.5rem 0 1.5rem;
      position: relative;
      &:before{
        content: '';
        width: 5rem;
        height: 5rem;
        background-size: contain;
        background-repeat: no-repeat;
        background-position: center;
        position: absolute;
        top: .5rem;
      }
      &.balance{
        &:before{
          background-image: url('/img/bank/balance.png');
        }
      }
      &.money{
        &:before{
          background-image: url('/img/bank/money.png');
        }
      }
      &.transfer{
        &:before{
          background-image: url('/img/bank/transfer.png');
        }
      }
      &-text{
        font-size: 1.5rem;
        line-height: 1.5rem;
        text-align: center;
        letter-spacing: 0.03em;
        color: #262640;
      }
      .bank-main__btn{
        position: absolute;
        bottom: 0;
        transform: translateY(50%);
        &:hover{
          transform: translateY(50%) scale(1.1);
        }
        &:active{
          transform: translateY(50%) scale(1.05);
        }
      }
    }
  }
  &__btn{
    background: #FFFFFF;
    box-shadow: 0 1rem 4.5rem #0A041E;
    border-radius: 2.5rem;
    width: 5rem;
    min-width: 5rem;
    height: 2.5rem;
    display: flex;
    align-items: center;
    justify-content: center;
    transition: .2s;
    &:before{
      content: '';
      width: 1.5rem;
      height: .75rem;
      background-size: contain;
      background-repeat: no-repeat;
      background-position: center;
      background-image: url('/img/bank/arrow.png');
    }
  }
  &__prompt{
    width: 100%;
    position: relative;
    font-weight: normal;
    font-size: 1.2rem;
    line-height: 1.45rem;
    letter-spacing: 0.03em;
    display: flex;
    align-items: center;
    margin-bottom: .85rem;
    &:after{
      content: '';
      width: 100%;
      height: .2rem;
      background: rgba(255, 255, 255, 0.14);
      margin-left: 1.85rem;
    }
  }
  &__info{
    display: grid;
    grid-auto-rows: 8rem;
    grid-template-columns: repeat(2, 1fr);
    grid-gap: 1rem;
    &-item{
      display: flex;
      flex-direction: column;
      position: relative;
      background: rgba(3, 12, 32, 0.7);
      border-radius: 1rem;
      padding: 2rem 1.5rem 0 1.5rem;
      transition: .1s;
      &:last-child{
        grid-column-start: 1;
        grid-column-end: 3;
      }
      &.hovered{
        &:hover{
          transform: scale(1.06);
        }
        &:active{
          transform: scale(1.03);
        }
      }
      &-title{
        font-weight: normal;
        font-size: 1.7rem;
        line-height: 2.05rem;
        letter-spacing: 0.03em;
        margin-bottom: .55rem;
      }
      &-desc{
        font-weight: normal;
        font-size: 1rem;
        line-height: 1.2rem;
        letter-spacing: 0.03em;
        color: rgba(255, 255, 255, 0.4);
        max-width: 15rem;
      }
      &-icon{
        position: absolute;
        top: 1rem;
        right: 1rem;
        width: 3rem;
        height: 3rem;
        background-repeat: no-repeat;
        background-position: center;
        background-size: contain;
        border-radius: 50%;
        &.house{
          background-image: url('/img/bank/circle-house.png');
        }
        &.biz{
          background-image: url('/img/bank/circle-biz.png');
        }
        &.fine{
          background-image: url('/img/bank/circle-fine.png');
        }
        &.mobile{
          background-image: url('/img/bank/circle-mobile.png');
        }
      }
      &__input{
        display: flex;
        align-items: center;
        position: absolute;
        width: 16.5rem;
        right: 2.5rem;
        padding: .25rem;
        background: rgba(255, 255, 255, 0.1);
        border: 1px solid rgba(255, 255, 255, 0.3);
        box-sizing: border-box;
        border-radius: 3rem;
        input{
          width: 100%;
          padding: 0 0 0 1.5rem;
          background: transparent;
          height: 100%;
          font-weight: normal;
          font-size: 1rem;
          line-height: 1.2rem;
          letter-spacing: 0.03em;
          color: #fff;
          &::placeholder{
            color: rgba(255, 255, 255, 0.5);
          }
          &::-webkit-outer-spin-button,
          &::-webkit-inner-spin-button {
            -webkit-appearance: none;
          }
        }
      }
    }
  }
}
</style>
