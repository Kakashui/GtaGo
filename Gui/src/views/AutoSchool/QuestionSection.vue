<template>
  <div class="question-section">
    <div class="question-section__header">
      <div class="title">{{loc('AutoSchool_8')}}</div>
    </div>
    <div class="question-section__info">
      <div class="current-question">{{loc('AutoSchool_9')}} <span class="current-value">{{this.currentQuestionIndex + 1}}</span> / <span>{{questions.length}}</span></div>
      <div 
        v-if="question.image !== ''"
        class="question-img" 
        v-bind:style="{backgroundImage: 'url(/img/autoSchool/' + question.image + '.png)'}"
      ></div>
      <div class="question-text">{{loc(question.quest)}}</div>
      <div class="variants-answer">
        <Answer
          v-for="answer in question.answers"
          :key="answer.id"
          :answer="answer"
          :questionKey="question.key"
        />
      </div>
      <div class="btn-next" v-if="this.currentAnswer === null">{{loc('AutoSchool_10')}}</div>
      <div 
        class="btn-next allowed" 
        v-else
        @click="pushAnswer"
      >{{loc('AutoSchool_10')}}</div>
    </div>
  </div>
</template>

<script>
import {mapGetters, mapMutations, mapState} from 'vuex'

import Answer from './Answer'

export default {
  name: 'GeneralSection',

  data: function() {
    return {
      answersOnQuestions: [],
    }
  },

  components: {
    Answer
  },

  computed: {
    ...mapState('autoSchool', ['question', 'questions', 'currentAnswer', 'currentQuestionKey', 'currentQuestionIndex']),
    ...mapGetters('localization', ['loc']),
    question: function() {
      return this.questions[this.currentQuestionIndex]
    },
  },

  methods: {
    ...mapMutations('autoSchool', ['switchNextQuestion', 'setCurrentSection', 'switchDropQuestion']),
    pushAnswer: function() {
      let keyQuest = this.currentQuestionKey
      let keyAnswer = this.currentAnswer
      let newObj = {keyQuest, keyAnswer}
      this.answersOnQuestions.push(newObj)
      if (this.currentQuestionIndex === (this.questions.length - 1)) {
        window.mp.trigger('school:sendAnswer', JSON.stringify(this.answersOnQuestions));
        this.setCurrentSection({section: 'ResultSection', data: 'theory'})
        this.switchDropQuestion()
      } else {
        this.switchNextQuestion()
      }
    }
  }
}
</script>

<style lang="scss" scoped>
  .question-section{
    display: flex;
    flex-flow: column;
    &__header{
      background: rgba(134, 0, 137, 0.13);
      backdrop-filter: blur(.9rem);
      display: flex;
      flex-flow: column;
      align-items: center;
      justify-content: center;
      width: 100%;
      .title{
        color: #FFFFFF;
        width: 100%;
        font-weight: bold;
        text-align: center;
        padding: 1.75rem 0;
        font-size: 1.65rem;
        line-height: 2rem;
      }
    }
    &__info{
      width: 100%;
      display: flex;
      flex-flow: column;
      align-items: flex-start;
      padding: 3.2rem 17rem 0 17rem;
      .current-question{
        display: flex;
        align-items: center;
        justify-content: center;
        padding: .75rem 1.2rem;
        border: 1px solid #AD00FF;
        box-sizing: border-box;
        border-radius: 2.95rem;
        font-weight: 500;
        font-size: .75rem;
        line-height: .4rem;
        margin-bottom: 1.5rem;
        white-space: pre;
        color: #FFFFFF;
        .current-value{
          color: #AD00FF;
        }
      }
      .question-img{
        background-size: contain;
        background-repeat: no-repeat;
        background-position: center;
        height: 9.85rem;
        width: 25.8rem;
      }
      .question-text{
        font-style: normal;
        font-weight: bold;
        font-size: 1.25rem;
        line-height: 1.45rem;
        color: #FFFFFF;
        margin-top: 2.4rem;
        margin-bottom: 3.2rem;
      }
      .variants-answer{
        display: grid;
        width: 44.35rem;
        grid-template-columns: repeat(2, 1fr);
        grid-auto-rows: auto;
        grid-row-gap: .95rem;
        grid-column-gap: 3.95rem;
      }
      .btn-next{
        display: flex;
        align-items: center;
        justify-content: center;
        box-sizing: border-box;
        font-style: normal;
        font-weight: 500;
        color: #FFFFFF;
        transition: .2s;
        margin-top: 3.8rem;
        padding: 0 1.5rem;
        font-size: .95rem;
        height: 3.65rem;
        min-height: 3.65rem;
        border: .1rem solid transparent;
        background: rgba(63, 37, 89, 0.26);
        border-radius: 2.55rem;
        &.allowed{
          background: transparent;
          border: .1rem solid #9B40F7;
          &:hover{
            background: #9B40F7;
            box-shadow: 0 .5rem 1.5rem -.5rem rgba(173, 0, 255, 0.8);
            transition: .2s;
          }
        }
        &:after{
          content: '';
          width: 1.3rem;
          height: 1.3rem;
          background-image: url('/img/autoSchool/btn-arr.svg');
          background-size: contain;
          background-position: center;
          background-repeat: no-repeat;
          margin-left: 1.9rem;
        }
      }
    }
  }
</style>