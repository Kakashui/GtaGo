<template>
<transition name="slide-fade-reversed">
  <div v-if="showNavElem == 'GeneralEvents'" class="general-events">
    <div class="side-bar">
      <div class="side-bar-title">
        <div class="side-bar-title__event-name"><marquee id="marque1" v-if="isCompleatedActive" behavior="alternate" scrolldelay="750" width="20rem" style="width: 20rem;" scrollamount="10" direction="left">{{loc('familyMenu_EventCompleate') }}</marquee><div v-else>{{loc('familyMenu_EventNew')}}</div></div>
        <div class="side-bar-title__event-desc ep-scroll">{{loc('fam:btl:gen:desc')}}</div>
      </div>
      <div class="side-bar-buttons">
        <div :class="[{active: isCompleatedActive === false}, 'side-bar-buttons__events']" @click="setCurrentEvents(false)">{{loc('fam:btl:gen:new')}}</div>
        <div :class="[{active: isCompleatedActive === true}, 'side-bar-buttons__events']" @click="setCurrentEvents(true)">{{loc('fam:btl:gen:compl')}}</div>
      </div>
    </div>
    <div class="content" v-if="currentBattleData">
      <div class="content-info">
        <div class="content-info-header">
          <div class="content-info-header__date"><img src="/img/familyMenu/events-page-date.svg">{{getDate}}</div>
          <div class="content-info-header__time"><img src="/img/familyMenu/events-page-time.svg">{{getTime}}</div>
          <div class="content-info-header__place"><img src="/img/familyMenu/events-page-place.svg">{{loc(getPlace.name)}}</div>
        </div>
        <div class="content-info__image" :style="{backgroundImage: 'url(/img/familyMenu/eventsPage/places/' + getPlace.img + '.png)'}"></div>
        <div class="content-info-footer">
          <div class="content-info-footer-desc">
            <div class="content-info-footer-desc__title">{{loc('fam:btl:gen:desc:tit')}}</div>
            <div class="content-info-footer-desc__text ep-scroll">{{loc(currentBattleData.Description)}}</div>
          </div>
          <div class="content-info-footer-desc">
            <div class="content-info-footer-desc__title">{{loc('fam:btl:gen:desc:prize')}}</div>
            <div class="content-info-footer-desc__text ep-scroll">{{loc(currentBattleData.Rewards)}}</div>
          </div>
          <div class="content-info-footer-buttons">
            <div v-if="isCompleatedActive" @click="showBattleResults" class="content-info-footer-buttons__btn">{{loc('fam:btl:gen:check')}}</div>
          </div>
        </div>
      </div>
      <div class="content-switcher">
        <div class="content-switcher-prev" @click="prevMP">
          <svg width="19" height="16" viewBox="0 0 19 16" fill="none" xmlns="http://www.w3.org/2000/svg">
            <path style="transition: all 0.2s linear;" d="M0.292614 7.29289C-0.0979099 7.68342 -0.0979099 8.31658 0.292614 8.70711L6.65658 15.0711C7.0471 15.4616 7.68027 15.4616 8.07079 15.0711C8.46131 14.6805 8.46131 14.0474 8.07079 13.6569L2.41393 8L8.07079 2.34315C8.46131 1.95262 8.46131 1.31946 8.07079 0.928932C7.68027 0.538408 7.0471 0.538408 6.65658 0.928932L0.292614 7.29289ZM19 7L0.999722 7V9L19 9V7Z" :fill="hoveredElem == 'footerArrowLeft' ? '#C1E704' : 'white'"/>
          </svg>
          {{loc('fam:btl:gen:back')}}
        </div>
        <div class="content-switcher-progress">{{currentBattle + 1}}/{{currentEvents.length}}</div>
        <div class="content-switcher-next" @click="nextMP">
          {{loc('fam:btl:gen:fwd')}}          
          <svg style="transform: scale(-1);" width="19" height="16" viewBox="0 0 19 16" fill="#C1E704" xmlns="http://www.w3.org/2000/svg">
            <path style="transition: all 0.2s linear;" d="M0.292614 7.29289C-0.0979099 7.68342 -0.0979099 8.31658 0.292614 8.70711L6.65658 15.0711C7.0471 15.4616 7.68027 15.4616 8.07079 15.0711C8.46131 14.6805 8.46131 14.0474 8.07079 13.6569L2.41393 8L8.07079 2.34315C8.46131 1.95262 8.46131 1.31946 8.07079 0.928932C7.68027 0.538408 7.0471 0.538408 6.65658 0.928932L0.292614 7.29289ZM19 7L0.999722 7V9L19 9V7Z" :fill="hoveredElem == 'footerArrowRight' ? '#C1E704' : 'white'"/>
          </svg>
        </div>
      </div>
    </div>
  </div>
