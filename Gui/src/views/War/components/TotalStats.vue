<template>
  <div class="total-stats">
    <div class="total-stats__header">
      <div>№</div>
      <div>{{loc('war_3')}}</div>
      <div>{{loc('war_4')}}</div>
      <div>{{loc('war_5')}}</div>
    </div>
    <div class="total-stats__body">
      <div 
        class="total-stats__item"
        v-for="(item, index) in getSortedStats"
        :key="item.FamilyId"
      >
        <div class="total-stats__item-index">{{index + 1}}</div>
        <div class="total-stats__item-name"><span class="total-stats__item-logo">{{item.FamilyName.slice(0,2)}}</span>{{item.FamilyName}}</div>
        <div class="total-stats__item-kills">{{item.Kills}}</div>
        <div class="total-stats__item-score">{{item.Points}}</div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
export default {
  name: 'TotalStats',
  computed: {
    ...mapState('war', ['totalList']),
    ...mapGetters('localization', ['loc']),
    getSortedStats: function() {
      let result = this.totalList.slice().sort((a,b) => b.Points - a.Points);
      return result;
    }
  }
}
</script>

<style lang="scss" scoped>
.total-stats{
  width: 100%;
  height: 100%;
  display: flex;
  flex-direction: column;
  font-weight: bold;
  font-size: 1.2rem;
  &__header{
    display: flex;
    align-items: center;
    border-bottom: 1px solid rgba(255, 255, 255, 0.2);
    padding-bottom: 1rem;
    color: rgba(255, 255, 255, 0.3);
    width: calc(100% - (1rem));
    &>div{
      &:nth-child(1) {
        width: 13%;
      }
      &:nth-child(2) {
        width: 56%;
      }
      &:nth-child(3) {
        width: 16%;
      }
      &:nth-child(4) {
        width: 14%;
      }
    }
  }
  &__body{
    padding-top: 1.5rem;
    overflow-y: auto;
    height: 100%;
    padding-right: 1rem;
    &::-webkit-scrollbar{
      width: .2rem;
      background-color: transparent;
      &-thumb{
        background: #B6D300;
        box-shadow: 0 1rem 4.5rem rgba(182, 211, 0, 0.6);
      }
    }
  }
  &__item{
    display: flex;
    align-items: center;
    width: 100%;
    height: 2.5rem;
    margin-bottom: .8rem;
    &-index{
      width: 13%;
    }
    &-logo{
      position: absolute;
      left: -.7rem;
      transform: translateX(-100%);
      border-radius: 50%;
      width: 2.5rem;
      height: 2.5rem;
      display: flex;
      align-items: center;
      justify-content: center;
      background: rgba(255, 255, 255, 0.1);
    }
    &-name{
      width: 56%;
      position: relative;
      display: flex;
      align-items: center;
    }
    &-kills{
      width: 16%;
    }
    &-score{
      width: 14%;
    }
  }
}
</style>
