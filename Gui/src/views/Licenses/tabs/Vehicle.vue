<template>
  <div class="vehicle-tab body-tab">
    <div class="body-tab__title">
      <div class="desc">{{loc('Licenses_15')}}</div>
      <div class="value">{{loc('Licenses_16')}}</div>
    </div>
    <div class="vehicle-tab__licenses">
      <template v-if="currentCategories.length > 0">
        <div 
          class="item" 
          v-for="item in currentCategories"
          :key="item.name"
        >
          <div class="item__head">
            <div class="category">{{item.name}}</div>
            <div 
              class="img"
              :style="{backgroundImage: 'url(/img/licenses/'+ item.img +'.svg)'}"
            ></div>
          </div>
          <div class="item__desc">{{loc('Licenses_6')}}:</div>
          <div class="item__date">{{item.date}}</div>
        </div>
      </template>
      <div 
        v-else
        class="no-categories"
      >{{loc('Licenses_17')}}</div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
export default {
  name: 'Vehicle',

  computed: {
    ...mapGetters('localization', ['loc']),
    ...mapState('licenses', ['licenses']),

    currentCategories: function() {
      let array = this.licenses
      return array.filter(element => element.type === 'vehicle')
    }
  }
}
</script>

<style lang="scss" scoped>
.vehicle-tab{
  &__licenses{
    width: 100%;
    height: 11.1rem;
    overflow-y: auto;
    border: 1px solid rgba(71, 80, 89, 0.2);
    padding: 1rem 1.25rem 1rem 1rem;
    display: grid;
    grid-template-columns: repeat(5, 1fr);
    grid-auto-rows: 4rem;
    grid-gap: 1.3rem;
    background-color: #fff;
    position: relative;
    &::-webkit-scrollbar{
      width: .2rem;
      background-color: transparent;
      &-thumb{
        background: #EBECED;
        border-radius: .1rem;
      }
    }
    .item{
      display: flex;
      flex-flow: column;
      align-items: flex-start;
      justify-content: space-between;
      &__head{
        width: 100%;
        display: flex;
        justify-content: space-between;
        align-items: center;
        padding: .2rem .7rem;
        background: rgba(51, 61, 71, 0.1);
        margin-bottom: .6rem;
        .img{
          width: 1.2rem;
          height: 1.2rem;
          background-size: contain;
          background-position: center;
          background-repeat: no-repeat;
        }
      }
      &__desc{
        font-weight: 600;
        font-size: .7rem;
        line-height: .8rem;
        color: rgba(51, 61, 71, 0.4);
      }
      &__date{
        font-weight: 600;
        font-size: .8rem;
        line-height: .9rem;
        color: #333D47;
      }
    }
    .no-categories{
      position: absolute;
      width: 100%;
      height: 100%;
      display: flex;
      flex-flow: column;
      align-items: center;
      justify-content: center;
      font-weight: 600;
      font-size: .7rem;
      line-height: .8rem;
      text-align: center;
      letter-spacing: 0.04em;
      color: #475059;
      &:before{
        content: '';
        width: 5rem;
        height: 5rem;
        background-size: contain;
        background-repeat: no-repeat;
        background-position: center;
        margin-bottom: .5rem;
        background-image: url('/img/licenses/no-categories.svg');
      }
    }
  }
}
</style>
