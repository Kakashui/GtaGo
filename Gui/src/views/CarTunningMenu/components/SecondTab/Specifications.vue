<template>
  <div class="second__specifications">
    <span class="specifications__title">{{ specifications.title }}</span>
    <div class="specifications__list">
      <div class="list__item">
        <div class="info">
          <div class="info__current">
            <img src="img/carTunningMenu/specifications/speed.svg" class="icon">
            <span class="value">{{ specifications.speed }}</span>
          </div>
          <div class="info__possible">
            <img
              v-if="specificationItemResult(specifications.speed, specificationsPossible.speed).type === 'up'"
              src="img/carTunningMenu/specifications/triangleUp.svg"
              class="icon"
            >
            <img
              v-else-if="specificationItemResult(specifications.speed, specificationsPossible.speed).type === 'down'"
              src="img/carTunningMenu/specifications/triangleDown.svg"
              class="icon"
            >
            <span :class="[specificationItemResult(specifications.speed, specificationsPossible.speed).type, 'value']">{{ specificationItemResult(specifications.speed, specificationsPossible.speed).value }}</span>
          </div>
        </div>
        <progress-bar
          :currentValue="specifications.speed"
          :difference="specificationItemResult(specifications.speed, specificationsPossible.speed).value"
          :type="specificationItemResult(specifications.speed, specificationsPossible.speed).type"
          :maxValue="specificationMaxValues.speed"
        />
      </div>
      <div class="list__item">
        <div class="info">
          <div class="info__current">
            <img src="img/carTunningMenu/specifications/brakes.svg" class="icon">
            <span class="value">{{ specifications.braking }}</span>
          </div>
          <div class="info__possible">
            <img
              v-if="specificationItemResult(specifications.braking, specificationsPossible.braking).type === 'up'"
              src="img/carTunningMenu/specifications/triangleUp.svg"
              class="icon"
            >
            <img
              v-else-if="specificationItemResult(specifications.braking, specificationsPossible.braking).type === 'down'"
              src="img/carTunningMenu/specifications/triangleDown.svg"
              class="icon"
            >
            <span :class="[specificationItemResult(specifications.braking, specificationsPossible.braking).type, 'value']">{{ specificationItemResult(specifications.braking, specificationsPossible.braking).value }}</span>
          </div>
        </div>
        <progress-bar
          :currentValue="specifications.braking"
          :difference="specificationItemResult(specifications.braking, specificationsPossible.braking).value"
          :type="specificationItemResult(specifications.braking, specificationsPossible.braking).type"
          :maxValue="specificationMaxValues.braking"
        />
      </div>
      <div class="list__item">
        <div class="info">
          <div class="info__current">
            <img src="img/carTunningMenu/specifications/acceleration.svg" class="icon">
            <span class="value">{{ specifications.acceleration }}</span>
          </div>
          <div class="info__possible">
            <img
              v-if="specificationItemResult(specifications.acceleration, specificationsPossible.acceleration).type === 'up'"
              src="img/carTunningMenu/specifications/triangleUp.svg"
              class="icon"
            >
            <img
              v-else-if="specificationItemResult(specifications.acceleration, specificationsPossible.acceleration).type === 'down'"
              src="img/carTunningMenu/specifications/triangleDown.svg"
              class="icon"
            >
            <span :class="[specificationItemResult(specifications.acceleration, specificationsPossible.acceleration).type, 'value']">{{ specificationItemResult(specifications.acceleration, specificationsPossible.acceleration).value }}</span>
          </div>
        </div>
        <progress-bar
          :currentValue="specifications.acceleration"
          :difference="specificationItemResult(specifications.acceleration, specificationsPossible.acceleration).value"
          :type="specificationItemResult(specifications.acceleration, specificationsPossible.acceleration).type"
          :maxValue="specificationMaxValues.acceleration"
        />
      </div>
      <div class="list__item">
        <div class="info">
          <div class="info__current">
            <img src="img/carTunningMenu/specifications/transmission.svg" class="icon">
            <span class="value">{{ specifications.traction }}</span>
          </div>
          <div class="info__possible">
            <img
              v-if="specificationItemResult(specifications.traction, specificationsPossible.traction).type === 'up'"
              src="img/carTunningMenu/specifications/triangleUp.svg"
              class="icon"
            >
            <img
              v-else-if="specificationItemResult(specifications.traction, specificationsPossible.traction).type === 'down'"
              src="img/carTunningMenu/specifications/triangleDown.svg"
              class="icon"
            >
            <span :class="[specificationItemResult(specifications.traction, specificationsPossible.traction).type, 'value']">{{ specificationItemResult(specifications.traction, specificationsPossible.traction).value }}</span>
          </div>
        </div>
        <progress-bar
          :currentValue="specifications.traction"
          :difference="specificationItemResult(specifications.traction, specificationsPossible.traction).value"
          :type="specificationItemResult(specifications.traction, specificationsPossible.traction).type"
          :maxValue="specificationMaxValues.traction"
        />
      </div>
    </div>
  </div>
</template>

<script>
import { mapState } from 'vuex'

import ProgressBar from './SpecificationsProgressBar'

export default {
  name: 'Specifications',

  components: { ProgressBar },

  computed: {
    ...mapState('carTunningMenu', [
      'specificationMaxValues',
      'specifications',
      'specificationsPossible'
    ])
  },

  methods: {
    specificationItemResult: function(current, future) {
      const result = { type: null, value: null }

      if (future > current) {
        result.type = 'up'
        result.value = Math.floor((future - current) * 10) / 10
      } else if (future < current) {
        result.type = 'down'
        result.value = Math.floor((current - future) * 10) / 10
      } else {
        result.type = null
        result.value = null
      }

      return result
    }
  }
}
</script>

<style lang="scss" scoped>
.second__specifications {
  color: #fff;
  display: flex;
  flex-direction: column;
  position: absolute;
  top: 7.75rem;
  left: 2.8rem;
  .specifications__title {
    font-size: 2.05rem;
    line-height: 2.45rem;
    text-transform: uppercase;
    margin: 0 0 1.25rem 0;
  }
  .specifications__list {
    display: flex;
    flex-direction: column;
    width: 7.4rem;
    .list__item {
      display: flex;
      flex-direction: column;
      &:not(:last-child) {
        margin: 0 0 2.2rem 0;
      }
      .info {
        display: flex;
        justify-content: space-between;
        align-items: center;
        margin: 0 0 0.35rem 0;
        &__current {
          display: flex;
          .icon {
            width: 1.15rem;
            height: 1.15rem;
            margin: 0 0.55rem 0 0;
          }
          .value {
            font-weight: bold;
          }
        }
        &__possible {
          display: flex;
          align-items: center;
          .icon {
            width: 0.65rem;
            height: 0.65rem;
            margin: 0 0.55rem 0 0;
          }
          .value {
            font-weight: bold;
            &.up {
              color: #C1E704;
            }
            &.down {
              color: #E03838;
            }
          }
        }
      }
    }
  }
}
</style>
