<template>
  <div class="body-tab">
    <div>
      <div class="body-tab__title">{{loc('cityHallWeb_117')}}</div>
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
import NameEditPage from './NameEditPage'
import LicensesPage from './LicensesPage'
import StatusPage from './StatusPage'
import { mapGetters } from 'vuex'
export default {
  name: 'SocialTab',

  components: {
    MenuNav,
    NameEditPage,
    LicensesPage,
    StatusPage,
  },

  data: function() {
    return{
      currentPage: null,
      navList: [
        {
          key: 'NameEditPage',
          text: 'cityHallWeb_118'
        },
        {
          key: 'LicensesPage',
          text: 'cityHallWeb_119'
        },
        {
          key: 'StatusPage',
          text: 'cityHallWeb_121'
        }
      ],
    }
  },

  computed: {
    ...mapGetters('localization',['loc'])
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

<style lang="scss" scoped>
.body-tab{
  &__title{
    max-width: 29.5rem;
  }
}
</style>
