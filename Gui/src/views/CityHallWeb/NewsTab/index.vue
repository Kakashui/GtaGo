<template>
  <div class="body-tab news-tab">
    <div>
      <div class="news-tab__title">Los Santos <span>news</span></div>
      <div class="news-tab__categories">
        <div 
          :class="[{active : item.key === currentCategory}, 'categories-item']"
          v-for="item in categoriesList"
          :key="item.key"
          @click="setCurrentCategory(item.key)"
        >{{loc(item.text)}}</div>
      </div>
      <div class="news-tab__list">
        <div 
          class="list-item"
          v-for="news in currentList"
          :key="news.key"
        >
          <div>
            <div 
              class="img"
              :style="{backgroundImage: 'url(/img/cityHallWeb/news/'+ news.img +'.png)'}"
            ></div>
            <div :class="[news.category, 'current-category']">{{loc(news.categoryName)}}</div>
          </div>
          <div>
            <div class="author">{{news.author}}</div>
            <div class="title">{{loc(news.title)}}</div>
            <div class="desc">{{loc(news.desc)}}</div>
          </div>
        </div>
      </div>
    </div>
    <div>
      <div class="news-tab__prompt">{{loc('cityHallWeb_70')}}</div>
      <div 
        class="news-tab__btn btn"
        @click="modalShow = true"
      >{{loc('cityHallWeb_71')}}</div>
      <div class="news-tab__img"></div>
    </div>
    <Modal
      v-if="modalShow"  
      :categoriesList="listForOptions"
      @closeModal="modalShow = false"
    />
  </div>
</template>

<script>
import Modal from './Modal'

import { mapGetters, mapState } from 'vuex'

export default {
  name: 'NewsTab',

  components: {
    Modal
  },

  data: function() {
    return {
      currentCategory: null,
      categoriesList: [
        {
          text: 'cityHallWeb_72',
          key: 'all'
        },
        {
          text: 'cityHallWeb_73',
          key: 'crime'
        },
        {
          text: 'cityHallWeb_74',
          key: 'sport'
        },
        {
          text: 'cityHallWeb_75',
          key: 'politics'
        },
        {
          text: 'cityHallWeb_76',
          key: 'culture'
        }
      ],
      modalShow: false,
    }
  },

  methods: {
    setCurrentCategory: function(value) {
      this.currentCategory = value
    }
  },

  computed: {
    ...mapState('cityHallWeb/news', ['list']),
    ...mapGetters('localization', ['loc']),
    currentList: function() {
      if (this.currentCategory === 'all') {
        return this.list
      } else {
        return this.list.filter(element => element.category === this.currentCategory)
      }
    },
    listForOptions: function() {
      let array = this.categoriesList
      return array.slice(1, array.length)
    }
  },

  created() {
    this.currentCategory = this.categoriesList[0].key
  }
}
</script>

