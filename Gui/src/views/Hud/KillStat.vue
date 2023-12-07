<template>
  <div class="killstat__list">
    <span class="list__timer" v-if="killStatTime !== null && killStatTime !== 0">{{ prettyTime }}</span>
    <div
      :class="[{ current: item.id === killstatCurrentUser.id }, 'list__item']"
      v-for="(item, index) in currentItems"
      :key="index"
    >
      <div class="item__content">
        <div class="content__stat">
          <span class="stat__kills">{{ item.kills }}</span>
          <span class="stat__separator">/</span>
          <span class="stat__deaths">{{ item.deaths }}</span>
        </div>
        <span class="content__username">{{ item.username }}</span>
        <span class="content__place">{{ index + 1 }}</span>
      </div>
    </div>
    <div class="list__item current">
      <div class="item__content">
        <div class="content__stat">
          <span class="stat__kills">{{ currentUser.kills }}</span>
          <span class="stat__separator">/</span>
          <span class="stat__deaths">{{ currentUser.deaths }}</span>
        </div>
        <span class="content__username">{{ currentUser.username }}</span>
      </div>
    </div>
  </div>
</template>

<script>
import { mapState } from 'vuex'

export default {
  name: 'KillStat',

  data: function () {
    return {
      timer: null
    }
  },

  computed: {
    ...mapState('hud', [
      'killstatItems',
      'killStatTime',
      'killstatCurrentUser'
    ]),

    currentItems: function () {
      return this.killstatItems.slice(0, 3)
    },

    currentUser: function() {
      return this.killstatItems.find(item => item.id === this.killstatCurrentUser.id)
    },

    prettyTime () {
      let time = this.killStatTime / 60
      let minutes = parseInt(time)
      let secondes = Math.round((time - minutes) * 60)

      if (minutes < 10) {
        minutes = '0'+minutes
      }
      if (secondes < 10) {
        secondes = '0'+secondes
      }

      return `${minutes}:${secondes}`
		}
  }
}
</script>

<style lang="scss" scoped>
.killstat__list {
  display: flex;
  flex-direction: column;
  align-items: center;
  position: absolute;
  bottom: 1rem;
  right: 1rem;
  color: #fff;
  .list__timer {
    font-weight: bold;
    letter-spacing: .05rem;
    transform: rotate(2.2deg);
  }
  .list__item {
    background-image: url('../../../public/img/hudKillStat/stat-last.png');
    background-size: contain;
    background-repeat: no-repeat;
    width: 16.9rem;
    height: 3.5rem;
    &.current {
      background-image: url('../../../public/img/hudKillStat/stat-top.png');
      .item__content .content__stat {
        color: #1fbf75;
      }
    }
    &:last-child {
      .item__content .content__username {
        margin: 0 1.6rem 0 0;
      }
    }
    .item__content {
      display: flex;
      justify-content: flex-end;
      transform: rotate(2.2deg);
      padding: 1rem .5rem 0 1.5rem;
      align-items: center;
      .content__stat {
        color: #ff4654;
        display: flex;
        margin: 0 auto 0 0;
      }
      .content__username {
        text-transform: uppercase;
        margin: 0 1rem 0 0;
      }
      .content__place {
        color: #090e13;
        font-weight: bold;
        font-size: 1.4rem;
      }
    }
    
  }
}
</style>
