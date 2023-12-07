<template>
  <div class="music-menu">
    <div class="music-menu__title">Music Party</div>
    <div class="music-menu__search">
      <div class="music-menu__search-desc">{{loc('Music_menu_0')}}</div>
      <div class="music-menu__search-title">{{loc('Music_menu_7')}}</div>
      <div class="music-menu__search-input">
        <input 
          type="text"
          :placeholder="loc('Music_menu_6')"
          v-model="currentSong"
        >
        <div 
          class="music-menu__search-btn"
          @click="searchSong"
        >Go</div>
      </div>
      <div class="music-menu__search-prompt">
        <div class="music-menu__search-desc">{{loc('Music_menu_1')}} <span>$ {{listenCost}}</span></div>
      </div>
      <div 
        class="music-menu__search__result"
        v-if="foundSong"
      >
        <div class="music-menu__search__result-desc">{{loc('Music_menu_2')}}</div>
        <div class="music-menu__search__result__song">
          <div 
            class="music-menu__search__result__song-btn"
            @click="listenSong"
          ></div>
          <div class="music-menu__search__result__song-info">
            <div>{{foundSong.text}}</div>
            <div class="music-menu__search__result__song-info__time">{{foundSong.time}}</div>
          </div>
        </div>
      </div>
    </div>
    <div class="music-menu__add">
      <div 
        class="music-menu__service"
        v-for="service in servicesList"
        :key="service.key"
      >
        <div 
          class="music-menu__service-img"
          :style="{backgroundImage: `url(/img/musicMenu/services/${service.img})`}"
        ></div>
        <div class="music-menu__service-cost">
          <span>$ {{service.cost.toLocaleString()}}</span>
        </div>
        <div class="music-menu__service-title">{{loc(service.title)}}</div>
        <div 
          class="music-menu__service-btn"
          @click="orderService(service.key)"
        >{{loc('Music_menu_3')}}</div>
      </div>
    </div>
    <div class="music-menu_load" v-if="load">Search...</div>
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
export default {
  name: 'MusicMenu',
  data() {
    return {
      currentSong: "",
      load: false,
      auad: false,
      audio: null,
      servicesList: [
        {
          key: 'light-music',
          title: 'Music_menu_4',
          img: 'light-music.png',
          cost: 2000
        },
        {
          key: 'disco-smoke',
          title: 'Music_menu_5',
          img: 'disco-smoke.png',
          cost: 3000
        },
      ]
    }
  },
  computed: {
    ...mapState('musicMenu', ['listenCost', 'foundSong']),
    ...mapGetters('localization', ['loc']),
  },
  methods: {
    orderService(key) {
      window.mp.trigger('MusicMenu:orderService', key)
    },
    searchSong() {
      if(this.load)return;
      if (this.currentSong !== "") {
        this.load = true;
        const audio = new Audio(this.currentSong);
        audio.onerror += (error)=>{
          window.console.log(`error: ${error}`)
        };
        audio.onload += (e)=>{
           window.console.log(`loaded: ${e}`)
        }
        audio.play();
        //window.mp.trigger('MusicMenu:searchSong', this.currentSong)
        this.currentSong = null
      }
    },
    onError(error){
      window.console.log(`error: ${error}`)
      this.load = false;
    },
    onLoad(e){
      window.console.log(`loaded: ${e}`)
      this.load = false;
    },
    listenSong() {
      window.mp.trigger('MusicMenu:listenSong')
    }
  }
}
</script>

