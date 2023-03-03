# AutoClickerLibrary
Windows用のマウスとキーボードの操作用ライブラリです。
簡単なスクリプト実行用エンジンも入っています。
デュアルモニタでの動作は保証しません。
このソフトウェアは、 Apache 2.0ライセンスで配布されている製作物が含まれています。

# 座標
プライマリモニタの左上を(x,y)=(0,0)として扱います。
セカンダリモニタがプライマリモニタより左に存在する場合はx座標はマイナスになります。

# 例
## マウスの座標取得
```
System.Drawing.Point position = AutoClicker.Mouse.GetPos();
```
## マウスの座標設定
```
#座標を指定して移動
int x = 100;
int y = 200;
await AutoClicker.Mouse.SetPosAsync(x,y);

#画像を指定して移動
#offsetにnullを指定すると画像の中心へ移動
int xOffset = 100;
int yOffset = 100;
string imagePath = "template.png";
await AutoClicker.Mouse.MoveToImage(imagePath, xOffset, yOffset);
```
## クリック
```
#クリック
await AutoClicker.Mouse.Click();

#ダブルクリック
await AutoClicker.Mouse.DoubleClick();

#右クリック
await AutoClicker.Mouse.RightClick();

#画像を指定してクリック
#クリックの種別はClickTypesで指定
#offsetにnullを指定すると画像の中心をクリック
int xOffset = 100;
int yOffset = 100;
string imagePath = "template.png";
AutoClicker.Mouse.ClickTypes clickType = AutoClicker.Mouse.ClickTypes.LEFT;
await AutoClicker.Mouse.ClickImage(imagePath, clickType, xOffset, yOffset);
```

## キー送信
中括弧（{}）は送信できません。
```
#文字を送信
await AutoClicker.Keyboard.SendAsync("some keys send.");
#エンターキーを送信
await AutoClicker.Keyboard.SendAsync(AutoClicker.Keyboard.Codes.Enter);
```

# スクリプト
## エンジン
```
var script = "SetMousePos 100 100";
var engine = new AutoClicker.Engine.BaseEngine();
await engine.ExecuteScriptAsync(script)
```

## コメント
\# で始まる行はコメントとして扱います
```
# #から始まる行はコメント
```

## 指定した座標へマウスを移動
```
# (x,y) = (100, 200) へマウスを移動
SetMousePos 100 200
```

## 指定した画像が表示されている場所へマウスを移動
```
# template.pngが表示されている左上から(x,y)=(10,20)移動した場所へマウスを移動
MoveToImage template.png (10,20)

# template.pngが表示されている場所の画像の中心へマウスを移動
MoveToImage template.png Center
```

## クリック
```
# クリック
Click
# ダブルクリック
DoubleClick
# 右クリック
RightClick
```

## 待機
```
# 100msec待機
Wait 100
```

## 画像があるディレクトリを指定
```
SetImageDir image
# image/template.pngが表示されている場所の画像の中心へマウスを移動
MoveToImage template.png Center
```
