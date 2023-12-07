<template>
  <div class="general-section">
    <div class="preloader-content">
      <span class="preloader-content__title">taxi job</span>
      <p class="preloader-content__desc">{{loc('phone:taxi:job:1')}}</p>
      <div class="preloader-content__main" v-if="!taxiJobData">
        <div class="block-left"></div>
        <div class="block-center"></div>
        <div class="block-right"></div>
      </div>
      <div class="car-anim" v-else>
        <div class="btn" @click="startWorkDay">{{loc('phone:taxi:job:2')}}</div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapState, mapMutations, mapGetters } from 'vuex'

export default {
  name: 'GeneralSection',

  computed: {
    ...mapState('smartphone', ['currentTab']),
    ...mapState('smartphone/taxiPage', ['taxiJobData']),
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    ...mapMutations('smartphone', ['setCurrentTab', 'setColorTheme']),
    ...mapMutations('smartphone/taxiPage', ['taxijob_setStateWork']),
    
    
    setProfile: function() {
      this.setCurrentTab({ key: this.currentTab.key, section: 'ProfileSection', data: null })
      this.taxijob_setStateWork({value: 'not working'})
    },

    startWorkDay: function() {
      window.mp.triggerServer('phone::taxijob::startWorkDay');
    }
  },

  mounted: function() {
    this.setColorTheme({ header: 'light', button: 'light' })
  }
}
</script>

<style lang="scss" scoped>
@keyframes fade-in {
  from {
    opacity: 0;
  }
  to {
    opacity: 1;
  }
}
@keyframes slide-in {
  from {
    transform: translateY(100%);
    opacity: 0;
  }
  to {
    transform: translateY(0);
    opacity: 1;
  }
}
@keyframes slide {
  from {
    transform: translateY(0);
  }
  to {
    transform: translateY(8rem);
  }
}
@keyframes car-anim {
  from { 
    transform: translateY(100%);
  }
  to {
    transform: translateY(0%);
  }
}
.general-section {
  height: 100%;
  .preloader-content {
    height: 100%;
    background: center / cover no-repeat url("/img/hud/smartphone/taxiJobTab/bg-main.png");
    text-align: center;
    padding: 5.5rem 0 0 0;
    color: #fff;
    display: flex;
    flex-flow: column;
    align-items: center;
    &__title {
      text-transform: uppercase;
      font-size: 4rem;
      line-height: 4rem;
      letter-spacing: 0.04em;
      font-weight: bold;
      text-transform: uppercase;
    }
    &__desc {
      font-size: 0.8rem;
      max-width: 11.5rem;
    }
    &__main {
      margin: 1.95rem 0 0 0;
      display: flex;
      .block-left {
        width: 5rem;
        height: 5rem;
        background: rgba(255, 255, 255, 0.2);
        margin: 3rem 0 0 0;
      }
      .block-center {
        width: 3rem;
        height: 3rem;
        background: #fff;
        animation: slide 0.8s linear infinite alternate;
      }
      .block-right {
        width: 3.75rem;
        height: 3.75rem;
        background: rgba(255, 255, 255, 0.5);
        margin: 3rem 0 0 0;
      }
    }
    .car-anim{
      width: 100%;
      height: 19.25rem;
      background-image: url('/img/hud/smartphone/taxiJobTab/car-anim.png');
      background-size: contain;
      background-repeat: no-repeat;
      position: absolute;
      background-position: bottom;
      bottom: 0;
      padding: 0 1rem 2.25rem 1rem;
      display: flex;
      align-items: flex-end;
      animation: car-anim .5s linear;
      .btn{
        display: flex;
        align-items: center;
        justify-content: center;
        width: 100%;
        height: 2.5rem;
        min-height: 2.5rem;
        background: #F1C115;
        border-radius: .5rem;
        font-size: .7rem;
        color: #FFFFFF;
        &:after{
          content: '';
          margin-left: .5rem;
          width: .7rem;
          height: .45rem;
          background-image: url('/img/hud/smartphone/taxiJobTab/Arrow.svg');
          background-size: contain;
          background-repeat: no-repeat;
        }
        &:hover{
          box-shadow: 0 .5rem 1.5rem rgba(244, 193, 11, 0.3);
          transition: .3s;
        }
      }
    }
  }
}
.img-enter-active {
  animation: fade-in 0.4s;
}
.slide-in-enter-active {
  animation: slide-in 0.4s;
}
// .slide-in-leave-active  {
//   animation: slide-in 0.4s reverse;
// }
</style>