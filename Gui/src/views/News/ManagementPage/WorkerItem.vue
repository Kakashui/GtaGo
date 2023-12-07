<template>
  <div class="worker-item">
    <div class="worker-item__info">
      <div class="worker-item__info-name">{{item.workerName}}</div>
      <div class="worker-item__info-post">{{currentRankName}}</div>
    </div>
  </div>
</template>

<script>
import { mapState } from 'vuex'
export default {
  name: 'WorkerItem',

  props: {
    item: Object
  },

  computed: {
    ...mapState('news', ['getCurrentRank', 'ranksList']),
    currentRankName: function() {
      let list = this.ranksList
      return list.find(element => element.rankId === this.item.workerRank).rankName
    }
  }
}
</script>

<style lang="scss" scoped>
.worker-item{
  background: #FFFFFF;
  border-radius: .3rem;
  padding: 1.2rem 1.5rem;
  display: flex;
  align-items: center;
  justify-content: space-between;
  &__info{
    display: flex;
    flex-direction: column;
    &-name, &-post{
      font-weight: normal;
      font-size: 1rem;
      line-height: 1.15rem;
    }
    &-name{
      color: #000000;
      margin-bottom: .2rem;
    }
    &-post{
      color: rgba(0, 0, 0, 0.45);
    }
  }
}
</style>
