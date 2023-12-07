<template>
  <div class="modal">
    <div class="modal-wrap">
      <div class="modal-wrap__title">{{loc('BattlegroundReg_7')}}</div>
      <div class="modal-wrap__desc">{{loc('BattlegroundReg_8')}}</div>
      <div 
        class="modal-wrap__btn"
        @click="closeModal"
      >{{loc('BattlegroundReg_9')}}</div>
    </div>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
export default {
  name: 'Modal',

  computed: {
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    closeModal: function() {
      window.mp.trigger('battlegroundReg:closeModal')
    }
  }
}
</script>

<style lang="scss" scoped>
.modal{
  @keyframes showModal{
    from {
      opacity: 0
    }
    to {
      opacity: 1;
    }
  }
  position: fixed;
  left: 0;
  top: 0;
  width: 100%;
  height: 100vh;
  background: rgba(0, 0, 0, 0.6);
  display: flex;
  align-items: center;
  justify-content: center;
  animation: showModal .5s;
  .modal-wrap{
    border-radius: .5rem;
    background-image: url('/img/battlegroundReg/bg-modal.png');
    background-size: contain;
    background-repeat: no-repeat;
    background-position: center;
    width: 27.9rem;
    display: flex;
    flex-flow: column;
    padding: 2.5rem 0;
    align-items: center;
    color: #FFFFFF;
    text-transform: uppercase;
    &__title{
      font-size: 2rem;
      line-height: 2.3rem;
      text-align: center;
      letter-spacing: 0.04em;
      margin-bottom: .7rem;
    }
    &__desc{
      font-weight: 600;
      font-size: 1rem;
      line-height: 1.15rem;
      margin-bottom: 2.7rem;
      max-width: 13.55rem;
    }
    &__btn{
      background-color: #2B963A;
      border-radius: .1rem;
      width: 10.4rem;
      height: 3rem;
      display: flex;
      align-items: center;
      justify-content: center;
      font-size: 1rem;
      transition: .3s;
      &:hover{
        background-color: #267831;
        transition: .3s;
      }
    }
  }
}
</style>
