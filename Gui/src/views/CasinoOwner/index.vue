<template>
  <div class="casino-owner">
    <div class="casino-owner__close">
      <div class="desc-wrap">
        <div class="desc-wrap__prompt">{{loc('cas:owner:1')}}</div>
        <div class="desc-wrap__value">{{loc('cas:owner:2')}}</div>
      </div>
      <div 
        class="btn"
        @click="closeInterface"
      ></div>
    </div>
    <div class="casino-owner__wrap">
      <Modal
        v-if="modal.show"
        :type="modal.value"
        @closeModal="closeModal"
      />
      <div>
        <div class="desc">{{loc('cas:owner:3')}}</div>
        <div class="owner">{{nickname}}</div>
        <div class="tax-wrap">
          <div class="desc">{{loc('cas:owner:4')}}</div>
          <div class="tax-wrap__value">
            <div class="number">{{taxGames}}%</div>
            <div class="prompt">?
              <div class="prompt__wrap">{{loc('cas:owner:5')}}</div>
            </div>
          </div>
        </div>
        <div class="profit-wrap">
          <div class="profit-wrap__desc desc">{{loc('cas:owner:6')}}</div>
          <div class="profit-wrap__range">
            <range-slider
              class="slider"
              min="0"
              max="15"
              step="1"
              v-model.number="currentPercent"
            />
          </div>
          <div class="profit-wrap__values">
            <div :class="{'current view': currentPercent === 0}">0%</div>
            <div :class="[{'view': currentPercent > 0}, 'current']">{{currentPercent}}%</div>
          </div>
        </div>
      </div>
      <div>
        <div class="bank-wrap">
          <div class="bank-wrap__desc desc">{{loc('cas:owner:7')}}</div>
          <div class="bank-wrap__value">{{bank}}</div>
          <div class="bank-wrap__btns">
            <div 
              class="btn"
              @click="setModal(0)"
            >
              <span>{{loc('cas:owner:8')}}</span>
            </div>
            <div 
              class="btn"
              @click="setModal(1)"
            >
              <span>{{loc('cas:owner:9')}}</span>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import RangeSlider from 'vue-range-slider'
import 'vue-range-slider/dist/vue-range-slider.css'
import { mapGetters, mapState } from 'vuex'

import Modal from './Modal'

export default {
  name: 'CasinoOwner',

  components: {
    RangeSlider,
    Modal
  },

  data: function() {
    return{
      currentPercent: null,
      modal: {
        show: false,
        value: null
      }
    }
  },

  methods: {
    setModal: function(value) {
      this.modal.show = true
      this.modal.value = value
    },

    closeModal: function() {
      this.modal.show = false
      this.modal.value = null
    },
    closeInterface: function() {
      window.mp.trigger('casinoOwner:closeInterface', this.currentPercent)
    },

    escPress(event) {
      if (event.keyCode === 27) {
        this.closeInterface()
      }
    }

  },

  computed: {
    ...mapState('casinoOwner', ['nickname', 'taxGames', 'currentTax', 'bank']),
    ...mapGetters('localization', ['loc'])
  },

  mounted() {
    this.currentPercent = this.currentTax

    document.addEventListener('keyup', this.escPress);
  }
}
</script>

