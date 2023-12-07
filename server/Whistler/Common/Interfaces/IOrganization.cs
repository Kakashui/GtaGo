using System;
using System.Collections.Generic;
using System.Text;
using Whistler.MoneySystem.DTO;

namespace Whistler.Common.Interfaces
{
    interface IOrganization
    {
        public int Id { get; }
        internal string GetName();
        internal OrganizationType TypeOrganization { get; }
        public OrgActivityType OrgActiveType { get; }
        public List<OrgPaymentDTO> PaymentHystory { get; set; }
        public List<OrgPaymentDTO> GetMemberPayments();
        public void UpdatePayments(OrgPaymentDTO payment);
    }
}
