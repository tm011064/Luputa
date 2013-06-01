using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Web;

namespace CommonTools.Components.Localization
{
    /// <summary>
    /// This class contains time zone utility methods. It stores the following time zone definitions:
    /// 
    /// Morocco Standard Time,
    /// GMT Standard Time,
    /// Greenwich Standard Time,
    /// W. Europe Standard Time,
    /// Central Europe Standard Time,
    /// Romance Standard Time,
    /// Central European Standard Time,
    /// W. Central Africa Standard Time,
    /// Jordan Standard Time,
    /// GTB Standard Time,
    /// Middle East Standard Time,
    /// Egypt Standard Time,
    /// South Africa Standard Time,
    /// FLE Standard Time,
    /// Israel Standard Time,
    /// E. Europe Standard Time,
    /// Namibia Standard Time,
    /// Arabic Standard Time,
    /// Arab Standard Time,
    /// Russian Standard Time,
    /// E. Africa Standard Time,
    /// Georgian Standard Time,
    /// Iran Standard Time,
    /// Arabian Standard Time,
    /// Azerbaijan Standard Time,
    /// Caucasus Standard Time,
    /// Mauritius Standard Time,
    /// Armenian Standard Time,
    /// Afghanistan Standard Time,
    /// Ekaterinburg Standard Time,
    /// Pakistan Standard Time,
    /// West Asia Standard Time,
    /// India Standard Time,
    /// Sri Lanka Standard Time,
    /// Nepal Standard Time,
    /// N. Central Asia Standard Time,
    /// Central Asia Standard Time,
    /// Myanmar Standard Time,
    /// SE Asia Standard Time,
    /// North Asia Standard Time,
    /// China Standard Time,
    /// North Asia East Standard Time,
    /// Singapore Standard Time,
    /// W. Australia Standard Time,
    /// Taipei Standard Time,
    /// Tokyo Standard Time,
    /// Korea Standard Time,
    /// Yakutsk Standard Time,
    /// Cen. Australia Standard Time,
    /// AUS Central Standard Time,
    /// E. Australia Standard Time,
    /// AUS Eastern Standard Time,
    /// West Pacific Standard Time,
    /// Tasmania Standard Time,
    /// Vladivostok Standard Time,
    /// Central Pacific Standard Time,
    /// New Zealand Standard Time,
    /// Fiji Standard Time,
    /// Tonga Standard Time,
    /// Azores Standard Time,
    /// Cape Verde Standard Time,
    /// Mid-Atlantic Standard Time,
    /// E. South America Standard Time,
    /// Argentina Standard Time,
    /// SA Eastern Standard Time,
    /// Greenland Standard Time,
    /// Montevideo Standard Time,
    /// Newfoundland Standard Time,
    /// Atlantic Standard Time,
    /// SA Western Standard Time,
    /// Central Brazilian Standard Time,
    /// Pacific SA Standard Time,
    /// Venezuela Standard Time,
    /// SA Pacific Standard Time,
    /// Eastern Standard Time,
    /// US Eastern Standard Time,
    /// Central America Standard Time,
    /// Central Standard Time,
    /// Central Standard Time (Mexico),
    /// Mexico Standard Time,
    /// Canada Central Standard Time,
    /// US Mountain Standard Time,
    /// Mountain Standard Time (Mexico),
    /// Mexico Standard Time 2,
    /// Mountain Standard Time,
    /// Pacific Standard Time,
    /// Pacific Standard Time (Mexico),
    /// Alaskan Standard Time,
    /// Hawaiian Standard Time,
    /// Samoa Standard Time,
    /// Dateline Standard Time,
    /// UTC
    /// </summary>
    public class TimeZoneUtility
    {
        #region members
        /// <summary>
        /// 
        /// </summary>
        protected static Dictionary<string, TimeZoneInfo> _TimeZoneInfos = new Dictionary<string, TimeZoneInfo>()
        {
            { "Morocco Standard Time", TimeZoneInfo.FromSerializedString("Morocco Standard Time;0;(GMT) Casablanca;Morocco Standard Time;Morocco Daylight Time;[01:01:2008;12:31:2008;60;[0;23:59:59.999;5;5;6;];[0;23:59:59.999;8;5;0;];];") },
            { "GMT Standard Time", TimeZoneInfo.FromSerializedString("GMT Standard Time;0;(GMT) Greenwich Mean Time : Dublin, Edinburgh, Lisbon, London;GMT Standard Time;GMT Daylight Time;[01:01:0001;12:31:9999;60;[0;01:00:00;3;5;0;];[0;02:00:00;10;5;0;];];") },
            { "Greenwich Standard Time", TimeZoneInfo.FromSerializedString("Greenwich Standard Time;0;(GMT) Monrovia, Reykjavik;Greenwich Standard Time;Greenwich Daylight Time;;") },
            { "W. Europe Standard Time", TimeZoneInfo.FromSerializedString("W. Europe Standard Time;60;(GMT+01:00) Amsterdam, Berlin, Bern, Rome, Stockholm, Vienna;W. Europe Standard Time;W. Europe Daylight Time;[01:01:0001;12:31:9999;60;[0;02:00:00;3;5;0;];[0;03:00:00;10;5;0;];];") },
            { "Central Europe Standard Time", TimeZoneInfo.FromSerializedString("Central Europe Standard Time;60;(GMT+01:00) Belgrade, Bratislava, Budapest, Ljubljana, Prague;Central Europe Standard Time;Central Europe Daylight Time;[01:01:0001;12:31:9999;60;[0;02:00:00;3;5;0;];[0;03:00:00;10;5;0;];];") },
            { "Romance Standard Time", TimeZoneInfo.FromSerializedString("Romance Standard Time;60;(GMT+01:00) Brussels, Copenhagen, Madrid, Paris;Romance Standard Time;Romance Daylight Time;[01:01:0001;12:31:9999;60;[0;02:00:00;3;5;0;];[0;03:00:00;10;5;0;];];") },
            { "Central European Standard Time", TimeZoneInfo.FromSerializedString("Central European Standard Time;60;(GMT+01:00) Sarajevo, Skopje, Warsaw, Zagreb;Central European Standard Time;Central European Daylight Time;[01:01:0001;12:31:9999;60;[0;02:00:00;3;5;0;];[0;03:00:00;10;5;0;];];") },
            { "W. Central Africa Standard Time", TimeZoneInfo.FromSerializedString("W. Central Africa Standard Time;60;(GMT+01:00) West Central Africa;W. Central Africa Standard Time;W. Central Africa Daylight Time;;") },
            { "Jordan Standard Time", TimeZoneInfo.FromSerializedString("Jordan Standard Time;120;(GMT+02:00) Amman;Jordan Standard Time;Jordan Daylight Time;[01:01:0001;12:31:2006;60;[0;00:00:00;3;5;4;];[0;01:00:00;9;5;5;];][01:01:2007;12:31:9999;60;[0;23:59:59.999;3;5;4;];[0;01:00:00;10;5;5;];];") },
            { "GTB Standard Time", TimeZoneInfo.FromSerializedString("GTB Standard Time;120;(GMT+02:00) Athens, Bucharest, Istanbul;GTB Standard Time;GTB Daylight Time;[01:01:0001;12:31:9999;60;[0;03:00:00;3;5;0;];[0;04:00:00;10;5;0;];];") },
            { "Middle East Standard Time", TimeZoneInfo.FromSerializedString("Middle East Standard Time;120;(GMT+02:00) Beirut;Middle East Standard Time;Middle East Daylight Time;[01:01:0001;12:31:9999;60;[0;00:00:00;3;5;0;];[0;23:59:59.999;10;5;6;];];") },
            { "Egypt Standard Time", TimeZoneInfo.FromSerializedString("Egypt Standard Time;120;(GMT+02:00) Cairo;Egypt Standard Time;Egypt Daylight Time;[01:01:0001;12:31:2005;60;[0;00:00:00;4;5;5;];[0;23:59:59.999;9;5;4;];][01:01:2006;12:31:2006;60;[0;00:00:00;4;5;5;];[0;23:59:59.999;9;3;4;];][01:01:2007;12:31:2007;60;[0;23:59:59.999;4;5;4;];[0;23:59:59.999;9;1;4;];][01:01:2008;12:31:2008;60;[0;23:59:59.999;4;5;4;];[0;23:59:59.999;8;5;4;];][01:01:2009;12:31:2009;60;[0;23:59:59.999;4;4;4;];[0;23:59:59.999;9;5;4;];][01:01:2010;12:31:9999;60;[0;23:59:59.999;4;5;4;];[0;23:59:59.999;9;5;4;];];") },
            { "South Africa Standard Time", TimeZoneInfo.FromSerializedString("South Africa Standard Time;120;(GMT+02:00) Harare, Pretoria;South Africa Standard Time;South Africa Daylight Time;;") },
            { "FLE Standard Time", TimeZoneInfo.FromSerializedString("FLE Standard Time;120;(GMT+02:00) Helsinki, Kyiv, Riga, Sofia, Tallinn, Vilnius;FLE Standard Time;FLE Daylight Time;[01:01:0001;12:31:9999;60;[0;03:00:00;3;5;0;];[0;04:00:00;10;5;0;];];") },
            { "Israel Standard Time", TimeZoneInfo.FromSerializedString("Israel Standard Time;120;(GMT+02:00) Jerusalem;Jerusalem Standard Time;Jerusalem Daylight Time;[01:01:2005;12:31:2005;60;[0;02:00:00;4;1;5;];[0;02:00:00;10;2;0;];][01:01:2006;12:31:2006;60;[0;02:00:00;3;5;5;];[0;02:00:00;10;1;0;];][01:01:2007;12:31:2007;60;[0;02:00:00;3;5;5;];[0;02:00:00;9;3;0;];][01:01:2008;12:31:2008;60;[0;02:00:00;3;5;5;];[0;02:00:00;10;1;0;];][01:01:2009;12:31:2009;60;[0;02:00:00;3;5;5;];[0;02:00:00;9;5;0;];][01:01:2010;12:31:2010;60;[0;02:00:00;3;5;5;];[0;02:00:00;9;2;0;];][01:01:2011;12:31:2011;60;[0;02:00:00;4;1;5;];[0;02:00:00;10;1;0;];][01:01:2012;12:31:2012;60;[0;02:00:00;3;5;5;];[0;02:00:00;9;4;0;];][01:01:2013;12:31:2013;60;[0;02:00:00;3;5;5;];[0;02:00:00;9;2;0;];][01:01:2014;12:31:2014;60;[0;02:00:00;3;5;5;];[0;02:00:00;9;4;0;];][01:01:2015;12:31:2015;60;[0;02:00:00;3;5;5;];[0;02:00:00;9;3;0;];][01:01:2016;12:31:2016;60;[0;02:00:00;4;1;5;];[0;02:00:00;10;2;0;];][01:01:2017;12:31:2017;60;[0;02:00:00;3;5;5;];[0;02:00:00;9;4;0;];][01:01:2018;12:31:2018;60;[0;02:00:00;3;5;5;];[0;02:00:00;9;3;0;];][01:01:2019;12:31:2019;60;[0;02:00:00;3;5;5;];[0;02:00:00;10;1;0;];][01:01:2020;12:31:2020;60;[0;02:00:00;3;5;5;];[0;02:00:00;9;4;0;];][01:01:2021;12:31:2021;60;[0;02:00:00;3;5;5;];[0;02:00:00;9;2;0;];][01:01:2022;12:31:2022;60;[0;02:00:00;4;1;5;];[0;02:00:00;10;1;0;];];") },
            { "E. Europe Standard Time", TimeZoneInfo.FromSerializedString("E. Europe Standard Time;120;(GMT+02:00) Minsk;E. Europe Standard Time;E. Europe Daylight Time;[01:01:0001;12:31:9999;60;[0;02:00:00;3;5;0;];[0;03:00:00;10;5;0;];];") },
            { "Namibia Standard Time", TimeZoneInfo.FromSerializedString("Namibia Standard Time;120;(GMT+02:00) Windhoek;Namibia Standard Time;Namibia Daylight Time;[01:01:0001;12:31:9999;-60;[0;02:00:00;4;1;0;];[0;02:00:00;9;1;0;];];") },
            { "Arabic Standard Time", TimeZoneInfo.FromSerializedString("Arabic Standard Time;180;(GMT+03:00) Baghdad;Arabic Standard Time;Arabic Daylight Time;[01:01:0001;12:31:2006;60;[0;03:00:00;4;1;0;];[0;04:00:00;10;1;0;];][01:01:2007;12:31:2007;60;[0;03:00:00;4;1;0;];[0;04:00:00;10;1;1;];];") },
            { "Arab Standard Time", TimeZoneInfo.FromSerializedString("Arab Standard Time;180;(GMT+03:00) Kuwait, Riyadh;Arab Standard Time;Arab Daylight Time;;") },
            { "Russian Standard Time", TimeZoneInfo.FromSerializedString("Russian Standard Time;180;(GMT+03:00) Moscow, St. Petersburg, Volgograd;Russian Standard Time;Russian Daylight Time;[01:01:0001;12:31:9999;60;[0;02:00:00;3;5;0;];[0;03:00:00;10;5;0;];];") },
            { "E. Africa Standard Time", TimeZoneInfo.FromSerializedString("E. Africa Standard Time;180;(GMT+03:00) Nairobi;E. Africa Standard Time;E. Africa Daylight Time;;") },
            { "Georgian Standard Time", TimeZoneInfo.FromSerializedString("Georgian Standard Time;180;(GMT+03:00) Tbilisi;Georgian Standard Time;Georgian Daylight Time;;") },
            { "Iran Standard Time", TimeZoneInfo.FromSerializedString("Iran Standard Time;210;(GMT+03:30) Tehran;Iran Standard Time;Iran Daylight Time;[01:01:0001;12:31:2005;60;[0;02:00:00;3;1;0;];[0;02:00:00;9;4;2;];][01:01:2008;12:31:2008;60;[0;23:59:59.999;3;3;4;];[0;23:59:59.999;9;3;6;];][01:01:2009;12:31:9999;60;[0;23:59:59.999;3;3;6;];[0;23:59:59.999;9;3;1;];];") },
            { "Arabian Standard Time", TimeZoneInfo.FromSerializedString("Arabian Standard Time;240;(GMT+04:00) Abu Dhabi, Muscat;Arabian Standard Time;Arabian Daylight Time;;") },
            { "Azerbaijan Standard Time", TimeZoneInfo.FromSerializedString("Azerbaijan Standard Time;240;(GMT+04:00) Baku;Azerbaijan Standard Time;Azerbaijan Daylight Time;[01:01:0001;12:31:9999;60;[0;04:00:00;3;5;0;];[0;05:00:00;10;5;0;];];") },
            { "Caucasus Standard Time", TimeZoneInfo.FromSerializedString("Caucasus Standard Time;240;(GMT+04:00) Caucasus Standard Time;Caucasus Standard Time;Caucasus Daylight Time;;") },
            { "Mauritius Standard Time", TimeZoneInfo.FromSerializedString("Mauritius Standard Time;240;(GMT+04:00) Port Louis;Mauritius Standard Time;Mauritius Daylight Time;[01:01:2008;12:31:2008;60;[0;02:00:00;10;5;0;];[0;00:00:00;1;1;2;];][01:01:2009;12:31:9999;60;[0;02:00:00;10;5;0;];[0;02:00:00;3;5;0;];];") },
            { "Armenian Standard Time", TimeZoneInfo.FromSerializedString("Armenian Standard Time;240;(GMT+04:00) Yerevan;Armenian Standard Time;Armenian Daylight Time;[01:01:0001;12:31:9999;60;[0;02:00:00;3;5;0;];[0;03:00:00;10;5;0;];];") },
            { "Afghanistan Standard Time", TimeZoneInfo.FromSerializedString("Afghanistan Standard Time;270;(GMT+04:30) Kabul;Afghanistan Standard Time;Afghanistan Daylight Time;;") },
            { "Ekaterinburg Standard Time", TimeZoneInfo.FromSerializedString("Ekaterinburg Standard Time;300;(GMT+05:00) Ekaterinburg;Ekaterinburg Standard Time;Ekaterinburg Daylight Time;[01:01:0001;12:31:9999;60;[0;02:00:00;3;5;0;];[0;03:00:00;10;5;0;];];") },
            { "Pakistan Standard Time", TimeZoneInfo.FromSerializedString("Pakistan Standard Time;300;(GMT+05:00) Islamabad, Karachi;Pakistan Standard Time;Pakistan Daylight Time;[01:01:2008;12:31:2008;60;[0;23:59:59.999;5;5;6;];[0;23:59:59.999;10;5;5;];];") },
            { "West Asia Standard Time", TimeZoneInfo.FromSerializedString("West Asia Standard Time;300;(GMT+05:00) Tashkent;West Asia Standard Time;West Asia Daylight Time;;") },
            { "India Standard Time", TimeZoneInfo.FromSerializedString("India Standard Time;330;(GMT+05:30) Chennai, Kolkata, Mumbai, New Delhi;India Standard Time;India Daylight Time;;") },
            { "Sri Lanka Standard Time", TimeZoneInfo.FromSerializedString("Sri Lanka Standard Time;330;(GMT+05:30) Sri Jayawardenepura;Sri Lanka Standard Time;Sri Lanka Daylight Time;;") },
            { "Nepal Standard Time", TimeZoneInfo.FromSerializedString("Nepal Standard Time;345;(GMT+05:45) Kathmandu;Nepal Standard Time;Nepal Daylight Time;;") },
            { "N. Central Asia Standard Time", TimeZoneInfo.FromSerializedString("N. Central Asia Standard Time;360;(GMT+06:00) Almaty, Novosibirsk;N. Central Asia Standard Time;N. Central Asia Daylight Time;[01:01:0001;12:31:9999;60;[0;02:00:00;3;5;0;];[0;03:00:00;10;5;0;];];") },
            { "Central Asia Standard Time", TimeZoneInfo.FromSerializedString("Central Asia Standard Time;360;(GMT+06:00) Astana, Dhaka;Central Asia Standard Time;Central Asia Daylight Time;;") },
            { "Myanmar Standard Time", TimeZoneInfo.FromSerializedString("Myanmar Standard Time;390;(GMT+06:30) Yangon (Rangoon);Myanmar Standard Time;Myanmar Daylight Time;;") },
            { "SE Asia Standard Time", TimeZoneInfo.FromSerializedString("SE Asia Standard Time;420;(GMT+07:00) Bangkok, Hanoi, Jakarta;SE Asia Standard Time;SE Asia Daylight Time;;") },
            { "North Asia Standard Time", TimeZoneInfo.FromSerializedString("North Asia Standard Time;420;(GMT+07:00) Krasnoyarsk;North Asia Standard Time;North Asia Daylight Time;[01:01:0001;12:31:9999;60;[0;02:00:00;3;5;0;];[0;03:00:00;10;5;0;];];") },
            { "China Standard Time", TimeZoneInfo.FromSerializedString("China Standard Time;480;(GMT+08:00) Beijing, Chongqing, Hong Kong, Urumqi;China Standard Time;China Daylight Time;;") },
            { "North Asia East Standard Time", TimeZoneInfo.FromSerializedString("North Asia East Standard Time;480;(GMT+08:00) Irkutsk, Ulaan Bataar;North Asia East Standard Time;North Asia East Daylight Time;[01:01:0001;12:31:9999;60;[0;02:00:00;3;5;0;];[0;03:00:00;10;5;0;];];") },
            { "Singapore Standard Time", TimeZoneInfo.FromSerializedString("Singapore Standard Time;480;(GMT+08:00) Kuala Lumpur, Singapore;Malay Peninsula Standard Time;Malay Peninsula Daylight Time;;") },
            { "W. Australia Standard Time", TimeZoneInfo.FromSerializedString("W. Australia Standard Time;480;(GMT+08:00) Perth;W. Australia Standard Time;W. Australia Daylight Time;[01:01:2006;12:31:2006;60;[1;02:00:00;12;1;];[1;00:00:00;1;1;];][01:01:2007;12:31:9999;60;[0;02:00:00;10;5;0;];[0;03:00:00;3;5;0;];];") },
            { "Taipei Standard Time", TimeZoneInfo.FromSerializedString("Taipei Standard Time;480;(GMT+08:00) Taipei;Taipei Standard Time;Taipei Daylight Time;;") },
            { "Tokyo Standard Time", TimeZoneInfo.FromSerializedString("Tokyo Standard Time;540;(GMT+09:00) Osaka, Sapporo, Tokyo;Tokyo Standard Time;Tokyo Daylight Time;;") },
            { "Korea Standard Time", TimeZoneInfo.FromSerializedString("Korea Standard Time;540;(GMT+09:00) Seoul;Korea Standard Time;Korea Daylight Time;;") },
            { "Yakutsk Standard Time", TimeZoneInfo.FromSerializedString("Yakutsk Standard Time;540;(GMT+09:00) Yakutsk;Yakutsk Standard Time;Yakutsk Daylight Time;[01:01:0001;12:31:9999;60;[0;02:00:00;3;5;0;];[0;03:00:00;10;5;0;];];") },
            { "Cen. Australia Standard Time", TimeZoneInfo.FromSerializedString("Cen. Australia Standard Time;570;(GMT+09:30) Adelaide;Cen. Australia Standard Time;Cen. Australia Daylight Time;[01:01:0001;12:31:2007;60;[0;02:00:00;10;5;0;];[0;03:00:00;3;5;0;];][01:01:2008;12:31:9999;60;[0;02:00:00;10;1;0;];[0;03:00:00;4;1;0;];];") },
            { "AUS Central Standard Time", TimeZoneInfo.FromSerializedString("AUS Central Standard Time;570;(GMT+09:30) Darwin;AUS Central Standard Time;AUS Central Daylight Time;;") },
            { "E. Australia Standard Time", TimeZoneInfo.FromSerializedString("E. Australia Standard Time;600;(GMT+10:00) Brisbane;E. Australia Standard Time;E. Australia Daylight Time;;") },
            { "AUS Eastern Standard Time", TimeZoneInfo.FromSerializedString("AUS Eastern Standard Time;600;(GMT+10:00) Canberra, Melbourne, Sydney;AUS Eastern Standard Time;AUS Eastern Daylight Time;[01:01:0001;12:31:2007;60;[0;02:00:00;10;5;0;];[0;03:00:00;3;5;0;];][01:01:2008;12:31:9999;60;[0;02:00:00;10;1;0;];[0;03:00:00;4;1;0;];];") },
            { "West Pacific Standard Time", TimeZoneInfo.FromSerializedString("West Pacific Standard Time;600;(GMT+10:00) Guam, Port Moresby;West Pacific Standard Time;West Pacific Daylight Time;;") },
            { "Tasmania Standard Time", TimeZoneInfo.FromSerializedString("Tasmania Standard Time;600;(GMT+10:00) Hobart;Tasmania Standard Time;Tasmania Daylight Time;[01:01:0001;12:31:2007;60;[0;02:00:00;10;1;0;];[0;03:00:00;3;5;0;];][01:01:2008;12:31:9999;60;[0;02:00:00;10;1;0;];[0;03:00:00;4;1;0;];];") },
            { "Vladivostok Standard Time", TimeZoneInfo.FromSerializedString("Vladivostok Standard Time;600;(GMT+10:00) Vladivostok;Vladivostok Standard Time;Vladivostok Daylight Time;[01:01:0001;12:31:9999;60;[0;02:00:00;3;5;0;];[0;03:00:00;10;5;0;];];") },
            { "Central Pacific Standard Time", TimeZoneInfo.FromSerializedString("Central Pacific Standard Time;660;(GMT+11:00) Magadan, Solomon Is., New Caledonia;Central Pacific Standard Time;Central Pacific Daylight Time;;") },
            { "New Zealand Standard Time", TimeZoneInfo.FromSerializedString("New Zealand Standard Time;720;(GMT+12:00) Auckland, Wellington;New Zealand Standard Time;New Zealand Daylight Time;[01:01:0001;12:31:2006;60;[0;02:00:00;10;1;0;];[0;03:00:00;3;3;0;];][01:01:2007;12:31:2007;60;[0;02:00:00;9;5;0;];[0;03:00:00;3;3;0;];][01:01:2008;12:31:9999;60;[0;02:00:00;9;5;0;];[0;03:00:00;4;1;0;];];") },
            { "Fiji Standard Time", TimeZoneInfo.FromSerializedString("Fiji Standard Time;720;(GMT+12:00) Fiji, Kamchatka, Marshall Is.;Fiji Standard Time;Fiji Daylight Time;;") },
            { "Tonga Standard Time", TimeZoneInfo.FromSerializedString("Tonga Standard Time;780;(GMT+13:00) Nuku'alofa;Tonga Standard Time;Tonga Daylight Time;;") },
            { "Azores Standard Time", TimeZoneInfo.FromSerializedString("Azores Standard Time;-60;(GMT-01:00) Azores;Azores Standard Time;Azores Daylight Time;[01:01:0001;12:31:9999;60;[0;02:00:00;3;5;0;];[0;03:00:00;10;5;0;];];") },
            { "Cape Verde Standard Time", TimeZoneInfo.FromSerializedString("Cape Verde Standard Time;-60;(GMT-01:00) Cape Verde Is.;Cape Verde Standard Time;Cape Verde Daylight Time;;") },
            { "Mid-Atlantic Standard Time", TimeZoneInfo.FromSerializedString("Mid-Atlantic Standard Time;-120;(GMT-02:00) Mid-Atlantic;Mid-Atlantic Standard Time;Mid-Atlantic Daylight Time;[01:01:0001;12:31:9999;60;[0;02:00:00;3;5;0;];[0;02:00:00;9;5;0;];];") },
            { "E. South America Standard Time", TimeZoneInfo.FromSerializedString("E. South America Standard Time;-180;(GMT-03:00) Brasilia;E. South America Standard Time;E. South America Daylight Time;[01:01:0001;12:31:2006;60;[0;00:00:00;11;1;0;];[0;02:00:00;2;2;0;];][01:01:2007;12:31:2007;60;[0;00:00:00;10;2;0;];[0;00:00:00;2;5;0;];][01:01:2008;12:31:2008;60;[0;23:59:59.999;10;3;6;];[0;00:00:00;2;3;0;];][01:01:2009;12:31:9999;60;[0;23:59:59.999;10;3;6;];[0;23:59:59.999;2;2;6;];];") },
            { "Argentina Standard Time", TimeZoneInfo.FromSerializedString("Argentina Standard Time;-180;(GMT-03:00) Buenos Aires;Argentina Standard Time;Argentina Daylight Time;[01:01:2007;12:31:2007;60;[0;00:00:00;12;5;0;];[0;00:00:00;1;1;1;];][01:01:2008;12:31:2008;60;[0;23:59:59.999;10;3;6;];[0;00:00:00;3;3;0;];][01:01:2009;12:31:9999;60;[0;23:59:59.999;10;3;6;];[0;23:59:59.999;3;2;6;];];") },
            { "SA Eastern Standard Time", TimeZoneInfo.FromSerializedString("SA Eastern Standard Time;-180;(GMT-03:00) Georgetown;SA Eastern Standard Time;SA Eastern Daylight Time;;") },
            { "Greenland Standard Time", TimeZoneInfo.FromSerializedString("Greenland Standard Time;-180;(GMT-03:00) Greenland;Greenland Standard Time;Greenland Daylight Time;[01:01:0001;12:31:9999;60;[0;02:00:00;4;1;0;];[0;02:00:00;10;5;0;];];") },
            { "Montevideo Standard Time", TimeZoneInfo.FromSerializedString("Montevideo Standard Time;-180;(GMT-03:00) Montevideo;Montevideo Standard Time;Montevideo Daylight Time;[01:01:0001;12:31:9999;60;[0;02:00:00;10;1;0;];[0;02:00:00;3;2;0;];];") },
            { "Newfoundland Standard Time", TimeZoneInfo.FromSerializedString("Newfoundland Standard Time;-210;(GMT-03:30) Newfoundland;Newfoundland Standard Time;Newfoundland Daylight Time;[01:01:0001;12:31:2006;60;[0;00:01:00;4;1;0;];[0;00:01:00;10;5;0;];][01:01:2007;12:31:9999;60;[0;00:01:00;3;2;0;];[0;00:01:00;11;1;0;];];") },
            { "Atlantic Standard Time", TimeZoneInfo.FromSerializedString("Atlantic Standard Time;-240;(GMT-04:00) Atlantic Time (Canada);Atlantic Standard Time;Atlantic Daylight Time;[01:01:0001;12:31:2006;60;[0;02:00:00;4;1;0;];[0;02:00:00;10;5;0;];][01:01:2007;12:31:9999;60;[0;02:00:00;3;2;0;];[0;02:00:00;11;1;0;];];") },
            { "SA Western Standard Time", TimeZoneInfo.FromSerializedString("SA Western Standard Time;-240;(GMT-04:00) La Paz;SA Western Standard Time;SA Western Daylight Time;;") },
            { "Central Brazilian Standard Time", TimeZoneInfo.FromSerializedString("Central Brazilian Standard Time;-240;(GMT-04:00) Manaus;Central Brazilian Standard Time;Central Brazilian Daylight Time;[01:01:0001;12:31:2006;60;[0;00:00:00;11;1;0;];[0;02:00:00;2;2;0;];][01:01:2007;12:31:2007;60;[0;00:00:00;10;2;0;];[0;00:00:00;2;5;0;];][01:01:2008;12:31:2008;60;[0;23:59:59.999;10;3;6;];[0;00:00:00;2;3;0;];][01:01:2009;12:31:9999;60;[0;23:59:59.999;10;3;6;];[0;23:59:59.999;2;2;6;];];") },
            { "Pacific SA Standard Time", TimeZoneInfo.FromSerializedString("Pacific SA Standard Time;-240;(GMT-04:00) Santiago;Pacific SA Standard Time;Pacific SA Daylight Time;[01:01:0001;12:31:2007;60;[0;23:59:59.999;10;2;6;];[0;23:59:59.999;3;2;6;];][01:01:2008;12:31:2008;60;[0;23:59:59.999;10;2;6;];[0;23:59:59.999;3;5;6;];][01:01:2009;12:31:9999;60;[0;23:59:59.999;10;2;6;];[0;23:59:59.999;3;2;6;];];") },
            { "Venezuela Standard Time", TimeZoneInfo.FromSerializedString("Venezuela Standard Time;-270;(GMT-04:30) Caracas;Venezuela Standard Time;Venezuela Daylight Time;;") },
            { "SA Pacific Standard Time", TimeZoneInfo.FromSerializedString("SA Pacific Standard Time;-300;(GMT-05:00) Bogota, Lima, Quito, Rio Branco;SA Pacific Standard Time;SA Pacific Daylight Time;;") },
            { "Eastern Standard Time", TimeZoneInfo.FromSerializedString("Eastern Standard Time;-300;(GMT-05:00) Eastern Time (US & Canada);Eastern Standard Time;Eastern Daylight Time;[01:01:0001;12:31:2006;60;[0;02:00:00;4;1;0;];[0;02:00:00;10;5;0;];][01:01:2007;12:31:9999;60;[0;02:00:00;3;2;0;];[0;02:00:00;11;1;0;];];") },
            { "US Eastern Standard Time", TimeZoneInfo.FromSerializedString("US Eastern Standard Time;-300;(GMT-05:00) Indiana (East);US Eastern Standard Time;US Eastern Daylight Time;;") },
            { "Central America Standard Time", TimeZoneInfo.FromSerializedString("Central America Standard Time;-360;(GMT-06:00) Central America;Central America Standard Time;Central America Daylight Time;;") },
            { "Central Standard Time", TimeZoneInfo.FromSerializedString("Central Standard Time;-360;(GMT-06:00) Central Time (US & Canada);Central Standard Time;Central Daylight Time;[01:01:0001;12:31:2006;60;[0;02:00:00;4;1;0;];[0;02:00:00;10;5;0;];][01:01:2007;12:31:9999;60;[0;02:00:00;3;2;0;];[0;02:00:00;11;1;0;];];") },
            { "Central Standard Time (Mexico)", TimeZoneInfo.FromSerializedString("Central Standard Time (Mexico);-360;(GMT-06:00) Guadalajara, Mexico City, Monterrey - New;Central Standard Time (Mexico);Central Daylight Time (Mexico);[01:01:0001;12:31:9999;60;[0;02:00:00;4;1;0;];[0;02:00:00;10;5;0;];];") },
            { "Mexico Standard Time", TimeZoneInfo.FromSerializedString("Mexico Standard Time;-360;(GMT-06:00) Guadalajara, Mexico City, Monterrey - Old;Mexico Standard Time;Mexico Daylight Time;[01:01:0001;12:31:9999;60;[0;02:00:00;4;1;0;];[0;02:00:00;10;5;0;];];") },
            { "Canada Central Standard Time", TimeZoneInfo.FromSerializedString("Canada Central Standard Time;-360;(GMT-06:00) Saskatchewan;Canada Central Standard Time;Canada Central Daylight Time;;") },
            { "US Mountain Standard Time", TimeZoneInfo.FromSerializedString("US Mountain Standard Time;-420;(GMT-07:00) Arizona;US Mountain Standard Time;US Mountain Daylight Time;;") },
            { "Mountain Standard Time (Mexico)", TimeZoneInfo.FromSerializedString("Mountain Standard Time (Mexico);-420;(GMT-07:00) Chihuahua, La Paz, Mazatlan - New;Mountain Standard Time (Mexico);Mountain Daylight Time (Mexico);[01:01:0001;12:31:9999;60;[0;02:00:00;4;1;0;];[0;02:00:00;10;5;0;];];") },
            { "Mexico Standard Time 2", TimeZoneInfo.FromSerializedString("Mexico Standard Time 2;-420;(GMT-07:00) Chihuahua, La Paz, Mazatlan - Old;Mexico Standard Time 2;Mexico Daylight Time 2;[01:01:0001;12:31:9999;60;[0;02:00:00;4;1;0;];[0;02:00:00;10;5;0;];];") },
            { "Mountain Standard Time", TimeZoneInfo.FromSerializedString("Mountain Standard Time;-420;(GMT-07:00) Mountain Time (US & Canada);Mountain Standard Time;Mountain Daylight Time;[01:01:0001;12:31:2006;60;[0;02:00:00;4;1;0;];[0;02:00:00;10;5;0;];][01:01:2007;12:31:9999;60;[0;02:00:00;3;2;0;];[0;02:00:00;11;1;0;];];") },
            { "Pacific Standard Time", TimeZoneInfo.FromSerializedString("Pacific Standard Time;-480;(GMT-08:00) Pacific Time (US & Canada);Pacific Standard Time;Pacific Daylight Time;[01:01:0001;12:31:2006;60;[0;02:00:00;4;1;0;];[0;02:00:00;10;5;0;];][01:01:2007;12:31:9999;60;[0;02:00:00;3;2;0;];[0;02:00:00;11;1;0;];];") },
            { "Pacific Standard Time (Mexico)", TimeZoneInfo.FromSerializedString("Pacific Standard Time (Mexico);-480;(GMT-08:00) Tijuana, Baja California;Pacific Standard Time (Mexico);Pacific Daylight Time (Mexico);[01:01:0001;12:31:9999;60;[0;02:00:00;4;1;0;];[0;02:00:00;10;5;0;];];") },
            { "Alaskan Standard Time", TimeZoneInfo.FromSerializedString("Alaskan Standard Time;-540;(GMT-09:00) Alaska;Alaskan Standard Time;Alaskan Daylight Time;[01:01:0001;12:31:2006;60;[0;02:00:00;4;1;0;];[0;02:00:00;10;5;0;];][01:01:2007;12:31:9999;60;[0;02:00:00;3;2;0;];[0;02:00:00;11;1;0;];];") },
            { "Hawaiian Standard Time", TimeZoneInfo.FromSerializedString("Hawaiian Standard Time;-600;(GMT-10:00) Hawaii;Hawaiian Standard Time;Hawaiian Daylight Time;;") },
            { "Samoa Standard Time", TimeZoneInfo.FromSerializedString("Samoa Standard Time;-660;(GMT-11:00) Midway Island, Samoa;Samoa Standard Time;Samoa Daylight Time;;") },
            { "Dateline Standard Time", TimeZoneInfo.FromSerializedString("Dateline Standard Time;-720;(GMT-12:00) International Date Line West;Dateline Standard Time;Dateline Daylight Time;;") },
            { "UTC", TimeZoneInfo.Utc }
        };
        #endregion

