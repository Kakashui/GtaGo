<template>
  <div class="notify-section">
    <div class="animation-block">
      <div class="car-point">
        <div class="icon"></div>
      </div>
    </div>
    <div class="prompt" v-if="taxiJobData.currentOrder.status === 'accepted'" :v-html="loc('phone:taxi:1')"></div>
    <div class="prompt" v-else-if="taxiJobData.currentOrder.status === 'inProgress'" :v-html="loc('phone:taxi:2')"></div>
    <div class="desc">{{loc('phone:taxi:3')}}</div>
    <div class="desc">{{loc(`phone:taxi:4@${taxiJobData.currentOrder.sum }`)}}</div>
    <div class="desc">{{loc(`phone:taxi:5@${taxiJobData.currentOrder.isCardPayment ?  'phone:taxi:9' : 'phone:taxi:10'}`)}} </div>
    <div class="current-location">
      <div class="current-location__desc" v-if="taxiJobData.currentOrder.status === 'accepted'">{{loc('phone:taxi:6')}}</div>
      <div class="current-location__desc" v-else-if="taxiJobData.currentOrder.status === 'inProgress'">{{loc('phone:taxi:7')}}</div>
      <div class="current-location__value">{{taxiJobData.currentOrder.location}}</div>
    </div>
    <div class="btn" @click="cancel">{{loc('phone:taxi:8')}}</div>
  </div>
</template>

<script>
import { mapState, mapMutations, mapGetters } from 'vuex'

export default {
  name: 'NotifySection',

  computed: {
		...mapState('smartphone', ['currentTab']),
    ...mapState('smartphone/taxiPage', ['taxiJobData']),
    ...mapGetters('localization', ['loc'])
	},

  mounted: function() {
    this.setColorTheme({ header: 'light', button: 'light' })
  },

  methods: {
    ...mapMutations('smartphone', ['setCurrentTab', 'setColorTheme']),
    ...mapMutations('smartphone/taxiPage', ['taxijob_setStateWork']),

    cancel: function() {
      window.mp.triggerServer('phone::taxijob::cancel');
    }
  }
}
</script>

<style lang="scss" scoped>
  @keyframes pulse-large{
    0% {
      transform: scale(1);
      opacity: 1;
    }
    75%{
      transform: scale(1.75);
      opacity: .8;
    }
    100%{
      transform: scale(2);
      opacity: 0;
    }
  }
  @keyframes pulse-small{
    0% {
      transform: scale(1);
    }
    75%{
      transform: scale(1.2);
    }
    100%{
      transform: scale(1);
    }
  }
  @keyframes page-transition {
    from {
      opacity: 0;
    }
    to {
      opacity: 1;
    }
  }
  .notify-section{
    height: 100%;
    width: 100%;
    background-image: url('/img/hud/smartphone/taxiJobTab/notify-bg.png');
    background-size: cover;
    background-position: center;
    background-repeat: no-repeat;
    padding: 2.5rem 1rem;
    display: flex;
    flex-flow: column;
    align-items: flex-start;
    justify-content: flex-end;
    animation: page-transition 0.3s;
    .animation-block{
      width: 10.25rem;
      height: 7.8rem;
      background-image: url('/img/hud/smartphone/taxiJobTab/notify-img.png');
      background-size: cover;
      background-position: center;
      background-repeat: no-repeat;
      position: absolute;
      top: 6.1rem;
      right: 1.1rem;
      .car-point{
        width: 2.25rem;
        height: 2.25rem;
        position: absolute;
        left: -.6rem;
        bottom: 0;
        display: flex;
        align-items: center;
        justify-content: center;
        border-radius: 50%;
        z-index: 0;
        transform: rotate(15deg);
        .icon{
          position: absolute;
          width: 2.5rem;
          height: 2.5rem;
          background-image: url('/img/hud/smartphone/taxiJobTab/car-icon.png');
          background-size: contain;
          background-repeat: no-repeat;
          background-position: center;
          z-index: 2;
        }
        &:before, &:after{
          content: '';
          position: absolute;
          border-radius: 50%;
        }
        &:before{
          width: 2.25rem;
          height: 2.25rem;
          background: rgba(255, 199, 0, 0.1);
          z-index: -1;
          animation: pulse-large 1s linear infinite;
        }
        &:after{
          width: 2.25rem;
          height: 2.25rem;
          background: rgba(255, 199, 0, 0.3);
          z-index: 1;
          animation: pulse-small 1.7s linear infinite;
        }
      }
    }
    .prompt{
      text-transform: uppercase;
      font-weight: bold;
      font-size: 3rem;
      line-height: 3rem;
      letter-spacing: 0.04em;
      text-transform: uppercase;
      color: #FFFFFF;
    }
    .desc{
      font-size: .8rem;
      line-height: .95rem;
      color: #FFFFFF;
    }
    .current-location{
      display: flex;
      flex-flow: column;
      align-items: flex-start;
      justify-content: center;
      padding-left: .95rem;
      position: relative;
      margin: 1.75rem 0;
      width: 100%;
      &:before, &:after{
        content: '';
        position: absolute;
        left: 0;
        top: 0;
      }
      &:before{
        width: 100%;
        height: 1px;
        background: rgba(255, 255, 255, 0.2);
        transform: translateY(-.75rem);
      }
      &:after{
        background: #FFFFFF;
        border: .2rem solid #F4C10B;
        box-sizing: border-box;
        width: .65rem;
        height: .65rem;
        border-radius: 50%;
      }
      &__desc{
        font-size: .65rem;
        line-height: .75rem;
        color: rgba(255, 255, 255, 0.4);
      }
      &__value{
        font-size: .65rem;
        line-height: .75rem;
        color: #FFFFFF;
        margin-top: .25rem;
      }
    }
    .btn{
      display: flex;
      align-items: center;
      justify-content: center;
      width: 100%;
      height: 2.5rem;
      min-height: 2.5rem;
      background: rgba(255, 255, 255, 0.2);
      border-radius: .5rem;
      font-size: .7rem;
      line-height: .8rem;
      color: #FFFFFF;
    }
  }
</style>
