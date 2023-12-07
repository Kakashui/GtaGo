<template>
  <div class="body-tab">
    <div>
      <div class="body-tab__title">{{loc('cityHallWeb_130')}}</div>
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
import { mapGetters } from 'vuex'
import MenuNav from '../components/MenuNav'

import LetterPage from './LetterPage'
import TaxPage from './TaxPage'
export default {
  name: 'LegalAidTab',

  components: {
    MenuNav,
    LetterPage,
    TaxPage,
  },

  data: function() {
    return{
      currentPage: null,
      navList: [
        {
          key: 'LetterPage',
          text: 'cityHallWeb_131'
        },
        {
          key: 'TaxPage',
          text: 'cityHallWeb_132'
        },
      ],
    }
  },

  computed: {
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    setCurrentPage: function(value) {
      this.currentPage = value
    },
  },

  mounted() {
    this.currentPage = this.navList[0].key
  }
}
</script>
