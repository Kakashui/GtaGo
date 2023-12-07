<template>
  <div class="view-page">
    <div class="btn-close" @click="closeInterface">
      <div class="prompt">
        <span>{{loc('carDealership_1')}}</span>
        <div class="desc">ESC {{loc('carDealership_2')}}</div>
      </div>
      <div class="btn icon"></div>
    </div>
    <div class="header">
      <div class="header__model">{{nameModel}}</div>
      <div class="header__price">${{price}}</div>
      <div 
        class="btn btn-buy" 
        @click="setShowModal"
        v-if="!soldOut"
      >
        <div class="text">{{loc('carDealership_3')}}</div>
      </div>
      <div
        class="btn btn-sold"
        v-else
      >
        <div class="text">Sold out</div>
      </div>
    </div>
    <div class="stats">
      <div class="stats__parameters">
        <div 
          v-for="(item, index) in specifications"
          :key="index"
          class="stats__parameters-item"
        >
          <div class="title">
            <div class="desc">{{loc(item.desc)}}</div>
            <div class="value">{{item.value}}/{{maxValueSpecifications}}</div>
          </div>
          <div class="progress-wrap">
            <div class="progress-line" :style="{width: 100*(item.value/maxValueSpecifications) + '%'}"></div>
          </div>
        </div>
      </div>
      <div class="stats__features">
        <div class="stats__features-item">
          <div class="title">
            <div class="value">{{loc(fuelConsumption)}}</div>
            <div class="desc">{{loc('carDealership_4')}}</div>
          </div>
          <div class="icon" style="background-image: url(/img/carDealership/consumption.svg)"></div>
        </div>
        <div class="stats__features-item">
          <div class="title">
            <div class="value">{{fuelTank}} {{loc('carDealership_5')}}</div>
            <div class="desc">{{loc('carDealership_6')}}</div>
          </div>
          <div class="icon" style="background-image: url(/img/carDealership/tank-capacity.svg)"></div>
        </div>
        <div class="stats__features-item">
          <div class="title">
            <div class="value">{{trunk}} kg</div>
            <div class="desc">{{loc('carDealership_7')}}</div>
          </div>
          <div class="icon" style="background-image: url(/img/carDealership/trunk-capacity.svg)"></div>
        </div>
      </div>
    </div>
    <div class="interaction">
      <div class="interaction__car">
        <div class="btn arr-left" @click="prevVehicle"></div>
        <div class="btn btn-test" @click="testDrive">
          <div class="text">{{loc('carDealership_8')}}</div>
        </div>
        <div class="btn arr-right" @click="nextVehicle"></div>
      </div>
      <div class="interaction__color">
        <div 
          :class="[{ active: item === currentColor }, 'item']" 
          v-for="item in colors"
          :style="{backgroundColor: 'rgb(' + item.r + ', ' + item.g + ', ' + item.b + ')'}"
          :key="item.id"
          @click="setCurrentColor(item)"
        ></div>
      </div>
    </div>
    <div class="modal-wrap" v-if="showModal">
      <div class="modal">
        <div class="btn btn-close" @click="setShowModal"></div>
        <div class="title">{{loc('carDealership_9')}}</div>
        <div class="cost">
          <div class="cost__desc">{{nameModel}}</div>
          <div class="cost__value">$ {{price}}</div>
        </div>
        <div class="for-family">
          <input id="family-buy" type="checkbox" v-model="purchaseForFamily">
          <label for="family-buy">{{loc('carDealership_10')}}</label>
        </div>
        <div class="btns-wrap">
          <div class="btn btn-cash" @click="buyVehicleCash">
            <div class="text">{{loc('carDealership_11')}}</div>
          </div>
          <div class="btn btn-card" @click="buyVehicleCard">
            <div class="text">{{loc('carDealership_12')}}</div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'

