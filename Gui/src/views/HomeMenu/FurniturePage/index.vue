<template>
  <div class="furniture-page">
    <Head :titleText="loc('home:furn:head')"/>
    <div class="header">
      <div class="header__info">
        <div class="info-item">
          <div class="value">{{furnitureList.length}}</div>
          <div class="desc">{{loc('HomeMenu_14')}}</div>
        </div>
        <div class="info-item">
          <div class="value">{{furnitureNotInstalled}}</div>
          <div class="desc">{{loc('HomeMenu_15')}}</div>
        </div>
      </div>
      <div class="header__nav">
        <!--<div class="header__nav-item nav-edit" @click="setCurrentPage('InteriorsPage')">{{loc('homeMenu_16')}}</div>-->
        <div class="header__nav-item nav-remove" @click="uninstallAllFurniture">{{loc('HomeMenu_17')}}</div>
      </div>
    </div>
    <div class="scroll">
      <div class="body">
        <div
          class="body__item"
          v-for="(item) in furnitureSortedList"
          :key="item.id"
        >
          <div class="desc">{{item.name}}</div>
          <!-- <div class="title">#{{item.key}}</div> -->
          <div class="icon-done" v-if="item.placed"></div>
          <div 
            class="btn btn-install"
            v-if="!item.placed"
            @click="installFurniture(item.name, item.id)"
          ></div>
          <div 
            class="btn btn-uninstall"
            v-else
            @click="uninstallFurniture(item.id)"
          ></div>
          <div class="img" :style="{backgroundImage: 'url(/img/homeMenu/items/' + item.key + '.png)'}"></div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapMutations, mapState } from 'vuex'
import Head from '../components/Head'

export default {
  name: 'FurniturePage',

  components: {
    Head
  },

  computed: {
    ...mapState('homeMenu', ['furnitureList', 'houseId']),
    ...mapGetters('localization', ['loc']),
    furnitureNotInstalled: function() {
      return this.furnitureList.filter(item =>item.placed === false).length
    },
    furnitureSortedList: function() {
      return this.furnitureList.slice().sort(function (a, b) {
        if (a.placed && !b.placed) {
          return 1;
        }
        if (!a.placed && b.placed) {
          return -1;
        }
        return 0;
      });
    }
  },

  methods: {
    ...mapMutations('homeMenu', ['setCurrentPage', 'uninstallFurniture', 'uninstallAllFurniture']),
    installFurniture: function(name, id) {
      window.mp.trigger('homeMenu:installFurniture', this.houseId, id)
    }
  }
}
</script>