</transition>
</template>

<script>
import { mapState, mapMutations, mapGetters } from 'vuex'

export default {
  name: 'GeneralEvents',
  data() {
    return {
      isCompleatedActive: false,
      hoveredElem: null,
      currentBattle: 0,
      currentBattleData: null
    }
  },
  components: {

  },
  computed:{
    ...mapGetters('localization', ['loc']),
    ...mapState('familyMenu/eventsPage', ['showNavElem', 'globalEvents', 'placesList']),
    currentEvents() {
      return this.globalEvents.filter(item => item.IsFinished == this.isCompleatedActive).sort((a,b) => a.Date - b.Date)
    },
    getTime() {
      let date = new Date(this.currentBattleData.Date * 1000);
      var options = { timezone: 'UTC', hour: 'numeric', minute: 'numeric' }
      return date.toLocaleString("ru", options)
    },
    getDate() {
      let date = new Date(this.currentBattleData.Date * 1000);
      var options = { timezone: 'UTC', year: 'numeric', month: 'numeric', day: 'numeric' }
      return date.toLocaleString("ru", options)
    },
    getPlace() {
      return this.placesList.find(item => item.id == this.currentBattleData.Location);
    }
  },
  methods: {
    ...mapMutations('familyMenu', ['toggleNav']),
    ...mapMutations('familyMenu/eventsPage', [
      'toggleBattleResults'
    ]),
    showBattleResults() {
      this.toggleNav(false);
      this.toggleBattleResults(this.currentBattleData)
    },
    setCurrentEvents(value) {
      this.isCompleatedActive = value;
      this.currentBattleData = this.currentEvents[0];
    },
    marqueeBounced(elem) {
      console.log(elem)
    },
    prevMP() {
      this.currentBattle--;
      if (this.currentBattle < 0)
        this.currentBattle = this.currentEvents.length - 1;
      if (this.currentEvents.length > this.currentBattle)
        this.currentBattleData = this.currentEvents[this.currentBattle]
    },
    nextMP() {
      this.currentBattle++;
      if (this.currentBattle >= this.currentEvents.length)
        this.currentBattle = 0;
      if (this.currentEvents.length > this.currentBattle)
        this.currentBattleData = this.currentEvents[this.currentBattle]
    }
  },
  mounted() {
    this.currentBattleData = this.currentEvents[0];
  }
}

</script>

<style lang="scss" scoped>

.slide-fade-reversed-enter-active, .slide-fade-reversed-leave-active {
  transition: all .3s ease;
}
.slide-fade-reversed-enter, .slide-fade-reversed-leave-to {
  transform: translateX(-3rem);
  opacity: 0;
}

.ep-scroll {
  &::-webkit-scrollbar {
      width: 0.1rem;
      background-color: rgba(255, 255, 255, 0.0);
  }

  &::-webkit-scrollbar-thumb {
      background: #DCFF00;
  }

  &::-webkit-scrollbar-button:horizontal:start:decrement {
      display: none;
  }

  &::-webkit-scrollbar-button:horizontal:end:increment {
      display: none;
  }
}

