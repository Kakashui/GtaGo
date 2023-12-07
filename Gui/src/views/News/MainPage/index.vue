<template>
  <div class="main-page">
    <control-window/>
    <edit-window/>
  </div>
</template>

<script>
import ControlWindow from './ControlWindow'
import EditWindow from './EditWindow'
export default {
  name: 'MainPage',

  components: {
    ControlWindow,
    EditWindow
  }
}
</script>

<style lang="scss" scoped>
.main-page{
  display: flex;
}
</style>
