<template>
  <div class="cash-machine">
    <div class="cash-machine-bg"></div>
    <div v-if="currentPage.number === 1" class="cash-machine__first-screen">
      <div class="first-screen__info">
        <img
          src="img/atm/icon_wallet.png"
          alt="wallet"
          class="info__icon"
        >
        <span class="info_balance">{{ currentBalance }}</span>
        <div class="info__letter">{{ capitalLetter }}</div>
      </div>
      <div class="first-screen__greeting">
        <span class="greeting__title">{{`${loc('atm_1')}, ${username}!` }} 👋</span>
        <p class="greeting__info">
          {{loc('atm_2')}}
        </p>
        <img
          src="img/atm/pay_online.svg"
          alt="img"
          class="greeting__img"
        >
      </div>
      <div class="first-screen__actions">
        <button
          v-for="(item, index) in firstScreenButtons"
          :key="index"
          class="actions__btn"
          @click="changePage(item.page, item.key, item.title)"
        >{{ loc(item.title) }}</button>
      </div>
    </div>
    <div v-else-if="currentPage.number === 2" class="cash-machine__second-screen">
      <button class="second-screen__back" @click="changePage(1, null, '')">
        <img
          class="back__icon"
          src="img/atm/arrow_back.svg"
          alt="back"
        >
      </button>
      <div class="second-screen__card">
        <span class="card__number">{{ space(creditNumber) }}</span>
        <div class="card__info">
          <span class="info__balance">{{ currentBalance }}</span>
          <span class="info__user">{{ username }}</span>
        </div>
      </div>
      <button class="second-screen__close">
        <img
          class="close__icon"
          @click="close"
          src="img/atm/cross_close.svg"
          alt="close"
        >
      </button>
      <span class="second-screen__title">
        {{ loc(currentPage.title) }}  
      </span>
      <span class="second-screen__title" v-if="currentPage.key === 3"> 
        {{`${bizName}: ${businessBalance}/${maxBusinessBalanceFixed} $`}}
      </span>
      <span class="second-screen__title" v-if="currentPage.key === 2">
        {{`${homeBalance}/${maxHomeBalanceFixed} $`}}
      </span>
      <div class="second-screen__input" v-if="currentPage.key === 4">
        <span class="input__title">{{loc('atm_5')}}</span>
        <input
          v-model="accountNumberValue"
          type="number"
          class="input__field"
          maxlength="19"
        >
      </div>
      <div class="second-screen__input">
        <span class="input__title">{{loc('atm_4')}}</span>
        <input
          v-model="transferAmountValue"
          type="number"
          class="input__field"
        >
      </div>
      <div @click="agree" class="second-screen_pay">{{loc('atm_3')}}</div>
    </div>
  </div>
</template>

<script>
import { mapState, mapMutations, mapGetters } from 'vuex'
import primeAccount from '../../configs/newDonateShop/primeAccount'

