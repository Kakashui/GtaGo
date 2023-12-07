<template>
  <div class="battle-results-content">

    <transition name="hexagon-slide">
      <div v-show="show" @click="hideResults()" class="btn-back">
        <div class="btn"><img src="/img/familyMenu/battle-results-back-icon.svg"></div>
       {{loc('fam:btl:res:back')}}
      </div>
    </transition>

    <transition name="hexagon-slide">
      <div v-show="show" class="hexagon">

        <div style="position: absolute;">
          <img class="hexagon__border" style="animation: rotAnim 100s linear infinite;" src="/img/familyMenu/eventsPage/polygon_2.svg">
          <img class="hexagon__border" style="animation: rotAnimReversed 100s linear infinite;" src="/img/familyMenu/eventsPage/polygon_3.svg">
          <svg class="hexagon__image" width="35.3rem" height="30rem" viewBox="0 0 706 600" xmlns="http://www.w3.org/2000/svg">
            <defs>
              <pattern id="img" patternUnits="userSpaceOnUse" width="150vh" height="100vh">
              <image href="/img/familyMenu/eventsPage/polygon_1.png" /></pattern>
            </defs>
            <path fill="url(#img)" d="M0 306L176.5 0.29303H529.5L706 306L529.5 611.707H176.5L0 306Z" fill-opacity="0.5"/>
          </svg>
        </div>

        <div class="hexagon__content">
          <div class="div1 date-time">
            <div class="date"><img src="/img/familyMenu/battle-results-date.svg">{{getDate}}</div>
            <div class="time"><img src="/img/familyMenu/battle-results-time.svg">{{getTime}}</div>
          </div>
          <div class="title"> </div>
          <div class="div1 place"><img src="/img/familyMenu/battle-results-place.svg">{{loc(getPlace.name)}}</div>
        </div>

      </div>
    </transition>

    <transition name="score-slide">
      <div v-show="show" class="score">

        <div class="winner">
          {{loc('fam:btl:res:win')}}          
          <div class="winner__name">{{currentEvent.WinnerFamilyName}}</div>
        </div>

        <div class="shift"></div>

        <div class="table">
          <div class="table__name">{{loc('fam:btl:res:top')}}</div>
          <div v-for="(item, index) in currentKillLog" :key="item.UUID"> 
            <div class="table__row">
              <div class="row-hyphen"></div>
              <div class="row-index">{{index + 1}}</div>
              <div class="row-family">{{item.Familyname}}</div>
              <div class="row-name">{{item.Name}}</div>
              <div class="row-kills">{{item.Kills}}</div>
              <div class="row-icon"><img src="/img/familyMenu/battle-kills-icon.svg"></div>
            </div>
          </div>
        </div>

      </div>
    </transition>

  </div>
</template>

<script>
import { mapMutations, mapState, mapGetters } from 'vuex'

export default {
  name: 'BattleResults',
  data() {
    return {
      show: false,
      exitBtnHovered: false
    }
  },
  mounted() {
    this.show = true;
  },
  destroyed() {
    this.toggleNav(true);
  },
  computed: {
    ...mapGetters('localization', ['loc']),
    ...mapState('familyMenu/eventsPage', ['currentEvent', 'placesList']),
    currentKillLog() {
      return this.currentEvent.KillLog.slice().sort((a, b) => b.Kills - a.Kills);
    },
    getTime() {
      let date = new Date(this.currentEvent.Date * 1000);
      var options = { timezone: 'Europe/Moscow', hour: 'numeric', minute: 'numeric' }
      return date.toLocaleString("ru", options)       
    },
    getDate() {
      let date = new Date(this.currentEvent.Date * 1000);
      var options = { timezone: 'Europe/Moscow', year: 'numeric', month: 'numeric', day: 'numeric' }
      return date.toLocaleString("ru", options)
    },
    getPlace() {
      return this.placesList.find(item => item.id == this.currentEvent.Location);
    }
  },
  methods: {
    ...mapMutations('familyMenu/eventsPage', ['toggleBattleResults']),
    ...mapMutations('familyMenu', ['toggleNav']),
    hideResults() {
      this.show = false
      setTimeout(() => {
        this.toggleBattleResults(null)
      }, 300)
    }
  }
}
</script>

