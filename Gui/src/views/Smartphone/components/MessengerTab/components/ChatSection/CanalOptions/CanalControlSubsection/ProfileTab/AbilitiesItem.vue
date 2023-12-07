<template>
  <div class="abilities-item">
    <span class="abilities-item__title">{{ loc(item.title) }}</span>
    <label :class="[labelClass, 'abilities-item__label']" @click="handleClick">
      <input
        type="checkbox"
        class="checkbox"
        v-model="item.value"
      >
      <span class="span"></span>
    </label>
  </div>
</template>

<script>
import { mapGetters, mapMutations } from 'vuex'

export default {
  name: 'AbilitiesItem',

  props: {
    item: Object
  },

  computed: {
    labelClass: function() {
      return this.item.value ? 'active' : 'inactive'
    },
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    ...mapMutations('smartphone/messagePage', ['setMessengerCanalProfileAbility']),

    handleClick: function() {
      this.setMessengerCanalProfileAbility({ key: this.item.key, value: !this.item.value });
    }
  }
}
</script>

<style lang="scss" scoped>
.abilities-item {
  display: flex;
  align-items: center;
  justify-content: space-between;
  height: 2.45rem;
  border-bottom: 1px solid rgba(195, 195, 195, 0.35);
  padding: 0 0.75rem 0 0;
  &:last-child {
    border: none;
    .abilities-item__label.inactive {
      background:#EE443A;
    }
  }
  &__title {
    font-size: 0.7rem;
    font-weight: 300;
    line-height: 0.8rem;
    color: #020202;
  }
  &__label {
    flex: 0 0 2.5rem;
    height: 1.5rem;
    border-radius: 5rem;
    background: rgba(120, 120, 128, 0.32);
    position: relative;
    transition: all .15s;
    &.active {
      background: #32D74B;
    }
    &.inactive {
      background: rgba(120, 120, 128, 0.32);
    }
    .checkbox {
      position: absolute;
      opacity: 0;
      z-index: -1;
      appearance: none;
    }
    .span {
      width: 1.3rem;
      height: 1.3rem;
      box-shadow: 0px 3px 8px rgba(0, 0, 0, 0.15), 0px 3px 1px rgba(0, 0, 0, 0.06);
      background-color: #fff;
      border-radius: 50%;
      transform: translate(-50%, -50%);
      position: absolute;
      top: 50%;
      left: 30%;
      transition: left .15s;
    }
    .checkbox:checked + .span {
      left: 70%;
    }
  }
}
</style>
