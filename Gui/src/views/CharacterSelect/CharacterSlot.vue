<template>
  <div :class="[{'blocked' : index >= slotsCount}, 'character-slot']">
    <div class="img"></div>
    <template v-if="index < slotsCount">
      <div class="block-wrap">
        <div class="title">{{loc('characterSelect_14')}}</div>
        <div class="info">
          <div class="info__desc">{{loc('characterSelect_15')}}</div>
          <div 
            class="info__btn btn"
            @click="createCharacter"
          >{{loc('characterSelect_16')}}</div>
        </div>
      </div>
    </template>
    <template v-else>
      <div class="block-wrap">
        <div class="decorate">CLOSE</div>
        <div class="title">{{loc('characterSelect_17')}}</div>
        <div class="info">
          <div class="info__desc">{{loc('characterSelect_18')}}</div>
          <div 
            class="info__btn btn"
            @click="setModal(true, 0, index)"
          >{{loc('characterSelect_19')}}</div>
        </div>
      </div>
    </template>
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
export default {
  name: 'CharacterSlot',

  props: {
    index: Number,
    item: Object,
    setModal: Function,

  },
  data() {
    return {
      lastClick: 0,
      floodTime: 1500
    }
  },
  computed: {
    ...mapState('characterSelect', ['slotsCount']),
    ...mapGetters('localization', ['loc']),
  },
  methods: {
    createCharacter: function() {
      if(this.lastClick > Date.now()) return;
      this.lastClick = Date.now() + this.floodTime;
      window.mp.triggerServer('auth:char:select', this.index)
    }
  }
}
</script>

<style lang="scss" scoped>
.character-slot{
  display: flex;
  flex-flow: column;
  width: 25.25rem;
  height: 39.85rem;
  transform: skewX(-8deg);
  margin-right: .9rem;
  position: relative;
  overflow: hidden;
  transition: .3s;
  padding-top: 1.5rem;
  &>div{
    transform: skewX(8deg);
    &.character__ban{
      transform: skewX(0);
    }
  }
  &:last-child{
    margin-right: 0;
  }
  .img{
    background-image: url('/img/characterSelect/empty-slot.png');
    background-size: cover;
    background-repeat: no-repeat;
    background-position: center;
    width: 120%;
    height: 37rem;
    position: absolute;
    left: -10%;
    top: 0;
  }
  &.blocked{
    .img{
      background-image: url('/img/characterSelect/blocked-slot.png');
    }
  }
  &:hover{
    .block-wrap{
      .decorate{
        &:before{
          bottom: 3rem;
        }
        &:after{
          top: 1rem;
          transition: .3s;
        }
      }
    }
  }
  .block-wrap{
    position: absolute;
    left: 48%;
    bottom: 18.2rem;
    transform: skewX(8deg) translateX(-50%);
    width: fit-content;
    .decorate{
      position: absolute;
      font-size: 11.5rem;
      line-height: 11.5rem;
      text-align: center;
      font-weight: 300;
      letter-spacing: 0.03em;
      color: rgba(255, 255, 255, 0.08);
      left: 50%;
      transform: translateX(-45%) translateY(-92%);
      top: 0;
      &:before, &:after{
        content: '';
        position: absolute;
        left: 49%;
        transform: translateX(-50%);
        background-size: contain;
        background-repeat: no-repeat;
        background-position: center;
      }
      &:before{
        width: 5.2rem;
        height: 4.4rem;
        background-image: url('/img/characterSelect/lock.svg');
        bottom: 3rem;
      }
      &:after{
        width: 3.5em;
        height: 3.5rem;
        background-image: url('/img/characterSelect/lock-d.svg');
        top: 1.5rem;
        transition: .3s;
      }
    }
    .title{
      font-weight: bold;
      font-size: 2.5rem;
      line-height: 2.5rem;
      letter-spacing: 0.03em;
      color: #FFFFFF;
      white-space: pre;
    }
    .info{
      display: flex;
      flex-flow: column;
      align-items: center;
      position: absolute;
      bottom: -.75rem;
      transform: translateY(100%);
      &__desc{
        font-size: 1rem;
        line-height: 1.3rem;
        text-align: center;
        letter-spacing: 0.03em;
        color: rgba(255, 255, 255, 0.8);
        max-width: 16rem;
        text-align: center;
        margin-bottom: 2rem;
        min-height: 3.9rem;
      }
      &__btn{
        width: 11.5rem;
        min-height: 3.6rem;
        height: 3.6rem;
        border: .1rem solid rgba(255, 255, 255, 0.2);
        border-radius: 3.6rem;
        font-size: 1rem;
        letter-spacing: 0.03em;
        color: #FFFFFF;
        &:hover{
          color: #1C1C1C;
          background-color: #fff;
          border-color: #fff;
          box-shadow: 0 .5rem 2.5rem rgba(255, 255, 255, 0.25);
        }
      }
    }
  }
  

}
</style>
