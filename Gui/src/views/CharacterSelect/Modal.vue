<template>
  <div class="modal-wrap">
    <div class="modal">
      <div class="prompt">{{loc('characterSelect_20')}}</div>
      <div 
        v-if="modal.type === 0"
        class="desc"
      >{{loc('characterSelect_21')}}</div>
      <div
        v-else-if="modal.type === 1"
        class="desc"
      >{{loc('characterSelect_22')}}</div>
      <div
        v-else-if="modal.type === 2"
        class="desc"
      >{{loc('characterSelect_25')}}</div>
      <div class="interaction">
        <div 
          class="btn btn-apply"
          @click="applyAction"
        >{{loc('characterSelect_23')}}</div>
        <div 
          class="btn btn-cancel"
          @click="setModal(null)"
        >{{loc('characterSelect_24')}}</div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
export default {
  name: 'Modal',

  props: {
    modal: Object,
    setModal: Function
  },

  computed: {
    ...mapGetters('localization', ['loc']),
  },

  methods: {
    applyAction: function() {
      switch (this.modal.type) {
        case 0:
          window.mp.triggerServer('auth:char:select', this.modal.index)
          break;
        case 1:
          window.mp.triggerServer('auth:char:delete', this.modal.index)          
          break;
      
        default:
          break;
      }
      this.setModal(false);
    }
  },
}
</script>

<style lang="scss" scoped>
.modal-wrap{
  width: 100%;
  height: 100vh;
  position: absolute;
  left: 0;
  top: 0;
  background: linear-gradient(0deg, rgba(2, 3, 16, 0.9), rgba(2, 3, 16, 0.9));
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 10;
  .modal{
    display: flex;
    flex-flow: column;
    background-image: url('/img/characterSelect/modal-bg.png');
    background-color: rgba(196, 196, 196, .2);
    background-size: cover;
    border-radius: .3rem;
    background-repeat: no-repeat;
    background-position: center;
    width: 25.5rem;
    padding: 3rem 3.25rem;
    .prompt{
      font-weight: bold;
      font-size: .8rem;
      line-height: .8rem;
      text-align: center;
      letter-spacing: 0.1em;
      text-transform: uppercase;
      color: rgba(255, 255, 255, 0.5);
      margin-bottom: .5rem;
    }
    .desc{
      font-weight: bold;
      font-size: 1.5rem;
      line-height: 1.6rem;
      text-align: center;
      letter-spacing: 0.1em;
      text-transform: uppercase;
      color: #FFFFFF;
      margin-bottom: 1.25rem;
    }
    .interaction{
      display: flex;
      align-items: center;
      width: 100%;
      justify-content: space-between;
      .btn-apply, .btn-cancel{
        width: 47%;
        height: 3.5rem;
        min-height: 3.5rem;
        border-radius: .2rem;
        font-weight: bold;
        font-size: .8rem;
        letter-spacing: 0.1em;
        text-transform: uppercase;
        color: #FFFFFF;
        &:before{
          content: '';
          width: 1.3rem;
          height: 1.3rem;
          margin-right: .75rem;
          background-size: contain;
          background-repeat: no-repeat;
          background-position: center;
        }
      }
      .btn-apply{ 
        background: #B6D300;
        &:before{
          background-image: url('/img/characterSelect/apply.svg');
        }
        &:hover{
          box-shadow: 0 .5rem 2.5rem rgba(168, 195, 2, 0.6);
        }
      }
      .btn-cancel{
        background: rgba(255, 255, 255, 0.1);
        &:before{
          background-image: url('/img/characterSelect/cancel.svg');
        }
        &:hover{
          box-shadow: 0 .5rem 2.5rem rgba(255, 255, 255, 0.2);
        }
      }
    }
  }
}
</style>
