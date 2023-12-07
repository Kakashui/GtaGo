<template>
  <div class="bank-modal-bonus bank-main-modal">
    <div class="bank-modal-bonus__wrap bank-main-modal__wrap">
      <div 
        class="btn-close"
        @click="$emit('closeModal')"
      ></div>
      <template v-if="currentPage === 0">
        <div class="bank-main-modal-title">{{loc('bank:menu:46')}}</div>
        <div class="bank-main-modal-desc">{{loc('bank:menu:47')}}</div>
        <div 
          class="bank-modal-bonus__list"
          v-if="dataModal.staffList.length > 0"
        >
          <div
            class="bank-modal-bonus__list-item"
            v-for="item in dataModal.staffList"
            :key="item.id"
          >
            <div class="bank-modal-bonus__list-item__info">
              <div class="bank-modal-bonus__list-item__info-name">{{item.name}}</div>
              <div class="bank-modal-bonus__list-item__info-payment">{{loc(getLastPay(item))}}</div>
            </div>
            <div 
              class="bank-modal-bonus__list-item__info-btn btn-main"
              @click="selectWorker(item)"
            >{{loc('bank:menu:48')}}</div>
          </div>
        </div>
        <div 
          class="bank-modal-bonus__desc"
          v-else
        >{{loc('bank:menu:49')}}</div>
      </template>
      <template v-if="currentPage === 1">
        <BtnBack 
          class="bank-modal-bonus__back"
          @click="backToList()"
        />
        <div class="bank-modal-bonus__name">{{currentWorker.name}}</div>
        <div class="bank-modal-bonus__payment">{{loc(getLastPay(currentWorker))}}</div>
        <div class="bank-modal__block-shadow bank-modal-bonus__input">
          <div class="bank-modal-bonus__input-desc">{{loc('bank:menu:50')}}</div>
          <input 
            type="number"
            placeholder=""
            v-model.number="currentSumm"
          >
        </div>
        <div 
          class="bank-modal-btn btn-main"
          @click="payBonus"
        >{{loc('bank:menu:51')}}</div>
      </template>
    </div>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
import BtnBack from './components/BtnBack'
export default {
  name: 'BankModalBonus',
  components: {
    BtnBack
  },
  props: {
    dataModal: Object
  },
  data() {
    return {
      currentWorker: null,
      currentPage: 0,
      currentSumm: null,
    }
  },
  computed: {
    ...mapGetters('localization',['loc'])
  },
  methods: {
    selectWorker(worker) {
      this.currentWorker = worker
      this.currentPage = 1
    },
    backToList() {
      this.currentPage = 0
      this.currentWorker = null
      this.currentSumm = null
    },
    payBonus() {
      if (this.currentSumm > 0) {
        window.mp.triggerServer('bank:payBonus', this.currentWorker.uuid, this.currentSumm, this.dataModal.type)
        this.$emit('closeModal')
      }
    },
    getLastPay(item) {
      if (item.date)
        return `bank:menu:3@${item.date}@${item.money}`
      return 'bank:menu:2'
    }
  }
}
</script>

<style lang="scss">
.bank{
  .bank-modal-bonus{
    z-index: 1;
    &__btn{
      margin-bottom: .7rem;
    }
    .bank-main-modal-title{
      max-width: 22rem;
    }
    &__wrap.bank-main-modal__wrap{
      width: fit-content;
      padding: 3rem 2.5rem 2rem 2.5rem;
    }
    &__list{
      max-height: 30rem;
      overflow-y: auto;
      padding-right: 1.35rem;
      margin-right: -1.35rem;
      &::-webkit-scrollbar{
        width: .2rem;
        background-color: transparent;
        &-thumb{
          background: #5E37B0;
          border-radius: .3rem;
        }
      }
      &-item{
        width: 22rem;
        min-width: 22rem;
        padding: .75rem 0;
        display: flex;
        flex-direction: column;
        position: relative;
        border-top: 1px solid rgba(94, 55, 176, 0.2);
        &__info{
          display: flex;
          flex-direction: column;
          &-name{
            font-weight: normal;
            font-size: 1.2rem;
            line-height: 1.45rem;
            color: #200C49;
            margin-bottom: .1rem;
          }
          &-payment{
            font-weight: normal;
            font-size: .9rem;
            line-height: 1.1rem;
            color: rgba(94, 55, 176, 0.4);
          }
          &-btn.btn-main{
            position: absolute;
            right: 0;
            width: 6rem;
            height: 2.4rem;
            border: .1rem solid #5E37B0;
            border-radius: 5rem;
            background-color: transparent;
            font-weight: normal;
            font-size: 1rem;
            color: #5E37B0;
            &:hover{
              background-color: #5E37B0;
              color: #fff;
            }
          }
        }
        &:last-child{
          margin-bottom: 0;
        }
      }
    }
    &__desc{
      padding: 0 0 2rem 1rem;
      font-size: 2rem;
      line-height: 2.2rem;
      letter-spacing: 0.03em;
      color: #200C49;
    }
    &__name{
      font-weight: bold;
      font-size: 2rem;
      line-height: 2rem;
      text-align: center;
      letter-spacing: 0.03em;
      color: #000000;
      margin-bottom: .5rem;
    }
    &__payment{
      font-weight: normal;
      font-size: .9rem;
      line-height: 1.1rem;
      text-align: center;
      color: rgba(94, 55, 176, 0.4);
      margin-bottom: 1rem;
    }
    &__input{
      width: 22rem;
      display: flex;
      flex-direction: column;
      align-items: center;
      justify-content: center;
      margin-bottom: 1.5rem;
      &-desc{
        font-weight: normal;
        font-size: 1rem;
        line-height: 1.2rem;
        letter-spacing: 0.03em;
        color: rgba(0, 0, 0, 0.4);
      }
      input{
        width: 100%;
        text-align: center;
        font-weight: normal;
        font-size: 2.5rem;
        line-height: 3rem;
        letter-spacing: 0.03em;
        color: #000000;
        &::placeholder{
          color: #000000;
        }
        &::-webkit-outer-spin-button,
        &::-webkit-inner-spin-button {
          -webkit-appearance: none;
          margin: 0;
        }
      }
    }
  }
}
</style>
