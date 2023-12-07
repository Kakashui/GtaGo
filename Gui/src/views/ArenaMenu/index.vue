<template>
  <div class="arena-menu">
    <div class="arena-menu-body">
      <attachment-modal v-if="isAttachment" />
      <transition appear name="arena-header">
        <div class="arena-menu__header">
          <img class="header__logo" src="/img/arenaMenu/Gostrike.png" alt="exit">
          <div class="header__separator"></div>
          <div :class="[{ active: currentTab === 'MainTab' }, 'header__btn']" @click="setTab('MainTab')">
            <span>{{ loc('arena_dm_1') }}</span>
          </div>
          <div class="header__btn" @click="setAttachment(true)">{{ loc('arena_dm_3') }}</div>
          <div class="header__btn" @click="showRating">{{ loc('arena_dm_29') }}</div>
          <button v-if="currentTab === 'MainTab'" class="header__exit" @click="closeArena">
            <span>{{loc('arena:exit')}}</span>
            <img class="exit__icon" src="/img/arenaMenu/arena-close.svg" alt="exit">
          </button>
          <button v-if="currentTab === 'LobbieTab'" class="header__exit" @click="setLobbieLeave(true)">
            <span>{{loc('arena:lobby:exit')}}</span>
            <img class="exit__icon" src="/img/arenaMenu/arena-close.svg" alt="leave">
          </button> 
        </div>
      </transition>
      <component :is="currentTab" />
      <div class="arena-menu__filters" v-show="currentTab === 'MainTab'">
        <span class="filters__header">{{ loc('arena_dm_4') }}</span>
        <span class="filters__desc">{{ loc('arena_dm_33') }}</span>
        <span class="filters__secdesc">{{ loc('arena_dm_34') }}</span>
        <div class="filters__main" v-if="filters[4].currentValue === 'Gun game'">
          <filters-item
            v-for="item in filtersWithoutRounds"
            :key="item.id"
            :item="item"
            :currentMode="filters[4].currentValue"
            :currentPlayers="filters[1].currentValue"
          />
          <button class="main__btn" @click="addLobbie">{{ loc('arena_dm_2') }}</button>
        </div>
        <div class="filters__main" v-else-if="filters[4].currentValue === 'Death match'">
          <filters-item
              v-for="item in filtersWithoutOnlyRounds"
              :key="item.id"
              :item="item"
              :currentMode="filters[4].currentValue"
              :currentPlayers="filters[1].currentValue"
          />
          <button class="main__btn" @click="addLobbie">{{ loc('arena_dm_2') }}</button>
        </div>
        <div class="filters__main" v-else>
          <filters-item
            v-for="item in filters"
            :key="item.id"
            :item="item"
            :currentMode="filters[4].currentValue"
            :currentPlayers="filters[1].currentValue"
          />
          <button class="main__btn" @click="addLobbie">{{ loc('arena_dm_2') }}</button>
        </div>
      </div>
    </div>    
  </div>
</template>

<script>
import { mapState, mapGetters, mapMutations } from 'vuex'

import MainTab from './components/MainTab'
import LobbieTab from './components/LobbieTab'
import FiltersItem from './components/FiltersItem'
import AttachmentModal from './components/AttachmentModal'

export default {
  name: 'ArenaMenu',

  components: {
    MainTab,
    LobbieTab,
    FiltersItem,
    AttachmentModal
  },

  computed: {
    ...mapState('arenaMenu', [
      'modes',
      'lobbies',
      'filters',
      'currentTab',
      'isCreate',
      'isAttachment'
    ]),

    ...mapGetters('localization', ['loc']),

    filtersWithoutRounds: function () {
      return this.filters.filter(item => item.id !== 5 && item.id !== 3)
    },
    filtersWithoutOnlyRounds: function () {
      return this.filters.filter(item => item.id !== 5)
    },
  },

  methods: {
    ...mapMutations('arenaMenu', [
      'setCurrentTab',
      'createLobbiesItem',
      'setCurrentLobbieId',
      'setIsCreate',
      'setIsAttachment',
      'setIsLobbieLeave'
    ]),

    ...mapMutations('sounds', ['play']),

    setLobbieLeave: function (value) {
      this.setIsLobbieLeave(value)
      this.play({ name: 'choice', volume: 0.1 })
    },

    showRating: function () {
      window.mp.trigger('arena:showRating')
    },
    setAttachment: function (value) {
      this.play({ name: 'choice', volume: 0.1 })
      this.setIsAttachment(value)
    },

    setTab: function (value) {
      this.play({ name: 'choice2', volume: 0.1 })
      if (value === 'MainTab' || value === 'LobbieTab') this.openCreateBar(false)
      this.setCurrentTab(value)
    },

    openCreateBar: function (value) {
      this.setIsCreate(value)
      this.play({ name: 'choice2', volume: 0.1 })
    },

    addLobbie: function () {
      const currentFilters = this.filters.map(item => item.currentValue)
      // const item = {
      //   id: Date.now(),
      //   type: currentFilters[4],
      //   title: currentFilters[0],
      //   maxPlayers: currentFilters[1],
      //   started: false,
      //   rate: currentFilters[2],
      //   weapons: currentFilters[3],
      //   rounds: currentFilters[5],
      //   redTeam: [],
      //   greenTeam: []
      // }

      var mode = currentFilters[4]
      var map = currentFilters[0]
      var maxPlayers = currentFilters[1]
      var weapon = currentFilters[3]
      var rounds = currentFilters[5]
      var bet = currentFilters[2]

      window.mp.trigger('ARENA::ADD::LOBBY::VUE', mode, map, maxPlayers, weapon, rounds, bet)

      //this.createLobbiesItem(item)
      //this.setIsCreate(false)
      //this.setCurrentTab('LobbieTab')
      //this.setCurrentLobbieId(item.id)
    },

    closeArena: function () {
      window.mp.trigger('ARENA::CLOSE::GUI::SERVER')
    }
  },
  
  mounted: function () {
    window.mp.trigger('ARENA::SET::PLAYER::NAME::VUE')
  }
}
</script>

