export default {
  namespaced: true,
  state: {
    messengerData: {
      CurrentAccount: {
        Id: -1,
        Username: 'milt',
        DisplayedName: 'matvey',
        IsNumberHided: false
      },
      SearchSubscribers: [
        // { Id: 0, DisplayedName: 'Gay Man', LastVisitTime: 203331212332 }
      ],
      Chats: [
        // { Id: 8, Name: 'First chat8', IsOnline: true, Type: 0, NonReadMessagesCount: 0,
        // LastMessage: { Id: 0, ChatId: 0, Text: 'Hi! Who are you?', Type: 0,
        // Time: 1603005132000, IsRead: false, Admins: [
        //    { DisplayedName: 'Gondon', Id: 2, AdminLvl: 2, Permission: [0, 2, 3, 4] } 
        // ] } },
      ],
      Contacts: [
        // { Id: 12, Name: 'სანტექნიკოსი', Number: 88005543535, Time: 1130443200000, IsOnline: true },
      ],
      _cachedAccounts: [
        // { Id: 0, DisplayedName: 'факбой' }
      ],
      _cachedMessages: [

      ],
      CurrentChat: {
        Id: 0,
        Name: 'SJSJFKWPCOEJV SJEKRIFJANDLWR',
        IsOnline: null,
        Type: 0,
        IconColors: 'linear-gradient(90deg, #43E97B 0%, #38F9D7 100%)',
        SubscribersCount: 123,
        Items: [
          // { Id: 0, ChatId: 0, SenderId: 0, Text: 'Hi! Who are you? 😃😁', Attachment: null , Time: 1603202898482, IsRead: true },
          // { Id: 1, ChatId: 0, SenderId: 1, Text: 'It`s me, nigga, ayo', Attachment: { Id: 1, Type: 2, Url: 'https://c4.wallpaperflare.com/wallpaper/500/442/354/outrun-vaporwave-hd-wallpaper-preview.jpg' }, Time: 1603202898487, IsRead: true },
          // { Id: 0, ChatId: 0, SenderId: 1, Text: '', Attachment: { Id: 1, Type: 0, GeoMark: { X: 100, Y: 100, Z: 100} } , Time: 1603202898482, IsRead: true },
          // { Id: 0, ChatId: 0, SenderId: 0, Text: '', Attachment: { Id: 1, Type: 0, GeoMark: { X: 100, Y: 100, Z: 100} } , Time: 1603202898482, IsRead: true },
          // { Id: 1, ChatId: 0, SenderId: 0, Text: '', Attachment: { Id: 1, Type: 2, Url: 'https://c4.wallpaperflare.com/wallpaper/500/442/354/outrun-vaporwave-hd-wallpaper-preview.jpg' }, Time: 1603202898487, IsRead: true },
          // { Id: 1, ChatId: 0, SenderId: 1, Text: '', Attachment: { Id: 1, Type: 2, Url: 'https://c4.wallpaperflare.com/wallpaper/500/442/354/outrun-vaporwave-hd-wallpaper-preview.jpg' }, Time: 1603202898487, IsRead: true },
          // { Id: 2, ChatId: 0, SenderId: 2, Text: 'Mmm, ok.Mmm, okMmm, okMmm, okMmm, okMmm, okMmm, okMmm, okMmm, okMmm, okMmm, okMmm, ok', Time: 1603308898487, IsRead: false },
          // { Id: 2, ChatId: 0, SenderId: 2, Text: 'Mmm, ok.Mmm, okMmm, okMmm, okMmm, okMmm, okMmm, okMmm, okMmm, okMmm, okMmm, okMmm, ok', Time: 1603308898487, IsRead: false },
          // { Id: 2, ChatId: 0, SenderId: 2, Text: 'Mmm, ok.Mmm, okMmm, okMmm, okMmm, okMmm, okMmm, okMmm, okMmm, okMmm, okMmm, okMmm, ok', Time: 1603308898487, IsRead: false },
          // { Id: 2, ChatId: 0, SenderId: 2, Text: 'Mmm, ok.Mmm, okMmm, okMmm, okMmm, okMmm, okMmm, okMmm, okMmm, okMmm, okMmm, okMmm, ok', Time: 1603308898487, IsRead: false },
          // { Id: 2, ChatId: 0, SenderId: 2, Text: 'Mmm, ok.Mmm, okMmm, okMmm, okMmm, okMmm, okMmm, okMmm, okMmm, okMmm, okMmm, okMmm, ok', Time: 1603308898487, IsRead: false },
        ],
        IsLoading: false,
      },
      CanalProfile: {
        Name: 'Самый длинный ник в игре',
        IsOnline: true,
        AdminLvl: 2,
        Abilities: {
          changeProfile: true,
          createPosts: false,
          deletePosts: false,
          appointmentAdmins: false
        }
      },
      viewedProfile: {
        Id: 1,
        Username: 'username',
        DisplayedName: 'Самый длинный ник в игре',
        Number: 939394,
        IsNumberHided: false,
        IsMuted: false,
        IsBlocked: false,
        LastVisitTime: 1603959071000,
        IsOnline: false,
        IsInContacts: false,

        isLoading: false
      }
    }

  },
  mutations: {
    setMessengerCanalProfileAbility: function (state, { key, value }) {
      const currentChat = state.messengerData.CurrentChat;
      const chat = state.messengerData.Chats.find(c => c.Id === currentChat.Id);

      const admin = chat.Admins.find(a => a.Id === state.messengerData.CanalProfile.Id);
      if (admin) {
        if (value) admin.Permissions.push(key);
        else {
          const idx = admin.Permissions.indexOf(key);
          if (idx) admin.Permissions.splice(idx, 1);
        }
      }

      currentChat.Admins = chat.Admins;
      state.messengerData.CanalProfile.Permissions = admin.Permissions;

      window.mp.triggerServer('phone::msg::manage::setAccountPermission', chat.Id, admin.Id, key, value);

    },

    setMessengerCurrentChatIconColors: function (state, value) {
      state.messengerData.CurrentChat.IconColors = value
    },

    msg_setAccountSettings: function (state, accountSettings) {
      state.messengerData.CurrentAccount = accountSettings;
    },

    //#region Msg Contacts
    msg_setSubscribersForSearch: function (state, subscribers) {
      subscribers.forEach(subscriber => {
        if (subscriber.LastVisitTime)
          subscribers.LastVisitTime *= 1000;
      });

      state.messengerData.SearchSubscribers = subscribers;
    },

    msg_loadContacts: function (state, contacts) {
      contacts.forEach(contact => {
        if (contact.Time)
          contact.Time *= 1000;

        const oldContactIndex = state.messengerData.Contacts.findIndex(c => c.Id === contact.Id);
        if (oldContactIndex === -1) {
          state.messengerData.Contacts.push(contact);
        }
        else {
          state.messengerData.Contacts[oldContactIndex] = contact;
        }

        const privateChat = state.messengerData.Chats.find(c => c.AccountId === contact.AccountId);
        if (privateChat) {
          privateChat.Name = contact.Name;
        }
      });
    },

    msg_removeStoreContact: function (state, contactId) {
      const contactIndex = state.messengerData.Contacts.findIndex(c => c.Id === contactId);
      if (contactIndex !== -1) {
        state.messengerData.Contacts.splice(contactIndex, 1);
      }
    },

    msg_setContactIsOnline: function (state, { contactId, isOnline}) {
      const contactIndex = state.messengerData.Contacts.findIndex(c => c.Id === contactId);
      if (contactIndex !== -1) {
        state.messengerData.Contacts[contactIndex].IsOnline = isOnline
      }
    },
    //#endregion Msg Contacts

    //#region Msg Chats
    msg_loadChats: function (state, chats) {
      chats.forEach(chat => {
        const chatIndex = state.messengerData.Chats.findIndex(c => c.Id === chat.Id);

        chat.ActionTime = chat.LastMessage?.Time ?? chat.CreatedTime;
        chat.ActionTime *= 1000;

        if (chatIndex === -1) {
          state.messengerData.Chats.push(chat);
        }
        else {
          state.messengerData.Chats.splice(chatIndex, 1);
          state.messengerData.Chats.push(chat);
        }
      });
    },

    msg_removeChat: function (state, chatId) {
      const chatIdx = state.messengerData.Chats.findIndex(c => c.Id === chatId);
      if (chatIdx !== -1) {
        state.messengerData.Chats.splice(chatIdx, 1);
      }
    },

    msg_cacheMessage: function (state, message) {
      const cachedMessageIndex = state.messengerData._cachedMessages.findIndex(m => m.Id === message.Id);
      if (cachedMessageIndex === -1) {
        state.messengerData._cachedMessages.push(message);
      }
      else {
        state.messengerData._cachedMessages[cachedMessageIndex] = message;
      }
    },

    msg_uncacheMessage: function (state, messageId) {
      const cachedMessageIndex = state.messengerData._cachedMessages.findIndex(m => m.Id === messageId);
      if (cachedMessageIndex !== -1) {
        state.messengerData._cachedMessages.splice(cachedMessageIndex, 1);
      }
    },

    msg_markChatMessagesAsRead: function (state, chatId) {
      const chat = state.messengerData.Chats.find(c => c.Id === chatId);
      if (chat) {
        chat.NonReadMessagesCount = 0;
      }
    },

    msg_setOwnMessagesReadInChat: function (state, chatId) {
      if (state.messengerData.CurrentChat.Id === chatId) {
        state.messengerData.CurrentChat.Items.forEach(m => {
          if (m.SenderId === state.messengerData.CurrentAccount.Id) {
            m.IsRead = true;
          }
        });
      }

      state.messengerData._cachedMessages.forEach(m => {
        if (m.SenderId === state.messengerData.CurrentAccount.Id) {
          m.IsRead = true;
        }
      });
    },

    msg_setLastMessage: function (state, message) {
      const chat = state.messengerData.Chats.find(c => c.Id === message.ChatId);
      if (chat) {
        chat.LastMessage = message;
        chat.ActionTime = message.Time;
      }
    },

    msg_setCurrentChatIsLoading: function (state, toggle) {
      state.messengerData.CurrentChat.IsLoading = toggle;
    },

    msg_setCurrentChatMessages: function (state, messages) {
      state.messengerData.CurrentChat.Items = messages;
      state.messengerData.CurrentChat.IsLoading = false;
    },

    msg_setCurrentChat: function (state, { Id, Name, IsOnline, Type, SubscribersCount,
      AccountId, IconColors, Permissions, Description, AdminLvl, InviteCode, Admins, BlockItems
    }) {
      state.messengerData.CurrentChat = {
        Id, Name, IsOnline, Type, AdminLvl, Description,
        SubscribersCount, InviteCode, Admins, BlockItems
      }

      // Channel and group chats fields
      if (Permissions) {
        state.messengerData.CurrentChat.CanChangingGroupProfile = Permissions.includes(1);
        state.messengerData.CurrentChat.CanDeletingMessages = Permissions.includes(2);
        state.messengerData.CurrentChat.CanBlockingAccounts = Permissions.includes(3);
        state.messengerData.CurrentChat.CanPurposingAdmins = Permissions.includes(4);
        state.messengerData.CurrentChat.CanCreatingPosts = Permissions.includes(5);
      }

      if (IconColors) {
        state.messengerData.CurrentChat.IconColors = IconColors;
      }

      // Private chats field
      state.messengerData.CurrentChat.AccountId = AccountId;
    },

    msg_loadAccounts: function (state, accounts) {
      accounts.forEach(account => {
        const contact = state.messengerData.Contacts.find(c => c.AccountId === account.Id);
        if (contact) {
          account.DisplayedName = contact.Name;
        }

        const accountIndex = state.messengerData._cachedAccounts.findIndex(a => a.Id == account.Id);
        if (accountIndex === -1) {
          state.messengerData._cachedAccounts.push(account);
        }
        else {
          state.messengerData._cachedAccounts[accountIndex] = account;
        }
      });
    },

    msg_setChatMuted: function (state, { ChatId, IsMuted }) {
      if (state.messengerData.CurrentChat.Id === ChatId) {
        state.messengerData.CurrentChat.IsMuted = IsMuted;
      }

      const chat = state.messengerData.Chats.find(c => c.Id === ChatId);
      if (chat) {
        chat.IsMuted = IsMuted;
      }
    },

    msg_renameChat: function (state, { ChatId, Name }) {
      const chat = state.messengerData.Chats.find(c => c.Id === ChatId);
      if (chat) {
        chat.Name = Name;
      }

      if (state.messengerData.CurrentChat.Id === ChatId) {
        state.messengerData.CurrentChat.Name = Name;
      }
    },

    msg_increaseSubscribersCount: function (state, chatId) {
      if (state.messengerData.CurrentChat.Id === chatId) {
        state.messengerData.CurrentChat.SubscribersCount++;
      }

      const chat = state.messengerData.Chats.find(c => c.Id === chatId);
      if (chat) {
        if (!chat.SubscribersCount) {
          chat.SubscribersCount = 0;
        }

        chat.SubscribersCount++;
      }
    },

    msg_decreaseSubscribersCount: function (state, chatId) {
      if (state.messengerData.CurrentChat.Id === chatId) {
        state.messengerData.CurrentChat.SubscribersCount--;
      }

      const chat = state.messengerData.Chats.find(c => c.Id === chatId);
      if (chat) {
        if (!chat.SubscribersCount) {
          chat.SubscribersCount = 0;
        }

        chat.SubscribersCount--;
      }
    },

    msg_removeFromBlocklist: function (state, { ChatId, AccountId }) {
      const chat = state.messengerData.Chats.find(c => c.Id === ChatId);
      if (!chat)
        return;

      const blockIdx = chat.BlockItems.findIndex(b => b.AccountId === AccountId);
      if (blockIdx === -1)
        return;

      chat.BlockItems.splice(blockIdx, 1);

      if (state.messengerData.CurrentChat.Id === ChatId) {
        state.messengerData.CurrentChat.BlockItems = chat.BlockItems;
      }
    },

    msg_addBlocklistItems: function (state, { ChatId, BlockItems }) {
      const chat = state.messengerData.Chats.find(c => c.Id === ChatId);
      if (chat) {
        if (!chat.BlockItems)
          chat.BlockItems = [];

        BlockItems.forEach(i => {
          if (!chat.BlockItems.find(bi => bi.Id === i.Id)) {
            chat.BlockItems.push(i);
          }
        });
      }

      if (state.messengerData.CurrentChat.Id === ChatId) {
        state.messengerData.CurrentChat.BlockItems = chat.BlockItems;
      }
    },

    msg_setChatIsOnline: function (state, { ChatId, IsOnline }) {
      const chat = state.messengerData.Chats.find(c => c.Id === ChatId);
      if (chat) {
        chat.IsOnline = IsOnline;
      }

      if (state.messengerData.CurrentChat.Id === ChatId) {
        state.messengerData.CurrentChat.IsOnline = chat.IsOnline;
      }
    },
    //#endregion Msg Chats

    msg_setViewedProfileInfo: function (state, info) {
      info.IsInContacts = false;

      const contact = state.messengerData.Contacts.find(c => c.AccountId === info.Id);
      if (contact) {
        info.DisplayedName = contact.Name;
        info.IsInContacts = true;
      }

      info.isLoading = false;
      info.LastVisitTime*=1000;
      state.messengerData.viewedProfile = info;
    },

    msg_setAccountDisplayedName: function (state, { newDisplayedName }) {
      state.messengerData.CurrentAccount.DisplayedName = newDisplayedName;
    },

    msg_setAccountIsNumberHided: function (state, isNumberHided) {
      state.messengerData.CurrentAccount.IsNumberHided = isNumberHided;
    },

  },

  actions: {

    checkChatIsLoading: function ({ state, dispatch }) {
      if (state.messengerData.CurrentChat.IsLoading) {
        dispatch('smartphone/setTabOrBack', { key: 'MessengerTab', section: 'GeneralSection', subsection: 'GeneralSubsection', data: null }, { root: true });
        state.messengerData.CurrentChat.IsLoading = false;
      }
      else if (state.messengerData.viewedProfile.IsLoading) {
        dispatch('smartphone/setTabOrBack', { key: 'MessengerTab', section: 'GeneralSection', subsection: 'GeneralSubsection', data: null }, { root: true });
        state.messengerData.viewedProfile.IsLoading = false;
      }
    },

    msg_dropCurrentChat: function ({ state }) {
      state.messengerData.CurrentChat.Id = -1;
    },

    msg_setAdminsForCurrentChat: function ({ state }, admins) {
      const chatId = state.messengerData.CurrentChat.Id;
      const chat = state.messengerData.Chats.find(c => c.Id === chatId);
      if (!chat) {
        return;
      }

      state.messengerData.CurrentChat.Admins = admins;
      chat.Admins = admins;
    },

    msg_addAdminForChat: function ({ state }, { ChatId, AccountId }) {
      const subscriber = state.messengerData.SearchSubscribers.find(s => s.Id === AccountId);
      if (!subscriber) return;

      const chat = state.messengerData.Chats.find(c => c.Id === ChatId);
      if (!chat) return;

      subscriber.AdminLvl = 1;
      subscriber.Permissions = [];

      chat.Admins.push(subscriber);

      if (state.messengerData.CurrentChat.Id === ChatId) {
        state.messengerData.CurrentChat.Admins.push(subscriber);
      }
    },

    msg_deleteAdminForChat: function ({ state }, { ChatId, AdminId }) {
      const chat = state.messengerData.Chats.find(c => c.Id === ChatId);
      if (!chat) return;

      const adminIdx = chat.Admins.findIndex(a => a.Id === AdminId);
      if (adminIdx !== -1) {
        chat.Admins.splice(adminIdx, 1);
      }

      if (state.messengerData.CurrentChat.Id === ChatId) {
        state.messengerData.CurrentChat.Admins = chat.Admins;
      }
    },

    msg_setMyAdminPermissionsForChat: function ({ state }, { ChatId, AdminLvl, Permissions }) {
      const chat = state.messengerData.Chats.find(c => c.Id === ChatId);
      if (!chat) return;

      chat.Permissions = Permissions;
      chat.AdminLvl = AdminLvl;
    },

    msg_openChat: function ({ commit, state }, chatId) {
      const chat = state.messengerData.Chats.find(c => c.Id === chatId);

      const subsectionsByChatType = {
        0: 'PrivateSubsection',
        1: 'GroupSubsection',
        2: 'CanalSubsection'
      }

      commit('msg_setCurrentChat', chat);

      // IsAlreadyLoaded - у игрока уже подгружены последние сообщения в чате
      if (chat.IsAlreadyLoaded) {
        const messages = state.messengerData._cachedMessages.filter(m => m.ChatId === chatId);
        commit('msg_setCurrentChatMessages', messages);
      }
      else {
        window.mp.triggerServer('phone::msg::requestChat', chatId);
        commit('msg_setCurrentChatIsLoading', true);
      }

      commit('smartphone/setCurrentTab', {
        key: 'MessengerTab',
        section: 'ChatSection',
        subsection: subsectionsByChatType[chat.Type],
        data: null
      }, { root: true });

      if (chat.NonReadMessagesCount > 0) {
        window.mp.triggerServer('phone::msg::markAsRead', chatId);
      }
    },

    msg_openPrivateChat: function ({ dispatch, commit, state }, accountId) {
      const privateChat = state.messengerData.Chats.find(c => c.AccountId === accountId);
      if (privateChat) {
        dispatch('msg_openChat', privateChat.Id);
      }
      else {
        commit('msg_setCurrentChatIsLoading', true);
        commit('msg_setCurrentChat', {
          Id: -1,
          Name: 'Private Chat',
          IsOnline: false,
          Type: 0,
          AccountId: accountId
        });

        window.mp.triggerServer('phone::msg::createPrivateChat', accountId);
        commit('smartphone/setCurrentTab', {
          key: 'MessengerTab',
          section: 'ChatSection',
          subsection: 'PrivateSubsection',
          data: null
        }, { root: true });
      }
    },

    msg_loadInchatInfo: function ({ commit, state }, { ChatId, SubscribersCount,
      Messages, Permissions, AdminLvl, InviteCode
    }) {
      Messages.forEach(message => {
        message.Time *= 1000;
        message.ChatId = ChatId;
        commit('msg_cacheMessage', message);
      });

      const chat = state.messengerData.Chats.find(c => c.Id === ChatId);
      chat.SubscribersCount = SubscribersCount;
      chat.Permissions = Permissions;
      chat.IsAlreadyLoaded = true;
      chat.AdminLvl = AdminLvl;
      chat.InviteCode = InviteCode

      commit('msg_setCurrentChat', chat);
      commit('msg_setCurrentChatMessages', Messages);

      // Подгрузка незакешированных аккаунтов
      const sendersIds = [...new Set(Messages.map(m => m.SenderId))];
      const nonCachedAccountsIds = sendersIds
        .filter(accountId => !state.messengerData._cachedAccounts.find(a => a.Id === accountId));

      window.mp.triggerServer('phone::msg::requestAccountsNames', JSON.stringify(nonCachedAccountsIds));
    },

    msg_openAccount: function ({ commit, state }, accountId) {
      const contact = state.messengerData.Contacts.find(c => c.AccountId === accountId);
      if (contact) {
        console.log(`${accountId}`);
      }

      state.messengerData.viewedProfile.isLoading = true;
      window.mp.triggerServer('phone::msg::requestAccount', accountId);

      commit('smartphone/setCurrentTab', {
        key: 'MessengerTab',
        section: 'ViewedProfileSection',
        data: null
      }, { root: true });
    },

    msg_setAccountAndChatMuted: function ({ commit, state }, { ChatId, AccountId, IsMuted }) {
      commit('msg_setChatMuted', { ChatId, IsMuted });

      if (state.messengerData.viewedProfile.Id === AccountId) {
        state.messengerData.viewedProfile.IsMuted = IsMuted;
      }
    },

    msg_setAccountBlocked: function ({ state }, { AccountId, IsBlocked }) {
      if (state.messengerData.viewedProfile.Id === AccountId) {
        state.messengerData.viewedProfile.IsBlocked = IsBlocked;
      }
    },

    msg_removeContact: function ({ commit, state }, { ContactId, AccountDisplayedName }) {
      const contact = state.messengerData.Contacts.find(c => c.Id === ContactId);
      if (contact) {
        const privateChat = state.messengerData.Chats.find(c => c.AccountId == contact.AccountId);
        if (privateChat) {
          commit('msg_renameChat', { ChatId: privateChat.Id, Name: AccountDisplayedName });
        }

        if (state.messengerData.viewedProfile.Id === contact.AccountId) {
          state.messengerData.viewedProfile.DisplayedName = AccountDisplayedName;
          state.messengerData.viewedProfile.IsInContacts = false;
        }
      }

      commit('msg_removeStoreContact', ContactId);
    },

    msg_addContact: function ({ commit, state }, contact) {
      if (state.messengerData.viewedProfile.Id === contact.AccountId) {
        state.messengerData.viewedProfile.DisplayedName = contact.Name;
        state.messengerData.viewedProfile.IsInContacts = true;
      }

      commit('msg_loadContacts', [contact]);
    },

    msg_loadChat: function ({ commit, state }, chat) {
      if (!chat.SubscribersCount) {
        chat.SubscribersCount = 0;
      }

      commit('msg_loadChats', [chat]);

      if (state.messengerData.CurrentChat.Id === -1 || state.messengerData.CurrentChat.Id === chat.Id) {
        window.mp.triggerServer('phone::msg::requestChat', chat.Id);
      }
    },

    msg_createChat: function ({ commit }, { ChatType, Name, Description, IconColors, UsersToInvite }) {
      commit('msg_setCurrentChat', {
        Id: -1,
        Name: Name,
        IsOnline: false,
        Type: ChatType,
        IconColors: IconColors,
        SubscribersCount: 0
      });

      const subsectionsByChatType = {
        0: 'PrivateSubsection',
        1: 'GroupSubsection',
        2: 'CanalSubsection'
      }
      commit('smartphone/setCurrentTab', {
        key: 'MessengerTab',
        section: 'ChatSection',
        subsection: subsectionsByChatType[ChatType],
        data: null
      }, { root: true });

      commit('msg_setCurrentChatIsLoading', true);
      if (ChatType === 1) {
        window.mp.triggerServer('phone::msg::createGroupChat', Name, Description, IconColors, JSON.stringify(UsersToInvite));
      }
      else if (ChatType === 2) {
        window.mp.triggerServer('phone::msg::createChannel', Name, Description, IconColors);
      }
    },

    msg_sendMessage({ commit, state, dispatch }, { ChatId, Message }) {
      Message.ChatId = ChatId;
      Message.Time *= 1000;
      commit('msg_cacheMessage', Message);
      commit('msg_setLastMessage', Message);

      if (!state.messengerData._cachedAccounts.find(a => a.Id === Message.SenderId)) {
        window.mp.triggerServer('phone::msg::requestAccountsNames', `[${Message.SenderId}]`);
      }

      if (state.messengerData.CurrentChat.Id === ChatId) {
        state.messengerData.CurrentChat.Items.push(Message);

        if (Message.SenderId !== state.messengerData.CurrentAccount.Id) {
          window.mp.triggerServer('phone::msg::markAsRead', ChatId);
        }
      }
      else if (Message.SenderId !== state.messengerData.CurrentAccount.Id) {
        const chat = state.messengerData.Chats.find(c => c.Id === ChatId);
        if (chat) {
          chat.NonReadMessagesCount++;
          if (!chat.IsMuted) {
            dispatch('smartphone/sounds_playNotification', { root: true });
          }
        }
      }
    },

    msg_deleteMessage({ commit, state }, MessageId) {
      commit('msg_uncacheMessage', MessageId);

      if (state.messengerData.CurrentChat.Items) {
        const msgIdx = state.messengerData.CurrentChat.Items.findIndex(m => m.Id === MessageId);
        if (msgIdx !== -1) {
          state.messengerData.CurrentChat.Items.splice(msgIdx, 1);
        }
      }
    },

    msg_editMessage({ commit, state }, { ChatId, Message }) {
      Message.ChatId = ChatId;
      Message.Time *= 1000;
      commit('msg_cacheMessage', Message);

      if (state.messengerData.CurrentChat.Id === ChatId) {
        const msgIdx = state.messengerData.CurrentChat.Items.findIndex(m => m.Id === Message.Id);
        if (msgIdx !== -1) {
          state.messengerData.CurrentChat.Items.splice(msgIdx, 1);
          state.messengerData.CurrentChat.Items.push(Message);
        }
      }
    },

    msg_deleteChat({ state, commit }, chatId) {
      commit('msg_removeChat', chatId);

      if (state.messengerData.CurrentChat.Id === chatId) {
        commit('smartphone/setCurrentTab', {
          key: 'MessengerTab',
          section: 'GeneralSection',
          subsection: 'GeneralSubsection',
          data: null
        }, { root: true });
      }
    },

    msg_setInChatBlocked({ state, dispatch }, { ChatId, IsBlocked }) {
      if (state.messengerData.CurrentChat.Id === ChatId) {
        dispatch('smartphone/msg_goToChats', true, { root: true });
      }

      if (IsBlocked) {
        const chatIdx = state.messengerData.Chats.findIndex(c => c.Id === ChatId);
        if (chatIdx !== -1) {
          state.messengerData.Chats.splice(chatIdx, 1);
        }
      }
    },
  }
}
