<template>
  <div class="page">
    <div class="page__title">{{loc('cityHallWeb_32')}}</div>
    <div class="page__desc">{{loc('cityHallWeb_34')}}</div>
    <menu-drop
      :dropList="list"
      :currentItem="currentSertificate"
      :setCurrentItem="setCurrentItem"
    />
    <div class="page__desc">{{loc('cityHallWeb_35')}}</div>
    <textarea
      :placeholder="loc('cityHallWeb_36')"
      v-model="currentReason"
    ></textarea>
    <div 
      v-if="currentSertificate && currentReason"
      class="page__btn btn"
      @click="sendForm"
    >{{loc('cityHallWeb_37')}}</div>
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
import MenuDrop from '../../components/MenuDrop'
export default {
  name: 'CertificatePage',

  components: {
    MenuDrop
  },

  data: function() {
    return{
      currentSertificate: null,
      currentReason: null,
    }
  },

  methods: {
    setCurrentItem: function(obj) {
      this.currentSertificate = obj
    },

    sendForm: function() {
      window.mp.trigger('cityHallWeb:sendFormCertificatePage', this.currentSertificate, this.currentReason)
      this.currentSertificate = this.list[0]
      this.currentReason = null
    }
  },

  computed: {
    ...mapState('cityHallWeb/certificate', ['list']),
    ...mapState('cityHallWeb', ['cardId']),
    ...mapGetters('localization', ['loc'])
  },

  mounted() {
    this.currentSertificate = this.list[0]
  }
}
</script>
