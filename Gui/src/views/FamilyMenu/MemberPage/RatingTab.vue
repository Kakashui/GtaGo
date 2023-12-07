<template>
  <div class="rating-tab">
    <div class="rating-tab__info">
      <div class="title">{{loc('fam:menu:memb:rait:1')}}</div>
      <div class="stats">
        <div class="desc">{{loc('familyMenu_63')}}</div>
        <div class="value">{{currentRating}} {{loc('familyMenu_64')}}</div>
      </div>
    </div>
    <div class="rating-tab__entity">
      <div class="title">
        <div class="title__value"><span>№</span>{{loc('fam:menu:memb:rait:2')}}</div>
        <div class="title__value">{{loc('fam:menu:memb:rait:3')}}</div>
        <div class="title__value">ID</div>
        <div class="title__value"><div class="svg"></div></div>
        <div class="title__value"><div class="svg down"></div></div>
        <div class="title__value">{{loc('fam:menu:memb:rait:4')}}</div>
      </div>
      <div class="list">
        <member-item 
          v-for="(item, index) in sortMembers"
          :key="item.key"
          :item="item"
          :index="index"
          @ratingFound="setCurrentRating"
        />
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex';
import MemberItem from './MemberItem'
export default {
  name: 'RatingTab',

  components: {
    MemberItem
  },

  data: function() {
    return{
      currentRating: null
    }
  },

  computed: {
    ...mapState('familyMenu/membersPage', ['members']),
    ...mapGetters('localization', ['loc']),
    getMemberRank: function () {         
      return this.$store.getters['familyMenu/ratingPage/getMemberRank'](this.infoPage.familyId);
    },

    sortMembers: function() {
      let newArr = this.members.slice(0, this.members.length).sort(function( a, b ){
        return b.rating - a.rating
      })
      return newArr
    }
  },

  methods: {
    setCurrentRating: function(id) {
      this.currentRating = id + 1
    }
  }
}
</script>

<style lang="scss" scoped>
.rating-tab{
  display: flex;
  justify-content: space-between;
  width: 100%;
  height: 100vh;
  &__info{
    display: flex;
    flex-flow: column;
    width: 15.9rem;
    min-width: 15.9rem;
    margin-right: 1.65rem;
    .title{
      margin-bottom: 4rem;
      font-size: 3.5rem;
      line-height: 3.5rem;
    }
    .stats{
      display: flex;
      flex-flow: column;
      .desc{
        font-size: 1.2rem;
        line-height: 1.2rem;
        color: rgba(255, 255, 255, 0.4);
        margin-bottom: .3rem;
      }
      .value{
        font-size: 2rem;
        line-height: 2rem;
        display: flex;
        align-items: center;
        position: relative;
        &:after{
          content: '';
          margin-left: 1rem;
          width: 1.9rem;
          height: 1.9rem;
          background-size: contain;
          background-repeat: no-repeat;
          background-position: center;
          background-image: url('/img/familyMenu/ratingPage/icon-rating.svg');
          border-radius: 50%;
          background-color: #B6D300;
          box-shadow: 0 .5rem 2rem .2rem rgba(162, 188, 31, 0.3);
        }
      }
    }
  }
  &__entity{
    display: flex;
    flex-flow: column;
    width: 54.7rem;
    .title, .list .item{
      display: flex;
      align-items: center;
      width: 54.7rem;
      height: 4rem;
      min-height: 4rem;
      font-size: 1.1rem;
      &__value{
        display: flex;
        align-items: center;
        justify-content: center;
        width: 100%;
        height: 100%;
        position: relative;
        &:after{
          content: '';
          width: .1rem;
          height: 100%;
          position: absolute;
          right: 0;
          top: 0;
          background: linear-gradient(270deg, rgba(255, 255, 255, 0.141) 0%, rgba(0, 0, 0, 0.159) 100%);
        }
        &:before{
          content: '';
          height: .1rem;
          width: 100%;
          position: absolute;
          left: 0;
          bottom: 0;
          background: linear-gradient(180deg, rgba(255, 255, 255, 0.141) 0%, rgba(0, 0, 0, 0.159) 100%);
        }
        &:nth-child(1), &:nth-child(2){
          justify-content: flex-start;
        }
        &:nth-child(1){
          width: 8.7rem;
          min-width: 8.7rem;
          padding-left: 5.1rem;
          &:after{
            content: none;
          }
        }
        &:nth-child(2){
          width: 16.8rem;
          min-width: 16.8rem;
        }
        &:nth-child(3){
          width: 8.55rem;
          min-width: 8.55rem;
        }
        &:nth-child(4){
          color: #DCFF00;
        }
        &:nth-child(5){
          color: #FF3838;
        }
        &:last-child{
          width: 7.95rem;
          min-width: 7.95rem;
          &:after{
            content: none;
          }
        }
        span{
          position: absolute;
          left: 2rem;
        }
      }
    }
    .title{
      background: #252A2D;
      border-radius: .5rem .5rem 0 0;
      color: rgba(255, 255, 255, 0.4);
      font-weight: 300;
      overflow: hidden;
      &__item{
        background-color: transparent;
      }
      .svg{
        width: 1.05rem;
        height: 1.55rem;
        background-size: contain;
        background-repeat: no-repeat;
        background-position: center;
        background-image: url('/img/familyMenu/IconDevelopment.svg');
        &.down{
          background-image: url('/img/familyMenu/IconDevelopmentDown.svg');
        }
      }
    }
    .list{
      display: flex;
      flex-flow: column;
      overflow-y: auto;
      padding-right: .3rem;
      width: calc(100% + (.5rem));
      margin-right: -.5rem;
      max-height: calc(100vh - (14.4rem));
      &::-webkit-scrollbar{
        width: .2rem;
        background-color: transparent;
        &-thumb{
          background: #414145;
          border-radius: .1rem;
        }
      }
    }
  }
}
</style>
