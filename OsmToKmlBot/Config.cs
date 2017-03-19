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
@"=== Как пользоваться ===

Отправьте свое местоположение, чтобы получить `KML` файл с отметками задний без этажности.

Чтобы получить другой набор данных:
1. Отправьте /rules
2. Выберите интересующий набор, нажав на вариант из списка
3. Отправьте свое местоположение

=== Как добавить своё правило ===

1. Отправьте `/set <rule_name>`. Где `<rule_name>` название нового правила, может содержать только буквы латинского алфавита и цифры.
2. Отправьте overpass запрос, который должен возвращать `csv` файл с координатами.

Пример overpass запроса:
```
[out:csv(::lat,::lon)][timeout:25];
(
  node[""shop""][""opening_hours""!~"".*""]({{bbox}});
  way[ ""shop"" ][ ""opening_hours""!~"".*"" ]({{bbox
    }
});
  relation[ ""shop"" ][ ""opening_hours""!~"".* ""]({{bbox}});
);
out center;
```
=== Другие команды ===

`/show<rule_name>` - посмотреть overpass запрос
";
    }
}
