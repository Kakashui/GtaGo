<template>
  <div class="map-tab">
    <div class="title">{{loc('familyMenu_77')}}</div>
    <div class="radio-wrap">
      <div class="radio-wrap__item">
        <input 
          id="mapMemberTrue" 
          name="mapMember" 
          type="radio"
          :value="true"
          v-model="mapMember"
        >
        <label for="mapMemberTrue">{{loc('familyMenu_87')}}</label>
      </div>
      <div class="radio-wrap__item">
        <input 
          id="mapMemberFalse" 
          name="mapMember" 
          type="radio"
          :value="false"
          v-model="mapMember"
        >
        <label for="mapMemberFalse">{{loc('familyMenu_88')}}</label>
      </div>
    </div>
    <div 
      v-if="mapMember !== membersOnMap"
      class="btns-wrap"
    >
      <div 
        class="btn btn-save"
        @click="saveMapOptions"
      >{{loc('familyMenu_83')}}</div>
      <div 
        class="btn"
        @click="loadOptions"
      >{{loc('familyMenu_84')}}</div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
export default {
  name: 'MapTab',

  data: function() {
    return {
      mapMember: null,
    }
  },

  computed: {
    ...mapState('familyMenu', ['membersOnMap']),
    ...mapGetters('localization', ['loc'])

  },

  methods: {
    saveMapOptions: function() {
      window.mp.trigger('familyMenu:saveMapOptions', this.mapMember)
    },
    loadOptions: function() {
      this.mapMember = this.membersOnMap
    }
  },

  mounted(){
    this.loadOptions()
  }
}
</script>

<style lang="scss" scoped>
.map-tab{
  display: flex;
  flex-flow: column;
  align-items: flex-start;
  .title{
    font-weight: bold;
    font-size: 3.5rem;
    line-height: 3.5rem;
    max-width: 32rem;
    margin-bottom: 1.5rem;
  }
  .radio-wrap{
    display: flex;
    flex-flow: column;
    margin-bottom: 2rem;
    &__item{
      margin-bottom: 1rem;
      position: relative;
      display: flex;
      align-items: center;
      &:last-child{
        margin-bottom: 0;
      }
      input{
        opacity: 0;
        width: 1.2rem;
        height: 1.2rem;
      }
      label{
        font-size: 1rem;
        line-height: 1.15rem;
        margin-left: .5rem;
        font-family: "Arial";
      }
      input + label:before, input + label:after{
        content: '';
        position: absolute;
        border-radius: 50%;
        top: 50%;
        transform: translateY(-50%);
        box-sizing: border-box;
      }
      input + label:before{
        width: 1.2rem;
        height: 1.2rem;
        left: 0;
        border: .1rem solid rgba(220, 255, 0, 0.4);
      }
      input + label:after{
        background: #DCFF00;
        left: .3rem;
        width: .6rem;
        height: .6rem;
        opacity: 0;
        transition: .3s;
      }
      input:checked + label:after{
        opacity: 1;
        transition: .3s;
      }
    }
  }
  .btns-wrap{
    display: flex;
    align-items: center;
    .btn{
      font-weight: bold;
      font-size: 1.2rem;
      color: rgba(255, 255, 255, 0.7);
      padding: 1rem 1.2rem;
      margin-right: 1.2rem;
      &:last-child{
        margin-right: 0;
      }
      &.btn-save{
        color: #fff;
        background: #C1E704;
        border-radius: .1rem;
        &:hover{
          box-shadow: 0 0 1rem #DCFF00;
        }
      }
    }
  }
}
</style>
