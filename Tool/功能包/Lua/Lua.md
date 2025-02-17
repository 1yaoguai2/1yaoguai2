### 学习Lua编程语言

1. 网址

   [Lua 数据类型 | 菜鸟教程](https://www.runoob.com/lua/lua-data-types.html)

   [菜鸟教程在线编辑器](https://www.runoob.com/try/runcode.php?filename=HelloWorld&type=lua)

2. 代码

   ```
   print("Hello World1!")
   print(b);
   b = 10;
   print(b);
   --删除b变量
   b = nil;
   print("---------数据类型----------")
   print(type("Hello world"))      --> string
   print(type(10.4*3))             --> number
   print(type(print))              --> function
   print(type(type))               --> function
   print(type(true))               --> boolean
   print(type(nil))                --> nil
   print(type(type(X)))            --> string
   
   print("---------数据变量示例-------")
   html = [[
   <html>
   <head></head>
   <body>
       <a href="http://www.runoob.com/">菜鸟教程</a>
   </body>
   </html>
   ]]
   print(html)
   s = '字符串'
   s1 = "字符串1"
   print(s)
   print(#s)
   print(s1)
   print(#s1)
   
   print("--------数据拼接--------")
   print("1" + "2")
   print("20" + '33')
   print("ni" .. "33")   --正确拼接
   
   
   
   ```

   ```
   Hello World1!
   nil
   10
   ---------数据类型----------
   string
   number
   function
   function
   boolean
   nil
   string
   ---------数据变量示例-------
   
   
   
       菜鸟教程
   
   
   
   字符串
   9
   字符串1
   10
   --------数据拼接--------
   3.0
   53.0
   ni33
   
   
   ```

   