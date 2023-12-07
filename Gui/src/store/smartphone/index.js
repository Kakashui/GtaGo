import bankPage from './modules/bank-page';
import weatherPage from './modules/weather-page';
import radioPage from './modules/radio-page';
import forbesPage from './modules/forbes-page';
import newsPage from './modules/news-page';
import messagePage from './modules/message-page';
import taxiPage from './modules/taxi-page';
import appPage from './modules/app-page';



export default {
  namespaced: true,

  state: {
    phoneBalance: 100,
    notificationsSounds: [
      { id: 0, soundName: 'notifications_chord', title: 'Chord' },
      { id: 1, soundName: 'notifications_bamboo', title: 'Bamboo' },
      { id: 2, soundName: 'notifications_bubble', title: 'Bubble' },
      { id: 3, soundName: 'notifications_email', title: 'Email' },
      { id: 4, soundName: 'notifications_haveamessage', title: 'You have a message' },
      { id: 5, soundName: 'notifications_ping', title: 'Ping' },
      { id: 6, soundName: 'notifications_sneeze', title: 'Sneeze' },
      { id: 7, soundName: 'notifications_tequila', title: 'Tequila' },
      { id: 8, soundName: 'notifications_vibro', title: 'Vibro' }
    ],
    ringtonesSounds: [
      { id: 0, soundName: 'ringtone_reflection', title: 'Reflection' },
      { id: 1, soundName: 'ringtone_apex', title: 'Apex' },
      { id: 2, soundName: 'ringtone_beacon', title: 'Beacon' },
      { id: 3, soundName: 'ringtone_bulletin', title: 'Bulletin' },
      { id: 4, soundName: 'ringtone_cosmic', title: 'Cosmic' },
      { id: 5, soundName: 'ringtone_crystals', title: 'Crystals' },
      { id: 6, soundName: 'ringtone_hillside', title: 'Hillside' },
      { id: 7, soundName: 'ringtone_illuminate', title: 'Illuminate' },
      { id: 8, soundName: 'ringtone_presto', title: 'Presto' },
    ],

    currentTab: {
      key: 'DesktopTab',
      section: null,
      subsection: null,
      data: null
    },
    backgroundTabs: {
      /* "key": { section, subsection, data } */
    },
    colorTheme: {
      // header: 'dark',
      // button: 'dark'
    },
    configuration: {
      Username: 'Darlene_Robertson',
      Number: 88005553535,
      IsAirplaneMode: false,
      NotificationSound: 0,
      Ringtone: 0,
      Wallpaper: 1
    },
    geolocation: {
      currentPosition: { x: 277.2, y: 737.2, z: 837.1 },
      zoneName: "NO",
      streetName: "LOCATION"
    },
    contacts: [
      // { id: 7, name: 'Conan_Barbarian', number: 88305573535 },
      // { id: 0, name: 'Abraham_Mint', number: 88005553535 },
      // { id: 3, name: 'alfred_Bond', number: 88005543535 },
    ],
    blockedContacts: [
      // { number: 88305543535 }
    ],
    calls: [
      // { id: 0, name: 'Conan_Barbarian', date: '13:20', type: 'outgoing' },
      // { id: 1, name: 'Conan_Barbarian', date: '14:20', type: 'income', isSuccessful: false },
      // { id: 2, name: '800777354', date: '14:20', type: 'outgoing' },
    ],
    callData: {
      destination: '444521',
      number: 444521,
      status: 'outcome'
    },
    contactsUserData: {
      isKnown: true,
      name: 'Brandon_Bond',
      isBlocked: null,
      number: 879436,
      callHistory: [
        // { id: 0, type: 'income', date: '11.09.2020', duration: 160 },
        // { id: 1, type: 'outgoing', date: '21.09.2020', duration: 160 },
        // { id: 2, type: 'outgoing', date: '21.09.2020', duration: 160 },
      ]
    },
    notificationData: null,
    gpsData: {
      currentArea: 'South Dakota 83475',
      currentLocation: '2715 Ash Dr. San Jose',
      categories: [
        { title: 'phone:gps:cat:1', icon: 'gun', key: 'gun' },
        { title: 'phone:gps:cat:2', icon: 'tShirt', key: 'clothes' },
        { title: 'phone:gps:cat:3', icon: 'goverment', key: 'gov' },
        { title: 'phone:gps:cat:4', icon: 'briefcase', key: 'works' },
        { title: 'phone:gps:cat:5', icon: 'skull', key: 'gangs' },
        { title: 'phone:gps:cat:6', icon: 'atm', key: 'atms' },
        { title: 'phone:gps:cat:7', icon: 'car ', key: 'carrent' },
        { title: 'phone:gps:cat:8', icon: 'fuel', key: 'gasstations' },
        { title: '24/7', icon: 'store', key: 'shops24' },
        { title: 'phone:gps:cat:9', icon: 'carKey', key: 'autorooms' },
        { title: 'phone:gps:cat:10', icon: 'burger', key: 'burgershots' },
        { title: 'phone:gps:cat:11', icon: 'waterDrops', key: 'carwashes' },
        { title: 'phone:gps:cat:12', icon: 'pinTool', key: 'servicestation' }
      ],
      _cachedItems: {
        "atms": [
          { Title: 'Банкомат', Icon: 'atm', Address: '3517 W. Gray St. Utica, Pennsylvania 57867', Position: { X: 0, Y: 0, Z: 0 } }
        ]
      },
      categoryItems: [
        { title: 'Оружейные магазины №12', icon: 'fuel', address: '3517 W. Gray St. Utica, Pennsylvania 57867', distance: 12000 },
        { title: 'Оружейные магазины №12', icon: 'fuel', address: '3517 W. Gray St. Utica, Pennsylvania 57867', distance: 12000 },
      ]
    }
  },

  mutations: {
    setCurrentTab: function(state, { key, section = null, subsection = null, data = null }) {
      if (!data)
        data = { location: {key: state.currentTab.key, section: state.currentTab.section, subsection: state.currentTab.subsection} }
      else
        data.location = {key: state.currentTab.key, section: state.currentTab.section, subsection: state.currentTab.subsection}
      state.oldTab = state.currentTab;
      state.currentTab = { key, section, subsection, data };

      state.backgroundTabs[key] = { section, subsection, data };
    },

    setOldTab: function(state) {
      state.currentTab = state.oldTab;
    },

    setConfigWallpaper: function(state, value) {
      state.configuration.Wallpaper = value
    },

    setConfigNotificationSound: function(state, value) {
      state.configuration.NotificationSound = value
    },

    setConfigRingtoneSound: function(state, value) {
      state.configuration.Ringtone = value
    },

    setConfigIsAirplaneMode: function(state, value) {
      state.configuration.IsAirplaneMode = value
    },

    setContactsUserData: function(state, data) {
      state.contactsUserData = data
    },

    setColorTheme: function(state, data) {
      state.colorTheme = data
    },

    setCurrentPosition: function(state, location) {
      state.geolocation = location;
    },

    //#region Contacts
    addContacts: function(state, data) {
      data.forEach(contact => {
        state.contacts.push(contact);
      });      
    },

    addContact: function(state, contactData) {
      state.contacts.push(contactData);

      if (state.contactsUserData.number === contactData.number) {
        state.contactsUserData.isKnown = true;
        state.contactsUserData.id = contactData.id;
        state.contactsUserData.name = contactData.name;
      }
    },

    editContact: function(state, contactData) {
      const contactIndex = state.contacts.findIndex(c => c.id === contactData.id);
      if (contactIndex !== -1) {
        state.contacts[contactIndex] = contactData;
      }

      if (state.contactsUserData.id === contactData.id) {        
        state.contactsUserData.name = contactData.name;
        state.contactsUserData.number = contactData.number;
      }
    },

    removeContact: function(state, contactId) {
      const contactIndex = state.contacts.findIndex(c => c.id === contactId);
      if (contactIndex !== -1) {
        state.contacts.splice(contactIndex, 1);

        if (state.contactsUserData.id === contactId) {
          state.contactsUserData.isKnown = false;
        }
      }
    },

    addBlockItems: function(state, blockedContacts) {
      blockedContacts.forEach((blocked) => {
        state.blockedContacts.push(blocked);
      });
    },

    addBlockItem: function(state, blocked) {
      const oldBlocked = state.blockedContacts.find(b => b.number === blocked.number);
      if (!oldBlocked) {
        state.blockedContacts.push(blocked);

        if (state.contactsUserData.number === blocked.number) {
          state.contactsUserData.isBlocked = true;
        }
      }
    },

    removeBlockItem: function(state, number) {
      const blockedIndex = state.blockedContacts.findIndex(b => b.number === number);
      if (blockedIndex !== -1) {
        state.blockedContacts.splice(blockedIndex, 1);
        
        if (state.contactsUserData.number === number) {
          state.contactsUserData.isBlocked = false;
        }
      }
    },

    setPersonalConfiguration: function(state, { Number, Name }) {
      state.configuration.Number = Number;
      state.configuration.Username = Name;
    },

    setOpenedContactData: function(state, contactId) {
      const contact = state.contacts.find(c => c.id === contactId);
      if (contact) {
        const isBlocked = state.blockedContacts.findIndex(b => b.number === contact.number) !== -1;
        const callHistory = state.calls.filter(c => c.number === contact.number);
        state.contactsUserData = {
          id: contactId,
          isKnown: true,
          name: contact.name,
          isBlocked: isBlocked,
          number: contact.number,
          callHistory: callHistory
        };
      }
    },

    setOpenedContactByNumber: function(state, number) {
      const contact = state.contacts.find(c => c.number === number);
      if (contact) {
        const isBlocked = state.blockedContacts.findIndex(b => b.number === contact.number) !== -1;
        const callHistory = state.calls.filter(c => c.number === contact.number);
        state.contactsUserData = {
          id: contact.id,
          isKnown: true,
          name: contact.name,
          isBlocked: isBlocked,
          number: contact.number,
          callHistory: callHistory
        };
      }
      else {
        const isBlocked = state.blockedContacts.findIndex(b => b.number === number) !== -1;
        const callHistory = state.calls.filter(c => c.number === number);

        state.contactsUserData = {
          isKnown: false,
          name: number,
          number: number,
          isBlocked: isBlocked,
          callHistory: callHistory
        };
      }
    },
    //#endregion Contacts

    //#region Calls
    setNotificationData: function(state, data) {
      state.notificationData = data
    },

    setFakeCall: function(state, { callerName }) {
      state.callData = {
        destination: callerName,
        number: callerName,
        status: 'active',
        isFake: true
      };

      state.oldTab = state.currentTab;
      state.currentTab = {
        data: {theme: 'dark'},
        key: 'CallTab'
      };
    },

    endFakeCall: function(state) {
      state.callData.dropReason = 'phone:call:end';
      state.callData.status = 'drop';
    },
    
    incomeCall: function(state, number) {
      const data = {
        name: number,
        number: number
      };
      
      const contact = state.contacts.find(c => c.number === data.number);
      if (contact) {
        data.name = contact.name;
      }

      // Открываем на весь экран
      if (state.currentTab.key === 'DesktopTab') {
        state.callData = {
          destination: data.name,
          number: number,
          status: 'income'
        };

        state.oldTab = state.currentTab;
        state.currentTab = {
          data: {theme: 'dark'},
          key: 'CallTab'
        };
      }
      // Кидаем в пуш уведомление
      else {
        state.notificationData = { name: data.name, number: data.number }
      }


      if (!state.configuration.IsAirplaneMode) {
        const sound = state.ringtonesSounds
          .find(rs => rs.id === state.configuration.Ringtone);

        if (sound) {
          window.setData('sounds/play', { name: sound.soundName, volume: 0.3, loop: true });
        }
      }
    },

    takeCall: function(state, number) {
      state.notificationData = null;

      const contact = state.contacts.find(c => c.number === number);
      let name = number;
      if (contact) {
        name = contact.name;
      }

      state.callData = {
        destination: name,
        number: number,
        status: 'active'
      };

      state.oldTab = state.currentTab;
      state.currentTab = {
        data: {theme: 'dark'},
        key: 'CallTab'
      };
      
      window.mp.triggerServer('phone::calls::take', number);
      window.mp.trigger('phone::calls::phoneUp');

      window.setData('sounds/stop');
    },

    setCallActive: function(state, number) {
      const contact = state.contacts.find(c => c.number === number);
      let name = number;
      if (contact) {
        name = contact.name;
      }
      
      state.callData.name = name;
      state.callData.number = number;
      state.callData.status = 'active';
    },

    dropCall: function(state, { reason }) {
      state.callData.dropReason = reason;
      state.callData.status = 'drop';
      
      window.mp.trigger('phone::calls::phoneDown');
      window.setData('sounds/stop');
    },

    setCallData: function(state, data) {
      data.number = data.destination;

      const contact = state.contacts.find(c => c.number === data.number);
      if (contact) {
        data.destination = contact.name;
      }
      
      state.callData = data;

      if (data.status === 'outcome') {
        window.mp.triggerServer('phone::calls::call', data.number);
        window.mp.trigger('phone::calls::phoneUp');
      }
    },
    //#endregion Calls

    //#region Call history
    loadCallhistory: function(state, calls) {
      calls.forEach(call => {
        call.type = (call.type === 0) ? 'income' : 'outgoing';

        call.date *= 1000;

        if (!state.calls.find(c => c.id === call.id)) {
          state.calls.push(call);
        }

        // Добавляем в текущего пользователя
        if (state.contactsUserData.number === call.number) {
          if (!state.contactsUserData.callHistory.find(c => c.id === call.id)) {
            state.contactsUserData.callHistory.push(call);
          }
        }

        state.calls.sort((c1, c2) => c2.date - c1.date);
      });
    },
    //#endregion Call history

    gps_loadData: function(state, data) {
      state.gpsData._cachedItems = data;
    },
    setPhoneBalance: function(state, value) {
      state.phoneBalance = value
    }
  },

  actions: {
    /**
     * Если key равен текущему открытому табу, то он сразу устанавливает его.
     * Иначе сохраняет секцию для ключа и при открытии приложения будет открыта эта секция
     */
     setTabOrBack: function({ state, commit }, { key, section = null, subsection = null, data = null }) {
      if (key == state.currentTab.key) {
        commit('setCurrentTab', { key, section, subsection, data });
      }
      else {
        state.backgroundTabs[key] = { section, subsection, data }
      }
    },

    msg_goToChats: function({ state, commit, dispatch }) {
      if (state.currentTab.key === 'MessengerTab' && state.currentTab.section === 'RegistrationSection')
      {
        const key = state.currentTab.key;
        const section = 'GeneralSection';
        const subsection = 'GeneralSubsection';
        commit('setCurrentTab', { key, section, subsection, data: null });

        dispatch('smartphone/messagePage/msg_dropCurrentChat')
      }
    },

    gps_openCategory({ state, commit }, categoryKey) {
      function getDistance(pos1, pos2) {
        return Math.sqrt(
          Math.pow((pos2.X ?? pos2.x) - (pos1.X ?? pos1.x), 2) +
          Math.pow((pos2.Y ?? pos2.y) - (pos1.Y ?? pos1.y), 2) +
          Math.pow((pos2.Z ?? pos2.z) - (pos1.Z ?? pos1.z), 2)
        );
      }
      
      const category = state.gpsData.categories
        .find(cat => cat.key === categoryKey);
      const title = category.title;
        
      state.gpsData.categoryItems = [];

      if (state.gpsData._cachedItems[category.key]) {
        state.gpsData._cachedItems[category.key]
          .forEach(gpsItem => {
            const item = {
              title: gpsItem.Title,
              icon: category.icon,
              address: gpsItem.Address,
              position: gpsItem.Position,
              distance: Math.floor(getDistance(gpsItem.Position, state.geolocation.currentPosition))
            };

            state.gpsData.categoryItems.push(item);
          });
      }

      state.gpsData.categoryItems.sort((a, b) => a.distance - b.distance);
      
      commit('setCurrentTab', {
        key: state.currentTab.key, 
        section: 'SecondarySection', 
        data: { title: title }
      });
    },


    sounds_playNotification({ state }) {
      const notificationSound = state.notificationsSounds
        .find(ns => ns.id === state.configuration.notificationSound);

      if (!notificationSound) return;

      window.setData('sounds/play', { name: notificationSound.soundName, volume: 0.3 });
    }
  },
  modules: {
    bankPage,
    weatherPage,
    radioPage,
    forbesPage,
    newsPage,
    messagePage,
    taxiPage,
    appPage,

  }
}
