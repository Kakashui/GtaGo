<template>
  <div class="parliament-portal">
    <div class="parliament-portal__tablet">
      <component 
        :is="currentTab"
        :setCurrentTab="setCurrentTab"
      />
      <Decorate
        v-if="!currentMeeting"
      />
    </div>
  </div>
</template>

<script>
import EnterTab from './EnterTab'
import MainTab from './MainTab'
import Decorate from './components/Decorate'
import { mapState } from 'vuex'
export default {
  name: 'ParliamentPortal',

  components: {
    EnterTab,
    MainTab,
    Decorate
  },

  data: function() {
    return {
      currentTab: null
    }
  },

  methods: {
    setCurrentTab: function(value) {
      this.currentTab = value
    }
  },

  computed: {
    ...mapState('parliamentPortal', ['currentMeeting', 'partiesList'])
  },

  created() {
    this.setCurrentTab('EnterTab')
  }
}
</script>

<style lang="scss" scoped>
.parliament-portal{
  display: flex;
  align-items: center;
  justify-content: center;
  width: 100%;
  height: 100vh;
  font-family: Roboto;
  &__tablet{
    display: flex;
    padding: 3rem;
    height: 51.6rem;
    width: 74.4rem; 
    position: relative;
    background-size: contain;
    background-position: center;
    background-repeat: no-repeat;
    background-image: url('/img/parliamentPortal/bg.png');
  }
}
</style>

<style lang="scss" >
.parliament-portal{
  .btn{
    display: flex;
    align-items: center;
    justify-content: center;
    position: relative;
    transition: .3s;
    &:hover{
      transition: .3s;
    }
  }
  .tab-wrap{
    width: 100%;
    height: 100%;
    position: relative;
    border-radius: 1rem;
    overflow: hidden;
    background-color: #F3F5F7;
  }
  &__tablet{
    &>div{
      z-index: 1;
      &.decorate{
        z-index: 2;
      }
    }
  }
}
</style>
