<template>
  <div class="team-stats">
    <div class="team-stats__header">
      <div>№</div>
      <div>{{loc('war_6')}}</div>
      <div>{{loc('war_7')}}</div>
    </div>
    <div class="team-stats__body">
      <div 
        class="team-stats__item"
        v-for="(item, index) in getSortedStats"
        :key="item.FamilyId"
      >
        <div class="team-stats__item-index">{{index + 1}}</div>
        <div class="team-stats__item-name">{{item.Name}}</div>
        <div class="team-stats__item-kills">{{item.Kills}}</div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
export default {
  name: 'TeamStats',
  computed: {
    ...mapState('war', ['teamList']),
    ...mapGetters('localization', ['loc']),
    getSortedStats: function() {
      let result = this.teamList.slice().sort((a,b) => b.Kills - a.Kills);
      return result;
    }

  }
}
</script>

<style lang="scss" scoped>
.team-stats{
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
        width: 7%;
      }
      &:nth-child(2) {
        width: 78%;
      }
      &:nth-child(3) {
        width: 15%;
        text-align: center;
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
      width: 7%;
    }
    &-name{
      width: 78%;
    }
    &-kills{
      width: 15%;
      text-align: center;
    }
  }
}
</style>
