<template>
  <div class="bank-credit">
    <div class="bank-credit__interaction">
      <div class="bank-credit-title ">{{loc('bank:menu:144')}}</div>
      <div class="bank-credit-desc">{{loc('bank:menu:145')}}</div>
      <div class="bank-credit__line-decorate"></div>
      <div class="bank-credit__interaction-title">{{loc('bank:menu:146')}}</div>
      <div class="bank-credit-desc">{{loc('bank:menu:147')}}</div>
      <div class="bank-credit__interaction-property personal">
        <div class="bank-credit__interaction-property-title">{{loc('bank:menu:148')}}</div>
        <template v-if="selectedCreditProperty && selectedCreditProperty.familyProperty">
          <div class="bank-credit__interaction-property-desc"><span>{{selectedCreditProperty.name}}</span></div>
          <div class="bank-credit__interaction-property-desc">{{loc('bank:menu:149')}}<span>{{selectedCreditProperty.price.toLocaleString('ru')}}</span></div>
        </template>
        <div 
          class="bank-credit__interaction-property-desc"
          v-else-if="selectedCreditProperty"
        >{{loc('bank:menu:150')}}</div>
        <div 
          class="bank-credit__interaction-property-desc"
          v-else
        >
          <span>{{loc('bank:menu:151')}}</span>
        </div>
        <div 
          class="bank-credit__interaction-property-btn"
          @click="setCurrentModal({
            component: 'Property',
            dataModal: {
              familyProperty: true,
            }
          })"
        ></div>
      </div>
      <div class="bank-credit__interaction-property family">
        <div class="bank-credit__interaction-property-title">{{loc('bank:menu:152')}}</div>
        <template v-if="selectedCreditProperty && !selectedCreditProperty.familyProperty">
          <div class="bank-credit__interaction-property-desc"><span>{{selectedCreditProperty.name}}</span></div>
          <div class="bank-credit__interaction-property-desc">{{loc('bank:menu:149')}}<span>${{selectedCreditProperty.price.toLocaleString('ru')}}</span></div>
        </template>
        <div 
          class="bank-credit__interaction-property-desc"
          v-else-if="selectedCreditProperty"
        >{{loc('bank:menu:150')}}</div>
        <div 
          class="bank-credit__interaction-property-desc"
          v-else
        >
          <span>{{loc('bank:menu:151')}}</span>
        </div>
        <div 
          class="bank-credit__interaction-property-btn"
          @click="setCurrentModal({
            component: 'Property',
            dataModal: {
              familyProperty: false,
            }
          })"
        ></div>
      </div>
      <div class="bank-credit__line-decorate"></div>
      <div 
        class="bank-credit__interaction-btn"
        @click="setContractModal"
      >{{loc('bank:menu:153')}}</div>
    </div>
    <Options :isPledged="true"/>
    <Current @setCurrentModal="setCurrentModal"/>
    <transition name="bank-modal">
      <Contract
        v-if="showContractModal"
        @closeModal="closeContractModal"
      />
    </transition>
    <transition name="bank-modal">
      <component
        v-if="currentModal.component"
        :is="currentModal.component"
        :dataModal="currentModal.dataModal"
        @closeModal="closeModal"
        @setCurrentModal="setCurrentModal"
      />
    </transition>
  </div>
</template>

<script>
import Options from './BankCreditOptions'
import Current from './BankCreditCurrent'
import Contract from './modals/BankCreditContract'
import PaymentsList from './modals/BankCreditPaymentsList'
import PayOff from './modals/BankCreditPayOff'
import Pay from './modals/BankCreditPay'
import Property from './modals/BankCreditProperty'
import Fast from './modals/BankCreditFast'
import { mapState, mapGetters } from 'vuex'
export default {
  name: 'BankCredit',
  components: {
    Options,
    Current,
    Contract,
    PaymentsList,
    PayOff,
    Pay,
    Property,
    Fast,
  },
  data() {
    return {
      showContractModal: false,
      currentModal: {
        component: null,
        dataModal: {}
      },
      creditOptionsData: {
        minAmount: 100000,
        maxAmount: 1000000,
        minDays: 1,
        maxDays: 365,
      },
    }
  },
  computed: {
    ...mapState('bank/credit', ['selectedCreditProperty']),
    ...mapGetters('localization',['loc']),
  },
  methods: {
    setContractModal() {
      this.showContractModal = true
    },
    closeContractModal() {
      this.showContractModal = false
    },
    setCurrentModal({component, dataModal}) {
      this.currentModal.dataModal = dataModal
      this.currentModal.component = component
    },
    closeModal() {
      this.currentModal.component = null
      this.currentModal.dataModal = {}
    },
  }
}
</script>

