export default {
  namespaced: true,
  state: {
    applications: [
      { id: 7, title: 'phone:app:tit:1', icon: 'appStore', key: 'AppStoreTab', section: 'GeneralSection', isAvailable: true, isInstalled: false, desc: 'phone:app:desc:1', img: 'bank', tagline: 'phone:app:tag:1', forMoney: false },
      { id: 1, title: 'phone:app:tit:2', icon: 'weather', key: 'WeatherTab', isAvailable: true, isInstalled: false, desc: 'phone:app:desc:2', img: 'weather', tagline: 'phone:app:tag:2', forMoney: true },
      { id: 2, title: 'phone:app:tit:3', icon: 'radioGo', key: 'RadioTab', isAvailable: true, isInstalled: false, desc: 'phone:app:desc:3', img: 'radio', tagline: 'phone:app:tag:3', forMoney: false },
      { id: 3, title: 'phone:app:tit:4', icon: 'forbes', key: 'ForbesTab', isAvailable: true, isInstalled: false, desc: 'phone:app:desc:4', img: 'forbes', tagline: 'phone:app:tag:4', forMoney: true },
      { id: 4, title: 'phone:app:tit:5', icon: 'bankGo', key: 'BankTab', section: 'GeneralSection', isAvailable: true, isInstalled: true, desc: 'phone:app:desc:5', img: 'bank', tagline: 'phone:app:tag:5', forMoney: true },
      { id: 5, title: 'phone:app:tit:6', icon: 'taxiGo', key: 'TaxiTab', section: 'GeneralSection', isAvailable: true, isInstalled: false, desc: 'phone:app:desc:6', img: 'taxi', tagline: 'phone:app:tag:6', forMoney: true },
      { id: 6, title: 'phone:app:tit:7', icon: 'taxiJob', key: 'TaxiJobTab', section: 'GeneralSection', isAvailable: true, isInstalled: false, desc: 'phone:app:desc:7', img: 'taxijob', tagline: 'phone:app:tag:7', forMoney: true },
      { id: 8, title: 'phone:app:tit:8', icon: 'news', key: 'NewsTab', section: 'GeneralSection', isAvailable: true, isInstalled: false, desc: 'phone:app:desc:8', img: 'news', tagline: 'phone:app:tag:8', forMoney: true },
      { id: 9, title: 'phone:app:tit:9', icon: 'camera', type: 'triggerEvent', eventName: 'phone::camera::open', isAvailable: true, isInstalled: false, desc: 'phone:app:desc:9', img: 'camera', tagline: 'phone:app:tag:9', forMoney: false },
    ],
    mainApplications: [
      { id: 0, key: 'ContactsTab', icon: 'contacts', section: 'GeneralSection', isAvailable: true, forMoney: false },
      { id: 1, key: 'MessengerTab', icon: 'message', section: 'GeneralSection', isAvailable: true, forMoney: true },
      { id: 2, key: 'MapTab', icon: 'map', section: 'GeneralSection', isAvailable: true, forMoney: false },
      { id: 3, key: 'SettingsTab', icon: 'settings', section: 'GeneralSection', isAvailable: true, forMoney: false }
    ]
  },
  mutations: {

    setAppStoreItemIsInstalled: function (state, { id, value }) {
      const rightIndex = state.applications.findIndex(item => item.id === id)
      state.applications[rightIndex].isInstalled = value
    },
    setInstalledApps: function (state, data) {
      data.forEach(id => {
        const rightIndex = state.applications.findIndex(item => item.id === id)
        state.applications[rightIndex].isInstalled = true
      });
    },
  },

  actions: {
  }
}