<style lang="scss" scoped>

.hexagon-slide-enter-active {
  transition: all 0.4s ease;
}
.hexagon-slide-leave-active {
  transition: all 0.4s ease;
}
.hexagon-slide-enter {
  transform: translateX(-20rem);
  opacity: 0;
}
.hexagon-slide-leave-to {
  transform: translateX(-20rem);
  opacity: 0;
}

.score-slide-enter-active {
  transition: all 0.4s ease;
}
.score-slide-leave-active {
  transition: all 0.4s ease;
}
.score-slide-enter {
  transform: translateX(20rem);
  opacity: 0;
}
.score-slide-leave-to {
  transform: translateX(20rem);
  opacity: 0;
}

.battle-results-content {
  display: flex;
  margin-top: 4rem;

  .score {
    margin-left: 8rem;

    .winner {
      font-weight: 200;
      font-size: 2rem;

      &__name {
        font-weight: bold;
        font-size: 4rem;
      }
    }

    .shift {
      width: 23rem;
      height: 0.1rem;
      background: radial-gradient(100% 5290000.22% at 100% 49.91%, rgba(255, 255, 255, 0) 0%, rgba(255, 255, 255, 0.6) 100%);
      margin-bottom: 2.5rem;
    }

    .table {

      &__name {
        font-weight: bold;
        font-size: 2rem;
        color: rgba(255, 255, 255, 0.7);
      }

      &__row {
        display: grid;
        grid-template-columns: 1.3rem 1.35rem 7.1rem 10.85rem 1.25rem 1.25rem;
        align-content: center;
        align-items: center;
        margin-top: 1.1rem;

        .row-hyphen {
          width: 0.8rem;
          height: 1px;
          background: #DCFF00;
        }

        .row-index {
          font-weight: bold;
          font-size: 1.2rem;
        }

        .row-family {
          font-weight: bold;
          font-size: 1.2rem;
        }

        .row-name {
          font-weight: 300;
          font-size: 1.2rem;
        }

        .row-kills {
          font-weight: bold;
          font-size: 1.2rem;
        }

        .row-icon {

          & img {
            width: 1.2rem;
            height: 1.2rem;
          }
        }
      }
    }
  }

  .btn-back {
    transition: all 0.1s linear;
    position: absolute;
    top: 0;
    left: 0;
    margin: 2.5rem;
    display: flex;
    align-items: center;
    font-weight: bold;
    font-size: 1.2rem;
    background: #222428;

    &:hover {
      transform: scale(1.1);
      background: rgba(220, 255, 0, 1);
    }

    .btn {
      width: 2rem;
      height: 2rem;
      margin-right: 0.5rem;
      border: 1px solid #343539;
    }
  }

  .hexagon {
    margin-top: 3rem;

    &__border {
      position: absolute; 
      width: 38.7rem; 
      height: 33.6rem;
    }

    &__image {
      margin: 1.7rem
    }

    &__content {
      width: 39rem;
      height: 28.7rem;
      text-align: -webkit-center;
      padding-top: 4rem;

      .title {
        font-weight: bold;
        font-size: 3.5rem;
        color: #FFFFFF;
        filter: opacity(1);
        margin-top: 7.5rem;
        margin-bottom: 7.5rem;
      }

      .date-time {

        .date {
          margin-right: 1.25rem;
        }
      }

      .div1 {
        width: 13.2rem;
        height: 3.2rem;
        background: linear-gradient(90deg, #000000 0%, #46555F 68.75%);
        backdrop-filter: opacity(1);
        border-radius: 2rem;
        display: flex;
        align-items: center;
        place-content: center;

        & img {
          vertical-align: middle;
          margin-right: 0.5rem;
        }
      }
    }
  }
}

</style>

<style>

@keyframes rotAnim {
  from {
    transform: rotate(0deg);
  }
  to {
    transform: rotate(360deg);
  }
}
@keyframes rotAnimReversed {
  from {
    transform: rotate(0deg);
  }
  to {
    transform: rotate(-360deg);
  }
}

</style>

