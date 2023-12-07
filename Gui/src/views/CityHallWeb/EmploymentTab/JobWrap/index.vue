<template>
  <div class="job-wrap">
    <div 
      class="job-wrap__img"
      :style="{backgroundImage: 'url(/img/optionsMenu/informationTab/works/' + selectedJob.img + '.png)'}"
    ></div>
    <div class="job-wrap__level">{{selectedJob.entryLevel}} {{loc('cityHallWeb_28')}}</div>
    <div class="job-wrap__name">{{loc(selectedJob.title)}}</div>
    <div class="job-wrap__desc">{{loc(selectedJob.description)}}</div>
    <div 
      class="page__btn btn"
      @click="getDirection"
    >{{loc('cityHallWeb_27')}}</div>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
export default {
  name: 'JobWrap',

  props: {
    selectedJob: Object
  },
  computed:{
    ...mapGetters('localization', ['loc']),
  },

  methods: {
    getDirection: function() {
      window.mp.triggerServer("mmenu:job:waypoint", this.selectedJob.point)
    }
  }
}
</script>

<style lang="scss" scoped>
.job-wrap{
  display: flex;
  flex-flow: column;
  align-items: flex-start;
  width: 34.5rem;
  height: 100%;
  overflow: hidden;
  background: #F3F5F7;
  border-radius: .9rem;
  padding: 11.6rem 3rem 3rem 3rem;
  position: relative;
  &__img{
    position: absolute;
    top: 0;
    left: 0;
    width: 100%;
    height: 9.9rem;
    background-size: cover;
    background-repeat: no-repeat;
    background-position: center;
  }
  &__level{
    margin-bottom: .2rem;
    font-family: Roboto;
    font-style: normal;
    font-weight: bold;
    font-size: .8rem;
    line-height: .95rem;
    text-transform: uppercase;
    color: #E00B29;
  }
  &__name{
    font-family: Merriweather;
    font-style: normal;
    font-weight: bold;
    font-size: 1.7rem;
    line-height: 2.15rem;
    text-transform: uppercase;
    color: #46555F;
    margin-bottom: .8rem;
  }
  &__desc{
    font-family: Roboto;
    font-style: normal;
    font-weight: normal;
    font-size: .8rem;
    line-height: 1.2rem;
    color: #46555F;
    margin-bottom: 1.7rem;
  }
  .page__btn{
    padding: 0 2.6rem;
  }
}
</style>
