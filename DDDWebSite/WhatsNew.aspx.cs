using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WhatsNew : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            ReportLabel.Text = "";

            ReportLabel.Text += Environment.NewLine + "- 10.06.2012" + Environment.NewLine;
            ReportLabel.Text += @"-  Решены проблемы с DDD-файлами. Создан шаблон для отчетов. В разделе Отчеты отображаются соответствующие отчеты по ранее загруженным DDD-файлам." + Environment.NewLine;
            ReportLabel.Text += @"-  Добавлен индикатор ajax-запросов." + Environment.NewLine;
            ReportLabel.Text += @"-  Добавлена обработка ajax-ошибок и их отображение в случае появления таковых." + Environment.NewLine;
            ReportLabel.Text += @"-  Добавлен баннер в шапке." + Environment.NewLine;
            ReportLabel.Text += @"-  Решены проблемы с форматом времени в Напоминаниях в отправляемых письмах." + Environment.NewLine;
            ReportLabel.Text += @"-  Решены некоторые проблемы с поиском в Журнале Управления." + Environment.NewLine;
            ReportLabel.Text += @"-  Поправлены стили Фильтра Журнала." + Environment.NewLine;
            ReportLabel.Text += @"-  В фильтре Счета добавлен пункт Все." + Environment.NewLine;
            ReportLabel.Text += @"-  Поправлены стили в разделе Общее Управления." + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 28.05.2012" + Environment.NewLine;
            ReportLabel.Text += @"-  Исправлены стили в счетах и журнале в Управлении. Поправлены множество (порядка 10) других недочетов в Управлении." + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 19.05.2012" + Environment.NewLine;
            ReportLabel.Text += @"-  Добавлен раздел Управление. Подробности в письме." + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 02.05.2012" + Environment.NewLine;
            ReportLabel.Text += @"-  Окончательный (последний) вариант дизайна." + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 28.04.2012" + Environment.NewLine;
            ReportLabel.Text += @"-  Возможность выбора различных видов отчетов." + Environment.NewLine;
            ReportLabel.Text += @"-  Поправлен баг с картой (при просмотре маршрута нескольких файлов подряд)." + Environment.NewLine;
            ReportLabel.Text += @"-  Некоторые изменения в дизайне." + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 04.04.2012" + Environment.NewLine;
            ReportLabel.Text += @"-  Поддержка Google Map API v3 в отчетах." + Environment.NewLine;
            ReportLabel.Text += @"-  Изменен дизайн, увеличена рабочая область." + Environment.NewLine;
            ReportLabel.Text += @"-  Устранены проблемы с общей группой, другие небольшие изменения." + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 17.03.2012" + Environment.NewLine;
            ReportLabel.Text += @"-  Новый движок генерации отчетов + экспорт в различные форматы." + Environment.NewLine;
            ReportLabel.Text += @"-  Завершена работа над разделом Напоминания." + Environment.NewLine;
            ReportLabel.Text += @"-  Поправлены некоторые стили." + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 03.03.2012" + Environment.NewLine;
            ReportLabel.Text += @"-  Убрана кнопка экспорта графиков." + Environment.NewLine;
            ReportLabel.Text += @"-  Изменен вид графиков." + Environment.NewLine;
            ReportLabel.Text += @"-  Устранен баг с переходом в другой раздел из раздела Отчеты/PLF Файлы." + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 13.02.2012" + Environment.NewLine;
            ReportLabel.Text += @"-  Завершена работа над разделом Отчеты/PLF Файлы" + Environment.NewLine;
            ReportLabel.Text += @"-  Упрощен ПИ, оставлены только самые востребованные возможности." + Environment.NewLine;
            ReportLabel.Text += @"-  Отображение отчета по выбранному plf-файлу на странице и в виде PDF." + Environment.NewLine;
            ReportLabel.Text += @"-  Отображение графиков по значениям выбранного plf-файла (Скорость, Напряжение, RPM, Уровень топлива)." + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 12.01.2012" + Environment.NewLine;
            ReportLabel.Text += @"-  Отчеты/PLF Файлы переделано дерево водителей." + Environment.NewLine;
            ReportLabel.Text += @"-  Во всех переделанных деревьях убрана рамка вокруг них." + Environment.NewLine;
            ReportLabel.Text += @"-  Решена проблема с размерами иконок в таблицах раздела Архив данных." + Environment.NewLine;
            ReportLabel.Text += @"-  Решена проблема с изменением размеров центральной панели (тот баг, что мы видели, но не могли повторить)." + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 22.12.2011" + Environment.NewLine;
            ReportLabel.Text += @"-  1. Переименованы названия общих настроек в БД с английского на русский." + Environment.NewLine;
            ReportLabel.Text += @"-  2. Завершена работа над разделом Группы: отображение данных и их изменение (Редактировать, Удалить, Создать, Сохранить, Отмена)" + Environment.NewLine;
            ReportLabel.Text += @"-  3. Завершена работа над разделом Водители: отображение данных и их изменение (Редактировать, Удалить, Создать, Сохранить, Отмена)" + Environment.NewLine;
            ReportLabel.Text += @"-  4. Завершена работа над разделом ТС: отображение данных и их изменение (Редактировать, Удалить, Создать, Сохранить, Отмена)" + Environment.NewLine;
            ReportLabel.Text += @"-  5. Завершена работа над разделом Установки по умолчанию: отображение данных и их изменение (Редактировать, Сохранить, Отмена)" + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 18.12.2011" + Environment.NewLine;
            ReportLabel.Text += @"-  Завершена работа над разделом Архив/Загрузить на сервер" + Environment.NewLine;
            ReportLabel.Text += @"  -  Как оказалось файлы после загрузки на сервер сразу не разбираются, они просто добавляются в БД + несколько других записей." + Environment.NewLine;
            ReportLabel.Text += @"  -  Далее все загруженные, но не разобранные файлы отображаются в этом же разделе на центральной панели. Эти файлы можно удалить (если их не надо разбирать) или разобрать их все." + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 02.12.2011" + Environment.NewLine;
            ReportLabel.Text += @"-  Сделан раздел Настройки/Организация/Группы" + Environment.NewLine;
            ReportLabel.Text += @"  -  Все функции (редактировать, удалить, сохранить, отмена) работают" + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 27.11.2011" + Environment.NewLine;
            ReportLabel.Text += @"-  Доработан раздел Архив/Загрузить на сервер" + Environment.NewLine;
            ReportLabel.Text += @"  -  Переделан внешний вид (размер combobox, отображение полей при добавлении водителя, контролы и др.)" + Environment.NewLine;
            ReportLabel.Text += @"  -  Решена проблема валидации полей, сейчас осуществляется проверка всех полей" + Environment.NewLine;
            ReportLabel.Text += @"  -  Устранена проблема, когда в БД добавлялся водитель 'Введите значение! Введите значение!' из-за неверной проверки" + Environment.NewLine;
            ReportLabel.Text += @"  -  Добавлен функционал кнопки 'Отмена', переработан функционал 'Создать'" + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 27.11.2011" + Environment.NewLine;
            ReportLabel.Text += @"-  Доработан раздел Архив/Загрузить на сервер" + Environment.NewLine;
            ReportLabel.Text += @"  -  Переделан внешний вид (размер combobox, отображение полей при добавлении водителя, контролы и др.)" + Environment.NewLine;
            ReportLabel.Text += @"  -  Решена проблема валидации полей, сейчас осуществляется проверка всех полей" + Environment.NewLine;
            ReportLabel.Text += @"  -  Устранена проблема, когда в БД добавлялся водитель 'Введите значение! Введите значение!' из-за неверной проверки" + Environment.NewLine;
            ReportLabel.Text += @"  -  Добавлен функционал кнопки 'Отмена', переработан функционал 'Создать'" + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 14.11.2011" + Environment.NewLine;
            ReportLabel.Text += @"-  Переделаны разделы Просмотреть Водитель и Просмотреть ТС" + Environment.NewLine;
            ReportLabel.Text += @"-  Для тестирования в базе данных есть данные на транспортное средство ТС №135 за период с 01.11.2006 по 01.12.2006, можно пробовать интервалы внутри этого промежутка, можно, конечно, и другие, но скорее всего там не будет данных" + Environment.NewLine;
            ReportLabel.Text += @"-  Произведена оптимизация запроса, сейчас запрос за год в среднем занимает около 8 секунд" + Environment.NewLine;
            ReportLabel.Text += @"-  Произведена оптимизация отображения данных, если Процент данных 0, то соответствующие дни, месяцы не отображаются,
            те. если в ответе на запрос за год 364 дня будут пустыми, они просто не отобразятся, а 1 день с непустым значением отобразится" + Environment.NewLine;
            ReportLabel.Text += @"-  Переделан раздел Настройки/Общие, аналогичным образом будут переделаны остальные разделы" + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 19.10.2011" + Environment.NewLine;
            ReportLabel.Text += @"-  Введена поддержка групп в разделах Просмотреть водителей и Просмотреть ТС" + Environment.NewLine;
            ReportLabel.Text += @"-  Оптимизирована работа с разделами Просмотреть водителей и Просмотреть ТС" + Environment.NewLine;
            ReportLabel.Text += @"-  Дополнена база данных таблицей fn_groups, а также внесены изменения в таблицу fn-cards " + Environment.NewLine;
            ReportLabel.Text += @"-  Исправлен баг с кнопкой Войти" + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 06.10.2011" + Environment.NewLine;
            ReportLabel.Text += @"-  Завершен п.3, можно выбирать 'Водители' и 'Транспортные средства', при этом отображается информация дочерних узлов" + Environment.NewLine;
            ReportLabel.Text += @"  -  Почта теперь отправляется от office@smartfis.ru" + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 02.10.2011" + Environment.NewLine;
            ReportLabel.Text += @"- Вариант новой архитектуры request/response (чистый AJAX + чистая js генерация выходных результатов) в рамках п.3.
