<template>
  <div class="statistics-tab">
    <div class="statistics-tab__left">
      <div class="general">
        <div class="general__level">
          <div class="value">{{ statistics.level }}</div>
          <div class="desc">Level</div>
        </div>
        <div class="general__main">
          <div class="username">{{ statistics.username }}</div>
          <div class="progress">
            <progress-bar
              :curVal="statistics.exp"
              :maxVal="maxExp"
              :styles="{ margin: '0 0 0.3rem 0' }"
            />
            <div class="progress__bottom">
              <div class="exp">EXP: {{ statistics.exp }}/{{ maxExp }}</div>
              <div class="lvl">{{ statistics.level + 1 }} lvl</div>
            </div>
          </div>
        </div>
      </div>
      <div class="info">
        <info-item
          v-for="(item, index) in infoItems"
          :key="index"
          :item="item"
        />
      </div>
      <div class="extra">
        <extra-item
          v-for="(item, index) in extraItems"
          :key="index"
          :item="item"
        />
      </div>
      <div class="additional">
        <div class="additional__registration">
          <div class="desc">{{loc('mmain:stats:date')}}</div>
          <div class="value">{{ formatTime(statistics.registrationDate) }}</div>
        </div>
        <div class="additional__desc">{{loc('mmain:stats:contract')}}</div>
      </div>
    </div>
    <div class="statistics-tab__center"></div>
    <div class="statistics-tab__right">
      <div class="title">{{loc('mmain:stats:props')}}</div>
      <div class="desc">
        <div class="desc__text">
          <div class="title">{{loc('mmain:stats:control')}}</div>
          <div class="text">{{loc('mmain:stats:desc')}}</div>
        </div>
      </div>
      <div class="action">
        <action-item
          v-for="(item, index) in actionItems"
          :key="index"
          :item="item"
        />
      </div>
    </div>
    <transition name="layer">
      <div class="statistics-tab__layer" v-if="isTransportModal || isBusinessModal"></div>
    </transition>
    <transition-group tag="div" name="modal">
      <transport-modal :key="1" v-if="isTransportModal" @close="isTransportModal = false" />
      <business-modal :key="2" v-if="isBusinessModal" @close="isBusinessModal = false"/>
    </transition-group>
  </div>
</template>

<script>
import { mapState, mapGetters } from 'vuex'
import ProgressBar from './common/ProgressBar'
import InfoItem from './common/InfoItem'
import ExtraItem from './common/ExtraItem'
import ActionItem from './common/ActionItem'
import TransportModal from './TransportModal'
import BusinessModal from './BusinessModal'

export default {
  name: 'StatisticsTab',

  components: {
    ProgressBar,
    InfoItem,
    ExtraItem,
    ActionItem,
    TransportModal,
    BusinessModal
  },

  data: function() {
    return {
      isTransportModal: false,
      isBusinessModal: false,
      infoItems: [
        { title: 'mmain:stats:info:number', icon: 'smartphone', key: "smart" },
        { title: 'mmain:stats:info:passp', icon: 'passport', key: 'passportNumber' },
        { title: 'mmain:stats:info:lic', icon: 'license', key: 'licenses' },
        { title: 'mmain:stats:info:bacc', icon: 'museum', key: 'bankCount' },
        { title: 'mmain:stats:info:org', icon: 'batallia', key: 'organization' },
        { title: 'mmain:stats:info:rang', icon: 'rank', key: 'rank' },
        { title: 'mmain:stats:info:work', icon: 'suitcase', key: 'work' },
        { title1: 'mmain:stats:info:fams1', title2: 'mmain:stats:info:fams2', title3: 'mmain:stats:info:fams3', icon: 'hands', key: 'maritalStatus' }
      ],
      extraItems: [
        { title: 'premium', icon: 'premium', icon1: 'premium1', key: 'premium', text: '' },
        { title: 'mmain:stats:warns', icon: 'alert', keyFirst: 'warns', keySecond: 'bans', textFirst: 'mmain:stats:warn', textSecond: 'mmain:stats:ban' }
      ],
      actionItems: [
        { title: 'mmain:stats:house', bg: 'home', key: 'house', event: () => { 
          window.mp.triggerServer("house:OwnerInteracted");
            window.mp.trigger("cef:mmenu:close");
            } },
        { title: 'mmain:stats:vehs', bg: 'transport', key: 'transport', event: () => { this.isTransportModal = true } },
        { title: 'mmain:stats:biz', bg: 'bank', key: 'business', event: () => {
            this.isBusinessModal = true;
            window.mp.triggerServer("mmenu:products:get", this.property.business.number);
          }
        }
      ]
    }
  },

  computed: {
    ...mapState('optionsMenu', ['statistics', 'property']),    
    ...mapGetters('localization', ['loc']),
    maxExp(){
      return this.statistics.level * 3 + 3;
    }
  },

  methods: {
    formatTime: function(time) {
      const date = new Date(time)
      return date.toLocaleDateString('ru-RU')
    }
  }
}
</script>

