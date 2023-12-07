<template>
  <div class="menu-nav">
    <Head :titleText="loc('home:menu:head')"/>
    <div class="house-info">
      <div class="house-info__number">№{{houseId}}</div>
      <div :class="[{locked: houseLocked}, 'house-info__status']"></div>
    </div>
    <div 
      class="house-type"
      v-if="houseType === 0"
    >{{loc('HomeMenu_21')}}</div>
    <div 
      class="house-type"
      v-else
    >{{loc('HomeMenu_22')}}</div>
    <div class="nav-list">
      <div 
        :class="[{'item-lock' : !houseLocked}, {'item-unlock' : houseLocked}, 'nav-list__item']"
        @click="toggleHouseLocked(!houseLocked)"
      >
        <div 
          class="desc"
          v-if="houseLocked"
        >{{loc('HomeMenu_23')}}</div>
        <div 
          class="desc"
          v-else
        >{{loc('HomeMenu_24')}}</div>
        <div class="title">{{loc('HomeMenu_25')}}</div>
        <div class="img" style="background-image: url(/img/homeMenu/lock.png)"></div>
      </div>
      <div class="nav-list__item" @click="setCurrentPage('GaragePage')">
        <div class="desc">{{loc('HomeMenu_26')}}</div>
        <div class="title">{{loc('HomeMenu_27')}}</div>
        <div class="img" style="background-image: url(/img/homeMenu/garage.png)"></div>
      </div>
      <div class="nav-list__item" @click="sellHouse">
        <div class="desc">{{loc('HomeMenu_28')}}</div>
        <div class="title">{{loc('HomeMenu_25')}}</div>
        <div class="cost">{{houseCost}}</div>
        <div class="img" style="background-image: url(/img/homeMenu/sell-house.png)"></div>
      </div>
      <div class="nav-list__item" @click="setCurrentPage('FurniturePage')">
        <div class="desc">{{loc('HomeMenu_29')}}</div>
        <div class="title">{{loc('HomeMenu_30')}}</div>
        <div class="img" style="background-image: url(/img/homeMenu/interior.png)"></div>
      </div>
      <div class="nav-list__item item-colored" @click="setCurrentPage('ResidentsPage')">
        <div class="desc">{{loc('HomeMenu_31')}}</div>
        <div class="title">{{occupiers.length}}</div>
      </div>
      <div class="nav-list__item item-colored">
        <div class="desc">{{loc('HomeMenu_32')}} <span>{{loc('HomeMenu_33')}} {{paidBefore}}</span></div>
        <div class="title">{{loc('HomeMenu_34')}}</div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapMutations, mapState, mapGetters } from 'vuex'
import Head from './Head'

export default {
  name: 'MenuNav',

  components: {
    Head
  },

  computed: {
    ...mapState('homeMenu', ['houseLocked', 'houseType', 'houseCost', 'houseId', 'occupiers', 'paidBefore']),
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    ...mapMutations('homeMenu', ['setCurrentPage', 'setHouseLocked']),
    sellHouse: function() {
      window.mp.trigger('homeMenu:sellHouse', this.houseId)
    },
    toggleHouseLocked: function(toggle) {
      this.locked = toggle
      //this.setHouseLocked()
      window.mp.trigger('homeMenu:toggleHouseLocked', this.houseId, toggle)
    }
  }
}
</script>

