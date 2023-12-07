<template>
  <div class="edit-ad">
    <div class="edit-ad-info">
      <div class="edit-ad-info__row">
        <div>{{adInfo.Sender}}</div>
        <div>{{getTime(adInfo.DateCreate)}}</div>
      </div>
      <div class="edit-ad-info__row">
        <div>#{{adInfo.SenderUUID}}</div>
        <div>{{adInfo.Simnum}}</div>
      </div>
    </div>
    <div 
      v-if="!adInfo.Picture && adInfo.PrimeAdvert && adInfo.Status === 'Created'"
      class="edit-ad-img edit-ad-img--empty"
      :style="{backgroundImage: `url(${adInfo.Picture})`}"
    >
    </div>
    <div 
      class="edit-ad-img"
      :style="{backgroundImage: `url(${adInfo.Picture})`}"
      v-else-if="isPremiumAd"
    >
      <div 
        class="edit-ad-img__btn edit-ad-img__btn--zoom"
        @click="zoomImage"
        v-if="adInfo.Status === 'Created'"
      ></div>
      <div 
        class="edit-ad-img__btn edit-ad-img__btn--delete"
        @click="deleteImage"
        v-if="adInfo.Status === 'Created'"
      ></div>
    </div>
    <div class="edit-ad-edit">
      <div class="edit-ad-edit-title">{{loc('news:edit:1')}}</div>
      <textarea 
        :class="[{edited: adInfo.Status !== 'Created'}, 'edit-ad-edit-text']"
        v-model="adInfo.Title"
        :readonly="adInfo.Status !== 'Created'"
      ></textarea>
    </div>
    <div 
      class="edit-ad-affirmation"
      v-if="adInfo.Status === 'Created'"
    >
      <div class="edit-ad-affirmation__title">{{loc('news:edit:2')}} <span> ${{(pricePerSymbol * adInfo.Title.length) + isPremiumAd}}</span></div>
      <div class="edit-ad-affirmation__btns">
        <div 
          class="edit-ad-affirmation__btns-btn btn edit-ad-affirmation__btns-btn--apply"
          @click="applyAd"
        >{{loc('news:edit:3')}}</div>
        <div 
          class="edit-ad-affirmation__btns-btn btn"
          @click="rejectAd"
        >{{loc('news:edit:4')}}</div>
      </div>
    </div>
    <div 
      class="edit-ad-affirmation"
      v-if="adInfo.Status === 'Compleate'"
    >
      <div class="edit-ad-affirmation__title">{{loc('news:edit:5')}} <span> {{adInfo.Editor}}</span></div>
      <div class="edit-ad-affirmation__evaluate">
        <div 
          class="edit-ad-affirmation__evaluate-star"
          v-for="(item, index) in 5"
          :key="index"
        ></div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapMutations, mapState, mapGetters } from 'vuex'
export default {
  name: 'EditAd',

  data: function() {
    return {
      adInfo: {},
      isAddingImage: null,
      options: {
        timezone: 'UTC',
        hour: 'numeric',
        minute: 'numeric'
      }
    }
  },

  computed: {
    ...mapGetters('localization', ['loc']),
    ...mapState('news', ['currentAd', 'pricePerSymbol', 'pricePrePremium']),
    isPremiumAd: function() {
      if (this.adInfo.PrimeAdvert) {
        return this.pricePrePremium
      }
      return 0
    }
  },

  methods: {
    ...mapMutations('news', ['setModal']),
    getTime(seconds) {
      return new Date(seconds * 1000).toLocaleString("ru", this.options); 
    },
    deleteImage: function() {
      this.adInfo.Picture = null;
      // window.mp.trigger('news:deleteImage', this.adInfo)
    },
    zoomImage: function() {
      this.setModal({show: true, currentModal: 'ImageModal', data: this.adInfo.Picture})
    },
    applyAd: function() {
      window.mp.triggerServer('lsnews::compleateAdvert', this.adInfo.Id, this.adInfo.Title, this.adInfo.Picture)
    },
    rejectAd: function() {
      window.mp.triggerServer('lsnews::canceledAdvert', this.adInfo.Id)
    },
    // addNewImage: function() {
    //   window.mp.trigger('news:addNewImage', this.isAddingImage)
    //   this.setClarification
    // }
  },

  watch: {
    currentAd: function (val) {
      this.adInfo = {...val}
    }
  },

  mounted() {
    this.adInfo = {...this.currentAd}
  }
}
</script>

