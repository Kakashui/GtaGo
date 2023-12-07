<template>
  <div class="input-menu">
    <div class="input-menu-wrap">
      <div class="input-menu__field">
        <span class="field__desc">{{ loc('dynamic_input_3') }}</span>
        <span class="field__title">{{ loc(title) }}</span>
        <input
          v-model="input"
          type="text"
          class="field__action"
          :length="length"
          :placeholder="loc(plHolder)"
        >
      </div>   
      <div class="input-menu__buttons">
        <div class="btn btn-one" @click="send">
          {{ loc('dynamic_input_1') }}
        </div>
        <div class="btn btn-two" @click="close">
          {{ loc('dynamic_input_2') }}
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapState, mapGetters } from 'vuex'

export default {
  name: 'InputMenu',
  data: function () {
    return {
      input: ''
    }
  },

  computed: {
    ...mapState('inputMenu', ['title','plHolder','length']),
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    send: function () {
      if(this.input == '') return;
      window.mp.trigger('input', this.input)
      this.input = ''
    },
    close(){
      window.mp.trigger('closeInput')
    }
  },
  mounted() {
    addEventListener("mousemove", this.onMouseMove);
  },
  beforeDestroy(){
    removeEventListener("mousemove", this.onMouseMove);
  }
}
</script>

<style lang="scss" scoped>
.input-menu-wrap{
  display: flex;
  flex-flow: column;
  width: 26.5rem;
  padding: 3rem 3.75rem;
  border-radius: .3rem;
  background: url('/img/inputMenu/bg.png');
  background-size: cover;
  background-repeat: no-repeat;
  background-position: center;
}
.input-menu{
  width: 100%;
  height: 100%;
  position: absolute;
  display: flex;
  align-items: center;
  justify-content: center;
  top: 0;
  left: 0;
  background: radial-gradient(28.13% 50% at 50% 50%, rgba(2, 3, 16, 0.5) 0%, rgba(2, 3, 16, 0.85) 100%);
  &__field {
    display: flex;
    flex-flow: column;
    align-items: center;
    .field__desc{
      text-transform: uppercase;
      font-style: normal;
      font-weight: bold;
      font-size: 16px;
      text-align: center;
      letter-spacing: 0.1em;
      text-transform: uppercase;
      color: rgba(255, 255, 255, 0.5);
    }
    .field__title {
      text-transform: uppercase;
      font-style: normal;
      font-weight: bold;
      font-size: 1.5rem;
      line-height: 1.6rem;
      text-align: center;
      letter-spacing: 0.1em;
      text-transform: uppercase;
      color: #FFFFFF;
      margin: .5rem 0 1.25rem 0;
    }
    .field__action {
      width: 100%;
      padding: 0 1.5rem;
      height: 3.75rem;
      min-height: 3.75rem;
      box-sizing: border-box;
      background: rgba(255, 255, 255, 0.1);
      border: 1px solid rgba(255, 255, 255, 0.1);
      box-sizing: border-box;
      box-shadow: 0 .5rem 1.5rem rgba(0, 0, 0, 0.2);
      border-radius: .2rem;
      margin-bottom: 1rem;
      text-transform: uppercase;
      font-style: normal;
      font-weight: bold;
      font-size: .8rem;
      letter-spacing: 0.1em;
      text-transform: uppercase;
      color: rgba(255, 255, 255, 0.3);
      &::placeholder{
        color: rgba(255, 255, 255, 0.3);
      }
    }
  }
  &__buttons{
    display: flex;
    align-items: center;
    justify-content: space-between;
    .btn{
      display: flex;
      align-items: center;
      justify-content: flex-start;
      padding: 0 1.1rem;
      width: 100%;
      margin-right: 1rem;
      border-radius: .2rem;
      height: 3.75rem;
      min-height: 3.75rem;
      text-transform: uppercase;
      font-weight: bold;
      font-size: .8rem;
      letter-spacing: 0.1em;
      text-transform: uppercase;
      color: #FFFFFF;
      position: relative;
      transition: .3s;
      &:hover{
        transition: .3s;
      }
      &:before{
        content: '';
        width: 1.3rem;
        height: 1.3rem;
        margin-right: .75rem;
        background-size: contain;
        background-repeat: no-repeat;
        background-position: center;
      }
      &:last-child{
        margin-right: 0;
      }
      &-one{
        background: #B6D300;
        &:before{
          background-image: url('/img/inputMenu/btn-one.svg');
        }
        &:hover{
          box-shadow: 0 .5rem 2.5rem rgba(168, 195, 2, 0.6);
        }
      }
      &-two{
        background: rgba(255, 255, 255, 0.1);
        &:before{
          background-image: url('/img/inputMenu/btn-two.svg');
        }
        &:hover{
          background: rgba(255, 255, 255, 0.2);
        }
      }
    }
  }

}
</style>
