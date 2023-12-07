<template>
  <div 
    :class="[{active: item.id === currentMember.id}, 'member-item']"
    @click="setCurrentMember(item)"
  >
    <div class="nickname">{{item.nickname}}</div>
    <div class="rankname">{{currentRank}}</div>
    <div 
      :class="[{'online' : item.online}, 'status-online']"
    ></div>
  </div>
</template>

<script>
import { mapState } from 'vuex'
export default {
  name: 'MemberItem',
  props: {
    item: Object,
    currentMember: Object
  },

  methods: {
    setCurrentMember: function(obj) {
      this.$emit('setCurrentMember', obj)
    }
  },

  computed: {
    ...mapState('familyMenu/controlPage', ['ranksList']),
    currentRank: function() {
      let rank = this.ranksList.find(element => element.rankId === this.item.rank)
      return rank.rankName
    }
  }
}
</script>

<style lang="scss" scoped>
.member-item{
  display: flex;
  flex-flow: column;
  align-items: flex-start;
  justify-content: space-between;
  padding: .8rem 1rem .8rem 2.2rem;
  position: relative;
  height: 4.4rem;
  min-height: 4.4rem;
  font-size: 1.2rem;
  line-height: 1.2rem;
  transform: rotateY(-180deg);
  &:after{
    content: '';
    z-index: 0;
    width: 100%;
    height: 100%;
    position: absolute;
    left: 0;
    top: 0;
    background: linear-gradient(90deg, #39393C 0%, rgba(57, 57, 60, 0) 82.76%);
    opacity: 0;
    transition: .2s;
  }
  &:hover, &.active{
    &:after{
      opacity: 1;
      transition: .2s;
    }
  }
  &>div{
    z-index: 1;
  }
  .nickname{
    font-weight: bold;
  }
  .rankname{
    font-weight: 300;
    color: rgba(255, 255, 255, 0.6);
  }
  .status-online{
    width: .3rem;
    height: .3rem;
    border-radius: 50%;
    position: absolute;
    left: 1rem;
    top: 50%;
    transform: translateY(-50%);
    background: #202226;
    &.online{
      background: #DCFF00
    }
  }
}
</style>
