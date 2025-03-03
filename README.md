# 關卡制戰鬥遊戲
## 主旨
主要由四種主要功能的實作，包含角色建構及互動、關卡、簡易背包及消費系統、系統設置變更等等。
## 角色建構及互動
### 角色建立
以工廠模式建立及管理角色，以下為流程：

1.取得資源：透過```Addressable```插件，取得角色的所有資源。   
2.建立角色物件：取得角色的人物資源並建置於場景中。   
3.建立角色屬性：建立角色所需的參數，可提供模組或資源（腳本）使用。   
4.建立角色模組：建立管理及操作腳色的模組。  
5.建立角色資源：取得角色所需的資源，包含 Prefab, ScriptableObject, Object 等，將其移入或複製進角色物件。  
6.建立狀態機：從角色資源中取得[狀態機資源](https://github.com/zylin1998/StateMachine)，並取得狀態機並設定置角色中。

### 角色管理

1.取得角色：從物件池中取得角色，若當前角色池中未包含角色則創建角色。  
2.重製屬性：將角色裡的屬性重製供角色重覆使用。

### 互動

以觀察者模式為基礎，在用於互動的物件（攻擊物件）上觀察互動的情形並觸發事件（受傷事件），透過代理人（Presenter）發送事件，根據事件這側的對象處理事件。

1.觸發事件：若互動物件達到觸發標準，則觸發相對應得事件。  
2.發送事件：若代理人（Presenter）收到觸發的事件，要求模組處理事件或和其他代理人交流事件。  
3.處理事件：收到事件的代理人會像模組提供參數進行邏輯處理，並根據結果確認是否回復事件。  

## 關卡

1. 取得資訊：根據選擇的關卡從資源中取得資訊及資源（地圖資源、敵人資訊、遊戲時間）。
2. 建立場景：將地圖資源注入到場景中並開始遊戲。
3. 遊戲循環：透過狀態機建立遊戲循環的順序，並以 fixedUpdate 的方式檢查遊戲進程。
