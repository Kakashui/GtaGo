<template>
  <div class="name-tab">
    <div class="option-block">
      <div class="title">{{loc('familyMenu_89')}}</div>
      <div :class="[{'is-edited': isEdited}, {error: organizationName.error}, 'input-wrap']">
        <input 
          type="text" 
          :placeholder="loc('familyMenu_91')"
          v-model="currentOrgName"
        >
        <div 
          v-if="isEdited && organizationName.error"
          class="error-text"
        >{{loc('familyMenu_90')}}</div>
      </div>
      <div 
        class="btns-wrap"
        v-if="currentOrgName !== organizationName.value"
      >
        <div 
          class="btn btn-save"
          @click="saveOrganizationName"
        >{{loc('familyMenu_83')}}</div>
        <div 
          class="btn"
          @click="refreshData"
        >{{loc('familyMenu_84')}}</div>
      </div>
    </div>
    <div 
      v-if="isLeader"
      class="delete-block"
    >
      <div class="title">{{loc('familyMenu_92')}}</div>
      <div class="desc">{{loc('familyMenu_93')}}</div>
      <div class="btns-wrap">
        <div 
          class="btn btn-save"
          @click="setShowModal"
        >{{loc('familyMenu_74')}}</div>
      </div>
    </div>
    <modal-delete
      v-if="showModal"
      @closeModal="setShowModal"
    />
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'

import ModalDelete from './ModalDelete'

export default {
  name: 'NameTab',

  components: {
    ModalDelete
  },

  data: function() {
    return {
      currentOrgName: null,
      isEdited: null,
      showModal: false,
    }
  },

  computed: {
    ...mapState('familyMenu', ['isLeader']),
    ...mapState('familyMenu/controlPage', ['organizationName']),
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    refreshData: function() {
      this.currentOrgName = this.organizationName.value
      this.isEdited = false
    },
    saveOrganizationName: function() {
      window.mp.trigger('familyMenu:saveOrganizationName', this.currentOrgName)
      this.isEdited = true
    },
    setShowModal: function() {
      this.showModal = !this.showModal
    }
  },

  mounted(){
    this.refreshData()
  }
}
</script>

<style lang="scss" scoped>
.name-tab{
  display: flex;
  flex-flow: column;
  align-items: flex-start;
  .title{
    font-size: 2rem;
    line-height: 2.4rem;
    margin-bottom: .8rem;
  }
  .desc{
    font-size: 1rem;
    line-height: 1rem;
    color: rgba(255, 255, 255, 0.6);
    margin-bottom: 1.2rem;
    max-width: 27.3rem;
  }
  .input-wrap{
    position: relative;
    width: 25rem;
    height: 3.3rem;
    min-height: 3.3rem;
    display: flex;
    align-items: center;
    justify-content: center;
    border: 1px solid rgba(255, 255, 255, 0.1);
    border-radius: .1rem;
    margin-bottom: 2.8rem;
    &:after{
      content: '';
      width: 1.25rem;
      height: 1.25rem;
      position: absolute;
      right: 1.7rem;
      background-size: contain;
      background-repeat: no-repeat;
      background-position: center;
    }
    &.is-edited{
      &:after{
        background-image: url('/img/familyMenu/icon-is-edited.svg');
      }
      input{
        border-color: #DCFF00;
      }
      &.error{
        &:after{
          background-image: url('/img/familyMenu/icon-error.svg');
        }
        input{
          border-color: #FF3838;
        }
      }
    }
    .error-text{
      position: absolute;
      left: 0;
      bottom: -.3rem;
      transform: translateY(100%);
      font-weight: bold;
      font-size: 1.2rem;
      line-height: 1.2rem;
      text-transform: uppercase;
      color: #FF3838;
    }
    input{
      height: 100%;
      width: 100%;
      border: none;
      border-bottom: .1rem solid #fff;
      padding: 0 1.6rem;
     text-transform: uppercase;
      font-style: normal;
      font-weight: bold;
      font-size: 1.5rem;
      text-transform: uppercase;
      color: #FFFFFF;
      background: transparent;
      box-sizing: border-box;
      &::placeholder{
        color: rgba(255, 255, 255, 0.6);
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
  .option-block, .delete-block{
    display: flex;
    flex-flow: column;
    align-items: flex-start;
    min-width: 30rem;
    min-height: 15rem;
    position: relative;
  }
  .delete-block{
    padding-top: 3.5rem;
    &:before{
      content: '';
      width: 100%;
      height: .1rem;
      background: linear-gradient(90deg, rgba(255, 255, 255, 0) 0%, rgba(255, 255, 255, 0.1) 53.12%, rgba(255, 255, 255, 0) 100%);
      position: absolute;
      left: 0;
      top: 0;
    }
  }
}
</style>
