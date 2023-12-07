<template>
  <div class="list-item" @click="$emit('event')">
    <div 
      class="list-item__avatar"
      v-bind:class="{ avatarOnline : item.IsOnline }"
    ></div>
    <div class="list-item__main">
      <div class="info">
        <div class="info__title">{{ item.Name }}</div>
        <div class="info__status">{{ item.IsOnline ? 'Online' : lastVisitDate }}</div>
      </div>
      <label class="label">
        <input
          type="checkbox"
          class="label__checkbox"
          v-model="item.isMarked"
        >
        <span class="label__span"></span>
      </label>
    </div>
  </div>
</template>

<script>
export default {
  name: 'ListItem',

  props: {
    item: Object
  },

  computed: {
    lastVisitDate: function() {
      const receivedDate = new Date(this.item.Time)
      const currentDate = new Date()
      let rightDate = ''

      if (receivedDate.toLocaleDateString() === currentDate.toLocaleDateString()) {
        rightDate = receivedDate.toLocaleTimeString('ru', { hour: 'numeric', minute: 'numeric' })
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
  &__main {
    width: 100%;
    display: flex;
    align-items: center;
    justify-content: space-between;
    padding: .5rem 0.75rem 0.5rem 0;
    border-bottom: 1px solid rgba(195, 195, 195, 0.35);
    .info {
      &__title {
        font-weight: normal;
        font-size: .8rem;
        line-height: .95rem;
        color: #020202;
        margin-bottom: .25rem;
        width: 9.7rem;
        overflow: hidden;
        white-space: nowrap;
        text-overflow: ellipsis;
      }
      &__status {
        font-size: .65rem;
        line-height: .8rem;
        color: #9E9EA2;
      }
    }
    .label {
      width: 1rem;
      height: 1rem;
      &__checkbox {
        display: none;
      }
      &__span {
        width: 100%;
        height: 100%;
        display: inline-block;
        border: 1px solid #9E9EA2;
        border-radius: 50%;
        position: relative;
        &::after {
          position: absolute;
          width: 100%;
          height: 100%;
          background: center / cover no-repeat url("/img/hud/smartphone/messengerTab/chatSection/canalOptions/markDone.svg");
          content: '';
          transform: scale(0);
          transition: transform 0.15s;
        }
      }
      .label__checkbox:checked + .label__span {
        border: none;
      }
      .label__checkbox:checked + .label__span::after {
        transform: scale(1);
      }
    }
  }
}
</style>
