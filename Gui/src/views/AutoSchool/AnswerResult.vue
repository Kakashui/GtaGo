<template>
  <div class="answers-list__desc">
    <div class="answers-list__question">{{loc(question.quest)}}</div>
    <div class="answers-list__info">
      <div class="true-answer">
        <div class="prompt">{{loc('AutoSchool_18')}}</div>
        <span class="text">{{loc(question.correctAnswer)}}</span>
      </div>
      <div 
        class="your-answer" 
        v-bind:class="{true: question.myAnswer === question.correctAnswer}"
      >
        <div class="prompt">{{loc('AutoSchool_19')}}</div>
        <span class="text">{{loc(question.myAnswer)}}</span>
      </div>
    </div>
  </div>
</template>

<script>
import {mapGetters} from 'vuex'

export default {
  name: 'AnswerResult',

  props: {
    question: Object
  },

  computed: {
    ...mapGetters('localization', ['loc']),
  }
}

</script>

<style lang="scss" scoped>
.answers-list{
  &__desc{    
    background: rgba(0, 0, 0, 0.14);
    border-radius: .75rem;
    padding: 1.5rem 0 .8rem 0;
    margin-bottom: .65rem;
    &:last-child{
      margin-bottom: 0;
    }
  }
  &__question{
    font-weight: bold;
    font-size: .9rem;
    line-height: 1.05rem;
    color: #FFFFFF;
    padding: 0 1.8rem 1.2rem 1.8rem;
    width: 100%;
    border-bottom: 1px solid rgba(255, 255, 255, 0.08);
  }
  &__info{
    padding: 1.05rem 1.8rem 0 1.8rem;
    display: flex;
    justify-content: flex-start;
    align-content: flex-start;
    .true-answer, .your-answer{
      display: flex;
      flex-flow: column;
      align-content: flex-start;
      width: 14.2rem;
      .prompt{
        font-size: .65rem;
        line-height: .75rem;
        color: #FFFFFF;
        margin-bottom: .6rem;
      }
      .text{
        font-weight: normal;
        font-size: .7rem;
        line-height: .8rem;
        width: 100%;
      }
    }
    .true-answer{
      margin-right: 3.5rem;
      .text{
        color: #FFFFFF;
      }
    }
    .your-answer{
      &.true{
        .text{
          color: #00FF0A;
        }
      }
      .text{
        color: #AE0000;
      }
    }
  }
}
</style>
