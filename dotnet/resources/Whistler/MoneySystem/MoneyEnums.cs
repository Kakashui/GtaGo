using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.MoneySystem
{
    public enum PaymentsType
    {
        Cash = 0,
        Card = 1,
        Gov = 2,
        Media = 3,
    }
    public enum DepositTypes
    {
        Default,
        BlessAndSave,
        Profitable,
        Universal,

    }
    public enum TypeOfInterestCalculation
    {

    }
    public enum TypeMoneyAcc
    {
        Server = 0,
        Player = 1,
        BankAccount = 2,
        Business = 3,
        Fraction = 4,
        Family = 5,
        Admin = 6,
        Deposit = 7,
    }

    public enum TypeBankAccount
    {
        invalid = -1,
        Player = 1,
        House,
        BusinessNalog,
        Business,
        Family,
        Fraction,
        Phone,
    }

    public enum CreditTypePayment
    {
        Annuity,
        Differentiated,
    }

    public enum PropertyType
    {
        Invalid = -1,
        Vehicle,
        Business,
        House,
    }
}
