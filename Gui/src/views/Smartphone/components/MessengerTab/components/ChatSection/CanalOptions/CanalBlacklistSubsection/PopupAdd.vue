<template>
  <div class="popup-add">
    <div class="popup-add__header">
      <button class="cancel" @click="$emit('close')">{{loc('phone:cancel')}}</button>
      <span class="title">{{loc('phone:chat:pop:2')}}</span>
      <p class="about">{{loc('phone:chat:pop:3')}}</p>
    </div>
    <div class="popup-add__search">
      <search-label :value="searchQuery" @input="handleInputSearch($event)" />
    </div>
    <div v-bar>
      <div class="popup-add__list">
        <list-item
          v-for="(item, index) in items"
          :key="index"
          :item="item"
          @event="blockUser(item.Id)"
        />
      </div>
    </div>
  </div>
</template>

<script>
import { mapState, mapMutations, mapGetters } from 'vuex'
import SearchLabel from '../common/SearchLabel'
import ListItem from '../common/ListItem'

export default {
  name: 'PopupAdd',

  components: {
    SearchLabel,
    ListItem
  },

  props: { setTab: Function },

  data: function() {
    return {
      searchQuery: '',
      items: [
        // {
        //   id: 1,
        //   nickname: 'Owner name',
        //   isOnline: true,
        //   entryTime: 1603005132000
        // },
      ]
    }
  },

  watch: {
    messengerData: {
      handler: function() {
        this.items = this.messengerData.SearchSubscribers;
      },
      deep: true
    }
  },

  computed: {
    ...mapState('smartphone', ['currentTab']),
    ...mapState('smartphone/messagePage', ['messengerData']),
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    ...mapMutations('smartphone', ['setCurrentTab']),

    blockUser: function(id) {
      window.mp.triggerServer('phone::msg::manage::blockUser', this.messengerData.CurrentChat.Id, id);
      this.$emit('close');
    },

    handleInputSearch: function(text) {
      this.searchQuery = text;

      if (!this.searchTimeout) {
        this.searchTimeout = setTimeout(this.sendRequestQuery, 1000);
      }
    },

    sendRequestQuery: function() {
      window.mp.triggerServer('phone::msg::requestSubscribers', this.messengerData.CurrentChat.Id, this.searchQuery);
      delete this.searchTimeout;
    },
  },

  beforeDestroy: function() {
    if (this.searchTimeout)
    {
      clearTimeout(this.searchTimeout);
      delete this.searchTimeout;
    }
  }
}
</script>

<style lang="scss" scoped>
.popup-add {
  position: absolute;
  bottom: 0;
  left: 0;
  width: 100%;
  height: 31rem;
  border-top-left-radius: 0.6rem;
  border-top-right-radius: 0.6rem;
  background: #FAFAFA;
  &__header {
    padding: 1rem 1rem 0 1rem;
    display: flex;
    flex-direction: column;
    align-items: flex-start;
    .cancel {
      background: transparent;
      color: #4E6ED9;
      font-size: 0.7rem;
      &:hover {
        text-decoration: underline;
      }
    }
    .title {
      color: #020202;
      line-height: 1.15rem;
      font-weight: bold;
      margin: 0.7rem 0 0 0;
    }
    .about {
      font-size: 0.65rem;
      line-height: 0.8rem;
      color: #8A8A8E;
      margin: 0.4rem 0 0 0;
    }
  }
  &__search {
    margin: 0.75rem 0 0.5rem 0;
    display: flex;
    justify-content: center;
  }
  &__list {
    padding: 0 0 0 0.75rem;
    height: 22.25rem !important;
    border-top: solid 1px rgba(195, 195, 195, 0.35);
  }
}
</style>