<style lang="scss" scoped>
.furniture-page{
  width: 100%;
  padding: 2.85rem 2.85rem 0 2.85rem;
  display: flex;
  flex-flow: column;
  height: 100%;
  position: relative;
  &:after{
    content: '';
    width: 100%;
    height: 5.85rem;
    position: absolute;
    left: 0;
    bottom: 0;
    background: linear-gradient(180deg, rgba(24, 29, 37, 0) 0%, #181D25 100%);
    z-index: 4;
  }
  .header{
    display: flex;
    align-items: flex-start;
    margin: 2rem 0;
    position: relative;
    &__info{
      display: flex;
      .info-item{
        position: relative;
        padding: 0 2rem;
        display: flex;
        flex-flow: column;
        position: relative;
        &:first-child{
          padding-left: 0;
        }
        &:last-child{
          &:after{
            content: none;
          }
        }
        &:after{
          content: '';
          background: radial-gradient(50% 50% at 50% 50%, rgba(255, 255, 255, 0.45) 0%, rgba(255, 255, 255, 0) 100%);
          height: 100%;
          width: 1px;
          position: absolute;
          right: 0;
          top: 0;
        }
        .value{
          font-size: 3.6rem;
          line-height: 4.3rem;
        }
        .desc{
          font-size: 1.2rem;
          line-height: 1.45rem;
          color: rgba(255, 255, 255, 0.45);
          margin-top: .3rem;
        }
      }
    }
    &__nav{
      &-item{
        margin-right: .9rem;
        &:last-child{
          margin-right: 0;
        }
        &.nav-edit{
          &:before{
            background-image: url('/img/homeMenu/icon-edit.svg');
          }
        }
        &.nav-remove{
          &:before{
            background-image: url('/img/homeMenu/icon-remove.svg');
          }
        }
      }
    }
  }
  .scroll{
    width: 100%;
    display: flex;
    flex-flow: column;
    overflow-y: auto;
    height: 100%;
    padding-bottom: 5.85rem;
    &::-webkit-scrollbar{
      width: .3rem;
      background: transparent;
    }
    &::-webkit-scrollbar-thumb{
      background: #A9ABAE;
      border-radius: 1rem;
    }
  }
  .body{
    display: grid;
    grid-template-columns: repeat(5, 1fr);
    grid-auto-rows: 9.8rem;
    grid-gap: 2.2rem;
    width: 100%;
    padding-right: 4rem;
    &__item{
      display: flex;
      flex-flow: column;
      justify-content: flex-start;
      align-items: flex-start;
      padding: 1.6rem 1.45rem;
      background: rgba(255, 255, 255, 0.05);
      position: relative;
      transition: .2s;
      &>div{
        z-index: 3;
      }
      &.item-lock{
        &:before{
          background: radial-gradient(66.58% 66.58% at 50% 26.79%, rgba(255, 64, 64, 0.15) 0%, rgba(255, 64, 64, 0) 100%), rgba(255, 255, 255, 0.05);
        }
        &:after{ 
          background: radial-gradient(66.58% 66.58% at 50% 26.79%, rgba(255, 64, 64, 0.45) 0%, rgba(255, 64, 64, 0) 100%), rgba(255, 255, 255, 0.05);
        }
      }
      &.item-unlock{
        &:before{
          background: radial-gradient(66.58% 66.58% at 50% 26.79%, rgba(193, 231, 4, 0.15) 0%, rgba(193, 231, 4, 0) 100%), rgba(255, 255, 255, 0.05);
        }
        &:after{ 
          background: radial-gradient(66.58% 66.58% at 50% 26.79%, rgba(193, 231, 4, 0.35) 0%, rgba(193, 231, 4, 0) 100%), rgba(255, 255, 255, 0.05);
        }
      }
      &.item-colored{
        .title{
          color: #C1E704;
        }
      }
      .desc{
        font-size: 1.05rem;
        line-height: 1.25rem;
        white-space: pre;
      }
      .title{
        font-size: 2.05rem;
        line-height: 2.45rem;
      }
      .cost{
        margin-top: 1.15rem;
        display: flex;
        align-items: center;
        padding-left: 1.4rem;
        font-size: 1.05rem;
        line-height: 1.25rem;
        letter-spacing: 0.05em;
        color: #C1E704;
        position: relative;
        &:before{
          content: '';
          width: 1rem;
          height: 1rem;
          background-size: contain;
          background-repeat: no-repeat;
          background-position: center;
          background-image: url('/img/homeMenu/cost-icon.svg');
          position: absolute;
          left: 0;
        }
      }
      .icon-done{
        display: flex;
        justify-content: center;
        align-items: center;
        width: .7rem;
        height: .6rem;
        position: absolute;
        top: 1.5rem;
        right: 1.25rem;
        background-size: contain;
        background-repeat: no-repeat;
        background-position: center;
        background-image: url('/img/homeMenu/done.svg');
        &:after{
          content: '';
          position: absolute;
          width: 50%;
          height: 50%;
          background-color: rgba(193, 231, 4, 0.65);
          filter: blur(.3rem);
        }
      }
      .btn{
        position: absolute;
        left: 1.5rem;
        bottom: 1.5rem;
        width: 2.35rem;
        height: 2.35rem;
        border-radius: 50%;
        opacity: .65;
        &:hover{
          opacity: 1;
        }
        &-uninstall{
          background-color: rgba(255, 96, 96, 0.65);
          background-image: url('/img/homeMenu/btn-uninstall.svg');
        }
        &-install{
          background-color: rgba(255, 255, 255, 0.65);
          background-image: url('/img/homeMenu/btn-install.svg');
        }
      }
      .img{
        background-size: cover;
        background-repeat: no-repeat;
        background-position: bottom;
        position: absolute;
        height: 100%;
        width: 100%;
        bottom: 0;
        right: 0;
        z-index: 2;
      }
      &:before, &:after{
        content: '';
        width: 100%;
        height: 100%;
        position: absolute;
        border: 1px solid;
        border-image-slice: 1;
        top: 0;
        left: 0;
        z-index: 1;
        box-sizing: border-box;
        transition: .2s;
      }
      &:before{
        border-image-source: linear-gradient(to bottom, rgba(255,255,255, .15), rgba(255,255,255,0));
        opacity: 1;
      }
      &:after{ 
        background: radial-gradient(75.92% 75.92% at 50% 12.89%, rgba(255, 255, 255, 0.25) 0%, rgba(255, 255, 255, 0) 100%), rgba(255, 255, 255, 0.05);
        border-image-source: linear-gradient(to bottom, rgba(255,255,255, .65), rgba(255,255,255,0));
        opacity: 0;
      }
      &:hover{
        &:before, &:after{
          transition: .2s;
        }
        &:before{
          opacity: 0;
        }
        &:after{
          opacity: 1;
        }
      }
    }
  }
}
</style>
