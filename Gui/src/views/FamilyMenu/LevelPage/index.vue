<template>
  <div class="level-page">
    <div 
      class="level-item"
      v-for="item in 3"
      :key="item.key"
    >
      <div class="desc">Level</div>
      <div class="value">{{item}}</div>
    </div>
  </div>
</template>

<script>
export default {
  name: 'LevelPage',
}
</script>

<style lang="scss" scoped>
@keyframes isShow{
  0%{
    opacity: 0;
  }
  25%{
    opacity: 0;
  }
  50%{
    opacity: 0
  }
  100%{
    opacity: 1
  }
}
.level-page{
  display: flex;
  justify-content: space-between;
  width: 100%;
  height: 100%;
  margin-top: 1.7rem;
  &:after{
    content: '';
    width: 100%;
    height: 100%;
    position: absolute;
    left: 0;
    top: 0;
    background: rgba(0, 0, 0, 0.02);
    backdrop-filter: blur(.25rem);
  }
  &:before{
    content: '';
    width: 21.9rem;
    height: 13.5rem;
    background-image: url('/img/familyMenu/coming-soon.png');
    background-size: contain;
    background-repeat: no-repeat;
    position: absolute;
    top: 50%;
    left: 50%;
    transform: translateX(-50%) translateY(-50%);
    z-index: 1;
    animation: 1s isShow;
  }
  .level-item{
    display: flex;
    flex-flow: column;
    align-items: center;
    justify-content: space-between;
    width: 30%;
    height: 10.75rem;
    padding: 2rem;
    background: #08090B;
    .desc{
      font-size: 2.5rem;
      line-height: 3rem;
    }
    .value{
      font-weight: bold;
      font-size: 5rem;
      line-height: 5rem;
    }
  }
}
</style>
