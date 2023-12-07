<template>
  <div class="modal-wrap">
    <div 
      v-if="currentRank"
      :class="[{'empty': currentRank.rankId === 0 || currentRank.rankId === 1}, 'modal']"
    >
      <div
        class="btn btn-close"
        @click="closeModal"
      ></div>
      <template v-if="currentRank !== null && isLeader">
        <div class="title">{{loc('familyMenu_97')}}</div>
        <input
          class="input"
          type="text"
          :placeholder="loc('fam:menu:plh:name')"
          v-model="currentRank.rankName"
        />
        <div 
          class="main-options"
          v-if="currentRank.rankId !== 0 && currentRank.rankId !== 1"
        >
          <div class="block-cols">
            <div class="col">
              <div class="col__block">
                <div class="info">
                  <div class="desc">{{loc('familyMenu_98')}}</div>
                  <div
                    class="value-wrap"
                    @click="setChooseAccessHouse"
                  >
                    <div class="value">{{loc(accessHouseList.list[currentRank.accessHouse])}}</div>
                    <div
                      v-if="accessHouseList.choose"
                      class="value-wrap__list"
                    >
                      <div class="value">{{loc(accessHouseList.list[currentRank.accessHouse])}}</div>
                      <div class="list-wrap">
                        <div
                          v-for="(item, index) in accessHouseList.list"
                          :key="item.id"
                          :class="[{active: index === currentRank.accessHouse}, 'item']"
                          @click="setAccessHouse(index)"
                        >{{loc(item)}}</div>
                      </div>
                    </div>
                  </div>
                </div>
                <div class="info">
                  <div class="desc">{{loc('familyMenu_99')}}</div>
                  <div 
                    class="value-wrap"
                    @click="setChooseAccessFurniture"
                  >
                    <div class="value">{{loc(accessFurnitureList.list[currentRank.accessFurniture])}}</div>
                    <div
                      v-if="accessFurnitureList.choose" 
                      class="value-wrap__list"
                    >
                      <div class="value">{{loc(accessFurnitureList.list[currentRank.accessFurniture])}}</div>
                      <div class="list-wrap">
                        <div 
                          v-for="(item, index) in accessFurnitureList.list"
                          :key="item.id"
                          :class="[{active: index === currentRank.accessFurniture}, 'item']"
                          @click="setAccessFurniture(index)"
                        >{{loc(item)}}</div>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>
            <div class="col">
              <div class="col__block">
                <div class="info-check">
                  <div class="checkbox-wrap">
                    <input
                      type="checkbox" 
                      id="clothes"
                      v-model="currentRank.accessClothes"
                    >
                    <label for="clothes">{{loc('familyMenu_100')}}</label>
                  </div>
                </div>
                <div class="info-check">
                  <div class="checkbox-wrap">
                    <input
                      type="checkbox" 
                      id="war"
                      v-model="currentRank.accessWar"
                    >
                    <label for="war">{{loc('familyMenu_101')}}</label>
                  </div>
                </div>
                <div class="info-check">
                  <div class="checkbox-wrap">
                    <input
                      type="checkbox" 
                      id="members"
                      v-model="currentRank.accessMembers"
                    >
                    <label for="members">{{loc('familyMenu_110')}}</label>
                  </div>
                </div>
              </div>
            </div>
          </div>
          <div class="block-row">
            <div class="desc">{{loc('familyMenu_102')}}:</div>
            <div 
              class="info"
              v-for="(item, index) in currentRank.accessCars"
              :key="index"
            >
              <div class="desc">{{item.carName}}</div>
              <div 
                class="value-wrap"
                @click="setChooseAccessCars(index)"
              >
                <div class="value">{{loc(accessCarsList.list[item.access])}}</div>
                <div
                  v-if="accessCarsList.choose === index"
                  class="value-wrap__list"
                >
                  <div class="value">{{loc(accessCarsList.list[item.access])}}</div>
                  <div class="list-wrap">
                    <div 
                      v-for="(option, number) in accessCarsList.list"
                      :key="option.key"
                      :class="[{active: number === currentRank.accessCars[index].access}, 'item']"
                      @click="setAccessCar(number, index)"
                    >{{loc(option)}}</div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
        <div class="btns-wrap">
          <div 
            class="btn btn-inter"
            @click="setRank"
          >{{loc('familyMenu_35')}}</div>
          <div 
            class="btn btn-inter cancel"
            @click="copyArray"
          >{{loc('familyMenu_36')}}</div>
        </div>
      </template>
      <template v-else-if="currentRank !== null && !isLeader">
        <div class="title">{{loc('familyMenu_103')}}</div>
        <div class="rank-name">{{currentRank.rankName}}</div>
        <div 
          class="main-options"
          v-if="currentRank.rankId !== 0 && currentRank.rankId !== 1"
        >
          <div class="block-cols">
            <div class="col">
              <div class="col__block">
                <div class="info">
                  <div class="desc">{{loc('familyMenu_98')}}</div>
                  <div 
                    class="value-wrap static"
                  >
                    <div class="value">{{loc(accessHouseList.list[currentRank.accessHouse])}}</div>
                  </div>
                </div>
                <div class="info">
                  <div class="desc">{{loc('familyMenu_99')}}</div>
                  <div
                    class="value-wrap static"
                  >
                    <div class="value">{{loc(accessFurnitureList.list[currentRank.accessFurniture])}}</div>
                  </div>
                </div>
              </div>
            </div>
            <div class="col">
              <div class="col__block">
                <div class="info-check">
                  <div :class="[{active: currentRank.accessClothes}, 'checkbox-wrap']">
                    <div class="icon"></div>
                    <div class="text-value">{{loc('familyMenu_100')}}</div>
                  </div>
                </div>
                <div class="info-check">
                  <div :class="[{active: currentRank.accessWar}, 'checkbox-wrap']">
                    <div class="icon"></div>
                    <div class="text-value">{{loc('familyMenu_101')}}</div>
                  </div>
                </div>
                <div class="info-check">
                  <div :class="[{active: currentRank.accessMembers}, 'checkbox-wrap']">
                    <div class="icon"></div>
                    <div class="text-value">{{loc('familyMenu_110')}}</div>
                  </div>
                </div>
              </div>
            </div>
          </div>
          <div class="block-row">
            <div class="desc">{{loc('familyMenu_102')}}:</div>
            <div 
              class="info"
              v-for="(item, index) in currentRank.accessCars"
              :key="index"
            >
              <div class="desc">{{item.carName}}</div>
              <div 
                class="value-wrap static"
              >
                <div class="value">{{loc(accessCarsList.list[item.access])}}</div>
              </div>
            </div>
          </div>
        </div>
      </template>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapMutations, mapState } from 'vuex'
