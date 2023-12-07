<template>
  <div class="settings-lang">
      <div class="settings-lang-item" 
        v-for="(lang, key, index) in langs.list" 
        :key="index"
        :class="{'settings-lang-current': langs.cur === key}"
        @click="select(key)"
      >
        {{lang}}
      </div>
  </div>
</template>

<script>
export default {
  name: 'SettingsLang',
  props: ['langs'],
  methods: {
    select(tag){
      window.mp.triggerServer("setlangev", tag);
      this.$emit("onSelect", tag)
    }
  },
}
</script>

<style lang="scss" scoped>
.settings-lang{
  display: flex;
  justify-content: flex-start;
  align-items: center;
  flex-wrap: wrap;
  width: 100%;
  color: rgba(#fff, .5);
  margin-top: 1.25rem;
  &-item{
    width: 5rem;
    height: 2rem;
    font-size: .9rem;
    line-height: 1.1rem;
    letter-spacing: .04rem;
    border: 1px solid rgba(#fff, .1);
    background: rgba(255, 255, 255, 0.05);
    display: flex;
    align-items: center;
    justify-content: center;
    margin-right: .5rem;
    margin-bottom: .5rem;
  }
  &-current{
    background-color: #fff;
    color: #000;
  }
}
</style>