export default {
  name: 'CashMachine',

  data: function () {
    return {
      accountNumberValue: null,
      transferAmountValue: null,
      firstScreenButtons: [
        { key: 4, page: 2, title: 'gui_780'},
        { key: 1, page: 2, title: 'gui_781'},
        { key: 2, page: 2, title: 'gui_782'},
        { key: 0, page: 2, title: 'gui_783'},
        { key: 3, page: 2, title: 'gui_784'},
        { key: -1, page: null, title: 'gui_785'}
      ],
      lastClick: Date.now()
    }
  },

  computed: {
    ...mapState('cashMachine', [
      'currentPage',
      'username',
      'bizName'
    ]),
    ...mapState('smartphone/bankPage', ['balance', 'bankAccount', 'homeBalance', 'maxHomeBalance', 'businessBalance', 'maxBusinessBalance']),
    ...mapState('newDonateShop/prime',['days']),

    creditNumber: function () {
      const userIdStr = String(this.bankAccount)
      return userIdStr.padStart(16, '0')
    },

    ...mapGetters('localization',['loc']),

    capitalLetter: function () {
      return this.username.charAt(0).toUpperCase()
    },

    currentBalance: function () {
      return this.balance.toLocaleString() + '$'
    },
    maxHomeBalanceFixed: function() {
      return this.maxHomeBalance * (this.days > 0 ? primeAccount.paymentForBusinessInDays : 14)
    },
    maxBusinessBalanceFixed: function() {
      return this.maxBusinessBalance * (this.days > 0 ? primeAccount.paymentForBusinessInDays : 14)
    },
    
  },

  methods: {
    ...mapMutations('cashMachine', [
      'setCurrentPage'
    ]),

    changePage: function (number, key, title) {
      if (key === -1) {
        this.close();
      } else {
        if(key === 2 && this.maxHomeBalance < 0) return;
        if(key === 3 && this.maxBusinessBalance < 0) return;
        this.setCurrentPage({ number, key, title })
      }
    },
    close(){
      this.changePage(1, null, '');
      window.mp.trigger('guiClose');
    },
    agree: function () {
      if(Date.now() < this.lastClick) return;
      if (this.currentPage.key === 4){
        if(this.accountNumberValue == null || this.transferAmountValue == null) return;
        if(this.accountNumberValue < 1 || this.transferAmountValue < 1) return;
        window.mp.triggerServer("atmCB", this.currentPage.key, this.transferAmountValue, this.accountNumberValue.replace(' ', ''));
        this.accountNumberValue = null;
        this.transferAmountValue=null;
      } else {
        if(this.transferAmountValue == null) return;
        if(this.transferAmountValue < 1) return;
        window.mp.triggerServer("atmCB", this.currentPage.key, this.transferAmountValue);
        this.transferAmountValue = null;
      }
      this.lastClick = Date.now() + 1000;
    },

    space: function (str, after) {
      if (!str) {
        return false
      }
      after = after || 4
      let v = str.replace(/[^\dA-Z]/g, '')
      let reg = new RegExp(".{" + after + "}", "g")

      if (str.length >= 19) {
        return str
      } else {
        return v.replace(reg, function (a) {
          return a + ' '
        })
      }
    }
  }
}
</script>

