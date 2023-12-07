<template>
  <div class="board-shop">
    <div class="board-shop__col">
      <div class="board-shop-title">{{loc('farmHouse_2')}}</div>
      <div class="board-shop__seeds">
        <ProductSeed
          v-for="seed in seedsList"
          :key="seed.id"
          :seed="seed"
          @buyProduct="buyProduct"
        />
      </div>
    </div>
    <div class="board-shop__col">
      <div class="board-shop-title board-shop-title--small">{{loc('farmHouse_3')}}</div>
      <div class="board-shop__needful">
        <ProductNeedful
          v-for="needful in needfulList"
          :key="needful.id"
          :needful="needful"
          @buyProduct="buyProduct"
        />
      </div>
      <div class="board-shop-title board-shop-title--small">{{loc('farmHouse_4')}}</div>
      <div class="board-shop__fertilizers">
        <ProductFertilizer
          v-for="fertilizer in fertilizersList"
          :key="fertilizer.id"
          :fertilizer="fertilizer"
          @buyProduct="buyProduct"
        />
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
import ProductSeed from './ProductSeed'
import ProductFertilizer from './ProductFertilizer'
import ProductNeedful from './ProductNeedful'
export default {
  name: 'BoardShop',
  components: {
    ProductSeed,
    ProductFertilizer,
    ProductNeedful
  },
  computed: {
    ...mapState('farmHouse', ['seedsList', 'fertilizersList', 'needfulList']),
    ...mapGetters('localization', ['loc'])
  },
  methods: {
    buyProduct: function(id) {
      window.mp.triggerServer('farmHouse:buyProduct', id)
    }
  }
}
</script>

<style lang="scss" scoped>
.board-shop{
  display: flex;
  background-image: url('/img/farmHouse/bg-body.png');
  background-size: cover;
  background-position: center;
  background-repeat: no-repeat;
  &:before{
    content: '';
    width: .1rem;
    height: calc(100% - (6.8rem));
    position: absolute;
    bottom: 0;
    left: 50%;
    background: rgba(255, 255, 255, 0.1);
  }
  &__col{
    width: 100%;
    height: 100%;
    display: flex;
    flex-direction: column;
    margin-right: 1.1rem;
    padding: 2rem 1.8rem 1rem 2.5rem;
    overflow-y: auto;
    &::-webkit-scrollbar{
      width: .2rem;
      background-color: transparent;
      &-thumb{
        background: rgba(255, 255, 255, 0.2);
        border-radius: .2rem;
      }
    }
  }
  &-title{
    font-weight: bold;
    font-size: 2.5rem;
    line-height: 2.5rem;
    margin-bottom: 1rem;
    text-transform: uppercase;
    &--small{
      font-size: 1.5rem;
      line-height: 1.5rem;
      margin-bottom: .8rem;
    }
  }
  &__seeds, &__needful, &__fertilizers{
    display: grid;
  }
  &__seeds{
    grid-template-columns: repeat(3, 1fr);
    grid-gap:  3.2rem;
    padding-top: 1.2rem;
  }
  &__needful{
    grid-template-columns: repeat(2, 1fr);
    grid-auto-rows: 6.25rem;
    grid-gap: 1.5rem;
    margin-bottom: 2rem;
  }
  &__fertilizers{
    grid-template-columns: repeat(3, 1fr);
    grid-column-gap:  1.5rem;
    grid-row-gap: 3.15rem;
    padding-top: 1.3rem;
  }
}
</style>
