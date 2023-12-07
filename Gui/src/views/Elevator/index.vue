<template>
  <div class="elevator">
    <div class="current-floor">{{ loc(floors[floor]) }}</div>
    <div class="interaction">
      <div class="btn btn-up" @click="up"></div>
      <div class="display-info">{{ loc(floors[floor]) }}</div>
      <div class="btn btn-down" @click="down"></div>
      <div class="btn btn-start" @click="start">start</div>
    </div>
  </div>
</template>

<script>
import {mapState, mapGetters} from 'vuex'

export default {
  data() {
    return {
      floor: 0
    }
  },
  computed:{
    ...mapState("elevator", ["floors"]),
    ...mapGetters("localization", ["loc"])
  },
  methods: {
    down(){
      this.floor--;
      if(this.floor < 0)this.floor = this.floors.length - 1;
    },
    up(){
      this.floor++;
      if(this.floor >= this.floors.length) this.floor = 0;
    },
    // stop(){
    //     window.mp.trigger('lift', 'stop');
    // },
    start(){
      window.mp.trigger('lift', 'start', this.floor);
    },
  },
  beforeDestroy() {
    this.$store.commit("elevator/reset")
  },
}
</script>

<style lang="scss" scoped>
.elevator {
  display: flex;
  flex-flow: column;
  background: rgba(0, 0, 0, 0.65);
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translateX(-50%) translateY(-50%);
  padding: 2.5rem;
  text-transform: uppercase;
  .current-floor{
    font-style: normal;
    font-weight: bold;
    font-size: 2.8rem;
    line-height: 2.8rem;
    color: #B6D300;
    margin-bottom: 2rem;
    &:after{
      content: '';
      width: 3.4rem;
      height: 3.4rem;
      position: absolute;
      top: 2.5rem;
      right: 2.5rem;
      background-size: contain;
      background-repeat: no-repeat;
      background-position: center;
      background-image: url('/img/elevator/arrows.svg');
    }
  }
  .interaction{
    display: flex;
    align-items: center;
    &>div{
      margin-right: .45rem;
      &:last-child{
        margin-right: 0;
      }
    }
    .display-info{
      margin-top: .7rem;
      background: rgba(0, 0, 0, 0.65);
      box-shadow: inset 0 -.2rem 1.1rem rgba(255, 255, 255, 0.25);
      border-radius: 11.1rem;
      margin-top: 0;
      height: 3.6rem;
      width: 4.35rem;
      display: flex;
      align-items: center;
      justify-content: center;
      font-size: .8rem;
      line-height: 1.15rem;
      color: #FFFFFF;
      border: 1px solid rgba(255,255,255, .2);
    }
    .btn{
      display: flex;
      align-items: center;
      justify-content: center;
      border-radius: 50%;
      width: 3.6rem;
      height: 3.6rem;
      transition: .2s;
      &:hover{
        transition: .2s;
      }
    }
    .btn-up, .btn-down{
      background-color: rgba(255, 255, 255, 0.15);
      background-image: url('/img/elevator/btn-arr.svg');
      background-size: contain;
      background-repeat: no-repeat;
      background-position: center;
      &:hover{
        background-color: rgba(255, 255, 255, 0.35);
      }
    }
    .btn-down{
      transform: rotate(180deg);
      transform-origin: 50% 50%;
    }
    .btn-start{
      background: #B6D300;
      color: #000;
      &:hover{
        box-shadow: 0 .5rem 2.5rem #A8C302;
      }
    }
  }
}

    

</style>