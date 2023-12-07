<template>
  <div class="settings-interface">
    <div class="settings-interface-item">
      <div class="settings-interface-tittle">{{loc("mmain:setts:iface:help")}}</div>
      <SettingsSwitch :status="settings.hint" class="settings-special-switch"  @onSwitch="setSettingLocal($event, 'hint')"/>
    </div>
    <div class="settings-interface-item">
      <div class="settings-interface-tittle">{{loc("mmain:setts:iface:pname")}}</div>
      <SettingsSwitch :status="settings.showNames" class="settings-special-switch"  @onSwitch="setSettingLocal($event, 'showNames')"/>
    </div>
    <div class="settings-interface-item">
      <div class="settings-interface-tittle">{{loc("mmain:setts:iface:shud")}}</div>
      <SettingsSwitch :status="settings.showHud" class="settings-special-switch"  @onSwitch="setSettingLocal($event, 'showHud')"/>
    </div>
    <div class="settings-interface-item">
      <div class="settings-interface-tittle">{{loc("mmain:setts:iface:mmap")}}</div>
      <SettingsSwitch :status="settings.showMiniMap" class="settings-special-switch"  @onSwitch="setSettingLocal($event, 'showMiniMap')"/>
    </div>
    <div class="settings-interface-item">
      <div class="settings-interface-tittle">{{loc("mmain:setts:iface:drift")}}</div>
      <SettingsSwitch :status="settings.showDrift" class="settings-special-switch"  @onSwitch="setSettingLocal($event, 'showDrift')"/>
    </div>
    <div class="settings-interface-item">
      <div class="settings-interface-tittle">
        {{loc("mmain:setts:iface:mute")}} 
        <input type="naumber" class="settings-interface-input" v-model="level">
        lvl
      </div>
      <SettingsSwitch :status="settings.muteLowLevel" class="settings-special-switch"  @onSwitch="setSettingLocal($event, 'muteLowLevel')"/>
    </div>
    <div class="settings-interface-item">
      <div class="settings-interface-tittle">{{loc("mmain:setts:iface:shfrac")}}</div>
      <SettingsSwitch 
        :status="settings.showFamilyMembers" 
        class="settings-special-switch"  
        @onSwitch="setSettingLocal($event, 'showFamilyMembers')
      "/>
    </div> 
    <div class="settings-interface-item">
      <div class="settings-interface-tittle">{{loc("mmain:setts:iface:traffic")}}</div>
      <SettingsSwitch 
        :status="settings.trafficOff" 
        class="settings-special-switch"  
        @onSwitch="setSettingLocal($event, 'trafficOff')
    "/>
    </div> 
    <div class="settings-interface-item">
      <div class="settings-interface-tittle">
        {{loc("mmain:setts:iface:chreload")}}
      </div>
      <div class="settings-interface-reload" @click="reloadMicro">
        {{loc("mmain:setts:iface:chreload:tit")}}
      </div>
    </div>
  </div>
</template>

<script>
import SettingsSwitch from '../SettingsSwitch'
import {mapState, mapMutations, mapGetters} from 'vuex'

export default {
  name: 'Interface',
  computed:{
    ...mapState('optionsMenu', ['settings']),
    ...mapGetters('localization', ['loc']),
    level:{
      get(){
        return this.settings.muteLowLevelValue;
      },
      set(val){
        this.setSetting({name: 'muteLowLevelValue', status: val})
      }
    }
  },
  data() {
    return {
      settingsList:[]
    }
  },
  methods: {
    ...mapMutations('optionsMenu', ['setSetting']),
    setSettingLocal(status, name){
        this.setSetting({name, status})
    },
    reloadMicro(){
      window.mp.trigger('v_reload');
    }
  },
  components:{
    SettingsSwitch
  }
}
</script>

<style lang="scss">
.settings-interface{
  &-item{
    height: 3.5rem;
    display: flex;
    justify-content: space-between;
    align-items: center;
    border-bottom: 1px solid rgba(#fff, .15);
  }
  &-tittle{
    letter-spacing: .03rem;
    font-size: 1.2rem;
    line-height: 1.45rem;
  }
  &-reload{
    width: 7.5rem;
    height: 2rem;
    display: flex;
    align-items: center;
    justify-content: center;
    border: 2px solid  #B6D300;
    &:hover{
      background: #B6D300;
      box-shadow: 0px 1rem 3rem rgba(168, 195, 2, 0.5);
      border-radius: 2px;
    }
  }
  &-input{
    width: 2rem;
    height: 2rem;
    border: 1px solid rgba(#fff, .1);
    background-color: rgba(#fff, .05);
    font-size: 1.2rem;
    line-height: 1.45rem;
    text-align: center;
    color: #fff;
    margin: 0 .3rem;
  }
}
</style>