.general-events {
  display: flex;
  justify-content: space-between;
  .side-bar {
    &-title {
      &__event-name {
        font-weight: normal;
        font-size: 3rem;

        width: 22.7rem;
        height: 2.9rem;

        margin-bottom: 1rem;

        white-space: pre;
      }

      &__event-desc {
        font-weight: 300;
        font-size: 0.9rem;
        letter-spacing: 0.03em;
        line-height: 1.2rem;

        color: rgba(255, 255, 255, 0.5);

        height: 6.85rem;
        width: 16.05rem;

        margin-bottom: 1rem;
        
        overflow-y: auto;
      }
    }

    &-buttons {
      font-weight: 300;
      font-size: 1.2rem;
      letter-spacing: 0.03em;

      &__events {
        width: 10.75rem;
        height: 3rem;

        border-color: #343539;
        border-style: solid;

        display: flex;
        
        align-items: center;

        padding-left: 1.25rem;

        transition: color 0.1s linear, background-color 0.1s linear;

        margin-bottom: 0.5rem;

        background-color: rgba(0, 0, 0, 0);
        border-width: 0px;
        color: rgba(255, 255, 255, 0.5);
        &:hover {
          color: rgba(255, 255, 255, 1);
        }
        &.active {
          color: rgba(255, 255, 255, 1);          
          background-color: #222428;
          border-width: 1px;
        }
      }
    }
  }

  .content {
    &-info {
      &-header {
        display: flex;

        font-weight: bold;
        font-size: 2rem;

        margin-bottom: 1rem;

        & img {
          vertical-align: middle;

          width: 2rem;
          height: 2rem;

          margin-right: 0.5rem;
        }

        &__date {
          color: rgba($color: #000000, $alpha: 0);

          -webkit-text-stroke-width: 0.05rem;
          -webkit-text-stroke-color: rgb(255, 255, 255);

          margin-right: 1rem;
        }

        &__place {
          width: 31.6rem;
          text-align: right;
        }
      }
      
      &__image {
        height: 20.3rem;
        width: 48.3rem;

        background-size: cover;
      }

      &-footer {
        display: flex;
        margin-top: 1rem;

        &-desc {
          margin-right: 2rem;

          &__title {
            color: rgba($color: #000000, $alpha: 0);

            -webkit-text-stroke-width: 0.02rem;
            -webkit-text-stroke-color: rgb(255, 255, 255);

            font-weight: bold;
            font-size: 1.6rem;

            margin-bottom: 0.5rem;
          }

          &__text {
            width: 17.5rem;
            height: 5.65rem;

            overflow-y: auto;

           text-transform: uppercase;
            font-weight: 400;
            font-size: 1.2rem;

            line-height: 1.12rem;
            letter-spacing: 0.03rem;

            color: rgba(255, 255, 255, 0.7);
          }
        }

        &-buttons {
          display: flex;
          
          width: -webkit-fill-available;
          height: 8rem;

          align-items: flex-end;
          justify-content: flex-end;

          &__btn {
            display: flex;

            justify-content: center;
            align-items: center;

            width: 8.65rem;
            height: 2.9rem;

            background: #C1E704;
            border-radius: 0.1rem;

            font-weight: bold;
            font-size: 1.25rem;

            transition: all 0.1s linear;

            &:hover {
              transform: scale(1.1);
              box-shadow: 0px 0.5rem 3.5rem rgba(220, 255, 0, 0.4);
            }
          }
        }
      }
    }

    &-switcher {
      margin-top: 4rem;
      display: flex;
      font-family: "Roboto";
      font-style: normal;
      font-weight: normal;
      font-size: 1.1rem;
      text-transform: uppercase;
      & svg {
        vertical-align: sub;
        width: 1.3rem;
        height: 1.3rem;
      }

      &-prev {
        margin-right: 0.2rem;
        height: 2rem;
        position: relative;
        transition: all 0.2s;
        display: flex;
        & svg {
          margin-right: 0.7rem;
        }

        &:hover {
          color: #C1E704;  
          &::after {
            position: absolute;
            bottom: 0;
            left: 0;
            content: "";
            height: 0.1rem;
            width: 100%;
            background: #C1E704;
            transition: all 0.2s;
            transform: scale(1.1);   
          }
        }
        &::after {
          position: absolute;
          bottom: 0;
          left: 0;
          content: "";
          height: 0.1rem;
          width: 100%;
          background: #FFF;
          transition: all 0.2s;   
        }
      }

      &-progress {
        width: 36.4rem;
        height: 2rem;
        text-align: center;
        font-size: 1rem;
        position: relative;

        &::after {
          position: absolute;
          bottom: 0;
          left: 0;
          content: "";
          height: 0.1rem;
          width: 100%;
          background: rgba(255, 255, 255, 0.2);
          transition: all 0.2s;        
        }
      }

      &-next {
        margin-left: 0.2rem;
        height: 2rem;
        position: relative;
        transition: all 0.2s;
        display: flex;
        & svg {
          margin-left: 0.7rem;
        }

        &:hover {
          color: #C1E704;
          &::after {
            position: absolute;
            bottom: 0;
            left: 0;
            content: "";
            height: 0.1rem;
            width: 100%;
            background: #C1E704;
            transition: all 0.2s;
            transform: scale(1.1);   
          }
        }
        &::after {
          position: absolute;
          bottom: 0;
          left: 0;
          content: "";
          height: 0.1rem;
          width: 100%;
          background: #FFF;
          transition: all 0.2s;   
        }
      }
    }
  }
}

</style>