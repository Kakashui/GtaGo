<template>
  <div class="full-stat">
    <div class="full-stat__vote" v-if="isVote">
      <div
        class="vote__item"
        v-for="(item, index) in voteItems"
        :key="index"
        :style="{ 'background': `linear-gradient(to bottom, rgba(0, 0, 0, .5), rgba(0, 0, 0, .5)), url(${setBackground(item.title)}) center / cover no-repeat` }"
        @click="vote(item.title)"
      >
        <div class="item__count">
          <span class="count__current">{{ item.currentCount }}</span>
          <span class="count__separator">/</span>
          <span class="count__max">{{ item.maxCount }}</span>
        </div>
      </div>
    </div>
    <div v-if="isFullKillstat" class="header__container">
      <span v-if="killstatType === 'global_rating'" class="header__title">{{ loc('arena_dm_29') }}</span>
      <span v-else-if="killstatType === 'match_result'" class="header__title">{{ loc('arena_dm_30') }}</span>
      <span v-else class="header__title">{{ loc('arena_dm_31') }}</span>
      <span class="header__exit">{{ loc('arena_dm_35') }}</span>
    </div>
    
    <div class="full-stat__header" v-if="killstatType === 'global_rating' && isFullKillstat">
      <span class="header__username">Nickname</span>
      <span class="header__kills">Place</span>
      <span class="header__deaths">Points</span>
    </div>
    <div class="full-stat__header" v-else-if="isFullKillstat">
      <span class="header__username">Nickname</span>
      <span class="header__kills">Kills</span>
      <span class="header__deaths">Deaths</span>
      <span class="header__kd">Ratio</span>
    </div>
    
    <div class="full-stat__list" v-if="killstatType === 'global_rating' && isFullKillstat">
      <div
          class="list__item"
          v-for="item in currentItems"
          :key="item.id"
      >
        <span class="item__username">{{ item.username }}</span>
        <span class="item__deaths">{{ item.place }}</span>
        <span class="item__kills">{{ item.points }}</span>
      </div>
    </div>
    <div class="full-stat__list" v-else-if="isFullKillstat">
      <div
        class="list__item"
        v-for="item in currentItems"
        :key="item.id"
      >
        <span class="item__username">{{ item.username }}</span>
        <span class="item__kills">{{ item.kills }}</span>
        <span class="item__deaths">{{ item.deaths }}</span>
        <span class="item__kd">{{ receiveKD(item.kills, item.deaths) }}</span>
      </div>
    </div>
  </div>
</template>

<script>
import { mapState, mapGetters } from 'vuex'

export default {
  name: 'FullKillStat',

  computed: {
    ...mapState('hud', [
      'killstatItems',
      'killstatType',
      'voteItems',
      'isVote',
      'isFullKillstat'
    ]),
    ...mapGetters('localization', ['loc']),
    ...mapState('arenaMenu', ['backgrounds']),

    currentItems: function () {
      return this.sortByKills(this.killstatItems)
    }
  },

  methods: {
    sortByKills: function (array) {
      return array.sort(this.sortFunc)
    },

    sortFunc: function (a, b) {
      if (b.kills === a.kills) {
        return a.deaths - b.deaths
      } else {
        return b.kills - a.kills
      }
    },

    receiveKD: function (kills, deaths) {
      if (kills > 0 && deaths === 0) {
        return kills
      } else if (kills === 0 || deaths === 0) {
        return null
      } else {
        return (kills / deaths).toFixed(1)
      }
      
    },

    setBackground: function (title) {
      const rightIndex = this.backgrounds.findIndex(item => item.title === title)
      return this.backgrounds[rightIndex].image
    },

    vote: function (title) {
      //console.log('TRIGGER SOME EVENT!')
      window.mp.trigger('ARENA::CHOOSE::MAP::NAME::VUE', title)
    }
  }
}
</script>

<style lang="scss" scoped>
.full-stat {
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  display: flex;
  flex-direction: column;
  width: 54rem;
  z-index: 1;
  &__vote {
    display: flex;
    justify-content: space-between;
    .vote__item {
      width: 17rem;
      height: 10rem;
      display: flex;
      align-items: center;
      justify-content: center;
      border-top: .2rem solid #1fbf75;
      box-shadow: inset 0 -4px 10px #1fbf75;
      transition: all .15s;
      &:hover {
        transform: translateY(-.2rem) scale(1.01);
        box-shadow: inset 0 -6px 18px #1fbf75;
      }
      .item__count {
        font-size: 3.4rem;
        color: #fff;
        display: flex;
        .count__current {
          color: #1fbf75;
        }
      }
    }
  }
  &__header {
    box-shadow: 0px 5px 7px 0 rgba(31, 191, 117, 0.5);
    background-color: #1fbf75;
    color: #fff;
    letter-spacing: 0.05rem;
    text-shadow: 0 0 2px rgba(255, 255, 255, .8);
    height: 1.8rem;
    display: flex;
    align-items: center;
    justify-content: flex-end;
    position: relative;
    margin: 1rem 0 0 0;
    &::after {
      content: '';
      position: absolute;
      left: 100%;
      top: 0;
      width: 0;
      height: 0;
      border-right: 1rem solid transparent;
      border-top: 1.8rem solid #1fbf75;
    }
    &::before {
      content: '';
      position: absolute;
      right: 100%;
      top: 0;
      width: 0;
      height: 0;
      border-left: 1rem solid transparent;
      border-top: 1.8rem solid #1fbf75;
    }
    .header__username {
      margin: 0 auto 0 2.1rem;
    }
    .header__kills {
      margin: 0 2.75rem 0 0;
    }
    .header__deaths {
      margin: 0 2.75rem 0 0;
    }
    .header__kd {
      margin: 0 3.75rem 0 0;
    }
  }
  .header__container {
    
  }
  .header__exit {
    text-transform: uppercase;
    color:#F1F8FF;
    //vertical-align: bottom;
    float: right;
    font-size: 1.5rem;
  }
  .header__title {
    text-transform: uppercase;
    color:#F1F8FF;
    line-height: .9rem;
    font-size: 3.0rem;
  }
  
  &__list {
    display: flex;
    flex-direction: column;
    height: 26rem;
    overflow-y: auto;
    margin: .4rem 0 0 0;
    transform: scaleX(-1);
    padding: 0 .2rem 0 0;
    &::-webkit-scrollbar-track {
      background-color: #000;
    }
    &::-webkit-scrollbar {
      width: .2rem;
      background-color: transparent;
    }
    &::-webkit-scrollbar-thumb {
      background-color: #1fbf75;
      height: 33.3%;
    }
    .list__item {
      transform: scaleX(-1);
      background-image: url('../../../public/img/hudKillStat/full-stat-item-bg.png');
      background-size: contain;
      background-repeat: no-repeat;
      min-height: 3.2rem;
      display: flex;
      align-items: center;
      justify-content: flex-end;
      color: #1fbf75;
      margin: 0 0 .4rem 0;
      font-weight: bold;
      letter-spacing: .05rem;
      &:last-child {
        margin: 0;
      }
      .item__username {
        color: #fff;
        margin: 0 auto 0 1.6rem;
        font-weight: normal;
      }
      .item__kills {
        width: 4.7rem;
        margin: 0 .2rem 0 0;
      }
      .item__deaths {
        width: 3.8rem;
        margin: 0 2.7rem 0 0;
      }
      .item__kd {
        width: 1.65rem;
        margin: 0 4.2rem 0 0;
      }
    }
  }
}
</style>
