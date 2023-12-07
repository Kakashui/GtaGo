<template>
  <div class="auto-school">
    <div class="auto-school-body">
      <div class="header-browser">
        <div class="header-browser__title">
          <div class="desc">Browser</div>
          <div class="icons">
            <div class="icon icon-roll-up"></div>
            <div class="icon icon-modal"></div>
            <div class="icon icon-close" @click="closeBrowser"></div>
          </div>
        </div>
        <div class="header-browser__url">
          <div class="icons">
            <div class="icon icon-back"></div>
            <div class="icon icon-forward"></div>
            <div class="icon icon-refresh"></div>
            <div class="icon icon-home"></div>
          </div>
          <div class="input-url">{{loc(this.currentSchoolName)}}</div>
        </div>
      </div>
      <component :is="this.currentSection.section"/>
    </div>    
  </div>
</template>

<script>
import {mapGetters, mapState} from 'vuex'

import GeneralSection from './GeneralSection'
import QuestionSection from './QuestionSection'
import ResultSection from './ResultSection'

export default {
  name: 'AutoSchool',

  components: {
    GeneralSection,
    QuestionSection,
    ResultSection
  },

  computed: {
    ...mapState('autoSchool', ['currentSection', 'currentSchoolName']),
    ...mapGetters('localization', ['loc']),
  },

  methods: {
    closeBrowser: function() {
      window.mp.trigger('school:closeMenu');
    }
  }
}
</script>

<style lang="scss" scoped>
  .auto-school{
    position: absolute;
    top: 0;
    left: 0;
    width: 100vw;
    height: 100vh;
    background-image: url('/img/autoSchool/bg-main.png');
    background-size: cover;
    z-index: 1; 
    &-body{
      max-width: 1920px;
      height: 100vh;
      margin: auto;
      position: relative;
    }
    .header-browser{
      width: 100%;
      display: flex;
      flex-flow: column;
      &__title, &__url{
          display: flex;
          justify-content: space-between;
          align-items: center;
          .icons{
            display: flex;
            align-items: center;
            .icon{
              width: 1.35rem;
              height: 1.35rem;
              display: flex;
              background-size: contain;
              background-repeat: no-repeat;
              background-position: center;
              margin-right: .5rem;
              &:last-child{
                margin-right: 0;
              }
              &.icon-roll-up{
                background-image: url('/img/autoSchool/icon-roll-up.svg');
              }
              &.icon-modal{
                background-image: url('/img/autoSchool/icon-modal.svg');
              }
              &.icon-close{
                background-image: url('/img/autoSchool/icon-close.svg');
              }
              &.icon-back{
                background-image: url('/img/autoSchool/icon-back.svg');
              }
              &.icon-forward{
                background-image: url('/img/autoSchool/icon-forward.svg');
              }
              &.icon-refresh{
                background-image: url('/img/autoSchool/icon-refresh.svg');
              }
              &.icon-home{
                background-image: url('/img/autoSchool/icon-home.svg');
              }
            }
          }
      }
      &__title{
        background-color: #F2F2F2;
        height: 2.4rem;
        min-height: 2.4rem;
        padding: 0 1.3rem;
        .desc{
          display: flex;
          align-items: center;
          justify-content: flex-start;
          font-weight: bold;
          font-size: 1rem;
          color: #B6B6B6;
          padding-left: 2.3rem;
          position: relative;
          text-transform: uppercase;
          &:before{
            content: '';
            width: 1.3rem;
            height: 1.3rem;
            position: absolute;
            left: 0;
            background-image: url('/img/autoSchool/brows-icon.svg');
            background-size: contain;
            background-position: center;
            background-repeat: no-repeat;
          }
        }
      }
      &__url{
        background: #fff;
        height: 2rem;
        min-height: 2rem;
        padding: 0 .45rem;
        .input-url{
          width: 100%;
          margin-left: .5rem;
          height: 1.5rem;
          min-height: 1.5rem;
          display: flex;
          align-items: center;
          justify-content: flex-start;
          background-color: #F2F2F2;
          border-radius: 1.5rem;
          padding: 0 1.1rem;
          font-weight: 500;
          font-size: .8rem;
          color: #676767;
        }
      }
    }
  }
</style>
