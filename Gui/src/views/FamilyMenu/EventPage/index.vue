<template>
  <div class="event-page">

    
        <transition name="fading">
        <div v-if="showNavElem != 'BattleResults'" class="event-page__nav">
          <div @mouseover="hoveredElem = 0" @mouseleave="hoveredElem = -1" :style="{ color: showNavElem == 'PrivateEvents' ? '#DCFF00' : hoveredElem == 0 ? '#DCFF00' : '#FFF' }" @click="toggleNavElem('PrivateEvents')" :class="[{active: showNavElem === 'PrivateEvents'}, 'nav-element']">
            {{loc('fam:menu:events:priv')}}
          </div>
          <div @mouseover="hoveredElem = 1" @mouseleave="hoveredElem = -1" :style="{ color: showNavElem == 'GeneralEvents' ? '#DCFF00' : hoveredElem == 1 ? '#DCFF00' : '#FFF' }" @click="toggleNavElem('GeneralEvents')" :class="[{active: showNavElem === 'GeneralEvents'}, 'nav-element']">
            {{loc('fam:menu:events:comm')}}
          </div>
        </div>
        </transition>

        <GeneralEvents />
        <PrivateEvents />
    
    
  
  
    <battle-results v-if="showBattleResults" />
 
  
  </div>
</template>

<script>
import { mapState, mapMutations, mapGetters } from 'vuex'

import BattleResults from './views/BattleResults'
import GeneralEvents from './views/GeneralEvents'
import PrivateEvents from './views/PrivateEvents'

export default {
  name: 'EventPage',
  data() {
    return {
      navElem: 'GeneralEvents',
      hoveredElem: -1
    }
  },
  components: {
    PrivateEvents,
    GeneralEvents,
    BattleResults
  },
  computed:{
    ...mapState('familyMenu/eventsPage', [
      'showBattleResults',
      'showNavElem'
    ]),
    ...mapGetters('localization', ['loc'])
  },
  methods: {
    ...mapMutations('familyMenu/eventsPage', [
      'toggleNavElem'
    ])
  },
  mounted() {

  }
}

</script>

<style lang="scss" scoped>

// anim for dummy
@keyframes isShow1{
  0%{
    opacity: 0;
  }
  25%{
    opacity: 1;
  }
  100%{
    opacity: 1;
  }
}
@keyframes isShow2{
  0%{
    opacity: 0;
  }
  25%{
    opacity: 0;
  }
  50%{
    opacity: 0;
  }
  100%{
    opacity: 1;
  }
}

.fading-enter-active, .fading-leave-active {
  transition: all .3s ease;
}
.fading-enter, .fading-leave-to {
  opacity: 0;
}

.event-page {
  width: 100%;
  &__nav {
    display: flex;
    margin-top: 2rem;
    .nav-element {
      font-weight: 300;
      font-size: 1.25rem;
      letter-spacing: 0.18em;

      transition: color 0.2s linear;

      margin-right: 2.5rem;
      margin-bottom: 2rem;

      height: 1.75rem;

      position: relative;

      &.active{
        @keyframes showing {
          0% {
            width: 0%;
          }
          100% {
            width: 100%;
          }
        }

        &::after {
          position: absolute;
          bottom: 0;
          left: 0;
          content: "";
          height: 0.1rem;
          width: 100%;
          background: #DCFF00;
          transition: 0.2s;
          animation: showing 0.3s;
        }
      }
    }
  }
}

</style>