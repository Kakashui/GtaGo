<template>
  <div class="radio__container">
    <div class="radio__screen">
      <div class="screen__input" >
        <span class="input__item" v-for="(item, index) in inputItems" :key="index">{{ item }}</span>
      </div>
      <img
        src="/img/radio/communication_level.png"
        alt="level"
        class="screen__level"
      >
      <span class="screen__message" v-if="isMuted">Muted</span>
      <span class="screen__message_conn" v-if="isConnected">Connected</span>
    </div>
    <div class="radio__actions">
      <button @click="mute" class="actions__button actions__button-mute">Mute</button>
      <button @click="call" class="actions__button actions__button-ok">Ok</button>
      <button @click="clear" class="actions__button actions__button-clear">Clear</button>
    </div>
    <div class="radio__keyboard">
      <button
        v-for="(item, index) in keyboardValues"
        :key="index"
        class="keyboard__button"
        @click="writeSymbol(item)"
      >{{ item }}</button>
    </div>
    <button @click="exit" class="radio__exit">Exit</button>
  </div>
</template>

<script>
import { mapState, mapMutations } from 'vuex'

export default {
  name: 'Radio',

  data: function () {
    return {
      currentIndex: 0
    }
  },

  computed: {
    ...mapState('radio', [
      'inputItems',
      'keyboardValues',
      'isMuted',
      'isConnected'
    ])
  },

  methods: {
    ...mapMutations('radio', [
      'appendInputItem',
      'resetInputItems',
      'removeInputFirstItem'
    ]),

    writeSymbol: function (value) {
      const item = {
        index: this.inputItems.length - 1,
        value
      }

      if (this.inputItems.length - 1 === '-') {
        this.appendInputItem(item)
        this.currentIndex++
      } else if (this.currentIndex === 6) {
        return
      } else {
        this.removeInputFirstItem()
        this.appendInputItem(item)
        this.currentIndex++
      }
    },

    call: function () {
      const rightArray = this.inputItems.join('')

      if (this.currentIndex !== 6) {
        return
      }
      
      window.mp.trigger('radio:setWave', rightArray)
    },

    mute: function () {
      window.mp.trigger('radio:mute')
    },

    clear: function () {
      window.mp.trigger('radio:clearWave')
      this.resetInputItems()
      this.currentIndex = 0
    },

    exit: function () {
      window.mp.trigger('radio:close')
    }
  }
}
</script>

<style lang="scss" scoped>
.radio__container {
  background-image: url('../../../public/img/radio/radio_bg.png');
  background-repeat: no-repeat;
  background-position: center;
  background-size: cover;
  width: 14.7rem;
  height: 28.4rem;
  position: relative;
  .radio__screen {
    position: absolute;
    overflow: hidden;
    left: .85rem;
    top: 7rem;
    background-image: linear-gradient(12deg, #1e4711 0%, #2d6f19 100%);
    border-radius: .5rem;
    height: 4.25rem;
    width: 12.5rem;
    color: #c2c2c2;
    .screen__input {
      display: inline-flex;
      width: 100%;
      height: 100%;
      background: transparent;
      align-items: center;
      justify-content: center;
      color: #c2c2c2;
      .input__item {
        font-size: 1.2rem;
        display: block;
        height: 1.4rem;
        width: .8rem;
        text-align: center;
      }
    }
    .screen__message {
      position: absolute;
      top: .3rem;
      right: .5rem;
      text-transform: lowercase;
      font-size: .8rem;
    }
    .screen__message_conn {
      position: absolute;
      top: .3rem;
      left: 2rem;
      text-transform: lowercase;
    }
    .screen__level {
      position: absolute;
      top: .3rem;
      left: .5rem;
      width: .6rem;
    }
  }
  .radio__actions {
    position: absolute;
    top: 12.9rem;
    left: .85rem;
    width: 12.5rem;
    height: 3.35rem;
    box-shadow: inset 0 0 8px #1a1a1a;
    border-radius: .5rem;
    background-color: #303030;
    display: flex;
    justify-content: space-around;
    align-items: center;
    .actions__button {
      width: 28%;
      height: 50%;
      border-radius: 3px;
      font-size: .85rem;
      font-weight: 700;
      text-transform: uppercase;
      letter-spacing: 0.64px;
      color: #ffffff;
      &-mute {
        background-color: #ff0000;
      }
      &-ok {
        background-color: #2d6f19;
      }
      &-clear {
        background-color: #FF7000;
      }
    }
  }
  .radio__keyboard {
    height: 8.3rem;
    width: 9.8rem;
    display: flex;
    flex-wrap: wrap;
    justify-content: space-around;
    position: absolute;
    bottom: 2rem;
    left: 2.1rem;
    .keyboard__button {
      width: 26%;
      margin: 0 0 .7rem 0;
      font-size: .95rem;
      color: #ffffff;
      background-image: linear-gradient(12deg, #23292e 0%, #28333c 99%, #28333c 100%);
    }
  }
  .radio__exit {
    position: absolute;
    transform: translate(-50%, -50%);
    bottom: 0;
    left: 7rem;
    background: transparent;
    background-image: linear-gradient(12deg, #23292e 0%, #28333c 99%, #28333c 100%);
    color: #ff0000;
    padding: .2rem 1rem;
    text-transform: uppercase;
    font-weight: 700;
    letter-spacing: 1.6px;
    font-size: .95rem;
    border-radius: 3px;
  }
}
</style>
