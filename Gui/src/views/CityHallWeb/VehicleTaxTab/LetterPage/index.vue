<template>
  <div class="page">
    <div class="page__title">{{loc('cityHallWeb_133')}}</div>
    <div class="page__prompt">{{loc('cityHallWeb_136')}}</div>
    <textarea
      :placeholder="loc('chall:modal:plh:2')"
      v-model="currentLetter"
    ></textarea>
    <div 
      v-if="currentLetter"
      class="page__btn btn"
      @click="sendForm"
    >{{loc('cityHallWeb_22')}}</div>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
export default {
  name: 'LetterPage',

  data: function() {
    return{
      currentLetter: null,
    }
  },

  methods: {

    sendForm: function() {
      window.mp.trigger('cityHallWeb:sendFormLetterPage', this.currentLetter)
      this.currentLetter = null
    }
  },

  computed: {
    ...mapGetters('localization', ['loc'])
  }
}
</script>
