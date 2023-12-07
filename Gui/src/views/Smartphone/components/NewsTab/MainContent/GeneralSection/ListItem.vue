<template>
  <div class="list-item">
    <img
      :src="item.Picture"
      class="list-item__img"
      @error="$event.target.src='/img/hud/smartphone/newsTab/main/default.png'"
    >
    <div class="list-item__main">
      <span class="title">{{ item.Title }}</span>
      <div class="footer">
        <div class="footer__sender">
          <span class="desc">{{loc('phone:news:1')}}</span>
          <span class="value">{{ item.Sender }}</span>
        </div>
        <button
          class="footer__btn"
          @click="setCurrentTab({ key: currentTab.key, section: 'CurrentSection', data: {id: item.Id} })"
        >{{loc('phone:news:2')}}</button>
      </div>
    </div>
  </div>
</template>

<script>
import { mapState, mapMutations, mapGetters } from 'vuex'

export default {
  name: 'ListItem',

  props: {
    item: Object
  },

  computed: {
    ...mapState('smartphone', ['currentTab']),
    ...mapGetters('localization', ['loc'])
  },
  
  methods: {
    ...mapMutations('smartphone', ['setCurrentTab'])
  }
}
</script>

<style lang="scss" scoped>
.list-item {
  display: flex;
  flex-direction: column;
  width: 14.2rem;
  border-radius: 0.6rem;
  overflow: hidden;
  background: #fff;
  &:not(:last-child) {
    margin: 0 0 0.5rem 0;
  }
  &__img {
    width: 100%;
    height: auto;
  }
  &__main {
    background: #fff;
    display: flex;
    flex-direction: column;
    padding: 1rem 0.75rem;
    .title {
      font-size: 0.8rem;
      font-weight: 500;
      line-height: 1rem;
      color: #020202;
      margin: 0 0 0.75rem 0;
    }
    .footer {
      padding: 0.5rem 0 0 0;
      position: relative;
      display: flex;
      justify-content: space-between;
      &::before {
        content: "";
        position: absolute;
        top: 0;
        left: 0;
        height: 1px;
        width: 100%;
        background: #EDEDED;
      }
      &__sender {
        display: flex;
        flex-direction: column;
        font-size: 0.65rem;
        line-height: 0.75rem;
        .desc {
          color: #9E9E9E;
          margin: 0 0 0.1rem 0;
        }
        .value {
          color: #020202;
          white-space: nowrap;
          overflow: hidden;
          width: 7rem;
          text-overflow: ellipsis;
        }
      }
      &__btn {
        background: #E7EEF5;
        width: 5rem;
        height: 1.7rem;
        border-radius: 5rem;
        font-size: 0.6rem;
        font-weight: 500;
        color: #7C92A7;
        transition: all 0.15s;
        z-index: 5;
        &:hover {
          color: #E7EEF5;
          background: #7C92A7;
        }
      }
    }
  }
}
</style>
