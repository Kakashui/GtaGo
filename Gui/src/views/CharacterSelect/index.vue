<template>
  <div class="character-select">
    <SoundSwitch />
    <div class="character-select__title">{{loc('characterSelect_0')}}</div>
    <div class="add-block">
      <div class="add-block__donat">
        <div class="icon"></div>
        <div class="info">
          <div class="info__desc">{{loc('characterSelect_1')}}</div>
          <div class="info__value">{{coin}}</div>
        </div>
        <!-- <div 
          class="btn btn-add"
          @click="buyCoins"
        >+</div> -->
      </div>
      <!-- <div class="add-block__back">
        <div class="info">
          <div class="info__desc">{{loc('characterSelect_2')}}</div>
          <div class="info__value">{{loc('characterSelect_3')}}</div>
        </div>
        <div 
          class="btn btn-back"
          @click="exitMenu"
        ></div>
      </div> -->
    </div>
    <div class="characters-switch">
      <Component 
        v-for="(item, index) in slots"
        :key="index" 
        :is="item == null ? 'CharacterSlot' : 'Character'" 
        :index="index" 
        :item="item"
        :setModal="setModal"
      />
      <!-- <character-slot
        v-for="(slot, index) in emptySlots" 
        :key="slot" 
        :index="index"
        :setModal="setModal"
      /> -->
      <Modal 
        v-if="modal.show"
        :modal="modal"
        :setModal="setModal"
      />
    </div>
  </div>
</template>

<script>
import Character from './Character'
import CharacterSlot from './CharacterSlot'
import SoundSwitch from '../Auth/SoundSwitch'
import Modal from './Modal'

import {mapGetters, mapState} from 'vuex'

export default {
  name: 'CharacterSelect',

  components:{
    Character,
    CharacterSlot,
    Modal,
    SoundSwitch
  },

  data: function() {
    return {
      modal: {
        show: false,
        type: null,
        index: 0
      }
    }
  },

  computed:{
    ...mapState('characterSelect', ['slots', 'coin']),   
    ...mapGetters('localization', ['loc']),
    // charactersSlots: function() {
    //   return this.slots.filter(element => element !== null)
    // },
    // emptySlots: function() {
    //   return this.slots.filter(element => element === null)
    // }
  },

  methods: {
    setModal: function(show, type, index) {
      this.modal.type = type
      this.modal.index = index;
      this.modal.show = show;
    },
    buyCoins: function() {
      window.mp.trigger('characterSelect:buyCoins')
    },
    exitMenu: function() {
      window.mp.trigger('characterSelect:exitMenu')
    },
    keyUp: function(key) {
      if (key.keyCode === 27) { 
        this.exitMenu()
      }
    }
  },

  mounted() {
    window.addEventListener('keyup', this.keyUp)
  }
  
}
</script>

<style lang="scss" scoped>
@keyframes decorate-view{
  from{
    transform: translateX(15%);
  }
  to{
    transform: translateX(0);
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
.character-select{
  width: 100%;
  height: 100vh;
  display: flex;
  flex-flow: column;
  position: absolute;
  top: 0;
  left: 0;
  z-index: 1;
  text-transform: uppercase;
  font-style: normal;
  padding: 4.5rem 7.5rem 5.75rem 4.25rem;
  background-size: cover;
  background-repeat: no-repeat;
  background-position: center;
  background-image: url('/img/characterSelect/bg.png');
  &::before{
    content: '';
    width: 100vw;
    height: 10.8rem;
    background-size: contain;
    background-repeat: no-repeat;
    background-position: top;
    background-image: url('/img/characterSelect/decorate.svg');
    position: absolute;
    top: 0;
    left: 0;
    animation: decorate-view 1s;
  }
  &__title{
    font-weight: bold;
    font-size: 7rem;
    line-height: 7rem;
    letter-spacing: 0.03em;
    margin-bottom: -.8rem;
    color: #FFFFFF;
    text-shadow: 0 0 3.5rem rgba(255, 255, 255, 0.5);
    animation: title-view 1s;
    z-index: 3;
  }
  .add-block{
    display: flex;
    position: absolute;
    top: 4.5rem;
    right: 7.5rem;
    color: #fff;
    &__donat, &__back{
      font-weight: bold;
      letter-spacing: 0.03em;
      display: flex;
      align-items: center;
      .icon, .btn-add{
        width: 2rem;
        height: 2rem;
      }
      .icon{
        background-size: contain;
        background-repeat: no-repeat;
        background-position: center;
        background-image: url('/img/characterSelect/money.svg');
        position: absolute;
        left: -.65rem;
        transform: translateX(-100%);
      }
      .btn-add{
        border: .1rem solid #B6D300;
        border-radius: 50%;
        background-color: transparent;
        font-weight: bold;
        font-size: 1.2rem;
        line-height: 1.2rem;
        letter-spacing: 0.03em;
        color: #FFFFFF;
        &:hover{
          background-color: #B6D300;
        }
      }
      .info{
        display: flex;
        flex-flow: column;
        margin-right: 1rem;
        &__desc{
          font-size: .9rem;
          line-height: .9rem;
          color: rgba(255, 255, 255, 0.5);
          margin-bottom: .25rem;
        }
        &__value{
          font-weight: bold;
          font-size: 1.2rem;
          line-height: 1.2rem;
          letter-spacing: 0.03em;
          color: #FFFFFF;
        }
      }
      .btn-back{
        width: 2.5rem;
        height: 2.5rem;
        background: rgba(255, 255, 255, 0.1);
        border: 1px solid rgba(255, 255, 255, 0.1);
        position: relative;
        &:after{
          content: '';
          position: absolute;
          width: 1rem;
          height: 1rem;
          background-size: contain;
          background-repeat: no-repeat;
          background-position: center;
          background-image: url('/img/characterSelect/close.svg');
          transition: .2s;
        }
        &:hover{
          background: #FFFFFF;
          border-color: #FFFFFF;
          box-shadow: 0 .5rem 2.5rem rgba(255, 255, 255, 0.25);
          &:after{
            background-image: url('/img/characterSelect/close-hover.svg');
            transition: .2s;
          }
        }
      }
    }
    &>div{
      &:first-child{
        padding-right: 2rem;
        margin-right: 2rem;
        border-right: .1rem solid rgba(255, 255, 255, 0.1);
        .info{
          align-items: flex-start;
        }
      }
      &:last-child{
        .info{
          align-items: flex-end;
        }
      }
    }
  }
  .characters-switch{
    display: flex;
    justify-content: center;
    width: 100%;
    margin-left: 1.625rem;
  }
}
</style>

<style lang="scss">
.character-select{
  .btn{
    display: flex;
    align-items: center;
    justify-content: center;
    transition: .2s;
    &:hover{
      transition: .2s;
    }
  }
}
</style>
