<template>
  <div class="item">
    <div class="btn-delete" @click="deleteItem({key: index})"></div>
    <div class="item__image" v-bind:style="{ backgroundImage: 'url(/img/roundTheClockShop/products/'+ item.image + '_icon.png)' }"></div>
    <div class="item__quantity">
      <div class="btn btn-remove" @click="changeInQuantityDown({key: index})"></div>
      <div class="value">{{item.count}}</div>
      <div class="btn btn-add"  @click="changeInQuantityUp({key: index})"></div>
    </div>
  </div>
</template>

<script>
import { mapState, mapMutations } from 'vuex'

export default {
  name: 'BasketListItem',

  props: {
    item: Object,
    index: Number
  },

  computed: {
    ...mapState('roundTheClockShop', ['basket']),
  },

  methods: {
    ...mapMutations('roundTheClockShop', ['deleteItem', 'changeInQuantityUp', 'changeInQuantityDown']),
  }
}
</script>

<style lang="scss" scoped>
  .item{
    background: rgba(255, 255, 255, 0.06);
    border-radius: 5px;
    padding: .65rem .65rem .5rem .65rem;
    position: relative;
    display: flex;
    flex-flow: column;
    align-items: center;
    justify-content: space-between;
    .btn-delete{
      position: absolute;
      width: 1.35rem;
      height: 1.35rem;
      min-height: 1.35rem;
      right: .25rem;
      top: .25rem;
      background-image: url('/img/roundTheClockShop/btn-delete.svg');
      background-size: 100%;
      background-position: center;
      background-repeat: no-repeat;
      opacity: .85;
      &:hover{
        opacity: 1;
        transition: .3s;
      }
    }
    &__image{
      height: 3.55rem;
      width: 3rem;
      background-size: contain;
      background-repeat: no-repeat;
      background-position: center;
    }
    &__quantity{
      display: flex;
      justify-content: center;
      align-items: center;
      width: 100%;
      height: 1.6rem;
      min-height: 1.6rem;
      border-radius: 1rem;
      background: rgba(255, 255, 255, 0.1);
      position: relative;
      .value{
        font-size: .9rem;
        text-align: center;
        color: rgba(255, 255, 255, 0.7);
      }
      .btn{
        display: flex;
        align-items: center;
        justify-content: center;
        width: 1.6rem;
        height: 1.6rem;
        border-radius: 50%;
        background: rgba(255, 255, 255, 0.1);
        position: absolute;
        &:hover{
          background: rgba(255, 255, 255, 0.2);
          transition: .3s;
        }
        &:before{
          content: '';
          width: 100%;
          height: 100%;
          background-size: contain;
          background-repeat: no-repeat;
          background-position: center;
        }
        &.btn-remove{
          left: 0;
          &:before{
            background-image: url('/img/roundTheClockShop/icon-remove.svg');
          }
        }
        &.btn-add{
          right: 0;
          &:before{
            background-image: url('/img/roundTheClockShop/icon-add.svg');
          }
        }
      }
    }
  }
</style>
