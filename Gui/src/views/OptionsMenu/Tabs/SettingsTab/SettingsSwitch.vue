<template>
  <div class="settings-switch">
    <div class="settings-switch-off" :class="{'settings-switch-off-active': !status}" @click="switchSet(false)" >off</div>
    <div class="settings-switch-on" :class="{'settings-switch-on-active': status}" @click="switchSet(true)">on</div>
  </div>
</template>

<script>
export default {
    props:['status'],
    methods: {
      switchSet(status){
        this.$emit("onSwitch", status);
      }
    },
}
</script>

<style lang="scss" scoped>
.settings-switch{
    width: 7.5rem;
    height: 2rem;
    display: flex;
    align-items: center;
    justify-content: space-between;
    color: #fff;
    &-on{
        width: 3.5rem;
        height: 2rem;
        display: flex;
        align-items: center;
        justify-content: center;
        border: 1px solid rgba(255, 255, 255, 0.1);
        background: rgba(255, 255, 255, 0.05);
        &-active{
          background: #B6D300;
          box-shadow: 0px 10px 50px rgba(168, 195, 2, 0.6);
          border-radius: 2px;
        }
    }
    &-off{
        width: 3.5rem;
        height: 2rem;
        display: flex;
        align-items: center;
        justify-content: center;
        border: 1px solid rgba(255, 255, 255, 0.1);
        background: rgba(255, 255, 255, 0.05);
        &-active{
            background-color: #fff;
            color: #000;
        }
    }
}
</style>
