### Добавление новой фигурки в игру
1. Добавить название фигурки в [EntityType](https://github.com/Lopol2010/2d-project-sprites-grid/blob/master/Assets/Scripts/Entity/EntityType.cs)
2. Добавить правила столкновения с другими фигурками внутри метода [CollisionResolver.Init](https://github.com/Lopol2010/2d-project-sprites-grid/blob/045e9f154408e51c1878f3f584b98193c79422df/Assets/Scripts/CollisionResolver.cs#L99)
3. Создать компонент фигурки наследующий [Entity](https://github.com/Lopol2010/2d-project-sprites-grid/blob/master/Assets/Scripts/Entity/Entity.cs)
   1. В методе Awake или Start заполнить спискок preyList - список тех за кем гоняется фигурка
   2. Так же заполнить predatorList - список тех от кого убегает
   3. Так же установить поле type на ранее созданный EntityType
4. Создать префаб фигурки, например на основе EntityPrefab
   1. Добавить ранее созданый компонент
   2. Установить нужный спрайт
5. Добавить ссылку на префаб в массив Entity Prefabs в игровом объекте Game
6. Удалить префаб фигурки со сцены
