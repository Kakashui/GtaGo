<template>
  <div class="keys-list">
    <div
      :class="[{saved: item.animation !== null}, {selected: item.key === currentKey}, 'keys-list-slot']"
      v-for="item in quickAccess"
      :key="item.id"
      @click="setCurrentKey(item.key)"
    >
      <div class="icon">{{item.key}}</div>
      <div 
        class="text"
        v-if="item.animation != null"
      >{{loc(item.animationName)}}</div>
      <div 
        v-else 
        class="text"
      >{{loc('AnimationsMenu_2')}}</div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapMutations, mapState } from 'vuex'
export default {
  name: 'KeysList',

  computed: {
    ...mapState('animationsMenu', ['quickAccess', 'currentKey']),
    ...mapGetters('localization',['loc']),
  },
  
  methods: {
    ...mapMutations('animationsMenu', ['setCurrentKey'])
  }
}
</script>

<style lang="scss" scoped>
.keys-list{
  display: flex;
  align-items: center;
  justify-content: flex-start;
  &-slot{
    display: flex;
    align-items: center;
    justify-content: center;
    margin-right: 1rem;
    opacity: .65;
    min-width: 10rem;
    &.selected, &.saved{
      opacity: 1;
    }
    &.saved{
      .icon{
        background: rgba(193, 231, 4, 0.15);
        box-shadow: 0 .6rem 1.1rem rgba(0, 0, 0, 0.35);
      }
    }
    &.selected{
      .icon{
        background: rgba(231, 4, 4, 0.15);
        box-shadow: 0 .6rem 1.1rem rgba(0, 0, 0, 0.35);
      }
    }
    .icon{
      margin-right: .95rem;
      min-width: 2.6rem;
      width: 2.6rem;
      height: 2.6rem;
      padding: .5rem;
      border-radius: .5rem;
      background: rgba(255, 255, 255, 0.15);
      box-shadow: 0 .6rem 1.1rem rgba(0, 0, 0, 0.35);
      border: 1px solid rgba(255, 255, 255, 0.15);
      color: rgba(255, 255, 255, 0.65);
    }
    .text{
      width: 7rem;
      min-width: 7rem;
      max-width: 7rem;
      text-align: left;
    }
  }
}
</style>
