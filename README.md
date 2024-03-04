# Ab-Log v.3 (неофициальный)

## Введение
Сервер + клиент для доступа к контроллерам умного дома [ab-log](https://ab-log.ru/) (а так же доступ через TelegramBot):
- *Серверная часть* **ASP .NET8**. Поддерживаемые платформы: Linux, Windows, Mac. Сервер имеет свой полноценный WEB интерфейс. При желании серверный WEB.UI можно использовать без использования штатного/оригинального клиента удалённого доступа (тем более, при наличии доступа через TelegramBot)
- *Интерактиный доступ* к серверу через **TelegramBot**. Если сравнивать этот вариант взаимодействия с штатным клиентом удалённого доступа, то функционал тут не полноценный. Из-за естественных ограничений возможностей клиента Telegram в нём сложно реализовывать сложную логику взаимодейтсвия с сервером и элементами управления если сравнивать с возможностями, которые даёт обычный HTML. Однако есть важная особенность: удалённый доступ к серверным настройкам MQTT возможен только через телегу. Разумеется, MQTT установки сервера можно настроить локально через его же WEB интерфейс, но в случае необходимости удалённого редактиирования этих настроек через интернет - потребуется именно Telegram доступ. Штатные клиенты удалённого управления равноправны между собой, а клиенты телеги персинифицированы. Другими словами: для пользователей Telegram вы можете персонально указывать права к тому или иному функционалу, а штатные клиенты для сервера не отличаются правами между собой.
- *Удалённый/штатный клиент* **.NET MAUIBlazor Hybrid** через промежуточный MQTT (например через бесплатный сервис MQTT [hivemq.cloud](https://console.hivemq.cloud/)). Поддерживаемые платформы: Windows, Android. Полноценный клиент, который ни чем не уступает локальному/серверному web клиенту. Если быть точным, то это буквально одно и то же решение Blazor с той лишь разницей, что серверный web клиент общается с сервером напрамую, а удалённый клиент делает то же самое, но через промежуточный MQTT сервер. Важное ограничение в том, что штатные удалённые клиенты не имеют возможности редактировать серверные настройки MQTT (для этого пригоден TelegramBot доступ).

## План
- [x] Локальный WEB клиент для серврной части
- [x] Клиент удалённого доступа к серверу
- [x] Интерактивный доступ к системе через TelegramBot
- [ ] Сценарии: пакеты команд с базовой логикой ветвления по условию
- [ ] Тригеры: автозапуск сценариев/команд по событию
- [ ] Доступ к USB камерам сервера средствами решения [FlashCap](https://github.com/kekyo/FlashCap)

## Настройка сервера

### Требования
Серверная часть представляет из себя **ASP.NET8** приложение (порт по умолчанию 5000) со своим собственным веб клиентом `Blazor UI`.
Для Linux или Windows рекомендуется сконфигурировать работу приложения в качестве службы.
Есть рабочий [пример файла](https://github.com/badhitman/AbLog/blob/main/AbLogServer.Server/ab-log.service) демона под Linux, но вы можете сконфигурировать службу по своему усмотрению.

### TelegramBot conf
Клиент Telegram (по сравнению с клиентами удалённого доступа) хоть и ограничен в плане функциональности, но имеет несколько важных преимуществ.
1. Удалённая настройка MQTT сервера возможна только через телегу. Штатные удалённые клиенты как и серверная часть кинфигурируют только своё персональное/локальное MQTT подключение. Для возможности удалённо настраивать MQTT для серверного приложения без доступа к нему локально - потребуется доступ через Телегу. 
2. Отсутсвуют лимиты на трафик в отличие от MQTT, где такое ограничени может быть. Например предлагаемый [hivemq.cloud](https://console.hivemq.cloud/) в бесплатной версии ограничен 10GB в месяц. Израсходовать такой MQTT лимит - сложная задача, но на такой случай Telegram доступ останется.
3. Для телеги есть возможность разграничить права для каждого клиента - персонально, в то время как штатные клиенты удалённого доступа все равны между собой и имеют одинаковый полный доступ к удалённой системе.
4. Получение уведомлений от удалённой системы. Штатные клиенты удалённого доступа в этом плане пасивны. Клиент удалённого доступа не получает уведомлений пока не запущен, ну и при закрытии клиента уведомления туда не могут быть доставлены, в то время как Телега всегда оповестит и даст возможность оперативно среагировать.

![Конфигурация TelegramBot](/docs/img/telegram-bot-conf.jpeg)
Прежде всего нужно указать токен TelegramBot и включить автозапуск. Ручной запуск/перезапуск/остановка службы TelegramBot недоступен. Для того что бы бот запустился нужно указать токен, включить автозапуск (разумеется сохранить настройки) и перзапустить сервреное приложение. Бот стартует при запуске самого приложения (если включён автозапуск). Кнопка "Проверка токена" только проверяет корректность токена, но не запускает фоновую службы бота.
Пока сервер бота запущен и исправно функционирует (указан правильный токен) клиентам телеги нужно что-то написать этому боту (что угодно). Бот запоминает всех пользователей, которые писали когда либо ему. Таким образом пользователи попадают в соответсвующую таблицу "Пользователи. Права доступа". Обновить спсиок пользователей можно либо кнопкой F5 (обновить страницу), либо специальной кнопкой в правом верхнем углу данной области. Имея доступ к сохранённым пользователям им можно выдавать права (или отключать их).

![стартовое меню](/docs/img/tg-start.png)
![настройки mqtt](/docs/img/tg-config-mqtt.png)
![просмотр контролера](/docs/img/tg-controller-view.png)
![порт настроенный](/docs/img/tg-port-view-out.png)
![порт без конфигурации](/docs/img/tg-port-view-nc.png)

### MQTT conf
Для удалённого доступа клиентов к системе может быть использован Telegram, но функционал в нём не полноценный, хотя Телега всё таки имеет некоторые преимущества в сравнении с штатными удалёнными клиентами.
![Конфигурация MQTT серверной части.](/docs/img/mqtt-conf-server.png)
Помимо стандартных настроек: сервер, порт, логин и пароль существуют дополнительные:
* размер пакета (bytes max) - ограничение на максимальный размер одного сообщения MQTT
* идентификатор клиента - не для авторизации, а для персонализации логов. Серверная часть ведёт логи дейтсвий и для того что бы в этих логах можно было понять от чьего имени была выполнена та или иная команда рекомендуется каждому клиенту дать своё уникальное имя
* постоянное подключение - признак того, что подключение к MQTT должно быть запущено автоматически при старте программы
* шифрование - установка парольной фразы для шифрования всего трафика между серверной частью и удалёнными клиентами (за исключением Telegram). Каждое сообщение/пакет будет зашифровано методом AES/RFC2898 с применением вашей парольной фразы. *Эта настройка должна быть одинаковой для сервера и всех её клиентов (win, android)*
* префикс MQTT - имена топиков будут модифицироваться/дополняться на лету, для того что бы разные группы пользователей могли подключаться к своим удалённым серверам через один общий MQTT сервер не конфликтувя между собой. *Эта настройка должна быть одинаковой для сервера и всех её клиентов*.

### Настройка клиента
Настройки клиента схожи с серверными. Префикс имён топиков, сервер и парольная фраза должны быть идентичными с серверными для согласованности, а логин/пароль могут быть персональными. Идентификатор клиента обязательно должен быть у каждого свой, иначе в логах будет не отличить узлы друг от друга

### Требования
Приложение **.NET MAUIBlazor Hybrid** может быть установлено на Windows или Android устройство. Серверная часть приложения и контроллеры AbLog должны находиться в одной сети

### Ретранслятор
Непосредственная работа с контроллерами выполнена через так называемую ретрансляцию HTTP/HTML. При каждом обращении к контроллеру сервером выполняется обычный HTTP запрос к контроллеру, а из ответа HTML средствами [AngleSharp](https://github.com/AngleSharp/AngleSharp) формируется DOM и уже с ним происходит взаимодейтсвие. Таким образом достигается адапивность на случай если в прошивке контроллера произойдут изменения.
![Ретранслятор HTML](/docs/img/html-retranslator.jpeg).

## Вопросы и ответы
### Конфидициальность/Безопасность
Использование шифрования трафика MQTT парольной фразой обеспечит достаточную анонимнось что бы ни кто не мог увидеть данные, которые ходят между серверной частью и удалённым клиентом. В то же время: серверная часть так же как и удалённый клиент Все данные открыто хранят в своей локлаьной БД. Если злоумышленник получит доступ локальному диску сервера или клиента - он получит доступ в том числе и к логинам/паролям MQTT, что позволит получить полный доступ к данному программному решению. Следует иметь ввиду, что удалённый клиент хранит у себя только настройки MQTT, а всё остальное для него проходит в режиме онлайн через MQTT транспорт.

### Управление правами доступа
Уровни прав регулируются только для TelegramBot доступа. Для него можно отдельно разрешить разные области доступа. Что касается доступ через удалённый клиент, то разграничений по правам нет. Вы можете добавлять/удалять пользователей средствами управления акаунтами MQTT. Технически вы можете везде (на сервере и удалённых клиентах одновременно) использовать один и тот же акаунт MQTT, но тогда для того что бы отключить одного клиента вам прийдётся перенастроить и серверную часть и на всех удалённых клиентах. Если же для каждого узла использовать отдельные логин/пароль, то отключение одного отдельно взятого клиента будет проходить безболезненно. Например hivemq.cloud в бесплатной версии позволяет заводить до сотни акаунтов на сервер.

### База данных
В качестве СУБД используется SQLite. Это касается как удалённого клиента так и сервера. В то же время: удалённый клиент хранит у себя только настройки подключения к MQTT, а остальные настройки хранятся в БД серверной части.

### Использование MQTT
Вы можете использовать любой MQTT сервер, в т.ч. свой собственный. Единственное требование, что бы была поддержка протокола MQTT v.5.

### Особенности и ограничения использования бесплатного MQTT hivemq.cloud
На данный момент бесплатный тарифный план имеет следующие ограничения:
+ 100 одновременных подключений устройств к серверу.
+ 10 GB трафика в месяц.
+ До 3 дней хранения **retention** сообщений.
+ 5 MB максимальный размер одного сообщения.
> 10 гигабайт в месяц - это довольно много, но если вдруг у вас кончится трафик, то вы можете просто удалить и заново создать сервер в личном кабинете hivemq.cloud и счётчик начнётся с нуля. В таком случае вам конечно потребуется сменить настройки сервера и удалённых клиентов. На удалённых клиентах настройки меняются только локально, а серверная часть в дополнении к локлаьному изменению имеет возможнсоть менять эти настрйоки через Telegram.

### Изоляция нескольких контуров в рамках одного сервера MQTT (префиксы топиков)
Если у вас один единственный сервер MQTT, то по умолчанию все клиенты и сервер вместе с ними находятся в едином контуре. Т.е. имена MQTT топиков и подписки на них в рамках одного сервера будут разносить данные из разных контекстов в единую кучу. Избежать этого позволяет использование префиксов MQTT топиков. Благодаря префиксам имена топиков становятся уникальными в зависимости от рабочего контура. Таким образом разные группы пользователей могут работать не мешаю друг другу.

### Предыдущие версии
Это третья версия подобного решения моего авторства.
* Первая версия представляла из себя сервер под Android (Xamarin), а удалённый доступ предоставлялся через TelegramBot. Проект закрыт.
* Вторая версия приложения работала как под андроидом так и под Windows, Mac и Linux. В роли транспортного протокола использовался IMAP. Проект закрыт.
> Первые версии имели ряд критических недостатков (прежде всего в силу выбранных  архитектурных решений/подходов и были окончательно закрыты). Идея хоститься на Android изначально мне очень нравилась, но надёжность хостинга в службах на этих устройствах была непредсказуема/недостаточна. На устройствах некотрых производителей Android устройств (прежде всего Xiaomi и другие китайфоны) работа OS жёстко ограничена и в погоне за максимальной производительностью службы (*Foreground services*) там могли быть внезапно остановлены вопреки ожидаемому поведению, которое официально задукоментировано Android.
Использование протокола IMAP как транспортного так же показало свою ненадёжность в зависимости от почтового хостинга.
