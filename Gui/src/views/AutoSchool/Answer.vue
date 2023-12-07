<template>
  <div
    v-bind:class="{active: answer.key === this.currentAnswer, answer}"
    @click="setAnswer({answer: answer.key, question: questionKey})"
  >
    <div class="check-icon"></div>
    <span class="text">{{loc(answer.title)}}</span>
  </div>
</template>

<script>
import {mapGetters, mapState, mapMutations} from 'vuex'

export default {
  name: 'Answer',
  
  props: {
    answer: Object,
    questionKey: Number
  },

  computed: {
    ...mapState('autoSchool', ['currentAnswer']),
    ...mapGetters('localization', ['loc']),
  },

  methods: {
    ...mapMutations('autoSchool', ['setAnswer'])
  }
}
</script>

<style lang="scss" scoped>
  .answer{
    background: #1A0424;
    border-radius: .45rem;
    display: flex;
    align-items: center;
    justify-content: flex-start;
    padding: 1.15rem;
    border: 1px solid transparent;
    transition: .2s;
    .check-icon{
      width: 1.5rem;
      min-width: 1.5rem;
      height: 1.5rem;
      min-height: 1.5rem;
      margin-right: 1.75rem;
      position: relative;
      border: .1rem solid #550163;
      box-sizing: border-box;
      border-radius: .25rem;
      display: flex;
      align-items: center;
      justify-content: center;
    }
    .text{
      font-weight: 500;
      font-size: .95rem;
      line-height: 1.1rem;
      color: #FFFFFF;
    }
    &.active{
      border: 1px solid #E700A6;
      transition: .2s;
      .check-icon{
        &:before{
          content: '';
          width: .9rem;
          height: .9rem;
          background: #DB00FF;
          box-shadow: 0 0 .8rem rgba(255, 0, 199, 0.57);
          border-radius: .15rem;
          transition: .2s;
        }
      }
    }
  }
</style>
