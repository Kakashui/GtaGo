<template>
  <div class="technical-certificate">
    <div class="car-info">
      <div class="car-info__desc">{{loc('TechnicalCertificate_1')}}</div>
      <div class="car-info__name">
        <div class="desc">{{loc('TechnicalCertificate_2')}}</div>
        <div class="value">{{carState.carName}}</div>
      </div>
      <div class="car-info__state">
        <div class="car-info__state-condition">
          <div class="desc">{{loc('TechnicalCertificate_3')}}</div>
          <div class="value">{{carState.generalState}}%</div>
        </div>
        <div class="car-info__state-run">
          <div class="desc">{{loc('TechnicalCertificate_4')}}</div>
          <div class="value">{{carState.miliage}} {{loc('TechnicalCertificate_7')}}</div>
        </div>
      </div>
      <div class="car-info__owner">
        <div class="desc">{{loc('TechnicalCertificate_5')}}</div>
        <div class="value">{{carState.owner}}</div>
      </div>
      <div class="car-info__price">
        <div class="desc">{{loc('TechnicalCertificate_6')}}</div>
        <div class="value">${{carState.price}}</div>
      </div>
    </div>
    <div class="car-state">
      <list-item
        v-for="item in carState.mechState"
        :key="item.id"
        :item="item"
      />
    </div>
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
import ListItem from './ListItem'

export default {
  name: 'TechnicalCertificate',

  components: {
    ListItem
  },

  computed: {
    ...mapState('technicalCertificate', ['carState']),
    ...mapGetters('localization', ['loc'])
  }
}
</script>

<style lang="scss" scoped>
.technical-certificate{
  display: flex;
  align-items: center;
  justify-content: center;
  background: radial-gradient(44.58% 79.26% at 50% 50%, rgba(21, 23, 28, 0.7) 0%, rgba(11, 13, 18, 0.9) 100%);
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  text-transform: uppercase;
  .car-info{
    display: flex;
    flex-flow: column;
    align-items: flex-start;
    justify-content: space-between;
    background:  rgba(2, 3, 16, 0.9) url('/img/technicalCertificate/bg.png') no-repeat center;
    background-size: cover;
    width: 27.5rem;
    min-width: 27.5rem;
    height: 25rem;
    padding: 3.5rem 2.75rem;
    position: relative;
    letter-spacing: 0.03em;
    &__desc{
      position: absolute;
      font-size: 2.3rem;
      line-height: 2.75rem;
      color: rgba(255, 255, 255, 0.2);
      transform: rotate(-90deg) translateX(2rem);
      transform-origin: 100% 100%;
      top: 0;
      right: 0;
    }
    &__name{
      display: flex;
      flex-flow: column;
      align-items: flex-start;
      .desc{
        font-size: .9rem;
        line-height: 1.1rem;
        color: rgba(255, 255, 255, 0.5);
      }
      .value{
        font-size: 2.3rem;
        line-height: 2.75rem;
        font-weight: 300;
        color: #FFFFFF;
      }
    }
    &__state{
      display: flex;
      align-items: center;
      justify-content: flex-start;
      width: 100%;
      position: relative;
      padding-bottom: 2rem;
      margin-bottom: 2rem;
      &:after{
        content: '';
        width: 100%;
        height: .1rem;
        background: #B6D300;
        box-shadow: 0 .5rem 2.5rem rgba(168, 195, 2, 0.6);
        border-radius: 1px;
        position: absolute;
        bottom: 0;
        left: 0;
      }
      &-condition{
        display: flex;
        align-items: center;
        margin-right: 2rem;
        .desc{
          font-size: .9rem;
          line-height: 1.2rem;
          color: rgba(255, 255, 255, 0.5);
          max-width: 4rem;
          margin-right: .35rem;
        }
        .value{
          font-weight: 300;
          font-size: 2.5rem;
          line-height: 3rem;
          text-align: center;
          color: #B6D300;
          text-shadow: 0px 10px 50px rgba(168, 195, 2, 0.6);
        }
      }
      &-run{
        display: flex;
        flex-flow: column;
        .desc{
          font-size: .9rem;
          line-height: 1.1rem;
          color: rgba(255, 255, 255, 0.5);
        }
        .value{
          font-weight: 300;
          font-size: 1.3rem;
          line-height: 1.55rem;
          color: #FFFFFF;
        }
      }
    }
    &__owner, &__price{
      display: flex;
      flex-flow: column;
      .desc{
        font-size: 1rem;
        line-height: 1.2rem;
        color: rgba(255, 255, 255, 0.5);
      }
      .value{
        font-size: 1.5rem;
        line-height: 1.8rem;
        text-transform: uppercase;
        color: #FFFFFF;
        font-weight: 300;
      }
    }
    &__price{
      margin-top: 1.25rem;
    }
  }
  .car-state{
    display: grid;
    grid-template-columns: repeat(2, 12rem);
    grid-auto-rows: 12rem;
    grid-gap: 1rem;
    margin-left: 1rem;
    &__item{
      background: rgba(2, 3, 16, 0.9);
      position: relative;
      display: flex;
      flex-flow: column;
      align-items: flex-start;
      justify-content: flex-start;
      padding: 1.5rem 1.25rem;
      color: #fff;
      background-size: contain;
      background-repeat: no-repeat;
      background-position: bottom center;
      transition: .2s;
      &>div{
        z-index: 2;
      }
      &:before, &:after{
        content: '';
        width: 100%;
        height: 100%;
        position: absolute;
        top: 0;
        left: 0;
      }
      &:before{
        background: linear-gradient(0deg, #B6D300, #B6D300);
        opacity: 0;
        z-index: 0;
      }
      &:after{
        background-size: contain;
        background-repeat: no-repeat;
        background-position: bottom center;
        z-index: 1;
      }
      &.engine{
        &:after{
          background-image: url('/img/technicalCertificate/engine.png');
        }
      }
      &.turbine{
        &:after{
          background-image: url('/img/technicalCertificate/turbine.png');
        }
      }
      &.transmission{
        &:after{
          background-image: url('/img/technicalCertificate/transmission.png');
        }
      }
      &.brakes{
        &:after{
          background-image: url('/img/technicalCertificate/brakes.png');
        }
      }
      &:hover{
        background-color: transparent;
        transition: .2s;
        &:before{
          opacity: 1;
          transition: .2s;
        }
        .progress-wrap{
          &__item{
            &.active{
              background: #fff;
            }
          }
        }
      }
      &-title{
        font-size: 1.8rem;
        line-height: 2.15rem;
      }
      &-level{
        font-weight: normal;
        font-size: 1rem;
        line-height: 1.2rem;
        letter-spacing: 0.03em;
      }
      .progress-wrap{
        display: flex;
        align-items: center;
        justify-content: space-between;
        margin-top: .65rem;
        width: 100%;
        &__item{
          width: 100%;
          height: .25rem;
          margin-right: .3rem;
          background: rgba(255, 255, 255, 0.2);
          border-radius: 1px;
          &.active{
            background: #B6D300;
            box-shadow: 0 .5rem 2.5rem rgba(168, 195, 2, 0.6);
          }
          &:last-child{
            margin-right: 0;
          }
        }
      }
    }
  }
}
</style>