export default {
  name: 'ViewPage', 

  data: function() {
    return {
      currentColor: null,
      showModal: false,
      purchaseForFamily: false
    }
  },

  computed: {
    ...mapState('carDealership', [
      'nameModel', 
      'price',
      'colors', 
      'specifications', 
      'maxValueSpecifications', 
      'fuelConsumption', 
      'fuelTank', 
      'trunk', 
      'soldOut',
    ]),
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    setCurrentColor: function(color) {
      window.mp.trigger('carDealership:setColor', color.r, color.g, color.b)
      this.currentColor = color
    },
    testDrive: function() {
      window.mp.trigger('carDealership:testDrive', this.nameModel)
    },
    prevVehicle: function() {
      window.mp.trigger('carDealership:prevVehicle')
    },
    nextVehicle: function() {
      window.mp.trigger('carDealership:nextVehicle')
    },
    buyVehicleCash: function() {
      window.mp.trigger('carDealership:buyVehicleCash', this.purchaseForFamily)
      this.setShowModal()
      this.purchaseForFamily = false
    },
    buyVehicleCard: function() {
      window.mp.trigger('carDealership:buyVehicleCard', this.purchaseForFamily)
      this.setShowModal()
      this.purchaseForFamily = false
    },
    closeInterface: function() {
      window.mp.trigger('carDealership:closeInterface')
    },
    setShowModal: function() {
      this.showModal = !this.showModal
    }
  }
}
</script>

