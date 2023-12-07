<template>
  <div class="rating-item">
    <div class="rating-item__info" v-if="currentRank">
      <div class="rating-item__info-name">{{item.workerName}}</div>
      <div class="rating-item__info-post">{{currentRank.rankName}}</div>
    </div>
    <div>{{item.editedPerDay}}</div>
    <div>{{item.editedPerMonth}}</div>
    <div>{{item.rating}}</div>
    <div class="rating-item-index">{{index + 1}}</div>
  </div>
</template>

<script>
import { mapState } from 'vuex'
export default {
  name: 'RatingItem',

  props: {
    index: Number,
    item: Object
  },

  computed: {
    ...mapState('news', ['ranksList']),
    currentRank: function() {
      let list = this.ranksList
      return list.find(element => element.rankId === this.item.workerRank)
    }
  }
}
</script>

<style lang="scss" scoped>
.rating-item{
  display: flex;
  align-items: center;
  padding: .85rem 1.2rem .85rem 3.65rem;
  background: #FFFFFF;
  font-size: .9rem;
  line-height: 1.05rem;
  color: #000000;
  border-radius: .3rem;
  text-align: center;
  position: relative;
  overflow: hidden;
  &:first-child{
    &:before, &:after{
      content: '';
      position: absolute;
      height: 2.8rem;
    }
    &:before{
      left: -.3rem;
      width: 3.6rem;
      background-size: contain;
      background-repeat: no-repeat;
      background-position: center;
      background-image: url('/img/news/icon-first.svg');
      z-index: 1;
    }
    &:after{
      left: 0;
      width: .1rem;
      background: #D92C2C;
      border-radius: .1rem;
      z-index: 2;
    }
  }
  &:nth-child(1), &:nth-child(2), &:nth-child(3), &:last-child{
    margin-bottom: .4rem;
  }
  &-index{
    position: absolute;
    left: 1.8rem;
  }
  &__info{
    display: flex;
    flex-direction: column;
    align-items: flex-start;
    text-align: left;
    &-name{
      margin-bottom: .2rem;
    }
    &-post{
      color: rgba(0, 0, 0, 0.4);
    }
  }
  &>div{
    margin-right: 2rem;
    &:nth-child(1){
      width: 15.9rem;
      min-width: 15.9rem;
    }
    &:nth-child(2){
      width: 3.2rem;
      min-width: 3.2rem;
    }
    &:nth-child(3){
      width: 3.2rem;
      min-width: 3.2rem;
    }
    &:nth-child(4){
      width: 3.8rem;
      min-width: 3.8rem;
      margin-right: 0;
    }
  }
}
</style>
