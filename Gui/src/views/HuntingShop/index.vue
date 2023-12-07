<template>
  <div class="hunting-shop">
    <div class="hunting-shop__logo"></div>
    <div class="btn-exit" @click="exit">{{loc('HuntingShop_1')}}</div>
    <div class="hunting-shop__list">
      <list-item
        v-for="item in this.items"
        :key="item.id"
        :item="item"
      />
    </div>
  </div>
</template>

<script>
import { mapState, mapGetters } from 'vuex'

import ListItem from './ListItem'

export default {
  name: 'HuntingShop',

  components: {
    ListItem
  },

  computed: {
    ...mapState('huntingShop', ['items']),
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    exit: function() {
      window.mp.trigger('huntingStore::close');
    }
  }
}
</script>

<style lang="scss" scoped>
.hunting-shop{
  display: flex;
  flex-flow: column;
  align-items: flex-start;
  justify-content: flex-start;
  height: 100%;
  padding: 3rem 0 0 3rem;
  position: absolute;
  left: 0;
  top: 0;
  text-transform: uppercase;
  text-transform: uppercase;
  color: #FFFFFF;
  letter-spacing: 0.05em;
  font-weight: bold;
  &__logo{
    width: 6.75rem;
    height: 4.5rem;
    background-image: url('/img/huntingShop/logo.png');
    background-size: contain;
    background-repeat: no-repeat;
    background-position: center;
  }
  .btn-exit{
    padding: 0 1.7rem;
    height: 1rem;
    min-height: 1rem;
    display: flex;
    align-items: center;
    justify-content: center;
    background: .9rem 50% / .8rem no-repeat url('/img/huntingShop/arrow.png');
    background-position: left;
    font-size: 1rem;
    margin: 2.5rem 0 1.4rem .65rem;
    transition: .3s;
    &:hover{
      transition: .3s;
      margin-left: 0;
    }
  }
  &__list{
    display: flex;
    flex-flow: column;
  }
}
</style>
