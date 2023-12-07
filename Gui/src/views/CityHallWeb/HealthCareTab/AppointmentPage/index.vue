<template>
  <div class="page">
    <div class="page__title">{{loc('cityHallWeb_33')}}</div>
    <div class="page__prompt">{{loc('cityHallWeb_38')}}.</div>
    <div 
      class="page__btn btn"
      @click="sendForm"
    >{{loc('cityHallWeb_27')}}</div>
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
export default {
  name: 'AppointmentPage',

  methods: {
    setCurrentItem: function(obj) {
      this.currentDestination = obj
    },

    sendForm: function() {
      window.mp.trigger('cityHallWeb:sendFormAppointmentPage')
    }
  },

  computed: {
    ...mapState('cityHallWeb', ['cardId']),
    ...mapGetters('localization', ['loc'])
  }
}
</script>

<style lang="scss" scoped>
.city-hall-web{
  .body-tab{
    .page{
      &__prompt{
        color: #46555F;
      }
    }
  }
}
</style>
