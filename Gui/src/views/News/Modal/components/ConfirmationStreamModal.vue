<template>
  <div class="confirmation-stream-modal">
    <div class="confirmation-stream-modal__wrap">
      <div class="confirmation-stream-modal__wrap-title">{{loc('news:srteam:1')}}</div>
      <div class="confirmation-stream-modal__wrap-desc">{{loc('news:srteam:2')}}</div>
      <div class="confirmation-stream-modal__wrap__btns">
        <div 
          class="btn confirmation-stream-modal__wrap__btns-btn confirmation-stream-modal__wrap__btns-btn--apply"
          @click="setOnStream"
        >{{loc('news:srteam:3')}}</div>
        <div 
          class="btn confirmation-stream-modal__wrap__btns-btn"
          @click="setModal({show: false, currentModal: null, data: null})"
        >{{loc('news:srteam:4')}}</div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapMutations } from 'vuex'
export default {
  name: 'ConfirmationStreamModal', 
  computed: {
    ...mapGetters('localization', ['loc'])
  },
  methods: {
    ...mapMutations('news', ['setModal']),
    setOnStream: function() {
      window.mp.trigger('news:setOnStream')
      this.setModal({show: false, currentModal: null, data: null})
    },
  }
}
</script>

<style lang="scss" scoped>
.confirmation-stream-modal{
  position: relative;
  &__wrap{
    display: flex;
    flex-direction: column;
    align-items: center;
    background: #FFFFFF;
    border-radius: 1rem;
    padding: 2rem 0;
    width: 29.55rem;
    &-title, &-desc{
      max-width: 21.55rem;
      color: #000000;
      text-align: center;
      margin-bottom: 1rem;
    }
    &-title{
      font-weight: bold;
      font-size: 1.5rem;
      line-height: 1.7rem;
    }
    &-desc{
      font-weight: normal;
      font-size: .9rem;
      line-height: 1.05rem;
    }
    &__btns{
      display: flex;
      align-items: center;
      &-btn{
        width: 7.05rem;
        height: 2.5rem;
        border-radius: 6px;
        border: 1px solid #E30000;
        color: #E30000;
        margin-right: 1rem;
        transition: .3s;
        &:hover{
          filter: drop-shadow(0 0 .15rem #D92C2C);
        }
        &:last-child{
          margin-right: 0;
        }
        &--apply{
          background: linear-gradient(0deg, #E30000, #E30000);
          color: #fff;
          border: none;
          &:hover{
            box-shadow: 0 0 .6rem #D92C2C;
          }
        }
      }
    }
  }
}
</style>
