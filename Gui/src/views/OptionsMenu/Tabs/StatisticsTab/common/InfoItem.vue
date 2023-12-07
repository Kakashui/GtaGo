<template>
  <div class="info-item">
    <img :src="`img/optionsMenu/statisticsTab/icons/${item.icon}.svg`" class="info-item__icon">
    <div v-if="item.key === 'maritalStatus'" class="info-item__main">
      <div class="value" v-if="statistics[item.key].secondHalf">
        <div class="value__desc" v-if="statistics[item.key].sex === 0">{{ loc(item.title3) }}</div>
        <div class="value__desc" v-else-if="statistics[item.key].sex === 1">{{ loc(item.title2) }}</div>
        <div class="value__name">{{ statistics[item.key].secondHalf }}</div>
      </div>
      <div class="value" v-else>
        <div class="value__desc" >{{ loc(item.title1) }}</div>
        <div class="value__name" v-if="statistics[item.key].sex === 0">{{loc('mmain:stats:info:freef')}}</div>
        <div class="value__name" v-else>{{loc('mmain:stats:info:freem')}}</div>
      </div>
    </div>
    <div v-else class="info-item__main">
      <div class="desc">{{ loc(item.title) }}</div>
      <div class="value">{{ item.key == "smart" ? configuration.Number : loc(statistics[item.key]) }}</div>
    </div>
  </div>
</template>

<script>
import { mapState, mapGetters } from 'vuex'

export default {
  name: 'InfoItem',

  props: {
    item: Object
  },

  computed: {
    ...mapState('optionsMenu', ['statistics']),
    ...mapGetters('localization', ['loc']),
    ...mapState('smartphone', ['configuration'])
  }
}
</script>

<style lang="scss" scoped>
.info-item {
  display: flex;
  &__icon {
    width: 2rem;
    height: 2rem;
    margin: 0.25rem 0 0 0;
  }
  &__main {
    display: flex;
    flex-direction: column;
    margin: 0 0 0 1rem;
    text-transform: uppercase;
    letter-spacing: 0.03em;
    .desc {
      font-size: 0.9rem;
      color: rgba(255, 255, 255, 0.4);
      margin: 0 0 0.25rem 0;
      font-weight: 300;
    }
    .value {
      font-size: 1.1rem;
      line-height: 1.3rem;
      color: #fff;
      &__desc {
        font-size: 0.9rem;
        color: rgba(255, 255, 255, 0.4);
        margin: 0 0 0.25rem 0;
        font-weight: 300;
      }
      &__name {
        font-size: 1.1rem;
        line-height: 1.1rem;
      }
    }
  }
}
</style>
