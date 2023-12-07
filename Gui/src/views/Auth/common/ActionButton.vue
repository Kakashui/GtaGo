<template>
  <button
    :style="styles"
    class="btn"
    @click="$emit('event')"
  >{{ loc(text) }}</button>
</template>

<script>
import { mapGetters } from 'vuex'
export default {
  name: 'ActionButton',  
  computed:{
    ...mapGetters('localization', ['loc'])
  },
  props: {
    text: String,
    styles: Object
  }
}
</script>

<style lang="scss" scoped>
.btn {
  width: 20rem;
  height: 4.5rem;
  margin: 0 0 0.75rem 0;
 text-transform: uppercase;
  font-size: 1.5rem;
  font-weight: bold;
  color: #fff;
  border-radius: 0.4rem;
  box-shadow: 0px 0.5rem 4rem rgba(209, 233, 62, 0.3), inset 0px -0.15rem 0px #98B300;
  background: center / cover no-repeat url("/img/auth/btnBg.png"), linear-gradient(268.14deg, #B0C724 0%, #D5EC45 23.77%, #B6D300 100%);
  transition: all 0.15s;
  &:hover {
    box-shadow: none;
  }
}
</style>
