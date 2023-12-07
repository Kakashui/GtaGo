<template>
  <div class="modal-wrap">
    <div class="modal-helpers">
      <div 
        class="btn btn-close"
        @click="closeModal"
      ></div>
      <div class="title">{{loc('Pda_5')}}:</div>
      <div class="modal-helpers__list">
        <div
          class="item"
          v-for="item in currentHelpers"
          :key="item.key"
        >
          <div class="id">#{{item.id}}</div>
          <div class="nickname" v-if="item.id === myId">{{loc('Pda_27')}}</div>
          <div class="nickname" v-else>{{item.name}}</div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
export default {
  name: 'ModalHelpers',

  props: {
    currentHelpers: Array
  },

  computed:{
    ...mapGetters('localization', ['loc']),
    ...mapState('personalDigitalAssistant', ['myId'])
  },

  methods: {
    closeModal: function() {
      this.$emit('closeModal')
    }
  }
}
</script>

<style lang="scss" scoped>
.modal-helpers{
  display: flex;
  flex-flow: column;
  align-items: flex-start;
  justify-content: center;
  background: #FFFFFF;
  border-radius: 1rem;
  padding: 1.5rem 0;
  position: relative;
  max-height: 26.3rem; 
  color: #000;
  font-size: 1rem;
  line-height: 1.15rem;
  width: 17.25rem;
  .btn-close{
    position: absolute;
    top: 0;
    right: -.5rem;
    transform: translateX(100%);
    width: 1.7rem;
    height: 1.7rem;
    background-image: url('/img/personalDigitalAssistant/btn-close-modal.svg');
    background-size: contain;
    background-position: center;
    background-repeat: no-repeat;
    &:hover{
      transform: translateX(100%) scale(1.1);
    }
  }
  .title{
    font-weight: bold;
    margin-left: 2rem;
    margin-bottom: 1rem;
  }
  &__list{
    width: 100%;
    overflow-y: auto;
    height: 100%;
    padding: 0 1.3rem 0 2rem;
    margin-left: .5rem;
    transform: rotateY(180deg);
    &>div{
      transform: rotateY(-180deg);
    }
    &::-webkit-scrollbar{
      background: #D5D4D4;
      border-radius: .1rem;
      width: .2rem;
      &-thumb{
        background: #81878F;
        border-radius: .1rem;
      }
    }
    .item{
      display: flex;
      width: 100%;
      align-items: flex-start;
      justify-content: space-between;
      margin-bottom: 1rem;
      .id{        
        color: #81878F;
      }
      .nickname{
        width: 100%;
        text-align: right;
      }
    }
  }
}
</style>
