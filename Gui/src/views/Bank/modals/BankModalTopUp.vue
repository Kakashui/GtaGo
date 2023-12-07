<template>
  <div class="bank-top-up bank-modal">
    <div class="bank-top-up__wrap bank-modal__wrap">
      <div 
        class="btn-close"
        @click="$emit('closeModal')"
      ></div>
      <div class="bank-modal-title">{{loc('bank:menu:32')}}</div>
      <div class="bank-modal-desc">{{loc('bank:menu:33')}}</div>
      <div class="bank-top-up__info bank-modal__block-shadow cash">
        <div class="bank-modal-prompt">{{loc('bank:menu:34')}}</div>
        <div class="bank-top-up__balance">$ {{dataModal.balance.toLocaleString('ru')}}</div>
      </div>
      <div class="bank-modal-prompt">{{loc('bank:menu:35')}}</div>
      <div class="bank-top-up__input bank-modal__input">
        <input 
          type="number"
          v-model="currentSumm"
        >
      </div>
      <div 
        class="bank-modal-btn btn-main"
        @click="topUp"
      >{{loc('bank:menu:36')}}</div>
    </div>
  </div>
</template>

<script>
import { mapGetters } from 'vuex';
export default {
  name: 'BankModalTopUp',
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
    topUp() {
      if (!this.currentSumm ||  this.currentSumm <= 0) 
        return;
      switch (this.dataModal.type) {
        case 'personal':
        case 'family':
        case 'business':
        case 'fraction':
          window.mp.triggerServer('bank:topUp', this.currentSumm, this.dataModal.type, 0)
          break;
        case 'deposit':
          window.mp.triggerServer('bank:topUp', this.currentSumm, this.dataModal.type, this.dataModal.id)
          break;
      }
      
      this.$emit('closeModal')
      
    }
  }
}
</script>

<style lang="scss">
.bank-top-up{
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
