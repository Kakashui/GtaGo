<template>
  <div class="bank-main-pay-for-house bank-main-modal">
    <div class="bank-main-pay-for-house__wrap bank-main-modal__wrap">
      <div 
        class="btn-close"
        @click="$emit('closeModal')"
      ></div>
      <div class="bank-main-modal-title">{{loc('bank:menu:80')}}</div>
      <div class="bank-main-modal-desc">{{loc(`bank:menu:81@${(houseBalanceMax - houseBalance).toLocaleString('ru')}`)}}</div>
      <div class="bank-main-pay-for-house__info bank-main-modal__block-shadow house">
        <div class="bank-main-modal-prompt house">{{loc('bank:menu:82')}}</div>
        <div class="bank-main-pay-for-house__balance">$ {{houseBalance.toLocaleString('ru')}} <span>/ {{houseBalanceMax.toLocaleString('ru')}}</span></div>
      </div>
      <div class="bank-main-modal-prompt">{{loc('bank:menu:83')}}</div>
      <div class="bank-main-pay-for-house__input bank-main-modal__input">
        <input 
          type="number"
          v-model="currentSumm"
        >
      </div>
      <div 
        class="bank-main-modal-btn btn-main"
        @click="payForHouse"
      >{{loc('bank:menu:84')}}</div>
    </div>
  </div>
</template>

<script>
import { mapState, mapGetters } from 'vuex'
export default {
  name: 'BankMainPayForHouse',
  data() {
    return {
      currentSumm: null
    }
  },
  computed: {
    ...mapState('bank', ['houseBalance', 'houseBalanceMax']),
    ...mapGetters('localization',['loc']),
  },
  methods: {
    payForHouse() {
      if (this.currentSumm && this.currentSumm > 0) {
        window.mp.triggerServer('bank:payHouseTax', this.currentSumm, true)
        this.$emit('closeModal')
      }
    }
  }
}
</script>

<style lang="scss">
.bank-main-pay-for-house{
  &__wrap{

  }
  &__info{
    margin-bottom: 2rem;
  }
  &__balance{
    font-weight: normal;
    font-size: 1.6rem;
    line-height: 1.9rem;
    letter-spacing: 0.03em;
    color: #000000;
    span{
      color: rgba(0, 0, 0, 0.3);
    }
  }
  &__input{
    &:before{
      content: '$';
      pointer-events: none;
      position: absolute;
      left: 0;
    }
    input{
      padding-left: 1.5rem;
    }
  }
}
</style>
