<template>
  <div class="modal-wrap">
    <div class="modal-delete">
      <div class="title">{{loc('familyMenu_94')}}</div>
      <div class="btns-wrap">
        <div 
          class="btn"
          @click="deleteOrganization"
        >{{loc('familyMenu_74')}}</div>
        <div 
          class="btn btn-cancel"
          @click="closeModal"
        >{{loc('familyMenu_75')}}</div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
export default {
  name: 'ModalDelete',

  methods: {
    closeModal: function() {
      this.$emit('closeModal')
    },
    deleteOrganization: function() {
      window.mp.trigger('familyMenu:deleteOrganization')
      this.$emit('closeModal')
    }
  },

  computed: {
    ...mapGetters('localization', ['loc'])
  }
}
</script>

<style lang="scss" scoped>
.modal-delete{
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
}
</style>
