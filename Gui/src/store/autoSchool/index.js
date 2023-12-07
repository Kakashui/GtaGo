export default {
  namespaced: true,

  state: {
    currentSchoolName: 'AutoSchool_26',
    currentSchoolWelcome: 'AutoSchool_32',
    currentSection: {
      section: 'GeneralSection',
      data: null,
    },
    questions: [
      {
        key: 1,
        quest: 'school_1',
        answers: [
          { key: 0, title: 'school_2' },
          { key: 1, title: 'school_3' },
          { key: 2, title: 'school_4' },
        ],
        image: '',
        correctAnswer: 1
      },
      {
        key: 2,
        quest: 'school_5',
        answers: [
          { key: 0, title: 'school_6' },
          { key: 1, title: 'school_7' },
          { key: 2, title: 'school_8' },
        ],
        image: 'question-img.png',
        correctAnswer: 3
      },
    ],
    currentAnswer: null,
    currentQuestionKey: null,
    currentQuestionIndex: 0,

    answersOnQuestions: { 
      totalQuest:10, 
      correctQuest:5, 
      result: false, 
      questions: [
        {
          quest: 'school_9',
          myAnswer: 'school_10',
          correctAnswer: 'school_11'
        },
        {
          quest: 'school_12',
          myAnswer: 'school_13',
          correctAnswer: 'school_13'
        }
      ] 
    },
    practicResults: {
      time: '04:27',
      quality: 5,
      result: false
    }
  },

  mutations: {
    setCurrentSection: function(state, value) {
      state.currentSection.section = value.section
      state.currentSection.data = value.data
    },
    setAnswer: function(state, {answer, question}) {
      state.currentAnswer = answer;
      state.currentQuestionKey = question;
    },
    switchNextQuestion: function(store) {
      store.currentQuestionKey = null
      store.currentAnswer = null
      store.currentQuestionIndex++
    },
    switchDropQuestion: function(store) {
      store.currentQuestionKey = null
      store.currentAnswer = null
      store.currentQuestionIndex = 0
    },
    setQuestions: function(state, value) {
      state.questions = value;
    },
    setAnswersOnQuestions: function(state, value) {
      state.answersOnQuestions = value;
    },
    setPracticResults: function(state, value) {
      state.practicResults = value;
    },
    setCurrentSchoolName: function(state, value) {
      state.currentSchoolName = value;
    },
    setCurrentSchoolWelcome: function(state, value) {
      state.currentSchoolWelcome = value;
    }
  }
}