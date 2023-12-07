<template>
  <div class="modal-wrap">
    <div class="modal">
      <div 
        class="btn btn-close"
        @click="closeModal"
      ></div>
      <div class="col">
        <div class="title">{{loc('familyMenu_43')}}</div>
        <div class="desc">{{loc('familyMenu_44')}}</div>

        <template v-if="!isEdit && isLeader">
          <div 
            class="btn btn-inter" 
            @click="editCharter">{{loc('familyMenu_45')}}</div>
        </template>

        <template v-if="isEdit">
          <div 
            class="btn btn-inter"
            @click="saveEditFamilyRules"
          >{{loc('familyMenu_35')}}</div>
          <div 
            class="btn btn-inter cancel"
            @click="cancelEdit"
          >{{loc('familyMenu_36')}}</div>
        </template>

      </div>
      <div class="col">

        <template v-if="!isEdit">
          <div class="block-info">
            <div class="prompt">{{loc('familyMenu_46')}}:</div>
            <div class="block-info__list">
              <div 
                class="item"
                v-for="(item, index) in infoPage.tabooList"
                :key="index"
              ><span>{{index + 1}}</span>. {{item.text}}</div>
            </div>
          </div>
          <div class="block-info">
            <div class="prompt">{{loc('familyMenu_47')}}:</div>
            <div class="block-info__list">
              <div 
                class="item"
                v-for="(item, index) in infoPage.rulesList"
                :key="index"
              ><span>{{index + 1}}</span>. {{item.text}}</div>
            </div>
          </div>
        </template>

        <template v-else>
          <div class="block-info">
            <div class="prompt">{{loc('familyMenu_46')}}:</div>
            <div class="block-info__list edit">
              <div 
                class="item-wrap"
                v-for="(item, index) in tabooEdit"
                :key="index"
              >
                <span>{{index + 1}}</span> 
                <input 
                  type="text"
                  v-model="item.text"
                  :placeholder="loc('familyMenu_37')"
                >
                <div 
                  class="btn btn-delete"
                  @click="deleteElement(tabooEdit, index)"
                ></div>
              </div>
              <div 
                class="item-wrap item-add"
                @click="addToArray(tabooEdit)"
              >
                <span>{{tabooEdit.length + 1}}</span>
                {{loc('familyMenu_48')}}
              </div>
            </div>
          </div>
          <div class="block-info">
            <div class="prompt">{{loc('familyMenu_47')}}:</div>
            <div class="block-info__list edit">
              <div 
                class="item-wrap"
                v-for="(item, index) in rulesEdit"
                :key="index"
              >
                <span>{{index + 1}}</span> 
                <input 
                  type="text"
                  v-model="item.text"
                  :placeholder="loc('familyMenu_37')"
                >
                <div 
                  class="btn btn-delete"
                  @click="deleteElement(rulesEdit, index)"
                ></div>
              </div>
              <div 
                class="item-wrap item-add"
                @click="addToArray(rulesEdit)"
              >
                <span>{{rulesEdit.length + 1}}</span>
                {{loc('familyMenu_48')}}
              </div>
            </div>
          </div>
        </template>
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
export default {
  name: 'ModalRules',

  data: function() {
    return {
      currentTextBio: null,
      isEdit: false,
      tabooEdit: [],
      rulesEdit: []
    }
  },

  computed: {
    ...mapState('familyMenu', ['isLeader', 'infoPage']),
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    closeModal: function() {
      this.$emit('closeRulesModal')
    },
    setBio: function() {
      this.closeModal()
      window.mp.trigger('familyMenu:setBio', this.currentTextBio)
    },
    editCharter: function() {
      this.isEdit = true
      this.tabooEdit = []
      this.rulesEdit = []
      this.copyArray(this.tabooEdit, this.infoPage.tabooList)
      this.copyArray(this.rulesEdit, this.infoPage.rulesList)
    },
    copyArray: function(to, from) {
      from.forEach(element => {
        to.push({...element})
      });
    },
    addToArray: function(array) {
      array.push({text: ''})
    },
    deleteElement: function(array, index) {
      array.splice(index, 1)
    },
    cancelEdit: function() {
      this.isEdit = false
      this.tabooEdit = []
      this.rulesEdit = []
    },
    saveEditFamilyRules: function() {
      window.mp.trigger('familyMenu:saveEditFamilyRules', JSON.stringify(this.tabooEdit), JSON.stringify(this.rulesEdit))
      this.isEdit = false
      this.tabooEdit = []
      this.rulesEdit = []
    }
  },
}
</script>

