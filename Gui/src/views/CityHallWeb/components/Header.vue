<template>
  <div class="header">
    <div class="header__url">
      <div class="site-tab">
        <div class="logo-head"></div>
        <div class="text">Gov.com</div>
        <div class="btn btn-close"></div>
        <div class="btn btn-add"></div>
      </div>
      <div class="btn btn-back"></div>
      <div class="btn btn-forward"></div>
      <div class="btn btn-refresh"></div>
      <div class="url">www.gov.com</div>
    </div>
  </div>
</template>

<style lang="scss" scoped>
.header{
  display: flex;
  flex-flow: column;
  width: 100%;
  padding-top: 3.5rem;
  background-color: #000;
  &__url, &__site{
    width: 100%;
    display: flex;
    align-items: center;
    justify-content: flex-start;
  }
  &__url{
    background-color: #fff;
    position: relative;
    min-height: 3.35rem;
    padding: 0 5rem 0 1rem;
    font-size: 1rem;
    .site-tab{
      position: absolute;
      top: 0;
      left: .6rem;
      transform: translateY(-98%);
      width: fit-content;
      display: flex;
      align-items: center;
      justify-content: flex-start;
      height: 2.25rem;
      min-height: 2.25rem;
      color: #000;
      background-image: url('/img/siteHeader/Subtract.png');
      background-size: contain;
      background-repeat: no-repeat;
      background-position: bottom;
      padding: 0 2rem;
      .text{
        font-weight: bold;
        font-size: .9em;
        line-height: 1.05rem;
        color: #000000;
        margin: 0 4.65rem 0 .5rem;
      }
      .logo-head, .btn-close{
        width: 1.1rem;
        height: 1.1rem;
      }
      .logo-head{
        border-radius: 50%;
        background-color: #D92C2C;
      }
      .btn-close{
        background-image: url('/img/siteHeader/close.svg');
        margin-right: 0;
      }
      .btn-add{
        width: 1.4rem;
        height: 1.4rem;
        margin-right: 0;
        position: absolute;
        right: 0;
        transform: translateX(1.15rem);
        background-image: url('/img/siteHeader/add.svg');
      }
    }
    &>.btn{
      width: 1.4rem;
      height: 1.4rem;
      min-width: 1.4rem;
      margin-right: 1.5rem;
      &.btn-back, &.btn-forward{
        background-image: url('/img/siteHeader/arrow.svg');
      }
      &.btn-forward{
        opacity: .5;
        transform-origin: 50% 50%;
        transform: rotate(180deg);
      }
    }
    .btn-refresh{
      background-image: url('/img/siteHeader/refresh.svg');
      width: 1.9rem;
      height: 1.9rem;
    }
    .url{
      margin-left: .5rem;
      height: 2.2rem;
      min-height: 2.2rem;
      background: #E7EBEE;
      border-radius:  2.2rem;
      display: flex;
      align-items: center;
      justify-content: center;
      width: 100%;
    }
  }
}
</style>
