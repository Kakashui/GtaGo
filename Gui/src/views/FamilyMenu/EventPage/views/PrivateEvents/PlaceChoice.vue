<template>
  <div class="choice place-choice">
    <div class="place">{{loc('fam:menu:priv:place')}}</div>
    <div class="desc">{{loc('fam:menu:priv:desc')}}</div>
    <div class="place-list">
      <div 
        :class="[{active: item.id === currentPlace}, 'item']"
        @click="setCurrentPlace(item.id)"
        v-for="item in getPlaceList"
        :key="item.key"
      >
        <div 
          class="item__img"
          :style="{backgroundImage: 'url(/img/familyMenu/eventsPage/places/' + item.img + '.png)'}"
        ></div>
        <div class="item__name">{{loc(item.name)}}</div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapState, mapGetters } from 'vuex'
export default {
  name: 'PlaceChoice',

  props: {
    setCurrentPlace: Function,
    currentPlace: Number
  },

  computed: {
    ...mapGetters('localization', ['loc']),
    ...mapState('familyMenu/eventsPage', ['placesList']),
    getPlaceList() {
      let result = this.placesList.slice().filter(item => item.active)
      return result;
    }
  }
}
</script>

<style lang="scss" scoped>
.place-choice{
  display: flex;
  flex-flow: column;
  .place{
    font-size: 2rem;
    line-height: 2.4rem;
    letter-spacing: 0.03em;
    margin-bottom: .1rem;
  }
  .desc{
    font-size: 1.2rem;
    line-height: 1.45rem;
    letter-spacing: 0.03em;
    color: rgba(255, 255, 255, 0.5);
    margin-bottom: 1.5rem;
  }
  .place-list{
    display: grid;
    width: calc(100% + (.8rem) + (.4rem));
    margin-left: -.4rem;
    max-height: calc(100vh - (15.7rem));
    overflow-y: auto;
    padding: 0 .8rem .4rem .4rem;
    grid-template-columns: repeat(3, 1fr);
    grid-column-gap: .95rem;
    grid-row-gap: 1.5rem;
    grid-auto-rows: 10.5rem;
    &::-webkit-scrollbar{
      background-color: transparent;
      width: .2rem;
      &-thumb{
        background: rgba(255, 255, 255, 0.1);
        border-radius: .1rem;
      }
    }
    .item{
      width: 100%;
      position: relative;
      display: flex;
      align-items: center;
      justify-content: center;
      padding: 5.45rem .5rem .1rem .5rem;
      background: rgba(15, 15, 16, 0.7);
      &:hover{
        &:after{
          opacity: 1;
          transition: .2s;
        }
      }
      &:before, &:after{
        content: '';
        position: absolute;
        width: 100%;
        opacity: 0;
      }
      &:before{
        height: .2rem;
        bottom: -.2rem;
        transform: translateY(100%);
        background: #DCFF01;
      }
      &:after{
        height: 100%;
        bottom: 0;
        left: 0;
        z-index: 1;
        background-image: url('/img/familyMenu/eventsPage/place-bg.svg');
        background-size: 100%;
        background-position: center bottom;
        background-repeat: no-repeat;
      }
      &.active{
        &:before, &:after{
          opacity: 1;
        }
      }
      &__img{
        position: absolute;
        left: .1rem;
        top: .1rem;
        height: 5.45rem;
        width: calc(100% - (.2rem));
        background-size: cover;
        background-position: center;
        background-repeat: no-repeat;
        overflow: hidden;
      }
      &__name{
        font-weight: bold;
        font-size: 1.2rem;
        line-height: 1.2rem;
        text-align: center;
      }
    }
  }
}
</style>
