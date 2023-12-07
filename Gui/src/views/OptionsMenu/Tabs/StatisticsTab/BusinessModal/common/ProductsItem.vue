<template>
  <div class="item">
    <div class="item__title">{{ item.title }}</div>
    <div class="item__count">
      <progress-circle
        :styles="{ margin: '0 0.75rem 0 0' }"
        :maxVal="item.maxCount"
        :curVal="item.curCount"
      />
      <div class="text">
        <div class="text__desc">{{loc('mmain:stats:biz:prod:count')}}</div>
        <div class="text__value">{{ item.curCount + '/' + item.maxCount }}</div>
      </div>
    </div>
    <div class="item__price">
      <div class="text">
        <div class="text__desc">{{loc('mmain:stats:biz:prod:price')}}</div>
        <div class="text__field">
          <div v-if="!isEdit" class="value">{{ item.price.toLocaleString() }}$</div>
          <input
            v-else
            type="text"
            class="input"
            v-model="price"
            v-focus
          >
        </div>
      </div>
      <button class="btn">
        <img
          :src="`img/optionsMenu/statisticsTab/businessModal/${isEdit ? 'check' : 'pencil'}.svg`"
          class="btn__icon"
          @click="changePrice"
        >
      </button>
    </div>
  </div>
</template>

<script>
import ProgressCircle from './ProgressCircle'
import { mapGetters } from 'vuex'


export default {
  name: 'ProductsItem',

  components: {
    ProgressCircle
  },

  computed:{
    ...mapGetters('localization', ['loc'])
  },
  props: {
    item: Object
  },

  data: function() {
    return {
      isEdit: false,
      price: null
    }
  },

  methods: {
    setPrice: function(value) {
      this.price = value
    },
    changePrice(){
      if(this.isEdit) {
        const sum = parseInt(this.price)
        if(!isNaN(sum)) 
          this.$emit("onChange", this.item.title, sum);
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
    this.setPrice(this.item.price)
  }
}
</script>

<style lang="scss" scoped>
.item {
  display: flex;
  align-items: center;
  height: 6rem;
  width: 45rem;
  letter-spacing: 0.03em;
  padding: 0 4.5rem 0 2rem;
  background: rgba(0, 0, 7, 0.6);
  &:not(:last-child) {
    margin: 0 0 0.1rem 0;
  }
  &__title {
    font-size: 1.2rem;
    font-weight: bold;
    line-height: 1.2rem;
    color: #FFFFFF;
    width: 15rem;
    margin: 0 2.5rem 0 0;
  }
  &__count {
    display: flex;
    align-items: center;
    .text {
      display: flex;
      flex-direction: column;
      &__desc {
        font-size: 0.9rem;
        line-height: 0.9rem;
        color: rgba(255, 255, 255, 0.5);
      }
      &__value {
        font-size: 1.1rem;
        letter-spacing: 0.03em;
        color: #FFFFFF;
      }
    }
  }
  &__price {
    margin: 0 0 0 auto;
    display: flex;
    align-items: flex-end;
    .text {
      display: flex;
      flex-direction: column;
      align-items: flex-end;
      margin: 0 0.7rem 0 0;
      &__desc {
        font-size: 0.9rem;
        color: rgba(255, 255, 255, 0.5);
        letter-spacing: 0.03em;
      }
      &__field {
        width: fit-content;
        .value {
          font-size: 1.1rem;
          line-height: 1.5rem;
          color: #fff;
          letter-spacing: 0.03em;
        }
        .input {
          background: transparent;
          width: 5rem;
          height: 100%;
          font-size: 1.1rem;
          text-align: right;
          color: #fff;
         text-transform: uppercase;
        }
      }
    }
    .btn {
      background: transparent;
    }
  }
}
</style>