<style lang="scss" scoped>
.statistics-tab {
  display: flex;
  padding: 3.55rem 0 0 0;
  &__left {
    display: flex;
    flex-direction: column;
    padding: 0 1rem 0 0;
    position: relative;
    &::after {
      content: "";
      position: absolute;
      right: 0;
      top: 0;
      width: 0.1rem;
      height: 100%;
      border-radius: 0.15rem;
      background: rgba(255, 255, 255, 0.1);
    }
    .general {
      display: flex;
      &__level {
        position: relative;
        margin: 0 2.8rem 0 0;
        .value {
          display: flex;
          align-items: flex-start;
          font-size: 6.5rem;
          line-height: 5rem;
          font-weight: 300;
          background: linear-gradient(180deg, #FFFFFF 0%, rgba(255, 255, 255, 0) 100%);
          background-clip: text;
          -webkit-text-fill-color: transparent;
        }
        .desc {
          font-size: 1.3rem;
          letter-spacing: 0.4em;
          color: #fff;
          position: absolute;
          text-transform: uppercase;
          writing-mode: vertical-rl;
          text-orientation: mixed;
          position: absolute;
          top: 2.3rem;
          transform: translate(0, -50%) rotate(180deg);
          left: 100%;
        }
      }
      &__main {
        display: flex;
        flex-direction: column;
        margin: 0 0 2.5rem 0;
        .username {
          font-size: 2.5rem;
          line-height: 2.5rem;
          letter-spacing: 0.03em;
          color: #FFFFFF;
          font-weight: 300;
          margin: 0 0 0.1rem 0;
        }
        .progress {
          display: flex;
          flex-direction: column;
          width: 15rem;
          &__bottom {
            display: flex;
            justify-content: space-between;
            color: rgba(255, 255, 255, 0.4);
            font-size: 0.9rem;
            line-height: 1.1rem;
            letter-spacing: 0.1em;
            font-weight: 300;
          }
        }
      }
    }
    .info {
      display: grid;
      grid-template-columns: repeat(2, 13.5rem);
      grid-auto-rows: 3.8rem;
      margin: 0 0 1.9rem 0;
    }
    .extra {
      display: flex;
      flex-direction: column;
      padding: 3rem 0 0 0;
      position: relative;
      margin: 0 0 2rem 0;
      &::before {
        content: "";
        position: absolute;
        top: 0;
        left: 0;
        height: 0.15rem;
        width: 25.4rem;
        background: center / contain no-repeat url("/img/optionsMenu/statisticsTab/separator.png");
      }
    }
    .additional {
      display: flex;
      font-size: 0.9rem;
      line-height: 1.1rem;
      letter-spacing: 0.03em;
      color: rgba(255, 255, 255, 0.2);
      &__registration {
        margin: 0 1.5rem 0 0;
        display: flex;
        .desc {
          display: flex;
          margin: 0 0.5rem 0 0;
        }
        .value {
          color: #fff;
        }
      }
    }
  }
  &__center {
    margin: 0 9.5rem 0 1.7rem;
    width: 27rem;
    height: 39.8rem;
    background: center / cover no-repeat url("/img/optionsMenu/statisticsTab/soonBg.png");
  }
  &__right {
    display: flex;
    flex-direction: column;
    align-items: flex-end;
    .title {
      font-size: 5rem;
      text-align: right;
      color: #FFFFFF;
    }
    .desc {
      display: flex;
      margin: 0 0 1.5rem 0;
      &::after {
        content: "";
        min-width: 4.5rem;
        max-width: 4.5rem;
        height: 4.5rem;
        background: center / cover no-repeat url("/img/optionsMenu/statisticsTab/icons/buildings.png");
      }
      &__text {
        display: flex;
        flex-direction: column;
        text-align: right;
        margin: 0 1.25rem 0 0;
        .title {
          font-size: 2rem;
          letter-spacing: 0.03em;
          font-weight: 300;
          margin: 0 0 0.25rem 0;
        }
        .text {
          font-size: 0.9rem;
          line-height: 1.2rem;
          letter-spacing: 0.03em;
          color: rgba(255, 255, 255, 0.5);
          width: 12.5rem;
        }
      }
    }
    .action {
      padding: 2.5rem 0 0 0;
      display: flex;
      flex-direction: column;
      position: relative;
      &::before {
        content: "";
        width: 100%;
        height: 0.15rem;
        background: center / contain no-repeat url("/img/optionsMenu/statisticsTab/separator.png");
        transform: rotate(180deg);
        transform-origin: top center;
        position: absolute;
        top: 0;
        left: 0;
      }
    }
  }
  &__layer {
    position: absolute;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background: rgba(17, 20, 27, 0.8);
  }
}
// VUE TRANSITIONS
.modal-enter-active, .modal-leave-active {
  transition: all 0.3s;
}
.modal-enter, .modal-leave-to {
  opacity: 0;
  transform: scale(0.5);
}
.layer-enter-active, .layer-leave-active {
  transition: opacity .5s
}
.layer-enter, .layer-leave-to {
  opacity: 0
}
</style>
