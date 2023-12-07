<template>
  <div class="management-page">
    <div 
      class="management-page__btn-back"
      @click="setCurrentPage('MainPage')"
    >{{loc('news:mngr:back')}}</div>
    <div class="management-page-title">{{loc('news:mngr:ctrl')}}</div>
    <div class="management-page__body">
      <div class="management-page__body-col">
        <div class="scroll workers-list">
          <WorkerItem 
            v-for="item in workersList" 
            :key="item.workerId"
            :item="item"
          />
        </div>
      </div>
      <div class="management-page__body-col">
        <div class="management-page__body-col-title">{{loc('news:mngr:rangs')}}</div>
        <div class="scroll ranks-list">
          <RankItem 
            v-for="item in ranksList" 
            :key="item.rankId"
            :item="item"
            />
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapMutations, mapState } from 'vuex'

import WorkerItem from './WorkerItem'
import RankItem from './RankItem'
export default {
  name: 'ManagementPage', 

  components: {
    WorkerItem,
    RankItem
  },

  computed: {
    ...mapState('news', ['ranksList', 'workersList']),
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    ...mapMutations('news', ['setCurrentPage'])
  }
}
</script>

<style lang="scss" scoped>
.management-page{
  padding: 5rem;
  position: relative;
  width: 100%;
  height: 100%;
  flex-direction: column;
  &:before{
    content: '';
    width: 100%;
    height: 1px;
    position: absolute;
    top: 4rem;
    left: 0;
    background: rgba(51, 51, 51, 0.1);
  }
  &__btn-back{
    position: absolute;
    font-weight: bold;
    font-size: 1rem;
    color: #212121;
    position: absolute;
    display: flex;
    align-items: center;
    top: 1.5rem;
    left: 5rem;
    &:hover{
      &:before{
        transform: scale(1.1);
      }
    }
    &:before{
      content: '';
      width: 1rem;
      height: 1rem;
      margin-right: .5rem;
      background-size: contain;
      background-repeat: no-repeat;
      background-position: center;
      background-image: url('/img/news/icon-arr.svg');
      transform-origin: 50% 50%;
      transition: .2s;
    }
  }
  &-title{
    padding-left: 1.5rem;
    font-weight: normal;
    font-size: 3rem;
    line-height: 3.45rem;
    max-width: 14rem;
    color: #000000;
    border-left: .2rem solid #D92C2C;
    margin-bottom: 1rem;
  }
  &__body{
    display: flex;
    width: 100%;
    height: calc(100% - (2.5rem));
    justify-content: space-between;
    &-col{
      position: relative;
      &-title{
        font-weight: normal;
        font-size: 2rem;
        line-height: 2.3rem;
        color: #000000;
        position: absolute;
        top: -.9rem;
        transform: translateY(-100%);
      }
      &:first-child{
        width: 25rem;
        min-width: 25rem;
        margin-right: 4rem;
      }
      &:last-child{
        width: 100%;
        margin-right: -1.2rem;
      }
      .scroll{
        overflow-y: auto;
        height: 100%;
        padding-right: 1.2rem;
        &::-webkit-scrollbar{
          width: .2rem;
          &-thumb{
            background: rgba(175, 185, 197, 0.4);
            border-radius: .1rem;
          }
        }
      }
      .workers-list, .ranks-list{
        display: grid;
        padding-bottom: 1rem;
      }
      .workers-list{
        grid-auto-rows: 4.9rem;
        grid-row-gap: .5rem;
      }
      .ranks-list{
        grid-auto-rows: 10rem;
        grid-row-gap: 1rem;
      }
    }
  }
}
</style>
