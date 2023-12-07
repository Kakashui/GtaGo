<template>
  <div class="minigame">   
    <div class="minigame_btn" 
      v-for="(btn, index) in keys" :key="index"
      :class="{'minigame_press': key === btn.keyCode}"
    >
      <img src="/img/common/elements/arrow.svg" alt="arrow">
    </div>
    
    <div class="minigame_progress">
      <div class="minigame_thumb" :style="{'width': `${progress}%`}"></div>
    </div>
  </div>
</template>

<script>
import {mapState} from 'vuex'

export default {
  data(){
    return{
      keys:[
        {"pressed": false, keyCode: 38},        
        {"pressed": false, keyCode: 37},        
        {"pressed": false, keyCode: 40},        
        {"pressed": false, keyCode: 39}
      ],
      key: -1,
      time: 100,
      pressInterval: -1,
      miniGameInterval: -1,
      progress: 50,
      step: 20,
      miss: 0
    }
  },
  methods:{    
    endGame(status){
      if(process.env.NODE_ENV == 'development') 
        this.progress = 50;
      else 
        this.$store.commit('fishing/miniGameEnd',status)
    },
    tickGame(){
      this.progress -= 1;
      if(this.progress > 99) this.endGame(true);
      if(this.progress < 1) this.endGame(false);
    },
    put(e){
      if(!this.keys.find(k=>k.keyCode === e.keyCode)) return;
      if(e.keyCode === this.key)
        this.progress += this.step - this.mgDifficult;
      else {
        this.progress -= (this.step - this.mgDifficult) * 2;
      }

      this.key = this.getRandomKey();
    },
    getRandomKey() {     
      return this.keys[Math.floor(Math.random() * this.keys.length)].keyCode
    }
  },
  computed: {
    ...mapState('fishing', ['mgDifficult'])
  },
  mounted(){
    this.key = this.getRandomKey();
    window.document.addEventListener("keyup", this.put);
    this.pressInterval = window.setInterval(() => {
      this.tickGame();
    }, 100);      
  },
  beforeDestroy(){
    window.document.removeEventListener("keyup", this.put);
    window.clearInterval(this.pressInterval);
    window.clearInterval(this.miniGameInterval);
  }
}
</script>

<style lang="scss">
.minigame{
    position: absolute;
    right: 40rem;
    bottom: 15rem;
    &_btn{
        position: absolute;
        background-color: #eee;
        width: 5vh;
        height: 5vh;
        border-radius: 10px;
        border: 2px solid #000;
        img{
            width: 50%;
            height: 50%;
            margin: 25% 0 0 25%;
        }   
        &:nth-child(1){
            top: 0;
            left: 6vh;
            box-shadow: 1px 1px 3px 0 rgba(#000, .7);
            img{
                transform: rotate(180deg);
            }
        }
        &:nth-child(2){
            top: 6vh;
            left: 0;
            box-shadow: 1px 1px 3px 0 rgba(#000, .7);  
            img{
                transform: rotate(90deg);
            }
        }
        &:nth-child(3){
            top: 6vh;
            left: 6vh;
            box-shadow: 1px 1px 3px 0 rgba(#000, .7);
        }
        &:nth-child(4){
            top: 6vh;
            left: 12vh;
            box-shadow: 1px 1px 3px 1px rgba(#000, .7);
            img{
                transform: rotate(-90deg);
            }
        }     
    }
    &_press{
        transform: scale(1.01) translate(1px, 1px);
        background-color: #0f0;
    }
    &_progress{
        position: absolute;
        top: 13vh;
        left: 0;
        width: 18vh;
        height: 1vh;
        border-radius: 10px;
        background-color: rgb(24, 131, 42);
        overflow: hidden;        
    }
    &_thumb{
        background-color: rgb(203, 241, 195);
        height: 100%;
        position: absolute;
        left: 0;
    }
//     $clr_1: rgb(20, 177, 28);
// $clr_2: rgb(24, 131, 42);
// $clr_3: rgb(255,255,255);
// $clr_4: rgb(203, 241, 195);
// $clr_5: rgb(10, 10, 10);
}
</style>