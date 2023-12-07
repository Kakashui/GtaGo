<template>
  <div class="bank-credit-fast bank-modal">
    <div class="bank-credit-fast__wrap">
      <div 
        class="btn-close"
        @click="closeFast()"
      ></div>
      <div class="bank-credit-fast__prompt">
        <div class="bank-credit-fast__prompt-title">{{loc('bank:menu:176')}}</div>
        <div class="bank-credit-fast__prompt-desc">{{loc('bank:menu:177')}}</div>
      </div>
      <Options :isPledged="false"/>
    </div>
  </div>
</template>

<script>
import { mapMutations, mapState, mapGetters } from 'vuex'
import Options from '../BankCreditOptions'

export default {
  name: 'BankCreditProperty',
  components: {
    Options
  },
  computed: {
    ...mapState('bank/credit', ['selectedCreditProperty']),
    ...mapGetters('localization',['loc']),
    
  },
  methods: {
    ...mapMutations('bank/credit', ['setMaxAmountCredit']),
  
  closeFast() {
    if (this.selectedCreditProperty)
      this.setMaxAmountCredit(this.selectedCreditProperty.price);
    this.$emit('closeModal')
  }
  },
  mounted() {
    this.setMaxAmountCredit(100000);
  }
}
</script>

<style lang="scss">
.bank-credit-fast{
  color: #fff;
  font-weight: normal;
  .bank-modal-title{
    margin-bottom: 1rem;
    color: #fff;
  }
  &__wrap{
    background-color: transparent;
    padding: 0;
    position: relative;
    margin-left: 3.5rem;
  }
  &__prompt{
    position: absolute;
    left: -2rem;
    top: 0;
    transform: translateX(-100%);
    display: flex;
    flex-direction: column;
    width: 20.5rem;
    &-title{
      font-weight: bold;
      font-size: 3.5rem;
      line-height: 3.5rem;
      margin-bottom: .5rem;
      max-width: 90%;
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
  }
}
</style>
