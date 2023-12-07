<template>
  <div class="modal-wrap">
    <div class="modal-leave">
      <div 
        class="btn btn-close"
        @click="closeModal"
      ></div>
      <div class="title">{{loc('familyMenu_8')}}</div>
      <div class="desc">{{loc('familyMenu_9')}}</div>
      <div class="btns-wrap">
        <div 
          class="btn"
          @click="leaveFromOrganization"
        >{{loc('familyMenu_10')}}</div>
        <div 
          class="btn btn-cancel"
          @click="closeModal"
        >{{loc('familyMenu_11')}}</div>
      </div>
      <!-- <template v-else>
        <div class="title">{{loc('familyMenu_12')}}</div>
        <div class="desc">{{loc('familyMenu_13')}}</div>
        <div 
          class="btn"
        >{{loc('familyMenu_14')}}</div>
      </template> -->
    </div>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
export default {
  name: 'ModalLeave',
  
  props: {
    premium: Boolean
  },

  computed: {
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    closeModal: function() {
      this.$emit('closeModalLeave')
    },
    goToPremium: function() {
      window.mp.trigger('familyMenu:goToPremium')
      this.closeModal()
    },
    leaveFromOrganization: function() {
      window.mp.trigger('familyMenu:leaveFromOrganization')
      this.closeModal()
    }
  }
}
</script>

<style lang="scss" scoped>
.modal-wrap{
  position: absolute;
  z-index: 2;
  top: 0;
  left: 0;
  height: 100vh;
  width: 100%;
  background: rgba(0, 0, 0, 0.6);
  display: flex;
  align-items: center;
  justify-content: center;
  animation: .3s modalShow linear;
}
.modal-leave{
  background-size: contain;
  background-position: center;
  background-repeat: no-repeat;
  background-image: url('/img/familyMenu/modal-leave.png');
  border-radius: .5rem;
  display: flex;
  flex-flow: column;
  align-items: center;
  text-align: center;
  padding: 2.5rem;
  position: relative;
  .title{
    font-weight: bold;
    font-size: 1.5rem;
    line-height: 1.5rem;
    margin-bottom: .5rem;
  }
  .desc{
    font-size: 1.1rem;
    line-height: 1.3rem;
    color: rgba(255, 255, 255, 0.5);
    margin-bottom: 1.9rem;
    max-width: 18.7rem;
  }
  .btns-wrap{
    display: flex;
    align-items: center;
  }
  .btn{
    padding: 1rem 1.6rem;
    font-weight: bold;
    align-items: flex-end;
    font-size: 1.2rem;
    line-height: 1.2rem;
    border-radius: .1rem;
    background: #C1E704;
    margin-right: 1.2rem;
    &:hover{
      box-shadow: 0 0 1rem #C1E704;
    }
    &:last-child{
      margin-right: 0;
    }
    &:before{
      content: '';
      width: 1.5rem;
      height: 1.5rem;
      margin-right: .5rem;
      background-size: contain;
      background-position: center;
      background-repeat: no-repeat;
      background-image: url('/img/familyMenu/btn.svg');
    }
    &.btn-cancel{
      background: rgba(255, 255, 255, 0.2);
      &:before{
        background-image: url('/img/familyMenu/btn-cancel.svg');
      }
      &:hover{
        box-shadow: 0 0 1rem rgba(255, 255, 255, 0.4);
      }
    }
  }
  .btn-close{
    background-size: contain;
    background-position: center;
    background-repeat: no-repeat;
    background-color: transparent;
    width: 2rem;
    height: 2rem;
    background-image: url('/img/familyMenu/btn-close.svg');
    border-radius: none;
    margin: 0;
    position: absolute;
    right: 0;
    top: 0;
    transform: translateX(100%);
    &:hover{
      box-shadow: none;
      transform: translateX(100%) scale(1.1);
    }
  }
}
</style>
