<template>
  <div class="bank-pay-for-biz bank-modal">
    <div class="bank-pay-for-biz__wrap bank-modal__wrap">
      <div 
        class="btn-close"
        @click="$emit('closeModal')"
      ></div>
      <div class="bank-modal-title">{{loc('bank:menu:37')}}</div>
      <div class="bank-modal-desc">{{loc(`bank:menu:38@${(bizTaxMax - bizTaxBalance).toLocaleString('ru')}`)}}</div>
      <div class="bank-pay-for-biz__info bank-modal__block-shadow biz">
        <div class="bank-modal-prompt biz">{{loc('bank:menu:39')}}</div>
        <div class="bank-pay-for-biz__balance">$ {{bizTaxBalance.toLocaleString('ru')}} <span>/ {{bizTaxMax.toLocaleString('ru')}}</span></div>
      </div>
      <div class="bank-modal-prompt">{{loc('bank:menu:40')}}</div>
      <div class="bank-pay-for-biz__input bank-modal__input">
        <input 
          type="number"
          v-model="currentSumm"
        >
      </div>
      <div 
        class="bank-modal-btn btn-main"
        @click="payForBiz"
      >{{loc('bank:menu:41')}}</div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
export default {
  name: 'BankModalPayForBiz',
  props: {
    dataModal: Object
  },
  data() {
    return {
      currentSumm: null
    }
  },
  computed: {
    ...mapState('bank/business', ['bizTaxBalance', 'bizTaxMax']),
    ...mapGetters('localization',['loc'])
  },
  methods: {
    payForBiz() {
      if (this.currentSumm && this.currentSumm > 0) {
        window.mp.triggerServer('bank:payBizTax', this.currentSumm)
        this.$emit('closeModal')
      }
    }
  }
}
</script>

<style lang="scss">
.bank-pay-for-biz{
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
