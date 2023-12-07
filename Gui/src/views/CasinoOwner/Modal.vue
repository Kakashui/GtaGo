<template>
  <div class="modal-block">
    <div class="modal-block__title">{{loc('cas:owner:modal:1')}}</div>
    <input 
      type="number"
      class="modal-block__input"
      :placeholder="loc('cas:owner:modal:2')"
      v-model="cash"
    >
    <div class="modal-block__btns">
      <div 
        class="btn"
        @click="applyInput"
      ><span>{{loc(getText)}}</span></div>
      <div 
        class="btn"
        @click="$emit('closeModal')"
      ><span>{{loc('cas:owner:modal:3')}}</span></div>
    </div>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
export default {
  name: 'Modal',

  props: {
    type: Number
  },

  data: function() {
    return{
      cash: null
    }
  },

  methods: {
    applyInput: function() {
      if (this.cash) {
        if (this.type) {
          window.mp.triggerServer('casinoOwner:toDeposit', this.cash)
        } else {
          window.mp.triggerServer('casinoOwner:toCredit', this.cash)
        }
        this.$emit('closeModal')
      }
    }
  },

  computed: {
    getText: function() {
      if (this.type) {
        return 'cas:owner:modal:4'
      } else {
        return 'cas:owner:modal:5'
      } 
    },
    ...mapGetters('localization', ['loc'])
  }
}
</script>

<style lang="scss" scoped>
.casino-owner{
  &__wrap{
    .modal-block{
      width: 26.5rem;
      padding: 3.25rem 3.75rem 3.5rem 3.75rem;
      background-size: cover;
      background-position: top;
      background-repeat: no-repeat;
      background-image: url('/img/casinoOwner/Modal.png');
      box-shadow: 0 0 4.5rem rgba(22, 22, 22, 0.5);
      position: absolute;
      text-transform: uppercase;
      top: 0;
      z-index: 3;
      &__title{
        font-weight: normal;
        font-size: 1.5rem;
        line-height: 1.8rem;
        text-align: center;
        letter-spacing: 0.1em;
        text-transform: uppercase;
        color: #FFFFFF;
        white-space: pre;
        margin-bottom: 1rem;
      }
      &__input{
        outline: none;
        background: rgba(255, 255, 255, 0.1);
        border: 1px solid rgba(255, 255, 255, 0.1);
        box-sizing: border-box;
        box-shadow: 0 .5rem 1.5rem rgba(0, 0, 0, 0.2);
        padding: 0 1.5rem;
        height: 3.5rem;
        width: 100%;
        margin-bottom: 1rem;
        font-weight: bold;
        font-size: .8rem;
        letter-spacing: 0.1em;
        text-transform: uppercase;
        color: #fff;
        text-transform: uppercase;
        &::placeholder{
          color: rgba(255, 255, 255, 0.3);
        }
        &::-webkit-outer-spin-button,
        &::-webkit-inner-spin-button {
          -webkit-appearance: none;
        }
      }
      &__btns{
        width: 100%;
        display: flex;
        align-items: center;
        .btn{
          display: flex;
          align-items: center;
          justify-content: center;
          height: 3.5rem;
          width: 100%;
          margin-right: 1rem;
          background: rgba(255, 255, 255, 0.1);
          font-weight: bold;
          font-size: .8rem;
          letter-spacing: 0.1em;
          text-transform: uppercase;
          color: rgba(255, 255, 255, 0.4);
          background: rgba(255, 255, 255, 0.1);
          position: relative;
          transition: .3s;
          &>span{
            z-index: 3;
          }
          &:after{
            content: '';
            width: 100%;
            height: 100%;
            position: absolute;
            top: 0;
            left: 0;
            background: linear-gradient(90deg, #E6B980 0%, #EACDA3 100%);
            opacity: 0;
            z-index: 2;
            transition: .3s;
          }
          &:hover{
            box-shadow: 0 .5rem 3.5rem rgba(234, 205, 163, 0.5);
            color: #fff;
            &:after{
              opacity: 1;
              transition: .3s;
            }
          }
          &:last-child{
            margin-right: 0;
          }
        }
      }
    }
  }
}
</style>
