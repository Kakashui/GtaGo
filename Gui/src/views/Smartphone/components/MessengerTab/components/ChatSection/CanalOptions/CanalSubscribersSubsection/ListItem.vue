<template>
  <div class="list-item">
    <transition name="slide-in">
      <div 
        class="list-item__btn-delete"
        v-if="edit" 
        @click="$emit('delete', item.id)"
      ></div>
    </transition>
    <div 
      class="list-item__avatar"
      v-bind:class="{avatarOnline : item.isOnline === true}"
    ></div>
    <div class="list-item__info" @click="$emit('get-profile', item.id)">
      <div class="nickname">{{item.nickname}}</div>
      <div
        class="status"
        v-if="item.isOnline === true"
      >Online</div>
      <div 
        class="status"
        v-else
      >{{lastVisitDate}}</div>
    </div>
  </div>
</template>

<script>
export default {
  name: 'ListItem',

  props: {
    item: Object,
    edit: Boolean
  },

  computed: {
    lastVisitDate: function() {
      const receivedDate = new Date(this.$props.item.entryTime)
      const currentDate = new Date()
      let rightDate = ''

      if (receivedDate.toLocaleDateString() === currentDate.toLocaleDateString()) {
        rightDate = receivedDate.toLocaleTimeString(undefined, { hour: 'numeric', minute: 'numeric' })
      } else {
        rightDate = receivedDate.toLocaleDateString()
      }

      return rightDate
    }
  }
}
</script>

<style lang="scss" scoped>
.list-item{
  display: flex;
  align-items: center;
  justify-content: flex-start;
  &:last-child{
    .list-item__info{
      border-bottom: 0;
    }
  }
  &__btn-delete{
    width: 1rem;
    height: 1rem;
    min-width: 1rem;
    min-height: 1rem;
    margin-right: .5rem;
    background-image: url('/img/hud/smartphone/messengerTab/chatSection/canalOptions/minus.svg');
    background-size: contain;
    background-repeat: no-repeat;
    background-position: center;
  }
  &__avatar{
    margin-right: .5rem;
    width: 2rem;
    height: 2rem;
    min-width: 2rem;
    min-height: 2rem;
    position: relative;
    background-image: url('/img/hud/smartphone/messengerTab/chatSection/canalOptions/avatar.png');
    background-size: contain;
    background-repeat: no-repeat;
    background-position: center;
    &.avatarOnline{
      &:after{
        position: absolute;
        right: 1px;
        top: 1px;
        content: '';
        width: .6rem;
        height: .6rem;
        box-sizing: border-box;
        border-radius: 50%;
        background: #4BDB4B;
        border: .1rem solid #FFFFFF;
      }
    }
  }
  &__info{
    width: 100%;
    display: flex;
    flex-flow: column;
    align-items: flex-start;
    justify-content: center;
    padding: .5rem 0;
    border-bottom: 1px solid rgba(195, 195, 195, 0.35);
    .nickname{
      font-weight: normal;
      font-size: .8rem;
      line-height: .95rem;
      color: #020202;
      margin-bottom: .25rem;
      width: 10.1rem;
      overflow: hidden;
      white-space: nowrap;
      text-overflow: ellipsis;
    }
    .status{
      font-size: .65rem;
      line-height: .8rem;
      color: #9E9EA2;
    }
  }
}
.slide-in-enter-active, .slide-in-leave-active {
  transition: all .2s;
}
.slide-in-enter, .slide-in-leave-to {
  transform: translate(-100%, 0) scale(0);
  margin: 0 0 0 -1rem;
}
</style>