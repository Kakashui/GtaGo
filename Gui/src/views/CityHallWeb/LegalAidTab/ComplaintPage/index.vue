<template>
  <div class="page">
    <div class="page__title">{{loc('cityHallWeb_61')}}</div>
    <div class="page__desc">{{loc('cityHallWeb_62')}}</div>
    <menu-drop
      :dropList="list"
      :currentItem="currentStructure"
      :setCurrentItem="setCurrentItem"
    />
    <div class="page__desc">{{loc('cityHallWeb_63')}}</div>
    <input 
      :placeholder="loc('cityHallWeb_47')"
      v-model="currentWorker"
      type="text"
    >
    <div class="page__desc">{{loc('cityHallWeb_64')}}</div>
    <textarea
      :placeholder="loc('cityHallWeb_47')"
      v-model="currentDescription"
    ></textarea>
    <div 
      v-if="currentStructure && currentWorker && currentDescription"
      class="page__btn btn"
      @click="sendForm"
    >{{loc('cityHallWeb_22')}}</div>
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
import MenuDrop from '../../components/MenuDrop'
export default {
  name: 'ComplaintPage',

  components: {
    MenuDrop
  },

  data: function() {
    return{
      currentStructure: null,
      currentWorker: null,
      currentDescription: null,
    }
  },

  computed: {
    ...mapState('cityHallWeb/complaint', ['list']),
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    setCurrentItem: function(obj) {
      this.currentStructure = obj
    },

    sendForm: function() {
      window.mp.trigger('cityHallWeb:sendFormComplaintPage', this.currentStructure.id, this.currentWorker, this.currentDescription)
      this.currentStructure = this.list[0]
      this.currentWorker = null
      this.currentDescription = null
    }
  },

  mounted() {
    this.currentStructure = this.list[0]
  }
}
</script>
