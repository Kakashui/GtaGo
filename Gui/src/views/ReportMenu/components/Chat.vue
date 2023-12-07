<template>
  <div class="chat">
    <template v-if="currentChat !== null">
      <div class="chat__header">
        <div class="btns-wrap">
          <div class="btn btn-turn" @click="setCurrentChat(null)"></div>
          <div
            class="btn btn-close"
            v-if="dynamic"
            @click="setShowModal('closeReport')"
          ></div>
        </div>
        <div class="chat__header-player">{{ currentChat.reporterName }} <span class="id">[{{ currentChat.reporterId }}]</span>
        </div>
        <div class="chat__header-interaction" v-if="dynamic">
          <div
            class="btn"
            v-for="item in quickActions"
            :key="item.key"
            @click="setAction(item.type, item.key)"
          >
            {{ loc(item.name) }}
          </div>
        </div>
        <div class="chat__header-reputation" v-else>
          <div
            :class="[{ active: item <= currentChat.rating }, 'star']"
            v-for="item in 5"
            :key="item.id"
          ></div>
        </div>
      </div>
      <div class="chat__body">
        <div class="messages-list scroll">
          <div
            :class="[
              { player: item.sender === currentChat.reporterName },
              'message',
            ]"
            v-for="(item) in currentChat.messages"
            :key="item.key"
          >
            <div class="text">{{ loc(item.text) }}</div>
            <div class="message-sender" v-if="item.sender === mySettings.name">
              {{ loc("ReportMenu_9") }}
            </div>
            <div
              class="message-sender"
              v-else-if="item.sender !== currentChat.reporterName"
            >
              {{ loc("ReportMenu_10") }} ({{ item.sender }})
            </div>
            <!-- <div
              class="btn-delete"
              v-if="
                item.sender !== currentChat.reporterName && !currentChat.rating
              "
              @click="deleteMessage(currentChat.id, index)"
            >
              {{ loc("ReportMenu_11") }}
            </div> -->
          </div>
        </div>
        <div class="input-wrap" v-if="dynamic">
          <input
            type="text"
            :placeholder="loc('ReportMenu_22')"
            v-model="currentMessageModel"
            @keydown.enter="sendMessage"
          />
          <div 
            class="btn btn-send" 
            @click="sendMessage"
          ></div>
        </div>
        <div class="input-wrap" v-else>
          <div class="input-text">{{ loc("ReportMenu_12") }}.</div>
          <div class="btn btn-send locked"></div>
        </div>
      </div>
    </template>
  </div>
</template>

<script>
import { mapGetters, mapMutations, mapState } from "vuex";
export default {
  name: "Chat",

  props: {
    dynamic: Boolean,
  },

  computed: {
    ...mapState("reportMenu", ["quickActions", "currentChat", "mySettings", "messageModel"]),
    ...mapGetters("localization", ["loc"]),
    currentMessageModel: {
      get: function() {
        return this.messageModel
      },
      set: function(value) {
        this.setMessageModel(value)
      }
    }
  },

  methods: {
    ...mapMutations("reportMenu", ["setCurrentChat", "setMessageModel"]),
    deleteMessage: function (id, index) {
      window.mp.trigger("reportMenu:deleteMessage", id, index);
    },
    setAction: function (type, index) {
      if(type == "position"){
        window.mp.trigger("reportMenu:position", this.currentChat.id);
      }else if (type === "kick") {
        this.setShowModal(type);
      } else {
        window.mp.trigger("reportMenu:hotkeys", this.currentChat.id, index);
      }
    },
    setShowModal: function (type) {
      this.$emit("showModal", type);
    },
    sendMessage: function () {
      if (this.currentMessageModel != null && this.currentMessageModel != "") {
        window.mp.trigger(
          "reportMenu:sendmessage",
          this.currentChat.id,
          this.currentMessageModel
        );
        this.currentMessageModel = null;
      }
    },
  },
};
</script>

