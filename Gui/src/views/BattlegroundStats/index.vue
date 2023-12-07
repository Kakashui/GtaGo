<template>
  <div class="battleground-stats">
    <div class="battleground-stats__decorate">{{loc('BattleStats_0')}}</div>
    <div class="battleground-stats__title">{{loc('BattleStats_0')}}</div>
    <div class="battleground-stats__nav">
      <div class="nav-wrap">
        <div 
          :class="[{active: currentNav === 1}, 'nav-wrap__item']"
          @click="setCurrentNav(1)"
        >{{loc('BattleStats_1')}}</div>
        <div 
          :class="[{active: currentNav === 0}, 'nav-wrap__item']"
          @click="setCurrentNav(0)"
        >{{loc('BattleStats_2')}}</div>
      </div>
      <div class="search-wrap">
        <input 
          type="text" 
          :placeholder="loc('BattleStats_3')"
          v-model="currentFilter"
        >
        <div 
          class="btn-search"
          @click="setSearch"
        ></div>
      </div>
    </div>
    <div class="battleground-stats__table">
      <div class="table-header">
        <div>№</div>
        <div>{{loc('BattleStats_4')}}</div>
        <div>{{loc('BattleStats_5')}}</div>
      </div>
      <div class="table-list">
        <div 
          :class="[{first: item.place === 1}, {second: item.place === 2}, {third: item.place === 3}, 'table-list__item']"
          v-for="(item, index) in currentList"
          :key="index"
        >
          <div>{{item.place}}</div>
          <div>{{item.nickname}}</div>
          <div>{{item.kills}}</div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
export default {
  name: 'BattlegroundStats',

  data: function() {
    return {
      currentNav: 1, // 1 - current, 0 - all time
      isSearchList: false,
      currentFilter: null
    }
  },

  methods: {
    setCurrentNav: function(value) {
      this.currentNav = value
      window.mp.trigger('battlegroundStats:setCurrentNav', value)
    },
    setSearch: function() {
      window.mp.trigger('battlegroundStats:setSearch', this.currentFilter)
      this.isSearchList = true;
    }
  },

  computed: {
    ...mapState('battlegroundStats', ['currentMatch', 'allTime', 'listSearch']),
    ...mapGetters('localization', ['loc']),
    currentList: function() {
      let array = null
      if (this.currentNav) {
        array = this.currentMatch
      } else if (this.isSearchList) {
        array = this.listSearch
      } else {
        array = this.allTime
      }
        return array
    }
  },
  watch: {
    currentFilter: function(value) {
      if (!value) {
        this.isSearchList = false
      }
    }
  }
}
</script>

