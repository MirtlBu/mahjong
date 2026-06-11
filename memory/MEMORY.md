# Mahjong Project Memory

## Project: Unity URP Mahjong Solitaire
Path: /Users/bubuzoglo/Work/output/games/Mahjong

## Tile Setup (рабочая конфигурация)

### FBX модель
- Файл: Assets/3d/tiles/game_tile.fbx (рабочий оригинал в git, не трогать)
- Два материала: slot 0 = тело/бока (body), slot 1 = лицо (face/front, видна камере)
- Размеры в local space: x=2 (ширина), y=1 (толщина), z=3 (высота)

### Prefab
- Компоненты: TileView (MonoBehaviour), BoxCollider
- Назначить в BoardManager → поле Tile Prefab

### Размеры сетки (рабочие значения в сцене)
- tileWidth  = 2.0  (шаг по X)
- tileHeight = 3.0  (шаг по Y)
- tileDepth  = 1.05 (смещение слоёв по Z)

## Face материал — Shader Graph

- Surface Type: Transparent, Blending Mode: Alpha
- Текстурный property: Reference = `_BaseMap` (обязательно)
- `SetTextureOffset("_BaseMap", ...)` и `SetTextureScale("_BaseMap", ...)` работают корректно

## Архитектура материалов

### TileVisualSettings (ScriptableObject)
- bodyMaterial  → слот 0 (тело, всегда одинаковый)
- faceMaterial  → слот 1 (лицо, атлас 6×6)
- useAtlas = true → BuildAtlasMaterials() создаёт 36 инстансов с UV-офсетами

## Архитектура уровней (level = раскладка)

### LayoutSO (abstract ScriptableObject) — Assets/Scripts/Layouts/
- Содержит и логику раскладки, и метаданные уровня
- Поля: levelName, previewSprite, mapPosition
- Метод: GetPositions() → List<Vector3Int>
- Наследники: TurtleLayoutSO, CrossLayoutSO, PyramidLayoutSO
- Создаются через Create → Mahjong → Layouts → ...

### GameManager.levels = LayoutSO[]
- Каждый элемент массива = один уровень на карте
- CurrentLevel → LayoutSO для текущего уровня

### Прогресс уровней — LevelProgress (static, PlayerPrefs)
- IsUnlocked(i): index==0 всегда открыт; иначе требует IsCompleted(i-1)
- SetCompleted(i): вызывается из GameManager.OnLevelComplete
- Уровни 1, 2, ... разблокируются последовательно после победы

### Roadmap (MapScene)
- Ноды в ряд/цепочку, dashed lines между ними
- Label ноды = layoutSO.levelName (название раскладки: Turtle, Cross, Pyramid...)
- mapPosition задаётся в каждом LayoutSO asset
- Порядок уровней: порядок в массиве GameManager.levels

## Ключевые файлы
- Assets/Scripts/BoardManager.cs
- Assets/Scripts/TileView.cs
- Assets/Scripts/TileData.cs
- Assets/Scripts/Layouts/LayoutSO.cs
- Assets/Scripts/Layouts/TurtleLayoutSO.cs
- Assets/Scripts/Layouts/CrossLayoutSO.cs
- Assets/Scripts/Layouts/PyramidLayoutSO.cs
- Assets/Scripts/TileVisualSettings.cs
- Assets/Scripts/GameManager.cs
- Assets/Scripts/Levels/LevelProgress.cs
- Assets/Scripts/UI/GameHUD.cs
- Assets/Scripts/UI/LevelMapUI.cs
- Assets/Scripts/UI/LevelNodeUI.cs
- Assets/Scripts/UI/MapSceneUI.cs
- Assets/Scripts/GameStrings.cs
- Assets/TileVisualSettings.asset
- Assets/Scenes/GameScene.unity
- Assets/Scenes/MapScene.unity
