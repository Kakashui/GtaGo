<template>
  <div class="page">
    <div class="page__title">{{loc('cityHallWeb_65')}}</div>
    <div class="page__prompt">{{loc('cityHallWeb_66')}}</div>
    <div class="page__desc">{{loc('cityHallWeb_67')}}</div>
    <input 
      :placeholder="loc('cityHallWeb_47')"
      v-model="currentName"
      type="text"
    >
    <div class="page__desc">{{loc('cityHallWeb_68')}}</div>
    <textarea
      :placeholder="loc('cityHallWeb_69')"
      v-model="currentUrl"
    ></textarea>
    <div 
      v-if="currentName && currentUrl"
      class="page__btn btn"
      @click="sendForm"
    >{{loc('cityHallWeb_22')}}</div>
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
export default {
  name: 'BillPage',

  data: function() {
    return{
      currentName: null,
      currentUrl: null,
    }
  },

  computed: {
    ...mapState('cityHallWeb', ['cardId']),
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    sendForm: function() {
      window.mp.trigger('cityHallWeb:sendFormBillPage', this.currentName, this.currentUrl)
      this.currentName = null
      this.currentUrl = null
    }
  }
}
</script>
