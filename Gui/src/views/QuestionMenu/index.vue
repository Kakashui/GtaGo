<template>
  <div class="question-menu" v-if="title">
    <div class="question-menu-body">
      <div class="question-menu-tittle">{{loc(title)}}</div>      
      <div class="question-menu-buttons">
        <div class="question-menu-button">
          <div class="question-menu-img">Y</div>
          <div class="question-menu-text">
            <h3>{{loc('question_1')}}</h3> 
            <h4>{{loc('question_3')}}</h4>
          </div>
        </div>
        <div class="question-menu-button">
          <div class="question-menu-img">N</div>
          <div class="question-menu-text">
            <h3>{{loc('question_2')}}</h3> 
            <h4>{{loc('question_3')}}</h4>
          </div>
        </div>
      </div>
    </div>
   
  </div>
</template>

<script>
import { mapState, mapGetters } from 'vuex'

export default {
  name: 'QuestionMenu',

  computed: {
    ...mapState('questionMenu', ['title']),
    ...mapGetters('localization', ['loc'])
  },

  methods: {
  
  }
}
</script>

<style lang="scss" scoped>

.question-menu {
  text-transform: uppercase;
  width: 100vw;
  position: fixed;
  bottom: 4vh;
  left: 0;
  display: flex;
  align-items: center;
  justify-content: center;
  &-body{
    width: 20rem;
    color: #fff;
  }
  &-tittle{
    padding-left: 1.3rem;
    font-weight: 700;
    text-transform: uppercase;
    font-size: 1.5rem;
    line-height: 1.6rem;
    letter-spacing: 0.1em;
    position: relative;
    &:before{
      content: '';
      width: 2px;
      height: 100%;
      position: absolute;
      left: 0;
      top: 0;
      background: #B6D300;
      box-shadow: 0 0 2rem #A8C302;
      border-radius: .2rem;
    }
  }
  &-buttons{
    margin-top: 1.5rem;
    width: 100%;
    display: flex;
    justify-content: space-between;
    align-items: center;
  }
  &-button{
    width: 50%;
    display: flex;
    align-items: center;
    h3{
      font-size: 1rem;
      letter-spacing: 0.1em;
      margin-bottom: .25rem;
      font-weight: normal;
    }
    h4{
      font-size: .9rem;
      color: #777;
      letter-spacing: 0.2em;
      font-weight: normal;
    }
  }
  &-img{
    border-radius: 50%;
    width: 2rem;
    height: 2rem;
    padding: 1px;
    box-sizing: initial;
    display: flex;
    justify-content: center;
    align-items: center;
    position: relative;
    display: flex;
    align-items: center;
    justify-content: center;
    background-color: #A8C302;
    box-shadow: 0 0 1vh 0 #A8C302;
    margin-right: .7rem;
    font-size: 1.3rem;
    &:before{
      content: '';
      width: 2.3rem;
      height: 2.3rem;
      position: absolute;
      border: .1rem solid rgba(255, 255, 255, 0.3);
      border-radius: 50%;
      top: 50%;
      left: 50%;
      transform: translateX(-50%) translateY(-50%);
    }
  }
}
</style>
