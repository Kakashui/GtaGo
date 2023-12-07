<template>
  <div class="bank-withdraw bank-modal">
    <div class="bank-withdraw__wrap bank-modal__wrap">
      <div 
        class="btn-close"
        @click="$emit('closeModal')"
      ></div>
      <div class="bank-modal-title">{{loc('bank:menu:10')}}</div>
      <div class="bank-modal-desc">{{loc('bank:menu:11')}}</div>
      <div class="bank-withdraw__info bank-modal__block-shadow cash">
        <div class="bank-modal-prompt">{{loc('bank:menu:12')}}</div>
        <div class="bank-withdraw__balance">$ {{dataModal.balance.toLocaleString('ru')}}</div>
      </div>
      <div class="bank-modal-prompt">{{loc('bank:menu:13')}}</div>
      <div class="bank-withdraw__input bank-modal__input">
        <input 
          type="number"
          v-model="currentSumm"
        >
      </div>
      <div 
        class="bank-modal-btn btn-main"
        @click="withdraw"
      >{{loc('bank:menu:14')}}</div>
    </div>
  </div>
</template>

<script>
import { mapGetters } from 'vuex';
export default {
  name: 'BankModalWithdraw',
  props: {
    dataModal: Object
  },
  data() {
    return {
      currentSumm: null
    }
  },
  computed: {
    ...mapGetters('localization',['loc'])
  },
  methods: {
    withdraw() {
      if (!this.currentSumm ||  this.currentSumm <= 0) 
        return;
      switch (this.dataModal.type) {
        case 'personal':
        case 'business':
        case 'family':
        case 'fraction':
          window.mp.triggerServer('bank:withdraw', this.currentSumm, this.dataModal.type, 0)
          break;
        case 'deposit':
          window.mp.triggerServer('bank:withdraw', this.currentSumm, this.dataModal.type, this.dataModal.id)
          break;
      }
      this.$emit('closeModal')
    }
  }
}
</script>

<style lang="scss">
.bank-withdraw{
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
