<template>
    <div class="character">
      <div :class="[{woman: item.gender === 0}, 'img']"></div>
      <div class="character__level">
        <div class="value">{{item.level}}</div>
        <span>level</span>
      </div>
      <div 
        class="character__ban"
        v-if="item.ban"
      >
        <div class="desc">
        </div>
        <div class="value">{{loc('characterSelect_4')}} {{parseTime(item.ban.untilBan)}}</div>
        <div class="reason-block">
          <div class="reason-block__info">
            <div class="desc">{{loc('characterSelect_5')}}</div>
            <div class="value">{{item.ban.admin}}</div>
          </div>
          <div class="reason-block__info">
            <div class="desc">{{loc('characterSelect_6')}}</div>
            <div class="value">{{parseTime(item.ban.banTime)}}</div>
          </div>
          <div class="reason-block__info">
            <div class="desc">{{loc('characterSelect_7')}}</div>
            <div class="value">{{item.ban.reason}}</div>
          </div>
        </div>
      </div>
      <div class="character__nickname">{{item.name.replace('_', ' ')}}
        <div class="decorate">{{loc('characterSelect_8')}}</div>
      </div>
      <div class="character__info">
        <div class="info-block">
          <div class="info-block__desc">{{loc('characterSelect_9')}}</div>
          <div class="info-block__value">{{item.frac}}</div>
        </div>
        <div class="row">
          <div class="info-block">
            <div class="info-block__desc">{{loc('characterSelect_10')}}</div>
            <div class="info-block__value">${{item.bank}} </div>
          </div>
          <div class="info-block">
            <div class="info-block__desc">{{loc('characterSelect_11')}}</div>
            <div class="info-block__value">${{item.cash}} </div>
          </div>
        </div>
        <div class="stats">
          <div 
            class="stats__item"
            v-for="stat in item.stats"
            :key="stat.key"
          >
            <div class="stats-title">
              <div class="stats-title__desc">{{loc(stat.desc)}}</div>
              <div class="stats-title__value">{{stat.value}}%</div>
            </div>
            <div class="progress-bar">
              <div 
                class="line"
                :style="{width: stat.value + '%'}"
              ></div>
            </div>
          </div>
        </div>
      </div>
      <div
        v-if="!item.ban" 
        class="character__interaction"
      >
        <div 
          @click="chooseCharacter"
          class="btn btn-apply"
        >{{loc('characterSelect_12')}}</div>
        <div 
          @click="deleteCharacter"
          class="btn btn-delete"
        >{{loc('characterSelect_13')}}</div>
      </div>
    </div>
</template>

<script>
import { mapGetters } from 'vuex'

export default {
  props:{
    index: Number,
    item: Object,
    setModal: Function
  },
  data() {
    return {
      lastClick: 0,
      floodTime: 1500
    }
  },
  computed: {
    ...mapGetters('localization', ['loc']),
  },
  methods: {
    deleteCharacter(){
      if(this.item.level < 10)
        this.setModal(true, 2, this.index);
      else
        this.setModal(true, 1, this.index);
    },
    parseTime(time){
      return new Date(time).toLocaleString('be-BY')
    },

    chooseCharacter: function() {
      if(this.lastClick > Date.now()) return;
      this.lastClick = Date.now() + this.floodTime;
      window.mp.triggerServer('auth:char:select', this.index);
    }
  },
}
</script>

