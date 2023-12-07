using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Newtonsoft.Json;
using Whistler.SDK;

namespace Whistler.Businesses.Manager
{
    public class BusinessTypeModel
    {
        public int BizType { get; set; }
        public List<ProductSettings> Products { get; set; }
        public string TypeName { get; set; }
        public int BlipType { get; set; }
        public int BlipColor { get; set; }
        public int MinimumPercentProduct { get; set; }
        public BusinessTypeModel(DataRow row)
        {
            BizType = Convert.ToInt32(row["biztype"]);
            Products = JsonConvert.DeserializeObject<List<ProductSettings>>(row["settings"].ToString());
            TypeName = row["name"].ToString();
            BlipType = Convert.ToInt32(row["bliptype"]);
            BlipColor = Convert.ToInt32(row["blipcolor"]);
            MinimumPercentProduct = Convert.ToInt32(row["minimumpercentproduct"]);
        }

        public void ChangeBlipType(int blipType)
        {
            BlipType = blipType;
            MySQL.Query("UPDATE bizsettings SET bliptype = @prop0 WHERE biztype = @prop1", BlipType, BizType);
        }

        public void ChangeBlipColor(int blipColor)
        {
            BlipColor = blipColor;
            MySQL.Query("UPDATE bizsettings SET blipcolor = @prop0 WHERE biztype = @prop1", BlipColor, BizType);
        }

        public void ChangeTypeName(string name)
        {
            TypeName = name;
            MySQL.Query("UPDATE bizsettings SET name = @prop0 WHERE biztype = @prop1", TypeName, BizType);
        }

        public void ChangeMinimumPercentProduct(int minimumPercentProduct)
        {
            MinimumPercentProduct = minimumPercentProduct;
            MySQL.Query("UPDATE bizsettings SET minimumpercentproduct = @prop0 WHERE biztype = @prop1", MinimumPercentProduct, BizType);
        }
    }
}
