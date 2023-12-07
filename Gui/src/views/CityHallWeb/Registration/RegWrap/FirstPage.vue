<template>
  <div class="page">
    <div class="current-page">1/2</div>
    <div class="page__title">{{loc('cityHallWeb_85')}}</div>
    <div class="page__desc">{{loc('cityHallWeb_86')}}</div>
    <input 
      type="text"
      :placeholder="loc('cityHallWeb_106')"
      :value="name"
      readonly
    >
    <div class="page__desc">{{loc('cityHallWeb_87')}}</div>
    <div class="page__data">
      <div class="page__data-item day">
        <div class="desc">{{loc('cityHallWeb_88')}}</div>
        <input 
          type="number"
          :value="day"
          readonly
        >
      </div>
      <div class="page__data-item month">
        <div class="desc">{{loc('cityHallWeb_89')}}</div>
        <input 
          type="text"
          :value="month.desc"
          readonly
        >
      </div>
      <div class="page__data-item year">
        <div class="desc">{{loc('cityHallWeb_90')}}</div>
        <input 
          type="number"
          :value="year"
          readonly
        >
      </div>
    </div>
    <div 
      class="btn btn-next"
      @click="setCurrentPage('SecondPage')"
    >{{loc('cityHallWeb_91')}}</div>
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
export default {
  name: 'FirstPage',

  props: {
    setCurrentPage: Function
  },

  computed: {
    ...mapState('cityHallWeb', ['name', 'day', 'month', 'year']),
    ...mapGetters('localization', ['loc'])
  }
}
</script>
