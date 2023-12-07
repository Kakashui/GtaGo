<template>
  <div class="modal">
    <close-button :styles="{ position: 'absolute', top: '2rem', right: '2rem' }" @close="$emit('close')" />
    <div class="modal__left">
      <div class="header">
        <div class="header__id">ID {{ property.business.number }}</div>
        <div class="header__btn" @click="changeName">{{ loc(isEdit ? 'mmain:stats:biz:sname' : 'mmain:stats:biz:chname') }}</div>
      </div>
      <div class="main">
        <div class="main__type">{{ typeName }}</div>
        <div class="main__title">
          <div class="text" v-if="!isEdit">{{ property.business.name }}</div>
          <textarea
            v-else
            v-model="property.business.name"
            class="area"
            v-focus
          ></textarea>
        </div>
        <div class="main__item">
          <div class="desc">{{loc('mmain:stats:biz:taxph')}}</div>
          <div class="value">{{ property.business.tax }}$</div>
        </div>
        <div class="main__item">
          <div class="desc">{{loc('mmain:stats:biz:taxcount')}}</div>
          <div class="value">{{ property.business.taxCount + '/' + taxMax }}</div>
        </div>
        <div class="main__item">
          <div class="desc">{{loc('mmain:stats:biz:cost')}}</div>
          <div class="value">${{ property.business.price }}</div>
        </div>
        <div class="main__sell" @click="sell">{{loc('mmain:stats:biz:cell')}}</div>
        <div class="main__sell" @click="sellToPlayer">{{loc('mmain:stats:biz:pcell')}}</div>
      </div>
    </div>
    <div class="modal__right">
      <div class="title">{{loc('mmain:stats:biz:prod')}}</div>
      <div class="products">
        <products-item
          v-for="(item, index) in property.business.products"
          :key="index"
          :item="item"
          @onChange="changePrice"
        />
      </div>
    </div>
  </div>
</template>

<script>
import { mapState, mapGetters, mapMutations } from 'vuex'
import CloseButton from '../../../common/CloseButton.vue'
import ProductsItem from './common/ProductsItem'

export default {
  name: 'BusinessModal',

  components: {
    ProductsItem,
    CloseButton
  },

  data: function() {
    return {
      isEdit: false,
      title: '',
      bizNames:[
        "24/7",
        "Petrol Station",
        "Premium Autoroom",
        "Luxor Autoroom",
        "Low Autoroom",
        "Motoroom",
        "Gun",
        "Clothes Shop",
        "Burger-Shot",
        "Tatto",
        "Barbershop",
        "Masks shop",
        "LS Customs",
        "Car wash",
        "Pet shop",
        "Super CarRoom",
        "Autorepair",
        "Rent car",
        "Car trader",
        "Casino",
        "Retro Autoroom",
        "JDM Autoroom",
        "New Autoroom",
        "New Autoroom",
        "New Autoroom",
        "New Autoroom",
        "New Autoroom",
        "Furniture",
        "Illegal Shop"
      ]
    }
  },

  computed: {
    ...mapState('optionsMenu', ['property', 'statistics']),
    ...mapGetters('localization', ['loc']),
    taxMax(){
      const days = this.statistics.premium ? 30 : 7;
      return this.property.business.price / 100 * 0.013 * 1.9 * 24 * days
      //return this.property.business.tax * days * 24;
    },
    typeName(){
      return this.bizNames[this.property.business.type] || 'noInfo'
    }
  },

  methods: {
    ...mapMutations('optionsMenu', ['setDialog']),
    setTitle: function(value) {
      this.title = value
    } ,
    sell(){
      window.mp.triggerServer("mmenu:biz:sell", this.property.business.number);
      this.$emit("close");
    },
    sellToPlayer(){
      this.setDialog({
          input: 'number',
          input2: 'number',
          callback: (val, val2)=>{
            val = parseInt(val);
            val2 = parseInt(val2);
            if(isNaN(val) || isNaN(val2) || val2 < this.property.business.price / 2){
              window.setData('notifyList/notify', {type: 1, position:2, message: "mmain:frac:dialog:data:wrong", time: 3000});
            }else{
             window.mp.triggerServer('mmenu:biz:sell:player', val, val2);
            }
          },
          value: '',
          placeholder: 'mmain:stats:biz:pcell:pl',
          placeholder2: 'mmain:stats:biz:pcell:pl2',
          tittle: `mmain:stats:biz:pcell:tit`,
          subtittle: 'mmain:stats:biz:pcell:sub',
          bg: undefined
      });
    },
    changePrice(name, price){
      window.mp.triggerServer("mmenu:product:price:set", this.property.business.number, name, price);
    },
    changeName(){
      if(this.isEdit){
         window.mp.triggerServer("mmenu:biz:name:set", this.property.business.number, this.property.business.name);
      }
      this.isEdit = !this.isEdit;
    }
  },
  
  directives: {
    focus: {
      inserted: function (el) {
        el.focus()
      }
    }
  },

  mounted: function() {
    this.setTitle(this.property.business.title)
  }
}
</script>

