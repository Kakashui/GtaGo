<template>
  <div class="top-worker">
    <div class="top-worker-index">{{index + 1}}</div>
    <div class="top-worker__info">
      <div class="top-worker__info-name">{{worker.workerName}}</div>
      <div class="top-worker__info-post">{{currentRank.rankName}}</div>
    </div>
    <div class="top-worker-rating">{{worker.editedPerDay}}</div>
  </div>
</template>

<script>
import { mapState } from 'vuex'
export default {
  name: 'TopWorker',

  props: {
    worker: Object,
    index: Number
  },

  computed: {
    ...mapState('news', ['ranksList']),
    currentRank: function() {
      let list = this.ranksList
      return list.find(element => element.rankId === this.worker.workerRank)
    } 
  }
}
</script>

<style lang="scss" scoped>
.top-worker{
  display: flex;
  align-items: center;
  &-index{
    margin-right: 1.5rem;
  }
  &__info{
    display: flex;
    flex-direction: column;
    margin-right: 1.5rem;
    width: 100%;
    &-name{
      margin-bottom: .2rem;
    }
    &-post{
      color: rgba(0, 0, 0, 0.4);
    }
  }
  &-rating{
    min-width: 2rem;
  }
}
</style>
