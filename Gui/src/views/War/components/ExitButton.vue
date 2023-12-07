<template>
  <div class="exit-button">
    <div class="exit-button__info">
      <div class="exit-button__info-desc">{{loc('war_8')}}</div>
      <div class="exit-button__info-value">{{loc('war_9')}}</div>
    </div>
    <div 
      class="exit-button__img"
      @click="exitFromWar"
    >
      <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20" fill="none">
        <g clip-path="url(#clip0)">
        <path d="M11.8323 10.0164L19.6199 2.22853C20.1267 1.72196 20.1267 0.902911 19.6199 0.396344C19.1134 -0.110223 18.2943 -0.110223 17.7878 0.396344L9.9999 8.18419L2.21229 0.396344C1.70548 -0.110223 0.886671 -0.110223 0.380104 0.396344C-0.126701 0.902911 -0.126701 1.72196 0.380104 2.22853L8.16772 10.0164L0.380104 17.8042C-0.126701 18.3108 -0.126701 19.1298 0.380104 19.6364C0.632557 19.8891 0.964495 20.016 1.2962 20.016C1.6279 20.016 1.9596 19.8891 2.21229 19.6364L9.9999 11.8486L17.7878 19.6364C18.0404 19.8891 18.3721 20.016 18.7038 20.016C19.0355 20.016 19.3672 19.8891 19.6199 19.6364C20.1267 19.1298 20.1267 18.3108 19.6199 17.8042L11.8323 10.0164Z" fill="white"/>
        </g>
        <defs>
        <clipPath id="clip0">
        <rect width="20" height="20" fill="white"/>
        </clipPath>
        </defs>
      </svg>
    </div>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
export default {
  name: 'ExitButton',
  computed: {
    ...mapGetters('localization', ['loc'])
  },
  methods: {
    exitFromWar: function() {
      window.mp.trigger('familyMp::statMenu::exit')
    }
  }
}
</script>

<style lang="scss" scoped>
.exit-button{
  display: flex;
  align-items: center;
  position: absolute;
  right: 2.5rem;
  top: 2.5rem;
  z-index: 1;
  text-transform: uppercase;
  &__info{
    display: flex;
    flex-direction: column;
    align-items: flex-end;
    margin-right: 1rem;
    letter-spacing: 0.03em;
    font-weight: bold;
    &-desc{
      font-size: .9rem;
      color: rgba(255, 255, 255, 0.5);
      margin-bottom: .1rem;
    }
    &-value{
      font-size: 1.2rem;
      color: #FFFFFF;
    }
  }
  &__img{
    width: 2.5rem;
    height: 2.5rem;
    display: flex;
    align-items: center;
    justify-content: center;
    background: rgba(255, 255, 255, 0.1);
    border: 1px solid rgba(255, 255, 255, 0.1);
    transition: .2s;
    svg{
      width: 1rem;
      height: 1rem;
    }
    &:hover{
      border: 1px solid rgba(255, 255, 255, 0.2);
      background: rgba(255, 255, 255, 0.2);
    }
  }
}
</style>
