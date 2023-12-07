<template>
  <div class="body-tab">
    <div>
      <div class="body-tab__title">{{loc('cityHallWeb_108')}}</div>
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

import RegPage from './RegPage'
import DivorcePage from './DivorcePage'
import { mapGetters } from 'vuex'
export default {
  name: 'RelationshipsTab',

  components: {
    MenuNav,
    RegPage,
    DivorcePage,
  },

  data: function() {
    return{
      currentPage: null,
      navList: [
        {
          key: 'RegPage',
          text: 'cityHallWeb_109'
        },
        {
          key: 'DivorcePage',
          text: 'cityHallWeb_110'
        },
      ],
    }
  },

  computed: {
    ...mapGetters('localization', ['loc']),
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
