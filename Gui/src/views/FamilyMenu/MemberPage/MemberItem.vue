<template>
  <div 
    class="member-item"
  >
    <div class="member-item__value">
      <span>{{index + 1}}</span>
      <div 
        class="status status-online"
        v-if="item.online"
      >online</div>
      <div 
        class="status"
        v-else
      >offline</div>
    </div>
    <div class="member-item__value">{{item.nickname}}</div>
    <div class="member-item__value">{{item.id}}</div>
    <div class="member-item__value">+{{item.up}}</div>
    <div class="member-item__value">{{item.down}}</div>
    <div class="member-item__value">{{item.rating}}</div>
  </div>
</template>

<script>
import { mapState } from 'vuex'
export default {
  name: 'MemberItem',

  props: {
    item: Object,
    index: Number
  },

  computed: {
    ...mapState('familyMenu', ['currentMemberId']),
  },

  mounted(){
    if (this.item.id === this.currentMemberId) {
      this.$emit('ratingFound', this.index)
    }
  }
}
</script>

<style lang="scss" scoped>
.member-item{
  display: flex;
  align-items: center;
  width: 54.7rem;
  height: 4rem;
  min-height: 4rem;
  font-size: 1.1rem;
  background: rgba(255, 255, 255, 0.04);
  &__value{
    display: flex;
    align-items: center;
    justify-content: center;
    width: 100%;
    height: 100%;
    position: relative;
    &:after{
      content: '';
      width: .1rem;
      height: 100%;
      position: absolute;
      right: 0;
      top: 0;
      background: linear-gradient(270deg, rgba(255, 255, 255, 0.141) 0%, rgba(0, 0, 0, 0.159) 100%);
    }
    &:before{
      content: '';
      height: .1rem;
      width: 100%;
      position: absolute;
      left: 0;
      bottom: 0;
      background: linear-gradient(180deg, rgba(255, 255, 255, 0.141) 0%, rgba(0, 0, 0, 0.159) 100%);
    }
    &:nth-child(1), &:nth-child(2){
      justify-content: flex-start;
    }
    &:nth-child(1){
      width: 8.7rem;
      min-width: 8.7rem;
      padding-left: 5.1rem;
      &:after{
        content: none;
      }
    }
    &:nth-child(2){
      width: 16.8rem;
      min-width: 16.8rem;
    }
    &:nth-child(3){
      width: 8.55rem;
      min-width: 8.55rem;
    }
    &:nth-child(4){
      color: #DCFF00;
    }
    &:nth-child(5){
      color: #FF3838;
    }
    &:last-child{
      width: 7.95rem;
      min-width: 7.95rem;
      &:after{
        content: none;
      }
    }
    span{
      position: absolute;
      left: 2rem;
    }
    .status{
      position: relative;
      display: flex;
      align-items: center;
      &:before{
        content: '';
        position: absolute;
        width: .3rem;
        height: .3rem;
        border-radius: 50%;
        left: -.3rem;
        background-color: rgba(255, 255, 255, 0.3);
        transform: translateX(-100%);
      }
      &.status-online{
        &::before{
          background-color: #DCFF00;;
        }
      }
    }
  }
}
</style>
