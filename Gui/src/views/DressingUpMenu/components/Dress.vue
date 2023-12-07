<template>
    <div :class="[{ 'active' : dress.id === currentDressId }, 'dress']" @click="setCurrentDressId(dress.id)">{{ loc(dress.label) }}</div>
</template>

<script>
import { mapGetters } from 'vuex'


export default {
  
  computed: {
    ...mapGetters('localization', ['loc'])
  },

  props: {
    dress: Object,
    currentDressId: Number,
    setCurrentDressId: Function
  }
}
</script>

<style lang="scss" scoped>
  .dressing-up-menu{
    .dress{
      display: flex;
      align-items: center;
      justify-content: flex-start;
      width: 100%;
      font-size: .75rem;
      color: rgba(255, 255, 255, 0.3);
      margin-bottom: .75rem;
      height: 2.55rem;
      min-height: 2.55rem;
      background: rgba(255, 255, 255, 0.06);
      padding: 0 2rem;
      position: relative;
      cursor: pointer;
      &:before{
        content: '';
        opacity: 0;
        height: 1px;
        width: 1.5rem;
        position: absolute;
        left: .7rem;
        background: linear-gradient(90.01deg, #1C1C1C 0%, #C1E704 38.53%, #1C1C1C 85.03%);
        transform: rotate(90deg);
      }
      &:last-child{
        margin-bottom: 0;
      }
      &.active, &:hover{
        background: rgba(255, 255, 255, 0.1);
        color: #C1E704;
        letter-spacing: 0.05em;
        transition: .3s;
        &:before{
          opacity: 1;
          transition: .3s;
        }
      }
    }
  }
</style>
