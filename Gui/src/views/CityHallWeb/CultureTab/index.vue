<template>
  <div class="body-tab">
    <div class="page">
      <div class="page__title">{{loc('cityHallWeb_16')}}</div>
      <div class="page__list">
        <div 
          class="list-item"
          v-for="item in list"
          :key="item.key"
        >
          <div 
            class="list-item__img"
            :style="{backgroundImage: 'url(/img/cityHallWeb/events/'+ item.img +'.png)'}"
          >
            <div 
              class="btn btn-event"
              @click="eventReg(item.key)"
            >{{loc('cityHallWeb_17')}}</div>
          </div>
          <div class="list-item__info">
            <div class="info-title">{{loc(item.title)}}</div>
            <div class="info-desc">{{loc(item.desc)}}</div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
export default {
  name: 'CultureTab',

  computed: {
    ...mapState('cityHallWeb/events', ['list']),
    ...mapState('cityHallWeb', ['cardId']),
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    eventReg: function(key) {
      window.mp.trigger('cityHallWeb:eventReg', this.cardId, key)
    },
  }
}
</script>

<style lang="scss" scoped>
.city-hall-web{
  .body-tab{
    display: flex;
    padding: 0;
    background-color: transparent;
    overflow: initial;
    .page{
      width: 100%;
      height: calc(100vh - (10rem));
      padding-bottom: 2rem;
      &__title{
        font-family: Merriweather;
        font-style: normal;
        font-weight: bold;
        font-size: 2.4rem;
        color: #000000;
        margin-bottom: 2rem;
        width: fit-content;
        text-transform: initial;
      }
      &__list{
        display: grid;
        width: 100%;
        grid-template-columns: repeat(3, 1fr);
        grid-gap: 1rem;
        grid-auto-rows: 16.2rem;
        overflow-y: auto;
        height: calc(100vh - (16.2rem));
        &::-webkit-scrollbar{
          background-color: transparent;
          width: 0;
        }
        .list-item{
          display: flex;
          background: #FFFFFF;
          border-radius: .5rem;
          overflow: hidden;
          position: relative;
          padding: 9.85rem 1.5rem 1.5rem 1.5rem;
          &:hover{
            .list-item__img{
              &::after, .btn-event{
                opacity: 1;
                transition: .2s;
              }
            }
          }
          &__img{
            display: flex;
            align-items: center;
            justify-content: center;
            width: 100%;
            height: 8.35rem;
            position: absolute;
            left: 0;
            top: 0;
            background-size: cover;
            background-repeat: no-repeat;
            background-position: center;
            &::after{
              content: '';
              width: 100%;
              height: 100%;
              position: absolute;
              top: 0;
              left: 0;
              z-index: 1;
              background: rgba(0, 0, 0, 0.6);
              opacity: 0;
              transition: .2s;
            }
            .btn-event{
              z-index: 2;
              opacity: 0;
              transition: .2s;
              position: absolute;
              background: #E00B29;
              border-radius: .3rem;
              padding: .8rem 1.2rem;
              font-family: Roboto;
              font-style: normal;
              font-weight: normal;
              font-size: .8rem;
              color: #FFFFFF;
              &:hover{
                background: #91081a;
              }
            }
          }
          &__info{
            display: flex;
            flex-flow: column;
            .info-title{
              font-family: Merriweather;
              font-style: normal;
              font-weight: bold;
              font-size: 1rem;
              line-height: 1.25rem;
              color: #000000;
              margin-bottom: .7rem;
            }
            .info-desc{
              font-family: Roboto;
              font-style: normal;
              font-weight: normal;
              font-size: .8rem;
              line-height: .95rem;
              color: #000000;
            }
          }
        }
      }
    }
  }
}
</style>
