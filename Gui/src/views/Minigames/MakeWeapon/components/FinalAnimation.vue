<template>
  <div class="final-animation" v-bind:class="finalDetail ? 'active' : ''">
    <transition name="fade">
      <span class="final-animation__circle_1" v-show="animationActive"></span>
    </transition>
    <transition name="fade">
      <span class="final-animation__circle_2" v-show="animationActive"></span>
    </transition>
    <transition name="fade">
      <span class="final-animation__circle_3" v-show="animationActive"></span>
    </transition>
    <transition name="fade">
      <span class="final-animation__circle_4" v-show="animationActive"></span>
    </transition>
    <transition name="incr">
      <span class="final-animation__center" v-show="animationActive"></span>
    </transition>
    <transition name="bounce">
      <span class="final-animation__dollars" v-show="animationActive"></span>
    </transition>
    <transition name="fade">
      <span class="final-animation__break" v-show="animationActive" @click="next">
        {{loc('mw:anim:final')}}
      </span>
    </transition>
  </div>
</template>
<script>
import { mapState, mapActions, mapGetters, mapMutations } from "vuex";
export default {
  name: "FinalAnimation",
  data() {
    return {
      current:0,
      width:{
        random: 0,
        offset: 0
      },
      height:{
        random: 0,
        offset: 0
      },
      styles:[
        {"background-color": "#fff", "color": "#000", "left": "300px", "top": "100px"},
        {"background-color": "#555", "color": "#fff", "left": "300px", "top": "100px"},
        {"background-color": "#c1e704", "color": "#000", "left": "300px", "top": "100px"}
      ]
    }
  },
  computed: {
    ...mapState("gameMakeWeapon/pageState", ["finalDetail", "animationActive"]),
    ...mapState('gameMakeWeapon/userData', ['details']),
    ...mapState('gameMakeWeapon/gameState', ['waitNewGame']),
    ...mapGetters('localization', ['loc'])
  },  
  methods: {    
    ...mapActions('gameMakeWeapon/gameState',['newGame']),
    ...mapMutations("gameMakeWeapon/pageState", ["setProfileVisibility","setAnimationActive"]),
    ...mapMutations("gameMakeWeapon/gameState", ["stopGame"]),
    next(){
      if(this.details < 1){
        this.stopGame();
        this.setProfileVisibility(true);
        this.setAnimationActive(false);
        window.setData('notifyList/notify', { type: 4, position: 0, message: "mw:game:detils:miss", time: 3000 })
        window.mp.triggerServer("mg:makeweapon:quit");
      }else if(this.waitNewGame){
        this.newGame();        
      }
    }
  },
  mounted(){
    const width = window.innerWidth;
    const height = window.innerHeight;
    this.width.offset = Math.floor(width * .25)
    this.width.random = width - this.width.offset * 2;

    this.height.offset = Math.floor(height * .25)
    this.height.random = height - this.height.offset * 2;
  }
};
</script>
<style scoped lang="scss">
.final-animation {
  display: block;
  position: fixed;
  top: 0;
  left: 0;
  width: 100vw;
  height: 100vh;
  background: url("/img/miniGames/makeWeapon/animation-bg.png") center center no-repeat;
  background-size: contain;
  opacity: 0;
  z-index: -1;
  &.active {
    opacity: 1;
    z-index: 2;
  }

  &__dollars {
    display: block;
    position: absolute;
    top: 12.5rem;
    left: 0;
    width: 100%;
    height: 7.4rem;
    background: url("/img/miniGames/makeWeapon/dollars.png") center center no-repeat;
    background-size: contain;
  }
  &__center {
    display: block;
    width: 18.9rem;
    height: 18.9rem;
    border-radius: 18.9rem;
    //background: rgba(#fff, 0.05);
    position: absolute;
    top: 50%;
    left: 50%;
    margin-top: -9.45rem;
    margin-left: -9.45rem;
  }
  &__break {
    width: 15rem;
    height: 4rem;
    display: flex;
    align-items: center;
    justify-content: center;
    position: absolute;
    bottom: 3rem;
    left: 40rem;
    font-size: 1.5rem;
    font-weight: bold;
    letter-spacing: .07rem;
    text-transform: uppercase;
    font-size: 1rem;
    color: #000;
    background-color: #c1e704;
    box-shadow: 0 0 2rem 0 #000;
    border-radius: .2rem;
  }
  &__circle {
    &_1,
    &_2,
    &_3,
    &_4 {
      display: block;
      position: absolute;
      background-size: contain;
      top: 50%;
      left: 50%;
    }
    &_1 {
      width: 91.8rem;
      height: 54rem;
      margin-top: -27rem;
      margin-left: -45.9rem;
      background: url("/img/miniGames/makeWeapon/circle-1.png");
      animation: clockwise-in linear 4s infinite;
    }
    &_2 {
      width: 71.5rem;
      height: 54rem;
      margin-top: -27rem;
      margin-left: -35.75rem;
      background: url("/img/miniGames/makeWeapon/circle-2.png");
      animation: anticlockwise-in linear 3s infinite;
    }
    &_3 {
      width: 58.2rem;
      height: 54rem;
      margin-top: -27rem;
      margin-left: -29.1rem;
      background: url("/img/miniGames/makeWeapon/circle-3.png");
      animation: clockwise-in linear 4s infinite;
    }
    &_4 {
      width: 45.3rem;
      height: 45.3rem;
      margin-top: -22.65rem;
      margin-left: -22.65rem;
      background: url("/img/miniGames/makeWeapon/circle-4.png");
      animation: anticlockwise-in linear 2s infinite;
    }
  }
}
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.5s;
}
.fade-enter,
.fade-leave-to {
  opacity: 0;
}

.bounce-enter-active {
  animation: bounce-in 3s;
}
.bounce-leave-active {
  animation: bounce-in 1s reverse;
}
@keyframes bounce-in {
  0% {
    transform: scale(0);
  }
  25% {
    transform: scale(1.3);
  }
  50% {
    transform: scale(0.8);
  }
  100% {
    transform: scale(1);
  }
}

.incr-enter-active {
  animation: incr-in 1s;
}
.incr-leave-active {
  animation: incr-in 1s reverse;
}
@keyframes incr-in {
  0% {
    transform: scale(0);
  }
  100% {
    transform: scale(1);
  }
}

.clockwise-enter-active {
  animation: clockwise-in linear infinite;
}
.clockwise-leave-active {
  animation: clockwise-in linear infinite;
}
@keyframes clockwise-in {
  from {
    transform: rotate(0deg);
  }
  to {
    transform: rotate(360deg);
  }
}

.anticlockwise-enter-active {
  animation: anticlockwise-in linear infinite;
}
.anticlockwise-leave-active {
  animation: anticlockwise-in linear infinite;
}
@keyframes anticlockwise-in {
  from {
    transform: rotate(0deg);
  }
  to {
    transform: rotate(-360deg);
  }
}
</style>
