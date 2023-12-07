<template>
  <div class="page">
    <div class="page__title">{{loc('cityHallWeb_113')}}</div>
    <div class="page__prompt">{{loc('cityHallWeb_114')}}</div>
    <div class="page__desc">{{loc('cityHallWeb_112')}}</div>
    <input 
      :placeholder="loc('cityHallWeb_47')"
      v-model="partner"
      type="text"
    >

    <Checkbox
      :checkboxKey="'dispute'"
      :flag="dispute"
      :text="loc(disputeText)"
      @change="changeCheckbox"
    />
    <div 
      v-if="partner"
      class="page__btn btn"
      @click="sendForm"
    >{{loc('cityHallWeb_116')}}</div>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
import Checkbox from '../../components/Checkbox'
export default {
  name: 'DivorcePage',

  components: {
    Checkbox
  },

  data: function() {
    return{
      partner: null,
      dispute: false,
      disputeText: 'cityHallWeb_115',
    }
  },

  computed: {
    ...mapGetters('localization',['loc'])
  },

  methods: {
    sendForm: function() {
      window.mp.trigger('cityHallWeb:sendFormRelationshipsDivorce', this.partner, this.dispute)
      this.partner = null
      this.dispute = false
    },

    changeCheckbox: function() {
      this.dispute = !this.dispute
    }
  }
}
</script>
