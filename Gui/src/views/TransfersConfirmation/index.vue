<template>
  <div class="transfers-confirmation">
    <div class="transfers-confirmation__wrap">
      <div 
        class="transfers-confirmation__btn-close"
        @click="closeModal"
      ></div>
      <div class="transfers-confirmation-title">{{loc('bank:menu:233')}} <span v-if="transfersList.length > 0">({{transfersList.length}})</span></div>
      <div class="transfers-confirmation__list">
        <div 
          class="transfers-confirmation__list-item"
          v-for="item in transfersList"
          :key="item.ID"
        >
          <div class="transfers-confirmation__list-item__info">
            <div>
              <div class="transfers-confirmation__list-item__info-desc sender">{{loc('bank:menu:234')}}</div>
              <div class="transfers-confirmation__list-item__info-value">{{item.FromName}}</div>
            </div>
            <div>
              <div class="transfers-confirmation__list-item__info-desc recipient">{{loc('bank:menu:235')}}</div>
              <div class="transfers-confirmation__list-item__info-value">{{item.ToName}}</div>
            </div>
          </div>
          <div class="transfers-confirmation__list-item-desc">{{item.Reason}}</div>
          <div class="transfers-confirmation__list-item__interaction">
            <div class="transfers-confirmation__list-item-summ">
              <div class="transfers-confirmation__list-item-summ-desc">{{loc('bank:menu:236')}}</div>
              <div class="transfers-confirmation__list-item-summ-value">$ {{item.Amount.toLocaleString('ru')}}</div>
            </div>
            <div class="transfers-confirmation__list-item__interaction-btns">
              <div 
                class="transfers-confirmation__list-item__interaction-btn apply"
                @click="applyTransfer(item.ID)"
              >{{loc('bank:menu:237')}}</div>
              <div 
                class="transfers-confirmation__list-item__interaction-btn"
                @click="cancelTransfer(item.ID)"
              >{{loc('bank:menu:238')}}</div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapState, mapGetters } from 'vuex'
export default {
  name: 'TransfersConfirmation',
  computed: {
    ...mapState('transfersConfirmation', ['transfersList']),
    ...mapGetters('localization',['loc'])
  },
  methods: {
    applyTransfer(id) {
      window.mp.triggerServer('transfer:success', id)
    },
    cancelTransfer(id) {
      window.mp.triggerServer('transfer:cancel', id)
    },
    closeModal() {
      window.mp.trigger('transfer:closemenu')
    }
  }
}
</script>

<style lang="scss">
.transfers-confirmation{
  position: fixed;
  display: flex;
  align-items: center;
  justify-content: center;
  width: 100%;
  height: 100vh;
  left: 0;
  top: 0;
  background: radial-gradient(43.85% 77.96% at 50% 50%, rgba(94, 55, 176, 0.7) 0%, rgba(94, 55, 176, 0) 100%), rgba(0, 0, 0, 0.7);
  text-transform: uppercase;
  font-size: 1rem;
  line-height: 1.2rem;
  color: #fff;
  &__wrap{
    background: rgba(3, 12, 32, 0.7);
    border-radius: .8rem;
    padding: 3rem;
    width: 40rem;
    height: 45rem;
    position: relative;
  }
  &__btn-close{
    display: flex;
    align-items: center;
    justify-content: center;
    background: rgba(94, 55, 176, 0.1);
    border: 1px solid rgba(94, 55, 176, 0.1);
    box-sizing: border-box;
    border-radius: .2rem;
    width: 2.5rem;
    height: 2.5rem;
    position: absolute;
    right: 1.25rem;
    top: 1.25rem;
    transition: .2s;
    &:hover{
      background: rgba(94, 55, 176, 0.2);
      border-color: rgba(94, 55, 176, 0.2);
    }
    &:before{
      content: '';
      width: 1rem;
      height: 1rem;
      background-size: contain;
      background-repeat: no-repeat;
      background-position: center;
      background-image: url('/img/transfersConfirmation/close.png');
    }
  }
  &-title{
    font-weight: bold;
    font-size: 2.5rem;
    line-height: 2.5rem;
    letter-spacing: 0.03em;
    white-space: pre;
    margin-bottom: 1.5rem;
    span{
      color: #5E37B0;
    }
  }
  &__list{
    overflow-y: auto;
    max-height: 38rem;
    padding-right: 2.5rem;
    margin-right: -2.5rem;
    padding-bottom: 3rem;
    min-width: 100%;
    &::-webkit-scrollbar{
      width: .2rem;
      background-color: transparent;
      &-thumb{
        background: #5E37B0;
        border-radius: .3rem;
      }
    }
    &-item{
      border-top: 1px solid rgba(255, 255, 255, 0.2);
      padding: 1rem 0;
      display: flex;
      flex-direction: column;
      &__info{
        display: flex;
        align-items: center;
        margin-bottom: 1rem;
        &>div{
          display: flex;
          flex-direction: column;
          width: 50%;
        }
        &-desc{
          display: flex;
          align-items: flex-end;
          margin-bottom: .2rem;
          color: rgba(255, 255, 255, 0.5);
          &:before{
            content: '';
            width: 1.1rem;
            height: 1.1rem;
            min-width: 1.1rem;
            margin-right: .5rem;
            background-size: contain;
            background-repeat: no-repeat;
            background-position: center;
          }
          &.sender{
            &:before{
              background-image: url('/img/transfersConfirmation/sender.png');
            }
          }
          &.recipient{
            &:before{
              background-image: url('/img/transfersConfirmation/recipient.png');
            }
          }
        }
        &-value{
          line-height: 1rem;
        }
      }
      &-desc{
        font-size: 1.1rem;
        line-height: 1.3rem;
        margin-bottom: 1rem;
      }
      &__interaction{
        display: flex;
        align-items: center;
        justify-content: space-between;
        &-btns{
          display: flex;
          align-items: center;
        }
        &-btn{
          display: flex;
          align-items: center;
          justify-content: center;
          border: .1rem solid #EE443A;
          border-radius: 5rem;
          padding: 0 1.5rem;
          height: 2.3rem;
          margin-right: 1rem;
          transition: .2s;
          &:last-child{
            margin-right: 0;
          }
          &:hover{
            background-color: #EE443A;
          }
          &.apply{
            border: .1rem solid #5E37B0;
            &:hover{
              background-color: #5E37B0;
            }
          }
        }
      }
      &-summ{
        display: flex;
        flex-direction: column;
        &-desc{
          color: rgba(255, 255, 255, 0.5);
          margin-bottom: .2rem;
        }
        &-value{
          font-weight: bold;
          font-size: 1.5rem;
        }
      }
      
    }
  }
}
</style>
