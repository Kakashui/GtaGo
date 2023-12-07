<template>
  <div class="tab">
    <div class="tab__header">
      <div class="title">{{loc('auth:greeten:tit')}}</div>
      <div class="desc">{{loc('auth:greeten:desc')}}</div>
    </div>
    <div class="tab__main">
      <div class="contacts">
        <div
          v-for="(item, index) in contacts"
          :key="index"
          class="contacts__item"
        >
          <img :src="`img/auth/icons/${item.icon}.svg`" class="icon">
          <div class="text">{{ item.text }}</div>
        </div>
      </div>
      <div class="action">
        <action-button :text="loc('auth:greeten:action')" @event="someEvent"/>
        <div class="action__desc">{{loc('auth:greeten:cursor')}}</div>
      </div>
    </div>
  </div>
</template>

<script>
import {  mapGetters } from 'vuex'
import ActionButton from '../common/ActionButton'

export default {
  components: { ActionButton },
  name: 'GreetingsTab',

  data: function() {
    return {
      flood: 0,
      clickInterval: 5000,
      contacts: [
        { icon: 'globalization', text: 'eliterp.ru' },
        { icon: 'vk', text: 'vk.com/eliterp_gta5' },
        { icon: 'discord', text: 'discord.gg/46t85fD5ku' }
      ]
    }
  },
  computed:{
    ...mapGetters('localization', ['loc']),
  },

  methods: {
    someEvent: function() {
      if(this.flood > Date.now())return;
      this.flood = Date.now() + this.clickInterval;
      window.mp.triggerServer("auth:characters:request")
    }
  }
}
</script>

<style lang="scss" scoped>
.tab {
  display: flex;
  flex-direction: column;
  &__header {
    position: relative;
    margin: 0 0 1.9rem 0;
    .title {
      font-size: 8.5rem;
      line-height: 8rem;
      font-weight: bold;
      text-transform: uppercase;
      color: #FFFFFF;
      width: 36.1rem;
    }
    .desc {
      display: flex;
      position: absolute;
      top: 0.5rem;
      left: 19rem;
      &::before {
        content: "";
        width: 0.2rem;
        height: 5.95rem;
        margin: 0 1rem 0 0;
        background: linear-gradient(268.14deg, #B0C724 0%, #D5EC45 23.77%, #B6D300 100%);
      }
      font-size: 1.5rem;
      text-transform: uppercase;
      color: #FFFFFF;
      width: 14.5rem;
    }
  }
  &__main {
    display: flex;
    justify-content: space-between;
    .contacts {
      display: flex;
      flex-direction: column;
      &__item {
        display: flex;
        align-self: start;
        &:not(:last-child) {
          margin: 0 0 1rem 0;
        }
        .icon {
          width: 1.5rem;
          height: 1.5rem;
          margin: 0 0.6rem 0 0;
        }
        .text {
          font-size: 1.2rem;
          color: #FFFFFF;
          text-transform: uppercase;
          font-weight: 400;
        }
      }
    }
    .action {
      display: flex;
      flex-direction: column;
      &__desc {
        font-size: 1.2rem;
        color: rgba(255, 255, 255, 0.3);
      }
    }
  }
}
</style>