<style lang="scss" scoped>
.arena-menu {
  width: 100vw;
  height: 100vh;
  background-image: url('../../../public/img/arenaMenu/bg.png');
  color: #1B1B1B;
  position: absolute;
  top: 0;
  left: 0;
  &-body{
    max-width: 1920px;
    height: 100%;
    margin: auto;
    position: relative;
  }
  .arena-menu__header {
    display: flex;
    align-items: center;
    padding: 0 0 1.7rem 0;
    //border-bottom: 1px solid rgba(255, 255, 255, 0.2);
    margin-right: 7rem;
    margin-left: 7rem;
    .header__exit {
      display: flex;
      padding: 1rem 2rem 1rem 2rem;
      position: absolute;
      width: 12.2rem;
      text-align: center;
      height: 3rem;
      line-height: 1rem;
      right: 7rem;
      top: 2rem;
      color: rgba(255, 255, 255, 0.4);
      background: rgba(255, 255, 255, 0.05);
      border: 1px solid rgba(255, 255, 255, 0.15);
      box-sizing: border-box;
      border-radius: 100px;
      text-transform: uppercase;
      font-size: 1.1rem;
      &:hover {
        background: rgba(255, 255, 255, 0.08);
      }
    }
    .header__logo {
      padding-top: 1.5rem;
    }
    .header__separator {
      border-left: 1px solid;
      background: rgba(255, 255, 255, 0.2);
      height: 2rem;
      left: 20.25rem;
      margin-top: 2.2rem;
      margin-left: 2.5rem;
      margin-right: 2rem;
    }
    .header__btn {
      background: transparent;
      text-transform: uppercase;
      font-size: 1.9rem;
      line-height: 1.9rem;
      font-weight: bold;
      padding: 2.5rem 1.3rem 0;
      letter-spacing: 1px;
      color:rgba(255, 255, 255, 0.4);
      &.active {
        color: rgba(255, 255, 255, 1);
        background: linear-gradient(180deg, rgba(182, 211, 0, 0.2) 0%, rgba(182, 211, 0, 0) 100%);
        border-top: .1rem solid #B6D300;
        &::after {
          position: absolute;
          content: '';
          bottom: -.1rem;
          left: 0;
          height: .1rem;
          width: 100%;
          background-color: #fff;
          box-shadow: 0px 10px 50px rgba(182, 211, 0, 0.7);
        }
      }
      &.active::after {
        opacity: 1;
      }
      &:hover {
        color: rgba(255, 255, 255, 0.6);
        background: linear-gradient(180deg, rgba(182, 211, 0, 0.2) 0%, rgba(182, 211, 0, 0) 100%);
      }
    }
  }
  .exit__icon {
    width: 1rem;
    height: 1.3rem;
    
    position: absolute;
    margin: auto 0 auto 6.9rem;
    top: 0;
    bottom: 0;
  }
  .arena-menu__filters {
    position: absolute;
    opacity: 1;
    top: 0;
    right: 0;
    margin-top: 12rem;
    margin-bottom: 3.5rem;
    width: 25.45rem;
    height: 100%;
    display: flex;
    flex-direction: column;
    padding-left: 1.45rem;
    padding-right: 7rem;
    border-left: 1px solid rgba(255, 255, 255, 0.2);
    .filters__header {
      text-transform: uppercase;
      font-style: normal;
      font-weight: bold;
      font-size: 3rem;
      padding-bottom: .75rem;
      color: #FFF;
    }
    .filters__desc {
      text-transform: uppercase;
      font-size: 1rem;
      padding-bottom: 1.9rem;
      color: rgba(255, 255, 255, 0.5);
    }
    .filters__secdesc {
      text-transform: uppercase;
      color: rgba(255, 255, 255, 0.3);
      font-size: .9rem;
      padding-bottom: .5rem;
      border-bottom: 1px solid rgba(255, 255, 255, 0.2);
    }
    .filters__main {
      //margin: 2.4rem 0 0 0;
      padding: 0 .5rem 1rem .5rem;
      display: flex;
      flex-direction: column;
      align-items: center;
      .main__btn {
        background: #B6D300;
        box-shadow: 0px 20px 90px rgba(168, 195, 2, 0.5);
        border-radius: 2px;
        width: 100%;
        margin: 1rem 0 2rem 0;
        text-transform: uppercase;
        padding: 1.6rem 3.2rem;
        font-size: 1.2rem;
        font-weight: bold;
        letter-spacing: 1.5px;
        //transition: .15s all ease;
        color: #fff;
        &:hover {
          box-shadow: 0 0 0 0;
        }
      }
    }
  }
}
.arena-header-enter-active, .arena-header-leave-active {
  transition: all .4s;
}
.arena-header-enter, .arena-header-leave-to {
  opacity: 0;
  transform: translateY(-30px);
}
</style>
