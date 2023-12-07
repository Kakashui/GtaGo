<template>
  <div class="in-developing">
    <span>Oops...</span>
    <div class="gears">
      <div class="small-gear"></div>
      <div class="main-gear-wrap">
        <div class="main-gear"></div>
        <div class="ring"></div>
        <div class="central-lock"></div>
      </div>
      <div class="small-gear"></div>
    </div>
    <div class="desc">{{loc('chall:comp:indev')}}</div>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
export default {
  computed: {
    ...mapGetters('localization', ['loc'])
  },
}
</script>

<style lang="scss" scoped>
@keyframes rotateSmall{
  from{
    transform: rotate(0);
  }
  to{
    transform: rotate(360deg);
  }
}

@keyframes rotateMain{
  from{
    transform: rotate(0);
  }
  to{
    transform: rotate(-360deg);
  }
}

.in-developing{
  display: flex;
  align-items: center;
  justify-content: center;
  flex-flow: column;
  background: rgba(41, 51, 64, 0.02);
  width: 29.15rem;
  height: 29.15rem;
  border-radius: 50%;
  font-family: Merriweather;
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translateX(-50%) translateY(-50%);
  span{
    margin-bottom: 2.3rem;
    font-weight: bold;
    font-size: 4rem;
    line-height: 5.05rem;
    color: #E00B29;
  }
  .gears{
    display: flex;
    align-items: flex-start;
    justify-content: center;
    margin-bottom: 1.9rem;
    .small-gear{
      width: 4.9rem;
      height: 4.9rem;
      background-size: contain;
      background-repeat: no-repeat;
      background-position: center;
      transform-origin: 50% 50%;
      background-image: url('/img/cityHallWeb/small-gear.svg');
      animation: rotateSmall 1s linear infinite;
    }
    .main-gear-wrap{
      margin: 1.3rem .8rem 0 .8rem;
      width: 10.9rem;
      height: 10.9rem;
      position: relative;
      display: flex;
      align-items: center;
      justify-content: center;
      .main-gear, .ring, .central-lock{
        background-size: contain;
        background-repeat: no-repeat;
        background-position: center;
        transform-origin: 50% 50%;
        position: absolute;
      }
      .main-gear{
        width: 100%;
        height: 100%;
        background-image: url('/img/cityHallWeb/main-gear.svg');
        animation: rotateMain 2s linear infinite;
      }
      .ring{
        width: 5.8rem;
        height: 5.8rem;
        background-image: url('/img/cityHallWeb/ring.svg');
        animation: rotateSmall 1.5s linear infinite;
      }
      .central-lock{
        width: 4.2rem;
        height: 4.2rem;
        background-image: url('/img/cityHallWeb/central-lock.svg');
      }
    }
  }
  .desc{
    font-weight: normal;
    white-space: pre;
    text-align: center;
    font-size: 1.5rem;
    line-height: 2.5rem;
    color: #293340;
  }
}
</style>
