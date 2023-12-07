<template>
  <div  :class="[item.unicName, 'list-item']">
    <div class="list-item-title">{{ item.name }}</div>
    <div 
      :class="[{ max: item.currentLevel === item.maxLevel }, 'list-item-level']"
    >{{ item.typeLevel }}</div>
    <div class="progress-wrap">
      <div 
        v-for="(col, index) in item.maxLevel"
        :key="index"
        :class="[{ active: index < item.currentLevel }, 'progress-wrap__item']"
      ></div>
    </div>
  </div>
</template>

<script>
export default {
  name: 'ListItem',

  props: {
    item: Object
  }
}
</script>

<style lang="scss" scoped>
.list-item{
  background: rgba(2, 3, 16, 0.9);
  position: relative;
  display: flex;
  flex-flow: column;
  align-items: flex-start;
  justify-content: flex-start;
  padding: 1.5rem 1.25rem;
  color: #fff;
  background-size: contain;
  background-repeat: no-repeat;
  background-position: bottom center;
  transition: .2s;
  &>div{
    z-index: 2;
  }
  &:before, &:after{
    content: '';
    width: 100%;
    height: 100%;
    position: absolute;
    top: 0;
    left: 0;
  }
  &:before{
    background: linear-gradient(0deg, #B6D300, #B6D300);
    opacity: 0;
    z-index: 0;
  }
  &:after{
    background-size: contain;
    background-repeat: no-repeat;
    background-position: bottom center;
    z-index: 1;
  }
  &.engine{
    &:after{
      background-image: url('/img/technicalCertificate/engine.png');
    }
  }
  &.turbine{
    &:after{
      background-image: url('/img/technicalCertificate/turbine.png');
    }
  }
  &.transmission{
    &:after{
      background-image: url('/img/technicalCertificate/transmission.png');
    }
  }
  &.brakes{
    &:after{
      background-image: url('/img/technicalCertificate/brakes.png');
    }
  }
  &:hover{
    background-color: transparent;
    transition: .2s;
    &:before{
      opacity: 1;
      transition: .2s;
    }
    .list-item-level{
      &.max{
        color: #fff;
      }
    }
    .progress-wrap{
      &__item{
        &.active{
          background: #fff;
        }
      }
    }
  }
  &-title{
    font-size: 1.8rem;
    line-height: 2.15rem;
  }
  &-level{
    font-weight: normal;
    font-size: 1rem;
    line-height: 1.2rem;
    letter-spacing: 0.03em;
    &.max{
      color: #B6D300;
    }
  }
  .progress-wrap{
    display: flex;
    align-items: center;
    justify-content: space-between;
    margin-top: .65rem;
    width: 100%;
    &__item{
      width: 100%;
      height: .25rem;
      margin-right: .3rem;
      background: rgba(255, 255, 255, 0.2);
      border-radius: 1px;
      &.active{
        background: #B6D300;
        box-shadow: 0 .5rem 2.5rem rgba(168, 195, 2, 0.6);
      }
      &:last-child{
        margin-right: 0;
      }
    }
  }
}
</style>
