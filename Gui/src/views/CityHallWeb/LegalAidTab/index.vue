<template>
  <div class="body-tab">
    <div>
      <div class="body-tab__title">{{loc('cityHallWeb_39')}}</div>
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

import LetterPage from './LetterPage'
import ComplaintPage from './ComplaintPage'
import RecordingPage from './RecordingPage'
import DebatePage from './DebatePage'
import BillPage from './BillPage'
import NotaryPage from './NotaryPage'
import { mapGetters } from 'vuex'
export default {
  name: 'LegalAidTab',

  components: {
    MenuNav,
    LetterPage,
    ComplaintPage,
    RecordingPage,
    DebatePage,
    BillPage,
    NotaryPage
  },

  data: function() {
    return{
      currentPage: null,
      navList: [
        {
          key: 'LetterPage',
          text: 'cityHallWeb_40'
        },
        {
          key: 'ComplaintPage',
          text: 'cityHallWeb_41'
        },
        {
          key: 'RecordingPage',
          text: 'cityHallWeb_42'
        },
        {
          key: 'DebatePage',
          text: 'cityHallWeb_43'
        },
        {
          key: 'BillPage',
          text: 'cityHallWeb_44'
        },
        {
          key: 'NotaryPage',
          text: 'cityHallWeb_45'
        },
      ],
    }
  },

  methods: {
    setCurrentPage: function(value) {
      this.currentPage = value
    }
  },

  computed: {
    ...mapGetters('localization', ['loc'])
  },

  mounted() {
    this.currentPage = this.navList[0].key
  }
}
</script>
