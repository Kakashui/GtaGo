<template>
  <div class="control-window">
    <div class="control-window__header">
      <div class="control-window__header-logo img-contain-style"></div>
      <Nav/>
      <div 
        class="control-window__header-worker"
      >
        <div class="control-window__header-worker-name">{{name}}</div>
        <input 
          type="text"  
          readonly
          @focus="infoWorker = false"
          @blur="infoWorker = false"
        >
        <template name="info-worker">
          <div 
            class="control-window__header-worker__info"
            v-if="infoWorker"
          >
            <div class="control-window__header-worker__info-id control-window__header-worker__info--text">id: {{currentWorker.workerId}}</div>
            <div class="control-window__header-worker__info-nickname control-window__header-worker__info--text">{{currentWorker.workerName}}</div>
            <div class="control-window__header-worker__info-post control-window__header-worker__info--text">{{getCurrentRank.rankName}}</div>
            <div class="control-window__header-worker__info-income control-window__header-worker__info--text">${{getCurrentRank.income}}</div>
            <div class="control-window__header-worker__info-edit">
              <div>{{loc('news:cntrl:1')}}</div>  
              <div>{{currentWorker.editedPerDay}}</div>
            </div>
          </div>
        </template>
      </div>
    </div>
    <div class="control-window__body">
      <BodyNav 
        :setStatusAds="setStatusAds"
        :statusAds="statusAds"
      />
      <div class="body-info">
        <div class="body-info__title">{{loc(currentTitle)}} <span> {{currentAdsCount.length}}</span></div>
        <div class="body-info__filters">
          <div 
            :class="[{ active: !isPremium }, 'body-info__filters-item']"
            @click="setPremium(false)"
          >{{loc('news:cntrl:2')}} <span>{{getSimpleArray.length}}</span></div>
          <div 
            :class="[{ active: isPremium }, 'body-info__filters-item']"
            @click="setPremium(true)"
          >{{loc('news:cntrl:3')}} <span>{{getPremiumArray.length}}</span></div>
        </div>
        <div :class="[{'body-info__ads--width-img' : isPremium}, 'body-info__ads']">
          <div class="body-info__ads-wrap">
            <AdsItem
              v-for="(item, index) in currentList"
              :key="index"
              :item="item"
            />
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapMutations, mapState } from 'vuex'
import Nav from './Nav'
import BodyNav from './BodyNav'
import AdsItem from './AdsItem'
export default {
  name: 'ControlWindow',

  components: {
    Nav,
    BodyNav,
    AdsItem
  },

  data: function() {
    return {
      statusAds: 'Created',
      isPremium: false,
      infoWorker: false
    }
  },

  computed: {
    ...mapState('news', ['adsList', 'workerId', 'ranksList', 'workersList', 'name']),    
    ...mapGetters('localization', ['loc']),
    currentAdsCount: function() {
      let array = this.adsList
      return array.filter(element => element.Status === this.statusAds)
    },
    currentList: function() {
      if (this.isPremium) {
        return this.getPremiumArray
      }
      return this.getSimpleArray
    },
    getPremiumArray: function() {
      return this.currentAdsCount.filter(element => element.PrimeAdvert)
    },
    getSimpleArray: function() {
      return this.currentAdsCount.filter(element => !element.PrimeAdvert)
    },
    currentTitle: function() {
      if (this.statusAds === 'Created') {
        return 'news:cntrl:tit:1'
      } else if (this.statusAds === 'Compleate') {
        return 'news:cntrl:tit:2'
      }
      return 'news:cntrl:tit:3'
    },
    currentWorker: function() {
      let list = this.workersList
      return list.find(element => element.workerId === this.workerId)
    },
    getCurrentRank: function(state) {
      return state.ranksList.find(element => element.rankId === this.currentWorker.workerRank)
    }
  },

  methods: {
    ...mapMutations('news', ['setCurrentAd']),
    setStatusAds: function(value) {
      if (this.statusAds !== value){
        this.statusAds = value
        this.isPremium = false
        this.setCurrentAd({})
      }
    },
    setPremium: function(value) {
      this.isPremium = value
      this.setCurrentAd({})
    }
  }
}
</script>

