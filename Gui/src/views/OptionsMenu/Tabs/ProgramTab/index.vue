<template>
  <div class="program-tab">
    <div class="program-tab__control">
      <div class="program-tab__nav">
        <div 
          :class="[{active: item.tab === currentTab}, 'program-tab__nav-item']"
          v-for="(item, index) in navList"
          :key="index"
          @click="setCurrentTab(item.tab)"
        >
          <svg 
            v-html="item.icon"
            viewBox="0 0 40 40"
          ></svg>
          <span>{{loc(item.desc)}}</span>
        </div>
      </div>
      <div class="program-tab__control-info">
        <div class="program-tab__control-info-desc">{{loc('options_program_0')}}</div>
        <div class="program-tab__control-info-value">{{countOfFinishContracts}}</div>
      </div>
      <!-- <div class="program-tab__control-info">
        <div class="program-tab__control-info-desc">{{loc('options_program_1')}}</div>
        <div class="program-tab__control-info-value program-tab__control-info-value--small">${{moneyEarned.toLocaleString()}}</div>
      </div> -->
    </div>
    <component :is="`${currentTab}Tab`"/>
  </div>
</template>

<script>
import ContractsTab from './tabs/ContractsTab'
import QuestsTab from './tabs/QuestsTab'
import AchievementsTab from './tabs/AchievementsTab'
import { mapGetters, mapState } from 'vuex'
export default {
  name: 'ProgramTab',
  components: {
    ContractsTab,
    QuestsTab,
    AchievementsTab
  },
  data: function() {
    return {
      currentTab: 'Contracts',
      navList: [
        {
          icon: `<g xmlns="http://www.w3.org/2000/svg">
<path d="M22.069 33.3533V30.6034H0V33.3644C0 35.8648 2.02047 37.931 4.52115 37.931H24.2932C24.1036 37.7586 23.9207 37.6074 23.748 37.4343C22.668 36.351 22.0639 34.8828 22.069 33.3533Z" fill="white"/>
<path d="M15.6255 16.6831L18.828 18.8817L22.0429 16.6831C22.9221 16.0833 23.6206 14.7299 23.6206 13.6658V9.12245L18.8361 6.75613L14.0516 9.11639V13.6658C14.0516 14.7296 14.746 16.0833 15.6255 16.6831ZM16.3465 12.3889C16.5832 12.1545 16.9655 12.1562 17.1998 12.3933L18.1616 13.3644L20.4764 11.05C20.7121 10.8143 21.0943 10.8143 21.33 11.0503C21.5657 11.286 21.5654 11.6682 21.3297 11.904L18.5862 14.6488C18.4731 14.7623 18.3198 14.8269 18.1596 14.8276H18.1582C17.9972 14.8266 17.8433 14.7616 17.7309 14.6464L16.3421 13.2435C16.1078 13.0061 16.1098 12.6239 16.3465 12.3889Z" fill="white"/>
<path d="M36.8009 2.06898H36.7817C35.0088 2.06898 33.5341 3.55033 33.5341 5.32431V10.8621H39.9997V5.31353C40.0077 3.53619 38.5783 2.08616 36.8009 2.06898Z" fill="white"/>
<path d="M9.26449 2.06898C7.58783 2.06898 6.22334 3.4153 6.22334 5.0667L6.0452 29.3966H22.6724C23.0108 29.4101 23.2778 29.6892 23.2758 30.0283V33.3533C23.2697 35.8672 25.2963 37.9129 27.8097 37.9311H27.8249C30.3293 37.9129 32.3447 35.8682 32.3275 33.3638V5.32532C32.3275 4.0568 32.8983 2.93105 33.756 2.06898L9.26449 2.06898ZM12.8448 8.73992C12.837 8.51295 12.9613 8.30181 13.1637 8.1981L18.5678 5.54218C18.7338 5.46002 18.9284 5.46002 19.0944 5.54218L24.5083 8.2055C24.7107 8.30922 24.8349 8.52003 24.8275 8.74733V13.6658C24.8275 15.1297 23.9341 16.8555 22.7242 17.6802L19.179 20.1112C19.0796 20.1795 18.9618 20.2162 18.8412 20.2162C18.721 20.2162 18.6038 20.1799 18.5051 20.1112L14.9494 17.6802C13.7402 16.8552 12.8448 15.1293 12.8448 13.6658V8.73992ZM27.4137 26.638H10.862C10.5286 26.638 10.2586 26.3679 10.2586 26.0345C10.2586 25.7011 10.5286 25.4311 10.862 25.4311H27.4137C27.7471 25.4311 28.0172 25.7011 28.0172 26.0345C28.0172 26.3679 27.7471 26.638 27.4137 26.638ZM27.4137 23.4483H10.862C10.5286 23.4483 10.2586 23.1782 10.2586 22.8449C10.2586 22.5115 10.5286 22.2414 10.862 22.2414H27.4137C27.7471 22.2414 28.0172 22.5115 28.0172 22.8449C28.0172 23.1782 27.7471 23.4483 27.4137 23.4483Z" fill="white"/>
</g>`,
          desc: 'options_program_2',
          tab: 'Contracts'
        },
        {
          icon: `<g xmlns="http://www.w3.org/2000/svg">
<path d="M19.9996 16.8453C15.8765 16.8453 12.5222 20.1997 12.5222 24.3227C12.5222 28.4457 15.8765 31.8001 19.9996 31.8001C24.1227 31.8001 27.477 28.4457 27.477 24.3227C27.477 20.1997 24.1226 16.8453 19.9996 16.8453ZM18.7918 28.5312L14.7436 24.5286L16.3915 22.862L18.8539 25.2966L23.6402 20.9143L25.2229 22.6429L18.7918 28.5312Z" fill="white"/>
<path d="M28.5227 3.29324H23.6975L23.433 2.48643C22.9454 0.999229 21.566 0 20.0004 0C18.4348 0 17.0554 0.999229 16.5678 2.48643L16.3033 3.29324H11.4781V7.19413H28.5227V3.29324Z" fill="white"/>
<path d="M30.866 5.19492V9.53785H9.13391V5.19492H4.6873V39.9999H35.3126V5.19492H30.866ZM20 34.1439C14.5845 34.1439 10.1788 29.738 10.1788 24.3227C10.1788 18.9073 14.5845 14.5015 20 14.5015C25.4154 14.5015 29.8211 18.9073 29.8211 24.3227C29.8211 29.738 25.4153 34.1439 20 34.1439Z" fill="white"/>
</g>`,
          desc: 'options_program_3',
          tab: 'Quests'
        },
        {
          icon: `<g xmlns="http://www.w3.org/2000/svg">
<path d="M5.90854 36.4706H34.1438V40H5.90854V36.4706Z" fill="white"/>
<path d="M9.43817 28.2353H30.6147V34.1176H9.43817V28.2353Z" fill="white"/>
<path d="M15.9765 22.6247C15.9501 22.6121 15.9245 22.5984 15.8983 22.5856L15.0074 25.8824H25.0468L24.1643 22.58C21.5297 23.8669 18.4544 23.81 15.9765 22.6247Z" fill="white"/>
<path d="M37.5174 3.34L37.3563 2.35294H30.5394C30.4433 0.908 30.2908 0 30.2908 0H9.81382C9.81382 0 9.66135 0.908 9.56527 2.35294H2.64371L2.48269 3.34C2.46355 3.45772 2.02316 6.25992 2.95743 9.40635C3.8332 12.3558 6.10104 16.1581 11.9287 17.5734C11.9502 17.5786 12.8977 17.7857 13.5594 17.9305C14.493 18.9569 15.6319 19.8389 17.0184 20.502C17.9366 20.9412 18.984 21.1765 20.0524 21.1765C21.1207 21.1765 22.1682 20.9412 23.0865 20.502C24.4855 19.8329 25.6323 18.9405 26.5708 17.9021C27.2196 17.7601 28.0513 17.5783 28.0715 17.5734C33.8991 16.1581 36.167 12.3558 37.0428 9.40635C37.9769 6.25992 37.5365 3.4578 37.5174 3.34ZM5.23571 8.812C4.74363 7.19796 4.69108 5.66172 4.72535 4.70588H9.48888C9.49382 7.72227 9.90245 11.686 11.5942 15.0391C8.31971 14.0084 6.18245 11.9177 5.23571 8.812ZM20.0262 14.2647C17.6747 14.2647 15.7615 12.3515 15.7615 9.99999C15.7615 7.64847 17.6747 5.73529 20.0262 5.73529C22.3777 5.73529 24.2908 7.64847 24.2908 9.99999C24.2908 12.3515 22.3777 14.2647 20.0262 14.2647ZM34.7643 8.812C33.8297 11.8782 31.7345 13.9551 28.5305 14.9992C30.2054 11.6547 30.6108 7.71035 30.6158 4.70588H35.2747C35.309 5.66172 35.2564 7.19796 34.7643 8.812Z" fill="white"/>
<path d="M20.0262 8.08823C18.972 8.08823 18.1144 8.94587 18.1144 9.99999C18.1144 11.0541 18.972 11.9118 20.0262 11.9118C21.0802 11.9118 21.9379 11.0541 21.9379 9.99999C21.9379 8.94587 21.0802 8.08823 20.0262 8.08823Z" fill="white"/>
</g>`,
          desc: 'options_program_4',
          tab: 'Achievements'
        }
      ]
    }
  },
  computed: {
    ...mapState('optionsMenu', ['myContracts', 'moneyEarned']),
    ...mapGetters('localization', ['loc']),
    countOfFinishContracts() {
      return this.myContracts.reduce((sum, current) => sum + current.CountCompleted, 0);
    }
    
  },
  methods: {
    setCurrentTab: function (value) {
      this.currentTab = value
    } 
  },
  mounted() {
    this.currentTab = this.navList[0].tab
  }
}
</script>

