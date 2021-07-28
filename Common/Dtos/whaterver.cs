namespace API.Dtos
{
    using System;
    using System.Collections;
using System.Collections.Generic;
    using System.Text;

    public class ListFilterer
{
   public static IEnumerable<int> GetIntegersFromList(List<object> listOfItems)
   {
     var newlist = new List<int>();
      foreach(var item in listOfItems)
        {
        if(Type.GetTypeCode(item.GetType())==TypeCode.Int32){
          newlist.Add(Convert.ToInt32(item));
        }
      }
     return newlist;
   }

   //65-90 97-122
   
  
   public static string AlphabetPosition(string text)
  {
      byte[] asciiBytes = Encoding.ASCII.GetBytes(text);
      var result = new List<byte>();
    foreach(var c in asciiBytes){
      if(c>=65 && c<=90 || c>=97 && c<=122){
          result.Add(c);
      }
    }
    return result.ToString();
  }
  

}
}