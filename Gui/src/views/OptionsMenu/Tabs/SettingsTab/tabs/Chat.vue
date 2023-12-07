<template>
  <div class="settings-chat">
      <!-- <img src="/img/optionsMenu/settings/lockbig.png" alt="" class="settings-chat-lock"> -->
      <div class="settings-chat-item">
        <div class="settings-chat-tittle">{{loc("mmain:setts:chat:censore")}}</div>
        <SettingsSwitch 
          :status="settings.censore" 
          class="settings-special-switch"  
          @onSwitch="setSettingLocal($event, 'censore')
        "/>
      </div>
  </div>
</template>

<script>
import {mapState, mapMutations, mapGetters} from 'vuex'
import SettingsSwitch from '../SettingsSwitch'
export default {
  name: 'Chat',
   computed:{
    ...mapState('optionsMenu', ['settings']),
    ...mapGetters('localization', ['loc']),   
  },
  methods: {
    ...mapMutations('optionsMenu', ['setSetting']),
    setSettingLocal(status, name){
        this.setSetting({name, status})
    }
  },
  components:{
    SettingsSwitch
  }
}
</script>

<style lang="scss">
.settings-chat{
    height: 100%;
    display: flex;
    align-items: center;
    justify-content: center;
    &-item{
      height: 3.5rem;
      width: 100%;
      display: flex;
      justify-content: space-between;
      align-items: center;
      border-bottom: 1px solid rgba(#fff, .15);
    }
}
</style>