        #region properties

        #endregion

        #region properties

        /// <summary>
        /// Gets the GMT standard time zone.
        /// </summary>
        /// <returns>The GMT standard time zon</returns>
        /// <exception cref="System.TimeZoneNotFoundException">Throws a System.TimeZoneNotFoundException if the gmt standard time can't be found.</exception>
        public static TimeZoneInfo GetGMTStandardTimeZone()
        {
            if (!_TimeZoneInfos.ContainsKey("GMT Standard Time"))
                throw new TimeZoneNotFoundException("GMT Standard Time timezone not found");

            return _TimeZoneInfos["GMT Standard Time"];
        }

        /// <summary>
        /// Gets the time zone info.
        /// </summary>
        /// <param name="timeZoneName">Name of the time zone.</param>
        /// <returns></returns>
        /// <exception cref="System.TimeZoneNotFoundException">Throws a System.TimeZoneNotFoundException if the timezone was not found.</exception>
        public static TimeZoneInfo GetTimeZoneInfo(string timeZoneName)
        {
            if (!_TimeZoneInfos.ContainsKey(timeZoneName))
                throw new TimeZoneNotFoundException(timeZoneName + " timezone not found");

            return _TimeZoneInfos[timeZoneName];
        }

        /// <summary>
        /// Determines whether a specified timezone if present at this instance
        /// </summary>
        /// <param name="timeZoneName">Name of the time zone.</param>
        /// <returns>
        /// 	<c>true</c> if a specified timezone if present at this instance; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasTimeZone(string timeZoneName)
        {
            if (timeZoneName == null)
                return false;

            return _TimeZoneInfos.ContainsKey(timeZoneName);
        }

