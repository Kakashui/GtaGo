<template>
  <div class="rating-page">
    <div 
      class="btn btn-back"
      @click="goBack"
    ><span></span>{{loc('familyMenu_61')}}</div>
    <div class="rating-page__info">
      <div class="title">{{loc('familyMenu_62')}}</div>
      <div class="stats">
        <div class="desc">{{loc('familyMenu_63')}}</div>
        <div class="value">{{getFamilyRank}} {{loc('familyMenu_64')}}</div>
      </div>
    </div>
    <div class="rating-page__entity">
      <div class="title">
        <div class="title__value"><span>№</span>{{loc('familyMenu_65')}}</div>
        <div class="title__value">{{loc('familyMenu_66')}}</div>
        <div class="title__value">{{loc('familyMenu_67')}}</div>
        <div class="title__value">{{loc('familyMenu_68')}}</div>
        <div class="title__value">{{loc('familyMenu_69')}}</div>
        <div class="title__value">{{loc('familyMenu_70')}}</div>
      </div>
      <div class="list">
        <div 
          class="item"
          v-for="(item, index) in orgList"
          :key="item.key"
        >
          <div class="item__value">
            <span v-if="index <= 2"></span>
            <span v-else>{{index + 1}}</span>
            {{item.name}}
          </div>
          <div class="item__value">{{item.owner}}</div>
          <div class="item__value">{{item.victories}}</div>
          <div class="item__value">{{item.buissCount}}</div>
          <div class="item__value">{{item.membersCount}}</div>
          <div class="item__value">{{item.rating}}</div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapMutations, mapState } from 'vuex'
export default {
  name: 'RatingPage',

  methods: {
    ...mapMutations('familyMenu', ['toggleNav', 'setCurrentPage']),
    goBack: function() {
      this.toggleNav(true);
      this.setCurrentPage('InfoPage')
    }
  },

  computed: {
    ...mapState('familyMenu', ['infoPage']),
    ...mapState('familyMenu/ratingPage', ['orgList']),
    ...mapGetters('localization', ['loc']),
    getFamilyRank: function () {         
      return this.$store.getters['familyMenu/ratingPage/getFamilyRank'](this.infoPage.familyId);
    }
  }
}
</script>

<style lang="scss" scoped>
.rating-page{
  display: flex;
  justify-content: space-between;
  position: absolute;
  left: 0;
  top: 0;
  width: 100%;
  height: 100vh;
  padding: 2.5rem 3rem 0 3rem;
  .btn-back{
    position: fixed;
    bottom: 3rem;
    left: 3rem;
    display: flex;
    align-items: center;
    &:hover{
      span{
        transform: scale(1.1);
        transition: .2s;
      }
    }
    span{
      width: 2.5rem;
      height: 2.5rem;
      margin-right: 1rem;
      background-image: url('/img/familyMenu/ratingPage/btn-back.svg');
      background-size: contain;
      background-repeat: no-repeat;
      background-position: center;
      background-color: #222428;
      border: 1px solid #343539;
      box-sizing: border-box;
      transition: .2s;
    }
  }
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
    width: 73.85rem;
    background: linear-gradient(90deg, rgba(25, 28, 26, 0.9) 0%, rgba(45, 50, 54, 0.9) 100%);
    backdrop-filter: blur(2rem);
    .title, .list .item{
      display: flex;
      align-items: center;
      width: 73.85rem;
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
          width: 22.1rem;
          min-width: 22.1rem;
          padding-left: 4.5rem;
        }
        &:nth-child(2){
          width: 21.3rem;
          min-width: 21.3rem;
          padding: 0 1rem;
        }
        &:last-child{
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
      &__value{
        background: #111111;
      }
    }
    .list{
      display: flex;
      flex-flow: column;
      overflow-y: auto;
      padding-right: .3rem;
      width: calc(100% + (.5rem));
      margin-right: -.5rem;
      &::-webkit-scrollbar{
        width: .2rem;
        background-color: transparent;
        &-thumb{
          background: #414145;
          border-radius: .1rem;
        }
      }
      .item{
        &:nth-child(1), &:nth-child(2), &:nth-child(3){
          background-color: rgba(255, 255, 255, 0.1);
          background-size: cover;
          background-repeat: no-repeat;
          background-position: center;
          background-image: url('/img/familyMenu/ratingPage/place-bg.svg');
          .item__value{
            &:nth-child(1){
              span{
                width: 2.5rem;
                height: 2.5rem;
                left: 1rem;
                background-size: contain;
                background-repeat: no-repeat;
                background-position: center;
              }
            }
          }
        }
        &:nth-child(1) {
          .item__value{
            &:nth-child(1){
              span{
                background-image: url('/img/familyMenu/ratingPage/place-1.png');
              }
            }
          }
        }
        &:nth-child(2) {
          .item__value{
            &:nth-child(1){
              span{
                background-image: url('/img/familyMenu/ratingPage/place-2.png');
              }
            }
          }
        }
        &:nth-child(3) {
          .item__value{
            &:nth-child(1){
              span{
                background-image: url('/img/familyMenu/ratingPage/place-3.png');
              }
            }
          }
        }
      } 
    }
  }
}
</style>
