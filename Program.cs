   using System;
using System.Net;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;

namespace mobidick
{

public class KelimeVeAdetler{
	public string kelime {get;set;}
	public int adet{get;set;}
					}
public class Program
{
	public static void Main()
	{
		WebClient wc=new WebClient();
		string[] kelimeDizisi=HtmlToPlainText(wc.DownloadString("http://www.gutenberg.org/files/60547/60547-h/60547-h.htm")).Split();
		List<KelimeVeAdetler> kelimeler =new List<KelimeVeAdetler>();
		foreach(string kelime in kelimeDizisi){
		KelimeVeAdetler varmi=kelimeler.Find(p=>p.kelime.Equals(kelime));
			 if(varmi!=null)
				varmi.adet++;
			else 
				kelimeler.Add(new KelimeVeAdetler{adet=1,kelime=kelime});
		}
		
        string xmlText=@"<?xml version=""1.0 encoding=UTF-8""?>";
         xmlText+="<words>";
		 foreach(KelimeVeAdetler kelime in kelimeler){
		 xmlText+="<word text=";
         xmlText+='"';
         xmlText+=kelime.kelime;
         xmlText+='"';
         xmlText+=" count=";
         xmlText+='"';
         xmlText+=kelime.adet;
         xmlText+='"';
         xmlText+="/>";
		 }
        string dosyaYolu=System.IO.Directory.GetCurrentDirectory();
         xmlText+="</words>";
         dosyaYolu+=@"\metin.xml";
      if (File.Exists(dosyaYolu)) {
          File.Delete(dosyaYolu);
      }  
        using(System.IO.StreamWriter file = 
            new System.IO.StreamWriter(@dosyaYolu, true)){
                file.WriteLine(xmlText);
         }
	}
		
	

	
	private static string HtmlToPlainText(string html)
{
    const string tagWhiteSpace = @"(>|$)(\W|\n|\r)+<";
    const string stripFormatting = @"<[^>]*(>|$)";
    const string lineBreak = @"<(br|BR)\s{0,1}\/{0,1}>";
    var lineBreakRegex = new Regex(lineBreak, RegexOptions.Multiline);
    var stripFormattingRegex = new Regex(stripFormatting, RegexOptions.Multiline);
    var tagWhiteSpaceRegex = new Regex(tagWhiteSpace, RegexOptions.Multiline);

    var text = html;
    text = System.Net.WebUtility.HtmlDecode(text); 
    text = tagWhiteSpaceRegex.Replace(text, "><");
    text = lineBreakRegex.Replace(text, Environment.NewLine);
    text = stripFormattingRegex.Replace(text, string.Empty);

    return text;
}
}
}

