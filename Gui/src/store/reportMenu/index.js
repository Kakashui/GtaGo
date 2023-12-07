export default {
  namespaced: true,
  state: {
    mySettings: {
      name: 'Milt Devovich'
    },
    answersTemplates: [
      {
        text: 'Был рад помочь!'
      }, // при нажатии на ответ он должен в инпут поле "Ваше сообщение" вставляться
      {
        text: 'Хороший вопрос, дайте мне, пожалуйста, минуту, уточню информацию у (разработчиков, инженеров, коллег)'
      },
      {
        text: 'Данная ошибка зафиксирована, приношу наши извинения. Нам есть куда расти, спасибо за Ваше обращение!'
      },
      {
        text: 'Ситуация неприятная и я понимаю каково Вам, но к счастью я не позволю этому произойти'
      },
      {
        text: 'Приятно было помочь, если будут вопросы обязательно задавайте, я помогу!'
      },
      {
        text: 'Подскажите, пожалуйста, какую версию платформы и конфигурации используете?'
      },
      {
        text: 'Появляется ли какая-либо ошибка? Опишите пошаговые действия приводящие к проблеме'
      },
      {
        text: 'Подскажите, пожалуйста, какую версию платформы используете?'
      },
      {
        text: 'Приятно было помочь.'
      },
      {
        text: 'Всего доброго!'
      }
    ],

    messageModel: '',

    reportsInQueue: 23,
    reportsAnswered: 235,
    reports: [
      {
        id: 5467,
        time: 1600182247800,
        reporterName: 'Sharmeek Cherlynnes',
        reporterId: 428,
        // 0 - обычный
        // 1 - vip
        // 2 - media
        // 3 - хз в душе не ебу, пусть будет лидер?
        reporterStatus: 1,
        unreadMessages: 0,
        messages: [
          // если sender === reporterName, тогда сообщение слева, иначе справа
          // если sender === mySettings.Name, тогда пишет "Ваш ответ:", иначе "Ответ {{ sender }}:"
          {
            id: 0,
            sender: 'Sharmeek Cherlynnes',
            text: 'Хороший вопрос, дайте мне, пожалуйста, минуту, уточню информацию...'
          },
          {
            id: 1,
            sender: 'Vasya Devovich',
            text: 'Используйте клавишу "N" на вашей клавиатуре'
          }
        ]
      },
      {
        id: 5468,
        time: 1600182247800,
        reporterName: 'Sharmeek Cherlynnes',
        reporterId: 429,
        reporterStatus: 2,
        unreadMessages: 1,
        messages: [
          {
            id: 0,
            sender: 'Sharmeek Cherlynnes',
            text: 'Хороший вопрос, дайте мне, пожалуйста, минуту, уточню информацию...'
          },
          {
            id: 1,
            sender: 'Vasya Devovich',
            text: 'Используйте клавишу "N" на вашей клавиатуре'
          }
        ]
      },
      {
        id: 5469,
        time: 1600182247800,
        reporterName: 'Sharmeek Cherlynnes',
        reporterId: 428,
        reporterStatus: 3,
        unreadMessages: 99,
        messages: [
          {
            id: 0,
            sender: 'Sharmeek Cherlynnes',
            text: 'Хороший вопрос, дайте мне, пожалуйста, минуту, уточню информацию...'
          },
          {
            id: 1,
            sender: 'Milt Devovich',
            text: 'Используйте клавишу "N" на вашей клавиатуре'
          },
          {
            id: 0,
            sender: 'Sharmeek Cherlynnes',
            text: 'Хороший вопрос'
          },
          {
            id: 0,
            sender: 'Sharmeek Cherlynnes',
            text: 'Хороший вопрос, дайте мне, пожалуйста, минуту, уточню информацию...'
          },
          {
            id: 1,
            sender: 'Vasya Devovich',
            text: 'Используйте клавишу "N" на вашей клавиатуре'
          },
          {
            id: 0,
            sender: 'Alexey Nenavalny',
            text: 'Хороший вопрос, дайте мне, пожалуйста, минуту, уточню информацию...'
          },
        ]
      },
      {
        id: 5470,
        time: 1600182247800,
        reporterName: 'Sharmeek Cherlynnes',
        reporterId: 428,
        reporterStatus: 0,
        unreadMessages: 0,
        messages: [
          {
            id: 0,
            sender: 'Sharmeek Cherlynnes',
            text: 'Хороший вопрос, дайте мне, пожалуйста, минуту, уточню информацию...'
          },
          {
            id: 1,
            sender: 'Milt Devovich',
            text: 'Используйте клавишу "N" на вашей клавиатуре'
          }
        ]
      },
      {
        id: 5471,
        time: 1600182247800,
        reporterName: 'Sharmeek Cherlynnes',
        reporterId: 428,
        reporterStatus: 0,
        unreadMessages: 0,
        messages: [
          {
            id: 0,
            sender: 'Sharmeek Cherlynnes',
            text: 'Хороший вопрос, дайте мне, пожалуйста, минуту, уточню информацию...'
          },
          {
            id: 1,
            sender: 'Vasya Devovich',
            text: 'Используйте клавишу "N" на вашей клавиатуре'
          },
          {
            id: 0,
            sender: 'Alexey Nenavalny',
            text: 'Хороший вопрос, дайте мне, пожалуйста, минуту, уточню информацию...'
          },
          {
            id: 0,
            sender: 'Sharmeek Cherlynnes',
            text: 'Хороший вопрос, дайте мне, пожалуйста, минуту, уточню информацию...'
          }
        ]
      },
      {
        id: 5472,
        time: 1600182247800,
        reporterName: 'Sharmeek Cherlynnes',
        reporterId: 428,
        reporterStatus: 0,
        unreadMessages: 0,
        messages: [
          {
            id: 0,
            sender: 'Sharmeek Cherlynnes',
            text: 'Хороший вопрос, дайте мне, пожалуйста, минуту, уточню информацию...'
          },
          {
            id: 1,
            sender: 'Milt Devovich',
            text: 'Используйте клавишу "N" на вашей клавиатуре'
          }
        ]
      },
    ],

    myReports: [
      {
        id: 5468,
        time: 1600182247800,
        reporterName: 'Sharmeek Cherlynnes',
        reporterId: 429,
        reporterStatus: 2,
        rating: 4,
        messages: [
          {
            id: 0,
            sender: 'Sharmeek Cherlynnes',
            text: 'Хороший вопрос, дайте мне, пожалуйста, минуту, уточню информацию...'
          },
          {
            id: 1,
            sender: 'Milt Devovich',
            text: 'Используйте клавишу "N" на вашей клавиатуре'
          },
          {
            id: 1,
            sender: 'Milt Devovich',
            text: 'Используйте клавишу "N"'
          }
        ]
      },
      {
        id: 5470,
        time: 1600182247800,
        reporterName: 'Sharmeek Cherlynnes',
        reporterId: 428,
        reporterStatus: 0,
        rating: 2,
        messages: [
          {
            id: 0,
            sender: 'Sharmeek Cherlynnes',
            text: 'Хороший вопрос, дайте мне, пожалуйста, минуту, уточню информацию...'
          },
          {
            id: 1,
            sender: 'Milt Devovich',
            text: 'Используйте клавишу "N" на вашей клавиатуре'
          }
        ]
      },
      {
        id: 5471,
        time: 1600182247800,
        reporterName: 'Sharmeek Cherlynnes',
        reporterId: 428,
        reporterStatus: 0,
        rating: 5,
        messages: [
          {
            id: 0,
            sender: 'Sharmeek Cherlynnes',
            text: 'Хороший вопрос, дайте мне, пожалуйста, минуту, уточню информацию...'
          },
          {
            id: 1,
            sender: 'Milt Devovich',
            text: 'Используйте клавишу "N" на вашей клавиатуре'
          }
        ]
      },
      {
        id: 5472,
        time: 1600182247800,
        reporterName: 'Sharmeek Cherlynnes',
        reporterId: 428,
        reporterStatus: 0,
        rating: 5,
        messages: [
          {
            id: 0,
            sender: 'Sharmeek Cherlynnes',
            text: 'Хороший вопрос, дайте мне, пожалуйста, минуту, уточню информацию...'
          },
          {
            id: 1,
            sender: 'Milt Devovich',
            text: 'Используйте клавишу "N" на вашей клавиатуре'
          },
          {
            id: 1,
            sender: 'Milt Devovich',
            text: 'Используйте'
          }
          ,
          {
            id: 1,
            sender: 'Milt Devovich',
            text: 'На  вашей клавиатуре... '
          },
          {
            id: 0,
            sender: 'Sharmeek Cherlynnes',
            text: 'Хороший вопрос, дайте мне информацию...'
          },
        ]
      },
    ],

    reportsLogs: [
      {
        id: 5467,
        time: 1600182247800,
        reporterName: 'Sharmeek Cherlynnes',
        reporterId: 428,
        reporterStatus: 1,
        rating: 5,
        messages: [
          {
            id: 0,
            sender: 'Sharmeek Cherlynnes',
            text: 'Хороший вопрос, дайте мне, пожалуйста, минуту, уточню информацию...'
          },
          {
            id: 1,
            sender: 'Milt Kekovich',
            text: 'Используйте клавишу "N" на вашей клавиатуре'
          }
        ]
      },
      {
        id: 5469,
        time: 1600182247800,
        reporterName: 'Sharmeek Cherlynnes',
        reporterId: 428,
        reporterStatus: 3,
        rating: 3,
        messages: [
          {
            id: 0,
            sender: 'Sharmeek Cherlynnes',
            text: 'Хороший вопрос, дайте мне, пожалуйста, минуту, уточню информацию...'
          },
          {
            id: 1,
            sender: 'Milt Devovich',
            text: 'Используйте клавишу "N" на вашей клавиатуре'
          }
        ]
      },
      {
        id: 5470,
        time: 1600182247800,
        reporterName: 'Sharmeek Cherlynnes',
        reporterId: 428,
        reporterStatus: 0,
        rating: 2,
        messages: [
          {
            id: 0,
            sender: 'Sharmeek Cherlynnes',
            text: 'Хороший вопрос, дайте мне, пожалуйста, минуту, уточню информацию...'
          },
          {
            id: 1,
            sender: 'Milt Devovich',
            text: 'Используйте клавишу "N" на вашей клавиатуре'
          }
        ]
      },
      {
        id: 5471,
        time: 1600182247800,
        reporterName: 'Sharmeek Cherlynnes',
        reporterId: 428,
        reporterStatus: 0,
        rating: 5,
        messages: [
          {
            id: 0,
            sender: 'Sharmeek Cherlynnes',
            text: 'Хороший вопрос, дайте мне, пожалуйста, минуту, уточню информацию...'
          },
          {
            id: 1,
            sender: 'Milt Devovich',
            text: 'Используйте клавишу "N" на вашей клавиатуре'
          }
        ]
      },
      {
        id: 5472,
        time: 1600182247800,
        reporterName: 'Sharmeek Cherlynnes',
        reporterId: 428,
        reporterStatus: 0,
        rating: 5,
        messages: [
          {
            id: 0,
            sender: 'Sharmeek Cherlynnes',
            text: 'Хороший вопрос, дайте мне, пожалуйста, минуту, уточню информацию...'
          },
          {
            id: 1,
            sender: 'Milt Devovich',
            text: 'Используйте клавишу "N" на вашей клавиатуре'
          },
          {
            id: 1,
            sender: 'Milt Devovich',
            text: 'Используйте'
          }
          ,
          {
            id: 1,
            sender: 'Milt Devovich',
            text: 'На  вашей клавиатуре... '
          },
          {
            id: 0,
            sender: 'Sharmeek Cherlynnes',
            text: 'Хороший вопрос, дайте мне информацию...'
          },
        ]
      },
      {
        id: 5473,
        time: 1600182247800,
        reporterName: 'Sharmeek Cherlynnes',
        reporterId: 428,
        reporterStatus: 0,
        rating: 5,
        messages: [
          {
            id: 0,
            sender: 'Sharmeek Cherlynnes',
            text: 'Хороший вопрос, дайте мне, пожалуйста, минуту, уточню информацию...'
          },
          {
            id: 1,
            sender: 'Milt Devovich',
            text: 'Используйте клавишу "N" на вашей клавиатуре'
          },
          {
            id: 1,
            sender: 'Kekovich Devovich',
            text: 'Используйте'
          }
          ,
          {
            id: 1,
            sender: 'Kekovic Devovich',
            text: 'На  вашей клавиатуре... '
          },
          {
            id: 0,
            sender: 'Sharmeek Cherlynnes',
            text: 'Хороший вопрос, дайте мне информацию...'
          },
        ]
      },
      {
        id: 5474,
        time: 1600182247800,
        reporterName: 'Sharmeek Cherlynnes',
        reporterId: 428,
        reporterStatus: 0,
        rating: 5,
        messages: [
          {
            id: 0,
            sender: 'Sharmeek Cherlynnes',
            text: 'Хороший вопрос, дайте мне, пожалуйста, минуту, уточню информацию...'
          },
          {
            id: 1,
            sender: 'Milt Devovich',
            text: 'Используйте клавишу "N" на вашей клавиатуре'
          },
          {
            id: 1,
            sender: 'Milt Devovich',
            text: 'Используйте'
          }
          ,
          {
            id: 1,
            sender: 'Milt Kekovich',
            text: 'На  вашей клавиатуре... '
          },
          {
            id: 0,
            sender: 'Sharmeek Cherlynnes',
            text: 'Хороший вопрос, дайте мне информацию...'
          },
        ]
      },
      {
        id: 5475,
        time: 1600182247800,
        reporterName: 'Sharmeek Cherlynnes',
        reporterId: 428,
        reporterStatus: 0,
        rating: 5,
        messages: [
          {
            id: 0,
            sender: 'Sharmeek Cherlynnes',
            text: 'Хороший вопрос, дайте мне, пожалуйста, минуту, уточню информацию...'
          },
          {
            id: 1,
            sender: 'Milt Devovich',
            text: 'Используйте клавишу "N" на вашей клавиатуре'
          },
          {
            id: 1,
            sender: 'Milt Testovich',
            text: 'Используйте'
          }
          ,
          {
            id: 1,
            sender: 'Milt Devovich',
            text: 'На  вашей клавиатуре... '
          },
          {
            id: 0,
            sender: 'Sharmeek Cherlynnes',
            text: 'Хороший вопрос, дайте мне информацию...'
          },
        ]
      },
    ],

    reportsLogAdmins: [],

    quickActions: [
      {
        key: 0,
        name: 'ReportMenu_25',
        type: 'trigger'
      },
      {
        key: 4,
        name: 'ReportMenu_30',
        type: 'position'
      },
      {
        key: 1,
        name: 'ReportMenu_26',
        type: 'trigger'
      },
      {
        key: 2,
        name: 'ReportMenu_27',
        type: 'kick'
      },
      {
        key: 3,
        name: 'ReportMenu_28',
        type: 'trigger'
      }
    ],

    currentPage: 'ReportsList',
    currentChat: null,
  },

  mutations: {
    setCurrentPage: function (state, page) {
      state.currentPage = page
      state.currentChat = null
    },
    setCurrentChat: function (state, chat) {
      state.currentChat = chat
      if (chat != null && state.currentPage == 'ReportsList' && chat.adminName == null) {
        window.mp.trigger("reportMenu:trytakereport", chat.id)
      }
      if (chat != null) {
        const index = state.reports.findIndex(item => item.id == chat.id)
        if (index > -1)
          state.reports[index].unreadMessages = 0;
      }
    },
    refreshCurrentChat: function (state) {
      state.currentChat = null
    },
    deleteAnswerTemplate: function (state, index) {
      state.answersTemplates.splice(index, 1)
    },
    addAnswerTemplate: function (state, text) {
      state.answersTemplates.push(text)
    },
    setMySettingsName: function (state, value) {
      state.mySettings.name = value;
    },
    setAnswersTemplates: function (state, value) {
      state.answersTemplates = value;
    },
    setReportsLogAdmins: function (state, value) {
      state.reportsLogAdmins = value;
    },
    addReportsLogAdmins: function (state, value) {
      if (value != null && state.reportsLogAdmins.findIndex(item => item == value) == -1)
        state.reportsLogAdmins.push(value)
    },

    setReports: function (state, value) {
      state.reports = value;
      state.reports.sort(function (a, b) {
        return b.reporterStatus - a.reporterStatus;
      })
    },
    addReport: function (state, value) {
      const index = state.reports.findIndex(item => item.id == value);
      if (index == -1)
        state.reports.push(value);
      else
        state.reports[index] = value;
      state.reports.sort(function (a, b) {
        return b.reporterStatus - a.reporterStatus;
      })
    },
    deleteReport: function (state, value) {
      const index = state.reports.findIndex(item => item.id == value);
      if (index != -1)
        state.reports.splice(index, 1);
      if (state.currentPage == 'ReportsList' && state.currentChat.id == value)
        state.currentChat = null
    },
    addReportMessage: function (state, value) {
      const index = state.reports.findIndex(item => item.id == value.reportId);
      if (index != -1) {
        state.reports[index].messages.push(value);
        if (state.chat == null || state.chat.id != state.reports[index].id)
          state.reports[index].unreadMessages ++
      }
    },

    setMyReports: function (state, value) {
      state.myReports = value;
    },
    addMyReport: function (state, value) {
      state.myReports.unshift(value);
    },

    setReportsLogs: function (state, value) {
      state.reportsLogs = value;
    },
    addReportLogs: function (state, value) {
      const index = state.reportsLogs.findIndex(item => item.id == value.id);
      if (index == -1)
        state.reportsLogs.unshift(value);
      else
        state.reportsLogs[index] = value;
    },
    deleteReportLogs: function (state, value) {
      const index = state.reportsLogs.findIndex(item => item.id == value);
      if (index != -1)
        state.reportsLogs.splice(index, 1);
      if (state.currentPage == 'ReportsLogs' && state.currentChat.id == value)
        state.currentChat = null
    },
    addReportLogsMessage: function (state, value) {
      const index = state.reportsLogs.findIndex(item => item.id == value.reportId);
      if (index != -1)
        state.reportsLogs[index].messages.push(value);
    },
    updateAdminNameForReportLog: function (state, value) {
      const index = state.reportsLogs.findIndex(item => item.id == value.id);
      if (index != -1)
        state.reportsLogs[index].adminName = value.adminName
    },

    updateRating: function (state, value) {
      const index = state.reportsLogs.findIndex(item => item.id == value.id);
      if (index != -1)
        state.reportsLogs[index].rating = value.rating
      const myIndex = state.myReports.findIndex(item => item.id == value.id);
      if (myIndex != -1)
        state.myReports[index].rating = value.rating
    },

    setReportsInQueue: function (state, value) {
      state.reportsInQueue = value;
    },
    setReportsAnswered: function (state, value) {
      state.reportsAnswered = value;
    },
    setMessageModel: function(state, value) {
      state.messageModel = value
    }
  }
}