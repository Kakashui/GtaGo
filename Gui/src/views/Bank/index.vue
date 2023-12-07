<template>
  <div class="bank">
    <div class="bank-decorate bank-decorate--small"></div>
    <div class="bank-decorate bank-decorate--middle"></div>
    <div class="bank-decorate bank-decorate--big"></div>
    <div class="bank__nav">
      <div class="bank-logo"></div>
      <div 
        :class="[{active: item.key === currentPage}, 'bank__nav-item']"
        v-for="item in navigations"
        :key="item.key"
        @click="setCurrentPage(item.key)"
      >
        <div 
          class="bank__nav-item__icon"
          :style="{backgroundImage: `url(/img/bank/${item.img})`}"
        ></div>
        <div class="bank__nav-item__text">{{loc(item.text)}}</div>
      </div>
    </div>
    <transition name="bank-component" mode="out-in">
      <component
        class="bank__body"
        :is="currentPage"
        @setCurrentModal="setCurrentModal"
        @setCurrentPage="setCurrentPage"
      />
    </transition>
    <transition name="bank-modal">
      <component 
        v-if="currentModal.component"
        :is="currentModal.component"
        :dataModal="currentModal.dataModal"
        @closeModal="closeModal"
      />
    </transition>
  </div>
</template>

<script>
import Main from './BankMain'
import Deposits from './BankDeposits'
import Business from './BankBusiness'
import Family from './BankFamily'
import Organization from './BankOrganization'
import Credit from './BankCredit'
import Withdraw from './modals/BankModalWithdraw'
import Transfer from './modals/BankModalTransfer'
import TopUp from './modals/BankModalTopUp'
import PayForBiz from './modals/BankModalPayForBiz'
import Bonus from './modals/BankModalBonus'
import { mapGetters, mapState } from 'vuex'
export default {
  name: 'Bank',
  components: {
    Main,
    Deposits,
    Business,
    Family,
    Organization,
    Credit,
    Withdraw,
    Transfer,
    TopUp,
    PayForBiz,
    Bonus,
  },
  data() {
    return {
      currentModal: {
        component: null,
        dataModal: {}
      },
      currentPage: null,
      navList: [
        {
          key: 'Main',
          text: 'bank:menu:4',
          img: 'museum.png'
        },
        {
          key: 'Deposits',
          text: 'bank:menu:5',
          img: 'money-bag.png'
        },
        {
          key: 'Business',
          text: 'bank:menu:6',
          img: 'suitcase.png'
        },
        {
          key: 'Family',
          text: 'bank:menu:7',
          img: 'group.png'
        },
        {
          key: 'Organization',
          text: 'bank:menu:8',
          img: 'teamwork.png'
        },
        {
          key: 'Credit',
          text: 'bank:menu:9',
          img: 'credit.png'
        },
      ]
    }
  },
  computed: {
    ...mapState('bank/business', ['bizName']),
    ...mapState('bank/family', ['familyName']),
    ...mapState('bank/organization', ['organizationName']),
    ...mapGetters('localization',['loc']),
    navigations() {
      return this.navList.filter(item => 
        (item.key == 'Main') ||
        (item.key == 'Deposits') ||
        (item.key == 'Business' && this.bizName != null) ||
        (item.key == 'Family' && this.familyName != null) ||
        (item.key == 'Organization' && this.organizationName != null) ||
        (item.key == 'Credit') 

      )
    }
  },
  methods: {
    setCurrentPage(value) {
      this.currentPage = value
    },
    setCurrentModal(obj) {
      this.currentModal.dataModal = obj.dataModal
      this.currentModal.component = obj.component
    },
    closeModal() {
      this.currentModal.component = null
      this.currentModal.dataModal = {}
    }
  },
  mounted() {
    this.currentPage = this.navList[0].key
  }
}
</script>

