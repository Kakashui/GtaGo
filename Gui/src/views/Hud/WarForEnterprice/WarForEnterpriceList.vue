<template>
  <div class="war-for-enterprice-list">
    <template v-if="listIsCollapsed && captureList.length > 1">
      <WarForEnterpriceListItem
        v-for="item in [captureList[0]]"
        :key="item.id"
        :item="item"
      />
      <div class="war-for-enterprice-list__prompt">
        <div class="war-for-enterprice-list__prompt__desc">+ {{captureList.length - 1}} {{loc('war_for_enterprice_7')}}</div>
        <div class="war-for-enterprice-list__prompt__value">{{loc('war_for_enterprice_8')}}<span>0</span></div>
      </div>
    </template>
    <WarForEnterpriceListItem
      v-else
      v-for="item in captureList"
      :key="item.id"
      :item="item"
    />
  </div>
</template>

<script>
import { mapGetters, mapMutations, mapState } from "vuex";
import WarForEnterpriceListItem from "./components/WarForEnterpriceListItem";
export default {
  name: "WarForEnterpirceList",
  components: {
    WarForEnterpriceListItem,
  },
  data() {
    return {
      listIsCollapsed: true,
      timer: null,
    };
  },
  computed: {
    ...mapState("hud/warForEnterprice", ["captureList", "keyCode"]),
    ...mapGetters('localization', ['loc']),
  },
  methods: {
    ...mapMutations("hud/warForEnterprice", ["SetCurrentTime"]),
    currentTimer() {
      console.log('createTimer')
      this.timer = setInterval(() => {
        this.SetCurrentTime()
      }, 1000)
    },
    collapseList(e) {
      if (e.keyCode === this.keyCode) {
        this.listIsCollapsed = !this.listIsCollapsed
      }
    }
  },
  created() {
    this.currentTimer();
    window.addEventListener('keypress', this.collapseList);
  },
  beforeDestroy() {
    clearInterval(this.timer)
  },
  destroyed() {
    window.removeEventListener('keypress', this.collapseList);
  },
};
</script>

<style lang="scss" scoped>
.war-for-enterprice-list {
  display: flex;
  align-items: flex-start;
  justify-content: center;
  flex-direction: column;
  position: absolute;
  right: 19rem;
  min-width: 10.7rem;
  top: 0;
  padding-top: 1.5rem;
  text-transform: uppercase;
  color: #ffffff;
  min-width: 11.15rem;
  &:before {
    content: "";
    position: absolute;
    top: 0;
    left: 50%;
    transform: translateX(-50%);
    width: 20rem;
    height: 7.5rem;
    background: radial-gradient(
      37.34% 100% at 51.87% 0%,
      #369e98 0%,
      rgba(54, 158, 152, 0) 100%
    );
    z-index: 0;
  }
  &__prompt{
    display: flex;
    flex-flow: column;
    margin-left: 3.65rem;
    width: 7.5rem;
    padding-top: .5rem;
    border-top: .1rem solid rgba(255, 255, 255, 0.2);
    font-size: 1rem;
    line-height: 1rem;
    &__desc{
      font-weight: bold;
      color: #29FFF2;
      margin-bottom: .1rem;
    }
    &__value{
      display: flex;
      align-items: center;
      span{
        margin-left: .5rem;
        width: 1.3rem;
        height: 1rem;
        display: flex;
        align-items: center;
        justify-content: center;
        font-size: .8rem;
        line-height: .95rem;
        color: #000000;
        background: #FFFFFF;
        border: 1px solid #FFFFFF;
        box-sizing: border-box;
        box-shadow: inset 0 -.1rem .1rem rgba(0, 0, 0, 0.29);
        border-radius: .2rem;
      }
    }
  }
}
</style>
