<template>
  <div class="item">
    <div class="item__header">{{ item.category }}</div>
    <div class="item__contacts">
      <span
        class="contact"
        v-for="contact in item.contacts"
        :key="contact.id"
        @click="openContact(contact.id)"
      >{{ contact.name }}</span>
    </div>
  </div>
</template>

<script>
import { mapState, mapMutations } from 'vuex'

export default {
  name: 'ListItem',

  props: {
    item: Object
  },

  computed: {
    ...mapState('smartphone', ['currentTab'])
  },

  methods: {
    ...mapMutations('smartphone', ['setCurrentTab', 'setOpenedContactData']),

    openContact: function(contactId) {
      this.setOpenedContactData(contactId);
      this.setCurrentTab({ key: this.currentTab.key, section: this.currentTab.section, subsection: 'UserSubsection' });
    }
  }
}
</script>

<style lang="scss" scoped>
.item {
  display: flex;
  flex-direction: column;
  &__header {
    background: rgba(118, 118, 128, 0.12);
    padding: 0 0 0 1rem;
    font-size: .8rem;
  }
  &__contacts {
    display: flex;
    flex-direction: column;
    .contact {
      padding: .6rem .6rem .6rem 0;
      margin: 0 0 0 1rem;
      font-size: .8rem;
      border-bottom: 1px solid rgba(195, 195, 195, 0.35);
      color: #020202;
      &:hover {
        transform: translateY(-.1rem);
      }
      &:last-child {
        border: none;
      }
    }
  }
}
</style>
