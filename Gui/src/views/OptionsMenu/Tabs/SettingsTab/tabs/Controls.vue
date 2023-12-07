<template>
  <div class="settings-controls">
    <div
      class="settings-controls-item"
      v-for="(button, index) in buttons"
      :key="index"
      @click="changeButton(button)"
    >
      <div class="settings-controls-item-btn">{{ getTag(button.key) }}</div>
      <div class="settings-controls-item-tittle">{{ loc(button.text) }}</div>
      <img
        v-if="button.lock"
        src="/img/optionsMenu/settings/lock.svg"
        alt="lock"
        class="settings-controls-item-loc"
      />
    </div>
  </div>
</template>

<script>
import { mapState, mapMutations, mapGetters } from "vuex";
export default {
  name: "Controls",
  computed: {
    ...mapState("optionsMenu", ["buttons", "keyTags"]),
    ...mapGetters('localization', ['loc'])
  },
  methods: {
    ...mapMutations("optionsMenu", ["setDialog"]),
    getTag(key){
        for (const tKey in this.keyTags) {
                if(this.keyTags[tKey] == key) return tKey;
        }
        return "unk"
    },
    changeButton(button) {
      if (button.lock) return;
      this.setDialog({
        callback: (val) => {
          if(!val || val == '') {
              window.mp.trigger("cef:mmenu:action:key:wrang", button.name, val);
              return;
          }
          const exists = this.buttons.find((b) => b.key && b.key == val);
          if (exists)
            window.setData("notifyList/notify", {
              type: 1,
              position: 2,
              message: "mmain:control:key:exists",
              time: 3000,
            });
          else window.mp.trigger("cef:mmenu:action:key:bind", button.name, val);
        },
        value: "",
        tittle: `mmain:controls:key:tit`,
        subtittle: "mmain:controls:key:sub",
        bg: "invite",
        keyHandler: true,
      });
    },
  },
};
</script>

<style lang="scss" scoped>
.settings-controls {
  &-item {
    background: linear-gradient(
      270deg,
      rgba(41, 47, 51, 0.8) 0%,
      rgba(20, 21, 23, 0.8) 100%
    );
    border: 1px solid rgba(255, 255, 255, 0.05);
    box-sizing: border-box;
    display: flex;
    align-items: center;
    margin-bottom: 0.3rem;
    &:hover {
      background: linear-gradient(
        270deg,
        rgba(41, 47, 51, 1) 0%,
        rgba(20, 21, 23, 1) 100%
      );
    }
    &-btn {
      min-width: 3rem;
      padding: 0 .5rem;
      height: 3rem;
      display: flex;
      justify-content: center;
      align-items: center;
      background-color: #b6d300;
      border-radius: 2px;
      margin: 0.5rem;
    }
    &-loc {
      margin-left: auto;
      margin-right: 1.4rem;
    }
  }
}
</style>
