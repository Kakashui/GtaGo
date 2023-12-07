<template>
  <div class="enterprice-item">
    <div 
      class="enterprice-item__img"
      :style="{backgroundImage: `url(/img/warForEnterprice/enterprices/${companiesConfig[item.key].Image}.png)`}"
    ></div>
    <div 
      class="enterprice-item__date"
      v-if="item.date"
    > 
      <svg 
        xmlns="http://www.w3.org/2000/svg"
        viewBox="0 0 20 20" 
        fill="none"
      >
        <path d="M10 0C4.48578 0 0 4.48578 0 10C0 15.5142 4.48578 20 10 20C15.5142 20 20 15.5142 20 10C20 4.48578 15.5142 0 10 0ZM14.7559 15.1724C14.5934 15.3349 14.38 15.4167 14.1667 15.4167C13.9534 15.4167 13.7399 15.3349 13.5776 15.1724L9.41086 11.0059C9.25415 10.8501 9.16672 10.6383 9.16672 10.4167V5C9.16672 4.53918 9.53995 4.16672 10 4.16672C10.4601 4.16672 10.8333 4.53918 10.8333 5V10.0717L14.7559 13.9941C15.0816 14.3201 15.0816 14.8466 14.7559 15.1724Z"/>
      </svg>
      <span>{{item.date}}</span>
    </div>
    <div class="enterprice-item__info">
      <div class="enterprice-item__name">{{companiesConfig[item.key].Name}}</div>
      <div 
        class="enterprice-item__owner enterprice-item__owner--capture"
        v-if="item.captureisInProgress"
      >{{loc('war_for_enterprice_2')}}</div>
      <div 
        :class="[{'enterprice-item__owner--free' : !item.orgId}, 'enterprice-item__owner']"
        v-else
      >{{item.orgId > 0 ? currentOrgName : loc('war_for_enterprice_3')}}</div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
import warcompanies from '../../../configs/families/warcompanies';
export default {
  name: 'EnterpriceItem',
  props: {
    item: Object
  },
  computed: {
    ...mapState('familyMenu/ratingPage', ['orgList']),
    ...mapState('warForEnterprice', ['fractionNames']),
    ...mapGetters('localization', ['loc']),
    companiesConfig: function(){
      return warcompanies;
    },
    currentOrgName() {
      if (this.item.orgId > 0)
        return this.item.orgName;
        return 'Unknown'
    }
  }
}
</script>

<style lang="scss" scoped>
.enterprice-item{
  display: flex;
  flex-direction: column;
  align-items: flex-start;
  justify-content: flex-end;
  position: relative;
  background-image: url('/img/warForEnterprice/border.png');
  background-size: contain;
  background-repeat: no-repeat;
  background-position: center;
  padding: 1.1rem 1.7rem;
  color: #FFFFFF;
  &__img, &:before{
    width: calc(100% - (.5rem));
    height: calc(100% - (.5rem));
    position: absolute;
    left: .25rem;
    top: .25rem;
    border-radius: .5rem;
    overflow: hidden;
  }
  &__img{
    background-size: contain;
    background-repeat: no-repeat;
    background-position: center;
    z-index: 0;
  }
  &:before{
    content: '';
    background: linear-gradient(180deg, rgba(37, 46, 68, 0) 0%, rgba(37, 46, 68, 0.9) 82.5%);
    z-index: 1;
  }
  &__date, &__info{
    z-index: 2;
  }
  &__date{
    padding: .5rem .6rem;
    background: #FFFFFF;
    border-radius: .25rem;
    display: flex;
    align-items: flex-end;
    font-size: .8rem;
    line-height: .95rem;
    letter-spacing: 0.03em;
    color: #000;
    position: absolute;
    left: 1.25rem;
    top: 1.25rem;
    svg{
      fill: #000;
      width: 1rem;
      height: 1rem;
      margin-right: .4rem;
    }
  }
  &__info{
    display: flex;
    flex-flow: column;
  }
  &__name{
    font-size: 1.8rem;
    line-height: 2.15rem;
    margin-bottom: .1rem;
  }
  &__owner{
    font-size: 1.1rem;
    line-height: 1.3rem;
    &--free{
      color: #29FFF2;
    }
    &--capture{
      color: #EE443A;
      text-shadow: .5rem 0 2rem #EE443A;
    }
  }
}
</style>
