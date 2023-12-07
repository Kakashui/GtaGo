<template>
  <div class="page">
    <div class="current-page">2/2</div>
    <div class="page__desc">{{loc('cityHallWeb_92')}}</div>
    <textarea 
      :placeholder="loc('cityHallWeb_47')"
      v-model="place"
    ></textarea>
    <div class="page__desc">{{loc('cityHallWeb_93')}}</div>
    <textarea 
      :placeholder="loc('cityHallWeb_107')"
      v-model="target"
    ></textarea>
    <div 
      v-if="place && target"
      class="btn btn-next"
      @click="goToCard"
    >{{loc('cityHallWeb__94')}}</div>
    <div 
      v-else
      class="btn btn-next"
    >{{loc('cityHallWeb__94')}}</div>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
export default {
  name: 'SecondPage',

  props: {
    setCurrentPage: Function
  },

  data: function() {
    return{
      place: null,
      target: null
    }
  },

  computed: {
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    goToCard: function() {
      this.$emit('goToCard')
    }
  }
}
</script>

<style lang="scss" scoped>
.second-page{
  display: flex;
  flex-flow: column;
}
</style>
