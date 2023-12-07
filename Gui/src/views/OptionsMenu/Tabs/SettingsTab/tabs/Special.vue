<template>
  <div class="settings-special">
      <div class="settings-special-tittle">{{loc("mmain:setts:spec:tit")}}</div>
      <div class="settings-special-subtittle">{{loc("mmain:setts:spec:subtit")}}</div>
      <div class="settings-special-info">
          {{loc("mmain:setts:spec:desc")}}
          
      </div>
      <SettingsSwitch :status="settings.noMicro" class="settings-special-switch"  @onSwitch="setSettingLocal($event, 'noMicro')"/>
      <div class="settings-special-hr"></div>
      <div class="settings-special-lang">
          <div class="settings-special-lang-tittle">{{loc("mmain:setts:spec:lang")}}</div>
          <div class="settings-special-lang-subtittle">{{loc("mmain:setts:spec:lang2")}}</div>
          <SettingsLang :langs="langs" @onSelect="setLang"/>
      </div>
  </div>
</template>

<script>
import SettingsSwitch from '../SettingsSwitch'
import SettingsLang from '../SettingsLang'
import {mapState, mapMutations, mapGetters, mapActions} from 'vuex'

export default {
  name: 'Special',
  computed:{
      ...mapState('optionsMenu', ['settings']),
      ...mapState('localization', ['langs']),
      ...mapGetters('localization', ['loc'])
  },
  methods: {
      ...mapMutations('optionsMenu', ['setSetting']),      
      ...mapActions('localization', ['setLang']),
      setSettingLocal(status, name){
          this.setSetting({name, status})
      }
  },
  components:{
      SettingsSwitch,
      SettingsLang
  }
}
</script>

<style lang="scss" scoped>
    .settings-special{
        letter-spacing: 0.03em;
        &-tittle{
            font-size: 2.5rem;
            line-height: 3rem;
        }
        &-subtittle{
            font-size: 1.5rem;
            line-height: 1.5rem;
            margin: 1rem 0;
            margin-right: 2rem;
        }
        &-info{
            font-size: .9rem;
            line-height: 1.2rem;    
            color: rgba(#fff, .5);        
        }
        &-switch{
            margin: 1rem 0;
        }
        &-hr{
            background: rgba(255, 255, 255, 0.15);
            transform: matrix(1, 0, 0, -1, 0, 0);
            height: 2px;
            margin: 1.25rem 0;
        }
        &-lang{
           &-tittle{
               font-size: 1.5rem;
               line-height: 1.5rem;
           }
           &-subtittle{
               font-size: .9rem;
               line-height: 1.2rem;
               color: rgba(#fff, .5); 
           }
        }
    }
</style>
