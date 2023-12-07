<template>
  <div class="checkbox">
    <input 
      type="checkbox"
      :id="checkboxKey"
      :name="checkboxKey"
      :value="flag"
      @change="$emit('change')"
    >
    <label :for="checkboxKey">{{text}}</label>
  </div>
</template>

<script>
export default {
  name: 'Checkbox',

  props: {
    checkboxKey: String,
    text: String,
    flag: Boolean
  }
}
</script>

<style lang="scss" scoped>
.checkbox{
  display: flex;
  align-items: center;
  width: 100%;
  height: 2.5rem;
  min-height: 2.5rem;
  position: relative;
  padding-left: 1.95rem;
  margin-bottom: 1.2rem;
  padding-top: 1.3rem;
  border-top: 1px solid rgba(70, 85, 95, 0.2);
  box-sizing: border-box;
  input{
    opacity: 0;
    width: 0;
    height: 0;
    & + label{
      display: flex;
      align-items: center;
      font-family: Roboto;
      font-style: normal;
      font-weight: normal;
      font-size: .8rem;
      color: #46555F;
      &:before, &:after{
        content: '';
        width: 1.2rem;
        height: 1.2rem;
        position: absolute;
        border-radius: .1rem;
        left: 0;
      }
      &:before{
        border: .1rem solid rgba(70, 85, 95, 0.2);
        box-sizing: border-box;
        z-index: 1;
      }
      &:after{
        z-index: 2;
        background-color: #E00B29;
        background-image: url('/img/cityHallWeb/checkbox-icon.svg');
        background-size: contain;
        background-repeat: no-repeat;
        background-position: center;
        opacity: 0;
      }
    }
    &:checked + label{
      &:after{
        opacity: 1;
      }
    }
  }
}
</style>
