<template>
  <div :class="[{active: currentContract && item.ContractName === currentContract.ContractName}, 'contract-item']">
    <div 
      class="contract-item__img"
      :style="{backgroundImage: `url(/img/optionsMenu/programTab/contracts/${item.Image})`}"
    ></div>
    <div 
      class="contract-item__date"
      v-if="contractState && contractState.InProgress"
    >{{contractState.ExpirationDate.toLocaleString("ru", options)}}</div>
    <div class="contract-item-title">{{item.Name}}</div>
    <div 
      class="contract-item__btn"
      @click="setCurrentContract"
    >{{loc('options_program_19')}}</div>
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
export default {
  name: 'ContractItem',
  data: function () {
    return {
      options: {
        month: 'long',
        day: 'numeric',
        hour: 'numeric',
        minute: 'numeric',
      },
    }
  },
  props: {
    item: Object,
    currentContract: Object
  },
  computed: {
    ...mapGetters('localization', ['loc']),
    ...mapState('optionsMenu', ['myContracts', 'familyContracts']),
    contractState() {
      if (this.item.ContractType === "Family")
        return this.familyContracts.find(c => c.ContractName == this.item.ContractName);
      if (this.item.ContractType === "Single")
        return this.myContracts.find(c => c.ContractName == this.item.ContractName);
      return null;
    }
  },
  methods: {
    setCurrentContract: function() {
      this.$emit('setCurrentContract', this.item)
    }
  }
}
</script>

<style lang="scss" scoped>
.contract-item{ 
  min-width: 12rem;
  height: 17rem;
  border: .1rem solid rgba(255, 255, 255, 0.2);
  border-radius: .5rem;
  padding: 1rem 1.5rem 2.2rem 1.5rem;
  margin: .5rem;
  flex-wrap: wrap;
  position: relative;
  transition: .3s;
  &:hover, &.active{
    border-color: #FFFFFF;
  }
  &>div{
    z-index: 1;
  }
  &__img{
    background-size: cover;
    background-repeat: no-repeat;
    background-position: center;
    width: calc(100% - (1rem));
    height: calc(100% - (1rem));
    position: absolute;
    left: .5rem;
    top: .5rem;
    border-radius: .3rem;
    overflow: hidden;
    z-index: 0;
  }
  &__date{
    font-size: .8rem;
    line-height: .95rem;
    letter-spacing: 0.03em;
    color: #000000;
    background: #FFFFFF;
    border-radius: .25rem;
    padding: 0 .6rem 0 2rem;
    position: relative;
    display: flex;
    align-items: center;
    justify-content: center;
    position: absolute;
    left: 1rem;
    top: 1rem;
    height: 2rem;
    &:before{
      content: '';
      width: 1rem;
      height: 1rem;
      position: absolute;
      left: .6rem;
      background-size: contain;
      background-repeat: no-repeat;
      background-position: center;
      background-image: url('/img/optionsMenu/programTab/date.png');
    }
  }
  &-title{
    font-size: 2rem;
    line-height: 2.4rem;
    position: absolute;
    bottom: 2rem;
    left: 0;
    text-align: center;    
    width: 100%;
    padding: 0 .5rem;
  }
  &__btn{
    position: absolute;
    left: 10%;
    bottom: 0;
    transform: translateY(50%);
    background: #000000;
    border: .1rem solid rgba(255, 255, 255, 0.2);
    box-sizing: border-box;
    box-shadow: 0 .5rem 4rem rgba(0, 0, 0, 0.6);
    border-radius: 5rem;
    font-weight: bold;
    font-size: 1rem;
    line-height: 1rem;
    text-transform: uppercase;
    display: flex;
    align-items: center;
    justify-content: center;
    height: 3.2rem;
    width: 80%;
    transition: .3s;
    &:hover{
      background: #FFFFFF;
      color: #000;
      border-color: #fff;
    }
  }
}
</style>
