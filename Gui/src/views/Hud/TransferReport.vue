<template>
  <div v-if="transfersList.length > 0" class="transfer-report">
    <div class="transfer-report-prompt">F7</div>
    <svg 
      viewBox="0 0 60 60"
      xmlns="http://www.w3.org/2000/svg">
      <circle 
        :stroke-dasharray="calcCircumference" 
        :stroke-dashoffset="circleValue" 
      />
      <circle class="circle-decorate"/>
      </svg>
  </div>
</template>

<script>
import { mapState } from 'vuex'
export default {
  name: 'TransferReport',
  computed: {
    ...mapState('transfersConfirmation', ['transfersList']),
    ...mapState('hud', ['transferReport']),
    calcCircumference: function () {
      let number = 2 * Math.PI * 30
      return  number
    },
    circleValue: function () {
      const maxCircleValue = this.calcCircumference
      const maxCircleValuePer = (maxCircleValue / this.transferReport.maxReports) * this.transfersList.length
      
      return maxCircleValue - maxCircleValuePer
    },
  }
}
</script>

<style lang="scss">
.transfer-report{
  position: absolute;
  right: 18rem;
  top: 1.75rem;
  width: 3rem;
  height: 3rem;
  background-image: url('/img/transfersConfirmation/transfers-icon.png');
  background-size: 1.2rem 1.2rem;
  background-position: center;
  background-repeat: no-repeat;
  &-prompt{
    display: flex;
    align-items: center;
    justify-content: center;
    width: .9rem;
    height: .9rem;
    position: absolute;
    left: 0;
    top: 0;
    background-color: #fff;
    border-radius: 50%;
    font-weight: bold;
    font-size: .8rem;
    line-height: .8rem;
    text-transform: uppercase;
    color: #000000;
  }
  svg{
    width: 100%;
    height: 100%;
    circle{
      transform-origin: 50% 50%;
      transform: rotate(-90deg);
      fill: none;
      stroke: #8857EF;
      cx: 30;
      cy: 30;
      r: 27;
      stroke-width: 3;
      &.circle-decorate{
        stroke: rgba(255, 255, 255, 0.4);
      }
    }
  }
}
</style>
