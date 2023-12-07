<template>
  <div class="first-subsection">
    <div class="first-subsection__header">
      <button class="btn-back" @click="setCurrentTab({ key: currentTab.key, section: currentTab.section, subsection: 'GeneralSubsection' })">
        <img src="img/hud/smartphone/messengerTab/chatSection/chevronLeftDark.svg" class="btn-back__icon">
        <span class="btn-back__text">{{loc('phone:back')}}</span>
      </button>
    </div>
    <span class="first-subsection__title">{{loc('phone:group:first:1')}}</span>
    <p class="first-subsection__desc">{{loc('phone:group:first:2')}}</p>
    <search-label
      :value="searchQuery"
      @input="searchQuery = $event"
      :styles="{ margin: '0 auto 0.5rem auto' }"
    />
    <div v-bar>
      <div class="first-subsection__list">
        <list-item
          v-for="(item, index) in filteredItems"
          :key="index"
          :item="item"
        />
      </div>
    </div>
    <button class="first-subsection__btn" @click="setCurrentTab({ key: currentTab.key, section: currentTab.section, subsection: 'GroupSecondSubsection', data: { users: invitations } })">
      <span class="text" >{{loc('phone:group:first:3')}}</span>
      <span class="count" v-if="invitations.length">({{ invitations.length }})</span>
    </button>
  </div>
</template>

<script>
import { mapState, mapMutations, mapGetters } from 'vuex'
import SearchLabel from '../../ChatSection/CanalOptions/common/SearchLabel'
import ListItem from './ListItem'

export default {
  name: 'FirstSubsection',

  components: {
    SearchLabel,
    ListItem
  },

  data: function() {
    return {
      searchQuery: '',
      currentItems: []
    }
  },

  computed: {
    ...mapState('smartphone', ['currentTab']),
    ...mapState('smartphone/messagePage', ['messengerData']),
    ...mapGetters('localization', ['loc']),

    filteredItems: function() {
      const query = this.searchQuery.trim()
      if(query.length > 2) {
        return this.currentItems.filter(item => {
          return item.Name.toLowerCase().includes(query.toLowerCase())
        })
      } else {
        return this.currentItems
      }
    },

    invitations: function() {
      return this.currentItems.filter(item => item.isMarked)
    },
  },

  methods: {
    ...mapMutations('smartphone', ['setCurrentTab']),

    setCurrentItems: function(items) {
      this.currentItems = items
    }
  },

  mounted: function() {
    const rightItems = this.messengerData.Contacts.map(item => {
      const rightItem = { ...item, isMarked: false }
      return rightItem
    })
    this.setCurrentItems(rightItems)
  }
}
</script>

<style lang="scss" scoped>
@keyframes shake {
  25% {
    transform: translateX(0.5rem);
  }
  50% {
    transform: translateX(0);
  }
  75% {
    transform: translateX(0.25rem);
  }
}
.first-subsection {
  padding: 2.5rem 0 0 0;
  display: flex;
  flex-direction: column;
  width: 100%;
  height: 100%;
  position: relative;
  &__header {
    padding: 0 1rem;
    margin: 0 0 1rem 0;
    .btn-back {
      display: flex;
      align-items: center;
      background: transparent;
      &__icon {
        width: 0.6rem;
        height: auto;
        margin: 0 0.4rem 0 0;
      }
      &__text {
        color: #4E6ED9;
        font-size: 0.7rem;
      }
      &:hover .btn-back__icon {
        animation: shake 0.4s;
      }
    }
  }
  &__title {
    line-height: 1.15rem;
    color: #020202;
    font-weight: bold;
    margin: 0 0 0.4rem 0;
    padding: 0 0 0 1rem;
  }
  &__desc {
    font-size: 0.65rem;
    line-height: 0.8rem;
    color: #8A8A8E;
    padding: 0 1rem;
    margin: 0 0 0.75rem 0;
  }
  &__list {
    border-top: rgba(195, 195, 195, 0.35) 1px solid;
    padding: 0 0 0 0.75rem;
    height: 22.8rem !important;
  }
  &__btn {
    position: absolute;
    transform: translate(-50%, 0);
    bottom: 1.5rem;
    left: 50%;
    width: 13.2rem;
    height: 2rem;
    border-radius: 5rem;
    background: #4E6ED9;
    transition: all 0.2s;
    font-size: 0.7rem;
    line-height: 0.8rem;
    color: #FFFFFF;
    box-shadow: 0px 0.5rem 1.5rem rgba(78, 110, 216, 0.3);
    &:hover {
      box-shadow: none;
    }
  }
}
</style>
