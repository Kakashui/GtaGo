<template>
  <div class="bank-credit-property bank-modal">
    <div class="bank-credit-property__wrap">
      <div 
        class="btn-close"
        @click="$emit('closeModal')"
      ></div>
      <div class="bank-modal-title">{{dataModal.familyProperty ? loc('bank:menu:179') : loc('bank:menu:180')}}</div>
      <div class="bank-credit-property__nav">
        <div 
          :class="[{ active: nav.key === currentNav }, 'bank-credit-property__nav-item']"
          v-for="nav in navList"
          :key="nav.key"
          @click="setCurrentNav(nav.key)"
        >{{loc(nav.text)}}</div>
      </div>
      <div class="bank-credit-property__list">
        <div 
          class="bank-credit-property__list-item"
          v-for="item in filteredList"
          :key="item.id"
        >
          <div class="bank-credit-property__list-item-name">{{item.name}}</div>
          <div class="bank-credit-property__list-item-desc"><span>{{loc('bank:menu:181')}} </span>{{item.price > 0 ? `$${item.price.toLocaleString('ru')}` : '------'}}</div>
          <div 
            class="bank-credit-property__list-item-desc"
            :style="item.isPledged ? {color: '#EE443A'} : {color: '#B6D300'}"
          >{{item.isPledged ? loc('bank:menu:182') : loc('bank:menu:183')}}</div>
          <div class="bank-credit-property__btns">
            <div 
              class="bank-credit-property-btn"
              @click="checkStatusProperty(item)"
            >{{loc('bank:menu:184')}}</div>
            <div 
              class="bank-credit-property-btn-main"
              @click="selectCreditProperty(item)"
            >{{loc('bank:menu:185')}}</div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapMutations, mapState, mapGetters } from 'vuex'
export default {
  name: 'BankCreditProperty',
  props: {
    dataModal: Object
  },
  data() {
    return {
      currentNav: '',
      navList: [
        {
          key: 'Vehicle',
          text: 'bank:menu:186',
        },
        {
          key: 'Business',
          text: 'bank:menu:187',
        },
        {
          key: 'House',
          text: 'bank:menu:188',
        },
      ]
    }
  },
  computed: {
    ...mapState('bank/credit', ['propertyList']),
    ...mapGetters('localization',['loc']),
    filteredList() {
      let list = [...this.propertyList]
      return list.filter(element => this.dataModal.familyProperty === element.familyProperty && element.type === this.currentNav)
    },
  },
  methods: {
    ...mapMutations('bank/credit', ['setSelectedCreditProperty']),
    setCurrentNav(key) {
      this.currentNav = key
    },
    checkStatusProperty(item) {
      console.log(item)
      // window.mp.triggerServer('bank:checkStatusProperty', item.type, item.id)
    },
    selectCreditProperty(item) {
      if (item.price > 0 && !item.isPledged)
      {
        this.setSelectedCreditProperty(item)
        this.$emit('closeModal')
      }
    }
  },
  mounted() {
    this.currentNav = this.navList[0].key
  }
}
</script>

<style lang="scss">
.bank-credit-property{
  color: #fff;
  font-weight: normal;
  .bank-modal-title{
    margin-bottom: 1rem;
    color: #fff;
  }
  &__wrap{
    background-color: transparent;
    padding: 0;
    width: 20.5rem;
    position: relative;
    max-height: calc(100vh - (9rem));
  }
  &__nav{
    display: flex;
    align-items: center;
    justify-content: space-between;
    padding: .25rem;
    background: rgba(3, 12, 32, 0.7);
    border-radius: 5rem;
    height: 3rem;
    margin-bottom: 1rem;
    &-item{
      display: flex;
      align-items: center;
      justify-content: center;
      width: 100%;
      height: 100%;
      background-color: transparent;
      font-size: 1rem;
      line-height: 1.2rem;
      letter-spacing: 0.03em;
      border-radius: 5rem;
      color: rgba(255, 255, 255, 0.5);
      transition: .2s;
      &:hover{
        color: #fff;
      }
      &.active{
        background-color: #fff;
        color: #000000;
      }
    }
  }
  &__list{
    overflow-y: auto;
    max-height: calc(100vh - (12rem));
    padding-right: .65rem;
    min-width: 100%;
    margin-right: -.65rem;
    &::-webkit-scrollbar{
      width: .2rem;
      background-color: transparent;
      &-thumb{
        background: rgba(255, 255, 255, 0.2);
        border-radius: .3rem;
      }
    }
    &-item{
      display: flex;
      flex-direction: column;
      padding: 1.5rem;
      background: rgba(3, 12, 32, 0.7);
      border-radius: .6rem;
      margin-bottom: .5rem;
      &-name{
        font-size: 1.5rem;
        line-height: 1.8rem;
        letter-spacing: 0.03em;
        margin-bottom: .7rem;
      }
      &-desc{
        font-size: 1rem;
        line-height: 1.2rem;
        white-space: pre;
        margin-bottom: .4rem;
        span{
          color: rgba(255, 255, 255, 0.5);
        }
      }
    }
  }
  &__btns{
    height: 2.5rem;
    display: flex;
    align-items: center;
    justify-content: space-between;
    margin-top: .5rem;
  }
  &-btn, &-btn-main{
    border-radius: .2rem;
    height: 100%;
    padding: 0 1.7rem;
    display: flex;
    align-items: center;
    justify-content: center;
    transition: .2s;
  }
  &-btn{
    border: .1rem solid rgba(255, 255, 255, 0.3);
    &:hover{
      border-color: #fff;
      background-color: rgba(255, 255, 255, 0.1);
    }
  }
  &-btn-main{
    background-color: transparent;
    border: .1rem solid #7E55D4;
    &:hover{
      background-color: #7E55D4;
    }
  }
}
</style>
