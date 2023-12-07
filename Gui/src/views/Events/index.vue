<template>
  <div class="events">
    <div class="events-body">
      <div class="events__all">
        <div class="event-checkbox"></div>
        <div 
          :class="[{active: currentEventObj.eventId === currentEvent}, 'event-item', 'event-current']"
          @click="setCurrentEvent(currentEventObj.eventId)"
        >
          <div 
            class="event-item__img"
            :style="{backgroundImage: 'url(/img/events/events-img/event-' + currentEventObj.eventId + '.png)'}"
          ></div>
        </div>
        <div class="event-list">
          <div 
            :class="[{active: item.eventId === currentEvent}, 'event-item']"
            v-for="item in featureEvents"
            :key="item.key"
            @click="setCurrentEvent(item.eventId)"
          >
            <div 
              class="event-item__img"
              :style="{backgroundImage: 'url(/img/events/events-img/event-' + item.eventId + '.png)'}"
            ></div>
            <div class="event-item__title">{{item.name}}</div>
          </div>
        </div>
      </div>
      <div 
        class="events__body"
        v-if="this.selectedEvent"
      >
        <div class="event-info">
          <div 
            class="event-info__img"
            :style="{backgroundImage: 'url(/img/events/events-img/event-' + selectedEvent.eventId + '.png)'}"
          ></div>
          <div 
            class="event-info__best"
            v-if="selectedEvent.bestTime"
          >
            <div class="prompt">{{loc('events_0')}}:</div>
            <div class="value">{{selectedEvent.bestTime}}</div>
          </div>
          <div class="event-info__prompt">{{loc('events_1')}}</div>
          <div class="event-info__title">{{selectedEvent.name}}</div>
          <div class="info">
            <div class="info__time">{{selectedEvent.startTime}}</div>
            <div class="info__place">{{loc(selectedEvent.locationName)}}</div>
          </div>
          <div class="event-info__desc">{{loc(selectedEvent.description)}}</div>
          <template v-if="selectedEvent.eventId === currentEventId">
            <div class="registration">
              <div class="registration__desc">{{loc('events_2')}}:</div>
              <div class="registration__value">{{currentEventObj.players}}/{{currentEventObj.maxPlayers}}</div>
            </div>
            <div 
              class="btn btn-reg"
              @click="regEvent"
              v-if="!currentEventObj.registered"
            >{{loc('events_3')}}</div>
            <template v-else>
              <div class="access">{{loc('events_4')}}</div>
              <div 
                class="btn btn-cancel"
                @click="cancelRegEvent"
              >{{loc('events_5')}}</div>
            </template>
          </template> 
          <div 
            class="reg-date"
            v-else
          >{{loc('events_6')}} {{selectedEvent.countStartTime}}</div>
        </div>
      </div>
    </div>
    
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
export default {
  name: 'Events',

  data: function() {
    return{
      currentEvent: null,
    }
  },

  computed: {
    ...mapState('events', ['events', 'currentEventId', 'notify']),
    ...mapGetters('localization', ['loc']),
    featureEvents: function() {
      return this.events.filter(element => element.eventId !== this.currentEventId)
    },
    currentEventObj: function() {
      return this.events.find(element => element.eventId === this.currentEventId)
    },
    selectedEvent: function() {
      return this.events.find(element => element.eventId === this.currentEvent)
    }
  },

  methods: {
    setCurrentEvent: function(value) {
      this.currentEvent = value
    },
    regEvent: function() {
      window.mp.triggerServer('ge:reg')
      setTimeout(() => {
        this.$forceUpdate()
      }, 2500) //todo: delete kostil
    },
    cancelRegEvent: function() {
      window.mp.triggerServer('ge:unreg')
    }
  },

  mounted() {
    this.currentEvent = this.currentEventId
  }
}
</script>

