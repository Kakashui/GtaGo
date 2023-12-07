<template>
  <div class="war">
    <ExitButton/>
    <div class="war__nav">
      <div class="war__nav-title">{{loc('war_1')}}</div>
      <div class="war__nav-list">
        <div 
          :class="[{active: item.page === currentPage}, 'war__nav-item']"
          v-for="item in navList"
          :key="item.page"
          @click="setCurrentPage(item.page)"
        >
          <div class="item-desc">{{loc(item.desc)}}</div>
          <div class="item-icon">
            <span :style="{backgroundImage: `url(/img/war/${item.icon}.png)`}"></span>
          </div>
        </div>
      </div>
      <div class="war__nav__info">
        <div class="war__nav__info-title">{{loc(title)}}</div>
        <div class="war__nav__info-desc">{{currentOrg}}</div>
      </div>
    </div>
    <component :is="currentPage"/>
  </div>
</template>

<script>
import ExitButton from './components/ExitButton'
import TotalStats from './components/TotalStats'
import TeamStats from './components/TeamStats'
import { mapGetters, mapState } from 'vuex'
export default {
  name: 'War',
  components: {
    ExitButton,
    TotalStats,
    TeamStats
  },
  data: function() {
    return {
      currentPage: null,
      navList: [
        {
          page: 'TotalStats',
          icon: 'total',
          desc: 'war_10'
        },
        {
          page: 'TeamStats',
          icon: 'team',
          desc: 'war_11'
        },
      ]
    }
  },
  computed: {
    ...mapState('war', ['currentOrg', 'title']),
    ...mapGetters('localization', ['loc'])
  },
  methods: {
    setCurrentPage: function(page) {
      this.currentPage = page
    }
  },

  mounted() {
    this.currentPage = this.navList[0].page
  }
}
</script>

<style lang="scss" scoped>
.war{
  display: flex;
  width: 100%;
  height: 100vh;
  position: absolute;
  left: 0;
  top: 0;
  background-size: cover;
  background-position: left bottom;
  background-repeat: no-repeat;
  background-image: url('/img/war/bg.png');
  padding: 4.5rem 13.5rem 0 13.5rem;
  text-transform: uppercase;
  color: #FFFFFF;
  &:before{
    content: '';
    width: 40.75rem;
    height: 31.3rem;
    background-size: contain;
    background-repeat: no-repeat;
    background-position: left bottom;
    background-image: url('/img/war/rifle.png');
    position: absolute;
    left: 0;
    bottom: 0;
    pointer-events: none;
    animation: showRifle .5s;
  }
  &__nav{
    display: flex;
    flex-direction: column;
    align-items: flex-end;
    position: relative;
    border-right: 1px solid rgba(255, 255, 255, 0.2);
    padding-right: 1.5rem;
    height: 100%;
    width: fit-content;
    margin-right: 3rem;
    &:before{
      content: '';
      width: .2rem;
      height: 15rem;
      background: #B6D300;
      box-shadow: 0 1rem 4.5rem rgba(182, 211, 0, 0.6);
      position: absolute;
      right: 0;
      top: 0;
    }
    &-title{
      font-weight: bold;
      font-size: 6rem;
      line-height: 6rem;
      margin-bottom: 1rem;
    }
    &-list{
      display: flex;
      flex-direction: column;
      align-items: flex-end;
      position: relative;
      padding-bottom: 2.5rem;
      margin-bottom: 3rem;
      &:after{
        content: '';
        height: 1px;
        width: 22rem;
        background: rgba(255, 255, 255, 0.2);
        position: absolute;
        right: 0;
        bottom: 0;
      }
    }
    &-item{
      display: flex;
      align-items: center;
      justify-content: center;
      margin-bottom: .1rem;
      transition: .3s;
      &:hover{
        .item-desc{
          color: rgba(255, 255, 255, 1);
        }
      }
      &.active{
        .item-desc{
          color: rgba(255, 255, 255, 1);
        }
        .item-icon{
          background: #B6D300;
          box-shadow: 0 1rem 4.5rem rgba(182, 211, 0, 0.6);
        }
      }
      .item-desc{
        color: rgba(255, 255, 255, 0.4);
        font-weight: bold;
        font-size: 1.2rem;
        line-height: 1.2rem;
        margin-right: 1rem;
        transition: .3s;
      }
      .item-icon{
        width: 4rem;
        height: 4rem;
        display: flex;
        align-items: center;
        justify-content: center;
        background: rgba(255, 255, 255, 0.1);
        transition: .3s;
        span{
          width: 1.5rem;
          height: 1.5rem;
          background-size: contain;
          background-repeat: no-repeat;
          background-position: center;
        }
      }
      &:last-child{
        margin-bottom: 0;
      }
    }
    &__info{
      display: flex;
      flex-direction: column;
      align-items: flex-end;
      &-title{
        font-weight: bold;
        font-size: 3rem;
        margin-bottom: .5rem;
      }
      &-desc{
        font-weight: bold;
        font-size: 1.2rem;
        color: #B6D300;
        text-shadow: 0 0 4rem rgba(182, 211, 0, 0.6);
      }
      &:after{
        content: '';
        width: 12.95rem;
        height: 24.25rem;
        background-size: contain;
        background-repeat: no-repeat;
        background-position: center;
        background-image: url('/img/war/portfolio.png');
        margin-right: -1.5rem;
      }
    }
  }
}
@keyframes showRifle {
  from {
    transform: translateX(-10%) translateY(100%) rotate(27deg);
  } 
  to {
    transform: translateX(0%) translateY(0%) rotate(0deg);
  }
}
</style>
