<template>
  <div class="reports-tab">
    <div class="reports-tab__side">
      <div class="title">{{loc('mmain:rep:tit')}}</div>
      <div class="decor">
        <div class="decor__text">
          <div class="title">{{loc('mmain:rep:sub')}}</div>
          <div class="desc">{{loc('mmain:rep:desc')}}</div>
        </div>
      </div>
      <div class="close" v-if="reports.length > 0" @click="closeReport">CLOSE REPORT</div>
      <div class="raiting" v-if="reportRaiting" @mouseleave="raitOver=0">
        <div class="raiting-tittle">{{loc("mmain:rep:rait:title")}}</div>
        <img 
          v-for="(star, index) in stars" 
          :key="index" 
          src="/img/optionsMenu/reportsTab/star.svg" 
          alt="star" 
          class="raiting-star"
          :class="{'raiting-over': star <= raitOver}"
          @mouseenter="raitOver = star"
          @click="sendRaiting(star)"
        >
      </div>
    </div>
    <div class="reports-tab__main">
      <div class="messages" ref="list">
        <messages-item
          v-for="(item, index) in reports"
          :key="index"
          :item="item"
        />
      </div>
      <div class="input">
        <input
          v-if="!reportRaiting"
          type="text"
          v-model="messageText"
          class="input__main"
          :placeholder="loc('optmenu:report:plh')"
          @change="send"
          
        >
        <button class="input__btn" @click="send"></button>
      </div>
    </div>
  </div>
</template>

<script>
import { mapState, mapGetters } from 'vuex'
import MessagesItem from './common/MessagesItem'

export default {
  name: 'ReportsTab',

  components: { MessagesItem },

  data: function() {
    return {
      messageText: '',
      stars:[1,2,3,4,5],
      raitOver: 0
    }
  },
  methods: {
    send(){
      if(this.messageText.length < 3) return
      this.$store.commit('optionsMenu/sendReport', this.messageText);
      this.messageText = '';
      this.scrollBottom();
    },
    scrollBottom(){
      setTimeout(()=>{
          const list = this.$refs.list;
          if(list) list.scrollTop = list.scrollHeight;
      }, 0)
    },
    closeReport(){
      this.$store.commit('optionsMenu/closeReport');
      this.messageText = '';
    },
    sendRaiting(rait){
      this.$store.commit('optionsMenu/sendRaiting', rait);
      this.messageText = '';
    }
  },
  computed: {
    ...mapState('optionsMenu', ['reports', 'reportRaiting']),
    ...mapGetters('localization', ['loc'])
  },
  mounted(){
    this.scrollBottom()
  }
}
</script>

<style lang="scss" scoped>
.reports-tab {
  width: 100%;
  height: 100%;
  display: flex;
  justify-content: space-between;
  &::before {
    content: "";
    width: 47.4rem;
    height: 34.2rem;
    position: absolute;
    bottom: 0;
    left: 0;
    background: center / cover no-repeat url("/img/optionsMenu/reportsTab/bg.png");
    z-index: -1;
  }
  &__side {
    display: flex;
    flex-direction: column;
    padding: 3.55rem 0 0 0;
    .close{      
      border-radius: 2rem;
      height: 4rem;
      width: 12rem;
      background-color: rgba(255, 255, 255, 0.1);
      color: rgba(#fff, .5);
      font-size: 2rem;
      border: 2px solid rgba(#fff, .1);
      display: flex;
      align-items: center;
      justify-content: center;
      margin: 2rem auto;
      &:hover{
        background-color: rgba(#fff, .2);
      }      
    }
    .raiting{
      margin: 2rem auto;
      width: 20rem;
      &-star{
        margin: 0 .5rem;
        opacity: .3;
        width: 3rem;
        height: 3rem;
      }
      &-over{
        opacity: 1;
      }
      &-tittle{
        color: #fff;
        text-align: center;
        font-size: 1.5rem;
        letter-spacing: .04rem;
      }
    }
    .title {
      font-size: 5rem;
      line-height: 5rem;
      color: #FFFFFF;
      width: 22.4rem;
    }
    .decor {
      display: flex;
      &::before {
        content: "";
        width: 4.5rem;
        height: 4.5rem;
        background: center / cover no-repeat url("/img/optionsMenu/reportsTab/megaphone.png");
        margin: 0 1.5rem 0 0;
      }
      &__text {
        display: flex;
        flex-direction: column;
        letter-spacing: 0.03em;
        .title {
          font-size: 2rem;
          line-height: 2rem;
          color: #FFFFFF;
          margin: 0 0 0.25rem 0;
        }
        .desc {
          font-size: 0.9rem;
          line-height: 1.2rem;
          color: rgba(255, 255, 255, 0.5);
          width: 12.7rem;
        }
      }
    }
  }
  &__main {
    padding: 1.5rem 1.5rem 2.5rem 1.5rem;
    border-right: 0.1rem solid rgba(255, 255, 255, 0.1);
    border-left: 0.1rem solid rgba(255, 255, 255, 0.1);
    background: rgba(0, 0, 0, 0.5);
    .messages {
      height: 36.5rem;
      width: 51.5rem;
      padding: 0 0.75rem;
      overflow-x: auto;
      margin: 0 0 2rem 0;
      &::-webkit-scrollbar {
        width: 0.15rem;
        background: transparent;
      }
      &::-webkit-scrollbar-thumb {
        background: rgba(255, 255, 255, 0.05);
      }
    }
    .input {
      width: 50rem;
      height: 5rem;
      position: relative;
      margin: auto;
      &__main {
        width: 100%;
        height: 100%;
        border-radius: 5rem;
        background: rgba(255, 255, 255, 0.1);
        font-size: 1.1rem;
        color: #fff;
       text-transform: uppercase;
        padding: 0 5rem 0 3.25rem;
        &::placeholder {
          color: rgba(255, 255, 255, 0.5);
        }
      }
      &__btn {
        position: absolute;
        transform: translate(0, -50%);
        right: 1rem;
        top: 50%;
        width: 3rem;
        height: 3rem;
        border-radius: 50%;
        background: #B6D300;
        box-shadow: 0px 1rem 4.5rem rgba(168, 195, 2, 0.5);
        display: flex;
        align-items: center;
        justify-content: center;
        &::after {
          content: "";
          width: 1.2rem;
          height: 1.2rem;
          background: center / cover no-repeat url("/img/optionsMenu/reportsTab/airplane.svg");
          display: block;
        }
        &:hover {
          box-shadow: none;
        }
      }
    }
  }
}
</style>
