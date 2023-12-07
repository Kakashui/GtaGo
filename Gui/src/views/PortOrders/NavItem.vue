<template>
  <div 
    :class="[{active : item.pageUrl === this.currentPage}, 'nav-item']" 
    @click="setCurrentPage(item.pageUrl)"
  >{{loc(item.name)}}
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
export default {
  name: 'NavItem',

  props: {
    item: Object,
    currentPage: String,
    setCurrentPage: Function
  },

  computed: {
    ...mapGetters('localization', ['loc']),
  }
}
</script>

<style lang="scss" scoped>
.nav-item{
  margin-right: 2.5rem;
  margin-left: 0;
  opacity: .6;
  font-size: .9rem;
  line-height: 1.15rem;
  color: #fff;
  &.active, &:hover{
    opacity: 1;
    box-shadow: none;
    text-shadow: none;
    transition: .2s;
  }
}
</style>
