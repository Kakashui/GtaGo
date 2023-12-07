<template>
  <div class="battleground-reg">
    <div class="battleground-reg-body">
      <div class="battleground-reg__info">
        <div class="info-wrap info-date">
          <div class="info-wrap__title">{{loc('BattlegroundReg_0')}}</div>
          <div class="info-wrap__value">{{loc('btlgrnd:end')}}</div>
        </div>  
        <div class="info-wrap info-people">
          <div class="info-wrap__title">{{loc('BattlegroundReg_1')}}</div>
          <div class="info-wrap__value">{{peopleCount}}/{{peopleCapacity}}</div>
        </div>
      </div>
      <div class="battleground-reg__title">{{loc('BattlegroundReg_2')}}</div>
      <div class="battleground-reg__desc">{{loc('BattlegroundReg_3')}}</div>
      <div class="battleground-reg__prompt">{{loc('BattlegroundReg_4')}}</div>
      <div class="battleground-reg__interaction">
        <div
          v-if="isReg"
          class="btn btn-reg locked"
        >{{loc('BattlegroundReg_10')}}</div>
        <div
          v-else
          class="btn btn-reg"
          @click="setShowModal"
        >{{loc('BattlegroundReg_5')}}</div>
      </div>
      <div class="battleground-reg__decorate">{{loc('BattlegroundReg_6')}}</div>
      <Modal v-if="showModal"/>
    </div>    
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
import Modal from './Modal'
export default {
  name: 'BattlegroundReg',

  components: {
    Modal
  },

  data: function() {
    return {
      showModal: null
    }
  },

  computed: {
    ...mapState('battlegroundReg', ['isReg', 'peopleCount', 'peopleCapacity']),
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    setShowModal: function() {
      window.mp.trigger('battlegroundReg:registerForBattle')
      this.showModal = true
    }
  },

  mounted(){
    this.showModal = false
  }
}
</script>

<style lang="scss" scoped>
@keyframes show{
  from {
    transform: translateX(100vw);
  }
  to { 
    transform: translateX(0);
  }
}
.battleground-reg{
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100vh;
  padding: 4rem;
  text-transform: uppercase;
  color: #fff;
  background-size: cover;
  background-position: center;
  background-image: url('/img/battlegroundReg/bg.png');
  &-body{
    max-width: 1920px;
    height: 100%;
    margin: auto;
    position: relative;
    display: flex;
    flex-flow: column;
    align-items: flex-start;
  }
  .btn{
    display: flex;
    align-items: center;
    justify-content: center;
    position: relative;
    transition: .3s;
    &:hover{
      transition: .3s;
    }
  }
  &__info{
    display: flex;
    margin-bottom: 4rem;
    .info-wrap{
      display: flex;
      flex-flow: column;
      align-items: flex-start;
      margin-right: 3.5rem;
      &:before{
        content: '';
        width: 1.2rem;
        height: 1.2rem;
        background-size: contain;
        background-position: center;
        background-repeat: no-repeat;
        margin-bottom: .4rem;
      }
      &__title{
        font-weight: 600;
        font-size: 1rem;
        line-height: 1.15rem;
        color: rgba(255, 255, 255, 0.6);
        max-width: 7.3rem;
        margin-bottom: .2rem;
      }
      &__value{
        font-weight: 600;
        font-size: 1rem;
        line-height: 1.15rem;
        color: #FFFFFF;
      }
      &:last-child{
        margin-right: 0;
      }
      &.info-date{
        &:before{
          background-image: url('/img/battlegroundReg/date.svg');
        }
      }
      &.info-people{
        &:before{
          background-image: url('/img/battlegroundReg/people.svg');
        }
      }
    }
  }
  &__title, &__desc{
    line-height: 100%;
    font-style: normal;
    font-weight: bold;
  }
  &__title{
    margin-bottom: -.8rem;
    font-size: 10rem;
    margin-left: -.4rem;
  }
  &__desc{
    font-size: 4.5rem;
    margin-bottom: 1.5rem;
  }
  &__prompt{
    font-weight: 400;
    font-size: .8rem;
    line-height: .9rem;
    max-width: 18.5rem;
  }
  &__interaction{
    position: absolute;
    top: 36.05rem;
    left: 0;
    width: 100%;
    height: 3.85rem;
    border-top: 1px solid rgba(255, 255, 255, 0.2);
    border-bottom: 1px solid rgba(255, 255, 255, 0.2);
    display: flex;
    align-items: center;
    justify-content: flex-start;
    .btn-reg{
      height: 100%;
      margin-left: 3.85rem;
      font-weight: 600;
      font-size: .8rem;
      line-height: .9rem;
      letter-spacing: 0.06em;
      padding: 0 2.65rem;
      background-color: #2B963A;
      animation: show .7s;
      transition: .3s;
      &.locked{
        animation: none;
        background-color: rgba(196, 196, 196, 0.42);
        &:hover{
          background-color: rgba(196, 196, 196, 0.42);
        }
      }
      &:hover{
        background-color: #267831;
        transition: .3s;
      }
    }
  }
  &__decorate{
    width: 7.9rem;
    height: 7.9rem;
    border-radius: 50%;
    padding: 0 1.1rem;
    background: #176A23;
    font-style: italic;
    font-weight: bold;
    font-size: 1.2rem;
    line-height: 1.35rem;
    display: flex;
    align-items: center;
    justify-content: center;
    position: absolute;
    top: 2.5rem;
    right: 2.5rem;
    text-align: center;
    transform-origin: 50% 50%;
    transform: rotate(-10deg)
  }
}
</style>
