<template>
  <div class="passport">
    <span @click="exit" class="passport__close">&times;</span>
    <div class="passport__top">
      <span class="top__seria">{{ number }}</span>
      <div class="top__general">
        <img
          src="img/passport/seal_top.png"
          alt="seal"
          class="general__img"
        >
        <div class="general__state">
          <span class="state__title">State of</span>
          <span class="state__value">San andreas</span>
        </div>
        <div class="general__issue">
          <p class="issue__title">Date of issue</p>
          <span class="issue__date">{{ date }}</span>
        </div>
      </div>
    </div>
    <div class="passport__bottom">
      <div class="bottom__header">
        <div class="header__number">
          <span class="number__title">PASSPORT №</span>
          <span class="number__value">{{ number }}</span>
        </div>
        <span class="header__city">CITY OF LOS SANTOS</span>
        <div class="header__seria">
          <span class="seria__title">SERIES</span>
          <span class="seria__value">546784</span>
        </div>
      </div>
      <div class="bottom__main">
        <img
          src="img/passport/main_avatar.png"
          alt="avatar"
          class="main__avatar"
        >
        <div class="main__general-info">
          <div class="general-info__item surename">
            <p class="item__title">surename</p>
            <span class="item__value">{{ lastname }}</span>
          </div>
          <div class="general-info__item given-name">
            <p class="item__title">given name</p>
            <span class="item__value">{{ firstname }}</span>
          </div>
          <div class="general-info__item job">
            <p class="item__title">job</p>
            <span class="item__value">{{ loc(work) }}</span>
          </div>
          <div class="general-info__item employee">
            <p class="item__title">member</p>
            <span class="item__value">{{ loc(member) }}</span>
          </div>
          <div class="general-info__item sex">
            <p class="item__title">sex</p>
            <span class="item__value">{{ loc(gender) }}</span>
          </div>
        </div>
        <div class="main__other-info">
          <div class="other-info__item sex">
            <p class="item__title">Date of registration</p>
            <span class="item__value">{{ date }}</span>
          </div>
          <div class="other-info__item merry">
            <p class="item__title">Merried to</p>
            <span class="item__value">{{ loc(partner) }}</span>
          </div>
          <div class="other-info__images">
            <img
              src="img/passport/seal_bottom_first.png"
              alt="seal"
              class="item__img first"
            >
            <img 
              src="img/passport/seal_bottom_second.png"
              alt="seal"
              class="item__img second"
            >
          </div>
        </div>
      </div>
    </div>
    <div class="passport__footer">
      <span class="footer__info">LS/{{ lastname }}/{{ firstname }}/546784/{{ number }}</span>
    </div>
  </div>
</template>

<script>
import { mapState, mapGetters } from 'vuex'

export default {
  name: 'Passport',

  computed: {
    ...mapState('passport', [
      'number',
      'firstname',
      'lastname',
      'date',
      'gender',
      'member',
      'work',
      'partner'
    ]),
     ...mapGetters('localization',['loc'])
  },

  methods: {
    exit: function () {
      window.mp.trigger('passport:close')
    }
  }
}
</script>

<style lang="scss" scoped>
.passport {
  background-image: url('../../../public/img/passport/main_bg.png');
  background-size: cover;
  width: 436px;
  height: 600px;
  padding: 10px 46px;
  color: #7F6F6F;
  position: relative;
  &__close {
    position: absolute;
    top: 20px;
    right: 20px;
    background-color: rgb(134, 56, 56);
    width: 24px;
    height: 24px;
    border-radius: 50%;
    display: flex;
    align-items: center;
    justify-content: center;
    font-size: 24px;
    color: #fff;
  }
  &__top {
    display: flex;
    flex-direction: column;
    align-items: center;
    .top__seria {
      font-weight: bold;
      letter-spacing: 5px;
    }
    .top__general {
      display: flex;
      flex-direction: column;
      align-self: flex-start;
      margin: 20px 0 0 0;
      .general__img {
        align-self: flex-start;
      }
      .general__state {
        margin: 5px 0 0 0;
        display: flex;
        flex-direction: column;
        text-transform: uppercase;
        font-weight: bold;
      }
      .general__issue {
        margin: 45px 0 0 0;
        text-transform: uppercase;
        .issue__title {
          font-size: 9px;
        }
        .issue__date {
          font-weight: bold;
        }
      }
    }
  }
  &__bottom {
    margin: 72px 0 0 0;
    .bottom__header {
      display: flex;
      align-items: center;
      justify-content: space-between;
      .header__number {
        display: flex;
        .number__title {
          font-size: 8px;
          text-transform: uppercase;
        }
        .number__value {
          font-size: 8px;
          font-weight: bold;
          margin: 0 0 0 3px;
        }
      }
      .header__city {
        text-transform: uppercase;
        font-weight: bold;
      }
      .header__seria {
        display: flex;
        .seria__title {
          font-size: 8px;
          text-transform: uppercase;
        }
        .seria__value {
          font-size: 8px;
          font-weight: bold;
          margin: 0 0 0 3px;
        }
      }
    }
    .bottom__main {
      margin: 12px 0 0 0;
      display: flex;
      .main__avatar {
        align-self: flex-start;
      }
      .main__general-info {
        margin: 0 0 0 10px;
        height: 200px;
        width: 220px;
        display: flex;
        flex-direction: column;
        .general-info__item {
          display: flex;
          flex-direction: column;
          margin: 8px 0 0 0;
          &:first-child {
            margin: 0;
          }
          .item__title {
            font-size: 8px;
            text-transform: uppercase;
          }
        }
      }
      .main__other-info {
        display: flex;
        flex-direction: column;
        .other-info__item {
          display: flex;
          flex-direction: column;
          &.merry {
            margin: 8px 0 0 0;
          }
          .item__title {
            font-size: 8px;
            text-transform: uppercase;
          }
        }
        .other-info__images {
          align-self: flex-end;
          position: relative;
          width: 135px;
          height: 135px;
          .item__img {
            transform: translate(-50%, -50%);
            position: absolute;
            top: 50%;
            left: 50%;
          }
        }
      }
    }
  }
  &__footer {
    text-align: center;
    margin: 20px 0 0 0;
    .footer__info {
      font-size: 11px;
    }
  }
}
</style>
