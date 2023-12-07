<template>
  <div class="messages-item">
    <div v-if="item.type === 1" class="messages-item__admin">
      <div class="avatar">Admin</div>
      <div class="main">
        <div class="main__header">
          <div class="info">{{loc('mmain:rep:answ:tit')}}: {{ item.name }} [id {{ item.id }}]</div>
          <div class="time">{{ formatTime(item.time) }}</div>
        </div>
        <div class="main__text">{{ item.text }}</div>

      </div>
    </div>
    <div v-else class="messages-item__user">
      <div class="header">{{ formatTime(item.time) }}</div>
      <div class="text">{{ item.text }}</div>
    </div>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
export default {
  name: 'MessagesItem',

  props: {
    item: Object
  },
  computed: {
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    formatTime: function(value) {
      const date = new Date(value)
      const day = date.toLocaleDateString('ru-RU')
      const time = date.toLocaleTimeString('ru-RU')

      return `${day} ${time}`
    }
  }
}
</script>

<style lang="scss" scoped>
.messages-item {
  margin: 0 0 1.25rem 0;
  &__admin {
    display: flex;
    .avatar {
      min-width: 2.5rem;
      max-width: 2.5rem;
      height: 2.5rem;
      border-radius: 50%;
      background: #F4C10B;
      box-shadow: 0px 0.5rem 4rem rgba(244, 193, 11, 0.3);
      display: flex;
      align-items: center;
      justify-content: center;
      font-size: 0.8rem;
      letter-spacing: 0.03em;
      color: #FFFFFF;
      margin: 0 0.75rem 0 0;
    }
    .main {
      display: flex;
      flex-direction: column;
      letter-spacing: 0.03em;
      max-width: 33rem;
      min-width: 20rem;
      &__header {
        display: flex;
        justify-content: space-between;
        color: rgba(255, 255, 255, 0.5);
        font-size: 0.9rem;
        margin: 0 0 0.25rem 0;
      }
      &__text {
        padding: 1.25rem 1.5rem;
        background: rgba(255, 255, 255, 0.05);
        border-radius: 0.2rem 0.6rem 0.6rem 0.6rem;
        color: #FFFFFF;
        margin: 0 0 0.5rem 0;
      }
      &__action {
        display: flex;
        .desc {
          font-size: 0.9rem;
          letter-spacing: 0.03em;
          color: rgba(255, 255, 255, 0.5);
        }
      }
    }
  }
  &__user {
    display: flex;
    flex-direction: column;
    align-items: flex-end;
    .header {
      color: rgba(255, 255, 255, 0.5);
      font-size: 0.9rem;
      margin: 0 0 0.25rem 0;
    }
    .text {
      padding: 1.25rem 1.5rem;
      background: rgba(255, 255, 255, 0.05);
      border-radius: 0.6rem 0.6rem 0.2rem 0.6rem;
      color: #FFFFFF;
      margin: 0 0 0.5rem 0;
      text-align: right;
      max-width: 33rem;
      min-width: 20rem;
    }
  }
}
</style>
