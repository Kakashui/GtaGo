<template>
  <div class="green-zone"
       :style="{ '--bottomOffset': this.bottomOffset + '%', '--rightOffset': this.rightOffset + '%' }">
    <div class="green-zone__icon"></div>
    <div class="green-zone__title">
      <span>Green</span>
      <span>zone</span>
    </div>
  </div>
</template>
<script>

export default {
  props: [
      'rightOffset',
      'bottomOffset'
  ]
}
</script>

<style lang='scss' scoped>
.green-zone{
  display: flex;
  align-items: center;
  justify-content: center;
  position: absolute;
  
  left: var(--rightOffset);
  bottom: var(--bottomOffset);
  
  text-transform: uppercase;
  color: #fff;
  &:before{
    content: '';
    width: 9rem;
    height: 9rem;
    position: absolute;
    top: -.75rem;
    //background: rgba(182, 211, 0, 0.7);
    //filter: blur(5rem);
  }
  &__icon{
    width: 2rem;
    height: 2rem;
    margin-right: .2rem;
    background-size: contain;
    background-position: center;
    background-repeat: no-repeat;
    background-image: url('/img/hud/icon-green-zone.svg');
  }
  &__title{
    display: flex;
    flex-flow: column;
    align-items: flex-end;
    span{
      position: relative;
      display: flex;
      align-items: center;
      justify-content: center;
      &:first-child{
        font-size: 1.6rem;
        line-height: 1.6rem;
      }
      &:nth-child(2){
        font-size: .9rem;
        line-height: .9rem;
        letter-spacing: 0.1em;
        display: flex;
        align-items: center;
        &:before{
          content: '';
          width: 1.2rem;
          height: .1rem;
          border-radius: .2rem;
          //background: #B6D300;
          //box-shadow: 0 0 2rem #A8C302;
          position: absolute;
          left: -.25rem;
          transform: translateX(-100%);
        }
      }
    }
  }
}
</style>