export default {
  name: 'ModalControl',

  props: {
    rank: Object
  },

  data: function() {
    return {
      currentRank: null,
    }
  },

  computed: {
    ...mapState('familyMenu', ['isLeader']),
    ...mapState('familyMenu/controlPage', [
      'accessHouseList',
      'accessFurnitureList',
      'accessCarsList'
    ]),
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    ...mapMutations('familyMenu/controlPage', [
      'setChooseAccessHouse',
      'setChooseAccessFurniture',
      'setChooseAccessCars'
    ]),
    copyArray: function() {
      this.currentRank = {...this.rank}
      this.currentRank.accessCars = []
      this.currentRank.accessStore = []
      this.rank.accessCars.forEach(element => {
        this.currentRank.accessCars.push({...element})
      });
    },
    closeModal: function() {
      this.$emit('closeModal');
    },

    setAccessHouse: function(value) {
      this.currentRank.accessHouse = value
    },
    setAccessFurniture: function(value) {
      this.currentRank.accessFurniture = value
    },
    setAccessCar: function(value, index) {
      this.currentRank.accessCars[index].access = value
    },

    setRank: function() {
      window.mp.trigger('familyMenu:setRank', JSON.stringify(this.currentRank))
      this.closeModal();
    }
  },

  mounted(){
    this.copyArray()
  }
}
</script>

