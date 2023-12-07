<template>
  <div class="arena-menu__lobbie">
    <div v-if="isLobbieLeave" class="lobbie__modal">
      <span class="modal__text">{{ loc('arena_dm_25') }}</span>
      <div class="modal__actions">
        <button class="actions__btn" @click="leaveLobbie">{{ loc('arena_dm_23') }}</button>
        <button class="actions__btn" @click="setIsLobbieLeave(false)">{{ loc('arena_dm_24') }}</button>
      </div>
    </div>
    <transition appear name="list-left">
      <team-list
        :team="lobbie.redTeam"
        :title="lobbieMode === 'Team fight' ? 'Team Red' : 'arena_dm_27'"
        :color="lobbieMode === 'Team fight' ? '#FF3B30' : '#B6D300'"
        :engTitle="'RED'"
        :owner="lobbie.owner"
      />
    </transition>
    <transition appear name="lobbie-desc">
      <div class="lobbie__desc">
        <div class="desc__card" :style="{ 'background-image': `url(${lobbieBg})` }">
          <div class="card__header">
            <span class="header__id">ID: {{ loc(lobbie.id) }}</span>
            <span class="header__title">{{ loc(lobbie.title) }}</span>
          </div>
        </div>
        <div class="desc__item">
          <span class="item__title">{{loc('arena_dm_12')}}:</span>
          <span class="item__value">{{ lobbie.rate }}</span>
        </div>
        <div class="desc__item">
          <span class="item__title">{{loc('arena_dm_14')}}:</span>
          <span class="item__value">{{ lobbieMode }}</span>
        </div>
        <div class="desc__item" v-if="lobbie.type === 'Team fight'">
          <span class="item__title">{{loc('arena_dm_15')}}:</span>
          <span class="item__value">{{ lobbie.rounds }}</span>
        </div>
        <div class="desc__item" v-if="lobbie.type !== 'Gun game'">
          <span class="item__title">{{loc('arena_dm_13')}}:</span>
          <span class="item__value">{{ lobbie.weapons }}</span>
        </div>
        <button
          v-show="currentPlayer === lobbie.owner && !lobbie.started"
          class="desc__btn" 
          @click="startGame"
        >
          <span class="btn__text">{{ loc('arena_dm_20') }}</span>
        </button>
      </div>
    </transition>
    <transition appear name="list-right">
      <team-list v-show="lobbieMode === 'Team fight'"
        :owner="lobbie.owner"
        :team="lobbie.greenTeam"
        :title="'Team Green'"
        :color="'#B6D300'"
        :engTitle="'GREEN'"
      />
    </transition>
  </div>
</template>

<script>
import { mapState, mapGetters, mapMutations } from 'vuex'

import TeamList from './TeamList'

export default {
  name: 'LobbieTab',
  
  components: { TeamList },

  data: function () {
    return {
      tooltipText: 'Click to copy!'
    }
  },

  computed: {
    ...mapState('arenaMenu', [
      'lobbies',
      'currentLobbieId',
      'backgrounds',
      'currentPlayer',
      'isLobbieLeave'
    ]),

    ...mapGetters('localization', ['loc']),

    lobbieMode: function () {
      return this.lobbie.type
    },

    lobbie: function () {
      const rightIndex = this.lobbies.findIndex(item => item.id == this.currentLobbieId)
      return this.lobbies[rightIndex]
    },

    lobbieBg: function () {
      const rightItem = this.backgrounds.find(item => item.title == this.lobbie.title)
      return rightItem.image
    }
  },
  methods: {
    ...mapMutations('arenaMenu', ['setIsLobbieLeave']),

    ...mapMutations('sounds', ['play', 'stop']),

    startGame: function () {
      //this.currentLobbieId;
      window.mp.trigger('ARENA::START::LOBBY::VUE', this.currentLobbieId)
    },

    copyToClipboard: function (str) {
      var area = document.createElement('textarea')

      document.body.appendChild(area)
      area.value = str
      area.select()
      document.execCommand("copy")
      document.body.removeChild(area)
      this.tooltipText = 'Copy done!'
    },

    leaveLobbie: function () {
      window.mp.trigger('ARENA::LOBBY::LEAVE::VUE', this.currentLobbieId)
      this.setIsLobbieLeave(false)
      // setCurrentTab('MainTab)
      // setCurrentLobbieId(null)
    }
  },
  mounted: function() {
    this.play({ name: 'choice2', volume: 0.1 })
    this.play({ name: 'chooseTeam', volume: 0.03, loop: true })
    // window.setData('notifyList/notify', { type: 1, position: 0, message: this.lobbies, time: 10000 })
  },
  
  beforeDestroy: function () {
    this.stop('chooseTeam')
  }
}
</script>

