<template>
  <div class="modal-wrap-modify">
    
      <div class="modal-notify">
        <transition name="modal-slide">
          <div v-if="show" >
            <battle-row :data="dataBuffer[currentWar]" />
            <div v-if="dataBuffer[currentWar].comment != null" class="comment">«{{ dataBuffer[currentWar].comment }}»<br><span style="font-size: 1.2rem; color: rgba(255, 255, 255, 0.5);"> © </span><span class="comment-author">{{ dataBuffer[currentWar].enemyName }}</span></div>
          </div>
        </transition>
      </div>
    
      <div class="btns-wrap">
        <div class="btns-wrap-text">
          <div v-if="dataBuffer.length > 1" :style="{ transform: clickedSwitcher == 1 ? 'scale(1.2)' : 'scale(1)' }" @click="showPrevWar" @mouseover="hoveredArrow = 1" @mouseleave="hoveredArrow = 0" class="switch-bg">
            <svg style="vertical-align: middle;" width="21" height="12" viewBox="0 0 21 12" fill="none" xmlns="http://www.w3.org/2000/svg">
              <path d="M0.505237 5.50502C0.23187 5.77839 0.23187 6.22161 0.505237 6.49497L4.96001 10.9497C5.23338 11.2231 5.67659 11.2231 5.94996 10.9497C6.22333 10.6764 6.22333 10.2332 5.94996 9.9598L1.99016 6L5.94996 2.0402C6.22333 1.76683 6.22333 1.32362 5.94996 1.05025C5.67659 0.776885 5.23338 0.776885 4.96001 1.05025L0.505237 5.50502ZM21 5.3L1.00021 5.3L1.00021 6.7L21 6.7L21 5.3Z" :fill="hoveredArrow == 1 ? '#333334' : 'white'"/>
            </svg>
          </div>
          {{loc('fam:btl:notify:select')}}
          <div v-if="dataBuffer.length > 1" :style="{ transform: clickedSwitcher == 2 ? 'scale(1.2)' : 'scale(1)' }" @click="showNextWar" @mouseover="hoveredArrow = 2" @mouseleave="hoveredArrow = 0" class="switch-bg">
            <svg style="transform: scale(-1, 1); vertical-align: middle;" width="21" height="12" viewBox="0 0 21 12" fill="none" xmlns="http://www.w3.org/2000/svg">
              <path d="M0.505237 5.50502C0.23187 5.77839 0.23187 6.22161 0.505237 6.49497L4.96001 10.9497C5.23338 11.2231 5.67659 11.2231 5.94996 10.9497C6.22333 10.6764 6.22333 10.2332 5.94996 9.9598L1.99016 6L5.94996 2.0402C6.22333 1.76683 6.22333 1.32362 5.94996 1.05025C5.67659 0.776885 5.23338 0.776885 4.96001 1.05025L0.505237 5.50502ZM21 5.3L1.00021 5.3L1.00021 6.7L21 6.7L21 5.3Z" :fill="hoveredArrow == 2? '#333334' : 'white'"/>
            </svg>
          </div>
        </div>
        <div style="display: flex; align-items: center; justify-content: center;">
        <div 
          class="btn btn-accept"
          @click="toNextWar(true)"
        >{{loc('fam:btl:notify:succ')}}</div>
        <div 
          class="btn btn-cancel"
          @click="toNextWar(false)"
        >{{loc('fam:btl:notify:cncl')}}</div>
        </div>
      </div>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
import BattleRow from './BattleRow.vue'

