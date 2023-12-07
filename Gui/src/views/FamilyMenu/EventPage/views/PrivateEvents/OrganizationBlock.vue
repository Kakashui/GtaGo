<template>
  <div class="organization-block">
    <div class="title">{{loc('fam:menu:org:tit')}}</div>
    <div class="desc">{{loc('fam:menu:org:desc')}}.</div>
    <div class="nav-list">
      <div 
        :class="[{active: item.key === currentChoice}, 'nav-list__item']"
        v-for="item in choiceList"
        :key="item.id"
        @click="setCurrentChoice(item.key)"
      >{{loc(item.text)}}</div>
    </div>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
export default {
  name: 'OrganizationBlock',
  computed:{
    ...mapGetters('localization', ['loc'])
  },
  props: {
    setCurrentChoice: Function,
    choiceList: Array,
    currentChoice: String
  }

}
</script>

<style lang="scss" scoped>
.organization-block{
  display: flex;
  flex-flow: column;
  width: 16.6rem;
  min-width: 16.6rem;
  margin-right: 4.3rem;
  .title{
    font-size: 3rem;
    line-height: 3.6rem;
    margin-bottom: 1rem;
  }
  .desc{
    font-size: .9rem;
    line-height: 1.2rem;
    letter-spacing: 0.03em;
    color: rgba(255, 255, 255, 0.5);
    margin-bottom: 2rem;
  }
  .nav-list{
    display: flex;
    flex-flow: column;
    width: 10.75rem;
    &__item{
      height: 3rem;
      min-height: 3rem;
      display: flex;
      align-items: center;
      justify-content: flex-start;
      padding: 0 1.2rem;
      font-size: 1.2rem;
      font-weight: 300;
      line-height: 1.45rem;
      letter-spacing: 0.03em;
      color: rgba(255, 255, 255, 0.5);
      transition: .2s;
      border: 1px solid rgba(52, 53, 57,0);
      &:hover, &.active{
        color: rgba(255, 255, 255, 1);
        transition: .2s;
      }
      &.active{
        background: #222428;
        border: 1px solid rgba(52, 53, 57, 1);
        transition: .2s;
      }
    }
  }
}
</style>
