<template>
  <div class="lobbies__item"
       @mouseenter="active = true"
       @mouseleave="active = false"
       :style="{ 'background-image': `url(${background})` }">
    <div class="item__header">
      <div class="header__places">
        <img src="/img/arenaMenu/user.svg" class="header__icon" alt="user">
        <span class="places__current">{{ currentPlayers }}</span>
        <span class="places__separator">/</span>
        <span class="places__maximum">{{ lobbie.maxPlayers }}</span>
      </div>
    </div>
    <div class="header__desc">
      <span class="header__status">{{loc(lobbie.started ? 'arena:lobby:status:1' : 'arena:lobby:status:2')}}</span>
      <span class="header__title">{{ lobbie.title }}</span>
      <button
          v-show="!lobbie.started && active"
          class="item__btn"
          @click="toLobbie(lobbie.id)"
          :disabled=lobbie.isMapChange
      >
        <span class="btn__text">{{ loc('arena_dm_8') }}</span>
        <img class="btn_img" src="/img/arenaMenu/play.png">
      </button>
    </div>
  </div>
</template>

<script>
import { mapState, mapGetters, mapMutations } from 'vuex'

export default {
  data() {
    return {
      active: false,
    }
  },
        
  name: 'LobbiesItem',

  props: ['lobbie'],
        
  computed: {
    ...mapState('arenaMenu', ['backgrounds']),

    ...mapGetters('localization', ['loc']),

    currentPlayers: function () {
      return this.lobbie.redTeam.length + this.lobbie.greenTeam.length
    },

    background: function () {
      const rightItem = this.backgrounds.find(item => item.title === this.lobbie.title)
      return rightItem.image
    }
  },

  methods: {
    ...mapMutations('arenaMenu', [
      'setCurrentLobbieId',
      'setIsCreate'
    ]),
    
    toLobbie: function (id) {
      // this.setCurrentLobbieId(id)
      // this.setIsCreate(false)
      // if (this.lobbie.started) {
      //   // some method
      // } else {
      //   //window.mp.trigger('ARENA::OPEN::LOBBY::SERVER', id)
      // }
      this.setCurrentLobbieId(id)
      this.setIsCreate(false)
    }
  }
}
</script>

<style lang="scss" scoped>
.lobbies__item {
  width: calc(30% - 1rem);
  margin: .5rem;
  display: flex;
  flex-direction: column;
  background-size: cover;
  background-position: center;
  border-radius: .4rem;
  height: 21.5rem;
  padding: 1.5rem 1rem;
  position: relative;
  overflow: hidden;
  .header__title {
    text-transform: uppercase;
    font-weight: bold;
    margin-top: auto;
    margin-bottom: auto;
    font-size: 1.6rem;
  }
  .header__desc {
    //margin-top: 8.25rem;
    position: absolute;
    display: flex;
    flex-direction: column;
    align-items: flex-start;
    text-shadow: 0 .2rem .6rem rgba(0, 0, 0, 0.25);
    color: #FFF;
    bottom: 1rem;
  }
  .header__status {
    margin-top: auto;
    margin-bottom: auto;
    text-transform: uppercase;
    font-size: .9rem;
  }
  .item__header {
    width: 100%;
    display: flex;
    color: #fff;
    font-size: 1.1rem;
    justify-content: flex-end;
    align-items: center;
    z-index: 1;
    .header__places {
      left: 1.2rem;
      position: absolute;
     text-transform: uppercase;
      font-size: .9rem;
      text-shadow: 0 .2rem .6rem rgba(0, 0, 0, 0.25);
    }
    .header__icon {
      left: 1.2rem;
      width: .9rem;
      height: .9rem;
      margin: 0 0 0 .5rem;
    }
  }
  .item__btn {
    background: #B6D300;
    box-shadow: 0px 20px 90px rgba(168, 195, 2, 0.5);
    border-radius: 100px;
    width: 11rem;
    position: relative;
    height: 3rem;
    .btn__text {
      color: #FFF;
     text-transform: uppercase;
      font-size: 1rem;
      letter-spacing: 0.03em;
      font-weight: bold;
      margin-right: .6rem;
    }
    .btn_img {
      margin: auto;
      position: absolute;
      top: 0; bottom: 0;
      height: .9rem;
    }
    &:hover {
      box-shadow: 0 0 0 0;
    }
    &--watch {
      background-image: url('../../../../../public/img/arenaMenu/btn-bg-black.jpg');
      color: #fff;
  }
  }
}
</style>
