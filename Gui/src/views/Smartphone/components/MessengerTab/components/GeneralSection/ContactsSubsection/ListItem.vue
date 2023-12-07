<template>
  <div class="list-item" @click="msg_openAccount(item.AccountId)">
    <div class="list-item__avatar">
      <div class="indicator" v-if="item.IsOnline"></div>
    </div>
    <div class="list-item__main">
      <span class="username">{{ item.Name }}</span>
      <span class="lastVisit">{{ getLastVisit(item.IsOnline, item.Time) }}</span>
    </div>
  </div>
</template>

<script>
import { mapActions, mapGetters } from 'vuex';

export default {
  name: 'ListItem',

  props: { item: Object },
  computed: {
    ...mapGetters('localization', ['loc'])
  },
  methods: {
    ...mapActions("smartphone/messagePage", ["msg_openAccount"]),

    getLastVisit: function(isOnline, lastVisitTime) {
      if (!isOnline) {
        const date = new Date(lastVisitTime);

        const dayMonth = `${this.getDoubleDigit(date.getUTCDate())}.${this.getDoubleDigit(date.getUTCMonth() + 1)}`;
        const hoursMinutes = `${this.getDoubleDigit(date.getUTCHours())}:${this.getDoubleDigit(date.getUTCMinutes())}`;
        
        return this.loc(`phone:contact:2@${dayMonth}@${hoursMinutes}`);
      }
      else {
        return 'Online';
      }      
    },

    getDoubleDigit: function(number) {
      return number < 10 ? `0${number}` : number;
    }
  }
}
</script>

<style lang="scss" scoped>
.list-item {
  display: flex;
  align-items: center;
  height: 3rem;
  &:last-child .list-item__main {
    border: none;
  }
  &:hover .list-item__main .username {
    transform: translateY(-.1rem);
  }
  &__avatar {
    width: 2rem;
    height: 2rem;
    background: center / contain no-repeat url("/img/hud/smartphone/messengerTab/user.svg");
    position: relative;
    margin: 0 0 0 0.75rem;
    .indicator {
      width: 0.6rem;
      height: 0.6rem;
      border: 0.1rem solid #FFFFFF;
      background: #4BDB4B;
      position: absolute;
      top: 1px;
      right: 1px;
      border-radius: 50%;
    }
  }
  &__main {
    flex: auto;
    height: 100%;
    display: flex;
    flex-direction: column;
    justify-content: center;
    border-bottom: 1px solid rgba(195, 195, 195, 0.35);
    margin: 0 0 0 0.5rem;
    .username {
      font-size: 0.8rem;
      line-height: 0.95rem;
      width: 10rem;
      color: #020202;
      white-space: nowrap;
      overflow: hidden;
      text-overflow: ellipsis;
    }
    .lastVisit {
      font-size: 0.65rem;
      line-height: 0.8rem;
      color: #9E9EA2;
    }
  }
}
</style>
