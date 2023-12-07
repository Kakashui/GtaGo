<template>
  <div class="residents-page">
    <Head :titleText="loc('home:resid:head')"/>
    <div class="header">
      <div class="header__info">
        <div class="info-item">
          <div class="value">{{occupiers.length}}</div>
          <div class="desc">{{loc('HomeMenu_1')}}</div>
        </div>
        <div class="info-item">
          <div class="value cash">${{rentCost}}</div>
          <div class="desc">{{loc('HomeMenu_2')}}</div>
          <div class="btn btn-edit" @click="setModal('rentCost')"></div>
        </div>
      </div>
      <div class="header__nav">
        <div class="header__nav-item nav-add" @click="setModal('addOccupier')">{{loc('HomeMenu_3')}}</div>
        <div class="header__nav-item nav-evict" @click="deleteAllOccupiers">{{loc('HomeMenu_4')}}</div>
      </div>
    </div>
    <div class="scroll">
      <div class="body">
        <span v-if="occupiers.length === 0">{{loc('HomeMenu_5')}}</span>
        <div class="body__head" v-else>{{loc('HomeMenu_6')}}</div>
        <div
          class="body__item"
          v-for="(item, index) in occupiers"
          :key="item.id"
        >
        <div class="nickname">{{item.name}}</div>
        <div class="btns-wrap">
          <div class="btn btn-edit">
            <div class="edit-block">
              <div class="edit-block__item">
                <div class="title">{{loc('HomeMenu_7')}}</div>
                <div class="checkbox-wrap">
                  <input @click="setGarage({item: !item.garageAccess, uuid: item.uuid})" v-model="item.garageAccess" :id="'garage-' + index" type="checkbox">
                  <label :for="'garage-' + index"></label>
                </div>
              </div>
              <div class="edit-block__item">
                <div class="title">{{loc('HomeMenu_8')}}</div>
                <div class="checkbox-wrap">
                  <input @click="setSafe({item: !item.safeAccess, uuid: item.uuid})" v-model="item.safeAccess" :id="'safe-' + index" type="checkbox">
                  <label :for="'safe-' + index"></label>
                </div>
              </div>
            </div>
          </div>
          <div class="btn btn-delete" @click="deleteOccupier(item.uuid)"></div>
        </div>
        </div>
      </div>
    </div>
    <Modal 
      v-if="dataModal.show"
      :type="dataModal.type"
      :closeModal="closeModal"
    />
  </div>
</template>

<script>
import { mapGetters, mapMutations, mapState } from 'vuex'
import Head from '../components/Head'
import Modal from '../components/Modal'

export default {
  name: 'ResidentsPage',

  components: {
    Head,
    Modal
  },

  data: function() {
    return {
      dataModal: {
        show: false,
        type: null
      }
    }
  },

  computed: {
    ...mapState('homeMenu', ['occupiers', 'rentCost', 'houseId']),
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    ...mapMutations('homeMenu', ['setSafe', 'setGarage', 'deleteOccupier', 'deleteAllOccupiers']),
    buyGarage: function(key) {
      window.mp.trigger('homeMenu:buyGarage', this.houseId, key)
    },
    setModal: function(type) {
      this.dataModal.type = type
      this.dataModal.show = true
    },
    closeModal: function() {
      this.dataModal.show = false
      this.dataModal.type = null
    },
  }
}
</script>