<style lang="scss" scoped>
@keyframes show {
  from {
    opacity: 0;
  }
  to {
    opacity: 1;
  }
}
.chat {
  display: flex;
  flex-flow: column;
  width: 50.4rem;
  min-width: 50.4rem;
  height: 100%;
  position: relative;
  &:before,
  &:after {
    content: "";
    width: 1px;
    height: 100%;
    position: absolute;
    top: 0;
    background: radial-gradient(
      50% 50% at 50% 50%,
      rgba(255, 255, 255, 0.65) 0%,
      rgba(255, 255, 255, 0) 100%
    );
  }
  &:before {
    left: -1px;
  }
  &:after {
    right: 0;
  }
  &__header,
  &__body {
    animation-name: show;
    animation-duration: 0.3s;
  }
  &__header {
    display: flex;
    flex-flow: column;
    align-items: flex-start;
    justify-content: space-between;
    height: 9.2rem;
    min-height: 9.2rem;
    padding: 1.9rem 2.85rem;
    background: rgba(38, 40, 46, 0.25);
    position: relative;
    .btns-wrap {
      display: flex;
      align-items: center;
      justify-content: center;
      position: absolute;
      top: 1.4rem;
      right: 1.1rem;
      .btn {
        margin-left: 1rem;
        width: 1.95rem;
        height: 1.95rem;
        opacity: 0.25;
        &:hover {
          opacity: 0.6;
        }
        &:first-child {
          margin-left: 0;
        }
      }
      .btn-close {
        background-image: url("/img/reportMenu/btn-close.svg");
      }
      .btn-turn {
        &:after {
          content: "";
          height: 0.2rem;
          width: 1.7rem;
          background: #fff;
        }
      }
    }
    &-player {
      font-size: 2.05rem;
      line-height: 119.5%;
      display: flex;
      align-items: center;
      white-space: pre;
      .id {
        color: rgba(255, 255, 255, 0.45);
      }
    }
    &-interaction {
      display: flex;
      align-items: center;
      .btn {
        background: rgba(231, 4, 4, 0.15);
        border: 1px solid #e70404;
        box-sizing: border-box;
        border-radius: 0.5rem;
        font-weight: bold;
        font-size: 1.05rem;
        line-height: 1.05rem;
        letter-spacing: 0.05em;
        padding: 0.6rem 1.2rem;
        margin-right: 0.65rem;
        opacity: 0.35;
        &:hover {
          opacity: 1;
        }
      }
    }
    &-reputation {
      display: flex;
      align-items: center;
      .star {
        width: 1.9rem;
        height: 1.9rem;
        background-size: contain;
        background-position: center;
        background-repeat: no-repeat;
        background-image: url("/img/reportMenu/star.svg");
        margin-right: 0.45rem;
        &.active {
          background-image: url("/img/reportMenu/star-active.svg");
        }
      }
    }
  }
  &__body {
    width: 100%;
    height: calc(100% - (9.3rem));
    display: flex;
    flex-flow: column;
    font-family: Roboto;
    .messages-list {
      display: flex;
      flex-flow: column;
      height: 100%;
      max-height: 100%;
      overflow-y: auto;
      padding: 2.35rem 1.65rem 2.35rem 2.9rem;
      margin-right: 0.95rem;
    }
    .message {
      padding: 1.1rem 2rem;
      background: #1f2025;
      border-radius: 5rem 0 5rem 5rem;
      max-width: 30rem;
      align-self: flex-end;
      margin: 3rem 0 1rem 0;
      position: relative;
      &:last-child {
        margin-bottom: 0;
      }
      &.player {
        background: #4c4f56;
        border-radius: 0 5rem 5rem 5rem;
        align-self: flex-start;
        margin: 0 0 0.7rem 0;
      }
      .text {
        font-size: 0.9rem;
        line-height: 119.5%;
      }
      .btn-delete,
      .message-sender {
        position: absolute;
        right: 1.8rem;
      }
      .btn-delete {
        font-size: 0.7rem;
        line-height: 119.5%;
        text-decoration-line: underline;
        color: rgba(255, 255, 255, 0.45);
        bottom: -0.5rem;
        transform: translateY(100%);
        &:hover {
          color: rgba(255, 255, 255, 0.75);
        }
      }
      .message-sender {
        top: -0.75rem;
        transform: translateY(-100%);
        font-weight: bold;
        font-size: 0.9rem;
        line-height: 119.5%;
        white-space: pre;
        color: rgba(255, 255, 255, 0.45);
      }
    }
    .input-wrap {
      background: rgba(38, 40, 46, 0.25);
      height: 5rem;
      min-height: 5rem;
      display: flex;
      align-items: center;
      input {
        outline: none;
        border: none;
        background: transparent;
        height: 100%;
        width: 100%;
        font-weight: bold;
        font-size: 0.9rem;
        color: rgba(255, 255, 255, 0.45);
        padding: 0 2.85rem;
      }
      .btn-send {
        width: 1.15rem;
        height: 1.15rem;
        min-width: 1.15rem;
        margin-right: 2.35rem;
        background-image: url("/img/reportMenu/btn-send.svg");
        &:hover {
          transform: scale(1.15);
        }
        &.locked {
          background-image: url("/img/reportMenu/btn-send-locked.svg");
          &:hover {
            transform: none;
          }
        }
      }
      .input-text {
        font-weight: bold;
        font-size: 0.9rem;
        width: 100%;
        height: 100%;
        padding: 0 2.85rem;
        color: rgba(255, 255, 255, 0.15);
        display: flex;
        align-items: center;
        justify-content: flex-start;
      }
    }
  }
}
</style>
