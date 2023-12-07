<template>

  <div class="battle-row" :style="battleRowStyle">
    <img :style="{ 'filter': data.modal ? 'drop-shadow(0px 0px 0.3rem #FFFFFF)' : 'none', 'margin-left': data.modal ? '3.1rem' : '1.5rem' }" :src="`/img/familyMenu/forward-${ data.incoming ? 'left' : 'right' }.svg`">
    <ul :style="ulStyle">
      <li><div class="column-desc" v-if="data.modal">{{loc('fam:btl:panel:1')}}</div>{{ `${data.bizName} (${data.bizId})` }}</li>
      <li><div class="column-desc" v-if="data.modal">{{loc('fam:btl:panel:2')}}</div>{{ data.enemyName }}</li>
      <li><div class="column-desc" v-if="data.modal">{{loc('fam:btl:panel:3')}}</div>{{ data.date }}</li>
      <li><div class="column-desc" v-if="data.modal">{{loc('fam:btl:panel:4')}}</div>{{ data.time }}</li>
      <li><div class="column-desc" v-if="data.modal">{{loc('fam:btl:panel:5')}}</div>{{ loc(getWeaponName(data.weaponName)) }}</li>
      <li><div class="column-desc" v-if="data.modal">{{loc('fam:btl:panel:6')}}</div>{{ loc(`Place_${data.placeName}`) }}</li>
    </ul>
    <div class="battle-row__status" v-if="!data.modal">
      <img v-if="data.completed" class="status-icon" :src="`/img/familyMenu/battle-row-${getPictureBattle(data.status, data.won)}.svg`">
      <div class="status-text" :style="statusTextStyle">{{ loc(getStatusBattle(data.status, data.won))}}</div>
    </div>
  </div>

</template>

<script>
import { mapGetters, mapState } from 'vuex'
export default {
  name: 'BattleRow',
  props: {
    data: {
      type: Object,
      required: true
    }
  },
  computed: {
    ...mapGetters('localization', ['loc']),
    ...mapState('familyMenu/eventsPage', ['weaponsList']),
    battleRowStyle: function() {
      return {
        'background-image': this.data.completed ? (this.data.incoming ? 'url(/img/familyMenu/battle-row-red.svg)' : 'url(/img/familyMenu/battle-row-green.svg)') : (this.data.incoming ? 'url(/img/familyMenu/battle-row-red-active.svg)' :'url(/img/familyMenu/battle-row-green-active.svg)'),
        'background-color': this.data.modal ? '#333334' : 'rgba(255, 255, 255, 0.1)',
        'background-size': this.data.modal ? '20.3rem' : '15.24rem',
        'height': this.data.modal ? '8.35rem' : '6.3rem',
        'width': this.data.modal ? '90rem' : '100%',
        'margin-left': this.data.modal ? '15rem' : '0'
      }
    },
    statusTextStyle: function() {
      return {
        'color': this.data.completed ? (this.data.won ? '#DCFF00' : '#F02D2D') : '#909090'
      }
    },
    ulStyle: function() {
      return {
        'font-size': this.data.modal ? '1.4rem' : '1.1rem',
        'grid-template-columns': this.data.modal ? '18rem 16.25rem 7.65rem 5rem 10.6rem 10rem' : '12.2rem 11rem 7.1rem 4.8rem 7.7rem 10rem'
      }
    }
  },
  methods: {
    getStatusBattle: function(status, won) {
      switch (status)
      {
        case 0:
          return 'fam:btl:status:1';
        case 1:
          return 'fam:btl:status:2';
        case 2:
          return 'fam:btl:status:3';
        case 3:
          return 'fam:btl:status:4';
        case 4:
          if (won)
            return 'fam:btl:status:5';
          else
            return 'fam:btl:status:6';
        case 5:
          if (won)
            return 'fam:btl:status:7';
          else
            return 'fam:btl:status:8';
      }
      return 'Unknown';
    },
    getPictureBattle: function(status, won) {
      switch (status)
      {
        case 3:
          return 'lost';
        case 4:
          if (won)
            return 'won';
          else
            return 'lost-cry';
        case 5:
          if (won)
            return 'won';
          else
            return 'lost';
      }
      return 'Unknown';
    },
    getWeaponName: function(id) {
      let index = this.weaponsList.findIndex(item => item.id == id);
      if (index > -1)
        return this.weaponsList[index].name;
      else
        return 'Unknown';
    }
  }
}
</script>

<style lang="scss" scoped>


.battle-row {
  width: 100%;
  height: 6.3rem;
  background-repeat: no-repeat;
  background-image: url(/img/familyMenu/battle-row-green.svg);
  border-radius: 0.5rem;
  background-position-x: -0.1rem;
  background-position-y: -0.08rem;
  border: 0.1rem inset rgba(255, 255, 255, 0.4);
  display: flex;

  margin-bottom: 1rem;

  .column-desc {
   text-transform: uppercase;
    font-style: normal;
    font-weight: normal;
    font-size: 1.1rem;
    letter-spacing: 0.04em;

    color: rgba(255, 255, 255, 0.6);
  }
  
  &__status {
    display: block;
    text-align: center;
    align-self: center;
    width: inherit;

    .status-icon {
      margin: 0.25rem;
    }

    .status-text {
     text-transform: uppercase;
      font-style: normal;
      font-weight: bold;
      font-size: 1.1rem;
      letter-spacing: 0.04em;
      margin: 0.25rem
    }
  }

  & img {
    margin-top: 0.3rem;
    vertical-align: middle;
  }
  & ul {
    list-style: none;
    display: grid;
    margin-left: 1.5rem;
    height: inherit;
    align-content: center;
    align-items: center;

    & li {
     text-transform: uppercase;
      font-style: normal;
      font-weight: normal;
      letter-spacing: 0.04em;
      color: #FFFFFF;
      text-align: left;
      overflow-wrap: anywhere;
    }
  }
}

</style>
