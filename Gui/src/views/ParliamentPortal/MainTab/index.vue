<template>
  <div class="main-tab tab-wrap">
    <div class="main-tab__nav">
      <div class="nav-title">{{loc('parliamentPortal_0')}}</div>
      <div class="nav-switch">
        <div 
          :class="[{active: currentSwitch === 1}, 'nav-switch__item']"
          @click="setCurrentSwitch(1)"
        >{{loc('parliamentPortal_1')}}</div>
        <div 
          :class="[{active: currentSwitch === 0}, 'nav-switch__item']"
          @click="setCurrentSwitch(0)"
        >{{loc('parliamentPortal_2')}}</div>
      </div>
      <div class="nav-list">
        <list-item
          v-for="item in currentList"
          :item="item"
          :key="item.id"
        />
      </div>
    </div>
    <current-meeting
      v-if="currentMeeting"
    />
  </div>
</template>

<script>
import { mapGetters, mapMutations, mapState } from 'vuex'
import ListItem from '../components/ListItem'
import CurrentMeeting from '../components/CurrentMeeting'
export default {
  name: 'MainTab',

  components: {
    ListItem,
    CurrentMeeting
  },

  data: function() {
    return { 
      currentSwitch: 1
    }
  },

  methods: {
    ...mapMutations('parliamentPortal', ['setCurrentMeeting']),
    setCurrentSwitch: function(value) {
      if (this.currentSwitch !== value) {
        this.currentSwitch = value
        this.setCurrentMeeting(null)
      }
    }
  },

  computed: {
    ...mapState('parliamentPortal', ['meetings', 'currentMeeting']),
    ...mapGetters('localization', ['loc']),

    currentList: function() {
      let array = this.meetings
      if (this.currentSwitch) {
        return array.filter(element => !element.isCompleted)
      } else {
        return array.filter(element => element.isCompleted)
      }
    }
  }
}
</script>

<style lang="scss" scoped>
.main-tab{
  display: flex;
  &__nav{
    display: flex;
    flex-flow: column;
    width: 25rem;
    min-width: 25rem;
    padding: 2.1rem .1rem 0 1rem;
    height: 100%;
    border-right: 1px solid rgba(70, 85, 95, 0.2);
    .nav-title, .nav-switch{
      margin-left: 1.1rem;
    }
    .nav-title{
      font-family: Merriweather;
      font-style: normal;
      font-weight: bold;
      font-size: 2rem;
      line-height: 2.5rem;
      color: #46555F;
      margin-bottom: 1rem;
    }
    .nav-switch{
      display: flex;
      align-content: center;
      margin-bottom: 1rem;
      &__item{
        font-style: normal;
        font-weight: 500;
        font-size: 1rem;
        line-height: 130%;
        color: rgba(70, 85, 95, 0.24);
        margin-right: 2rem;
        transition: .3s;
        &:hover{
          color: rgba(49, 100, 134, 0.54);
          transition: .3s;
        }
        &:last-child{
          margin-right: 0;
        }
        &.active{
          color: #3FA8E3;
        }
      }
    }
    .nav-list{
      width: 100%;
      overflow-y: auto;
      padding-right: 1.5rem;
      &::-webkit-scrollbar{
        width: .2rem;
        background-color: transparent;
        &-thumb{
          background: rgba(70, 85, 95, 0.2);
          border-radius: .1rem;
        }
      }
    }
  }
}
</style>
