<template>
  <div class="rank-item">
    <div class="rank-item-position">{{item.rankId}}</div>
    <div class="rank-item__info">
      <div class="rank-item__info-name">{{loc(item.rankName)}}</div>
      <div class="rank-item__info-post">$ {{item.income}}</div>
    </div>
    <div class="rank-item__options">
      <OptionItem
        v-for="option in item.accessList"
        :key="option.id"
        :option="option"
        @setRankOption="setRankOption"
      />
    </div>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
import OptionItem from './OptionItem.vue'
export default {
  name: 'RankItem',

  components: {
    OptionItem
  },
  computed: {
    ...mapGetters('localization', ['loc'])
  },
  props: {
    item: Object
  },

  methods: {   
    setRankOption: function(optionObject) {
      window.mp.trigger('news:setRankOption', this.item.rankId, optionObject)
    }
  }
}
</script>

<style lang="scss" scoped>
.rank-item{
  background: #FFFFFF;
  border-radius: .3rem;
  padding: 0 1.5rem;
  color: #000000;
  display: flex;
  position: relative;
  &-position{
    font-style: normal;
    font-weight: 800;
    font-size: 8.35rem;
    line-height: 9.55rem;
    color: #F3F5F7;
    position: absolute;
    left: -.55rem;
    bottom: -1.9rem;
    z-index: 0;
  }
  &__info, &__options{
    padding: 1.2rem 0;
    z-index: 1;
  }
  &__info{
    min-width: 10.85rem;
    position: relative;
    &:before{
      content: '';
      height: 100%;
      width: 1px;
      background: #E7EAEE;
      transform: matrix(-1, 0, 0, 1, 0, 0);
      position: absolute;
      right: 0;
      top: 0;
    }
    &-name{
      font-weight: bold;
      font-size: 1.1rem;
      line-height: 1.25rem;
      margin-bottom: .5rem;
      max-width: 9.35rem;
    }
    &-post{
      font-weight: normal;
      font-size: 1rem;
      line-height: 1.15rem;
      position: relative;
      display: flex;
      align-items: center;
      &:before{
        content: '';
        width: 1rem;
        height: 1rem;
        background-size: contain;
        background-repeat: no-repeat;
        background-position: center;
        background-image: url('/img/news/icon-cash.svg');
        margin-right: .6rem;
      }
    }
  }
  &__options{
    padding-left: 2.35rem;
    display: flex;
    flex-direction: column;
    align-items: flex-start;
    justify-content: space-between;
  }
}
</style>
