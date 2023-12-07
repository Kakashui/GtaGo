<template>
  <div class="wedding-notification">
    <div class="wedding-notification__header">
      <img
        src="img/weddingNotification/ring.png"
        alt="ring"
        class="header__img"
      >
      <div class="header__title">
        <span class="title__name">{{weddingName}}</span>
        <span class="title__text">{{loc('gui_748')}}</span>
      </div>
    </div>
    <div class="wedding-notification__actions">
      <button class="actions__btn" @click="apply">{{loc('gui_749')}}</button>
      <button class="actions__btn" @click="decline">{{loc('gui_750')}}</button>
    </div>
  </div>
</template>

<script>
import { mapState, mapGetters } from 'vuex'

export default {
  name: 'WeddingNotification',

  computed: {
    ...mapState('weddingMenu', [
      'weddingName'
    ]),
    ...mapGetters('localization',['loc'])
  },

  methods: {
    apply: function () {
      window.mp.trigger('marriage:apply')
    },

    decline: function () {
      window.mp.trigger('marriage:decline')
    }
  }
}
</script>

<style lang="scss" scoped>
.wedding-notification {
  background: rgba(255, 255, 255, 0.95);
  border-radius: 6px;
  align-self: flex-end;
  padding: 20px 110px;
  margin: 0 0 50px 0;
  &__header {
    display: flex;
    align-items: center;
    .header__img {
      max-width: 82px;
      max-height: 82px;
    }
    .header__title {
      display: flex;
      flex-direction: column;
      .title__name {
        color: #64B5F6;
        text-transform: uppercase;
        font-size: 24px;
        font-weight: bold;
      }
      .title__text {
        text-transform: uppercase;
        font-size: 24px;
        font-weight: bold;
      }
    }
  }
  &__actions {
    margin: 40px 0 0 0;
    display: flex;
    justify-content: space-between;
    .actions__btn {
      background: #363636;
      border-radius: 6px;
      border: 0;
      width: 168px;
      height: 38px;
      color: #fff;
    }
  }
}
</style>
