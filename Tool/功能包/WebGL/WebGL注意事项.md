1.两个Dll是web下解析json的包，放到unityAssets下的Plugins下，原因是Newtonsoft.Json无法在Web端使用。
2.禁用压缩格式：player->publishing settings -> Compression Format -> Disabled 。
3.IIS发布界面，MIME类型缺失，添加文件类型，.data+application/octet-stream或者.data+ application/data,和.wasm+ application/wasm

4.界面设置

4.1打包后的html文件，把div里面的style属性的width和height改为100%

4.2找到canvasScaler修改UIScaleMode为Scale With Screen Size，分辨率为开发的分辨率，Match修改为0.5