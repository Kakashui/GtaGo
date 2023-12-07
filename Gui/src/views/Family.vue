<template>
  <div class="app">
    <div class="container">
      <div class="agreement" v-if="family.isRequest">
        <div class="agreement__text">
          <p>{{loc('gui_755') + family.name + loc('gui_756') + family.money}}</p>
          <span>{{loc('gui_757')}}</span>
        </div>
        <div class="agreement__buttons">
          <button class="button yes" @click="answer(true)">{{loc('gui_758')}}</button>
          <button class="button no" @click="answer(false)">{{loc('gui_759')}}</button>
        </div>
      </div>
      <div class="creating" v-else>
        <span class="close" @click="closeCreate">&times;</span>
        <div class="title">{{loc('gui_760')}}</div>
        <div class="input">
          <p>{{loc('gui_761')}}</p>
          <input type="text" v-model="title">
        </div>
        <button class="button" @click="createFamily">{{loc('gui_762')}}</button>
      </div>
    </div>
  </div>
</template>

<script>
import {mapState, mapGetters} from 'vuex';
export default {
  props:["isRequest"],
  data: function () {
    return {
      title: '', // name     
    }
  },
  computed:{
    ...mapState(["family"]),
    ...mapGetters('localization',['loc'])
  },
  methods: {
    closeCreate () {
      window.close();
    },
    answer(state) {
      window.mp.trigger("family:answer", state)
      window.close()
    },

    createFamily () {
      if(this.title.length < 4) return;
      window.mp.trigger("family:create",this.title);
      window.close()
    },

    closeRequest () {
      this.isRequest = false
    }
  }
}
</script>

<style lang="scss" scoped>
.container {
  background: #FFFFFF;
  opacity: 0.93;
  border-radius: 12px;
  padding: 1.5rem;
  width: 30rem;
  .agreement {
    &__buttons {
      margin-top: 1rem;
      display: flex;
      justify-content: space-between;
      .button {
        padding: .3rem 2rem;
        border-radius: 6px;
        color: #FFFFFF;
      }
      .yes {
        background: #56CD54;
      }
      .no {
        background: #EB6161;
      }
    }
  }
  .creating {
    display: flex;
    flex-direction: column;
    align-items: center;
    position: relative;
    .close {
      position: absolute;
      top: -.75rem;
      right: -.75rem;
      width: 1rem;
      height: 1rem;
      background: #EB6161;
      border-radius: 4px;
      display: flex;
      align-items: center;
      justify-content: center;
      color: #fff;
      font-size: 1.2rem;
      cursor: pointer;
    }
    .title {
      font-size: 1.1rem;
      font-weight: bold;
    }
    .input {
      width: 90%;
      p {
        font-size: .75rem;
        font-weight: 500;
      }
      input {
        margin-top: 5px;
        width: 100%;
        box-shadow: 0px 0px 3px rgba(0, 0, 0, 0.25);
        background: #FFFFFF;
        padding: .5rem;
      }
    }
    .button {
      margin-top: 1rem;
      background: #56CD54;
      padding: .3rem 2rem;
      border-radius: 6px;
      color: #FFFFFF;
    }
  }
}
</style>
