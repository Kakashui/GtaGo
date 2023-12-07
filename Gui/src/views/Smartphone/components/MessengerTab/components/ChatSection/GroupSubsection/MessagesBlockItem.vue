<template>
  <div :class="[{ income: messengerData.CurrentAccount.Id !== item.SenderId }, 'messages-block-item']">
    <div v-if="!isIncome" class="messages-block-item__fit"></div>
    <div v-if="!isIncome" class="messages-block-item__info">
      <div :class="[{ read: item.IsRead }, 'indicator']"></div>
      <span class="time">{{ currentTime }}</span>
    </div>
    <div :class="[{ solo: !item.Text, 'solo-img': !item.Text }, 'messages-block-item__main']">
      <img
        v-if="item.Attachment.Type === 2"
        :src="item.Attachment.Url"
        class="img"
      >
      <div class="geolocation" v-if="item.Attachment.Type === 0">
        <div class="geolocation__info">
          <span class="desc">{{loc('phone:group:msgs:1')}}</span>
          <span class="value">{{ loc(`phone:group:msgs:1@${distance}`) }}</span>
        </div>
        <button class="geolocation__btn" @click="setWaypoint"></button>
      </div>
      <span class="title" v-if="messengerData.CurrentAccount.Id !== item.SenderId">{{ senderName }}</span>
      <span ref="text_msg" class="text" v-html="replaceUrlsOnRefs(getTextWithEmoji(item.Text))"></span>
    </div>
    <div v-if="isIncome" class="messages-block-item__info">
      <div :class="[{ read: item.IsRead }, 'indicator']"></div>
      <span class="time">{{ currentTime }}</span>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
import { getTextWithEmoji } from "../common/textEmojiConverter";
import { replaceUrlsOnRefs } from "../common/invitesUrlConverter";
import { addHandlersForInvitesClick } from "../common/invitesClickHandler";

export default {
  name: 'MessagesBlockItem',

  props: {
    item: Object
  },

  computed: {
    ...mapState('smartphone', ['geolocation']),
    ...mapState('smartphone/messagePage', ['messengerData']),
    ...mapGetters('localization', ['loc']),
    isIncome: function() {
      return this.messengerData.CurrentAccount.Id !== this.item.SenderId
    },

    currentTime: function() {
      const date = new Date(this.item.Time)
      const options = { timeZone: 'UTC', hour: 'numeric', minute: 'numeric' }
      
      return date.toLocaleTimeString('ru-RU', options)
    },

    senderName: function() {
      const cachedAccount = this.messengerData._cachedAccounts
        .find(a => a.Id === this.item.SenderId);

      if (cachedAccount) {
        return cachedAccount.DisplayedName;
      }
      else {
        return "...";
      }
    },

    distance: function() {
      if (this.item.Attachment.Type === 0) {
        const pos1 = this.geolocation.currentPosition
        const pos2 = this.item.Attachment

        return Math.ceil(this.calculateDistance(pos1.X, pos2.X, pos1.Y, pos2.Y, pos1.Z, pos2.Z))
      }
      else {
        return null;
      }
    }
  },

  methods: {
    calculateDistance: function(x1, x2, y1, y2, z1, z2) {
      // sqrt((x1 - x2)^2 + (y1 - y2)^2 + (z1 - z2)^2)
      const square = (number) => Math.pow(number, 2) 

      return Math.sqrt(square(x1 - x2) + square(y1 - y2) + square(z1 - z2))
    },
    
    getTextWithEmoji: (text) => getTextWithEmoji(text),

    replaceUrlsOnRefs: (text) => replaceUrlsOnRefs(text),
    
    setWaypoint: function() {
      window.mp.trigger('phone::msg::setGeoposition', this.item.Attachment.X, this.item.Attachment.Y);
    }
  },

  mounted: function() {
    if (!this.item.Attachment || this.item.Attachment === null) {
      this.item.Attachment = {}
    }

    addHandlersForInvitesClick(this.$refs['text_msg'], this.$bus);
  },

  created: function() {
    if (!this.item.Attachment || this.item.Attachment === null) {
      this.item.Attachment = {}
    }
  }
}
</script>

