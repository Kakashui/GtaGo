<template>
  <div
    class="main-content"
    :style="{ background: 'center / cover no-repeat url(' + `/img/hud/smartphone/weatherTab/backgrounds/${getWeather.Now.Weather}.png` + ')' }"
  >
    <div class="main-content__header">
      <span class="type">{{ loc(weatherTypes[getWeather.Now.Weather]) }}</span>
      <span class="value">{{ getWeather.Now.Degrees }}&#176;</span>
    </div>
    <div class="main-content__main">
      <span class="title">{{loc('phone:wheather:1')}}</span>
      <div class="list">
        <list-item
          v-for="(item, index) in getWeather.Future"
          :key="index"
          :item="item"
        />
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters } from 'vuex';
import ListItem from './MainContentItem'

export default {
  name: 'MainContent',

  props: {
    weatherData: Object,
  },

  methods: {
    getTime: function (date) {
      let hours = new Date(Date.parse(date)).getHours();
      return `${hours < 10 ? '0' : ''}${hours}:00`;
    },
    getCurrentDegrees: function() {
      let oldDegree = this.weatherData.Future.filter((item) => Date.parse(item.Date) <= Date.now()).slice(-1)[0];
      let newDegree = this.weatherData.Future.filter((item) => Date.parse(item.Date) > Date.now())[0];
      if (!oldDegree)
        oldDegree=newDegree
      if (!newDegree)
        newDegree=oldDegree
      let correct = (Date.now() - Date.parse(oldDegree.Date)) / (1000 * 60 * 60 * 3);
      return Math.round(oldDegree.Degrees + (newDegree.Degrees - oldDegree.Degrees) * correct);
    }
  },

  computed: {
    ...mapGetters('localization', ['loc']),
    getWeather: function () {
      let actualWeatherData = this.weatherData;
      actualWeatherData = {        
        Now: {
            Weather: this.weatherData.Now.Weather,
            Degrees: this.getCurrentDegrees()
        },
        Future: this.weatherData.Future
          .filter((item) => Date.parse(item.Date) > Date.now())          
          .map((item) => ({
            Time: this.getTime(item.Date),
            Weather: item.Weather,
            Degrees: item.Degrees,
          }))
          .slice(0, 5)
      }
      return actualWeatherData;
    }
  },

  components: {
    ListItem
  },

  data: function() {
    return {
      weatherTypes: {
        EXTRASUNNY: 'phone:wheather:2',
        CLEAR: 'phone:wheather:3',
        CLOUDS: 'phone:wheather:4',
        SMOG: 'phone:wheather:5',
        FOGGY: 'phone:wheather:6',
        OVERCAST: 'phone:wheather:7',
        RAIN: 'phone:wheather:8',
        THUNDER: 'phone:wheather:9',
        CLEARING: 'phone:wheather:10',
        SNOW: 'phone:wheather:11',
        SNOWLIGHT: 'phone:wheather:12',
        BLIZZARD: 'phone:wheather:13',
        XMAS: 'phone:wheather:14',
        HALLOWEEN: 'phone:wheather:15',
      }
    }
  }
}
</script>

<style lang="scss" scoped>
.main-content {
  width: 100%;
  height: 100%;
  display: flex;
  flex-direction: column;
  justify-content: flex-end;
  &__header {
    color: #fff;
    display: flex;
    flex-direction: column;
    margin: 0 0 0.5rem 0;
    padding: 0 1rem;
    .type {
      font-size: 0.8rem;
      line-height: 0.95rem;
    }
    .value {
      font-size: 5rem;
      line-height: 5.85rem;
      font-weight: 300;
    }
  }
  &__main {
    background: #fff;
    border-radius: 0.9rem 0.9rem 0 0;
    padding: 1.65rem 1rem 3.05rem 1rem;
    .title {
      font-size: 0.7rem;
      line-height: 0.8rem;
      font-weight: bold;
      margin: 0 0 1rem 0;
      display: block;
    }
    .list {
      display: flex;
    }
  }
}
</style>