<style lang="scss">
.music-menu{
  position: absolute;
  display: flex;
  width: 54.05rem;
  height: 35.45rem;
  background-size: cover;
  background-image: url('/img/musicMenu/bg.png');
  font-style: normal;
  text-transform: uppercase;
  color: #fff;
  padding-top: 7.8rem;
  position: relative;
  &_load{
    z-index: 5;
    position: absolute;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background-color: rgba(#000, .7);
    display: flex;
    align-items: center;
    justify-content: center;
    letter-spacing: .06rem;
  }
  &__title{
    font-weight: bold;
    position: absolute;
    left: 0;
    top: 0;
    display: flex;
    align-items: center;
    justify-content: center;
    width: 100%;
    height: 7.8rem;
    font-size: 4rem;
    line-height: 4.6rem;
    text-transform: uppercase;
    text-shadow: 0 1rem 2rem rgba(255, 255, 255, 0.3);
    background-size: cover;
    background-image: url('/img/musicMenu/bg-header.png');
    padding-top: .85rem;
    &:before{
      content: '';
      width: 25.8rem;
      height: 12.4rem;
      background-size: contain;
      background-repeat: no-repeat;
      background-position: center;
      background-image: url('/img/musicMenu/decorate.png');
      position: absolute;
      top: -2.2rem;
      pointer-events: none;
      animation: decorateAnim 1s infinite;
    }
  }
  &__search{
    width: 69%;
    display: flex;
    flex-direction: column;
    padding: 5.45rem 7.2rem 0 7.2rem;
    border-right: .1rem solid rgba(255, 255, 255, 0.1);
    &-desc{
      font-size: .8rem;
      line-height: .9rem;
      letter-spacing: 0.04em;
      color: rgba(255, 255, 255, 0.6);
    }
    &-title{
      font-size: 2.5rem;
      line-height: 2.85rem;
      margin-bottom: 1rem;
    }
    &-input{
      background: #222222;
      border: 1px solid rgba(255, 255, 255, 0.2);
      padding: .65rem .65rem .65rem 0;
      width: 100%;
      display: flex;
      align-items: center;
      margin-bottom: 1rem;
      input{
        padding-left: 1.2rem;
        height: 100%;
        width: 100%;
        background: transparent;
        font-size: 1rem;
        color: #fff;
        &::placeholder{
          color: rgba(255, 255, 255, 0.4);
        }
      }
    }
    &-btn{
      display: flex;
      align-items: center;
      justify-content: center;
      height: 2rem;
      width: 2rem;
      min-width: 2rem;
      background: #6933DB;
      transition: .3s;
      &:hover{
        box-shadow: 0 .1rem 1rem #6933DB;
      }
    }
    &-prompt{
      display: flex;
      align-items: center;
      padding-top: 1rem;
      border-top: .1rem solid rgba(255, 255, 255, 0.2);
      margin-bottom: 1rem;
      span{
        color: #fff;
      }
    }
    &__result{
      display: flex;
      align-items: flex-start;
      &-desc{
        display: flex;
        align-items: center;
        font-size: 1.5rem;
        line-height: 1.7rem;
        text-transform: uppercase;
        color: #A178F7;
        height: 1.9rem;
        padding-right: 1.5rem;
        border-right: .1rem solid rgba(255, 255, 255, 0.2);
        margin-right: 1.5rem;
        min-width: 11rem;
        &:before{
          content: '';
          width: 1.5rem;
          height: 1.5rem;
          margin-right: .3rem;
          background-size: contain;
          background-repeat: no-repeat;
          background-position: center;
          background-image: url('/img/musicMenu/done-icon.png');
        }
      }
      &__song{
        display: flex;
        align-items: flex-start;
        &-btn{
          width: 2.2rem;
          height: 2.2rem;
          min-width: 2.2rem;
          border-radius: 50%;
          background-color: #6933DB;
          background-size: contain;
          background-repeat: no-repeat;
          background-position: center;
          background-image: url('/img/musicMenu/play-icon.png');
          display: flex;
          align-items: center;
          justify-content: center;
          margin-right: .8rem;
          transition: .3s;
          &:hover{
            box-shadow: 0 .1rem 1rem #6933DB;
          }
        }
        &-info{
          display: flex;
          flex-direction: column;
          font-size: .8rem;
          line-height: .9rem;
          letter-spacing: 0.04em;
          text-transform: initial;
          &__time{
            color: rgba(255, 255, 255, 0.7);
            font-weight: normal;
          }
        }
      }
    }
  }
  &__add{
    width: 31%;
    display: flex;
    flex-direction: column;
    padding: 2.5rem 3.85rem 0 2.85rem;
  }
  &__service{
    display: flex;
    flex-direction: column;
    margin-bottom: 2rem;
    position: relative;
    &:last-child{
      margin-bottom: 0;
    }
    &-img{
      width: 100%;
      height: 5.7rem;
      background-size: contain;
      background-repeat: no-repeat;
      background-position: left;
      margin-bottom: .5rem;
    }
    &-cost{
      position: absolute;
      top: 2.5rem;
      left: 5rem;
      font-size: 1.2rem;
      height: 2rem;
      width: fit-content;
      padding: .35rem .7rem;
      background: #6933DB;
      display: flex;
      align-items: center;
      white-space: pre;
      z-index: 1;
      span{
        z-index: 1;
      }
      &:before{
        content: '';
        background: #6933DB;
        border: .7rem solid transparent;
        border-left: .7rem solid #6933DB;
        border-bottom: .7rem solid #6933DB;
        position: absolute;
        left: 0;
        transform: translateX(-49%) rotate(45deg);
        z-index: 0;
      }
    }
    &-title{
      font-size: 1.2rem;
      line-height: 1.35rem;
      text-shadow: 0 1rem 2rem rgba(255, 255, 255, 0.4);
      margin-bottom: .6rem;
    }
    &-btn{
      width: fit-content;
      display: flex;
      align-items: center;
      justify-content: center;
      background: rgba(255, 255, 255, 0.1);
      padding: .7rem 1rem;
      font-size: .8rem;
      line-height: .9rem;
      letter-spacing: 0.04em;
      text-shadow: 0 1rem 2rem rgba(255, 255, 255, 0.4);
      transition: .3s;
      &:hover{
        background: rgba(255, 255, 255, 0.2);
      }
    }
  }
  @keyframes decorateAnim {
    0%{
      transform: scale(1);
    }
    25%{
      transform: scale(1.15);
    }
    55%{
      transform: scale(.95);
    }
    75%{
      transform: scale(1.1);
    }
    100%{
      transform: scale(1);
    }
  }
}
</style>
