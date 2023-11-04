using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsmExportBot
{
    public static class Config
    {
        public static string Token { get; set; } = "NEED TOKEN";
        public static string RulesFolder { get; set; } = "Queries\\";


        public static string StartText =
@"=== Как пользоваться ===

1. Отправьте /rules
2. Выберите интересующий набор, нажав на вариант из списка
3. Отправьте свое местоположение

Если бот заснул и не отвечает, разбудите, посетив http://milestone.somee.com/home/bot

Добавить свое правило: /create
";

        public static string CreateInfoText =
@"=== Как добавить своё правило ===

1. Отправьте `/set <rule_name>`. Где `<rule_name>` название нового правила, может содержать только буквы латинского алфавита и цифры.
2. Отправьте overpass запрос, который должен возвращать `csv` файл с координатами.

Пример overpass запроса:
```
[out:csv(::lat,::lon)][timeout:25];
(
  node[""shop""][""opening_hours""!~"".*""]({{bbox}});
  way[ ""shop"" ][ ""opening_hours""!~"".*"" ]({{bbox}});
  relation[ ""shop"" ][ ""opening_hours""!~"".* ""]({{bbox}});
);
out center;
```
=== Другие команды ===

`/show <rule_name>` - посмотреть код overpass запроса
";
    }
}
