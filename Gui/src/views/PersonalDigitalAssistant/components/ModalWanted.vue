<template>
  <div class="modal-wrap">
    <div class="modal-wanted">
      <div
        class="btn btn-close"
        @click="closeModal"
      ></div>
      <div class="title">{{loc('Pda_30')}}</div>
      <div class="stars-list">
        <div 
          :class="[{active: item <= currentWantedLevel}, 'star']"
          v-for="(item, index) in 5"
          :key="item.key"
          @click="setCurrentWantedLevel(index + 1)"
        ></div>
      </div>
      <textarea 
        class="textarea" 
        rows="3" 
       :placeholder="loc('pda:wanted:plh')"
        v-model="reason"
      ></textarea>
      <div class="btns-wrap">
        <div 
          :class="[{army: type === 2}, {fbi: type === 1}, 'btn', 'btn-edit', 'active']"
          v-if="wantedLevel !== currentWantedLevel && reason"
          @click="setWanted"
        >{{loc('Pda_36')}}</div>
        <div 
          class="btn btn-edit"
          v-else
        >{{loc('Pda_36')}}</div>
        <div 
          class="btn active"
          v-if="reason"
          @click="removeWanted"
        >{{loc('Pda_35')}}</div>
        <div 
          class="btn"
          v-else
        >{{loc('Pda_35')}}</div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
export default {
  name: 'ModalWanted',

  props: {
    typeModal: String,
    id: Number,
    wantedLevel: Number,
    reason: String
  },

  data: function() {
    return {
      currentWantedLevel: null,
      currentReason: null
    }
  },

  computed: {
    ...mapState('personalDigitalAssistant', ['type']),
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    closeModal: function() {
      this.$emit('closeModal')
    },
    setCurrentWantedLevel: function(index) {
      this.currentWantedLevel = index
    },
    setWanted: function() {
      window.mp.trigger('pda:setWanted', this.typeModal, this.id, this.currentWantedLevel, this.reason)
      this.closeModal()
    },
    removeWanted: function() {
      window.mp.trigger('pda:setWanted', this.typeModal, this.id, 0, this.reason)
      this.closeModal()
    }
  },

  created(){
    this.setCurrentWantedLevel(this.wantedLevel)
    this.currentReason = this.reason
  }
}
</script>

<style lang="scss" scoped>
.modal-wanted{
  display: flex;
  flex-flow: column;
  align-items: center;
  justify-content: center;
  background: #FFFFFF;
  border-radius: 1rem;
  padding: 2rem 2rem 1.4rem 2rem;
  position: relative;
  color: #000;
  font-size: 1rem;
  line-height: 1.15rem;
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
  }
  .stars-list{
    display: flex;
    align-items: center;
    margin: 1rem 0;
    &:hover{
      .star{
        background-image: url('/img/personalDigitalAssistant/wanted-star-active.svg');
      }
    }
    .star{
      width: calc((1.7rem) + (.5rem));
      height: 1.7rem;
      background-size: contain;
      background-position: left center;
      background-repeat: no-repeat;
      background-image: url('/img/personalDigitalAssistant/wanted-star.svg');
      padding-right: .5rem;
      &:last-child{
        padding-right: 0;
        width: 1.7rem;
      }
      &:hover ~ .star{
        background-image: url('/img/personalDigitalAssistant/wanted-star.svg');
      }
      &.active{
        background-image: url('/img/personalDigitalAssistant/wanted-star-active.svg');
      }
    }
  }
  .textarea{
    resize: none;
    background: #F2F3F5;
    border-radius: .3rem;
    border: none;
    outline: none;
    padding: .5rem;
    box-sizing: border-box;
    font-family: Arial;
    font-style: normal;
    font-weight: normal;
    font-size: 1rem;
    line-height: 1.15rem;
    color: #616162;
    width: 100%;
    margin-bottom: 1rem;
    &::placeholder{
      color: #CBCCCE;
    }
  }
  .btns-wrap{
    display: flex;
    align-items: center;
    justify-content: center;
    .btn{
      width: 100%;
      padding: .6rem 1rem;
      border-radius: .4rem;
      color: #CBCCCE;
      white-space: pre;
      font-weight: bold;
      &.btn-edit{
        color: rgba(0, 0, 0, 0.16);
        background: #F2F3F5;
        &.active{
          color: #000000;
          background: #FFD130;
          &:hover{
            filter: drop-shadow(0 0 1rem rgba(255, 209, 48, 0.8));
          }
        }
        &.fbi{
          &.active{
            background: #FB7712;
            color: #fff;
            &:hover{
              filter: drop-shadow(0 0 1rem rgba(251, 119, 18, .8));
            }
          }
        }
        &.army{
          &.active{
            background: #6C8073;
            color: #fff;
            &:hover{
              filter: drop-shadow(0 0 1rem rgba(108, 128, 115, .8));
            }
          }
        }
      }
      &.active{
        color: #616162;
        &:hover{
          color: #000;
        }
      }
    }
  }
}
</style>
