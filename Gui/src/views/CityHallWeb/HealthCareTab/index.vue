<template>
  <div :class="[{banner: donationConsent}, 'body-tab']">
    <div 
      class="banner-donation"
      v-if="donationConsent"
    >
      <div class="text">{{loc('cityHallWeb_29')}}</div>
      <div 
        class="btn btn-apply"
        @click="applyForDontaion"
      >{{loc('cityHallWeb_30')}}</div>
    </div>
    <div>
      <div class="body-tab__title">{{loc('cityHallWeb_31')}}</div>
      <menu-nav
        :list="navList"
        :currentPage="currentPage"
        :setCurrentPage="setCurrentPage"
      />
    </div>
    <div>
      <component
        :is="currentPage"
      />
    </div>
  </div>
</template>

<script>
import MenuNav from '../components/MenuNav'

import CertificatePage from './CertificatePage'
import AppointmentPage from './AppointmentPage'
import { mapGetters, mapState } from 'vuex'
export default {
  name: 'LegalAidTab',

  components: {
    MenuNav,
    CertificatePage,
    AppointmentPage,
  },

  data: function() {
    return{
      currentPage: null,
      navList: [
        {
          key: 'CertificatePage',
          text: 'cityHallWeb_32'
        },
        {
          key: 'AppointmentPage',
          text: 'cityHallWeb_33'
        }
      ],
    }
  },

  computed: {
    ...mapState('cityHallWeb', ['cardId', 'donationConsent']),
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    setCurrentPage: function(value) {
      this.currentPage = value
    },

    applyForDontaion: function() {
      window.mp.trigger('cityHallWeb:applyForDonation', this.cardId)
    }
  },

  mounted() {
    this.currentPage = this.navList[0].key
  }
}
</script>

<style lang="scss" scoped>
.city-hall-web{
  .body-tab{
    &.banner{
      padding-top: 11.5rem;
    }
    .banner-donation{
      display: flex;
      flex-flow: initial;
      align-items: center;
      justify-content: center;
      width: calc(100% - (3rem));
      height: 8.35rem;
      position: absolute;
      left: 1.5rem;
      top: 1.5rem;
      background-image: url('/img/cityHallWeb/healthy.png');
      background-size: cover;
      background-repeat: no-repeat;
      background-position: center;
      overflow: hidden;
      border-radius: .3rem;
      .text{
        text-transform: uppercase;
        font-style: normal;
        font-weight: 800;
        font-size: 1.5rem;
        line-height: 1.7rem;
        color: #FFFFFF;
        max-width: 29.8rem;
        margin-right: 2rem;
      }
      .btn-apply{
        min-height: 3.3rem;
        height: 3.3rem;
        text-transform: uppercase;
        font-style: normal;
        font-weight: bold;
        font-size: 1.1rem;
        text-align: center;
        color: #FFFFFF;
        background: #E00B29;
        border-radius: .25rem;
        padding: 0 2.5rem;
        &:hover{
          background: #C00621;
        }
      }
    }
  }
}
</style>
