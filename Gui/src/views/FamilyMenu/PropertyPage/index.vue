<template>
  <div class="property-page">
    <template v-if="propertyList.length > 0">
      <div class="property-list">
        <div 
          :class="[{active : item.id === currentProperty.id}, 'property-list__item']"
          v-for="item in propertyList"
          :key="item.key"
          @click="setCurrentProperty(item)"
        >
          {{item.name}}
        </div>
      </div>
      <div class="property-info">
        <div class="property-info__title">{{currentProperty.name}} <span>ID: {{currentProperty.id}}</span></div>
        <div class="property-info__stats">
          <div 
            class="item"
            :id="'item_' + index"
            v-for="(item, index) in currentProperty.stats"
            :key="item.key"
          >
            <div class="item__icon"></div>
            <div class="item__info">
              <div class="desc">{{item.desc}}</div>
              <div class="value">{{item.value}}</div>
            </div>
          </div>
        </div>
        <div class="property-info__interaction">
          <div 
            class="img"
            :style="{backgroundImage: 'url(/img/familyMenu/propertyPage/property-' + currentProperty.img + '.png)'}"
          ></div>
          <div class="info">
            <div class="desc">{{loc('familyMenu_59')}}</div>
            <div class="owner">{{currentProperty.owner}}</div>
            <div 
              class="btn btn-gps"
              @click="setOnGps"
            >{{loc('familyMenu_60')}}</div>
          </div>
        </div>
      </div>
    </template>
    <div 
      v-else
      class="empty-list"
    >{{loc('fam:menu:prop:notfound')}}</div>
  </div>
</template>

<script>
import { mapState, mapGetters } from 'vuex'
export default {
  name: 'PropertyPage',

  data: function() {
    return {
      currentProperty: {},
    }
  },

  computed: {
    ...mapState('familyMenu/propertyPage', ['propertyList']),
    ...mapGetters('localization', ['loc']),
  },

  methods: {
    setCurrentProperty: function(obj) {
      this.currentProperty = {...obj}
    },
    setOnGps: function() {
      window.mp.trigger('familyMenu:setOnGps', this.currentProperty.id)
    }
  },

  mounted() {
    this.currentProperty = {...this.propertyList[0]}
  }
}
</script>

<style lang="scss" scoped>
.property-page{
  display: flex;
  margin-top: 3rem;
  width: 100%;
  .empty-list{
    width: 100%;
    position: relative;
    padding-top: 11rem;
    font-size: 2.5rem;
    display: flex;
    align-items: center;
    justify-content: center;
    text-align: center;
    &:before{
      content: '';
      width: 7rem;
      height: 7rem;
      position: absolute;
      bottom: -2.5rem;
      transform: translateY(100%);
      background-size: contain;
      background-position: center;
      background-repeat: no-repeat;
      background-image: url('/img/familyMenu/world-grid.svg');
    }
  }
  .property-list{
    display: flex;
    flex-flow: column;
    overflow-y: auto;
    height: calc(100vh - (7.4rem));
    padding-right: 1rem;
    margin-left: -1rem;
    transform: rotateY(180deg);
    &::-webkit-scrollbar{
      background: rgba(255, 255, 255, 0.1);
      border-radius: .1rem;
      width: .1rem;
      &-thumb{
        background: rgba(255, 255, 255, 0.6);
        border-radius: .1rem;
      }
    }
    &__item{
      display: flex;
      align-items: center;
      justify-content: flex-start;
      padding-left: 1rem;
      width: 15.5rem;
      min-height: 3.5rem;
      height: 3.5rem; 
      font-size: 1.2rem;
      text-transform: uppercase;
      color: #FFFFFF;
      border-left: 2px solid transparent;
      transform: rotateY(-180deg);
      &:hover, &.active{
        background: #1C1D20;
        color: #DCFF00;
        border-color: #DCFF00;
        transition: .2s;
      }
    }
  }
  .property-info{
    display: flex;
    flex-flow: column;
    margin-left: 7.5rem;
    &__title{
      display: flex;
      font-size: 3rem;
      line-height: 3rem;
      margin-bottom: 1.5rem;
      align-items: flex-end;
      span{
        font-size: 1.2rem;
        line-height: 1.45rem;
        color: rgba(255, 255, 255, 0.7);
        margin-left: .6rem;
        margin-bottom: .5rem
      }
    }
    &__stats{
      display: flex;
      align-items: center;
      padding-bottom: 2.2rem;
      margin-bottom: 2.2rem;
      position: relative;
      &:before{
        content: '';
        width: 100%;
        height: 1px;
        position: absolute;
        left: 0;
        bottom: 0;
        background: linear-gradient(270deg, rgba(255, 255, 255, 0) 0%, rgba(255, 255, 255, 0.5) 51.04%, rgba(255, 255, 255, 0) 100%);
      }
      .item{
        display: flex;
        align-items: flex-start;
        padding: 0 1.4rem;
        position: relative;
        &:before{
          content: '';
          height: 100%;
          width: 1px;
          position: absolute;
          top: 0;
          right: 0;
          background: linear-gradient(180deg, rgba(255, 255, 255, 0.5) 0%, rgba(255, 255, 255, 0) 100%);
        }
        &:first-child{
          padding-left: 0;
        }
        &:last-child{
          padding-right: 0;
          &:before{
            content: none;
          }
        }
        &#item_0{
          .item__icon{
            background-image: url('/img/familyMenu/propertyPage/icon-0.svg');
          }
        }
        &#item_1{
          .item__icon{
            background-image: url('/img/familyMenu/propertyPage/icon-1.svg');
          }
        }
        &#item_2{
          .item__icon{
            background-image: url('/img/familyMenu/propertyPage/icon-2.svg');
          }
        }
        &__icon{
          width: 2.2rem;
          height: 2.2rem;
          background-size: contain;
          background-repeat: no-repeat;
          background-position: center;
          margin-right: 1rem;
        }
        &__info{
          display: flex;
          flex-flow: column;  
          .desc{
            font-size: 1.2rem;
            line-height: 1.2rem;
            text-transform: uppercase;
            color: rgba(255, 255, 255, 0.6);
            margin-bottom: .3rem;
          }
          .value{
            font-weight: bold;
            font-size: 2rem;
            line-height: 2rem;
            color: #FFFFFF;
          }
        }
      }
    }
    &__interaction{
      display: flex;
      align-items: flex-start;
      .img{
        height: 15rem;
        width: 22.25rem;
        margin-right: 1.65rem;
        background-size: cover;
        background-repeat: no-repeat;
        background-position: center;
      }
      .info{
        display: flex;
        flex-flow: column;
        .desc{
          font-size: 1.2rem;
          line-height: 1.2rem;
          color: rgba(255, 255, 255, 0.6);
          margin-bottom: .4rem;
        }
        .owner{
          font-size: 2rem;
          line-height: 1.4rem;
          margin-bottom: .5rem;
        }
        .btn-gps{
          background: #C1E704;
          border-radius: .2rem;
          font-size: 1rem;
          padding: .6rem 1rem;
          display: flex;
          align-items: center;
          &:before{
            content: '';
            width: 1.2rem;
            height: 1.2rem;
            margin-right: .5rem;
            background-size: contain;
            background-repeat: no-repeat;
            background-position: center;
            background-image: url('/img/familyMenu/propertyPage/btn-gps.svg');
          }
          &:hover{
            box-shadow: 0 0 1rem #C1E704;
          }
        }
      }
    }
  }
}
</style>
