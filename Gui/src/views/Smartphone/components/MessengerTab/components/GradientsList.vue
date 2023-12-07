<template>
  <transition name="smartphone-scale-in">
    <div :style="styles" class="gradients-list">
      <div 
        class="gradients-list__color" 
        v-for="(item, index) in pallette"
        :key="index"
        v-bind:style="{ background: item }"
        @click="$emit('set-gradient', item)"
      ></div>
    </div>
  </transition>
</template>

<script>
import { mapState } from 'vuex'
export default {
  props: {
    styles: Object
  },

  data: function() {
    return {
      name: 'GradientsList',

      pallette: [
        'linear-gradient(90deg, #D4FC79 0%, #96E6A1 100%)',
        'linear-gradient(90deg, #A1C4FD 0%, #C2E9FB 100%)',
        'linear-gradient(90deg, #FAD0C4 0%, #FAD0C4 1%, #FFD1FF 100%)',
        'linear-gradient(90deg, #FF9A9E 0%, #FECFEF 99%, #FECFEF 100%)',
        'linear-gradient(90deg, #43E97B 0%, #38F9D7 100%)',
        'linear-gradient(90deg, #A18CD1 0%, #FBC2EB 100%)',
        'linear-gradient(90deg, #84FAB0 0%, #8FD3F4 100%)',
        'linear-gradient(90deg, #A1C4FD 0%, #C2E9FB 100%)',
        'linear-gradient(90deg, #FAD0C4 0%, #FAD0C4 1%, #FFD1FF 100%)',
        'linear-gradient(90deg, #FF9A9E 0%, #FECFEF 99%, #FECFEF 100%)',
        'linear-gradient(90deg, #43E97B 0%, #38F9D7 100%)',
        'linear-gradient(90deg, #A18CD1 0%, #FBC2EB 100%)',
        'linear-gradient(90deg, #84FAB0 0%, #8FD3F4 100%)',
        'linear-gradient(90deg, #D4FC79 0%, #96E6A1 100%)',
        'linear-gradient(90deg, #D4FC79 0%, #96E6A1 100%)',
        'linear-gradient(90deg, #A1C4FD 0%, #C2E9FB 100%)',
        'linear-gradient(90deg, #FAD0C4 0%, #FAD0C4 1%, #FFD1FF 100%)',
        'linear-gradient(90deg, #FF9A9E 0%, #FECFEF 99%, #FECFEF 100%)',
        'linear-gradient(90deg, #43E97B 0%, #38F9D7 100%)',
        'linear-gradient(90deg, #A18CD1 0%, #FBC2EB 100%)',
        'linear-gradient(90deg, #84FAB0 0%, #8FD3F4 100%)',
        'linear-gradient(90deg, #A1C4FD 0%, #C2E9FB 100%)',
        'linear-gradient(90deg, #FAD0C4 0%, #FAD0C4 1%, #FFD1FF 100%)',
        'linear-gradient(90deg, #FF9A9E 0%, #FECFEF 99%, #FECFEF 100%)',
        'linear-gradient(90deg, #43E97B 0%, #38F9D7 100%)',
        'linear-gradient(90deg, #A18CD1 0%, #FBC2EB 100%)',
        'linear-gradient(90deg, #84FAB0 0%, #8FD3F4 100%)',
        'linear-gradient(90deg, #D4FC79 0%, #96E6A1 100%)'
      ],
    }
  },
  computed: {
    ...mapState('smartphone', ['currentGradient'])
  }
}
</script>

<style lang="scss" scoped>
.gradients-list{
  display: grid;
  position: absolute;
  // top: 7.65rem;
  background: #fff;
  box-shadow: 0 .5rem 1.5rem rgba(177, 188, 203, 0.5);
  border-radius: .5rem;
  padding: .75rem;
  grid-template-columns: repeat(7, 1.4rem);
  grid-auto-rows: 1.4rem;
  grid-gap: .4rem;
  z-index: 1;
  transition: all 0.15s;
  &__color{
    border-radius: 50%;
    &:hover {
      box-shadow: 0 .25rem 0.75rem rgba(0, 0, 0, 0.2);
    }
  }
}
</style>
