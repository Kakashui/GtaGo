<template>
  <div class="card-wrap">
    <div class="card-wrap__title">{{loc('cityHallWeb_98')}}</div>
    <div class="card-wrap__title">{{loc('cityHallWeb_99')}}</div>
    <div class="card-wrap__desc">{{loc('cityHallWeb_100')}}</div>
    <div class="card">
      <div class="card__logo"></div>
      <div class="card__info">
        <div class="info-item">
          <div class="info-item__desc">{{loc('cityHallWeb_101')}}</div>
          <div class="info-item__value">{{name}}</div>
        </div>
        <div class="info-item">
          <div class="info-item__desc">{{loc('cityHallWeb_102')}}</div>
          <div class="info-item__value">{{cardId}}</div>
        </div>
        <div class="info-row">
          <div class="info-item">
            <div class="info-item__desc">{{loc('cityHallWeb_103')}}</div>
            <div class="info-item__value">{{birthData}}</div>
          </div>
          <div class="info-item">
            <div class="info-item__desc">{{loc('cityHallWeb_104')}}</div>
            <div class="info-item__value">{{regData}}</div>
          </div>
        </div>
      </div>
    </div>
    <div 
      class="btn btn-next"
      @click="setCurrentWrap('PaintWrap')"
    >{{loc('cityHallWeb_105')}}</div>
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
export default {
  name: 'CardWrap',

  props: {
    setCurrentWrap: Function
  },

  methods: {
    formatDate: function (date) {

      let dd = date.getDate();
      if (dd < 10) dd = '0' + dd;

      let mm = date.getMonth() + 1;
      if (mm < 10) mm = '0' + mm;

      let yy = date.getFullYear();
      if (yy < 10) yy = '0' + yy;

      return dd + '/' + mm + '/' + yy;
    }
  },

  computed: {
    ...mapState('cityHallWeb', ['name', 'day', 'month', 'year', 'cardId']),
    ...mapGetters('localization', ['loc']),

    regData: function() {
      let currentDate = new Date()
      return this.formatDate(currentDate)
    },

    birthData: function() {

      let dd = this.day
      if (dd < 10) dd = '0' + dd;

      let mm = this.month.number
      if (mm < 10) mm = '0' + mm;

      let yy = this.year
      if (yy < 10) yy = '0' + yy;

      return dd + '/' + mm + '/' + yy;
    }
  }
}
</script>

<style lang="scss" scoped>
.card-wrap{
  display: flex;
  flex-flow: column;
  align-items: center;
  justify-content: center;
  .btn-next{
    padding: 0 2.5rem;
  }
  &__title{
    font-family: Merriweather;
    font-style: normal;
    font-weight: bold;
    font-size: 1.7rem;
    line-height: 2.15rem;
    text-align: center;
    text-transform: uppercase;
    color: #000000;
    margin-bottom: .6rem;
    &:first-child{
      margin-bottom: 0;
    }
  }
  &__desc{
    font-family: Roboto;
    font-style: normal;
    font-weight: normal;
    font-size: .8rem;
    line-height: .95rem;
    color: #46555F;
    margin-bottom: 2rem;
  }
  .card{
    display: flex;
    align-items: center;
    justify-content: space-between;
    padding: 4.5rem 2rem;
    background-size: cover;
    background-repeat: no-repeat;
    background-position: center;
    background-image: url('/img/cityHallWeb/card-bg.svg');
    width: 34.2rem;
    height: 19.2rem;
    border-radius: 1.1rem;
    margin-bottom: 4rem;
    &__logo{
      width: 9.85rem;
      height: 6rem;
      background-size: contain;
      background-repeat: no-repeat;
      background-position: center;
      background-image: url('/img/cityHallWeb/card-logo.svg');
    }
    &__info{
      display: flex;
      flex-flow: column;
      font-family: Roboto;
      justify-content: space-between;
      height: 100%;
      .info-item{
        display: flex;
        flex-flow: column;
        margin-bottom: 1rem;
        &__desc{ 
          font-style: normal;
          font-weight: normal;
          font-size: .8rem;
          line-height: .95rem;
          text-transform: uppercase;
          color: rgba(255, 255, 255, 0.6);
          margin-bottom: .4rem;
        }
        &__value{
          font-style: normal;
          font-weight: 500;
          font-size: 1.2rem;
          line-height: 1.4rem;
          text-transform: uppercase;
          color: #FFFFFF;
        }
      }
      .info-row{
        display: flex;
        align-items: center;
        .info-item{
          margin-right: 2rem;
          margin-bottom: 0;
          &:last-child{
            margin-right: 0;
          }
        }
      }
    }
  }
}
</style>
