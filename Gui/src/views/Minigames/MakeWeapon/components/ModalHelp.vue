<template>
  <div class="modal-help">
    <a v-on:click.stop.prevent="closeModal()" class="modal-help__close"></a>
    <div class="modal-help__title">{{ this.getGunByIndex(this.currentGun).name }}</div>
    <div class="modal-help__img">
      <img v-bind:src="imgGunPath" />
    </div>
    <div class="modal-help__details">
      <div class="detail" v-bind:key="detail.id" v-for="detail of details">
        <div class="detail__name">{{ detail.name }}</div>
        <div class="detail__img">
          <img
            v-bind:src="
              '/img/miniGames/makeWeapon/details/' +
                'gun' +
                getGunByIndex(currentGun).id +
                '/' +
                'detail' +
                detail.id +
                '.png'
            "
          />
        </div>
      </div>
    </div>
  </div>
</template>
<script>
import { mapState, mapGetters, mapMutations } from 'vuex';
export default {
  name: 'ModalHelp',
  computed: {
    ...mapState('gameMakeWeapon/gameState', ['currentGun']),
    ...mapState('gameMakeWeapon/gunsData', ['details']),
    ...mapGetters('gameMakeWeapon/gunsData', ['getGunByIndex']),
    imgGunPath() {
      let gunNum = 'gun' + this.currentGun;
      let gunPath = '/img/miniGames/makeWeapon/guns/' + gunNum + '/' + gunNum + '-1111.png';
      return gunPath;
    },
  },
  methods: {
    ...mapMutations('gameMakeWeapon/pageState', ['setHelpVisibility']),
    playClick(){
        this.$store.commit("sounds/play", {"name": "dshop_click", "volume": .1});
    },
    closeModal() {
      this.setHelpVisibility(false);
      this.playClick();
    },
  }
};
</script>
<style scoped lang="scss">
.modal-help {
  position: fixed;
  top: 12rem;
  left: 50%;
  margin-left: -26.2em;
  width: 52.4rem;
  padding: 2.5rem;
  background: #08090b;
  z-index: 1000;
  box-shadow: 0px 0px 2.5rem rgba(255, 255, 255, 0.14);
  &__title {
    margin-bottom: 1rem;
    text-align: center;
    font-size: 2rem;
    //font-weight: 700;
    color: #fff;
    line-height: 2.3rem;
  }
  &__img {
    display: flex;
    align-items: center;
    justify-content: center;
    width: 34.3rem;
    height: 12.5rem;
    margin: 0.6rem auto 0;
    background: url('/img/miniGames/makeWeapon/help-gun-bg.png') center center no-repeat;
    img {
      width: 80%;
    }
  }
  &__details {
    display: flex;
    flex-wrap: nowrap;
    justify-content: space-between;
    .detail {
      width: 10.3rem;
      margin-top: 2rem;
      &__name {
        margin-bottom: 0.5rem;
        width: 100%;
        line-height: 1.6rem;
        background: url('/img/miniGames/makeWeapon/detail-name-bg.png') left bottom
          no-repeat;
        
        font-size: 1rem;
        //font-weight: 400;
        text-align: center;
      }
      &__img {
        display: flex;
        justify-content: center;
        align-items: center;
        width: 100%;
        height: 5.5rem;
        background: url('/img/miniGames/makeWeapon/help-detail-angles.png') center center
          no-repeat;
        background-size: contain;
        img {
          display: block;
          max-height: 3.7rem;
        }
      }
    }
  }
  &__close {
    display: block;
    position: absolute;
    top: 0;
    right: -2.5rem;
    width: 2rem;
    height: 2rem;
    background: url('/img/miniGames/makeWeapon/close-modal.png') center center no-repeat;
    background-size: contain;
    cursor: pointer;
  }
}
</style>