<style lang="scss">
.bank-credit{
  display: flex;
  justify-content: space-between;
  height: 43rem;
  &-title{
    font-weight: bold;
    font-size: 3.5rem;
    line-height: 3.5rem;
    letter-spacing: 0.03em;
    margin-bottom: .5rem;
  }
  &__line-decorate{
    width: 100%;
    height: 1px;
    margin: 1rem 0;
    background: rgba(255, 255, 255, 0.2);
    border-radius: .2rem;
  }
  &-desc{
    font-weight: normal;
    font-size: 1rem;
    line-height: 1.2rem;
    letter-spacing: 0.03em;
    color: rgba(255, 255, 255, 0.5);
    padding-left: 1rem;
    position: relative;
    &:before{
      content: '';
      width: .2rem;
      height: 100%;
      position: absolute;
      left: 0;
      top: 0;
      background: #7E55D5;
      box-shadow: 0 0 1.5rem #5E37B0;
      border-radius: .2rem;
    }
  }
  &__interaction, &-options, &-current{
    height: 100%;
  }
  &__interaction{
    width: 20.5rem;
    display: flex;
    flex-direction: column;
    justify-content: space-between;
    &-title{
      font-weight: bold;
      font-size: 2.5rem;
      line-height: 2.5rem;
      letter-spacing: 0.03em;
      max-width: 90%;
    }
    &-property{
      display: flex;
      flex-direction: column;
      justify-content: center;
      align-items: flex-start;
      overflow: hidden;
      padding-left: 2rem;
      height: 7rem;
      position: relative;
      background: rgba(3, 12, 32, 0.7);
      border-radius: .6rem;
      font-weight: normal;
      letter-spacing: 0.03em;
      &-title{
        font-size: 1.5rem;
        line-height: 1.8rem;
      }
      &-desc{
        font-size: 1rem;
        line-height: 1.2rem;
        white-space: pre;
        color: rgba(255, 255, 255, 0.5);
        span{
          color: #fff;
        }
      }
      &-btn{
        width: 2.1rem;
        height: 2.1rem;
        background: #FFFFFF;
        display: flex;
        align-items: center;
        justify-content: center;
        position: absolute;
        right: 1.5rem;
        top: 50%;
        transform: translateY(-50%);
        box-shadow: 0 .5rem 2rem rgba(24, 33, 65, 0.31);
        border-radius: .4rem;
        &:before{
          content: '';
          width: .6rem;
          height: .6rem;
          background-size: contain;
          background-repeat: no-repeat;
          background-position: center;
          background-image: url('/img/bank/add-property.png');
        }
      }
      &:before{
        content: '';
        width: 7rem;
        height: 7rem;
        background-size: contain;
        background-repeat: no-repeat;
        background-position: center;
        position: absolute;
        right: 0;
        top: 0;
        pointer-events: none;
      }
      &.personal{
        &:before{
          background-image: url('/img/bank/personal.png');
        }      
      }
      &.family{
        &:before{
          background-image: url('/img/bank/family.png');
        }        
      }
    }
    &-btn{
      border: .1rem solid rgba(255, 255, 255, 0.3);
      border-radius: .2rem;
      display: flex;
      align-items: center;
      justify-content: center;
      height: 3.5rem;
      font-weight: normal;
      font-size: 1rem;
      letter-spacing: 0.03em;
      transition: .2s;
      &:hover{
        border-color: #fff;
        background-color: rgba(255, 255, 255, 0.1);
      }
    }
  }
}
</style>
