<template>
  <div class="joinchat-popup">
    <div class="joinchat-popup__header">
      <button class="close" @click="$emit('close')">{{loc('phone:cancel')}}</button>
      <span class="title">{{loc('phone:chat:join:2')}}</span>
      <p class="desc">{{loc('phone:chat:join:3')}}</p>
    </div>
    <button class="joinchat-popup__btn" @click="join">{{loc('phone:chat:join:4')}}</button>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
export default {
  name: 'JoinChatPopup',

  props: ['inviteCode'],

  computed: {
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    join: function() {
      this.$emit('close');
      window.mp.triggerServer('phone::msg::joinChatByInvite', this.inviteCode);
    }
  }
}
</script>

<style lang="scss" scoped>
.joinchat-popup {
  position: absolute;
  bottom: -0.05rem;
  left: 0;
  width: 100%;
  height: 10.75rem;
  background: #FAFAFA;
  border-radius: 0.6rem 0.6rem 0 0;
  display: flex;
  flex-direction: column;
  z-index: 44;
  &__header {
    display: flex;
    flex-direction: column;
    padding: 1rem;
    .close {
      align-self: flex-start;
      font-size: 0.7rem;
      line-height: 16px;
      margin: 0 0 0.7rem 0;
      background: transparent;
      color: #4E6ED9;
      &:hover {
        text-decoration: underline;
      }
    }
    .title {
      font-size: 1.3rem;
      line-height: 1.5rem;
      font-weight: bold;
      color: #020202;
      margin: 0 0 0.25rem 0;
    }
    .desc {
      font-size: 0.7rem;
      line-height: 0.9rem;
      color: rgba(2, 2, 2, 0.4);
    }
  }
  &__btn {
    height: 2rem;
    width: 13.2rem;
    box-shadow: 0px 10px 30px rgba(78, 110, 216, 0.3);
    background: #4E6ED9;
    color: #fff;
    font-size: 0.7rem;
    border-radius: 5rem;
    transition: all 0.15s;
    margin: auto auto 1.5rem auto;
    &:hover {
      box-shadow: none;
    }
  }
}
</style>