<style lang="scss" scoped>
.menu-nav{
  display: flex;
  flex-flow: column;
  height: 100%;
  position: relative;
  width: 28.55rem;
  min-width: 28.55rem;
  padding: 2.85rem 2.85rem 2.45rem 4.05rem; 
  &:after{
    content: '';
    background: radial-gradient(50% 50% at 50% 50%, rgba(255, 255, 255, 0.65) 0%, rgba(255, 255, 255, 0) 100%);
    height: 100%;
    width: 1px;
    position: absolute;
    right: 0;
    top:0;
  }
  .head{
    font-size: 1.2rem;
    line-height: 1.45rem;
    letter-spacing: 0.65em;
    padding-bottom: .8rem;
    position: relative;
    &:before{
      content: '';
      width: 3.25rem;
      height: .2rem;
      background: #C1E704;
      box-shadow: 0 .2rem 1.1rem rgba(193, 231, 4, 0.65);
      position: absolute;
      bottom: 0;
      left: 0;
    }
  }
  .house-info{
    display: flex;
    align-items: center;
    margin: 2.2rem 0 .3rem 0;
    &__number{
      font-size: 3.6rem;
      line-height: 4.3rem;
      margin-right: 1.05rem;
    }
    &__status{
      width: 1.55rem;
      height: 2.2rem;
      background-size: contain;
      background-repeat: no-repeat;
      background-position: center;
      background-image: url('/img/homeMenu/unlocked.svg');
      &.locked{
        background-image: url('/img/homeMenu/locked.svg');
      }
    }
  }
  .house-type{
    font-size: 1.2rem;
    line-height: 1.45rem;
    color: rgba(255, 255, 255, 0.45);
  }
  .nav-list{
    margin-top: 1.95rem;
    display: grid;
    grid-template-columns: repeat(2, 1fr);
    grid-auto-rows: 9.8rem;
    grid-template-rows: 9.8rem 16.5rem 6.5rem;
    grid-row-gap: 2.2rem;
    grid-column-gap: 2rem;
    &__item{
      display: flex;
      flex-flow: column;
      justify-content: flex-start;
      align-items: flex-start;
      padding: 1.6rem 1.45rem;
      background: rgba(255, 255, 255, 0.05);
      position: relative;
      transition: .2s;
      &>div{
        z-index: 3;
      }
      &.item-lock{
        &:before{
          background: radial-gradient(66.58% 66.58% at 50% 26.79%, rgba(255, 64, 64, 0.15) 0%, rgba(255, 64, 64, 0) 100%), rgba(255, 255, 255, 0.05);
        }
        &:after{ 
          background: radial-gradient(66.58% 66.58% at 50% 26.79%, rgba(255, 64, 64, 0.45) 0%, rgba(255, 64, 64, 0) 100%), rgba(255, 255, 255, 0.05);
        }
      }
      &.item-unlock{
        &:before{
          background: radial-gradient(66.58% 66.58% at 50% 26.79%, rgba(193, 231, 4, 0.15) 0%, rgba(193, 231, 4, 0) 100%), rgba(255, 255, 255, 0.05);
        }
        &:after{ 
          background: radial-gradient(66.58% 66.58% at 50% 26.79%, rgba(193, 231, 4, 0.35) 0%, rgba(193, 231, 4, 0) 100%), rgba(255, 255, 255, 0.05);
        }
      }
      &.item-colored{
        .title{
          color: #C1E704;
        }
      }
      .desc{
        font-size: 1.05rem;
        line-height: 1.25rem;
        white-space: pre;
      }
      .title{
        font-size: 2.05rem;
        line-height: 2.45rem;
      }
      .cost{
        margin-top: 1.15rem;
        display: flex;
        align-items: center;
        padding-left: 1.4rem;
        font-size: 1.05rem;
        line-height: 1.25rem;
        letter-spacing: 0.05em;
        color: #C1E704;
        position: relative;
        &:before{
          content: '';
          width: 1rem;
          height: 1rem;
          background-size: contain;
          background-repeat: no-repeat;
          background-position: center;
          background-image: url('/img/homeMenu/cost-icon.svg');
          position: absolute;
          left: 0;
        }
      }
      .img{
        background-size: cover;
        background-repeat: no-repeat;
        background-position: bottom;
        position: absolute;
        height: 100%;
        width: 100%;
        bottom: 0;
        right: 0;
        z-index: 2;
      }
      &:before, &:after{
        content: '';
        width: 100%;
        height: 100%;
        position: absolute;
        border: 1px solid;
        border-image-slice: 1;
        top: 0;
        left: 0;
        z-index: 1;
        box-sizing: border-box;
        transition: .2s;
      }
      &:before{
        border-image-source: linear-gradient(to bottom, rgba(255,255,255, .15), rgba(255,255,255,0));
        opacity: 1;
      }
      &:after{ 
        background: radial-gradient(75.92% 75.92% at 50% 12.89%, rgba(255, 255, 255, 0.25) 0%, rgba(255, 255, 255, 0) 100%), rgba(255, 255, 255, 0.05);
        border-image-source: linear-gradient(to bottom, rgba(255,255,255, .65), rgba(255,255,255,0));
        opacity: 0;
      }
      &:hover{
        &:before, &:after{
          transition: .2s;
        }
        &:before{
          opacity: 0;
        }
        &:after{
          opacity: 1;
        }
      }
    }
  }
}
</style>