<style lang="scss" scoped>
.residents-page{
  width: 100%;
  padding: 2.85rem 2.85rem 0 2.85rem;
  display: flex;
  flex-flow: column;
  height: 100%;
  position: relative;
  &:after{
    content: '';
    width: 100%;
    height: 5.85rem;
    position: absolute;
    left: 0;
    bottom: 0;
    background: linear-gradient(180deg, rgba(24, 29, 37, 0) 0%, #181D25 100%);
    z-index: 4;
  }
  .header{
    display: flex;
    align-items: flex-start;
    margin: 2rem 0;
    position: relative;
    &__info{
      display: flex;
      .info-item{
        position: relative;
        padding: 0 2rem;
        display: flex;
        flex-flow: column;
        position: relative;
        &:first-child{
          padding-left: 0;
        }
        &:last-child{
          &:after{
            content: none;
          }
        }
        &:after{
          content: '';
          background: radial-gradient(50% 50% at 50% 50%, rgba(255, 255, 255, 0.45) 0%, rgba(255, 255, 255, 0) 100%);
          height: 100%;
          width: 1px;
          position: absolute;
          right: 0;
          top: 0;
        }
        .value{
          font-size: 3.6rem;
          line-height: 4.3rem;
          &.cash{
            color: #C1E704;
          }
        }
        .desc{
          font-size: 1.2rem;
          line-height: 1.45rem;
          color: rgba(255, 255, 255, 0.45);
          margin-top: .3rem;
        }
        .btn-edit{
          position: absolute;
          right: 0;
          top: .4rem;
          transform: translateX(100%);
          width: 2.35rem;
          height: 2.35rem;
          border-radius: 50%;
          background-size: contain;
          background-position: center;
          background-repeat: no-repeat;
          margin-right: .5rem;
          opacity: .65;
          background-color: rgba(255, 255, 255, 0.35);
          background-image: url('/img/homeMenu/btn-edit.svg');
          &:hover{
            opacity: 1;
            transition: .2s;
          }
        }
      }
    }
    &__nav{
      &-item{
        margin-right: .9rem;
        &:last-child{
          margin-right: 0;
        }
        &.nav-add{
          &:before{
            background-image: url('/img/homeMenu/icon-add.svg');
          }
        }
        &.nav-evict{
          &:before{
            background-image: url('/img/homeMenu/icon-evict.svg');
          }
        }
      }
    }
  }
  .scroll{
    width: 100%;
    display: flex;
    flex-flow: column;
    overflow-y: auto;
    height: 100%;
    padding-bottom: 5.85rem;
    z-index: 4;
    &::-webkit-scrollbar{
      width: .3rem;
      background: transparent;
    }
    &::-webkit-scrollbar-thumb{
      background: #A9ABAE;
      border-radius: 1rem;
    }
  }
  .body{
    display: flex;
    flex-flow: column;
    width: 100%;
    padding-right: 4rem;
    font-family: Roboto;
    font-weight: bold;
    font-size: .9rem;
    &__head, &__item{
      display: flex;
      align-items: center;
      // border: 1px solid rgba(255, 255, 255, 0.35);
      min-height: 3.95rem;
      padding: 0 1.1rem 0 2.1rem;
    }
    &__head{
      width: 100%;
      justify-content: flex-start;
      background: rgba(255, 255, 255, 0.15);
      border-top: 1px solid rgba(255, 255, 255, 0.35);
      border-left: 1px solid rgba(255, 255, 255, 0.35);
      border-right: 1px solid rgba(255, 255, 255, 0.35);
      border-radius: 1rem 1rem 0 0;
    }
    &__item{
      display: flex;
      justify-content: space-between;
      background: rgba(255, 255, 255, 0.05);
      position: relative;
      transition: .2s;
      &>div{
        z-index: 3;
      }
      .nickname{
        font-size: 1.05rem;
        line-height: 1.25rem;
        white-space: pre;
      }
      .btns-wrap{
        display: flex;
        align-items: center;
        justify-content: center;
        opacity: 0;
        .btn{
          width: 2.35rem;
          height: 2.35rem;
          border-radius: 50%;
          background-size: contain;
          background-position: center;
          background-repeat: no-repeat;
          margin-right: .5rem;
          opacity: .65;
          &.btn-edit{
            background-color: rgba(255, 255, 255, 0.35);
            background-image: url('/img/homeMenu/btn-edit.svg');
            overflow: hidden;
            &:hover{
              overflow: initial;
                .edit-block{
                  opacity: 1;
                }
              }
            .edit-block{
              display: flex;
              flex-flow: column;
              align-items: center;
              justify-content: space-between;
              background: #47484F;
              box-shadow: 0 .6rem 1.1rem rgba(0, 0, 0, 0.35);
              border-radius: .5rem;
              opacity: 0;
              transform: translateX(-50%) translateY(50%);
              z-index: 5;
              min-width: 11rem;
              &__item{
                width: 100%;
                justify-content: space-between;
                display: flex;
                align-items: center;
                padding: 1.5rem;
                position: relative;
                .title{
                  text-transform: uppercase;
                  font-weight: bold;
                  font-size: .9rem;
                  line-height: .9rem;
                  letter-spacing: 0.05em;
                  color: #FFFFFF;
                }
                .checkbox-wrap{
                  display: flex;
                  align-items: center;
                  justify-content: center;
                  position: relative;
                  width: 1.8rem;
                  height: 1rem;
                  input[type=checkbox] + label {
                    display: flex;
                    align-items: center;
                  }
                  input[type=checkbox] {
                    opacity: 0;
                    position: absolute;
                  }
                  input[type=checkbox] + label:before {
                    position: absolute;
                    left: 0;
                    background: #242424;
                    width: 100%;
                    height: .6rem;
                    content: '';
                    border-radius: 1rem;
                    transition: .2s;
                  }
                  input[type=checkbox] + label:after {
                    position: absolute;
                    left: 0;
                    content: '';
                    border-radius: 50%;
                    width: 1rem;
                    height: 1rem;
                    background: #C4C4C4;
                    transition: .2s;
                  }
                  input[type=checkbox]:checked + label:after {
                    left: .8rem;
                    background: #C1E704;
                  }
                }
                &:last-child{
                  &:after{
                    content: none;
                  }
                }
                &:after{
                  content: '';
                  background: radial-gradient(50% 50% at 50% 50%, rgba(255, 255, 255, 0.25) 0%, rgba(255, 255, 255, 0) 100%);
                  height: 1px;
                  width: 100%;
                  position: absolute;
                  bottom: 0;
                  left: 0;
                }
              }
            }
          }
          &.btn-delete{
            background-color: rgba(255, 96, 96, 0.65);
            background-image: url('/img/homeMenu/icon-delete.svg');
          }
          &:last-child{
            margin-right: 0;
          }
          &:hover{
            opacity: 1;
            transition: .2s;
          }
        }
      }
      &:before, &:after{
        content: '';
        width: 100%;
        height: 100%;
        position: absolute;
        border: 1px solid;
        border-image-slice: 1;
        top: 0;
        left: 0;
        z-index: 1;
        box-sizing: border-box;
        transition: .2s;
      }
      &:before{
        border-image-source: linear-gradient(to bottom, rgba(255,255,255, .15), rgba(255,255,255,0));
        opacity: 1;
      }
      &:after{ 
        background: radial-gradient(75.92% 75.92% at 50% 12.89%, rgba(255, 255, 255, 0.25) 0%, rgba(255, 255, 255, 0) 100%), rgba(255, 255, 255, 0.05);
        border-image-source: linear-gradient(to bottom, rgba(255,255,255, .65), rgba(255,255,255,0));
        opacity: 0;
      }
      &:hover{
        .btns-wrap{
          opacity: 1;
        }
        &:before, &:after{
          transition: .2s;
        }
        &:before{
          opacity: 0;
        }
        &:after{
          opacity: 1;
        }
      }
      &:last-child{
        .btns-wrap{
          .btn{
            &.btn-edit{
              .edit-block{
                transform: translateX(-50%) translateY(-50%);
              }
            }
          }
        }
      }
      &:nth-last-child(2){
        .btns-wrap{
          .btn{
            &.btn-edit{
              .edit-block{
                transform: translateX(-50%) translateY(0%);
              }
            }
          }
        }
      }
    }
  }
}
</style>
