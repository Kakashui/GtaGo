<template>
  <div class="page">
    <div class="page__title">{{loc('cityHallWeb_109')}}</div>
    <div class="page__prompt">{{loc('cityHallWeb_111')}}</div>
    <div class="page__desc">{{loc('cityHallWeb_112')}}</div>
    <input 
      :placeholder="loc('cityHallWeb_47')"
      v-model="partner"
      type="text"
    >
    <div 
      v-if="partner"
      class="page__btn btn"
      @click="sendForm"
    >{{loc('cityHallWeb_109')}}</div>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
export default {
  name: 'RegPage',

  data: function() {
    return{
      partner: null,
    }
  },

  computed: {
    ...mapGetters('localization',['loc'])
  },

  methods: {
    sendForm: function() {
      window.mp.trigger('cityHallWeb:sendFormRelationshipsReg', this.partner);
      this.partner = null;
    }
  }
}
</script>
