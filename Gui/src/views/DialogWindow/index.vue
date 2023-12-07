<template>
  <div class="dialog-window">
    <div class="dialog-window__display">
      <div class="dialog-window__display-info">
        <div class="dialog-window__display-info__name">{{ loc(dialogData.name) }}</div>
        <div class="dialog-window__display-info__desc">{{ loc(dialogData.desc) }}</div>
      </div>
      <div class="dialog-window__display-text">{{ loc(dialogData.text) }}</div>
    </div>
    <div class="dialog-window__answers">
      <div
        :class="[{active: answer.id === currentAnswer}, 'dialog-window__answers-info']"
        v-for="answer in dialogData.answers"
        :key="answer.key"
        @click="setCurrentAnswer(answer.id)"
      >{{ loc(answer.text) }}</div>
    </div>
  </div>
</template>

<script>
import { mapState, mapGetters } from 'vuex'
export default {
  name: 'DialogWindow',

  data: function() {
    return {
      currentAnswer: null
    }
  },

  computed: {
    ...mapState('dialogWindow', ['dialogData']),
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    setCurrentAnswer: function(id) {
      this.currentAnswer = id
      window.mp.trigger("dialogWindow:currentAnswer", id)    
    }
  }
}
</script>

<style lang="scss" scoped>
.dialog-window{
  display: flex;
  flex-flow: column;
  align-items: center;
  justify-content: flex-end;
  width: 100%;
  position: absolute;
  bottom: 0;
  left: 0;
  padding-bottom: 2rem;
  text-align: center;
  color: #fff;
  &__display-info, &__answers{
    text-transform: uppercase;
    letter-spacing: 0.05em;
    text-transform: uppercase;
  }
  &__display, &__answers{
    width: 25.8rem;
  }
  &__display{
    position: relative;
    display: flex;
    flex-flow: column;
    background: radial-gradient(47% 501.24% at 50% 50%, #000000 0%, rgba(0, 0, 0, 0) 100%);
    filter: drop-shadow(0 .6rem 1.1rem rgba(0, 0, 0, 0.35));
    border-radius: .5rem;
    &:after{
      content: '';
      width: 100%;
      height: .2rem;
      position: absolute;
      top: 0;
      left: 0;
      background: radial-gradient(32.1% 175% at 49.9% 50%, #C1E704 0%, rgba(193, 231, 4, 0) 100%);
      filter: drop-shadow(0 .2rem .6rem rgba(193, 231, 4, 0.40));
    }
    &-info{
      width: 100%;
      padding: 1.15rem 0 .75rem 0;
      &__name{
        font-size: 1.55rem;
        font-weight: bold;
      }
      &__desc{
        opacity: .85;
        margin-top: .2rem;
        font-size: .9rem;
        font-weight: bold;
      }
    }
    &-text{
      position: relative;
      display: flex;
      align-items: center;
      justify-content: center;
      font-size: .8rem;
      line-height: .95rem;
      padding: 1rem 3rem 1.3rem 3rem;      
      letter-spacing: .05rem;  
      background: radial-gradient(49.49% 35.85% at 50.06% -11.32%, #000000 0%, rgba(0, 0, 0, 0) 100%);
      &:after{
        content: '';
        width: 100%;
        height: 1px;
        position: absolute;
        top: 0;
        left: 0;
        background: linear-gradient(90deg, rgba(255, 255, 255, 0) 0%, #FFFFFF 45.31%, rgba(255, 255, 255, 0) 100%);
        opacity: 0.7;
      }
    }
  }
  &__answers{
    display: grid;
    margin-top: .8rem;
    grid-template-columns: repeat(2, 1fr);
    grid-auto-rows: 3.3rem;
    grid-gap: .8rem;
    &-info{
      position: relative;
      display: flex;
      align-items: center;
      justify-content: center;
      background: radial-gradient(47% 501.24% at 50% 50%, #000000 0%, rgba(0, 0, 0, 0) 100%);
      filter: drop-shadow(0 .6rem 1.1rem rgba(0, 0, 0, 0.35));
      border-radius: .5rem;
      transition: .2s;
      &:hover{
        transition: .2s;
        color: #C1E704;
        text-shadow: 0 .2rem .6rem rgba(0, 0, 0, 0.25);
        background: radial-gradient(47% 501.24% at 50% 50%, rgba(193, 231, 4, 0.60) 0%, rgba(193, 231, 4, 0) 100%);
        filter: drop-shadow(0 .6rem 1.1rem rgba(0, 0, 0, 0.35));
        transform: scale(1.1);
        &:before, &:after{
          content: '';
          width: 100%;
          height: 1px;
          position: absolute;
          left: 0;
          background: radial-gradient(32.1% 175% at 49.9% 50%, #C1E704 0%, rgba(193, 231, 4, 0) 100%);
          filter: drop-shadow(0 .2rem .6rem rgba(193, 231, 4, 0.40));
        }
        &:before{
          top: 0;
        }
        &:after{
          bottom: 0;
        }
      }
      &.active{
        transition: .2s;
        color: #C1E704;
      }
    }
  }
}
</style>
