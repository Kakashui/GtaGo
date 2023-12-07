<template>
  <div class="stats-page">
    <div 
      class="stats-page__btn-back"
      @click="setCurrentPage('MainPage')"
    >{{loc('news:stats:1')}}</div>
    <div class="stats-page__col stats-page__col--first">
      <div class="stats-page-title">{{loc('news:stats:2')}}</div>
      <div class="stats-page__col-title">{{loc('news:stats:3')}}:</div>
      <div class="stats-page__col__list-top">
        <TopWorker
          v-for="(worker, index) in topFiveWorkers"
          :key="index"
          :worker="worker"
          :index="index"
        />
      </div>
    </div>
    <div class="stats-page__col">
      <div class="stats-page__col-header">
        <div class="stats-page__col-title">{{loc('news:stats:4')}}</div>
        <div class="stats-page__col-header__param">
          <div class="stats-page__col-header__param-value">{{currentOnline}}</div>
          <div class="stats-page__col-header__param-desc">{{loc('news:stats:5')}}</div>
        </div>
      </div>
      <div class="stats-page__worker-place">{{loc('news:stats:6')}} {{isYourRating}}</div>
      <div class="stats-page__col-list">
        <div class="stats-page__col-list__header">
          <div>{{loc('news:stats:7')}}</div>
          <div>{{loc('news:stats:8')}}</div>
          <div>{{loc('news:stats:9')}}</div>
          <div>{{loc('news:stats:10')}}</div>
        </div>
        <div class="stats-page__col-list__body">
          <RatingItem 
            v-for="(item, index) in workersList"
            :key="item.workerId"
            :item="item"
            :index="index"
          />
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapMutations, mapState } from 'vuex'
import TopWorker from './TopWorker'
import RatingItem from './RatingItem'
export default {
  name: 'StatsPage',

  components: {
    TopWorker,
    RatingItem
  },  
  
  computed: {
    ...mapState('news', ['workersList', 'workerId']),
    topFiveWorkers: function() {
      let list = this.workersList.slice()
      let sortList = list.sort((a, b) => b.editedPerDay - a.editedPerDay).slice(0, 5);
      return sortList
    },
    currentOnline: function() {
      let list = this.workersList
      return list.filter(element => element.isOnline).length
    },
    ratingList: function() {
      let list = this.workersList.slice()
      return list.sort((a, b) => b.rating - a.rating)
    },
    isYourRating: function() {
      let list = this.workersList
      let ratingList = list.sort((a, b) => b.rating - a.rating)
      let currentWorker = ratingList.find(element => element.workerId === this.workerId) 
      if (ratingList.indexOf(currentWorker)) {
        return ratingList.indexOf(currentWorker) + 1
      }
      return '0'
    }
  },

  methods: {
    ...mapMutations('news', ['setCurrentPage'])
  }
}
</script>

<style lang="scss" scoped>
.stats-page{
  padding: 3rem 2rem 0 2rem;
  position: relative;
  width: 100%;
  height: 100%;
  &:before{
    content: '';
    width: 18.5rem;
    height: 18.5rem;
    position: absolute;
    left: -5rem;
    top: -1.5rem;
    background-size: contain;
    background-repeat: no-repeat;
    background-position: center;
    background-image: url('/img/news/stats-decorate.svg');
    z-index: 0;
  }
  &>div{
    z-index: 1;
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
    left: 2rem;
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
    margin-top: 1.2rem;
    font-size: 4rem;
    line-height: 4.6rem;
    text-transform: uppercase;
    color: #212121;
    max-width: 17.5rem;
    position: relative;
    margin-bottom: 9.6rem;
    &:after{
      content: '';
      position: absolute;
      left: 0;
      bottom: 0;
      width: 5.3rem;
      height: .2rem;
      background: #D92C2C;
    }
  }
  &__col{
    height: 100%;
    margin-right: 1.6rem;
    display: flex;
    flex-direction: column;
    justify-content: space-between;
    &--first{
      width: 25rem;
      min-width: 25rem;
      padding-bottom: .75rem;
    }
    &:last-child{
      margin-right: 0;
      width: 100%;
    }
    &-title{
      font-weight: normal;
      font-size: 2rem;
      line-height: 2.3rem;
      color: #212121;
      white-space: pre;
      margin-bottom: .4rem;
    }
    &__list-top{
      display: flex;
      flex-direction: column;
      justify-content: space-between;
      font-weight: normal;
      font-size: .9rem;
      line-height: 1.05rem;
      padding: 1.4rem;
      height: 100%;
      background: #FFFFFF;
      border-radius: .3rem;
      color: #000000;
    }
    &-header{
      display: flex;
      align-items: center;
      justify-content: space-between;
      padding-bottom: 1rem;
      margin-bottom: .7rem;
      border-bottom: 1px solid rgba(70, 85, 95, 0.2);
      &__param{
        display: flex;
        align-items: flex-end;
        &-value{
          font-weight: normal;
          font-size: 2.5rem;
          color: #D92C2C;
          margin-right: .5rem;
        }
        &-desc{
          font-weight: normal;
          font-size: 1rem;
          line-height: 1rem;
          color: #212121;
          margin-bottom: .5rem;
        }
      }
    }
    &-list{
      &__header{
        width: 100%;
        display: flex;
        align-items: flex-end;
        justify-content: space-between;
        margin-bottom: 1rem;
        padding-left: 3.65rem;
        padding-right: 1.8rem;
        color: rgba(70, 85, 95, 0.5);
        position: relative;
        &:before{
          content: '№';
          position: absolute;
          left: 1.8rem;
        }
        &>div{
          &:nth-child(1){
            width: 17.5rem;
            min-width: 17.5rem;
          }
          &:nth-child(2){
            width: 4.2rem;
            min-width: 4.2rem;
            margin-right: 1rem;
          }
          &:nth-child(3){
            width: 4.2rem;
            min-width: 4.2rem;
            margin-right: 1rem;
          }
          &:nth-child(4){
            width: 3.8rem;
            min-width: 3.8rem;
            margin-right: 0;
          }
        }
      }
      &__body{
        overflow-y: auto;
        max-height: 33.05rem;
        &::-webkit-scrollbar{
          width: 0;
          background-color: transparent;
        }
      }
    }
  }
}
</style>
