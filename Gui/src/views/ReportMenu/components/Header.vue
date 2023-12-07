<template>
  <div class="header">
    <div class="header__title">{{loc('ReportMenu_8')}}</div>
    <div class="header__nav">
      <div 
        v-for="item in navItems"
        :key="item.id"
        :class="[{ active: item.key == currentPage }, 'header__nav-item']"
        @click="setCurrentPage(item.key)"
      >
        <div class="text">{{loc(item.text)}}</div>
      </div>
    </div>
    <div class="btn btn-close" @click="closeMenu"></div>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
export default {
  name: 'Header',

  props: {
    setCurrentPage: Function,
    currentPage: String
  },

  data: function() {
    return {
      navItems: [
        {
          text: 'ReportMenu_13',
          key: 'ReportsList'
        },
        {
          text: 'ReportMenu_14',
          key: 'AnswersList'
        },
        {
          text: 'ReportMenu_15',
          key: 'ReportsLogs'
        }
      ]
    }
  },
  computed: {
    ...mapGetters('localization', ['loc'])
  },
  methods: {
    closeMenu(){
      window.mp.trigger('report:closemenu');
    }
  }
}
</script>

<style lang="scss" scoped>
.header{
  height: 9.8rem;
  width: 100%;
  min-height: 9.8rem;
  display: flex;
  align-items: center;
  position: relative;
  background: rgba(38, 40, 46, 0.35);
  &:before{
    content: '';
    width: 100%;
    height: 1px;
    background: radial-gradient(50% 50% at 50% 50%, rgba(255, 255, 255, 0.65) 0%, rgba(255, 255, 255, 0) 100%);
    position: absolute;
    left: 0;
    bottom: 0;
  }
  &__title{
    display: flex;
    align-items: center;
    justify-content: center;
    text-transform: uppercase;
    width: 21rem;
    min-width: 21rem;
    height: 100%;
    font-size: 3.2rem;
    position: relative;
    &:before{
      content: '';
      width: 1.3rem;
      height: 2.45rem;
      background-size: contain;
      background-position: center;
      background-repeat: no-repeat;
      background-image: url('/img/reportMenu/slash.svg');
      position: absolute;
      left: 2.45rem;
    }
  }
  &__nav{
    display: flex;
    align-items: center;
    height: 100%;
    &-item{
      background: rgba(38, 40, 46, 0.35);
      padding: 3rem;
      border-bottom: .2rem solid transparent;
      height: 100%;
      .text{
        font-weight: bold;
        font-size: 1.55rem;
        line-height: 1.55rem;
        letter-spacing: 0.05em;
        color: rgba(255, 255, 255, 0.25);
        max-width: 5rem;
      }
      &.active, &:hover{
        background: #26282E;
        transition: .2s;
        .text{
          color: rgba(255, 255, 255, 1);
        }
      }
      &.active{
        border-bottom: .2rem solid #C1E704;
      }
    }
  }
  .btn-close{
    width: 2rem;
    height: 2rem;
    position: absolute;
    right: 3.6rem;
    opacity: .45;
    background-image: url('/img/reportMenu/btn-close-header.svg');
    &:hover{
      opacity: 1;
    }
  }
}
</style>
