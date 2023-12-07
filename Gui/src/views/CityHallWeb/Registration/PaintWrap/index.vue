<template>
  <div class="paint-wrap">
    <div class="paint-wrap__desk">
      <div class="title">{{loc('cityHallWeb_95')}}</div>
      <div class="desc">{{loc('cityHallWeb_96')}}</div>
      <div class="board">
        <div 
          class="btn btn-cancel" 
          id="clear"
          @click="cancelEdit"
        ></div>
        <canvas 
          id='canvas'
          width="2500"
          height="999"
          @click="applyEdit"
        ></canvas>
      </div>
      <div 
        class="btn btn-next"
        v-if="isEdit"
        @click="registrateUser"
      >{{loc('cityHallWeb_97')}}</div>
    </div>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
export default {
  name: 'PaintWrap',

  data: function() {
    return{
      isEdit: false
    }
  },

  computed: {
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    applyEdit: function() {
      this.isEdit = true
    },
    cancelEdit: function() {
      this.isEdit = false
    },
    registrateUser: function() {
      window.mp.trigger('cityHallWeb:registrateUser')
    }
  },

  mounted(){
    let canvas = document.getElementById("canvas")
    let context = canvas.getContext("2d")

    context.lineCap = "round"
    context.lineWidth = 1

    document.getElementById("clear").onclick = function clear() {
      context.clearRect(0, 0, canvas.width, canvas.height)
    }

    canvas.onmousemove = function drawIfPressed (e) {
      let x = e.offsetX
      let y = e.offsetY
      let dx = e.movementX
      let dy = e.movementY

      if (e.buttons > 0) {
        context.beginPath()
        context.moveTo(x, y)
        context.lineTo(x - dx, y - dy)
        context.stroke()
        context.closePath()
      }
    }
  }
}
</script>

<style lang="scss" scoped>
.paint-wrap{
  display: flex;
  align-items: center;
  justify-content: center;
  width: 100%;
  height: 100%;
  position: absolute;
  left: 0;
  top: 0;
  background: rgba(0, 0, 0, 0.76);
  &__desk{
    width: 56.3rem;
    height: 38.1rem;
    background: #FFFFFF;
    border-radius: 1rem;
    display: flex;
    flex-flow: column;
    align-items: center;
    padding: 2.5rem 3.5rem;
    .title{
      font-family: Merriweather;
      font-style: normal;
      font-weight: bold;
      font-size: 1.7rem;
      line-height: 2.15rem;
      margin-bottom: .45rem;
      text-transform: uppercase;
      color: #000000;
    }
    .desc{
      font-family: Roboto;
      font-style: normal;
      font-weight: normal;
      font-size: .8rem;
      line-height: .95rem;
      color: #46555F;
      margin-bottom: 2rem;
    }
    .board{
      width: 100%;
      height: 22rem;
      border: .1rem dashed rgba(70, 85, 95, 0.4);
      border-radius: .3rem;
      position: relative;
      overflow: hidden;
      canvas{
        overflow: hidden;
        vertical-align: top;
      }
      .btn-cancel{
        position: absolute;
        left: 1rem;
        top: 1rem;
        width: 1.7rem;
        height: 1.7rem;
        background-size: contain;
        background-repeat: no-repeat;
        background-position: center;
        background-image: url('/img/cityHallWeb/icon-refresh.svg');
      }
    }
    .btn-next{
      padding: 0 6rem;
    }
  }
}
</style>
