<template>
  <div class="control-page">
    <div class="control-menu">
      <div 
        class="control-menu__item"
        v-for="item in navList"
        :key="item.id"
      >
        <div class="btn-wrap">
          <div 
            :class="[{active: item.key === currentTab}, {leave: item.key === 'LeaveTab'}, 'btn', 'btn-menu']"
            @click="setCurrentTab(item.key)"
          >
            <div class="text">{{loc(item.text)}}</div>
          </div>
        </div>
      </div>
    </div>
    <component :is="currentTab"/>
  </div>
</template>

<script>
import RanksTab from './RanksTab'
import MapTab from './MapTab'
import VehicleTab from './VehicleTab'
import ChatOptionsTab from './ChatOptionsTab'
import NameTab from './NameTab'
import LeaveTab from './LeaveTab'
import { mapGetters, mapState } from 'vuex'

export default {
  name: 'ControlPage',

  components: {
    RanksTab,
    MapTab,
    VehicleTab,
    ChatOptionsTab,
    NameTab,
    LeaveTab
  },

  data: function() {
    return {
      navList: [
        {
          text: 'familyMenu_76',
          key: 'RanksTab'
        },
        {
          text: 'familyMenu_77',
          key: 'MapTab'
        },
        {
          text: 'familyMenu_78',
          key: 'VehicleTab'
        },
        {
          text: 'familyMenu_79',
          key: 'ChatOptionsTab'
        },
        {
          text: 'fam:menu:leave',
          key: 'LeaveTab'
        },
        {
          text: 'familyMenu_80',
          key: 'NameTab'
        },
      ],
      currentTab: 'RanksTab'
    }
  },

  computed: {
    ...mapState('familyMenu', ['isLeader']),
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    setCurrentTab: function(value) {
      this.currentTab = value
    }
  },

  mounted() {
    if (!this.isLeader) {
      this.navList = this.navList.filter(element => element.key !== 'NameTab')
    }
  }
}
</script>

<style lang="scss" scoped>
.control-page{
  display: flex;
  margin-top: 3.5rem;
  padding-top: .5rem;
  padding-left: 28.2rem;
  position: relative;
  width: 100%;
  .control-menu{
    width: 38.3rem;
    height: 38.3rem;
    background-size: contain;
    background-repeat: no-repeat;
    background-position: center;
    background-image: url('/img/familyMenu/control-menu-bg.svg');
    position: absolute;
    top: 0;
    left: -23.4rem;
    display: flex;
    align-items: center;
    justify-content: center;
    &__item{
      width: 3.75rem;
      height: 50%;
      position: absolute;
      display: flex;
      align-items: center;
      justify-content: center;
      left: 50%;
      top: 0;
      transform-origin: 50% 100%;
      .btn-wrap{
        position: absolute;
        top: 0;
        transform-origin: 50% 50%;
        display: flex;
        align-items: center;
        justify-content: center;
        .btn-menu{
          width: 2.7rem;
          height: 2.7rem;
          border-radius: 50%;
          background-color: #252A2D;
          box-shadow: 0 .2rem 2rem rgba(0, 0, 0, 0.2);
          border: .1rem solid transparent;
          .text{
            opacity: 0;
            font-family: "Roboto";
            font-style: normal;
            font-weight: normal;
            font-size: 1rem;
            line-height: 1.15rem;
            text-transform: uppercase;
            color: rgba(255, 255, 255, 0.5);
            position: absolute;
            top: 50%;
            right: -.4rem;
            transform: translateY(-50%) translateX(100%) scale(.81);
            transition: .2s;
          }
          &:hover, &.active{
            background-color: #0E1014;
            border-color: #DCFF00;
            transform: scale(1.22);
          }
          &:hover{
            .text{
              opacity: 1;
              transition: .2s;
            }
          }
          &.active{
            &:hover{
              .text{
                opacity: 0;
              }
            }
          }
        }
      }
      &:nth-child(1){
        transform: translateX(-50%) rotate(35deg);
        .btn-wrap{
          transform: translateY(-50%) rotate(-35deg);
          .btn-menu{
            background-image: url('/img/familyMenu/control-menu-1.svg');
            .text{
              min-width: 6.5rem;
            }
          }
        }
      }
      &:nth-child(2){
        transform: translateX(-50%) rotate(57.5deg);
        .btn-wrap{
          transform: translateY(-50%) rotate(-57.5deg);
          .btn-menu{
            background-image: url('/img/familyMenu/control-menu-2.svg');
            .text{
              min-width: 7.7rem;
            }
          }
        }
      }
      &:nth-child(3){
        transform: translateX(-50%) rotate(80deg);
        .btn-wrap{
          transform: translateY(-50%) rotate(-80deg);
          .btn-menu{
            background-image: url('/img/familyMenu/control-menu-3.svg');
            .text{
              min-width: 7.4rem;
            }
          }
        }
      }
      &:nth-child(4){
        transform: translateX(-50%) rotate(102.5deg);
        .btn-wrap{
          transform: translateY(-50%) rotate(-100.5deg);
          .btn-menu{
            background-image: url('/img/familyMenu/control-menu-4.svg');
            .text{
              min-width: 6.6rem;
            }
          }
        }
      }
      &:nth-child(5){
        transform: translateX(-50%) rotate(125deg);
        .btn-wrap{
          transform: translateY(-50%) rotate(-125deg);
          .btn-menu{
            background-image: url('/img/familyMenu/control-menu-5.svg');
            &.leave{
              background-image: url('/img/familyMenu/control-menu-leave.svg');
            }
            .text{
              min-width: 8.95rem;
            }
          }
        }
      }
      &:nth-child(6){
        transform: translateX(-50%) rotate(147.5deg);
        .btn-wrap{
          transform: translateY(-50%) rotate(-147.5deg);
          .btn-menu{
            background-image: url('/img/familyMenu/control-menu-5.svg');
            &.leave{
              background-image: url('/img/familyMenu/control-menu-leave.svg');
            }
            .text{
              min-width: 8.95rem;
            }
          }
        }
      }
    }
  }
}
</style>