export default {
  name: 'ModalNotify',

  props: {
    data: Array
  },

  components: { 
    BattleRow 
  },
  

  data() {
    return {
      show: false,
      dataBuffer: [],
      hoveredArrow: 0,
      currentWar: 0,
      clickedSwitcher: 0
    }
  },
  computed: {
    ...mapGetters('localization', ['loc'])
  },
  methods: {
    closeModal() {
      this.$emit('closeModalNotify')
    },
    showNextWar() {
      this.clickedSwitcher = 2
      setTimeout(() => {
        this.clickedSwitcher = 0
      }, 100)
      this.show = false
      setTimeout(() => {
        this.show = true
      }, 300)

      if(this.currentWar + 1 == this.dataBuffer.length) {
        this.currentWar = 0
      }
      else this.currentWar++
    },
    showPrevWar() {
      this.clickedSwitcher = 1
      setTimeout(() => {
        this.clickedSwitcher = 0
      }, 100)
      this.show = false
      setTimeout(() => {
        this.show = true
      }, 300)

      if(this.currentWar == 0) {
        this.currentWar = this.dataBuffer.length - 1
      }
      else this.currentWar--
    },
    toNextWar(accepted) {
      console.log(JSON.stringify(this.data))
      window.mp.trigger('familyMenu:acceptBattle', this.dataBuffer[this.currentWar].id, accepted);
      if(this.dataBuffer.length == 1) {

        this.show = false
        this.dataBuffer.shift()
        
        setTimeout(() => {
          this.closeModal()
        }, 300)
        
      }
      else {

        this.show = false
        this.dataBuffer.splice(this.currentWar, 1)
        this.currentWar = 0
        setTimeout(() => {
          this.show = true
        }, 300)
      }
    }
  },
  
  mounted() {
    this.show = true;
    this.dataBuffer = [...this.data]
  }
}
</script>

<style lang="scss" scoped>
@keyframes clickEffect {
  from {
    transform: scale(1);
  }
  to {
    transform: scale(1.1);
  }
  
}

.modal-slide-enter-active {
  transition: all 0.3s ease;
}
.modal-slide-leave-active {
  transition: all 0.3s ease;
}
.modal-slide-enter {
  transform: translateX(70rem);
  opacity: 0;
}
.modal-slide-leave-to {
  transform: translateX(-70rem);
  opacity: 0;
}
.modal-wrap-modify{
  position: absolute;
  z-index: 2;
  top: 0;
  left: 0;
  height: 100vh;
  width: 100%;
  background: rgba(28, 28, 30, 0.9);
  .btns-wrap{
    padding-top: 4rem;
    text-align: center;
    height: 30rem;
    background: rgba(27, 28, 30, 0.7);
    box-shadow: 0px -30px 250px rgba(128, 183, 200, 0.2);
    .switch-bg {
      transition: all 0.1s linear;
      margin-left: 1rem;
      margin-right: 1rem;
      width: 2.3rem;
      height: 2.3rem;
      border-radius: 5rem;
      background: rgba(255, 255, 255, 0.1);

      &:hover {
        background: rgb(193, 231, 4);
      }
    }
    .btns-wrap-text {
      display: flex;
      justify-content: center;
      margin-bottom: 2rem;
     text-transform: uppercase;
      font-style: normal;
      font-weight: bold;
      font-size: 2rem;
    }
    .btn{
      padding: 1rem 1.6rem;
      font-weight: bold;
      align-items: flex-end;
      font-size: 1.2rem;
      line-height: 1.2rem;
      border-radius: .1rem;
      margin-right: 1.2rem;
      &:last-child{
        margin-right: 0;
      }
      &:before{
        content: '';
        width: 1.5rem;
        height: 1.5rem;
        margin-right: .5rem;
        background-size: contain;
        background-position: center;
        background-repeat: no-repeat;
        background-image: url('/img/familyMenu/btn.svg');
      }
      &.btn-accept {
        background-image: url(/img/familyMenu/modal-accept-battle.svg);
      }
      &.btn-cancel{
        background-image: url(/img/familyMenu/modal-decline-battle.svg);
        &:before{
          background-image: url('/img/familyMenu/btn-cancel.svg');
        }
      }
    }
    .btn-close{
      background-size: contain;
      background-position: center;
      background-repeat: no-repeat;
      background-color: transparent;
      width: 2rem;
      height: 2rem;
      background-image: url('/img/familyMenu/btn-close.svg');
      border-radius: none;
      margin: 0;
      position: absolute;
      right: 0;
      top: 0;
      transform: translateX(100%);
    }
  }
}

.modal-notify{
  background-size: contain;
  background-position: center;
  background-repeat: no-repeat;
  border-radius: .5rem;
  display: flex;
  flex-flow: column;
  align-items: center;
  text-align: center;
  position: relative;
  margin-top: 18.4rem;
  min-height: 19.35rem;
  .comment {
    margin-top: 1.5rem;
    margin-left: 15.5rem;
   text-transform: uppercase;
    font-style: italic;
    font-weight: bold;
    font-size: 1.7rem;
    text-align: center;
    max-width: 73.8rem;
    color: #FFFFFF;

    &-author {
      font-style: normal;
      font-weight: bold;
      font-size: 1.2rem;
      color: rgba(255, 255, 255, 0.5);
    }
  }
}
</style>
