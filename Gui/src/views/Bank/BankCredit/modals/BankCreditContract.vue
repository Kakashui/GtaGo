<template>
  <div class="bank-credit-contract bank-main-modal">
    <div class="bank-credit-contract__wrap bank-main-modal__wrap">
      <div 
        class="btn-close"
        @click="$emit('closeModal')"
      ></div>
      <div class="bank-main-modal-title">{{loc('bank:menu:243')}}</div>
      <div class="bank-credit-contract-text">{{loc('bank:menu:240')}}</div>
      <div class="bank-credit-contract-text">{{loc('bank:menu:241')}}</div>
      <div class="bank-credit-contract-text">{{loc('bank:menu:242')}}</div>
      <div class="bank-credit-contract__consent">
        <div 
          :class="[{ active: currentConsent }, 'bank-credit-contract__consent-switch']"
          @click="setCurrentConsent"
        >{{loc('bank:menu:205')}}</div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapMutations, mapState, mapGetters } from 'vuex'
export default {
  name: 'BankMainFines',
  data() {
    return {
      currentConsent: false
    }
  },
  computed: {
    ...mapState('bank/credit', ['contractConsent']),
    ...mapGetters('localization',['loc']),
  },
  methods: {
    ...mapMutations('bank/credit', ['setContractConsent']),
    setCurrentConsent() {
      this.currentConsent = !this.currentConsent
      this.setContractConsent(this.currentConsent)
    },
  },
  mounted() {
    this.currentConsent = this.contractConsent
  }
}
</script>

<style lang="scss">
.bank{
  .bank-credit-contract{
    .bank-main-modal-title{
      margin-bottom: 1rem;
      font-weight: bold;
      font-size: 3.5rem;
      line-height: 3.5rem;
    }
    &__wrap{
      width: 30rem;
    }
    &-text{
      font-weight: normal;
      font-size: 1rem;
      line-height: 1.3rem;
      letter-spacing: 0.03em;
      color: rgba(38, 38, 64, 0.7);
      margin-bottom: .75rem;
    }
    &__consent{
      display: flex;
      align-items: center;
      justify-content: space-between;
      padding-top: 1.1rem;
      border-top: 1px solid rgba(94, 55, 176, 0.2);
      margin-top: 1.25rem;
      &:after{
        content: '';
        width: 8.85rem;
        height: 5rem;
        background-size: contain;
        background-repeat: no-repeat;
        background-position: center;
        background-image: url('/img/bank/signature.png');
      }
      &-switch{
        font-weight: normal;
        font-size: 1rem;
        line-height: 1.3rem;
        letter-spacing: 0.03em;
        color: #262640;
        display: flex;
        align-items: center;
        max-width: 13rem;
        &:before{
          content: '';
          width: 2rem;
          min-width: 2rem;
          height: 2rem;
          background-size: contain;
          background-repeat: no-repeat;
          background-position: center;
          margin-right: 1rem;
          border: .2rem solid #5E37B0;
          background-color: transparent;
          box-sizing: border-box;
          border-radius: .2rem;
          transition: .2s;
        }
        &.active{
          &:before{
            background-color: #5E37B0;
            background-image: url('/img/bank/done.png');
          }
        }
      }
    }
  }
}
</style>
