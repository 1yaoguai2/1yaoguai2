--初始化git配置
git init  
--添加所有文件到git
git add .
--提交文件到git
git commit -m "本次提交信息描述"
--查看提交记录
git log
--恢复所有文件到上一次提交
git restore .
--查看文件状态
git status
--恢复文件到commit_id这次的提交状态
git revert <commit_id>
:QW Enter

--推送到远程仓库
git push origin master