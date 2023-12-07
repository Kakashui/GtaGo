<template>
  <div class="modal-wrap">
    <div class="modal modal-bio">
      <div 
        class="btn btn-close"
        @click="closeModal"
      ></div>
      <div class="col">
        <div class="title">{{loc('familyMenu_33')}}</div>
        <div class="desc">{{loc('familyMenu_34')}}.</div>
        <div 
          class="btn btn-inter"
          @click="setBio"
        >{{loc('familyMenu_35')}}</div>
        <div 
          class="btn btn-inter cancel"
          @click="cancelEdit"
        >{{loc('familyMenu_36')}}</div>
      </div>
      <div class="col">
        <textarea 
          class="text-info" 
          :placeholder="loc('familyMenu_37')"
          v-model="currentTextBio"
        ></textarea>
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
export default {
  name: 'ModalBio',

  data: function() {
    return {
      currentTextBio: null,
    }
  },

  computed: {
    ...mapState('familyMenu', ['isLeader', 'infoPage']),
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    closeModal: function() {
      this.$emit('closeBioModal')
    },
    cancelEdit: function() {
      this.currentTextBio = this.infoPage.biography
    },
    setBio: function() {
      this.closeModal()
      window.mp.trigger('familyMenu:setBio', this.currentTextBio)
    },
  },

  mounted(){
    this.cancelEdit()
  }
}
</script>

<style lang="scss" scoped>
.modal{
  display: flex;
  justify-content: space-between;
 text-transform: uppercase;
  color: #FFFFFF;
  padding: 0 8rem;
  background: #0A0C11;
  width: 68.4rem;
  position: relative;
  height: 32.25rem;
  .btn-close{
    background-size: contain;
    background-position: center;
    background-repeat: no-repeat;
    background-color: transparent;
    width: 2rem;
    height: 2rem;
    background-image: url('/img/familyMenu/btn-close.svg');
    position: absolute;
    right: -.5rem;
    top: 0;
    transform: translateX(100%);
    &:hover{
      box-shadow: none;
      transform: translateX(100%) scale(1.1);
    }
  }
  .col{
    padding: 3.5rem 0;
    display: flex;
    width: 100%;
    flex-flow: column;
    align-items: flex-start;
    overflow-y: auto;
    &::-webkit-scrollbar{
      background: transparent;
      width: 0;
    }
    .title{
     text-transform: uppercase;
      font-size: 5rem;
      line-height: 6rem;
      margin-bottom: .6rem;
    }
    .desc{
      font-size: 1rem;
      line-height: 1.2rem;
      color: rgba(255, 255, 255, 0.5);
      margin-bottom: 1.5rem;
      padding-bottom: .8rem;
      border-bottom: 1px solid rgba(255, 255, 255, 0.2);
      width: 15.45rem;
    }
    .btn-inter{
      width: 15.45rem;
      margin-bottom: .9rem;
      border: 1px solid #DCFF00;
      height: 3rem;
      font-size: 1rem;
      color: #DCFF00;
      &:hover{
        filter: drop-shadow(0 0 .2rem #DCFF00);
        text-shadow: 0 0 .2rem #DCFF00;
      }
      &.cancel{
        border: 1px solid rgba(255, 255, 255, 0.4);
        color: rgba(255, 255, 255, 0.6);
        &:hover{
          filter: drop-shadow(0 0 .2rem #FFFFFF);
          text-shadow: 0 0 .2rem #FFFFFF;
        }
      }
    }
    .text-info{
      resize: none;
      border: 1px solid rgba(255, 255, 255, 0.4);
      padding: 1.3rem 1.6rem;
      width: 100%;
      height: 25rem;
      font-family: "Roboto";
      font-size: .9rem;
      background: transparent;
      outline: none;
      color: #fff;
      &::placeholder{
        color: rgba(255, 255, 255, 0.4);
        text-transform: uppercase;
      }
    }
  }
}
</style>

