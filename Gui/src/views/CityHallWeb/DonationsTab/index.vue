<template>
  <div class="body-tab">
    <div class="body-tab__interaction">
      <div class="title">{{loc('cityHallWeb_18')}}</div>
      <div class="prompt">{{loc('cityHallWeb_19')}}</div>
      <div class="switch-org">
        <div 
          :class="[{active: currentOrg === 0}, 'switch-org__item']"
          @click="setCurrentOrg(0)"
        >{{loc('cityHallWeb_20')}}</div>
        <div 
          :class="[{active: currentOrg === 1}, 'switch-org__item']"
          @click="setCurrentOrg(1)"
        >{{loc('cityHallWeb_21')}}</div>
      </div>
      <div class="desc">{{loc('cityHallWeb_24')}}</div>
      <div class="pay-wrap">
        <div class="pay-wrap__input">
          <input 
            type="number"
            v-model="currentPay"
            placeholder="0"
          >
        </div>
        <div 
          class="pay-wrap__btn btn"
          @click="sendPayDonat"
          v-if="currentPay"
        >{{loc('cityHallWeb_22')}}</div>
      </div>
    </div>
    <div class="body-tab__list">
      <div class="title">{{loc('cityHallWeb_23')}}:</div>
      <div 
        class="list-item"
        v-for="(item, index) in sortedArray"
        :key="item.key"
      >
        <span>{{index + 1}}</span>
        <div>{{item.name}}</div>
        <div>$ {{item.donate}}</div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
export default {
  name: 'DonationsTab',

  data: function() {
    return {
      currentOrg: 0,
      currentPay: null,
    }
  },

  methods: {
    setCurrentOrg: function(value) {
      this.currentOrg = value
    },
    sendPayDonat: function() {
      window.mp.trigger('cityHallWeb:sendPayDonat', this.currentPay, this.currentOrg)
    }
  },
  
  computed: {
    ...mapState('cityHallWeb/donations', ['list']),
    ...mapState('cityHallWeb', ['cardId']),
    ...mapGetters('localization', ['loc']),
    sortedArray: function() {
      let array = [...this.list]
      return array.sort((a, b) => b.donate - a.donate )
    }
  }
}
</script>

<style lang="scss" scoped>
@keyframes isShow{
  from{
    opacity: 0;
  }
  to{
    opacity: 1;
  }
}
.city-hall-web{
  .body-tab{
    display: flex;
    padding: 0;
    background-color: transparent;
    position: initial;
    justify-content: flex-start;
    &:after{
      content: '';
      width: 36.65rem;
      height: 38.2rem;
      background-size: contain;
      background-position: right bottom;
      background-repeat: no-repeat;
      background-image: url('/img/cityHallWeb/donations-decorate.png');
      position: absolute;
      right: 0;
      bottom: 0;
      animation: isShow .7s;
      z-index: 0;
    }
    &>div{
      z-index: 1;
    }
    &__interaction{
      padding: 3rem;
      background-color: #fff;
      border-radius: 1rem;
      height: 100%;
      display: flex;
      flex-flow: column;
      width: 27.5rem;
      margin-right: 4rem;
      .title{
        font-family: Merriweather;
        font-style: normal;
        font-weight: bold;
        font-size: 2.4rem;
        line-height: 3rem;
        color: #000000;
        margin-bottom: .4rem;
      }
      .prompt{
        font-family: Roboto;
        font-style: normal;
        font-weight: normal;
        font-size: .8rem;
        line-height: 1.2rem;
        color: #46555F;
        margin-bottom: 2rem;
      }
      .desc{
        display: flex;
        align-items: center;
        position: relative;
        font-family: Roboto;
        font-style: normal;
        font-weight: bold;
        font-size: .9rem;
        line-height: 1.05rem;
        text-transform: uppercase;
        color: #46555F;
        margin-bottom: 1rem;
        padding-left: 1rem;
        &:before{
          content: '';
          width: .4rem;
          height: .4rem;
          border-radius: 50%;
          position: absolute;
          left: 0;
          background: #E00B29;
        }
      }
      .switch-org, .pay-wrap{
        display: flex;
        align-items: center;
        &>div{
          width: 100%;
          margin-right: 1.1rem;
          &:last-child{
            margin-right: 0;
          }
          &:nth-child(1){
            min-width: 8.8rem;
            width: 8.8rem;
          }
        }
      }
      .switch-org{
        margin-bottom: 1.5rem;
        &__item{
          background: #F3F5F7;
          height: 3.7rem;
          min-height: 3.7rem;
          display: flex;
          align-items: center;
          justify-content: center;
          font-family: Roboto;
          font-style: normal;
          font-weight: 500;
          font-size: .8rem;
          text-transform: uppercase;
          color: rgba(70, 85, 95, 0.2);
          transition: .3s;
          &:hover, &.active{
            color: #46555F;
            transition: .3s;
          }
          &.active{
            &:before, &:after{
              content: '';
              width: .1rem;
              height: 1rem;
              background: #E00B29;
              transition: .3s;
            }
            &:before{
              margin-right: .3rem;
            }
            &:after{
              margin-left: .3rem;
            }
          }
        }
      }
      .pay-wrap{
        &__input, &__btn{
          height: 3.4rem;
          min-height: 3.4rem;
          border-radius: .3rem;
        }
        &__input{
          position: relative;
          display: flex;
          align-items: center;
          &:before, input{
            font-family: Roboto;
            font-style: normal;
            font-weight: normal;
            font-size: .8rem;
            color: #46555F;
          }
          &:before{
            content: '$';
            position: absolute;
            left: 1rem;
            z-index: 0;
          }
          input{
            z-index: 1;
            border: 1px solid rgba(70, 85, 95, 0.2);
            box-sizing: border-box;
            padding: 0 1.85rem;
            background-color: transparent;
            outline: none;
            border-radius: .3rem;
            height: 100%;
            width: 100%;
            &::-webkit-outer-spin-button,
            &::-webkit-inner-spin-button {
              -webkit-appearance: none;
              margin: 0;
            }
            &::placeholder{
              color: #46555F;
            }
          }
        }
        &__btn{
          text-transform: uppercase;
          font-style: normal;
          font-weight: bold;
          font-size: 1.1rem;
          text-transform: uppercase;
          color: #FFFFFF;
          background: #E00B29;
          &:hover{
            background: #C00621;
          }
        }
      }
    }
    &__list{
      .title{
        font-family: Merriweather;
        font-style: normal;
        font-weight: bold;
        font-size: 1.7rem;
        line-height: 2.15rem;
        text-transform: uppercase;
        color: #46555F;
        max-width: 27.05rem;
        margin-bottom: 1.35rem;
      }
      .list-item{
        display: flex;
        width: 18.4rem;
        justify-content: space-between;
        position: relative;
        margin-bottom: 2rem;
        font-family: Roboto;
        font-style: normal;
        font-weight: normal;
        font-size: .8rem;
        color: #46555F;
        padding-left: 2rem;
        span{
          position: absolute;
          left: 0;
          &::after{
            content: '.';
          }
        }
      }
    }
  }
}
</style>
