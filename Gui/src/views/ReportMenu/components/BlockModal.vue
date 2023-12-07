<template>
  <div class="block-modal">
    <div class="modal">
      <div class="btn btn-close" @click="setShowModal"></div>
      <div 
        class="title"
        v-if="param === 'closeReport'"
      >{{loc('ReportMenu_16')}}</div>
      <div 
        class="title"
        v-else-if="param === 'kick'"
      >{{loc('ReportMenu_17')}}</div>
      <div 
        class="title"
        v-else-if="param === 'template'"
      >{{loc('ReportMenu_18')}}</div>
      <template v-if="param === 'closeReport'">
        <div class="prompt">{{loc('ReportMenu_19')}}</div>
        <div class="stars-list">
          <div 
            :class="[{active: item <= currentRating}, 'star']"
            v-for="(item, index) in 5"
            :key="item.key"
            @click="setCurrentRating(index)"
          ></div>
        </div>
      </template>
      <template v-else>
        <input 
          type="text" 
          :placeholder="loc('ReportMenu_21')"
          v-model="currentText"
        >
      </template>
      <div 
        class="btn btn-apply"
        @click="finishEvent"
      >{{loc('ReportMenu_20')}}</div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapMutations, mapState } from 'vuex'
export default {
  name: 'BlockModal', 

  props: {
    param: String
  },

  data: function() {
    return {
      currentText: null,
      currentRating: null
    }
  },

  computed: {
    ...mapState('reportMenu', ['currentChat']),
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    ...mapMutations('reportMenu', ['addAnswerTemplate']),
    setShowModal: function() {
      this.$emit('closeModal')
    },
    finishEvent: function() {
      if (this.param ==='kick') {
        window.mp.trigger('reportMenu:kickPlayer', this.currentChat.id, this.currentText)
      } else if (this.param === 'closeReport') {
        window.mp.trigger('reportMenu:closereport', this.currentChat.id, this.currentRating)
      } else if (this.param === 'template') { 
        this.addAnswerTemplate({text: this.currentText})
        window.mp.trigger('reportMenu:addAnswerTemplate', this.currentText)
      }
      this.$emit('closeModal')
    },
    setCurrentRating: function(index) {
      this.currentRating = index + 1
    }
  }
}
</script>

<style lang="scss" scoped>
.block-modal{
  position: absolute;
  top: 0;
  left: 0;
  height: 100vh;
  width: 100%;
  display: flex;
  align-items: center;
  justify-content: center;
  background: rgba(0, 0, 0, 0.65);
  .modal{
    position: relative;
    display: flex;
    flex-flow: column;
    align-items: center;
    padding: 4.25rem 1.65rem 3.5rem 1.65rem;
    min-width: 28.7rem;
    background: #17191F;
    box-shadow: 0px 22px 32px rgba(0, 0, 0, 0.35);
    .btn-close{
      background-image: url('/img/reportMenu/btn-close-header.svg');
      width: 1.95rem;
      height: 1.95rem;
      position: absolute;
      right: 1.9rem;
      top: 1.65rem;
      opacity: .25;
      &:hover{
        opacity: .6;
      }
    }
    .title{
      font-size: 2.05rem;
      line-height: 2.45rem;
      color: #FFFFFF;
      text-transform: uppercase;
    }
    .prompt{
      font-family: Roboto;
      font-style: normal;
      font-weight: normal;
      font-size: .9rem;
      line-height: 1.05rem;
      color: #FFFFFF;
      opacity: 0.45;
      margin: .45rem 0 1.75rem 0;
    }
    .stars-list{
      display: flex;
      align-items: center;
      &:hover{
        .star{
          background-image: url('/img/reportMenu/star-active.svg');
        }
      }
      .star{
        width: calc((2.5rem) + (.6rem));
        height: 2.5rem;
        background-size: contain;
        background-position: left center;
        background-repeat: no-repeat;
        background-image: url('/img/reportMenu/star.svg');
        padding-right: .6rem;
        &:last-child{
          padding-right: 0;
          width: 2.5rem;
        }
        &:hover ~ .star{
          background-image: url('/img/reportMenu/star.svg');
        }
        &.active{
          background-image: url('/img/reportMenu/star-active.svg');
        }
      }
    }
    input{
      background: #101115;
      border: 1px solid rgba(255, 255, 255, 0.15);
      box-sizing: border-box;
      outline: none;
      font-weight: bold;
      font-size: .9rem;
      line-height: 119.5%;
      color: rgba(255, 255, 255, 0.45);
      margin-top: 1.7rem;
      padding: 1.3rem 0;
      width: 100%;
      text-align: center;
    }
    .btn-apply{
      background: #C1E704;
      border-radius: .2rem;
      font-weight: bold;
      font-size: 1.2rem;
      line-height: 1.2rem;
      text-transform: uppercase;
      color: #252D00;
      padding: 1.1rem 4.5rem;
      margin-top: 2.5rem;
      &:hover{
        box-shadow: 0 .2rem 1.1rem rgba(193, 231, 4, 0.45);
      }
    }
  }
}
</style>
