<template>
  <div class="modal-wrap">
    <div class="modal">
      <div 
        class="btn btn-close"
        @click="closeModal"
      ></div>
      <div class="title">{{loc('familyMenu_42')}}</div>
      <input 
        type="text"
        v-model="nation"
      >
      <div class="btns-wrap">
        <div 
          class="btn"
          @click="saveNation"
        >{{loc('familyMenu_35')}}</div>
        <div 
          class="btn btn-cancel"
          @click="closeModal"
        >{{loc('familyMenu_36')}}</div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
export default {
  name: 'ModalNation',

  computed: {
    ...mapState('familyMenu', ['infoPage']),
    ...mapGetters('localization', ['loc'])
  },

  data: function() {
    return {
      nation: null,
    }
  },

  methods: {
    closeModal: function() {
      this.$emit('closeModalNation')
    },
    saveNation: function() {
      window.mp.trigger('familyMenu:saveNation', this.nation)
      this.closeModal()
    }
  },
  
  created () {
    this.nation = this.infoPage.nation
  }
}
</script>

<style lang="scss" scoped>
.modal{
  background-size: cover;
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
  width: 20.7rem;
  .title{
    font-weight: bold;
    font-size: 1.5rem;
    line-height: 1.5rem;
    margin-bottom: .5rem;
    margin-bottom: 1.5rem;
  }
  input{
    margin-bottom: 2rem;
    background: rgba(255, 255, 255, 0.1);
    border-radius: .2rem .2rem 0 0;
    border-bottom: .2rem solid #DCFF00;
    height: 2.6rem;
    min-height: 2.6rem;
    text-align: center;
    width: 8.6rem;
    outline: none;
   text-transform: uppercase;
    font-style: normal;
    font-weight: bold;
    font-size: 1rem;
    color: #fff;
  }
  .btns-wrap{
    display: flex;
    align-items: center;
    width: 100%;
    position: relative;
    padding-top: 1rem;
    &:before{
      content: '';
      width: 100%;
      height: 1px;
      position: absolute;
      top: 0;
      left: 0;
      background: rgba(255, 255, 255, 0.1);
    }
    .btn{
      padding: 1rem 1.2rem;
      font-weight: bold;
      align-items: flex-end;
      font-size: 1rem;
      border-radius: .1rem;
      background: #C1E704;
      margin-right: 1rem;
      &:hover{
        box-shadow: 0 0 1rem #C1E704;
      }
      &:last-child{
        margin-right: 0;
      }
      &:before{
        content: '';
        width: 1rem;
        height: 1rem;
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
  .btn-close{
    background-size: contain;
    background-position: center;
    background-repeat: no-repeat;
    background-color: transparent;
    width: 2rem;
    height: 2rem;
    background-image: url('/img/familyMenu/btn-close.svg');
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
