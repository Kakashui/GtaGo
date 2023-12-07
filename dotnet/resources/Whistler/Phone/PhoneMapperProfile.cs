using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Domain.Phone.Contacts;
using Whistler.Domain.Phone.Messenger;
using Whistler.Phone.Calls.Dtos;
using Whistler.Phone.Contacts.Dtos;
using Whistler.Phone.Messenger.Dtos;
using Whistler.Helpers;
using Whistler.Phone.Taxi.Service;
using Whistler.Phone.Taxi.Dtos;
using Whistler.Phone.Taxi.Service.Models;

namespace Whistler.Phone
{
    public class PhoneMapperProfile : Profile
    {
        public PhoneMapperProfile()
        {
            #region Contacts
            CreateMap<Contact, ContactDto>()
                .ForMember(dto => dto.Id, dto => dto.MapFrom(source => source.Id))
                .ForMember(dto => dto.Number, dto => dto.MapFrom(source => source.TargetNumber));

            CreateMap<Block, Contacts.Dtos.BlockDto>();
            #endregion

            #region Messenger
            CreateMap<MsgContact, MsgContactDto>()
                .ForMember(dto => dto.Id, dto => dto.MapFrom(source => source.ContactId))
                .ForMember(dto => dto.AccountId, dto => dto.MapFrom(source => source.TargetAccountId))
                .AfterMap((source, dto) =>
                {
                    dto.Time = (source.TargetAccount?.LastVisit ?? source.TargetAccount?.CreatedAt).GetTotalSeconds();
                    dto.IsOnline = Main.GetExtPlayerByPredicate(player => (player.Character.PhoneTemporary?.Phone?.Account?.Id ?? 0) == source.TargetAccountId) != null;
                });

            CreateMap<Message, MessageDto>()
                .Include<Post, PostDto>()
                .ForMember(dto => dto.Time, dto => dto.MapFrom(source => source.CreatedAt.GetTotalSeconds()))
                .ForMember(dto => dto.Attachment, dto => dto.MapFrom(source => source.Attachments.Count == 0 ? null : source.Attachments[0]));

            CreateMap<Post, PostDto>()
                .ForMember(dto => dto.ImgUrl, dto => dto.MapFrom(source => source.Photo));

            CreateMap<Account, AccountDto>();

            CreateMap<Account, FullAccountDto>()
                .ForMember(dto => dto.Number, dto => dto.MapFrom(source => source.SimCard.Number))
                .AfterMap((source, dto) =>
                {
                    dto.LastVisitTime = (source.LastVisit ?? source.CreatedAt).GetTotalSeconds();
                    dto.IsBlocked = false;
                    dto.IsMuted = false;
                    dto.IsOnline = Main.GetExtPlayerByPredicate(player => (player.Character.PhoneTemporary?.Phone?.Account?.Id ?? 0) == source.Id) != null;
                });

            CreateMap<Account, SubscriberDto>()
                .AfterMap((source, dto) =>
                {
                    dto.LastVisitTime = (source.LastVisit ?? source.CreatedAt).GetTotalSeconds();
                    dto.IsOnline = Main.GetExtPlayerByPredicate(player => (player.Character.PhoneTemporary?.Phone?.Account?.Id ?? 0) == source.Id) != null;
                });

            CreateMap<AccountToChat, AdminDto>()
                .ForMember(dto => dto.Id, dto => dto.MapFrom(source => source.AccountId))
                .ForMember(dto => dto.DisplayedName, dto => dto.MapFrom(source => source.Account.DisplayedName))
                .ForMember(dto => dto.IsOnline, dto => dto.MapFrom(source => source.Account.LastVisit == null));

            CreateMap<AccountToChat, Messenger.Dtos.BlockDto>()
                .ForMember(dto => dto.DisplayedName, dto => dto.MapFrom(source => source.Account.DisplayedName))
                .ForMember(dto => dto.BlockedBy, dto => dto.MapFrom(source => source.BlockedBy.DisplayedName));
            #endregion

            #region Taxi
            CreateMap<TaxiOrderBase, TaxiOrderDto>();
            #endregion
        }
    }
}
