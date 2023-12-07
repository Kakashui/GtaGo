<template>
  <div class="basket-list">
    <div
      class="basket-list__item"
      v-for="(item, index) in this.basket"
      :key="item.id"
    >
      <div class="img" :style="{backgroundImage: 'url(/img/furnitureShop/items/'+ item.key + '.png)'}"></div>
      <div class="interaction">
        <div class="info">
          <div class="info__name">{{item.name}}</div>
          <div class="info__count-wrap">
            <div class="btn btn-down" @click="changeInQuantityDown({key: item.key})"></div>
            <div class="value">{{item.count}}</div>
            <div class="btn btn-up" @click="changeInQuantityUp({key: item.key})"></div>
          </div>
          <div class="info__cost">$ {{item.cost * pricePart}}</div>
        </div>
        <div class="btn btn-delete" @click="deleteItem({key: index})">{{loc('furnitureShop_19')}}</div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapMutations, mapState } from 'vuex'
export default {
  name: 'BasketPage',
  
  computed: {
    ...mapState('furnitureShop', ['basket', 'pricePart']),
    ...mapGetters('localization', ['loc']),
  },

  methods: {
    ...mapMutations('furnitureShop', ['deleteItem', 'changeInQuantityDown', 'changeInQuantityUp'])
  }
}
</script>

<style lang="scss" scoped>
.basket-list{
  display: grid;
  width: 100%;
  grid-auto-rows: 9.6rem;
  grid-row-gap: 1rem;
  &__item{
    display: flex;
    align-items: center;
    padding: 1rem 2rem 1rem 1rem;
    background: #E7EBEE;
    .img{
      height: 100%;
      width: 8.4rem;
      min-width: 8.4rem;
      background-size: contain;
      background-position: center;
      background-repeat: no-repeat;
      background-color: #fff;
      margin-right: 2rem;
      border-radius: .3rem;
    }
    .interaction{
      display: flex;
      flex-flow: column;
      width: 100%;
      justify-content: space-between;
      align-items: flex-start;
      height: 100%;
      .info{
        display: flex;
        width: 100%;
        align-items: center;
        justify-content: space-between;
        &__name{
          font-weight: bold;
          font-size: 1.2rem;
          line-height: 1.4rem;
          color: rgba(0, 0, 0, 0.5);
          white-space: pre;
        }
        &__count-wrap{
          width: 100%;
          display: flex;
          align-items: center;
          justify-content: flex-end;
          .value{
            min-width: 2.5rem;
            text-align: center;
            font-size: 1rem;
            line-height: 1.15rem;
            color: #000000;
          }
          .btn{
            width: 1.9rem;
            height: 1.9rem;
            min-width: 1.9rem;
            background: #53D3C3;
            border-radius: 50%;
            background-size: contain;
            background-position: center;
            background-repeat: no-repeat;
            &.btn-down{
              background-image: url('/img/furnitureShop/down.svg');
            }
            &.btn-up{
              background-image: url('/img/furnitureShop/up.svg');
            }
          }
        }
        &__cost{
          min-width: 10.35rem;
          font-weight: bold;
          font-size: 1.1rem;
          line-height: 1.25rem;
          color: #24486A;
          text-align: right;
        }
      }
      .btn-delete{
        font-size: 1rem;
        line-height: 1.15rem;
        color: #53D3C3;
      }
    }
  }
}
</style>
