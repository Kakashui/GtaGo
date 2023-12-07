<template>
  <div class="edit-window">
    <div class="edit-window__title">{{loc('news:edit:tit')}}</div>
    <Decorate v-if="currentAdIsEmpty"/>
    <EditAd v-else/>
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
import Decorate from './Decorate'
import EditAd from './EditAd'
export default {
  name: 'EditWindow',

  components: {
    Decorate,
    EditAd
  },

  computed: {
    ...mapState('news', ['currentAd']),
    ...mapGetters('localization', ['loc']),
    currentAdIsEmpty() {
      for (let key in this.currentAd) {
        return false;
      }
      return true;
    }
  }
}
</script>

<style lang="scss" scoped>
.edit-window{
  width: 17.6rem;
  min-width: 17.6rem;
  background-color: #fff;
  height: 100%;
  padding: 1.2rem 0 1rem 0;
  position: relative;
  &__title{
    font-weight: bold;
    font-size: 1.4rem;
    line-height: 1.6rem;
    color: #212121;
    margin-bottom: 2.75rem;
    width: 100%;
    text-align: center;
  }
}
</style>
