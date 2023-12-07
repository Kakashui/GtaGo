using System.ComponentModel.DataAnnotations;

namespace ServerGo.Casino.Business
{
    public enum CashBoxState
    {
        [Display(Name= "cbs:underthread")]
        UnderThreat,
        
        [Display(Name= "cbs:safe")]
        Safe
    }
}