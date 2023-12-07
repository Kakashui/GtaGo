<template>
  <div 
    :class="[{active: achievement === currentAchievement}, 'achievement-item']"
    @click="setCurrentAchievement"
  >
    <div 
      class="achievement-item__icon"
      v-if="myAchive.CurrentLevel >= achievement.MaxLevel"
    >
      <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 30 30" fill="none">
        <g>
          <path d="M11.4875 26.207C11.2012 26.4951 10.8105 26.6558 10.4046 26.6558C9.99876 26.6558 9.60806 26.4951 9.32169 26.207L0.673147 17.5571C-0.224383 16.6596 -0.224383 15.2042 0.673147 14.3083L1.75607 13.2251C2.65388 12.3276 4.1076 12.3276 5.00513 13.2251L10.4046 18.6249L24.9948 4.03441C25.8926 3.13688 27.3477 3.13688 28.2439 4.03441L29.3268 5.11762C30.2243 6.01515 30.2243 7.47027 29.3268 8.3664L11.4875 26.207Z" fill="white"/>
        </g>
      </svg>
      <span>Completed</span>
    </div>
    <div 
      class="achievement-item__icon achievement-item__icon--in-progress"
      v-else
    ></div>
    <div v-if="!achievement.IsHidden || myAchive.CurrentLevel >= achievement.MaxLevel" class="achievement-item__info">
      <div class="achievement-item__info-title">{{achievement.Name}}</div>
      <div class="achievement-item__info-label">{{achievement.ShortDesc}}</div>
    </div>
    <div v-else class="achievement-item__info">
      <div class="achievement-item__info-title">{{loc('options_program_31')}}</div>
    </div>
  </div>
</template>

<script>
import { mapState, mapGetters } from 'vuex'
export default {
  name: 'AchievementItem',
  props: {
    achievement: Object,
    currentAchievement: Object
  },
  computed: {
    ...mapState('optionsMenu', ['myAchievements']),
    ...mapGetters('localization', ['loc']),
    myAchive() {
      return this.myAchievements[this.achievement.AchieveName]
    }
  },
  methods: {
    setCurrentAchievement: function() {
      this.$emit('setCurrentAchievement', this.achievement)
    }
  }
}
</script>

<style lang="scss" scoped>
.achievement-item{
  display: flex;
  align-items: center;
  border-radius: .5rem;
  transition: .3s;
  &:hover, &.active{
    .achievement-item__icon{
      background-color: #B6D300;
      box-shadow: 0 .5rem 4rem rgba(168, 195, 2, 0.6);
      color: #fff;
      &--in-progress{
        box-shadow: none;
        background-color: #fff;
        background-image: url('/img/optionsMenu/programTab/in-progress-active.png');
      }
    }
    .achievement-item__info{
      background-color: #fff;
      color: #000;
    }
  }
  &>div{
    z-index: 1;
  }
  &__icon, &__info{
    transition: .3s;
  }
  &__icon{
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    height: 100%;
    width: 5rem;
    min-width: 5rem;
    margin-right: .25rem;
    background-color: rgba(0, 0, 0, 0.6);
    border-radius: 1px;
    color: rgba(255, 255, 255, 0.5);
    svg{
      width: 1.5rem;
      height: 1.5rem;
      margin-bottom: .5rem;
    }
    &--in-progress{
      background-size: contain;
      background-repeat: no-repeat;
      background-position: center;
      background-image: url('/img/optionsMenu/programTab/in-progress.png');
    }
  }
  &__info{
    height: 100%;
    background: rgba(0, 0, 0, 0.6);
    color: #fff;
    width: 100%;
    padding: 0 1.5rem;
    position: relative;
    display: flex;
    flex-direction: column;
    align-items: flex-start;
    justify-content: center;
    letter-spacing: 0.03em;
    &-title{
      font-size: 1.5rem;
      line-height: 1.8rem;
      margin-bottom: .2rem;
    }
    &-label{
      font-size: .9rem;
      line-height: 1.1rem;
    }
  }
}
</style>