        /// <summary>
        /// Gets all time zone names.
        /// </summary>
        /// <returns></returns>
        public static List<string> GetAllTimeZoneNames()
        {
            return _TimeZoneInfos.Keys.ToList();
        }

        /// <summary>
        /// Tries the convert a UTC date to a localized date.
        /// </summary>
        /// <param name="utcDate">The UTC date.</param>
        /// <param name="timeZoneName">Name of the time zone.</param>
        /// <param name="localizedDate">The localized date.</param>
        /// <returns></returns>
        public static bool TryConvertUTCDateToLocalizedDate(DateTime utcDate, string timeZoneName, out DateTime localizedDate)
        {
            localizedDate = DateTime.MinValue;

            if (!_TimeZoneInfos.ContainsKey(timeZoneName))
                return false;

            localizedDate = ConvertUTCDateToLocalizedDate(utcDate, _TimeZoneInfos[timeZoneName]);
            return true;
        }
        /// <summary>
        /// Converts the UTC date to localized date.
        /// </summary>
        /// <param name="utcDate">The UTC date.</param>
        /// <param name="timeZoneInfo">The time zone info.</param>
        /// <returns></returns>
        public static DateTime ConvertUTCDateToLocalizedDate(DateTime utcDate, TimeZoneInfo timeZoneInfo)
        {
            if (utcDate > DateTime.MinValue)
                return utcDate.Add(timeZoneInfo.GetUtcOffset(utcDate));

            return DateTime.MinValue;
        }

        /// <summary>
        /// Tries the convert a local date to UTC.
        /// </summary>
        /// <param name="localDate">The local date.</param>
        /// <param name="timeZoneName">Name of the time zone.</param>
        /// <param name="utcDate">The UTC date.</param>
        /// <returns></returns>
        public static bool TryConvertLocalizedDateToUTC(DateTime localDate, string timeZoneName, out DateTime utcDate)
        {
            utcDate = DateTime.MinValue;

            if (!_TimeZoneInfos.ContainsKey(timeZoneName))
                return false;

            utcDate = ConvertUTCDateToLocalizedDate(localDate, _TimeZoneInfos[timeZoneName]);
            return true;
        }
        /// <summary>
        /// Converts the localized date to UTC.
        /// </summary>
        /// <param name="localDate">The local date.</param>
        /// <param name="timeZoneInfo">The time zone info.</param>
        /// <returns></returns>
        public static DateTime ConvertLocalizedDateToUTC(DateTime localDate, TimeZoneInfo timeZoneInfo)
        {
            if (localDate > DateTime.MinValue)
                return localDate.AddSeconds(timeZoneInfo.GetUtcOffset(localDate).TotalSeconds * -1);
            return DateTime.MinValue;
        }


        #endregion
    }
}
