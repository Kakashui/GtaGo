<template>
  <div class="bank-deposits">
    <transition name="bank-modal">
      <BankDepositsModal 
        v-if="modal.show"
        :dataModal="modal.dataModal"
        @closeModal="closeModal"
      />
    </transition>
    <div 
      class="bank-deposits__current"
      v-if="currentDepositsList.length > 0"
    >
      <div class="bank-deposits__current-prompt">{{loc(`bank:menu:111@${currentDepositIndex + 1}@${currentDepositsList.length}`)}}</div>
      <div class="bank-deposits__current-title">{{loc(currentDeposit.Title)}}</div>
      <div class="bank-deposits__current-desc">{{loc(currentDeposit.Description)}}</div>
      <div class="bank-deposits__current__circs">
        <div :class="[{ active: currentDeposit.IsRefill }, 'bank-deposits__current__circs-item', 'replenishment']">
          <div class="bank-prompt">{{currentDeposit.IsRefill ? loc('bank:menu:112') : loc('bank:menu:113')}}</div>
        </div>
        <div :class="[{ active: currentDeposit.IsWithdraw }, 'bank-deposits__current__circs-item', 'completion']">
          <div class="bank-prompt">{{currentDeposit.IsWithdraw ? loc('bank:menu:114') : loc('bank:menu:115')}}</div>
        </div>
        <div class="bank-deposits__current__circs__info">
          <div class="bank-deposits__current__circs__info-value">{{currentDeposit.AnnualRate}}%</div>
          <div class="bank-deposits__current__circs__info-desc">{{loc('bank:menu:116')}}</div>
        </div>
        <template v-if="currentDepositsList.length > 1">
          <div 
            class="bank-deposits__current__btn prev"
            @click="prevDeposit"
          ></div>
          <div 
            class="bank-deposits__current__btn next"
            @click="nextDeposit"
          ></div>
        </template>
      </div>
      <div class="bank-deposits__current__info">
        <div class="bank-deposits__current__info-desc">{{loc('bank:menu:117')}}</div>
        <div class="bank-deposits__current__info-balance">{{currentDepositsList[currentDepositIndex].balance.toLocaleString('ru')}} $</div>
        <div class="bank-deposits__current__info-profit">+ {{currentDepositsList[currentDepositIndex].profit.toLocaleString('ru')}} $</div>
        <div class="bank-deposits__current__info-desc">{{loc('bank:menu:118')}}</div>
        <div class="bank-deposits__current__info-time">{{getLeftTime}}</div>
      </div>
      <div 
        class="bank-deposits__current-btn btn-main"
        v-if="currentDeposit.IsRefill"
        @click="$emit(
          'setCurrentModal', 
          {
            component: 'TopUp', 
            dataModal: {
              type: `deposit`,
              id: currentDepositsList[currentDepositIndex].id,
              balance: currentDepositsList[currentDepositIndex].balance
            }
          }
        )"
      >{{loc('bank:menu:119')}}</div>
      <div 
        class="bank-deposits__current-btn btn-main withdraw"
        v-if="currentDeposit.IsWithdraw"
        @click="$emit(
          'setCurrentModal', 
          {
            component: 'Withdraw', 
            dataModal: {
              type: `deposit`,
              id: currentDepositsList[currentDepositIndex].id,
              balance: currentDepositsList[currentDepositIndex].balance
            }
          }
        )"
      >{{loc('bank:menu:120')}}</div>
      <div
        class="bank-deposits__current-btn"
        @click="closeDeposit(currentDepositsList[currentDepositIndex].id)"
      >{{loc('bank:menu:121')}}</div>
    </div>
    <div 
      class="bank-deposits__current"
      v-else
    >
      <div class="bank-deposits__current-prompt">{{loc('bank:menu:122')}}</div>
      <div class="bank-deposits__current-title">{{loc('bank:menu:123')}}</div>
      <div class="bank-deposits__current__desc">{{loc('bank:menu:124')}}</div>
      <div class="bank-deposits__current__desc-list">
        <div class="bank-deposits__current__desc-list-item replenishment">{{loc('bank:menu:125')}}</div>
        <div class="bank-deposits__current__desc-list-item completion">{{loc('bank:menu:126')}}</div>
      </div>
    </div>
    <div class="bank-deposits__list">
      <BankDepositsItem
        v-for="item in depositList"
        :key="item.Id"
        :item="item"
        @setModal="setModal"
      />
    </div>
  </div>