<style lang="scss" scoped>
.messages-block-item {
  margin: 0.75rem 0.5rem 0 0.75rem;
  display: flex;
  align-items: flex-end;
  &.income {
    margin-left: 0.3rem;
    .messages-block-item__info {
      margin-left: 0.3rem;
      .indicator {
        display: none;
      }
    }
    .geolocation {
      &__info {
        .desc {
          color: #656F82;
        }
        .value {
          color: #4E6ED9;
        }
      }
      &__btn {
        background: #4E6ED9;
        &::after {
          background: #fff;
        }
      }
    }
    .messages-block-item__main {
      border-radius: 1rem 1rem 1rem 0;
      max-width: max-content;
      background: #FFFFFF;
      color: #464646;
    }
  }
  &__info {
    display: flex;
    flex-direction: column;
    align-items: flex-end;
    .indicator {
      width: 0.8rem;
      height: 0.8rem;
      background: #A7B7EC;
      mask: url("/img/hud/smartphone/messengerTab/chatSection/doubleTick.svg") no-repeat center / contain;
      &.read {
        background: #4E6ED9;
      }
    }
    .time {
      color: #A7B7EC;
      font-size: 0.65rem;
      line-height: 0.8rem;
    }
  }
  &__fit {
    width: 5000px;
  }
  &__main {
    max-width: max-content;
    min-width: fit-content;
    flex: auto;
    display: flex;
    flex-direction: column;
    align-items: flex-end;
    margin: 0 0 0 0.3rem;
    font-size: 0.7rem;
    line-height: 0.9rem;
    color: #fff;
    background: #4E6ED9;
    border-radius: 1rem 1rem 0 1rem;
    padding: 0.3rem;
    &.solo {
      .img {
        margin: 0;
      }
      .geolocation {
        margin: 0;
      }
    }
    &.solo-img {
      padding: 0;
      border-radius: 1rem;
    }
    .geolocation {
      display: flex;
      align-items: flex-end;
      justify-content: space-between;
      height: 5.5rem;
      width: 100%;
      border-radius: 0.8rem;
      margin: 0 0 0.4rem 0;
      padding: 0 0.45rem 0.55rem 0.45rem;
      background: center / cover no-repeat url("/img/hud/smartphone/messengerTab/chatSection/geolocationBg.png");
      &__info {
        display: flex;
        flex-direction: column;
        font-size: 0.7rem;
        margin: 0 0 0.2rem 0;
        .desc {
          color: #fff;
          line-height: 0.9rem;
          margin: 0 0 0.05rem 0;
        }
        .value {
          color: #fff;
          line-height: 0.8rem;
          font-weight: 500;
        }
      }
      &__btn {
        width: 1.8rem;
        height: 1.8rem;
        border-radius: 0.5rem;
        background: #fff;
        box-shadow: 0px 0.5rem 1.5rem rgba(78, 110, 216, 0.3);
        transition: all 0.15s;
        display: flex;
        align-items: center;
        justify-content: center;
        &::after {
          content: "";
          width: 0.8rem;
          height: 0.8rem;
          mask: center / cover no-repeat url("/img/hud/smartphone/messengerTab/chatSection/geoMark.svg");
          background: #4E6ED9;
        }
        &:hover {
          box-shadow: none;
        }
      }
    }
    .img {
      width: 100%;
      height: auto;
      border-radius: 0.8rem;
      margin: 0 0 0.4rem 0;
    }
    .title {
      font-size: 0.65rem;
      line-height: 0.75rem;
      color: #4E6ED9;
      margin: 0 0 0.15rem 0;
      padding: 0 0.6rem;
    }
    .text {
      display: block;
      padding: 0 0.6rem;
    }
  }
}
</style>
