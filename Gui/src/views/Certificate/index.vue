<template>
  <div 
    :class="[{rotate: !frontEnd}, 'certificate']"
    @click="turnOver"
  > 
    <div
      class="certificate__front"
      :style="{backgroundImage: 'url(/img/certificate/'+ key +'-front.png)'}"
    >
    </div>
    <div
      class="certificate__back"
      :style="{backgroundImage: 'url(/img/certificate/'+ key +'-back.png)'}"
    >
      <div class="info-main">
        <div class="info-main__block">
          <div class="desc">{{loc('Certificate_0')}}</div>
          <div class="value">{{firstName}}</div>
        </div>
        <div class="info-main__block">
          <div class="desc">{{loc('Certificate_1')}}</div>
          <div class="value">{{lastName}}</div>
        </div>
        <div class="info-main__block">
          <div class="desc">ID</div>
          <div class="value">{{id}}</div>
        </div>
        <div class="info-main__block">
          <div class="desc">{{loc('Certificate_2')}}</div>
          <div class="value">{{post}}</div>
        </div>
      </div>
      <div class="info-signature">
        <div class="info-signature__value" style="background-image: url(/img/certificate/signature.svg)"></div>
        <div class="info-signature__desc">{{loc('Certificate_3')}}</div>
      </div>
    </div>
    <div class="prompt">{{loc('Certificate_4')}}</div>
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
export default {
  name: 'Certificate',

  data: function() {
    return {
      frontEnd: true 
    }
  },

  methods: {
    turnOver: function() {
      this.frontEnd = !this.frontEnd
    }
  },

  computed: {
    ...mapState('certificate', ['key', 'firstName', 'lastName', 'id', 'post']),
    ...mapGetters('localization', ['loc'])
  }
}
</script>

<style lang="scss" scoped>
// @keyframes isShow {
//   0%{
//     transform: rotateY(-90deg);
//     opacity: 0;

//   }
//   50%{
//     transform: rotateY(-90deg);
//     opacity: 1;
//   }
//   100%{
//     transform: rotateY(0deg);
//     opacity: 1;
//   }
// }
// @keyframes isLeave {
//   0%{
//     transform: rotateY(0deg);
//     opacity: 1;
//   }
//   50%{
//     transform: rotateY(90deg);
//     opacity: 1;
//   }
//   100%{
//     transform: rotateY(90deg);
//     opacity: 0;
//   }
// }

.certificate{
  display: flex;
  position: absolute;
  top: 50%;
  left: 50%;
  transform-style: preserve-3d;
  perspective: 50rem;
  font-family: Roboto;
  color: #fff;
  width: 33.7rem;
  height: 19.2rem;
  transform: translateX(-50%) translateY(-50%);
  display: flex;
  align-items: center;
  justify-content: center;
  transform-origin: 50% 50%;
  &.rotate{
    .certificate__front{
      transform: rotateY(-180deg);
    }
    .certificate__back{
      transform: rotateY(0deg);
    }
  }
  &__front, &__back{
    background-position: center;
    background-size: contain;
    background-repeat: no-repeat;
    border-radius: 1.1rem;
    overflow: hidden;
    position: absolute;
    width: 100%;
    height: 100%;
    transform-origin: 50% 50%;
    backface-visibility: hidden;
    transition: .5s;
  }
  &__back{
    padding: 5.35rem 3.5rem 2.5rem 3.5rem;
    display: flex;
    flex-flow: column;
    transform: rotateY(180deg);
    .info-main{
      display: grid;
      width: 100%;
      margin-left: 9.1rem;
      margin-bottom: 1.35rem;
      grid-template-columns: 6.3rem 10.7rem;
      grid-auto-rows: 2.2rem;
      grid-row-gap: 2rem;
      grid-column-gap: .5rem;
      &__block{
        display: flex;
        flex-flow: column;
        align-items: flex-start;
        justify-content: space-between;
        font-size: .8rem;
        .desc{
          color: rgba(255, 255, 255, 0.6);
        }
        .value{
          font-weight: bold;
        }
      }
    }
    .info-signature{
      width: 100%;
      display: flex;
      align-items: flex-start;
      justify-content: flex-start;
      padding-top: .85rem;
      border-top: 1px solid rgba(255, 255, 255, 0.1);
      &__value{
        border: 1px solid rgba(255, 255, 255, 0.1);
        width: 6.8rem;
        height: 2.75rem;
        background-size: contain;
        background-position: center;
        background-repeat: no-repeat;
        margin-right: .5rem;
      }
      &__desc{
        font-size: .7rem;
        line-height: .8rem;
        color: rgba(255, 255, 255, 0.6);
        max-width: 10rem;
      }
    }
  }
  .prompt{
    text-transform: uppercase;
    font-size: .8rem;
    line-height: .9rem;
    position: absolute;
    bottom: -.6rem;
    left: 50%;
    transform: translateY(100%) translateX(-50%);
  }
}
</style>
