<template>
  <div class="rent-vehicle-item">
    <div 
      class="rent-vehicle-item__img"
      :style="{backgroundImage: `url(/img/vehicles/${item.Image}.png)`}"
    >
      <div class="rent-vehicle-item__interaction">
        <div 
          class="rent-vehicle-item__choice"
          v-if="currentChoice === item.Model"
        >
          <div 
            class="rent-vehicle-item__choice-item"
            @click="setPayment(1)"
          >{{loc('Rent_vehicle_1')}}</div>
          <div 
            class="rent-vehicle-item__choice-item"
            @click="setPayment(0)"
          >{{loc('Rent_vehicle_2')}}</div>
        </div>
        <div 
          v-else
          class="rent-vehicle-item__interaction-btn"
          @click="acceptRent(item.Model)"
        >
          <span>{{loc('Rent_vehicle_3')}}</span>
        </div>
      </div>
    </div>
    <div class="rent-vehicle-item__model">{{item.Name}}</div>
    <div class="rent-vehicle-item__cost">{{item.Price}}</div>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
export default {
  name: 'RentVehicleItem',
  props: {
    item: Object,
    category: Number,
    currentChoice: Number
  },
  computed: {
    ...mapGetters('localization', ['loc'])
  },
  methods: {
    acceptRent(model) {
      this.$emit('selectChoise', model)
    },
    setPayment(payment) {
      window.mp.trigger('vehicleRent:acceptRent', this.item.Model, this.category, payment)
      this.$emit('selectChoise', null)
    }
  }
}
</script>

<style lang="scss">
.rent-vehicle-item{
  display: flex;
  flex-flow: column;
  padding-bottom: .6rem;
  position: relative;
  &:after{
    content: '';
    pointer-events: none;
    width: 100%;
    height: .1rem;
    position: absolute;
    left: 0;
    bottom: 0;
    background: rgba(255, 255, 255, 0.2);
    box-shadow: 0 .2rem 2.5rem rgba(118, 155, 176, 0.5);
  }
  &__img{
    width: 100%;
    height: 8.5rem;
    background-size: cover;
    background-repeat: no-repeat;
    background-position: center;
    margin-bottom: .75rem;
    box-shadow: 0 .2rem 1.1rem rgba(118, 155, 176, 0.5);
  }
  &__interaction{
    width: 100%;
    height: 100%;
    display: flex;
    align-items: center;
    justify-content: center;
    transition: .7s;
    &:hover{
      background-color: rgba(0, 0, 0, 0.6);
      .rent-vehicle-item__interaction-btn{
        opacity: 1;
      }
    }
    &-btn{
      opacity: 0;
      display: flex;
      align-items: center;
      justify-content: center;
      transform: skew(-15deg);
      background: #B6D300;
      border-radius: 1px;
      width: 10rem;
      height: 3.5rem;
      font-weight: bold;
      font-size: 1.2rem;
      letter-spacing: 0.03em;
      transition: .2s;
      span{
        transform: skew(15deg);
      }
      &:hover{
        box-shadow: 0 .5rem 4rem rgba(168, 195, 2, 0.6);
      }
    }
  }
  &__choice{
    display: flex;
    flex-direction: column;
    width: 11.45rem;
    &-item{
      width: 100%;
      height: 3rem;
      background: #FFFFFF;
      border-radius: 1px;
      display: flex;
      align-items: center;
      justify-content: center;
      font-size: 1.2rem;
      line-height: 1.45rem;
      letter-spacing: 0.03em;
      color: #000;
      margin-bottom: .3rem;
      transition: .3s;
      &:last-child{
        margin-bottom: 0;
      }
      &:hover{
        background: #B6D300;
        color: #fff;
        box-shadow: 0 .5rem 4rem rgba(168, 195, 2, 0.6);
      }
    }
  }
  &__model{
    font-size: 1.2rem;
    line-height: 1.45rem;
    letter-spacing: 0.03em;
    color: rgba(255, 255, 255, 0.8);
  }
  &__cost{
    font-weight: normal;
    font-size: 2.2rem;
    line-height: 2.65rem;
    letter-spacing: 0.03em;
  }
}
</style>
