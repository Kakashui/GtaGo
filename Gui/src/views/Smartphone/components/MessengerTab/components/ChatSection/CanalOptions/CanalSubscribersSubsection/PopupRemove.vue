<template>
  <div class="popup-remove">
    <span class="popup-remove__title">{{loc('phone:chat:remove:1')}}</span>
    <p class="popup-remove__desc">{{loc('phone:chat:remove:2')}}</p>
    <button class="popup-remove__btn-agree" @click="confirmDelete">{{loc('phone:chat:remove:3')}}</button>
    <button class="popup-remove__btn-cancel" @click="$emit('close')">{{loc('phone:cancel')}}</button>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
export default {
  name: 'PopupRemove',
  computed:{
    ...mapGetters('localization', ['loc'])
  },
  methods: {
    confirmDelete: function() {
      this.$emit('confirm')
      this.$emit('close')
    }
  }
}
</script>

<style lang="scss" scoped>
.popup-remove {
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  margin: auto;
  width: 14.2rem;
  height: fit-content;
  padding: 1.75rem 1.5rem 1.5rem 1.5rem;
  border-radius: 0.6rem;
  background: #FAFAFA;
  display: flex;
  flex-direction: column;
  align-items: center;
  &__title {
    color: #020202;
    line-height: 1.15rem;
    font-weight: 500;
    text-align: center;
    margin: 0 0 0.4rem 0;
  }
  &__desc {
    font-size: 0.7rem;
    line-height: 0.9rem;
    text-align: center;
    color: rgba(2, 2, 2, 0.4);
    margin: 0 0 1.5rem 0;
  }
  &__btn-agree {
    width: 100%;
    height: 2rem;
    color: #fff;
    font-size: 0.7rem;
    border-radius: 5rem;
    background: #4E6ED9;
    transition: all 0.15s;
    margin: 0 0 1rem 0;
    &:hover {
      box-shadow: 0px 0.5rem 1.5rem rgba(78, 110, 216, 0.3);
    }
  }
  &__btn-cancel {
    background: transparent;
    color: #4E6ED9;
    font-size: 0.7rem;
    line-height: 0.6rem;
    &:hover {
      text-decoration: underline;
    }
  }
}
</style>
