<template>
  <div class="extra-item">
    <img
      v-if="item.key === 'premium' && vipActive"
      :src="`img/optionsMenu/statisticsTab/icons/${item.icon1}.png`"
      class="extra-item__img"
    >
    <img
      v-else
      :src="`img/optionsMenu/statisticsTab/icons/${item.icon}.png`"
      class="extra-item__img"
    >
    <div :class="[item.icon, 'extra-item__main']" v-if="item.key === 'premium'">
      <div class="header">
        <div class="header__desc">{{ item.title }}</div>
        <!-- <div class="header__value">{{ statistics[item.key] ? 'on' : 'off' }}</div> -->
        <div v-if="vipActive" class="header__value on">on</div>
        <div v-else class="header__value">off</div>
      </div>
      <div class="main">{{ loc(item.text) }}</div>
    </div>
    <div :class="[item.icon, 'extra-item__main']" v-else>
      <div class="header">{{ loc(item.title) }}</div>
      <div class="info">
        <div class="info__item">
          <div class="desc">{{ loc(item.textFirst) }}</div>
          <div class="value">{{ statistics[item.keyFirst] }}</div>
        </div>
        <div class="info__item">
          <div class="desc">{{ loc(item.textSecond) }}</div>
          <div class="value">{{ statistics[item.keySecond] }}</div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapState, mapGetters } from 'vuex'

export default {
  name: 'ExtraItem',

  props: {
    item: Object
  },

  computed: {
    ...mapState('optionsMenu', ['statistics']),
    ...mapGetters('localization', ['loc']),
    ...mapState('newDonateShop/prime',['days']),
    vipActive(){
      return this.days > 0
    }
  }
}
</script>

<style lang="scss" scoped>
.extra-item {
  display: flex;
  &:not(:last-child) {
    margin: 0 0 2.5rem 0;
  }
  &__img {
    width: 4.4rem;
    height: 4.4rem;
    margin: 0 1rem 0 0;
  }
  &__main {
    display: flex;
    flex-direction: column;
    &.premium {
      .header {
        font-size: 2.5rem;
        line-height: 2.5rem;
        letter-spacing: 0.03em;
        display: flex;
        align-items: center;
        margin: 0 0 0.3rem 0;
        &__desc {
          font-weight: 300;
          color: #FFFFFF;
          margin: 0 0.5rem 0 0;
        }
        &__value {
          color: rgba(255, 255, 255, 0.2);
          font-weight: bold;
          &.on {
            background: url("/img/optionsMenu/statisticsTab/textureGold.png");
            -webkit-background-clip: text;  
            background-clip: text;
            color: transparent;
          }
        }
      }
      .main {
        font-size: 0.9rem;
        line-height: 1.1rem;
        letter-spacing: 0.03em;
        color: rgba(255, 255, 255, 0.3);
        width: 12.5rem;
      }
    }
    &.alert {
      .header {
        font-size: 2.5rem;
        line-height: 2.5rem;
        letter-spacing: 0.03em;
        color: #FFFFFF;
        margin: 0 0 0.5rem 0;
      }
      .info {
        display: flex;
        &__item {
          font-size: 1.5rem;
          line-height: 1.8rem;
          letter-spacing: 0.03em;
          display: flex;
          &:not(:last-child) {
            margin: 0 1.5rem 0 0;
          }
          .desc {
            color: rgba(255, 255, 255, 0.3);
            margin: 0 0.5rem 0 0;
          }
          .value {
            color: #FFFFFF;
          }
        }
      }
    }
  }
}
</style>
