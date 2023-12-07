<template>
  <div 
    v-if="item.timePassed < item.time"
    class="war-for-enterprice-list-item"
  >
    <div class="war-for-enterprice-list-item__circle">
      <svg 
        viewBox="0 0 50 50" 
        xmlns="http://www.w3.org/2000/svg"
      >
        <circle 
          :stroke-dasharray="calcCircumference" 
          :stroke-dashoffset="circleValue" 
        />
      </svg>
      <div class="war-for-enterprice-list-item__circle-time">{{formatTimeLeft}}</div>
    </div>
    <div class="war-for-enterprice-list-item__info">
      <span class="name-family">{{currentOrgName}}</span>
      <span>{{loc('war_for_enterprice_6')}}</span>
      <span class="name-company">{{companiesConfig[item.key].Name}}</span>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex';
import warcompanies from '../../../../configs/families/warcompanies';
export default {
  name: 'WarForEnterpriceListItem',
  props: {
    item: Object
  },
  computed: {
    ...mapState('familyMenu/ratingPage', ['orgList']),
    ...mapState('warForEnterprice', ['fractionNames']),
    ...mapGetters('localization', ['loc']),
    companiesConfig(){
      return warcompanies;
    },
    currentOrgName() {
      if (this.item.orgId > 0)
        return this.item.orgName
      return 'Unknown'
    },
    calcCircumference: function () {
      let number = 2 * Math.PI * 22
      return  number
    },
    circleValue: function () {
      const maxCircleValue = this.calcCircumference
      const maxCircleValuePer = (maxCircleValue / this.item.time) * this.item.timePassed
      
      return maxCircleValue - maxCircleValuePer
    },
    formatTimeLeft: function() {
      let minutes = Math.floor((this.item.time - this.item.timePassed) / 60)
      let seconds = (this.item.time - this.item.timePassed) % 60;
      
      if (seconds < 10) {
        seconds = `0${seconds}`
      }
      if (minutes < 10) {
        minutes = `0${minutes}`
      }

      return `${minutes}:${seconds}`
    },
  },
}
</script>

<style lang="scss" scoped>
.war-for-enterprice-list-item{
  display: flex;
  align-items: center;
  z-index: 1;
  margin-bottom: .9rem;
  &__circle{
    display: flex;
    align-items: center;
    justify-content: center;
    width: 2.5rem;
    height: 2.5rem;
    border-radius: 50%;
    margin-right: 1.15rem;
    position: relative;
    &:before{
      content: '';
      width: .15rem;
      height: 3rem;
      position: absolute;
      right: -.5rem;
      transform: translateX(100%);
      background: #29FFF2;
    }
    &-time{
      font-weight: bold;
      font-size: .9rem;
      line-height: .9rem;
    }
    svg{
      position: absolute;
      width: 100%;
      height: 100%;
      circle{
        fill: none;
        stroke: #fff;
        cx: 25;
        cy: 25;
        r: 22;
        stroke-width: 3;
      }
    }
  }
  &__info{
    display: flex;
    flex-direction: column;
    align-items: flex-start;
    span{
      margin-bottom: .1rem;
      font-size: 1rem;
      line-height: 1rem;
      font-weight: 300;
      &.name{
        &-family, &-company{
          font-weight: bold;
        }
        &-company{
          color: #29FFF2;
        }
      }
    }
  }
}
</style>