</template>

<script>
import { mapState, mapGetters } from 'vuex'
import BankDepositsItem from './BankDepositsItem'
import BankDepositsModal from './BankDepositsModal'
import deposits from '../../../configs/bank/deposits'

export default {
  name: 'BankDeposits',
  components: {
    BankDepositsItem,
    BankDepositsModal
  },
  data() {
    return {
      modal: {
        show: false,
        dataModal: {}
      },
      currentDepositIndex: 0,
    }
  },
  computed: {
    ...mapState('bank/deposit', ['currentDepositsList']),
    ...mapState('smartphone/bankPage', ['balance']),
    ...mapGetters('localization',['loc']),
    depositList() {
      return deposits.filter(item => item.MinMoney <= this.balance && item.Actual);
    },
    currentDeposit() {
      return deposits.find(element => element.Id === this.currentDepositsList[this.currentDepositIndex].depositTypes)
    },
    getLeftTime() {
      let time = this.currentDepositsList[this.currentDepositIndex].time;
      if (time > 24)
        return `${Math.floor(time/24)} д. ${time % 24} ч.`
      else if (time > 0)
        return `${time % 24} ч.`
      else 
        return `Завершен`

    }
  },
  methods: {
    setModal(id) {
      this.modal.dataModal = deposits.find(element => element.Id === id)
      this.modal.show = true
    },
    closeModal() {
      this.modal.show = false
      this.modal.dataModal = {}
    },
    closeDeposit(id) {
      window.mp.triggerServer('bank:closeDeposit', id)
    },
    prevDeposit() {
      if (this.currentDepositIndex > 0) {
        this.currentDepositIndex --
      } else {
        this.currentDepositIndex = this.currentDepositsList.length - 1
      }
    },
    nextDeposit() {
      if (this.currentDepositIndex < (this.currentDepositsList.length - 1)) {
        this.currentDepositIndex ++
      } else {
        this.currentDepositIndex = 0
      }
    }
  }
}
</script>

