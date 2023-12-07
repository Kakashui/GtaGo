<template>
  <div class="achievements-tab">
    <div class="achievements-tab__board">
      <div class="achievements-tab__header">
        <div class="achievements-tab__title">{{loc('options_program_4')}}</div>
        <div class="achievements-tab__progress">
          <div class="achievements-tab__progress__title">
            <div class="achievements-tab__progress-desc">{{loc('options_program_8')}}</div>
            <div class="achievements-tab__progress-value">{{completedAchievements}}/{{Object.keys(achieves).length}}</div>
          </div>
          <div class="achievements-tab__progress-bar">
            <div
              :class="[
                { active: item < ((completedAchievements / Object.keys(achieves).length) * 36 )},
                'achievements-tab__progress-bar__item',
              ]"
              v-for="(item, index) in 36"
              :key="index"
            ></div>
          </div>
        </div>
      </div>
      <div class="achievements-tab__list">
        <AchievementItem
          v-for="(item, index) in Object.keys(achieves)"
          :key="index"
          :achievement="achieves[item]"
          :currentAchievement="currentAchievement"
          @setCurrentAchievement="setCurrentAchievement"
        />
      </div>
    </div>
    <transition name="current-achievement">
      <CurrentAchievement
        v-if="currentAchievement"
        :achievement="currentAchievement"
      />
    </transition>
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
import AchievementItem from './components/AchievementItem'
import CurrentAchievement from './components/CurrentAchievement'
import achievesConfig from '../../../../../configs/personalEvents/achievesConfig'

export default {
  name: 'AchievementsTab',
  components: {
    AchievementItem,
    CurrentAchievement
  },
  data: function() {
    return {
      currentAchievement: null,
    }
  },
  computed: {
    ...mapState('optionsMenu', ['myAchievements']),
    ...mapGetters('localization', ['loc']),
    completedAchievements() {
      return Object.keys(this.myAchievements).filter(key => this.achieves[key] !== undefined && this.myAchievements[key].CurrentLevel >= this.achieves[key].MaxLevel).length
    },
    achieves() {
      return achievesConfig
    }
  },
  methods: {
    setCurrentAchievement: function(obj) {
      if (this.currentAchievement != obj)
        this.currentAchievement = obj
      else 
        this.currentAchievement = null
    }
  },
  mounted() {
  }
}
</script>

<style lang="scss" scoped>
.achievements-tab {
  width: 100%;
  display: flex;
  justify-content: space-between;
  &__board{
    display: flex;
    flex-direction: column;
    margin-top: 2rem;
    width: 45rem;
    min-width: 45rem;
    margin-right: 1.8rem;
  }
  &__header{
    display: flex;
    align-items: center;
    margin-bottom: 1.5rem;
  }
  &__title{
    font-style: normal;
    font-weight: normal;
    font-size: 4rem;
    margin-right: 2rem;
  }
  &__progress{
    display: flex;
    flex-direction: column;
    width: 100%;
    &__title{
      display: flex;
      align-items: flex-end;
      justify-content: space-between;
      font-size: 1.2rem;
      line-height: 1.45rem;
      letter-spacing: 0.03em;
      margin-bottom: .5rem;
    }
    &-value{
      color: #B6D300;
      text-shadow: 0 .5rem 4rem rgba(168, 195, 2, 0.6);
    }
    &-bar{
      width: 100%;
      display: flex;
      align-items: center;
      justify-content: space-between;
      height: 2rem;
      &__item{
        height: 100%;
        width: .2rem;
        background: rgba(255, 255, 255, 0.5);
        &.active{
          background: rgba(255, 255, 255, 1);
        }
      }
    }
  }
  &__list{
    display: grid;
    grid-auto-rows: 5rem;
    grid-template-columns: repeat(1, 1fr);
    grid-row-gap: .8rem;
    padding-right: 1.2rem;
    margin-right: -1.2rem;
    overflow-y: auto;
    height: calc(100% - (10.2rem));
    &::-webkit-scrollbar{
      width: .2rem;
      background-color: transparent;
      &-thumb{
        background: #29FFF2;
        border-radius: .5rem;
      }
    }
  }
}
.current-achievement-enter-active, .current-achievement-leave-active {
  transition: transform .5s;
}
.current-achievement-enter, .current-achievement-leave-to /* .current-achievement-leave-active below version 2.1.8 */ {
  transform: translateX(100%)
}
</style>