<style lang="scss" scoped>
@keyframes showModal {
  from {
    opacity: 0;
    transform: translateY(100%);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}
.modal{
  display: flex;
  flex-flow: column;
  align-items: center;
  justify-content: space-between;
  width: 37.1rem;
  border-radius: .5rem;
  background: #0A0C11;
  box-shadow: 0 0 10rem rgba(128, 183, 200, 0.3);
  padding: 2rem 0;
  position: relative;
  animation: showModal .2s linear;
  &:before{
    content: '';
    width: 100%;
    height: 12.7rem;
    background: linear-gradient(180.85deg, rgba(10, 12, 17, 0) 4.18%, #0A0C11 24.36%);
    position: absolute;
    bottom: 0;
    left: 0;
    z-index: 3;
  }
  &.empty{
    &:before{
      content: none;
    }
  }
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
  .title{
    font-weight: bold;
    font-size: 1.5rem;
    line-height: 1.5rem;
    margin-bottom: 1.5rem;
  }
  .rank-name, .input{
    width: 15.9rem;
    height: 2.6rem;
    min-height: 2.6rem;
    text-transform: uppercase;
    border: 1px solid rgba(255, 255, 255, 0.2);
    border-radius: .1rem;
    background-color: transparent;
    text-align: center;
    font-weight: bold;
    font-size: 1rem;
    line-height: 1rem;
    margin-bottom: 2.35rem;
    color: #fff;
  }
  .rank-name{
    display: flex;
    align-items: center;
    justify-content: center;
  }
  .input{
    box-sizing: border-box;
    outline: none;
    &::placeholder{
      color: rgba(255, 255, 255, 0.7);
    }
  }
  .main-options{
    display: flex;
    flex-flow: column;
    width: 100%;
    overflow-y: auto;
    max-height: 28rem;
    margin-bottom: 4rem;
    padding: 0 2.5rem 0 2.5rem;
    &::-webkit-scrollbar{
      background: rgba(255, 255, 255, 0.04);
      width: .1rem;
      border-radius: .1rem;
      &-thumb{
        background: rgba(255, 255, 255, 0.4);
        border-radius: .1rem;
      }
    }
    .block-cols{
      display: flex;
      width: 100%;
      margin-bottom: 1rem;
      min-height: 7.1rem;
      .col{
        display: flex;
        flex-flow: column;
        align-items: flex-start;
        position: relative;
        height: fit-content;
        width: 100%;
        padding: 0 1.3rem 0 0;
        &:last-child{
          width: 13.6rem;
          min-width: 13.6rem;
          padding: 0 0 0 1.5rem;
          &:before{
            content: '';
            width: 1px;
            height: 100%;
            position: absolute;
            left: 0;
            top: 0;
            border-left: 1px dashed rgba(255, 255, 255, 0.1);
          }
        }
        &__block{
          width: 100%;
          display: flex;
          flex-flow: column;
          padding: 1.5rem 0;
          .desc{
            font-weight: bold;
            font-size: 1.1rem;
            line-height: 1.1rem;
          }
          .info, .info-check{
            display: flex;
            align-items: flex-start;
            width: 100%;
            margin-bottom: .6rem;
            &:last-child{
              margin-bottom: 0;
            }
          }
          .info{
            justify-content: space-between;
            min-height: 1.15rem;
            &:last-child{
              margin-bottom: 0;
            }
            .desc{
              max-width: 8.35rem;
            }
          }
          .info-check{
            align-items: center;
            .checkbox-wrap{
              display: flex;
              align-items: flex-start;
              justify-content: flex-start;
              position: relative;
              &.active{
                .icon{
                  background-image: url('/img/familyMenu/icon-checkbox.svg');
                  background-size: contain;
                  background-repeat: no-repeat;
                  background-position: center;
                }
              }
              .text-value, label{
                text-transform: uppercase;
                font-style: normal;
                font-weight: bold;
                font-size: 1.1rem;
                line-height: 1.2rem;
                margin-left: .5rem;
                color: #FFFFFF;
              }
              input{
                opacity: 0;
                width: 1.2rem;
                height: 1.2rem;
              }
              .icon{
                width: 1.2rem;
                height: 1.2rem;
                background-color: rgba(255, 255, 255, 0.2);
                border-radius: .2rem;
              }
              input + label{
                &:before, &:after{
                  content: '';
                  width: 1.2rem;
                  height: 1.2rem;
                  position: absolute;
                  left: 0;
                  top: 0;
                }
                &:before{
                  background: rgba(255, 255, 255, 0.2);
                  border-radius: .2rem;
                }
                &:after{
                  background-image: url('/img/familyMenu/icon-checkbox.svg');
                  background-size: contain;
                  background-repeat: no-repeat;
                  background-position: center;
                  opacity: 0;
                  transition: .2s;
                }
              }
              input:checked + label:after{
                opacity: 1;
                transition: .2s;
              }
            }
          }
        }
      }
    }
    .block-row{
      display: flex;
      flex-flow: column;
      align-items: flex-start;
      width: 100%;
      height: 100%;
      position: relative;
      padding-top: 1.3rem;
      padding-bottom: 2rem;
      &:before{
        content: '';
        width: 100%;
        height: 1px;
        position: absolute;
        left: 0;
        top: 0;
        border-top: 1px dashed rgba(255, 255, 255, 0.1);
      }
      .desc{
        font-weight: bold;
        font-size: 1.1rem;
        line-height: 1.1rem;
      }
      &>.desc{
        margin-bottom: 1rem;
      }
      .info, .info-check{
        display: flex;
        align-items: flex-start;
        width: 100%;
        margin-bottom: .6rem;
      }
      .info{
        justify-content: space-between;
        min-height: 1rem;
        &:last-child{
          margin-bottom: 0;
          min-height: 8rem;
          height: 8rem;
        }
        .desc{
          max-width: 8.35rem;
          font-size: 1rem;
          color: rgba(255, 255, 255, 0.6);
        }
      }
      
    }
  }
  .value-wrap{
    font-size: 1rem;
    line-height: 1rem;
    font-weight: bold;
    position: relative;
    width: 9.9rem;
    border-bottom: 1px solid rgba(255, 255, 255, 0.4);
    display: flex;
    align-items: center;
    justify-content: flex-start;
    &.static{
      border-color: transparent;
      &:after{
        content: none;
      }
    }
    &:after{
      content: '';
      width: 1rem;
      height: 1rem;
      position: absolute;
      right: .2rem;
      bottom: .1rem;
      background-position: center;
      background-size: contain;
      background-repeat: no-repeat;
      background-image: url('/img/familyMenu/chevron-down.svg');
      transform-origin: 50% 50%;
    }
    &.collapsed{
      &:after{
        transform: rotate(180deg);
      }
    }
    &__list{
      z-index: 3;
      display: flex;
      flex-flow: column;
      position: absolute;
      top: -.5rem;
      left: -.6rem;
      width: calc(100% + (.6rem) + (.3rem));
      padding: 1.7rem 0 .5rem .6rem;
      background: #111111;
      border: 1px solid #414141;
      &:after{
        content: '';
        width: 1rem;
        height: 1rem;
        position: absolute;
        right: .5rem;
        top: .3rem;
        background-position: center;
        background-size: contain;
        background-repeat: no-repeat;
        background-image: url('/img/familyMenu/chevron-down.svg');
        transform-origin: 50% 50%;
        transform: rotate(180deg);
        z-index: 2;
      }
      .value{
        position: absolute;
        top: .45rem;
        left: .52rem;
      }
      .list-wrap{
        display: flex;
        flex-flow: column;
        padding: .4rem 0 0 .4rem;
        border-left: 1px solid rgba(255, 255, 255, 0.2);
        .item{
          color: rgba(255, 255, 255, 0.4);
          margin-bottom: .6rem;
          position: relative;
          &:last-child{
            margin-bottom: 0;
          }
          &.active{
            color: #DCFF00;
            &:after{
              content: '';
              width: .7rem;
              height: .7rem;
              position: absolute;
              left: -.1rem;
              top: .1rem;
              background-color: #111111;
              background-position: center;
              background-size: contain;
              background-repeat: no-repeat;
              background-image: url('/img/familyMenu/chevron-down.svg');
              transform-origin: 50% 50%;
              transform: translateX(-100%) rotate(-90deg);
            }
          }
        }
      }
    }
  }
  .btns-wrap{
    display: flex;
    align-items: center;
    z-index: 4;
    .btn-inter{
      padding: 1rem 1.6rem;
      font-weight: bold;
      align-items: flex-end;
      font-size: 1.2rem;
      line-height: 1.2rem;
      border-radius: .1rem;
      background: #C1E704;
      margin-right: 1.2rem;
      &:hover{
        box-shadow: 0 0 1rem #C1E704;
      }
      &:last-child{
        margin-right: 0;
      }
      &:before{
        content: '';
        width: 1.5rem;
        height: 1.5rem;
        margin-right: .5rem;
        background-size: contain;
        background-position: center;
        background-repeat: no-repeat;
        background-image: url('/img/familyMenu/btn.svg');
      }
      &.cancel{
        background: rgba(255, 255, 255, 0.2);
        &:before{
          background-image: url('/img/familyMenu/btn-cancel.svg');
        }
        &:hover{
          box-shadow: 0 0 1rem rgba(255, 255, 255, 0.4);
        }
      }
    }
  }
}
</style>
