<template>
  <div class="page">
    <div class="page__title">{{loc('cityHallWeb_42')}}</div>
    <div class="page__desc">{{loc('cityHallWeb_46')}}</div>
    <menu-drop
      :dropList="list"
      :currentItem="currentWorker"
      :setCurrentItem="setCurrentItem"
    />
    <div class="page__desc">{{loc('cityHallWeb_35')}}</div>
    <textarea
      :placeholder="loc('cityHallWeb_47')"
      v-model="currentReason"
    ></textarea>
    <div 
      v-if="currentWorker && currentReason"
      class="page__btn btn"
      @click="sendForm"
    >{{loc('cityHallWeb_22')}}</div>
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
import MenuDrop from '../../components/MenuDrop'
export default {
  name: 'RecordingPage',

  components: {
    MenuDrop
  },

  data: function() {
    return{
      currentWorker: null,
      currentReason: null,
    }
  },

  methods: {
    setCurrentItem: function(obj) {
      this.currentWorker = obj
    },
    sendForm: function() {
      window.mp.trigger('cityHallWeb:sendFormRecordingPage', this.currentWorker.id, this.currentReason)
      this.currentWorker = this.list[0]
      this.currentReason = null
    }
  },

  computed: {
    ...mapState('cityHallWeb/recording', ['list']),
    ...mapState('cityHallWeb', ['cardId']),
    ...mapGetters('localization', ['loc'])
  },

  mounted() {
    this.currentWorker = this.list[0]
  }
}
</script>
