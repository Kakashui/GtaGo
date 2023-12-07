<template>
  <div class="donation-increase" v-if="show">
    <div class="donation-increase__timer">
      <div class="donation-increase__timer-desc">{{loc('donincrease:1')}}</div>
      <div class="donation-increase__timer-value">{{`${days}d ${hours}h ${minutes}m`}}</div>
    </div>
    <div class="donation-increase__factor">
      <div class="donation-increase__factor-value">X2</div>
    </div>
  </div>
</template>

<script>
import { mapGetters } from 'vuex';


export default {
  data(){
    return {
      date: new Date('2022-01-15T03:45:00Z'),
      interval: 0,
      days: 0,
      hours: 0,
      minutes: 0,
      show: true
    }
  },
  methods: {
    tick(){
      if(this.date < Date.now()){
            clearInterval(this.interval)
            this.show = false;
            return;
      }
      let minutes = Math.floor((this.date - Date.now()) / 1000 / 60);
      this.days = Math.floor(minutes / (60 * 24));
      minutes -= this.days * 60 * 24;
      this.hours = Math.floor(minutes / 60);
      this.minutes = minutes - this.hours * 60;
    }
  },
  computed:{
    ...mapGetters('localization', ['loc'])
  },
  mounted(){
    this.interval = setInterval(this.tick, 1000)
  },
  beforeDestroy(){
    clearInterval(this.interval)
  },
}
</script>

<style lang="scss" scoped>
.donation-increase{
  display: flex;
  position: absolute;
  right: 1rem;
  top: 5.75rem;
  font-style: normal;
  font-weight: normal;
  font-size: 1rem;
  line-height: 1.2rem;
  letter-spacing: 0.04em;
  color: #FFFFFF;
  padding-right: 1.15rem;
  margin-top: .5rem;
  &__timer{
    display: flex;
    flex-direction: column;
    margin-right: .6rem;
    text-shadow: 0 .1rem .3rem rgba(0, 0, 0, 0.25);
    &-desc{
      color: rgba(255, 255, 255, 0.8);
    }
  }
  &__factor{
    position: relative;
    display: flex;
    align-items: center;
    justify-content: center;
    &-value{
      background: -webkit-linear-gradient(rgba(255, 51, 128, 1), rgba(255, 132, 178, 1));
      -webkit-background-clip: text;
      -webkit-text-fill-color: transparent;
      font-weight: bold;
      font-size: 2.8rem;
      line-height: 2.8rem;
      text-transform: uppercase;
      text-shadow: 0 .2rem 2.5rem rgba(255, 51, 128, 0.5);
      z-index: 2;
    }
    &:before, &:after{
      content: '';
      position: absolute;
    }
    &:before{
      width: 3.85rem;
      height: 2.2rem;
      background: #DE729B;
      filter: blur(1rem);
      border-radius: 5rem;
      z-index: 0;
    }
    &:after{
      width: 4.9rem;
      height: 4.9rem;
      background-image: url('/img/hud/circular-stripes.png');
      background-size: contain;
      background-repeat: no-repeat;
      background-position: center;
      animation: rotating 25s linear infinite;
      z-index: 1;
    }
  }
  &:after{
    content: 'Donate';
    -webkit-writing-mode: vertical-rl; 
    writing-mode: tb-rl;
    letter-spacing: 0.02em;
    font-weight: bold;
    font-size: .95rem;
    position: absolute;
    right: 0;
    transform-origin: 50% 50%;
    transform: rotate(180deg);
  }
}
@keyframes rotating {
  from{
    transform: rotate(0deg);
  }
  to{
    transform: rotate(360deg);
  }
}
</style>