<style lang="scss" scoped>
.modal{
  display: flex;
  justify-content: space-between;
 text-transform: uppercase;
  color: #FFFFFF;
  padding: 0 8rem;
  background: #0A0C11;
  width: 68.4rem;
  height: 42.5rem;
  position: relative;
  .btn-close{
    background-size: contain;
    background-position: center;
    background-repeat: no-repeat;
    background-color: transparent;
    width: 2rem;
    height: 2rem;
    background-image: url('/img/familyMenu/btn-close.svg');
    position: absolute;
    right: -.5rem;
    top: 0;
    transform: translateX(100%);
    &:hover{
      box-shadow: none;
      transform: translateX(100%) scale(1.1);
    }
  }
  .col{
    padding: 3.5rem 0;
    display: flex;
    width: 100%;
    flex-flow: column;
    align-items: flex-start;
    overflow-y: auto;
    &::-webkit-scrollbar{
      background: transparent;
      width: 0;
    }
    .title{
     text-transform: uppercase;
      font-size: 5rem;
      line-height: 6rem;
      margin-bottom: .6rem;
    }
    .desc{
      font-size: 1rem;
      line-height: 1.2rem;
      color: rgba(255, 255, 255, 0.5);
      margin-bottom: 1.5rem;
      padding-bottom: .8rem;
      border-bottom: 1px solid rgba(255, 255, 255, 0.2);
      width: 15.45rem;
    }
    .btn-inter{
      width: 15.45rem;
      margin-bottom: .9rem;
      border: 1px solid #DCFF00;
      height: 3rem;
      font-size: 1rem;
      color: #DCFF00;
      &:hover{
        filter: drop-shadow(0 0 .2rem #DCFF00);
        text-shadow: 0 0 .2rem #DCFF00;
      }
      &.cancel{
        border: 1px solid rgba(255, 255, 255, 0.4);
        color: rgba(255, 255, 255, 0.6);
        &:hover{
          filter: drop-shadow(0 0 .2rem #FFFFFF);
          text-shadow: 0 0 .2rem #FFFFFF;
        }
      }
    }
    .text-info{
      resize: none;
      border: 1px solid rgba(255, 255, 255, 0.4);
      padding: 1.3rem 1.6rem;
      width: 100%;
      height: 25rem;
      font-family: "Roboto";
      font-size: .9rem;
      background: transparent;
      outline: none;
      color: #fff;
      &::placeholder{
        color: rgba(255, 255, 255, 0.4);
       text-transform: uppercase;
      }
    }
    .block-info{
      display: flex;
      flex-flow: column;
      align-items: flex-start;
      width: 100%;
      margin-bottom: 2.2rem;
      &:last-child{
        margin-bottom: 0;
      }
      .prompt{
        margin-bottom: 1rem;
        font-size: 2rem;
        line-height: 2rem;

      }
      &__list{
        width: 100%;
        display: flex;
        flex-flow: column;
        align-items: flex-start;
        text-align: left;
        &.edit{
          padding-left: 1.5rem;
        }
        .item, .item-wrap, .item-wrap input{
          font-size: 1.2rem;
          line-height: 1.45rem;
        }
        .item{
          margin-bottom: .9rem;
          &:last-child{
            margin-bottom: 0;
          }
        }
        .item-wrap{
          height: 3.5rem;
          min-height: 3.5rem;
          width: 100%;
          border: 1px solid rgba(255, 255, 255, 0.4);
          display: flex;
          align-items: center;
          justify-content: flex-start;
          position: relative;
          margin-bottom: .6rem;
          padding-right: 2.2rem;
          &:last-child{
            margin-bottom: 0;
          }
          span{
            position: absolute;
            left: -1rem;
            transform: translateX(-100%);
          }
          input{
            outline: none;
            width: 100%;
            height: 100%;
            padding-left: 1.6rem;
            text-align: left;
            outline: none;
            border: none;
            background: transparent;
            color: #fff;
           text-transform: uppercase;
            &::placeholder{
              color: rgba(255, 255, 255, 0.5);
            }
          }
          &.item-add{
            border: 1px dashed rgba(255, 255, 255, 0.4);
            color: #DCFF00;
            padding-left: 1.75rem;
            span{
              color: rgba(255, 255, 255, 0.5);
            }
            &:before{
              content: '';
              width: 1.2rem;
              height: 1.2rem;
              background-size: contain;
              background-repeat: no-repeat;
              background-position: center;
              background-image: url('/img/familyMenu/icon-add.svg');
              margin-right: .3rem;
            }
          }
          .btn-delete{
            background-image: url('/img/familyMenu/input-delete.svg');
            width: 2.2rem;
            height: 100%;
            position: absolute;
            right: 0;
            top: 0;
            opacity: 0;
            transition: .2s;
          }
          &:hover{
            .btn-delete{
              opacity: 1;
              transition: .2s;
            }
          }
        }
      }
    }
  }
}
</style>
