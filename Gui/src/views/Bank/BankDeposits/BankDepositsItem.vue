<template>
  <div 
    class="bank-deposits-item"
    :style="{backgroundImage: `url(/img/bank/deposits/${item.Image}.png)`}"
  >
    <div class="bank-deposits-item-title">{{loc(item.Title)}}</div>
    <div class="bank-deposits-item-desc">{{loc(item.Description)}}</div>
    <div class="bank-deposits-item__info">
      <div class="bank-deposits-item__info-col">
        <div class="bank-deposits-item__info-value">{{item.AnnualRate}}%</div>
        <div class="bank-deposits-item__info-desc">{{loc('bank:menu:139')}}</div>
      </div>
      <div class="bank-deposits-item__info-col">
        <div class="bank-deposits-item__info-value">{{item.MinMoney.toLocaleString('ru')}}$</div>
        <div class="bank-deposits-item__info-desc">{{loc('bank:menu:140')}}</div>
      </div>
      <div class="bank-deposits-item__info-col">
        <div class="bank-deposits-item__info-value">{{loc(`bank:menu:141@${item.MaxDays}`)}}</div>
        <div class="bank-deposits-item__info-desc">{{loc('bank:menu:142')}}</div>
      </div>
    </div>
    <div class="bank-deposits-item__interaction">
      <div 
        class="bank-deposits-item__interaction-btn btn-main"
        @click="watchDeposit(item.Id)"
      >{{loc('bank:menu:143')}}</div>
      <div :class="[{active: item.IsRefill}, 'bank-deposits-item-provision', 'replenishment']">
        <div class="bank-prompt">{{item.IsRefill ? loc('bank:menu:112') : loc('bank:menu:113')}}</div>
      </div>
      <div :class="[{active: item.IsWithdraw}, 'bank-deposits-item-provision', 'completion']">
        <div class="bank-prompt">{{item.IsWithdraw ? loc('bank:menu:114') : loc('bank:menu:115')}}</div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
export default {
  name: 'BankDepositsItem',
  props: {
    item: Object
  },
  computed: {
    ...mapGetters('localization',['loc']),
  },
  methods: {
    watchDeposit(id) {
      this.$emit('setModal', id)
    }
  }
}
</script>

<style lang="scss">
.bank-deposits-item{
  padding: 2.5rem 2rem 4.25rem 2rem;
  display: flex;
  flex-direction: column;
  justify-content: space-between;
  border-radius: 1rem;
  background-color: #030C20;
  background-size: contain;
  background-repeat: no-repeat;
  background-position: right;
  position: relative;
  height: 15rem;
  margin-bottom: 2.5rem;
  &-title{
    font-weight: bold;
    font-size: 2rem;
    line-height: 2rem;
    letter-spacing: 0.03em;
    margin-bottom: .2rem;
  }
  &-desc{
    font-size: 1rem;
    line-height: 1.2rem;
    letter-spacing: 0.03em;
    color: rgba(255, 255, 255, 0.4);
  }
  &__info{
    display: flex;
    align-items: center;
    &-col{
      display: flex;
      flex-direction: column;
      margin-right: 2.7rem;
    }
    &-value{
      font-weight: normal;
      font-size: 1.5rem;
      line-height: 1.8rem;
    }
    &-desc{
      font-weight: normal;
      font-size: 1rem;
      line-height: 1.2rem;
      letter-spacing: 0.03em;
      color: rgba(255, 255, 255, 0.4);
    }
  }
  &__interaction{
    position: absolute;
    bottom: 0;
    transform: translateY(1rem);
    left: 2rem;
    display: flex;
    align-items: flex-start;
    justify-content: flex-start;
    &-btn{
      display: flex;
      align-items: center;
      justify-content: center;
      width: 10rem;
      height: 4rem;
      margin-right: 1rem;
      background: #5E37B0;
      border-radius: .2rem;
      font-size: 1rem;
    }
  }
  &-provision{
    width: 2rem;
    height: 2rem;
    border: .1rem solid rgba(255, 255, 255, 0.3);
    box-sizing: border-box;
    border-radius: .2rem;
    margin-right: 1rem;
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
}
</style>
