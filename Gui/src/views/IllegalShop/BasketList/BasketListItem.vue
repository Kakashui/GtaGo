<template>
  <div class="basket-list-item">
    <div
      class="img"
      :style="{backgroundImage: 'url(/img/illegalShop/items/' + item.product + '.png)'}"
    ></div>
    <div class="title">{{loc(item.name)}}</div>
    <div class="count">
      <div 
        class="btn-prev"
        @click="changeInQuantityDown"
      ></div>
      <div class="value">{{item.count}}</div>
      <div 
        class="btn-next"
        @click="changeInQuantityUp"
      ></div>
    </div>
    <div class="cost">${{item.cost*item.count}}</div>
    <div class="btn-delete" @click="deleteItem({key: index})"></div>
  </div>
</template>

<script>
import { mapMutations, mapGetters } from 'vuex'
export default {
  name: 'BasketListItem',

  props: {
    item: Object,
    index: Number
  },

  computed: {
    ...mapGetters('localization', ['loc']),
  },

  methods: {
    ...mapMutations('illegalShop', ['deleteItem']),
    changeInQuantityDown: function() {
      if (this.item.count > 1) {
        this.item.count -= 1
      }
    },
    changeInQuantityUp: function() {
      this.item.count += 1
    },
  }
}
</script>

<style lang="scss" scoped>
.basket-list-item{
  display: flex;
  align-items: center;
  background: rgba(30, 45, 53, 0.4);
  border-radius: 1rem;
  padding: 0 1.5rem;
  font-weight: bold;
  position: relative;
  overflow: hidden;
  transition: .2s;
  &:before, &:after{
    position: absolute;
    top: 0;
    left: 0;
    content: '';
    width: 100%;
    height: 100%;
    z-index: 1;
    transition: .2s;
  }
  &:before{
    background: linear-gradient(180deg, rgba(30, 45, 53, 0) 39.06%, rgba(30, 45, 53, 0.432) 72.92%, rgba(30, 45, 53, 0.6) 100%);
    opacity: 1;
  }
  &:after{
    background: linear-gradient(180deg, rgba(20, 26, 0, 0) 25.91%, rgba(148, 192, 0, 0.4) 100%);
    opacity: 0;
  }
  &>div{
    z-index: 2;
  }
  &:hover{
    background: rgba(148, 192, 0, 0.1);
    transition: .2s;
    &:before{
      opacity: 0;
      transition: .2s;
    }
    &:after{
      opacity: 1;
      transition: .2s;
    }
    .count{
      .btn-prev, .btn-next{
        background-image: url('/img/illegalShop/arrow.svg');
        transition: .2s;
      }
    }
    .btn-delete{
      background-image: url('/img/illegalShop/delete-hover.svg');
      transition: .2s;
    }
  }
  .img{
    width: 4.5rem;
    height: 100%;
    background-size: cover;
    background-position: top;
    margin-right: 3rem;
  }
  .title{
    font-size: .7rem;
    width: 8rem;
  }
  .count{
    display: flex;
    align-items: center;
    justify-content: center;
    .value{
      font-size: .9rem;
      background: rgba(255, 255, 255, 0.05);
      box-shadow: inset 0 0 .5rem 1px rgba(0, 0, 0, 0.1);
      border-radius: .5rem;
      width: 2.7rem;
      height: 2.05rem;
      display: flex;
      align-items: center;
      justify-content: center;
    }
    .btn-prev, .btn-next{
      background-image: url('/img/illegalShop/arrow-basket.svg');
      background-size: contain;
      background-repeat: no-repeat;
      background-position: center;
      width: 1.5rem;
      height: 1.5rem;
      transition: .2s;
    }
    .btn-next{
      transform-origin: 50% 50%;
      transform: rotate(180deg);
    }
  }
  .cost{
    margin-left: 3rem;
    width: 6rem;
  }
  .btn-delete{
    width: 1rem;
    height: 1rem;
    background-image: url('/img/illegalShop/delete.svg');
    background-size: contain;
    background-repeat: no-repeat;
    background-position: center;
    transition: .2s;
  }
}
</style>
