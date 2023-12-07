<template>
  <div 
    :class="[{army: type === 2}, {fbi: type === 1},'footer-item']"
    @click="pushCode(item - 1)"
  >
    <div class="icon" :style="{backgroundImage: 'url(/img/personalDigitalAssistant/star-' + type + '.svg)'}"></div>
    <div class="text">{{loc('Pda_1')}}-{{item - 1}}</div>
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
export default {
  name: 'FooterItem',

  props: {
    item: Number
  },

  computed: {
    ...mapState('personalDigitalAssistant', ['type']),
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    pushCode: function(item) {
      window.mp.trigger('pda:pushCode', item)
    }
  }
}
</script>

<style lang="scss" scoped>
.footer-item{
  background: #111111;
  border-radius: .4rem;
  display: flex;
  justify-content: center;
  align-items: center;
  width: 10.3rem;
  height: 3.1rem;
  transition: .2s;
  &.fbi{
    .text{
      color: #FB7712;
    }
  }
  &.army{
    .text{
      color: #8DA997;
    }
  }
  &:hover{
    transform: scale(1.1);
    transition: .2s;
  }
  &:active{
    transform: scale(1.05);
  }
  .icon{
    background-size: contain;
    background-repeat: no-repeat;
    background-position: center;
    width: 1.7rem;
    height: 1.7rem;
    margin-right: .5rem;
  }
  .text{
    font-size: 1rem;
    line-height: 1.15rem;
    text-transform: uppercase;
    color: #FFD130;
  }
}
</style>
