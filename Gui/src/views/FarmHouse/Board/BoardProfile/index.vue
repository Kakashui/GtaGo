<template>
  <div class="board-profile">
    <div class="board-profile__scroll">
      <div class="board-profile__info">
        <div class="board-profile__info-profile">
          <div class="board-profile__info-profile-desc">{{loc('farmHouse_6')}}</div>
          <div class="board-profile__info-profile-nickname">{{nickname}}</div>
        </div>
        <div class="board-profile__info-evolution">
          <div class="board-profile__info-evolution__levels">
            <span>{{userLevel.level}}</span>
            <span>{{userLevel.level + 1}}</span>
          </div>
          <div class="board-profile__info-evolution__progress">
            <div 
              class="board-profile__info-evolution__progress-line"
              :style="{width: `${currentLevelPercent}%`}"
            ></div>
          </div>
          <div class="board-profile__info-evolution__desc">
            <span>exp: {{currentExp}}/{{userLevel.exp}}</span>
            <span>{{currentLevelPercent}}%</span>
          </div>
        </div>
      </div>
      <div class="board-profile__levels">
        <LevelItem
          v-for="item in levelsList"
          :key="item.requiredLevel"
          :item="item"
        />
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
import LevelItem from './LevelItem'
export default {
  name: 'BoardProfile',
  components: {
    LevelItem
  },
  computed: {
    ...mapState('farmHouse', [
      'nickname', 
      'levelsList',
      'currentExp'
    ]),
    ...mapGetters('localization', ['loc']),
    ...mapGetters('farmHouse', ['userLevel']),
    currentLevelPercent: function() {
      return Math.floor((this.currentExp/this.userLevel.exp)*100)
    }
  },
  
}
</script>

<style lang="scss" scoped>
.board-profile{
  position: relative;
  &:before, &:after{
    content: '';
    width: 1px;
    height: 100%;
    background: rgba(255, 255, 255, 0.1);
    position: absolute;
    bottom: 0;
  }
  &:before{
    left: 2.5rem;
  }
  &:after{
    right: 2.5rem;
  }
  &__scroll{
    padding-top: 2rem;
    overflow-y: auto;
    height: 100%;
    &::-webkit-scrollbar{
      width: 0;
      background-color: transparent;
      overflow-y: auto;
    }
  }
  &__info{
    display: flex;
    align-items: center;
    justify-content: space-between;
    padding: 1.5rem;
    border: 1px solid rgba(255, 255, 255, 0.1);
    margin: 0 3.8rem 1.8rem 3.8rem;
    &-profile{
      display: flex;
      flex-direction: column;
      justify-content: center;
      padding-left: 4.75rem;
      position: relative;
      &:before{
        content: '';
        position: absolute;
        left: 0;
        width: 4rem;
        height: 4rem;
        background-size: contain;
        background-repeat: no-repeat;
        background-position: center;
        background-image: url('/img/farmHouse/logo.png');
      }
      &-desc{
        font-style: normal;
        font-weight: 600;
        font-size: .9rem;
        letter-spacing: 0.1em;
        margin-bottom: .3rem;
        text-transform: uppercase;
        color: rgba(255, 255, 255, 0.5);
      }
      &-nickname{
        font-weight: 600;
        font-size: 1.6rem;
        color: #FFFFFF;
      }
    }
    &-evolution{
      width: 25.5rem;
      display: flex;
      flex-direction: column;
      overflow: hidden;
      &__levels{
        display: flex;
        align-items: center;
        justify-content: space-between;
        width: 100%;
        margin-bottom: .35rem;
        span{
          display: flex;
          align-items: flex-end;
          position: relative;
          font-style: normal;
          font-weight: bold;
          font-size: 1.5rem;
          line-height: 1.7rem;
          text-transform: uppercase;
          color: #FFFFFF;
          &:last-child{
            color: #DCFF00;
          }
          &:after{
            content: 'lvl';
            margin-left: .3rem;
            font-style: normal;
            font-weight: normal;
            font-size: 1rem;
            line-height: 1.15rem;
            text-transform: uppercase;
            color: rgba(255, 255, 255, 0.3);
          }
        }
      }
      &__progress{
        width: 100%;
        height: .7rem;
        border: 1px solid rgba(255, 255, 255, 0.2);
        margin-bottom: .5rem;
        position: relative;
        &-line{
          height: 100%;
          position: absolute;
          left: 0;
          top: 0;
          background: #DCFF00;
          display: flex;
          align-items: center;
          &:after{
            content: '';
            width: 4rem;
            height: 1rem;
            position: absolute;
            right: 0;
            background: radial-gradient(100% 50% at 100% 50%, #FFFFFF 0%, rgba(255, 255, 255, 0) 100%);
          }
        }
      }
      &__desc{
        width: 100%;
        display: flex;
        justify-content: space-between;
        text-transform: uppercase;
        font-style: normal;
        font-weight: normal;
        font-size: .9rem;
        line-height: 1.1rem;
        letter-spacing: 0.1em;

        color: rgba(255, 255, 255, 0.4);
      }
    }
  }
  &__levels{
    display: grid;
    grid-template-columns: repeat(3, 1fr);
    grid-auto-rows: 15rem;
    border-top: 1px solid rgba(255, 255, 255, 0.1);
    padding: 0 2.5rem;
  }
}
</style>
