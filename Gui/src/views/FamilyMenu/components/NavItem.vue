<template>
  <div 
    :class="[{active: item.key === currentPage}, 'navigation-item']"
    @click="setCurrentPage(item.key)"
  >{{loc(item.text)}}</div>
</template>

<script>
import { mapGetters, mapMutations, mapState } from 'vuex'
export default {
  name: 'NavItem',
  
  props: {
    item: Object
  },

  computed: {
    ...mapState('familyMenu', ['currentPage']),
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    ...mapMutations('familyMenu', ['setCurrentPage'])
  }
}
</script>

<style lang="scss" scoped>
.navigation-item{
 text-transform: uppercase;
  font-weight: 400;
  font-size: 1.2rem;
  line-height: 1.55rem;
  padding-bottom: 1rem;
  border-bottom: .1rem solid transparent;
  white-space: pre;
  transition: .2s;
  position: relative;
  display: flex;
  align-items: center;
  justify-content: center;
  &:hover, &.active{
    border-color: #C1E704;
    transition: .2s;
  }
  &.active{
    &:after{
      position: absolute;
      bottom: .5rem;
      content: '';
      width: 100%;
      height: .3rem;
      background: rgba(220, 255, 0, 0.4);
      filter: blur(.4rem);
      transition: .2s;
    }
  }
}
</style>
