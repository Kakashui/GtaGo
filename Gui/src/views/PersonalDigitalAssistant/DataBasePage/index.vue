<template>
  <div class="data-base-page">
    <div class="search-wrap">
      <input 
        type="text"
        v-model="currentSearchText"
        :placeholder="loc('Pda_33')"
      >
      <div 
        class="btn btn-discard"
        @click="discardCurrentSearchText"
        v-if="currentSearchText"
      ></div>
      <div class="btn btn-find" @click="findPrisoner">{{loc('Pda_44')}}</div>
    </div>
    <div class="prisoners-list">
      <div class="prisoners-list__head">
        <div class="head-item">{{loc('pda:db:1')}}</div>
        <div class="head-item">{{loc('pda:db:2')}}</div>
        <div class="head-item">{{loc('pda:db:3')}}</div>
        <div class="head-item">{{loc('pda:db:4')}}</div>
        <div class="head-item">{{loc('pda:db:5')}}</div>
        <div class="head-item">{{loc('pda:db:6')}}</div>

      </div>
      <div class="prisoners-list__body scroll">
        <prisoner-item
          v-for="item in currPrisoners"
          :key="item.id"
          :item="item"
        />
      </div>
    </div>
  </div>
</template>

<script>
import PrisonerItem from './PrisonerItem'
import { mapGetters, mapState } from 'vuex'
export default {
  name: 'DataBasePage',

  components: {
    PrisonerItem
  },

  data: function() {
    return{
      currentSearchText: null,
      currentFilter: null,
    }
  }, 

  methods: {
    discardCurrentSearchText: function() {
      this.currentSearchText = null
      this.currentFilter = null
      
    },
    findPrisoner: function() {
      this.currentFilter = this.currentSearchText.toLowerCase()
    }
  },

  computed: {
    ...mapGetters('localization', ['loc']),
    ...mapState('personalDigitalAssistant/dataBase', ['prisoners']),
    currPrisoners: function() {
      let array = this.prisoners.slice(0, this.prisoners.length).filter(item => !this.currentFilter || item.name != null && item.name.toLowerCase().includes(this.currentFilter))
      return array
    }
    
  },
}
</script>

<style lang="scss" scoped>
.data-base-page{
  width: 100%;
  height: 100%;
  .search-wrap{
    width: 100%;
    height: 3rem;
    min-height: 3rem;
    margin-bottom: 1.5rem;
    display: flex;
    align-items: center;
    position: relative;
    input{
      height: 100%;
      width: 100%;
      text-align: left;
      padding: 0 3rem 0 1rem;
      border-left: .1rem solid #F2F3F5;
      border-top: .1rem solid #F2F3F5;
      border-bottom: .1rem solid #F2F3F5;
      box-sizing: border-box;
      border-radius: .1rem 0 0 .1rem;
      outline: none;
      background: transparent;
      color: rgba(255, 255, 255, 1);
      &::placeholder{
        color: rgba(255, 255, 255, 0.6);
      }
    }
    .btn-find{
      font-weight: bold;
      font-size: 1rem;
      line-height: 1.15rem;
      padding: .9rem 1.8rem;
      height: 100%;
      white-space: pre;
      background-size: cover;
      background-position: center right;
      background-repeat: no-repeat;
      background-image: url('/img/personalDigitalAssistant/btn-find.svg');
      color: #000000;
      transform: translateX(-4%);
      &:hover{
        transform: translateX(-1%);
      }
    }
    .btn-discard{
      width: 1.7rem;
      height: 1.7rem;
      position: absolute;
      right: 7.7rem;
      background-size: contain;
      background-repeat: no-repeat;
      background-position: center;
      background-image: url('/img/personalDigitalAssistant/btn-discard.svg');
      &:hover{
        transform: scale(1.1);
      }
    }
  }
  .prisoners-list{
    display: flex;
    flex-flow: column;
    &__head{
      display: flex;
      padding: 0 1.5rem;
      align-items: center;
      width: 100%;
      margin-bottom: 1rem;
      color: rgba(255, 255, 255, 0.7);
    }
    &__head>div{
      display: flex;
      &:nth-child(1){
        width: 15rem;
        min-width: 15rem;
      }
      &:nth-child(2){
        width: 7rem;
        min-width: 7rem;
      }
      &:nth-child(3){
        width: 2rem;
        min-width: 2rem;
        justify-content: center;
        margin-right: 1.9rem;
      }
      &:nth-child(4){
        width: 8.65rem;
        min-width: 8.65rem;
      }
      &:nth-child(5){
        width: 10.5rem;
        min-width: 10.5rem;
      }
      &:nth-child(6){
        width: 100%;
      }
    }
    &__body{
      padding-right: 1.1rem;
      margin-right: -1.1rem;
      display: flex;
      flex-flow: column;
      width: 100%;
      overflow-y: auto;
      height: 100%;
      max-height: 100%;
      height: 23.55rem;
    }
  }
}
</style>