<style lang="scss">
.bank-deposits{
  display: flex;
  align-items: flex-start;
  position: relative;
  z-index: 1;
  width: 70rem;
  &__current{
    width: 20rem;
    display: flex;
    flex-direction: column;
    letter-spacing: 0.03em;
    &-prompt{
      display: flex;
      align-items: center;
      font-weight: normal;
      font-size: 1.2rem;
      line-height: 1.45rem;
      margin-bottom: .4rem;
      position: relative;
      text-align: left;
      width: 100%;
      span{
        position: absolute;
        right: 0;
        font-weight: bold;
        font-size: 1.2rem;
        letter-spacing: 0.03em;
        color: #B6D300;
      }
    }
    &-title{
      font-weight: bold;
      font-size: 3rem;
      line-height: 3rem;
    }
    &-desc{
      font-weight: normal;
      font-size: .9rem;
      line-height: 1.2rem;
      color: rgba(255, 255, 255, 0.4);
      margin-bottom: 1.2rem;
    }
    &__circs{
      display: flex;
      align-items: center;
      padding-bottom: 2rem;
      border-bottom: 1px solid rgba(255, 255, 255, 0.2);
      margin-bottom: 1.9rem;
      position: relative;
      &-item{
        width: 2rem;
        height: 2rem;
        border: .1rem solid rgba(255, 255, 255, 0.3);
        box-sizing: border-box;
        border-radius: .2rem;
        margin-right: .5rem;
        position: relative;
        display: flex;
        align-items: center;
        justify-content: center;
        &:before{
          content: '';
          width: 1.1rem;
          height: 1.1rem;
          background-size: contain;
          background-repeat: no-repeat;
          background-position: center;
          opacity: .3;
        }
        &.active{
          &:before{
            opacity: 1;
          }
        }
        &.replenishment{
          &:before{
            background-image: url('/img/bank/replenishment.png');
          }
        }
        &.completion{
          &:before{
            background-image: url('/img/bank/completion.png');
          }
        }
      }
      &__info{
        display: flex;
        flex-direction: column;
        margin-left: 1rem;
        font-weight: normal;
        &-value{
          font-size: 1.5rem;
          line-height: 1.8rem;
        }
        &-desc{
          font-size: 1rem;
          line-height: 1.2rem;
          color: rgba(255, 255, 255, 0.4);
        }
      }
    }
    &__info{
      display: flex;
      flex-direction: column;
      margin-bottom: 1.2rem;
      &-desc{
        font-weight: normal;
        font-size: 1rem;
        line-height: 1.2rem;
        color: rgba(255, 255, 255, 0.5);
      }
      &-balance, &-time{
        font-weight: bold;
        font-size: 2rem;
        line-height: 2rem;
      }
      &-profit{
        font-weight: normal;
        font-size: 1.3rem;
        line-height: 1.55rem;
        color: #B6D300;
        margin-bottom: 1.2rem;
      }
    }
    &-btn{
      display: flex;
      align-items: center;
      justify-content: center;
      width: 100%;
      margin-bottom: .4rem;
      height: 4rem;
      min-width: 4rem;
      border: .1rem solid #FFFFFF;
      &.btn-main{
        &:before{
          content: '';
          background-image: url('/img/bank/replenishment-icon.png');
        }
        &.withdraw{
          &:before{
            background-image: url('/img/bank/withdrawal-icon.png');
          }
        }
      }
      &:last-child{
        margin-bottom: 0;
      }
      &:hover{
        background-color: rgba(255,255,255, .1);
      }
    }
    &__desc{
      width: 20rem;
      padding-left: 1.2rem;
      position: relative;
      font-weight: normal;
      font-size: 1rem;
      line-height: 1.2rem;
      letter-spacing: 0.03em;
      margin-top: .35rem;
      margin-bottom: 1.9rem;
      &:before{
        content: '';
        width: .2rem;
        height: 100%;
        position: absolute;
        left: 0;
        top: 0;
        background: #5E37B0;
        box-shadow: 0 .5rem 5rem rgba(94, 55, 176, 0.6);
        border-radius: .2rem;
      }
      &-list{
        display: flex;
        flex-direction: column;
        &-item{
          display: flex;
          align-items: center;
          background: linear-gradient(90deg, rgba(255, 255, 255, 0.2) 0%, rgba(255, 255, 255, 0) 100%);
          border-radius: .3rem;
          position: relative;
          height: 3.5rem;
          width: 20rem;
          padding: 0 1.25rem;
          font-weight: normal;
          font-size: 1rem;
          line-height: 1.2rem;
          letter-spacing: 0.03em;
          margin-bottom: .5rem;
          &:last-child{
            margin-bottom: 0;
          }
          &:before, &:after{
            content: '';
          }
          &:before{
            content: '';
            width: 2rem;
            height: 2rem;
            margin-right: 1rem;
            background-size: contain;
            background-repeat: no-repeat;
            background-position: center;
          }
          &.replenishment{
            &:before{
              background-image: url('/img/bank/replenishment.png');
            }
          }
          &.completion{
            &:before{
              background-image: url('/img/bank/completion.png');
            }
          }
          &:after{
            width: .2rem;
            height: 2rem;
            background: #FFFFFF;
            box-shadow: 0 .5rem 5rem rgba(94, 55, 176, 0.6);
            border-radius: .2rem;
            position: absolute;
            left: 0;
          }
        }
      }
    }
    &__btn{
      width: 3rem;
      height: 3rem;
      position: absolute;
      bottom: 0;
      background-size: contain;
      background-repeat: no-repeat;
      background-position: center;
      background-image: url('/img/bank/prev.png');
      opacity: .7;
      transition: .2s;
      &:hover{
        opacity: 1;
      }
      &.prev{
        left: -1rem;
        transform: translateX(-100%) translateY(50%);
      }
      &.next{
        transform-origin: 50% 50%;
        right: -1rem;
        transform: translateX(100%) translateY(50%) rotate(180deg);
      }
    }
  }
  &__list{
    width: 43rem;
    height: calc(100vh - (100vh - (43rem))/2);
    overflow-y: auto;
    position: absolute;
    right: 0;
    top: 0;
    padding-right: 1.3rem;
    &::-webkit-scrollbar{
      width: .3rem;
      background-color: transparent;
      &-thumb{
        background-color: rgba(255, 255, 255, 0.3);
        border-radius: .5rem;
      }
    }
  }
}
</style>
