<template>
  <div class="list-item">
    <div class="list-item__header">
      <img v-if="item.ImgUrl"
        :src="item.ImgUrl"
        alt="img"
        class="img"
      >
      <div class="btn-options" v-if="messengerData.CurrentChat.CanDeletingMessages" @click="$emit('edit', item.Id)"></div>
    </div>
    <div class="list-item__main">
      <span class="title">{{ item.Title }}</span>
      <p class="text" v-html="getNormalText(item.Text)"></p>
      <div class="info">
        <div :class="[{ active: item.IsLiked }, 'info__item like']">
          <div class="icon"></div>
          <span class="count">{{ item.LikeCount }}</span>
        </div>
        <div class="info__item view">
          <div class="icon"></div>
          <span class="count">{{ item.ViewsCount }}</span>
        </div>
        <div class="info__date">{{ getCorrectTime(item.Time) }}</div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapState } from "vuex";

export default {
  name: 'ListItem',

  props: { item: Object },

  computed: {
    ...mapState('smartphone/messagePage', ['messengerData'])
  },

  methods: {
    getCorrectTime: function(time) {
      const options = { timeZone: 'UTC', locales: 'ru-RU', year: 'numeric', month: '2-digit', day: '2-digit', hour12: false, hour: 'numeric', minute: 'numeric'}
      const date = new Intl.DateTimeFormat(undefined, options).format(time)

      return date
    },

    getNormalText: function(text) {
      const normalText = text.replace( /\n/g, '<br/>');
      return normalText;
    }
  }
}
</script>

<style lang="scss" scoped>
.list-item {
  flex: 0 0 21.2rem;
  background: #fff;
  display: flex;
  flex-direction: column;
  margin: 0 0 0.75rem 0;
  &__header {
    position: relative;
    .img {
      width: 100%;
      height: auto;
    }
    .btn-options{
      position: absolute;
      top: .5rem;
      right: .5rem;
      width: 2rem;
      height: 2rem;
      display: flex;
      align-items: center;
      justify-content: center;
      background: #FFFFFF;
      border-radius: .2rem;
      transition: .2s;
      &:hover{
        box-shadow: 0 .5rem 1.5rem rgba(0, 0, 0, 0.25);
        transition: .2s;
      } 
      &:before{
        position: absolute;
        content: '';
        width: 1rem;
        height: 1rem;
        background: url('/img/hud/smartphone/messengerTab/chatSection/btn-options.svg') no-repeat center / contain;
      }
    }
  }
  &__main {
    padding: 0.75rem 1rem 0.65rem 1rem;
    display: flex;
    flex-direction: column;
    .title {
      line-height: 1.1rem;
      color: #272727;
      font-weight: bold;
    }
    .text {
      font-size: 0.7rem;
      line-height: 0.9rem;
      color: #454545;
      margin: 0.5rem 0 0 0;
    }
    .info {
      display: flex;
      align-items: center;
      margin: 0.6rem 0 0 0;
      border-top: 1px solid #F1F1F1;
      padding: 0.65rem 0 0 0;
      &__item {
        display: flex;
        align-items: center;
        &:nth-child(2) {
          margin: 0 0 0 1rem;
        }
        &.active {
          .icon {
            background: #EE443A;
          }
          .count {
            color: #272727;
          }
        }
        &.like .icon {
          mask: url("/img/hud/smartphone/messengerTab/chatSection/heart.svg") no-repeat center / contain;
        }
        &.view .icon {
          mask: url("/img/hud/smartphone/messengerTab/chatSection/eye.svg") no-repeat center / contain;
        }
        .icon {
          background: rgba(39, 39, 39, 0.3);
          width: 0.9rem;
          height: 0.9rem;
        }
        .count {
          color: rgba(39, 39, 39, 0.3);
          font-size: 0.65rem;
          margin: 0 0 0 0.25rem;
        }
      }
      &__date {
        margin: 0 0 0 auto;
        font-size: 0.65rem;
        color: rgba(39, 39, 39, 0.3);
      }
    }
  }
}
</style>
