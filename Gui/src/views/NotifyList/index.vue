<template>
  <transition-group name="list" tag="div" class="notify-list">
    <list-item
      v-for="item in items"
      :key="item.id"
      :item="item"
    />
  </transition-group>
</template>

<script>
import { mapState } from 'vuex'

import ListItem from './components/ListItem'

export default {
  name: 'App',

  components: { ListItem },
  
  computed: {
    ...mapState('notifyList', ['items'])
  }
}
</script>

<style lang="scss" scoped>
.notify-list {
  position: absolute;
  transform: translate(-50%, 0);
  top: 0;
  left: 50%;
  width: 51.2rem;
  z-index: 10;
}
.list-enter-active, .list-leave-active {
  transition: all .4s;
}
.list-enter, .list-leave-to {
  opacity: 0;
  transform: translateY(-1rem);
}
</style>
