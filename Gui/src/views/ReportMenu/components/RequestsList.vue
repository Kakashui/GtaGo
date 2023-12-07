<template>
  <div class="requests-list scroll">
    <div 
      :class="[{ active: currentChat !== null && item.id === currentChat.id }, 'requests-list__item']"
      v-for="item in reports"
      :key="item.key"
      @click="setCurrentChat(item)"
    >
      <div :class="[{ vip: item.reporterStatus === 2 }, { media: item.reporterStatus === 3 }, { leader: item.reporterStatus === 1 }, 'title']">
        <div 
          class="title__icon" 
          v-if="item.reporterStatus !== 0"
        ></div>
        <div class="title__name">{{item.reporterName}}</div>
        <div class="title__id">[{{item.reporterId}}]</div>
        <div class="title__unread" v-if="item.unreadMessages > 0">{{item.unreadMessages}}</div>
      </div>
      <div class="message">{{item.messages[0].text}}</div>
      <div class="report-data">
        <span>{{loc('ReportMenu_7')}} №{{item.id}}</span>
        <span>{{isReportDate(item.time)}}</span>
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapMutations, mapState } from 'vuex'
export default {
  name: 'RequestsList',

  props: {
    reports: Array
  },

  computed: {
    ...mapState('reportMenu', ['currentChat']),
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    ...mapMutations('reportMenu', ['setCurrentChat']),
    isReportDate: function(value) {
      let time = new Date(value).toLocaleTimeString('ru-RU', {timeZone: 'UTC'})
      let date = new Date(value).toLocaleDateString('ru-RU', {timeZone: 'UTC'})
      return (date + ' ' + time)
    }
  }
}
</script>

<style lang="scss" scoped>
.requests-list{
  display: flex;
  flex-flow: column;
  height: calc(100% - (4.85rem));
  overflow-y: auto;
  &__item{
    display: flex;
    flex-flow: column;
    align-items: flex-start;
    justify-content: center;
    padding: 1.25rem 2.35rem;
    width: 100%;
    min-height: 6.85rem;
    font-family: Roboto;
    &:hover, &.active{
      background: #26282E;
    }
    .title{
      display: flex;
      align-items: center;
      justify-content: flex-start;
      font-size: .9rem;
      height: 1rem;
      min-height: 1rem;
      width: 100%;
      &__icon{
        width: 1rem;
        height: 1rem;
        margin-right: .6rem;
        background-size: contain;
        background-repeat: no-repeat;
        background-position: center;
      }
      &__name, &__id{
        color: rgba(255, 255, 255, 0.45);
      }
      &__id{
        margin-right: .6rem;
      }
      &__unread{
        color: #000;
        width: 1.5rem;
        height: 1.5rem;
        display: flex;
        align-items: center;
        justify-content: center;
        background-color: #C1E704;
        font-weight: bold;
        border-radius: 50%;
        margin-left: auto;
      }
      &__name{
        font-weight: bold;
        margin-right: .5rem; 
      }
      &.vip{
        .title__icon{
          background-image: url('/img/reportMenu/vip-icon.svg');
        }
        .title__name{
          color: #FFC839;
        }
      }
      &.media{
        .title__icon{
          background-image: url('/img/reportMenu/media-icon.svg');
        }
        .title__name{
          color: #D55CFF;
        }
      }
      &.leader{
        .title__icon{
          background-image: url('/img/reportMenu/leader-icon.svg');
        }
        .title__name{
          color: #C1E704;
        }
      }
    }
    .message{
      font-size: .9rem;
      height: 1rem;
      min-height: 1rem;
      overflow: hidden;
      margin: .5rem 0 .85rem 0;
    }
    .report-data{
      width: 100%;
      display: flex;
      align-items: center;
      justify-content: space-between;
      font-size: .7rem;
      color: rgba(255, 255, 255, 0.45);
    }
  }
}
</style>
