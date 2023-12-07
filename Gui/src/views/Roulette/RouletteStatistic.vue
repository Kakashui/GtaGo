<template>
  <div class="roulette-statistic">
    <div 
      class="roulette-statistic__btn"
      @click="setShowStatisticList"
    >{{loc('Roulette_6')}} <img src="/img/roulette/arrow.png" alt="arrow" :class="{'roulette-statistic__btn_s': showStatisticList}"> </div>
    <transition name="statistic-list">
      <div 
        v-if="showStatisticList"
        class="roulette-statistic__list"
      >
        <div class="roulette-statistic__list-header">
          <div>{{loc('Roulette_7')}}</div>
          <div>{{loc('Roulette_8')}}</div>
        </div>
        <div 
          class="roulette-statistic__list-item"
          v-for="(item, index) in resultsList"
          :key="index"
        >
          <div 
            :class="[{'black' : item.throwValueColor === 'black'}, {'red' : item.throwValueColor === 'red'}, 'roulette-statistic__list-item__stat']">{{item.throwValue}}</div>
          <div 
            :class="[{'win' : item.win}, 'roulette-statistic__list-item__result']"
          >{{item.win ? '+' : '-'}}$ {{item.gain}}</div>
        </div>
      </div>
    </transition>
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
export default {
  name: 'RouletteStatistic',
  data: function() {
    return {
      showStatisticList: true
    }
  },
  computed: {
    ...mapGetters('localization', ['loc']),
    ...mapState('roulette', ['statsList']),
    resultsList() {
      if (this.statsList.length > 6) {
        return this.statsList.slice(this.statsList.length - 6)
      }
      return this.statsList
    }
  },
  methods: {
    setShowStatisticList: function() {
      //console.log('+')
      this.showStatisticList = !this.showStatisticList
    }
  }
}
</script>

<style lang="scss" scoped>
.roulette-statistic {
  position: absolute;
  left: 2rem;
  top: 1rem;
  font-weight: bold;
  font-size: .9rem;
  line-height: 130%;
  letter-spacing: 0.13em;
  display: flex;
  flex-direction: column;
  align-items: center;
  &__btn{
    display: flex;
    align-items: center;
    position: relative;
    text-transform: uppercase;
    padding: 1rem 0;
    img{
      width: 1.2rem;
      height: 1.2rem;
      margin-left: 1rem;
    }
    &_s{
      transform: rotateX(180deg)
    }
  }
  &__list{
    position: absolute;
    left: 0;
    top: 2.8rem;
    display: flex;
    flex-direction: column;
    align-items: center;
    padding: 3.2rem 1rem 1.2rem 1rem;
    width: 13.25rem;
    background: radial-gradient(102.09% 85.32% at 65.03% 55.79%, rgba(16, 23, 43, 0) 28.17%, rgba(16, 23, 43, 0.74) 64.91%);
    background-color: rgba(3, 27, 40, 0.5);
    border-radius: .3rem;
    pointer-events: none;
    &-header{
      color: rgba(255, 255, 255, 0.5);
      display: flex;
      justify-content: center;
      position: absolute;
      top: 1rem;
      &>div{
        margin-right: 1.8rem;
        &:last-child{
          margin-right: 0;
        }
      }
    }
    &-item{
      width: 100%;
      display: flex;
      align-items: center;
      justify-content: center;
      font-size: 1.2rem;
      line-height: 1.35rem;
      letter-spacing: 0.05em;
      padding: 1.2rem 0;
      position: relative;
      &:first-child{
        padding-top: 0;
      }
      &:last-child{
        padding-bottom: 0;
        &:after{
          content: none;
        }
      }
      &:after{
        content: '';
        width: 100%;
        height: 1px;
        position: absolute;
        left: 0;
        bottom: 0;    
        background: radial-gradient(50% 50% at 50% 50%, rgba(255, 255, 255, 0.3) 83.33%, rgba(255, 255, 255, 0) 100%);
      }
      &__stat, &__result{
        width: 50%;
      }
      &__stat{
        display: flex;
        align-items: center;
        justify-content: flex-start;
        padding-left: .6rem;
        position: relative;
        &:before, &:after{
          content: ''
        }
        &:before{
          width: .9rem;
          height: .9rem;
          margin-right: .4rem;
          border-radius: 50%;
          background-color: #C1E704;
        }
        &:after{
          width: 1px;
          height: 2.4rem;
          position: absolute;
          right: 0;
          background: radial-gradient(50% 50% at 50% 50%, rgba(255, 255, 255, 0.3) 0%, rgba(255, 255, 255, 0) 100%);
        }
        &.black{
          &:before{
            background-color: #000000;
            box-shadow: 0px 0px 12px rgba(255, 255, 255, 0.3);
          }
        }
        &.red{
          &:before{
            background-color: #FF3131;
            box-shadow: none;
          }
        }
      }
      &__result{
        display: flex;
        align-items: center;
        justify-content: center;
        color: #FF3131;
        &.win{
          color: #C1E704;
        }
      }
    }
  }
}

.statistic-list-enter-active, .statistic-list-leave-active{
  transition: all .3s ease;
}
.statistic-list-enter, .statistic-list-leave-to
/* .statistic-list-leave-active below version 2.1.8 */ {
  transform: translateY(100px);
  opacity: 0;
}
</style>
