<template>
  <div
    :class="[{ turned: title === 'Team Red' }, 'lobbie__team']"
    :style="{ '--curCol': this.color }"
  >
    <div class="team__header">
      <span class="header__title">{{ loc(title) }}</span>
      <span class="header__number">{{loc('arena:tlist:command')}} <span class="header__number-value">{{ numberOfPlayers }}</span></span>
    </div>
    <div class="team__main">
      <div
        class="main__item"
        v-for="(player, index) in team"
        :key="index"
      >
        <svg v-if="player === owner" version="1.1" class="item__icon" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" x="0px" y="0px"
          viewBox="0 0 156.745 156.745" style="enable-background:new 0 0 156.745 156.745;"
          xml:space="preserve">
          <filter id="shadow1">
            <feDropShadow dx="0" dy="0" stdDeviation="1.2" flood-color="#ff7200"/>
          </filter>
          <g filter="url(#shadow1)">
            <path d="M155.656,49.94c-0.97-0.722-2.299-0.717-3.259,0.01l-43.321,33.072L80.844,19.236c-0.864-1.954-4.07-1.954-4.936,0
              L47.332,83.772L4.364,49.991c-0.949-0.762-2.299-0.772-3.262-0.056c-0.988,0.73-1.36,2.009-0.917,3.148l28.907,74.669
              c0.4,1.039,1.402,1.725,2.515,1.725h93.542c1.113,0,2.12-0.686,2.516-1.725l28.904-74.669
              C156.996,51.944,156.626,50.67,155.656,49.94z M77.635,42.22L52.489,97.472c-0.277,0.602-0.81,1.045-1.458,1.203
              c-0.169,0.042-0.335,0.062-0.52,0.062c-0.475,0-0.944-0.152-1.326-0.453L17.632,73.636c-0.939-0.74-1.107-2.101-0.372-3.034
              c0.738-0.939,2.088-1.113,3.035-0.367l29.392,22.966l24.002-52.763c0.499-1.092,1.788-1.562,2.864-1.076
              C77.654,39.855,78.134,41.134,77.635,42.22z M124.722,138.974H32.018c-1.485,0-2.7-1.207-2.7-2.699c0-1.482,1.215-2.7,2.7-2.7
              h92.704c1.481,0,2.7,1.218,2.7,2.7C127.422,137.767,126.204,138.974,124.722,138.974z"/>
          </g>
        </svg>
        <span class="item__title">{{ player }}</span>
        <button
          v-if="currentPlayer === owner && player !== currentPlayer"
          class="item__close"
          @click="kickPlayer(player)"
        >&times;</button>
      </div>
      <button v-if="!searchCurrentPlayer" @click="joinTeam" class="main__btn">
        <span>{{ loc('arena_dm_26') }}</span>
      </button>
    </div>
  </div>
</template>

<script>
import { mapState, mapGetters } from 'vuex'

export default {
  name: 'TeamList',

  props: [
    'team',
    'title',
    'color',
    'engTitle',
    'owner'
  ],

  computed: {
    ...mapState('arenaMenu', ['currentLobbieId', 'currentPlayer']),

    ...mapGetters('localization', ['loc']),

    numberOfPlayers: function () {
      return this.team.length
    },

    searchCurrentPlayer: function () {
      return this.team.find(item => item === this.currentPlayer)
    }
  },

  methods: {
    joinTeam: function () {
      window.mp.trigger('ARENA::JOIN::LOBBY::VUE', this.engTitle, this.currentLobbieId)
    },

    kickPlayer: function (player) {
      window.mp.trigger('ARENA::KICK::LOBBY::VUE', player, this.currentLobbieId)
    }
  }
}
</script>

<style lang="scss" scoped>
.lobbie__team {
  display: flex;
  flex-direction: column;
  width: 25rem;
  padding: 1rem;
  border: 2px solid transparent;
  border-radius: 1.3rem;
  height: 35rem;
  overflow-y: auto;
  margin-top: 5rem;
  text-transform: uppercase;
  .header__number {
    font-size: 1.1rem;
    color: rgba(255, 255, 255, 0.4);
    margin-top: auto;
    .header__number-value {
      color: #FFF;
    }
  }
  .header__title {
    font-size: 4rem;
    line-height: 4rem;
    font-weight: lighter;
    color: var(--curCol);;
    text-shadow: 0px 4px 60px rgba(0, 0, 0, 0.5);
  }
  .team__header {
    font-size: 2.3rem;
    color: var(--curCol);
    border-bottom: 1px solid rgba(255, 255, 255, 0.2);
    padding-bottom: 1rem;
    display: flex;
    justify-content: space-between;
  }
  .team__main {
    display: flex;
    flex-direction: column;
    margin: 1rem 0 0 0;
  }
  .main__item {
    display: flex;
    align-items: center;
    margin: .5rem 0 0 0;
    position: relative;
    height: 2.2rem;
    background: rgba(255, 255, 255, 0.1);
    border-left: 2px solid var(--curCol);
    box-shadow: inset 1px 1px 50px -30px var(--curCol);
    &:first-child {
      margin: 0;
    }
    .item__icon {
      transform: skew(-20deg);
      fill: gold;
      width: 1.9rem;
      height: 1.9rem;
      margin: 0 0 0 1rem;
    }
    .item__title {
      display: inline-block;
      margin: 0 0 0 1rem;
      color: #FFF;
    }
    .item__close {
      margin: 0 .7rem 0 auto;
      font-size: 1.5rem;
      color: rgba(255, 255, 255, 0.4);
      background-color: transparent;
      border: 2px solid transparent;
      width: 1.6rem;
      display: flex;
      align-items: center;
      justify-content: center;
      height: 1.6rem;
      transition: all .2s;
      z-index: 20;
      &:hover {
        color: rgba(255, 255, 255, 1);
      }
    }
  }
  .main__btn {
    margin: .5rem 0 0 0;
    width: 100%;
    height: 3.5rem;
    align-self: center;
    color: #fff;
    border: 2px solid #B6D300;
    background: transparent;
    box-sizing: border-box;
    border-radius: 2px;
    transition: all .2s;
    &:hover {
      background: #B6D300;
    }
    span {
      display: inline-block;
    }
  }
}
</style>
