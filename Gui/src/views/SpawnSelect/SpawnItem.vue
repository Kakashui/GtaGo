<template>
  <div :class="[{open: item}, {blocked: !item},  'spawn-item']" @click="enter">
    <div class="spawn-item__decorate">{{loc('SpawnSelect_0')}}<span></span></div>
    <div 
      class="spawn-item__img"
      :style="{backgroundImage: 'url(./img/spawnSelect/spawn-'+ index +'.png)'}"
    ></div>
    <template v-if="item">
      <div 
        class="spawn-item__icon"
        :style="{backgroundImage: 'url(./img/spawnSelect/icon-'+ index +'.png)'}"
      ></div>
      <div class="spawn-item__info">
        <div class="title">{{loc(info[index])}}</div>
        <div class="desc">{{item}}</div>
      </div>
    </template>
    <div 
      class="spawn-item__info"
      v-else
    >
      <div class="title">{{loc(info[index])}}</div>
      <div class="desc">{{loc(blockedInfo[index])}}</div>
    </div>
    <div 
      class="spawn-item__select"
      v-if="item"
    >{{loc('SpawnSelect_1')}}</div>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
export default {
  name: 'SpawnItem',

  props: {
    index: Number,
    item: String
  },

  data: function() {
    return {
      spamProtection: 0,
      info: [
        'SpawnSelect_2',
        'SpawnSelect_3',
        'SpawnSelect_4',
        'SpawnSelect_5',
      ],
      blockedInfo: [
        'SpawnSelect_6',
        '',
        'SpawnSelect_7',
        'SpawnSelect_8',
      ]
    }
  },

  methods: {
    enter: function() {
      if(this.spamProtection > Date.now()) return;
      this.spamProtection = Date.now() + 1000;
      window.mp.triggerServer('auth:char:spawn', this.index)
    }
  },

  computed: {
    ...mapGetters('localization', ['loc'])
  }
}
</script>

<style lang="scss" scoped>
.spawn-item{
  display: flex;
  flex-flow: column;
  align-items: center;
  justify-content: flex-end;
  width: 21rem;
  height: 36rem;
  padding: .5rem;
  background: rgba(255, 255, 255, 0.2);
  margin-right: .5rem;
  transition: .5s;
  position: relative;
  &>div{
    z-index: 3;
    &.spawn-item__img{
      z-index: 1;
    }
  }
  &__decorate{
    font-size: 1rem;
    line-height: 1.2rem;
    letter-spacing: 0.2em;
    color: #FFFFFF;
    position: absolute;
    top: -17px;
    transform: translateY(-1000%);
    display: flex;
    align-items: center;
    justify-content: center;
    opacity: 0;
    &:before, &:after{
      content: '';
      height: 1px;
      width: 100vw;
      position: absolute;
      background: rgba(255, 255, 255, 0.4);
    }
    &:before{
      left: -1.5rem;
      transform: translateX(-100%);
    }
    &:after{
      right: -1.5rem;
      transform: translateX(100%);
    }
    span{
      border: .3rem solid transparent;
      border-top: .3rem solid #fff;
      position: absolute;
      bottom: -.1rem;
      transform: translateY(100%);
    }
  }
  &__img, &:after{
    width: calc(100% - (1rem));
    position: absolute;
  }
  &__img{
    height: calc(100% - (1rem));
    top: 50%;
    transform: translateY(-50%);
    background-size: cover;
    background-position: center;
    background-repeat: no-repeat;
  }
  &__icon{
    width: 7rem;
    height: 7rem;
    background-size: contain;
    background-repeat: no-repeat;
    background-position: center;
    position: absolute;
    top: 4rem;
  }
  &__info{
    display: flex;
    flex-flow: column;
    .title{
      font-weight: bold;
      font-size: 3rem;
      line-height: 3rem;
      text-align: center;
      letter-spacing: 0.03em;
      color: #FFFFFF;
      margin-bottom: .75rem;
      max-width: 17rem;
    }
    .desc{
      font-size: 1rem;
      line-height: 1.2rem;
      text-align: center;
      letter-spacing: 0.03em;
      font-weight: 300;
      color: #FFFFFF;
      margin-bottom: 2.5rem;
    }
  }
  &__select{
    display: flex;
    align-items: center;
    justify-content: center;
    width:  10.5rem;
    height: 3.6rem;
    background: #B6D300;
    border-radius: 5rem;
    position: absolute;
    bottom: -1.4rem;
    font-weight: normal;
    font-size: 1rem;
    letter-spacing: 0.03em;
    color: #FFFFFF;
    opacity: 0;
    transition: .3s;
    &:hover{
      box-shadow: 0 .5rem 3rem rgba(182, 211, 0, 0.5);
      transition: .3s;
    }
  }
  &:after{
    content: '';
    height: 22.5rem;
    background: linear-gradient(180deg, rgba(2, 10, 30, 0) 0%, #020A1E 100%);
    bottom: .5rem;
    z-index: 2;
  }
  &:last-child{
    margin-right: 0;
  }
  &.open{
    &:hover{
      height: 44rem;
      background: #fff;
      transition: .5s;
      .spawn-item__decorate{
        transform: translateY(-100%);
        opacity: 1;
        transition: .5s;
      }
      .spawn-item__select{
        opacity: 1;
        transition: .7s;
      }
    }
  }
  &.blocked{
    &:after{
      display: flex;
      align-items: center;
      justify-content: center;
      text-align: center;
      content: 'CLOSE';
      background: rgba(2, 10, 30, 0.8);
      height: calc(100% - (1rem));
      font-size: 11rem;
      line-height: 11rem;
      text-align: center;
      font-weight: 300;
      letter-spacing: 0.04em;
      color: rgba(255, 255, 255, 0.08);
    }
    &:before{
      content: '';
      position: absolute;
      width: 8rem;
      height: 8rem;
      top: 3.5rem;
      background-size: contain;
      background-position: center;
      background-repeat: no-repeat;
      background-image: url('/img/spawnSelect/icon-blocked.svg');
      z-index: 4;
    }
  }
}
</style>
