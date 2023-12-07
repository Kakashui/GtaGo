<template>
  <div class="weather-tab">
    <main-content :weatherData="weatherData" />
  </div>
</template>

<script>
import { mapMutations, mapState } from "vuex";
import MainContent from "./MainContent";

export default {
  name: "WeatherTab",

  components: {
    MainContent,
  },

  computed: {
    ...mapState("smartphone/weatherPage", ["weatherData"]),
  },

  methods: {
    ...mapMutations("smartphone", ["setColorTheme"]),
  },

  mounted: function () {
    this.setColorTheme({ header: "dark", button: "dark" });
  },
};
</script>

<style lang="scss" scoped>
.weather-tab {
  width: 100%;
  height: 100%;
}
</style>
