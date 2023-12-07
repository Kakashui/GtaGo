<template>
  <div class="tab">
    <div class="tab__side">
      <div class="title">{{loc('auth:chcreate:tit')}}</div>
      <div class="desc">{{loc('auth:chcreate:desc')}}</div>
    </div>
    <div class="tab__main">
      <div class="labels">
        <input-label
          v-for="(item, index) in labels"
          :key="index"
          :item="item"
          v-model="item.model"
        />
      </div>
      <action-button
        :text="'auth:chcreate:action'"
        :styles="{ width: '25rem' }"
        @event="create"
      />
    </div>
  </div>
</template>

<script>
import ActionButton from '../common/ActionButton'
import InputLabel from '../common/InputLabel'
import { mapGetters } from 'vuex'

export default {
  components: {
    InputLabel,
    ActionButton
  },
  name: 'CharacterCreate',

  data: function() {
    return {
      labels: [
        { name: 'auth:chcreate:lable:1', model: '', key: 'name', type: 'text' },
        { name: 'auth:chcreate:lable:2', model: '', key: 'surename', type: 'text' }
      ],
      flood: 0,
      clickInterval: 5000,
    }
  },

  methods: {
    getModels: function() {
      return this.labels.map(item => item.model)
    },

    clearModels: function() {
      this.labels.forEach(item => item.model = '')
    },

    create: function() {
      if(this.flood > Date.now())return;
      this.flood = Date.now() + this.clickInterval;
      const firstName = this.labels[0].model;
      const lastName = this.labels[1].model;
      if(firstName.length < 3 
          || lastName.length < 3
          || firstName.length > 12
          || lastName.length > 18) {
          this.$store.commit('notifyList/notify', {type: 1, position: 0, message: this.loc('chcr_18'), time: 3000});
          return;
      }
      var regexp = /^[a-z]+$/i;
      if (!regexp.test(firstName) || !regexp.test(lastName)){
        this.$store.commit('notifyList/notify', {type: 1, position: 0, message: this.loc('chcr_19'), time: 3000});
        return;
      }
      window.mp.triggerServer("newchar", firstName, lastName);
      //this.clearModels();
    },
    enter(e){
      if(e.keyCode == 13)
        this.create();
    }
  },
  mounted(){
    document.addEventListener("keyup", this.enter)
  },
  beforeDestroy(){
    document.removeEventListener("keyup", this.enter)
  },
  computed:{
    ...mapGetters('localization', ['loc']),
  }
}
</script>

<style lang="scss" scoped>
.tab {
  display: flex;
  &__side {
    display: flex;
    flex-direction: column;
    color: #FFFFFF;
    margin: 0 7.3rem 0 0;
    .title {
      font-size: 7.5rem;
      line-height: 7rem;
      font-weight: bold;
      text-transform: uppercase;
      position: relative;
      display: block;
      width: 28.5rem;
      &::after {
        content: "";
        width: 0.2rem;
        height: 5.5rem;
        background: linear-gradient(268.14deg, #B0C724 0%, #D5EC45 23.77%, #B6D300 100%);
        top: 0.4rem;
        right: 1.85rem;
        position: absolute;
      }
    }
    .desc {
      font-size: 1.2rem;
      text-transform: uppercase;
      color: #FFFFFF;
      width: 25rem;
    }
  }
  &__main {
    display: flex;
    flex-direction: column;
    .labels {
      margin: 0 0 1.5rem 0;
    }
  }
}
</style>
