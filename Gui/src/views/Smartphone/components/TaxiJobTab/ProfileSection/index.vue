<template>
  <div class="profile-section">
    <div class="current-income">
      <div class="current-income__desc">{{loc('phone:taxi:profile:1')}}</div>
      <div class="current-income__value">{{taxiJobData.profile.TotalSumForDay}}</div>
    </div>
    <div class="prompt">{{loc('phone:taxi:profile:2')}}</div>
    <div class="current-drive">
      <div class="current-drive__desc">{{loc('phone:taxi:profile:3')}}</div>
      <div class="current-drive__value"><span>{{taxiJobData.profile.TotalTripsForDay}}</span> pice</div>
    </div>
    <div class="statistics">
      <div class="statistics__item">
        <div class="item-desc" :v-html="loc('phone:taxi:profile:4')"></div>
        <div class="item-value cash">{{taxiJobData.profile.TotalSumForMonth}}</div>
      </div>
      <div class="statistics__item">
        <div class="item-desc" :v-html="loc('phone:taxi:profile:5')"></div>
        <div class="item-value cash">{{taxiJobData.profile.TotalSum}}</div>
      </div>
      <div class="statistics__item">
        <div class="item-desc" :v-html="loc('phone:taxi:profile:6')"></div>
        <div class="item-value">{{taxiJobData.profile.TotalTripsForMonth}}</div>
      </div>
      <div class="statistics__item">
        <div class="item-desc" :v-html="loc('phone:taxi:profile:7')"></div>
        <div class="item-value">{{taxiJobData.profile.TotalTrips}}</div>
      </div>
    </div>
    <div class="btn"
      @click="endWorkDay" 
      v-if="taxiJobData.stateWork === 'working'"
    >{{loc('phone:taxi:profile:8')}}</div>
    <div class="btn"
      v-else
      @click="startWorkDay"
    >{{loc('phone:taxi:profile:9')}}</div>
  </div>
</template>

<script>
import { mapState, mapMutations, mapGetters } from 'vuex'

export default {
  name: 'ProfileSection',

  computed: {
    ...mapState('smartphone', ['currentTab']),
    ...mapState('smartphone/taxiPage', ['taxiJobData']),
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    ...mapMutations('smartphone', ['setCurrentTab']),
    ...mapMutations('smartphone/taxiPage', ['taxijob_setStateWork']),

    startWorkDay: function () {
      this.setCurrentTab({ key: this.currentTab.key, section: 'OrdersSection', data: null })
      this.taxijob_setStateWork({value: 'working'})
      window.mp.triggerServer('phone::taxijob::startWorkDay');
    },

    endWorkDay: function () {
      this.setCurrentTab({ key: this.currentTab.key, section: 'GeneralSection', data: null })
      this.taxijob_setStateWork({value: 'not working'})
      window.mp.triggerServer('phone::taxijob::endWorkDay');
    }
  }
}
</script>

<style lang="scss" scoped>
  @keyframes page-transition {
    from {
      opacity: 0;
      transform: translateY(100%)
    }
    to {
      opacity: 1;
      transform: translateY(0)
    }
  }
  .profile-section{
    background-image: url('/img/hud/smartphone/taxiJobTab/profile-bg.png');
    background-color:  #FDFCFD;
    background-size: contain;
    background-position: top;
    background-repeat: no-repeat;
    height: 100%;
    width: 100%;
    padding: 2.85rem .75rem 1.5rem .75rem;
    display: flex;
    flex-flow: column;
    align-items: center;
    justify-content: flex-end;
    &>div{
      animation: page-transition 0.4s;
    }
    .prompt{
      position: absolute;
      top: 2.85rem;
      font-size: .6rem;
      line-height: .8rem;
      color: #FFFFFF;
    }
    .current-income{
      display: flex;
      flex-flow: column;
      align-self: center;
      justify-content: center;
      text-align: center;
      margin-bottom: 1.2rem;
      color: #FFFFFF;
      &__desc{
        font-size: .8rem;
        line-height: .95rem;
        margin-bottom: .1rem;
      }
      &__value{
        font-weight: 300;
        font-size: 2.5rem;
        line-height: 2.95rem;
        &:after{
          content: '$';
        }
      }
    }
    .current-drive{
      display: flex;
      flex-flow: column;
      align-items: flex-start;
      justify-content: space-between;
      padding: 1rem 1rem .65rem 1rem;
      background: #FFFFFF;
      box-shadow: 0 .5rem 1.5rem rgba(177, 188, 203, 0.5);
      border-radius: .7rem;
      overflow: hidden;
      position: relative;
      width: 100%;
      height: 4.5rem;
      min-height: 4.5rem;
      &__desc{
        font-size: .7rem;
        line-height: .8rem;
        color: rgba(0, 0, 0, 0.4);
      }
      &__value{
        color: #000000;
        font-size: .7rem;
        span{
          font-size: 1.5rem;
          line-height: 1.75rem;
          margin-right: .25rem;
        }
      }
      &:after{
        content: '';
        position: absolute;
        align-self: center;
        right: 0;
        top: 50%;
        margin-top: -1.95rem;
        height: 3.9rem;
        width: 6.3rem;
        background-image: url('/img/hud/smartphone/taxiJobTab/car-profile.png');
        background-size: contain;
        background-position: center right;
        background-repeat: no-repeat;
      }
    }
    .statistics{
      width: 100%;
      display: grid;
      background: #EBEBEB;
      grid-template-columns: repeat(2, 1fr);
      grid-auto-rows: 4rem;
      grid-gap: 1px;
      margin: 1.25rem 0 1.95rem 0;
      &__item{
        display: flex;
        flex-flow: column;
        align-items: flex-start;
        justify-content: space-between;
        padding: 0 1rem;
        background-color:  #FDFCFD;
        &:nth-child(1), &:nth-child(2){
          padding-bottom: .75rem;
          padding-top: .3rem;
        }
        &:nth-child(3), &:nth-child(4){
          padding-top: .75rem;
          padding-bottom: .3rem;
        }
        .item-desc{
          font-size: .7rem;
          line-height: .8rem;
          color: rgba(0, 0, 0, 0.4);
        }
        .item-value{
          font-size: .8rem;
          line-height: .95rem;
          color: #000000;
          &.cash{
            &:after{
              content: '$';
            }
          }
        }
      }
    }
    .btn{
      background: #F1C115;
      border-radius: .5rem;
      width: 100%;
      height: 2.5rem;
      min-height: 2.5rem;
      display: flex;
      align-items: center;
      justify-content: center;
      font-size: .7rem;
      line-height: .8rem;
      color: #FFFFFF;
      &:hover{
        box-shadow: 0 .5rem 1.5rem rgba(244, 193, 11, 0.3);
        transition: .3s;
      }
    }
  }
</style>
