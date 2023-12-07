<template>
  <div class="general-subsection">
    <div class="general-subsection__header">
      <div class="btn" @click="setCurrentTab({ key: currentTab.key, section: 'GeneralSection' })">
        <div class="btn__icon"></div>
      </div>
      <div class="title">{{loc('phone:bank:6')}}</div>
    </div>
    <div class="general-subsection__main">
      <div
        class="item"
        v-for="(item, index) in buttons"
        :key="index"
      >
        <button
          class="item-filled"
          v-if="getUnlockedButton(item.icon)"
          @click="setCurrentTab({ key: currentTab.key, section: currentTab.section, subsection: item.subsection })"
        >
          <div class="item-filled__icon">
            <div class="img" :style="{ '-webkit-mask': 'url(' + `img/hud/smartphone/bankTab/${item.icon}.svg` + ') no-repeat center / contain' }"></div>
          </div>
          <p class="item-filled__text">{{ loc(item.text) }}</p>
        </button>
        <div class="item-empty" v-else>
          <div class="item-empty__icon">
            <div class="img" :style="{ '-webkit-mask': 'url(' + `img/hud/smartphone/bankTab/${item.icon}.svg` + ') no-repeat center / contain' }"></div>
          </div>
          <p class="item-empty__text">{{ loc(item.text) }}</p>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapState, mapMutations, mapGetters } from 'vuex'

export default {
  name: 'ServicesSection',

  data: function() {
    return {
      buttons: [
        { text: 'phone:bank:sub:1', icon: 'house', subsection: 'HomeSubsection' },
        { text: 'phone:bank:sub:2', icon: 'capitol', subsection: 'BusinessSubsection' },
        { text: 'phone:bank:sub:3', icon: 'penalty', subsection: 'PenaltySubsection' },
        { text: 'phone:bank:sub:4', icon: 'heartbeat', subsection: 'PenaltySubsection' },
        { text: 'phone:bank:sub:5', icon: 'car', subsection: 'PenaltySubsection' },
        { text: 'phone:bank:sub:6', icon: 'smartphone', subsection: 'PenaltySubsection' },
      ]
    }
  },

  computed: {
    ...mapState('smartphone', ['currentTab']),
    ...mapState('smartphone/bankPage', ['maxHomeBalance', 'maxBusinessBalance', 'penaltySum']),
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    ...mapMutations('smartphone', ['setCurrentTab']),
    getUnlockedButton:function(icon) {
      switch (icon) {
        case 'house':
          return this.maxHomeBalance > 0;
        case 'capitol':
          return this.maxBusinessBalance > 0;
        case 'penalty':
          return this.penaltySum > 0;
      }
      return false;
    }
  }
}
</script>

<style lang="scss" scoped>
@keyframes shake {
  25% {
    transform: translateX(0.5rem);
  }
  50% {
    transform: translateX(0);
  }
  75% {
    transform: translateX(0.25rem);
  }
}
.general-subsection {
  padding: 3.35rem 1rem 0 1rem;
  &__header {
    position: relative;
    text-align: center;
    .btn {
      position: absolute;
      transform: translate(0, -50%);
      top: 50%;
      left: 0.25rem;
      width: 2rem;
      height: 2rem;
      background: linear-gradient(0deg, rgba(255, 255, 255, 0.7), rgba(255, 255, 255, 0.7));
      border-radius: 0.4rem;
      display: flex;
      align-items: center;
      justify-content: center;
      &:hover {
        .btn__icon {
          animation: shake 0.4s;
        }
      }
      &__icon {
        mask: url('/img/hud/smartphone/arrowLeft.svg') no-repeat center / contain;
        width: 0.4rem;
        height: 0.6rem;
        background: #273B40;
      }
    }
    .title {
      font-size: 0.7rem;
      color: #273B40;
      font-weight: 500;
    }
  }
  &__main {
    margin: 1.6rem 0 0 0;
    display: grid;
    grid-template-columns: repeat(2, 6.25rem);
    grid-gap: 0.7rem;
    .item {
      height: 6.25rem;
      border-radius: 0.6rem;
      overflow: hidden;
      &:nth-child(3n) {
        grid-column-start: 1;
        grid-column-end: 3;
        height: 4rem;
        .item-empty {
          flex-direction: row;
          align-items: center;
          justify-content: flex-start;
          &__text {
            margin: 0 0 0 0.7rem;
          }
          // background: center / cover no-repeat url("/img/hud/smartphone/bankTab/empty1.png");
        }
        .item-filled {
          flex-direction: row;
          align-items: center;
          justify-content: flex-start;
          &__text {
            margin: 0 0 0 0.7rem;
          }
        }
      }
      &-empty {
        height: 100%;
        width: 100%;
        background: #fff;
        box-shadow: 0px 1.5rem 2.5rem rgba(49, 41, 184, 0.1);
        display: flex;
        flex-direction: column;
        justify-content: space-between;
        padding: 0.75rem;
        &__icon {
          width: 2.5rem;
          height: 2.5rem;
          border-radius: 50%;
          background-color: rgb(184, 184, 184);
          display: flex;
          align-items: center;
          justify-content: center;
          .img {
            width: 1.1rem;
            height: 1.1rem;
            background: #fff;
          }
        }
        &__text {
          text-align: left;
          font-size: 0.7rem;
        }
      }
      &-filled {
        height: 100%;
        width: 100%;
        background: #fff;
        box-shadow: 0px 1.5rem 2.5rem rgba(49, 41, 184, 0.1);
        display: flex;
        flex-direction: column;
        justify-content: space-between;
        padding: 0.75rem;
        &__icon {
          width: 2.5rem;
          height: 2.5rem;
          border-radius: 50%;
          background: center / cover no-repeat url("/img/hud/smartphone/bankTab/texture1.png");
          display: flex;
          align-items: center;
          justify-content: center;
          .img {
            width: 1.1rem;
            height: 1.1rem;
            background: #fff;
          }
        }
        &__text {
          text-align: left;
          font-size: 0.7rem;
        }
        &:hover {
          background: center / cover no-repeat url("/img/hud/smartphone/bankTab/texture1.png");
          .item-filled__text {
            color: #fff;
          }
          .item-filled__icon {
            background: #fff;
            .img {
              background: #71E5A8;
            }
          }
        }
      }
    }
  }
}
</style>