<style lang="scss" scoped>
input::-webkit-outer-spin-button,
input::-webkit-inner-spin-button {
  -webkit-appearance: none;
  margin: 0;
}
.cash-machine {
  color: #131313;
  width: 48rem;
  padding: 4rem;
  &-bg{
    background-color: rgba(#fff, .75);
    position: fixed;
    left: 0;
    top: 0;
    width: 100vw;
    height: 100vh;
    z-index: -1;    
  }
  &__first-screen {
    display: flex;
    flex-direction: column;
    .first-screen__info {
      box-shadow: 0px 19px 16px 0 rgba(0, 0, 0, 0.06);
      background-color: #ffffff;
      border-radius: 1.8rem;
      display: flex;
      align-items: center;
      align-self: start;
      padding: .5rem 1rem;
      box-shadow: 1px 1px 3px 0 rgba(rgb(95, 93, 93), .5) inset;
      .info__icon {
        height: 1.2rem;
        width: 1.3rem;
      }
      .info_balance {
        font-size: 1.3rem;
        margin: 0 0 0 1.5rem;
      }
      .info__letter {
        display: flex;
        align-items: center;
        justify-content: center;
        font-size: 1.5rem;
        color: #fff;
        width: 2.6rem;
        height: 2.6rem;
        background-color: #74b65f;
        border-radius: 50%;
        margin: 0 0 0 1.5rem;
      }
    }
    .first-screen__greeting {
      margin: 1.5rem 0 0 0;
      background: #fff;
      box-shadow: 0px 19px 16px 0 rgba(0, 0, 0, 0.06);
      border-radius: .9rem;
      padding: 2.5rem 2.9rem;
      position: relative;
      overflow: hidden;
      box-shadow: 1px 1px 3px 1px rgba(rgb(34, 33, 33), .5);
      .greeting__title {
        color: #247f0b;
        font-size: 1.35rem;
      }
      .greeting__info {
        margin: 14px 0 0 0;
        width: 65%;
        font-size: 1.05rem;
        letter-spacing: .03rem;
      }
      .greeting__img {
        width: 18rem;
        position: absolute;
        left: 64%;
        top: -4%;
      }
    }
    .first-screen__actions {
      display: grid;
      grid-template-columns: 1fr 1fr;
      gap: 1.3rem 2.6rem;
      margin: 1.3rem 0 0 0;
      .actions__btn {
        box-shadow: 0px 19px 16px 0 rgba(0, 0, 0, 0.06);
        background-color: #ffffff;
        border-radius: .85rem;
        z-index: 11;
        font-size: 1.1rem;
        padding: 1rem 0;
        box-shadow: 1px 1px 3px 1px rgba(rgb(34, 33, 33), .5);
        &:hover{
          transform: scale(1.01);
        }
      }
    }
  }
  &__second-screen {
    background-color: #fff;
    border-radius: .85rem;
    box-shadow: 0px 19px 16px 0 rgba(0, 0, 0, 0.06);
    padding: 8rem 3rem 1.5rem 3rem;
    display: flex;
    flex-direction: column;
    position: relative;
    .second-screen__back, .second-screen__close {
      position: absolute;
      width: 3.55rem;
      height: 2.35rem;
      border-radius: 1.15rem;
      box-shadow: 0px 19px 16px 0 rgba(0, 0, 0, 0.06);
      background-color: #ffffff;
    }
    .second-screen__back {
      top: -2%;
      left: -2%;
      box-shadow: 1px 1px 3px 0 rgba(rgb(95, 93, 93), .5) inset;
      .back__icon {
        width: 1.5rem;
        height: 1.05rem;
      }
      &:hover{
        transform: scale(1.05);
      }
    }
    .second-screen__card {
      position: absolute;
      background: url('../../../public/img/atm/screen_card.png');
      background-size: cover;
      display: flex;
      flex-direction: column;
      justify-content: flex-end;
      align-items: center;
      width: 18.6rem;
      height: 11.45rem;
      border-radius: 1.95rem;
      box-shadow: 0px 17px 10px 0 rgba(62, 255, 0, 0.06);
      transform: translate(-50%, -50%);
      top: 0;
      left: 50%;
      padding: 0 0 1rem 0;
      .card__number {
        font-size: 1.25rem;
        color: #fff;
      }
      .card__info {
        color: #fff;
        display: flex;
        align-items: center;
        justify-content: space-around;
        padding: 0 .6rem;
        .info__balance {
          font-size: 1.1rem;
        }
        .info__user {
          font-size: 1.2rem;
          margin: 0 0 0 2rem;
          text-align: right;
        }
      }
    }
    .second-screen__close {
      top: -2%;
      right: -2%;
      box-shadow: -1px 1px 3px 0 rgba(rgb(95, 93, 93), .5) inset;
      .close__icon {
        width: .9rem;
        height: .9rem;
      }
      &:hover{
        transform: scale(1.05);
      }
    }
    .second-screen__title {
      font-size: 1.5rem;
      font-weight: bold;
      color: #1a242d;
      text-align: center;
    }
    .second-screen__input {
      margin: 1.4rem 0 0 0;
      display: flex;
      flex-direction: column;
      .input__title {
        font-weight: bold;
      }
      .input__field {
        width: 100%;
        background-color: #eff1f3;
        box-shadow: 2px 2px 5px 0 rgba(rgb(95, 93, 93), .5) inset;
        font-size: 1.25rem;
        color: #131313;
        font-weight: bold;
        padding: 1rem 0;
        text-align: center;
        border-radius: 1.25rem;
        margin: .4rem 0 0 0;
        &:hover{
          background-color: #e3e9ee;
        }
      }
    }
    .second-screen_pay {
      width: 100%;
      text-align: center;
      border-radius: 1rem;
      box-shadow: 0px 17px 10px 0 rgba(62, 255, 0, 0.06);
      background-color: #74b65f;
      color: #fff;
      padding: 1rem 0;
      font-weight: bold;
      margin: 3.5rem 0 0 0;
      letter-spacing: 1px;
      text-transform: uppercase;
      &:hover{
        transform: scale(1.03);
      }
    }
  }
}
</style>
