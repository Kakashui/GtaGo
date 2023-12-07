<template>
  <div class="general-section">
    <div class="general-section__header">
      <div class="title">{{loc(this.currentSchoolWelcome)}}</div>
      <div class="subtitle">{{loc('AutoSchool_3')}}</div>
    </div>
    <div class="general-section__choice">
      <div class="block-interaction theory">
        <div class="block-interaction__desc">{{loc('AutoSchool_4')}}</div>
        <div class="btn btn-apply" @click="startExam('theory')">{{loc('AutoSchool_5')}}</div>
        <div 
          class="btn btn-result allowed"
          v-if="Object.entries(this.answersOnQuestions).length !== 0"
          @click="showResult('theory')"
        >{{loc('AutoSchool_6')}}</div>
        <div 
          class="btn btn-result"
          v-else
        >{{loc('AutoSchool_6')}}</div>
      </div>
      <div class="block-interaction practic">
        <div class="block-interaction__desc">{{loc('AutoSchool_7')}}</div>
        <div class="btn btn-apply" @click="startExam('practic')">{{loc('AutoSchool_5')}}</div>
        <div 
          class="btn btn-result allowed"
          v-if="Object.entries(this.practicResults).length !== 0"
          @click="showResult('practic')"
        >{{loc('AutoSchool_6')}}</div>
        <div 
          class="btn btn-result"
          v-else
        >{{loc('AutoSchool_6')}}</div>
      </div>
    </div>
  </div>
</template>

<script>
import {mapState, mapMutations, mapGetters } from 'vuex'

export default {
  name: 'GeneralSection',

  computed: {
    ...mapGetters('localization', ['loc']),
    ...mapState('autoSchool', ['answersOnQuestions', 'practicResults', 'currentSchoolWelcome'])
  },

  methods: {
    ...mapMutations('autoSchool', ['setCurrentSection']),

    startExam: function(typeExam) {
      window.mp.trigger('school:startExam', typeExam);
    },
    
    showResult: function(typeResult) {
      this.setCurrentSection({section: 'ResultSection', data: typeResult})
    }
  }
}
</script>

<style lang="scss" scoped>
  .general-section{
    width: 100%;
    &__header{
      background: rgba(134, 0, 137, 0.13);
      backdrop-filter: blur(.9rem);
      display: flex;
      flex-flow: column;
      align-items: center;
      justify-content: center;
      .title, .subtitle{
        color: #FFFFFF;
        width: 100%;
        font-weight: bold;
        text-align: center;
      }
      .title{
        padding: 2.6rem 0 1.6rem 0;
        font-size: 2.05rem;
        line-height: 2.5rem;
        text-transform: uppercase;
        border-bottom: 1px solid rgba(255,255,255, .19); 
      }
      .subtitle{
        padding: .95rem 0;
        font-size: 1.65rem;
        line-height: 2rem;
      }
    }
    &__choice{
      display: flex;
      align-items: center;
      justify-content: center;
      margin-top: 3.7rem;
      .block-interaction{
        display: flex;
        flex-flow: column;
        align-items: flex-start;
        justify-content: flex-end;
        border-radius: .85rem;
        padding-bottom: 1.4rem;
        width: 23.55rem;
        height: 23.9rem;
        min-height: 23.9rem;
        overflow: hidden;
        background-size: cover;
        background-repeat: no-repeat;
        background-position: center;
        margin-right: 3.7rem;
        &:last-child{
          margin-right: 0;
        }
        &.theory{
          background-image: url('/img/autoSchool/bg-theory.png');
        }
        &.practic{
          background-image: url('/img/autoSchool/bg-practic.png');
        }
        &__desc{
          padding: 0 2.65rem 1.55rem 2.65rem;
          width: 100%;
          border-bottom: 1px solid rgba(169, 158, 158, 0.26);
          font-weight: bold;
          font-size: 1.65rem;
          line-height: 1.95rem;
          text-transform: uppercase;
          color: #FFFFFF;
        }
        .btn{
          display: flex;
          align-items: center;
          justify-content: center;
          margin-left: 2.65rem;
          box-sizing: border-box;
          font-style: normal;
          font-weight: 500;
          color: #FFFFFF;
          transition: .2s;
          &:hover{
            transition: .2s;
          }
          &.btn-apply{
            margin-top: 1.25rem;
            margin-bottom: .7rem;
            padding: 0 1.5rem;
            font-size: .95rem;
            height: 3.65rem;
            min-height: 3.65rem;
            border: .1rem solid #9B40F7;
            border-radius: 2.55rem;
            &:hover{
              background: #9B40F7;
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
          &.btn-result{
            font-size: .75rem;
            height: 2.95rem;
            min-height: 2.95rem;
            padding: 0 1.2rem;
            background: #26192E;
            border-radius: 2.05rem;
            &.allowed{
              &:hover{
                background: #FFFFFF;
                color: #000000;
                transition: .2s;
              }
            }
          }
        }
      }
    }
  }
</style>
