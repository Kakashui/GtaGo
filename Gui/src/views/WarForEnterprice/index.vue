<template>
  <div class="war-for-enterprice">
    <div class="war-for-enterprice-body">
      <div class="war-for-enterprice__info">
        <div class="war-for-enterprice__title">
          {{ loc("war_for_enterprice_0") }}
          <span>{{ enterpricesCount }}/{{ enterpricesList.length }}</span>
        </div>
        <div class="war-for-enterprice__profit">
          <div class="war-for-enterprice__profit-factor" v-if="getFactor > 1">
            <span>X{{ getFactor }}</span>
          </div>
          <div class="war-for-enterprice__profit-info">
            <div class="war-for-enterprice__profit-desc">
              {{ loc("war_for_enterprice_1") }}
            </div>
            <div class="war-for-enterprice__profit-value">
              {{ getProfit.toLocaleString("ru") }} $
            </div>
          </div>
        </div>
      </div>
      <div class="war-for-enterprice__list">
        <EnterpriceItem
          v-for="item in enterpricesList"
          :key="item.id"
          :item="item"
        />
      </div>
    </div>
    <ExitButton />
  </div>
</template>

<script>
import { mapGetters, mapState } from "vuex";
import EnterpriceItem from "./components/EnterpriceItem";
import ExitButton from "./components/ExitButton";
export default {
  name: "WarForEnterprice",
  components: {
    EnterpriceItem,
    ExitButton,
  },
  computed: {
    ...mapState("warForEnterprice", ["enterpricesList", "owner", "ownerType", "profit"]),
    ...mapGetters("localization", ["loc"]),
    enterpricesCount() {
      return this.enterpricesList.filter(
        (element) =>
          element.orgId === this.owner && element.orgType == this.ownerType
      ).length;
    },
    getProfit() {
      return this.profit * this.enterpricesCount * this.getFactor;
    },
    getFactor() {
      return this.enterpricesCount >= 3 ? 2 : 1;
    },
  },
};
</script>

<style lang="scss" scoped>
.war-for-enterprice {
  width: 100vw;
  height: 100vh;
  position: absolute;
  left: 0;
  top: 0;
  background-image: url("/img/warForEnterprice/bg.png");
  background-size: cover;
  background-repeat: no-repeat;
  background-position: center;
  text-transform: uppercase;
  color: #fff;
  &:before,
  &:after {
    content: "";
    background-size: contain;
    background-repeat: no-repeat;
    background-position: bottom left;
    position: absolute;
    left: 0;
    bottom: 0;
    pointer-events: none;
  }
  &:before {
    width: 31rem;
    height: 30.75rem;
    background-image: url("/img/warForEnterprice/sparks.png");
    mix-blend-mode: lighten;
    animation: sparks 0.5s;
  }
  &:after {
    width: 18.7rem;
    height: 26.65rem;
    background-image: url("/img/warForEnterprice/money.png");
    animation: money 0.9s;
  }
  &-body{
    display: flex;
    align-items: center;
    justify-content: center;
    width: 100%;
    max-width: 1920px;
    height: 100%;
    margin: auto;
    position: relative;
  }
  &__info {
    height: 72vh;
    width: 25rem;
  }
  &__title {
    font-weight: normal;
    font-size: 4.5rem;
    line-height: 5.4rem;
    margin-right: 3rem;
    margin-bottom: 1.5rem;
    span {
      font-weight: bold;
      color: #b6d300;
      text-shadow: 0 0.5rem 4rem rgba(168, 195, 2, 0.6);
    }
  }
  &__profit {
    display: flex;
    align-items: flex-start;
    &-factor {
      font-size: 5rem;
      line-height: 5rem;
      margin-right: 0.8rem;
      position: relative;
      display: flex;
      align-items: center;
      justify-content: center;
      font-weight: 300;
      span {
        z-index: 1;
      }
      &:before {
        content: "";
        position: absolute;
        width: 5rem;
        height: 5rem;
        border-radius: 50%;
        background: #29fff2;
        filter: blur(3rem);
        z-index: 0;
      }
    }
  
    &-desc {
      font-size: 1.1rem;
      line-height: 1.3rem;
      color: rgba(255, 255, 255, 0.5);
      margin-bottom: 0.2rem;
    }
    &-value {
      font-size: 3rem;
      line-height: 3.6rem;
      color: #b6d300;
      font-weight: 600;
      text-shadow: 0 0.5rem 4rem rgba(168, 195, 2, 0.6);
    }
  }
  &__list {
    width: 49.75rem;
    min-width: 49.75rem;
    height: 72vh;
    min-height: 39rem;
    padding-right: 1rem;
    margin-right: -1rem;
    overflow-y: auto;
    display: grid;
    grid-template-columns: repeat(2, 1fr);
    grid-auto-rows: 12.5rem;
    grid-gap: 0.75rem;
    &::-webkit-scrollbar {
      width: 0.3rem;
      background-color: transparent;
      &-thumb {
        background: #29fff2;
        border-radius: 0.5rem;
      }
    }
  }
}

@keyframes sparks {
  from {
    transform: translateX(-5%) translateY(25%);
  }
  to {
    transform: translateX(0) translateY(0);
  }
  0% {
    opacity: 0;
  }
  35% {
    opacity: 0;
  }
  100% {
    opacity: 1;
  }
}
@keyframes money {
  from {
    transform: translateX(-100%) translateY(100%);
  }
  to {
    transform: translateX(0) translateY(0);
  }
}
</style>
