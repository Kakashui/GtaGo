<template>
  <div class="member-page">
    <div class="member-page__nav">
      <div 
        :class="[{active: item.key === currentTab}, 'item']"
        v-for="item in navList"
        :key="item.id"
        @click="setCurrentTab(item.key)"
      >{{loc(item.text)}}</div>
    </div>
    <component :is="currentTab"/>
  </div>

</template>

<script>
import { mapGetters } from 'vuex'
import AccessTab from './AccessTab'
import RatingTab from './RatingTab'
export default {
  name: 'MemberPage',

  components: {
    AccessTab,
    RatingTab,
  },
  
  data: function() {
    return {
      currentTab: '',
      navList: [
        {
          text: 'fam:menu:member:1',
          key: 'AccessTab'
        },
        {
          text: 'fam:menu:member:2',
          key: 'RatingTab'
        }
      ]
    }
  },

  methods: {
    setCurrentTab: function(value) {
      this.currentTab = value
    }
  },

  computed: {
    ...mapGetters('localization', ['loc']),
  },

  mounted() {
    this.currentTab = this.navList[0].key
  }
}
</script>

<style lang="scss" scoped>
.member-page{
  display: flex;
  align-items: flex-start;
  padding-top: 6rem;
  position: relative;
  width: 100%;
  &__nav{
    position: absolute;
    top: 2rem;
    left: 0;
    display: flex;
    align-items: center;
    .item{
      display: flex;
      margin-right: 1.5rem;
      border-bottom: .1rem solid transparent;
      padding-bottom: .3rem;
      font-size: 1.2rem;
      line-height: 1.45rem;
      letter-spacing: 0.18em;
      transition: .2s;
      &.active, &:hover{
        color: #DCFF00;
        transition: .2s;
      }
      &.active{
        border-color: #DCFF00;
      }
      &:last-child{
        margin-right: 0;
      }
    }
  }
}
</style>
