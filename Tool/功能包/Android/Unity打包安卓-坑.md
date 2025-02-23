# unity 打包安卓遇到的坑
## Unity未安装安卓模块
1.最好在游戏内部点击切换平台到安卓，去下载
2.通过GitHub版本管理添加模块
3.卸载unity版本，重新安装，安装方式选用UnityHub安装，安装时勾选安卓模块

## Unity打包安卓时的设置
1.必须设置包的信息，player -> otherSetting -> Identification -> Package Name，com.[公司].[项目]
2.遇到CMake 3.22不存在，根据提示管理员启动终端并转到指定地址，执行
```
 ./sdkmanager.bat --install "cmake;3.22.1" //版本根据提示

 