<style lang="scss" scoped>
.arena-menu__lobbie {
  display: flex;
  justify-content: center;
  position: relative;
  .lobbie__modal {
    font-size: 1.1rem;
    z-index: 99;
    color: #FFF;
    width: 25rem;
    text-transform: uppercase;
    text-align: center;
    position: absolute;
    transform: translate(-50%, -50%);
    left: 50%;
    top: 50%;
    padding: 3rem 3rem;
    border-radius: 6px;
    background: #302e2e;
    .modal__actions {
      width: 100%;
      display: flex;
      justify-content: space-around;
      margin: 1rem 0 0 0;
      .actions__btn {
        text-transform: uppercase;
        padding: 1.4rem;
        //margin: .5rem 0 0 0;
        width: 7rem;
        height: 2rem;
        line-height: .1rem;
        border-radius: .2rem;
        color: #fff;
        //transition: all .2s;
        background: #B6D300;
        box-shadow: 0px 10px 50px rgba(168, 195, 2, 0.6);
        &:hover {
          box-shadow: 0 0 0 0;
        }
      }
    }
  }
  .lobbie__desc {
    display: flex;
    flex-direction: column;
    align-items: center;
    margin: 2rem 6rem 0 6rem;
    .desc__card {
      width: 14rem;
      height: 18.5rem;
      background-size: cover;
      background-position: center;
      border-radius: .9rem;
      margin: 3.4rem 0 1.2rem 0;
      display: flex;
      padding-left: 1.5rem;
      padding-right: 1.5rem;
      position: relative;
      text-transform: uppercase;
      .card__header {
        display: flex;
        flex-direction: column;
        color: #fff;
        font-size: 1.1rem;
        text-shadow: 0 0 8px rgba(0, 0, 0, 0.47);
        z-index: 1;
        .header__id {
          margin-top: auto;
        }
        .header__title {
          font-weight: bold;
          margin-bottom: 1.5rem;
          font-size: 1.8rem;
        }
        .header__icon {
          width: 1.25rem;
          height: 1.3rem;
          margin: 0 0 0 .5rem;
        }
      }
    }
    .desc__item {
      text-transform: uppercase;
      border-bottom: 1px solid rgba(255, 255, 255, 0.2);
      padding-top: .45rem;
      padding-bottom: .45rem;
      font-size: 1rem;
      line-height: 1.2rem;
      width: 100%;
      .item__title {
        color: #FFF;
        text-align: left;
      }
      .item__value {
        color: #B6D300;
        float: right;
      }
    }
    .desc__room-id {
      padding: .2rem .6rem;
      border-radius: .4rem;
      position: relative;
      transition: all .2s;
      margin: 1rem 0 0 0;
      .room-id__tooltip {
        position: absolute;
        transform: translate(-50%, -50%);
        left: 50%;
        top: 200%;
        width: 8rem;
        height: 2rem;
        display: flex;
        align-items: center;
        justify-content: center;
        background: rgb(0, 0, 0);
        color: #fff;
        border-radius: .4rem;
        opacity: 0;
        &::after {
          content: '';
          position: absolute;
          width: .6rem;
          height: .6rem;
          top: 0;
          left: 50%;
          transform: translate(-50%,-50%) rotate(45deg);
          background: rgb(0, 0, 0);
        }
      }
      &:hover {
        background: rgb(212, 212, 212);
        .room-id__tooltip {
          opacity: 1;
        }
      }
    }
    .desc__btn {
      background: #B6D300;
      box-shadow: 0px 20px 90px rgba(168, 195, 2, 0.5);
      border-radius: 2px;
      width: 14rem;
      height: 3.5rem;
      margin: 1rem 0 2rem 0;
      text-transform: uppercase;
      font-size: 1.2rem;
      font-weight: bold;
      letter-spacing: 1.5px;
      color: #fff;
      &:hover {
        box-shadow: 0 0 0 0;
      }
    }
  }
}
.lobbie-desc-enter-active, .lobbie-desc-leave-active {
  transition: all .4s;
}
.lobbie-desc-enter, .lobbie-desc-leave-to {
  opacity: 0;
  transform: translateY(30px);
}
.list-right-enter-active, .list-right-leave-active {
  transition: all .4s;
}
.list-right-enter, .list-right-leave-to {
  opacity: 0;
  transform: translateX(30px);
}
.list-left-enter-active, .list-left-leave-active {
  transition: all .4s;
}
.list-left-enter, .list-left-leave-to {
  opacity: 0;
  transform: translateX(-30px);
}
</style>
