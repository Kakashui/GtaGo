<template>
  <div class="bank-transfer bank-modal">
    <div class="bank-transfer__wrap bank-modal__wrap">
      <div class="btn-close" @click="$emit('closeModal')"></div>

      <template v-if="currentPage === 0">

        <div class="bank-modal-title">{{loc('bank:menu:15')}}</div>
        <div class="bank-modal-desc">{{loc('bank:menu:16')}}</div>
        <div class="bank-transfer__info bank-modal__block-shadow cash">
          <div class="bank-modal-prompt">{{loc('bank:menu:17')}}</div>
          <div class="bank-transfer__balance">
            $ {{ dataModal.balance.toLocaleString("ru") }}
          </div>
        </div>
        <div class="bank-modal-prompt card">{{loc('bank:menu:18')}}</div>
        <div class="bank-transfer__input bank-modal__input">
          <input type="number" v-model="targetNumber" />
        </div>
        <div class="bank-modal-prompt">{{loc('bank:menu:19')}}</div>
        <div class="bank-transfer__input bank-modal__input cash">
          <input type="number" v-model.number="transferAmount" />
        </div>

          <div class="bank-transfer__input bank-modal__input">
            <input
              type="text"
              v-model="transferComment"
              :placeholder="loc('bank:menu:20')"
            />
          </div>

        <template v-if="transfersPerDay + transferAmount > dailyTransferLimit">
          <div class="bank-transfer__error">{{loc(`bank:menu:21@${dailyTransferLimit.toLocaleString('ru')}`)}}</div>
          <div class="bank-modal-btn btn-main" @click="setReason">{{loc('bank:menu:22')}}</div>
        </template>

        <template v-else>          
          <div 
            class="bank-modal-btn btn-main" 
            @click="transfer"
          >{{loc('bank:menu:23')}}</div>
        </template>
        
      </template>

      <template v-if="currentPage === 1">
        <BtnBack 
          class="bank-transfer__back"
          @click="setPage(0)"
        />
        <div class="bank-modal-title">{{loc('bank:menu:24')}}</div>
        <div class="bank-modal-desc">{{loc(`bank:menu:25@${dailyTransferLimit.toLocaleString('ru')}`)}}</div>
        <div class="bank-transfer__fixed-transfer">
          <div class="bank-modal-desc">{{loc('bank:menu:26')}}</div>
          <div class="bank-transfer__fixed-transfer-value">$ {{transferAmount.toLocaleString('ru')}}</div>
        </div>
        <textarea 
          class="bank-transfer__desc"
          :placeholder="loc('bank:menu:27')"
          v-model="currentReason"
        ></textarea>
        <div 
          class="bank-modal-btn btn-main" 
          @click="sendRequest"
        >{{loc('bank:menu:28')}}</div>
      </template>

      <template v-if="currentPage === 2">
        <div class="bank-modal-title">{{loc('bank:menu:29')}}</div>
        <div class="bank-modal-desc">{{loc('bank:menu:30')}}</div>
        <div 
          class="bank-modal-btn btn-main" 
          @click="$emit('closeModal')"
        >{{loc('bank:menu:31')}}</div>
      </template> 
      
    </div>
  </div>
</template>

<script>
import { mapState, mapGetters } from 'vuex'
import BtnBack from './components/BtnBack'
export default {
  name: 'BankModalTransfer',
  components: {
    BtnBack
  },
  props: {
    dataModal: Object,
  },
  data() {
    return {
      currentPage: 0,
      targetNumber: null,
      transferAmount: null,
      transferComment: '',
      currentReason: '',
    };
  },
  computed: {
    ...mapState('smartphone/bankPage', ['balance']),
    ...mapState('bank', [
      'transfersPerDay', 
      'dailyTransferLimit'
    ]),
    ...mapGetters('localization',['loc'])
  },
  methods: {
    transfer() {
      if (this.targetNumber && this.transferAmount && this.transferAmount > 0) {
        window.mp.triggerServer('bank:transfer', this.targetNumber, this.transferAmount, this.transferComment, null);
        this.$emit('closeModal');
      }
    },
    setReason() {
      if (this.targetNumber && this.transferAmount > 0) {
        this.setPage(1)
      }
    },
    setPage(page) {
      this.currentPage = page
    },
    sendRequest() {
      if (this.currentReason && this.targetNumber) {
        window.mp.triggerServer('bank:transfer', this.targetNumber, this.transferAmount, this.transferComment, this.currentReason)
        this.setPage(2)
      }
    }
  },
};
</script>

<style lang="scss">
.bank-transfer {
  .bank-modal-title{
    margin-bottom: .5rem;
  }
  &__back{
    margin-bottom: .5rem;
  }
  &__wrap {
  }
  &__info {
    margin-bottom: 2rem;
  }
  &__balance {
    font-weight: normal;
    font-size: 1.6rem;
    line-height: 1.9rem;
    letter-spacing: 0.03em;
    color: #000000;
  }
  &__input {
    &.cash {
      &:before {
        content: "$";
        pointer-events: none;
        position: absolute;
        left: 0;
      }
      input {
        padding-left: 1.5rem;
      }
    }
  }
  &__error{
    font-weight: normal;
    font-size: 1rem;
    line-height: 1.1rem;
    letter-spacing: 0.03em;
    color: #EE443A;
    margin-bottom: 1.5rem;
  }
  &__fixed-transfer{
    display: flex;
    flex-direction: column;
    align-items: center;
    padding: 1.2rem 0;
    border-top: 1px solid rgba(0, 0, 0, 0.1);
    border-bottom: 1px solid rgba(0, 0, 0, 0.1);
    margin-bottom: 1rem;
    .bank-modal-desc{
      margin-bottom: .3rem;
    }
    &-value{
      font-weight: normal;
      font-size: 2.5rem;
      line-height: 3rem;
      letter-spacing: 0.03em;
      color: #000000;
    }
  }
  &__desc{
    padding: 1rem;
    background: #FFFFFF;
    box-shadow: 0 .5rem 3rem rgba(49, 41, 184, 0.2);
    border-radius: .5rem;
    height: 8rem;
    resize: none;
    font-size: 1rem;
    line-height: 1.2rem;
    letter-spacing: 0.03em;
    color: #5E37B0;
    border: none;
    outline: none;
    margin-bottom: 1.5rem;
    &::placeholder{
      color: #5E37B0;
    }
  }
}
</style>
