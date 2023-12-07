<template>
  <div class="current-achievement">
    <div v-if="!achievement.IsHidden || myAchive.CurrentLevel >= achievement.MaxLevel" class="current-achievement__title">{{achievement.Name}}</div>
    <div v-else class="current-achievement__title">{{loc('options_program_31')}}</div>
    <div v-if="!achievement.IsHidden || myAchive.CurrentLevel >= achievement.MaxLevel" class="current-achievement__desc">{{achievement.Desc}}</div>
    <div v-else class="current-achievement__desc">{{loc('options_program_31')}}</div>
    <div v-if="!achievement.IsHidden || myAchive.CurrentLevel >= achievement.MaxLevel" class="current-achievement__progress">
      <div class="current-achievement__progress__title">
        <div class="current-achievement__status">{{loc('options_program_17')}}:</div>
        <div class="current-achievement__progress-value">{{myAchive.CurrentLevel}} / <span>{{achievement.MaxLevel}}</span></div>
      </div>
      <div class="current-achievement__progress-bar">
        <div 
          :class="[{active: item <= ((myAchive.CurrentLevel / achievement.MaxLevel) * 36)}, 'current-achievement__progress-bar__item']"
          v-for="(item, index) in 36"
          :key="index"
        ></div>
      </div>
    </div>
    <div class="current-achievement__reward">
      <div class="current-achievement__reward-prompt">
        <div class="current-achievement__title-small">{{loc('options_program_9')}}</div>
        <div class="current-achievement__reward-desc">{{loc('options_program_10')}}</div>
      </div>
      <div class="current-achievement__reward-list">
        <div 
          class="current-achievement__reward-list__item"
          v-for="(item, index) in achievement.Rewards"
          :key="index"
        >
          <div class="current-achievement__reward-list__item__decorate">
            <div class="current-achievement__reward-list__item__decorate-img"
              :style="{backgroundImage: `url(/img/optionsMenu/programTab/${item.Image}.png)`}"
            ></div>
          </div>
          <div v-if="(!achievement.IsHidden || myAchive.CurrentLevel >= achievement.MaxLevel)" class="current-achievement__reward-list__item-desc">{{loc(item.Desc)}}</div>
          <div v-if="!achievement.IsHidden || myAchive.CurrentLevel >= achievement.MaxLevel && item.Value" class="current-achievement__reward-list__item-value">{{item.Value}}</div>
        </div>
      </div>
    </div>
    <template v-if="myAchive.CurrentLevel >= achievement.MaxLevel">
      <div 
        class="current-achievement__getted"
        v-if="myAchive.GivenReward"
      >{{loc('options_program_17')}}</div>
      <div 
        class="current-achievement__data-btn"
        v-else
        @click="acceptAchievement"
      >
        <div class="current-achievement__data-btn-icon">
          <svg 
            xmlns="http://www.w3.org/2000/svg" 
            viewBox="0 0 20 20" 
            fill="none"
          >
            <g>
              <path d="M19.4142 6.14034L8.86336 16.6904C8.08205 17.4719 6.81466 17.4719 6.03261 16.6904L0.586199 11.2436C-0.1954 10.4622 -0.1954 9.19463 0.586199 8.41303C1.36795 7.63129 2.63524 7.63129 3.41664 8.41274L7.4484 12.4445L16.5833 3.3096C17.3651 2.52785 18.6324 2.52844 19.4139 3.3096C20.1953 4.0912 20.1953 5.3583 19.4142 6.14034Z" fill="black"/>
            </g>
          </svg>
        </div>
        <div class="current-achievement__data-btn-text">{{loc('options_program_18')}}</div>
      </div>
    </template>

  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
export default {
  name: 'Currentachievement',
  props: {
    achievement: Object
  },
  computed: {
    ...mapState('optionsMenu', ['myAchievements']),
    ...mapGetters('localization', ['loc']),
    myAchive() {
      return this.myAchievements[this.achievement.AchieveName]
    }
  },
  methods: {
    acceptAchievement: function() {
      window.mp.triggerServer('personalEvents:acceptReward', this.achievement.AchieveName)
    }
  }
}
</script>

