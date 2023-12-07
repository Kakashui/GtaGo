<template>
  <div class="item">
    <div class="icon-wrap" v-bind:class="[icon, {'yellow' : value <= 40 && value > 10}, {'red' : value <= 10}]">
      <div class="icon icon-back"></div>
      <div class="icon icon-view" v-bind:style="{height: value + '%'}"></div>
    </div>
    <div class="value">{{value}}</div>
  </div>
</template>

<script>
export default {
  name: 'StatusDisplayItem',

  props: {
    value: Number,
    icon: String
  }
}
</script>

<style lang="scss" scoped>
  .item{
    margin-top: .75rem;
    display: flex;
    align-items: flex-end;
    justify-content: flex-start;
    .icon-wrap{
      position: relative;
      width: 1.2rem;
      height: 1.2rem;
      display: flex;
      align-items: center;
      justify-content: center;
      &.joy{
        .icon{
          &.icon-back, &.icon-view{
            background-image: url('/img/hud/joy.svg');
          }
        }
        &.red{
          .icon{
            &.icon-view{
              background-image: url('/img/hud/joy-red.svg');
            }
          }
        }
        &.yellow{
          .icon{
            &.icon-view{
              background-image: url('/img/hud/joy-yellow.svg');
            }
          }
        }
      }
      &.rest{
        .icon{
          &.icon-back, &.icon-view{
            background-image: url('/img/hud/rest.svg');
          }
        }
        &.red{
          .icon{
            &.icon-view{
              background-image: url('/img/hud/rest-red.svg');
            }
          }
        }
        &.yellow{
          .icon{
            &.icon-view{
              background-image: url('/img/hud/rest-yellow.svg');
            }
          }
        }
      }
      &.thirst{
        .icon{
          &.icon-back, &.icon-view{
            background-image: url('/img/hud/thirst.svg');
          }
        }
        &.red{
          .icon{
            &.icon-view{
              background-image: url('/img/hud/thirst-red.svg');
            }
          }
        }
        &.yellow{
          .icon{
            &.icon-view{
              background-image: url('/img/hud/thirst-yellow.svg');
            }
          }
        }
      }
      &.hunger{
        .icon{
          &.icon-back, &.icon-view{
            background-image: url('/img/hud/hunger.svg');
          }
        }
        &.red{
          .icon{
            &.icon-view{
              background-image: url('/img/hud/hunger-red.svg');
            }
          }
        }
        &.yellow{
          .icon{
            &.icon-view{
              background-image: url('/img/hud/hunger-yellow.svg');
            }
          }
        }
      }
      .icon{
        position: absolute;
        bottom: 0;
        width: 100%;
        background-size: cover;
        background-position: bottom;
        background-repeat: no-repeat;
        &.icon-back{
          height: 100%;
          opacity: .3;
        }
        &.icon-view{
          overflow: hidden;
        }
      }
    }
    .value{
      font-size: 1rem;
      line-height: 1rem;
      letter-spacing: 0.1em;
      text-transform: uppercase;
      color: rgba(255, 255, 255, 0.5);
      margin-left: .4rem;
      font-weight: 300;
      &:after{
        content: '%';
      }
    }
  }
</style>