<style lang="scss" scoped>
.events{
  width: 100vw;
  height: 100vh;
  position: absolute;
  left: 0;
  top: 0;
  z-index: 1;
  background-size: cover;
  background-position: center;
  background-image: url('/img/events/bg.png');
  text-transform: uppercase;
  color: #fff;
  &-body{
    margin: auto;
    max-width: 1920px;
    height: 100%;
    position: relative;
    display: flex;
  }
  .btn{
    display: flex;
    align-items: center;
    justify-content: center;
    position: relative;
    transition: .2s;
    &:hover{
      transition: .2s;
    }
  }
  &__all, &__body{
    display: flex;
    flex-flow: column;
  }
  &__all{
    border-right: 1px solid rgba(255, 255, 255, 0.1);
    height: 100vh;
    padding-top: 7.2rem;
    .event-checkbox{
      position: absolute;
    }
    .event-list{
      display: flex;
      flex-flow: column;
      overflow-y: auto;
      max-height: 100%;
      &::-webkit-scrollbar{
        background: transparent;
        width: 0;
      }
    }
    .event-item{
      display: flex;
      flex-flow: column;
      width: 16.2rem;
      margin-left: 3rem;
      margin-right: 3rem;
      margin-bottom: 1.8rem;
      min-height: 9.8rem;
      &:first-child{
        margin-top: 1.8rem;
      }
      &.event-current{
        padding-bottom: 1.8rem;
        margin-bottom: 0;
        margin-top: 0;
        border-bottom: 1px solid rgba(255, 255, 255, 0.2);
        .event-item__img{
          margin-bottom: 0;
        }
      }
      &.active{
        .event-item__img{
          &:before,  &:after{
            opacity: 1;
            transition: .2s;
          }
        }
      }
      &:hover{
        .event-item__img{
          &:before{
            opacity: 1;
            transition: .2s;
          }
        }
      }
      &__img{
        background-size: contain;
        background-position: center;
        background-repeat: no-repeat;
        position: relative;
        height: 7.6rem;
        margin-bottom: .5rem;
        width: 100%;
        &:before{
          content: '';
          width: 100%;
          height: 100%;
          position: absolute;
          top: 0;
          left: 0;
          background: linear-gradient(0deg, rgba(0, 0, 0, 0.6), rgba(0, 0, 0, 0.6));
          opacity: 0;
          transition: .2s;
        }
        &:after{
          content: '';
          height: 100%;
          width: .1rem;
          background: #DCFF00;
          box-shadow: 0 0 .2rem #DCFF00;
          border-radius: .1rem;
          position: absolute;
          left: -.8rem;
          top: 0;
          transform: translateX(-100%);
          opacity: 0;
        }
      }
      &__title{
        font-style: normal;
        font-weight: bold;
        font-size: 1.5rem;
        line-height: 1.7rem;
      }
    }
  }
  &__body{
    padding-right: 36.75rem;
    padding-left: 3.3rem;
    padding-top: 7.1rem;
    position: relative;
    .event-info{
      display: flex;
      flex-flow: column;
      &__img, &__best{
        position: absolute;
      }
      &__img{
        top: 0;
        right: 0;
        height: 100vh;
        width: 36.75rem;
        overflow: hidden;
        background-size: cover;
        background-position: center;
        background-repeat: no-repeat;
      }
      &__best{
        top: 1.5rem;
        left: 5.4rem;
        display: flex;
        flex-flow: column;
        .prompt{
          font-weight: normal;
          font-size: 1rem;
          line-height: 1.15rem;
          color: rgba(255, 255, 255, 0.3);
          margin-bottom: .25rem;
        }
        .value{
          font-style: normal;
          font-weight: bold;
          font-size: 1.6rem;
          line-height: 1.85rem;
          letter-spacing: 0.04em;
          color: rgba(247, 234, 133, 1);
          position: relative;
          display: flex;
          align-items: center;
          &:before{
            content: '';
            width: 1.5rem;
            height: 1.5rem;
            position: absolute;
            left: -.6rem;
            transform: translateX(-100%);
            background-size: contain;
            background-position: center;
            background-repeat: no-repeat;
            background-image: url('/img/events/star.png');
          }
        }
      }
      &__prompt{
        font-style: normal;
        font-weight: 500;
        font-size: 1rem;
        line-height: 1.15rem;
      }
      &__title{
        font-style: normal;
        font-weight: bold;
        font-size: 4rem;
        line-height: 4.6rem;
        margin-bottom: 1.1rem;
      }
      .info{
        display: flex;
        align-items: center;
        margin-bottom: .9rem;
        &__time, &__place{
          display: flex;
          align-items: center;
          font-weight: 600;
          font-size: 1.1rem;
          line-height: 1.25rem;
          &:before{
            content: '';
            width: 1.5rem;
            height: 1.5rem;
            margin-right: .6rem;
            background-size: contain;
            background-position: center;
            background-repeat: no-repeat;
          }
        }
        &__time{
          margin-right: 1.2rem;
          &:before{
            background-image: url('/img/events/time.svg');
          }
        }
        &__place{
          &:before{
            background-image: url('/img/events/place.svg');
          }
        }
      }
      &__desc{
        font-style: normal;
        font-weight: 500;
        font-size: 1rem;
        line-height: 1.15rem;
        letter-spacing: 0.04em;
        color: rgba(255, 255, 255, 0.8);
        margin-bottom: 1.5rem;
      }
      .registration{
        display: flex;
        align-items: center;
        margin-bottom: 1.5rem;
        &__desc, &__value{
          font-weight: bold;
          font-size: 1rem;
          line-height: 1.15rem;
          letter-spacing: 0.04em;
        }
        &__desc{
          margin-right: 1rem;
        }
        &__value{
          &:before{
            content: '';
            width: 1.1rem;
            height: 1.1rem;
            margin-right: .4rem;
            background-size: contain;
            background-position: center;
            background-repeat: no-repeat;
            background-image: url('/img/events/player.svg');
          }
        }
      }
      .btn-reg, .btn-cancel{
        border-radius: .2rem;
        padding: 1.1rem 1.6rem;
        font-weight: 600;
        font-size: .8rem;
        letter-spacing: 0.04em;
        width: fit-content;
      }
      .btn-reg{
        background-color: #B6D300;
        &:hover{
          box-shadow: 0 0 2.5rem rgba(189, 227, 0, 0.5);
        }
      }
      .access{
        display: flex;
        align-items: center;
        position: relative;
        font-style: normal;
        font-weight: 600;
        font-size: 1.1rem;
        line-height: 1.25rem;
        color: #BDE300;
        margin-bottom: 1rem;
        &:before{
          content: '';
          width: 1.3rem;
          height: 1.3rem;
          margin-right: .6rem;
          background-size: contain;
          background-position: center;
          background-repeat: no-repeat;
          background-image: url('/img/events/access.svg');
        }
      }
      .btn-cancel{
        background-color: rgba(255, 255, 255, 0.1);
        &:hover{
          box-shadow: 0 0 2.5rem rgba(255, 255, 0, 0.1);
        }
      }
      .reg-date{
        font-weight: bold;
        font-size: 1.5rem;
        line-height: 1.7rem;
        letter-spacing: 0.04em;
        text-transform: uppercase;
        color: #FA3B3B;
      }
    }
  }
}
</style>
