using AutoMapper.QueryableExtensions;
using QuranEducation.Models;
using QuranEducation.Models.VM;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace QuranEducation.Helpers
{
    public class GeneralHelper
    {
        public static List<InstVM> GetInstructors
        {
            get
            {
                using (var ctx = new ApplicationDbContext())
                {
                    var InstsRoleId = ctx.Roles.Where(r => r.Name == RoleNames.InstructorLevel).Select(r => r.Id).FirstOrDefault();
                    var insts = ctx.Users.Where(u => u.Roles.Select(r => r.RoleId).Contains(InstsRoleId)).ProjectTo<InstVM>().ToList();
                    return insts;
                }
            }
        }
        public static string GetInstFullName(string InstUName)
        {
            var fullname = "";
            using (var ctx = new ApplicationDbContext())
            {
                fullname = ctx.Users.FirstOrDefault(m => m.UserName == InstUName).FullName;

            }
            return fullname;
        }
        public static List<Tutorial> GetInstTut(string InstUName)
        {
            var tutorials = new List<Tutorial>();
            using (var ctx = new ApplicationDbContext())
            {
                tutorials = ctx.Tutorials.Where(m => m.InstUName == InstUName).ToList();

            }
            return tutorials;
        }
        public static string GetTutorialTitle(int Id)
        {
            var tutorial = "";
            using (var ctx = new ApplicationDbContext())
            {
                tutorial = ctx.Tutorials.Where(m => m.Id == Id).Select(m => m.Title).FirstOrDefault();

            }
            return tutorial;
        }

        public static InstEvalVM GetInst()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var _currentDate = DateTime.Now;

                var stInsts = ctx.StudentTutorials.Where(m => m.ManagemetAccept && m.UserName == HttpContext.Current.User.Identity.Name).Select(m => m.Tutorial.InstUName).ToList();
                var instRole = ctx.Roles.Where(r => r.Name == RoleNames.InstructorLevel).Select(r => r.Id).FirstOrDefault();
                var insts = ctx.Users.Where(m => m.Roles.Select(r => r.RoleId).Contains(instRole) && stInsts.Contains(m.UserName))
                    .Select(m => new InstVM { UserName = m.UserName, FullName = m.FullName }).ToList();

                var tuts = ctx.Tutorials.Where(t => t.Active && t.CloseDate >= _currentDate &&stInsts.Contains( t.InstUName)).ToList();
                return new InstEvalVM
                {
                    Insts = insts,
                    Tuts = tuts
                };
            }
        }
        
        public static bool IsSubscribe(int tutId, string username)
        {
            using (var ctx = new ApplicationDbContext())
            {
                return ctx.StudentTutorials.FirstOrDefault(m => m.ManagemetAccept && m.TutorialId == tutId && m.UserName == username) != null;
            }
        }
        public static CertifiedDegree Degree(int tutId, string username)
        {
            using (var ctx = new ApplicationDbContext())
            {
                return ctx.StudentTutorials.Where(m => m.ManagemetAccept && m.TutorialId == tutId && m.UserName == username
                && m.Certified).Select(m => m.CertifiedDegree).FirstOrDefault();
            }
        }
        public static bool IsRequested(int tutId, string username)
        {
            using (var ctx = new ApplicationDbContext())
            {
                return ctx.StudentTutorials.FirstOrDefault(m => m.TutorialId == tutId && m.UserName == username) != null;
            }
        }
        public static List<EvalStringsVM> GetEvalStrings
        {
            get
            {
                return new List<EvalStringsVM>
                {
                    new EvalStringsVM{Id=1,Title="ممتاز"},
                    new EvalStringsVM{Id=2,Title="جيد جدا"},
                    new EvalStringsVM{Id=3,Title="جيد"},
                    new EvalStringsVM{Id=4,Title="مقبول"},
                    new EvalStringsVM{Id=5,Title="ضعيف"},
                };
            }
        }
        public static string GetScoreById(int Id)
        {
            return GetScores.FirstOrDefault(m => m.Id == Id).Title;
        }
        public static List<EvalStringsVM> GetScores
        {
            get
            {
                return new List<EvalStringsVM>
                {
                    new EvalStringsVM{Id=1,Title=QuranRes.Excellent},
                    new EvalStringsVM{Id=2,Title=QuranRes.VGood},
                    new EvalStringsVM{Id=3,Title=QuranRes.Good},
                    new EvalStringsVM{Id=4,Title=QuranRes.Acceptable},
                    new EvalStringsVM{Id=5,Title=QuranRes.Poor},
                };
            }
        }
        public static string GetCountryById(int Id)
        {
            return GetCountries.FirstOrDefault(m => m.Id == Id).Title;
        }
        public static List<string> GetNationalities
        {
            get
            {
                return new List<string>
                {
                    "Afghan",
                    "Albanian",
                    "Algerian",
                    "American",
                    "Andorran",
                    "Angolan",
                    "Antiguans",
                    "Argentinean",
                    "Armenian",
                    "Australian",
                    "Austrian",
                    "Azerbaijani",
                    "Bahamian",
                    "Bahraini",
                    "Bangladeshi",
                    "Barbadian",
                    "Barbudans",
                    "Batswana",
                    "Belarusian",
                    "Belgian",
                    "Belizean",
                    "Beninese",
                    "Bhutanese",
                    "Bolivian",
                    "Bosnian",
                    "Brazilian",
                    "British",
                    "Bruneian",
                    "Bulgarian",
                    "Burkinabe",
                    "Burmese",
                    "Burundian",
                    "Cambodian",
                    "Cameroonian",
                    "Canadian",
                    "CapeVerdean",
                    "Central African",
                    "Chadian",
                    "Chilean",
                    "Chinese",
                    "Colombian",
                    "Comoran",
                    "Congolese",
                    "CostaRican",
                    "Croatian",
                    "Cuban",
                    "Cypriot",
                    "Czech",
                    "Danish",
                    "Djibouti",
                    "Dominican",
                    "Dutch",
                    "East Timorese",
                    "Ecuadorean",
                    "Egyptian",
                    "Emirian",
                    "Equatorial Guinean",
                    "Eritrean",
                    "Estonian",
                    "Ethiopian",
                    "Fijian",
                    "Filipino",
                    "Finnish",
                    "French",
                    "Gabonese",
                    "Gambian",
                    "Georgian",
                    "German",
                    "Ghanaian",
                    "Greek",
                    "Grenadian",
                    "Guatemalan",
                    "Guinea-Bissauan",
                    "Guinean",
                    "Guyanese",
                    "Haitian",
                    "Herzegovinian",
                    "Honduran",
                    "Hungarian",
                    "I-Kiribati",
                    "Icelander",
                    "Indian",
                    "Indonesian",
                    "Iranian",
                    "Iraqi",
                    "Irish",
                    "Israeli",
                    "Italian",
                    "Ivorian",
                    "Jamaican",
                    "Japanese",
                    "Jordanian",
                    "Kazakhstani",
                    "Kenyan",
                    "Kittianand Nevisian",
                    "Kuwaiti",
                    "Kyrgyz",
                    "Laotian",
                    "Latvian",
                    "Lebanese",
                    "Liberian",
                    "Libyan",
                    "Liechtensteiner",
                    "Lithuanian",
                    "Luxembourger",
                    "Macedonian",
                    "Malagasy",
                    "Malawian",
                    "Malaysian",
                    "Maldivian",
                    "Malian",
                    "Maltese",
                    "Marshallese",
                    "Mauritanian",
                    "Mauritian",
                    "Mexican",
                    "Micronesian",
                    "Moldovan",
                    "Monacan",
                    "Mongolian",
                    "Moroccan",
                    "Mosotho",
                    "Motswana",
                    "Mozambican",
                    "Namibian",
                    "Nauruan",
                    "Nepalese",
                    "NewZealander",
                    "Ni-Vanuatu",
                    "Nicaraguan",
                    "Nigerian",
                    "Nigerien",
                    "NorthKorean",
                    "Northern Irish",
                    "Norwegian",
                    "Omani",
                    "Pakistani",
                    "Palauan",
                    "Panamanian",
                    "Papua New Guinean",
                    "Paraguayan",
                    "Peruvian",
                    "Polish",
                    "Portuguese",
                    "Qatari",
                    "Romanian",
                    "Russian",
                    "Rwandan",
                    "SaintLucian",
                    "Salvadoran",
                    "Samoan",
                    "SanMarinese",
                    "SaoTomean",
                    "Saudi",
                    "Scottish",
                    "Senegalese",
                    "Serbian",
                    "Seychellois",
                    "SierraLeonean",
                    "Singaporean",
                    "Slovakian",
                    "Slovenian",
                    "Solomon Islander",
                    "Somali",
                    "SouthAfrican",
                    "SouthKorean",
                    "Spanish",
                    "SriLankan",
                    "Sudanese",
                    "Surinamer",
                    "Swazi",
                    "Swedish",
                    "Swiss",
                    "Syrian",
                    "Taiwanese",
                    "Tajik",
                    "Tanzanian",
                    "Thai",
                    "Togolese",
                    "Tongan",
                    "Trinidadianor Tobagonian",
                    "Tunisian",
                    "Turkish",
                    "Tuvaluan",
                    "Ugandan",
                    "Ukrainian",
                    "Uruguayan",
                    "Uzbekistani",
                    "Venezuelan",
                    "Vietnamese",
                    "Welsh",
                    "Yemenite",
                    "Zambian",
                    "Zimbabwean"
                };
            }
        }
        public static List<EvalStringsVM> GetCountries
        {
            get
            {
                var listOfCountries = new List<EvalStringsVM> {new EvalStringsVM{ Id = 44, Title = "UK (+44)" },
                                    new EvalStringsVM { Id = 1, Title = "USA (+1)" },
                                    new EvalStringsVM { Id = 213, Title = "Algeria (+213)" },
                                    new EvalStringsVM { Id = 376, Title = "Andorra (+376)" },
                                    new EvalStringsVM { Id = 244, Title = "Angola (+244)" },
                                    new EvalStringsVM { Id = 1264, Title = "Anguilla (+1264)" },
                                    new EvalStringsVM { Id = 1268, Title = "Antigua &amp; Barbuda (+1268)" },
                                    new EvalStringsVM { Id = 54, Title = "Argentina (+54)" },
                                    new EvalStringsVM { Id = 374, Title = "Armenia (+374)" },
                                    new EvalStringsVM { Id = 297, Title = "Aruba (+297)" },
                                    new EvalStringsVM { Id = 61, Title = "Australia (+61)" },
                                    new EvalStringsVM { Id = 43, Title = "Austria (+43)" },
                                    new EvalStringsVM { Id = 994, Title = "Azerbaijan (+994)" },
                                    new EvalStringsVM { Id = 1242, Title = "Bahamas (+1242)" },
                                    new EvalStringsVM { Id = 973, Title = "Bahrain (+973)" },
                                    new EvalStringsVM { Id = 880, Title = "Bangladesh (+880)" },
                                    new EvalStringsVM { Id = 1246, Title = "Barbados (+1246)" },
                                    new EvalStringsVM { Id = 375, Title = "Belarus (+375)" },
                                    new EvalStringsVM { Id = 32, Title = "Belgium (+32)" },
                                    new EvalStringsVM { Id = 501, Title = "Belize (+501)" },
                                    new EvalStringsVM { Id = 229, Title = "Benin (+229)" },
                                    new EvalStringsVM { Id = 1441, Title = "Bermuda (+1441)" },
                                    new EvalStringsVM { Id = 975, Title = "Bhutan (+975)" },
                                    new EvalStringsVM { Id = 591, Title = "Bolivia (+591)" },
                                    new EvalStringsVM { Id = 387, Title = "Bosnia Herzegovina (+387)" },
                                    new EvalStringsVM { Id = 267, Title = "Botswana (+267)" },
                                    new EvalStringsVM { Id = 55, Title = "Brazil (+55)" },
                                    new EvalStringsVM { Id = 673, Title = "Brunei (+673)" },
                                    new EvalStringsVM { Id = 359, Title = "Bulgaria (+359)" },
                                    new EvalStringsVM { Id = 226, Title = "Burkina Faso (+226)" },
                                    new EvalStringsVM { Id = 257, Title = "Burundi (+257)" },
                                    new EvalStringsVM { Id = 855, Title = "Cambodia (+855)" },
                                    new EvalStringsVM { Id = 237, Title = "Cameroon (+237)" },
                                    new EvalStringsVM { Id = 1, Title = "Canada (+1)" },
                                    new EvalStringsVM { Id = 238, Title = "Cape Verde Islands (+238)" },
                                    new EvalStringsVM { Id = 1345, Title = "Cayman Islands (+1345)" },
                                    new EvalStringsVM { Id = 236, Title = "Central African Republic (+236)" },
                                    new EvalStringsVM { Id = 56, Title = "Chile (+56)" },
                                    new EvalStringsVM { Id = 86, Title = "China (+86)" },
                                    new EvalStringsVM { Id = 57, Title = "Colombia (+57)" },
                                    new EvalStringsVM { Id = 269, Title = "Comoros (+269)" },
                                    new EvalStringsVM { Id = 242, Title = "Congo (+242)" },
                                    new EvalStringsVM { Id = 682, Title = "Cook Islands (+682)" },
                                    new EvalStringsVM { Id = 506, Title = "Costa Rica (+506)" },
                                    new EvalStringsVM { Id = 385, Title = "Croatia (+385)" },
                                    new EvalStringsVM { Id = 53, Title = "Cuba (+53)" },
                                    new EvalStringsVM { Id = 90392, Title = "Cyprus North (+90392)" },
                                    new EvalStringsVM { Id = 357, Title = "Cyprus South (+357)" },
                                    new EvalStringsVM { Id = 42, Title = "Czech Republic (+42)" },
                                    new EvalStringsVM { Id = 45, Title = "Denmark (+45)" },
                                    new EvalStringsVM { Id = 253, Title = "Djibouti (+253)" },
                                    new EvalStringsVM { Id = 1809, Title = "Dominica (+1809)" },
                                    new EvalStringsVM { Id = 1809, Title = "Dominican Republic (+1809)" },
                                    new EvalStringsVM { Id = 593, Title = "Ecuador (+593)" },
                                    new EvalStringsVM { Id = 20, Title = "Egypt (+20)" },
                                    new EvalStringsVM { Id = 503, Title = "El Salvador (+503)" },
                                    new EvalStringsVM { Id = 240, Title = "Equatorial Guinea (+240)" },
                                    new EvalStringsVM { Id = 291, Title = "Eritrea (+291)" },
                                    new EvalStringsVM { Id = 372, Title = "Estonia (+372)" },
                                    new EvalStringsVM { Id = 251, Title = "Ethiopia (+251)" },
                                    new EvalStringsVM { Id = 500, Title = "Falkland Islands (+500)" },
                                    new EvalStringsVM { Id = 298, Title = "Faroe Islands (+298)" },
                                    new EvalStringsVM { Id = 679, Title = "Fiji (+679)" },
                                    new EvalStringsVM { Id = 358, Title = "Finland (+358)" },
                                    new EvalStringsVM { Id = 33, Title = "France (+33)" },
                                    new EvalStringsVM { Id = 594, Title = "French Guiana (+594)" },
                                    new EvalStringsVM { Id = 689, Title = "French Polynesia (+689)" },
                                    new EvalStringsVM { Id = 241, Title = "Gabon (+241)" },
                                    new EvalStringsVM { Id = 220, Title = "Gambia (+220)" },
                                    new EvalStringsVM { Id = 7880, Title = "Georgia (+7880)" },
                                    new EvalStringsVM { Id = 49, Title = "Germany (+49)" },
                                    new EvalStringsVM { Id = 233, Title = "Ghana (+233)" },
                                    new EvalStringsVM { Id = 350, Title = "Gibraltar (+350)" },
                                    new EvalStringsVM { Id = 30, Title = "Greece (+30)" },
                                    new EvalStringsVM { Id = 299, Title = "Greenland (+299)" },
                                    new EvalStringsVM { Id = 1473, Title = "Grenada (+1473)" },
                                    new EvalStringsVM { Id = 590, Title = "Guadeloupe (+590)" },
                                    new EvalStringsVM { Id = 671, Title = "Guam (+671)" },
                                    new EvalStringsVM { Id = 502, Title = "Guatemala (+502)" },
                                    new EvalStringsVM { Id = 224, Title = "Guinea (+224)" },
                                    new EvalStringsVM { Id = 245, Title = "Guinea - Bissau (+245)" },
                                    new EvalStringsVM { Id = 592, Title = "Guyana (+592)" },
                                    new EvalStringsVM { Id = 509, Title = "Haiti (+509)" },
                                    new EvalStringsVM { Id = 504, Title = "Honduras (+504)" },
                                    new EvalStringsVM { Id = 852, Title = "Hong Kong (+852)" },
                                    new EvalStringsVM { Id = 36, Title = "Hungary (+36)" },
                                    new EvalStringsVM { Id = 354, Title = "Iceland (+354)" },
                                    new EvalStringsVM { Id = 91, Title = "India (+91)" },
                                    new EvalStringsVM { Id = 62, Title = "Indonesia (+62)" },
                                    new EvalStringsVM { Id = 98, Title = "Iran (+98)" },
                                    new EvalStringsVM { Id = 964, Title = "Iraq (+964)" },
                                    new EvalStringsVM { Id = 353, Title = "Ireland (+353)" },
                                    new EvalStringsVM { Id = 972, Title = "Israel (+972)" },
                                    new EvalStringsVM { Id = 39, Title = "Italy (+39)" },
                                    new EvalStringsVM { Id = 1876, Title = "Jamaica (+1876)" },
                                    new EvalStringsVM { Id = 81, Title = "Japan (+81)" },
                                    new EvalStringsVM { Id = 962, Title = "Jordan (+962)" },
                                    new EvalStringsVM { Id = 7, Title = "Kazakhstan (+7)" },
                                    new EvalStringsVM { Id = 254, Title = "Kenya (+254)" },
                                    new EvalStringsVM { Id = 686, Title = "Kiribati (+686)" },
                                    new EvalStringsVM { Id = 850, Title = "Korea North (+850)" },
                                    new EvalStringsVM { Id = 82, Title = "Korea South (+82)" },
                                    new EvalStringsVM { Id = 965, Title = "Kuwait (+965)" },
                                    new EvalStringsVM { Id = 996, Title = "Kyrgyzstan (+996)" },
                                    new EvalStringsVM { Id = 856, Title = "Laos (+856)" },
                                    new EvalStringsVM { Id = 371, Title = "Latvia (+371)" },
                                    new EvalStringsVM { Id = 961, Title = "Lebanon (+961)" },
                                    new EvalStringsVM { Id = 266, Title = "Lesotho (+266)" },
                                    new EvalStringsVM { Id = 231, Title = "Liberia (+231)" },
                                    new EvalStringsVM { Id = 218, Title = "Libya (+218)" },
                                    new EvalStringsVM { Id = 417, Title = "Liechtenstein (+417)" },
                                    new EvalStringsVM { Id = 370, Title = "Lithuania (+370)" },
                                    new EvalStringsVM { Id = 352, Title = "Luxembourg (+352)" },
                                    new EvalStringsVM { Id = 853, Title = "Macao (+853)" },
                                    new EvalStringsVM { Id = 389, Title = "Macedonia (+389)" },
                                    new EvalStringsVM { Id = 261, Title = "Madagascar (+261)" },
                                    new EvalStringsVM { Id = 265, Title = "Malawi (+265)" },
                                    new EvalStringsVM { Id = 60, Title = "Malaysia (+60)" },
                                    new EvalStringsVM { Id = 960, Title = "Maldives (+960)" },
                                    new EvalStringsVM { Id = 223, Title = "Mali (+223)" },
                                    new EvalStringsVM { Id = 356, Title = "Malta (+356)" },
                                    new EvalStringsVM { Id = 692, Title = "Marshall Islands (+692)" },
                                    new EvalStringsVM { Id = 596, Title = "Martinique (+596)" },
                                    new EvalStringsVM { Id = 222, Title = "Mauritania (+222)" },
                                    new EvalStringsVM { Id = 269, Title = "Mayotte (+269)" },
                                    new EvalStringsVM { Id = 52, Title = "Mexico (+52)" },
                                    new EvalStringsVM { Id = 691, Title = "Micronesia (+691)" },
                                    new EvalStringsVM { Id = 373, Title = "Moldova (+373)" },
                                    new EvalStringsVM { Id = 377, Title = "Monaco (+377)" },
                                    new EvalStringsVM { Id = 976, Title = "Mongolia (+976)" },
                                    new EvalStringsVM { Id = 1664, Title = "Montserrat (+1664)" },
                                    new EvalStringsVM { Id = 212, Title = "Morocco (+212)" },
                                    new EvalStringsVM { Id = 258, Title = "Mozambique (+258)" },
                                    new EvalStringsVM { Id = 95, Title = "Myanmar (+95)" },
                                    new EvalStringsVM { Id = 264, Title = "Namibia (+264)" },
                                    new EvalStringsVM { Id = 674, Title = "Nauru (+674)" },
                                    new EvalStringsVM { Id = 977, Title = "Nepal (+977)" },
                                    new EvalStringsVM { Id = 31, Title = "Netherlands (+31)" },
                                    new EvalStringsVM { Id = 687, Title = "New Caledonia (+687)" },
                                    new EvalStringsVM { Id = 64, Title = "New Zealand (+64)" },
                                    new EvalStringsVM { Id = 505, Title = "Nicaragua (+505)" },
                                    new EvalStringsVM { Id = 227, Title = "Niger (+227)" },
                                    new EvalStringsVM { Id = 234, Title = "Nigeria (+234)" },
                                    new EvalStringsVM { Id = 683, Title = "Niue (+683)" },
                                    new EvalStringsVM { Id = 672, Title = "Norfolk Islands (+672)" },
                                    new EvalStringsVM { Id = 670, Title = "Northern Marianas (+670)" },
                                    new EvalStringsVM { Id = 47, Title = "Norway (+47)" },
                                    new EvalStringsVM { Id = 968, Title = "Oman (+968)" },
                                    new EvalStringsVM { Id = 680, Title = "Palau (+680)" },
                                    new EvalStringsVM { Id = 507, Title = "Panama (+507)" },
                                    new EvalStringsVM { Id = 675, Title = "Papua New Guinea (+675)" },
                                    new EvalStringsVM { Id = 595, Title = "Paraguay (+595)" },
                                    new EvalStringsVM { Id = 51, Title = "Peru (+51)" },
                                    new EvalStringsVM { Id = 63, Title = "Philippines (+63)" },
                                    new EvalStringsVM { Id = 48, Title = "Poland (+48)" },
                                    new EvalStringsVM { Id = 351, Title = "Portugal (+351)" },
                                    new EvalStringsVM { Id = 1787, Title = "Puerto Rico (+1787)" },
                                    new EvalStringsVM { Id = 974, Title = "Qatar (+974)" },
                                    new EvalStringsVM { Id = 262, Title = "Reunion (+262)" },
                                    new EvalStringsVM { Id = 40, Title = "Romania (+40)" },
                                    new EvalStringsVM { Id = 7, Title = "Russia (+7)" },
                                    new EvalStringsVM { Id = 250, Title = "Rwanda (+250)" },
                                    new EvalStringsVM { Id = 378, Title = "San Marino (+378)" },
                                    new EvalStringsVM { Id = 239, Title = "Sao Tome &amp; Principe (+239)" },
                                    new EvalStringsVM { Id = 966, Title = "Saudi Arabia (+966)" },
                                    new EvalStringsVM { Id = 221, Title = "Senegal (+221)" },
                                    new EvalStringsVM { Id = 381, Title = "Serbia (+381)" },
                                    new EvalStringsVM { Id = 248, Title = "Seychelles (+248)" },
                                    new EvalStringsVM { Id = 232, Title = "Sierra Leone (+232)" },
                                    new EvalStringsVM { Id = 65, Title = "Singapore (+65)" },
                                    new EvalStringsVM { Id = 421, Title = "Slovak Republic (+421)" },
                                    new EvalStringsVM { Id = 386, Title = "Slovenia (+386)" },
                                    new EvalStringsVM { Id = 677, Title = "Solomon Islands (+677)" },
                                    new EvalStringsVM { Id = 252, Title = "Somalia (+252)" },
                                    new EvalStringsVM { Id = 27, Title = "South Africa (+27)" },
                                    new EvalStringsVM { Id = 34, Title = "Spain (+34)" },
                                    new EvalStringsVM { Id = 94, Title = "Sri Lanka (+94)" },
                                    new EvalStringsVM { Id = 290, Title = "St. Helena (+290)" },
                                    new EvalStringsVM { Id = 1869, Title = "St. Kitts (+1869)" },
                                    new EvalStringsVM { Id = 1758, Title = "St. Lucia (+1758)" },
                                    new EvalStringsVM { Id = 249, Title = "Sudan (+249)" },
                                    new EvalStringsVM { Id = 597, Title = "Suriname (+597)" },
                                    new EvalStringsVM { Id = 268, Title = "Swaziland (+268)" },
                                    new EvalStringsVM { Id = 46, Title = "Sweden (+46)" },
                                    new EvalStringsVM { Id = 41, Title = "Switzerland (+41)" },
                                    new EvalStringsVM { Id = 963, Title = "Syria (+963)" },
                                    new EvalStringsVM { Id = 886, Title = "Taiwan (+886)" },
                                    new EvalStringsVM { Id = 7, Title = "Tajikstan (+7)" },
                                    new EvalStringsVM { Id = 66, Title = "Thailand (+66)" },
                                    new EvalStringsVM { Id = 228, Title = "Togo (+228)" },
                                    new EvalStringsVM { Id = 676, Title = "Tonga (+676)" },
                                    new EvalStringsVM { Id = 1868, Title = "Trinidad &amp; Tobago (+1868)" },
                                    new EvalStringsVM { Id = 216, Title = "Tunisia (+216)" },
                                    new EvalStringsVM { Id = 90, Title = "Turkey (+90)" },
                                    new EvalStringsVM { Id = 7, Title = "Turkmenistan (+7)" },
                                    new EvalStringsVM { Id = 993, Title = "Turkmenistan (+993)" },
                                    new EvalStringsVM { Id = 1649, Title = "Turks &amp; Caicos Islands (+1649)" },
                                    new EvalStringsVM { Id = 688, Title = "Tuvalu (+688)" },
                                    new EvalStringsVM { Id = 256, Title = "Uganda (+256)" },
                                    new EvalStringsVM { Id = 44, Title = "UK (+44)" },
                                    new EvalStringsVM { Id = 380, Title = "Ukraine (+380)" },
                                    new EvalStringsVM { Id = 971, Title = "United Arab Emirates (+971)" },
                                    new EvalStringsVM { Id = 598, Title = "Uruguay (+598)" },
                                    new EvalStringsVM { Id = 1, Title = "USA (+1)" },
                                    new EvalStringsVM { Id = 7, Title = "Uzbekistan (+7)" },
                                    new EvalStringsVM { Id = 678, Title = "Vanuatu (+678)" },
                                    new EvalStringsVM { Id = 379, Title = "Vatican City (+379)" },
                                    new EvalStringsVM { Id = 58, Title = "Venezuela (+58)" },
                                    new EvalStringsVM { Id = 84, Title = "Vietnam (+84)" },
                                    new EvalStringsVM { Id = 84, Title = "Virgin Islands - British (+1284)" },
                                    new EvalStringsVM { Id = 84, Title = "Virgin Islands - US (+1340)" },
                                    new EvalStringsVM { Id = 681, Title = "Wallis &amp; Futuna (+681)" },
                                    new EvalStringsVM { Id = 969, Title = "Yemen (North)(+969)" },
                                    new EvalStringsVM { Id = 967, Title = "Yemen (South)(+967)" },
                                    new EvalStringsVM { Id = 260, Title = "Zambia (+260)" },
                                    new EvalStringsVM { Id = 263, Title = "Zimbabwe (+263)" }};
                return listOfCountries.OrderBy(m => m.Title).ToList();

            }
        }
        public static string SaveImage(string imageUrl, string filename)
        {
            try
            {
                WebClient client = new WebClient();
                Stream stream = client.OpenRead(imageUrl);
                Bitmap bitmap; bitmap = new Bitmap(stream);

                if (bitmap != null)
                {
                    bitmap.Save(filename, ImageFormat.Png);
                }

                stream.Flush();
                stream.Close();
                client.Dispose();
                return filename + ".png";
            }
            catch (Exception)
            {

                return "";
            }

        }
    }
}