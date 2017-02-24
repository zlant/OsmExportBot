using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsmToKmlBot
{
    public static class Config
    {
        public static string Token = "NEED TOKEN";
        public static string RulesFolder = "Queries\\";
        public static string LogFolder = "\\";


        public static string StartText =
@"Отправьте свое местоположение, чтобы получить kml файл с отметками задний без этажности.

/rules - возвращает названия всех доступных запросов

/get <rule> или /<rule> - получить объекты по указанному запросу, при следующей отправке местоположения

/set <rule> - создать новый запрос. Название может содержать только буквы латинского алфавита и цифры. Следующим сообщением отправить overpass запрос, который должен возвращать csv файл с координатами. Пример:
[out:csv(::lat,::lon)][timeout:25];
(
  node[""shop""][""opening_hours""!~"".*""]({{bbox}});
  way[""shop""][ ""opening_hours""!~"".*"" ]({{bbox}});
  relation[""shop""][""opening_hours""!~"".*""]({{bbox}});
);
out center;

/show <rule> - посмотреть overpass запрос
";
    }
}
