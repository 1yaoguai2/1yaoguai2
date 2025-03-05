# Git操作
1. 创建项目，自带主分支
```
git clone
git pull
```


2. 创建开发分支

```
git branch develop
```

3. 切换到开发分支

```
git checkout develop
```

4. 同步主分支main到开发分支

```
git merge main   //必须在开发分支上才能执行
```

5. 在开发分支下，进行开发, 上传开发的内容到develop分支

```
git push 
```

6. 通过网页创建合并请求，将开发分支合并到主分支

7. 拉取最新合并后的主分支

```
git pull 
```

8. 切换到开发分支后，同步主分支到开发分支

```
git checkout develop
git merge main
```

9. 继续开发，并循环以上步骤





# Git项目文件下，部分文件传递到新分支（主要应用于Unity的插件开发）

1. 到目标文件夹

```
cd [PackageName]
```

2. 创建仓库 ，查看文件,  查看默认分支master

```
git init
git ls-files 
git branch
```

3.上传到远端，（指定分支名称）

```
git push -f --set-upstream https://github.com/1yaoguai2/XTools.git -当前分支名称

git push -f https://github.com/1yaoguai2/XTools.git upm(当前分支)
```



# Git快捷键

1. 退出日志

   Q
