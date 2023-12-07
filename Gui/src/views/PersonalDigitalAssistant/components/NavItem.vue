<template>
  <div
    :class="[{active: item.key === currentPage}, {army: type === 2}, {fbi: type === 1}, 'header-item']"
    @click="setCurrentPage(item.key)"
  >{{loc(item.text)}} <span class="count" v-if="item.key === 'HelpPage' && helpList.length > 0">{{helpList.length}}</span> </div>
</template>

<script>
import { mapGetters, mapMutations, mapState } from 'vuex'
export default {
  name: "NavItem",

  props: {
    item: Object
  },

  computed: {
    ...mapState('personalDigitalAssistant', ['currentPage', 'helpList', 'type']),
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    ...mapMutations('personalDigitalAssistant', ['setCurrentPage'])
  }
}
</script>

<style lang="scss" scoped>
.header-item{
  position: relative;
  color: #81878F;
  display: flex;
  align-items: center;
  justify-content: center;
  height: 100%;
  padding-top: .2rem;
  border-bottom: .2rem solid transparent;
  font-weight: bold;
  font-size: .8rem;
  margin-right: 3.5rem;
  transition: .2s;
  &:last-child{
    margin-right: 0;
  }
  &:hover, &.active{
    border-color: #FFD130;
  }
  &.active{
    color: #000;
    transition: .2s;
  }
  &.fbi{
    &:hover, &.active{
      border-color:  #FB7712;
    }
  }
  &.army{
    &:hover, &.active{
      border-color:  #6C8073;
    }
  }
  .count{
    background: #D01717;
    border-radius: .3rem;
    width: 1.7rem;
    height: 1.3rem;
    display: flex;
    align-items: center;
    justify-content: center;
    color: #fff;
    font-size: .8rem;
    position: absolute;
    right: -.3rem;
    transform: translateX(100%);
  }
}
</style>
