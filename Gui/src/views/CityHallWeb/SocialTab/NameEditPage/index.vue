<template>
  <div class="page">
    <div class="page__title">{{loc('cityHallWeb_122')}}</div>
    <div class="current-name">{{loc('cityHallWeb_123')}}: <span>{{name}}</span></div>
    <div class="page__desc">{{loc('cityHallWeb_124')}}</div>
    <div :class="[{error: !nameIsFree}, 'input-wrap']"
    >
      <input
        :placeholder="loc('cityHallWeb_137')"
        v-model="currentName"
      />
      <div 
        class="input-wrap__error"
        v-if="!nameIsFree"
      >{{textError}}</div>
    </div>
    <Payment
      :currentPayment="currentPayment"
      :setCurrentPayment="setCurrentPayment"
    />
    <div 
      v-if="currentName && nameIsFree"
      class="page__btn btn"
      @click="sendForm"
    >
      <span>{{loc('cityHallWeb_125')}}</span>
      <span>${{currentCost}}</span>
    </div>
  </div>
</template>

<script>
import Payment from '../../components/Payment'
import { mapGetters, mapState } from 'vuex'
export default {
  name: 'NameEditPage',

  components: {
    Payment
  },

  data: function() {
    return{
      currentName: '',
      currentPayment: 0, // 0 - money , 1 - card
    }
  },

  methods: {
    sendForm: function() {
      window.mp.trigger('cityHallWeb:sendFormNameEditPage', this.currentName, this.currentPayment)
      if (this.nameIsFree) {
        this.currentName = null
      }
      this.currentPayment = 0
    },

    setCurrentPayment: function(value) {
      this.currentPayment = value
    },
  },

  computed: {
    ...mapState('cityHallWeb/nameEdit', ['nameIsFree', 'textError', 'currentCost']),
    ...mapState('cityHallWeb', ['cardId', 'name']),
    ...mapGetters('localization', ['loc'])
  },
}
</script>

<style lang="scss" scoped>
.city-hall-web{
  .body-tab{
    .page{
      &__btn{
        span{
          &:nth-child(1){
            margin-right: .6rem;
            padding-right: .65rem;
            position: relative;
            display: flex;
            align-items: center;
            &:after{
              content: '';
              width: 1px;
              height: 1.5rem;
              background-color:  rgba(255, 255, 255, 0.3);
              position: absolute;
              right: 0;
            }
          }
        }
      }
    }
    &__title{
      margin-bottom: 1.2rem;
    }
    .current-name{
      background: #F3F5F7;
      border-radius: .3rem;
      min-height: 3.9rem;
      padding: 0 1rem;
      display: flex;
      align-items: center;
      margin-bottom: 1.2rem;
      font-family: Roboto;
      font-style: normal;
      font-weight: bold;
      font-size: .8rem;
      text-transform: uppercase;
      color: #46555F;
      span{
        font-style: normal;
        font-weight: normal;
        text-transform: initial;
        margin-left: .45rem;
      }
    }
    .input-wrap{
      position: relative;
      border: 1px solid rgba(70, 85, 95, 0.2);
      border-radius: .3rem;
      margin-bottom: 1.7rem;
      display: flex;
      align-items: center;
      input{
        border: none;
        margin: 0;
        width: 100%;
        z-index: 1;
      }
      &.error{
        border-radius: .3rem .3rem 0 0;
        border-bottom: .1rem solid #FF3838;
        input{
          padding: 0 2.6rem 0 1rem;
        }
        .input-wrap__error, &::after{
          opacity: 1;
        }
      }
      &_error, &:after{
        opacity: 0;
      }
      &__error{
        position: absolute;
        left: 0;
        bottom: -.3rem;
        transform: translateY(100%);
        font-family: Roboto;
        font-style: normal;
        font-weight: normal;
        font-size: .8rem;
        line-height: .9rem;
        color: #E00B29;
      }
      &:after{
        content: '';
        width: 1.25rem;
        height: 1.25rem;
        position: absolute;
        right: 1.1rem;
        background-size: contain;
        background-repeat: no-repeat;
        background-position: center;
        background-image: url('/img/cityHallWeb/icon-error.svg');
        z-index: 0;
      }
    }
  }
}
</style>
