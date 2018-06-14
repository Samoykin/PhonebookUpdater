# PhonebookUpdater
Программа автоматического обновления информационного справочника Cindy.

Версии и пути расположения новых версий Cindy и PhonebookUpdater вносятся в файл RemoteProp.xml.

Наличие новой версии роверяется при запуске Cindy.

Внимание!
PhonebookUpdater запускается из программы Cindy.
Поместить PhonebookUpdater в папку Cindy\Updater\

Формат RemoteProp.xml

```
<?xml version="1.0" encoding="utf-8"?>
<Settings xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <Phonebook Version="1.9.0.0" Path="d:\Cindy" />
  <PhonebookUpd Version="3.2.0.0" Path="d:\PhonebookUpdater" />
</Settings>
```
