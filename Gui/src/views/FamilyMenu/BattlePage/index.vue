<template>

  <div class="battle-page">
    <modal-notify 
      v-if="modalNotify.show"
      :data="unAcceptedWars"
      @closeModalNotify="modalNotify.show = false"
    />
    <div class="battle-page__info-panel">
      <div class="info-panel-icons">
        <ul class="info-panel-list info-panel-list__icons">
          <li style="margin-right: 2.5rem;"><img src="/img/familyMenu/forward-right.svg">{{loc('fam:btl:outcom')}}</li>
          <li><img src="/img/familyMenu/forward-left.svg">{{loc('fam:btl:incom')}}</li>
        </ul>
      </div>
      <div class="info-panel-columns">
        <ul class="info-panel-list info-panel-list__columns">
          <li>{{loc('fam:btl:panel:1')}}</li>
          <li>{{loc('fam:btl:panel:2')}}</li>
          <li>{{loc('fam:btl:panel:3')}}</li>
          <li>{{loc('fam:btl:panel:4')}}</li>
          <li>{{loc('fam:btl:panel:5')}}</li>
          <li>{{loc('fam:btl:panel:6')}}</li>
        </ul>
      </div>
    </div>
    <div ref="scrollParent" class="battle-page__content">
      <div class="content-pending">
        <battle-row v-for="(item, index) in pendingWars" :key="index" :data="item" />
        <div class="shift-element"></div>
      </div>
      <div class="content-completed">
        <battle-row v-for="(item, index) in completedWars" :key="index" :data="item" />
      </div>
    </div>
    
  </div>

</template>

<script>
import BattleRow from './components/BattleRow'
import ModalNotify from './components/ModalNotify'

import { mapGetters, mapState } from 'vuex'

export default {
  name: 'BattlePage',
  data() {
    return {
      modalNotify: {
        show: true
      }
    } 
  },
  components: {
    BattleRow,
    ModalNotify
  },
  computed: {
    ...mapState('familyMenu/battlePage', [
      'pendingWars',
      'completedWars',
      'unAcceptedWars'
    ]),
    ...mapGetters('localization', ['loc'])
  },
  methods: {
    isVisible(elem) {
      let docViewTop = this.$refs.scrollParent.offsetTop,
      elemTop = elem.offsetTop;

      return (elemTop < docViewTop);
    },
  },
  created() {

  },
  mounted() {
    this.modalNotify.show = this.unAcceptedWars.length > 0;
    setTimeout(() => {
      //this.modalNotify.show = false;
    }, 500)
  }
}
</script>

<style lang="scss" scoped>
.battle-page{
  width: 100%;
  &__info-panel {
    width: 100%;
  }

  &__content {
    width: 100%;
    max-height: calc(100vh - (11.45rem));
    overflow: scroll;

    &::-webkit-scrollbar {
      display: none;
    }

    .content-pending {
      .shift-element {
        width: 100%;
        height: 1px;
        background-color: rgba(255, 255, 255, 0.2);
        margin-top: 0.5rem;
        margin-bottom: 1rem;
      }
    }
  }
}

.info-panel-icons {
  margin-top: 1.25rem;
}

.info-panel-columns {
  margin-top: 2.45rem;
  margin-bottom: 1rem;
}

.info-panel-list {
  list-style: none;
  display: flex;

  &__columns {
    margin-left: 4.8rem;
    display: grid;
    grid-template-columns: 12.1rem 11.3rem 6.85rem 4.85rem 7.7rem 2.65rem;

    & li {
     text-transform: uppercase;
      font-style: normal;
      font-weight: normal;
      font-size: 1.1rem;
      letter-spacing: 0.04em;

      color: rgba(255, 255, 255, 0.6);
    }
  }

  &__icons {
    & img {
      vertical-align: middle;
      margin-right: 0.6rem
    }
    & li {
      font-style: normal;
      font-weight: normal;
      font-size: 1.1rem;
      letter-spacing: 0.04em;
    }
  }
}

</style>
