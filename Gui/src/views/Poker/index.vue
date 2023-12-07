<template>
  <div class="poker__container">
    <audio id="cardSound" src="/sounds/poker/card.wav"></audio>
    <audio id="individualBitSound" src="/sounds/poker/individual_bit.wav"></audio>
    <audio id="ratesDoneSound" src="/sounds/poker/rates_done.wav"></audio>
    <audio id="turnSound" src="/sounds/poker/turn_sound.mp3"></audio>
    <audio id="winningSound" src="/sounds/poker/winning_sound.wav"></audio>
    <div class="central-block">
      <transition name="fade">
        <div v-if="allBets" class="poker__bets">
          <div
            v-for="(bet, index) in allBets"
            :key="index"
            class="poker__bet"
          >
            <img src="/img/poker/chip_blue.png" alt="chip">
            <span>$ {{ bet.toLocaleString() }}</span>
          </div>
        </div>
      </transition>
      <div class="poker__distribution" v-if="cardsDistribution.length">
        <distribution-card
          v-for="(card, index) in cardsDistribution"
          :key="index"
          :card="card"
          :index="index"
        />
      </div>
    </div>
    <other-player
      v-for="(player, index) in otherPlayers"
      :key="index"
      :player="player"
      :class="[`player-${index + 1}`]"
    />
    <current-player 
      @setSaleOfChips="setSaleOfChips"
      @setAvatarOption="setAvatarOption"
      @setExitWindow="setExitWindow"
    />
    <ExitWindow
      v-if="showExitWindow"
      @setExitWindow="setExitWindow"
    />
    <SaleOfChips 
      v-if="showSaleOfChips"
      @setSaleOfChips="setSaleOfChips"
    />
    <AvatarOption 
      v-if="showAvatarOption"
      @setAvatarOption="setAvatarOption"
    />
  </div>
</template>

<script>
import { mapState, mapGetters } from 'vuex'
import DistributionCard from './components/DistributionCard'
import OtherPlayer from './components/OtherPlayer'
import CurrentPlayer from './components/CurrentPlayer'
import SaleOfChips from './components/SaleOfChips'
import AvatarOption from './components/AvatarOption'
import ExitWindow from './components/ExitWindow'

export default {
  name: 'Poker',

  components: {
    DistributionCard,
    OtherPlayer,
    CurrentPlayer,
    SaleOfChips,
    AvatarOption,
    ExitWindow
  },

  data: function() {
    return {
      showSaleOfChips: false,
      showAvatarOption: false,
      showExitWindow: false,
    }
  },

  computed: {
    ...mapState('poker', [
      'cardsDistribution',
      'otherPlayers',
      'allBets',
      'ratesDone'
    ]),
    ...mapGetters('localization',['loc']),

    winItem: function () {
      return this.cardsDistributionCombinations.forEach(item => item)
    }
  },

  methods: {
    exitDone () {
      this.setShowExitWindow(false)
      window.mp.trigger('poker:exit')
      this.setShowExitWindow(false)
    },

    exitClose () {
      this.setShowExitWindow(false)
    },

    setSaleOfChips: function(value) {
      this.showSaleOfChips = value
    },
    setAvatarOption: function(value) {
      this.showAvatarOption = value
    },
    setExitWindow: function(value) {
      this.showExitWindow = value
    }
  }
}
</script>

<style lang="scss" scoped>
.poker__container {
  width: 100vw;
  height: 100vh;
  position: absolute;
  left: 0;
  top: 0;
  font-weight: bold;
  background-size: cover;
  background-position: center;
  background-image: url('/img/poker/bg.png');
  .central-block{
    position: absolute;
    top: 48%;
    left: 50%;
    transform: translate(-50%, -50%);
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
  }
  .poker__distribution {
    display: flex;
  }
  .poker__bets {
    display: flex;
    margin-bottom: 1.5rem;
    .poker__bet {
      display: flex;
      align-items: center;
      justify-content: center;
      margin-right: .6rem;
      padding: .65rem 1rem;
      background: rgba(255, 255, 255, 0.1);
      border-radius: .1rem;
      &:last-child {
        margin: 0;
      }
      img {
        width: 1.2rem;
        margin-right: .5rem;
      }
      span {
        color: #e5e5e5;
        font-weight: 600;
        letter-spacing: 1px;
      }
    }
  }
}
.fade-enter-active {
  transition: all 1s;
}
.fade-enter{
  opacity: 0;
}
</style>
