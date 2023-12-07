<template>
  <div class="confirm-popup">
    <span class="confirm-popup__title">{{loc('phone:chat:confirm:1')}}</span>
    <span class="confirm-popup__desc">{{loc('phone:chat:confirm:2')}}</span>
    <button class="confirm-popup__btn confirm-popup__btn-top" @click="unblock">{{loc('phone:chat:confirm:3')}}</button>
    <!-- <button class="confirm-popup__btn confirm-popup__btn-bottom" @click="setCurrentTab({ key: currentTab.key, section: 'ViewedProfileSection', data: { location: { key: currentTab.key, section: currentTab.section, subsection: currentTab.subsection }} })">{{loc('phone:chat:confirm:4')}}</button> -->
    <div class="confirm-popup__close" @click="$emit('close')">{{loc('phone:chat:confirm:5')}}</div>
  </div>
</template>

<script>
import { mapState, mapMutations, mapGetters } from 'vuex'

export default {
  name: 'ConfirmPopup',

  props: {
    selectedUserId: Number
  },

  computed: {
    ...mapState('smartphone', ['currentTab']),
    ...mapState('smartphone/messagePage', ['messengerData']),
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    ...mapMutations('smartphone', ['setCurrentTab']),

    unblock: function() {
      window.mp.triggerServer('phone::msg::manage::unblockUser', this.messengerData.CurrentChat.Id, this.selectedUserId)
      this.$emit('close');
    }
  }
}
</script>

<style lang="scss" scoped>
.confirm-popup {
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  margin: auto;
  padding: 1.75rem 1.5rem 1.5rem 1.5rem;
  background: #FAFAFA;
  border-radius: 0.6rem;
  display: flex;
  flex-direction: column;
  align-items: center;
  width: 14.2rem;
  height: fit-content;
  &__title {
    text-align: center;
    font-weight: 500;
    line-height: 1.15rem;
    color: #020202;
    margin: 0 0 0.4rem 0;
  }
  &__desc {
    line-height: 0.9rem;
    font-size: 0.7rem;
    text-align: center;
    color: rgba(2, 2, 2, 0.4);
    margin: 0 0 1.25rem 0;
  }
  &__btn {
    width: 100%;
    height: 2rem;
    border-radius: 5rem;
    transition: all 0.15s;
    font-size: 0.7rem;
  }
  &__btn-top {
    background: #4E6ED9;
    box-shadow: 0px 0.5rem 1.5rem rgba(78, 110, 216, 0.3);
    color: #fff;
    margin: 0 0 0.5rem 0;
    &:hover {
      box-shadow: none;
    }
  }
  &__btn-bottom {
    background: rgba(78, 110, 217, 0.15);
    color: #4E6ED9;
    margin: 0 0 1rem 0;
  }
  &__close {
    color: #4E6ED9;
    font-size: 0.7rem;
    &:hover {
      text-decoration: underline;
    }
  }
}
</style>
