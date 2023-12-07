<template>
  <div class="choice weapon-choice">
    <div class="weapon">{{loc('fam:btl:choice:weap')}}</div>
    <div class="desc">{{loc('fam:btl:choice:desc')}}</div>
    <div class="weapon-list">
      <div 
        :class="[{active: item.id === currentWeapon}, 'item']"
        @click="setCurrentWeapon(item.id)"
        v-for="item in weaponsList"
        :key="item.key"
      >
        <div class="item__name">{{loc(item.name)}}</div>
        <div 
          class="item__img"
          :style="{backgroundImage: 'url(/img/familyMenu/eventsPage/weapons/' + item.img + '.png)'}"
        ></div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapState, mapGetters } from 'vuex'
export default {
  name: 'WeaponChoice',

  props: {
    setCurrentWeapon: Function,
    currentWeapon: Number
  },

  computed: {
    ...mapGetters('localization', ['loc']),
    ...mapState('familyMenu/eventsPage', ['weaponsList'])
  }
}
</script>

<style lang="scss" scoped>
.weapon-choice{
  display: flex;
  flex-flow: column;
  .weapon{
    font-size: 2rem;
    line-height: 2.4rem;
    letter-spacing: 0.03em;
    margin-bottom: .1rem;
  }
  .desc{
    font-size: 1.2rem;
    line-height: 1.45rem;
    letter-spacing: 0.03em;
    color: rgba(255, 255, 255, 0.5);
    margin-bottom: 1.5rem;
  }
  .weapon-list{
    display: grid;
    width: calc(100% + (.8rem) + (.4rem));
    margin-left: -.4rem;
    max-height: calc(100vh - (18rem));
    overflow-y: auto;
    padding: 0 .8rem .4rem .4rem;
    grid-template-columns: repeat(2, 1fr);
    grid-column-gap: 1em;
    grid-row-gap: 1.2rem;
    grid-auto-rows: 7.55rem;
    &::-webkit-scrollbar{
      background-color: transparent;
      width: .2rem;
      &-thumb{
        background: rgba(255, 255, 255, 0.1);
        border-radius: .1rem;
      }
    }
    .item{
      width: 100%;
      position: relative;
      display: flex;
      flex-flow: column;
      align-items: center;
      justify-content: flex-end;
      background: linear-gradient(180deg, rgba(255, 255, 255, 0) 0%, rgba(255, 255, 255, 0.24) 99.95%), #0F0F10;
      &:hover{
        &:after{
          opacity: 1;
          transition: .2s;
        }
      }
      &:before, &:after{
        content: '';
        position: absolute;
        width: 100%;
        opacity: 0;
      }
      &:before{
        height: .2rem;
        bottom: -.2rem;
        transform: translateY(100%);
        background: #DCFF01;
      }
      &:after{
        height: 100%;
        bottom: 0;
        left: 0;
        z-index: 1;
        background-image: url('/img/familyMenu/eventsPage/weapon-bg.svg');
        background-size: 100%;
        background-position: center bottom;
        background-repeat: no-repeat;
      }
      &.active{
        &:before, &:after{
          opacity: 1;
        }
      }
      &__name{
        font-weight: bold;
        font-size: 1.2rem;
        line-height: 1.2rem;
        text-align: center;
      }
      &__img{
        width: 100%;
        height: 4.9rem;
        background-size: cover;
        background-position: center;
        background-repeat: no-repeat;
        overflow: hidden;
      }
    }
  }
}
</style>