<style lang="scss" scoped>
@keyframes show{
  from {
    transform: translateY(-100%);
    opacity: 0;
  }
  to {
    transform: translateY(0%);
    opacity: 1;
  }
}
.battleground-stats{
  width: 100%;
  height: 100vh;
  background-color: #1D232C;
  background-image: url('/img/battlegroundStats/bg.png');
  background-size: cover;
  display: flex;
  flex-flow: column;
  align-items: center;
  justify-content: flex-start;
  text-transform: uppercase;
  padding-top: 6rem;
  &>div{
    width: 35rem;
    z-index: 2;
    &.battleground-stats__decorate{
      width: 100%;
      z-index: 1;
    }
  }
  &__decorate{
    background: linear-gradient(180deg, rgba(255, 255, 255, .08) 0%, rgba(255, 255, 255, 0) 100%);
    -webkit-background-clip: text;
    background-clip: text;
    -webkit-text-fill-color: transparent;
    font-style: normal;
    font-weight: bold;
    font-size: 18rem;
    line-height: 100%;
    letter-spacing: 0.115em;
    text-transform: uppercase;
    text-align: center;
    position: absolute;
    top: 1.6rem;
    animation: show .5s;
  }
  &__title{
    font-style: normal;
    font-weight: bold;
    font-size: 4.4rem;
    line-height: 100%;
    letter-spacing: 0.08em;
    text-transform: uppercase;
    color: #FFFFFF;
    text-shadow: 0 .2rem 2.5rem rgba(255, 255, 255, 0.5);
    margin-bottom: 1.5rem;
    animation: show .5s;
  }
  &__nav{
    display: flex;
    align-items: flex-end;
    justify-content: space-between;
    margin-bottom: 1.5rem;
    .nav-wrap, .search-wrap{
      border-bottom: 1px solid rgba(255, 255, 255, 0.2);
    }
    .nav-wrap{
      display: flex;
      align-items: center;
      width: 100%;
      justify-content: flex-start;
      &__item{
        font-weight: 600;
        font-size: .8rem;
        letter-spacing: 0.12em;
        text-transform: uppercase;
        color: rgba(255, 255, 255, 0.5);
        padding: .5rem 1.6rem 1rem 1.6rem;
        border-bottom: .2rem solid transparent;
        transition: .3s;
        &:hover{
          color: #fff;
          transition: .3s;
        }
        &.active{
          color: #fff;
          border-color: #B6D300;
        }
      }
    }
    .search-wrap{
      position: relative;
      display: flex;
      width: 12rem;
      min-width: 12rem;
      align-items: center;
      background: #282E37;
      border: 1px solid rgba(255, 255, 255, 0.2);
      height: 3rem;
      input{
        background: transparent;
        outline: none;
        width: 100%;
        height: 100%;
        padding-left: 1rem;
        font-size: .9rem;
        letter-spacing: 0.03em;
        color: rgba(255, 255, 255, 0.8);
        text-transform: uppercase;
        &::placeholder{
          color: rgba(255, 255, 255, 0.5);
        }
      }
      .btn-search{
        width: 2rem;
        height: 2rem;
        position: absolute;
        right: .5rem;
        background-size: contain;
        background-repeat: no-repeat;
        background-position: center;
        background-image: url('/img/battlegroundStats/search.svg');
        background-color: rgba(255, 255, 255, 0.14);
        transition: .3s;
        &:hover{
          background-color: #B6D300;
          transition: .3s;
        }
      }
    }
  }
  &__table{
    display: flex;
    flex-flow: column;
    .table-header, .table-list__item{
      display: flex;
      align-items: center;
      width: 100%;
      justify-content: space-between;
      padding: 0 2.7rem 0 .55rem;
      &>div{
        width: 100%;
        &:nth-child(1){
          width: 2.5rem;
          min-width: 2.5rem;
          display: flex;
          align-items: center;
          justify-content: center;
          margin-right: 1.5rem;
        }
        &:nth-child(2){
          text-align: left;
        }
        &:last-child{
          text-align: right;
        }
      }
    }
    .table-header{
      font-style: normal;
      font-weight: 600;
      font-size: .8rem;
      line-height: 130%;
      color: rgba(255, 255, 255, 0.5);
      margin-bottom: .75rem;
    }
    .table-list{
      overflow-y: auto;
      max-height: 59vh;
      width: calc(100% + (1.05rem));
      margin-right: -1.05rem;
      padding-right: 1.05rem;
      &::-webkit-scrollbar{
        width: .3rem;
        background-color: transparent;
        &-thumb{
          background: rgba(255, 255, 255, 0.2);
        }
      }
      &__item{
        color: rgba(255, 255, 255, .9);
        height: 4rem;
        min-height: 4rem;
        font-weight: 600;
        font-size: 1.1rem;
        min-width: 35rem;
        position: relative;
        div{
          &:last-child{
            position: relative;
            padding-right: 1.65rem;
            display: flex;
            align-items: center;
            text-align: right;
            justify-content: flex-end;
            &:after{
              content: '';
              width: 1.2rem;
              min-width: 1.2rem;
              height: 1.2rem;
              background-size: contain;
              background-position: center;
              background-repeat: no-repeat;
              background-image: url('/img/battlegroundStats/kill.svg');
              position: absolute;
              right: 0;
            }
          }
        }
        &:nth-child(2n){
          background: rgba(255, 255, 255, 0.06);
        }
        &:nth-child(2n+1){
          background: rgba(255, 255, 255, 0.1);
        }
        &.first, &.second, &.third{
          &:before{
            content: '';
            width: 2.5rem;
            height: 2.5rem;
            background-size: contain;
            background-repeat: no-repeat;
            background-position: center;
            position: absolute;
            left: .55rem;
          }
          div{
            &:nth-child(1){
              opacity: 0;
            }
          }
        }
        &:nth-child(1), &:nth-child(3){
          background: #343941;
        }
        &:nth-child(2){
          background: #2B3039;
        }
        &.first{
          &:before{
            background-image: url('/img/battlegroundStats/first.png');
          }
        }
        &.second{
          &:before{
            background-image: url('/img/battlegroundStats/second.png');
          }
        }
        &.third{
          &:before{
            background-image: url('/img/battlegroundStats/third.png');
          }
        }
      }
    }
  }
}
</style>
