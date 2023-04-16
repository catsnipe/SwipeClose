# SwipeClose
[Unity] Screen transition by swiping - スワイプ操作で画面遷移

https://user-images.githubusercontent.com/85425896/148633069-9ac452ca-7c13-41bb-81bf-fa72576cfc60.mp4

* Samples include Unity InputSystem, Padd, SimpleUIEase.  
https://github.com/catsnipe/Padd  
https://github.com/catsnipe/SimpleUIEase  
  
* 日本語の説明は Blog を参照してください  
https://www.create-forever.games/swipe-window-change/  
  
## requirement
unity2019 or later  
Input System(package)  
  
## usage  
1. Attach '**SwipeClose**' to any UI.  
2. Set member '**CloseVector**'.  
3. '**Scenes/SampleScene.unity**' provides the same scene as the video.  
4. OnClosing, OnClosed, SwipeChanged are events to cooperate with other UI when swiping.
