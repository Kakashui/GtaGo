<template>
  <div class="reports-list">
    <div class="reports-list__requests">
      <requests-stats/>
      <requests-list
        :reports="reports"
      />
    </div>
    <Chat 
      :dynamic="true"
      @showModal="modalData.show = true, modalData.param = $event"
    />
    <div class="reports-list__response-templates">
      <div class="title">{{loc('ReportMenu_3')}}</div>
      <div 
        class="btn btn-add"
        @click="addTemplate"
      >{{loc('ReportMenu_4')}}</div>
      <div class="templates-list scroll">
        <div
          class="template"
          v-for="(item, index) in answersTemplates"
          :key="item.key"
        >
          <div 
            class="text"
            @click="appendMessagemodel(messageModel, item.text)"
          >{{item.text}}</div>
          <div
            class="btn btn-delete"
            @click="deleteTemplate(index)"
          ></div>
        </div>
      </div>
    </div>
    <block-modal 
      v-if="modalData.show"
      @closeModal="modalData.show = false"
      :param="modalData.param"
    />
  </div>
</template>

<script>
import { mapGetters, mapMutations, mapState } from 'vuex'
import RequestsStats from '../components/RequestsStats'
import RequestsList from '../components/RequestsList'
import Chat from '../components/Chat'
import BlockModal from '../components/BlockModal'
export default {
  name: "ReportsList",

  components: {
    RequestsStats,
    RequestsList,
    Chat,
    BlockModal
  },

  data: function() {
    return {
      currentMessage: null,
      modalData: {
        show: false,
        param: null
      }
    }
  },

  computed: {
    ...mapState('reportMenu', ['answersTemplates', 'reports', 'messageModel']),
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    ...mapMutations('reportMenu', ['deleteAnswerTemplate', 'setMessageModel']),
    addTemplate: function() {
      this.modalData.show = true
      this.modalData.param = 'template'
    },
    deleteTemplate: function(index) {
      this.deleteAnswerTemplate(index);
      window.mp.trigger('reportMenu:deleteAnswerTemplate', index)
    },
    appendMessagemodel: function(model, text) {
      if (model.length > 0) {
        this.setMessageModel(model + ' ' + text)
      } else {
        this.setMessageModel(model + text)
      }
    }
  }
}
</script>

<style lang="scss" scoped>
  .reports-list{
    width: 100%;
    height: 100%;
    display: flex;
    &__requests{
      display: flex;
      flex-flow: column;
      width: 21rem;
      min-width: 21rem;
      height: 100%;
    }
    &__response-templates{
      display: flex;
      flex-flow: column;
      align-items: flex-start;
      padding-top: 1.8rem;
      .title{
        font-weight: bold;
        font-size: 1.55rem;
        padding-left:  1.8rem;
      }
      .btn-add{
        font-size: .9rem;
        color: #C1E704;
        margin: .95rem 0 1.5rem;
        padding-left: 1.25rem;
        margin-left: 1.8rem;
        display: flex;
        align-items: center;
        position: relative;
        &:before{
          content:'';
          width: .7rem;
          height: .7rem;
          background-image: url('/img/reportMenu/btn-add.svg');
          background-size: contain;
          background-repeat: no-repeat;
          background-position: center;
          position: absolute;
          left: 0;
          transition: .2s;
        }
        &:hover{
          &:before{
            transform: scale(1.3);
            transition: .2s;
          }
        }
      }
      .templates-list{
        display: flex;
        flex-flow: column;
        align-items: flex-start;
        height: 100%;
        overflow-y: auto;
        padding: 0 1rem 1rem 1.15rem;
        margin-right: .6rem;
        .template{
          margin-bottom: 1.7rem;
          position: relative;
          font-family: Roboto;
          padding-left: .65rem;
          &:last-child{
            margin-bottom: 0;
          }
          .text{
            font-weight: bold;
            font-size: .9rem;
            line-height: 119.5%;
            color: rgba(255, 255, 255, 0.45);
          }
          .btn-delete{
            display: none;
            position: absolute;
            left: 0;
            top: 0;
            transform: translateX(-100%);
            background-image: url('/img/reportMenu/btn-close-header.svg');
            width: .65rem;
            height: .65rem;
          }
          &:hover{
            .text{
              color: rgba(255, 255, 255, 1);
            }
            .btn-delete{
              display: flex;
            }
          }
        }
      }
    }
  }
</style>
