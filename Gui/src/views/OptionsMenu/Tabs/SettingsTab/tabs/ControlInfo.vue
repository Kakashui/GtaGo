<template>
  <div class="settings-cinfo">
    <div class="settings-cinfo-tittle">
          {{loc('mmain:setts:ctrl:tit')}}
    </div>
    <div class="settings-cinfo-subtittle">
          {{loc('mmain:setts:ctrl:sub')}}
    </div>
    <img src="/img/optionsMenu/settings/keysbg.png" alt="keys" class="settings-cinfo-keys">
  </div>
</template>

<script>
import { mapGetters} from 'vuex'
export default {
  name: 'ControlInfo',
  computed:{
      ...mapGetters('localization', ['loc'])
  }
}
</script>

<style lang="scss" scoped>
    .settings-cinfo{
      letter-spacing: 0.03em;
        &-tittle{
            font-size: 2.5rem;
            line-height: 3rem;
        } 
        &-subtittle{
            margin-top: .75rem;
            font-size: .9rem;
            line-height: 1.2rem;
            color: rgba(#fff, .5);
        }
        &-keys{
            position: fixed;
            bottom: 0;
            right: 0;
        }
    }
</style>
