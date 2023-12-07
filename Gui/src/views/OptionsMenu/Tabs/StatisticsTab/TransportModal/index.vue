<template>
  <div class="modal">
    <close-button :styles="{ position: 'absolute', top: '2rem', right: '2rem' }" @close="$emit('close')" />
    <div class="modal__side">
      <div class="title">{{loc('mmain:stats:veh:tit')}}</div>
      <div class="info">
        <div class="info__count">{{ property.transport.length }}</div>
        <div class="info__text"> {{loc('mmain:stats:veh:desc1')}}</div>
      </div>
      <div class="list">
        <list-item
          v-for="(item, index) in property.transport"
          :key="index"
          :item="item"
          :currentId="currentCar.id"
          @set-current-car="setCurrentCar"
        />
      </div>
    </div>
    <div class="modal__main">
      <div class="header">
        <div class="header__title">{{loc('mmain:stats:veh:contr')}}</div>
        <div class="header__info">
          <div class="title">{{ currentCar.name }}</div>
          <div class="numbers">{{loc('mmain:stats:veh:number')}}: {{ currentCar.numbers }}</div>
        </div>
      </div>
      <div class="desc">{{loc('mmain:stats:veh:desc2')}}</div>
      <div class="actions">
        <actions-item
          v-for="item in actionsItems"
          :key="item.id"
          :item="item"
          :currentCarId="currentCar.id"
          @onClick="onAction"
        />
      </div>
    </div>
  </div>
</template>

<script>
import { mapState, mapGetters, mapMutations} from 'vuex'
import ListItem from './common/ListItem'
import ActionsItem from './common/ActionsItem'
import CloseButton from '../../../common/CloseButton.vue'

export default {
  name: 'TransportModal',

  components: {
    ListItem,
    ActionsItem,
    CloseButton
  },

  data: function() {
    return {
      currentCar: null,
    }
  },

  computed: {
    ...mapState('optionsMenu', ['property']),
    ...mapGetters('localization', ['loc']),
    actionsItems: function() {
      return [
        { id: 0, title: 'mmain:stats:veh:take', icon: 'car1', type: 1, event: 'mmenu:vehicle:togarage' },
        { id: 1, title: 'mmain:stats:veh:gkey', icon: 'key', type: 1, event: 'mmenu:vehicle:makekey' },
        { id: 2, title: 'mmain:stats:veh:chkey', icon: 'key', type: 1, event: 'mmenu:vehicle:changekey' },
        // { id: 3, title: 'mmain:stats:veh:sell', price: this.currentCar.price, icon: 'blank', type: 2, event: 'mmenu:vehicle:sell' },
        { id: 4, title: 'mmain:stats:veh:search', icon: 'transfer', type: 1, event: 'vehicle::key::enableGPS' },
        { id: 5, title: 'mmain:stats:veh:act:sellto', icon: 'sellto', type: 1, event: 'mmenu:cars:sell:toplayer' }
      ]
    }
  },

  methods: {
    ...mapMutations('optionsMenu', ['setDialog']),
    setCurrentCar: function(item) {
      this.currentCar = item
    },
    onAction(event){
      if(this.currentCar == null) return;
      if(event === "mmenu:cars:sell:toplayer"){
        this.setDialog({
            input: 'number',
            input2: 'number',
            callback: (val, val2)=>{
              val = parseInt(val);
              val2 = parseInt(val2);
              if(isNaN(val) || isNaN(val2)|| val < 1 || val2 < 1){
                window.setData('notifyList/notify', {type: 1, position:2, message: "mmain:frac:dialog:data:wrong", time: 3000});
              }else{
               window.mp.triggerServer(event, this.currentCar.id, val, val2);
              }

            },
            value: '',
            placeholder: 'mmain:stats:biz:pcell:pl',
            placeholder2: 'mmain:stats:biz:pcell:pl2',
            tittle: `mmain:stats:veh:act:sellto`,
            subtittle: 'mmain:stats:veh:sellto:sub',
            bg: undefined
        });
      } else window.mp.triggerServer(event, this.currentCar.id)
    }
  },

  created: function() {
    this.setCurrentCar(this.property.transport[0])
  }
}
</script>

<style lang="scss" scoped>
.modal {
  width: 70rem;
  height: 46rem;
  background: 100% 100% / contain no-repeat url("/img/optionsMenu/statisticsTab/transportModal/bg.png"), #070A11;
  position: absolute;
  top: 0;
  left: 0;
  bottom: 0;
  right: 0;
  margin: auto;
  box-shadow: 0px 0px 8.5rem rgba(182, 211, 0, 0.1);
  padding: 4rem 5.5rem 0 5.5rem;
  display: flex;
  &__side {
    margin: 0 3.9rem 0 0;
    .title {
      font-size: 5rem;
      line-height: 5rem;
      color: #fff;
    }
    .info {
      display: flex;
      margin: 0 0 1.85rem 0;
      &__count {
        font-size: 2.7rem;
        letter-spacing: 0.03em;
        color: #fff;
        display: flex;
        line-height: 2.5rem;
        margin: 0 0.75rem 0 0;
        &::before {
          content: "";
          width: 2.5rem;
          height: 2.5rem;
          background: center / cover no-repeat url("/img/optionsMenu/statisticsTab/transportModal/car.png");
          margin: 0 0.25rem 0 0;
        }
      }
      &__text {
        color: rgba(255, 255, 255, 0.5);
        font-size: 0.9rem;
        line-height: 1.2rem;
        letter-spacing: 0.03em;
        font-weight: 300;
        width: 15rem;
      }
    }
    .list {
      height: 28.7rem;
      padding: 0 1rem 0 0;
      width: fit-content;
      overflow-x: auto;
      &::-webkit-scrollbar {
        width: 0.15rem;
        background: transparent;
      }
      &::-webkit-scrollbar-thumb {
        background: rgba(255, 255, 255, 0.05);
      }
    }
  }
  &__main {
    padding: 1.5rem 0 0 0;
    .header {
      display: flex;
      margin: 0 0 0.5rem 0;
      &__title {
        font-size: 3rem;
        line-height: 3rem;
        color: #fff;
        margin: 0 1rem 0 0;
      }
      &__info {
        position: relative;
        padding: 0 0 0 1rem;
        letter-spacing: 0.03em;
        &::before {
          content: "";
          position: absolute;
          left: 0;
          top: 4%;
          height: 2.5rem;
          width: 0.1rem;
          border-radius: 0.1rem;
          background: #B6D300;
          box-shadow: 0px 0px 1rem rgba(168, 195, 2, 0.7);
        }
        .title {
          font-size: 1.5rem;
          line-height: 1.5rem;
          margin: 0 0 0.2rem 0;
          color: #fff;
        }
        .numbers {
          font-size: 0.9rem;
          color: rgba(255, 255, 255, 0.5);
        }
      }
    }
    .desc {
      font-size: 0.9rem;
      letter-spacing: 0.03em;
      line-height: 1.2rem;
      color: rgba(255, 255, 255, 0.5);
      width: 18rem;
      font-weight: 300;
      margin: 0 0 1.85rem 0;
    }
    .actions {
      display: grid;
      grid-template-columns: repeat(3, 10rem);
      grid-auto-rows: 10rem;
      grid-gap: 1rem;
    }
  }
}
</style>