<style lang="scss" scoped>
.modal {
  width: 80rem;
  height: 46rem;
  background: center / cover no-repeat url("/img/optionsMenu/statisticsTab/businessModal/bg.png");
  position: absolute;
  top: 0;
  left: 0;
  bottom: 0;
  right: 0;
  margin: auto;
  box-shadow: 0px 0px 8.5rem rgba(255, 51, 128, 0.1);
  display: flex;
  &__left {
    display: flex;
    flex-direction: column;
    padding: 2rem 0 0 3.2rem;
    margin: 0 2.5rem 0 0;
    .header {
      display: flex;
      justify-content: space-between;
      width: 29rem;
      margin: 0 0 1.25rem 0;
      &__id {
        font-size: 1.1rem;
        letter-spacing: 0.03em;
        color: #FFFFFF;
        padding: 0 0 0.65rem 0;
        display: block;
        position: relative;
        &::after {
          content: "";
          position: absolute;
          bottom: 0;
          left: 0;
          height: 0.1rem;
          width: 3.5rem;
          background: #B6D300;
          box-shadow: 0px 0.5rem 2.5rem rgba(168, 195, 2, 0.6);
          display: block;
        }
      }
      &__btn {
        width: 10rem;
        height: 2.5rem;
        border: 0.1rem solid rgba(255, 255, 255, 0.1);
        font-size: 0.9rem;
        color: rgba(255, 255, 255, 0.5);
        letter-spacing: 0.03em;
        display: flex;
        align-items: center;
        justify-content: center;
      }
    }
    .main {
      letter-spacing: 0.03em;
      color: #fff;
      &__type {
        font-size: 1.3rem;
        margin: 0 0 0.4rem 0;
      }
      &__title {
        width: 29rem;
        margin: 0 0 1.25rem 0;
        .text {
          font-size: 4rem;
          line-height: 4rem;
          font-weight: bold;
          padding: 0 0 0.75rem 0;
        }
        .area {
          padding: 0.4rem 0 0 0;
          font-size: 4rem;
          line-height: 4rem;
          font-weight: bold;
         text-transform: uppercase;
          width: 100%;
          background: transparent;
          border: none;
          color: #fff;
          overflow: hidden;
          resize: none;
          outline: none;
          letter-spacing: 0.0075em;
        }
      }
      &__item {
        display: flex;
        flex-direction: column;
        margin: 0 0 1rem 0;
        .desc {
          font-size: 0.9rem;
          color: rgba(255, 255, 255, 0.4);
        }
        .value {
          font-size: 1.8rem;
          line-height: 1.8rem;
        }
      }
      &__sell {
        width: 11rem;
        height: 2.5rem;
        border: 0.1rem solid #B6D300;
        font-size: 0.9rem;
       text-transform: uppercase;
        background: transparent;
        letter-spacing: 0.03em;
        color: #fff;
        display: flex;
        align-items: center;
        justify-content: center;
        margin: 1.5rem 0;
      }
    }
  }
  &__right {
    display: flex;
    flex-direction: column;
    letter-spacing: 0.03em;
    padding: 3rem 0 0 0;
    .title {
      font-size: 2.5rem;
      line-height: 2.5rem;
      padding: 0 0 0 1rem;
      color: #FFFFFF;
      margin: 0 0 1.5rem 0;
    }
    .products {
      height: 37rem;
      overflow-x: scroll;
      &::-webkit-scrollbar {
        display: none;
      }
    }
  }
}
</style>
