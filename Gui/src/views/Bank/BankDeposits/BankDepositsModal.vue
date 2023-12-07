<template>
  <div class="bank-deposits-modal">
    <div class="bank-deposits-modal__wrap">
      <div 
        class="btn-close"
        @click="$emit('closeModal')"
      ></div>
      <div
        class="bank-deposits-modal-info"
        :style="{ backgroundImage: `url(/img/bank/deposits/${dataModal.Image}.png)` }"
      >
        <div class="bank-deposits-modal-title">{{ loc(dataModal.Title) }}</div>
        <div class="bank-deposits-modal-desc">{{ loc(dataModal.Description) }}</div>
        <div class="bank-deposits-modal__info">
          <div class="bank-deposits-modal__info-col">
            <div class="bank-deposits-modal__info-value">
              {{ dataModal.AnnualRate }}%
            </div>
            <div class="bank-deposits-modal__info-desc">{{loc('bank:menu:127')}}</div>
          </div>
          <div class="bank-deposits-modal__info-col">
            <div class="bank-deposits-modal__info-value">
              {{ dataModal.MinMoney.toLocaleString("ru") }}$
            </div>
            <div class="bank-deposits-modal__info-desc">{{loc('bank:menu:128')}}</div>
          </div>
          <div class="bank-deposits-modal__info-col">
            <div class="bank-deposits-modal__info-value">
              до {{ dataModal.MaxDays }} д.
            </div>
            <div class="bank-deposits-modal__info-desc">{{loc('bank:menu:129')}}</div>
          </div>
        </div>
        <div class="bank-deposits-modal__provisions">
          <div :class="[{ active: dataModal.IsRefill }, 'bank-deposits-modal-provision', 'replenishment']">
            <div class="bank-prompt">{{dataModal.IsRefill ? loc('bank:menu:112') : loc('bank:menu:113')}}</div>
          </div>
          <div :class="[{ active: dataModal.IsWithdraw }, 'bank-deposits-modal-provision', 'completion']">
            <div class="bank-prompt">{{dataModal.IsWithdraw ? loc('bank:menu:114') : loc('bank:menu:115')}}</div>
          </div>
        </div>
      </div>
      <div class="bank-deposits-modal-interaction">
        <div class="bank-deposits-modal-title">{{loc('bank:menu:130')}}</div>
        <div class="bank-deposits-modal-desc">{{loc('bank:menu:131')}}</div>
        <div class="bank-deposits-modal-interaction__calculation">
          <div>
            <div class="bank-deposits-modal-interaction__calculation__range">
              <div class="bank-deposits-modal-interaction__calculation__range-title">{{loc('bank:menu:132')}}</div>
              <div class="bank-deposits-modal-interaction__calculation__range-wrap">
                <div class="bank-deposits-modal-interaction__calculation__range-value">{{depositSumm}}</div>
                <div class="bank-deposits-modal-interaction__calculation__range-desc">$</div>
                <range-slider
                  :min="dataModal.MinMoney"
                  :max="getMaxAmount"
                  step="1000"
                  v-model.number="depositSumm"
                />
              </div>
            </div>
            <div class="bank-deposits-modal-interaction__calculation__range">
              <div class="bank-deposits-modal-interaction__calculation__range-title">{{loc('bank:menu:133')}}</div>
              <div class="bank-deposits-modal-interaction__calculation__range-wrap">
                <div class="bank-deposits-modal-interaction__calculation__range-value">{{loc(`bank:menu:135@${depositTerm}`)}}</div>
                <div class="bank-deposits-modal-interaction__calculation__range-desc">{{loc(`bank:menu:134@${dataModal.MinDays}@${dataModal.MaxDays}`)}}</div>
                <range-slider
                  :min="dataModal.MinDays"
                  :max="dataModal.MaxDays"
                  step="1"
                  v-model.number="depositTerm"
                />
              </div>
            </div>
          </div>
          <div>
            <div class="bank-deposits-modal-interaction__calculation__display">
              <div class="bank-deposits-modal-interaction__calculation__display-desc">{{loc('bank:menu:136')}}</div>
              <div class="bank-deposits-modal-interaction__calculation__display-value">{{summByEndTerm.toLocaleString('ru')}} $</div>
              <div class="bank-deposits-modal-interaction__calculation__display-prompt">{{loc(`bank:menu:137@${dataModal.AnnualRate}`)}}</div>
              <div 
                class="bank-deposits-modal-interaction__calculation__display-btn btn-main"
                @click="openDeposit"
              >{{loc('bank:menu:138')}}</div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import RangeSlider from 'vue-range-slider'
