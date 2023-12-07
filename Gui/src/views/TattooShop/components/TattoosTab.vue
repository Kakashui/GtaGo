<template>
  <div class="tattoos-tab">
    <button class="tattoos-tab__btn" @click="back">
      <img
        src="img/tattooShop/arrow.svg"
        alt="arrow"
        class="icon"
      >
      <span class="text">{{loc('tattoo_shop_17')}}</span>
    </button>
    <div class="tattoos-tab__list">
      <tattoo-item
        v-for="(tattoo, index) in items"
        :key="index"
        :tattoo="tattoo"
        :index="index"
        :selected="current == index"
        :show="isShow(tattoo)"
        @onTattooSelect="selectTattoo"
      />
    </div>
    <div class="tattoos-tab__actions">
      <main-button
        v-for="(button, index) in buttons"
        :key="index"
        :button="button"
      />
    </div>
  </div>
</template>

<script>
import { mapState, mapGetters } from 'vuex'
import TattooItem from './TattooItem'
import MainButton from './MainButton'

export default {
  name: 'TattoosTab',

  props: {
    items: Array,
    category: String
  },

  components: { TattooItem, MainButton },

  data: function() {
    return {
      current: 0,
      buttons: [
        { text: 'tattoo_shop_18', class: 'close', event: this.toClose },
        { text: 'tattoo_shop_19', class: 'act', event: this.buy },
      ]
    }
  },

  computed: {
    ...mapState('tattooShop', ['tattoos', 'gender', 'currentTattoos']),
    ...mapGetters('localization', ['loc']),
    // allTattoosList(){
    //   const list = [];
    //   for (const key in this.tattoos) {
    //     this.tattoos[key].forEach(tattoo => {
    //       const overlay = this.gender ? tattoo.Male : tattoo.Female;
    //       list.push({dict: tattoo.Dict, overlay, slots: tattoo.Slots} );
    //     });        
    //   }
    //   return list;
    // },
    // occupied(){
    //   const occupied = [];
    //   this.currentTattoos.forEach(t=>{
    //     const config = this.allTattoosList.find(c=>c.dict === t.Collection && c.overlay === t.Overlay);
    //     if(config !== null) occupied.push(config.slots)
    //   })
    //   return occupied;
    // }
  },

  methods: {
    
    toClose: function() {
      window.mp.trigger('tattoo:close');
    },

    buy: function() {
      if (this.items[this.current]) {
        const config = this.items[this.current];
        if(this.currentTattoos.find(c=>c.Collection === config.Dict && (c.Overlay === config.Male || c.Overlay === config.Female))) {
          window.setData('notifyList/notify', { type: 1, position: 0, message: "tattoo:shop:already", time: 3000 })
          return;
        }
        window.mp.trigger('tattoo:buy', this.category, config.Id);
      }
    },

    back: function() {
      this.$emit("onBack")
    },

    selectTattoo(index){
      this.current = index;
      if(this.items[this.current]);
      {
        const config = this.items[this.current];
        const name = this.gender ? config.Male : config.Female;
        if(name === 0) return;
        window.mp.trigger("tattoo:select", config.Dict, name, JSON.stringify(config.Slots));
      }
       
    },
    isShow(tattoo){
        return (this.gender ? tattoo.Male : tattoo.Female) !== 0;
    },

    // isTattooAvailable(tattoo){
    //   let result = true;
    //   tattoo.slots.forEach(s=>{
    //     if(this.occupied.findIndex(o=>o === s) !== -1){
    //       result = false;
    //       return;
    //     }
    //   })
    //   return result;
    // }
  }
}
</script>

<style lang="scss" scoped>
@keyframes shake {
  25% {
    transform: translateX(30%);
  }
  50% {
    transform: translateX(15%);
  }
  75% {
    transform: translateX(30%);
  }
  100% {
    transform: translateX(0);
  }
}
.tattoos-tab {
  padding: 0.4rem 0 0 0;
  display: flex;
  flex-direction: column;
  &__btn {
    background: transparent;
    display: flex;
    align-items: center;
    align-self: flex-start;
    &:hover .icon {
      animation: shake 0.5s;
    }
    .icon {
      width: 1rem;
      height: 1rem;
    }
    .text {
      text-transform: uppercase;
      color: #fff;
      margin: 0 0 0 0.55rem;
      font-weight: 300;
    }
  }
  &__list {
    margin: 0.7rem 0 0 0;
    display: grid;
    grid-template-columns: repeat(2, 10.05rem);
    grid-gap: 0.85rem 1.2rem;
    height: 34.65rem;
    overflow-y: auto;
    padding: 0 0.55rem 0 0;
    &::-webkit-scrollbar-track {
      background-color: transparent;
    }
    &::-webkit-scrollbar {
      width: 0.2rem;
      background-color: transparent;
    }
    &::-webkit-scrollbar-thumb {
      background: rgba(255, 255, 255, 0.15);
      border-radius: 0.2rem;
    }
  }
  &__actions {
    display: flex;
    justify-content: space-between;
    align-self: flex-end;
    width: 15.2rem;
    margin: 1.05rem 0.75rem 0 0;
  }
}
</style>