<style lang="scss" scoped>
.current-achievement{
  display: flex;
  flex-direction: column;
  padding: 2.75rem 5rem 0 3rem;
  min-width: 33rem;
  background: linear-gradient(0deg, rgba(0, 0, 0, 0.7), rgba(0, 0, 0, 0.7));
  height: 100%;
  z-index: 1;
  overflow-y: auto;
  &::-webkit-scrollbar{
    width: .2rem;
    background-color: transparent;
    &-thumb{
      background: #29FFF2;
      border-radius: .5rem;
    }
  }
  &__title-small{
    margin-bottom: .25rem;
    font-size: 2rem;
    line-height: 2.4rem;
    letter-spacing: 0.03em;
  }
  &__title{
    font-size: 4rem;
    line-height: 4rem;
    color: #FFFFFF;
    margin-bottom: .5rem;
  }
  &__desc{
    width: 100%;
    font-size: .9rem;
    line-height: 1.2rem;
    letter-spacing: 0.03em;
    color: rgba(255, 255, 255, 0.5);
    margin-bottom: 1.5rem;
  }
  &__reward{
    width: 100%;
    display: flex;
    justify-content: space-between;
    margin-bottom: 2.5rem;
    &-prompt{
      display: flex;
      flex-direction: column;
      margin-right: 1rem;
    }
    &-desc{
      font-size: .9rem;
      line-height: 1.2rem;
      letter-spacing: 0.03em;
      color: rgba(255, 255, 255, 0.5);
      width: 8.5rem;
    }
    &-list{
      display: flex;
      align-items: flex-start;
      &__item{
        display: flex;
        flex-direction: column;
        align-items: center;
        width: 4.5rem;
        margin-right: .75rem;
        text-align: center;
        &:last-child{
          margin-right: 0;
        }
        &>div{
          z-index: 2;
        }
        &__decorate{
          height: 4.5rem;
          width: 4.5rem;
          display: flex;
          align-items: center;
          justify-content: center;
          border: .1rem solid #FFFFFF;
          border-radius: 50%;
          position: relative;
          margin-bottom: .35rem;
          z-index: 1;
          &>div{
            z-index: 2;
          }
          &:before, &:after{
            content: '';
            border-radius: 50%;
            position: absolute;
            box-sizing: border-box;
          }
          &:before{
            width: 4.5rem;
            height: 4.5rem;
            z-index: 0;
            border: .3rem solid rgba(255, 255, 255, 0.2);
            border-radius: 50%;
          }
          &:after{
            width: 3.5rem;
            height: 3.5rem;
            background: #29FFF2;
            filter: blur(2rem);
            z-index: 1;
          }
          &-img{
            width: 3.5rem;
            height: 3.5rem;
            background-color: rgba(255, 255, 255, 0.2);
            border-radius: 50%;
            background-size: contain;
            background-repeat: no-repeat;
            background-position: center;
          }
        }
        &-desc{
          font-size: .9rem;
          line-height: 1.1rem;
          letter-spacing: 0.03em;
          color: rgba(255, 255, 255, 0.5);
        }
        &-value{
          font-weight: bold;
          font-size: .9rem;
          line-height: 1.05rem;
          text-transform: uppercase;
          text-shadow: 0 .1rem .35rem rgba(0, 0, 0, 0.2);
        }
      }
    }
  }
  &__progress{
    display: flex;
    flex-direction: column;
    width: 100%;
    padding-bottom: 1.5rem;
    border-bottom: 1px solid rgba(255, 255, 255, 0.2);
    margin-bottom: 1.5rem;
    &__title{
      width: 100%;
      display: flex;
      align-items: flex-end;
      justify-content: space-between;
      font-size: 1rem;
      line-height: 1.2rem;
      letter-spacing: 0.03em;
      margin-bottom: .3rem;
    }
    &-value{
      span{
        color: rgba(182, 211, 0, 1);
      }
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
  &__data{
    display: flex;
    flex-direction: column;
    &-row{
      display: flex;
      align-items: center;
      font-size: 1rem;
      line-height: 1.2rem;
      letter-spacing: 0.03em;
      margin-bottom: .6rem;
      &-desc{
        margin-right: .5rem;
      }
      &-value{
        line-height: 1rem;
        color: #B6D300;
      }
    }
    &-btn{
      display: flex;
      padding: .2rem;
      align-items: center;
      background: #B6D300;
      width: 100%;
      height: 3.2rem;
      margin-top: 2rem;
      border-radius: 5rem;
      transition: .3s;
      position: relative;
      &:hover{
        box-shadow: 0px .5rem 4rem rgba(168, 195, 2, 0.6);
      }
      &-icon{
        width: 2.8rem;
        height: 2.8rem;
        background-color: #fff;
        border-radius: 50%;
        display: flex;
        align-items: center;
        justify-content: center;
        position: absolute;
        left: .2rem;
        svg{
          width: 1rem;
          height: 1rem;
        }
      }
      &-text{
        font-weight: bold;
        font-size: 1rem;
        width: 100%;
        text-align: center;
      }
    }
  }
  &__getted{
    display: flex;
    align-items: center;
    justify-content: center;
    width: 100%;
    font-weight: bold;
    font-size: 1.8rem;
    &::before{
      content: '';
      width: 2rem;
      height: 2rem;
      margin-right: 1rem;
      background-size: contain;
      background-repeat: no-repeat;
      background-position: center;
      background-image: url('/img/optionsMenu/programTab/done.png');
    }
  }
}
</style>
