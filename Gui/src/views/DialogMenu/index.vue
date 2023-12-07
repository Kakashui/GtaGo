<template>
  <transition name="scale-in">
    <div class="dialog-menu">
      <img src="img/dialogMenu/bgAddition.png" class="dialog-menu__img">
      <span class="dialog-menu__header">{{ loc(Body.Header) }}</span>
      <div :class="[{ column: isColumnClass },'dialog-menu__actions']">
        <dialog-menu-button
          v-for="(button, index) in Body.Buttons"
          :key="index"
          :button="{ text: button.Name, icon: button.Icon, index, isColumn: isColumnClass }"
        />
      </div>
    </div>
  </transition>
</template>

<script>
import { mapState, mapGetters } from 'vuex'
import DialogMenuButton from './DialogMenuButton'

export default {
  name: 'DialogMenu',

  components: { DialogMenuButton },

  computed: {
    ...mapGetters('localization', ['loc']),
    ...mapState('dialogMenu', ['Body']),

    isColumnClass: function() {
      return this.Body.Buttons.length > 2 ? true : false
    }
  }
}
</script>

<style lang="scss" scoped>
.dialog-menu {
  text-transform: uppercase;
  color: #fff;
  position: absolute;
  top: 0;
  right: 0;
  bottom: 0;
  left: 0;
  margin: auto;
  z-index: 99;
  width: 26.5rem;
  height: fit-content;
  padding: 4.3rem 3.75rem 3rem 3.75rem;
  border-radius: 0.3rem;
  background: center / cover no-repeat url("/img/dialogMenu/bg.png") rgba(2, 3, 16, 0.8);
  overflow: hidden;
  &__img {
    position: absolute;
    top: 0;
    right: -2%;
    width: 5.3rem;
    height: auto;
  }
  &__header {
    font-size: 1.5rem;
    line-height: 1.6rem;
    letter-spacing: 0.1em;
    text-align: center;
    display: block;
    text-transform: uppercase;
    font-weight: bold;
  }
  &__actions {
    display: flex;
    justify-content: space-between;
    margin: 1.5rem 0 0 0;
    z-index: 2;
    &.column {
      flex-direction: column;
    }
  }
}
.scale-in-enter-active, .scale-in-leave-active {
  transition: all .4s;
}
.scale-in-enter, .scale-in-leave-to {
  transform: scale(0.2);
  opacity: 0;
}
</style>
