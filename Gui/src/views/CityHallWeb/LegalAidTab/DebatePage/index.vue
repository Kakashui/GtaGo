<template>
  <div class="page">
    <div class="page__title">{{loc('cityHallWeb_59')}}</div>
    <div class="page__img"></div>
    <div class="page__prompt">{{loc(debateDescFeature)}}</div>
    <div 
      class="page__btn btn"
      @click="sendForm"
    >{{loc('cityHallWeb_60')}}</div>
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
export default {
  name: 'DebatePage',

  computed: {
    ...mapState('cityHallWeb', ['cardId']),
    ...mapState('cityHallWeb/vote', ['debateDescFeature']),
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    sendForm: function() {
      window.mp.trigger('cityHallWeb:sendFormDebatePage')
    }
  }
}
</script>

<style lang="scss" scoped>
.page{
  &__img{
    width: 100%;
    height: 5.85rem;
    background-size: contain;
    background-repeat: no-repeat;
    background-position: center;
    background-image: url('/img/cityHallWeb/debate.png');
    margin-bottom: 1rem;
  }
}
</style>