import 'vue-range-slider/dist/vue-range-slider.css'
import { mapState, mapGetters } from 'vuex'
export default {
  name: "BankDepositsModal",
  props: {
    dataModal: Object
  },
  components: {
    RangeSlider,
  },
  data() {
    return {
      depositSumm: 0,
      depositTerm: 0,
    }
  },
  computed: {
    ...mapState('smartphone/bankPage', ['balance']),
    ...mapGetters('localization',['loc']),
    // Считает общую сумму к концу срока, сделал пример подсчета
    summByEndTerm() {
      return Math.round(this.depositSumm * ( 1 + (this.depositTerm/360) * this.dataModal.AnnualRate/100))
    },
    getMaxAmount(){
      return this.dataModal.MaxMoney < this.balance ? this.dataModal.MaxMoney : this.balance;
    }
  },
  methods: {
    openDeposit() {
      if (this.balance > this.dataModal.MinMoney) {
        window.mp.triggerServer('bank:openDeposit', this.dataModal.Id, this.depositSumm, this.depositTerm)
      }
    }
  },
  mounted() {
    this.depositSumm = this.dataModal.MinMoney
    this.depositTerm = this.dataModal.MinDays
  }
}
</script>

<style lang="scss">
.bank-deposits-modal{
  width: 100vw;
  height: 100vh;
  position: fixed;
  left: 0;
  top: 0;
  z-index: 1;
  background: radial-gradient(43.85% 77.96% at 50% 50%, #4E6ED8 0%, rgba(78, 110, 216, 0) 100%), rgba(0, 0, 0, 0.7);
  display: flex;
  align-items: center;
  justify-content: center;
  &__wrap{
    width: 43rem;
    height: 43.5rem;
    background: #030C20;
    border-radius: 1rem;
    display: flex;
    flex-direction: column;
    position: relative;
  }
  &-info{
    display: flex;
    flex-direction: column;
    width: 100%;
    padding: 2.5rem 0 0 2rem;
    position: relative;
    height: 16.5rem;
    background-repeat: no-repeat;
    background-size: contain;
    background-position: right;
    margin-bottom: 3.8rem;
    &:after{
      content: '';
      width: calc(100% - (2*(2rem)));
      height: 1px;
      background-color: rgba(255, 255, 255, 0.2);
      position: absolute;
      left: 2rem;
      bottom: -1.5rem;
    }
  }
  &__provisions{
    display: flex;
    align-items: center;
  }
  &-interaction{
    display: flex;
    flex-direction: column;
    padding: 0 2rem;
    .bank-deposits-modal-desc{
      max-width: 25rem;
      margin-bottom: 1.5rem;
    }
    &__calculation{
      display: flex;
      align-items: flex-start;
      justify-content: space-between;
      &>div{
        display: flex;
        flex-direction: column;
        &:nth-child(1) {
          height: 100%;
          justify-content: space-between;
        }
      }
      &__range{
        display: flex;
        flex-direction: column;
        position: relative;
        font-size: 1.5rem;
        letter-spacing: 0.03em;
        margin-bottom: 3.5rem;
        &:last-child{
          margin-bottom: 0;
        }
        &-title{
          font-weight: normal;
          font-size: 1rem;
          line-height: 1.2rem;
          letter-spacing: 0.03em;
          color: rgba(255, 255, 255, 0.4);
          margin-bottom: .2rem;
        }
        &-wrap{
          background: rgba(255, 255, 255, 0.1);
          border-radius: .4rem;
          padding: 0 1.5rem;
          display: flex;
          align-items: center;
          justify-content: space-between;
          width: 100%;
          height: 4rem;
          width: 22rem;
        }
        &-desc{
          color: rgba(255, 255, 255, 0.5);
          font-weight: normal;
        }
        &-value{
          font-weight: bold;
        }
      }
      &__display{
        width: 15rem;
        height: 14rem;
        background: rgba(255, 255, 255, 0.1);
        border-radius: .4rem;
        padding: 1.5rem 2rem;
        letter-spacing: 0.03em;
        &-desc, &-prompt{
          font-size: 1.1rem;
          line-height: 1.2rem;
          color: rgba(255, 255, 255, 0.4);
        }
        &-desc{
          margin-bottom: .2rem;
        }
        &-value{
          font-weight: normal;
          font-size: 2.3rem;
          line-height: 2.75rem;
          margin-bottom: .7rem;
        }
        &-prompt{
          font-weight: normal;
          margin-bottom: 1.4rem;
          span{
            color: #fff;
            font-weight: bold;
          }
        }
        &-btn{
          width: 100%;
          height: 3rem;
        }
      }
    }
  }
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
    margin-bottom: 2.15rem;
  }
  &__info{
    display: flex;
    align-items: center;
    margin-bottom: 1.25rem;
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
