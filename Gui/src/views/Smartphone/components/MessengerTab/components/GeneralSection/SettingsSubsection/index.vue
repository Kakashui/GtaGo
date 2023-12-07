<template>
  <div class="settings-subsection">
    <div class="settings-subsection__header">{{loc('phone:setts:1')}}</div>
    <div class="settings-subsection__avatar"></div>
    <div class="settings-subsection__username">@{{ messengerData.CurrentAccount.Username }}</div>
    <div class="settings-subsection__number">+{{ configuration.Number }}</div>
    <div class="settings-subsection__field"></div>
    <div class="settings-subsection__toggle">
      <span class="title">{{loc('phone:setts:2')}}</span>
      <label :class="[labelClass, 'label']">
        <input
          type="checkbox"
          class="checkbox"
          v-model="messengerData.CurrentAccount.IsNumberHided"
          @click="switchNumberHided"
        >
        <span class="span"></span>
      </label>
    </div>
    <div class="settings-subsection__field">
      <span class="desc">{{loc('phone:setts:3')}}</span>
      <div class="action">
        <div class="action__text">
          <input
            v-if="isEdit"
            type="text"
            v-model="editName"
            class="input"
            v-focus
            maxlength="25"
            @keypress.enter="toggleIsEdit"
          >
          <div v-else class="field">{{ messengerData.CurrentAccount.DisplayedName }}</div>
        </div>
        <button class="action__btn" @click="toggleIsEdit">
          <img :src="`img/hud/smartphone/messengerTab/${isEdit ? 'check1' : 'edit1'}.svg`" class="icon">
        </button>
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapState } from "vuex";

export default {
  name: 'SettingsSubsection',

  data: function() {
    return {
      editName: 'WWWWWFJDHCMFG_IFODPWWWWNV',
      isEdit: false
    }
  },
  
  computed: {
    ...mapState('smartphone', ['configuration']),
    ...mapState('smartphone/messagePage', ['messengerData']),
    ...mapGetters('localization', ['loc']),
    
    labelClass: function() {
      return this.messengerData.CurrentAccount.IsNumberHided ? 'inactive' : 'active'
    }
  },

  methods: {
    switchNumberHided: function() {
      window.mp.triggerServer('phone::msg::switchNumberHided', !this.messengerData.CurrentAccount.IsNumberHided);
    },

    toggleIsEdit: function() {
      this.isEdit = !this.isEdit

      if (this.isEdit) {
        this.editName = this.messengerData.CurrentAccount.DisplayedName;
      }
      else {
        window.mp.triggerServer('phone::msg::renameDisplayedName', this.editName);
      }
    }
  },

  directives: {
    focus: {
      inserted: function (el) {
        el.focus()
      }
    }
  }
}
</script>

<style lang="scss" scoped>
.settings-subsection {
  background: #fff;
  display: flex;
  flex-direction: column;
  align-items: center;
  color: #020202;
  padding: 2.35rem 0 0 0;
  &__header {
    font-weight: 500;
    font-size: 0.7rem;
    line-height: 0.8rem;
    margin: 0 0 1rem 0;
  }
  &__avatar {
    margin: 0 0 0.5rem 0;
    &::after {
      content: "";
      width: 4rem;
      height: 4rem;
      background: center / cover no-repeat url("/img/hud/smartphone/messengerTab/avatar.png");
      display: block;
    }
  }
  &__username {
    font-size: 0.9rem;
    line-height: 1.05rem;
    margin: 0 0 0.4rem 0;
  }
  &__number {
    font-size: 0.65rem;
    line-height: 0.75rem;
    margin: 0 0 1.25rem 0;
  }
  &__field {
    margin: 0 0 0.75rem 0;
  }
  &__toggle {
    display: flex;
    align-items: center;
    justify-content: space-between;
    width: 100%;
    height: 2.55rem;
    border-bottom: 1px solid rgba(195, 195, 195, 0.35);
    border-top: 1px solid rgba(195, 195, 195, 0.35);
    padding: 0 0.75rem;
    .title {
      font-size: 0.7rem;
      font-weight: 300;
      color: #020202;
    }
    .label {
      flex: 0 0 2.5rem;
      height: 1.5rem;
      border-radius: 5rem;
      background: rgba(120, 120, 128, 0.32);
      position: relative;
      transition: all .15s;
      &.active {
        background: #32D74B;
      }
      &.inactive {
        background:#EE443A;
      }
      .checkbox {
        position: absolute;
        opacity: 0;
        z-index: -1;
        appearance: none;
      }
      .span {
        width: 1.3rem;
        height: 1.3rem;
        box-shadow: 0px 3px 8px rgba(0, 0, 0, 0.15), 0px 3px 1px rgba(0, 0, 0, 0.06);
        background-color: #fff;
        border-radius: 50%;
        transform: translate(-50%, -50%);
        position: absolute;
        top: 50%;
        left: 30%;
        transition: left .15s;
      }
      .checkbox:checked + .span {
        left: 70%;
      }
    }
  }
  &__field {
    width: 100%;
    background: rgba(195, 195, 195, 0.1);
    border-top: 1px solid rgba(195, 195, 195, 0.35);
    border-bottom: 1px solid rgba(195, 195, 195, 0.35);
    padding: 0.75rem;
    display: flex;
    flex-direction: column;
    .desc {
      color: rgba(2, 2, 2, 0.4);
      font-size: 0.65rem;
      line-height: 0.75rem;
      margin: 0 0 0.25rem 0;
    }
    .action {
      display: flex;
      justify-content: space-between;
      &__text {
        width: 12.1rem;
        height: 1.2rem;
        .input {
          font-size: 0.7rem;
          background: transparent;
          width: 100%;
          line-height: 1.2rem;
        }
        .field {
          font-size: 0.7rem;
          color: #020202;
          line-height: 1.5rem;
          width: 100%;
          white-space: nowrap;
          overflow: hidden;
          text-overflow: ellipsis;
        }
      }
      &__btn {
        background: transparent;
        width: 1rem;
        height: 1rem;
        align-self: flex-end;
        .icon {
          width: 100%;
          height: 100%;
        }
      }
    }
  }
}
</style>