см. закладку Архив данных/Восстановить у пользователя. Подробности в письме. " + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 23.09.2011" + Environment.NewLine;
            ReportLabel.Text += @"- Работы над процедурой восстановления пароля." + Environment.NewLine;
            ReportLabel.Text += @"  - Устранены баги." + Environment.NewLine;
            ReportLabel.Text += @"  - Протестирована процедура отправки письма (с реальным адресом)." + Environment.NewLine;
            ReportLabel.Text += @"  - Изменено содержание письма, которое приходит пользователю:
            <Имя пользователя>,

            Вы запросили напоминание Вашего пароля на сайте SmartFis.ru.
            Если Вы этого не делали, проигнорируйте это письмо.

            Ваш пароль: <пароль>

            Желаем удачи!

            С уважением,
            Администрация SmartFis.ru." + Environment.NewLine;
            ReportLabel.Text += @"  - При нажатии 'Отправить' кнопка становится неактивной до получения результата от сервера." + Environment.NewLine;
            ReportLabel.Text += @"  - Изменено отображение ошибок." + Environment.NewLine;
            ReportLabel.Text += @"  - При повторной ошибке ввода e-mail появление сообщения об ошибке теперь заметно." + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 19.09.2011" + Environment.NewLine;
            ReportLabel.Text += @"- Выполнен п.2." + Environment.NewLine;
            ReportLabel.Text += @"  - Изменен размер кнопки." + Environment.NewLine;
            ReportLabel.Text += @"  - 'Запомнить меня' выровнена относительно checkbox." + Environment.NewLine;
            ReportLabel.Text += @"  - При нажатии 'Войти' кнопка становится неактивной до получения результата от сервера." + Environment.NewLine;
            ReportLabel.Text += @"  - Изменено отображение ошибок." + Environment.NewLine;
            ReportLabel.Text += @"  - При повторной ошибке ввода логина/пароля появление сообщения об ошибке теперь заметно." + Environment.NewLine;
            ReportLabel.Text += @"  - Password Recovery -> Восстановление пароля." + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 16.09.2011" + Environment.NewLine;
            ReportLabel.Text += @"- Залита 1-ая пробная версия." + Environment.NewLine;
            ReportLabel.Text += @"- Разбор структуры БД." + Environment.NewLine;
            ReportLabel.Text += @"- Работа над п.1." + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 29.06.2011" + Environment.NewLine;
            ReportLabel.Text += @"- На странице Настройки в закладке Напоминания появился пункт Отправка отчетов на Email. Он позволяет настроить отправку выбранных отчетов на электронную почту каждый заданный промежуток времени." + Environment.NewLine;
            ReportLabel.Text += @"- Пока работают только отчеты для водителей и формат PDF." + Environment.NewLine;
            ReportLabel.Text += @"- Работает редактирование и удаление расписаний." + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 25.06.2011" + Environment.NewLine;
            ReportLabel.Text += @"- В окне входа теперь есть поле Профиль." + Environment.NewLine;
            ReportLabel.Text += @"- Тестируется фоновая работа сайта без пользователя.Файл CacheCallback.txt в корне ";
            ReportLabel.Text += @"обновляется ~каждую минуту. Стандартные средства не позволяют запускать какой-либо метод по-расписанию ";
            ReportLabel.Text += @"без выделенного сервера и установленного Microsoft Services. На нашем сайте с помошью кэша сервера вызываются ф-кции. Запись добавляется при старте, ";
            ReportLabel.Text += @"или при смерти старой записи кэша и запускается работа в отдельном потоке. Пока это только обновление файла CacheCallback.txt" + Environment.NewLine;
            ReportLabel.Text += @"- Далее можно использовать эту возможность для рассылки почты, проведения сервиса(обработки базы данных, сортировки записей и тд.)." + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 12.06.2011" + Environment.NewLine;
            ReportLabel.Text += @"- Доделал окно входа. Работает восстановление пароля." + Environment.NewLine;
            ReportLabel.Text += @"- На регистрации просто переходит на сайт smartfis.info в раздел Контакты, ничего заполнять автоматически нельзя." + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 27.06.2011" + Environment.NewLine;
            ReportLabel.Text += @"- Статистика по загруженным файлам работает с ПЛФ, ДДД водителя, ДДД Транспортного средства." + Environment.NewLine;
            ReportLabel.Text += @"- Можно выбрать как единичный файл, так и группу, или организацию." + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 27.06.2011" + Environment.NewLine;
            ReportLabel.Text += @"- Статистика по загруженным файлам работает с ПЛФ, ДДД водителя, ДДД Транспортного средства." + Environment.NewLine;
            ReportLabel.Text += @"- Можно выбрать как единичный файл, так и группу, или организацию." + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 24.06.2011" + Environment.NewLine;
            ReportLabel.Text += @"- Архив данных -> Просмотреть(Водитель) -> Выбрать любой ДДД(!) файл -> Кнопка Статистика загруженных файлов." + Environment.NewLine;
            ReportLabel.Text += @"- В появившемся окне можно просмотреть, какой процент загруженной информации относительно всего времени. За год, месяц и каждый день." + Environment.NewLine;
            ReportLabel.Text += @"- Можно выбрать Год, месяц. Пока работает с единичными ДДД файлами, далее будет работать со всеми файлами водителя/ТС сразу" + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 20.06.2011 - 21.06.2011" + Environment.NewLine;
            ReportLabel.Text += @"- Информация о загруженные файлах ДДД транспортных средств вся. работает частичное обновление страницы." + Environment.NewLine;
            ReportLabel.Text += @"- Закладка Восстановить у пользователя - работает." + Environment.NewLine;
            ReportLabel.Text += @"- " + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 16.06.2011" + Environment.NewLine;
            ReportLabel.Text += @"- Информация о загруженные файлах ДДД транспортных средств. Пока не полная." + Environment.NewLine;
            ReportLabel.Text += @"- " + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 15.06.2011" + Environment.NewLine;
            ReportLabel.Text += @"- Доработал просмотр загруженных файлов водителей." + Environment.NewLine;
            ReportLabel.Text += @"- " + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 14.06.2011" + Environment.NewLine;
            ReportLabel.Text += @"- Вложения приходят на почту нормально." + Environment.NewLine;
            ReportLabel.Text += @"- В закладке Архив данных Можно просмотреть информацию о загруженных файлах у водителей(PLF и ДДД, ДДД пока немного не хватает)." + Environment.NewLine;
            ReportLabel.Text += @"- " + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 10.06.2011" + Environment.NewLine;
            ReportLabel.Text += @"- Экпортированные файлы отправляются по электронной почте." + Environment.NewLine;
            ReportLabel.Text += @"- " + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 8.06.2011" + Environment.NewLine;
            ReportLabel.Text += @"- Экспорт работает для большинства форматов, выбирается из меню(можно еще больше, если это необходимо). Также есть фильтр, какие страницы экспортировать." + Environment.NewLine;
            ReportLabel.Text += @"- " + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 6.06.2011" + Environment.NewLine;
            ReportLabel.Text += @"- Кнопка Экспорт при загруженном отчете сохраняет этот отчет в PDF.(пока только PDF)" + Environment.NewLine;
            ReportLabel.Text += @"- " + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "-------------------------------------------------" + Environment.NewLine;
            ReportLabel.Text += Environment.NewLine + "- 26.05.2011" + Environment.NewLine;
            ReportLabel.Text += @"- Аккаунты, в аккордионе дерево." + Environment.NewLine;
            ReportLabel.Text += @"- Прокрутка не появляется в FireFox, также отображается нижняя полоса." + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 25.05.2011" + Environment.NewLine;
            ReportLabel.Text += @"- Доделал резиновый по высоте сайт." + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 22.05.2011" + Environment.NewLine;
            ReportLabel.Text += @"- Первая версия резинового по высоте сайта." + Environment.NewLine;


            ReportLabel.Text += Environment.NewLine + "- 19.05.2011" + Environment.NewLine;
            ReportLabel.Text += @"- Выбор во всех таблицах(RadioButton) срабатывает без задержек на стороне клиента." + Environment.NewLine;
            ReportLabel.Text += @"- Пробовал сделать резиновый сайт - похоже нужно будет переверстывать сильно все, если делать так, как в интернете советуют. Сходу не получилось сделать..." + Environment.NewLine;


            ReportLabel.Text += Environment.NewLine + "- 17.05.2011" + Environment.NewLine;
            ReportLabel.Text += @"- Переделал страницу с отчетами." + Environment.NewLine;
            ReportLabel.Text += @"- Вместо Клиентов и Дилеров - закладка аккаунты, в ней все сразу." + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 11.05.2011" + Environment.NewLine;
            ReportLabel.Text += @"- Появилась закладка - клиенты. Это организации - конечные пользователи продукта. У дилеров, субдилеров и Главной организации будут свои" + Environment.NewLine;
            ReportLabel.Text += @"- Можно сделать что-то вроде: Создать дилера, создать субдилера, и у субдилера создать несколько клиентов." + Environment.NewLine;


            ReportLabel.Text += Environment.NewLine + "- 10.05.2011" + Environment.NewLine;
            ReportLabel.Text += @"- Появились субдилеры. Максимальная вложенность - Главная организация(я назавал ее ПреДилер) -> Дилер -> Субдилер, у всех могут быть свои клиенты(организации)" + Environment.NewLine;
            ReportLabel.Text += @"- Теперь у дилеров видны только дилером созданные субдилеры, у субдилеров нету закладки с дилерами(тк субдилеры не могут создавать субдилеров)" + Environment.NewLine;


            ReportLabel.Text += Environment.NewLine + "- 30.04.2011 - 4.05.2011" + Environment.NewLine;
            ReportLabel.Text += @"- Переделал подключения к базе данных при генерации отчетов(а также по мелочи еще). Должно быть стабильнее и быстрее." + Environment.NewLine;
            ReportLabel.Text += @"- Написал перенос базы в FireBird." + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 27.04.2011" + Environment.NewLine;
            ReportLabel.Text += @"- Панель в файрфоксе работает." + Environment.NewLine;
            ReportLabel.Text += @"- Исправил неточности, при которых не запускались отчеты." + Environment.NewLine;
            ReportLabel.Text += @"- Сделал все таблицы закругленные, как надо." + Environment.NewLine;
            ReportLabel.Text += @"- С синей кнопкой работает валидация и отключается когда надо джаваскрипт." + Environment.NewLine;
            ReportLabel.Text += @"- В закладке данные теперь нормально работает кнопка разобрать файлы(иногда не становилась неактивной, а неактивная все равно выполняла джава-скипт)." + Environment.NewLine;
            ReportLabel.Text += @"- Исправил некоторые ошибки." + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 26.04.2011" + Environment.NewLine;
            ReportLabel.Text += @"- Написал свою кнопку, теперь видно ее везде, а не только в файрфоксе." + Environment.NewLine;
            ReportLabel.Text += @"- Выход не просто редеректит на страницу входа, а делает выход пользователя." + Environment.NewLine;
            ReportLabel.Text += @"- Переделал внешний вид выбора типа отчета(еще подумаю, что можно сделать)." + Environment.NewLine;
            ReportLabel.Text += @"- Учел некоторые замечания(еще осталось пару нюансов)." + Environment.NewLine;
            ReportLabel.Text += @"- Добавил как пример панель для доп кнопок в разделе отчеты. Правда в файрфоксе работать не хочет(это решаемо)." + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 25.04.2011" + Environment.NewLine;
            ReportLabel.Text += @"- Поменял поиск(выпадающий список) в закладке Отчеты. Решена проблема, когда кнопка выпадающего списка с поиском пропадала при переключении закладок аккордиона." + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 21.04.2011" + Environment.NewLine;
            ReportLabel.Text += @"- Внешний вид всех элементов привел к общей концепции" + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 19.04.2011" + Environment.NewLine;
            ReportLabel.Text += @"- Панель для отчета растягивается вместе с отчетом всегда, а не только при создании отчета." + Environment.NewLine;
            ReportLabel.Text += @"- Фотографии опять обновляются." + Environment.NewLine;
            ReportLabel.Text += @"- Почти везде сделал частичное обновление(Ajax), без него в новом дизайне не работало вертикальное меню(работало неправильно)." + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 18.04.2011" + Environment.NewLine;
            ReportLabel.Text += @"- Переделал Вертикальное меню" + Environment.NewLine;
            ReportLabel.Text += @"- Пока возможны нюансы работы с этим меню, нужно тестировать." + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 17.04.2011" + Environment.NewLine;
            ReportLabel.Text += @"- Учтены некоторые ошибки и недостатки в моей в верстке" + Environment.NewLine;
            ReportLabel.Text += @"- При нажатии кнопок управления отчетов(зум, след страница и др) график никуда не пропадает." + Environment.NewLine;
            ReportLabel.Text += @"- Панель для отчета растягивается вместе с отчетом." + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 14.04.2011" + Environment.NewLine;
            ReportLabel.Text += @"- Подключил к сайту разработаный шаблон" + Environment.NewLine;
            ReportLabel.Text += @"- Только вертикальное меню пока не того цвета, скоро поправлю." + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 13.04.2011" + Environment.NewLine;
            ReportLabel.Text += @"- Исправлен вход по нажатию Enter в FireFox" + Environment.NewLine;
            ReportLabel.Text += @"- Добавился фильтр по пользователям в закладке Управление" + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 12.04.2011" + Environment.NewLine;
            ReportLabel.Text += @"- Добавлен вход по нажатию кнопки Enter на странице LoginPage" + Environment.NewLine;
            ReportLabel.Text += @"- Также по нажатию Enter будет производится поиск в журнале(и в других местах, когда там будет поиск)." + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 8.04.2011 - 11.04.2011" + Environment.NewLine;
            ReportLabel.Text += @"- Переписал работу с базой данных. Открытие закрытие подключений и тд. Должно работать быстрее и стабильнее(теоретически)" + Environment.NewLine;
            ReportLabel.Text += @"- Добавил фильтр По времени, дате, типу действия и по любому слову в Журнале." + Environment.NewLine;
            ReportLabel.Text += @"- Сделал на этот фильтр продвинутую валидацию, потом везде так сделаю." + Environment.NewLine;
            ReportLabel.Text += @"- Решил ошибку, когда в один момент времени не могли добавиться две одинаковые записи в журнал(например изменение доступа к отчету нескольким ролям одновременно, то есть выбрали роль одну, вторую и нажали сохранить)." + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 7.04.2011" + Environment.NewLine;
            ReportLabel.Text += @"- В закладке счета выводятся счета(при добавлении в организацию нового отчета, автоматом выставляется счет)." + Environment.NewLine;
            ReportLabel.Text += @"- Счета оплачиваются. при этом добавляется запись в журнал, что счет оплачен." + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 5.04.2011 - 6.04.2011" + Environment.NewLine;
            ReportLabel.Text += @"- Создаются вручную отчеты И типы отчетов пользователя на странице SuperAdministrator. " + Environment.NewLine;
            ReportLabel.Text += @"- Выводятся отчеты в Управление->Отчеты. " + Environment.NewLine;
            ReportLabel.Text += @"- Добавляются отчеты организации, за это автоматом выставляется счет. " + Environment.NewLine;
            ReportLabel.Text += @"- Для ролей пользователей можно менять доступ к отчетам. С ролями пока неясно, потому что то, для чего нужно на самом деле менять доступ к отчетам называется ТИП ПОЛЬЗОВАТЕЛЯ. Но версия в разработке." + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 30.03.2011" + Environment.NewLine;
            ReportLabel.Text += @"- В списке пользователей в закладке Управление вместо ??? - название дилера" + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 29.03.2011" + Environment.NewLine;
            ReportLabel.Text += @"- Добавляются фотографии для ТС" + Environment.NewLine;
            ReportLabel.Text += @"- Журнал для действий пользователя работает с учетом изменений в БД, с комментариями" + Environment.NewLine;
            ReportLabel.Text += @"- Написал методы для работы с таблицами доп. информации по ТС. Куда и сохраняется сейчас адрес фотографии." + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 22.03.2011 - 25.03.2011" + Environment.NewLine;
            ReportLabel.Text += @"- Добавляются фотографии для водителей. Для ТС тоже все будет после обновления БД" + Environment.NewLine;
            ReportLabel.Text += @"- Написаны методы работы с таблицами Истории действий пользователей в БД" + Environment.NewLine;
            ReportLabel.Text += @"- Некоторые действия пользователей уже записываются в журнал. Требует тестирования еще." + Environment.NewLine;
            ReportLabel.Text += @"- Страница Логина рабочая." + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 21.03.2011" + Environment.NewLine;
            ReportLabel.Text += @"- Проблему с переключением закладок решил." + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 18.03.2011 - 21.03.2011" + Environment.NewLine;
            ReportLabel.Text += @"- Добавил проверку полей при редактировании водителей и ТС в Настройках и в управлении Дилеры и Пользователя проверяются вводимые данные. Проблема только, когда хочется переключится в разьезжающемся меня на закладку Дилеры из закладки ПОльзователи, то если страница не прошла валидацию, переключения не будет. Вот пытаюсь решить. Вообще, пока не будут выполнены условия(необходимые поля не будут введены и тд) странице запрещается делать запрос на сервер. Работает только ДжаваСкрипт, вот в нем хочу отключать валидацию при нажатии на закладку в меню, чтобы переключалось в любом случае" + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 17.03.2011" + Environment.NewLine;
            ReportLabel.Text += @"- Добавил валидацию поля ""Имя пользователя"" на странице входа в систему." + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 15.03.2011 - 16.03.2011" + Environment.NewLine;
            ReportLabel.Text += "- Решил проблему с дублированием строк. Теперь все неиспользуемые удаляются автоматически, а одинаковые строки используемые в нескольких местах превратились в одну строку которая везде используется. Сделал редактирование хорошее, отладил и протестировал, должно работать как минимум стабильнее и лучше, чем было. Нужно конечно еще потестировать." + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 12.03.2011 - 14.03.2011" + Environment.NewLine;
            ReportLabel.Text += "- Управление - > Дилеры + появилась верстка первой страницы(входа). ПОявилась проблема с дублированием строк, и потом с редактированием их в таблице строк. Пока не сделал нормально.." + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 11.03.2011" + Environment.NewLine;
            ReportLabel.Text += "- Управление - > Пользователи. Работает все, только пока удаления пользователей нету, валидации полей и дилеров." + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 4.03.2011" + Environment.NewLine;
            ReportLabel.Text += "- Перенес весь сайт на шаблон, который мне прислали." + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 3.03.2011" + Environment.NewLine;
            ReportLabel.Text += "- В управление-общие сведения_ появилась возможность сохранять информацию. Записи о доп.информации по пользователям и организациям в соответсвующий справочник добавляется автоматически. То есть справочник формируется динамически." + Environment.NewLine;
            ReportLabel.Text += "- Если на разбор приходит карта не водителя(проверяющего или мастерской например), статус записи становится Not supported, и при попытках разбора выдает соответсвующие сообщения. Также добавил возможность удалять неразобранный файл. Вот например Not Supported который." + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 2.03.2011" + Environment.NewLine;
            ReportLabel.Text += "- В закладке Коэффициенты можно просмотреть и поправить коэффициенты. Можно ли их редактировать пользователям(если да,то нужно добавить разные коэффициенты для разных предприятий) и те ли это коэффициенты?" + Environment.NewLine;
            ReportLabel.Text += "- Закладка управление, общие сведения." + Environment.NewLine;


            ReportLabel.Text += Environment.NewLine + "- 1.03.2011" + Environment.NewLine;
            ReportLabel.Text += "- Сохраняется доп информация по водителю." + Environment.NewLine;
            ReportLabel.Text += "- Водитель создается вручную(пока не валидируются поля)" + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 28.02.2011" + Environment.NewLine;
            ReportLabel.Text += "- Исправил ошибки с загрузкой файлов." + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 24.02.2011" + Environment.NewLine;
            ReportLabel.Text += "- Редактируются все критерии для ТС" + Environment.NewLine;
            ReportLabel.Text += "- Можно создать новое ТС руками(если есть такая необходимость)" + Environment.NewLine;
            ReportLabel.Text += "- Пока не работает валидация. Критерии имеют верхнюю и нижнюю границы, в которых должны быть значения этих критериев. Вот делаю такую валидацию, чтобы сообщалось, если значение введено за пределами диапазона. Ну и конечно где цифры нельзя буквы, ограниченная размерность поля для ввода и тд." + Environment.NewLine;
            ReportLabel.Text += "- По водителям выводится только коментарий и номер карты пока." + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 23.02.2011" + Environment.NewLine;
            ReportLabel.Text += "- Сохранение информации о водителях и ТС" + Environment.NewLine;
            ReportLabel.Text += "- Таблица связи пользователя и ТС работает(надо тестить)." + Environment.NewLine;
            ReportLabel.Text += "- Исправил загрузку PLF(после изменений перестало работать)." + Environment.NewLine;
            ReportLabel.Text += "- Подготовил страничку для теста." + Environment.NewLine;
            ReportLabel.Text += "- Буду делать тест на LoadStorm.com. Пока нравится, как все сделано. Отпишу о результатах." + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 22.02.2011" + Environment.NewLine;
            ReportLabel.Text += "- Описана работа с таблицами fd_device, fd_device_type, fd_device_firmware" + Environment.NewLine;
            ReportLabel.Text += "- При добавлении ТС, каждому присваивается свое бортовое устройство." + Environment.NewLine;
            ReportLabel.Text += "- Типы бортовых устройств можно добавлять в администрировании." + Environment.NewLine;
            ReportLabel.Text += "- Информация выводится в настройках ТС." + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 21.02.2011" + Environment.NewLine;
            ReportLabel.Text += "- Больше информации о ТС." + Environment.NewLine;
            ReportLabel.Text += "- " + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 18.02.2011" + Environment.NewLine;
            ReportLabel.Text += "- На транспортные средства заводятся карточки,  к карточкам создаются таблицы ТС.(обновлена база данных и работа с базой данных)" + Environment.NewLine;
            ReportLabel.Text += "- Свои карточки у каждой организации." + Environment.NewLine;
            ReportLabel.Text += "- Неразобранные файлы тоже у каждой организации свои." + Environment.NewLine;
            ReportLabel.Text += "- При разборе, блокирую экран так же, как при генерации отчетов." + Environment.NewLine;
            ReportLabel.Text += "- Критерии для ТС в закладке настройки можно редактировать, информация сохраняется в БД. Критериев пока мало." + Environment.NewLine;
            ReportLabel.Text += "- Для удобства написал клас для инициализцации БД, чтобы руками не вводить необходимые для запуска значения(справочники стран и регионов, типов карт, ТС и тд). Все остальное можно ввести уже с сайта" + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 15.02.2011" + Environment.NewLine;
            ReportLabel.Text += "- Обновил работу со строчками(таблица FN_STRING). Теперь не будет дублирования.(нужно еще потестировать)" + Environment.NewLine;
            ReportLabel.Text += "- Критерии и настройки ТС обновляются, редактируются." + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 14.02.2011" + Environment.NewLine;
            ReportLabel.Text += "- При создании нового транспортного средства(пока только при загрузке файла с новым ТС, создается таблица и все соответсвующие таблицы нового транспортного средства." + Environment.NewLine;
            ReportLabel.Text += "- Создаются критерии для транспортного средства и им присваиваются начальные значения(пока критерии только выводятся)." + Environment.NewLine;
            ReportLabel.Text += "- На странице администрирования(логин admin2) добавилась закладка Справочник критериев/ед.измерения. Там можно внести единицы измерения в справочник, а также создать необходимые критерии для ТС. Чтобы критерии ,необходимые для ТС, добавлялись автоматически, нужно в базе что-то сделать, чтобы можно было определить, какие критерии к чему относятся." + Environment.NewLine;
            ReportLabel.Text += "- Редактирование/добавление нового ТС руками пока не работает." + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 8.02.2011" + Environment.NewLine;
            ReportLabel.Text += "- Перенес сайт в папку WebService. Набирать теперь надо http://smartfis.ru/WebService/loginPage.aspx" + Environment.NewLine;
            ReportLabel.Text += "- ТС теперь так же как и водители, создаются независимо от карт загруженных и могут содержать блоки данных с информацией(а могут и не содержать). Добавляется новое ТС в базу, если загружается DDD файл от ТС, которого еще не было, либо информация добавляется к уже существуещему ТС." + Environment.NewLine;
            ReportLabel.Text += "- У каждой организации свои ТС. То есть у пользователя из организации 1 будут видны только загруженные пользователями из этой организации файлы, а у пользователя из организации 2, только файлы организации 2." + Environment.NewLine;
            ReportLabel.Text += "- В базу данных добавилось поле Vehicle_ID в таблицу FN_DATA_BLOCK, указывающее на ТС, которому приндлежит информация" + Environment.NewLine;
            ReportLabel.Text += "- Вывод информации тоже соответственно изменился" + Environment.NewLine;
            ReportLabel.Text += "- Добавил возможность выборки сразу нескольких файлов ТС за период(как было в PLF)" + Environment.NewLine;


            ReportLabel.Text += Environment.NewLine + "- 4.02.2011" + Environment.NewLine;
            ReportLabel.Text += "- Теперь для пользователей с разными типами пользователя разные возможности. Для сравнения для пользователя с логином admin(тип Administrator доступно все, для пользователя с логином admin1(тип driver) доступна только кнопка отчеты и тд. Типы пользователям можно менять в админке." + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 3.02.2011" + Environment.NewLine;
            ReportLabel.Text += "- Переделал выборку информации по транспортному средству" + Environment.NewLine;
            ReportLabel.Text += "- Доработал подсчет времени, длительности и тд всех отчетов, написал отдельный класс для представления времени в ДДД, и добавил везде. Теперь подсчет всего выполняет этот класс." + Environment.NewLine;
            ReportLabel.Text += "- Доработал отчеты(результаты в основном)." + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 31.01.2011" + Environment.NewLine;
            ReportLabel.Text += "- Сделал расчет недели по-другому принципу. Работает быстрее, можно выбрать с какого дня неделя начинается, плюс устранил ошибки, потому что раньше были неточности." + Environment.NewLine;
            ReportLabel.Text += "- Переделал расчет времени длительности и активности водителей и ТС. Теперь оно должно быть точнее и без багов" + Environment.NewLine;
            ReportLabel.Text += "- Занимаюсь обработкой информации от водителей, потому что появились нюансы в работе. Сейчас итоги в отчетах водителей неправильные, потому что информацию идет и по водителю и по напарнику вместе. Вот ее надо разграничить..." + Environment.NewLine;
            

            ReportLabel.Text += Environment.NewLine + "- 26.01.2011" + Environment.NewLine;
            ReportLabel.Text += "- Переделал немного все отчеты(внешний вид)." + Environment.NewLine;
            ReportLabel.Text += "- Скачал новые примеры отчетов VDO(они теже самые, только выглядят по-другому), буду доделывать под новые." + Environment.NewLine;
            ReportLabel.Text += "- Новый отчет. Транспортное средство - Активность ТС" + Environment.NewLine;
            

            ReportLabel.Text += Environment.NewLine + "- 25.01.2011" + Environment.NewLine;
            ReportLabel.Text += "- Сделал свое управление отчетом, убрал стандартное(учел все пожелания)." + Environment.NewLine;
            ReportLabel.Text += "- С шириной пока не определился. Сделать можно. Разрешение экрана получаю, по процентам пока не подогнал." + Environment.NewLine;
            

            ReportLabel.Text += Environment.NewLine + "- 24.01.2011" + Environment.NewLine;
            ReportLabel.Text += "- Убрал моргания кнопки генерировать отчет. Переделал обновление частей экрана, работает получше." + Environment.NewLine;
            ReportLabel.Text += "- Добавил всплывающее окно с прогресом на генерацию отчета." + Environment.NewLine;
            ReportLabel.Text += "- Сделал отчет по ширине, но не уверен, что работает, потому что проверить не могу на широком экране..." + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 20.01.2011" + Environment.NewLine;
            ReportLabel.Text += "- Переделал отчет по PLF." + Environment.NewLine;
            ReportLabel.Text += "- Добавил отчет по активности ТС." + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 19.01.2011" + Environment.NewLine;
            ReportLabel.Text += "- Убрал задержки между переключениями(особенно когда был загружен отчет на 200 страниц, задержка могла быть секунд 20 каждый клик по кнопке, которая вызывала обновление чего-нибудь). Теперь за раз загружается только одна страница отчета, и при запросе переключения страницы отчета только тогда новая создается. Это ускорило загрузку отчета, уменьшило трафик, и при постбеке была задержка, потому что весь отчет туда сюда гонялся все время(от клиента к серверу и обратно))" + Environment.NewLine;
            ReportLabel.Text += "- Начали работать кнопки Зум(там где проценты выбираются), а также выбор One page - Whole Report, показывать одну страницу или весь отчет." + Environment.NewLine;
            ReportLabel.Text += "- Заодно пробовал сделать печать и сохранение в другие форматы, но пока работать не захотело" + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 18.01.2011" + Environment.NewLine;
            ReportLabel.Text += "- Информация по ТС выбирается за период." + Environment.NewLine;
            ReportLabel.Text += "- Несколько карт по одному ТС за период" + Environment.NewLine;
            ReportLabel.Text += "- Новый вид выбора отчетов ТС."+ Environment.NewLine;
            ReportLabel.Text += "- Добавлен отчет Транспортное средсво - Превышение скорости" + Environment.NewLine;
            ReportLabel.Text += "- Добавлена поддержка AJAX на все выборы(водителей, групп и тс). Теперь ничего не моргает." + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 17.01.2011" + Environment.NewLine;
            ReportLabel.Text += "- Информация по водителям выбирается за период(не было такого раньше), а также несколько карт по одному водителю за период(правда тут только в теории, потому что потестить нету возможности, так как не хватает файлов карточек)" + Environment.NewLine;
            ReportLabel.Text += "- Переделан выбор типов отчетов Групп водителей." + Environment.NewLine;
            ReportLabel.Text += "-  Добавлен отчет Группа водителей - Итоги по активности" + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 14.01.2011" + Environment.NewLine;
            ReportLabel.Text += "- Сделал полностью отчеты Ежедневный/еженедельный протокол активности и Использование ТС. Переделал во многом формирование информации, чтобы подходило для генератора отчетов, и написал уже все итоги по отчетам." + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 12.01.2011" + Environment.NewLine;
            ReportLabel.Text += "- Переделан интерфейс выбора отчетов по водителям(первая закладка)" + Environment.NewLine;
            ReportLabel.Text += "- Добавлены отчеты ежедневый протокол активности и еженедельный протокол активности" + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 5.01.2011" + Environment.NewLine;
            ReportLabel.Text += "- График по ширине страницы" + Environment.NewLine;
            ReportLabel.Text += "- Временные файлы удаляются всегда(оказалось что так было не всегда)" + Environment.NewLine;
            ReportLabel.Text += "- Нету больше лесенки на графиках(нужно тестить конечно)" + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 5.01.2011" + Environment.NewLine;
            ReportLabel.Text += "-Переделал немного внешний вид" + Environment.NewLine;
            ReportLabel.Text += "-Теперь есть только один график, и на него выбираем оси, осей может быть сколько угодно"+Environment.NewLine;
            ReportLabel.Text += "-Добавил несколько новых графиков(осей)" + Environment.NewLine;
            ReportLabel.Text += "-Теперь при переключении менюшек и тд адекватнее появляются и исчезают график и отчет" + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 4.01.2011" + Environment.NewLine;
            ReportLabel.Text += "- Терерь не тормозит при наведении на скроллер в графике(это полоса прокрутки снизу). Так же решился вопрос со сглаживанием графиков. В корне есть файл web.config, там теперь есть запись с настройкой, сколько точек одновременно будет видно в графике(называется AnyChartMaxVisiblePoints, сейчас установлено 3000, можно поиграть с этим параметром, измерить производительность на разных ПК, потому что если много точек, начинает тормозить при наведении на окошко графика)" + Environment.NewLine;
            ReportLabel.Text += "- Увеличилась скорость появления графиков(загрузки раз в 20, то есть решил проблему, о которой писал в скайп). Удаление временных файлов теперь не на таймере, а вызывается скриптом." + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 3.01.2011" + Environment.NewLine;
            ReportLabel.Text += "- Теперь строятся все одиночные графики с помощью AnyChart, а также график с тремя осями(последний). Все еще одновременно работает только один график. 3х осевой еще настроится, лучше будет выглядеть и вести себя." + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 30.12.2010" + Environment.NewLine;
            ReportLabel.Text += "- Освоил передачу данных сгенерированных на Сервере графику AnyChart. Графики Уровень топлива, RPM Уже на AnyChart. След раз посмотрю внимательно настройки и сделаю, чтобы можно было извлекать из графика максимум информации и освою несколько осей и несколько графиков(также тут есть возможность сглаживать графики, использовать разные виды приближений и тд). Пока одновременно ток один график." + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 28.12.2010" + Environment.NewLine;
            ReportLabel.Text += "- Добавил поддержку AnyChart Stock. Выбор информации два раза не проходит(как в AnyCharts, достаточно одного). Для нормальной работы создаются временные файлы(Xml), которые потом удаляются автоматически, по прошествию времени(сейчас 30 секунд, может нужно будет менять время)." + Environment.NewLine;
            ReportLabel.Text += "- Пока не разобрался с форматом входных данных, так что выводится только пример(файлы все равно генерируются, но пока при использовании их в качестве источника данных для графика выводится пустое окно без графика(что-то не то пока что, надо разобратся))" + Environment.NewLine;
            ReportLabel.Text += "- Переверстал немного шаблон, теперь лучше изменяется размер страницы." + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 24.12.2010" + Environment.NewLine;
            ReportLabel.Text += "- Поменял принцип выборки информации по периоду. Это ускорило генерацию отчетов очень заметно(раза в два я думаю)." + Environment.NewLine;
            ReportLabel.Text += "- Поменял принцип открытия соединения с базой(теперь это делается один раз для генерации отчета), это еще ускорило работу." + Environment.NewLine;
            ReportLabel.Text += "- Оптимизировал подсчет средних значений/процентов и тд(это делается один раз и потом вносятся поправки в зависимости от колл-ва записей(тоже немного ускорило))." + Environment.NewLine;
            ReportLabel.Text += "- По-мелочам поправил функции, участвующие в генерации отчетов. Максимально уменьшил количество локальных промежуточных переменных и Тд. Незнаю, насколько это ускорило работу, но память сервера точно будет экономить, даже если совсем чуть-чуть..." + Environment.NewLine;
            ReportLabel.Text += "- В итоге проделанной работы время генерации отчета по самому большому файлу(тот который 25 мегабайт, 1 600 000 записей) уменьшилось с 3х минут до 30 секунд. Если провести индексацию правильную таблицы с данными, думаю результат будет еще лучше..." + Environment.NewLine;
            ReportLabel.Text += "- Исправил ошибки в проверке блоков данных по датам.(теперь данные за период выбираются стабильно, раньше были ошибки, и могло ничего не выбраться)" + Environment.NewLine;
            ReportLabel.Text += "- Теперь эничарт работает асинхронно(зато синхронно не работает =) ). Вообще в эничарт нужно все делать руками(вроде Асинхронного обновления). Зато я даволен, как это реализовал))) Пока могут быть конечно вопросы по работе Anychart. Но вообще он не хуже FusionCharts, может только не такой удобный. позже сделаю его для всех графиков, быстрее и добавлю Зум." + Environment.NewLine;
            ReportLabel.Text += "- Поправил проблемы с ошибкой Нет такого водителя." + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 23.12.2010" + Environment.NewLine;
            ReportLabel.Text += "- Появился график с эничарт. Нужно выбрать любое ТС и кликнуть на галочку RPM в выборе графиков. Работает только синхронно и асинхронно не хочет обновлятся(надеюсь что пока), то есть нужно нажать кнопку Генерировать Синхронно. Самый заметный плюс AnyChart - никаких проблем, если точек много. Зум сделан очень посредственно, не так как в Fusion Charts, можно просто кликнуть на определенный заранее отрезок и он приблизится, и на кнопку отдалить обратно вернется. При программировании всех этих событий появляется много ограничений(которых может быть удастся все таки избежать)." + Environment.NewLine;
            ReportLabel.Text += "- Можно выбирать как отдельные файлы так и ТС(все файлы за раз.)" + Environment.NewLine;
            ReportLabel.Text += "- Скоро доделаю Зум и несколько осей, чтобы можно было точно сравнить." + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 22.12.2010" + Environment.NewLine;
            ReportLabel.Text += "- Таки заставил работать генератор графиков AnyChart. С параметрами из програмного кода. Посмотреть можно тоже только в тестовом режиме в закладке Помощь. Пока ничего не настраивал, поэтому выглядит совсем не так как будет в итоге." + Environment.NewLine;
            ReportLabel.Text += "- " + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 21.12.2010" + Environment.NewLine;
            ReportLabel.Text += "- Написал прогрессБар для выбора и генерации отчетов. Пока только не прикрутил, можно посмотреть в закладке помошь(кнопка пуск-синхронно, кнопка без текста - асинхронно). Будет выбран самый большой файл на 1 600 000 записей. Пока тест, есть ошибки." + Environment.NewLine;
            ReportLabel.Text += "- Переписал функции соответственно, для того, чтобы показывать процесс готовности." + Environment.NewLine;
            ReportLabel.Text += "- Выборки пустил в нескольких потоках, может быть будет быстрее." + Environment.NewLine;
            ReportLabel.Text += "- Разбираюсь с запуском Anychart." + Environment.NewLine;
            ReportLabel.Text += "- " + Environment.NewLine;

            ReportLabel.Text += Environment.NewLine + "- 17.12.2010" + Environment.NewLine;
            ReportLabel.Text += "- решена проблема с таймаутами(сайта, команд и подключения к БД), сча должно работать стабильнее. Не засыпать долгом исполнении команд и не выдавать ошибок при удалении большого кол-ва записей(ПОСЛЕ ТЕСТА ВСЕ РАВНО ИНОГДА ЗАСЫПАЕТ)" + Environment.NewLine;
            ReportLabel.Text += "- Ускорил/оптимизировал работу, теперь моментально переключаются ТС в закладке PLF, и немного быстрее генерируется отчет." + Environment.NewLine;
            ReportLabel.Text += "- Теперь если поставить галочку Запомнить меня на странице ввода пароля, то не нужно будет вводить пароль 24часа, если не ставить, то через два часа бездействия опять нужно будет вводить пароль(это на случай, если забыли нажать выход)." + Environment.NewLine;
        }
    }
}
