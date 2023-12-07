<template>
  <div class="requests-stats">
    <div class="requests-info">
      <div class="value waiting">{{reportsInQueue}}</div>
      <div class="desc">{{loc('ReportMenu_5')}}</div>
    </div>
    <div class="requests-info">
      <div class="value">{{reportsAnswered}}</div>
      <div class="desc">{{loc('ReportMenu_6')}}</div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
export default {
  name: 'RequestsStats',

  computed: {
    ...mapState('reportMenu', ['reportsInQueue', 'reportsAnswered']),
    ...mapGetters('localization', ['loc'])
  }
}
</script>

<style lang="scss" scoped>
.requests-stats{
  background: rgba(38, 40, 46, 0.35);
  width: 100%;
  padding: 1.2rem 2.5rem;
  display: flex;
  align-items: center;
  justify-content: flex-start;
  .requests-info{
    display: flex;
    align-items: center;
    margin-right: 1.5rem;
    &:last-child{
      margin-right: 0;
    }
    .value{
      font-size: 2.05rem;
      color: #C1E704;
      font-weight: 300;
      &.waiting{
        color: #E93E3E;
      }
    }
    .desc{
      font-weight: bold;
      font-size: .7rem;
      line-height: .8rem;
      color: rgba(255, 255, 255, 0.65);
      max-width: 4rem;
      margin-left: .6rem;
      font-family: Roboto;
    }
  }
}
</style>
