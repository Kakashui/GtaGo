<template>
  <div class="container">
    <div class="container__box" v-if="!isJudged">
      <input
        :placeholder="loc('gui_765')"
        type="text"
        class="input"
        v-model.trim="firstname"
      >
      <input
        :placeholder="loc('gui_746')"
        type="text"
        class="input"
        v-model.trim="lastname"
      >
      <div class="actions">
        <button @click="show" class="button">{{loc('gui_763')}}</button>
        <button class="button" @click="hide">{{loc('gui_764')}}</button>
      </div>
    </div>
    <div class="container__box" v-else>
      <span @click="hide" class="close">&times;</span>
      <input
        :placeholder="loc('gui_766')"
        type="text"
        class="input"
        v-model.trim="time"
      >
      <input
        :placeholder="loc('gui_767')"
        type="text"
        class="input"
        v-model.trim="article"
      >
      <button class="button" @click="toJudge">{{loc('gui_763')}}</button>
    </div>
  </div>
</template>

<script>
import {mapGetters} from 'vuex';

export default {
  data: function () {
    return {
      isJudged: false,
      time: '',
      article: '',
      firstname: '',
      lastname: ''
    }
  },
  computed:{
    ...mapGetters('localization',['loc'])
  },

  methods: {
    show () {
      if (this.firstname.length !== 0 && this.lastname.length !== 0) {
        this.isJudged = true
      }
    },

    hide () {
      this.isJudged = false;
      this.firstname = '';
      this.lastname = '';
      window.mp.trigger('judje:close') ;
    },

    toJudge () {
      const time = this.time;
      const article = this.article;
      window.mp.trigger('judje:arrest', `${this.firstname}_${this.lastname}`, time, article);
    }
  }
}
</script>

<style lang="scss" scoped>
.container {
  width: 30rem;
  height: 20rem;
  &__box {
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: space-around;
    position: relative;
    height: 100%;
    background-color: rgba(0, 0, 0, .9);
    .close {
      position: absolute;
      top: 0;
      right: .4rem;
      font-size: 2rem;
      color: red;
      cursor: pointer;
    }
    .input {
      border: 2px solid #35870e;
      background-color: rgba(0, 0, 0, .29);
      color: #ffffff;
      width: 60%;
      padding: .5rem;
      outline: none;
    }
    .input::placeholder {
      color: #525252;
    }
    .button {
      background-color: #35870e;
      padding: .5rem 1rem;
      color: #ffffff;
      &:nth-child(2) {
        margin-left: .5rem;
      }
    }
  }
}
</style>
