
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartTrader.Core.Models {

public abstract class BaseModel {

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id {get; set;}

    [Timestamp]
    public byte[] TimeStamp {get;set;}
    
}
}