<template>
  <div class="page">
    <div class="page__title">{{loc('cityHallWeb_55')}}</div>
    <div class="page__prompt">{{loc('cityHallWeb_56')}}</div>
    <div class="page__desc">{{loc('cityHallWeb_57')}}</div>
    <menu-drop
      :dropList="list"
      :currentItem="currentDestination"
      :setCurrentItem="setCurrentItem"
    />
    <div class="page__desc">{{loc('cityHallWeb_58')}}</div>
    <textarea
      :placeholder="loc('cityHallWeb_36')"
      v-model="currentQuestion"
    ></textarea>
    <div 
      v-if="currentDestination && currentQuestion"
      class="page__btn btn"
      @click="sendForm"
    >{{loc('cityHallWeb_22')}}</div>
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
import MenuDrop from '../../components/MenuDrop'
export default {
  name: 'LetterPage',

  components: {
    MenuDrop
  },

  data: function() {
    return{
      currentDestination: null,
      currentQuestion: null,
    }
  },

  methods: {
    setCurrentItem: function(obj) {
      this.currentDestination = obj
    },

    sendForm: function() {
      window.mp.trigger('cityHallWeb:sendMessageToCityhall', this.currentDestination, this.currentQuestion)
      this.currentDestination = this.list[0]
      this.currentQuestion = null
    }
  },

  computed: {
    ...mapState('cityHallWeb/letter', ['list']),
    ...mapState('cityHallWeb', ['cardId']),
    ...mapGetters('localization', ['loc'])
  },

  mounted() {
    this.currentDestination = this.list[0]
  }
}
</script>
