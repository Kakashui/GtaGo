<template>
  <div class="settings-tab">
    <div class="settings-tab-left">
      <div class="settings-tab-left-tittle">{{loc('mm_setts_tit')}}</div>
      <div class="settings-tab-left-subtittle">
        {{loc('mm_setts_sub_1')}}
         <br>        
        {{loc('mm_setts_sub_2')}}
      </div>
      <div class="settings-tab-left-buttons">
        <div class="settings-tab-left-button" 
          v-for="(btn, index) in buttons" 
          :key="index"
          :class="{'settings-tab-left-button-selected': index == current}"
          @click="current = index"
        >
          {{loc(btn.name)}}
        </div>
      </div>
    </div>
    <component :is="buttons[current].tab1" class="settings-tab-center"/>
    <component :is="buttons[current].tab2" class="settings-tab-right"/>
  </div>
</template>

<script>
import Interface from './tabs/Interface'
import Special from './tabs/Special'
import Controls from './tabs/Controls'
import ControlInfo from './tabs/ControlInfo'
import Chat from './tabs/Chat'
import Empty from './tabs/Empty'
import Account from './tabs/Account'
import AutoLogin from './tabs/AutoLogin'
import { mapGetters } from 'vuex'

export default {
  name: 'SettingsTab',
  data() {
    return {
      current: 0,
      buttons:[
        {name: "mm_setts_m_t_1", tab1: "Interface", tab2: "Special"},        
        {name: "mm_setts_m_t_2", tab1: "Controls", tab2: "ControlInfo"},        
        {name: "mm_setts_m_t_3", tab1: "Chat", tab2: "Empty"},        
        {name: "mm_setts_m_t_4", tab1: "Account", tab2: "AutoLogin"}
      ]
    }
  },
  computed:{
    ...mapGetters('localization', ['loc'])
  },
  components:{
    Interface,
    Special,
    Controls,
    ControlInfo,
    Chat,
    Empty,
    AutoLogin,
    Account
  }
}
</script>

<style lang="scss" scoped>
.settings-tab{
  display: flex;
  justify-content: space-between;
  color: #fff;
  margin-top: 3.5rem;
  &-left{
    width: 20rem;
    min-height: 30rem;
    &-tittle{
      font-size: 5rem;
      line-height: 6rem;
      font-weight: normal;
    }
    &-subtittle{
      color: rgba(#fff, .5);
      font-size: .9rem;
      line-height: 1.2rem;
      letter-spacing: .04rem;
    }
    &-buttons{
      margin-top: 1.5rem;
    }
    &-button{
      padding: .8rem 0 0 1.25rem;
      width: 10.1rem;
      height: 3rem;
      font-size: 1.2rem;
      line-height: 1.45rem;
      border: 1px solid rgba(#000, 0);
      &-selected{
        background: rgba(255, 255, 255, 0.05);
        border: 1px solid rgba(255, 255, 255, 0.1);
      }
    }
  } 
  &-center{
    width: 30rem;
    min-height: 30rem;
    height: 100%;
    position: relative;
  }
  &-right{
    width: 20rem;
    min-height: 30rem;
    height: 100%;
    position: relative;
  }
}
</style>
