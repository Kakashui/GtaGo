<template>
  <div 
    class="btn-back"
    @click="$emit('click')"
  >{{loc('bank:menu:52')}}</div>
</template>

<script>
import { mapGetters } from 'vuex'
export default {
  name: 'BtnBack',
  computed: {
    ...mapGetters('localization',['loc'])
  }
}
</script>

<style lang="scss" scoped>
.btn-back{
  display: flex;
  align-items: center;
  position: relative;
  font-weight: normal;
  font-size: 1rem;
  line-height: 1.2rem;
  letter-spacing: 0.03em;
  color: rgba(94, 55, 176, 0.4);
  transition: .3s;
  &:hover{
    color: rgba(94, 55, 176, 0.8);
  }
  &:before{
    content: '';
    width: 1.8rem;
    height: 1.8rem;
    margin-right: .7rem;
    background-size: contain;
    background-repeat: no-repeat;
    background-position: center;
    background-image: url('/img/bank/arr-back.png');
  }
}
</style>
