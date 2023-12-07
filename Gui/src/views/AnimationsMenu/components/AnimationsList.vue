<template>
  <div class="animations-list">
    <div 
      class="animations-list__item" 
      v-for="item in currentAnimations.array" 
      :key="item.id"
      v-bind:style="{ 'background-image': 'url(https://language-api-zszc.onrender.com/animations/items/' + item.key + '.gif)' }"
      @click="saveAnim(item)"
    >
      <div class="info">
        <div class="info__name">{{loc(item.name)}}</div>
        <div class="info__btns">
          <div
            class="btn btn-select"
            @click="play(item)">
          </div>
          <div
            :class="[{ active: item.isFavorite }, 'btn btn-favorites']"
            @click="setFavorite(item)"
          ></div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapMutations, mapGetters } from 'vuex'

export default {
  name: 'AnimationList',

  props: {
    currentAnimations: Object
  },

  computed: {
    ...mapGetters('localization', ['loc']),
  },

  methods: {
    ...mapMutations('animationsMenu', ['setFavorite', 'saveAnim']),

    play: function(animation) {
      window.mp.trigger('animations::play', animation.key);
    }
  }
}
</script>

<style lang="scss" scoped>
.animations-list{
  display: grid;
  height: 35rem;
  grid-template-columns: repeat(8, 10rem);
  grid-auto-rows: 15.7rem;
  grid-column-gap: 1.6rem; 
  grid-row-gap: 1.7rem; 
  padding-left: 2.5rem;
  margin-top: 2rem;
  overflow-y: scroll;
  margin-right: .7rem;
  text-align: center;
  &::-webkit-scrollbar{
    background: transparent;
    padding-top: 2rem;
    width: .3rem;
  }
  &::-webkit-scrollbar-thumb{
    background: #E5E5E5;
    border-radius: 1.1rem;
  }
  &__item{
    background-color: rgba(0, 0, 0, 1);
    background-size: contain;
    background-repeat: no-repeat;
    background-position: center;
    box-shadow: 0 .6rem 1.1rem rgba(0, 0, 0, 0.35);
    border-radius: .5rem; 
    display: flex;
    flex-flow: column;
    justify-content: flex-end;
    overflow: hidden;
    border: .1rem solid transparent;
    &:hover{
      border: .1rem solid #C1E704;
      transition: .2s;
    }
    .info{
      width: 100%;
      background: radial-gradient(127.24% 127.24% at 50% 0%, #000000 0%, rgba(0, 0, 0, 0) 51.39%);
      padding: 1.25rem 0;
      min-height: 6.1rem;
      display: flex;
      flex-flow: column;
      justify-content: space-between;
      align-items: center;
      &__btns{
        display: flex;
        align-items: center;
        justify-content: center;
        .btn{
          width: 2.15rem;
          height: 1.7rem;
          border-radius: .7rem;
          margin-right: .55rem;
          background-size: contain;
          background-repeat: no-repeat;
          background-position: center;
          &:hover{
            transition: .2s;
          }
          &:last-child{
            margin-right: 0;
          }
          &.btn-select{
            background-image: url('/img/animationsMenu/btn-select.svg');
            background-color: rgba(232, 255, 119, 0.15);
            border: 1px solid rgba(232, 255, 119, 0.35);
            &:hover, &.active{
              background-image: url('/img/animationsMenu/btn-select-hover.svg');
              background-color: #C1E704;
              border: 1px solid #E8FF77;
            }
            &:hover{
              box-shadow: 0 .6rem .6rem -.4rem rgba(193, 231, 4, 0.45);
            }
          }
          &.btn-favorites{
            background-image: url('/img/animationsMenu/btn-favorites.svg');
            background-color: rgba(255, 255, 255, 0.15);
            border: 1px solid rgba(255, 255, 255, 0.35);
            &:hover, &.active{
              background-image: url('/img/animationsMenu/btn-favorites-hover.svg');
              background-color: #FFD233;
              border: 1px solid rgba(255, 255, 255, 0.35);
            }
            &:hover{
              box-shadow: 0 .6rem .6rem -.4rem rgba(255, 210, 51, 0.45);
            }
          }
        }
      }
      &__name{
        text-align: center;
      }
    }
  }
}
</style>
