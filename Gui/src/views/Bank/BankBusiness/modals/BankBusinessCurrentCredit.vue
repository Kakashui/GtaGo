<template>
  <div class="bank-business-current-credit bank-modal">
    <div class="bank-business-current-credit__wrap bank-modal__wrap">
      <div 
        class="btn-close"
        @click="$emit('closeModal')"
      ></div>
      <div class="bank-modal-title">{{loc('bank:menu:216')}}</div>
      <div class="bank-modal-desc bank-business-current-credit-desc">
        <div>{{loc('bank:menu:216')}}Если не погасите кредит вовремя, то банк заберает бизнес себе</div>
      </div>
      <div class="bank-modal-desc bank-business-current-credit-desc">
        <div>Погасите кредит до:</div>
        <div class="date">{{currentCredit.date}}</div>
      </div>
      <div class="bank-business-current-credit__prompt">
        <div class="bank-business-current-credit__prompt-desc">Сумма долга</div>
        <div class="bank-business-current-credit__prompt-value">$ {{finishSumm.toLocaleString('ru')}}</div>
        <div 
          class="bank-modal-btn btn-main"
          @click="repayBizCredit"
        >Погасить</div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapState, mapGetters } from 'vuex'
export default {
  name: 'BankBusinessCredit',
  computed: {
    ...mapState('bank/business', ['currentCredit']),
    ...mapGetters('localization',['loc']),
    finishSumm() {
      return Math.floor(this.currentCredit.approvedAmount * (1 + (this.currentCredit.rate)))
    }
  },
  methods: {
    repayBizCredit() {
      window.mp.triggerServer('Bank:repayBizCredit')
      this.$emit('closeModal')
    }
  }
}
</script>

<style lang="scss">
.bank-business-current-credit{
  .bank-modal-title{
    margin-bottom: .5rem;
  }
  &-desc{
    display: flex;
    flex-direction: column;
    align-items: flex-start;
    justify-content: center;
    position: relative;
    padding-left: 1rem;
    margin-bottom: .7rem;
    div{
      color: #000000;
      &.date{
        color: #EE443A;
      }
    }
    &:before{
      content: '';
      height: 1.9rem;
      width: .2rem;
      background: #EE443A;
      border-radius: .5rem;
      position: absolute;
      left: 0;
    }
  }
  &__info{
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    padding: 1.2rem 0;
    border-top: 1px solid rgba(0, 0, 0, 0.1);
    border-bottom: 1px solid rgba(0, 0, 0, 0.1);
    margin-bottom: 1rem;
    position: relative;
    &:before{
      content: '';
      width: 8.85rem;
      height: 5.45rem;
      position: absolute;
      left: -6.5rem;
      background-size: contain;
      background-repeat: no-repeat;
      background-position: center;
      background-image: url('/img/bank/credit-decorate.png');
    }
    .bank-modal-desc{
      margin-bottom: .3rem;
    }
    &-value{
      font-weight: normal;
      font-size: 2rem;
      line-height: 2.4rem;
      letter-spacing: 0.03em;
      color: #5E37B0;
    }
  }
  &__prompt{
    display: flex;
    flex-direction: column;
    align-items: flex-start;
    justify-content: center;
    padding-top: 1.5rem;
    border-top: 1px solid rgba(0, 0, 0, 0.1);
    font-weight: normal;
    font-size: 1rem;
    line-height: 1.2rem;
    letter-spacing: 0.03em;
    margin-bottom: 0;
    position: relative;
    &-desc{
      color: rgba(0, 0, 0, 0.4);
      margin-bottom: .3rem;
    }
    &-value{
      font-weight: normal;
      font-size: 2rem;
      line-height: 2.4rem;
      letter-spacing: 0.03em;
      color: #5E37B0;
    }
    .bank-modal-btn.btn-main{
      position: absolute;
      right: 0;
      width: 8rem;
    }
  }
}
</style>
