<template>
  <div class="removing-tattoo">
    <ExitButton/>
    <div class="removing-tattoo__desc">{{loc('removing_tattoo_2')}}</div>
    <div class="removing-tattoo__title">{{loc('removing_tattoo_3')}}</div>
    <div class="removing-tattoo__prompt">{{loc('removing_tattoo_4')}}</div>
    <div class="removing-tattoo__scroll">
      <div class="removing-tattoo__list">
        <div 
          class="removing-tattoo__list-item"
          v-for="(item, index) in tattooList"
          :key="index"
          :class="{'removing-tattoo__selected': item == current}"
          @click="selectTattoo(item)"
        >
          <div class="removing-tattoo__list-item__info">
            <div class="removing-tattoo__list-item__info-desc">{{item.desc}}</div>
            <div class="removing-tattoo__list-item__info-name">{{item.name}}</div>
            <div 
              class="removing-tattoo__list-item__info-img"
              :style="{backgroundImage: `url(${item.img})`}"
            ></div>
          </div>          
        </div>
      </div>      
    </div>
    <div 
      class="removing-tattoo__btn"
      @click="removeTattoo()"
    >
      <div class="removing-tattoo__btn-text">{{loc('removing_tattoo_5')}}</div>
      <div class="removing-tattoo__btn-value">{{current.price ? current.price.toLocaleString() : 0}} $</div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
import ExitButton from './ExitButton'
export default {
  name: 'RemovingTattoo',
  components: {
    ExitButton
  },
  computed: {
    ...mapState('tattooShop', ['tattoos', 'currentTattoos', 'gender']),
    ...mapGetters('localization', ['loc']),    
    ...mapGetters('tattooShop', ['cameraPosition']),
    tattooList(){
      const tattooList = [];
      for (const key in this.tattoos) {
        this.tattoos[key].forEach(tattoo => {
          const index = this.currentTattoos.findIndex(t=>t.Collection === tattoo.Dict && (t.Overlay === tattoo.Male || t.Overlay === tattoo.Female));
          if(index !== -1)
          {
            tattooList.push({
              name: tattoo.Name, 
              img: `img/tattooShop/tattoo/${tattoo.Name.toLowerCase().replace(/\s/g, '_')}.png`,
              price: tattoo.Price,
              desc: 'desc',
              slot: tattoo.Slots[0],
              index
            })
          }
        });        
      }
      return tattooList;
    },
  },
  data() {
    return {
      spamProtection: 0,
      current: 0,
    }
  },
  methods: {
    selectTattoo(item){
      this.current = item; 
      var camera = this.cameraPosition(item.slot);
      window.mp.trigger("camMoveCamZ", camera.cameraZ);
      window.mp.trigger("camMovePointZ", camera.pointZ);
      window.mp.trigger("camMoveAngleX", camera.angle);
    },
    removeTattoo() {
      if(this.current.index === undefined || this.spamProtection > Date.now()) return;
      this.spamProtection = Date.now() + 1000;
      window.mp.triggerServer("tattoo:remove:buy", this.current.index);
    }
  }
}
</script>

<style lang="scss">
.removing-tattoo{
  position: absolute;
  left: 0;
  top: 0;
  width: 100vw;
  height: 100vh;
  background: linear-gradient(90deg, rgba(0, 0, 0, 0.78) 28.43%, rgba(0, 0, 0, 0) 65.46%);
  overflow: hidden;
  padding: 2rem 0 0 2rem;
  color: #fff;
  &__selected{
    border: .1rem solid #A3BC3F;
  }
  &>div{
    z-index: 2;
  }
  &:before, &:after{
    content: '';
    position: absolute;
    border-radius: 50%;
    z-index: 1;
    pointer-events: none;
  }
  &:before{
    width: 27.6rem;
    height: 27.6rem;
    left: -6.2rem;
    top: -8.1rem;
    background: rgba(4, 231, 231, 0.24);
    filter: blur(7.5rem);
  }
  &:after{
    width: 12.5rem;
    height: 12.5rem;
    right: -2.5rem;
    top: -5rem;
    background: rgba(0, 0, 0, 0.6);
    filter: blur(5rem);
  }
  &__desc, &__title{
    letter-spacing: 0.04em;
  }
  &__desc, &__title, &__prompt{
    text-transform: uppercase;
    font-weight: bold;
  }
  &__desc{
    font-size: 1rem;
    line-height: 1.15rem;
    color: #AFCC3B;
  }
  &__title{
    position: relative;
    font-size: 3rem;
    line-height: 3.45rem;
    text-shadow: 0 .5rem 2rem rgba(255, 255, 255, 0.5);
    margin-bottom: 1.45rem;
    &:before{
      content: '';
      width: 6.45rem;
      height: .2rem;
      background: linear-gradient(90deg, #AFCC3B 0%, rgba(175, 204, 59, 0) 100%);
      position: absolute;
      left: .15rem;
      bottom: 0;
      transform: translateY(100%);
    }
  }
  &__prompt{
    font-size: 2rem;
    line-height: 2.3rem;
    margin-bottom: 1rem;
  }
  &__scroll{
    overflow-y: auto;
    height: calc(100% - (15rem));
    width: 22.6rem;
    transform: rotateY(180deg);
    padding-right: 1.3rem;
    margin-left: -1.3rem;
    &::-webkit-scrollbar{
      background: transparent;
      width: .2rem;
      &-thumb{
        background: rgba(255, 255, 255, 0.2);
        border-radius: .5rem;
      }
    }
  }
  &__list{
    display: grid;
    grid-template-columns: repeat(2, 10.1rem);
    grid-row-gap: .5rem;
    grid-column-gap: .5rem;
    grid-auto-rows: 11rem;
    transform: rotateY(-180deg);
    &-item{
      &__info, &__btn{
        background: rgba(255, 255, 255, 0.06);
      }
      &:hover{
        background: rgba(255, 255, 255, 0.3);
      }
      &__info{
        display: flex;
        flex-direction: column;
        padding: 1rem .8rem;
        position: relative;
        height: 11rem;
        &-desc{
          font-weight: normal;
          font-size: 1rem;
          line-height: 1.15rem;
          color: rgba(255, 255, 255, 0.6);
        }
        &-name{
          font-weight: bold;
          font-size: 1rem;
          line-height: 1.6rem;
        }
        &-img{
          position: absolute;
          left: 50%;
          bottom: 1.2rem;
          transform: translateX(-50%);
          width: 5.75rem;
          height: 6.15rem;
          background-size: contain;
          background-repeat: no-repeat;
          background-position: center;
        }
      }      
    }
  }
  &__btn{
        display: flex;
        align-items: center;
        justify-content: center;
        height: 2.3rem;
        font-size: .9rem;
        width: 21.2rem;
        margin-top: 2rem;
        border: .1rem solid rgba(#fff,.4);
        background: rgba(255, 255, 255, 0.2);
        padding: 0 2rem;
        &:hover{
          background: #A3BC3F;
        }
        &-text{
          font-weight: bold;
          letter-spacing: 0.07em;
          text-transform: uppercase;
          display: flex;
          align-items: center;
          &:before{
            content: '';
            margin-right: .3rem;
            width: 1.1rem;
            height: 1.1rem;
            background-size: contain;
            background-repeat: no-repeat;
            background-position: center;
            background-image: url('/img/removingTattoo/close.png');
          }
        }
        &-value{
          color: rgba(255, 255, 255, 0.7);
          margin-left: auto;
        }
      }
}
</style>
