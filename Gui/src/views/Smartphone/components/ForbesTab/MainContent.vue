<template>
  <div class="main-content">
    <div class="main-content__header">
      <span class="title">Forbes</span>
      <p class="desc">{{loc('phone:forbes:1')}}</p>
    </div>
    <div>
      <div class="main-content__main">
        <div
          class="list__item"
          v-for="(item, index) in currentItems"
          :key="index"
        >
          <div class="state">
            <span class="state__position">{{ formNumber(index) }}</span>
            <img
              src="img/hud/smartphone/forbesTab/arrowUp.svg"
              alt="up"
              class="state__icon"
              v-if="item.status === 'up'"
            >
            <img
              src="img/hud/smartphone/forbesTab/arrowDown.svg"
              alt="down"
              class="state__icon"
              v-else-if="item.status === 'down'"
            >
          </div>
          <div class="info">
            <span class="info__username">{{ item.Name }}</span>
            <span class="info__desc">{{loc('phone:forbes:2')}}</span>
            <span class="info__capital">${{ item.Money.toLocaleString('en-US') }}</span>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapMutations } from 'vuex'

export default {
  name: 'MainContent',

  props: {
    forbesItems: Array
  },

  computed: {
    currentItems: function() {
      return this.sortItems(this.forbesItems)
    },
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    ...mapMutations('smartphone', ['setColorTheme']),

    sortItems: function(items) {
      return items.sort((a, b) => b.Money - a.Money)
    },

    formNumber: function(value) {
      const current = value + 1
      if (current < 10) return `0${current}`
      return current
    }
  },

  mounted: function() {
    this.setColorTheme({ header: 'dark', button: 'light' })
  }
}
</script>

<style lang="scss" scoped>
@import url('https://fonts.googleapis.com/css2?family=Playfair+Display:wght@400;700&display=swap');
.main-content::v-deep .vb > .vb-dragger {
  margin: 0 0.1rem 0 0;
}
.main-content {
  height: 100%;
  // display: flex;
  // flex-direction: column;
  &__header {
    padding: 2.5rem 0 1.4rem 0;
    background: #FFFFFF;
    display: flex;
    flex-direction: column;
    align-items: center;
    text-transform: uppercase;
    .title {
      font-family: 'Playfair Display', serif;
      font-weight: 700;
      color: #13171C;
    }
    .desc {
      font-size: 0.7rem;
      color: #81858A;
      margin: 0.25rem 0 0 0;
    }
  }
  &__main {
    // background: center / cover no-repeat url("/img/hud/smartphone/forbesTab/layer.png"), #000;
    height: 24.8rem;
    overflow-y: scroll;
    &::-webkit-scrollbar {
      width: 0;
    }
    &::-webkit-scrollbar-track {
      border: solid 0 transparent;
      margin: .1rem;
    }
    &::-webkit-scrollbar-thumb {
      border: solid 0 transparent;      
      background-color: rgba(#fff,.1);
    }
    .list {
      &__item {
        display: flex;
        padding: 0.5rem 0 0.75rem 0.5rem;
        border-bottom: 1px solid rgba(255, 255, 255, 0.1);
        .state {
          display: flex;
          flex-direction: column;
          position: relative;
          &__position {
            font-size: 1.1rem;
            font-weight: 900;
            color: rgba(255, 255, 255, 0.3);
            line-height: 1.635rem;
          }
          &__icon {
            width: 0.55rem;
            height: 0.55rem;
            position: absolute;
            transform: translate(-50%, -50%);
            top: 60%;
            left: 50%;
          }
        }
        .info {
          display: flex;
          flex-direction: column;
          margin: 0 0 0 0.5rem;
          &__username {
            font-family: 'Playfair Display', serif;
            font-weight: 700;
            font-size: 1.1rem;
            color: #fff;
          }
          &__desc {
            font-size: 0.7rem;
            font-weight: 300;
            color: #5F5F5F;
          }
          &__capital {
            font-weight: 500;
            color: #B6D300;
          }
        }
      }
    }
  }
}
</style>
