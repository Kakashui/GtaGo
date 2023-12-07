using System.ComponentModel.DataAnnotations;

namespace ServerGo.Casino.Business
{
    public enum CashBoxState
    {
        [Display(Name= "Под угрозой изъятия")]
        UnderThreat,
        
        [Display(Name= "В безопасности")]
        Safe
    }
}