<style lang="scss" scoped>
.control-window{
  width: 100%;
  height: 100%;
  &__header{
    display: flex;
    align-items: center;
    height: 4rem;
    border-bottom: 1px solid rgba(51, 51, 51, 0.1);
    padding: 0 1rem 0 1.6rem;
    justify-content: flex-start;
    position: relative;
    &-logo{
      width: 5.55rem;
      height: 2.45rem;
      margin-right: 7.95rem;
      background-image: url('/img/news/logo.png');
    }
    &-worker{
      position: absolute;
      right: 1rem;
      display: flex;
      align-items: center;
      &-name{
        z-index: 0;
        font-weight: 600;
        font-size: .9rem;
        color: #46555F;
        padding-right: 1.5rem;
      }
      input{
        width: 100%;
        height: 100%;
        position: absolute;
        left: 0;
        top: 0;
        background-color: transparent;
        outline: none;
        z-index: 1;
      }
      &:after{
        content: '';
        position: absolute;
        right: 0;
        width: 1rem;
        height: 1rem;
        background-size: contain;
        background-repeat: no-repeat;
        background-position: center;
        background-image: url('/img/news/icon-drop-arr.svg');
        transform-origin: 50% 50%;
        z-index: 0;
      }
      &__info{
        position: absolute;
        right: 0;
        bottom: -.5rem;
        transform: translateY(100%);
        background: #FFFFFF;
        box-shadow: 0 0 .5rem rgba(0, 0, 0, 0.08);
        border-radius: .5rem;
        padding: 1rem 1rem 3.5rem 1rem;
        width: 15rem;
        display: flex;
        flex-flow: column;
        align-items: center;
        font-weight: 600;
        color: #46555F;
        line-height: .9rem;
        font-size: .8rem;
        text-align: center;
        z-index: 2;
        &--text{
          margin-bottom: .2rem;
        }
        &-id{
          color: rgba(0, 0, 0, 0.5);
          text-transform: uppercase;
        }
        &-nickname{
          font-size: 1rem;
          line-height: 1.15rem;
        }
        &-post{
          max-width: 8rem;
        }
        &-income{
          color: #E30000;
        }
        &-edit{
          display: flex;
          align-items: center;
          justify-content: space-between;
          width: 100%;
          height: 2.7rem;
          padding: 0 1rem;
          position: absolute;
          left: 0;
          bottom: 0;
          border-top: 1px solid rgba(0, 0, 0, 0.1);
        }
      }
    }
  }
  &__body{
    display: flex;
    width: 100%;
    height: calc(100% - (4rem));
    .body-info{
      width: 100%;
      padding: 1.5rem 1.4rem 0 1.4rem;
      &__title{
        font-weight: bold;
        font-size: 1.5rem;
        line-height: 1.7rem;
        margin-bottom: 1rem;
        color: #46555F;
        white-space: pre;
        span{
          color: #AFB9C5;
        }
      }
      &__filters{
        display: flex;
        margin-bottom: 1rem;
        height: 2.1rem;
        &-item{
          height: 100%;
          padding: 0 .8rem;
          display: flex;
          align-items: center;
          justify-content: center;
          background-color: transparent;
          border-radius: .3rem;
          color: #46555F;
          transition: .2s;
          margin-right: .2rem;
          white-space: pre;
          font-weight: 500;
          font-size: .8rem;
          line-height: .9rem;
          &:last-child{
            margin-right: 0;
          }
          &:hover, &.active{
            background-color: #DFE3E8;
            color: #D92C2C;
          }
          span{
            font-weight: bold;
          }
        }
      }
      &__ads{
        height: calc(100% - (5.75rem));
        padding-bottom: 1.4rem;
        overflow-y: auto;
        &::-webkit-scrollbar{
          background-color: transparent;
          width: 0;
        }
        &-wrap{
          display: grid;
          grid-template-columns: repeat(3, 1fr);
          grid-auto-rows: 13.55rem;
          grid-gap: .8rem;
        }
        &--width-img{
          .body-info__ads-wrap{
            grid-auto-rows: 18.3rem;
          }
        }
      }
    }
  }
  .info-worker-enter-active, .info-worker-leave-active {
    transition: opacity .5s;
  }
  .info-worker-enter, .info-worker-leave-to /* .info-worker-leave-active below version 2.1.8 */ {
    opacity: 0;
  }
}
</style>