<style lang="scss" scoped>
.edit-ad{
  display: flex;
  flex-direction: column;
  align-items: center;
  &-info, &-img, &-edit{
    width: calc(100% - (2rem));
  }
  &-info{
    font-weight: 600;
    font-size: .9rem;
    line-height: 1.05rem;
    color: #212121;
    display: flex;
    flex-direction: column;
    padding-bottom: .85rem;
    border-bottom: 1px solid rgba(0, 0, 0, 0.1);
    margin-bottom: 1rem;
    &__row{
      display: flex;
      align-items: center;
      justify-content: space-between;
      margin-bottom: .5rem;
      &:last-child{
        margin-bottom: 0;
      }
    }
  }
  &-img{
    height: 7.6rem;
    background-size: cover;
    background-position: center;
    margin-bottom: 1rem;
    border-radius: .3rem;
    overflow: hidden;
    position: relative;
    &--empty{
      border: 1px dashed #D3D3D3;
      padding: 0 1rem;
      display: flex;
      align-items: flex-start;
      justify-content: center;
      flex-direction: column;
    }
    &__btn{
      position: absolute;
      width: 1.2rem;
      height: 1.2rem;
      background-size: contain;
      background-position: center;
      background-repeat: no-repeat;
      opacity: .8;
      transition: .2s;
      &:hover{
        opacity: 1;
      }
      &--zoom{
        top: .5rem;
        right: .5rem;
        background-image: url('/img/news/icon-zoom.svg');
      }
      &--delete{
        bottom: .5rem;
        left: .5rem;
        background-image: url('/img/news/icon-delete.svg');
      }
    }
    &-desc{
      font-weight: normal;
      font-size: .8rem;
      line-height: .9rem;
      color: rgba(70, 85, 95, 0.7);
      margin-bottom: .65rem;
    }
    &-input{
      width: 100%;
      padding: 0 .7rem;
      height: 2.1rem;
      border: 1px solid rgba(126, 136, 143, 0.4);
      box-sizing: border-box;
      border-radius: .1rem;
      font-size: 14px;
      color: rgba(70, 85, 95, 0.7);
      &::placeholder{
        color: rgba(70, 85, 95, 0.4);
      }
    }
  }
  &-edit{
    display: flex;
    flex-direction: column;
    color: #000000;
    &-title{
      font-weight: bold;
      font-size: 1.2rem;
      line-height: 1.35rem;
      margin-bottom: .5rem;
    }
    &-text{
      width: 100%;
      border: 1px solid rgba(0, 0, 0, 0.1);
      box-sizing: border-box;
      border-radius: .1rem;
      height: 17.5rem;
      padding: .6rem .8rem;
      font-weight: 500;
      font-size: .9rem;
      line-height: 1.1rem;
      resize: none;
      outline: none;
      &.edited{
        border: 0;
        padding: 0;
      }
    }
  }
  &-affirmation{
    width: 100%;
    position: absolute;
    left: 0;
    bottom: 0;
    height: 7.7rem;
    background: #FFFFFF;
    box-shadow: 0 -1px .5rem rgba(0, 0, 0, 0.08);
    display: flex;
    flex-direction: column;
    padding: 1rem;
    &__title{
      font-weight: bold;
      font-size: .9rem;
      line-height: 1.05rem;
      color: #000000;
      margin-bottom: 1.15rem;
      span{
        font-weight: 500;
      }
    }
    &__btns{
      display: flex;
      align-items: center;
      &-btn{
        margin-right: 1rem;
        width: 100%;
        height: 2.5rem;
        font-weight: 600;
        font-size: .8rem;
        line-height: .9rem;
        text-align: center;
        color: #E30000;
        border: 1px solid #E30000;
        box-sizing: border-box;
        border-radius: .3rem;
        transition: .3s;
        &:hover{
          filter: drop-shadow(0 0 .15rem #D92C2C);
        }
        &:last-child{
          margin-right: 0;
        }
        &--apply{
          background: linear-gradient(0deg, #E30000, #E30000);
          color: #fff;
          border: none;
          &:hover{
            box-shadow: 0 0 .6rem #D92C2C;
          }
        }
      }
    }
    &__evaluate{
      display: flex;
      align-items: center;
      justify-content: center;
      padding-top: 1rem;
      width: calc(100% + 2rem);
      margin-left: -1rem;
      border-top: 1px solid rgba(70, 85, 95, .2);
      &:hover{
        .edit-ad-affirmation__evaluate-star{
          background-image: url('/img/news/star-active.svg');
        }
      }
      &-star{
        width: calc((2rem) + (1.1rem));
        height: 2rem;
        background-size: contain;
        background-position: left center;
        background-repeat: no-repeat;
        background-image: url('/img/news/star.svg');
        padding-right: 1.1rem;
        &:last-child{
          padding-right: 0;
          width: 2rem;
        }
        &:hover ~ .edit-ad-affirmation__evaluate-star{
          background-image: url('/img/news/star.svg');
        }
        &.active{
          background-image: url('/img/news/star-active.svg');
        }
      }
      // &-star{
      //   width: 2rem;
      //   height: 2rem;
      //   border: 1px solid red;
      // }
    }
  }
}
</style>