<style lang="scss">
.bank{
  display: flex;
  align-items: center;
  justify-content: center;
  width: 100vw;
  height: 100vh;
  overflow: hidden;
  position: absolute;
  left: 0;
  top: 0;
  color: #fff;
  font-weight: bold;
  text-transform: uppercase;
  background-size: cover;
  background-position: center;
  background-image: url('/img/bank/bg.png');
  .range-slider{
    width: calc(100% - (2*1rem));
    height: .2rem;
    border-radius: .5rem;
    position: absolute;
    left: 1rem;
    bottom: 0;
    padding: 0;
    &-inner{
      position: initial;
    }
    &-rail, &-fill{
      height: .2rem;
      border-radius: .5rem;
      left: initial;
      transform: initial;
      bottom: 0;
      top: initial;
    }
    &-rail{
      background: rgba(255, 255, 255, 0.2);
    }
    &-fill{
      background: #5E37B0;
      box-shadow: 0 .5rem 5rem rgba(94, 55, 176, 0.6);
    }
    &-knob{
      width: 1.15rem;
      height: 1.15rem;
      border: .35rem solid #fff;
      background: #5E37B0;
    }
  }
  &-prompt{
    position: absolute;
    top: -.3rem;
    left: 0;
    transform: translateY(-100%);
    font-weight: normal;
    font-size: 1rem;
    line-height: 1.2rem;
    letter-spacing: 0.03em;
    color: #000000;
    padding: 1rem;
    background: #FFFFFF;
    box-shadow: 0 .2rem 1rem rgba(0, 0, 0, 0.5);
    border-radius: .3rem .3rem .3rem 0;
    opacity: 0;
    max-width: 12.5rem;
    transition: .3s;
    pointer-events: none;
  }
  .replenishment, .completion{
    .bank-prompt{
      white-space: pre;
    }
  }
  .replenishment, .completion, .bank-prompt-wrap{
    &:hover{
      .bank-prompt{
        opacity: 1;
      }
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
    z-index: 2;
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
      span{
        white-space: pre;
      }
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
  &:before, &:after{
    content: '';
    pointer-events: none;
    position: absolute;
    background: #5E37B0;
    border-radius: 50%;
    z-index: 2;
  }
  // &:before{
  //   width: 30rem;
  //   height: 30rem;
  //   filter: blur(10rem);
  //   left: -15rem;
  //   bottom: -15rem;
  // }
  // &:after{
  //   width: 20rem;
  //   height: 20rem;
  //   right: -10rem;
  //   bottom: -10rem;
  //   filter: blur(12rem);
  //   animation: brownianMotion 36s linear infinite;
  // }
  .btn-main{
    display: flex;
    align-items: center;
    justify-content: center;
    position: relative;
    font-weight: normal;
    font-size: 1rem;
    border-radius: .2rem;
    background-color: #5E37B0;
    border: 1px solid transparent;
    transition: .2s;
    &:before{
      width: 1.5rem;
      height: 1.5rem;
      background-size: contain;
      background-repeat: no-repeat;
      background-position: center;
      margin-right: .7rem;
    }
    &:hover{
      background-color: #452883;
    }
  }
  .btn-close{
    width: 2.5rem;
    height: 2.5rem;
    display: flex;
    align-items: center;
    justify-content: center;
    background-color: rgba(255, 255, 255, 0.1);
    border: 1px solid rgba(255, 255, 255, 0.1);
    position: absolute;
    right: -1rem;
    top: 0;
    transform: translateX(100%);
    transition: .2s;
    &:hover{
      background-color: rgba(255, 255, 255, 0.2);
    }
    &:after{
      content: '';
      width: 1rem;
      height: 1rem;
      position: absolute;
      background-size: contain;
      background-repeat: no-repeat;
      background-position: center;
      background-image: url('/img/bank/close.png');
    }
  }
  &-logo{
    width: 6.75rem;
    height: 2.05rem;
    background-size: contain;
    background-repeat: no-repeat;
    background-position: center;
    background-image: url('/img/bank/logo.png');
    position: absolute;
    left: 1.5rem;
    top: 1.5rem;
    &:after{
      content: '';
      width: 6rem;
      height: .1rem;
      position: absolute;
      left: 50%;
      transform: translateX(-50%);
      bottom: -2rem;
      background: rgba(255, 255, 255, 0.14);
    }
  }
  &-decorate{
    border-radius: 50%;
    position: absolute;
    background: radial-gradient(50% 50% at 50% 50%, rgba(255, 255, 255, 0) 0%, rgba(255, 255, 255, 0.1) 100%);
    pointer-events: none;
    &--small{
      width: 3.7rem;
      height: 3.7rem;
      top: 3rem;
      left: 19rem;
      // animation: brownianMotion 11s linear infinite;
    }
    &--middle{
      width: 9rem;
      height: 9rem;
      top: 4.5rem;
      left: 10rem;
      // animation: brownianMotion 18s linear infinite;
    }
    &--big{
      width: 40rem;
      height: 40rem;
      left: 50%;
      top: 8.5rem;
      margin-left: -20rem;
      // animation: brownianMotion 36s linear infinite;
    }
  }
  &__nav{
    position: absolute;
    left: 0;
    top: 0;
    width: 9.5rem;
    min-width: 9.5rem;
    height: 100%;
    padding: 7.35rem 1.75rem 0 1.75rem;
    background: #030C20;
    &-item{
      width: 6rem;
      height: 6rem;
      background: rgba(255, 255, 255, 0.1);
      border-radius: .2rem;
      display: flex;
      align-items: center;
      justify-content: center;
      flex-direction: column;
      margin-bottom: .2rem;
      z-index: 3;
      &:last-child{
        margin-bottom: 0;
      }
      &:hover, &.active{
        background: #5E37B0;
        box-shadow: 0 .5rem 5rem rgba(94, 55, 176, 0.6);
      }
      &__icon{
        width: 2rem;
        height: 2rem;
        font-size: .9rem;
        background-size: contain;
        background-repeat: no-repeat;
        background-position: center;
        margin-bottom: .5rem;
      }
    }
  }
  &__body{
    display: flex;
    min-width: 64rem;
    min-height: 43rem;
    z-index: 1;
  }
  .bank-component-enter-active, .bank-component-leave-active{
    transition: .1s;
  }
  .bank-component-enter, .bank-component-leave-to
  /* .bank-component-leave-active below version 2.1.8 */ {
    opacity: 0;
  }

  .bank-modal-enter-active {
    transition: all .3s ease;
  }
  .bank-modal-leave-active {
    transition: all .2s cubic-bezier(1.0, 0.5, 0.8, 1.0);
  }
  .bank-modal-enter, .bank-modal-leave-to
  /* .bank-modal-leave-active below version 2.1.8 */ {
    transform: translateY(1rem);
    opacity: 0;
  }
  // @keyframes brownianMotion {
  //   0% {
  //     transform: translateX(0%) translateY(0%) scale(1) rotate(0deg);
  //   }
  //   20% {
  //     transform: translateX(5%) translateY(6%) scale(1.05) rotate(72deg);
  //   }
  //   40% {
  //     transform: translateX(-4%) translateY(-2%) scale(.9) rotate(144deg);
  //   }
  //   60% {
  //     transform: translateX(4%) translateY(3%) scale(1.07) rotate(216deg);
  //   }
  //   80% {
  //     transform: translateX(-3%) translateY(-1%) scale(.86) rotate(288deg);
  //   }
  //   100% {
  //     transform: translateX(0%) translateY(0%) scale(1) rotate(360deg);
  //   }
  // }
}
</style>