<style lang="scss" scoped>
.view-page{
  display: flex;
  justify-content: center;
  align-items: center;
  width: 100%;
  height: 100%;
  padding: 2rem;
  position: relative;
  color: #FFFFFF;
  font-family: Roboto;
  .btn-close{
    position: absolute;
    top: 2rem;
    right: 2rem;
    display: flex;
    align-items: center;
    &:hover{
      .icon{
        transform: scale(1.1);
      }
    }
    .prompt{
      display: flex;
      flex-flow: column;
      align-items: flex-end;
      margin-right: 1rem;
      text-transform: uppercase;
      text-transform: uppercase;
      font-weight: bold;
      span{
        color: #626366;
        font-size: .9rem;
        margin-bottom: .25rem;
      }
      .desc{
        font-size: 1.2rem;
      }
    }
    .icon{
      width: 2.5rem;
      height: 2.5rem;
      background-image: url('/img/carDealership/btn-close.svg');
      background-color: #222428;
      border: 1px solid #343539;
    }
  }
  .header{
    display: flex;
    align-items: center;
    flex-flow: column;
    position: absolute;
    top: 2rem;
    &__model{
      font-size: 1.7rem;
      line-height: 2rem;
      text-transform: uppercase;
    }
    &__price{
      text-transform: uppercase;
      font-weight: bold;
      font-size: 3rem;
      line-height: 3rem;
      color: #D6FF00;
      margin: .5rem 0;
      text-shadow: 0 .2rem .2rem rgba(0, 0, 0, 0.25);
    }
    .btn-buy, .btn-sold{
      z-index: 99;
      transform: skewX(-15deg);
      padding: .8rem 1.6rem;
      .text{
        font-size: 1.2rem;
        transform: skewX(15deg);
      }
    }
    .btn-buy{
      background: #B6D12A;
      &:hover{
        box-shadow: 0 0 1.5rem #D6FF00;
        transform: skewX(-15deg) scale(1.1);
      }
      &:active{
        transform: skewX(-15deg) scale(1.05);
      }
    }
    .btn-sold{
      background: rgba(255, 255, 255, 0.1);
      .text{
        color: #D12A2A;
        font-weight: bold;
      }
    }
  }
  .stats{
    display: flex;
    align-items: center;
    justify-content: space-between;
    width: 100%;
    &__parameters, &__features{
      display: flex;
      flex-flow: column;
      &-item{
        display: flex;
        margin-bottom: 2rem;
        &:last-child{
          margin-bottom: 0;
        }
      }
    }
    &__parameters{
      align-items: flex-start;
      width: 15rem;
      min-width: 15rem;
      &-item{
        align-items: flex-start;
        flex-flow: column;
        width: 100%;
        .title{
          display: flex;
          align-items: flex-end;
          margin-bottom: .5rem;
          text-transform: uppercase;
          font-weight: 500;
          .desc{
            font-size: 1.2rem;
            line-height: 1.4rem;
          }
          .value{
            font-size: 1rem;
            line-height: 1.15rem;
            color: rgba(255, 255, 255, 0.5);
            margin-left: .5rem;
          }
        }
        .progress-wrap{
          width: 100%;
          height: .5rem;
          position: relative;
          overflow: hidden;
          .progress-line{
            height: 100%;
            position: absolute;
            left: 0;
            top: 0;
            border-radius: .2rem;
            background: linear-gradient(90deg, #B6D12A 58.94%, #111111 96.19%);
            min-width: .3rem!important;
          }
        }
      }
    }
    &__features{
      align-items: flex-end;
      &-item{
        justify-content: flex-end;
        align-items: center;
        .title{
          display: flex;
          flex-flow: column;
          align-items: flex-end;
          justify-content: center;
          text-transform: uppercase;
          margin-right: .5rem;
          .value{
            font-size: 1.2rem;
            line-height: 1.4rem;
            color: #D6FF00;
            margin-bottom: .3rem;
          }
          .desc{
            font-size: 1rem;
            line-height: 1.15rem;
            color: #B8B8B8;
          }
        }
        .icon{
          width: 3.2rem;
          height: 3.2rem;
          background-size: contain;
          background-repeat: no-repeat;
          background-position: center;
        }
      }
    }
  }
  .interaction{
    z-index: 99;
    display: flex;
    flex-flow: column;
    align-items: center;
    position: absolute;
    bottom: 3rem;
    &__car{
      margin-bottom: 2rem;
      display: flex;
      align-items: center;
      justify-content: center;
      .btn-test{
        margin: 0 1.5rem;
        height: 3rem;
        min-height: 3rem;
        width: 9.5rem;
        min-width: 9.5rem;
        border-radius: 1rem 0 1rem 0;
        transform: skewX(-15deg);
        background: #292929;
        &:hover{
          box-shadow: 0 0 1.5rem #D6FF00;
          background: #B6D12A;
          transform: skewX(-15deg) scale(1.1);
          .text{
            color: #fff;
          }
        }
        &:active{
          transform: skewX(-15deg) scale(1.05);
        }
        .text{
          transform: skewX(15deg);
          font-size: 1rem;
          line-height: 1.15rem;
          text-align: center;
          color: #D6FF00;
        }
      }
      .arr-left, .arr-right{
        width: 3rem;
        height: 3rem;
        min-width: 3rem;
        min-height: 3rem;
        border-radius: 50%;
        background-color: #292929;
        &:hover{
          box-shadow: 0 0 1.5rem #D6FF00;
          background-color: #B6D12A;
          transform: scale(1.1);
        }
        &:active{
          transform: scale(1.05);
        }
      }
      .arr-left{
        background-image: url('/img/carDealership/arr-left.svg');
        &:hover{
          background-image: url('/img/carDealership/arr-left-hover.svg');
        }
      }
      .arr-right{
        background-image: url('/img/carDealership/arr-right.svg');
        &:hover{
          background-image: url('/img/carDealership/arr-right-hover.svg');
        }
      }
    }
    &__color{
      display: flex;
      align-items: center;
      justify-content: center;
      .item{
        margin-right: 1rem;
        border: .1rem solid rgba(255, 255, 255, 0.86);
        border-radius: 50%;
        width: 1.5rem;
        height: 1.5rem;
        min-width: 1.5rem;
        min-height: 1.5rem;
        transition: .2s;
        &:hover{
          box-shadow: 0 .1rem 1.5rem rgb(255,255,255);
          transform: scale(1.1);
          transition: .2s;
        }
        &:active{
          transform: scale(1.05);
        }
        &.active{
          box-shadow: 0 .1rem 1.5rem rgb(255,255,255);
        }
        &:last-child{
          margin-right: 0;
        }
      }
    }
  }
  .modal-wrap{
    z-index: 99;
    display: flex;
    align-items: center;
    justify-content: center;
    position: fixed;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background: rgba(0, 0, 0, 0.4);
    .modal{
      display: flex;
      flex-flow: column;
      align-items: center;
      background: #222222;
      border-radius: 1rem;
      padding: 2.5rem;
      text-transform: uppercase;
      color: #FFFFFF;
      text-transform: uppercase;
      min-width: 23.7rem;
      position: relative;
      .btn-close{
        position: absolute;
        top: -.5rem;
        right: 0;
        display: flex;
        width: 2.5rem;
        height: 2.5rem;
        background-image: url('/img/carDealership/btn-close.svg');
        transform: translateX(100%);
        &:hover{
          transform: translateX(100%) scale(1.1);
        }
      }
      .title{
        font-weight: bold;
        font-size: 2rem;
        line-height: 2rem;
      }
      .cost{
        background: rgba(255, 255, 255, 0.1);
        border-radius: 1rem;
        display: flex;
        flex-flow: column;
        align-items: center;
        padding: 1.5rem;
        width: 100%;
        margin: 1rem 0 2rem 0;
        &__desc{
          font-size: 1.2rem;
          line-height: 1.45rem;
          color: rgba(255, 255, 255, 0.7);
          margin-bottom: .95rem;
        }
        &__value{
          font-size: 1.7rem;
          line-height: 1.7rem;
          color: #D6FF00;
        }
      }
      .for-family{
        display: flex;
        align-items: center;
        width: fit-content;
        justify-content: flex-start;
        position: relative;
        align-self: flex-start;
        padding-right: 1.9rem;
        input{
          opacity: 0;
          position: absolute;
          left: 0;
          top: 0;
        }
        input + label{
          color: rgba(255, 255, 255, 0.6);
          transition: .2s;
        }
        input:checked + label{
          color: rgba(255, 255, 255, 1);
          transition: .2s;
        }
        input + label:before, input:checked + label:after{
          position: absolute;
          right: 0;
          top: 0;
          content: '';
          width: 1.3rem;
          height: 1.3rem;
        }
        input + label:before{
          background: rgba(255, 255, 255, 0.2);
          border-radius: .1rem;
        }
        input:checked + label:after{
          background-size: contain;
          background-repeat: no-repeat;
          background-position: center;
          background-image: url('/img/carDealership/checkmark.svg');
          transition: .2s;
        }
      }
      .btns-wrap{
        width: 100%;
        padding-top: 1rem;
        margin-top: 1rem;
        position: relative;
        display: flex;
        align-items: center;
        &:before{
          content: '';
          width: 100%;
          height: 1px;
          position: absolute;
          top: 0;
          left: 0;
          background: rgba(255, 255, 255, 0.1);
        } 
        .btn{
          width: 100%;
          margin-right: .6rem;
          transform: skewX(-15deg);
          padding: .8rem;
          border-radius: 1rem 0 1rem 0;
          font-family: Roboto;
          text-transform: initial;
          .text{
            font-size: 1rem;
            transform: skewX(15deg);
            font-weight: bold;
          }
          &:last-child{
            margin-right: 0;
          }
          &:active{
            transform: skewX(-15deg) scale(.95);
          }
        }
        .btn-cash{
          background: #B6D12A;
          &:hover{
            box-shadow: 0 0 1.5rem #D6FF00;
          }
        }
        .btn-card{
          background: rgba(255, 255, 255, 0.16);
          &:hover{
            background: #7A7A7A;
            .text{
              color: #D6FF00;
            }
          }
        }
      }
    }
  }
}
</style>
