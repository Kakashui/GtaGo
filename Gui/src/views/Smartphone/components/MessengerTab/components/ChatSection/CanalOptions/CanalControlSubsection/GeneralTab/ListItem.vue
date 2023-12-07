<template>
  <div class="list-item" @click="handleClick">
    <div class="list-item__avatar">
      <div class="indicator" v-if="item.IsOnline"></div>
    </div>
    <div class="list-item__main">
      <span class="username">{{ loc(item.DisplayedName) }}</span>
      <span class="position">{{ loc(item.AdminLvl === 2 ? 'phone:chat:gen:owner' : 'phone:chat:gen:admin') }}</span>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapState } from "vuex";

export default {
  name: 'ListItem',

  props: { item: Object },

  computed: {
    ...mapState('smartphone/messagePage', ['messengerData']),
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    handleClick: function() {
      this.messengerData.CanalProfile = this.item;
      this.$emit('event');
    }
  }
}
</script>

<style lang="scss" scoped>
.list-item {
  height: 3rem;
  display: flex;
  align-items: center;
  justify-content: space-between;
  &:last-child .list-item__main {
    border-color: transparent;
  }
  &__avatar {
    width: 2rem;
    height: 2rem;
    position: relative;
    margin: 0 0 0 0.75rem;
    background: center / contain no-repeat url("/img/hud/smartphone/messengerTab/chatSection/canalOptions/avatar.png");
    .indicator {
      width: 0.6rem;
      height: 0.6rem;
      position: absolute;
      top: 1px;
      right: 1px;
      background: #4BDB4B;
      border: 0.1rem solid #FFFFFF;
      border-radius: 50%;
    }
  }
  &__main {
    display: flex;
    flex-direction: column;
    justify-content: center;
    height: 100%;
    padding: 0 0.85rem 0 0;
    border-bottom: 1px solid rgba(195, 195, 195, 0.35);
    .username {
      width: 11.1rem;
      white-space: nowrap;
      overflow: hidden;
      text-overflow: ellipsis;
      color: #020202;
      font-size: 0.8rem;
      line-height: 0.95rem;
    }
    .position {
      color: #9E9EA2;
      font-size: 0.65rem;
      line-height: 0.8rem;
      margin: 0.25rem 0 0 0;
    }
  }
}
</style>
