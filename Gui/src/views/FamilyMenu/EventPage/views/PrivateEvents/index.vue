<template>
<transition name="slide-fade-reversed">
  <div v-if="showNavElem == 'PrivateEvents'" class="private-events">
    <organization-block
      :setCurrentChoice="setCurrentChoice"
      :choiceList="choiceList"
      :currentChoice="currentChoice"
    />
    <component 
      :is="currentChoice"
      :currentPlace="currentPlace"
      :currentBuiss="currentBuiss"
      :currentWeapon="currentWeapon"
      :setCurrentPlace="setCurrentPlace"
      :setCurrentBuiss="setCurrentBuiss"
      :setCurrentWeapon="setCurrentWeapon"
    />
    <result-block
      :currentPlace="currentPlace"
      :currentBuiss="currentBuiss"
      :currentWeapon="currentWeapon"
      :setNotification="setNotification"
      @pushRegBattle="setNullData"
    />
    <Notification 
      v-if="notification"
      :message="notificationMessage"
      :result="notificationResult"
      :setNotification="setNotification"
    />
  </div>
</transition>
</template>

<script>
import { mapState, mapGetters } from 'vuex'

import OrganizationBlock from './OrganizationBlock'
import ResultBlock from './ResultBlock'
import TargetChoice from './TargetChoice'
import PlaceChoice from './PlaceChoice'
import WeaponChoice from './WeaponChoice'
import Notification from './Notification'

export default {
  name: 'PrivateEvents',
  data: function() {
    return {
      currentChoice: '',
      currentPlace: null,
      currentBuiss: null,
      currentWeapon: null,
      choiceList: [
        {
          text: 'fam:btl:priv:1',
          key: 'TargetChoice'
        },
        {
          text: 'fam:btl:priv:2',
          key: 'PlaceChoice'
        },
        {
          text: 'fam:btl:priv:3',
          key: 'WeaponChoice'
        },
      ],
    }
  },
  components: {
    OrganizationBlock,
    ResultBlock,
    TargetChoice,
    PlaceChoice,
    WeaponChoice,
    Notification
  },
  computed:{
    ...mapState('familyMenu/eventsPage', ['showNavElem', 'businessList', 'isLocked', 'notification', 'notificationMessage', 'notificationResult']),
    ...mapGetters('familyMenu/eventsPage', ['setNotificationValue'])
  },
  methods: {
    setCurrentChoice: function(value) {
      this.currentChoice = value
    },
    setCurrentPlace: function(value) {
      this.currentPlace = value
    },
    setCurrentBuiss: function(value) {
      this.currentBuiss = value
    },
    setCurrentWeapon: function(value) {
      this.currentWeapon = value
    },

    setNullData: function() {
      this.currentPlace = null
      this.currentBuiss = null
      this.currentWeapon = null
    },

    setNotification: function() {
      this.setNotificationValue(!this.notification)
    }
  }, 
  mounted() {
    this.currentChoice = this.choiceList[0].key
  }
}

</script>

<style lang="scss" scoped>

.slide-fade-reversed-enter-active, .slide-fade-reversed-leave-active {
  transition: all .3s ease;
}
.slide-fade-reversed-enter, .slide-fade-reversed-leave-to {
  transform: translateX(-3rem);
  opacity: 0;
}

.private-events {
  display: flex;
  justify-content: space-between;
  width: 100%;
  height: 100%;
  position: relative;
}

</style>

<style lang="scss">
.private-events{
  .choice{
    width: 100%;
  }
}
</style>