<style lang="scss" scoped>
.city-hall-web{
  .body-tab{
    &.news-tab{
      background-color: transparent;
      padding: 0;
    }
  }
  .news-tab{
    display: flex;
    justify-content: space-between;
    align-items: flex-start;
    &>div{
      display: flex;
      flex-flow: column;
      width: 100%;
      &:nth-child(1){
        width: 45.1rem;
        min-width: 45.1rem;
        margin-right: 3.4rem;
      }
      &:nth-child(2){
        background-color: #fff;
        padding: 2.5rem .5rem .5rem .5rem;
        border-radius: 1rem;
        height: 100%;
      }
    }
    &__title{
      font-family: Merriweather;
      font-style: normal;
      font-weight: bold;
      font-size: 2.4rem;
      line-height: 3rem;
      color: #46555F;
      margin-bottom: 1.2rem;
      span{
        color: rgba(70, 85, 95, 0.6);
      }
    }
    &__categories{
      display: flex;
      align-items: center;
      margin-bottom: 2.2rem;
      .categories-item{
        display: flex;
        align-items: center;
        justify-content: center;
        padding: .5rem .8rem;
        font-family: Roboto;
        font-style: normal;
        font-weight: 500;
        font-size: .8rem;
        color: rgba(70, 85, 95, 0.5);
        border-radius: .2rem;
        margin-right: 1.3rem;
        transition: .2s;
        &:last-child{
          margin-right: 0;
        }
        &:hover, &.active{
          color: #E00B29;
          transition: .2s;
        }
        &.active{
          background: rgba(70, 85, 95, 0.05);
        }
      }
    }
    &__list{
      display: flex;
      flex-flow: column;
      overflow-y: auto;
      height: calc(100vh - (20.35rem));
      &::-webkit-scrollbar{
        background: transparent;
        width: 0;
      }
      .list-item{
        display: flex;
        align-items: flex-start;
        margin-bottom: 2.2rem;
        width: 100%;
        &>div{
          display: flex;
          flex-flow: column;
          width: 100%;
          &:nth-child(1){
            margin-right: 1rem;
            width: 5.45rem;
            min-width: 5.45rem;
          }
          .img{
            width: 5.45rem;
            height: 5.45rem;
            background-size: contain;
            background-repeat: no-repeat;
            background-position: center;
            border-radius: .2rem;
            overflow: hidden;
            margin-bottom: .5rem;
          }
          .current-category{
            width: 100%;
            display: flex;
            align-items: center;
            justify-content: center;
            font-family: Roboto;
            font-style: normal;
            font-weight: 500;
            font-size: .7rem;
            background: #EAEDEF;
            height: 1.95rem;
            min-height: 1.95rem;
            border-radius: .2rem;
            &.culture{
              color: #2F9664;
            }
            &.politics{
              color: #B57438;
            }
            &.crime{
              color: #E00B29;
            }
            &.sport{
              color: #3AAEFB;
            }
          }
          .author{
            font-family: Roboto;
            font-style: normal;
            font-weight: normal;
            font-size: .7rem;
            line-height: .8rem;
            color: #46555F;
          }
          .title{
            font-family: Merriweather;
            font-style: normal;
            font-weight: bold;
            font-size: 1.5rem;
            line-height: 1.9rem;
            color: #46555F;
            margin-bottom: .5rem;
          }
          .desc{
            font-family: Roboto;
            font-style: normal;
            font-weight: normal;
            font-size: .8rem;
            line-height: 1.1rem;
            color: #46555F;
          }
        }
      }
    }
    &__prompt, &__btn{
      margin-left: 2rem;
      margin-right: 2rem;
    }
    &__prompt{
      font-family: Merriweather;
      font-style: normal;
      font-weight: bold;
      font-size: 1.2rem;
      line-height: 1.5rem;
      color: #000000;
      margin-bottom: 1.2rem;
    }
    &__btn{
      margin-bottom: 2.5rem;
      justify-content: flex-start;
      font-family: Roboto;
      font-style: normal;
      font-weight: 500;
      font-size: .9rem;
      letter-spacing: 0.07em;
      color: #46555F;
      &:hover{
        &:before{
          transform: scale(1.1);
          transition: .2s;
        }
      }
      &:before{
        content: '+';
        text-transform: uppercase;
        font-style: normal;
        font-weight: bold;
        font-size: 1.2rem;
        letter-spacing: 0.03em;
        color: #46555F;
        display: flex;
        align-items: center;
        justify-content: center;
        width: 2rem;
        height: 2rem;
        border: .1rem solid #E00B29;
        border-radius: 50%;
        margin-right: .5rem;
        transition: .2s;
      }
    }
    &__img{
      border-radius: .5rem;
      overflow: hidden;
      background-size: cover;
      background-position: center;
      background-repeat: no-repeat;
      background-image: url('/img/cityHallWeb/news-img.png');
      width: 100%;
      height: 100%;
    }
  }
}
</style>
