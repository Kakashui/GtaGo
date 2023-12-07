<template>
  <div :class="[{'ads-item--width-img': item.Picture}, 'ads-item']">
    <div 
      class="ads-item__img"
      :style="{backgroundImage: `url(${item.Picture})`}"
      v-if="item.Picture"
    ></div>
    <div class="ads-item__contact-data">
      <div class="ads-item__contact-data__row">
        <div>{{item.Sender}}</div>
        <div>{{getTime(item.DateCreate)}}</div>
      </div>
      <div class="ads-item__contact-data__row">
        <div>#{{item.SenderUUID}}</div>
        <div>{{item.Simnum}}</div>
      </div>
    </div>
    <div class="ads-item__text">{{item.Title}}</div>
    <div 
      :class="[{'ads-item__btn--editor': item.Status !== 'Created'}, 'ads-item__btn', 'btn']"
      @click="setCurrentAd(item)"
    >{{loc(textBtnAd)}}</div>
  </div>
</template>

<script>
import { mapGetters, mapMutations } from 'vuex'
export default {
  name: 'AdsItem',

  props: {
    item: Object
  },
  data: function() {
    return {
      options: {
        timezone: 'UTC',
        hour: 'numeric',
        minute: 'numeric'
      }
    }
  },

  computed: {
    textBtnAd: function() {
      if (this.item.Status === 'Created') {
        return 'new:ads:edit'
      }
      return this.item.Editor
    },
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    ...mapMutations('news', ['setCurrentAd']),
    getTime(seconds) {
      return new Date(seconds * 1000).toLocaleString("ru", this.options); 
    }
  }
}
</script>

<style lang="scss" scoped>
.ads-item{
  display: flex;
  flex-direction: column;
  justify-content: space-between;
  padding: .7rem;
  background: #FFFFFF;
  border-radius: .3rem;
  overflow: hidden;
  position: relative;
  &--width-img{
    padding: 5.45rem .7rem .7rem;
  }
  &__img{
    width: 100%;
    height: 4.75rem;
    position: absolute;
    background-size: cover;
    background-repeat: no-repeat;
    background-position: center;
    left: 0;
    top: 0;
  }
  &__contact-data{
    display: flex;
    flex-direction: column;
    padding-bottom: .7rem;
    margin-bottom: .7rem;
    border-bottom: 1px solid rgba(70, 85, 95, 0.2);
    &__row{
      display: flex;
      align-items: center;
      justify-content: space-between;
      margin-bottom: .5rem;
      font-weight: 600;
      font-size: .8rem;
      line-height: .9rem;
      color: rgba(70, 85, 95, 0.5);
      &:last-child{
        margin-bottom: 0;
      }
    }
  }
  &__text{
    font-weight: 600;
    font-size: .9rem;
    line-height: 1.05rem;
    color: rgba(0, 0, 0, 0.7);
    height: 5.25rem;
    overflow: hidden;
    text-overflow: ellipsis;
  }
  &__btn{
    font-weight: 600;
    font-size: .8rem;
    line-height: .9rem;
    color: #FFFFFF;
    background: linear-gradient(0deg, #E30000, #E30000);
    border-radius: .3rem;
    text-align: center;
    height: 2.5rem;
    &--editor{
      &:before{
        content: '';
        width: .7rem;
        height: .7rem;
        background-image: url('/img/news/icon-editor.svg');
        background-size: contain;
        background-repeat: no-repeat;
        background-position: center;
        margin-right: .5rem;
      }
    }
    &:hover{
      box-shadow: 0 0 .6rem #D92C2C;
    }
  }
}
</style>
