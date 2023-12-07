<template>
  <div class="page">
    <div class="page__title">{{loc('cityHallWeb_45')}}</div>
    <div class="page__prompt">{{loc('cityHallWeb_48')}}.</div>
    <div class="page__desc">{{loc('cityHallWeb_49')}}</div>
    <menu-drop
      :dropList="serviseList"
      :currentItem="currentService"
      :setCurrentItem="setCurrentService"
    />
    <div class="page__desc">{{loc('cityHallWeb_50')}}</div>
    <menu-drop
      :dropList="proxyList"
      :currentItem="currentProxy"
      :setCurrentItem="setCurrentProxy"
    />
    <div class="page__desc">{{loc('cityHallWeb_51')}}</div>
    <input 
      :placeholder="loc('cityHallWeb_52')"
      v-model="currentNumber"
      type="number"
    >
    <div class="page__desc">{{loc('cityHallWeb_54')}}</div>
    <input 
      :placeholder="loc('cityHallWeb_53')"
      v-model="currentMail"
      type="text"
    >
    <div 
      v-if="currentService && currentProxy && currentNumber && currentMail"
      class="page__btn btn"
      @click="sendForm"
    >{{loc('cityHallWeb_22')}}</div>
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
import MenuDrop from '../../components/MenuDrop'
export default {
  name: 'NotaryPage',

  components: {
    MenuDrop
  },

  data: function() {
    return{
      currentService: null,
      currentProxy: null,
      currentNumber: null,
      currentMail: null,
    }
  },

  methods: {
    setCurrentService: function(obj) {
      this.currentService = obj
    },
    setCurrentProxy: function(obj) {
      this.currentProxy = obj
    },

    sendForm: function() {
      window.mp.trigger('cityHallWeb:sendFormNotaryPage', this.currentService, this.currentProxy, this.currentNumber, this.currentMail)
      this.currentService = this.serviseList[0]
      this.currentProxy = this.proxyList[0]
      this.currentNumber = null
      this.currentMail = null
    }
  },

  computed: {
    ...mapState('cityHallWeb/notary', ['serviseList', 'proxyList']),
    ...mapState('cityHallWeb', ['cardId']),
    ...mapGetters('localization', ['loc'])
  },

  mounted() {
    this.currentService = this.serviseList[0]
    this.currentProxy = this.proxyList[0]
  }
}
</script>
