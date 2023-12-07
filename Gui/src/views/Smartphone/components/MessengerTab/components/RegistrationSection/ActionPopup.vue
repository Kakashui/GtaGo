<template>
  <div class="action-popup">
    <button class="action-popup__close" @click="$emit('close')">{{loc('phone:cancel')}}</button>
    <div class="action-popup__title">{{loc('phone:reg:2')}}</div>
    <div class="action-popup__list">
      <div
        v-for="item in items"
        :key="item"
        class="item"
      >{{ loc(item) }}</div>
    </div>
    <div class="action-popup__inputs">
      <input
        v-model="username"
        type="text"
        :placeholder="loc('phone:reg:plh:1')"
        class="input"
        maxlength="25"
      >
      <input
        v-model="displayname"
        type="text"
        :placeholder="loc('phone:reg:plh:2')"
        class="input"
        maxlength="25"
      >
    </div>
    <button class="action-popup__btn" @click="create">{{loc('phone:reg:3')}}</button>
  </div>
</template>

<script>
import { mapGetters } from 'vuex';
export default {
  name: 'ActionPopup',

  data: function() {
    return {
      username: '',
      displayname: '',
      items: [
        'phone:reg:4',
        'phone:reg:5',
        'phone:reg:6'
      ]
    }
  },
  computed:{
    ...mapGetters('localization', ['loc'])
  },
  methods: {
    create: function() {
      const usernameRegexp = /^[a-zA-Z0-9]+$/
      if (!usernameRegexp.test(this.username)) {
        return;
      }
      
      const displaynameRegexp = /^[а-яА-Яa-zA-Z0-9\s]+$/
      if (!displaynameRegexp.test(this.displayname)) {
        return;
      }
      
      window.mp.triggerServer('phone::msg::createAccount', this.username, this.displayname);
    }
  }
}
</script>

<style lang="scss" scoped>
.action-popup {
  z-index: 6;
  height: 25.35rem;
  width: 100%;
  background: #fff;
  position: absolute;
  bottom: 0;
  left: 0;
  border-radius: 0.6rem 0.6rem 0 0;
  display: flex;
  flex-direction: column;
  padding: 0.85rem 0 2.25rem 0;
  &__close {
    background: transparent;
    color: #4E6ED9;
    font-size: 0.7rem;
    line-height: 0.8rem;
    margin: 0 0 1.35rem 0.75rem;
    align-self: flex-start;
    &:hover {
      text-decoration: underline;
    }
  }
  &__title {
    font-size: 1.5rem;
    line-height: 1.5rem;
    color: #020202;
    margin: 0 0 1rem 0.75rem;
    font-weight: bold;
  }
  &__list {
    display: flex;
    flex-direction: column;
    padding: 0 1.25rem 0 0.75rem;
    margin: 0 0 1.5rem 0;
    .item {
      display: flex;
      align-items: flex-start;
      color: rgba(0, 0, 0, 0.5);
      font-size: 0.7rem;
      line-height: 0.8rem;
      &:not(:last-child) {
        margin: 0 0 0.5rem 0;
      }
      &::before {
        content: "";
        min-width: 0.4rem;
        min-height: 0.4rem;
        border-radius: 50%;
        background: #4E6ED9;
        box-shadow: 0px 0.5rem 1.5rem rgba(78, 110, 216, 0.3);
        margin: 0.2rem 0.5rem 0 0;
      }
    }
  }
  &__inputs {
    display: flex;
    flex-direction: column;
    border-top: 1px solid rgba(195, 195, 195, 0.35);
    border-bottom: 1px solid rgba(195, 195, 195, 0.35);
    padding: 0 0 0 1rem;
    background: rgba(195, 195, 195, 0.1);
    margin: 0 0 2.75rem 0;
    .input {
      background: transparent;
      font-size: 0.7rem;
      height: 2.4rem;
      padding: 0 1rem 0 0;
      &::placeholder {
        color: rgba(2, 2, 2, 0.4);
      }
      &:not(:last-child) {
        border-bottom: 1px solid rgba(195, 195, 195, 0.35);
      }
    }
  }
  &__btn {
    font-size: 0.7rem;
    background: #4E6ED9;
    color: #fff;
    box-shadow: 0px 0.5rem 1.5rem rgba(78, 110, 216, 0.3);
    border-radius: 5rem;
    width: 13.2rem;
    min-height: 2rem;
    max-height: 2rem;
    align-self: center;
    &:hover {
      box-shadow: none;
    }
  }
}
</style>
