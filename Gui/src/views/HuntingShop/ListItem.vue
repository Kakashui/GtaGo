<template>
  <div class="list-item">
    <div class="img-wrap">
      <div class="img" :style="{ backgroundImage: 'url(/img/huntingShop/' + item.key + '.png'}"></div>
      <div
        class="count"
        v-if="item.amount"
      >{{ item.amount }}</div>
    </div>
    <div class="info">
      <div class="title">{{ loc(item.name) }}</div>
      <div class="price">{{ item.price }}</div>
      <div 
        class="btn"
        @click="purchaseItem(item.key)"
      >{{loc(item.btnText)}}</div>
    </div>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
export default {
  name: 'ListItem',

  props: {
    item: Object
  },

  computed: {
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    purchaseItem: function(key) {
      window.mp.trigger('huntingStore::select', key)
    }
  }
}
</script>

<style lang="scss" scoped>
.list-item{
  margin-bottom: 3rem;
  display: flex;
  align-items: center;
  justify-content: center;
  &:last-child{
    margin-bottom: 0;
  }
  .img-wrap{
    width: 7.2rem;
    height: 6.75rem;
    margin-right: 2.5rem;
    background-image: url('/img/huntingShop/bg-item.png');
    background-size: contain;
    background-position: center;
    display: flex;
    align-items: center;
    justify-content: center;
    position: relative;
    .img{
      position: absolute;
      width: 7rem;
      height: 7rem;
      background-size: contain;
      background-repeat: no-repeat;
      background-position: center;
    }
    .count{
      width: 2.1rem;
      height: 2.1rem;
      position: absolute;
      right: -.2rem;
      bottom: .5rem;
      background: #AA7A5A;
      font-size: 1.2rem;
      display: flex;
      align-items: center;
      justify-content: center;
    }
  }
  .info{
    display: flex;
    flex-flow: column;
    align-items: flex-start;
    justify-content: space-between;
    height: 100%;
    .title{
      font-size: 1.2rem;
    }
    .price{
      font-size: 1rem;
      color: #C1E704;
      &:before{
        content: '$ ';
        white-space: pre;
      }
    }
    .btn{
      font-size: 1.2rem;
      display: flex;
      align-items: center;
      justify-content: center;
      width: 6.95rem;
      height: 3.15rem;
      min-height: 3.15rem;
      background: #EC8F51;  
      border: 1px solid #FFAA71;
      transition: .2s;
      &:hover{
        box-shadow: 0 .6rem .6rem -.4rem rgba(236, 143, 81, 0.45);
        transform: scale(1.1);
        transition: .2s;
      }
    }
  }
}
</style>