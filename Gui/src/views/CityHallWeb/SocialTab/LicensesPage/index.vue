<template>
  <div class="page">
    <div class="page__title">{{loc('cityHallWeb_126')}}</div>
    <div class="page__desc">{{loc('cityHallWeb_127')}}</div>
    <menu-drop
      :dropList="list"
      :currentItem="currentLicense"
      :setCurrentItem="setCurrentItem"
    />
    <div class="page__desc">{{loc('cityHallWeb_128')}}</div>
    <textarea
      :placeholder="loc('cityHallWeb_36')"
      v-model="currentDesc"
    ></textarea>
    <Payment
      :currentPayment="currentPayment"
      :setCurrentPayment="setCurrentPayment"
    />
    <div 
      v-if="currentLicense && currentDesc"
      class="page__btn btn"
      @click="sendForm"
    >
      <span>{{loc('cityHallWeb_129')}}</span>
      <span>${{currentLicense.price}}</span>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
import Payment from '../../components/Payment'
import MenuDrop from '../../components/MenuDrop'
export default {
  name: 'LicensesPage',

  components: {
    Payment,
    MenuDrop
  },

  data: function() {
    return{
      currentLicense: null,
      currentDesc: null,
      currentPayment: 0, // 0 - money , 1 - card
    }
  },

  methods: {
    setCurrentItem: function(obj) {
      this.currentLicense = obj
    },

    setCurrentPayment: function(value) {
      this.currentPayment = value
    },

    sendForm: function() {
      window.mp.trigger('cityHallWeb:sendFormLicensesPage', this.currentLicense.key, this.currentDesc, this.currentPayment)
      this.currentLicense = this.list[0]
      this.currentDesc = null
      this.currentPayment = 0
    }
  },

  computed: {
    ...mapState('cityHallWeb/licenses', ['list']),
    ...mapGetters('localization', ['loc'])
  },

  mounted() {
    this.currentLicense = this.list[0]
  }
}
</script>

<style lang="scss" scoped>
.city-hall-web{
  .body-tab{
    .page{
      &__btn{
        span{
          &:nth-child(1){
            margin-right: .6rem;
            padding-right: .65rem;
            position: relative;
            display: flex;
            align-items: center;
            &:after{
              content: '';
              width: 1px;
              height: 1.5rem;
              background-color:  rgba(255, 255, 255, 0.3);
              position: absolute;
              right: 0;
            }
          }
        }
      }
    }
  }
}
</style>
