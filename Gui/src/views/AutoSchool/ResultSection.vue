<template>
  <div class="result-section">
    <template v-if="this.currentSection.data === 'theory'">
      <div class="result-section__header">
        <div class="title">{{loc('AutoSchool_11')}}</div>
      </div>
      <div class="result-section__info">
        <div class="info-row">
          <div class="desc">{{loc('AutoSchool_12')}}</div>
          <div class="value">{{this.answersOnQuestions.correctQuest}}</div>
        </div>
        <div class="info-row">
          <div class="desc">{{loc('AutoSchool_13')}}</div>
          <div class="value">{{this.answersOnQuestions.totalQuest}}</div>
        </div>
        <div class="info-row">
          <div class="desc">{{loc('AutoSchool_11')}}</div>
          <div 
            class="value-result"
            v-if="this.answersOnQuestions.result"
          >{{loc('AutoSchool_14')}}</div>
          <div 
            class="value-result negative"
            v-else
          >{{loc('AutoSchool_15')}}</div>
        </div>
        <div class="btn" @click="setCurrentSection({section: 'GeneralSection', data: null})">{{loc('AutoSchool_16')}}</div>
      </div>
      <div class="result-section__answers">
        <div class="title">{{loc('AutoSchool_17')}}</div>
        <div class="answers-list">
          <AnswerResult
            v-for="question in this.answersOnQuestions.questions"
            :key="question.id"
            :question="question"
          />
        </div>
      </div>
    </template>
    <template v-else-if="this.currentSection.data === 'practic'">
      <div class="result-section__header">
        <div class="title">{{loc('AutoSchool_20')}}</div>
      </div>
      <div class="result-section__info">
        <div class="info-row">
          <div class="desc">{{loc('AutoSchool_21')}}</div>
          <div class="value value-time">{{this.practicResults.time}}</div>
        </div>
        <div class="info-row">
          <div class="desc">{{loc('AutoSchool_22')}}</div>
          <div class="value">{{this.practicResults.quality}}</div>
        </div>
        <div class="info-row">
          <div class="desc">{{loc('AutoSchool_11')}}</div>
          <div 
            class="value-result"
            v-if="this.practicResults.result"
          >{{loc('AutoSchool_14')}}</div>
          <div 
            class="value-result negative"
            v-else
          >{{loc('AutoSchool_15')}}</div>
        </div>
        <div class="btn" @click="setCurrentSection({section: 'GeneralSection', data: null})">{{loc('AutoSchool_16')}}</div>
      </div>
    </template>
  </div>
</template>

<script>
import {mapGetters, mapMutations, mapState} from 'vuex'

import AnswerResult from './AnswerResult'

export default {
  name: 'ResultSection',

  components: {
    AnswerResult
  },

  computed: {
    ...mapState('autoSchool', ['answersOnQuestions', 'practicResults', 'currentSection']),
    ...mapGetters('localization', ['loc']),
  },

  methods: {
    ...mapMutations('autoSchool', ['setCurrentSection'])
  }
}
</script>

<style lang="scss" scoped>
  .result-section{
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
      padding: 3.2rem 17rem 0 17rem;
      .info-row{
        width: 17.75rem;
        display: flex;
        align-items: center;
        justify-content: space-between;
        margin-bottom: .65rem;
        height: 2.25rem;
        min-height: 2.25rem;
        &:last-child{
          margin-bottom: 0;
        }
        .desc, .value, .value-result{
          font-weight: bold;
          font-size: .9rem;
          line-height: 1.05rem;
        }
        .desc, .value{
          color: #FFFFFF;
        }
        .value-result{
          color: #00FF0A;
          &.negative{
            color: #AE0000;
          }
        }
        .value{
          height: 2.25rem;
          width: 2.25rem;
          border: 1px solid #FF008A;
          display: flex;
          align-items: center;
          justify-content: center;
          border-radius: 50%;
          &.value-time{
            width: fit-content;
            padding: 0 1.3rem;
            border-radius: 2.25rem;
            transform: translateX(50%);
          }
        }
      }
      .btn{
        display: flex;
        align-items: center;
        justify-content: center;
        background: transparent;
        min-height: 2.75rem;
        height: 2.75rem;
        width: 5.55rem;
        font-weight: 500;
        font-size: .95rem;
        line-height: 1.1rem;
        color: #FFFFFF;
        border: .1rem solid #9B40F7;
        border-radius: 2.55rem;
        margin-top: 1.6rem; 
        &:hover{
          background: #9B40F7;
          transition: .2s;
        }
      }
    }
    &__answers{
      display: flex;
      flex-flow: column;
      padding: .95rem 17rem 0 17rem;
      border-top: 1px solid rgba(255, 255, 255, 0.08);
      margin-top: 1.75rem;
      .title{
        font-weight: bold;
        font-size: 1.3rem;
        line-height: 1.5rem;
        color: #FFFFFF;
      }
      .answers-list{
        display: flex;
        flex-flow: column;
        padding-right: 1.3rem;
        width: 47.35rem;
        height: 21.35rem;
        overflow-y: auto;
        margin-top: 1.75rem;
        &::-webkit-scrollbar {
          width: .4rem;
          background: rgba(0, 0, 0, 0.21);
          border-radius: 1.05rem;
        }
        &::-webkit-scrollbar-thumb {
          background: linear-gradient(0deg, #AD00FF, #AD00FF), rgba(0, 0, 0, 0.43);
          border-radius: 1.05rem;
        }
      }
    }
  }
</style>
