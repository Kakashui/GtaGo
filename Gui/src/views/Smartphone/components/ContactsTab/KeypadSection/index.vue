<template>
  <div class="keypad-section">
    <span class="keypad-section__number">{{ currentNumber }}</span>
    <button
      class="keypad-section__btn"
      v-if="numbers.length > 0"
      @click="setContactPopupData(true, 'add', currentNumber)"
    >{{loc('phone:keypad:add')}}</button>
    <div class="keypad-section__keys">
      <button
        class="item"
        v-for="item in keys"
        :key="item"
        @click="setKey(item)"
      >{{ item }}</button>
      <button
        :disabled="numbers.length < 10 && numbers.length > 3"
        class="call"
        @click="call"
      >
        <img
          src="img/hud/smartphone/contactsTab/phone1.svg"
          alt="phone"
          class="call__icon"
        >
      </button>
      <div class="backtrack">
        <input
          class="backtrack__icon"
          type="image"
          src="img/hud/smartphone/contactsTab/backtrack.svg"
          @click="removeKey(numbers)"
        >
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapMutations, mapState } from 'vuex'

export default {
  name: 'KeypadSection',

  props: {
    setContactPopupData: Function
  },

  data: function() {
    return {
      numbers: []
    }
  },

  computed: {
    ...mapState('smartphone', ['configuration']),

    currentNumber: function() {
      return this.numbers.join('')
    },

    keys: function() {
      const arr = Array.from({length: 9}, (_, i) => i + 1)
      arr.push(0)

      return arr
    },
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    ...mapMutations('smartphone', ['setCurrentTab']),

    setKey: function(key) {
      if (this.numbers.length === 0 && key !== 1)
        this.numbers.push(1);
      
      if (this.numbers.length < 10) this.numbers.push(key)
    },

    removeKey: function(array) {
      array.splice(-1, 1)
    },

    call: function() {
      const intNumber = parseInt(this.currentNumber);
      if (intNumber === this.configuration.Number) {
        return;
      }

      window.setData('smartphone/setCallData', { destination: intNumber, status: 'outcome' })
      window.setData('smartphone/setCurrentTab', { key: 'CallTab' })
    }
  }
}
</script>

<style lang="scss" scoped>
@keyframes fade {
  from {
    opacity: 0;
  }
  to {
    opacity: 1;
  }
}
.keypad-section {
  padding: 0 1.75rem 2.5rem 1.75rem;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: flex-end;
  height: 100%;
  width: 100%;
  animation: fade 0.4s;
  &__number {
    font-size: 2rem;
  }
  &__btn {
    background: transparent;
    font-size: 0.7rem;
    color: #007AFF;
    &:hover {
      text-decoration: underline;
    }
  }
  &__keys {
    margin: 2rem 0 0 0;
    display: grid;
    grid-template-columns: repeat(3, 3.2rem);
    grid-gap: 0.5rem 1.05rem;
    .item {
      width: 3.2rem;
      height: 3.2rem;
      background: #F3F2F7;
      font-size: 1.6rem;
      border-radius: 50%;
      &:hover {
        background: #DFDEE3;
      }
    }
    .call {
      height: 3.2rem;
      background: #34C85A;
      display: flex;
      align-items: center;
      justify-content: center;
      border-radius: 50%;
      transition: all .15s;
      &:hover {
        transform: scale(1.05);
      }
      &__icon {
        width: 1rem;
        height: 1rem;
      }
    }
    .backtrack {
      background: transparent;
      display: flex;
      align-items: center;
      justify-content: center;
      &:hover {
        transform: scale(1.1);
      }
      &__icon {
        width: 1.5rem;
        height: 1.1rem;
      }
    }
  }
}
</style>
