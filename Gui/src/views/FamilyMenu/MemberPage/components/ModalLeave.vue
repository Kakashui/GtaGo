<template>
  <div class="modal-wrap">
    <div class="modal-leave">
      <div 
        class="btn btn-close"
        @click="setModalLeave"
      ></div>
      <div class="title">{{loc('familyMenu_73')}}</div>
      <div class="btns-wrap">
        <div 
          class="btn"
          @click="kickMember"
        >{{loc('familyMenu_74')}}</div>
        <div 
          class="btn btn-cancel"
          @click="setModalLeave"
        >{{loc('familyMenu_75')}}</div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
export default {
  name: "ModalLeave",

  props: {
    currentMember: Object
  },

  methods: {
    setModalLeave: function() {
      this.$emit('setModalLeave')
    },
    kickMember: function() {
      window.mp.trigger('familyMenu:kickMember', this.currentMember.id)
      this.setModalLeave()
    }
  },

  computed: {
    ...mapGetters('localization', ['loc'])
  }
}
</script>

<style lang="scss" scoped>
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
  padding: 2.4rem;
  position: relative;
  .title{
    font-weight: bold;
    font-size: 1.5rem;
    line-height: 1.5rem;
    margin-bottom: 1.2rem;
    max-width: 12.35rem;
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