<style lang="scss" scoped>
@keyframes info-view{
  from{
    transform: translateY(100%) skewX(8deg);
    opacity: 0;
  }
  to{
    transform: translateY(0) skewX(8deg);
    opacity: 1;
  }
}
@keyframes title-view{
  from{
    transform: translateX(-100%);
    opacity: 0;
  }
  to{
    transform: translateX(0);
    opacity: 1;
  }
}
.character{
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
  &:hover{
    transition: .3s;
    .character__nickname{
      left: 1.8rem;
      bottom: 17.2rem;
      transition: .2s;
      .decorate{
        opacity: 0;
        transition: .2s;
      }
    }
    .character__interaction{
      opacity: 1;
      transition: .2s;
    }
    .character__info{
      animation: info-view .2s;
      opacity: 1;
    }
  }
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
    background-image: url('/img/characterSelect/char-m.png');
    background-size: cover;
    background-repeat: no-repeat;
    background-position: center;
    width: 120%;
    height: 37rem;
    position: absolute;
    left: -10%;
    top: 0;
    &.woman{
      background-image: url('/img/characterSelect/char-w.png');
    }
  }
  &__level{
    margin-left: 3rem;
    text-transform: uppercase;
    font-style: normal;
    font-weight: 300;
    display: flex;
    align-items: center;
    .value{
      background: linear-gradient(180deg, #FFFFFF 0%, rgba(255, 255, 255, 0) 100%);
      -webkit-background-clip: text;
      background-clip: text;
      -webkit-text-fill-color: transparent;
      font-size: 6.5rem;
    }
    span{
      font-size: 1.3rem;
      line-height: 1.55rem;
      letter-spacing: 0.4em;
      color: #FFFFFF;
      -webkit-writing-mode: vertical-rl; 
      writing-mode:tb-rl;
      transform: rotate(180deg);
      margin-bottom: .3rem;
    }
  }
  &__ban{
    display: flex;
    align-items: center;
    background: #FF3333;
    min-height: 3.45rem;
    height: 3.45rem;
    padding: 0 0.875rem;
    width: fit-content;
    color: #fff;
    z-index: 9;
    margin-left: 1.75rem;
    overflow: hidden;
    &:hover{
      overflow: initial;
      .reason-block{
        opacity: 1;
        transition: .2s;
      }
    }
    &>div{
      transform: skewX(8deg);
    }
    .desc{
      font-size: 1.5rem;
      letter-spacing: 0.1em;
      margin-right: .75rem;
      padding-right: .75rem;
      position: relative;
      height: 2.15rem;
      min-height: 2.15rem;
      border-right: .1rem solid rgba(255, 255, 255, 0.4);
      display: flex;
      align-items: center;
      &:before{
        content: 'BAN';
      }
      &:after{
        content: '';
        width: 1.75rem;
        height: 1.75rem;
        background-size: contain;
        background-position: center;
        background-repeat: no-repeat;
        background-image: url('/img/characterSelect/icon-ban.svg');
        margin-left: .7rem;
      }
    }
    .value{
      font-size: 1rem;
      letter-spacing: 0.1em;
    }
    .reason-block{
      display: flex;
      flex-flow: column;
      padding: 1.5rem;
      position: absolute;
      left: 7.5rem;
      top: 0;
      width: 14rem;
      background-color: #fff;
      z-index: 9;
      transform: skewX(8deg);
      opacity: 0;
      transition: .2s;
      &__info{
        display: flex;
        flex-flow: column;
        margin-bottom: .75rem;
        .desc{
          font-weight: normal;
          font-size: .9rem;
          line-height: 1.1rem;
          letter-spacing: 0.2em;
          color: rgba(0, 0, 0, 0.3);
          text-align: left;
          &:before, &:after{
            content: none;
          }
        }
        .value{
          font-size: 1rem;
          line-height: 1.3rem;
          text-align: center;
          letter-spacing: 0.03em;
          color: rgba(0, 0, 0, 0.8);
          text-align: left;
        }
      }
    }
  }
  &__nickname{
    font-weight: bold;
    font-size: 2.5rem;
    line-height: 2.5rem;
    letter-spacing: 0.03em;
    color: #FFFFFF;
    position: absolute;
    left: 3.9rem;
    bottom: 3.5rem;
    transition: .2s;
    .decorate{
      font-size: .9rem;
      line-height: 1.1rem;
      letter-spacing: 0.2em;
      color: rgba(255, 255, 255, 0.4);
      display: flex;
      align-items: center;
      position: relative;
      width: fit-content;
      transition: .2s;
      &:after{
        content: '';
        width: 5rem;
        height: 1px;
        background-color: #fff;
        position: absolute;
        right: -.5rem;
        transform: translateX(100%);
      }
    }
  }
  &__interaction{
    display: flex;
    align-items: center;
    justify-content: center;
    position: absolute;
    bottom: 0;
    left: 4.5rem;
    opacity: 0;
    transition: .2s;
    .btn-apply{
      border: .1rem solid rgba(255, 255, 255, 0.5);
      border-radius: 5rem;
      height: 3.55rem;
      min-height: 3.55rem;
      padding: 0 4.2rem;
      color: #FFFFFF;
      &:hover{
        color: #1C1C1C;
        background-color: #fff;
        border-color: #fff;
      }
    }
    .btn{
      font-size: 1rem;
      line-height: 1.2rem;
      letter-spacing: 0.03em;
    }
    .btn-delete{
      color: rgba(255, 255, 255, 0.5);
      padding: 0 1.5rem;
      &:hover{
        color: #fff;
      }
    }
  }
  &__info{
    display: flex;
    flex-flow: column;
    width: 16.5rem;
    min-width: 16.5rem;
    position: absolute;
    left: 3rem;
    bottom: 5.6rem;
    opacity: 0;
    &>div{
      width: 100%;
    }
    .info-block{
      display: flex;
      flex-flow: column;
      font-weight: 300;
      margin-bottom: .6rem;
      min-width: 7.3rem;      
      &__desc{
        font-size: .9rem;
        line-height: 1.1rem;
        letter-spacing: 0.2em;
        color: rgba(255, 255, 255, 0.5);
      }
      &__value{
        font-size: 1.5rem;
        line-height: 1.8rem;
        letter-spacing: 0.03em;
        color: #FFFFFF;
      }
    }
    .row{
      display: flex;
      align-items: center;
      justify-content: space-between;
      padding-bottom: 1rem;
      margin-bottom: 1rem;
      border-bottom: 1px solid rgba(255, 255, 255, 0.2);
      .info-block{
        margin-bottom: 0;
      }
    }
    .stats{
      display: grid;
      grid-row-gap: 1rem;
      grid-column-gap: 2rem;
      grid-template-columns: repeat(2, 1fr);
      grid-auto-rows: 1.5rem;
      &__item{
        display: flex;
        flex-flow: column;
        .stats-title{
          display: flex;
          align-items: flex-end;
          justify-content: space-between;
          width: 100%;
          margin-bottom: .2rem;
          &__desc{
            font-style: normal;
            font-weight: bold;
            font-size: .8rem;
            line-height: .8rem;
            letter-spacing: 0.1em;
            text-transform: uppercase;
            color: rgba(255, 255, 255, 0.5);
          }
          &__value{
            font-size: 1rem;
            line-height: 1.2rem;
            text-align: right;
            letter-spacing: 0.1em;
            text-transform: uppercase;
            color: #FFFFFF;
          }
        }
        .progress-bar{
          position: relative;
          background: rgba(255, 255, 255, 0.2);
          border-radius: 100px;
          height: .3rem;
          min-height: .3rem;
          width: 100%;
          border-radius: .5rem;
          .line{
            position: absolute;
            top: 0;
            left: 0;
            height: 100%;
            box-shadow: 0 .2rem 1.8rem rgba(255, 51, 128, 0.5);
            background-color: #fff;
            border-radius: .5rem;
          }
        }
      }
    }
  }

}
</style>