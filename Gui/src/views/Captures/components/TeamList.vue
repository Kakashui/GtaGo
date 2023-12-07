<template>
  <div class="team-list">
    <div class="header">
      <div class="nickname">{{ loc("captmenu_7") }}</div>
      <div class="rang">{{ loc("captmenu_8") }}</div>
    </div>
    <div class="team-list_list">
      <TeamListItem
        v-for="(item, index) in teamList"
        :key="index"
        :index="index"
        :item="item"
      />
    </div>
    <div class="btns-group">
      <div
        class="btn btn-apply"
        v-if="capturing.attackStatus === 'we'"
        @click="attack"
      >
        {{ loc("captmenu_9") }}
      </div>
      <div
        class="btn btn-apply"
        v-if="capturing.attackStatus === 'us'"
        @click="protect"
      >
        {{ loc("captmenu_10") }}
      </div>
      <div class="btn btn-cancel" @click="cancel">{{ loc("captmenu_11") }}</div>
    </div>
  </div>
</template>

<script>
import { mapState, mapGetters } from "vuex";

import TeamListItem from "./TeamListItem";

export default {
  name: "TeamList",

  components: {
    TeamListItem,
  },

  computed: {
    ...mapState("captures", ["capturing", "teamList"]),
    ...mapGetters("localization", ["loc"]),
  },

  methods: {
    attack: function () {
      window.mp.trigger("capt:attack");
    },
    protect: function () {
      window.mp.trigger("capt:deff");
    },
    cancel: function () {
      window.mp.trigger("capt:closeMenu");
    },
  },
};
</script>

<style lang="scss" scoped>
.team-list {
	height: 35rem;
  width: 27rem;
  margin: 2rem 0 0 5rem;
  position: relative;
  z-index: 1;
  overflow: hidden;
  .header {
    display: flex;
    align-items: flex-start;
    justify-content: flex-start;
    width: 100%;
    color: rgba(255, 255, 255, 0.4);
    font-size: 0.8rem;
    padding: 0 0.9rem 1.25rem 0;
    .nickname {
      padding-left: 0.8rem;
      width: 44%;
    }
    .rang {
      width: 37%;
    }
  }
  &_list {
		overflow-y: scroll;
    padding-right: 0.75rem;
    height: 75%;
    &::-webkit-scrollbar-track {
      border-radius: 0.25rem;
    }
    &::-webkit-scrollbar {
      width: 0.2rem;
    }
    &::-webkit-scrollbar-thumb {
      border-radius: 0.25rem;
      background: rgba(255, 255, 255, 0.15);
    }
  }
  .btns-group {
    margin-top: 3rem;
    width: 100%;
    display: flex;
    align-items: center;
    position: relative;
    &:before {
      content: "";
      width: 100%;
      height: 1px;
      position: absolute;
      top: -1.8rem;
      background: rgba(255, 255, 255, 0.2);
    }
    .btn {
      display: flex;
      align-items: center;
      justify-content: center;
      width: 100%;
      border-radius: 0.1rem;
      margin-right: 2.5rem;
      height: 2.75rem;
      min-height: 2.75rem;
      font-weight: bold;
      font-size: 0.9rem;
      color: #ffffff;
      &:last-child {
        margin-right: 0;
      }
      &:hover {
        transition: 0.3s;
      }
      &.btn-apply {
        background: linear-gradient(95.13deg, #10cd45 0%, #1e7d39 100%);
        &:hover {
          box-shadow: 0 1rem 2rem rgba(193, 231, 4, 0.3);
        }
      }
      &.btn-cancel {
        background: linear-gradient(95.13deg, #db4733 0%, #de331d 100%);
        &:hover {
          box-shadow: 0px 1rem 2rem rgba(219, 71, 51, 0.3);
        }
      }
    }
  }
}
</style>