<style lang="scss" scoped>
.casino-owner{
  width: 100%;
  height: 100vh;
  position: absolute;
  left: 0;
  top: 0;
  display: flex;
  align-items: flex-end;
  justify-content: center;
  background-size: contain;
  background-repeat: no-repeat;
  background-position: center;
  background-image: url('/img/casinoOwner/bg.png');
  text-transform: uppercase;
  &__close{
    position: absolute;
    right: 2.5rem;
    top: 2.5rem;
    display: flex;
    align-items: center;
    .desc-wrap{
      display: flex;
      flex-flow: column;
      align-items: flex-end;
      margin-right: 1rem;
      &__prompt{
        font-weight: bold;
        font-size: .9rem;
        letter-spacing: 0.03em;
        color: rgba(255, 255, 255, 0.5);
      }
      &__value{
        font-weight: bold;
        font-size: 1.2rem;
        letter-spacing: 0.03em;
        color: #FFFFFF;
      }
    }
    .btn{
      width: 2.5rem;
      height: 2.5rem;
      background-image: url('/img/casinoOwner/close-icon.svg');
      background-size: contain;
      background-repeat: no-repeat;
      background-position: center;
      background-color: rgba(255, 255, 255, 0.1);
      border: 1px solid rgba(255, 255, 255, 0.1);
      transition: .3s;
      &:hover{
        background-color: rgba(255, 255, 255, 0.4);
      }
    }
  }
  &__wrap{
    width: 21rem;
    height: 33rem;
    margin-bottom: 3rem;
    max-height: 61vh;
    display: flex;
    flex-flow: column;
    align-items: center;
    justify-content: space-between;
    position: relative;
    &>div{
      width: 100%;
      display: flex;
      flex-flow: column;
      align-items: center;
    }
    .desc{
      font-weight: normal;
      font-size: 1.2rem;
      line-height: 1.45rem;
      letter-spacing: 0.14em;
      text-transform: uppercase;
      color: rgba(255, 255, 255, 0.5);
    }
    &>.desc{
      margin-bottom: .25rem;
    }
    .owner{
      color: rgba(234, 205, 163, 1);
      font-style: normal;
      font-weight: bold;
      font-size: 3.5rem;
      line-height: 3.5rem;
      text-transform: uppercase;
      white-space: pre;
      margin-bottom: 2.5rem;
    }
    .tax-wrap, .profit-wrap{
      padding: 0 1.95rem;
      display: flex;
    }
    .tax-wrap{
      align-items: flex-start;
      justify-content: space-between;
      margin-bottom: 3rem;
      &__value{
        display: flex;
        align-items: flex-end;
        .number{
          font-weight: bold;
          font-size: 2.5rem;
          text-transform: uppercase;
          color: rgba(234, 205, 163, 1);
          margin-right: .6rem;
        }
        .prompt{
          width: 1.3rem;
          height: 1.3rem;
          background-color: #fff;
          border-radius: 50%;
          font-weight: bold;
          font-size: 1rem;
          text-transform: uppercase;
          color: #000000;
          display: flex;
          justify-content: center;
          margin-bottom: .6rem;
          position: relative;
          &__wrap{
            position: absolute;
            right: -.4rem;
            top: 0;
            transform: translateX(100%);
            width: 13.5rem;
            padding: 1.25rem;
            font-style: normal;
            font-weight: normal;
            font-size: .9rem;
            line-height: 1.1rem;
            letter-spacing: 0.03em;
            color: rgba(0, 0, 0, 0.8);
            background-color: #fff;
            display: none;
          }
          &:hover{
            .prompt__wrap{
              display: block;
            }
          }
        }
      }
    }
    .profit-wrap{
      flex-flow: column;
      &__desc{
        width: 100%;
        font-size: 1rem;
        line-height: 1.2rem;
        letter-spacing: 0.25em;
        margin-bottom: .2rem;
        white-space: pre;
      }
      &__range{
        width: 100%;
        margin-bottom: .25rem;
      }
      &__values{
        width: 100%;
        display: flex;
        align-items: center;
        justify-content: space-between;
        font-weight: bold;
        font-size: 2.5rem;
        line-height: 2.5rem;
        text-transform: uppercase;
        color: rgba(255, 255, 255, 0.07);
        .current{
          color: rgba(234, 205, 163, 1);
          opacity: 0;
          &.view{
            opacity: 1;
          }
        }
      }
    }
    .bank-wrap{
      display: flex;
      flex-flow: column;
      align-items: center;
      width: 100%;
      padding-top: 1.7rem;
      &__desc{
        font-size: 1rem;
        line-height: 1.2rem;
        letter-spacing: 0.25em;
        margin-bottom: .25rem;
      }
      &__value{
        font-weight: bold;
        font-size: 3rem;
        line-height: 3rem;
        text-transform: uppercase;
        color: #FFFFFF;
        margin-bottom: 1rem;
        &:before{
          content: '$ ';
        }
      }
      &__btns{
        width: 100%;
        display: flex;
        align-items: center;
        .btn{
          width: 100%;
          height: 3.2rem;
          margin-right: 1rem;
          display: flex;
          align-items: center;
          justify-content: center;
          font-weight: bold;
          font-size: 1rem;
          line-height: 1rem;
          text-transform: uppercase;
          color: #FFFFFF;
          position: relative;
          &>span{
            z-index: 2;
          }
          &:last-child{
            margin-right: 0;
          }
          &:before, &:after{
            content: '';
            width: 100%;
            height: 3.2rem;
            position: absolute;
            box-sizing: border-box;
            z-index: 1;
            transition: .3s;
          }
          &:before{
            border: .1rem solid #E6B980;
            opacity: 1;
          }
          &:after{
            background: linear-gradient(90deg, #E6B980 0%, #EACDA3 100%);
            opacity: 0;
          }
          &:hover{
            &:before{
              opacity: 0;
            }
            &:after{
              opacity: 1;
            }
          }
        }
      }
    }
  }
}
</style>

<style lang="scss">
.casino-owner{
  &__wrap{
    .profit-wrap{
      &__range{
        .range-slider{
          width: 100%;
          padding: 0 .55rem;
          &-rail, &-fill{
            margin-left: -.55rem;
          }
          &-rail{
            width: calc(100% + (1.1rem));
            background: rgba(255, 255, 255, 0.2);
          }
          &-fill{
            background-color: #fff;
            border-radius: .1rem 0 0 .1rem;
          }
          &-knob{
            width: 1.1rem;
            height: 1.1rem;
            border: none;
            background: linear-gradient(90deg, #E6B980 0%, #EACDA3 100%);
          }
        }
      }
    }
  }
}
</style>