<style lang="scss" scoped>
.program-tab{
  display: flex;
  color: #fff;
  height: 100%;
  &:before, &:after{
    content: '';
    width: 20rem;
    height: 20rem;
    background: rgba(41, 255, 242, 0.6);
    filter: blur(9rem);
    border-radius: 50%;
    position: absolute;
    z-index: -1;
    pointer-events: none;
  }
  &:before{
    left: 0;
    top: 25rem;
    transform: translateX(-45%);
    animation: showBlurLeft .3s;
  }
  &:after{
    bottom: 0;
    right: 6.5rem;
    transform: translateY(45%) translateX(0%);
    animation: showBlurBottom .3s;
  }
  &__control{
    width: 7rem;
    min-width: 7rem;
    padding-right: 1rem;
    border-right: 1px solid  rgba(255, 255, 255, 0.2);
    padding-bottom: 8rem;
    margin: 2rem 1rem 0 0;
    &-info{
      display: flex;
      flex-direction: column;
      margin-bottom: 1.5rem;
      &:last-child{
        margin-bottom: 0;
      }
      &-desc{
        font-size: 1rem;
        line-height: 1.2rem;
        letter-spacing: 0.03em;
        color: rgba(255, 255, 255, 0.4);
        margin-bottom: .2rem;
      }
      &-value{
        font-weight: bold;
        font-size: 2.5rem;
        line-height: 2.5rem;
        letter-spacing: 0.03em;
        white-space: pre;
        &--small{
          font-size: 1.5rem;
          line-height: 1.5rem;
        }
      }
    }
  }
  &__nav{
    display: flex;
    flex-direction: column;
    padding-bottom: 1.5rem;
    margin-bottom: 1.5rem;
    border-bottom: 1px solid rgba(255, 255, 255, 0.1);
    z-index: 1;
    &-item{
      width: 100%;
      height: 6rem;
      background-color: rgba(255, 255, 255, 0.1);
      display: flex;
      flex-direction: column;
      align-items: center;
      justify-content: center;
      margin-bottom: .2rem;
      font-weight: bold;
      font-size: .9rem;
      text-transform: uppercase;
      border-radius: 1px;
      transition: .3s;
      &.active{
        background-color: #B6D300;
      }
      &:hover{
        background-color: #B6D300;
        box-shadow: 0 .5rem 4rem rgba(168, 195, 2, 0.6);
      }
      svg{
        width: 2rem;
        height: 2rem;
        margin-bottom: .5rem;
      }
      &:last-child{
        margin-bottom: 0;
      }
    }
  }
}
@keyframes showBlurLeft {
  from {
    transform: translateX(-150%);
  }
  to {
    transform: translateX(-45%);
  }
}
@keyframes showBlurBottom {
  from {
    transform: translateY(200%) translateX(-200%);
  }
  to {
    transform: translateY(45%) translateX(0%);
  }
}
</style>
