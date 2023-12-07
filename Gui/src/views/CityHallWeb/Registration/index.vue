<template>
  <div class="registration">
    <component 
      :is="currentWrap"
      :setCurrentWrap="setCurrentWrap"
    />
  </div>
</template>

<script>
import RegWrap from './RegWrap'
import CardWrap from './CardWrap'
import PaintWrap from './PaintWrap'

export default {
  name: 'Registration',

  components: {
    RegWrap,
    CardWrap,
    PaintWrap
  },

  data: function() {
    return{
      currentWrap: 'RegWrap',
    }
  },

  methods: {
    setCurrentWrap: function(value) {
      this.currentWrap = value
    }
  }, 

  mounted(){
    this.currentWrap = 'RegWrap'
  }
}
</script>

<style lang="scss" scoped>
.registration{
  display: flex;
  align-items: center;
  justify-content: center;
  width: 100%;
  height: 100%;
  position: relative;
  background-image: url('/img/cityHallWeb/page-card.png');
  background-size: cover;
}
</style>

<style lang="scss">
.city-hall-web{
  .registration{
    .btn-next{
      background: #E00B29;
      height: 3.5rem;
      min-height: 3.5rem;
      text-transform: uppercase;
      font-style: normal;
      font-weight: bold;
      font-size: 1.1rem;
      line-height: 1.25rem;
      text-align: center;
      text-transform: uppercase;
      color: #FFFFFF;
      margin-top: 1.5rem;
      border-radius: 0 0 .5rem .5rem;
      &:hover{
        background: #C00621;
      }
    }
    .card-wrap, .paint-wrap{
      .btn-next{
        border-radius: .3rem;
      }
    }
    .page{
      display: flex;
      flex-flow: column;
      justify-content: space-between;
      padding-top: 1.5rem;
      min-height: 25rem;
      &>div{
        padding: 0 4.1rem;
        width: 100%;
        &.btn-next{
          padding: 0;
        }
      }
      &>input, &>textarea{
        margin: 0 4.1rem;
        box-sizing: border-box;
      }
      input, textarea{
        border: 1px solid rgba(70, 85, 95, 0.2);
        box-sizing: border-box;
        border-radius: .3rem;
        background-color: transparent;
        outline: none;
        padding: 0 1rem;
        font-size: .8rem;
        font-family: Roboto;
        margin-bottom: 1.5rem;
      }
      input{
        min-height: 3.4rem;
        &::placeholder{
          color: rgba(70, 85, 95, 0.7);
        }
        &::-webkit-outer-spin-button,
        &::-webkit-inner-spin-button {
          -webkit-appearance: none;
        }
      }
      textarea {
        resize: none;
        height: 5.2rem;
        min-height: 5.2rem;
        padding: 1.25rem 1rem;
      }
      &__title{
        font-family: Merriweather;
        font-style: normal;
        font-weight: bold;
        font-size: 1.2rem;
        line-height: 1.5rem;
        margin-bottom: 1rem;
        color: #293340;
      }
      &__desc{
        font-style: bold;
        font-weight: 500;
        font-size: .9rem;
        line-height: 1.05rem;
        margin-bottom: .5rem;
        color: #46555F;
      }
      .current-page{
        align-self: center;
        text-align: center;
        font-family: Roboto;
        font-style: normal;
        font-weight: 500;
        font-size: .9rem;
        line-height: .95rem;
        color: #E00B29;
        margin-bottom: .5rem;
      }
      &__data{
        display: flex;
        align-items: center;
        justify-content: space-between;
        width: 100%;
        &-item{
          display: flex;
          flex-flow: column;
          &.day{
            min-width: 2.9rem;
            width: 2.9rem;
          }
          &.month{
            min-width: 8.35rem;
            width: 8.35rem;
          }
          &.year{
            min-width: 3.8rem;
            width: 3.8rem;
          }
          .desc{
            color: rgba(70, 85, 95, 0.7);
            font-weight: normal;
            font-size: .8rem;
            margin-bottom: .5rem;
          }
          input{
            padding: 0;
            text-align: center;
          }
        }
      }
    }
  }
}
</style>
