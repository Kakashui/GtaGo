<template>
  <div class="popup-invite">
    <div class="popup-invite__header">
      <div class="popup-invite__header-top">
        <button class="btn-cancel" @click="$emit('close')">{{loc('phone:cancel')}}</button>
        <span class="title">{{loc('phone:chat:invite:2')}}</span>
        <p class="desc">{{loc('phone:chat:invite:3')}}</p>
      </div>
      <div class="popup-invite__header-bottom">
        <search-label :value="searchQuery" @input="searchQuery = $event" />
      </div>
    </div>
    <div v-bar>
      <div class="popup-invite__list">
        <list-item
          v-for="(item, index) in filteredItems"
          :key="index"
          :item="item"
        />
      </div>
    </div>
    <button
      :disabled="!invitations.length"
      class="popup-invite__btn"
      @click="toInvite(invitations)"
    >
      <span class="text">{{loc('phone:chat:invite:4')}}</span>
      <span class="count" v-if="invitations.length">({{ invitations.length }})</span>
    </button>
  </div>
</template>

<script>
import { mapState, mapGetters } from 'vuex'
import SearchLabel from '../common/SearchLabel'
import ListItem from './PopupInviteListItem'

export default {
  name: 'PopupInvite',

  components: {
    SearchLabel,
    ListItem,
  },

  data: function() {
    return {
      searchQuery: '',
      currentItems: []
    }
  },

  computed: {
    ...mapState('smartphone/messagePage', ['messengerData']),
    ...mapGetters('localization', ['loc']),

    invitations: function() {
      return this.currentItems.filter(item => item.isMarked)
    },

    filteredItems: function() {
      const query = this.searchQuery.trim()
      if(query.length > 2) {
        return this.currentItems.filter(item => {
          return item.Name.toLowerCase().includes(query.toLowerCase())
        })
      } else {
        return this.currentItems
      }
    }
  },

  methods: {
    setCurrentItems: function(items) {
      this.currentItems = items
    },

    toInvite: function(items) {
      console.log(items.map(item => item.Id))
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
.popup-invite {
  height: 31rem;
  width: 100%;
  position: absolute;
  bottom: 0;
  left: 0;
  background: #FAFAFA;
  border-top-right-radius: 0.6rem;
  border-top-left-radius: 0.6rem;
  display: flex;
  flex-direction: column;
  &__header {
    display: flex;
    flex-direction: column;
    padding: 0 0 0.5rem 0;
    border-bottom: 1px solid rgba(195, 195, 195, 0.35);
    &-top {
      padding: 1rem 1rem 0 1rem;
      display: flex;
      flex-direction: column;
      align-items: flex-start;
      .btn-cancel {
        background: transparent;
        font-size: 0.7rem;
        line-height: 0.8rem;
        color: #4E6ED9;
        &:hover {
          text-decoration: underline;
        }
      }
      .title {
        margin: 0.7rem 0 0 0;
        line-height: 1.15rem;
        color: #020202;
        font-weight: bold;
      }
      .desc {
        margin: 0.4rem 0 0 0;
        font-size: 0.65rem;
        line-height: 0.8rem;
        color: #8A8A8E;
      }
    }
    &-bottom {
      margin: 0.75rem 0 0 0;
      height: fit-content;
      display: flex;
      justify-content: center;
    }
  }
  &__list {
    display: flex;
    flex-direction: column;
    padding: 0 0 0 0.75rem;
    height: 22.25rem;
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
    &:hover {
      box-shadow: 0px 0.5rem 1.5rem rgba(78, 110, 216, 0.3);
    }
  }
}
</style>
